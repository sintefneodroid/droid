using System;
using Neodroid.Prototyping.Actors;
using Neodroid.Utilities;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [ExecuteInEditMode]
  [Serializable]
  public class Motor : PrototypingGameObject {
    public Actor ParentActor { get { return this._actor; } set { this._actor = value; } }

    public float EnergySpendSinceReset {
      get { return this._energy_spend_since_reset; }
      set { this._energy_spend_since_reset = value; }
    }

    public float EnergyCost { get { return this._energy_cost; } set { this._energy_cost = value; } }

    public ValueSpace MotionValueSpace {
      get { return this._motion_value_space; }
      set { this._motion_value_space = value; }
    }

    public override String PrototypingType { get { return "Motor"; } }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentActor = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentActor, this);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentActor) {
        this.ParentActor.UnRegister(this);
      }
    }

    public void ApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + motion + " To " + this.name);
      }
      #endif

      if (motion.Strength < this.MotionValueSpace._Min_Value
          || motion.Strength > this.MotionValueSpace._Max_Value) {
        Debug.Log(
            $"It does not accept input {motion.Strength}, outside allowed range {this.MotionValueSpace._Min_Value} to {this.MotionValueSpace._Max_Value}");
        return; // Do nothing
      }

      this.InnerApplyMotion(motion);
      this.EnergySpendSinceReset += Mathf.Abs(this.EnergyCost * motion.Strength);
    }

    protected virtual void InnerApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) { }

    public virtual float GetEnergySpend() { return this._energy_spend_since_reset; }

    public override string ToString() { return this.Identifier; }

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
