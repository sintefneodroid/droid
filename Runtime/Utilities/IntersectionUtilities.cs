using UnityEngine;

namespace droid.Runtime.Utilities {
  public static class IntersectionUtilities {
    // https://www.geometrictools.com/Documentation/IntersectionSphereCone.pdf
    public static bool ConeSphereIntersection(Light spot_light,
                                              Transform dynamic_object,
                                              float boundingSphereRadius = 1) {
      // FIXME Optimize computations of boundingSphereRadius^2, 1.0f / Mathf.Sin(angle * 0.5f), Mathf.Sin(angle * 0.5f)^2 and  Mathf.Cos(angle * 0.5f)^2
      var sin = Mathf.Sin(spot_light.spotAngle * 0.5f * Mathf.Deg2Rad);
      var cos = Mathf.Cos(spot_light.spotAngle * 0.5f * Mathf.Deg2Rad);
      var offset = boundingSphereRadius / sin;
      var transform = spot_light.transform;
      var forward = transform.forward;
      var u = transform.position - offset * forward; // Assumes unit length forward.
      var d = dynamic_object.transform.position - u;

      var distance = Vector3.Dot(forward, d); // Assumes unit length forward.

      return distance >= d.magnitude * cos && distance <= offset + spot_light.range + boundingSphereRadius;
    }

    public static bool SphereSphereIntersection(Light point_light,
                                                Transform dynamic_object,
                                                float boundingSphereRadius = 1) {
      var diff = dynamic_object.transform.position - point_light.transform.position;
      return diff.magnitude - boundingSphereRadius <= point_light.range;
    }
  }
}
