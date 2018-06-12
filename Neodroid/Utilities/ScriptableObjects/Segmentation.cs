using droid.Neodroid.Utilities.Segmentation;
using UnityEngine;

namespace droid.Neodroid.Utilities.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CreateAssetMenu(
      fileName = "Segmentation",
      menuName = "Neodroid/ScriptableObjects/Segmentation",
      order = 1)]
  public class Segmentation : ScriptableObject {
    /// <summary>
    /// 
    /// </summary>
    public ColorByTag[] _Color_By_Tags;
  }
}
