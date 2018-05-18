using System;
using System.Collections.Generic;
using System.Linq;
using droid.Neodroid.Environments;
using droid.Neodroid.Utilities.GameObjects;
using droid.Neodroid.Utilities.Interfaces;
using droid.Neodroid.Utilities.Unsorted;
using UnityEngine;
using UnityEngine.Playables;

namespace droid.Neodroid.Prototyping.Observers {
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
    public bool NormaliseObservation {
      get { return this._normaliseObservation; }
      set { this._normaliseObservation = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public virtual IEnumerable<float> FloatEnumerable { get; protected set; }

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
    public virtual void UpdateObservation() { }

    void Update() {
      if(Application.isPlaying) {
        if (this.FloatEnumerable == null || !this.FloatEnumerable.Any()) {
          Debug.LogWarning(
              $"FloatEnumerable of {this.Identifier} is empty! Maybe you forget an assignment to it when updating observations");
        }
      }
    }

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
    bool _normaliseObservation;

    #endregion
  }
}
