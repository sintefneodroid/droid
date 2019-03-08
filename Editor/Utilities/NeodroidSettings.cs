using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace droid.Editor.Utilities
{
// Create a new type of Settings Asset.
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  class NeodroidSettings : ScriptableObject
  {
    #region Fields

    [SerializeField] int _number = 0;

    [SerializeField] string _string = "Has no effect yet";

    [SerializeField] bool _bool = true;

    #endregion

    #region Properties

    public String SomeString
    {
      get { return this._string; }
      set { this._string = value; }
    }

    public int Number
    {
      get { return this._number; }
      set { this._number = value; }
    }

    public bool SomeBool
    {
      get { return this._bool; }
      set { this._bool = value; }
    }

    #endregion

    internal static NeodroidSettings Defaults()
    {
      var settings = CreateInstance<NeodroidSettings>();
      settings.Number = 42;
      settings.SomeString = "The answer to the universe";
      settings.SomeBool = true;

      return settings;
    }


    internal static NeodroidSettings GetOrCreateSettings()
    {
      var settings =
        AssetDatabase.LoadAssetAtPath<NeodroidSettings>(NeodroidEditorConstants._NeodroidSettingsPath);
      if (settings == null)
      {
        settings = Defaults();

        AssetDatabase.CreateAsset(settings, NeodroidEditorConstants._NeodroidSettingsPath);
        AssetDatabase.SaveAssets();
      }

      return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
      return new SerializedObject(GetOrCreateSettings());
    }

    public class Properties
    {
      public const string _Number = "_number";
      public const string _SomeString = "_string";
      public const string _SomeBool = "_bool";
    }

    public class Styles
    {
      public static GUIContent _Number = new GUIContent("Number");
      public static GUIContent _SomeString = new GUIContent("String");
      public static GUIContent _SomeBool = new GUIContent("Bool");
    }
  }

// Register a SettingsProvider using IMGUI for the drawing framework:
  static class NeodroidSettingsImguiRegister
  {
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
      var provider = new SettingsProvider(NeodroidEditorConstants.neodroid_project_settings_menu_path,
        SettingsScope.Project)
      {
        //Second parameter is the scope of this setting: it only appears in the Project Settings window.

        label = "Neodroid", // By default the last token of the path is used as display name if no label is provided.

        guiHandler = search_context =>
        {
          var settings = NeodroidSettings.GetSerializedSettings();
          EditorGUILayout.PropertyField(settings.FindProperty(NeodroidSettings.Properties._Number),
          NeodroidSettings.Styles._Number);
          EditorGUILayout.PropertyField(settings.FindProperty(NeodroidSettings.Properties._SomeString),
          NeodroidSettings.Styles._SomeString);
          EditorGUILayout.PropertyField(settings.FindProperty(NeodroidSettings.Properties._SomeBool), NeodroidSettings.Styles._SomeBool);
        }, // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:


        keywords = new HashSet<string>(new[]
        {
          "Number", "Some String"
        }) // Populate the search keywords to enable smart search filtering and label highlighting:
      };

      return provider;
    }
  }

/*

  class NeodroidSettingsProvider : SettingsProvider // Create MyCustomSettingsProvider by deriving from SettingsProvider:
  {
    SerializedObject _m_custom_settings;

    public NeodroidSettingsProvider(string path, SettingsScope scope = SettingsScope.User) :
      base(path, scope)
    {
    }

    public static bool IsSettingsAvailable()
    {
      return File.Exists(NeodroidEditorConstants._NeodroidSettingsPath);
    }

    public void OnActivate(string search_context, VisualElement root_element)
    {
      // This function is called when the user clicks on the MyCustom element in the Settings window.
      this._m_custom_settings = NeodroidSettings.GetSerializedSettings();
    }

    public override void OnGUI(string search_context)
    {
      EditorGUILayout.PropertyField(this._m_custom_settings.FindProperty(NeodroidSettings.Properties._Number),
        NeodroidSettings.Styles._Number);
      EditorGUILayout.PropertyField(this._m_custom_settings.FindProperty(NeodroidSettings.Properties._SomeString),
        NeodroidSettings.Styles._SomeString);
      EditorGUILayout.PropertyField(this._m_custom_settings.FindProperty(NeodroidSettings.Properties._SomeBool),
        NeodroidSettings.Styles._SomeBool);
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
      if (IsSettingsAvailable())
      {
        var provider = new NeodroidSettingsProvider(
          NeodroidEditorConstants.neodroid_project_settings_menu_path,
          SettingsScope.Project)
        {
          keywords = GetSearchKeywordsFromGUIContentProperties<NeodroidSettings.Styles>()
        };

        // Automatically extract all keywords from the Styles.
        return provider;
      }

      // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
      return null;
    }
  }
  */
}