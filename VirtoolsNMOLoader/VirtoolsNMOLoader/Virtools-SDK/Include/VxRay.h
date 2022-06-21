/*************************************************************************/
/*	File : VxRay.h														 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef VXRAY_H

#define VXRAY_H

/**********************************************************
{filename:VxRay}
Name: VxRay

Summary: Class representation of a ray (an origin and a direction).

Remarks:
A Ray is defined by 2 VxVector and is used to represents
a ray in space (useful for intersection purposes.)

A VxRay is defined as:

			class VxRay 
			{
			public:
				VxVector	m_Origin;
				VxVector	m_Direction;
			};




See Also : VxMatrix,VxVector
*********************************************************/
class VxRay
{
public:
	// Ctors
	VxRay() {}
	VxRay(const VxVector& start,const VxVector& end) : m_Origin(start),m_Direction(end-start) {}
	VxRay(const VxVector& start,const VxVector& dir,int* dummy) : m_Origin(start),m_Direction(dir) {}


	// Transform the ray into the other referential
	VX_EXPORT void Transform(VxRay& dest,const VxMatrix& mat);

	/************************************************
	Summary: Interpolates a vector along the ray.

	Input Arguments: 
		t : A float value for interpolation.

	Output Arguments: 
		p: A point in space = m_Origin+ t* m_Direction. 

	************************************************/
	void Interpolate (VxVector& p,float t) const 
	{
		p = m_Origin + m_Direction * t;
	}

	/************************************************
	Summary: Returns the square distance from a point to this ray..

	Input Arguments: 
		p: A point in space. 

	Remarks:
	************************************************/
	float SquareDistance(const VxVector& p) const
	{
		VxVector v	= p - m_Origin;
		float a		= SquareMagnitude(v);
		float ps	= DotProduct(v,m_Direction);
		return a - ps*ps;
	}

	/************************************************
	Summary: Returns the distance from a point to this ray..

	Input Arguments: 
		p: A point in space. 

	Return Value:
		The algebraic distance from the point to the line
  
	Remarks:
		return the minimum distance between a point p and 
	a line defined by a point o and a direction d.
	All the inputs must be in the same referential.
	************************************************/
	float Distance(const VxVector& p) const
	{
		return XSqrt(SquareDistance(p));
	}
	
	bool operator == (const VxRay& iRay) const {
		return (m_Origin == iRay.m_Origin) && (m_Direction == iRay.m_Direction);
	}


	const VxVector& GetOrigin() const {return m_Origin;}
	VxVector& GetOrigin() {return m_Origin;}

	const VxVector& GetDirection() const {return m_Direction;}
	VxVector& GetDirection() {return m_Direction;}

	// Origin of the ray
	VxVector	m_Origin;
	// Direction of the ray (not normalized)
	VxVector	m_Direction;

};


#endif
