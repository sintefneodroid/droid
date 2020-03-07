using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Adds the given define symbols to PlayerSettings define symbols.
  ///   Just add your own define symbols to the Symbols property at the below.
  /// </summary>
  [InitializeOnLoad]
  public class DefineSymbolsController : UnityEditor.Editor {
    /// <summary>
    ///   Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static DefineSymbolsController() { DefineSymbolsFunctionality.AddDefineSymbols(); }
  }

  /// <summary>
  ///
  /// </summary>
  public static class DefineSymbolsFunctionality {
    /// <summary>
    ///   Symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Symbols = {"NEODROID", "NEODROID_EXISTS"};

    /// <summary>
    ///   Debug symbols that will be added to the editor
    /// </summary>
    public static readonly string[] _Debug_Symbols = {"NEODROID_DEBUG"};

    /// <summary>
    ///
    /// </summary>
    public static readonly string[] _Github_Symbols = {"NEODROID_USE_GITHUB_EXTENSION"};

    /// <summary>
    ///
    /// </summary>
    public static readonly string[] _IsPackage_Symbols = {"NEODROID_IS_PACKAGE"};

    /// <summary>
    /// </summary>
    public static void AddDefineSymbols() {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup : EditorUserBuildSettings
                                                               .selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      all_defines.AddRange(collection : _Symbols.Except(second : all_defines));
      PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup :
                                                       EditorUserBuildSettings.selectedBuildTargetGroup,
                                                       defines : string.Join(";",
                                                                             value : all_defines.ToArray()));
    }

    /// <summary>
    /// </summary>
    public static void AddDebugDefineSymbol() {
      AddDefineSymbols(symbols : _Debug_Symbols);

      Debug.LogWarning("Neodroid Debugging enabled");
    }

    /// <summary>
    ///
    /// </summary>
    public static void RemoveDebugDefineSymbols() {
      RemoveDefineSymbols(symbols : _Debug_Symbols);

      Debug.LogWarning("Neodroid Debugging disabled");
    }

    /// <summary>
    ///
    /// </summary>
    public static void AddGithubDefineSymbols() {
      AddDefineSymbols(symbols : _Github_Symbols);

      Debug.LogWarning("Github Extension enabled");
    }

    /// <summary>
    ///
    /// </summary>
    public static void RemoveGithubDefineSymbols() {
      RemoveDefineSymbols(symbols : _Github_Symbols);

      Debug.LogWarning("Github Extension disabled");
    }

    /// <summary>
    ///
    /// </summary>
    public static void AddIsPackageDefineSymbols() {
      AddDefineSymbols(symbols : _IsPackage_Symbols);

      Debug.LogWarning("Neodroid is assumed to be an imported asset");
    }

    /// <summary>
    ///
    /// </summary>
    public static void RemoveIsPackageDefineSymbols() {
      RemoveDefineSymbols(symbols : _IsPackage_Symbols);
      Debug.LogWarning("Neodroid is assumed to be an installed package");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="symbols"></param>
    public static void AddDefineSymbols(string[] symbols) {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup : EditorUserBuildSettings
                                                               .selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      all_defines.AddRange(collection : symbols.Except(second : all_defines));

      PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup :
                                                       EditorUserBuildSettings.selectedBuildTargetGroup,
                                                       defines : string.Join(";",
                                                                             value : all_defines.ToArray()));
    }

    /// <summary>
    /// </summary>
    public static void RemoveDefineSymbols(string[] symbols) {
      var defines_string =
          PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup : EditorUserBuildSettings
                                                               .selectedBuildTargetGroup);
      var all_defines = defines_string.Split(';').ToList();
      foreach (var b in symbols) {
        var res = all_defines.RemoveAll(c => c == b);
        Debug.LogWarning(message :
                         $"Removed define symbols {symbols.Aggregate((aa, bb) => aa + "," + bb)} : number of entries removed {res}");
      }

      PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup :
                                                       EditorUserBuildSettings.selectedBuildTargetGroup,
                                                       defines : string.Join(";",
                                                                             value : all_defines.ToArray()));
    }
  }
}

#endif
