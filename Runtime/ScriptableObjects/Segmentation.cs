using droid.Runtime.Structs;
using UnityEngine;

namespace droid.Runtime.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [CreateAssetMenu(fileName = "Segmentation",
      menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath + "Segmentation",
      order = 1)]
  public class Segmentation : ScriptableObject {
    /// <summary>
    /// </summary>
    public ColorByCategory[] _color_by_categories;
  }
}
