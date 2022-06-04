/*************************************************************************/
/*	File : VxVector.h													 */
/*	Author :  Romain SIDIDRIS											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef VXVECTOR_H
#define VXVECTOR_H

#include "VxMathDefines.h"
#include "XUtil.h"

VX_EXPORT int radToAngle(float val);
VX_EXPORT float Tsin(int angle);
VX_EXPORT float Tcos(int angle);
struct VxCompressedVector;
struct VxCompressedVectorOld;
class VxMatrix;

//______________________
// some global variables

/*******************************************************
{filename:VxVector}
Name: VxVector

Summary: Class representation of a Vector in 3 dimensions

Remarks:
A VxVector is defined as:

      typedef struct VxVector {
           union {
                struct {
                     float x,y,z;
                };
                float v[3];
           };
      }

Elements can be accessed with x,y,z value or through the array v.


***********************************************************/
struct VxVector
{
#if defined(_LINUX)
    float x, y, z;
#else
    union
    {
        struct
        {
            float x, y, z;
        };
        float v[3];
    };
#endif

public:
    // =====================================
    // Constructors
    // =====================================
    VxVector();
    VxVector(float f);
    VxVector(float _x, float _y, float _z);
    VxVector(const float f[3]);

    void Absolute();
    /*************************************************
    Summary: Returns the square magnitude(length) of a vector.

    Return Value:
        Square magnitude of the vector

    Remarks:
        This method returns the square magnitude (length*length) of the vector v.


    See also: Magnitude
    *************************************************/
    float SquareMagnitude() const { return x * x + y * y + z * z; }

    /*************************************************
    Summary: Returns the magnitude(length) of a vector.

    Return Value:
        Magnitude of the vector

    Remarks:
        This method returns the length of the vector v.
        WARNING: Calling this function with a null vector (0,0,0) will
        return an indefinite value (constant NaN - not a number)


    See also: SquareMagnitude
    *************************************************/
    float Magnitude() const { return sqrtf(SquareMagnitude()); }

    // =====================================
    // Access grants
    // =====================================
    const float &operator[](int i) const;
    float &operator[](int i);
    void Set(float X, float Y, float Z);

    VxVector &operator+=(const VxVector &v);
    VxVector &operator-=(const VxVector &v);
    VxVector &operator*=(const VxVector &v);
    VxVector &operator/=(const VxVector &v);
    VxVector &operator*=(float s);
    VxVector &operator/=(float s);
    VX_EXPORT VxVector &operator=(const VxCompressedVector &v);

    float Dot(const VxVector &iV) const { return x * iV.x + y * iV.y + z * iV.z; }
    VxVector operator+(float s) const { return VxVector(x + s, y + s, z + s); }
    VxVector operator-(float s) const { return VxVector(x - s, y - s, z - s); }

    // =====================================
    // Unary operators
    // =====================================

    friend const VxVector operator+(const VxVector &v);
    friend const VxVector operator-(const VxVector &v);

    // =====================================
    // Binary operators
    // =====================================

    // Addition and subtraction
    friend const VxVector operator+(const VxVector &v1, const VxVector &v2);
    friend const VxVector operator-(const VxVector &v1, const VxVector &v2);
    // Scalar multiplication and division
    friend const VxVector operator*(const VxVector &v, float s);
    friend const VxVector operator*(float s, const VxVector &v);
    friend const VxVector operator/(const VxVector &v, float s);
    // Memberwise multiplication and division
    friend const VxVector operator*(const VxVector &v1, const VxVector &v2);
    friend const VxVector operator/(const VxVector &v1, const VxVector &v2);

    // Vector dominance
    friend int operator<(const VxVector &v1, const VxVector &v2);
    friend int operator<=(const VxVector &v1, const VxVector &v2);

    // Bitwise equality
    friend int operator==(const VxVector &v1, const VxVector &v2);
    friend int operator!=(const VxVector &v1, const VxVector &v2);

    // Return min/max component of the input vector
    friend float Min(const VxVector &v);
    friend float Max(const VxVector &v);

    // Return memberwise min/max of input vectors
    friend const VxVector Minimize(const VxVector &v1, const VxVector &v2);
    friend const VxVector Maximize(const VxVector &v1, const VxVector &v2);

    // Interpolate two vectors
    friend const VxVector Interpolate(float step, const VxVector &v1, const VxVector &v2);

    VX_EXPORT void Normalize();
    VX_EXPORT void Rotate(const VxMatrix &M);

    VX_EXPORT const static VxVector &axisX();
    VX_EXPORT const static VxVector &axisY();
    VX_EXPORT const static VxVector &axisZ();
    VX_EXPORT const static VxVector &axis0();
    VX_EXPORT const static VxVector &axis1();
    const static VxVector m_AxisX;
    const static VxVector m_AxisY;
    const static VxVector m_AxisZ;
    const static VxVector m_Axis0;
    const static VxVector m_Axis1;
};

