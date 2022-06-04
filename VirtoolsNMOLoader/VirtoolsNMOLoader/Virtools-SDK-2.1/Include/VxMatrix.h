/*************************************************************************/
/*	File : VxMatrix.h													 */
/*	Author :  Romain SIDIDRIS											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef VXMATRIX_H
#define VXMATRIX_H

#include "VxQuaternion.h"
#include "VxVector.h"

class VxMatrix;
struct Vx2DVector;
struct VxStridedData;

VX_EXPORT void Vx3DMatrixIdentity(VxMatrix &Mat);

VX_EXPORT void Vx3DMultiplyMatrixVector(VxVector *ResultVector, const VxMatrix &Mat, const VxVector *Vector);
VX_EXPORT void Vx3DMultiplyMatrixVectorMany(VxVector *ResultVectors, const VxMatrix &Mat, const VxVector *Vectors, int count, int stride);
VX_EXPORT void Vx3DMultiplyMatrixVector4(VxVector4 *ResultVector, const VxMatrix &Mat, const VxVector4 *Vector);
VX_EXPORT void Vx3DMultiplyMatrixVector4(VxVector4 *ResultVector, const VxMatrix &Mat, const VxVector *Vector); // w=1
VX_EXPORT void Vx3DRotateVector(VxVector *ResultVector, const VxMatrix &Mat, const VxVector *Vector);
VX_EXPORT void Vx3DRotateVectorMany(VxVector *ResultVector, const VxMatrix &Mat, const VxVector *Vector, int count, int stride);
VX_EXPORT void Vx3DMultiplyMatrix(VxMatrix &ResultMat, const VxMatrix &MatA, const VxMatrix &MatB);
VX_EXPORT void Vx3DMultiplyMatrix4(VxMatrix &ResultMat, const VxMatrix &MatA, const VxMatrix &MatB);
VX_EXPORT void Vx3DInverseMatrix(VxMatrix &InverseMat, const VxMatrix &Mat);
VX_EXPORT float Vx3DMatrixDeterminant(const VxMatrix &Mat);
VX_EXPORT void Vx3DMatrixFromRotation(VxMatrix &ResultMat, const VxVector &Vector, float Angle);
VX_EXPORT void Vx3DMatrixFromRotationAndOrigin(VxMatrix &ResultMat, const VxVector &Vector, const VxVector &Origin, float Angle);
VX_EXPORT void Vx3DMatrixFromEulerAngles(VxMatrix &Mat, float eax, float eay, float eaz);
VX_EXPORT void Vx3DMatrixToEulerAngles(const VxMatrix &Mat, float *eax, float *eay, float *eaz);
VX_EXPORT void Vx3DInterpolateMatrix(float step, VxMatrix &Res, const VxMatrix &A, const VxMatrix &B);
VX_EXPORT void Vx3DInterpolateMatrixNoScale(float step, VxMatrix &Res, const VxMatrix &A, const VxMatrix &B);

VX_EXPORT void Vx3DMultiplyMatrixVectorStrided(VxStridedData *Dest, VxStridedData *Src, const VxMatrix &Mat, int count);
VX_EXPORT void Vx3DMultiplyMatrixVector4Strided(VxStridedData *Dest, VxStridedData *Src, const VxMatrix &Mat, int count);
VX_EXPORT void Vx3DRotateVectorStrided(VxStridedData *Dest, VxStridedData *Src, const VxMatrix &Mat, int count);

VX_EXPORT void Vx3DTransposeMatrix(VxMatrix &Result, const VxMatrix &A);

VX_EXPORT void Vx3DDecomposeMatrix(const VxMatrix &A, VxQuaternion &Quat, VxVector &Pos, VxVector &Scale);
VX_EXPORT float Vx3DDecomposeMatrixTotal(const VxMatrix &A, VxQuaternion &Quat, VxVector &Pos, VxVector &Scale, VxQuaternion &URot);
VX_EXPORT float Vx3DDecomposeMatrixTotalPtr(const VxMatrix &A, VxQuaternion *Quat, VxVector *Pos, VxVector *Scale, VxQuaternion *URot);

VX_EXPORT void VxInverseProject(const VxMatrix &iProjection, const Vx2DVector &i2D, const float iZ, VxVector *o3D);

/*****************************************

{filename:VxMatrix}
******************************************/
class VxMatrix
{
public:
    VxMatrix() {}
    VxMatrix(float m[4][4]) { memcpy(m_Data, m, sizeof(VxMatrix)); }

    VX_EXPORT const static VxMatrix &Identity();
    VX_EXPORT XBOOL Compare(const VxMatrix &mat) const;

