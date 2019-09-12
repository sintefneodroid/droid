using System.Collections.Generic;
using droid.Runtime.Structs;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using UnityEngine;

namespace droid.Runtime.GameObjects.Plotting {
  public static class PlotFunctions {
    static List<Points.ValuePoint> _points = new List<Points.ValuePoint>();

    /// <summary>
    /// </summary>
    /// <param name="size"></param>
    /// <param name="min_val"></param>
    /// <param name="max_val"></param>
    /// <param name="particle_size_min"></param>
    /// <param name="particle_size_max"></param>
    /// <returns></returns>
    public static Points.ValuePoint[] SampleRandomSeries(int size,
                                                         float min_val = 0,
                                                         float max_val = 5,
                                                         float particle_size_min = 0.2f,
                                                         float particle_size_max = 1.8f) {
      var s = new SampleSpace3 {_space = Space3.MinusOneOne};
      _points.Clear();
      for (var j = 0; j < size; j++) {
        var point = s.Sample() * max_val;
        var vp = new Points.ValuePoint(point,
                                       Random.Range(min_val, max_val),
                                       Random.Range(particle_size_min, particle_size_max));
        _points.Add(vp);
      }

      var points = _points.ToArray();
      return points;
    }
  }
}
