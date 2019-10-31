
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && USE_GITHUB_EXTENSION
using System.Linq;
using UnityEngine.UIElements;
//using UnityEditor.PackageManager.UI;

namespace droid.Editor.Utilities.Git {
  /// <inheritdoc cref="IPackageManagerExtension" />
  /// <summary>
  /// </summary>
  [InitializeOnLoad]
  class GithubExtensionUserInterface : VisualElement {
    //IPackageManagerExtension {
    //################################
    // Constant or Static Members.
    //################################

    static readonly string _resources_path =
        NeodroidSettings.Current.NeodroidImportLocationProp + "Editor/Resources/";

    static readonly string _template_path = _resources_path + "GithubExtension.uxml";
    static readonly string _style_path = _resources_path + "GithubExtension.uss";

    static GithubExtensionUserInterface() {
      //PackageManagerExtensions.RegisterExtension(new GithubExtensionUserInterface());
    }

    //################################
    // Public Members.
    //################################
    /// <summary>
    /// Creates the extension UI visual element.
    /// </summary>
    /// <returns>A visual element that represents the UI or null if none</returns>
    public VisualElement CreateExtensionUi() {
      this._initialized = false;
      return this;
    }

    /// <summary>
    /// Called by the Package Manager UI when a package is added or updated.
    /// </summary>
    /// <param name="package_info">The package information</param>
    public void OnPackageAddedOrUpdated(PackageInfo package_info) { this._detail_controls?.SetEnabled(true); }

    /// <summary>
    /// Called by the Package Manager UI when a package is removed.
    /// </summary>
    /// <param name="package_info">The package information</param>
    public void OnPackageRemoved(PackageInfo package_info) { this._detail_controls?.SetEnabled(true); }

    /// <summary>
    /// Called by the Package Manager UI when the package selection changed.
    /// </summary>
    /// <param name="package_info">The newly selected package information (can be null)</param>
    public void OnPackageSelectionChange(PackageInfo package_info) {
      this.InitializeUi();
      if (!this._initialized || package_info == null || this._package_info == package_info) {
        return;
      }

      this._package_info = package_info;

      var is_git = package_info.source == PackageSource.Git;

      GithubExtension.SetElementDisplay(this._git_detail_actoins, is_git);
      GithubExtension.SetElementDisplay(this._original_detail_actions, !is_git);
      GithubExtension.SetElementDisplay(this._detail_controls.Q("", "popupField"), !is_git);
      GithubExtension.SetElementDisplay(this._update_button, is_git);
      GithubExtension.SetElementDisplay(this._version_popup, is_git);

      if (is_git) {
        GithubExtension.RequestTags(this._package_info.packageId, this._tags);
        GithubExtension.RequestBranches(this._package_info.packageId, this._branches);

        this.SetVersion(this._package_info.version);

        var combo_element = this._detail_controls.Q("updateCombo");
        var remove_element = this._detail_controls.Q("remove");
        EditorApplication.delayCall += () => {
                                         if (combo_element != null) {
                                           GithubExtension.SetElementDisplay(combo_element, true);
                                         }

                                         if (remove_element != null) {
                                           GithubExtension.SetElementDisplay(remove_element, true);
                                           remove_element.SetEnabled(true);
                                         }
                                       };
      }

      GithubExtension.SetElementClass(this.HostingIcon, "github", true);
      GithubExtension.SetElementClass(this.HostingIcon, "dark", EditorGUIUtility.isProSkin);
    }

    //################################
    // Private Members.
    //################################
    bool _initialized;
    PackageInfo _package_info;
    Button HostingIcon { get { return this._git_detail_actoins.Q<Button>("hostingIcon"); } }
    Button ViewDocumentation { get { return this._git_detail_actoins.Q<Button>("viewDocumentation"); } }
    Button ViewChangelog { get { return this._git_detail_actoins.Q<Button>("viewChangelog"); } }
    Button ViewLicense { get { return this._git_detail_actoins.Q<Button>("viewLicense"); } }
    VisualElement _detail_controls;
    VisualElement _documentation_container;
    VisualElement _original_detail_actions;
    VisualElement _git_detail_actoins;
    Button _version_popup;
    Button _update_button;
    List<string> _tags = new List<string>();
    List<string> _branches = new List<string>();

