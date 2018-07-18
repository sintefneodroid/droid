namespace Neodroid.Utilities.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    string Identifier { get; }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IHasRegister<in T> where T : IRegisterable {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    void Register(T obj);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="identifier"></param>
    void Register(T obj, string identifier);
  }
}
