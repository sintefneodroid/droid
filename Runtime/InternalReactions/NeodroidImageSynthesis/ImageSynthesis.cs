using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


namespace NeodroidImageSynthesis {
	[RequireComponent (typeof(Camera))]
	public class ImageSynthesis : MonoBehaviour {

		// pass configuration
		CapturePass[] _capture_passes = {
				                                                        new CapturePass { _Name = "_img" },
				                                                        new CapturePass { _Name = "_id", _SupportsAntialiasing = false },
				                                                        new CapturePass { _Name = "_layer", _SupportsAntialiasing = false },
				                                                        new CapturePass { _Name = "_depth" },
				                                                        new CapturePass { _Name = "_normals" },
				                                                        new CapturePass { _Name = "_flow", _SupportsAntialiasing = false, _NeedsRescale = true } // (see issue with Motion Vectors in @KNOWN ISSUES)
		                                                        };

		struct CapturePass {
			// configuration
			public string _Name;
			public bool _SupportsAntialiasing;
			public bool _NeedsRescale;
			public CapturePass(string name) { this._Name = name; this._SupportsAntialiasing = true; this._NeedsRescale = false; this._Camera = null; }

			// impl
			public Camera _Camera;
		}

		public Shader segmentationShader;
		public Shader opticalFlowShader;

		public float opticalFlowSensitivity = 1.0f;

		// cached materials
		Material _optical_flow_material;

		void Start()
		{
			// default fallbacks, if shaders are unspecified
			if (!this.segmentationShader) {
				this.segmentationShader = Shader.Find("Hidden/UberReplacement");
			}

			if (!this.opticalFlowShader) {
				this.opticalFlowShader = Shader.Find("Hidden/OpticalFlow");
			}

			// use real camera to capture final image
			this._capture_passes[0]._Camera = this.GetComponent<Camera>();
			for (var q = 1; q < this._capture_passes.Length; q++) {
				this._capture_passes[q]._Camera = this.CreateHiddenCamera (this._capture_passes[q]._Name);
			}

			this.OnCameraChange();
			this.OnSceneChange();
		}

		void LateUpdate()
		{
			#if UNITY_EDITOR
			if (this.DetectPotentialSceneChangeInEditor()) {
				this.OnSceneChange();
			}
			#endif // UNITY_EDITOR

			this.OnCameraChange();
		}

		Camera CreateHiddenCamera(string name_)
		{
			var go = new GameObject (name_, typeof (Camera));
			go.hideFlags = HideFlags.HideAndDontSave;
			go.transform.parent = this.transform;

			var new_camera = go.GetComponent<Camera>();
			return new_camera;
		}


		static void SetupCameraWithReplacementShader(Camera cam, Shader shader, ReplacementModes mode)
		{
			SetupCameraWithReplacementShader(cam, shader, mode, Color.black);
		}

		static void SetupCameraWithReplacementShader(Camera cam, Shader shader, ReplacementModes mode, Color clear_color)
		{
			var cb = new CommandBuffer();
			cb.SetGlobalFloat("_OutputMode", (int)mode); // @TODO: CommandBuffer is missing SetGlobalInt() method
			cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, cb);
			cam.AddCommandBuffer(CameraEvent.BeforeFinalPass, cb);
			cam.SetReplacementShader(shader, "");
			cam.backgroundColor = clear_color;
			cam.clearFlags = CameraClearFlags.SolidColor;
		}

		static void SetupCameraWithPostShader(Camera cam, Material material, DepthTextureMode depth_texture_mode = DepthTextureMode.None)
		{
			var cb = new CommandBuffer();
			cb.Blit(null, BuiltinRenderTextureType.CurrentActive, material);
			cam.AddCommandBuffer(CameraEvent.AfterEverything, cb);
			cam.depthTextureMode = depth_texture_mode;
		}

		enum ReplacementModes {
			Object_id_ 			= 0,
			Catergory_id_			= 1,
			Depth_compressed_		= 2,
			Depth_multichannel_	= 3,
			Normals_				= 4
		}

