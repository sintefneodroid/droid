using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Conditional = System.Diagnostics.ConditionalAttribute;
using Object = UnityEngine.Object;

namespace droid.Editor.Utilities {
/*

Important Notes:
 	1.	You are expected to make some changes in this file before intergrating this into you projects.
 			a.	`_DEBUG` symbol, you should this to your project's debugging symbol so these draw calls will be compiled away in final release builds.
 				If you forget to do this, DrawXXX calls won't be shown.
 			b.	`RuntimeDebugDraw` namespace and `Draw` class name, you can change this into your project's namespace to make it more accessable.
 			c.	`Draw.DrawLineLayer` is the layer the lines will be drawn on. If you have camera postprocessing turned on, set this to a layer that is ignored
 				by the post processor.
 			d.	`GetDebugDrawCamera()` will be called to get the camera for line drawings and text coordinate calcuation.
 				It defaults to `Camera.main`, returning null will mute drawings.
 			e.	`DrawTextDefaultSize`/`DrawDefaultColor` styling variables, defaults as Unity Debug.Draw.
 	2.	Performance should be relatively ok for debugging,  but it's never intended for release use. You should use conditional to
 		compile away these calls anyway. Additionally DrawText is implemented with OnGUI, which costs a lot on mobile devices.
 	3.	Don't rename this file of 'RuntimeDebugDraw' or this won't work. This file contains a MonoBehavior also named 'RuntimeDebugDraw' and Unity needs this file
 		to have the same name. If you really want to rename this file, remember to rename the 'RuntimeDebugDraw' class below too.

 */

  namespace RuntimeDebugDraw {
    /// <summary>
    /// </summary>
    public static class Draw {
      #region Editor

      #if UNITY_EDITOR
      /// <summary>
      /// </summary>
      [InitializeOnLoad]
      public static class DrawEditor {
        static DrawEditor() {
          //	set a low execution order
          var name = typeof(RuntimeDebugDraw.Internal.RuntimeDebugDraw).Name;
          foreach (var mono_script in MonoImporter.GetAllRuntimeMonoScripts()) {
            if (name != mono_script.name) {
              continue;
            }

            if (MonoImporter.GetExecutionOrder(mono_script) != 9990) {
              MonoImporter.SetExecutionOrder(mono_script, 9990);
              return;
            }
          }
        }
      }
      #endif

      #endregion

      #region Main Functions

      /// <summary>
      ///   Which layer the lines will be drawn on.
      /// </summary>
      public const int _DrawLineLayer = 4;

      /// <summary>
      ///   Default font size for DrawText.
      /// </summary>
      public const int _DrawTextDefaultSize = 12;

      /// <summary>
      ///   Default color for Draws.
      /// </summary>
      public static Color _DrawDefaultColor = Color.white;

      /// <summary>
      ///   Which camera to use for line drawing and texts coordinate calculation.
      /// </summary>
      /// <returns>Camera to debug draw on, returns null will mute debug drawing.</returns>
      public static Camera GetDebugDrawCamera() { return Camera.main; }

      const string _conditional_flag = "NEODROID_DEBUG";

      /// <summary>
      ///   Draw a line from <paramref name="start" /> to <paramref name="end" /> with <paramref name="color" />.
      /// </summary>
      /// <param name="start">Point in world space where the line should start.</param>
      /// <param name="end">Point in world space where the line should end.</param>
      /// <param name="color">Color of the line.</param>
      /// <param name="duration">How long the line should be visible for.</param>
      /// <param name="depth_test">Should the line be obscured by objects closer to the camera?</param>
      [Conditional(_conditional_flag)]
      public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depth_test) {
        CheckAndBuildHiddenRtDrawObject();
        _rt_draw.RegisterLine(start,
                              end,
                              color,
                              duration,
                              !depth_test);
      }

