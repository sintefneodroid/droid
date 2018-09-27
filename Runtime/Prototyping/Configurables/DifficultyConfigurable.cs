using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Difficulty"
      + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : Configurable {
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "DifficultyConfigurable"; } }
  }
}
