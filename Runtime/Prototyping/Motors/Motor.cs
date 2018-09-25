using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Prototyping.Actors;
using Neodroid.Runtime.Utilities.GameObjects;
using Neodroid.Runtime.Utilities.Structs;
using Neodroid.Runtime.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Motors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode, Serializable]
  public abstract class Motor : PrototypingGameObject,
                                //IResetable,
                                IMotor {
    /// <summary>
    /// 
    /// </summary>
    public IActor ParentActor {
      get { return this._actor; }
      set { this._actor = value; }
    }

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
    public float EnergyCost {
      get { return this._energy_cost; }
      set { this._energy_cost = value; }
    }

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
    public override String PrototypingTypeName {
      get { return "Motor"; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void RegisterComponent() {
      this.ParentActor = NeodroidUtilities.MaybeRegisterComponent(
          (Actor)this.ParentActor,
          this,
          only_parents : true);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentActor != null) {
        this.ParentActor.UnRegister(this);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    public void ApplyMotion(IMotorMotion motion) {
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

      motion.Strength = this._motion_value_space.Round(motion.Strength);

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    protected abstract void InnerApplyMotion(IMotorMotion motion);

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
    public void EnvironmentReset() { this._energy_spend_since_reset = 0; }

    #region Fields

    [Header("References", order = 99), SerializeField]
    IActor _actor;

    [Header("General", order = 101), SerializeField]
    ValueSpace _motion_value_space =
        new ValueSpace {_Decimal_Granularity = 0, _Min_Value = -1, _Max_Value = 1};

    [SerializeField] float _energy_spend_since_reset;

    [SerializeField] float _energy_cost;

    #endregion

    public virtual float Sample() { return this.MotionValueSpace.Sample(); }
  }
}