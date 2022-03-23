using System;
using UnityEngine;

namespace BallancePhysics
{
  [Serializable]
  public struct PhysicsLayerTags : IEquatable<PhysicsLayerTags>
  {
    public static PhysicsLayerTags Everything => new PhysicsLayerTags { Value = unchecked((uint)~0) };
    public static PhysicsLayerTags Nothing => new PhysicsLayerTags { Value = 0 };

    public bool Layer00;
    public bool Layer01;
    public bool Layer02;
    public bool Layer03;
    public bool Layer04;
    public bool Layer05;
    public bool Layer06;
    public bool Layer07;
    public bool Layer08;
    public bool Layer09;
    public bool Layer10;
    public bool Layer11;
    public bool Layer12;
    public bool Layer13;
    public bool Layer14;
    public bool Layer15;
    public bool Layer16;
    public bool Layer17;
    public bool Layer18;
    public bool Layer19;
    public bool Layer20;
    public bool Layer21;
    public bool Layer22;
    public bool Layer23;
    public bool Layer24;
    public bool Layer25;
    public bool Layer26;
    public bool Layer27;
    public bool Layer28;
    public bool Layer29;
    public bool Layer30;
    public bool Layer31;

    internal bool this[int i]
    {
      get
      {
        SafetyChecks.CheckInRangeAndThrow(i, new Vector2(0, 31), nameof(i));
        switch (i)
        {
          case 0: return Layer00;
          case 1: return Layer01;
          case 2: return Layer02;
          case 3: return Layer03;
          case 4: return Layer04;
          case 5: return Layer05;
          case 6: return Layer06;
          case 7: return Layer07;
          case 8: return Layer08;
          case 9: return Layer09;
          case 10: return Layer10;
          case 11: return Layer11;
          case 12: return Layer12;
          case 13: return Layer13;
          case 14: return Layer14;
          case 15: return Layer15;
          case 16: return Layer16;
          case 17: return Layer17;
          case 18: return Layer18;
          case 19: return Layer19;
          case 20: return Layer20;
          case 21: return Layer21;
          case 22: return Layer22;
          case 23: return Layer23;
          case 24: return Layer24;
          case 25: return Layer25;
          case 26: return Layer26;
          case 27: return Layer27;
          case 28: return Layer28;
          case 29: return Layer29;
          case 30: return Layer30;
          case 31: return Layer31;
          default: return default;
        }
      }
      set
      {
        SafetyChecks.CheckInRangeAndThrow(i, new Vector2(0, 31), nameof(i));
        switch (i)
        {
          case 0: Layer00 = value; break;
          case 1: Layer01 = value; break;
          case 2: Layer02 = value; break;
          case 3: Layer03 = value; break;
          case 4: Layer04 = value; break;
          case 5: Layer05 = value; break;
          case 6: Layer06 = value; break;
          case 7: Layer07 = value; break;
          case 8: Layer08 = value; break;
          case 9: Layer09 = value; break;
          case 10: Layer10 = value; break;
          case 11: Layer11 = value; break;
          case 12: Layer12 = value; break;
          case 13: Layer13 = value; break;
          case 14: Layer14 = value; break;
          case 15: Layer15 = value; break;
          case 16: Layer16 = value; break;
          case 17: Layer17 = value; break;
          case 18: Layer18 = value; break;
          case 19: Layer19 = value; break;
          case 20: Layer20 = value; break;
          case 21: Layer21 = value; break;
          case 22: Layer22 = value; break;
          case 23: Layer23 = value; break;
          case 24: Layer24 = value; break;
          case 25: Layer25 = value; break;
          case 26: Layer26 = value; break;
          case 27: Layer27 = value; break;
          case 28: Layer28 = value; break;
          case 29: Layer29 = value; break;
          case 30: Layer30 = value; break;
          case 31: Layer31 = value; break;
        }
      }
    }