/*************************************************
Name: SquareMagnitude

Summary: Returns the square magnitude(length) of a vector.

Input Arguments:
    v: a pointer to a VxVector which magnitude should be returned

Return Value:
    Square magnitude of v

Remarks:
    This method returns the square magnitude (length*length) of the vector v.



See also: Magnitude
*************************************************/
inline float SquareMagnitude(const VxVector &v) { return v.x * v.x + v.y * v.y + v.z * v.z; }

/*************************************************
Name: Magnitude

Summary: Returns the magnitude(length) of a vector.

Input Arguments:
    v: a pointer to a VxVector which magnitude should be returned

Return Value:
    Magnitude of v

Remarks:
    This method returns the length of the vector v.
    WARNING: Calling this function with a null vector (0,0,0) will
    return an indefinite value (constant NaN - not a number)



See also: SquareMagnitude
*************************************************/
inline float Magnitude(const VxVector &v) { return sqrtf(SquareMagnitude(v)); }

/*************************************************
Name: InvSquareMagnitude

Summary: Returns the inverse square magnitude(length) of a vector.

Input Arguments:
    v: a pointer to a VxVector which inverse magnitude should be returned

Return Value:
    Inverse Square magnitude of v

Remarks:
    This method returns the inverse square magnitude (1/(length*length)) of the vector v.



See also: SquareMagnitude
*************************************************/
inline float InvSquareMagnitude(const VxVector &v) { return 1.0f / SquareMagnitude(v); }

/*************************************************
Name: InvMagnitude

Summary: Returns the inverse magnitude(length) of a vector.

Input Arguments:
    v: a pointer to a VxVector which inverse magnitude should be returned

Remarks:
    This method returns the inverse magnitude (1/length) of the vector v.
    WARNING: Calling this function with a null vector (0,0,0) will
    return an indefinite value (constant NaN - not a number)

Return Value:
    Inverse magnitude of v



See also: Magnitude
*************************************************/
inline float InvMagnitude(const VxVector &v) { return 1.0f / Magnitude(v); }

/*************************************************
Name: Normalize

Summary: Returns a normalized vector (length=1).

Input Arguments:
    Vect: a pointer to a VxVector
    v: Vector to normalize.

Return Value:
    A Vector equal to Vect Normalized.

Remarks:
    This method returns a vector equal to Vect normalized to length 1.0.
    WARNING: Calling this function with a null vector (0,0,0) will
    return an indefinite vector (constant NaN - not a number).
    This function is more precise than VxVector::Normalize.



See also: Magnitude,VxVector,Vx2DVector
*************************************************/
inline const VxVector Normalize(const VxVector &v) { return v * InvMagnitude(v); }

// Necessary for binding in VSL
#if defined(macintosh) || defined(PSX2)

const VxVector NormalizeVectorNotInlined(const VxVector &v);
#endif

inline const VxVector Normalize(const VxVector *vect)
{
    return Normalize(*vect);
}

/*************************************************
Name: DotProduct

Summary: Calculates the dot product of two vectors.

Input Arguments:
    Vect1:  First source Vector.
    Vect2:  Second source Vector.

Return Value:
    A float result of the dot product of Vect1.Vect2.

Remarks:


See also: VxVector,Vx2DVector
*************************************************/
inline float DotProduct(const VxVector &v1, const VxVector &v2)
{
    return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
}

/*************************************************
Name: CrossProduct

Summary: Calculates the cross product of two vectors.

Input Arguments:
    Vect1:  First source Vector.
    Vect2:  Second source Vector.

Return Value:
    A VxVector result of the cross product of Vect1^Vect2.



See also: VxVector
*************************************************/
inline const VxVector CrossProduct(const VxVector &Vect1, const VxVector &Vect2)
{
    return VxVector(Vect1.y * Vect2.z - Vect1.z * Vect2.y,
                    Vect1.z * Vect2.x - Vect1.x * Vect2.z, Vect1.x * Vect2.y - Vect1.y * Vect2.x);
}

/*************************************************
Name: Reflect

Summary: Returns the reflection of vector against a plane.

Input Arguments:
    v1: A pointer to a VxVector giving the incident vector (pointing away from the plane)
    Norm: A VxVector giving the normal point away from the plane of reflection.
Return Value:
    A Vector equal to the reflection of v1.

Remarks:
    This method calculates the vector reflection of v1 against the plane described by normal Norm.
using the following equation :
    R = 2*(V dot N)*N - V

It is expected that the incident vector v1 is pointing away from the plane.
The resultant vector R will also be pointing away from the plane.


See also: DotProduct
*************************************************/
inline const VxVector Reflect(const VxVector &v1, const VxVector &Norm)
{
    float dp2 = 2.0f * (DotProduct(v1, Norm));
    return VxVector(dp2 * Norm.x - v1.x, dp2 * Norm.y - v1.y, dp2 * Norm.z - v1.z);
}

