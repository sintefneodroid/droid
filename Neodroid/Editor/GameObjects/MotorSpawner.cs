#if UNITY_EDITOR
using Neodroid.Prototyping.Motors;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.GameObjects {
  public class MotorSpawner : MonoBehaviour {
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Motors/TransformMotor", false, 10)]
    static void CreateTransformMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject("TransformMotor");
      go.AddComponent<EulerTransformMotor1Dof>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Motors/RigidbodyMotor", false, 10)]
    static void CreateRigidbodyMotorGameObject(MenuCommand menu_command) {
      var go = new GameObject("RigidbodyMotor");
      go.AddComponent<RigidbodyMotor1Dof>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
}
#endif
