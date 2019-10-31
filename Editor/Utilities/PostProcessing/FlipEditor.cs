#if UNITY_POST_PROCESSING_STACK_V2
using droid.Runtime.Utilities.PostProcessesEffects;
using UnityEditor.Rendering.PostProcessing;

namespace droid.Editor.Utilities.PostProcessing {
    [PostProcessEditor(typeof(Flipper))]
    public sealed class FlipEditor : PostProcessEffectEditor<Flipper>
    {

        SerializedParameterOverride _m_flip_x;
        SerializedParameterOverride _m_flip_y;

        public override void OnEnable(){
            this._m_flip_x = this.FindParameterOverride(x => x.flip_x);
            this._m_flip_y = this.FindParameterOverride(x => x.flip_y);
        }

        public override void OnInspectorGUI(){
            this.PropertyField(this._m_flip_x);
            this.PropertyField(this._m_flip_y);
        }
    }
}
#endif