VX_EXPORT const VxVector Rotate(const VxMatrix &mat, const VxVector &pt);
VX_EXPORT const VxVector Rotate(const VxVector &v1, const VxVector &v2, float angle);
VX_EXPORT const VxVector Rotate(const VxVector *v1, const VxVector *v2, float angle);

// For VSL Binding
#if defined(macintosh) || defined(PSX2)

VX_EXPORT const VxVector RotateMV(const VxMatrix &mat, const VxVector &pt);

VX_EXPORT const VxVector RotateVVF(const VxVector &v1, const VxVector &v2, float angle);
#endif

/*******************************************************
Name: VxCompressedVector

Summary: Class representation of a unit vector in 3 dimensions

Remarks:
A VxCompressedVector is defined as:

      typedef struct VxCompressedVector
      {
         short int xa,ya
      }

The xa and ya members are polar angles

This representation can be used to store normals or unit vectors using less memory than a conventionnal vector.


***********************************************************/
typedef struct VxCompressedVector
{
public:
    short int xa, ya; // Polar Angles

    VxCompressedVector() { xa = ya = 0; }
    VxCompressedVector(float _x, float _y, float _z) { Set(_x, _y, _z); }

    void Set(float X, float Y, float Z);
    void Slerp(float step, VxCompressedVector &v1, VxCompressedVector &v2);

    // =====================================
    // Unary operators
    // =====================================

    VxCompressedVector &operator=(const VxVector &v);
    VxCompressedVector &operator=(const VxCompressedVectorOld &v);
} VxCompressedVector;

typedef struct VxCompressedVectorOld
{
public:
    int xa, ya; // Polar Angles

    VxCompressedVectorOld() { xa = ya = 0; }
    VxCompressedVectorOld(float _x, float _y, float _z) { Set(_x, _y, _z); }

    void Set(float X, float Y, float Z);
    void Slerp(float step, VxCompressedVectorOld &v1, VxCompressedVectorOld &v2);

    // =====================================
    // Unary operators
    // =====================================

    VxCompressedVectorOld &operator=(const VxVector &v);
    VxCompressedVectorOld &operator=(const VxCompressedVector &v);
} VxCompressedVectorOld;

/*******************************************************
{filename:VxVector4}
Name: VxVector4

Summary: Class representation of a Vector of 4 elements (x,y,z,w)

Remarks:
VxVector4 is used for 3D Transformation when the w component
is used for perspective information.
Most of the methods available for a VxVector are also implemented
for the VxVector4

A VxVector4 is defined as:

      typedef struct VxVector4 {
           union {
                struct {
                     float x,y,z,w;
                };
                float v[4];
           };
      }



***********************************************************/
class VxVector4 : public VxVector
{
public:
    float w;

    VxVector4() { x = y = z = w = 0.0f; }
    VxVector4(float f) { x = y = z = w = f; }
    VxVector4(float _x, float _y, float _z, float _w)
    {
        x = _x;
        y = _y;
        z = _z;
        w = _w;
    }
    VxVector4(const float f[4])
    {
        x = f[0];
        y = f[1];
        z = f[2];
        w = f[3];
    }
    VxVector4 &operator=(const VxVector &v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        return *this;
    }

    // =====================================
    // Access grants
    // =====================================
    const float &operator[](int i) const;
    float &operator[](int i);

#if defined(_LINUX)
    operator float *() const
    {
        return (float *)&x;
    }
#else
    operator float *() const
    {
        return (float *)&v[0];
    }
#endif

    // Initialization
    void Set(float X, float Y, float Z, float W);
    void Set(float X, float Y, float Z);
    float Dot(const VxVector4 &iV) const { return x * iV.x + y * iV.y + z * iV.z; }

    VxVector4 &operator+=(const VxVector4 &v);
    VxVector4 &operator-=(const VxVector4 &v);
    VxVector4 &operator*=(const VxVector4 &v);
    VxVector4 &operator/=(const VxVector4 &v);

    VxVector4 &operator+=(const VxVector &v);
    VxVector4 &operator-=(const VxVector &v);
    VxVector4 &operator*=(const VxVector &v);
    VxVector4 &operator/=(const VxVector &v);

    VxVector4 &operator*=(float s);
    VxVector4 &operator/=(float s);
    VxVector4 operator+(float s) const { return VxVector4(x + s, y + s, z + s, w + s); }
    VxVector4 operator-(float s) const { return VxVector4(x - s, y - s, z - s, w - s); }

    // =====================================
    // Unary operators
    // =====================================

