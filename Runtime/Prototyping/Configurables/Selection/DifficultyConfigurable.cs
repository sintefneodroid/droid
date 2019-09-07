using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Difficulty"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : Configurable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "DifficultyConfigurable"; } }

    public override ISamplable ConfigurableValueSpace { get; }

    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}
