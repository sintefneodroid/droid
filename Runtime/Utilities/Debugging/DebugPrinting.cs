using System;
using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Utilities.Debugging {
  public static class DebugPrinting {
    public  static void ApplyPrint(bool debugging,IConfigurableConfiguration configuration, string identifier) {
      if (debugging) {
        Debug.Log("Applying " + configuration + " To " + identifier);
      }
    }
  }
}
