using System;
using droid.Neodroid.Prototyping.Actors;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Messaging.Messages;
using droid.Neodroid.Utilities.Structs;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Motors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public abstract class Motor : PrototypingGameObject {
    /// <summary>
    /// 
    /// </summary>
    public Actor ParentActor { get { return this._actor; } set { this._actor = value; } }

    /// <summary>
    /// 
    /// </summary>
    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    /// <summary>
    /// 
    /// </summary>
    public ValueSpace MotionValueSpace {
      get { return this._motion_value_space; }
      set { this._motion_value_space = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public override String PrototypingTypeName { get { return "Motor"; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this.ParentActor =
          NeodroidUtilities.MaybeRegisterComponent(this.ParentActor, this, only_parents : true);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentActor) {
        this.ParentActor.UnRegister(this);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    public void ApplyMotion(MotorMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + motion + " To " + this.name);
      }
      #endif

      if (motion.Strength < this.MotionValueSpace._Min_Value
          || motion.Strength > this.MotionValueSpace._Max_Value) {
        Debug.LogWarning(
            $"It does not accept input {motion.Strength}, outside the allowed range from {this.MotionValueSpace._Min_Value} to {this.MotionValueSpace._Max_Value}");
        return; // Do nothing
      }

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    protected virtual void InnerApplyMotion(MotorMotion motion) { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual float GetEnergySpend() { return this._energy_spend_since_reset; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.Identifier; }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Reset() { this._energy_spend_since_reset = 0; }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    Actor _actor;

    [Header("General", order = 101)]
    [SerializeField]
    ValueSpace _motion_value_space =
        new ValueSpace {_Decimal_Granularity = 0, _Min_Value = -10, _Max_Value = 10};

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion
  }
}
