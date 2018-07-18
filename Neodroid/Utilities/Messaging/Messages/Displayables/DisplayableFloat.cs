using System;

namespace Neodroid.Utilities.Messaging.Messages.Displayables {
  /// <summary>
  /// 
  /// </summary>
  class DisplayableFloat : Displayable {
    public DisplayableFloat(String displayable_name, double displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    public DisplayableFloat(String displayable_name, Double? displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value.GetValueOrDefault();
    }

    public DisplayableFloat(String displayable_name, float displayable_value) {
      this.DisplayableName = displayable_name;
      this.DisplayableValue = displayable_value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<Displayable> " + this.DisplayableName + ", " + this.DisplayableValue + " </Displayable>";
    }

    /// <summary>
    /// 
    /// </summary>
    public override String DisplayableName { get; }

    /// <summary>
    /// 
    /// </summary>
    public override dynamic DisplayableValue { get; }
  }
}
