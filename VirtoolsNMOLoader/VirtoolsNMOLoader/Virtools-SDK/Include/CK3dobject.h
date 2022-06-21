/*************************************************************************/
/*	File : CK3dObject.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CK3DOBJECT_H

#define CK3DOBJECT_H "$Id:$"

#include "CK3dEntity.h"

/***************************************************************************
{filename:CK3dobject}
Name: CK3dObject

Summary: Base class for 3D objects with geometry

Remarks:
	+ The CK3dObject does not yet provide new functions compare to CK3dEntity 

	+ You are advised to use CK3dObjects for all of
	your real 3D objects with a geometry. The loading plug-ins of Virtools should all create instances of CK3dObject 
	when loading models imported from foreign formats. Even though you can instanciate CK3dEntity directly, using a
	CK3dObject provides you with additionnal services that may be increased in future.

	+ The class id of CK3dObject is CKCID_3DOBJECT.



See also: CK3dEntity
***************************************************************************/
class CK3dObject : public CK3dEntity {
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
static CK3dObject* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_3DOBJECT)?(CK3dObject*)iO:NULL;
}

};

#endif
