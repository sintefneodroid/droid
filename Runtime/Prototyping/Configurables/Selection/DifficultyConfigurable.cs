using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Selection {
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "Difficulty"
                               + ConfigurableComponentMenuPath._Postfix)]
  public class DifficultyConfigurable : Configurable {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "DifficultyConfigurable"; } }

    public ISamplable ConfigurableValueSpace { get; }

    public override void UpdateCurrentConfiguration() { }

    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      if (Math.Abs(value : configuration.ConfigurableValue - 1) < double.Epsilon) {
        //print ("Increased Difficulty");
      } else if (Math.Abs(value : configuration.ConfigurableValue - -1) < double.Epsilon) {
        //print ("Decreased Difficulty");
      }
    }

    public override Configuration[] SampleConfigurations() {
      return new Configuration[] {
                                     new Configuration(configurable_name : this.Identifier,
                                                       configurable_value :
                                                       this.ConfigurableValueSpace.Sample())
                                 };
    }
  }
}
