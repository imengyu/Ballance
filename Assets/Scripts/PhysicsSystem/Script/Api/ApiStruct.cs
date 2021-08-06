using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PhysicsRT
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct sVec3
  {
    public float x;
    public float y;
    public float z;

    public static Vector3 FromNativeToVector3(IntPtr ptr) { 
      var vOut = Marshal.PtrToStructure<sVec3>(ptr);
      return new Vector3(vOut.x, vOut.y, vOut.z);
    }
  };
  [StructLayout(LayoutKind.Sequential)]
  internal struct sVec4
  {
    public float x;
    public float y;
    public float z;
    public float w;

    public static Vector4 FromNativeToVector4(IntPtr ptr) { 
      var vOut = Marshal.PtrToStructure<sVec4>(ptr);
      return new Vector4(vOut.x, vOut.y, vOut.z, vOut.w);
    }
  };


  [StructLayout(LayoutKind.Sequential)]
  internal struct sInitStruct
  {
    public IntPtr errCallback;
    public bool mulithread;
    public int smallPoolSize;
  };
  [StructLayout(LayoutKind.Sequential)]
  public struct sConvexHullResult
  {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.R4)]
      public float[] aabb; //min(4) max(4) 
      public int verticesCount;
      public int trianglesCount;
  };
  [StructLayout(LayoutKind.Sequential)]
  public struct sRayCastResult
  {
      public float hitFraction;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
      public float[] normal; //float normal[3]
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
      public float[] pos; //float pos[3]
      public int bodyId;
      public IntPtr body;
  };
  [StructLayout(LayoutKind.Sequential)]
  public struct sConstraintBreakData {
      public bool breakable;
      /// float
      public float threshold;
      /// float
      public float maximumAngularImpulse;
      /// float
      public float maximumLinearImpulse;
  }
  [StructLayout(LayoutKind.Sequential)]
  public struct sConstraintMotorData {
      /// int
      public bool enable;
      /// int
      public int spring;
      /// float
      public float m_tau;
      /// float
      public float m_damping;
      /// float
      public float m_proportionalRecoveryVelocity;
      /// float
      public float m_constantRecoveryVelocity;
      /// float
      public float m_minForce;
      /// float
      public float m_maxForce;
      /// float
      public float m_springConstant;
      /// float
      public float m_springDamping;
  }
  [StructLayout(LayoutKind.Sequential)]
  public struct sPhysicsBodyContactData {
    public float distance;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
    public float[] pos;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
    public float[] normal;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.R4)]
    public float[] separatingNormal;
    public float separatingVelocity;
    public int isRemoved;
  }
}