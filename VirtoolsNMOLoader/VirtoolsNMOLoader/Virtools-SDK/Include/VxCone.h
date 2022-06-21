/*************************************************************************/
/*	File : VxCone.h														 */
/*	Author :  Romain SIDIDRIS											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2004, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef VXCONE_H

#define VXCONE_H

/**********************************************************
{filename:VxCone}
Name: VxCone

Summary: Class representation of a cone.

Remarks:
A cone is defined by an origin , a direction and an angle [0..pi/2]

A VxCone is defined as:

			class VxCone 
			{
			public:
				VxVector	m_Origin;
				VxVector	m_Direction;
				float		m_CosTheta,m_SinTheta;
			};




See Also : VxMatrix,VxVector
*********************************************************/
class VxCone
{
public:
	// Ctors
	VxCone() {}
	/************************************************
	Summary: Constructs a VxCone object
	Input Arguments: 
		iOrigin: origin of the cone
		iDir: Direction vector of the cone (must be normalized)
		iAngle: Angle in radian of the cone.
	Remarks:
	************************************************/
	VxCone(const VxVector& iOrigin,const VxVector& iDir,float iAngle) : m_Origin(iOrigin),m_Direction(iDir) {
		m_CosTheta = XCos(iAngle);
		m_SinTheta = XSin(iAngle);
	}
	VxCone(const VxVector& iOrigin,const VxVector& iAxePoint,const VxVector& iSurfacePoint) : m_Origin(iOrigin) {
		m_Direction = Normalize(iAxePoint - iOrigin);
		VxVector tmp = (iSurfacePoint- iOrigin);
		float invLength = 1.0f / Magnitude(tmp); 
		m_CosTheta  = DotProduct(tmp,m_Direction) * invLength;
		m_SinTheta = Magnitude(CrossProduct(tmp,m_Direction)) * invLength;
	}	


	// Transform the cone into the other referential
	VX_EXPORT void Transform(VxCone& dest,const VxMatrix& mat) const;


	/************************************************
	Summary: Returns the square distance from a point to this cone surface
	Input Arguments: 
		p: A point in space. 

	Remarks:
	************************************************/
	VX_EXPORT float Distance(const VxVector& p) const;



	/************************************************
	Summary: Returns if a point is inside the cone
	Input Arguments: 
		p: A point in space. 

	Remarks:
	************************************************/
	XBOOL IsPointInside(const VxVector& p) const
	{
		VxVector v	= p - m_Origin;
		float cosa	= DotProduct(v,m_Direction);
		if (cosa <0) {
			return FALSE;
		}
		return cosa*cosa > m_CosTheta*m_CosTheta * SquareMagnitude(v);		
	}

	bool operator == (const VxCone& iCone) const {
		return (m_Origin == iCone.m_Origin) && (m_Direction == iCone.m_Direction) && (m_CosTheta == iCone.m_CosTheta);
	}

	

	const VxVector& GetOrigin() const {return m_Origin;}
	VxVector& GetOrigin() {return m_Origin;}

	const VxVector& GetDirection() const {return m_Direction;}
	VxVector& GetDirection() {return m_Direction;}

	// Origin of the ray
	VxVector	m_Origin;
	// Direction of the ray (normalized)
	VxVector	m_Direction;
	// Cosine and Sine of cone angle
	float		m_CosTheta,m_SinTheta;
};


#endif
