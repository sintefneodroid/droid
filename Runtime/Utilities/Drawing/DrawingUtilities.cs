using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Utilities.Drawing {
  /// <summary>
  /// </summary>
  public static partial class NeodroidDrawingUtilities {
    /// <summary>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <param name="arrow_head_length"></param>
    /// <param name="arrow_head_angle"></param>
    public static void ForGizmo(Vector3 pos,
                                Vector3 direction,
                                Color color,
                                float arrow_head_length = 0.25f,
                                float arrow_head_angle = 20.0f) {
      Gizmos.DrawRay(pos, direction);
      NeodroidDrawingUtilitiesEnd(true,
                                  pos,
                                  direction,
                                  color,
                                  arrow_head_length,
                                  arrow_head_angle);
    }

    /// <summary>
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <param name="arrow_head_length"></param>
    /// <param name="arrow_head_angle"></param>
    /// <param name="ray_duration"></param>
    public static void ForDebug(Vector3 pos,
                                Vector3 direction,
                                Color color,
                                float arrow_head_length = 0.25f,
                                float arrow_head_angle = 20.0f,
                                float ray_duration = 0f) {
      if (ray_duration > 0) {
        Debug.DrawRay(pos,
                      direction,
                      color,
                      ray_duration);
      } else {
        Debug.DrawRay(pos, direction, color);
      }

      NeodroidDrawingUtilitiesEnd(false,
                                  pos,
                                  direction,
                                  color,
                                  arrow_head_length,
                                  arrow_head_angle,
                                  ray_duration);
    }

    static void NeodroidDrawingUtilitiesEnd(bool gizmos,
                                            Vector3 pos,
                                            Vector3 direction,
                                            Color color,
                                            float arrow_head_length = 0.25f,
                                            float arrow_head_angle = 20.0f,
                                            float ray_duration = 0f) {
      var right = Quaternion.LookRotation(direction)
                  * Quaternion.Euler(arrow_head_angle, 0, 0)
                  * Vector3.back;
      var left = Quaternion.LookRotation(direction)
                 * Quaternion.Euler(-arrow_head_angle, 0, 0)
                 * Vector3.back;
      var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrow_head_angle, 0) * Vector3.back;
      var down = Quaternion.LookRotation(direction)
                 * Quaternion.Euler(0, -arrow_head_angle, 0)
                 * Vector3.back;
      if (gizmos) {
        Gizmos.color = color;
        Gizmos.DrawRay(pos + direction, right * arrow_head_length);
        Gizmos.DrawRay(pos + direction, left * arrow_head_length);
        Gizmos.DrawRay(pos + direction, up * arrow_head_length);
        Gizmos.DrawRay(pos + direction, down * arrow_head_length);
      } else {
        if (ray_duration > 0) {
          Debug.DrawRay(pos + direction,
                        right * arrow_head_length,
                        color,
                        ray_duration);
          Debug.DrawRay(pos + direction,
                        left * arrow_head_length,
                        color,
                        ray_duration);
          Debug.DrawRay(pos + direction,
                        up * arrow_head_length,
                        color,
                        ray_duration);
          Debug.DrawRay(pos + direction,
                        down * arrow_head_length,
                        color,
                        ray_duration);
        } else {
          Debug.DrawRay(pos + direction, right * arrow_head_length, color);
          Debug.DrawRay(pos + direction, left * arrow_head_length, color);
          Debug.DrawRay(pos + direction, up * arrow_head_length, color);
          Debug.DrawRay(pos + direction, down * arrow_head_length, color);
        }
      }

/*
      var arrow_size = 2;
      Handles.color = Handles.xAxisColor;
      Handles.ArrowHandleCap( 0, pos, Quaternion.Euler(direction), arrow_size,EventType.Repaint );
 */
    }

    /// <summary>
    /// </summary>
    /// <param name="rb"></param>
    /// <returns></returns>
    public static float KineticEnergy(Rigidbody rb) {
      return
          0.5f
          * rb.mass
          * Mathf.Pow(rb.velocity.magnitude,
                      2); // mass in kg, velocity in meters per second, result is joules
    }

    /// <summary>
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="width"></param>
    public static void DrawLine(Vector3 p1, Vector3 p2, float width) {
      var count = Mathf.CeilToInt(width); // how many lines are needed.
      if (count == 1) {
        Gizmos.DrawLine(p1, p2);
      } else {
        var c = Camera.current;
        if (c == null) {
          Debug.LogError("Camera.current is null");
          return;
        }

        var v1 = (p2 - p1).normalized; // line direction
        var v2 = (c.transform.position - p1).normalized; // direction to camera
        var n = Vector3.Cross(v1, v2); // normal vector
        for (var i = 0; i < count; i++) {
          //Vector3 o = n * width ((float)i / (count - 1) - 0.5f);
          var o = width * ((float)i / (count - 1) - 0.5f) * n;
          Gizmos.DrawLine(p1 + o, p2 + o);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Texture2D RenderTextureImage(Camera camera) {
      // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      var current_render_texture = RenderTexture.active;
      RenderTexture.active = camera.targetTexture;
      camera.Render();
      var target_texture = camera.targetTexture;
      var texture = new Texture2D(target_texture.width, target_texture.height);
      texture.ReadPixels(new Rect(0,
                                  0,
                                  target_texture.width,
                                  target_texture.height),
                         0,
                         0);
      texture.Apply();
      RenderTexture.active = current_render_texture;
      return texture;
    }

    /// <summary>
    /// </summary>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string ColorArrayToString(IEnumerable<Color> colors) {
      var s = "";
      foreach (var color in colors) {
        s += color.ToString();
      }

      return s;
    }

    /** Contains logic for converting a camera component into a Texture2D. Soft ignored */
    /*public Texture2D ObservationToTex(Camera camera, int width, int height)
        {
          Camera cam = camera;
          Rect oldRec = camera.rect;
          cam.rect = new Rect(0f, 0f, 1f, 1f);
          bool supportsAntialiasing = false;
          bool needsRescale = false;
          var depth = 24;
          var format = RenderTextureFormat.Default;
          var readWrite = RenderTextureReadWrite.Default;
          var antiAliasing = (supportsAntialiasing) ? Mathf.Max(1, QualitySettings.antiAliasing) : 1;

          var finalRT =
            RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
          var renderRT = (!needsRescale) ? finalRT :
            RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
          var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

          var prevActiveRT = RenderTexture.active;
          var prevCameraRT = cam.targetTexture;

          // render to offscreen texture ( from CPU side)
          RenderTexture.active = renderRT;
          cam.targetTexture = renderRT;

          cam.Render();

          if (needsRescale)
          {
            RenderTexture.active = finalRT;
            Graphics.Blit(renderRT, finalRT);
            RenderTexture.ReleaseTemporary(renderRT);
          }

          tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
          tex.Apply();
          cam.targetTexture = prevCameraRT;
          cam.rect = oldRec;
          RenderTexture.active = prevActiveRT;
          RenderTexture.ReleaseTemporary(finalRT);
          return tex;
        }

        /// Contains logic to convert the agent's cameras into observation list
        ///  (as list of float arrays)
        public List<float[,,,]> GetObservationMatrixList(List<int> agent_keys)
        {
          List<float[,,,]> observation_matrix_list = new List<float[,,,]>();
          Dictionary<int, List<Camera>> observations = CollectObservations();
          for (int obs_number = 0; obs_number < brainParameters.cameraResolutions.Length; obs_number++)
          {
            int width = brainParameters.cameraResolutions[obs_number].width;
            int height = brainParameters.cameraResolutions[obs_number].height;
            bool bw = brainParameters.cameraResolutions[obs_number].blackAndWhite;
            int pixels = 0;
            if (bw)
              pixels = 1;
            else
              pixels = 3;
            float[,,,] observation_matrix = new float[agent_keys.Count
              , height
              , width
              , pixels];
            int i = 0;
            foreach (int k in agent_keys)
            {
              Camera agent_obs = observations[k][obs_number];
              Texture2D tex = ObservationToTex(agent_obs, width, height);
              for (int w = 0; w < width; w++)
              {
                for (int h = 0; h < height; h++)
                {
                  Color c = tex.GetPixel(w, h);
                  if (!bw)
                  {
                    observation_matrix[i, tex.height - h - 1, w, 0] = c.r;
                    observation_matrix[i, tex.height - h - 1, w, 1] = c.g;
                    observation_matrix[i, tex.height - h - 1, w, 2] = c.b;
                  }
                  else
                  {
                    observation_matrix[i, tex.height - h - 1, w, 0] = (c.r + c.g + c.b) / 3;
                  }
                }
              }
              UnityEngine.Object.DestroyImmediate(tex);
              Resources.UnloadUnusedAssets();
              i++;
            }
            observation_matrix_list.Add(observation_matrix);
          }
          return observation_matrix_list;
        }*/
  }
}
