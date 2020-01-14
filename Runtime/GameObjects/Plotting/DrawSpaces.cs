using UnityEngine;

namespace droid.Runtime.GameObjects.Plotting {
  /// <summary>
  /// </summary>
  public class DrawSpaces : MonoBehaviour {
    void OnDrawGizmos() {
      if (this.enabled) {
        var color = Color.green;
        // local up
        this.DrawHelperAtCenter(direction : this.transform.up, color : color, 2f);

        color.g -= 0.5f;
        // global up
        this.DrawHelperAtCenter(direction : Vector3.up, color : color, 1f);

        color = Color.blue;
        // local forward
        this.DrawHelperAtCenter(direction : this.transform.forward, color : color, 2f);

        color.b -= 0.5f;
        // global forward
        this.DrawHelperAtCenter(direction : Vector3.forward, color : color, 1f);

        color = Color.red;
        // local right
        this.DrawHelperAtCenter(direction : this.transform.right, color : color, 2f);

        color.r -= 0.5f;
        // global right
        this.DrawHelperAtCenter(direction : Vector3.right, color : color, 1f);
      }
    }

    void DrawHelperAtCenter(Vector3 direction, Color color, float scale) {
      Gizmos.color = color;
      var position = this.transform.position;
      var destination = position + direction * scale;
      Gizmos.DrawLine(@from : position, to : destination);
    }
  }
}
