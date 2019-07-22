using UnityEngine;

namespace droid.Runtime.GameObjects.Plotting {
  /// <summary>
  /// </summary>
  public class DrawSpaces : MonoBehaviour {
    void OnDrawGizmos() {
      if (this.enabled) {
        var color = Color.green;
        // local up
        this.DrawHelperAtCenter(this.transform.up, color, 2f);

        color.g -= 0.5f;
        // global up
        this.DrawHelperAtCenter(Vector3.up, color, 1f);

        color = Color.blue;
        // local forward
        this.DrawHelperAtCenter(this.transform.forward, color, 2f);

        color.b -= 0.5f;
        // global forward
        this.DrawHelperAtCenter(Vector3.forward, color, 1f);

        color = Color.red;
        // local right
        this.DrawHelperAtCenter(this.transform.right, color, 2f);

        color.r -= 0.5f;
        // global right
        this.DrawHelperAtCenter(Vector3.right, color, 1f);
      }
    }

    void DrawHelperAtCenter(Vector3 direction, Color color, float scale) {
      Gizmos.color = color;
      var position = this.transform.position;
      var destination = position + direction * scale;
      Gizmos.DrawLine(position, destination);
    }
  }
}
