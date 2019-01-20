using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Utilities.Debugging {
  public static class DebugPrinting {
    public static void ApplyPrint(
        bool debugging,
        IConfigurableConfiguration configuration,
        string identifier) {
      if (debugging) {
        Debug.Log("Applying " + configuration + " To " + identifier);
      }
    }

    public static void DisplayPrint(dynamic value, string identifier, bool debugging) {
      if (debugging) {
        Debug.Log("Applying " + value + " To " + identifier);
      }
    }
  }
}
