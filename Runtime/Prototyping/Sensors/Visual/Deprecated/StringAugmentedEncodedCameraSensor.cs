using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "StringAugmentedCamera"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class StringAugmentedEncodedCameraSensor : EncodedCameraSensor,
                                                    IHasString {
    const string _color_identifier = "Colors";

    string _colors;

    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    protected string serialised_string;

    /// <summary>
    /// </summary>
    public String ObservationValue { get { return this.serialised_string; } }

    /// <summary>
    /// 
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      this._colors = this.Identifier + _color_identifier;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this, this.Identifier);

      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, this, this._colors);
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
      this.serialised_string = "";
    }
  }
}
