using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(Camera))]
  [ExecuteInEditMode]
  public class UberCamera : MonoBehaviour {
    #region fields

    [SerializeField] Shader copy_shader = null;
    [SerializeField] Material _copy_material = null;
    [SerializeField] Material _off_screen_mat = null;
    [SerializeField] Camera _camera = null;
    [SerializeField] Boolean _debugging = true;
    [SerializeField] GUISkin gui_style = null;

    CommandBuffer[] _copy_cbs = null;
    CommandBuffer _copy_fb_cb = null;
    CommandBuffer _copy_gb_cb = null;
    CommandBuffer _clear_gb_cb = null;
    CommandBuffer _copy_velocity_cb = null;
    RenderTexture[] _fb_rts = null;
    RenderTexture[] _gb_rts = null;
    Mesh _quad_mesh = null;

    RenderTargetIdentifier[] _m_rt_fb_ids = null;
    RenderTargetIdentifier[] _m_rt_gb_ids = null;
    int _tmp_texture_id = Shader.PropertyToID("_TmpFrameBuffer");
    static readonly int _clear_color = Shader.PropertyToID("_ClearColor");

    static readonly Tuple<Int32, Int32> _texture_wh = new Tuple<int, int>(256, 256);

    const int _preview_size = 100;
    const int _preview_margin = 20;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public Boolean Debugging { get { return this._debugging; } set { this._debugging = value; } }

    protected Tuple<int, int> GetCaptureResolutionFromCamera() {
      var w = this._camera.pixelWidth;
      var h = this._camera.pixelHeight;
      var aspect = (float)h / w;
      w = _texture_wh.Item1;
      h = (int)(w * aspect);
      return new Tuple<int, int>(w, h);
    }

    void Update() {
      if (_texture_wh.Item1 == Screen.width && _texture_wh.Item2 == Screen.height) {
        return;
      }

      var xw = _texture_wh.Item1;
      var yh = _texture_wh.Item2;

      var x = Screen.width / 2 - xw / 2;
      var y = Screen.height / 2 - yh / 2;

      this._camera.pixelRect = new Rect(x,
                                        y,
                                        xw,
                                        yh);
    }

    void Awake() {
      if (!this.gui_style) {
        this.gui_style = Resources.FindObjectsOfTypeAll<GUISkin>().First(a => a.name == "BoundingBox");
      }

      if (!this._copy_material) {
        this._copy_material = new Material(this.copy_shader);
      }

      if (!this._quad_mesh) {
        this._quad_mesh = CreateFullscreenQuad();
      }

      if (!this._camera) {
        this._camera = this.GetComponent<Camera>();
      }

      this.Dispose();

      if (this._fb_rts == null || this._fb_rts.Length != 2) {
        this._fb_rts = new RenderTexture[2];
        for (var i = 0; i < this._fb_rts.Length; ++i) {
          this._fb_rts[i] = new RenderTexture(_texture_wh.Item1,
                                              _texture_wh.Item2,
                                              0,
                                              RenderTextureFormat.ARGBHalf) {
                                                                                filterMode = FilterMode.Point,
                                                                                name = $"rt_fb{i}"
                                                                            };
          this._fb_rts[i].Create();
        }
      }

      this._m_rt_gb_ids = new RenderTargetIdentifier[] {this._fb_rts[0], this._fb_rts[1]};

      if (this._gb_rts == null || this._gb_rts.Length != 8) {
        /*
 half4 albedo        : SV_Target0;
  half4 occlusion     : SV_Target1;
  half4 specular      : SV_Target2;
  half4 smoothness    : SV_Target3;
  half4 normal        : SV_Target4;
  half4 emission      : SV_Target5;
  half4 depth         : SV_Target6;
 */
        var names = new[] {
                              "albedo",
                              "occlusion",
                              "specular",
                              "smoothness",
                              "normal",
                              "emission",
                              "depth",
                              "velocity"
                          };
        this._gb_rts = new RenderTexture[8];
        for (var i = 0; i < this._gb_rts.Length; ++i) {
          this._gb_rts[i] = new RenderTexture(_texture_wh.Item1,
                                              _texture_wh.Item2,
                                              0,
                                              RenderTextureFormat.ARGBHalf) {
                                                                                filterMode = FilterMode.Point,
                                                                                name = $"{names[i]}"
                                                                            };
          this._gb_rts[i].Create();
        }
      }

      this._m_rt_fb_ids = new RenderTargetIdentifier[] {
                                                           this._gb_rts[0],
                                                           this._gb_rts[1],
                                                           this._gb_rts[2],
                                                           this._gb_rts[3],
                                                           this._gb_rts[4],
                                                           this._gb_rts[5],
                                                           this._gb_rts[6]
                                                       };

      this.Setup();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Mesh CreateFullscreenQuad() {
      var r = new Mesh {
                           vertices = new[] {
                                                new Vector3(1.0f, 1.0f, 0.0f),
                                                new Vector3(-1.0f, 1.0f, 0.0f),
                                                new Vector3(-1.0f, -1.0f, 0.0f),
                                                new Vector3(1.0f, -1.0f, 0.0f)
                                            },
                           triangles = new[] {
                                                 0,
                                                 1,
                                                 2,
                                                 2,
                                                 3,
                                                 0
                                             }
                       };
      r.UploadMeshData(true);
      return r;
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (this.copy_shader == null) {
        Debug.LogError("Copy shader is missing!");
        return;
      }

      if (this._off_screen_mat != null) {
        if (this._camera.targetTexture != null) {
          this._off_screen_mat.EnableKeyword("OFFSCREEN");
        } else {
          this._off_screen_mat.DisableKeyword("OFFSCREEN");
        }
      }

      this._copy_fb_cb = new CommandBuffer {name = "Copy FrameBuffer"};
      this._copy_fb_cb.GetTemporaryRT(this._tmp_texture_id,
                                      -1,
                                      -1,
                                      0,
                                      FilterMode.Point);
      this._copy_fb_cb.Blit(BuiltinRenderTextureType.CurrentActive, this._tmp_texture_id);
      this._copy_fb_cb.SetRenderTarget(this._m_rt_gb_ids, this._fb_rts[0]);
      this._copy_fb_cb.DrawMesh(this._quad_mesh,
                                Matrix4x4.identity,
                                this._copy_material,
                                0,
                                0);
      this._copy_fb_cb.ReleaseTemporaryRT(this._tmp_texture_id);
      this._camera.AddCommandBuffer(CameraEvent.AfterEverything, this._copy_fb_cb);

      this._clear_gb_cb = new CommandBuffer {
                                                name = "Cleanup GBuffer"
                                            }; // clear gbuffer (Unity doesn't clear emission buffer - it is not needed usually)
      if (this._camera.allowHDR) {
        this._clear_gb_cb.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
      } else {
        this._clear_gb_cb.SetRenderTarget(BuiltinRenderTextureType.GBuffer3);
      }

      this._clear_gb_cb.DrawMesh(this._quad_mesh,
                                 Matrix4x4.identity,
                                 this._copy_material,
                                 0,
                                 3);
      this._copy_material.SetColor(_clear_color, this._camera.backgroundColor);

      this._copy_gb_cb = new CommandBuffer {name = "Copy GBuffer"}; // copy gbuffer
      this._copy_gb_cb.SetRenderTarget(this._m_rt_fb_ids, this._gb_rts[0]);
      this._copy_gb_cb.DrawMesh(this._quad_mesh,
                                Matrix4x4.identity,
                                this._copy_material,
                                0,
                                2);
      this._camera.AddCommandBuffer(CameraEvent.BeforeGBuffer, this._clear_gb_cb);
      this._camera.AddCommandBuffer(CameraEvent.BeforeLighting, this._copy_gb_cb);

      this._copy_velocity_cb = new CommandBuffer {name = "Copy Velocity"};
      this._copy_velocity_cb.SetRenderTarget(this._gb_rts[7]);
      this._copy_velocity_cb.DrawMesh(this._quad_mesh,
                                      Matrix4x4.identity,
                                      this._copy_material,
                                      0,
                                      4);
      this._camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this._copy_velocity_cb);
      this._camera.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.MotionVectors;

      this._copy_cbs = new[] {
                                 this._copy_fb_cb,
                                 this._clear_gb_cb,
                                 this._copy_gb_cb,
                                 this._copy_velocity_cb
                             };
    }

    void OnGUI() {
      if (this._debugging) {
        var index = 0;

        if (this._gb_rts != null) {
          foreach (var pass in this._gb_rts) {
            var xi = (_preview_size + _preview_margin) * index++;
            var x = xi % (Screen.width - _preview_size);
            var y = (_preview_size + _preview_margin) * (xi / (Screen.width - _preview_size));
            var r = new Rect(_preview_margin + x,
                             _preview_margin + y,
                             _preview_size,
                             _preview_size);
            //this._asf?.Flip(pass._RenderTexture);

            GUI.DrawTexture(r, pass, ScaleMode.ScaleToFit);
            GUI.TextField(r, pass.name, this.gui_style.box);
          }
        }

        if (this._fb_rts != null) {
          foreach (var pass in this._fb_rts) {
            var xi = (_preview_size + _preview_margin) * index++;
            var x = xi % (Screen.width - _preview_size);
            var y = (_preview_size + _preview_margin) * (xi / (Screen.width - _preview_size));
            var r = new Rect(_preview_margin + x,
                             _preview_margin + y,
                             _preview_size,
                             _preview_size);
            //this._asf?.Flip(pass._RenderTexture);

            GUI.DrawTexture(r, pass, ScaleMode.ScaleToFit);
            GUI.TextField(r, pass.name, this.gui_style.box);
          }
        }
      }
    }

    void OnDestroy() {
      //this.Dispose();
    }

    void Dispose() {
      this._camera.RemoveAllCommandBuffers(); // cleanup capturing camera

      if (this._copy_fb_cb != null) {
        this._camera.RemoveCommandBuffer(CameraEvent.AfterEverything, this._copy_fb_cb);
        this._copy_fb_cb.Release();
        this._copy_fb_cb = null;
      }

      if (this._clear_gb_cb != null) {
        this._camera.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, this._clear_gb_cb);
        this._clear_gb_cb.Release();
        this._clear_gb_cb = null;
      }

      if (this._copy_gb_cb != null) {
        this._camera.RemoveCommandBuffer(CameraEvent.BeforeLighting, this._copy_gb_cb);
        this._copy_gb_cb.Release();
        this._copy_gb_cb = null;
      }

      if (this._copy_velocity_cb != null) {
        this._camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this._copy_velocity_cb);
        this._copy_velocity_cb.Release();
        this._copy_velocity_cb = null;
      }

      if (this._fb_rts != null) {
        foreach (var rt in this._fb_rts) {
          rt.Release();
        }

        this._fb_rts = null;
      }

      if (this._gb_rts != null) {
        foreach (var rt in this._gb_rts) {
          rt.Release();
        }

        this._gb_rts = null;
      }
    }
  }
}
