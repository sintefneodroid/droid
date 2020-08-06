using System;

namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DisplayableString : Displayable {
    public DisplayableString(string displayable_name, string displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}
