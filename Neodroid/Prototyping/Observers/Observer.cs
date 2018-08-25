using System;
using System.Collections.Generic;
using System.Linq;
using Neodroid.Environments;
using Neodroid.Prototyping.Internals;
using Neodroid.Utilities.GameObjects;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode, Serializable]
  public abstract class Observer : PrototypingGameObject,
                                   IHasFloatEnumarable,
                                   IResetable {
    /// <summary>
    /// 
    /// </summary>
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool NormaliseObservation {
      get { return this._normalise_observation; }
      set { this._normalise_observation = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual IEnumerable<float> FloatEnumerable { get; protected set; }

    //protected abstract void UpdateFloatEnumerable(IEnumerable<float> vals);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected sealed override void Setup() {
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
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
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
    public abstract void UpdateObservation();

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Update() {
      if (Application.isPlaying) {
        if (this.FloatEnumerable == null || !this.FloatEnumerable.Any()) {
          Debug.LogWarning(
              $"FloatEnumerable of {this.Identifier} is empty! Maybe you forget an assignment to it when updating observations");
        }
      }
    }

    #region Fields

    [Header("References", order = 99), SerializeField]
    PrototypingEnvironment _environment;

    [Header("Normalisation", order = 100), SerializeField]
    bool _normalise_observation;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void EnvironmentReset() { }
  }
}
