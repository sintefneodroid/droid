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
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
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
                                                                                                      Mathf.PI
                                                                                                      * 0.01f,
                                                                                                      4f),
                                                                                    Max = new Vector3(Mathf.PI
                                                                                                      * 2f,
                                                                                                      Mathf.PI
                                                                                                      * 0.5f,
                                                                                                      10f)
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
      if (this.coordinate_space == CoordinateSpace.Environment_) {
        reference_point = this.ParentEnvironment.TransformPoint(reference_point);
      }

      this.sc = SphericalSpace.FromCartesian(reference_point,
                                             this._spherical_space.Space.Min.z,
                                             this._spherical_space.Space.Max.z,
                                             this._spherical_space.Space.Min.x,
                                             this._spherical_space.Space.Max.x,
                                             this._spherical_space.Space.Min.y,
                                             this._spherical_space.Space.Max.y);
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
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._z);
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
      this.ParentEnvironment.UnRegister(this, this._z);
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._spherical_space; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) { //TODO: IMPLEMENT LOCAL SPACE
      if (simulator_configuration.ConfigurableName == this._x) {
        this.sc.Polar = simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._y) {
        this.sc.Elevation = simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._z) {
        this.sc.Radius = simulator_configuration.ConfigurableValue;
      }

      var reference_point = this.sc.ToCartesian();

      if (this.coordinate_space == CoordinateSpace.Environment_) {
        reference_point = this.ParentEnvironment.InverseTransformPoint(reference_point);
      }

      this.transform.position = reference_point;

      this.transform.LookAt(this.ParentEnvironment.CoordinateReferencePoint);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._spherical_space.Sample();

      return new[] {
                       new Configuration(this._x, sample.x),
                       new Configuration(this._y, sample.y),
                       new Configuration(this._z, sample.z)
                   };
    }

    Vector3 IHasTriple.ObservationValue { get { return this.sc.ToVector3; } }
  }
}
