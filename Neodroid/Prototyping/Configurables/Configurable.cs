using droid.Neodroid.Utilities.GameObjects;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Configurables {
  [ExecuteInEditMode]
  public abstract class Configurable : PrototypingGameObject {
    public abstract void ApplyConfiguration(Utilities.Messaging.Messages.Configuration obj);
  }
}
