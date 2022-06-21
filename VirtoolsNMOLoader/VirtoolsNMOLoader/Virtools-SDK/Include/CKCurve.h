/*************************************************************************/
/*	File : CKCurve.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#if !defined(CKCURVE_H) || defined(CK_3DIMPLEMENTATION)

#define CKCURVE_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CK3dEntity.h"


#undef CK_PURE

#define CK_PURE = 0

/*************************************************
{filename:CKCurve}
Summary: Representation of a 3D curve

Remarks:
{image:curve}
+ The CKCurve class is derivated from CK3dEntity. It adds functions
create spline curves made up of CKCurvePoint (control points).

+ Each control points defines where the curve should pass and how it
acts at this point (incoming and outgoing tangents  or TCB parameters)

+ Its class id is CKCID_CURVE

See also: CKCurvePoint
*************************************************/
class CKCurve : public CK3dEntity {
public:
#endif
//------------------------------------
// Length

/************************************************
Summary: Returns the length of the curve
Return value: Length of the curve
Remarks:
	+ The curve length is updated every time a curvepoint moves.

See also: SetLength
************************************************/
virtual float	GetLength() CK_PURE;

//------------------------------
// Open/Closed Curves

/************************************************
Summary: Sets the curve as opened .

See also: IsOpen,Close
************************************************/
virtual void	Open() CK_PURE;

/************************************************
Summary: Sets the curve as closed .

See also: IsOpen,Open
************************************************/
virtual void	Close() CK_PURE;

/************************************************
Summary: Checks the curve as opened or closed.
Return Value: TRUE if the curve is opened.

See also: Close, Open
************************************************/
virtual CKBOOL	IsOpen() CK_PURE;

//----------------------------------------
// Get Point position in the world referential

/************************************************
Summary: Gets a position along the curve.
Arguments:
	step: A float coefficient between 0 and 1 indicating position along the curve.
	pos: A pointer to a VxVector which will be filled with position on the curve at specified step.
	dir: A optional pointer to a VxVector which will contain the direction of the curve at the specified step.
Return Value: CK_OK if successful or an error code otherwise.  
Remarks:
	+ The returned position is given in the world referential.

See also: GetLocalPos
************************************************/
virtual CKERROR GetPos(float step,VxVector *pos,VxVector *dir=NULL) CK_PURE;

/************************************************
Summary: Gets a local position along the curve.
	
Arguments:
	step: A float coefficient between 0 and 1 indicating position along the curve.
	pos: A pointer to a VxVector which will be filled with position on the curve at specified step.
	dir: A optionnal pointer to a VxVector which will contain the direction of the curve at the specified step.
    
Return Value: CK_OK if successful or an error code otherwise.  
Remarks:
	+ The returned position is given in the curve referential      

See also: GetPos
************************************************/
virtual CKERROR GetLocalPos(float step,VxVector *pos,VxVector *dir=NULL) CK_PURE;

//---------------------------------------
// Control points

/************************************************
Summary: Gets tangents to a control point
Arguments:
	index: Index of the control point to get the tangents of.
	pt: A pointer to the CKCurvePoint to get the tangents of.
	in:  A pointer to a VxVector containing incoming tangent.
	out: A pointer to a VxVector containing outgoing tangent.
Return Value: CK_OK if successful,an error code otherwise.

See also: SetTangents
************************************************/
virtual CKERROR GetTangents(int index,VxVector *in,VxVector *out) CK_PURE;
virtual CKERROR GetTangents(CKCurvePoint *pt,VxVector *in,VxVector *out) CK_PURE;


/************************************************
Summary: Sets tangents to a control point
Arguments:
	index: Index of the control point to get the tangents of.
	pt: A pointer to the CKCurvePoint to get the tangents of.
	in:  A pointer to a VxVector containing incoming tangent.
	out: A pointer to a VxVector containing outgoing tangent.
   
Return Value: CK_OK if successful,an error code otherwise.

See also: GetTangents
************************************************/
virtual CKERROR SetTangents(int index,VxVector *in,VxVector *out) CK_PURE;
virtual CKERROR SetTangents(CKCurvePoint *pt,VxVector *in,VxVector *out) CK_PURE;


/************************************************
Summary: Sets the fitting coefficient for the curve.
Arguments:
	fit: Fitting coefficient.
Remarks:
	{Image:FittingCoef}
	+ A fitting coefficient of 0 make the curve pass by every control point.

See also: GetFittingCoeff
************************************************/
virtual void	SetFittingCoeff(float fit) CK_PURE;

virtual float	GetFittingCoeff() CK_PURE;

//------------------------------
// Control points

/************************************************
Summary: Removes a control point.
Arguments:
	pt: A pointer to the control point to remove.
	removeall: TRUE if all references to the control point should be removed.
Return Value: CK_OK if successful or CKERR_INVALIDPARAMETER if pt is an invalid control point 

See also: RemoveAllControlPoints,InsertControlPoint,AddControlPoint,GetControlPointCount,GetControlPoint
************************************************/
virtual CKERROR	 RemoveControlPoint(CKCurvePoint *pt,CKBOOL removeall=FALSE) CK_PURE;
	
/************************************************
Summary: Inserts a control point in the curve.
Arguments:
	prev: A pointer to the control point after which the point should be inserted.
	pt: A pointer to the control point to insert.
Return Value: CK_OK if successful or an error code otherwise.

See also: RemoveControlPoint,AddControlPoint,GetControlPointCount,GetControlPoint
************************************************/
virtual CKERROR	 InsertControlPoint(CKCurvePoint *prev,CKCurvePoint *pt) CK_PURE;

/************************************************
Summary: Adds a control point to the curve.
Arguments:
	pt: A pointer to the CKCurvePoint to add.
Return Value: CK_OK if successful, an error code otherwise.

See also: RemoveControlPoint,InsertControlPoint,GetControlPointCount,GetControlPoint
************************************************/
virtual CKERROR	 AddControlPoint(CKCurvePoint *pt) CK_PURE;


/************************************************
Summary: Returns the number of control points 
Return Value: Number of control points 

See also: RemoveControlPoint,InsertControlPoint,AddControlPoint,GetControlPoint
************************************************/
virtual int		 GetControlPointCount() CK_PURE;

/************************************************
Summary: Gets a control point according to its index.
Arguments:
	pos: Index of the cotrol point to retrieve.
Return Value: A pointer to the CKCurvePoint.

See also: RemoveControlPoint,InsertControlPoint,GetControlPointCount,AddControlPoint
************************************************/
virtual CKCurvePoint *GetControlPoint(int pos) CK_PURE;

	
/************************************************
Summary: Removes all the control points
Return Value: CK_OK if successful, an error code otherwise.

See also: RemoveControlPoint
************************************************/
virtual CKERROR	 RemoveAllControlPoints() CK_PURE;

//------------------------------
// Mesh Representation

/************************************************
Summary: Sets the number of segments used to represent the cruve. 
Arguments:
	steps: Number of segments.
Return Value: CK_OK, if successful
Remarks:
	+ A line mesh can be created to represent the curve with CreateLineMesh, this method
	sets the number of segments used for this mesh.

See also: GetStepCount
************************************************/
	virtual CKERROR  SetStepCount(int steps) CK_PURE;

	virtual int		 GetStepCount() CK_PURE;

/************************************************
Summary: Creates a line mesh to represent the curve
Return value: CK_OK if successful an error code otherwise.

See also: SetStepCount,SetColor
************************************************/
	virtual CKERROR	 CreateLineMesh() CK_PURE;

	virtual CKERROR	 UpdateMesh() CK_PURE;

/************************************************
Summary: Gets the color used for the curve mesh. 
Return Value: Current color.

See also: SetColor
************************************************/
	virtual VxColor	 GetColor() CK_PURE;

/************************************************
Summary: Sets the color used for the curve mesh.
Arguments:
	Color: new color for the curve.
Remarks:

See also: GetColor,CreateLineMesh
************************************************/
	virtual void	 SetColor(const VxColor& Color) CK_PURE;


	virtual void Update() CK_PURE;

/*************************************************
Summary: Dynamic cast operator. 
Arguments:
	iO: A pointer to a CKObject to cast.
Return Value:
	iO casted to the appropriate class or NULL if iO is not from the required class .
Example:
	  CKObject* Object;
	  CKAnimation* anim = CKAnimation::Cast(Object);
Remarks:

*************************************************/
	static CKCurve* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_CURVE)?(CKCurve*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
