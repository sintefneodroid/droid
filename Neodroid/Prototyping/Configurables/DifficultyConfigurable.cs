using System;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;
using Random = System.Random;

namespace droid.Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Difficulty"
      + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : ConfigurableGameObject {
    public override void ApplyConfiguration(Configuration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }

    public override Configuration SampleConfiguration(Random random_generator) {
      throw new NotImplementedException();
    }
  }
}
