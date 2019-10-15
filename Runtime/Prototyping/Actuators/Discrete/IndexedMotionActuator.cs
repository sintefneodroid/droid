using System;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace droid.Runtime.Prototyping.Actuators.Discrete {
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "IndexedMotion"
                    + ActuatorComponentMenuPath._Postfix)]
  public class IndexedMotionActuator : Actuator {



    [SerializeField] UnityEvent[] _events;

    /// <summary>
    ///
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var ind = (Int32)motion.Strength;
      if (ind >= this._events.Length) {
        return;
      }

      this._events[ind].Invoke();
    }

    /// <summary>
    ///
    /// </summary>
    public override void Setup() {
      this.MotionSpace = new Space1 {
                                        _min_ = 0,
                                        _max_ = this._events.Length,
                                        DecimalGranularity = 0,
                                        NormalisedBool = false
                                    };

    }

    public override String[] InnerMotionNames { get; }
  }
}
