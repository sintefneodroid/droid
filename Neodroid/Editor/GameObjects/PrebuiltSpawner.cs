#if UNITY_EDITOR
using Neodroid.Environments;
using Neodroid.Managers;
using Neodroid.Prototyping.Actors;
using Neodroid.Prototyping.Configurables;
using Neodroid.Prototyping.Motors;
using Neodroid.Prototyping.Observers;
using Neodroid.Utilities.BoundingBoxes;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.GameObjects {
  public class PrebuiltSpawner : MonoBehaviour {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="menu_command"></param>
    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "Prebuilt/SimpleEnvironment", false, 10)]
    static void CreateSingleEnvironmentGameObject(MenuCommand menu_command) {
      var go = new GameObject("SimpleEnvironment");
      go.AddComponent<PausableManager>();
      var env = go.AddComponent<PrototypingEnvironment>();
      go.AddComponent<BoxCollider>();
      var bounding_box = go.AddComponent<BoundingBox>();
      env.PlayableArea = bounding_box;


      var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
      plane.transform.parent = go.transform;

      var actor = new GameObject("Actor");
      actor.AddComponent<Actor>();
      actor.AddComponent<EulerTransformMotor3Dof>();
      actor.AddComponent<EulerTransformObserver>();
      actor.AddComponent<PositionConfigurable>();
      actor.transform.parent = go.transform;

      var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
      capsule.transform.parent = actor.transform;
      capsule.transform.localPosition = Vector3.up;

      bounding_box.Initialise();
      
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
