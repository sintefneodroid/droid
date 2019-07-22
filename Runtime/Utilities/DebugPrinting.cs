using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// 
  /// </summary>
  public static class DebugPrinting {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="debugging"></param>
    /// <param name="configuration"></param>
    /// <param name="identifier"></param>
    public static void ApplyPrint(bool debugging,
                                  IConfigurableConfiguration configuration,
                                  string identifier) {
      if (debugging) {
        Debug.Log("Applying " + configuration + " To " + identifier);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="identifier"></param>
    /// <param name="debugging"></param>
    public static void DisplayPrint(dynamic value, string identifier, bool debugging) {
      if (debugging) {
        Debug.Log("Applying " + value + " To " + identifier);
      }
    }
  }
}
