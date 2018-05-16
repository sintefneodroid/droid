#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Neodroid.Editor.Windows {
  public class RenderTextureConfiguratorWindow : EditorWindow {
    int _preview_image_size = 100;
    Texture _icon;
    //float[] _render_texture_height;
    //float[] _render_texture_width;

    List<RenderTexture> _render_textures;

    Vector2 _scroll_position;
    Vector2 _texture_size;

    [MenuItem(EditorWindowMenuPath._WindowMenuPath + "RenderTextureConfiguratorWindow")]
    [MenuItem(EditorWindowMenuPath._ToolMenuPath + "RenderTextureConfiguratorWindow")]
    public static void ShowWindow() {
      GetWindow(
          typeof(RenderTextureConfiguratorWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    void OnEnable() {
      this._icon = (Texture2D)AssetDatabase.LoadAssetAtPath(
          "Assets/Neodroid/Gizmos/Icons/images.png",
          typeof(Texture2D));
      this.titleContent = new GUIContent("Neo:Tex", this._icon, "Window for RenderTexture configuration");
    }

    void OnGUI() {
      this._render_textures = new List<RenderTexture>();
      var cameras = FindObjectsOfType<Camera>();
      foreach (var camera in cameras) {
        if (camera.targetTexture != null) {
          this._render_textures.Add(camera.targetTexture);
        }
      }

      this._scroll_position = EditorGUILayout.BeginScrollView(this._scroll_position);
      foreach (var render_texture in this._render_textures) {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(render_texture.name);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var rect = GUILayoutUtility.GetRect(this._preview_image_size, this._preview_image_size);
        EditorGUI.DrawPreviewTexture(rect, render_texture);
        this._texture_size = new Vector2(render_texture.width, render_texture.height);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
      }

      EditorGUILayout.EndScrollView();
      this._texture_size = EditorGUILayout.Vector2Field("Set All Render Texture Sizes:", this._texture_size);
      if (GUILayout.Button("Apply(Does not work yet)")) {
        // ReSharper disable once UnusedVariable
        foreach (var render_texture in this._render_textures) {
//render_texture.width = (int)_texture_size[0]; //TODO: Read only property to change the asset, it has to be replaced with a new asset
//render_texture.height = (int)_texture_size[1]; // However it is easy to change run time genereted texture by just creating a new texure and replacing the old
        }
      }
    }

    public void OnInspectorUpdate() { this.Repaint(); }
  }
}
#endif
