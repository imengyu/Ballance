
/*************************************************************************/
/*	File : VxSphere.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef VXSPHERE_H
#define VXSPHERE_H


/**********************************************************
{filename:VxSphere}
Name: VxSphere

Remarks:
A 3D Sphere defined by a center and a radius.



See Also : 
*********************************************************/
class VxSphere
{
public:
	VxSphere() {}
	VxSphere(const VxVector& iCenter, const float iRadius):m_Center(iCenter),m_Radius(iRadius) {}


	// Accessors
	VxVector&		Center()		{return m_Center;}
	const VxVector&	Center() const	{return m_Center;}
	float&			Radius()		{return m_Radius;}
	const float&	Radius() const	{return m_Radius;}

	bool
	IsPointInside(const VxVector& iPoint)
	{
		return (SquareMagnitude(iPoint - m_Center) <= (m_Radius*m_Radius));
	}

	bool
	IsBoxTotallyInside(const VxBbox& iBox)
	{
		float totalDist = 0; 

		VxVector minD = m_Center - iBox.Min;
		VxVector maxD = m_Center - iBox.Max;
		minD.Absolute();
		maxD.Absolute();
		maxD = Maximize(minD, maxD);
		return (maxD.SquareMagnitude() < (m_Radius * m_Radius));
	}

	
	bool
	IsPointOnSurface(const VxVector& iPoint)
	{
		return (SquareMagnitude(iPoint - m_Center) == (m_Radius*m_Radius));
	}

	bool operator == (const VxSphere& iSphere) const {
		return (m_Radius == iSphere.m_Radius) && (m_Center == iSphere.m_Center);
	}

protected:
	VxVector	m_Center;
	float		m_Radius;
};


#endif // VXSPHERE_H