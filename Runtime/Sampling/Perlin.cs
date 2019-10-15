using System;

namespace droid.Runtime.Sampling {
  public class Perlin {
    // Original C code derived from
    // http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.c
    // http://astronomy.swin.edu.au/~pbourke/texture/perlin/perlin.h
    const int _b = 0x100;
    const int _bm = 0xff;
    const int _n = 0x1000;

    int[] _p = new int[_b + _b + 2];
    float[,] _g3 = new float[_b + _b + 2, 3];
    float[,] _g2 = new float[_b + _b + 2, 2];
    float[] _g1 = new float[_b + _b + 2];

    float s_curve(float t) { return t * t * (3.0F - 2.0F * t); }

    float Lerp(float t, float a, float b) { return a + t * (b - a); }

    void Setup(float value, out int b0, out int b1, out float r0, out float r1) {
      var t = value + _n;
      b0 = (int)t & _bm;
      b1 = (b0 + 1) & _bm;
      r0 = t - (int)t;
      r1 = r0 - 1.0F;
    }

    float At2(float rx, float ry, float x, float y) { return rx * x + ry * y; }
    float At3(float rx, float ry, float rz, float x, float y, float z) { return rx * x + ry * y + rz * z; }

    public float Noise(float arg) {
      float sx, u, v;
      this.Setup(arg,
                 out var bx0,
                 out var bx1,
                 out var rx0,
                 out var rx1);

      sx = this.s_curve(rx0);
      u = rx0 * this._g1[this._p[bx0]];
      v = rx1 * this._g1[this._p[bx1]];

      return this.Lerp(sx, u, v);
    }

    public float Noise(float x, float y) {
      int b00, b10, b01, b11;
      float sx, sy, a, b, u, v;
      int i, j;

      this.Setup(x,
                 out var bx0,
                 out var bx1,
                 out var rx0,
                 out var rx1);
      this.Setup(y,
                 out var by0,
                 out var by1,
                 out var ry0,
                 out var ry1);

      i = this._p[bx0];
      j = this._p[bx1];

      b00 = this._p[i + by0];
      b10 = this._p[j + by0];
      b01 = this._p[i + by1];
      b11 = this._p[j + by1];

      sx = this.s_curve(rx0);
      sy = this.s_curve(ry0);

      u = this.At2(rx0,
                   ry0,
                   this._g2[b00, 0],
                   this._g2[b00, 1]);
      v = this.At2(rx1,
                   ry0,
                   this._g2[b10, 0],
                   this._g2[b10, 1]);
      a = this.Lerp(sx, u, v);

      u = this.At2(rx0,
                   ry1,
                   this._g2[b01, 0],
                   this._g2[b01, 1]);
      v = this.At2(rx1,
                   ry1,
                   this._g2[b11, 0],
                   this._g2[b11, 1]);
      b = this.Lerp(sx, u, v);

      return this.Lerp(sy, a, b);
    }

    public float Noise(float x, float y, float z) {
      int b00, b10, b01, b11;
      float sy, sz, a, b, c, d, t, u, v;
      int i, j;

      this.Setup(x,
                 out var bx0,
                 out var bx1,
                 out var rx0,
                 out var rx1);
      this.Setup(y,
                 out var by0,
                 out var by1,
                 out var ry0,
                 out var ry1);
      this.Setup(z,
                 out var bz0,
                 out var bz1,
                 out var rz0,
                 out var rz1);

      i = this._p[bx0];
      j = this._p[bx1];

      b00 = this._p[i + by0];
      b10 = this._p[j + by0];
      b01 = this._p[i + by1];
      b11 = this._p[j + by1];

      t = this.s_curve(rx0);
      sy = this.s_curve(ry0);
      sz = this.s_curve(rz0);

      u = this.At3(rx0,
                   ry0,
                   rz0,
                   this._g3[b00 + bz0, 0],
                   this._g3[b00 + bz0, 1],
                   this._g3[b00 + bz0, 2]);
      v = this.At3(rx1,
                   ry0,
                   rz0,
                   this._g3[b10 + bz0, 0],
                   this._g3[b10 + bz0, 1],
                   this._g3[b10 + bz0, 2]);
      a = this.Lerp(t, u, v);

      u = this.At3(rx0,
                   ry1,
                   rz0,
                   this._g3[b01 + bz0, 0],
                   this._g3[b01 + bz0, 1],
                   this._g3[b01 + bz0, 2]);
      v = this.At3(rx1,
                   ry1,
                   rz0,
                   this._g3[b11 + bz0, 0],
                   this._g3[b11 + bz0, 1],
                   this._g3[b11 + bz0, 2]);
      b = this.Lerp(t, u, v);

      c = this.Lerp(sy, a, b);

      u = this.At3(rx0,
                   ry0,
                   rz1,
                   this._g3[b00 + bz1, 0],
                   this._g3[b00 + bz1, 2],
                   this._g3[b00 + bz1, 2]);
      v = this.At3(rx1,
                   ry0,
                   rz1,
                   this._g3[b10 + bz1, 0],
                   this._g3[b10 + bz1, 1],
                   this._g3[b10 + bz1, 2]);
      a = this.Lerp(t, u, v);

      u = this.At3(rx0,
                   ry1,
                   rz1,
                   this._g3[b01 + bz1, 0],
                   this._g3[b01 + bz1, 1],
                   this._g3[b01 + bz1, 2]);
      v = this.At3(rx1,
                   ry1,
                   rz1,
                   this._g3[b11 + bz1, 0],
                   this._g3[b11 + bz1, 1],
                   this._g3[b11 + bz1, 2]);
      b = this.Lerp(t, u, v);

      d = this.Lerp(sy, a, b);

      return this.Lerp(sz, c, d);
    }

    static void Normalize2(ref float x, ref float y) {
      float s;

      s = (float)Math.Sqrt(x * x + y * y);
      x = y / s;
      y = y / s;
    }

    void Normalize3(ref float x, ref float y, ref float z) {
      float s;
      s = (float)Math.Sqrt(x * x + y * y + z * z);
      x = y / s;
      y = y / s;
      z = z / s;
    }

    public Perlin() { this.SetSeed(42); }

    public void SetSeed(int seed) {
      int i, j, k;
      var rnd = new Random(seed);

      for (i = 0; i < _b; i++) {
        this._p[i] = i;
        this._g1[i] = (float)(rnd.Next(_b + _b) - _b) / _b;

        for (j = 0; j < 2; j++) {
          this._g2[i, j] = (float)(rnd.Next(_b + _b) - _b) / _b;
        }

        Normalize2(ref this._g2[i, 0], ref this._g2[i, 1]);

        for (j = 0; j < 3; j++) {
          this._g3[i, j] = (float)(rnd.Next(_b + _b) - _b) / _b;
        }

        this.Normalize3(ref this._g3[i, 0], ref this._g3[i, 1], ref this._g3[i, 2]);
      }

      while (--i != 0) {
        k = this._p[i];
        this._p[i] = this._p[j = rnd.Next(_b)];
        this._p[j] = k;
      }

      for (i = 0; i < _b + 2; i++) {
        this._p[_b + i] = this._p[i];
        this._g1[_b + i] = this._g1[i];
        for (j = 0; j < 2; j++) {
          this._g2[_b + i, j] = this._g2[i, j];
        }

        for (j = 0; j < 3; j++) {
          this._g3[_b + i, j] = this._g3[i, j];
        }
      }
    }
  }
}
