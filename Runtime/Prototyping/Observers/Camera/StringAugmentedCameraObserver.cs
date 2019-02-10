using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.Camera {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath
      + "StringAugmentedCamera"
      + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(UnityEngine.Camera))]
  public class StringAugmentedCameraObserver : CameraObserver,
                                               IHasString {
    const string _ColorIdentifier = "Colors";

    string _colors;

    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    protected string serialisedString;

    /// <summary>
    /// </summary>
    public String ObservationValue { get { return this.serialisedString; } }

    protected override void PreSetup()
    {
      base.PreSetup();
      this._colors = this.Identifier + _ColorIdentifier;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this,
          this.Identifier);

      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          this,
          this._colors);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.ParentEnvironment?.UnRegister(this, this.Identifier);
      this.ParentEnvironment?.UnRegister(this, this._colors);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      base.UpdateObservation();
      this.serialisedString = "";
    }
  }
}
