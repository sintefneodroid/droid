using System;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IPrototypingEnvironment : ISpatialPrototypingEnvironment,
                                             IHasRegister<IActuator> { }
}
