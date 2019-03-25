using System;
using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Position"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class PositionConfigurable : Configurable,
                                      IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position = Vector3.zero;

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

    [SerializeField] Space3 _triple_space = Space3.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 ObservationValue { get { return this._position; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._triple_space; } }

    /// <summary>
    /// 
    /// </summary>
    protected override void PreSetup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._x);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._y);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent((PrototypingEnvironment)this.ParentEnvironment,
                                              (Configurable)this,
                                              this._z);
    }

    /// <summary>
    /// 
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
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space) {
        this._position = this.ParentEnvironment.TransformPoint(this.transform.position);
      } else {
        this._position = this.transform.position;
      }
    }

    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPoint(this.transform.position);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.TripleSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(v, this.TripleSpace.DecimalGranularity);
      }

      if (this.TripleSpace._Min_Values[0].CompareTo(this.TripleSpace._Max_Values[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, TripleSpace._Min_Values == TripleSpace._Max_Values
        if (v < this.TripleSpace._Min_Values[0] || v > this.TripleSpace._Max_Values[0]) {
          Debug.Log($"Configurable does not accept input{v}, outside allowed range {this.TripleSpace._Min_Values[0]} to {this.TripleSpace._Max_Values[0]}");
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v - pos.x, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v - pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v - pos.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v);
        }
      }

      var inv_pos = pos;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(inv_pos);
      }

      this.transform.position = inv_pos;
    }

    public override IConfigurableConfiguration[] SampleConfigurations() {
      var sample = this.TripleSpace.Sample();
      var r = Random.Range(0, 3);
      switch (r) {
        case 0:
          return new[] {new Configuration(this._x, sample.x)};

        case 1:
          return new[] {new Configuration(this._y, sample.y)};

        case 2:
          return new[] {new Configuration(this._z, sample.z)};

        default:
          throw new IndexOutOfRangeException();
      }
    }
  }
}
