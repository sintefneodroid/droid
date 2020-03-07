using droid.Runtime.Prototyping.Sensors;
using droid.Runtime.Prototyping.Sensors.Spatial.Transform;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  /// <summary>
  ///
  /// </summary>
  public class SensorSpawner : MonoBehaviour {
    [MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Sensors/Base", false, 10)]
    static void CreateSensorGameObject(MenuCommand menu_command) {
      var go = new GameObject("Sensor");
      go.AddComponent<Sensor>();
      GameObjectUtility.SetParentAndAlign(child : go,
                                          parent : menu_command
                                                           .context as
                                                       GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                     name : "Create " + go.name); // Register the creation in the undo system
      Selection.activeObject = go;
    }

    [MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "Sensors/EulerTransform", false, 10)]
    static void CreateEulerTransformSensorGameObject(MenuCommand menu_command) {
      var go = new GameObject("EulerTransformSensor");
      go.AddComponent<EulerTransformSensor>();
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
