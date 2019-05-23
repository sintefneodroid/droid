namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasByteArray {
    /// <summary>
    /// </summary>
    byte[] Bytes { get; }

    /// <summary>
    ///
    /// </summary>
    int[] Shape { get; }

    /// <summary>
    ///
    /// </summary>
    string ArrayEncoding { get; }
  }
}
