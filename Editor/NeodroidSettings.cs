﻿using System;
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

    [SerializeField] bool NeodroidIsImportedAsset = true;

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
        if (value != this.NeodroidEnableDebug) {
          ApplyDebug(value);
          this.NeodroidEnableDebug = value;
        }
      }
    }

    public static void ApplyDebug(bool value) {
      if (value) {
        Debug.Log($"Neodroid Debugging enabled");
        DefineSymbolsFunctionality.AddDebugDefineSymbol();
      } else {
        Debug.Log($"Neodroid Debugging disabled");
        DefineSymbolsFunctionality.RemoveDebugDefineSymbols();
      }
    }

    public bool NeodroidGithubExtensionProp {
      get { return this.NeodroidGithubExtension; }
      set {
        if (value != this.NeodroidGithubExtension) {
          ApplyGithubExt(value);

          this.NeodroidGithubExtension = value;
        }
      }
    }

    public static void ApplyGithubExt(bool value) {
      if (value) {
        DefineSymbolsFunctionality.AddGithubDefineSymbols();
        Debug.Log($"Neodroid GitHub Extension enabled");
      } else {
        DefineSymbolsFunctionality.RemoveGithubDefineSymbols();
        Debug.Log($"Neodroid GitHub Extension disabled");
      }
    }

    public bool NeodroidIsImportedAssetProp {
      get { return this.NeodroidIsImportedAsset; }
      set {
        if (value != this.NeodroidIsImportedAsset) {
          ApplyIsPackage(value);
          this.NeodroidIsImportedAsset = value;
        }
      }
    }

    public static void ApplyIsPackage(bool value) {
      if (!value) {
        DefineSymbolsFunctionality.AddImportedAssetDefineSymbols();
        Debug.Log($"Neodroid is set as an imported asset");
      } else {
        DefineSymbolsFunctionality.RemoveImportedAssetDefineSymbols();
        Debug.Log($"Neodroid is set as an package asset");
      }
    }

    public String NeodroidImportLocationProp {
      get { return this.NeodroidImportLocation; }
      set {
        if (value != this.NeodroidImportLocation) {
          var new_path = PathTrim(value);
          Debug.Log($"Setting Neodroid import location to: {new_path}");

          this.NeodroidImportLocation = new_path;
        }
      }
    }

    public bool NeodroidGeneratePreviewsProp {
      get { return this.NeodroidGeneratePreviews; }
      set { this.NeodroidGeneratePreviews = value; }
    }

    public String NeodroidPreviewsLocationProp {
      get { return this.NeodroidPreviewsLocation; }
      set {
        if (value != this.NeodroidPreviewsLocation) {
          var new_path = PathTrim(value);
          Debug.Log($"Setting Neodroid ScenePreview location to: {new_path}");

          this.NeodroidPreviewsLocation = new_path;
        }
      }
    }

    public bool NeodroidGenerateDescriptionsProp {
      get { return this.NeodroidGenerateDescriptions; }
      set { this.NeodroidGenerateDescriptions = value; }
    }

    public String NeodroidDescriptionLocationProp {
      get { return this.NeodroidDescriptionLocation; }
      set {
        if (value != this.NeodroidDescriptionLocation) {
          var new_path = PathTrim(value);
          Debug.Log($"Setting Neodroid SceneDescription location to: {new_path}");

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
      settings.NeodroidIsImportedAssetProp = false;
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
            AssetDatabase.LoadAssetAtPath<NeodroidSettings>(NeodroidEditorConstants._NeodroidSettingsPath);
        if (settings == null) {
          settings = Defaults();

          var path = Path.GetDirectoryName(NeodroidEditorConstants._NeodroidSettingsPath);
          Directory.CreateDirectory(path);
          AssetDatabase.CreateAsset(settings, NeodroidEditorConstants._NeodroidSettingsPath);
          AssetDatabase.SaveAssets();
        }

        return settings;
      }
    }

    internal static void ReapplyProperties() {
      Current.NeodroidEnableDebugProp = Current.NeodroidEnableDebug;
      Current.NeodroidGithubExtensionProp = Current.NeodroidGithubExtension;
      Current.NeodroidIsImportedAssetProp = Current.NeodroidIsImportedAsset;
      Current.NeodroidImportLocationProp = Current.NeodroidImportLocation;
      Current.NeodroidGeneratePreviewsProp = Current.NeodroidGeneratePreviews;
      Current.NeodroidPreviewsLocationProp = Current.NeodroidPreviewsLocation;
      Current.NeodroidGenerateDescriptionsProp = Current.NeodroidGenerateDescriptions;
      Current.NeodroidDescriptionLocationProp = Current.NeodroidDescriptionLocation;
    }

    void OnValidate() { ReapplyProperties(); }

    internal static SerializedObject GetSerializedSettings() {
      var serialized_object = new SerializedObject(Current);
      return serialized_object;
    }
  }

  /// <summary>
  /// SettingsProvider for Neodroid
  /// </summary>
  class NeodroidSettingsProvider : SettingsProvider {
    SerializedObject _neodroid_settings;

    /// <summary>
    ///
    /// </summary>
    class Styles {
      public static GUIContent _EnableNeodroidDebug = new GUIContent(NeodroidEditorConstants._Debug_Pref_Key);

      public static GUIContent _EnableGithubExtension =
          new GUIContent(NeodroidEditorConstants._Github_Extension_Pref_Key);

      public static GUIContent _IsImportedAsset =
          new GUIContent(NeodroidEditorConstants._Imported_Asset_Pref_Key);

      public static GUIContent _ImportLocation =
          new GUIContent(NeodroidEditorConstants._Import_Location_Pref_Key);

      public static GUIContent _GenerateScenePreview =
          new GUIContent(NeodroidEditorConstants._Generate_Previews_Pref_Key);

      public static GUIContent _ScenePreviewLocation =
          new GUIContent(NeodroidEditorConstants._Generate_Previews_Loc_Pref_Key);

      public static GUIContent _GenerateSceneDescription =
          new GUIContent(NeodroidEditorConstants._Generate_Descriptions_Pref_Key);

      public static GUIContent _SceneDescriptionLocation =
          new GUIContent(NeodroidEditorConstants._Generate_Descriptions_Loc_Pref_Key);
    }

    public NeodroidSettingsProvider(string path, SettingsScope scope = SettingsScope.User) :
        base(path, scope) { }

    public static bool IsSettingsAvailable() {
      return File.Exists(NeodroidEditorConstants._NeodroidSettingsPath);
    }

    /// <summary>
    ///  This function is called when the user clicks on the MyCustom element in the Settings window.
    /// </summary>
    public override void OnActivate(string search_context, VisualElement root_element) {
      this._neodroid_settings = NeodroidSettings.GetSerializedSettings();
    }

    public override void OnGUI(string search_context) {
      EditorGUILayout.HelpBox($"Version {NeodroidRuntimeInfo._Version}", MessageType.Info);

      var is_imported_asset =
          this._neodroid_settings.FindProperty(NeodroidEditorConstants._Imported_Asset_Pref_Key);
      EditorGUILayout.PropertyField(is_imported_asset, Styles._IsImportedAsset);
      if (is_imported_asset.boolValue) {
        EditorGUILayout.HelpBox("Enter import path of Neodroid", MessageType.Info);
        EditorGUILayout.PropertyField(this._neodroid_settings.FindProperty(NeodroidEditorConstants
                                                                               ._Import_Location_Pref_Key),
                                      Styles._ImportLocation);
      }

      EditorGUILayout.HelpBox("Functionality", MessageType.Info);

      EditorGUILayout.PropertyField(this._neodroid_settings.FindProperty(NeodroidEditorConstants
                                                                             ._Debug_Pref_Key),
                                    Styles._EnableNeodroidDebug);
      EditorGUILayout.PropertyField(this._neodroid_settings.FindProperty(NeodroidEditorConstants
                                                                             ._Github_Extension_Pref_Key),
                                    Styles._EnableGithubExtension);

      var generate_scene_preview =
          this._neodroid_settings.FindProperty(NeodroidEditorConstants._Generate_Previews_Pref_Key);
      EditorGUILayout.PropertyField(generate_scene_preview, Styles._GenerateScenePreview);
      if (generate_scene_preview.boolValue) {
        EditorGUILayout.HelpBox("Enter path for scene preview storage", MessageType.Info);
        EditorGUILayout.PropertyField(this._neodroid_settings.FindProperty(NeodroidEditorConstants
                                                                               ._Generate_Previews_Loc_Pref_Key),
                                      Styles._ScenePreviewLocation);
      }

      var generate_scene_descriptions =
          this._neodroid_settings.FindProperty(NeodroidEditorConstants._Generate_Descriptions_Pref_Key);
      EditorGUILayout.PropertyField(generate_scene_descriptions, Styles._GenerateSceneDescription);
      if (generate_scene_descriptions.boolValue) {
        EditorGUILayout.HelpBox("Enter path for scene description storage", MessageType.Info);
        EditorGUILayout.PropertyField(this._neodroid_settings.FindProperty(NeodroidEditorConstants
                                                                               ._Generate_Descriptions_Loc_Pref_Key),
                                      Styles._SceneDescriptionLocation);
      }

      this._neodroid_settings.ApplyModifiedProperties();
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateNeodroidSettingsProvider() {
      if (IsSettingsAvailable()) {
        var provider =
            new NeodroidSettingsProvider(NeodroidEditorConstants._Neodroid_Project_Settings_Menu_Path,
                                         SettingsScope.Project) {
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