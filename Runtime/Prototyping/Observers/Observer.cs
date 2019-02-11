using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.GameObjects;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public abstract class Observer : PrototypingGameObject,
                                   IObserver {
    /// <summary>
    /// </summary>
    public IPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// </summary>
    public bool NormaliseObservation {
      get { return this._normalise_observation; }
      set { this._normalise_observation = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual IEnumerable<float> FloatEnumerable { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void UpdateObservation();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void EnvironmentReset() { }

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
    /// </summary>
    protected virtual void PreSetup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(this); }

    /// <summary>
    /// </summary>
    protected virtual void Update() {
      if (Application.isPlaying) {
        if (this.FloatEnumerable == null || !this.FloatEnumerable.Any()) {
          if (this.Debugging) {
            Debug.LogWarning(
                $"FloatEnumerable of {this.Identifier} is empty! Maybe you forget an assignment to it when updating observations");
          }
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return this.FloatEnumerable.Any() ? string.Join(",", this.FloatEnumerable) : "Empty FloatEnumerable";
    }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    IPrototypingEnvironment _environment;

    [Header("Normalisation", order = 100)]
    [SerializeField]
    bool _normalise_observation;

    #endregion
  }
}
