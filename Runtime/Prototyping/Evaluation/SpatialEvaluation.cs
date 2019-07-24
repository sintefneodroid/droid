using UnityEngine;
using System.Collections.Generic;
using droid.Runtime.GameObjects.BoundingBoxes;

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
        // TODO: Look at how to simplify a way to describe which objects should be in this list
        [SerializeField]
        protected BoundingBox boundingBox;

        protected override void PostSetup()
        {
            base.PostSetup();

            if(boundingBox == null)
                boundingBox = gameObject.GetComponent<BoundingBox>();
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
