using System.Collections.Generic;
using Neodroid.Prototyping.Motors;
using UnityEngine;

namespace Neodroid.Prototyping.Actors {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(ActorComponentMenuPath._ComponentMenuPath + "Killable" + ActorComponentMenuPath._Postfix)]
  public class KillableActor : Actor {
    [SerializeField] bool _is_alive = true;
    public void Kill() { this._is_alive = false; }
    public bool IsAlive { get { return this._is_alive; } }

    public override void ApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
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

    public override string PrototypingType { get { return "KillableActor"; } }

    public override void Reset() {
      base.Reset();

      this._is_alive = true;
    }
  }
}
