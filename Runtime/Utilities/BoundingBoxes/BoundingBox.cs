using System;
using System.Collections.Generic;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.BoundingBoxes.Experimental;
using droid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

#if UNITY_EDITOR

#endif

namespace droid.Runtime.Utilities.BoundingBoxes {
  public enum BasedOn {
    /// <summary>
    /// Base the bounding box on geometries
    /// </summary>
    Geometry_,

    /// <summary>
    /// Base the bounding box on colliders
    /// </summary>
    Collider_
  }

  /// <summary>
  ///
  /// </summary>
  public enum BoundingBoxOrientation {
    /// <summary>
    ///
    /// </summary>
    Axis_aligned_,

    /// <summary>
    ///
    /// </summary>
    Object_oriented_,
    /// <summary>
    ///
    /// </summary>
    Camera_oriented_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class BoundingBox : MonoBehaviour {
    /// <summary>
    /// </summary>
    protected Bounds _Bounds = new Bounds();

    /// <summary>
    /// </summary>
    protected Vector3 _Bounds_Offset;

    /// <summary>
    /// </summary>
    Collider[] _children_colliders = null;

    /// <summary>
    /// </summary>
    MeshFilter[] _children_meshes = null;

    /// <summary>
    /// </summary>
    [SearchableEnum]
    public BasedOn basedOn = BasedOn.Geometry_;

    /// <summary>
    /// </summary>
    Vector3[] _points = null;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool freezeAfterFirstCalculation = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool includeChildren = false;

    [SerializeField] bool _only_active_children = true;

    Vector3 _last_position;
    Quaternion _last_rotation;

    /// <summary>
    /// </summary>
    Vector3 _last_scale;

    /// <summary>
    /// </summary>
    public Color editorPreviewLineColor = new Color(1f, 0.36f, 0.38f, 0.74f);

    /// <summary>
    /// </summary>
    Vector3[,] _lines = null;

    List<Vector3[]> _lines_list = new List<Vector3[]>();

    /// <summary>
    /// </summary>
    Quaternion _rotation;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool OnAwakeSetup = false;

    [SerializeField] IPrototypingEnvironment environment = null;

    Vector3 _bbl_extend;
    Vector3 _bbr_extend;
    Vector3 _bfl_extend;
    Vector3 _bfr_extend;
    Vector3 _tbl_extend;
    Vector3 _tbr_extend;
    Vector3 _tfl_extend;
    Vector3 _tfr_extend;

    [SerializeField] bool cacheChildren = true;
    [SerializeField] bool RunInEditModeSetup = false;
    [SerializeField] float margin = 0;

    BoxCollider _local_collider;

    MeshFilter _local_mesh;
    [SerializeField] bool includeSelf = true;

    public Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
                         this._tfl_extend,
                         this._tfr_extend,
                         this._tbl_extend,
                         this._tbr_extend,
                         this._bfl_extend,
                         this._bfr_extend,
                         this._bbl_extend,
                         this._bbr_extend
                     };
      }
    }

    public Bounds Bounds { get { return this._Bounds; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a_camera"></param>
    /// <returns></returns>
    public Rect ScreenSpaceBoundingRect(Camera a_camera) {
      return this.GetBoundingBoxScreenRect(a_camera, this.margin);
    }

    public Vector3 Max { get { return this._Bounds.max; } }

    public Vector3 Min { get { return this._Bounds.min; } }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += $"\"_top_front_left\":{this.BoundingBoxCoordinates[0]}, ";
        str_rep += $"\"_top_front_right\":{this.BoundingBoxCoordinates[1]}, ";
        str_rep += $"\"_top_back_left\":{this.BoundingBoxCoordinates[2]}, ";
        str_rep += $"\"_top_back_right\":{this.BoundingBoxCoordinates[3]}, ";
        str_rep += $"\"_bottom_front_left\":{this.BoundingBoxCoordinates[4]}, ";
        str_rep += $"\"_bottom_front_right\":{this.BoundingBoxCoordinates[5]}, ";
        str_rep += $"\"_bottom_back_left\":{this.BoundingBoxCoordinates[6]}, ";
        str_rep += $"\"_bottom_back_right\":{this.BoundingBoxCoordinates[7]}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesWorldSpaceAsJson {
      get {
        var str_rep = "{";
        var transform1 = this.transform;
        if (this._use_bb_transform) {
          transform1 = this._bb_transform;
        }

        var rotation = transform1.rotation;
        var position = transform1.position;
        if (this.environment != null) {
          str_rep +=
              $"\"top_front_left\":{this.JsonifyVec3(this.environment.TransformPoint(rotation * this._tfl_extend + position))}, ";
          str_rep +=
              $"\"bottom_back_right\":{this.JsonifyVec3(this.environment.TransformPoint(rotation * this._bbr_extend + position))}";
        }

        str_rep += "}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public Vector3[,] Lines { get { return this._lines; } set { this._lines = value; } }

    /// <summary>
    /// </summary>
    public Vector3[] Points { get { return this._points; } set { this._points = value; } }

    /// <summary>
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    string JsonifyVec3(Vector3 vec) { return $"[{vec.x},{vec.y},{vec.z}]"; }

    /// <summary>
    /// </summary>
    void Reset() {
      this.Awake();
      this.Start();
    }

    /// <summary>
    /// </summary>
    void Start() {
      if (!this.OnAwakeSetup) {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Awake() {
      if (this.environment == null) {
        this.environment = FindObjectOfType<PrototypingEnvironment>();
      }

      if (!this._camera) {
        this._camera = FindObjectOfType<Camera>();
      }

      if (!this._bb_transform) {
        this._empty_go = new GameObject {hideFlags = HideFlags.HideAndDontSave};
        this._bb_transform = this._empty_go.transform;
      }

      if (this.OnAwakeSetup) {
        this.Setup();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (!this.RunInEditModeSetup && !Application.isPlaying) {
        return;
      }

      if (!this._bb_transform) {
        this._empty_go = new GameObject {hideFlags = HideFlags.HideAndDontSave};
        this._bb_transform = this._empty_go.transform;
      }

      if (this.includeSelf) {
        this._local_collider = this.GetComponent<BoxCollider>();
        this._local_mesh = this.GetComponent<MeshFilter>();
      }

      var transform1 = this.transform;
      this._last_position = transform1.position;
      this._last_rotation = transform1.rotation;
      this._last_scale = transform1.localScale;

      if (this.includeChildren) {
        this._children_meshes = this.GetComponentsInChildren<MeshFilter>();
        this._children_colliders = this.GetComponentsInChildren<Collider>();
      }

      this.CalculateBoundingBox();
      this.RecalculatePoints();
      this.RecalculateLines();
    }

    /// <summary>
    /// </summary>
    void LateUpdate() {
      if (this.freezeAfterFirstCalculation) {
        return;
      }

      if (this.includeChildren && !this.cacheChildren) {
        if (this._children_meshes != this.GetComponentsInChildren<MeshFilter>()) {
          this.Reset();
        }

        if (this._children_colliders != this.GetComponentsInChildren<Collider>()) {
          this.Reset();
        }
      } else {
        this.CalculateBoundingBox();
        this.RecalculatePoints();
        this.RecalculateLines();

        if (this.transform.localScale != this._last_scale) {
          this.ScaleBounds();
          this.RecalculatePoints();
        }

        if (this.transform.position != this._last_position
            || this.transform.rotation != this._last_rotation
            || this.transform.localScale != this._last_scale) {
          this.RecalculateLines();
          var transform1 = this.transform;
          this._last_rotation = transform1.rotation;
          this._last_position = transform1.position;
          this._last_scale = transform1.localScale;
        }
      }
    }

    /// <summary>
    /// </summary>
    public void ScaleBounds() {
      var local_scale = this.transform.localScale;
      /*if (this._use_bb_transform) {
        local_scale = this._bb_transform.localScale;
      }*/

      this._Bounds.size = new Vector3(this._Bounds.size.x * local_scale.x / this._last_scale.x,
                                      this._Bounds.size.y * local_scale.y / this._last_scale.y,
                                      this._Bounds.size.z * local_scale.z / this._last_scale.z);

      this._Bounds_Offset = new Vector3(this._Bounds_Offset.x * local_scale.x / this._last_scale.x,
                                        this._Bounds_Offset.y * local_scale.y / this._last_scale.y,
                                        this._Bounds_Offset.z * local_scale.z / this._last_scale.z);
      //this._Bounds.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
      //this._Bounds.center = transform.TransformPoint(startingBoundCenterLocal);
    }

    /// <summary>
    /// </summary>
    void FitCollidersAabb() {
      this._bb_transform.rotation = this.transform.rotation;
      this._bb_transform.position = this.transform.position;

      var bounds = new Bounds(this._bb_transform.position, Vector3.zero);

      if (this.includeSelf && this._local_collider) {
        if (false) {
          var center = this._local_collider.transform.TransformPoint(this._local_collider.bounds.center);
          var size = this._local_collider.transform.TransformVector(this._local_collider.bounds.size);
          var b = new Bounds(center, size);
          bounds = b;
          var min = this._local_collider.transform.TransformPoint(this._local_collider.bounds.min);
          var max = this._local_collider.transform.TransformPoint(this._local_collider.bounds.max);
          bounds.Encapsulate(min);
          bounds.Encapsulate(max);
        } else {
          this._bb_transform.position = this._local_collider.bounds.center;
          bounds = this._local_collider.bounds;
        }
      }

      if (this.includeChildren && this._children_colliders != null) {
        foreach (var a_collider in this._children_colliders) {
          if (a_collider && a_collider != this._local_collider) {
            if (this._only_active_children) {
              if (a_collider.gameObject.activeInHierarchy
                  && a_collider.gameObject.activeSelf
                  && a_collider.enabled) {
                if (false) {
                  if (bounds.size == Vector3.zero) {
                    this._bb_transform.rotation = a_collider.transform.rotation;
                    this._bb_transform.position = a_collider.transform.position;
                    var center = a_collider.transform.TransformPoint(a_collider.bounds.center);
                    var size = a_collider.transform.TransformVector(a_collider.bounds.size);
                    var b = new Bounds(center, size);
                    bounds = b;
                  }

                  var min = a_collider.transform.TransformPoint(a_collider.bounds.min);
                  var max = a_collider.transform.TransformPoint(a_collider.bounds.max);
                  bounds.Encapsulate(min);
                  bounds.Encapsulate(max);
                } else {
                  if (bounds.size == Vector3.zero) {
                    this._bb_transform.rotation = a_collider.transform.rotation;
                    this._bb_transform.position = a_collider.bounds.center;
                    bounds = a_collider.bounds;
                  } else {
                    bounds.Encapsulate(a_collider.bounds);
                  }
                }
              }
            } else {
              if (bounds.size == Vector3.zero) {
                this._bb_transform.rotation = a_collider.transform.rotation;
                this._bb_transform.position = a_collider.bounds.center;
                bounds = a_collider.bounds;
              } else {
                bounds.Encapsulate(a_collider.bounds);
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - this._bb_transform.position;
    }

    /// <summary>
    /// </summary>
    void FitRenderersAabb() {
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var bounds = new Bounds(position, Vector3.zero);

      if (this.includeSelf && this._local_mesh) {
        var a_mesh = this._local_mesh.sharedMesh;
        var vc = a_mesh.vertexCount;
        for (var i = 0; i < vc; i++) {
          bounds.Encapsulate(this._local_mesh.transform.TransformPoint(a_mesh.vertices[i]));
        }
      }

      if (this.includeChildren && this._children_meshes != null) {
        foreach (var t in this._children_meshes) {
          if (t) {
            if (this._only_active_children) {
              if (t.gameObject.activeInHierarchy && t.gameObject.activeSelf) {
                if (bounds.size == Vector3.zero) {
                  var transform2 = t.transform;
                  position = transform2.position;
                  this._bb_transform.position = position;
                  this._bb_transform.rotation = transform2.rotation;
                  bounds = new Bounds(position, Vector3.zero);
                }

                var a_mesh = t.sharedMesh;
                if (a_mesh) {
                  var vc = a_mesh.vertexCount;
                  for (var j = 0; j < vc; j++) {
                    bounds.Encapsulate(t.transform.TransformPoint(a_mesh.vertices[j]));
                  }
                }
              }
            } else {
              if (bounds.size == Vector3.zero) {
                bounds = new Bounds(t.transform.position, Vector3.zero);
              }

              var a_mesh = t.sharedMesh;
              if (a_mesh) {
                var vc = a_mesh.vertexCount;
                for (var j = 0; j < vc; j++) {
                  bounds.Encapsulate(t.transform.TransformPoint(a_mesh.vertices[j]));
                }
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - position;
    }

    /// <summary>
    /// </summary>
    void CalculateBoundingBox() {
      if (!this.RunInEditModeSetup && !Application.isPlaying || this._bb_transform == null) {
        return;
      }

      if (this.basedOn == BasedOn.Collider_) {
        switch (this.use_unity_aabb) {
          case BoundingBoxOrientation.Axis_aligned_:
            this.FitCollidersAabb();
            break;
          case BoundingBoxOrientation.Object_oriented_:
            this.FitCollidersObb();
            break;
          case BoundingBoxOrientation.Camera_oriented_:
            this._points = BoundingBoxUtilities.GetMinMaxPointsCollider(null,this._camera);
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      } else {
        switch (this.use_unity_aabb) {
          case BoundingBoxOrientation.Axis_aligned_:
            this.FitRenderersAabb();
            break;
          case BoundingBoxOrientation.Object_oriented_:
            this.FitRenderersObb();
            break;
          case BoundingBoxOrientation.Camera_oriented_:
            this._points = BoundingBoxUtilities.GetMinMaxPointsMesh(null,this._camera);
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }
    }

    void FitRenderersObb() {
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var min = Vector3.zero;
      var max = min;

      if (this.includeSelf && this._local_mesh) {
        var a_mesh = this._local_mesh.sharedMesh;
        var vc = a_mesh.vertexCount;
        for (var i = 0; i < vc; i++) {
          max = min = this._camera.WorldToViewportPoint(a_mesh.vertices[i]);
        }
      }

      if (this.includeChildren && this._children_meshes != null) {
        foreach (var t in this._children_meshes) {
          if (t) {
            if (this._only_active_children) {
              if (t.gameObject.activeInHierarchy && t.gameObject.activeSelf) {
                if (min == Vector3.zero) {
                  var transform2 = t.transform;
                  position = transform2.position;
                  this._bb_transform.position = position;
                  this._bb_transform.rotation = transform2.rotation;
                  min = max = this._camera.WorldToViewportPoint(position);
                }

                var a_mesh = t.sharedMesh;
                if (a_mesh) {
                  var vc = a_mesh.vertexCount;
                  for (var j = 0; j < vc; j++) {
                    min = max = this._camera.WorldToViewportPoint(a_mesh.vertices[j]);
                  }
                }
              }
            } else {
              if (min == Vector3.zero) {
                min = max = this._camera.WorldToViewportPoint(t.transform.position);
              }

              var a_mesh = t.sharedMesh;
              if (a_mesh) {
                var vc = a_mesh.vertexCount;
                for (var j = 0; j < vc; j++) {
                  min = max = this._camera.WorldToViewportPoint(a_mesh.vertices[j]);
                }
              }
            }
          }
        }
      }

      this._Bounds_Offset = this._Bounds.center - this._camera.WorldToViewportPoint(position);
    }

    void FitCollidersObb() { throw new NotImplementedException(); }

    static Vector3 _top_front_right = new Vector3(1, 1, 1);
    static Vector3 _top_front_left = new Vector3(-1, 1, 1);
    static Vector3 _bottom_back_right = new Vector3(1, -1, -1);
    static Vector3 _bottom_back_left = new Vector3(-1, -1, -1);
    static Vector3 _bottom_front_left = new Vector3(-1, -1, 1);
    static Vector3 _bottom_front_right = new Vector3(1, -1, 1);
    static Vector3 _top_back_right = new Vector3(1, 1, -1);
    static Vector3 _top_back_left = new Vector3(-1, 1, -1);
    [SerializeField] public Transform _bb_transform = null;
    [SerializeField] BoundingBoxOrientation use_unity_aabb = BoundingBoxOrientation.Axis_aligned_;
    [SerializeField] Camera _camera = null;
    [SerializeField] public bool _use_bb_transform = false;
    GameObject _empty_go = null;

    /// <summary>
    /// </summary>
    void RecalculatePoints() {
      this._tfr_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _top_front_right);
      this._tfl_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _top_front_left);
      this._tbl_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _top_back_left);
      this._tbr_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _top_back_right);
      this._bfr_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _bottom_front_right);
      this._bfl_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _bottom_front_left);
      this._bbl_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _bottom_back_left);
      this._bbr_extend = this._Bounds_Offset + Vector3.Scale(this._Bounds.extents, _bottom_back_right);

      this.Points = new[] {
                              this._tfr_extend,
                              this._tfl_extend,
                              this._tbl_extend,
                              this._tbr_extend,
                              this._bfr_extend,
                              this._bfl_extend,
                              this._bbl_extend,
                              this._bbr_extend
                          };
    }

    /// <summary>
    /// </summary>
    void RecalculateLines() {
      var transform1 = this.transform;
      if (this._bb_transform) {
        transform1 = this._bb_transform;
      }

      var rot = Quaternion.identity;
      var pos = Vector3.zero;
      if (this._use_bb_transform) {
        rot = transform1.rotation;
      }

      pos = transform1.position;

      this._lines_list.Clear();
      const int num_points_per_line = 2;
      const int num_lines = 12;

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._lines_list.Add(line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._lines_list.Add(line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._lines_list.Add(line);
      }

      this.Lines = new Vector3[num_lines, num_points_per_line];
      for (var j = 0; j < num_lines; j++) {
        this.Lines[j, 0] = this._lines_list[j][0];
        this.Lines[j, 1] = this._lines_list[j][1];
      }
    }

    /// <summary>
    /// </summary>
    void OnMouseDown() {
      //if (_permanent)
      //  return;
      //this.enabled = !this.enabled;
    }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() {
      if (EditorApplication.isPlaying) {
        return;
      }

      this.CalculateBoundingBox();
      this.RecalculatePoints();
      this.RecalculateLines();
    }

    /// <summary>
    /// </summary>
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this.enabled) {
          Gizmos.color = this.editorPreviewLineColor;
          if (this.Lines != null) {
            for (var i = 0; i < this.Lines.GetLength(0); i++) {
              Gizmos.DrawLine(this.Lines[i, 0], this.Lines[i, 1]);
            }
          }

          if (this._bb_transform) {
            Handles.Label(this._bb_transform.position, this._bb_transform.gameObject.name);
          } else {
            Handles.Label(this.transform.position, this.gameObject.name);
          }
        }
      }
    }
    #endif
  }
}
