using FlatBuffers;

namespace droid.Runtime.Messaging.FBS.Deprecated {
  /// <summary>
  /// </summary>
  public static class CustomFlatBufferImplementation {
    //Custom implementation of copying bytearray, faster than generated code
    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static VectorOffset CreateByteVector(FlatBufferBuilder builder, byte[] data) {
      //builder.StartVector(1, data.Length, 1);
      //var additional_bytes = data.Length - 2;
      //builder.Prep(sizeof(byte), additional_bytes * sizeof(byte));

      // for (var i = data.Length - 1; i >= 0; i--)
      //  builder.PutByte(data[i]);
      //return builder.EndVector();

      //TODO: return builder.CreateByteVector(data);

      return new VectorOffset();
    }

    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static VectorOffset CreateFloatVector(FlatBufferBuilder builder, float[] data) {
/*
       builder.StartVector(4, data.Length, 4);
      for (var i = data.Length - 1; i >= 0; i--)
      {
        builder.AddFloat(data[i]);
      }

      return builder.EndVector();
*/

      //TODO: return builder.CreateFloatVector(data); //TODO: Calculate proper lenght of vector! lenght*4
      return new VectorOffset();
    }
  }
}
