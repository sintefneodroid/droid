using droid.Runtime.Prototyping.Actuators;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ActuatorSpawner : MonoBehaviour {
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Actuators/TransformActuator", false, 10)]
    static void CreateTransformActuatorGameObject(MenuCommand menu_command) {
      var go = new GameObject("TransformActuator");
      go.AddComponent<EulerTransform1DofActuator>();
      GameObjectUtility.SetParentAndAlign(go,
                                          menu_command
                                                  .context as
                                              GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Actuators/RigidbodyActuator", false, 10)]
    static void CreateRigidbodyActuatorGameObject(MenuCommand menu_command) {
      var go = new GameObject("RigidbodyActuator");
      go.AddComponent<Rigidbody1DofActuator>();
      GameObjectUtility.SetParentAndAlign(go,
                                          menu_command
                                                  .context as
                                              GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
}
#endif
