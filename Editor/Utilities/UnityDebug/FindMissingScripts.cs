#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities.UnityDebug {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class FindMissingScripts : EditorWindow {
    static int _game_object_count, _components_count, _missing_count;
    [SerializeField] Texture2D icon;

    /// <summary>
    /// </summary>
    [MenuItem("Tools/Debug/FindMissingScripts")]
    [MenuItem("Window/Debug/FindMissingScripts")]
    public static void ShowWindow() { GetWindow(typeof(FindMissingScripts)); }

    void OnEnable() {
      this.icon =
          (Texture2D)AssetDatabase.LoadAssetAtPath(NeodroidSettings.Current.NeodroidImportLocationProp
                                                   + "Gizmos/Icons/information.png",
                                                   typeof(Texture2D));
      this.titleContent = new GUIContent("Unity:Debug", this.icon, "Window for debugging Unity");
    }

    /// <summary>
    /// </summary>
    public void OnGUI() {
      if (GUILayout.Button("Find Missing Scripts in selected GameObjects")) {
        FindInSelected();
      }
    }

    static void FindInSelected() {
      var game_objects = Selection.gameObjects;
      _game_object_count = 0;
      _components_count = 0;
      _missing_count = 0;
      foreach (var g in game_objects) {
        SearchInGameObject(g);
      }

      Debug.Log($"Searched {_game_object_count} GameObjects, {_components_count} components, found {_missing_count} missing");
    }

    static void SearchInGameObject(GameObject game_object) {
      _game_object_count++;
      var components = game_object.GetComponents<Component>();
      for (var i = 0; i < components.Length; i++) {
        _components_count++;
        if (components[i] == null) {
          _missing_count++;
          var name = game_object.name;
          var parent = game_object.transform;
          while (parent.parent != null) {
            var parent1 = parent.parent;
            name = parent1.name + "/" + name;
            parent = parent1;
          }

          Debug.Log(name + " has an empty script attached in position: " + i, game_object);
        }
      }

      // Now recurse through each child GameObject (if there are any):
      foreach (Transform child in game_object.transform) {
        SearchInGameObject(child.gameObject);
      }
    }
  }
}
#endif