      /// <summary>
      ///   Draws a line from start to start + dir in world coordinates.
      /// </summary>
      /// <param name="start">Point in world space where the ray should start.</param>
      /// <param name="dir">Direction and length of the ray.</param>
      /// <param name="color">Color of the drawn line.</param>
      /// <param name="duration">How long the line will be visible for (in seconds).</param>
      /// <param name="depth_test">Should the line be obscured by other objects closer to the camera?</param>
      [Conditional(_conditional_flag)]
      public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depth_test) {
        CheckAndBuildHiddenRtDrawObject();
        _rt_draw.RegisterLine(start,
                              start + dir,
                              color,
                              duration,
                              !depth_test);
      }

      /// <summary>
      ///   Draw a text at given position.
      /// </summary>
      /// <param name="pos">Position</param>
      /// <param name="text">String of the text.</param>
      /// <param name="color">Color for the text.</param>
      /// <param name="size">Font size for the text.</param>
      /// <param name="duration">How long the text should be visible for.</param>
      /// <param name="pop_up">
      ///   Set to true to let the text moving up, so multiple texts at the same position can be
      ///   visible.
      /// </param>
      [Conditional(_conditional_flag)]
      public static void DrawText(Vector3 pos,
                                  string text,
                                  Color color,
                                  int size,
                                  float duration,
                                  bool pop_up = false) {
        CheckAndBuildHiddenRtDrawObject();
        _rt_draw.RegisterDrawText(pos,
                                  text,
                                  color,
                                  size,
                                  duration,
                                  pop_up);
      }

      /// <summary>
      ///   Attach text to a transform.
      /// </summary>
      /// <param name="transform">Target transform to attach text to.</param>
      /// <param name="str_func">Function will be called on every frame to get a string as attached text. </param>
      /// <param name="offset">Text attach offset to transform position.</param>
      /// <param name="color">Color for the text.</param>
      /// <param name="size">Font size for the text.</param>
      [Conditional(_conditional_flag)]
      public static void AttachText(Transform transform,
                                    Func<string> str_func,
                                    Vector3 offset,
                                    Color color,
                                    int size) {
        CheckAndBuildHiddenRtDrawObject();
        _rt_draw.RegisterAttachText(transform,
                                    str_func,
                                    offset,
                                    color,
                                    size);
      }

      #endregion

      #region Internal

      /// <summary>
      ///   Singleton RuntimeDebugDraw component that is needed to call Unity APIs.
      /// </summary>
      static RuntimeDebugDraw.Internal.RuntimeDebugDraw _rt_draw;

      /// <summary>
      ///   Check and build
      /// </summary>
      const string _hidden_go_name = "________HIDDEN_C4F6A87F298241078E21C0D7C1D87A76_";

      static void CheckAndBuildHiddenRtDrawObject() {
        if (_rt_draw != null) {
          return;
        }

        //	try reuse existing one first
        _rt_draw = Object.FindObjectOfType<RuntimeDebugDraw.Internal.RuntimeDebugDraw>();
        if (_rt_draw != null) {
          return;
        }

        //	instantiate an hidden game_object w/ RuntimeDebugDraw attached.
        //	hardcode an GUID in the name so one won't accidentally get this by name.
        var go = new GameObject(_hidden_go_name);
        var child_go = new GameObject(_hidden_go_name);
        child_go.transform.parent = go.transform;
        _rt_draw = child_go.AddComponent<RuntimeDebugDraw.Internal.RuntimeDebugDraw>();
        //	hack to only hide outer go, so that RuntimeDebugDraw's OnGizmos will work properly.
        go.hideFlags = HideFlags.HideAndDontSave;
        if (Application.isPlaying) {
          Object.DontDestroyOnLoad(go);
        }
      }

