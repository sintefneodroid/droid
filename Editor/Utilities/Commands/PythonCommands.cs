using System;
using System.Threading;

namespace droid.Editor.Utilities.Commands {
  public static partial class Commands {
    public static void AsyncPythonCommand(string str_command,
                                          bool create_window = true,
                                          bool use_shell = false,
                                          String python_path = "/usr/bin/python") {
      var thread = new Thread(delegate() {
                                PythonCommand(input : str_command,
                                              create_window : create_window,
                                              use_shell : use_shell,
                                              python_path : python_path = python_path);
                              });
      thread.Start();
    }

    public static string PythonCommand(string input,
                                       bool create_window = true,
                                       bool use_shell = false,
                                       String python_path = "/usr/bin/python") {
      input = input.Replace(';', '\n');
      input = $"-c \"{input}\"";

      return SystemCommand(input : python_path,
                           arguments : input,
                           create_window : create_window,
                           use_shell : use_shell);
    }
  }
}
