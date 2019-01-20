using System;
using UnityEngine;

namespace droid.Editor.Utilities.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ReadMe : ScriptableObject {
    /// <summary>
    /// </summary>
    public Texture2D icon;

    /// <summary>
    /// </summary>
    public bool loadedLayout;

    /// <summary>
    /// </summary>
    public Section[] sections;

    /// <summary>
    /// </summary>
    public string title;

    /// <summary>
    /// </summary>
    [Serializable]
    public class Section {
      /// <summary>
      /// </summary>
      public string heading, text, linkText, url;
    }
  }
}
