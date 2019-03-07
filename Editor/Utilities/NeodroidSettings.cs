using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace droid.Editor.Utilities {
// Create a new type of Settings Asset.
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  class NeodroidSettings : ScriptableObject {
    [SerializeField] int m_Number = 0;

    [SerializeField] string m_SomeString = "Has no effect yet";

    public String MSomeString { get { return this.m_SomeString; } set { this.m_SomeString = value; } }

    public Int32 MNumber { get { return this.m_Number; } set { this.m_Number = value; } }

    internal static NeodroidSettings GetOrCreateSettings() {
      var settings =
          AssetDatabase.LoadAssetAtPath<NeodroidSettings>(NeodroidEditorConstants._MyCustomSettingsPath);
      if (settings == null) {
        settings = CreateInstance<NeodroidSettings>();
        settings.m_Number = 42;
        settings.m_SomeString = "The answer to the universe";
        AssetDatabase.CreateAsset(settings, NeodroidEditorConstants._MyCustomSettingsPath);
        AssetDatabase.SaveAssets();
      }

      return settings;
    }

    internal static SerializedObject GetSerializedSettings() {
      return new SerializedObject(GetOrCreateSettings());
    }
  }

// Register a SettingsProvider using IMGUI for the drawing framework:
  static class NeodroidSettingsImguiRegister {
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider() {
      // First parameter is the path in the Settings window.
      // Second parameter is the scope of this setting: it only appears in the Project Settings window.
      var provider = new SettingsProvider("Project/NeodroidSettingsIMGUI", SettingsScope.Project) {
                                                                                                      // By default the last token of the path is used as display name if no label is provided.
                                                                                                      label =
                                                                                                          "Neodroid",
                                                                                                      // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                                                                                                      guiHandler
                                                                                                          = search_context => {
                                                                                                              var
                                                                                                                  settings
                                                                                                                      = NeodroidSettings
                                                                                                                          .GetSerializedSettings();
                                                                                                              EditorGUILayout
                                                                                                                  .PropertyField(settings
                                                                                                                                     .FindProperty("m_Number"),
                                                                                                                                 new
                                                                                                                                     GUIContent("My Number"));
                                                                                                              EditorGUILayout
                                                                                                                  .PropertyField(settings
                                                                                                                                     .FindProperty("m_SomeString"),
                                                                                                                                 new
                                                                                                                                     GUIContent("My String"));
                                                                                                            },

                                                                                                      // Populate the search keywords to enable smart search filtering and label highlighting:
                                                                                                      keywords
                                                                                                          = new
                                                                                                              HashSet
                                                                                                              <string
                                                                                                              >(new
                                                                                                                [] {
                                                                                                                       "Number",
                                                                                                                       "Some String"
                                                                                                                   })
                                                                                                  };

      return provider;
    }
  }

  /*
// Register a SettingsProvider using UIElements for the drawing framework:
  static class MyCustomSettingsUIElementsRegister {
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider() {
      // First parameter is the path in the Settings window.
      // Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
      var provider = new SettingsProvider("Project/MyCustomUIElementsSettings", SettingsScope.Project) {
                                                                                                           label
                                                                                                               = "Custom UI Elements",
                                                                                                           // activateHandler is called when the user clicks on the Settings item in the Settings window.
                                                                                                           activateHandler
                                                                                                               = (searchContext,
                                                                                                                  rootElement) => {
                                                                                                                   var
                                                                                                                       settings
                                                                                                                           = NeodroidSettings
                                                                                                                               .GetSerializedSettings();

                                                                                                                   // rootElement is a VisualElement. If you add any children to it, the OnGUI function
                                                                                                                   // isn't called because the SettingsProvider uses the UIElements drawing framework.
                                                                                                                   rootElement
                                                                                                                       .AddStyleSheetPath("Assets/Editor/settings_ui.uss");
                                                                                                                   var
                                                                                                                       title
                                                                                                                           = new
                                                                                                                             Label() {
                                                                                                                                         text
                                                                                                                                             = "Custom UI Elements"
                                                                                                                                     };
                                                                                                                   title
                                                                                                                       .AddToClassList("title");
                                                                                                                   rootElement
                                                                                                                       .Add(title);

                                                                                                                   var
                                                                                                                       properties
                                                                                                                           = new
                                                                                                                             VisualElement() {
                                                                                                                                                 style
                                                                                                                                                     = {
                                                                                                                                                           flexDirection
                                                                                                                                                               = FlexDirection
                                                                                                                                                                   .Column
                                                                                                                                                       }
                                                                                                                                             };
                                                                                                                   properties
                                                                                                                       .AddToClassList("property-list");
                                                                                                                   rootElement
                                                                                                                       .Add(properties);

                                                                                                                   var
                                                                                                                       tf
                                                                                                                           = new
                                                                                                                             TextField() {
                                                                                                                                             value
                                                                                                                                                 = settings
                                                                                                                                                   .FindProperty("m_SomeString")
                                                                                                                                                   .stringValue
                                                                                                                                         };
                                                                                                                   tf
                                                                                                                       .AddToClassList("property-value");
                                                                                                                   properties
                                                                                                                       .Add(tf);
                                                                                                                 },

                                                                                                           // Populate the search keywords to enable smart search filtering and label highlighting:
                                                                                                           keywords
                                                                                                               = new
                                                                                                                   HashSet
                                                                                                                   <string
                                                                                                                   >(new
                                                                                                                     [] {
                                                                                                                            "Number",
                                                                                                                            "Some String"
                                                                                                                        })
                                                                                                       };

      return provider;
    }
  }
  */

// Create MyCustomSettingsProvider by deriving from SettingsProvider:
  class NeodroidSettingsProvider : SettingsProvider {
    SerializedObject _m_custom_settings;

    public NeodroidSettingsProvider(string path, SettingsScope scope = SettingsScope.User) :
        base(path, scope) { }

    public static bool IsSettingsAvailable() {
      return File.Exists(NeodroidEditorConstants._MyCustomSettingsPath);
    }

    public void OnActivate(string search_context, VisualElement root_element) {
      // This function is called when the user clicks on the MyCustom element in the Settings window.
      this._m_custom_settings = NeodroidSettings.GetSerializedSettings();
    }

    public override void OnGUI(string search_context) {
      // Use IMGUI to display UI:
      EditorGUILayout.PropertyField(this._m_custom_settings.FindProperty("m_Number"), Styles._Number);
      EditorGUILayout.PropertyField(this._m_custom_settings.FindProperty("m_SomeString"), Styles._SomeString);
    }

    // Register the SettingsProvider
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider() {
      if (IsSettingsAvailable()) {
        var provider =
            new NeodroidSettingsProvider("Project/NeodroidSettingsProvider", SettingsScope.Project) {
                                                                                                        keywords
                                                                                                            = GetSearchKeywordsFromGUIContentProperties
                                                                                                            <Styles
                                                                                                            >()
                                                                                                    };

        // Automatically extract all keywords from the Styles.
        return provider;
      }

      // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
      return null;
    }

    class Styles {
      public static GUIContent _Number = new GUIContent("My Number");
      public static GUIContent _SomeString = new GUIContent("Some string");
    }
  }
}
