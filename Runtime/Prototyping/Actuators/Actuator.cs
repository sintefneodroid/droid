using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public abstract class Actuator : PrototypingGameObject,
                                   //IResetable,
                                   IActuator {
    /// <summary>
    /// </summary>
    public IActor ParentActor { get { return this._actor; } set { this._actor = value; } }

    /// <summary>
    /// </summary>
    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    /// <summary>
    /// </summary>
    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "Actuator"; } }

    /// <summary>
    /// </summary>
    public Space1 MotionSpace1 {
      get { return this._motion_value_space; }
      set { this._motion_value_space = value; }
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public void ApplyMotion(IMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + motion + " To " + this.name);
      }
      #endif

      if (motion.Strength < this.MotionSpace1._Min_Value || motion.Strength > this.MotionSpace1._Max_Value) {
        Debug.LogWarning($"It does not accept input {motion.Strength}, outside the allowed range from {this.MotionSpace1._Min_Value} to {this.MotionSpace1._Max_Value}");
        return; // Do nothing
      }

      motion.Strength = this._motion_value_space.Round(motion.Strength);

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float GetEnergySpend() { return this._energy_spend_since_reset; }

    /// <summary>
    /// </summary>
    public void EnvironmentReset() { this._energy_spend_since_reset = 0; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual float Sample() { return this.MotionSpace1.Sample(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, this, true);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentActor?.UnRegister(this); }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected abstract void InnerApplyMotion(IMotion motion);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.Identifier; }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    IActor _actor;

    [Header("General", order = 101)]
    [SerializeField]
    Space1 _motion_value_space = new Space1 {_Decimal_Granularity = 0, _Min_Value = -1, _Max_Value = 1};

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion
  }
}
