using UnityEngine;

namespace droid.Runtime.Sampling {
  public class UniformnessTest : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
      const int MAX_CNT = 100000;
      var total = 0f;
      var a = 0;
      var b = 0;
      var c = 0;
      for (var i = 0; i < MAX_CNT; i++) {
        var v = Random.Range(min : (float)0, max : (float)3);
        total += v;
        switch ((int)v) {
          case 0:
            a++;
            break;
          case 1:
            b++;
            break;
          case 2:
            c++;
            break;
        }
      }

      Debug.Log(message : total / MAX_CNT);
      Debug.Log(message : a);
      Debug.Log(message : b);
      Debug.Log(message : c);
    }
  }
}
