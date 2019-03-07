using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities.ScriptableObjects.Editor {
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
    [SerializeField] GUIStyle bodyStyle = new GUIStyle(EditorStyles.label) {wordWrap = true, fontSize = 14};
    [SerializeField] GUIStyle headingStyle;
    [SerializeField] GUIStyle linkStyle;

    bool _m_initialized;
    [SerializeField] GUIStyle titleStyle;

    GUIStyle LinkStyle { get { return this.linkStyle; } }

    GUIStyle TitleStyle { get { return this.titleStyle; } }

    GUIStyle HeadingStyle { get { return this.headingStyle; } }

    GUIStyle BodyStyle { get { return this.bodyStyle; } }

    static void SelectReadmeAutomatically() {
      if (!SessionState.GetBool(_showed_readme_session_state_name, false)) {
        var readme = SelectReadme();
        SessionState.SetBool(_showed_readme_session_state_name, true);

        if (readme && !readme.loadedLayout) {
          LoadLayout();
          readme.loadedLayout = true;
        }
      }
    }

    static void LoadLayout() {
      var assembly = typeof(EditorApplication).Assembly;
      var window_layout_type = assembly.GetType("UnityEditor.WindowLayout", true);
      var method =
          window_layout_type.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
      method?.Invoke(null,
                     new object[] {
                                      Path.Combine(Application.dataPath, "Excluded/Common/ReadMe/Layout.wlt"),
                                      false
                                  });
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
        GUILayout.Label(readme.icon, GUILayout.Width(icon_width), GUILayout.Height(icon_width));
        GUILayout.Label(readme.title, this.TitleStyle);
      }
      GUILayout.EndHorizontal();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      var readme = (ReadMe)this.target;
      this.Init();

      if (readme.sections != null) {
        foreach (var section in readme.sections) {
          if (!string.IsNullOrEmpty(section.heading)) {
            GUILayout.Label(section.heading, this.HeadingStyle);
          }

          if (!string.IsNullOrEmpty(section.text)) {
            GUILayout.Label(section.text, this.BodyStyle);
          }

          if (!string.IsNullOrEmpty(section.linkText)) {
            if (NeodroidEditorUtilities.LinkLabel(new GUIContent(section.linkText), this.LinkStyle)) {
              Application.OpenURL(section.url);
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

      this.titleStyle = new GUIStyle(this.bodyStyle) {fontSize = 26};

      this.headingStyle = new GUIStyle(this.bodyStyle) {fontSize = 18};

      this.linkStyle = new GUIStyle(this.bodyStyle) {
                                                        wordWrap = false,
                                                        normal = {
                                                                     textColor =
                                                                         new Color(0x00 / 255f,
                                                                                   0x78 / 255f,
                                                                                   0xDA / 255f,
                                                                                   1f)
                                                                 },
                                                        stretchWidth = false
                                                    };
      // Match selection color which works nicely for both light and dark skins

      this._m_initialized = true;
    }
  }
}
