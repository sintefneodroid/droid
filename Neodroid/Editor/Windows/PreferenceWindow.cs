#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Neodroid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PreferenceWindow : MonoBehaviour {
    static bool _prefs_loaded = false;

    /// <summary>
    ///
    /// </summary>
    public static bool _EnableNeodroidDebug = false;

    const string _debug_pref_key = "EnableNeodroidDebug";

    /// <summary>
    ///
    /// </summary>
    [PreferenceItem("Neodroid")]
    public static void PreferencesGui() {
      if (!_prefs_loaded) {
        _EnableNeodroidDebug = EditorPrefs.GetBool(_debug_pref_key, false);
        _prefs_loaded = true;
      }

      _EnableNeodroidDebug = EditorGUILayout.Toggle(_debug_pref_key, _EnableNeodroidDebug);

      EditorGUILayout.HelpBox("Great!",MessageType.Info);



      if (GUI.changed) {
        
        if (_EnableNeodroidDebug) {
          DefineSymbolsFunctionality.AddDebugDefineSymbol();
        } else {
          DefineSymbolsFunctionality.RemoveDebugDefineSymbol();
        }
        
        EditorPrefs.SetBool(_debug_pref_key, _EnableNeodroidDebug);
      }
    }
  }
  
 
  /// <inheritdoc />
  /// <summary>
  /// Adds the given define symbols to PlayerSettings define symbols.
  /// Just add your own define symbols to the Symbols property at the below.
  /// </summary>
  [InitializeOnLoad]
  public class DefineSymbolsController : UnityEditor.Editor
  {


    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    static DefineSymbolsController() {
      DefineSymbolsFunctionality.AddDefineSymbols();
      
    }
    


  }

  public static class DefineSymbolsFunctionality {
    /// <summary>
    /// Symbols that will be added to the editor
    /// </summary>
    public static readonly string [] _Symbols = {
        "NEODROID",
        "NEODROID_EXISTS"
    };

    /// <summary>
    ///  Debug symbols that will be added to the editor
    /// </summary>
    public static readonly string [] _Debug_Symbols = {"NEODROID_DEBUG"};
  

    /// <summary>
    /// 
    /// </summary>
    public static void  AddDefineSymbols (){
      var defines_string = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
      var all_defines = defines_string.Split ( ';' ).ToList ();
      all_defines.AddRange ( _Symbols.Except ( all_defines ) );
      PlayerSettings.SetScriptingDefineSymbolsForGroup (
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join ( ";", all_defines.ToArray () ) );
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static void  AddDebugDefineSymbol () {
      var defines_string = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
      var all_defines = defines_string.Split ( ';' ).ToList ();
      all_defines.AddRange ( _Debug_Symbols.Except ( all_defines ) );
      
      Debug.LogWarning($"Debug enabled: {true}");
      
      PlayerSettings.SetScriptingDefineSymbolsForGroup (
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join ( ";", all_defines.ToArray () ) );
    }

    /// <summary>
    /// 
    /// </summary>
    public static void RemoveDebugDefineSymbol() {
      var defines_string = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
      var all_defines = defines_string.Split ( ';' ).ToList ();
      foreach (var b in _Debug_Symbols) {
        var res = all_defines.RemoveAll( c=> c==b );
        Debug.LogWarning($"Debug disabled: {res}");
      }

      PlayerSettings.SetScriptingDefineSymbolsForGroup (
          EditorUserBuildSettings.selectedBuildTargetGroup,
          string.Join ( ";", all_defines.ToArray () ) );
    }
  }
  
}
#endif

