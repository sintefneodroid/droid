using droid.Runtime.Environments.Prototyping;
using droid.Runtime.Managers;
using droid.Runtime.Prototyping.Actors;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "SimulationManager", false, 10)]
    static void CreateSimulationManagerGameObject(MenuCommand menu_command) {
      var go = new GameObject("SimulationManager");
      go.AddComponent<NeodroidManager>();
      GameObjectUtility.SetParentAndAlign(go,
                                          menu_command
                                                  .context as
                                              GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Environment", false, 10)]
    static void CreateEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("Environment");
      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<PrototypingEnvironment>();
      GameObjectUtility.SetParentAndAlign(go,
                                          menu_command
                                                  .context as
                                              GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Actor", false, 10)]
    static void CreateActorGameObject(MenuCommand menu_command) {
      var go = new GameObject("Actor");
      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
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
