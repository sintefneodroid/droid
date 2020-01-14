namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IPrototypingEnvironment : IAbstractPrototypingEnvironment,
                                             IHasRegister<IActuator> { }
}
