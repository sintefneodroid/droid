namespace droid.Runtime.Utilities.Orientation {
  public interface IMotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}
