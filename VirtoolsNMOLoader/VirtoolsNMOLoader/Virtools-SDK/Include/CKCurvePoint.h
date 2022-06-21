/*************************************************************************/
/*	File : CKCurvePoint.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#if !defined(CKCURVEPOINT_H) || defined(CK_3DIMPLEMENTATION)

#define CKCURVEPOINT_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CK3dEntity.h"


#undef CK_PURE

#define CK_PURE = 0

/*************************************************************************
{filename:CKCurvePoint}
Name: CKCurvePoint

Summary: Class describing curve control point (tangent and TCB parameters )

Remarks:
	+ A Control point describes where a curve should pass and how it 
	should behave at this point. This can be set using incoming and outgoing tangents 
	or using TCB parameters which are tension, continuity and bias.

	+ The class id of CKCurvePoint is CKCID_CURVEPOINT.

	{image:curvepointtcb}	


See also: CKCurve
*************************************************************************/
class CKCurvePoint : public CK3dEntity {
public:
#endif

//----------------------------------------------
// Curve functions

/************************************************
Summary: Gets the curve which owns this control point.
Return Value: Pointer to CKCurve

See also: CKCurve
************************************************/
virtual CKCurve *GetCurve()	CK_PURE; 

//----------------------------------------------
// TCB Parameters

/*************************************************
Summary: Gets the Bias of the curve at this point.
Return Value: Bias Value.

See Also: SetBias, SetTension, SetContinuity.
*************************************************/
virtual float GetBias() CK_PURE;

/*************************************************
Summary: Sets the Bias of the curve at this point

Arguments:
	b: Bias value
Remarks:
See Also: GetBias, SetTension, SetContinuity.
*************************************************/
virtual void  SetBias(float b) CK_PURE; 	

/*************************************************
Summary: Gets the Tension of the curve at this point

 Return Value: Tension Value.
See Also: SetBias, SetTension, SetContinuity.
*************************************************/
virtual float GetTension() CK_PURE;

/*************************************************
Summary: Sets the Tension of the curve at this point

Arguments:
	t: Tension value
See Also: SetBias, GetTension, SetContinuity.
*************************************************/
virtual void  SetTension(float t) CK_PURE;

/*************************************************
Summary: Gets the Continuity of the curve at this point

Return Value: Continuity value.
See Also: SetBias, SetTension, SetContinuity.
*************************************************/
virtual float GetContinuity() CK_PURE;

/*************************************************
Summary: Sets the Continuity of the curve at this point

Arguments:
	c: Continuity value.
See Also: SetBias, SetTension, GetContinuity.
*************************************************/
virtual void  SetContinuity(float c) CK_PURE;


/*************************************************
Summary: Checks whether the curve is linear from this point to the next.
Return Value: TRUE if it is linear, FALSE otherwise.

See Also: SetLinear
*************************************************/
virtual CKBOOL	 IsLinear() CK_PURE;


/*************************************************
Summary: Sets the curve to be linear from this point to the next.

Arguments:
	linear: TRUE to set the curve to linear FALSE otherwise.
See Also: IsLinear
*************************************************/
virtual void	 SetLinear(CKBOOL linear=FALSE) CK_PURE;

/*************************************************
Summary: Uses TCB data or explicit tangents

Arguments: use : TRUE to force usage of TCB data, FALSE to use tangents.
Remarks:
	+ Each curve point has incoming and outgoing tangents. These tangents can be automatically calculated
	using TCB parameters ( SetTension,SetContinuity,SetBias) or given explicitly through SetTangents
See Also:SetTangents,SetTension,SetContinuity,SetBias
*************************************************/
virtual void UseTCB(CKBOOL use=TRUE) CK_PURE;

/************************************************
Summary: Checks the usage of TCB parameters.

Return Value: TRUE to force usage of TCB data, FALSE to use tangents.
See Also: UseTCB
************************************************/
virtual CKBOOL IsTCB() CK_PURE;

//---------------------------------------------

/*************************************************
Summary: Gets Curve length at this point.
Return value: Length of the curve from the first control point to this point.

See Also: CKCurve::GetLength
*************************************************/
virtual float GetLength() CK_PURE;

//----------------------------------------------
// Tangents

/*************************************************
Summary: Gets the tangents of the curve at this control point.

Arguments:
	in: pointer to VxVector
	out:pointer to VxVector
See also: SetTangents
*************************************************/
virtual void GetTangents(VxVector *in,VxVector *out) CK_PURE;

/*************************************************
Summary: Sets the tangents at this point.
Arguments:
	in: A pointer to VxVector to the incoming tangent
	out: A pointer to VxVector to the outgoin tangent
Remarks:
	+ Sets the incoming and outgoing tangents to the 
	  curve at this control point.

See also: GetTangents
*************************************************/
virtual void SetTangents(VxVector *in,VxVector *out) CK_PURE;

/*************************************************
Summary: Notifies the curve if any parameter gets modified.
Remarks:
	+ Notifies the curve that it should be updated.

See also: 
*************************************************/	
virtual	void NotifyUpdate() CK_PURE;

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
static CKCurvePoint* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_CURVEPOINT)?(CKCurvePoint*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
