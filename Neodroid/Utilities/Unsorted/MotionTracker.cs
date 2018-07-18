namespace Neodroid.Utilities.Unsorted {
  public interface IMotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}
