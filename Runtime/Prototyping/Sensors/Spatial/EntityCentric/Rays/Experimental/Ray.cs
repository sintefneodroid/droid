using System;
using System.Collections.Generic;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// Ray perception component. Attach this to agents to enable "local perception"
  /// via the use of ray casts directed outward from the agent.
  /// </summary>
  public class Ray : MonoBehaviour {
    List<float> _perception_buffer = new List<float>();
    Vector3 _end_position;
    RaycastHit _hit;

    /// <summary>
    /// Creates perception vector to be used as part of an observation of an agent.
    /// </summary>
    /// <returns>The partial vector observation corresponding to the set of rays</returns>
    /// <param name="ray_distance">Radius of rays</param>
    /// <param name="ray_angles">Anlges of rays (starting from (1,0) on unit circle).</param>
    /// <param name="detectable_objects">List of tags which correspond to object types agent can see</param>
    /// <param name="start_offset">Starting height offset of ray from center of agent.</param>
    /// <param name="end_offset">Ending height offset of ray from center of agent.</param>
    public List<float> Perceive(float ray_distance,
                                IEnumerable<Single> ray_angles,
                                string[] detectable_objects,
                                float start_offset,
                                float end_offset) {
      this._perception_buffer.Clear();
      // For each ray sublist stores categorial information on detected object
      // along with object distance.
      foreach (var angle in ray_angles) {
        this._end_position = this.transform.TransformDirection(PolarToCartesian(ray_distance, angle));
        this._end_position.y = end_offset;
        if (Application.isEditor) {
          Debug.DrawRay(this.transform.position + new Vector3(0f, start_offset, 0f),
                        this._end_position,
                        Color.black,
                        0.01f,
                        true);
        }

        var sub_list = new float[detectable_objects.Length + 2];
        if (Physics.SphereCast(this.transform.position + new Vector3(0f, start_offset, 0f),
                               0.5f,
                               this._end_position,
                               out this._hit,
                               ray_distance)) {
          for (var i = 0; i < detectable_objects.Length; i++) {
            if (this._hit.collider.gameObject.CompareTag(detectable_objects[i])) {
              sub_list[i] = 1;
              sub_list[detectable_objects.Length + 1] = this._hit.distance / ray_distance;
              break;
            }
          }
        } else {
          sub_list[detectable_objects.Length] = 1f;
        }

        this._perception_buffer.AddRange(sub_list);
      }

      return this._perception_buffer;
    }

    /// <summary>
    /// Converts polar coordinate to cartesian coordinate.
    /// </summary>
    public static Vector3 PolarToCartesian(float radius, float angle) {
      var x = radius * Mathf.Cos(DegreeToRadian(angle));
      var z = radius * Mathf.Sin(DegreeToRadian(angle));
      return new Vector3(x, 0f, z);
    }

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    public static float DegreeToRadian(float degree) { return degree * Mathf.PI / 180f; }
  }
}
