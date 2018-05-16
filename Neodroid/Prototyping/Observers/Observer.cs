using System;
using System.Collections.Generic;
using Neodroid.Environments;
using Neodroid.Utilities;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public class Observer : PrototypingGameObject,
                          IHasFloatEnumarable {
    /// <summary>
    /// 
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingType { get { return "Observer"; } }

    /// <summary>
    /// 
    /// </summary>
    public Boolean NormaliseObservationUsingSpace {
      get { return this._normalise_observation_using_space; }
      set { this._normalise_observation_using_space = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual IEnumerable<float> FloatEnumerable { get; protected set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this.PreSetup();
      this.FloatEnumerable = new float[] { };
      this.UpdateObservation();
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void PreSetup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegister(this);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateObservation() { }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Reset() { }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    [Header("Normalisation", order = 100)]
    [SerializeField]
    bool _normalise_observation_using_space = true;

    #endregion
  }
}
