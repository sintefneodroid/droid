using System;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;

namespace Neodroid.Utilities.Messaging.Messages {
  /// <summary>
  ///
  /// </summary>
  [Serializable]
  public class Reaction {
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var motions_str = "";
      if (this.Motions != null) {
        motions_str = Enumerable.Aggregate(
            this.Motions,
            motions_str,
            (current, motion) => current + (motion + "\n"));
      }

      var configurations_str = "";
      if (this.Configurations != null) {
        configurations_str = Enumerable.Aggregate(
            this.Configurations,
            configurations_str,
            (current, configuration) => current + (configuration + "\n"));
      }

      var displayables_str = "";
      if (this.Displayables != null) {
        displayables_str = Enumerable.Aggregate(
            this.Displayables,
            displayables_str,
            (current, displayable) => current + (displayable + "\n"));
      }

      return "<Reaction>\n "
             + $"{this.RecipientEnvironment},{this.Parameters},{motions_str},{configurations_str},{this.Unobservables},{displayables_str},{this.SerialisedMessage}"
             + "\n</Reaction>";
    }

    #region Constructors

    public Reaction(
        ReactionParameters parameters,
        MotorMotion[] motions,
        Configuration[] configurations,
        Unobservables unobservables,
        Displayables.Displayable[] displayables,
        String serialised_message,
        string recipient_environment = "all") {
      this.Parameters = parameters;
      this.Motions = motions;
      this.Configurations = configurations;
      this.Unobservables = unobservables;
      this.Displayables = displayables;
      this.RecipientEnvironment = recipient_environment;
      this.SerialisedMessage = serialised_message;
    }

    /// <summary>
    ///
    /// </summary>
    public String RecipientEnvironment { get; } = "all";

    /// <summary>
    ///
    /// </summary>
    public string SerialisedMessage { get; }

    public Reaction(ReactionParameters reaction_parameters, String recipient_environment) {
      this.Parameters = reaction_parameters;
      this.RecipientEnvironment = recipient_environment;
    }

    public Reaction(ReactionParameters reaction_parameters) { this.Parameters = reaction_parameters; }

    #endregion

    #region Getters

    /// <summary>
    ///
    /// </summary>
    public Displayables.Displayable[] Displayables { get; }

    /// <summary>
    ///
    /// </summary>
    public MotorMotion[] Motions { get; }

    /// <summary>
    ///
    /// </summary>
    public Configuration[] Configurations { get; }

    /// <summary>
    ///
    /// </summary>
    public ReactionParameters Parameters { get; } = new ReactionParameters();

    /// <summary>
    ///
    /// </summary>
    public Unobservables Unobservables { get; } = new Unobservables(null, new Transform[] { });

    #endregion
  }
}
