using UnityEngine;

namespace droid.Neodroid.Utilities.Interfaces {
  public interface IHasQuaternionTransform {
    Vector3 Position { get; }

    Quaternion Rotation { get; }
  }
}
