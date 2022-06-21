/*************************************************************************/
/*	File : CKTargetLight.h			 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKTARGETLIGHT_H

#define CKTARGETLIGHT_H "$Id:$"

#include "CKLight.h"



/*************************************************
Name: CKTargetLight

Summary: A light with a target

Remarks:
	+ A CKTargetLight is a light with a target. This class is derived from the
	CKLight class. It manages a target entity, instance of CK3dEntity, to which it always points.
	If the target is undefined, the light behaves like a standard light.

	+ At creation time, the instance of CKTargetLight has no target.

	+ The class identifier of CKTargetLight is CKCID_TARGETLIGHT.



See also: 
*************************************************/
class CKTargetLight : public CKLight {
public :

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
static CKTargetLight* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_TARGETLIGHT)?(CKTargetLight*)iO:NULL;
}

};

#endif
