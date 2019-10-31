using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actors {
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Killable" + ActorComponentMenuPath._Postfix)]
  public class KillableActor : Actor {
    [SerializeField] bool _is_alive = true;

    /// <summary>
    ///
    /// </summary>
    public bool IsAlive { get { return this._is_alive; } }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "KillableActor"; } }

    /// <summary>
    ///
    /// </summary>
    public void Kill() { this._is_alive = false; }

    public override void ApplyMotion(IMotion motion) {
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

    /// <summary>
    ///
    /// </summary>
    public override void PrototypingReset() {
      base.PrototypingReset();

      this._is_alive = true;
    }
  }
}