    // Matrix construction
    void Clear() { memset(m_Data, 0, sizeof(VxMatrix)); }
    void SetIdentity();
    void Orthographic(float Zoom, float Aspect, float Near_plane, float Far_plane);
    void Perspective(float Fov, float Aspect, float Near_plane, float Far_plane);
    void OrthographicRect(float Left, float Right, float Top, float Bottom, float Near_plane, float Far_plane);
    void PerspectiveRect(float Left, float Right, float Top, float Bottom, float Near_plane, float Far_plane);

    // operators
    const VxVector4 &operator[](int i) const { return (const VxVector4 &)(*(VxVector4 *)(m_Data + i)); }
    VxVector4 &operator[](int i) { return (VxVector4 &)(*(VxVector4 *)(m_Data + i)); }

    operator const void *() const { return &m_Data[0]; }
    operator void *() { return &m_Data[0]; }

    BOOL operator==(const VxMatrix &mat) const { return (this == &mat); }
    BOOL operator!=(const VxMatrix &mat) const { return (this != &mat); }

    VxMatrix &operator*=(const VxMatrix &mat)
    {
        VxMatrix tmp = *this;
        Vx3DMultiplyMatrix(*this, tmp, mat);
        return *this;
    }

    VxMatrix operator*(const VxMatrix &iMat) const
    {
        VxMatrix temp;
        Vx3DMultiplyMatrix(temp, *this, iMat);
        return temp;
    }

    friend VxVector operator*(const VxMatrix &m, const VxVector &v);
    friend VxVector4 operator*(const VxMatrix &m, const VxVector4 &v);
    friend VxVector operator*(const VxVector &v, const VxMatrix &m);
    friend VxVector4 operator*(const VxVector4 &v, const VxMatrix &m);

protected:
    float m_Data[4][4];
};

//////////////////////////////////////////////////////////////////////
////////////////////////// Matrix operations /////////////////////////
//////////////////////////////////////////////////////////////////////

inline void VxMatrix::SetIdentity()
{
    m_Data[0][1] = m_Data[0][2] = m_Data[0][3] =
        m_Data[1][0] = m_Data[1][2] = m_Data[1][3] =
            m_Data[2][0] = m_Data[2][1] = m_Data[2][3] =
                m_Data[3][0] = m_Data[3][1] = m_Data[3][2] = 0;
    m_Data[0][0] = m_Data[1][1] = m_Data[2][2] = m_Data[3][3] = 1.0f;
}

inline VxVector operator*(const VxVector &v, const VxMatrix &m)
{
    return VxVector(m[0][0] * v.x + m[1][0] * v.y + m[2][0] * v.z + m[3][0],
                    m[0][1] * v.x + m[1][1] * v.y + m[2][1] * v.z + m[3][1],
                    m[0][2] * v.x + m[1][2] * v.y + m[2][2] * v.z + m[3][2]);
}

inline VxVector operator*(const VxMatrix &m, const VxVector &v)
{
    return VxVector(m[0][0] * v.x + m[1][0] * v.y + m[2][0] * v.z + m[3][0],
                    m[0][1] * v.x + m[1][1] * v.y + m[2][1] * v.z + m[3][1],
                    m[0][2] * v.x + m[1][2] * v.y + m[2][2] * v.z + m[3][2]);
}

inline VxVector4 operator*(const VxMatrix &m, const VxVector4 &v)
{
    return VxVector4(m[0][0] * v.x + m[1][0] * v.y + m[2][0] * v.z + m[3][0],
                     m[0][1] * v.x + m[1][1] * v.y + m[2][1] * v.z + m[3][1],
                     m[0][2] * v.x + m[1][2] * v.y + m[2][2] * v.z + m[3][2],
                     m[0][3] * v.x + m[1][3] * v.y + m[2][3] * v.z + m[3][3]);
}

inline VxVector4 operator*(const VxVector4 &v, const VxMatrix &m)
{
    return VxVector4(m[0][0] * v.x + m[1][0] * v.y + m[2][0] * v.z + m[3][0],
                     m[0][1] * v.x + m[1][1] * v.y + m[2][1] * v.z + m[3][1],
                     m[0][2] * v.x + m[1][2] * v.y + m[2][2] * v.z + m[3][2],
                     m[0][3] * v.x + m[1][3] * v.y + m[2][3] * v.z + m[3][3]);
}

/************************************************
Summary: Constructs a perspective projection matrix.

Input Arguments:
    Fov: Field of View.
    Aspect: Aspect ratio (Width/height)
    Near_plane: Distance of the near clipping plane.
    Far_plane: Distance of the far clipping plane.
Output Arguments:
    Mat: Matrix to set to the perspective projection.

Remarks:
Sets Mat to

        A	=Cos(Fov/2)/Sin(Fov/2)
        F	=	Far_plane
        N	=	Near_plane

                [ A			0			0			0]
                [ 0			A*Aspect	0			0]
           MAT=	[ 0			0			F/F-N		1]
                [ 0			0			-F.N/F-N	0]



See also: PerspectiveRect,Orthographic,Identity,OrthographicRect
************************************************/
inline void VxMatrix::Perspective(float Fov, float Aspect, float Near_plane, float Far_plane)
{
    Clear();
    m_Data[0][0] = cosf(Fov * 0.5f) / sinf(Fov * 0.5f);
    m_Data[1][1] = m_Data[0][0] * Aspect;
    m_Data[2][2] = Far_plane / (Far_plane - Near_plane);
    m_Data[3][2] = -m_Data[2][2] * Near_plane;
    m_Data[2][3] = 1;
}

