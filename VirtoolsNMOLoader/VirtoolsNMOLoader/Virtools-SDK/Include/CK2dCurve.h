/*************************************************************************/
/*	File : CK2dCurve.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CK2DCURVE_H

#define CK2DCURVE_H "$Id:$"

#include "CKObject.h"
#include "CK2dCurvePoint.h"


#define  CKCHNK_2DCURVE					14

/***********************************************************************
{filename:CK2dCurve}
Name: CK2dCurve

Summary: Class describing curves (Acceleration,percentage curves)

Remarks: 
	+ Important : CK2dCurve DOES NOT derive from CKObject... !

	+ A 2dCurve is a spline defined by an array of control points  (CK2dCurvePoint). A 
	property of this curve is that it is injective ( for every X there is one 
	and only one corresponding Y value). This class is usually used to describe
	acceleration or time variations.

	+ Most of the time this class is used in parameters for behaviors. 
  
	+ Its 3D equivalent  is a CKCurve.

{Image:2DCurve}



See also: CK2dCurvePoint,CKCurve
**********************************************************************/
class CK2dCurve {
public :
/*************************************************
Summary: Returns the total length of the curve.
Return Value:
	Length of the curve.
See also: GetY,GetPos
*************************************************/
	float	GetLength() { return m_Length; }

/*************************************************
Summary: Gets the 2D vector corresponding to a given step on the curve
Arguments:
	step : Float ranging from 0.0 to 1.0, it represents the progression along the curve.
	pos : A pointer to a Vx2dVector that will be filled with the coordinates matching the given step
Return Value:
	CK_OK if the function succeeds
	CKERR_INVALIDPARAMETER if step is invalid
See also: GetY
*************************************************/
	CKERROR GetPos(float step,Vx2DVector *pos);
/*************************************************
Summary: Retrieves the Y value corresponding to a given X value
Arguments:
	X: Coordinate on the X axis in the range 0..1
Return Value:
   The Y coordinate corresponding to the X value
See also: GetPos
*************************************************/
	float	GetY(float X);

//--------------------------------------------------------------------------
// Control points access
	
/*************************************************
Summary: Removes a control point.
Arguments:
	- cpt: A pointer to the CK2dCurvePoint to remove	
Remarks:
	+ To retrieve a specific control point use GetControlPoint.
			
See also: GetControlPoint,GetControlPointCount,RemoveControlPoint
*************************************************/
	void	DeleteControlPoint(CK2dCurvePoint* cpt) { m_ControlPoints.Remove(cpt); Update(); }
/*************************************************
Summary: Adds a control point.
Arguments:	
	- pos : A Vx2DVector containing the  position of the point to create.
Remarks:
	The x and y components values of the position should be between 0 and 1
		
See also: GetControlPoint,GetControlPointCount,RemoveControlPoint
*************************************************/
	void	AddControlPoint(const Vx2DVector&  pos);
/*************************************************
Summary: Gets the number of control points 
Return Value: The number of control points in the curve
Remarks: The minimum control point count is 2
See also: GetControlPoint
*************************************************/
	int		GetControlPointCount()	 { return m_ControlPoints.Size(); }
/*************************************************
Summary: Returns a specific control point 
Return Value: A pointer to the pos th control point in the curve
Remarks: The minimum control point count is 2
See also: GetControlPointCount
*************************************************/
	CK2dCurvePoint*	GetControlPoint(int pos) { return &m_ControlPoints[pos]; }

	void Update(); 

//----------------------------------------------------------
//Internal functions 

//----------------------------------------------------------
	CK2dCurve();									
	CKStateChunk*	Dump();							
	CKERROR			Read(CKStateChunk *chunk);		

protected:
	XArray<CK2dCurvePoint> m_ControlPoints;
	
	float	m_Length;
	float   FittingCoef;
	
	void UpdatePointsAndTangents();
	int Rindex(int index) {
		if (index<0)					   return 0;
		if (index>=m_ControlPoints.Size()) return m_ControlPoints.Size()-1;
		return index;		 
	}	

};

#endif

