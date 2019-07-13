using UnityEngine;

namespace droid.Runtime.Utilities.Procedural {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class GameObjectCloner : MonoBehaviour {
    [SerializeField] GameObject[] _clones = null;
    [SerializeField] Vector3 _initial_offset = new Vector3(0, 0, 0);
    [SerializeField] [Range(0, 99)] int _num_clones = 0;
    [SerializeField] Vector3 _offset = new Vector3(20, 0, 20);
    [SerializeField] GameObject _prefab = null;

    void Start() { this.InstantiateClones(); }

    void InstantiateClones() {
      if (this._clones.Length > 0) {
        this.ClearClones();
      }

      var clone_id = 0;
      this._clones = new GameObject[this._num_clones];
      if (this._prefab) {
        var clone_coords = NeodroidUtilities.SnakeSpaceFillingGenerator(this._num_clones);
        foreach (var c in clone_coords) {
          var go = Instantiate(this._prefab,
                               this._initial_offset + Vector3.Scale(this._offset, c),
                               Quaternion.identity,
                               this.transform);
          go.name = $"{go.name}{clone_id}";
          this._clones[clone_id] = go;
          clone_id++;
        }
      }
    }

    void ClearClones() {
      foreach (var clone in this._clones) {
        Destroy(clone);
      }
    }

    void Update() {
      if (this._num_clones != this._clones.Length) {
        this.InstantiateClones();
      }
    }
  }
}
