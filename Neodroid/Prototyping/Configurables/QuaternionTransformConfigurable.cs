using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "QuaternionTransform"
      + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : ConfigurableGameObject,
                                                 IHasQuaternionTransform {
    [Header("Specfic", order = 102), SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _x; //TODO: Implement applyconfiguration

    [SerializeField] string _y;

    [SerializeField] string _z;

    /// <summary>
    /// 
    /// </summary>
    public Quaternion Rotation { get { return this._rotation; } }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Position { get { return this._position; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "QuaternionTransformConfigurable"; } }
    
    public override Configuration SampleConfiguration(Random random_generator) {
      throw new NotImplementedException();
    }

    public override void ApplyConfiguration(Configuration obj) { throw new NotImplementedException(); }
  }
}
