using System;

namespace Neodroid.Utilities.Messaging.Messages.Displayables {
  /// <summary>
  /// 
  /// </summary>
  public abstract class Displayable {
    String _displayable_name;
    dynamic _displayable_value;

    /// <summary>
    /// 
    /// </summary>
    public virtual string DisplayableName {
      get { return this._displayable_name; }
      set { this._displayable_name = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual dynamic DisplayableValue {
      get { return this._displayable_value; }
      set { this._displayable_value = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return $"<Displayable> {this.DisplayableName}, {this.DisplayableValue} </Displayable>";
    }
  }
}
