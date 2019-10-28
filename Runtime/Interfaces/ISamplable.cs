namespace droid.Runtime.Interfaces {
  /// <summary>
  /// 
  /// </summary>
  public interface ISamplable {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    dynamic Sample();

    /// <summary>
    ///
    /// </summary>
    ISpace Space { get; }
  }
}
