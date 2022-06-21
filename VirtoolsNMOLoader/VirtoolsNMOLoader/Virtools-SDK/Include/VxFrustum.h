/*************************************************************************/
/*	File : VxFrustum.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef VXFRUSTUM_H

#define VXFRUSTUM_H

/**********************************************************
{filename:VxFrustum}
Name: VxFrustum

Summary: Class representation of an Frustum (like the view Frustum for example).

Remarks:





See Also : VxVector,VxRay,Intersections,VxIntersect
*********************************************************/
class VxFrustum
{
public:
VX_EXPORT	VxFrustum();
VX_EXPORT	VxFrustum(const VxVector& origin,const VxVector& right,const VxVector& up,const VxVector& dir,float nearplane,float farplane,float fov,float aspectratio);

	VxVector& GetOrigin() {return m_Origin;}
	const VxVector& GetOrigin() const {return m_Origin;}

	VxVector& GetRight() {return m_Right;}
	const VxVector& GetRight() const {return m_Right;} 

	VxVector& GetUp() {return m_Up;}
	const VxVector& GetUp() const {return m_Up;} 

	VxVector& GetDir() {return m_Dir;}
	const VxVector& GetDir() const {return m_Dir;}

    float& GetRBound () {return m_RBound;}
    const float& GetRBound () const {return m_RBound;}
    
	float& GetUBound () {return m_UBound;}
    const float& GetUBound () const {return m_UBound;}

    float& GetDMin () {return m_DMin;}
    const float& GetDMin () const {return m_DMin;}

    float& GetDMax () {return m_DMax;}
    const float& GetDMax () const {return m_DMax;}

    float GetDRatio() const {return m_DRatio;}
    float GetRF() const {return m_RF;}
    float GetUF() const {return m_UF;}

    const VxPlane& GetNearPlane() const {return m_NearPlane;}
    const VxPlane& GetFarPlane() const {return m_FarPlane;}
    const VxPlane& GetLeftPlane() const {return m_LeftPlane;}
    const VxPlane& GetRightPlane() const {return m_RightPlane;}
    const VxPlane& GetUpPlane() const {return m_UpPlane;}
    const VxPlane& GetBottomPlane() const {return m_BottomPlane;}

	DWORD Classify(const VxVector& v) const
	{
		DWORD flags = 0;
		// Classification of the vertex to the 6 planes
		if (GetNearPlane().Classify(v) > 0.0f)		flags |= VXCLIP_FRONT; // the vertex is fully off near
		else if (GetFarPlane().Classify(v) > 0.0f)	flags |= VXCLIP_BACK; // the vertex is fully off back
		if (GetLeftPlane().Classify(v) > 0.0f)		flags |= VXCLIP_LEFT; // the vertex is fully off left
		else if (GetRightPlane().Classify(v)> 0.0f) flags |= VXCLIP_RIGHT; // the vertex is fully off right
		if (GetBottomPlane().Classify(v) > 0.0f)	flags |= VXCLIP_BOTTOM; // the vertex is fully off left
		else if (GetUpPlane().Classify(v) > 0.0f)	flags |= VXCLIP_TOP; // the vertex is fully off right
		
		return flags;
	}

	float Classify(const VxBbox& v) const
	{
		float cumul = 1.0f;

		// Classification of the bounding box to the 6 planes
		float f = m_NearPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		f = m_FarPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		f = m_LeftPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		f = m_RightPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		f = m_UpPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		f = m_BottomPlane.Classify(v);
		if (f > 0.0f) return f;
		cumul *= f;

		return -cumul;
	}

	float Classify(const VxBbox& b, const VxMatrix& mat) const
	{
		float cumul = 1.0f;

		VxVector axis[4];
		axis[0] = mat[0]*((b.Max.x - b.Min.x)*0.5f);
		axis[1] = mat[1]*((b.Max.y - b.Min.y)*0.5f);
		axis[2] = mat[2]*((b.Max.z - b.Min.z)*0.5f);
		VxVector v = b.GetCenter();
		Vx3DMultiplyMatrixVector(axis+3,mat,&v);

		// Classification of the bounding box to the 6 planes
		float f = XClassify(axis,m_NearPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		f = XClassify(axis,m_FarPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		f = XClassify(axis,m_LeftPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		f = XClassify(axis,m_RightPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		f = XClassify(axis,m_UpPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		f = XClassify(axis,m_BottomPlane);
		if (f > 0.0f) return f;
		cumul *= f;

		return -cumul;
	}

	BOOL IsInside(const VxVector& v) const
	{
		// Classification of the vertex to the 6 planes
		if (GetNearPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off near
		if (GetFarPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off back
		if (GetLeftPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off left
		if (GetRightPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off right
		if (GetBottomPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off left
		if (GetUpPlane().Classify(v) > 0.0f) return FALSE; // the vertex is fully off right
		
		return TRUE;
	}

VX_EXPORT	void Transform(const VxMatrix& invworldmat);
VX_EXPORT	void ComputeVertices(VxVector vertices[8]) const;
VX_EXPORT	void Update();

	bool operator == (const VxFrustum& iFrustum) const {
		return (m_Origin == iFrustum.m_Origin) && 
				(m_Right == iFrustum.m_Right) && 
				(m_Up == iFrustum.m_Up) && 
				(m_Dir == iFrustum.m_Dir) && 
				(m_RBound == iFrustum.m_RBound) && 
				(m_UBound == iFrustum.m_UBound) && 
				(m_DMin == iFrustum.m_DMin) && 
				(m_DMax == iFrustum.m_DMax); 
	}

protected:

	float XClassify(const VxVector axis[4], const VxPlane& plane) const {return plane.XClassify(axis);}

	VxVector	m_Origin;
	VxVector	m_Right;
	VxVector	m_Up;
	VxVector	m_Dir;

    float		m_RBound;
    float		m_UBound;
    float		m_DMin;
    float		m_DMax;

    // derived quantities
	float		m_DRatio;
	float		m_RF;
	float		m_UF;
	VxPlane		m_LeftPlane;
	VxPlane		m_RightPlane;
	VxPlane		m_UpPlane;
	VxPlane		m_BottomPlane;
	VxPlane		m_NearPlane;
	VxPlane		m_FarPlane;
};


#endif
