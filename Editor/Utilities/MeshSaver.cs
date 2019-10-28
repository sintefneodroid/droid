#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace droid.Editor.Utilities {
  /// <summary>
  ///
  /// </summary>
  public static class MeshSaverEditor {
    const string _menu_path = "CONTEXT" + "/MeshFilter" + "/SaveMesh";

    /// <summary>
    ///
    /// </summary>
    /// <param name="menu_command"></param>
    [MenuItem(_menu_path)]
    public static void SaveMeshInPlace(MenuCommand menu_command) {
      var mf = menu_command.context as MeshFilter;

      if (mf != null) {
        var m = mf.sharedMesh;
        SaveMesh(m,
                 m.name,
                 false,
                 true);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="menu_command"></param>
    [MenuItem(_menu_path + "AsANewInstance")]
    public static void SaveMeshNewInstanceItem(MenuCommand menu_command) {
      var mf = menu_command.context as MeshFilter;

      if (mf != null) {
        var m = mf.sharedMesh;
        SaveMesh(m,
                 m.name,
                 true,
                 true);
      }
    }

    public static void SaveMesh(Mesh mesh, string name, bool make_new_instance, bool optimize_mesh) {
      var path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset",
                                             "Neodroid/Runtime/Meshes",
                                             name,
                                             "asset");
      Debug.Log($"Trying to save mesh to {path}");
      if (string.IsNullOrEmpty(path)) {
        return;
      }

      path = FileUtil.GetProjectRelativePath(path);

      var mesh_to_save = make_new_instance ? Object.Instantiate(mesh) : mesh;

      if (optimize_mesh) {
        MeshUtility.Optimize(mesh_to_save);
      }

      AssetDatabase.CreateAsset(mesh_to_save, path);
      AssetDatabase.SaveAssets();
    }
  }
}
#endif
