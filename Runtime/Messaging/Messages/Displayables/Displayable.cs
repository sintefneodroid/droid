namespace droid.Runtime.Messaging.Messages.Displayables {
  /// <summary>
  /// </summary>
  public abstract class Displayable {
    /// <summary>
    /// </summary>
    public virtual string DisplayableName { get; set; }

    /// <summary>
    /// </summary>
    public virtual dynamic DisplayableValue { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return $"<Displayable> {this.DisplayableName}, {this.DisplayableValue} </Displayable>";
    }
  }
}
