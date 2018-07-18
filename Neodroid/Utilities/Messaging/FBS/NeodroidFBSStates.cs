// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

using System;
using FlatBuffers;

namespace Neodroid.Utilities.Messaging.FBS
{
  public struct FStates : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return this.__p.bb; } }
  public static FStates GetRootAsFStates(ByteBuffer _bb) { return GetRootAsFStates(_bb, new FStates()); }
  public static FStates GetRootAsFStates(ByteBuffer _bb, FStates obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool FStatesBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "XSTA"); }
  public void __init(int _i, ByteBuffer _bb) { this.__p.bb_pos = _i; this.__p.bb = _bb; }
  public FStates __assign(int _i, ByteBuffer _bb) { this.__init(_i, _bb); return this; }

  public FState? States(int j) { int o = this.__p.__offset(4); return o != 0 ? (FState?)(new FState()).__assign(this.__p.__indirect(this.__p.__vector(o) + j * 4), this.__p.bb) : null; }
  public int StatesLength { get { int o = this.__p.__offset(4); return o != 0 ? this.__p.__vector_len(o) : 0; } }
  public FState? StatesByKey(string key) { int o = this.__p.__offset(4); return o != 0 ? FState.__lookup_by_key(this.__p.__vector(o), key, this.__p.bb) : null; }
  public string ApiVersion { get { int o = this.__p.__offset(6); return o != 0 ? this.__p.__string(o + this.__p.bb_pos) : null; } }
  public ArraySegment<byte>? GetApiVersionBytes() { return this.__p.__vector_as_arraysegment(6); }
  public FSimulatorConfiguration? SimulatorConfiguration { get { int o = this.__p.__offset(8); return o != 0 ? (FSimulatorConfiguration?)(new FSimulatorConfiguration()).__assign(o + this.__p.bb_pos, this.__p.bb) : null; } }

  public static void StartFStates(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddStates(FlatBufferBuilder builder, VectorOffset statesOffset) { builder.AddOffset(0, statesOffset.Value, 0); }
  public static VectorOffset CreateStatesVector(FlatBufferBuilder builder, Offset<FState>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartStatesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddApiVersion(FlatBufferBuilder builder, StringOffset apiVersionOffset) { builder.AddOffset(1, apiVersionOffset.Value, 0); }
  public static void AddSimulatorConfiguration(FlatBufferBuilder builder, Offset<FSimulatorConfiguration> simulatorConfigurationOffset) { builder.AddStruct(2, simulatorConfigurationOffset.Value, 0); }
  public static Offset<FStates> EndFStates(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<FStates>(o);
  }
  public static void FinishFStatesBuffer(FlatBufferBuilder builder, Offset<FStates> offset) { builder.Finish(offset.Value, "XSTA"); }
};


}
