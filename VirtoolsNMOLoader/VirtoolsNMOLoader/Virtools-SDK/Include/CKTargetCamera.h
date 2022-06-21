/*************************************************************************/
/*	File : CKTargetCamera.h			 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKTARGETCAMERA_H

#define CKTARGETCAMERA_H "$Id:$"

#include "CKCamera.h"

/*************************************************
Name: CKTargetCamera

Summary: A camera with a target

Remarks:
A target camera is derived from a CKCamera, and has a target: it always points to its
target 3D entity.
When the target is undefined, the camera behaves like a standard camera.

The class id of CKCamera is CKCID_TARGETCAMERA.



See also: 
*************************************************/
class CKTargetCamera : public CKCamera {
public:

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
static CKTargetCamera* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_TARGETCAMERA)?(CKTargetCamera*)iO:NULL;
}

};

#endif
