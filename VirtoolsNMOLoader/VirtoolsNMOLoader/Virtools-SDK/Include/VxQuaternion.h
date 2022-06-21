/*************************************************************************/
/*	File : VxQuaternion.h												 */
/*	Author :  Romain SIDIDRIS											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef VXQUATERNION_H

#define VXQUATERNION_H


#if defined(_LINUX)
#include "VxVector.h"
#endif



enum QuatPart { Quat_X, Quat_Y, Quat_Z, Quat_W };


VX_EXPORT 	VxQuaternion	Vx3DQuaternionSnuggle		(VxQuaternion *Quat,VxVector *Scale);
VX_EXPORT 	VxQuaternion	Vx3DQuaternionFromMatrix	(const VxMatrix& Mat);
VX_EXPORT 	VxQuaternion	Vx3DQuaternionConjugate		(const VxQuaternion& Quat);
VX_EXPORT 	VxQuaternion	Vx3DQuaternionMultiply		(const VxQuaternion& QuatL, const VxQuaternion& QuatR);
VX_EXPORT 	VxQuaternion	Vx3DQuaternionDivide		(const VxQuaternion& P, const VxQuaternion& Q);
VX_EXPORT 	VxQuaternion	Slerp(float Theta,const VxQuaternion& Quat1,const VxQuaternion& Quat2);
VX_EXPORT 	VxQuaternion	Squad(float Theta,const VxQuaternion& Quat1,const VxQuaternion& Quat1Out,const VxQuaternion& Quat2In,const VxQuaternion& Quat2);
VX_EXPORT 	VxQuaternion	LnDif(const VxQuaternion& P,const VxQuaternion& Q);	// Ln(P/Q) = Ln(Q(-1).P)
VX_EXPORT 	VxQuaternion	Ln(const VxQuaternion& Quat);
VX_EXPORT 	VxQuaternion	Exp(const VxQuaternion& Quat);

/**********************************************************
{filename:VxQuaternion}
Name: VxQuaternion

Summary: Class representation of a Quaternion
Remarks:
A Quaternion is defined by 4 floats and is used to represents
an orientation in space. Its common usage is for interpolation 
between two orientations through the Slerp() method.

Quaternions can be converted to VxMatrix or Euler Angles.

A VxQuaternion is defined as:

		typedef struct VxQuaternion {
			union {
					struct {
						float x,y,z,w;
					};
					float v[4];
			};
		}

Elements can be accessed with x,y,z,w value or through the array v. 



See Also : VxMatrix,VxVector,Quaternions
*********************************************************/
typedef struct VxQuaternion
{
#if defined(_LINUX) || defined(PSX2) || defined(PSP)  || defined (__GNUC__)
  float x,y,z;
  union{
   	float w;
	float angle;
  };
#else
	union
	{
		struct {
			VxVector axis;
			float	angle;
		};
		struct {
			float x,y,z,w;
		};
		float v[4];
	};
#endif
	
public:
	VxQuaternion() { x=y=z=0; w=1.0f; }
	VxQuaternion(const VxVector& Vector,float Angle) { FromRotation(Vector,Angle); }
	VxQuaternion(float X,float Y,float Z,float W) {	x=X; y=Y; z=Z; w=W;}

	VX_EXPORT 	void	FromMatrix(const VxMatrix& Mat,BOOL MatIsUnit=TRUE,BOOL RestoreMat=TRUE);
	VX_EXPORT	void	ToMatrix(VxMatrix& Mat) const;
	VX_EXPORT 	void	Multiply(const VxQuaternion& Quat);
	VX_EXPORT 	void	FromRotation(const VxVector& Vector,float Angle);
	VX_EXPORT 	void	ToRotation(VxVector& Vector,float& Angle);
	VX_EXPORT 	void	FromEulerAngles(float eax,float eay,float eaz);
	VX_EXPORT 	void	ToEulerAngles(float *eax,float *eay,float *eaz) const;
	VX_EXPORT 	void	Normalize();

        const float&	operator[](int i) const;
	float&			operator[](int i);

	// Addition and subtraction
	VxQuaternion operator + (const VxQuaternion& q) const { return VxQuaternion(x+q.x, y+q.y, z+q.z, w+q.w);}
	VxQuaternion operator - (const VxQuaternion& q) const { return VxQuaternion(x-q.x, y-q.y, z-q.z, w-q.w);}
	VxQuaternion operator * (const VxQuaternion& q) const { return Vx3DQuaternionMultiply(*this,q); }
	VxQuaternion operator / (const VxQuaternion& q) const { return Vx3DQuaternionDivide(*this,q); }

	// Float operator
	friend VxQuaternion operator*(float, const VxQuaternion&);	
	friend VxQuaternion operator*(const VxQuaternion&, float);	
	VxQuaternion& operator*=(float s) { 	x*=s; y*=s; z*=s; w*=s; 	return *this;}

	VxQuaternion operator-() const { return(VxQuaternion(-x,-y,-z,-w)); } 
	VxQuaternion operator+() const { return *this; }

	// Bitwise equality
	friend int operator == (const VxQuaternion& q1, const VxQuaternion& q2);
	friend int operator != (const VxQuaternion& q1, const VxQuaternion& q2);

	friend float	Magnitude (const VxQuaternion& q);
	friend float	DotProduct (const VxQuaternion& p,const VxQuaternion& q);
} VxQuaternion;  





inline int operator == (const VxQuaternion& q1, const VxQuaternion& q2)
{
   return (q1.x==q2.x && q1.y==q2.y && q1.z==q2.z && q1.w==q2.w);
}

inline int operator != (const VxQuaternion& q1, const VxQuaternion& q2)
{
   return (q1.x!=q2.x || q1.y!=q2.y || q1.z!=q2.z || q1.w!=q2.w);
}


inline VxQuaternion operator * (float s, const VxQuaternion& q)
{
	return VxQuaternion(q.x*s,q.y*s,q.z*s,q.w*s);
}

inline VxQuaternion operator * (const VxQuaternion& q, float s)
{
	return VxQuaternion(q.x*s,q.y*s,q.z*s,q.w*s);
}

inline float Magnitude (const VxQuaternion& q)
{
   return (q.x*q.x + q.y*q.y + q.z*q.z + q.w*q.w);   	
}


inline float DotProduct (const VxQuaternion& q1, const VxQuaternion& q2)
{
	return (q1.x*q2.x + q1.y*q2.y + q1.z*q2.z +q1.w*q2.w);
}

inline const float& VxQuaternion::operator[](int i) const
{
  return *((&x)+i);
}

inline float& VxQuaternion::operator[](int i)
{
  return *((&x)+i);
}


#endif
