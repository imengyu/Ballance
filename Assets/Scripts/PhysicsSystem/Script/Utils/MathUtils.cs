using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class MathUtils
{
    public const float UnityEpsilonNormalSqrt = 1e-15F;
    public const float UnityEpsilon = 0.00001F;

    public static float Angle(float3 from, float3 to)
    {
        // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
        var denominator = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
        if (denominator < UnityEpsilonNormalSqrt)
            return 0F;

        var dot = math.clamp(math.dot(from, to) / denominator, -1F, 1F);
        return math.degrees(math.acos(dot));
    }     
    public static float3 ProjectOnPlane(float3 vector, float3 planeNormal)
    {
        var sqrMag = math.dot(planeNormal, planeNormal);
        if (sqrMag < UnityEpsilon)
            return vector;

        var dot = math.dot(vector, planeNormal);
        return vector - planeNormal * (dot / sqrMag);
    }
    public static float SignedAngle(float3 from, float3 to, float3 axis)
    {
        var unsignedAngle = Angle(from, to);
        var sign = math.sign(math.dot(math.cross(from, to), axis));
        return unsignedAngle * sign;
    }

    /// <summary>
    /// Physics internally represents all rigid bodies in world space.
    /// If a static body is in a hierarchy, its local-to-world matrix must be decomposed when building the physics world.
    /// This method returns a world-space RigidTransform that would be decomposed for such a rigid body.
    /// </summary>
    /// <returns>A world-space RigidTransform as used by physics.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform DecomposeRigidBodyTransform(in float4x4 localToWorld) =>
        new RigidTransform(DecomposeRigidBodyOrientation(localToWorld), localToWorld.c3.xyz);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion DecomposeRigidBodyOrientation(in float4x4 localToWorld) =>
        quaternion.LookRotationSafe(localToWorld.c2.xyz, localToWorld.c1.xyz);
}
