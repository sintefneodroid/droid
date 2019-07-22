using UnityEngine;
using System.Collections.Generic;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;

namespace droid.Runtime.Prototyping.Evaluation
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class SpatialEvaluation : ObjectiveFunction
    {
        /// <summary>
        /// </summary>
        [SerializeField]
        protected List<Transform> terminatingTransforms;
        [SerializeField]
        protected BoundingBox boundingBox;

        public new virtual void PostSetup()
        {
            base.PostSetup();
            gameObject.GetComponent<BoundingBox>();
        }

        /// <summary>
        /// </summary>
        public abstract override void InternalReset();

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public abstract override float InternalEvaluate();

        
    }
}
