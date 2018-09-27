namespace Neodroid.Runtime.Utilities.Misc.Orientation {
  public interface IMotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}
