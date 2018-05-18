using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Configurables {
  /// <summary>
  /// 
  /// </summary>
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public abstract void ApplyConfiguration(Configuration obj);
  }
}
