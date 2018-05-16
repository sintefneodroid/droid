using System;
using UnityEngine;

namespace Neodroid.Utilities.Segmentation {
  [Serializable]
  public struct ColorByTag {
    public string _Tag;
    public Color _Col;
  }

  [Serializable]
  public struct ColorByInstance {
    public GameObject _Obj;
    public Color _Col;
  }
}
