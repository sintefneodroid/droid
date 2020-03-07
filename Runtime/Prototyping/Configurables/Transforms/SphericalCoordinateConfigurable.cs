using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "SphericalCoordinate"
                               + ConfigurableComponentMenuPath._Postfix)]
  public class SphericalCoordinateConfigurable : SpatialConfigurable,
                                                 IHasTriple {
    [Header("Observation", order = 103)]
    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    [SerializeField]
    SampleSpace3 _spherical_space = new SampleSpace3 {
                                                         _space = new Space3 {
                                                                                 Min = new Vector3(0,
                                                                                                   y : Mathf
                                                                                                           .PI
                                                                                                       * 0.01f,
                                                                                                   z : 4f),
                                                                                 Max =
                                                                                     new Vector3(x : Mathf.PI
                                                                                                     * 2f,
                                                                                                 y : Mathf.PI
                                                                                                     * 0.5f,
                                                                                                 z : 10f)
                                                                             }
                                                     };

    [SerializeField] SphericalSpace sc;

    /// <summary>
    ///
    /// </summary>
    public Space3 TripleSpace { get { return (Space3)this._spherical_space.Space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "Polar_";
      this._y = this.Identifier + "Elevation_";
      this._z = this.Identifier + "Radius_";

      var reference_point = this.transform.position;
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_ && this.ParentEnvironment) {
        reference_point = this.ParentEnvironment.TransformPoint(point : reference_point);
      }

      this.sc = SphericalSpace.FromCartesian(cartesian_coordinate : reference_point,
                                             min_radius : this._spherical_space.Space.Min.z,
                                             max_radius : this._spherical_space.Space.Max.z,
                                             min_polar : this._spherical_space.Space.Min.x,
                                             max_polar : this._spherical_space.Space.Max.x,
                                             min_elevation : this._spherical_space.Space.Min.y,
                                             max_elevation : this._spherical_space.Space.Max.y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, c : (Configurable)this);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(configurable : this);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
    }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._spherical_space; } }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      //TODO: IMPLEMENT LOCAL SPACE
      if (simulator_configuration.ConfigurableName == this._x) {
        this.sc.Polar = simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._y) {
        this.sc.Elevation = simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._z) {
        this.sc.Radius = simulator_configuration.ConfigurableValue;
      }

      var reference_point = this.sc.ToCartesian();

      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        reference_point = this.ParentEnvironment.InverseTransformPoint(point : reference_point);
      }

      this.transform.position = reference_point;

      this.transform.LookAt(target : this.ParentEnvironment.CoordinateReferencePoint);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._spherical_space.Sample();

      return new[] {
                       new Configuration(configurable_name : this._x, configurable_value : sample.x),
                       new Configuration(configurable_name : this._y, configurable_value : sample.y),
                       new Configuration(configurable_name : this._z, configurable_value : sample.z)
                   };
    }

    Vector3 IHasTriple.ObservationValue { get { return this.sc.ToVector3; } }
  }
}
