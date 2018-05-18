﻿using System;
using droid.Neodroid.Utilities.Structs;

namespace droid.Neodroid.Utilities.Messaging.Messages.Displayables {
  public class DisplayableValuedVector3S : Displayable {
    public DisplayableValuedVector3S(
        String displayable_name,
        Points.ValuePoint[] displayable_value) {
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