		public void OnCameraChange()
		{
			var target_display = 1;
			var main_camera = this.GetComponent<Camera>();
			foreach (var pass in this._capture_passes)
			{
				if (pass._Camera == main_camera) {
					continue;
				}

				// cleanup capturing camera
				pass._Camera.RemoveAllCommandBuffers();

				// copy all "main" camera parameters into capturing camera
				pass._Camera.CopyFrom(main_camera);

				// set targetDisplay here since it gets overriden by CopyFrom()
				pass._Camera.targetDisplay = target_display++;
			}

			// cache materials and setup material properties
			if (!this._optical_flow_material || this._optical_flow_material.shader != this.opticalFlowShader) {
				this._optical_flow_material = new Material(this.opticalFlowShader);
			}

			this._optical_flow_material.SetFloat("_Sensitivity", this.opticalFlowSensitivity);

			// setup command buffers and replacement shaders
			SetupCameraWithReplacementShader(this._capture_passes[1]._Camera, this.segmentationShader, ReplacementModes.Object_id_);
			SetupCameraWithReplacementShader(this._capture_passes[2]._Camera, this.segmentationShader, ReplacementModes.Catergory_id_);
			SetupCameraWithReplacementShader(this._capture_passes[3]._Camera, this.segmentationShader, ReplacementModes.Depth_compressed_, Color.white);
			SetupCameraWithReplacementShader(this._capture_passes[4]._Camera, this.segmentationShader, ReplacementModes.Normals_);
			SetupCameraWithPostShader(this._capture_passes[5]._Camera, this._optical_flow_material, DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
		}


		public void OnSceneChange()
		{
			var renderers = FindObjectsOfType<Renderer>();
			var mpb = new MaterialPropertyBlock();
			foreach (var r in renderers)
			{
				GameObject game_object;
				var id = (game_object = r.gameObject).GetInstanceID();
				var layer = game_object.layer;
				var tag_ = game_object.tag;

				mpb.SetColor("_ObjectColor", ColorEncoding.EncodeIdAsColor(id));
				mpb.SetColor("_CategoryColor", ColorEncoding.EncodeLayerAsColor(layer));
				r.SetPropertyBlock(mpb);
			}
		}

		public void Save(string filename, int width = -1, int height = -1, string path = "")
		{
			if (width <= 0 || height <= 0)
			{
				width = Screen.width;
				height = Screen.height;
			}

			var filename_extension = Path.GetExtension(filename);
			if (filename_extension == "") {
				filename_extension = ".png";
			}

			var filename_without_extension = Path.GetFileNameWithoutExtension(filename);

			var path_without_extension = Path.Combine(path, filename_without_extension);

			// execute as coroutine to wait for the EndOfFrame before starting capture
			this.StartCoroutine(
			               this.WaitForEndOfFrameAndSave(path_without_extension, filename_extension, width, height));
		}

		IEnumerator WaitForEndOfFrameAndSave(string filename_without_extension, string filename_extension, int width, int height)
		{
			yield return new WaitForEndOfFrame();
			this.Save(filename_without_extension, filename_extension, width, height);
		}

		void Save(string filename_without_extension, string filename_extension, int width, int height)
		{
			foreach (var pass in this._capture_passes) {
				this.Save(pass._Camera, filename_without_extension + pass._Name + filename_extension, width, height, pass._SupportsAntialiasing, pass._NeedsRescale);
			}
		}

		void Save(Camera cam, string filename, int width, int height, bool supports_antialiasing, bool needs_rescale)
		{
			var main_camera = this.GetComponent<Camera>();
			var depth = 24;
			var format = RenderTextureFormat.Default;
			var read_write = RenderTextureReadWrite.Default;
			var anti_aliasing = (supports_antialiasing) ? Mathf.Max(1, QualitySettings.antiAliasing) : 1;

			var final_rt =
					RenderTexture.GetTemporary(width, height, depth, format, read_write, anti_aliasing);
			var render_rt = (!needs_rescale) ? final_rt :
					               RenderTexture.GetTemporary(main_camera.pixelWidth, main_camera.pixelHeight, depth, format, read_write, anti_aliasing);
			var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

			var prev_active_rt = RenderTexture.active;
			var prev_camera_rt = cam.targetTexture;

			// render to offscreen texture (readonly from CPU side)
			RenderTexture.active = render_rt;
			cam.targetTexture = render_rt;

			cam.Render();

			if (needs_rescale)
			{
				// blit to rescale (see issue with Motion Vectors in @KNOWN ISSUES)
				RenderTexture.active = final_rt;
				Graphics.Blit(render_rt, final_rt);
				RenderTexture.ReleaseTemporary(render_rt);
			}

			// read offsreen texture contents into the CPU readable texture
			tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
			tex.Apply();

			// encode texture into PNG
			var bytes = tex.EncodeToPNG();
			File.WriteAllBytes(filename, bytes);

			// restore state and cleanup
			cam.targetTexture = prev_camera_rt;
			RenderTexture.active = prev_active_rt;

			Destroy(tex);
			RenderTexture.ReleaseTemporary(final_rt);
		}

		#if UNITY_EDITOR
		GameObject _last_selected_go;
		int _last_selected_go_layer = -1;
		string _last_selected_go_tag = "unknown";
		bool DetectPotentialSceneChangeInEditor()
		{
			var change = false;
			// there is no callback in Unity Editor to automatically detect changes in scene objects
			// as a workaround lets track selected objects and check, if properties that are
			// interesting for us (layer or tag) did not change since the last frame
			if (Selection.transforms.Length > 1)
			{
				// multiple objects are selected, all bets are off!
				// we have to assume these objects are being edited
				change = true;
				this._last_selected_go = null;
			}
			else if (Selection.activeGameObject)
			{
				var go = Selection.activeGameObject;
				// check if layer or tag of a selected object have changed since the last frame
				var potential_change_happened = this._last_selected_go_layer != go.layer || this._last_selected_go_tag != go.tag;
				if (go == this._last_selected_go && potential_change_happened) {
					change = true;
				}

				this._last_selected_go = go;
				this._last_selected_go_layer = go.layer;
				this._last_selected_go_tag = go.tag;
			}

			return change;
		}
		#endif // UNITY_EDITOR
	}
}
