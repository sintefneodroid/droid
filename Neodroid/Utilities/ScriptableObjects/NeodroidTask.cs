using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "NeodroidTask",
      menuName = "Neodroid/ScriptableObjects/NeodroidTask",
      order = 1)]
  public class NeodroidTask : ScriptableObject {
    public Vector3 _Position;
    public float _Radius;
  }
}
