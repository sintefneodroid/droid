using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static partial class NeodroidSceneUtilities {
    /// <summary>
    /// Find UnityEngine.Object assignables from Generic Type T, this allows for FindObjectOfType with interfaces.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static T[] FindObjectsOfType<T>() {
      if (FindAllObjectsOfTypeInScene<T>() is T[] obj) {
        return obj;
      }

      throw new ArgumentException($"Found no UnityEngine.Object assignables from type {typeof(T).Name}");
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindObjectOfType<T>() { return FindObjectsOfType<T>()[0]; }

    /// <summary>
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static GameObject[] FindAllGameObjectsExceptLayer(int layer) {
      var goa = Object.FindObjectsOfType<GameObject>();
      var game_objects = new List<GameObject>();
      foreach (var go in goa) {
        if (go.layer != layer) {
          game_objects.Add(go);
        }
      }

      return game_objects.ToArray();
    }

    /// <summary>
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public static T RecursiveFirstSelfSiblingParentGetComponent<T>(Transform child) where T : Component {
      var a = child.GetComponent<T>();
      if (a != null) {
        return a;
      }

      if (child.parent) {
        foreach (Transform go in child.parent) {
          a = go.GetComponent<T>();
          if (a != null) {
            return a;
          }
        }

        a = child.parent.GetComponent<T>();
        if (a != null) {
          return a;
        }

        return RecursiveFirstSelfSiblingParentGetComponent<T>(child.parent);
      }

      return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static GameObject[] RecursiveChildGameObjectsExceptLayer(Transform parent, int layer) {
      var game_objects = new List<GameObject>();
      foreach (Transform go in parent) {
        if (go) {
          if (go.gameObject.layer != layer) {
            game_objects.Add(go.gameObject);
            var children = RecursiveChildGameObjectsExceptLayer(go, layer);
            if (children != null && children.Length > 0) {
              game_objects.AddRange(children);
            }
          }
        }
      }

      return game_objects.ToArray();
    }

    /// Use this method to get all loaded objects of some type, including inactive objects.
    /// This is an alternative to Resources.FindObjectsOfTypeAll (returns project assets, including prefabs), and GameObject.FindObjectsOfTypeAll (deprecated).
    public static T[] FindAllObjectsOfTypeInScene<T>() {
      //(Scene scene) {
      var results = new List<T>();
      for (var i = 0; i < SceneManager.sceneCount; i++) {
        var s = SceneManager.GetSceneAt(i); // maybe EditorSceneManager
        if (!s.isLoaded) {
          continue;
        }

        var all_game_objects = s.GetRootGameObjects();
        foreach (var go in all_game_objects) {
          results.AddRange(go.GetComponentsInChildren<T>(true));
        }
      }

      return results.ToArray();
    }
  }
}
