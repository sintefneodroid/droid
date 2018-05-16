using Neodroid.Utilities.Segmentation;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "Segmentations",
      menuName = "Neodroid/ScriptableObjects/Segmentations",
      order = 1)]
  public class Segmentations : ScriptableObject {
    public ColorByTag[] _Color_By_Tags;
  }
}
