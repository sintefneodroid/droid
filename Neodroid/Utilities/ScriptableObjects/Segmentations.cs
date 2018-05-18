using droid.Neodroid.Utilities.Segmentation;
using UnityEngine;

namespace droid.Neodroid.Utilities.ScriptableObjects {
  [CreateAssetMenu(
      fileName = "Segmentations",
      menuName = "Neodroid/ScriptableObjects/Segmentations",
      order = 1)]
  public class Segmentations : ScriptableObject {
    public ColorByTag[] _Color_By_Tags;
  }
}
