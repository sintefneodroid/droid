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
                                                    new CameraObservation(this.AnimalImages[0], "A"),
                                                    new CameraObservation(this.AnimalImages[1], "B"),
                                                    new CameraObservation(this.AnimalImages[2], "C"),
                                                    new CameraObservation(this.AnimalImages[3], "D")
                                                };

      if (this.ListItemPrefab) {
        foreach (CameraObservation animal in this._camera_observations) {
          var r = Instantiate(this.ListItemPrefab, this.ContentPanel.transform, true);
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
