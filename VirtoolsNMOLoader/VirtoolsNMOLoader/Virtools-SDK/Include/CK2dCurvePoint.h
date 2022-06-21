/*************************************************************************/
/*	File : CK2dCurvePoint.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CK2DCURVEPOINT_H

#define CK2DCURVEPOINT_H "$Id:$"

#include "CKDefines.h"


#define CK2DCURVEPOINT_USETANGENTS 0x01

#define CK2DCURVEPOINT_LINEAR	   0x02

/*************************************************************************
{filename:CK2dCurvePoint}
Name: CK2dCurvePoint

Summary: Class describing curve control point (tangent and TCB parameters )

Remarks:
	+ A Control point describes where a curve should pass and how it 
	should behave at this point. This can be set using incoming and outgoing tangents 
	or using TCB parameters which are tension, continuity and bias. 




See also: CK2dCurve
*************************************************************************/
class CK2dCurvePoint {
friend class CK2dCurve;
public :
//----------------------------------------------
// Curve functions

/*************************************************
Summary: Gets the 2D curve which owns this control point.
Return Value: A pointer to the CK2dCurve
See Also:CKObject, CK2dCurve
*************************************************/
CK2dCurve *GetCurve() const { return m_Curve; }

//----------------------------------------------
// TCB Parameters

/*************************************************
Summary: Gets the Bias of the curve at this point.
Return Value: Bias
Remarks:

See Also: SetBias,SetTension,SetContinuity
*************************************************/
float GetBias()		const	{	return m_Bias; }

/*************************************************
Summary: Sets the Bias of the curve at this point
Arguments:
	b: Bias value
Remarks:

See Also: GetBias,SetTension,SetContinuity
*************************************************/
void  SetBias(float b)	{ m_Bias =b; NotifyUpdate(); }

/*************************************************
Summary: Gets the Tension of the curve at this point
Return Value: Tension
Remarks:

See Also: SetBias,SetTension,SetContinuity
*************************************************/
float GetTension()		const	{ return m_Tension; }

/*************************************************
Summary: Sets the Tension of the curve at this point
Arguments:
	t: Tension value
Remarks:

See Also: SetBias,GetTension,SetContinuity
*************************************************/
void  SetTension(float t)	{ m_Tension=t; NotifyUpdate(); }

/*************************************************
Summary: Gets the Continuity of the curve at this point
Return Value: Continuity
Remarks:

See Also: SetBias,SetTension,SetContinuity
*************************************************/
float GetContinuity()	const	{ return m_Continuity; }

/*************************************************
Summary: Sets the Continuity of the curve at this point
Arguments:
	c: Continuity value
Remarks:

See Also: SetBias,SetTension,GetContinuity
*************************************************/
void  SetContinuity(float c) { m_Continuity=c; NotifyUpdate(); }

/*************************************************
Summary: Checks whether the next segment of the curve is linear.

Arguments:
	linear: TRUE to have straight line from this point to the next.
Return Value:
	Returns TRUE if the curve will be a straight line to next point.
See Also:UseTCB,SetBias,SetTension,GetContinuity
*************************************************/
CKBOOL	 IsLinear()			const		{ return m_Flags & CK2DCURVEPOINT_LINEAR; }

void	 SetLinear(CKBOOL linear=FALSE) { if (linear) m_Flags|=CK2DCURVEPOINT_LINEAR; else m_Flags&=~CK2DCURVEPOINT_LINEAR; NotifyUpdate(); }

/*************************************************
Summary: Uses TCB data or explicit tangents
SetBias,SetTension,GetContinuity
Arguments:
	 use : TRUE to force usage of TCB data, FALSE to use tangents.
Remarks:
+ Each curve point has incoming and outgoing tangents. These tangents can be automatically calculated
using TCB parameters ( SetTension,SetContinuity,SetBias) or given explicitly through SetInTangent,SetOutTangent
See Also:SetTension,SetContinuity,SetBias,SetInTangent,SetOutTangent
*************************************************/
void UseTCB(CKBOOL use=TRUE)  { if (use) m_Flags&=~CK2DCURVEPOINT_USETANGENTS; else m_Flags|=CK2DCURVEPOINT_USETANGENTS; }

CKBOOL IsTCB()		const		 { return !(m_Flags & CK2DCURVEPOINT_USETANGENTS); }

//----------------------------------------------
// Position 

/*************************************************
Summary: Gets Curve length at this point.
Return value: Length of the curve.
Remarks;

*************************************************/
float GetLength()	const		 { return m_Length; }

/*************************************************
Summary: Returns the position of this control point.
Return Value:
	A reference to the position of this control point.
Remarks:

See also: SetPosition
*************************************************/
Vx2DVector&  GetPosition() { return m_Position; } 

/*************************************************
Summary: Sets the position of this control point.
Arguments:
	pos: A reference to a Vx2DVector that contain the new position of the point.
Remarks:
	+ Changing the position of a control point in a curve may change its 
	index in the list of control point of the curve as they are sorted by increasing x values.

See also: GetPosition
*************************************************/
void  SetPosition(const Vx2DVector& pos) { m_Position=pos; NotifyUpdate();} 

//----------------------------------------------
// Tangents

/*************************************************
Summary: Gets the incoming tangents at this control point.
Return Value:
	A reference on the incoming tangent
Remarks:
	+ If the control point is using TCB parameters the tangents will be
	automatically computed by the curve

See also: GetInTangent,SetOutTangent
*************************************************/
Vx2DVector& GetInTangent() { return m_InTang;  }

/*************************************************
Summary: Gets the outgoing tangents at this control point.
Return Value:
	A reference on the outgoing tangent
Remarks:
	+ If the control point is using TCB parameters the tangents will be
	automatically computed by the curve

See also: GetInTangent,SetOutTangent
*************************************************/
Vx2DVector& GetOutTangent() { return m_OutTang;  }


/*************************************************
Summary: Sets the incoming tangent
Arguments:
	out:A Vx2DVector describing the incoming
Remarks:
	+ If the control point is using TCB parameters the tangents will be
	automatically computed by the curve

See also: GetInTangent,GetOutTangent
*************************************************/
void SetInTangent(const Vx2DVector& in) { m_InTang = in; NotifyUpdate(); }

/*************************************************
Summary: Sets the outgoing tangent
Arguments:
	out:A Vx2DVector describing the outgoing tangent
Remarks:
	+ If the control point is using TCB parameters the tangents will be
	automatically computed by the curve

See also: GetInTangent,GetOutTangent
*************************************************/
void SetOutTangent(const Vx2DVector& out) { m_OutTang = out; NotifyUpdate(); }


void NotifyUpdate();  														


	CK2dCurvePoint() {
		m_Tension	= m_Continuity = m_Bias		= 0.0f;
		m_Curve		= NULL;
		m_Flags		= 0;
	}

protected:
	
	void SetCurve(CK2dCurve* curve)	 { m_Curve=curve; }								
	void SetLength(float l)			 { m_Length=l; }								
	Vx2DVector& GetRCurvePos()		 { return m_RCurvePos; }							
	void SetRCurvePos(Vx2DVector& v) { m_RCurvePos=v; }							
	void Read(CKStateChunk *chunk);		
	
	CK2dCurve* m_Curve;				// Owner curve	
	float m_Tension,m_Continuity,m_Bias;	// t,c,b, parameters	
	float m_Length;					// Length of the curve at this control point
	Vx2DVector m_Position;			// Control point Position
	Vx2DVector m_InTang,m_OutTang;	// Incoming and outgoing tangents (may be overriden by t,c,b parameters)
	Vx2DVector m_RCurvePos;			// Real curve pos (according to curve fittig coef)
	CKDWORD	   m_Flags;				// Linear and Usetangents flags
};

#endif
