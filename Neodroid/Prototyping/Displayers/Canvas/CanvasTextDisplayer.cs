using System;
using System.Globalization;
using droid.Neodroid.Utilities.Structs;
using UnityEngine;
using UnityEngine.UI;

namespace droid.Neodroid.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode, RequireComponent(typeof(Text)),
   AddComponentMenu(
       DisplayerComponentMenuPath._ComponentMenuPath
       + "Canvas/CanvasText"
       + DisplayerComponentMenuPath._Postfix)]
  public class CanvasTextDisplayer : Displayer {
    Text _text_component;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() { this._text_component = this.GetComponent<Text>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      if(this._text_component) {
        this._text_component.text = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Double value) { if(this._text_component) {
        this._text_component.text = value.ToString(CultureInfo.InvariantCulture);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) { if(this._text_component) {
        this._text_component.text = values.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(String value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + value + " To " + this.name);
      }
      #endif

      this.SetText(value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3 value) { if(this._text_component) {
        this._text_component.text = value.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Vector3[] value) { if(this._text_component) {
        this._text_component.text = value.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint points) { if(this._text_component) {
        this._text_component.text = points.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.ValuePoint[] points) { if(this._text_component) {
        this._text_component.text = points.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint point) { if(this._text_component) {
        this._text_component.text = point.ToString();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(Points.StringPoint[] points) { if(this._text_component) {
        this._text_component.text = points.ToString();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text) { if(this._text_component) {
        this._text_component.text = text;
      }
    }
  }
}
