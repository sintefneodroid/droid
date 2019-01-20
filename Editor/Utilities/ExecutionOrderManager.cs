#if UNITY_EDITOR
using UnityEditor;

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [InitializeOnLoad]
  public class ExecutionOrderManager : UnityEditor.Editor {
    static ExecutionOrderManager() {
      foreach (var mono_script in MonoImporter.GetAllRuntimeMonoScripts()) {
        var type = mono_script.GetClass();
        if (type == null) {
          continue;
        }

        var attributes = type.GetCustomAttributes(typeof(ScriptExecutionOrderAttribute), true);

        if (attributes.Length == 0) {
          continue;
        }

        var attribute = (ScriptExecutionOrderAttribute)attributes[0];
        if (MonoImporter.GetExecutionOrder(mono_script) != attribute.GetOrder()) {
          MonoImporter.SetExecutionOrder(mono_script, attribute.GetOrder());
        }
      }
    }
  }
}
#endif
