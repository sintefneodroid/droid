using System;
using System.Collections.Generic;
using System.Linq;
using droid.Runtime.Environments.Prototyping;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  [Serializable]
  public abstract class Sensor : PrototypingGameObject,
                                 ISensor {
    /// <summary>
    /// </summary>
    public AbstractPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract IEnumerable<float> FloatEnumerable { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void UpdateObservation();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, c : this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(sensor : this); }

    /// <summary>
    /// </summary>
    protected virtual void Update() {
      if (Application.isPlaying) {
        if (this.FloatEnumerable == null) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            Debug.LogWarning(message :
                             $"FloatEnumerable of {this.Identifier} is empty! Maybe you forget an assignment to it when updating observations");
          }
          #endif
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var any = false;
      if (this.FloatEnumerable != null) {
        foreach (var f in this.FloatEnumerable) {
          any = true;
          break;
        }
      }

      return any ? string.Join(",", values : this.FloatEnumerable) : "Empty FloatEnumerable";
    }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    AbstractPrototypingEnvironment _environment;

    #endregion
  }
}
