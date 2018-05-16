using System;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Position" + ConfigurableComponentMenuPath._Postfix)]
  public class PositionConfigurable : ConfigurableGameObject,
                                      IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] bool _use_environments_space;

    /// <summary>
    ///
    /// </summary>
    string _x;

    /// <summary>
    ///
    /// </summary>
    string _y;

    /// <summary>
    ///
    /// </summary>
    string _z;

    protected override void Setup() {
      base.Setup();
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
    }

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "PositionConfigurable"; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 ObservationValue { get { return this._position; } }

    public Utilities.Structs.Space3 TripleSpace { get; }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._x);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._y);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._z);
    }

    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegister(this);
        this.ParentEnvironment.UnRegisterConfigurable(this._x);
        this.ParentEnvironment.UnRegisterConfigurable(this._y);
        this.ParentEnvironment.UnRegisterConfigurable(this._z);
      }
    }

    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space) {
        this._position = this.ParentEnvironment.TransformPosition(this.transform.position);
      } else {
        this._position = this.transform.position;
      }
    }

    public override void ApplyConfiguration(Utilities.Messaging.Messages.Configuration configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition(this.transform.position);
      }

      var v = configuration.ConfigurableValue;
      if (this.TripleSpace._Decimal_Granularity >= 0) {
        v = (int)Math.Round(v, this.TripleSpace._Decimal_Granularity);
      }

      if (this.TripleSpace._Min_Values[0].CompareTo(this.TripleSpace._Max_Values[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, TripleSpace._Min_Values == TripleSpace._Max_Values
        if (v < this.TripleSpace._Min_Values[0] || v > this.TripleSpace._Max_Values[0]) {
          Debug.Log(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.TripleSpace._Min_Values[0],
                  this.TripleSpace._Max_Values[0],
                  v));
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(v - pos.x, pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v - pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v - pos.z);
        }
      } else {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(v, pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v, pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v);
        }
      }

      var inv_pos = pos;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPosition(inv_pos);
      }

      this.transform.position = inv_pos;
    }

    public override Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator) {
      throw new NotImplementedException();
    }
  }
}
