#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.GameObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrefabSpawnerPopup : EditorWindow {
    static Rect _rect = new Rect(0,
                                 0,
                                 0,
                                 0);

    [MenuItem(EditorGameObjectMenuPath._GameObjectMenuPath + "SpawnPrefab", false, 10)]
    static void Init2() {
      try {
        PopupWindow.Show(_rect, new PrefabsPopup());
      } catch (ExitGUIException) {
        //Debug.Log(e);
      }
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrefabsPopup : PopupWindowContent {
    Vector2 _scroll_position;
    bool _updated_pos;
    int _x_size = 300;
    int _y_size = 200;

    public override Vector2 GetWindowSize() { return new Vector2(this._x_size, this._y_size); }

    public override void OnGUI(Rect rect) {
      if (!this._updated_pos) {
        var mp = Event.current.mousePosition;
        rect.x = mp.x;
        rect.y = mp.y;

        this.editorWindow.position = rect;
        this._updated_pos = true;
      }

      GUILayout.Label("Spawn Prefab", EditorStyles.boldLabel);

      // Supports the following syntax:
      // 't:type' syntax (e.g 't:Texture2D' will show Texture2D objects)
      // 'l:assetlabel' syntax (e.g 'l:architecture' will show assets with AssetLabel 'architecture')
      // 'ref[:id]:path' syntax (e.g 'ref:1234' will show objects that references the object with instanceID 1234)
      // 'v:versionState' syntax (e.g 'v:modified' will show objects that are modified locally)
      // 's:softLockState' syntax (e.g 's:inprogress' will show objects that are modified by anyone (except you))
      // 'a:area' syntax (e.g 'a:all' will s search in all assets, 'a:assets' will s search in assets folder only and 'a:packages' will s search in packages folder only)

      var prefabs = AssetDatabase.FindAssets("t:Prefab a:all");

      this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
      EditorGUILayout.BeginVertical();
      foreach (var prefab in prefabs) {
        var path = AssetDatabase.GUIDToAssetPath(prefab);
        //Debug.Log(path);
        var go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        if (path.Contains("Neodroid")) {
          if (GUILayout.Button(go.name)) {
            Object.Instantiate(go, Selection.activeTransform);
          }
        }
      }

      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();
    }

    public override void OnOpen() {
      //Debug.Log("Popup opened: " + this);
    }

    public override void OnClose() {
      //Debug.Log("Popup closed: " + this);
    }
  }
}
#endif
