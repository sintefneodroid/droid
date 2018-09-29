using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Actors {
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Killable" + ActorComponentMenuPath._Postfix)]
  public class KillableActor : Actor {
    [SerializeField] bool _is_alive = true;
    public bool IsAlive { get { return this._is_alive; } }

    public override string PrototypingTypeName { get { return "KillableActor"; } }
    public void Kill() { this._is_alive = false; }

    public override void ApplyMotion(IMotorMotion motion) {
      if (this._is_alive) {
        base.ApplyMotion(motion);
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          Debug.Log("Actor is dead, cannot apply motion");
        }
        #endif
      }
    }

    public override void EnvironmentReset() {
      base.EnvironmentReset();

      this._is_alive = true;
    }
  }
}
