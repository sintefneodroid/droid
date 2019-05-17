using UnityEngine.Experimental.Rendering;

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
    GraphicsFormat DataType { get; }
  }
}
