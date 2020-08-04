namespace droid.Editor.Utilities.Commands {
  public static partial class Commands {
    public static void RunPgAgent() {
      AsyncPythonCommand("from neodroidagent.entry_points.agent_tests.torch_agent_tests.pg_test import pg_run;pg_run()");
    }

    public static void RunDqnAgent() {
      AsyncPythonCommand("from neodroidagent.entry_points.agent_tests.torch_agent_tests.dqn_test import dqn_run;dqn_run()");
    }
  }
}
