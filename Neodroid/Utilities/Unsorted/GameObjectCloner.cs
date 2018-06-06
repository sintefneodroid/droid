using UnityEngine;

namespace droid.Neodroid.Utilities.Unsorted {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class GameObjectCloner : MonoBehaviour {
    [SerializeField] GameObject _prefab;
    [SerializeField] [Range(0, 99)] int _num_clones;
    [SerializeField] Vector3 _initial_offset = new Vector3(20, 0);
    [SerializeField] Vector3 _offset = new Vector3(20, 0, 20);

    [SerializeField] GameObject[] _clones;

    void Start() { this.InstanciateClones(); }

    void InstanciateClones() {
      if (this._clones.Length > 0) {
        this.ClearClones();
      }

      var eid = 0;
      this._clones = new GameObject[this._num_clones];
      if (this._prefab) {
        var clone_coords = NeodroidUtilities.SnakeSpaceFillingGenerator(this._num_clones);
        foreach (var clone_coord in clone_coords) {
          var go = Instantiate(
              this._prefab,
              this._initial_offset + Vector3.Scale(this._offset, clone_coord),
              Quaternion.identity,
              this.transform);
          go.name = $"{go.name}{eid}";
          this._clones[eid] = go;
          eid++;
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
        this.InstanciateClones();
      }
    }
  }
}
