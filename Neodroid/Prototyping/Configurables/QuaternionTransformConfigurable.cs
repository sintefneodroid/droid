using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "QuaternionTransform"
      + ConfigurableComponentMenuPath._Postfix)]
  public class QuaternionTransformConfigurable : ConfigurableGameObject,
                                                 IHasQuaternionTransform {
    [Header("Specfic", order = 102)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [SerializeField] string _x; //TODO: Implement applyconfiguration

    [SerializeField] string _y;

    [SerializeField] string _z;

    public Quaternion Rotation { get { return this._rotation; } }

    public Vector3 Position { get { return this._position; } }

    public override Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator) {
      throw new System.NotImplementedException();
    }
  }
}
