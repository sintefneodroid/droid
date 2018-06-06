#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BioIK = SceneAssets.Manipulator.Excluded.BioIK.BioIK;
using Object = UnityEngine.Object;

namespace droid.Neodroid.Prototyping.Configurables.Editors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CustomEditor(typeof(IkSolverEnablerConfigurable))]
  [CanEditMultipleObjects]
  public class EnablerConfigurableEditor : Editor {
    /// <summary>
    /// 
    /// </summary>
    internal static IkSolverEnablerConfigurable _IkSolverEnablerConfigurable;

    /// <summary>
    /// 
    /// </summary>
    SerializedProperty _enablee;

    SerializedProperty _enablee_script;

    Type _stype;

    /// <summary>
    /// 
    /// </summary>
    internal void OnEnable() {
      _IkSolverEnablerConfigurable = this.target as IkSolverEnablerConfigurable;

      this._enablee = this.serializedObject.FindProperty("_Enablee");
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnInspectorGUI() {
      if (_IkSolverEnablerConfigurable == null) {
        return;
      }

      EditorGUI.BeginChangeCheck();
      {
        EditorGUILayout.ObjectField(this._enablee, typeof(BioIK), new GUIContent("Enablee"));
        /* var e = this._enablee.objectReferenceValue as MonoBehaviour;
         if (e != null) {
           
           var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default; // BindingFlags is located in System.Reflection - modify these to your liking to get the methods you're interested in
           comps = e.GetComponents<MonoBehaviour>().ToList();
           
           /*foreach (var mb in comps)
           {
             methods.AddRange(mb.GetMethods(flags));  
           }*/

        /*
        var iterator = comps.GetEnumerator();
        while (iterator.MoveNext()) {
          EditorGUILayout.ObjectField("Enablee Script",
              iterator.Current as MonoBehaviour,
              typeof(MonoBehaviour),
              true);
        }*/

        //EditorGUIUtility.ShowObjectPicker<MonoBehaviour>(this._enablee.objectReferenceValue, true, "", 0);
      }

      if (GUILayout.Button("Toggle")) {
        _IkSolverEnablerConfigurable.ActiveToggle();
      }

      if (GUILayout.Button("Activate")) {
        _IkSolverEnablerConfigurable.Activate();
      }

      if (EditorGUI.EndChangeCheck()) {
        EditorUtility.SetDirty(this.target);
        this.ApplyChanges();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    void ApplyChanges() {
      Undo.RecordObject(_IkSolverEnablerConfigurable, "Apply changes");

      foreach (var o in this.targets) {
        var s = (IkSolverEnablerConfigurable)o;
        s._Enablee = (BioIK)this._enablee.objectReferenceValue;
      }
    }
  }
}
#endif