    /// <summary>
    /// Initializes UI.
    /// </summary>
    void InitializeUi() {
      if (this._initialized) {
        return;
      }

      var package_manager_element = this.parent?.parent;
      if (package_manager_element == null) {
        return;
      }

      this._detail_controls = package_manager_element
                              .parent?.parent?.parent?.Q("packageToolBar")?.Q("toolbarContainer")
                              ?.Q("rightItems");
      if (this._detail_controls == null) {
        return;
      }

      var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_template_path);
      if (!asset) {
        Debug.Log($"Asset {_template_path} was not found");
        return;
      }

      this._git_detail_actoins = asset.CloneTree().Q("detailActions");
      this._git_detail_actoins.styleSheets.Add(EditorGUIUtility.Load(_style_path) as StyleSheet);

      // Add callbacks
      this.HostingIcon.clickable.clicked +=
          () => Application.OpenURL(GithubExtension.GetRepoUrl(this._package_info));
      this.ViewDocumentation.clickable.clicked +=
          () => Application.OpenURL(GithubExtension.GetFileUrl(this._package_info, "README.md"));
      this.ViewChangelog.clickable.clicked +=
          () => Application.OpenURL(GithubExtension.GetFileUrl(this._package_info, "CHANGELOG.md"));
      this.ViewLicense.clickable.clicked +=
          () => Application.OpenURL(GithubExtension.GetFileUrl(this._package_info, "LICENSE.md"));

      this._documentation_container = package_manager_element.Q("documentationContainer");
      this._original_detail_actions = this._documentation_container.Q("detailActions");
      this._documentation_container.Add(this._git_detail_actoins);

      this._update_button = new Button(this.AddOrUpdatePackage) {name = "update", text = "Up to date"};
      this._update_button.AddToClassList("action");
      this._version_popup = new Button(this.PopupVersions) {
                                                               text = "hoge",
                                                               style = {
                                                                           marginLeft = -4,
                                                                           marginRight = -3,
                                                                           marginTop = -3,
                                                                           marginBottom = -3,
                                                                       },
                                                           };
      this._version_popup.AddToClassList("popup");
      this._version_popup.AddToClassList("popupField");
      this._version_popup.AddToClassList("versions");

      //this._detail_controls.Q("updateCombo").Add(this._update_button);
      this._detail_controls.Insert(0, this._update_button);
//this._detail_controls.Q("updateCombo").Insert(1, this._update_button);
      //this._detail_controls.Insert(1, this._update_button);
      //this._detail_controls.Q("updateDropdownContainer").Add(this._version_popup);
      this._detail_controls.Insert(0, this._version_popup);

      this._initialized = true;
    }

    public static string GetVersionText(string version, string current = null) {
      return current == null || current != version ? version : version + " - current";
    }

    void PopupVersions() {
      var menu = new GenericMenu();
      var current = this._package_info.version;

      menu.AddItem(new GUIContent(current + " - current"),
                   this._version_popup.text == current,
                   this.SetVersion,
                   current);

      foreach (var t in this._tags.OrderByDescending(x => x)) {
        var tag = t;
        var text = new GUIContent("All Tags/" + (current == tag ? tag + " - current" : tag));
        menu.AddItem(text,
                     this._version_popup.text == tag,
                     this.SetVersion,
                     tag);
      }

      menu.AddItem(new GUIContent("All Branches/(default)"),
                   false,
                   this.SetVersion,
                   "(default)");
      foreach (var t in this._branches.OrderBy(x => x)) {
        var tag = t;
        var text = new GUIContent("All Branches/" + (current == tag ? tag + " - current" : tag));
        menu.AddItem(text,
                     this._version_popup.text == tag,
                     this.SetVersion,
                     tag);
      }

      menu.DropDown(new Rect(this._version_popup.LocalToWorld(new Vector2(0, 10)), Vector2.zero));
    }

    void SetVersion(object version) {
      var ver = version as string;
      this._version_popup.text = ver;
      var same_ver = this._package_info.version != ver;
      this._update_button.SetEnabled(same_ver);
      if (!same_ver) {
        this._update_button.text = "Update";
      }

      //this._update_button.SetEnabled(true);
    }

    void AddOrUpdatePackage() {
      var target = this._version_popup.text != "(default)" ? this._version_popup.text : "";
      var id = GithubExtension.GetSpecificPackageId(this._package_info.packageId, target);
      Client.Remove(this._package_info.name);
      Client.Add(id);
    }
  }
}
#endif
