using System.Collections.Generic;
using droid.Neodroid.Utilities.Structs;
using UnityEngine;

namespace droid.Neodroid.Utilities.Plotting {
  public static class PlotFunctions {
    
    static List<Points.ValuePoint> _points = new List<Points.ValuePoint>();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static Points.ValuePoint[] SampleRandomSeries(float size) {
      _points.Clear();
      for (var j = 0; j < 100; j++) {
        var point = new Vector3(j, Random.Range(0, 10), 0);
        var vp = new Points.ValuePoint(point, j / 100f, size);
        _points.Add(vp);
      }

      var points = _points.ToArray();
      return points;
    }
  }
}
