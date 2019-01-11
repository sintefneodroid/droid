using System.IO;
using System.Reflection;
using Neodroid.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Common.ReadMe.ScriptableObjects.Editor {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CustomEditor(typeof(ReadMe))]
  [InitializeOnLoad]
  public class ReadMeEditor : UnityEditor.Editor {
    const string _showed_readme_session_state_name = "ReadMeEditor.showedReadMe";

    const float _space = 16f;
    public const string _NewAssetPath = "Assets/";

    const string _scriptable_object_menu_path = "Tools/ReadMe/";
    [SerializeField] GUIStyle _body_style = new GUIStyle(EditorStyles.label) {wordWrap = true, fontSize = 14};
    [SerializeField] GUIStyle _heading_style;
    [SerializeField] GUIStyle _link_style;

    bool _m_initialized;
    [SerializeField] GUIStyle _title_style;

    GUIStyle LinkStyle { get { return this._link_style; } }

    GUIStyle TitleStyle { get { return this._title_style; } }

    GUIStyle HeadingStyle { get { return this._heading_style; } }

    GUIStyle BodyStyle { get { return this._body_style; } }

    static void SelectReadmeAutomatically() {
      if (!SessionState.GetBool(_showed_readme_session_state_name, false)) {
        var readme = SelectReadme();
        SessionState.SetBool(_showed_readme_session_state_name, true);

        if (readme && !readme._LoadedLayout) {
          LoadLayout();
          readme._LoadedLayout = true;
        }
      }
    }

    static void LoadLayout() {
      var assembly = typeof(EditorApplication).Assembly;
      var window_layout_type = assembly.GetType("UnityEditor.WindowLayout", true);
      var method = window_layout_type.GetMethod(
          "LoadWindowLayout",
          BindingFlags.Public | BindingFlags.Static);
      method?.Invoke(
          null,
          new object[] {Path.Combine(Application.dataPath, "Excluded/Common/ReadMe/Layout.wlt"), false});
    }

    [MenuItem(_scriptable_object_menu_path + "Show ReadMe")]
    static ReadMe SelectReadme() {
      var ids = AssetDatabase.FindAssets("ReadMe t:ReadMe");
      if (ids.Length == 1) {
        var readme_object = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

        Selection.objects = new[] {readme_object};

        return (ReadMe)readme_object;
      }

      Debug.Log("Couldn't find a readme");
      return null;
    }

    /// <summary>
    /// </summary>
    [MenuItem(_scriptable_object_menu_path + "Create new ReadMe")]
    public static void CreateReadMeAsset() {
      var asset = CreateInstance<ReadMe>();

      AssetDatabase.CreateAsset(asset, _NewAssetPath + "NewReadMe.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }

    /// <summary>
    /// </summary>
    protected override void OnHeaderGUI() {
      var readme = (ReadMe)this.target;
      this.Init();

      var icon_width = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

      GUILayout.BeginHorizontal("In BigTitle");
      {
        GUILayout.Label(readme._Icon, GUILayout.Width(icon_width), GUILayout.Height(icon_width));
        GUILayout.Label(readme._Title, this.TitleStyle);
      }
      GUILayout.EndHorizontal();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var readme = (ReadMe)this.target;
      this.Init();

      if (readme._Sections != null) {
        foreach (var section in readme._Sections) {
          if (!string.IsNullOrEmpty(section._Heading)) {
            GUILayout.Label(section._Heading, this.HeadingStyle);
          }

          if (!string.IsNullOrEmpty(section._Text)) {
            GUILayout.Label(section._Text, this.BodyStyle);
          }

          if (!string.IsNullOrEmpty(section._LinkText)) {
            if (NeodroidEditorUtilities.LinkLabel(new GUIContent(section._LinkText), this.LinkStyle)) {
              Application.OpenURL(section._Url);
            }
          }

          GUILayout.Space(_space);
        }
      }
    }

    void Init() {
      if (this._m_initialized) {
        return;
      }

      this._title_style = new GUIStyle(this._body_style) {fontSize = 26};

      this._heading_style = new GUIStyle(this._body_style) {fontSize = 18};

      this._link_style = new GUIStyle(this._body_style) {
          wordWrap = false,
          normal = {textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f)},
          stretchWidth = false
      };
      // Match selection color which works nicely for both light and dark skins

      this._m_initialized = true;
    }
  }
}