    friend const VxVector4 operator+(const VxVector4 &v);
    friend const VxVector4 operator-(const VxVector4 &v);

    // =====================================
    // Binary operators
    // =====================================

    // Addition and subtraction
    friend const VxVector4 operator+(const VxVector4 &v1, const VxVector4 &v2);
    friend const VxVector4 operator-(const VxVector4 &v1, const VxVector4 &v2);
    // Scalar multiplication and division
    friend const VxVector4 operator*(const VxVector4 &v, float s);
    friend const VxVector4 operator*(float s, const VxVector4 &v);
    friend const VxVector4 operator/(const VxVector4 &v, float s);
    // Memberwise multiplication and division
    friend const VxVector4 operator*(const VxVector4 &v1, const VxVector4 &v2);
    friend const VxVector4 operator/(const VxVector4 &v1, const VxVector4 &v2);

    // Bitwise equality
    friend int operator==(const VxVector4 &v1, const VxVector4 &v2);
    friend int operator!=(const VxVector4 &v1, const VxVector4 &v2);
};

/*******************************************************
{filename:VxBbox}
Name: VxBbox

Summary: Class representation of a Bounding Box

The VxBbox structure contains two VxVector Min and Max to represents
the Minimum and Maximum coordinates of the corners of a box.

A VxBbox is defined as:

      typedef struct VxBbox {
        union {
            struct {
                    VxVector Max;
                    VxVector Min;
                    }
            };
            float v[6];
        };
      }



***********************************************************/
typedef struct VxBbox
{
#if !defined(_MSC_VER)
    VxVector Max; // Maximum corner of the box
    VxVector Min; // Minimum corner of the box
#else
    union
    {
        struct
        {
            VxVector Max; // Maximum corner of the box
            VxVector Min; // Minimum corner of the box
        };
        float v[6];
    };
#endif

public:
    VxBbox() : Max(-1e6f, -1e6f, -1e6f), Min(1e6f, 1e6f, 1e6f) {}
    VxBbox(VxVector iMin, VxVector iMax) : Max(iMax), Min(iMin) {}
    VxBbox(float value)
    {
        Max.x = value;
        Max.y = value;
        Max.z = value;
        Min.x = -value;
        Min.y = -value;
        Min.z = -value;
    }
    BOOL IsValid() const
    {
        if (Min.x > Max.x)
            return FALSE;
        if (Min.y > Max.y)
            return FALSE;
        if (Min.z > Max.z)
            return FALSE;

        return TRUE;
    }
    VxVector GetSize() const { return Max - Min; }
    VxVector GetHalfSize() const { return (Max - Min) * 0.5f; }
    VxVector GetCenter() const { return (Max + Min) * 0.5f; }
    void SetCorners(const VxVector &min, const VxVector &max)
    {
        Min = min;
        Max = max;
    }
    void SetDimension(const VxVector &position, const VxVector &size)
    {
        Min = position;
        Max = position + size;
    }
    void SetCenter(const VxVector &center, const VxVector &halfsize)
    {
        Min = center - halfsize;
        Max = center + halfsize;
    }

    //-------------------------------------------------------
    // Name: Reset
    // Summary: Resets the minimum and maximum values of the box
    // Remarks:
    // The Reset method sets the Minimum Value to (1E6,1E6,1E6)
    // and maximum value to (-1E6,-1E6,-1E6)
    //-------------------------------------------------------
    void Reset()
    {
        Max.x = -1e6f;
        Max.y = -1e6f;
        Max.z = -1e6f;
        Min.x = 1e6f;
        Min.y = 1e6f;
        Min.z = 1e6f;
    }

    //-------------------------------------------------------
    // Name: Merge
    // Summary: Merges two boxes
    // Arguments:
    //		v : A VxBbox to merge to this box
    // Remarks:
    // The Merge method calculates the new extents of this box
    // so it contains the v Box
    //-------------------------------------------------------
    void Merge(const VxBbox &v)
    {
        Max.x = XMax(v.Max.x, Max.x);
        Max.y = XMax(v.Max.y, Max.y);
        Max.z = XMax(v.Max.z, Max.z);

        Min.x = XMin(v.Min.x, Min.x);
        Min.y = XMin(v.Min.y, Min.y);
        Min.z = XMin(v.Min.z, Min.z);
    }

    //-------------------------------------------------------
    // Name: Merge
    // Summary: Merges a vector with a box
    // Arguments:
    //		v : A vector to merge to this box
    // Remarks:
    // The Merge method calculates the new extents of this box
    // so it contains the v point
    //-------------------------------------------------------
    void Merge(const VxVector &v)
    {
        if (v.x > Max.x)
            Max.x = v.x;
        if (v.x < Min.x)
            Min.x = v.x;
        if (v.y > Max.y)
            Max.y = v.y;
        if (v.y < Min.y)
            Min.y = v.y;
        if (v.z > Max.z)
            Max.z = v.z;
        if (v.z < Min.z)
            Min.z = v.z;
    }

    //-------------------------------------------------------
    // Name: Classify
    // Summary: Returns on which side a point is
    // Remarks:
    //
    // Return Value:
    //		A combination of the culling flags
    //
    //-------------------------------------------------------
    DWORD Classify(const VxVector &iPoint) const
    {
        DWORD flag = 0;
        if (iPoint.x < Min.x)
            flag |= VXCLIP_LEFT;
        else if (iPoint.x > Max.x)
            flag |= VXCLIP_RIGHT;
        if (iPoint.y < Min.y)
            flag |= VXCLIP_BOTTOM;
        else if (iPoint.y > Max.y)
            flag |= VXCLIP_TOP;
        if (iPoint.z < Min.z)
            flag |= VXCLIP_BACK;
        else if (iPoint.z > Max.z)
            flag |= VXCLIP_FRONT;
        return flag;
    }

    //-------------------------------------------------------
    // Name: Classify
    // Summary: Returns on which side a box is
    // Remarks:
    //
    // Return Value:
    //		A combination of the culling flags
    //
    //-------------------------------------------------------
    DWORD Classify(const VxBbox &iBox) const
    {
        DWORD flag = 0;
        if (iBox.Max.z < Min.z)
            flag |= VXCLIP_BACK;
        else if (iBox.Min.z > Max.z)
            flag |= VXCLIP_FRONT;
        if (iBox.Max.x < Min.x)
            flag |= VXCLIP_LEFT;
        else if (iBox.Min.x > Max.x)
            flag |= VXCLIP_RIGHT;
        if (iBox.Max.y < Min.y)
            flag |= VXCLIP_BOTTOM;
        else if (iBox.Min.y > Max.y)
            flag |= VXCLIP_TOP;
        return flag;
    }

    //-------------------------------------------------------
    // Name: Classify
    // Summary: Returns on which side a box is compared to point
    // Remarks:
    //
    // Return Value:
    //	 2 : If viewed from point pt the box box2 is on the opposite side of this box
    //	 1 : box2 is inside this box
    //   0 : No idea where the box is
    //
    //-------------------------------------------------------
    VX_EXPORT int Classify(const VxBbox &box2, const VxVector &pt) const;

    //-------------------------------------------------------
    // Summary: classify an array of vertices against
    // the box. An array of dword is filled with
    // flags from the enum VXCLIP_BOXFLAGS
    // Remarks:
    //
    //-------------------------------------------------------
    VX_EXPORT void ClassifyVertices(const int iVcount, BYTE *iVertices, DWORD iStride, DWORD *oFlags) const;
    //-------------------------------------------------------
    // Summary: classify an array of vertices against
    // one axis of the box. An array of dword is filled with
    // flags 0x01 if < min, 0x10 if > max
    // Remarks:
    //
    //-------------------------------------------------------
    VX_EXPORT void ClassifyVerticesOneAxis(const int iVcount, BYTE *iVertices, DWORD iStride, const int iAxis, DWORD *oFlags) const;

    //-------------------------------------------------------
    // Name: Intersect
    // Summary: Intersects two boxes
    // Arguments:
    //		v : A VxBbox to intersect with this box
    // Remarks:
    // The Intersect method calculates the new extents of this box
    // so it only contains the intersection with the v Box
    //-------------------------------------------------------
    void Intersect(const VxBbox &v)
    {
        Max.x = XMin(v.Max.x, Max.x);
        Max.y = XMin(v.Max.y, Max.y);
        Max.z = XMin(v.Max.z, Max.z);

        Min.x = XMax(v.Min.x, Min.x);
        Min.y = XMax(v.Min.y, Min.y);
        Min.z = XMax(v.Min.z, Min.z);
    }

    //-------------------------------------------------------
    // Name: VectorIn
    // Summary: Tests if a point is inside the box.
    // Arguments:
    //		v : A VxVector to test if it is inside the box.
    //
    // Return Value: TRUE if v is inside this box, FALSE otherwise
    //-------------------------------------------------------
    BOOL VectorIn(const VxVector &v) const
    {
        if (v.x < Min.x)
            return FALSE;
        if (v.x > Max.x)
            return FALSE;
        if (v.y < Min.y)
            return FALSE;
        if (v.y > Max.y)
            return FALSE;
        if (v.z < Min.z)
            return FALSE;
        if (v.z > Max.z)
            return FALSE;
        return TRUE;
    }

    //-------------------------------------------------------
    // Name: IsBoxInside
    // Summary: Tests if a box is totally inside this box.
    // Arguments:
    //		b : A VxBbox to test if it is inside the box.
    //
    // Return Value: TRUE if b is inside this box, FALSE otherwise
    //-------------------------------------------------------
    BOOL IsBoxInside(const VxBbox &b) const
    {
        if (b.Min.x < Min.x)
            return 0;
        if (b.Min.y < Min.y)
            return 0;
        if (b.Min.z < Min.z)
            return 0;

        if (b.Max.x > Max.x)
            return 0;
        if (b.Max.y > Max.y)
            return 0;
        if (b.Max.z > Max.z)
            return 0;

        return 1;
    }

    bool operator==(const VxBbox &iBox) const
    {
        return (Max == iBox.Max) && (Min == iBox.Min);
    }

    // Transform this box to eight points according to matrix mat
    VX_EXPORT void TransformTo(VxVector *pts, const VxMatrix &Mat) const;
    // Creates this box from sbox acording to matrix mat
    VX_EXPORT void TransformFrom(const VxBbox &sbox, const VxMatrix &Mat);
} VxBbox;

