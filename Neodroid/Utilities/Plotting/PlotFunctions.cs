using System.Collections.Generic;
using Neodroid.Prototyping.Displayers;
using UnityEngine;

namespace Neodroid.Utilities.Plotting {
  public static class PlotFunctions {
    public static Structs.Points.ValuePoint[] SampleRandomSeries(float size) {
      var poin = new List<Structs.Points.ValuePoint>();
      for (var j = 0; j < 100; j++) {
        var point = new Vector3(j, Random.Range(0, 10), 0);
        var vp = new Structs.Points.ValuePoint(point, j / 100f, size);
        poin.Add(vp);
      }

      var points = poin.ToArray();
      return points;
    }
  }
}
