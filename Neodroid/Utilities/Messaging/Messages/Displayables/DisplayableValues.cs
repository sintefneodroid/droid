using System;

namespace Neodroid.Utilities.Messaging.Messages.Displayables {
  class DisplayableValues : Displayable {
    public DisplayableValues(String displayable_name, double[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public DisplayableValues(String displayable_name, float[] displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public override string ToString() {
      return "<Displayable> " + this.DisplayableName + ", " + this.DisplayableValue + " </Displayable>";
    }

    public override String DisplayableName { get; }
    public override dynamic DisplayableValue { get; }
  }
}
