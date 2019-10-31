using System;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public abstract class Actuator : PrototypingGameObject,
                                   IActuator {
    /// <summary>
    /// </summary>
    public IHasRegister<Actuator> Parent { get { return this._parent; } set { this._parent = value; } }


    /// <summary>
    /// </summary>
    public override String PrototypingTypeName { get { return "Actuator"; } }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    public void ApplyMotion(IMotion motion) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + motion + " To " + this.name);
      }
      #endif

      motion.Strength = this._motion_value_space._space.Reproject(motion.Strength);

      this.InnerApplyMotion(motion);
    }

    public Space1 MotionSpace {
      get { return this._motion_value_space._space; }
      set { this._motion_value_space.Space = value; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual float Sample() { return this._motion_value_space.Sample(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._overriden = false;
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, true);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this._overriden) {
        throw new NotImplementedException();
      }

      this._overriden = true;
      this.Parent?.UnRegister(this);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected abstract void InnerApplyMotion(IMotion motion);

    public abstract string[] InnerMotionNames { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.Identifier; }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    IHasRegister<Actuator> _parent;

    [Header("General", order = 101)]
    [SerializeField]
    SampleSpace1 _motion_value_space = new SampleSpace1 {_space = Space1.DiscreteMinusOneOne};


    bool _overriden = false;

    #endregion
  }
}
