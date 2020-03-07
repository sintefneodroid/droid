using System;
using System.Diagnostics;
using System.Threading;

namespace droid.Editor.Utilities.Commands {
  public static partial class Commands {
    public static void AsyncSystemCommand(string str_command,
                                          string arguments,
                                          bool create_window = true,
                                          bool use_shell = false) {
      var thread = new Thread(delegate() {
                                SystemCommand(input : str_command,
                                              arguments : arguments,
                                              create_window : create_window,
                                              use_shell : use_shell);
                              });
      thread.Start();

    }

    public static string SystemCommand(string input,
                                       string arguments,
                                       bool create_window = true,
                                       bool use_shell = false,
                                       string working_directory = "") {
      /*
       * The WorkingDirectory property behaves differently when UseShellExecute is true than when UseShellExecute is false. When UseShellExecute is true, the WorkingDirectory property specifies the location of the executable. If WorkingDirectory is an empty string, the current directory is understood to contain the executable.
       * When UseShellExecute is true, the working directory of the application that starts the executable is also the working directory of the executable.
       * When UseShellExecute is false, the WorkingDirectory property is not used to find the executable. Instead, its value applies to the process that is started and only has meaning within the context of the new process.
       */

      var process_info = new ProcessStartInfo(fileName : input, arguments : arguments) {
                                                                                           CreateNoWindow =
                                                                                               !create_window,
                                                                                           RedirectStandardOutput
                                                                                               = !use_shell,
                                                                                           RedirectStandardError
                                                                                               = !use_shell,
                                                                                           UseShellExecute =
                                                                                               use_shell,
                                                                                           WorkingDirectory = working_directory
                                                                                       };

      var process = new Process {StartInfo = process_info};
      //var process = Process.Start(startInfo : process_info);

      var out_str = String.Empty;
      try {
        {
          // This code assumes the process you are starting will terminate itself.
          // Given that is is started without a window so you cannot terminate it
          // on the desktop, it must terminate itself or you can do it programmatically
          // from this application using the Kill method.
        }

        process.Start();

        if (!use_shell) {
          var std_str = process.StandardOutput.ReadToEnd();
          var error_str = process.StandardError.ReadToEnd();

          out_str = std_str + '\n' + error_str;

          #if UNITY_EDITOR
          UnityEngine.Debug.Log(std_str);

          if (error_str.Length > 0) {
            UnityEngine.Debug.LogError(message : error_str);
          }
          #else
          Console.WriteLine(value : std_str);

          if(error_str.Length>0) {
            UnityEngine.Debug.LogError(message: error_str );
          }
          Console.WriteLine(value : error_str);
          #endif
        }

        process.WaitForExit();
        process.Close();
      } catch (Exception e) {
        #if UNITY_EDITOR
        UnityEngine.Debug.LogError(message : e.Message);
        #else
        Console.WriteLine(value : e.Message);
        #endif
      }

      return out_str;
    }
  }
}
