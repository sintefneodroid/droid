using System;

namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DisplayableValues : Displayable {
    public DisplayableValues(String displayable_name, double[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public DisplayableValues(String displayable_name, float[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}
