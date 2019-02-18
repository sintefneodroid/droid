using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeodroidImageSynthesis {
	[RequireComponent (typeof(ImageSynthesis))]
	public class ButtonOverlay : MonoBehaviour {

		public int width = 320;
		public int height = 200;
		int _image_counter = 1;

		void OnGUI ()
		{
			if (GUILayout.Button($"Capture ({this._image_counter})"))
			{
				var scene_name = SceneManager.GetActiveScene().name;
				this.GetComponent<ImageSynthesis>().Save(scene_name + "_" + this._image_counter++, this.width, this.height);
			}
		}
	}
}
