using System;
using UnityEngine;

namespace Common.ReadMe.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ReadMe : ScriptableObject {
    /// <summary>
    /// </summary>
    public Texture2D _Icon;

    /// <summary>
    /// </summary>
    public bool _LoadedLayout;

    /// <summary>
    /// </summary>
    public Section[] _Sections;

    /// <summary>
    /// </summary>
    public string _Title;

    /// <summary>
    /// </summary>
    [Serializable]
    public class Section {
      /// <summary>
      /// </summary>
      public string _Heading, _Text, _LinkText, _Url;
    }
  }
}