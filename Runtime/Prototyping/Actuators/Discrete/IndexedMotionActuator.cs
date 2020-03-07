using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;
using UnityEngine.Events;

namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                               + "IndexedMotion"
                               + ActuatorComponentMenuPath._Postfix)]
  public class IndexedMotionActuator : Actuator {
    [SerializeField] UnityEvent[] _events = { };

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="motion"></param>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var ind = (Int32)motion.Strength;
      if (ind >= this._events.Length) {
        return;
      }

      this._events[ind].Invoke();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void Setup() {
      this.MotionSpace = new Space1 {Min = 0, Max = this._events.Length - 1, DecimalGranularity = 0};
    }

    public override String[] InnerMotionNames { get; }
  }
}
