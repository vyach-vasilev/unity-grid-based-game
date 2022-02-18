using UnityEngine;

public static class MathUtility
{
    public static Matrix4x4 TRS(CustomTransform transform)
    {
        var p = Translate(transform.Position);
        var r = Rotate(transform.Rotation);
        var s = Scale(transform.Scale);

        return p * r * s;
    }
    
    public static Matrix4x4 TRS(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var p = Translate(position);
        var r = Rotate(rotation);
        var s = Scale(scale);

        return p * r * s;
    }
    
    public static Matrix4x4 Translate(Vector3 vector)
    {
        Matrix4x4 m;
        m.m00 = 1F; m.m01 = 0F; m.m02 = 0F; m.m03 = vector.x;
        m.m10 = 0F; m.m11 = 1F; m.m12 = 0F; m.m13 = vector.y;
        m.m20 = 0F; m.m21 = 0F; m.m22 = 1F; m.m23 = vector.z;
        m.m30 = 0F; m.m31 = 0F; m.m32 = 0F; m.m33 = 1F;
        return m;
    }
    
    public static Matrix4x4 Rotate(Quaternion q)
    {
        // Precalculate coordinate products
        float x = q.x * 2.0F;
        float y = q.y * 2.0F;
        float z = q.z * 2.0F;
        float xx = q.x * x;
        float yy = q.y * y;
        float zz = q.z * z;
        float xy = q.x * y;
        float xz = q.x * z;
        float yz = q.y * z;
        float wx = q.w * x;
        float wy = q.w * y;
        float wz = q.w * z;

        // Calculate 3x3 matrix from orthonormal basis
        Matrix4x4 m;
        m.m00 = 1.0f - (yy + zz); m.m10 = xy + wz; m.m20 = xz - wy; m.m30 = 0.0F;
        m.m01 = xy - wz; m.m11 = 1.0f - (xx + zz); m.m21 = yz + wx; m.m31 = 0.0F;
        m.m02 = xz + wy; m.m12 = yz - wx; m.m22 = 1.0f - (xx + yy); m.m32 = 0.0F;
        m.m03 = 0.0F; m.m13 = 0.0F; m.m23 = 0.0F; m.m33 = 1.0F;
        return m;
    }
    
    public static Matrix4x4 Scale(Vector3 vector)
    {
        Matrix4x4 m;
        m.m00 = vector.x; m.m01 = 0F; m.m02 = 0F; m.m03 = 0F;
        m.m10 = 0F; m.m11 = vector.y; m.m12 = 0F; m.m13 = 0F;
        m.m20 = 0F; m.m21 = 0F; m.m22 = vector.z; m.m23 = 0F;
        m.m30 = 0F; m.m31 = 0F; m.m32 = 0F; m.m33 = 1F;
        return m;
    }
}