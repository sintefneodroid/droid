#if UNITY_EDITOR
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Prototyping.Actors;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.GameObjects {
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "SimulationManager", false, 10)]
    static void CreateSimulationManagerGameObject(MenuCommand menu_command) {
      var go = new GameObject("SimulationManager");
      go.AddComponent<PausableManager>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Environment", false, 10)]
    static void CreateEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("Environment");
      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<PrototypingEnvironment>();
      GameObjectUtility.SetParentAndAlign(
          go,
          menu_command
              .context as GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Actor", false, 10)]
    static void CreateActorGameObject(MenuCommand menu_command) {
      var go = new GameObject("Actor");
      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
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
