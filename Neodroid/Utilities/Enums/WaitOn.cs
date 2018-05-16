namespace Neodroid.Utilities.Enums {
  public enum WaitOn {
    Never_,

    // Dont wait from reactions from agent
    Update_,

    // Frame
    Fixed_update_
    // Note: unstable physics with the FixedUpdate setting
  }
}
