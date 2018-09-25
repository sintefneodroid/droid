namespace Neodroid.Runtime.Utilities.Unsorted {
  public interface IMotionTracker {
    bool IsInMotion();

    bool IsInMotion(float sensitivity);
  }
}