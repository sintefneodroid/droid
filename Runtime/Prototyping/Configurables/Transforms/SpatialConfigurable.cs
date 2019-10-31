using droid.Runtime.Enums;
using droid.Runtime.GameObjects;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Spatial"
                    + ConfigurableComponentMenuPath._Postfix)]
  public abstract class SpatialConfigurable : Configurable {
    /// <summary>
    /// </summary>
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    #region Fields

    /// <summary>
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value = false;

    [SerializeField] protected CoordinateSpace coordinate_space = CoordinateSpace.Environment_;
    #endregion
  }
}
