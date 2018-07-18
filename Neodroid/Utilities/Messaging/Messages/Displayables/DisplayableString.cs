using System;

namespace Neodroid.Utilities.Messaging.Messages.Displayables {
  public class DisplayableString : Displayable {
    public DisplayableString(String displayable_name, String displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public override String DisplayableName { get; }
    public override dynamic DisplayableValue { get; }

    public override string ToString() {
      return "<Displayable> " + this.DisplayableName + ", " + this.DisplayableValue + " </Displayable>";
    }
  }
}
