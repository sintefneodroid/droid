using System;
using System.Linq;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Managers;
using Neodroid.Runtime.Utilities.ScriptableObjects;
using Neodroid.Runtime.Utilities.Segmentation;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Neodroid.Runtime.Prototyping.Observers.Camera {
  /// <inheritdoc cref="Observer" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "SegmentationCamera" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, RequireComponent(typeof(UnityEngine.Camera))]
  public class SegmentationCameraObserver : StringAugmentedCameraObserver {
    /// <summary>
    ///
    /// </summary>

    [SerializeField] Segmenter _segmenter;


    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        this._camera.Render();
        this.UpdateBytes();
      }

      this._Serialised_String = this._segmenter != null ? this._segmenter.ColorsDict
                                    .Select(c=>$"{c.Key}: {c.Value.ToString()}")
                                    .Aggregate("", (current, next) => $"{current}, {next}") : "Nothing";
      
    }
  }
}