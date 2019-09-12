using UnityEngine;

namespace droid.Runtime.Structs {
  public class OutPose {
    /// Encapsulates a rotation and a translation.  This is a convenience class that allows
    /// construction and value access either by Matrix4x4 or Quaternion + Vector3 types.
    /// Right-handed to left-handed matrix converter (and vice versa).
    protected static readonly Matrix4x4 _FlipZ = Matrix4x4.Scale(new Vector3(1, 1, -1));

    /// The translation component of the pose.
    public Vector3 Position { get; protected set; }

    /// The rotation component of the pose.
    public Quaternion Orientation { get; protected set; }

    /// The pose as a matrix in Unity gameobject convention (left-handed).
    public Matrix4x4 Matrix { get; protected set; }

    /// The pose as a matrix in right-handed coordinates.
    public Matrix4x4 RightHandedMatrix { get { return _FlipZ * this.Matrix * _FlipZ; } }

    /// Default constructor.
    /// Initializes position to the origin and orientation to the identity rotation.
    public OutPose() {
      this.Position = Vector3.zero;
      this.Orientation = Quaternion.identity;
      this.Matrix = Matrix4x4.identity;
    }

    /// Constructor that takes a Vector3 and a Quaternion.
    public OutPose(Vector3 position, Quaternion orientation) { this.Set(position, orientation); }

    /// Constructor that takes a Matrix4x4.
    public OutPose(Matrix4x4 matrix) { this.Set(matrix); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    /// <param name="orientation"></param>
    protected void Set(Vector3 position, Quaternion orientation) {
      this.Position = position;
      this.Orientation = orientation;
      this.Matrix = Matrix4x4.TRS(position, orientation, Vector3.one);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matrix"></param>
    protected void Set(Matrix4x4 matrix) {
      this.Matrix = matrix;
      this.Position = matrix.GetColumn(3);
      this.Orientation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
    }
  }

  /// <summary>
  ///
  /// </summary>
  public class MutableOutPose : OutPose {
    /// Sets the position and orientation from a Vector3 + Quaternion.
    public new void Set(Vector3 position, Quaternion orientation) { base.Set(position, orientation); }

    /// Sets the position and orientation from a Matrix4x4.
    public new void Set(Matrix4x4 matrix) { base.Set(matrix); }

    /// Sets the position and orientation from a right-handed Matrix4x4.
    public void SetRightHanded(Matrix4x4 matrix) { this.Set(_FlipZ * matrix * _FlipZ); }
  }
}
