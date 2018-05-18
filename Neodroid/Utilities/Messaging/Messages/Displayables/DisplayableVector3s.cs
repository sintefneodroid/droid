﻿using System;
using UnityEngine;

namespace droid.Neodroid.Utilities.Messaging.Messages.Displayables {
  public class DisplayableVector3S : Displayable {
    public DisplayableVector3S(String displayable_name, Vector3[] displayable_value) {
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