/************************************************
Summary: Constructs a perspective projection matrix given a view rectangle .

Arguments:
    Left - [In][Out]Left clipping plane value.
    Right - Right clipping plane value.
    Top - [Out]top clipping plane value.
    Bottom - [In]bottom clipping plane value.
    Near_plane - [In][Out]Distance of the near clipping plane.
    Far_plane - [In/Out]Distance of the far clipping plane.

Remarks:
Sets Mat to

        F	=	Far_plane
        N	=	Near_plane
        R	=	Right
        L	=	Left
        T	=	Top
        B	=	Bottom

                [ 2/(R-L)		0			0			0]
                [ 0				-2/(T-B)	0			0]
          MAT =	[ 0				0			1/F-N		0]
                [ -(L+R)/(R-L)	(T+B)/(T-B)	-N/F-N		1]



See also: Perspective,Orthographic,Identity,OrthographicRect
************************************************/
inline void VxMatrix::PerspectiveRect(float Left, float Right, float Top, float Bottom, float Near_plane, float Far_plane)
{
    Clear();
    float RL = 1.0f / (Right - Left);
    float TB = 1.0f / (Top - Bottom);
    m_Data[0][0] = 2.0f * Near_plane * RL;
    m_Data[1][1] = 2.0f * Near_plane * TB;
    m_Data[2][0] = -(Right + Left) * RL;
    m_Data[2][1] = -(Top + Bottom) * TB;
    m_Data[2][2] = Far_plane / (Far_plane - Near_plane);
    m_Data[3][2] = -m_Data[2][2] * Near_plane;
    m_Data[2][3] = 1;
}

/************************************************
Summary: Constructs a orthographic projection matrix.

Input Arguments:
    Zoom: Zoom factor.
    Aspect: Aspect ratio (Width/height)
Output Arguments:
    Near_plane: Distance of the near clipping plane.
    Far_plane: Distance of the far clipping plane.

Remarks:
Sets Mat to

        F	=	Far_plane
        N	=	Near_plane
                        [ Zoom		0			0			0]
                        [ 0			Zoom*Aspect	0			0]
                MAT =	[ 0			0			1/F-N		0]
                        [ 0			0			-N/F-N		1]
See also: Vx3DMatrixIdentity,Perspective,OrthographicRect
************************************************/
inline void VxMatrix::Orthographic(float Zoom, float Aspect, float Near_plane, float Far_plane)
{
    Clear();
    float iz = 1.0f / (Far_plane - Near_plane);
    m_Data[0][0] = Zoom;
    m_Data[1][1] = Zoom * Aspect;
    m_Data[2][2] = iz;
    m_Data[3][2] = -Near_plane * iz;
    m_Data[3][3] = 1.0f;
}

/************************************************
Summary: Constructs a orthographic projection matrix.

Input Arguments:
    Left: Left clipping plane value.
    Right: Right clipping plane value.
    Top: top clipping plane value.
    Bottom: bottom clipping plane value.
    Near_plane: Distance of the near clipping plane.
    Far_plane: Distance of the far clipping plane.
Remarks:
Sets Mat to

        F	=	Far_plane
        N	=	Near_plane
        R	=	Right
        L	=	Left
        T	=	Top
        B	=	Bottom

                [ 2/(R-L)		0			0			0]
                [ 0				-2/(T-B)	0			0]
          MAT =	[ 0				0			1/F-N		0]
                [ -(L+R)/(R-L)	(T+B)/(T-B)	-N/F-N		1]




See also: Vx3DMatrixIdentity,Perspective,Orthographic
************************************************/
inline void VxMatrix::OrthographicRect(float Left, float Right, float Top, float Bottom, float Near_plane, float Far_plane)
{
    Clear();
    float ix = 1.0f / (Right - Left);
    float iy = 1.0f / (Top - Bottom);
    float iz = 1.0f / (Far_plane - Near_plane);
    m_Data[0][0] = 2.0f * ix;
    m_Data[1][1] = -2.0f * iy;
    m_Data[2][2] = iz;
    m_Data[3][0] = -(Left + Right) * ix;
    m_Data[3][1] = (Top + Bottom) * iy;
    m_Data[3][2] = -Near_plane * iz;
    m_Data[3][3] = 1.0f;
}

#endif
