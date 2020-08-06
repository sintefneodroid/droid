using System;
using System.Linq;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages.Displayables;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  [Serializable]
  public class Reaction {
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var motions_str = "";
      if (this.Motions != null) {
        motions_str =
            this.Motions.Aggregate(seed : motions_str, (current, motion) => current + (motion + "\n"));
      }

      var configurations_str = "";
      if (this.Configurations != null) {
        configurations_str = this.Configurations.Aggregate(seed : configurations_str,
                                                           (current, configuration) =>
                                                               current + (configuration + "\n"));
      }

      var displayables_str = "";
      if (this.Displayables != null) {
        displayables_str = this.Displayables.Aggregate(seed : displayables_str,
                                                       (current, displayable) =>
                                                           current + (displayable + "\n"));
      }

      return "<Reaction>\n "
             + $"{this.RecipientEnvironment},{this.Parameters},{motions_str},{configurations_str},{this.Unobservables},{displayables_str},{this.SerialisedMessage}"
             + "\n</Reaction>";
    }

    #region Constructors

    public Reaction(ReactionParameters parameters,
                    IMotion[] motions,
                    Configuration[] configurations,
                    Unobservables unobservables,
                    Displayable[] displayables,
                    string serialised_message,
                    string recipient_environment = "all",
                    string reaction_source = "somewhere") {
      this.Parameters = parameters;
      this.Motions = motions;
      this.Configurations = configurations;
      this.Unobservables = unobservables;
      this.Displayables = displayables;
      this.RecipientEnvironment = recipient_environment;
      this.SerialisedMessage = serialised_message;
      this.ReactionSource = reaction_source;
    }

    /// <summary>
    ///
    /// </summary>
    public string ReactionSource { get; set; }

    /// <summary>
    /// </summary>
    public string RecipientEnvironment { get; } = "all";

    /// <summary>
    /// </summary>
    public string SerialisedMessage { get; }

    public Reaction(ReactionParameters reaction_parameters, string recipient_environment) {
      this.Parameters = reaction_parameters;
      this.RecipientEnvironment = recipient_environment;
    }

    public Reaction(ReactionParameters reaction_parameters) { this.Parameters = reaction_parameters; }

    #endregion

    #region Getters

    /// <summary>
    /// </summary>
    public Displayable[] Displayables { get; }

    /// <summary>
    /// </summary>
    public IMotion[] Motions { get; }

    /// <summary>
    /// </summary>
    public Configuration[] Configurations { get; }

    /// <summary>
    /// </summary>
    public ReactionParameters Parameters { get; }

    /// <summary>
    /// </summary>
    public Unobservables Unobservables { get; } = new Unobservables();

    #endregion
  }
}
