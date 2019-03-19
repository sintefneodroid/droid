using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class LineProject : MonoBehaviour
    {
        LineRenderer _lineRenderer = null;
        [SerializeField] Vector3 _direction = Vector3.down;
        [SerializeField] float _length = 30f;

        Vector3 old_pos;
        void Awake()
        {
            this._lineRenderer = this.GetComponent<LineRenderer>();
            this.Project();
        }

        void OnEnable()
        {
            this.Project();
        }

        void Update()
        {
            if(Application.isPlaying)
            {
                if (this.transform.position != this.old_pos)
                {
                    this.Project();
                }
            }
        }

        void Project()
        {
            var position = this.transform.position;
            if(Physics.Raycast(position, this._direction, out var ray, this._length)){
                this._lineRenderer.SetPositions(new []{position, ray.point});
            }
        }
    }
}
