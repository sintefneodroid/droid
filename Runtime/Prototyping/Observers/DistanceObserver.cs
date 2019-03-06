using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Evaluation;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers
{
    public class DistanceObserver : Observer, IHasSingle
    {
        [SerializeField] UnityEngine.Transform t1;
        [SerializeField] UnityEngine.Transform t2;
        [SerializeField] float _observationValue;


        public override IEnumerable<float> FloatEnumerable { get{return new[] {this.ObservationValue };} }

        public override void UpdateObservation()
        {
            this.ObservationValue = Vector3.Distance(this.t1.position, this.t2.position);

        }


        public float ObservationValue
        {
            get => this._observationValue;
            private set => this._observationValue = value;
        }

        public ValueSpace SingleSpace { get; }
    }
}
