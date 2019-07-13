using droid.Runtime.Structs.Vectors;
using UnityEngine;

namespace droid.Runtime.Utilities.Grid {
  public abstract class GridCell : MonoBehaviour {
    protected Collider _Col;
    protected Renderer _Rend;
    public IntVector3 GridCoordinates { get; set; }

    public abstract void Setup(string name, Material mat);
  }

  public class EmptyCell : GridCell {
    public override void Setup(string n, Material mat) {
      this._Rend = this.GetComponent<Renderer>();
      this._Col = this.GetComponent<Collider>();
      this.name = n;
      this._Col.isTrigger = true;
      this._Rend.enabled = false;

      //Destroy (this.GetComponent<Renderer> ());
      //this.GetComponent<Renderer>().material = mat;
    }

    public void SetAsGoal(string n, Material mat) {
      this.name = n;
      this._Rend.enabled = true;
      this._Rend.material = mat;
      this.tag = "Goal";
    }
  }

  public class FilledCell : GridCell {
    public override void Setup(string n, Material mat) {
      this.name = n;
      this.GetComponent<Collider>().isTrigger = false;
      this.GetComponent<Renderer>().material = mat;
      this.tag = "Obstruction";
    }
  }

  public class GoalCell : EmptyCell {
    public override void Setup(string n, Material mat) {
      this.name = n;
      this.GetComponent<Collider>().isTrigger = true;
      this.GetComponent<Renderer>().material = mat;
      this.tag = "Goal";
    }
  }
}
