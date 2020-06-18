using droid.Runtime.Utilities.Extensions;
using UnityEngine;

namespace droid.Runtime.Structs.Space {
  public static class Normalisation {
    internal static float Normalise01_(float v, float min, float span) { return (v - min) / span; }
    internal static float Denormalise01_(float v, float min, float span) { return v * span + min; }

    internal static Vector2 Normalise01_(Vector2 v, Vector2 min, Vector2 span) { return (v - min) / span; }
    internal static Vector2 Denormalise01_(Vector2 v, Vector2 min, Vector2 span) { return v * span + min; }

    internal static Vector3 Normalise01_(Vector3 v, Vector3 min, Vector3 span) {
      return (v - min).Divide(b : span);
    }

    internal static Vector3 Denormalise01_(Vector3 v, Vector3 min, Vector3 span) {
      return v.Multiply(b : span) + min;
    }

    internal static Vector4 Normalise01_(Vector4 v, Vector4 min, Vector4 span) {
      return (v - min).Divide(b : span);
    }

    internal static Vector4 Denormalise01_(Vector4 v, Vector4 min, Vector4 span) {
      return v.Multiply(b : span) + min;
    }

    internal static float NormaliseMinusOneOne_(dynamic v, float min, float span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f) * 2;
    }

    internal static float DenormaliseMinusOneOne_(float v, float min, float span) {
      return Denormalise01_(v : v / 2 + .5f, min : min, span : span);
    }

    internal static Vector2 NormaliseMinusOneOne_(Vector2 v, Vector2 min, Vector2 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * Vector2.one) * 2;
    }

    internal static Vector2 DenormaliseMinusOneOne_(Vector2 v, Vector2 min, Vector2 span) {
      return Denormalise01_(v : v / 2 + .5f * Vector2.one, min : min, span : span);
    }

    internal static Vector3 NormaliseMinusOneOne_(Vector3 v, Vector3 min, Vector3 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * Vector3.one) * 2;
    }

    internal static Vector3 DenormaliseMinusOneOne_(Vector3 v, Vector3 min, Vector3 span) {
      return Denormalise01_(v : v / 2 + .5f * Vector3.one, min : min, span : span);
    }

    internal static Vector4 NormaliseMinusOneOne_(Vector4 v, Vector4 min, Vector4 span) {
      return (Normalise01_(v : v, min : min, span : span) - 0.5f * Vector4.one) * 2;
    }

    internal static Vector4 DenormaliseMinusOneOne_(Vector4 v, Vector4 min, Vector4 span) {
      return Denormalise01_(v : v / 2 + .5f * Vector4.one, min : min, span : span);
    }
  }
}
