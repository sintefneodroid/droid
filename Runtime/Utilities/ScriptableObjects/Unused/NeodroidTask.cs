using UnityEngine;

namespace droid.Runtime.Utilities.ScriptableObjects.Unused {
  [CreateAssetMenu(fileName = "NeodroidTask",
      menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath+"NeodroidTask",
      order = 1)]
  public class NeodroidTask : ScriptableObject {
    public Vector3 _Position;
    public float _Radius;
  }
}
