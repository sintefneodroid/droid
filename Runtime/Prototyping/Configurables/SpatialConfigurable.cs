
using droid.Runtime.Enums;
using droid.Runtime.Environments;
using droid.Runtime.GameObjects;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities;
using UnityEngine;


namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Spatial"
                    + ConfigurableComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public abstract class SpatialConfigurable : Configurable{
    /// <summary>
    /// </summary>
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    #region Fields


    /// <summary>
    /// </summary>
    [Header("Configurable", order = 30)]
    [SerializeField]
    bool _relative_to_existing_value = false;


    #endregion
  }
}
