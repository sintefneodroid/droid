namespace droid.Editor {
  /// <summary>
  ///
  /// </summary>
  public static class NeodroidEditorConstants {
    /// <summary>
    ///
    /// </summary>
    public const string _NeodroidSettingsPath = "Assets/droid/Editor/neodroid_settings.asset";

    /// <summary>
    ///
    /// </summary>
    public const string _Neodroid_Project_Settings_Menu_Path = "Project/Neodroid Settings";

    /// <summary>
    ///
    /// </summary>
    public const string _Default_Scene_Previews_Location = "ScenePreviews/";

    /// <summary>
    ///
    /// </summary>
    public const string _Default_Scene_Description_Location = "SceneDescriptions/";

    #if NEODROID_IS_PACKAGE
        /// <summary>
    ///
    /// </summary>
    //public const string _Default_Import_Location = "Packages/com.neodroid.droid/";
    public const string _Default_Import_Location = "Assets/droid/";

    #else
    /// <summary>
    ///
    /// </summary>
    public const string _Default_Import_Location = "Assets/droid/";
    #endif

    /// <summary>
    ///
    /// </summary>
    public const string _Debug_Pref_Key = "NeodroidEnableDebug";

    /// <summary>
    ///
    /// </summary>
    public const string _Github_Extension_Pref_Key = "NeodroidGithubExtension";

    /// <summary>
    ///
    /// </summary>
    public const string _IsPackage_Pref_Key = "NeodroidIsPackage";

    /// <summary>
    ///
    /// </summary>
    public const string _Import_Location_Pref_Key = "NeodroidImportLocation";

    /// <summary>
    ///
    /// </summary>
    public const string _Generate_Previews_Pref_Key = "NeodroidGeneratePreviews";

    /// <summary>
    ///
    /// </summary>
    public const string _Generate_Previews_Loc_Pref_Key = "NeodroidPreviewsLocation";

    /// <summary>
    ///
    /// </summary>
    public const string _Generate_Descriptions_Pref_Key = "NeodroidGenerateDescriptions";

    /// <summary>
    ///
    /// </summary>
    public const string _Generate_Descriptions_Loc_Pref_Key = "NeodroidDescriptionLocation";

    /// <summary>
    ///
    /// </summary>
    public const float _Editor_Margin = 50;

    /// <summary>
    ///
    /// </summary>
    public const float _Preview_Margin = 5;
  }
}
