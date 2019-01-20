using System;
using UnityEngine;

namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DisplayableVector3S : Displayable {
    public DisplayableVector3S(String displayable_name, Vector3[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}
