/*************************************************************************/
/*	File : VxOBB.h														 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef VxOBB_H

#define VxOBB_H

/**********************************************************
{filename:VxOBB}
Name: VxOBB

Summary: Class representation of a ray (an origin and a direction).

Remarks:
A Ray is defined by 2 VxVector and is used to represents
a ray in space (useful for intersection purposes.)

A VxOBB is defined as:

			class VxOBB 
			{
			public:
				VxVector	m_Origin;
				VxVector	m_Direction;
			};




See Also : VxMatrix,VxVector
*********************************************************/
class VxOBB
{
public:
	// Ctors
	VxOBB() {}
	VxOBB(const VxBbox& box,const VxMatrix& mat) {Create(box,mat);}

    VxVector& GetCenter () {return m_Center;};
    const VxVector& GetCenter () const {return m_Center;}

    VxVector& GetAxis (int i) {return m_Axis[i];}
    const VxVector& GetAxis (int i) const {return m_Axis[i];}
    VxVector* GetAxes () {return m_Axis;}
    const VxVector* GetAxes () const {return m_Axis;};

    float& GetExtent(int i) {return m_Extents[i];}
    const float& GetExtent(int i) const {return m_Extents[i];}
    float* GetExtents() {return &m_Extents.x;}
    const float* GetExtents() const {return &m_Extents.x;}

	// Transform the ray into the other referential
	void Create(const VxBbox& box,const VxMatrix& mat)
	{
		VxVector v = box.GetCenter();
		Vx3DMultiplyMatrixVector(&m_Center,mat,&v);
		m_Axis[0]		= *(VxVector*)&mat[0][0];
		m_Axis[1]		= *(VxVector*)&mat[1][0];
		m_Axis[2]		= *(VxVector*)&mat[2][0];
		m_Extents[0]	= Magnitude(m_Axis[0]);
		m_Extents[1]	= Magnitude(m_Axis[1]);
		m_Extents[2]	= Magnitude(m_Axis[2]);
		m_Axis[0]		/= m_Extents[0];
		m_Axis[1]		/= m_Extents[1];
		m_Axis[2]		/= m_Extents[2];
		m_Extents[0]	*= 0.5f*(box.Max[0]-box.Min[0]);
		m_Extents[1]	*= 0.5f*(box.Max[1]-box.Min[1]);
		m_Extents[2]	*= 0.5f*(box.Max[2]-box.Min[2]);
	}

	// return TRUE if a point is inside this box
	XBOOL VectorIn(const VxVector& iV) const
	{
		VxVector d = (iV - m_Center);

		float xRes = DotProduct(d,m_Axis[0]);
		if (XAbs(xRes) > m_Extents[0]) return FALSE;
		float yRes = DotProduct(d,m_Axis[1]);
		if (XAbs(yRes) > m_Extents[1]) return FALSE;
		float zRes = DotProduct(d,m_Axis[2]);
		if (XAbs(zRes) > m_Extents[2]) return FALSE;
		return TRUE;
	}	
	// return TRUE if iB is completely inside this box
	XBOOL IsBoxInside(const VxBbox& iB) const
	{
		if (!VectorIn(iB.Max)) return FALSE;	// mmm
		if (!VectorIn(iB.Min)) return FALSE;	// MMM
		// Quick test on extrema passed 
		// need to check all other 6 points
		VxVector tmp(iB.Min.x,iB.Min.y,iB.Max.z);  
		if (!VectorIn(tmp)) return FALSE;	// mmM
		tmp.y = iB.Max.y;
		if (!VectorIn(tmp)) return FALSE;	// mMM
		tmp.z = iB.Min.z;
		if (!VectorIn(tmp)) return FALSE;	// mMm
		tmp.x = iB.Max.x;
		if (!VectorIn(tmp)) return FALSE;	// MMm
		tmp.y = iB.Min.y;
		if (!VectorIn(tmp)) return FALSE;	// MmM
		tmp.z = iB.Min.z;
		if (!VectorIn(tmp)) return FALSE;	// Mmm
		return TRUE;
	}	
	
	bool operator == (const VxOBB& iBox) const {
		return (m_Extents == iBox.m_Extents) && (m_Center == iBox.m_Center)
			 && (m_Axis[0] == iBox.m_Axis[0])  && (m_Axis[1] == iBox.m_Axis[1])  && (m_Axis[2] == iBox.m_Axis[2]);
	}

	// Center of the box
	VxVector	m_Center;
	// Axis of the box
	VxVector	m_Axis[3];
	// Extents of the box
    VxVector	m_Extents;
};


#endif
