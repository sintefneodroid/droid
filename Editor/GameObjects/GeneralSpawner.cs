using droid.Runtime.Environments.Prototyping;
using droid.Runtime.Managers;
using droid.Runtime.Prototyping.Actors;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  public class GeneralSpawner : MonoBehaviour {
    [MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "SimulationManager", false, 10)]
    static void CreateSimulationManagerGameObject(MenuCommand menu_command) {
      var go = new GameObject("SimulationManager");
      go.AddComponent<NeodroidManager>();
      GameObjectUtility.SetParentAndAlign(child : go,
                                          parent : menu_command
                                                           .context as
                                                       GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Environment", false, 10)]
    static void CreateEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("Environment");
      var plane = GameObject.CreatePrimitive(type : PrimitiveType.Plane);
      plane.transform.parent = go.transform;
      go.AddComponent<PrototypingEnvironment>();
      GameObjectUtility.SetParentAndAlign(child : go,
                                          parent : menu_command
                                                           .context as
                                                       GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Actor", false, 10)]
    static void CreateActorGameObject(MenuCommand menu_command) {
      var go = new GameObject("Actor");
      var capsule = GameObject.CreatePrimitive(type : PrimitiveType.Capsule);
      capsule.transform.parent = go.transform;
      go.AddComponent<Actor>();
      GameObjectUtility.SetParentAndAlign(child : go,
                                          parent : menu_command
                                                           .context as
                                                       GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }
  }
}
#endif
