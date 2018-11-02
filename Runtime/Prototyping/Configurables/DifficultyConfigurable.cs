using System;
using Neodroid.Runtime.Interfaces;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Difficulty"
      + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : Configurable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "DifficultyConfigurable"; } }

    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }
  }
}