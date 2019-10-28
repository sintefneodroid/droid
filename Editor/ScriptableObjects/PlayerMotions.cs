using droid.Editor.Windows;
using droid.Runtime.Prototyping.Actuators;
using droid.Runtime.ScriptableObjects;
using UnityEngine;
#if UNITY_EDITOR
using droid.Runtime.Prototyping.Actors;
using UnityEditor;

namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// 
  /// </summary>
  public static class CreatePlayerMotions {
    /// <summary>
    ///
    /// </summary>
    [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = ScriptableObject.CreateInstance<PlayerMotions>();

      AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "NewPlayerMotions.asset");
      AssetDatabase.SaveAssets();

      EditorUtility.FocusProjectWindow();

      Selection.activeObject = asset;
    }

    /// <summary>
    ///
    /// </summary>
    public class CreatePlayerMotionsWizard : ScriptableWizard {
      const float WINDOW_WIDTH = 260, WINDOW_HEIGHT = 500;

      [MenuItem(EditorScriptableObjectMenuPath._ScriptableObjectMenuPath + "PlayerMotions (Wizard)")]
      private static void Init() {
        var window = CreateWindow<CreatePlayerMotionsWizard>("Create Player Motions...");
        window.Show();
      }

      private void Awake() {
        var icon =
            AssetDatabase.LoadAssetAtPath<Texture2D>(NeodroidSettings.Current.NeodroidImportLocationProp
                                                     + "Gizmos/Icons/table.png");
        this.minSize = this.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        this.titleContent = new GUIContent(this.titleContent.text, icon);
      }

      /// <summary>
      ///
      /// </summary>
      [Header("Actuators to generate motions for")]
      public Actor[] actors;

      private void OnWizardCreate() {
        var asset = CreateInstance<PlayerMotions>();
        int motionCount = 0;

        foreach (var actor in this.actors) {
          foreach (var actuator in actor.Actuators) {
            motionCount += ((Actuator)actuator.Value).InnerMotionNames.Length;
          }
        }

        asset._Motions = new PlayerMotion[motionCount];
        int i = 0;
        foreach (var actor in this.actors) {
          foreach (var actuator in actor.Actuators) {
            for (int j = 0; j < ((Actuator)actuator.Value).InnerMotionNames.Length; j++, i++) {
              asset._Motions[i] = new PlayerMotion {
                                                         _Actor = actor.Identifier,
                                                         _Actuator = ((Actuator)actuator.Value)
                                                             .InnerMotionNames[j]
                                                     };
            }
          }
        }

        AssetDatabase.CreateAsset(asset, EditorWindowMenuPath._NewAssetPath + "NewPlayerMotions.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
      }
    }
  }
}
#endif
