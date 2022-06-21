/*************************************************************************/
/*	File : VxPlane.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef VXPLANE_H

#define VXPLANE_H

/**********************************************************
{filename:VxPlane}
Name: VxPlane

Summary: Class representation of an infinite plane.

Remarks:
	A VxPlane is defined by a normal VxVector and and is used to represents
	a infinite plane in space (usefull for intersection purposes.)
	It can be created with a point in space and normal,an equation or with 3 points.

A VxPlane is defined as:

			class VxPlane 
			{
				public:
					VxVector	m_Normal;	// The normal to the plane
					float		m_D;		// The D component of Ax + By + Cz + D = 0 equation	
			};




See Also : VxVector,VxRay,Intersections,VxIntersect
*********************************************************/
class VxPlane
{
public:
	
	/************************************************
	Summary: Constructors.

	Input Arguments: 
		a,b,c,d: Equation of the plane.
		n: A VxVector representing the normal of the plane.
		p: A point in space lying on the plane 

	Remarks:
		A VxPlane can be created from its equation, a normal and a point lying on the plane
		or three points belonging to the plane (which must not be aligned).
	************************************************/
	VxPlane() : m_Normal(0,0,0), m_D(0) {}
	VxPlane(const VxVector& n,float d) : m_Normal(n), m_D(d) {}
	VxPlane(float a,float b,float c,float d) : m_Normal(a,b,c), m_D(d) {}
	VxPlane(const VxVector& n,const VxVector& p)  { Create(n,p); }
	VxPlane(const VxVector& a,const VxVector& b,const VxVector& c) { Create(a,b,c); }

	friend const VxPlane operator - (const VxPlane& p);

	const VxVector& GetNormal() const {return m_Normal;}

	/************************************************
	Summary: Returns on which side a point is regarding to a plane..

	Input Arguments: 
		p: A point in space. 

	Remarks:
		Classify the point with regards of the plane : a positive value means the point is front of the plane.

	See Also:Distance
	************************************************/
	float Classify(const VxVector& p) const {return DotProduct(m_Normal,p) + m_D;}

	/************************************************
	Summary: Returns on which side a box is regarding to a plane..

	Input Arguments: 
		box: A box in the plane referential. 

	Remarks:
		Classify the box with regards of the plane : a positive value means the box is front of the plane.
		0 means the box intersects the plane. 

	See Also:Distance
	************************************************/
	float Classify(const VxBbox& box) const {
		VxVector vmin = box.Min;
		VxVector vmax = box.Max;
		for (int axis=0; axis<3; ++axis) {
			if (m_Normal[axis] < 0) {
				vmin[axis] = box.Max[axis];
				vmax[axis] = box.Min[axis];
			}
		}
		float f = Classify(vmin);
		if (f > 0.0f) return f;
		f = Classify(vmax);
		if (f >= 0.0f) return 0.0f;
		return f;
	}

	/************************************************
	Summary: Returns on which side an oriented box is regarding to a plane..

	Input Arguments: 
		box: A box in the referential of the given matrix. 
		max: matrix of the referntial of the box.

	Remarks:
		Classify the box with regards of the plane : a positive value means the box is front of the plane.
		0 means the box intersects the plane. 

	See Also:Distance
	************************************************/
	float Classify(const VxBbox& box, const VxMatrix& mat) const {
		VxVector hsize		= box.GetHalfSize();
		
		float r =	XAbs(DotProduct(m_Normal,mat[0]*hsize[0])) + 
					XAbs(DotProduct(m_Normal,mat[1]*hsize[1])) + 
					XAbs(DotProduct(m_Normal,mat[2]*hsize[2]));

		// hsize is now used as position
		VxVector v = box.GetCenter();
		Vx3DMultiplyMatrixVector(&hsize,mat,&v);
		
		float d =	DotProduct(hsize,m_Normal) + m_D;
		if	(d > r)	return (d-r);
		else if (-d > r) return -(-d - r);
		else return 0.0f;
	}

	/************************************************
	Summary: Returns on which side a face is regarding to a plane..

	Input Arguments: 
		pt0: the first index of the face. 
		pt1: the second index of the face. 
		pt2: the third index of the face. 

	Retun Value: The minimum distance of the face to the plane.

	Remarks:
		Classify the face with regards of the plane : a positive value means the point is front of the plane, a
		negative value that the face is behind the plane.. 0 means that the face is crossing.

	See Also:Distance
	************************************************/
	// Intersection Box - Plane
	float
	ClassifyFace(const VxVector& pt0,const VxVector& pt1,const VxVector& pt2) const
	{
		float d = Classify(pt0);
		float min = d;
		
		d = Classify(pt1);
		if (min > 0.0f) {
			if (d < 0.0f) return 0.0f;
			if (d < min) min = d;
		} else {
			if (d > 0.0f) return 0.0f;
			if (d > min) min = d;
		}
		
		d = Classify(pt2);	
		if (min > 0.0f) {
			if (d < 0.0f) return 0.0f;
			if (d < min) min = d;
		} else {
			if (d > 0.0f) return 0.0f;
			if (d > min) min = d;
		}
		
		return min;
	}

	/************************************************
	Summary: Return the distance of a point to the plane

	Input Arguments: 
		p: A point in space. 

	Return Value : Distance from the point to the plane.
	Remarks:

	See also: Classify
	************************************************/
	float Distance(const VxVector& p) const {return XAbs(Classify(p));}

	/************************************************
	Summary: Return the nearest point from a point on the plane

	Input Arguments: 
		p: A point in space. 

	Return Value : A point on the plane.
	Remarks:

	See also: Classify
	************************************************/
	const VxVector NearestPoint(const VxVector& p) const {return p+m_Normal*-Classify(p);}

	
VX_EXPORT void Create(const VxVector& n,const VxVector& p);
	
VX_EXPORT void Create(const VxVector& a,const VxVector& b,const VxVector& c);
	
	bool operator == (const VxPlane& iPlane) const {
		return (m_Normal == iPlane.m_Normal) && (m_D == iPlane.m_D);
	}


	float XClassify(const VxVector boxaxis[4]) const {
		float r =	XAbs(DotProduct(m_Normal,boxaxis[0])) + 
					XAbs(DotProduct(m_Normal,boxaxis[1])) + 
					XAbs(DotProduct(m_Normal,boxaxis[2]));
		
		float d =	DotProduct(m_Normal,boxaxis[3]) + m_D;
		if	(d > r)	return d;
		else if (d < -r) return d;
		else return 0.0f;
	}

	// The normal to the plane
	VxVector	m_Normal;
	// The D component of Ax + By + Cz + D = 0 equation
	float		m_D;

};

inline const VxPlane operator - (const VxPlane& p)
{
   return VxPlane(-p.m_Normal,-p.m_D);
}

#endif
