using System;
using System.IO;
using droid.Editor.Utilities;
using droid.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace droid.Editor {
  /// <inheritdoc />
  /// <summary>
  /// Create a new type of Settings Asset.
  /// </summary>
  [Serializable]
  class NeodroidSettings : ScriptableObject {
    #region Fields

    [SerializeField] bool NeodroidEnableDebug = false;

    [SerializeField] bool NeodroidGithubExtension = false;

    [SerializeField] bool NeodroidIsPackage = false;

    [SerializeField] string NeodroidImportLocation = NeodroidEditorConstants._Default_Import_Location;
    [SerializeField] bool NeodroidGeneratePreviews = false;

    [SerializeField]
    string NeodroidPreviewsLocation = NeodroidEditorConstants._Default_Scene_Previews_Location;

    [SerializeField] bool NeodroidGenerateDescriptions = false;

    [SerializeField]
    string NeodroidDescriptionLocation = NeodroidEditorConstants._Default_Scene_Description_Location;

    #endregion

    #region Properties

    public bool NeodroidEnableDebugProp {
      get { return this.NeodroidEnableDebug; }
      set {
        if (value != this.NeodroidEnableDebug || _force) {
          ApplyDebug(value : value);
          this.NeodroidEnableDebug = value;
        }
      }
    }

    public static void ApplyDebug(bool value) {
      if (value) {
        Debug.Log(message : $"Neodroid Debugging enabled");
        DefineSymbolsFunctionality.AddDebugDefineSymbol();
      } else {
        Debug.Log(message : $"Neodroid Debugging disabled");
        DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
      }
    }

    public bool NeodroidGithubExtensionProp {
      get { return this.NeodroidGithubExtension; }
      set {
        if (value != this.NeodroidGithubExtension || _force) {
          ApplyGithubExt(value : value);

          this.NeodroidGithubExtension = value;
        }
      }
    }

    public static void ApplyGithubExt(bool value) {
      if (value) {
        DefineSymbolsFunctionality.AddGithubDefineSymbols();
        Debug.Log(message : $"Neodroid GitHub Extension enabled");
      } else {
        DefineSymbolsFunctionality.RemoveGithubDefineSymbols();
        Debug.Log(message : $"Neodroid GitHub Extension disabled");
      }
    }

    public bool NeodroidIsPackageProp {
      get { return this.NeodroidIsPackage; }
      set {
        if (value != this.NeodroidIsPackage || _force) {
          ApplyIsPackage(value : value);
          this.NeodroidIsPackage = value;
        }
      }
    }

    public static void ApplyIsPackage(bool value) {
      if (value) {
        DefineSymbolsFunctionality.AddIsPackageDefineSymbols();
        Debug.Log(message : $"Neodroid is set as an imported asset");
      } else {
        DefineSymbolsFunctionality.RemoveIsPackageDefineSymbols();
        Debug.Log(message : $"Neodroid is set as an package asset");
      }
    }

    public string NeodroidImportLocationProp {
      get { return this.NeodroidImportLocation; }
      set {
        if (value != this.NeodroidImportLocation || _force) {
          var new_path = PathTrim(value : value);
          Debug.Log(message : $"Setting Neodroid import location to: {new_path}");

          this.NeodroidImportLocation = new_path;
        }
      }
    }

    public bool NeodroidGeneratePreviewsProp {
      get { return this.NeodroidGeneratePreviews; }
      set { this.NeodroidGeneratePreviews = value; }
    }

    public string NeodroidPreviewsLocationProp {
      get { return this.NeodroidPreviewsLocation; }
      set {
        if (value != this.NeodroidPreviewsLocation || _force) {
          var new_path = PathTrim(value : value);
          Debug.Log(message : $"Setting Neodroid ScenePreview location to: {new_path}");

          this.NeodroidPreviewsLocation = new_path;
        }
      }
    }

    public bool NeodroidGenerateDescriptionsProp {
      get { return this.NeodroidGenerateDescriptions; }
      set { this.NeodroidGenerateDescriptions = value; }
    }

    public string NeodroidDescriptionLocationProp {
      get { return this.NeodroidDescriptionLocation; }
      set {
        if (value != this.NeodroidDescriptionLocation || _force) {
          var new_path = PathTrim(value : value);
          Debug.Log(message : $"Setting Neodroid SceneDescription location to: {new_path}");

          this.NeodroidDescriptionLocation = new_path;
        }
      }
    }

    public static string PathTrim(string value) {
      var new_path = value.TrimEnd('/') + "/";
      return new_path;
    }

    #endregion

    internal static NeodroidSettings Defaults() {
      var settings = CreateInstance<NeodroidSettings>();
      settings.NeodroidEnableDebugProp = false;
      settings.NeodroidGithubExtensionProp = false;
      settings.NeodroidIsPackageProp = false;
      settings.NeodroidImportLocationProp = NeodroidEditorConstants._Default_Import_Location;

      settings.NeodroidGeneratePreviewsProp = false;
      settings.NeodroidPreviewsLocationProp = NeodroidEditorConstants._Default_Scene_Previews_Location;
      settings.NeodroidGenerateDescriptionsProp = false;
      settings.NeodroidDescriptionLocationProp = NeodroidEditorConstants._Default_Scene_Description_Location;

      return settings;
    }

    internal static NeodroidSettings Current {
      get {
        var settings =
            AssetDatabase.LoadAssetAtPath<NeodroidSettings>(assetPath : NeodroidEditorConstants
                                                                ._NeodroidSettingsPath);
        if (settings == null) {
          settings = Defaults();

          var path = Path.GetDirectoryName(path : NeodroidEditorConstants._NeodroidSettingsPath);
          Directory.CreateDirectory(path : path);
          AssetDatabase.CreateAsset(asset : settings, path : NeodroidEditorConstants._NeodroidSettingsPath);
          AssetDatabase.SaveAssets();
        }

        return settings;
      }
    }

    static bool _force = false;

    internal static void ReapplyProperties(bool force = false) {
      _force = force;
      Current.NeodroidEnableDebugProp = Current.NeodroidEnableDebug;
      Current.NeodroidGithubExtensionProp = Current.NeodroidGithubExtension;
      Current.NeodroidIsPackageProp = Current.NeodroidIsPackage;
      Current.NeodroidImportLocationProp = Current.NeodroidImportLocation;
      Current.NeodroidGeneratePreviewsProp = Current.NeodroidGeneratePreviews;
      Current.NeodroidPreviewsLocationProp = Current.NeodroidPreviewsLocation;
      Current.NeodroidGenerateDescriptionsProp = Current.NeodroidGenerateDescriptions;
      Current.NeodroidDescriptionLocationProp = Current.NeodroidDescriptionLocation;
      _force = false;
    }

    void OnValidate() {
      //ReapplyProperties();
    }

    internal static SerializedObject GetSerializedSettings() {
      var serialized_object = new SerializedObject(obj : Current);
      return serialized_object;
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// SettingsProvider for Neodroid
  /// </summary>
  class NeodroidSettingsProvider : SettingsProvider {
    SerializedObject _neodroid_settings;

    /// <summary>
    ///
    /// </summary>
    class Styles {
      public static GUIContent _EnableNeodroidDebug =
          new GUIContent(text : NeodroidEditorConstants._Debug_Pref_Key);

      public static GUIContent _EnableGithubExtension =
          new GUIContent(text : NeodroidEditorConstants._Github_Extension_Pref_Key);

      public static GUIContent
          _IsPackage = new GUIContent(text : NeodroidEditorConstants._IsPackage_Pref_Key);

      public static GUIContent _ImportLocation =
          new GUIContent(text : NeodroidEditorConstants._Import_Location_Pref_Key);

      public static GUIContent _GenerateScenePreview =
          new GUIContent(text : NeodroidEditorConstants._Generate_Previews_Pref_Key);

      public static GUIContent _ScenePreviewLocation =
          new GUIContent(text : NeodroidEditorConstants._Generate_Previews_Loc_Pref_Key);

      public static GUIContent _GenerateSceneDescription =
          new GUIContent(text : NeodroidEditorConstants._Generate_Descriptions_Pref_Key);

      public static GUIContent _SceneDescriptionLocation =
          new GUIContent(text : NeodroidEditorConstants._Generate_Descriptions_Loc_Pref_Key);
    }

    public NeodroidSettingsProvider(string path, SettingsScope scope = SettingsScope.User) :
        base(path : path, scopes : scope) { }

    public static bool IsSettingsAvailable() {
      return File.Exists(path : NeodroidEditorConstants._NeodroidSettingsPath);
    }

    /// <summary>
    ///  This function is called when the user clicks on the MyCustom element in the Settings window.
    /// </summary>
    public override void OnActivate(string search_context, VisualElement root_element) {
      this._neodroid_settings = NeodroidSettings.GetSerializedSettings();
    }

    public override void OnGUI(string search_context) {
      EditorGUILayout.HelpBox(message : $"Version {NeodroidRuntimeInfo._Version}", type : MessageType.Info);

      var is_package =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants._IsPackage_Pref_Key);
      EditorGUILayout.PropertyField(property : is_package, label : Styles._IsPackage);
      if (!is_package.boolValue) {
        EditorGUILayout.HelpBox("Enter import path of Neodroid", type : MessageType.Info);
        EditorGUILayout.PropertyField(property :
                                      this._neodroid_settings.FindProperty(propertyPath :
                                                                           NeodroidEditorConstants
                                                                               ._Import_Location_Pref_Key),
                                      label : Styles._ImportLocation);
      }

      EditorGUILayout.HelpBox("Functionality", type : MessageType.Info);

      EditorGUILayout.PropertyField(property :
                                    this._neodroid_settings.FindProperty(propertyPath :
                                                                         NeodroidEditorConstants
                                                                             ._Debug_Pref_Key),
                                    label : Styles._EnableNeodroidDebug);
      EditorGUILayout.PropertyField(property :
                                    this._neodroid_settings.FindProperty(propertyPath :
                                                                         NeodroidEditorConstants
                                                                             ._Github_Extension_Pref_Key),
                                    label : Styles._EnableGithubExtension);

      var generate_scene_preview =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants
                                                   ._Generate_Previews_Pref_Key);
      EditorGUILayout.PropertyField(property : generate_scene_preview, label : Styles._GenerateScenePreview);
      if (generate_scene_preview.boolValue) {
        EditorGUILayout.HelpBox("Enter path for scene preview storage", type : MessageType.Info);
        EditorGUILayout.PropertyField(property :
                                      this._neodroid_settings.FindProperty(propertyPath :
                                                                           NeodroidEditorConstants
                                                                               ._Generate_Previews_Loc_Pref_Key),
                                      label : Styles._ScenePreviewLocation);
      }

      var generate_scene_descriptions =
          this._neodroid_settings.FindProperty(propertyPath : NeodroidEditorConstants
                                                   ._Generate_Descriptions_Pref_Key);
      EditorGUILayout.PropertyField(property : generate_scene_descriptions,
                                    label : Styles._GenerateSceneDescription);
      if (generate_scene_descriptions.boolValue) {
        EditorGUILayout.HelpBox("Enter path for scene description storage", type : MessageType.Info);
        EditorGUILayout.PropertyField(property :
                                      this._neodroid_settings.FindProperty(propertyPath :
                                                                           NeodroidEditorConstants
                                                                               ._Generate_Descriptions_Loc_Pref_Key),
                                      label : Styles._SceneDescriptionLocation);
      }

      this._neodroid_settings.ApplyModifiedProperties();

      if (EditorGUILayout.Toggle("Apply", false)) {
        NeodroidSettings.ReapplyProperties(force : true);
        EditorUtility.SetDirty(target : NeodroidSettings.Current);
      }
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateNeodroidSettingsProvider() {
      if (IsSettingsAvailable()) {
        var provider =
            new NeodroidSettingsProvider(path : NeodroidEditorConstants._Neodroid_Project_Settings_Menu_Path,
                                         scope : SettingsScope.Project) {
                                                                            keywords =
                                                                                GetSearchKeywordsFromGUIContentProperties
                                                                                    <Styles>()
                                                                        };

        //provider.keywords = GetSearchKeywordsFromPath(NeodroidEditorConstants._Neodroid_Project_Settings_Menu_Path);

        // Automatically extract all keywords from the Styles.
        return provider;
      }

      // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
      return null;
    }
  }
}
