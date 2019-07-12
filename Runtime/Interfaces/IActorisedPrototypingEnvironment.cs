using System;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IActorisedPrototypingEnvironment : IAbstractPrototypingEnvironment
      , IHasRegister<IActor> { }
}
