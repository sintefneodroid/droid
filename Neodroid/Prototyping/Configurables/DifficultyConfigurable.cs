using System;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Difficulty"
      + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : ConfigurableGameObject {
    public override void ApplyConfiguration(Utilities.Messaging.Messages.Configuration configuration) {
      if (Math.Abs(configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }

    public override Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator) {
      throw new NotImplementedException();
    }
  }
}
