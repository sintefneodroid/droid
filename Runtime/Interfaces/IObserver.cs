using System.Collections.Generic;

namespace Neodroid.Runtime.Interfaces {
  public interface IObserver : IRegisterable {
    void UpdateObservation();
    IEnumerable<float> FloatEnumerable { get; set; }
    void EnvironmentReset();
  }
}