    public uint Value
    {
      get
      {
        var result = 0;
        result |= (Layer00 ? 1 : 0) << 0;
        result |= (Layer01 ? 1 : 0) << 1;
        result |= (Layer02 ? 1 : 0) << 2;
        result |= (Layer03 ? 1 : 0) << 3;
        result |= (Layer04 ? 1 : 0) << 4;
        result |= (Layer05 ? 1 : 0) << 5;
        result |= (Layer06 ? 1 : 0) << 6;
        result |= (Layer07 ? 1 : 0) << 7;
        result |= (Layer08 ? 1 : 0) << 8;
        result |= (Layer09 ? 1 : 0) << 9;
        result |= (Layer10 ? 1 : 0) << 10;
        result |= (Layer11 ? 1 : 0) << 11;
        result |= (Layer12 ? 1 : 0) << 12;
        result |= (Layer13 ? 1 : 0) << 13;
        result |= (Layer14 ? 1 : 0) << 14;
        result |= (Layer15 ? 1 : 0) << 15;
        result |= (Layer16 ? 1 : 0) << 16;
        result |= (Layer17 ? 1 : 0) << 17;
        result |= (Layer18 ? 1 : 0) << 18;
        result |= (Layer19 ? 1 : 0) << 19;
        result |= (Layer20 ? 1 : 0) << 20;
        result |= (Layer21 ? 1 : 0) << 21;
        result |= (Layer22 ? 1 : 0) << 22;
        result |= (Layer23 ? 1 : 0) << 23;
        result |= (Layer24 ? 1 : 0) << 24;
        result |= (Layer25 ? 1 : 0) << 25;
        result |= (Layer26 ? 1 : 0) << 26;
        result |= (Layer27 ? 1 : 0) << 27;
        result |= (Layer28 ? 1 : 0) << 28;
        result |= (Layer29 ? 1 : 0) << 29;
        result |= (Layer30 ? 1 : 0) << 30;
        result |= (Layer31 ? 1 : 0) << 31;
        return unchecked((uint)result);
      }
      set
      {
        Layer00 = (value & (1 << 0)) != 0;
        Layer01 = (value & (1 << 1)) != 0;
        Layer02 = (value & (1 << 2)) != 0;
        Layer03 = (value & (1 << 3)) != 0;
        Layer04 = (value & (1 << 4)) != 0;
        Layer05 = (value & (1 << 5)) != 0;
        Layer06 = (value & (1 << 6)) != 0;
        Layer07 = (value & (1 << 7)) != 0;
        Layer08 = (value & (1 << 8)) != 0;
        Layer09 = (value & (1 << 9)) != 0;
        Layer10 = (value & (1 << 10)) != 0;
        Layer11 = (value & (1 << 11)) != 0;
        Layer12 = (value & (1 << 12)) != 0;
        Layer13 = (value & (1 << 13)) != 0;
        Layer14 = (value & (1 << 14)) != 0;
        Layer15 = (value & (1 << 15)) != 0;
        Layer16 = (value & (1 << 16)) != 0;
        Layer17 = (value & (1 << 17)) != 0;
        Layer18 = (value & (1 << 18)) != 0;
        Layer19 = (value & (1 << 19)) != 0;
        Layer20 = (value & (1 << 20)) != 0;
        Layer21 = (value & (1 << 21)) != 0;
        Layer22 = (value & (1 << 22)) != 0;
        Layer23 = (value & (1 << 23)) != 0;
        Layer24 = (value & (1 << 24)) != 0;
        Layer25 = (value & (1 << 25)) != 0;
        Layer26 = (value & (1 << 26)) != 0;
        Layer27 = (value & (1 << 27)) != 0;
        Layer28 = (value & (1 << 28)) != 0;
        Layer29 = (value & (1 << 29)) != 0;
        Layer30 = (value & (1 << 30)) != 0;
        Layer31 = (value & (1 << 31)) != 0;
      }
    }

    public bool Equals(PhysicsLayerTags other) => Value == other.Value;

    public override bool Equals(object obj) => obj is PhysicsLayerTags other && Equals(other);

    public override int GetHashCode() => unchecked((int)Value);
  }
}