      #endregion
    }

    namespace RuntimeDebugDraw.Internal {
      /// <inheritdoc />
      /// <summary>
      /// </summary>
      class RuntimeDebugDraw : MonoBehaviour {
        #region Basics

        void CheckInitialized() {
          //	as RuntimeDebugDraw component has a very low execution order, other script might Awake()
          //	earlier than this and at that moment it's not initialized. check and init on every public
          //	member
          if (this._draw_text_entries == null) {
            this._z_test_batch = new BatchedLineDraw(true);
            this._always_batch = new BatchedLineDraw(false);
            this._line_entries = new List<DrawLineEntry>(16);

            this._text_style = new GUIStyle {alignment = TextAnchor.UpperLeft};
            this._draw_text_entries = new List<DrawTextEntry>(16);
            this._attach_text_entries = new List<AttachTextEntry>(16);
          }
        }

        void Awake() { this.CheckInitialized(); }

        void OnGUI() { this.DrawTextOnGui(); }

        #if UNITY_EDITOR
        void OnDrawGizmos() { this.DrawTextOnDrawGizmos(); }
        #endif

        void LateUpdate() {
          this.TickAndDrawLines();
          this.TickTexts();
        }

        void OnDestroy() {
          this._always_batch.Dispose();
          this._z_test_batch.Dispose();
        }

        void Clear() {
          this._draw_text_entries.Clear();
          this._line_entries.Clear();
          this._lines_need_rebuild = true;
        }

        #endregion

        #region Draw Lines

        /// <summary>
        /// </summary>
        class DrawLineEntry {
          public Color _Color;
          public Vector3 _End;
          public bool _NoZTest;
          public bool _Occupied;
          public Vector3 _Start;
          public float _Timer;
        }

        List<DrawLineEntry> _line_entries;

        //	helper class for batching
        /// <summary>
        /// </summary>
        class BatchedLineDraw : IDisposable {
          List<Color> _colors;
          List<int> _indices;
          public Material _Mat;
          public Mesh _Mesh;

          List<Vector3> _vertices;

          public BatchedLineDraw(bool depth_test) {
            this._Mesh = new Mesh();
            this._Mesh.MarkDynamic();

            //	relying on a builtin shader, but it shouldn't change that much.
            this._Mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            this._Mat.SetInt(_z_test,
                             depth_test
                                 ? 4 // LEqual
                                 : 0 // Always
                            );

            this._vertices = new List<Vector3>();
            this._colors = new List<Color>();
            this._indices = new List<int>();
          }

          public void Dispose() {
            DestroyImmediate(this._Mesh);
            DestroyImmediate(this._Mat);
          }

          public void AddLine(Vector3 from, Vector3 to, Color color) {
            this._vertices.Add(from);
            this._vertices.Add(to);
            this._colors.Add(color);
            this._colors.Add(color);
            var vertice_count = this._vertices.Count;
            this._indices.Add(vertice_count - 2);
            this._indices.Add(vertice_count - 1);
          }

          public void Clear() {
            this._Mesh.Clear();
            this._vertices.Clear();
            this._colors.Clear();
            this._indices.Clear();
          }

          public void BuildBatch() {
            this._Mesh.SetVertices(this._vertices);
            this._Mesh.SetColors(this._colors);
            this._Mesh.SetIndices(this._indices.ToArray(),
                                  MeshTopology.Lines,
                                  0); // cant get rid of this alloc for now
          }
        }

        BatchedLineDraw _z_test_batch;
        BatchedLineDraw _always_batch;
        bool _lines_need_rebuild;

        public void RegisterLine(Vector3 start, Vector3 end, Color color, float timer, bool no_z_test) {
          this.CheckInitialized();

          DrawLineEntry entry = null;
          foreach (var t in this._line_entries) {
            if (!t._Occupied) {
              entry = t;
              break;
            }
          }

          if (entry == null) {
            entry = new DrawLineEntry();
            this._line_entries.Add(entry);
          }

          entry._Occupied = true;
          entry._Start = start;
          entry._End = end;
          entry._Color = color;
          entry._Timer = timer;
          entry._NoZTest = no_z_test;
          this._lines_need_rebuild = true;
        }

        void RebuildDrawLineBatchMesh() {
          this._z_test_batch.Clear();
          this._always_batch.Clear();

          foreach (var entry in this._line_entries) {
            if (!entry._Occupied) {
              continue;
            }

            if (entry._NoZTest) {
              this._always_batch.AddLine(entry._Start, entry._End, entry._Color);
            } else {
              this._z_test_batch.AddLine(entry._Start, entry._End, entry._Color);
            }
          }

          this._z_test_batch.BuildBatch();
          this._always_batch.BuildBatch();
        }

        void TickAndDrawLines() {
          if (this._lines_need_rebuild) {
            this.RebuildDrawLineBatchMesh();
            this._lines_need_rebuild = false;
          }

          //	draw on UI layer which should bypass most postFX setups
          Graphics.DrawMesh(this._always_batch._Mesh,
                            Vector3.zero,
                            Quaternion.identity,
                            this._always_batch._Mat,
                            Draw._DrawLineLayer,
                            null,
                            0,
                            null,
                            false,
                            false);
          Graphics.DrawMesh(this._z_test_batch._Mesh,
                            Vector3.zero,
                            Quaternion.identity,
                            this._z_test_batch._Mat,
                            Draw._DrawLineLayer,
                            null,
                            0,
                            null,
                            false,
                            false);

          //	update timer late so every added entry can be drawed for at least one frame
          foreach (var entry in this._line_entries) {
            if (!entry._Occupied) {
              continue;
            }

            entry._Timer -= Time.deltaTime;
            if (entry._Timer < 0) {
              entry._Occupied = false;
              this._lines_need_rebuild = true;
            }
          }
        }

        #endregion

        #region Draw Text

        [Flags]
        public enum DrawFlag : byte {
          None_ = 0,
          Drawn_gizmo_ = 1 << 0,
          Drawn_gui_ = 1 << 1,
          Drawn_all_ = Drawn_gizmo_ | Drawn_gui_
        }

        class DrawTextEntry {
          public Vector3 _Anchor;
          public Color _Color;
          public GUIContent _Content;
          public float _Duration;

          //	Text entries needs to be draw in both OnGUI/OnDrawGizmos, need flags for mark
          //	has been visited by both
          public DrawFlag _Flag = DrawFlag.None_;
          public bool _Occupied;
          public bool _PopUp;
          public int _Size;
          public float _Timer;

          public DrawTextEntry() { this._Content = new GUIContent(); }
        }

        /// <summary>
        /// </summary>
        class AttachTextEntry {
          public Color _Color;
          public GUIContent _Content;

          public DrawFlag _Flag = DrawFlag.None_;
          public bool _Occupied;
          public Vector3 _Offset;
          public int _Size;
          public Func<string> _StrFunc;

          public Transform _Transform;

          public AttachTextEntry() { this._Content = new GUIContent(); }
        }

        List<DrawTextEntry> _draw_text_entries;
        List<AttachTextEntry> _attach_text_entries;
        GUIStyle _text_style;
        static readonly Int32 _z_test = Shader.PropertyToID("_ZTest");

        public void RegisterDrawText(Vector3 anchor,
                                     string text,
                                     Color color,
                                     int size,
                                     float timer,
                                     bool pop_up) {
          this.CheckInitialized();

          DrawTextEntry entry = null;
          foreach (var t in this._draw_text_entries) {
            if (!t._Occupied) {
              entry = t;
              break;
            }
          }

          if (entry == null) {
            entry = new DrawTextEntry();
            this._draw_text_entries.Add(entry);
          }

          entry._Occupied = true;
          entry._Anchor = anchor;
          entry._Content.text = text;
          entry._Size = size;
          entry._Color = color;
          entry._Duration = entry._Timer = timer;
          entry._PopUp = pop_up;
          #if UNITY_EDITOR
          entry._Flag = DrawFlag.None_;
          #else
          //	in builds consider gizmo is already drawn
			entry.flag = DrawFlag.DrawnGizmo;
          #endif
        }

        public void RegisterAttachText(Transform target,
                                       Func<string> str_func,
                                       Vector3 offset,
                                       Color color,
                                       int size) {
          this.CheckInitialized();

          AttachTextEntry entry = null;
          foreach (var t in this._attach_text_entries) {
            if (!t._Occupied) {
              entry = t;
              break;
            }
          }

          if (entry == null) {
            entry = new AttachTextEntry();
            this._attach_text_entries.Add(entry);
          }

          entry._Occupied = true;
          entry._Offset = offset;
          entry._Transform = target;
          entry._StrFunc = str_func;
          entry._Color = color;
          entry._Size = size;
          //	get first text
          entry._Content.text = str_func();
          #if UNITY_EDITOR
          entry._Flag = DrawFlag.None_;
          #else
          //	in builds consider gizmo is already drawn
			entry.flag = DrawFlag.DrawnGizmo;
          #endif
        }

        void TickTexts() {
          foreach (var entry in this._draw_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            entry._Timer -= Time.deltaTime;
            if (entry._Flag == DrawFlag.Drawn_all_) {
              if (entry._Timer < 0) {
                entry._Occupied = false;
              }

              //	actually no need to tick DrawFlag as it won't move
            }
          }

          foreach (var entry in this._attach_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            if (entry._Transform == null) {
              entry._Occupied = false;
              entry._StrFunc = null; // needs to release ref to callback
            } else if (entry._Flag == DrawFlag.Drawn_all_) {
              // tick content
              entry._Content.text = entry._StrFunc();
              // tick flag
              #if UNITY_EDITOR
              entry._Flag = DrawFlag.None_;
              #else
              //	in builds consider gizmo is already drawn
					entry.flag = DrawFlag.DrawnGizmo;
              #endif
            }
          }
        }

        void DrawTextOnGui() {
          var this_camera = Draw.GetDebugDrawCamera();
          if (this_camera == null) {
            return;
          }

          foreach (var entry in this._draw_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            this.GuiDrawTextEntry(this_camera, entry);
            entry._Flag |= DrawFlag.Drawn_gui_;
          }

          foreach (var entry in this._attach_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            this.GuiAttachTextEntry(this_camera, entry);
            entry._Flag |= DrawFlag.Drawn_gui_;
          }
        }

        void GuiDrawTextEntry(Camera n_camera, DrawTextEntry entry) {
          var world_pos = entry._Anchor;
          var screen_pos = n_camera.WorldToScreenPoint(world_pos);
          screen_pos.y = Screen.height - screen_pos.y;

          if (entry._PopUp) {
            var ratio = entry._Timer / entry._Duration;
            screen_pos.y -= (1 - ratio * ratio) * entry._Size * 1.5f;
          }

          this._text_style.normal.textColor = entry._Color;
          this._text_style.fontSize = entry._Size;
          var rect = new Rect(screen_pos, this._text_style.CalcSize(entry._Content));
          GUI.Label(rect, entry._Content, this._text_style);
        }

        void GuiAttachTextEntry(Camera n_camera, AttachTextEntry entry) {
          if (entry._Transform == null) {
            return;
          }

          var world_pos = entry._Transform.position + entry._Offset;
          var screen_pos = n_camera.WorldToScreenPoint(world_pos);
          screen_pos.y = Screen.height - screen_pos.y;

          this._text_style.normal.textColor = entry._Color;
          this._text_style.fontSize = entry._Size;
          var rect = new Rect(screen_pos, this._text_style.CalcSize(entry._Content));
          GUI.Label(rect, entry._Content, this._text_style);
        }

        #if UNITY_EDITOR
        void DrawTextOnDrawGizmos() {
          if (!(Camera.current == Draw.GetDebugDrawCamera()
                || Camera.current == SceneView.lastActiveSceneView.camera)) {
            return;
          }

          var n_camera = Camera.current;
          if (n_camera == null) {
            return;
          }

          Handles.BeginGUI();
          foreach (var entry in this._draw_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            this.GuiDrawTextEntry(n_camera, entry);
            entry._Flag |= DrawFlag.Drawn_gizmo_;
          }

          foreach (var entry in this._attach_text_entries) {
            if (!entry._Occupied) {
              continue;
            }

            this.GuiAttachTextEntry(n_camera, entry);
            entry._Flag |= DrawFlag.Drawn_gizmo_;
          }

          Handles.EndGUI();
        }
        #endif

        #endregion
      }
    }
  }
}
