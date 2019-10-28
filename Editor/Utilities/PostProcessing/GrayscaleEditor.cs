#if UNITY_POST_PROCESSING_STACK_V2
using droid.Runtime.Utilities.PostProcessesEffects;
using UnityEditor.Rendering.PostProcessing;

namespace droid.Editor.Utilities.PostProcessing {
    [PostProcessEditor(typeof(Grayscale))]
    public sealed class GrayscaleEditor : PostProcessEffectEditor<Grayscale>
    {
        SerializedParameterOverride _m_blend;


        public override void OnEnable(){
            this._m_blend = this.FindParameterOverride(x => x.blend);

        }

        public override void OnInspectorGUI(){
            this.PropertyField(this._m_blend);
        }
    }
}
#endif
