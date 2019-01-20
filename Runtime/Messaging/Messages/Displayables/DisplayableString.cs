using System;

namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class DisplayableString : Displayable {
    public DisplayableString(String displayable_name, String displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }
  }
}
