namespace droid.Editor.Utilities.Commands {
  public static partial class Commands {

    public static void BashCommand(string arguments,
                                   bool create_window = true,
                                   bool use_shell = false) {
      #if UNITY_EDITOR_LINUX
      const string file_name = "/bin/bash";
      #else
      const string file_name = "cmd.exe";
      #endif

      SystemCommand(input : file_name,arguments : arguments,create_window : create_window,use_shell : use_shell);
    }

  }
}
