using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Runtime.GameObjects.StatusDisplayer {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class RenderTextureList : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    public Sprite[] AnimalImages;

    /// <summary>
    ///
    /// </summary>
    public GameObject ContentPanel;

    /// <summary>
    ///
    /// </summary>
    public GameObject ListItemPrefab;

    ArrayList _camera_observations;

    void Start() {
      // 1. Get the data to be displayed
      this._camera_observations = new ArrayList {
                                                    new CameraObservation(icon : this.AnimalImages[0], "A"),
                                                    new CameraObservation(icon : this.AnimalImages[1], "B"),
                                                    new CameraObservation(icon : this.AnimalImages[2], "C"),
                                                    new CameraObservation(icon : this.AnimalImages[3], "D")
                                                };

      if (this.ListItemPrefab) {
        for (var index = 0; index < this._camera_observations.Count; index++) {
          var animal = (CameraObservation)this._camera_observations[index : index];
          var r = Instantiate(original : this.ListItemPrefab, parent : this.ContentPanel.transform, true);
          var controller = r.GetComponent<RenderTextureListItem>();
          controller.Icon.sprite = animal._Icon;
          controller.Name.text = animal._Name;
          r.transform.localScale = Vector3.one;
        }
      }
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class RenderTextureListItem : MonoBehaviour {
    /// <summary>
    ///
    /// </summary>
    public Image Icon;

    /// <summary>
    ///
    /// </summary>
    public Text Name;
  }

  /// <summary>
  ///
  /// </summary>
  public class CameraObservation {
    public Sprite _Icon;
    public string _Name;

    public CameraObservation(Sprite icon, string name) {
      this._Icon = icon;
      this._Name = name;
    }
  }
}
