/*************************************************************************/
/*	File : Vx2DVector.h													 */
/*	Author :  Romain SIDIDRIS											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef VX2DVECTOR_H

#define VX2DVECTOR_H


/*******************************************************
{filename:Vx2DVector}
Name: Vx2DVector

Summary: Class representation of a Vector in 2 dimensions



Remarks:
A Vx2DVector is defined as:

		struct Vx2DVector {
			union {
				struct {
					float x,y;
				};
			float v[2];
			};
		}

***********************************************************/
struct Vx2DVector
{
#if defined(_LINUX)
  float x,y;
#else
	union
	{
		struct
		{
			float x,y;
		};
		float v[2];
	};
#endif

	/*************************************************
	Summary: Calculates the length of the vector.

	Return Value:
		The length of the vector.

	Remarks:
		WARNING: Calling this function with a null vector (0,0,0) will
		return an indefinite length (constant NaN - not a number)

	

	See also: Normalize, SquareMagnitude
	*************************************************/
	float Magnitude() const {
		return XSqrt(SquareMagnitude());
	}

	/*************************************************
	Summary: Calculates the square length of the vector.

	Return Value:
		The length of the vector.

	Remarks:
		WARNING: Calling this function with a null vector (0,0,0) will
		return an indefinite length (constant NaN - not a number)

	

	See also: Normalize, Magnitude
	*************************************************/
	float SquareMagnitude() const {
		return (x*x + y*y);
	}

	/*************************************************
	Summary: Normalize the vector (length=1).

	Remarks:
		This method sets the magnitude of the vector to 1.0.
		WARNING: Calling this function with a null vector (0,0,0) will
		return an indefinite vector (constant NaN - not a number)

	

	See also: Magnitude
	*************************************************/
	Vx2DVector& Normalize() {
		float m = Magnitude();
		if (m != 0.00000f) {
			m = 1.0f/m;
			x *= m;
			y *= m;
		}
		return *this;
	}
	
	/*************************************************
	Summary: Initializes a Vx2DVector from 2 float (or int) 
	components.

	Arguments:
		iX,iY: floating point (or integer) components.

	

	See also: VxVector
	*************************************************/
	void Set(float iX, float iY) {
		x = iX; y = iY;
	}
	void Set(int iX,int iY) {
		x = (float)iX; y = (float)iY;
	}

	/*************************************************
	Summary: Calculates the dot product of two 2D vectors.

	Input Arguments:
		iV:  vector to be multiplied by the calling vector.

	Return Value:
		Result of the dot product (this . iV).

	Remarks:
		To calculates a distance from a point to a line 
	using a dot product, the vector representing the line
	should be normalized.

	

	See also: Vx2DVector, Normalize
	*************************************************/
	float Dot(const Vx2DVector& iV) const
	{
		return x*iV.x + y * iV.y;
	}

	/*************************************************
	Summary: Calculates the cross vector.

	Remarks:
	

	See also: Vx2DVector, Normalize
	*************************************************/
	Vx2DVector Cross() const
	{
		return Vx2DVector(-y,x);
	}

	// =====================================
	// Access grants
	// =====================================

	/*************************************************
	Summary: Access to the given index of a vector.

	Input Arguments:
		i:  index to access.

	Return Value:
		A reference to the component.

	Remarks:
		0 is x
		1 is y

	

	See also: Vx2DVector, Normalize
	*************************************************/
	const float& operator[] (int i) const {
	#if defined(_LINUX)
		return *((&x)+i);
	#else
		return v[i];
	#endif
	}
	float& operator[] (int i) {
	#if defined(_LINUX)
		return *((&x)+i);
	#else
		return v[i];
	#endif
	}

	// =====================================
	// Assignment operators
	// =====================================

	Vx2DVector& operator += (const Vx2DVector& v) {
		x += v.x;   y += v.y; 
		return *this;
	}
	Vx2DVector& operator -= (const Vx2DVector& v) {
		x -= v.x;   y -= v.y; 
		return *this;
	}
	Vx2DVector& operator *= (const Vx2DVector& v) {
		x *= v.x;   y *= v.y; 
		return *this;
	}
	Vx2DVector& operator /= (const Vx2DVector& v) {
		x /= v.x;   y /= v.y; 
		return *this;
	}
	Vx2DVector& operator *= (float s) {
		x *= s;   y *= s;   
		return *this;
	}
	Vx2DVector& operator /= (float s) {
		s = 1.0f / s;
		x *= s;   y *= s;   
		return *this;
	} 

	// =====================================
	// Unary operators
	// =====================================

	Vx2DVector operator + () const {
		return *this;
	}
	Vx2DVector operator - () const {
		return Vx2DVector(-x,-y);
	}


	// =====================================
	// Binary operators
	// =====================================

	// Addition and subtraction
	Vx2DVector operator + (const Vx2DVector& v)	const {
		return Vx2DVector(x + v.x, y + v.y);
	}
	Vx2DVector operator - (const Vx2DVector& v)	const {
		return Vx2DVector(x - v.x, y - v.y);
	}
	// Memberwise multiplication and division
	Vx2DVector operator * (const Vx2DVector& v)	const {
		return Vx2DVector(x * v.x, y * v.y);
	}
	Vx2DVector operator / (const Vx2DVector& v)	const {
		return Vx2DVector(x / v.x, y / v.y);
	}
	// Scalar multiplication and division
	Vx2DVector operator * (float s) const {
		return Vx2DVector(x * s, y * s);
	}
	Vx2DVector operator / (float s) const {
		s = 1.0f / s;
		return Vx2DVector(x * s, y * s);
	}

	// extern scalar multiplicator
	friend Vx2DVector operator * (float s, const Vx2DVector& v);
	friend Vx2DVector operator / (float s, const Vx2DVector& v);	

	// Vector dominance
	bool operator < (const Vx2DVector& v) const {
		return (x < v.x) && (y < v.y);
	}
	bool operator <= (const Vx2DVector& v) const {
		return (x <= v.x) && (y <= v.y);
	}

	// Bitwise equality
	bool operator == (const Vx2DVector& v) const {
		return (x == v.x) && (y == v.y);
	}
	bool operator != (const Vx2DVector& v) const {
		return (x != v.x) || (y != v.y);
	}

	/*************************************************
	Summary: returns the Min of the two components of 
	a vector.

	Return Value:
		Min component.

	

	See also: Vx2DVector, Normalize
	*************************************************/
	float Min() const {
		return XMin(x,y);
	}

	/*************************************************
	Summary: returns the Max of the two components of 
	a vector.

	Return Value:
		Max component.

	

	See also: Vx2DVector, Normalize
	*************************************************/
	float Max() const {
		return XMax(x,y);
	}

	// =====================================
	// Constructors
	// =====================================

	Vx2DVector():x(0.0f),y(0.0f) {}
	Vx2DVector(float f):x(f),y(f) {}
	Vx2DVector(float iX, float iY):x(iX),y(iY) {}
	Vx2DVector(int iX, int iY):x((float)iX),y((float)iY) {}
	Vx2DVector(const float f[2]):x(f[0]),y(f[1]) {}

};

inline Vx2DVector operator * (float s, const Vx2DVector& v)
{
   return Vx2DVector(s*v.x, s*v.y);
}

inline Vx2DVector operator / (float s, const Vx2DVector& v)
{
   return Vx2DVector(s/v.x, s/v.y);
}

#endif