inline VxVector::VxVector() : x(0), y(0), z(0)
{
}

inline VxVector::VxVector(float f) : x(f), y(f), z(f)
{
}

inline VxVector::VxVector(float _x, float _y, float _z) : x(_x), y(_y), z(_z)
{
}

inline VxVector::VxVector(const float f[3]) : x(f[0]), y(f[1]), z(f[2])
{
}

// Initialization
inline void VxVector::Set(float _x, float _y, float _z)
{
    x = _x;
    y = _y;
    z = _z;
}

inline const float &VxVector::operator[](int i) const
{
    return *((&x) + i);
}

inline float &VxVector::operator[](int i)
{
    return *((&x) + i);
}

inline VxVector &VxVector::operator+=(const VxVector &v)
{
    x += v.x;
    y += v.y;
    z += v.z;
    return *this;
}

inline VxVector &VxVector::operator-=(const VxVector &v)
{
    x -= v.x;
    y -= v.y;
    z -= v.z;
    return *this;
}

inline VxVector &VxVector::operator*=(const VxVector &v)
{
    x *= v.x;
    y *= v.y;
    z *= v.z;
    return *this;
}

inline VxVector &VxVector::operator/=(const VxVector &v)
{
    x /= v.x;
    y /= v.y;
    z /= v.z;
    return *this;
}

