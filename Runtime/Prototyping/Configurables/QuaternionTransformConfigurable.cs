using System;
using Neodroid.Runtime.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "QuaternionTransform"
      + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : Configurable,
                                                 IHasQuaternionTransform {
    [Header("Specific", order = 102), SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _x; //TODO: Implement apply configuration

    [SerializeField] string _y;

    [SerializeField] string _z;

    /// <summary>
    /// 
    /// </summary>
    public Quaternion Rotation {
      get { return this._rotation; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "QuaternionTransformConfigurable"; }
    }

    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      throw new NotImplementedException();
    }

    public override void ApplyConfiguration(IConfigurableConfiguration obj) {
      throw new NotImplementedException();
    }
  }
}