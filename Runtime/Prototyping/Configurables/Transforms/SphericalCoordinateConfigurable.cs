using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "SphericalCoordinate"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class SphericalCoordinateConfigurable : SpatialConfigurable,
                                                 IHasDouble {
    [Header("Observation", order = 103)]
    [SerializeField]
    Quaternion _euler_rotation = Quaternion.identity;

    [SerializeField] bool _use_environments_space = false;

    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    string _w;

    [SerializeField]
    SampleSpace2 _spherical_space = new SampleSpace2 {
                                                         _space = new Space2(4) {
                                                                                    Min = Vector2.zero,
                                                                                    Max = new Vector2(Mathf.PI * 2f,
                                                                                                      Mathf.PI *2f)
                                                                                }
                                                     };

    [SerializeField] SphericalCoordinates sc;

    /// <summary>
    ///
    /// </summary>
    public Space2 DoubleSpace { get { return (Space2)this._spherical_space.Space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._x = this.Identifier + "Polar_";
      this._y = this.Identifier + "Elevation_";

      this.sc = new SphericalCoordinates(this.transform.position,
                                         3f,
                                         10f,
                                         0f,
                                         Mathf.PI * 2f,
                                         0f,
                                         Mathf.PI *2f);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this);
      this.ParentEnvironment.UnRegister(this, this._x);
      this.ParentEnvironment.UnRegister(this, this._y);
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._spherical_space; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space && this.ParentEnvironment != null) {
        this._euler_rotation = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      } else {
        this._euler_rotation = this.transform.rotation;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      if (simulator_configuration.ConfigurableName == this._x) {
        this.sc.Polar = simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._y) {
        this.sc.Elevation = simulator_configuration.ConfigurableValue;
      }

      this.transform.position = this.sc.ToCartesian;

      this.transform.LookAt(this.ParentEnvironment.CoordinateReferencePoint);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._spherical_space.Sample();

      return new[] {new Configuration(this._x, sample.x), new Configuration(this._y, sample.y)};
    }

    Vector2 IHasDouble.ObservationValue { get { return this._euler_rotation.eulerAngles; } }
  }
}
