using System;
using Neodroid.Prototyping.Displayers;
using Neodroid.Prototyping.Motors;
using Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace ExampleScenes.MultiArmedBandit {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      DisplayerComponentMenuPath._ComponentMenuPath
      + "MultiArmedBanditMotor"
      + MotorComponentMenuPath._Postfix)]
  public class MultiArmedBanditMotor : Motor {
    [SerializeField] Material[] _indicators;
    [SerializeField] Color _inactive_color;
    [SerializeField] Color _active_color;
    [SerializeField] float[] _values;
    [SerializeField] int _last_index;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "MultiArmedBanditMotor"; } }

    /// <summary>
    /// 
    /// </summary>
    public Single[] Values { get { return this._values; } set { this._values = value; } }

    /// <summary>
    /// 
    /// </summary>
    public Int32 LastIndex { get { return this._last_index; } set { this._last_index = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      var mvs = this.MotionValueSpace;
      mvs._Min_Value = 0;
      mvs._Max_Value = this._indicators.Length - 1;
      this.MotionValueSpace = mvs;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(MotorMotion motion) {
      foreach (var indicator in this._indicators) {
        indicator.color = this._inactive_color;
      }

      this._last_index = (int)motion.Strength;
      this._indicators[this._last_index].color = this._active_color;
    }
  }
}
