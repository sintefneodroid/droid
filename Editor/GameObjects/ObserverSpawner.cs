#if UNITY_EDITOR
using droid.Runtime.Prototyping.Sensors;
using droid.Runtime.Prototyping.Sensors.Transform;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  public class ObserverSpawner : MonoBehaviour {
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Observers/Base", false, 10)]
    static void CreateObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject("Sensor");
      go.AddComponent<Sensor>();
      GameObjectUtility.SetParentAndAlign(go,
                                          menu_command
                                                  .context as
                                              GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(go, "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem("GameObject/Neodroid/Observers/EulerTransform", false, 10)]
    static void CreateEulerTransformObserverGameObject(MenuCommand menu_command) {
      var go = new GameObject("EulerTransformSensor");
      go.AddComponent<EulerTransformSensor>();
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
