using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using System.Linq;
using UnityEngine;

namespace droid.Editor.Utilities.UnityDebug {
  /// <inheritdoc />
  /// <summary>
  ///   A helper editor script for finding missing references to objects.
  /// </summary>
  public class MissingReferencesFinder : MonoBehaviour {
    const string _menu_root = "Tools/" + "Missing References/";

    /// <summary>
    ///   Finds all missing references to objects in the currently loaded scene.
    /// </summary>
    [MenuItem(itemName : _menu_root + "Search in scene", false, 50)]
    public static void FindMissingReferencesInCurrentScene() {
      var scene_objects = GetSceneObjects();
      FindMissingReferences(context : SceneManager.GetActiveScene().name, objects : scene_objects);
    }

    /// <summary>
    ///   Finds all missing references to objects in all enabled scenes in the project.
    ///   This works by loading the scenes one by one and checking for missing object references.
    /// </summary>
    [MenuItem(itemName : _menu_root + "Search in all scenes", false, 51)]
    public static void MissingSpritesInAllScenes() {
      foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled)) {
        EditorSceneManager.OpenScene(scenePath : scene.path);
        FindMissingReferencesInCurrentScene();
      }
    }

    /// <summary>
    ///   Finds all missing references to objects in assets (objects from the project window).
    /// </summary>
    [MenuItem(itemName : _menu_root + "Search in assets", false, 52)]
    public static void MissingSpritesInAssets() {
      var all_assets = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith("Assets/")).ToArray();
      var objs = all_assets
                 .Select(a => AssetDatabase.LoadAssetAtPath(assetPath : a, type : typeof(GameObject)) as
                                  GameObject).Where(a => a != null).ToArray();

      FindMissingReferences("Project", objects : objs);
    }

    static void FindMissingReferences(string context, GameObject[] objects) {
      foreach (var go in objects) {
        var components = go.GetComponents<Component>();

        foreach (var c in components) {
          // Missing components will be null, we can't find their type, etc.
          if (!c) {
            Debug.LogError(message : "Missing Component in GO: " + GetFullPath(go : go), context : go);
            continue;
          }

          var so = new SerializedObject(obj : c);
          var sp = so.GetIterator();

          // Iterate over the components' properties.
          while (sp.NextVisible(true)) {
            if (sp.propertyType == SerializedPropertyType.ObjectReference) {
              if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0) {
                ShowError(context : context,
                          go : go,
                          component_name : c.GetType().Name,
                          property_name : ObjectNames.NicifyVariableName(name : sp.name));
              }
            }
          }
        }
      }
    }

    static GameObject[] GetSceneObjects() {
      // Use this method since GameObject.FindObjectsOfType will not return disabled objects.
      return Resources.FindObjectsOfTypeAll<GameObject>()
                      .Where(go => string.IsNullOrEmpty(value : AssetDatabase.GetAssetPath(assetObject : go))
                                   && go.hideFlags == HideFlags.None).ToArray();
    }

    static void ShowError(string context, GameObject go, string component_name, string property_name) {
      const string error_template = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";

      Debug.LogError(message : string.Format(format : error_template,
                                             GetFullPath(go : go),
                                             component_name,
                                             property_name,
                                             context),
                     context : go);
    }

    static string GetFullPath(GameObject go) {
      return go.transform.parent == null
                 ? go.name
                 : GetFullPath(go : go.transform.parent.gameObject) + "/" + go.name;
    }
  }
}
#endif
