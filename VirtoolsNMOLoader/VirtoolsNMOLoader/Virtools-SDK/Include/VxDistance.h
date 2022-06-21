/*************************************************************************/
/*	File : VxDistance.h												 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef VXDISTANCE_H

#define VXDISTANCE_H

/**********************************************************
Name: VxDistance

Summary: Static methods for Distance tests.

Remarks: VxDistance class contains a list of static methods useful for miscellaneous Distance tests.
	Using these methods one can test Distance of ray,segment or line with a Box,infinite planes, triangle faces
	or Sphere.




See Also : VxPlane,VxRay,VxBbox,VxVector,Distances
*********************************************************/
class VxDistance
{
public:

	// Lines - Lines Distances

	VX_EXPORT static float LineLineSquareDistance (const VxRay& line0, const VxRay& line1,float* t0 = NULL, float* t1 = NULL);
	VX_EXPORT static float LineRaySquareDistance (const VxRay& line, const VxRay& ray,float* t0 = NULL, float* t1 = NULL);
	VX_EXPORT static float LineSegmentSquareDistance (const VxRay& line, const VxRay& segment,float* t0 = NULL, float* t1 = NULL);
	VX_EXPORT static float RayRaySquareDistance (const VxRay& ray0, const VxRay& ray1,float* t0 = NULL, float* t1 = NULL);
	VX_EXPORT static float RaySegmentSquareDistance (const VxRay& ray, const VxRay& segment,float* t0 = NULL, float* t1 = NULL);
	VX_EXPORT static float SegmentSegmentSquareDistance (const VxRay& segment0, const VxRay& segment1,float* t0 = NULL, float* t1 = NULL);

	static float LineLineDistance (const VxRay& line0, const VxRay& line1,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(LineLineSquareDistance(line0,line1,t0,t1));}
	static float LineRayDistance (const VxRay& line, const VxRay& ray,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(LineRaySquareDistance(line,ray,t0,t1));}
	static float LineSegmentDistance (const VxRay& line, const VxRay& segment,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(LineSegmentSquareDistance(line,segment,t0,t1));}
	static float RayRayDistance (const VxRay& ray0, const VxRay& ray1,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(RayRaySquareDistance(ray0,ray1,t0,t1));}
	static float RaySegmentDistance (const VxRay& ray, const VxRay& segment,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(RaySegmentSquareDistance(ray,segment,t0,t1));}
	static float SegmentSegmentDistance (const VxRay& segment0, const VxRay& segment1,float* t0 = NULL, float* t1 = NULL)
	{return XSqrt(SegmentSegmentSquareDistance(segment0,segment1,t0,t1));}

	// Line - Point distance

	VX_EXPORT static float PointLineSquareDistance(const VxVector& point, const VxRay& line1,float* t0 = NULL);
	VX_EXPORT static float PointRaySquareDistance(const VxVector& point, const VxRay& ray,float* t0 = NULL);
	VX_EXPORT static float PointSegmentSquareDistance(const VxVector& point, const VxRay& segment,float* t0 = NULL);

	static float PointLineDistance(const VxVector& point, const VxRay& line,float* t0 = NULL)
	{return XSqrt(PointLineSquareDistance(point,line,t0));}
	static float PointRayDistance(const VxVector& point, const VxRay& ray,float* t0 = NULL)
	{return XSqrt(PointRaySquareDistance(point,ray,t0));}
	static float PointSegmentDistance(const VxVector& point, const VxRay& segment,float* t0 = NULL)
	{return XSqrt(PointSegmentSquareDistance(point,segment,t0));}
};


#endif
