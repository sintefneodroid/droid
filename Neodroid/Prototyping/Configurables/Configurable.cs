using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Configurables {
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject {
    public abstract void ApplyConfiguration(Configuration obj);
  }
}
