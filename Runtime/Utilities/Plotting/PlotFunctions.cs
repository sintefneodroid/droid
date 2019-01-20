using System.Collections.Generic;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Utilities.Plotting {
  public static class PlotFunctions {
    static List<Points.ValuePoint> _points = new List<Points.ValuePoint>();

    /// <summary>
    /// </summary>
    /// <param name="size"></param>
    /// <param name="min_val"></param>
    /// <param name="max_val"></param>
    /// <param name="particle_size"></param>
    /// <returns></returns>
    public static Points.ValuePoint[] SampleRandomSeries(
        int size,
        float min_val = 0,
        float max_val = 5,
        float particle_size = 1) {
      _points.Clear();
      for (var j = 0; j < size; j++) {
        var point = new Vector3(j, Random.Range(min_val, max_val), 0);
        var vp = new Points.ValuePoint(point, Random.Range(min_val, max_val), particle_size);
        _points.Add(vp);
      }

      var points = _points.ToArray();
      return points;
    }
  }
}
