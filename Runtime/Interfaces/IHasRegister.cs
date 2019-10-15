namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IRegisterable {
    /// <summary>
    /// </summary>
    string Identifier { get; }

    /// <summary>
    ///
    /// </summary>
    void Tick();

    /// <summary>
    /// </summary>
    void PrototypingReset();

    /// <summary>
    /// </summary>
    void PreSetup();

    /// <summary>
    /// </summary>
    void Setup();

    /// <summary>
    ///
    /// </summary>
    void RemotePostSetup();
  }

  /// <summary>
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IHasRegister<in T> where T : IRegisterable {
    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    void Register(T obj);

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    void Register(T obj, string identifier);

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    void UnRegister(T obj);

    /// <summary>
    ///
    /// </summary>
    /// <param name="t"></param>
    /// <param name="obj"></param>
    void UnRegister(T t, string obj);


  }
}
