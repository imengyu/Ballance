/*************************************************************************/
/*	File : CKKinematicChain.h											 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKKINEMATICCHAIN_H

#define CKKINEMATICCHAIN_H "$Id:$"


#include "CKBodyPart.h"


/*************************************************
Name: CKKinematicChain

Summary: Kinematic Chain for Inverse Kinematic

Remarks: 
+ This class provides methods for applying IK to a chain of 
hierachical bodyparts. To define a KinematicChain you should 
specify a start effector (parent) and a end effector (child),
the chain is defined by all the bodyparts that are between 
parent and child in the hierarchy. If the two effectors
are not linked directly or indirectly via the hierarchy an error
occurs.

+ Once defined you can use KinematicChain methods to make any
bodypart of the chain move toward a desired position using IK, that is 
moving all the bodyparts of the chain according to their joint data
so that the designed bodypart position is equal to the desired position.

+ The class id of CKKinematicChain is CKCID_KINEMATICCHAIN.



See also:CKBodyPart,CKCharacter
*************************************************/
class CKKinematicChain : public CKObject {
public :

//-------------------------------------------------------
// info functions

/*************************************************
Summary: Returns the kinematic chain length.
Arguments:
	End: Ending effector of the chain, NULL for the default ending effector.
Remarks:
	+ Returns the length of the kinematic chain from the starting bodypart
	to the ending bodypart.
See also:SetStartEffector,GetChainBodyCount,SetEndEffector
*************************************************/
virtual float	GetChainLength(CKBodyPart *End=NULL) = 0;

/*************************************************
Summary: Returns the number body parts in this chain.
Arguments:
	End: Ending effector of the chain, NULL for the default ending effector
Remarks:
	+ Returns the number of bodypart from the starting bodypart
	to the ending bodypart.
See also:SetStartEffector, GetChainLength,SetEndEffector
*************************************************/
virtual int		GetChainBodyCount(CKBodyPart *End=NULL) = 0;

//-------------------------------------------------------
// Effectors functions

/*************************************************
Summary: Returns the start effector of this chain.
Return Value:
	A CKBodyPart pointer of the start effector.
See also:SetStartEffector,SetEndEffector
*************************************************/
virtual CKBodyPart* GetStartEffector() = 0;

/*************************************************
Summary: Sets the start effector of this chain.
Arguments:
	A pointer to the CKBodyPart which will be the start effector of this Kinematic Chain.
Return Value:
	CK_OK if successful, error code otherwise.
See also:GetStartEffector,SetEndEffector
*************************************************/
virtual CKERROR		SetStartEffector(CKBodyPart*) = 0;

/*************************************************
Summary: Returns an effector at a given index
Arguments:
	pos: Index of the effector to be obtained.
Remarks:
	GetEffector(0) returns the start effector and 
	GetEffector(GetChainBodyCount()-1) returns the ending effector.
See also:GetChainBodyCount
*************************************************/
virtual CKBodyPart* GetEffector(int pos) = 0;

/*************************************************
Summary: Returns the ending effector of this chain
Return Value:
	A CKBodyPart pointer of the end effector.
See also:GetStartEffector,SetEndEffector
*************************************************/
virtual CKBodyPart* GetEndEffector() = 0;

/*************************************************
Summary: Sets the ending effector of this chain.
Arguments:
	A pointer to the CKBodyPart which will be the end effector of this Kinematic Chain.
Return Value:
	CK_OK if successful, error code otherwise.
See also:SetStartEffector,GetEndEffector
*************************************************/
virtual CKERROR		SetEndEffector(CKBodyPart*) = 0;

//---------------------------------------------------------
// Move end effector (or body if specified ) so that its position
// goes to pos (in the referential ref : World by default ) 

/*************************************************
Summary: Sets the position of an effector in the kinematic chain.

Arguments:
	pos: A vector representing the targeted position.
	ref: Reference 3D Entity in which pos is given or NULL for world coordinates.
 	body: A pointer to BodyPart of the chain that should be moved to pos.
Return Value:
	CK_OK if successful, error code otherwise.
Remarks:
+ This methods moves all the bodyparts from the start effector to body so that body position
goes to pos.
+ If body is not specified, the end effector is moved to pos.

See also:CKBodyPart, CKCharater
*************************************************/
virtual  CKERROR  IKSetEffectorPos(VxVector *pos,CK3dEntity *ref=NULL,CKBodyPart *body=NULL) = 0;


CKKinematicChain(CKContext *Context,CKSTRING name=NULL) : CKObject(Context,name) {}	

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
static CKKinematicChain* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_KINEMATICCHAIN)?(CKKinematicChain*)iO:NULL;
}
};

#endif