inline VxVector &VxVector::operator*=(float s)
{
    x *= s;
    y *= s;
    z *= s;
    return *this;
}

inline VxVector &VxVector::operator/=(float s)
{
    float temp = 1.0f / s;
    x *= temp;
    y *= temp;
    z *= temp;
    return *this;
}

//
inline const VxVector operator+(const VxVector &v)
{
    return v;
}

//
inline const VxVector operator-(const VxVector &v)
{
    return VxVector(-v.x, -v.y, -v.z);
}

inline const VxVector operator+(const VxVector &v1, const VxVector &v2)
{
    return VxVector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
}

inline const VxVector operator-(const VxVector &v1, const VxVector &v2)
{
    return VxVector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
}

//
inline const VxVector operator*(const VxVector &v1, const VxVector &v2)
{
    return VxVector(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
}
//
inline const VxVector operator/(const VxVector &v1, const VxVector &v2)
{
    return VxVector(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
}

//
inline int operator<(const VxVector &v1, const VxVector &v2)
{
    return v1[0] < v2[0] && v1[1] < v2[1] && v1[2] < v2[2];
}

//
inline int operator<=(const VxVector &v1, const VxVector &v2)
{
    return v1[0] <= v2[0] && v1[1] <= v2[1] && v1[2] <= v2[2];
}

inline const VxVector operator*(const VxVector &v, float s)
{
    return VxVector(s * v.x, s * v.y, s * v.z);
}

inline const VxVector operator*(float s, const VxVector &v)
{
    return VxVector(s * v.x, s * v.y, s * v.z);
}

inline const VxVector operator/(const VxVector &v, float s)
{
    float temp = 1.0f / s;
    return VxVector(v.x * temp, v.y * temp, v.z * temp);
}

//
inline int operator==(const VxVector &v1, const VxVector &v2)
{
    return ((v1.x == v2.x) && (v1.y == v2.y) && (v1.z == v2.z));
}

//
inline int operator!=(const VxVector &v1, const VxVector &v2)
{
    return !(v1 == v2);
}

/*************************************************
Name: Absolute

Summary: Calculates absolute value of a vector.

    - v: A reference to a VxVector.

Return value:
     A VxVector with each element set to the absolute value of the corresponding element in v.



See also: VxVector,
*************************************************/
inline const VxVector Absolute(const VxVector &v)
{
    return VxVector(XAbs(v.x), XAbs(v.y), XAbs(v.z));
}

/*************************************************
Name: Absolute

Summary: Calculates absolute value of a vector.

Each element is set to its absolute value.

See also: VxVector
*************************************************/
void inline VxVector::Absolute()
{
    x = XAbs(x);
    y = XAbs(y);
    z = XAbs(z);
}

/*************************************************
Name: Min

Summary: Calculates the minimum value among the elements of a vector.

Arguments:
    v: A Vector.

Return value: A float value containing the minimum of the elements of v.



See also: VxVector,Vx2DVector
*************************************************/
inline float Min(const VxVector &v)
{
    return XMin(v.x, v.y, v.z);
}

/*************************************************
Name: Max

Summary: Calculates the maximum value among the elements of a vector.

Arguments:
    v: A Vector.

Return value: A float value containing the maximum of the elements of v.



See also: VxVector,Vx2DVector
************************************************/
inline float Max(const VxVector &v)
{
    float ret = v.x;
    if (ret < v.y)
        ret = v.y;
    if (ret < v.z)
        ret = v.z;
    return ret;
}

/*************************************************
Name: Minimize

Summary: Constructs a vector containing minimum values of two vectors.

Arguments:
    v1: A Vector.
    v2: A Vector.

Return value: A VxVector containing with each element equal to the smallest element of v1 or v2.



See also: VxVector
*************************************************/
inline const VxVector Minimize(const VxVector &v1, const VxVector &v2)
{
    return VxVector(XMin(v1[0], v2[0]), XMin(v1[1], v2[1]), XMin(v1[2], v2[2]));
}

/*************************************************
Name: Maximize

Summary: Constructs a vector containing maximum values of two vectors.

Arguments:
    v1: A reference to a VxVector.
    v2: A reference to a VxVector.

Return value: A VxVector containing with each element equal to the greatest element of v1 or v2.



See also: VxVector
*************************************************/
inline const VxVector Maximize(const VxVector &v1, const VxVector &v2)
{
    return VxVector(XMax(v1[0], v2[0]), XMax(v1[1], v2[1]), XMax(v1[2], v2[2]));
}

/*************************************************
Name: Interpolate

Summary: Constructs a vector representing the interpolation of two vectors.

Arguments:
    step : The interpolation factor.
    v1: A reference to a VxVector.
    v2: A reference to a VxVector.

Return value: A VxVector .



See also: VxVector
*************************************************/
inline const VxVector Interpolate(float step, const VxVector &v1, const VxVector &v2)
{
    return VxVector(v1.x + (v2.x - v1.x) * step,
                    v1.y + (v2.y - v1.y) * step,
                    v1.z + (v2.z - v1.z) * step);
}

//------------------------------------------------------------------------------------------------------
// VxVector4
//-------------------------------------------------------------------------------------------------------

inline VxVector4 &VxVector4::operator+=(const VxVector4 &v)
{
    x += v.x;
    y += v.y;
    z += v.z;
    w += v.w;
    return *this;
}

inline VxVector4 &VxVector4::operator-=(const VxVector4 &v)
{
    x -= v.x;
    y -= v.y;
    z -= v.z;
    w -= v.w;
    return *this;
}

inline VxVector4 &VxVector4::operator*=(const VxVector4 &v)
{
    x *= v.x;
    y *= v.y;
    z *= v.z;
    w *= v.w;
    return *this;
}

inline VxVector4 &VxVector4::operator/=(const VxVector4 &v)
{
    x /= v.x;
    y /= v.y;
    z /= v.z;
    w /= v.w;
    return *this;
}

inline VxVector4 &VxVector4::operator+=(const VxVector &v)
{
    x += v.x;
    y += v.y;
    z += v.z;
    return *this;
}

inline VxVector4 &VxVector4::operator-=(const VxVector &v)
{
    x -= v.x;
    y -= v.y;
    z -= v.z;
    return *this;
}

inline VxVector4 &VxVector4::operator*=(const VxVector &v)
{
    x *= v.x;
    y *= v.y;
    z *= v.z;
    return *this;
}

inline VxVector4 &VxVector4::operator/=(const VxVector &v)
{
    x /= v.x;
    y /= v.y;
    z /= v.z;
    return *this;
}

inline VxVector4 &VxVector4::operator*=(float s)
{
    x *= s;
    y *= s;
    z *= s;
    w *= s;
    return *this;
}

inline VxVector4 &VxVector4::operator/=(float s)
{
    float temp = 1.0f / s;
    x *= temp;
    y *= temp;
    z *= temp;
    w *= temp;
    return *this;
}

//
inline const VxVector4 operator+(const VxVector4 &v)
{
    return v;
}

//
inline const VxVector4 operator-(const VxVector4 &v)
{
    return VxVector4(-v.x, -v.y, -v.z, -v.w);
}

inline const VxVector4 operator+(const VxVector4 &v1, const VxVector4 &v2)
{
    return VxVector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
}

inline const VxVector4 operator-(const VxVector4 &v1, const VxVector4 &v2)
{
    return VxVector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
}

//
inline const VxVector4 operator*(const VxVector4 &v1, const VxVector4 &v2)
{
    return VxVector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
}
//
inline const VxVector4 operator/(const VxVector4 &v1, const VxVector4 &v2)
{
    return VxVector4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
}

inline const VxVector4 operator*(const VxVector4 &v, float s)
{
    return VxVector4(s * v.x, s * v.y, s * v.z, s * v.w);
}

inline const VxVector4 operator*(float s, const VxVector4 &v)
{
    return VxVector4(s * v.x, s * v.y, s * v.z, s * v.w);
}

inline const VxVector4 operator/(const VxVector4 &v, float s)
{
    float invs = 1.0f / s;
    return VxVector4(invs * v.x, invs * v.y, invs * v.z, invs * v.w);
}

//
inline int operator==(const VxVector4 &v1, const VxVector4 &v2)
{
    return ((v1.x == v2.x) && (v1.y == v2.y) && (v1.z == v2.z) && (v1.w == v2.w));
}

//
inline int operator!=(const VxVector4 &v1, const VxVector4 &v2)
{
    return !(v1 == v2);
}

// Initialization
inline void VxVector4::Set(float _x, float _y, float _z, float _w)
{
    x = _x;
    y = _y;
    z = _z;
    w = _w;
}

// Initialization
inline void VxVector4::Set(float _x, float _y, float _z)
{
    x = _x;
    y = _y;
    z = _z;
}

inline const float &VxVector4::operator[](int i) const
{
    return v[i];
}

inline float &VxVector4::operator[](int i)
{
    return v[i];
}

//------------------------------------------------------------------------------------------------------
// VxCompressed Vector
//-------------------------------------------------------------------------------------------------------

/*************************************************
Name: Slerp

Summary: Performs a linear interpolation between two vectors.

Arguments:
    v1: A reference to a VxCompressedVectorOld.
    v2: A reference to a VxCompressedVectorOld.
    step: The interpolation factor, 0 means v1 and 1 means v2.

Remarks:



See also: VxVector
*************************************************/
inline void VxCompressedVectorOld::Slerp(float step, VxCompressedVectorOld &v1, VxCompressedVectorOld &v2)
{
    int v1y = ((int)v1.ya + 16384) & 16383;
    int v2y = ((int)v2.ya + 16384) & 16383;
    v2y = (v2y - v1y);
    if (v2y > 8192)
        v2y = 16384 - v2y;
    else if (v2y < -8192)
        v2y = 16384 + v2y;
    xa = (int)((float)v1.xa + (float)(v2.xa - v1.xa) * step);
    ya = (int)((float)v1y + (float)v2y * step);
}

//
inline VxCompressedVectorOld &VxCompressedVectorOld::operator=(const VxVector &v)
{
    Set(v.x, v.y, v.z);
    return *this;
}

/*************************************************
Name: Set

Summary: Creates a VxCompressedVectorOld from 3 components.

Arguments:
    X,Y,Z: float components.

Remarks:



See also: VxVector
*************************************************/
inline void VxCompressedVectorOld::Set(float X, float Y, float Z)
{
    // calcul de l'angle x
    xa = -radToAngle((float)asin(Y));
    // calcul de l'angle y
    ya = radToAngle((float)atan2(X, Z));
}

inline void VxCompressedVector::Slerp(float step, VxCompressedVector &v1, VxCompressedVector &v2)
{
    int coef = (int)(65536.0f * step);
    int v1y = ((int)v1.ya + 16384) & 16383;
    int v2y = ((int)v2.ya + 16384) & 16383;
    v2y = (v2y - v1y);
    if (v2y > 8192)
        v2y = 16384 - v2y;
    else if (v2y < -8192)
        v2y = 16384 + v2y;
    xa = (short int)((int)v1.xa + (((int)(v2.xa - v1.xa) * coef) >> 16));
    ya = (short int)(v1y + ((v2y * coef) >> 16));
}

//
inline VxCompressedVector &VxCompressedVector::operator=(const VxVector &v)
{
    Set(v.x, v.y, v.z);
    return *this;
}

inline void VxCompressedVector::Set(float X, float Y, float Z)
{
    // calcul de l'angle x
    xa = (short int)-radToAngle((float)asin(Y));
    // calcul de l'angle y
    ya = (short int)radToAngle((float)atan2(X, Z));
}

//
inline VxCompressedVectorOld &VxCompressedVectorOld::operator=(const VxCompressedVector &v)
{
    xa = (int)v.xa;
    ya = (int)v.ya;
    return *this;
}

//
inline VxCompressedVector &VxCompressedVector::operator=(const VxCompressedVectorOld &v)
{
    xa = (short int)v.xa;
    ya = (short int)v.ya;
    return *this;
}

#endif
