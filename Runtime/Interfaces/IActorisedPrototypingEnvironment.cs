using System;
using UnityEngine;

namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IActorisedPrototypingEnvironment : IAbstractPrototypingEnvironment,
                                                      IHasRegister<IActor> { }
}
