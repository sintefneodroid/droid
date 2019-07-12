using System;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IPrototypingEnvironment : IAbstractPrototypingEnvironment,
                                             IHasRegister<IActuator>
                                              {
                                              }
}
