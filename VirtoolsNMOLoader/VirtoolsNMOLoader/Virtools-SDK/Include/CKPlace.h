/*************************************************************************/
/*	File : CKPlace.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CKPLACE_H) || defined(CK_3DIMPLEMENTATION)

#define CKPLACE_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CKBeObject.h"
#include "XObjectArray.h"



#undef CK_PURE

#define CK_PURE = 0


/****************************************************************************
{filename:CKPlace}
Summary: Group of 3D entities.

Remarks:
	+ CKPlaces are the basic bricks for the logical grouping of 3D entities, and for attaching behaviors to groups of entities.
	
	+ Every objects that are children of a place are marked as belonging to this place.

	+ 3D entities, like ligths, cameras, 3D objects, characters, etc... are logically grouped into places.
	A 3d entity cannot belong to more than one place, but some entities may be attached to no place. They
	will be displayed and activated if they are global to the curent scene or level or to the whole game.

	+ Portals can be used to declare the links between places so that a quick rejection
	can be performed by a portal manager to remove unseen places from a given place.

	+ Portals are represented by a CK3dEntity and are considered as a unit box in the entity referential.

	+ The class id of CKPlace is CKCID_PLACE.

See also: CKLevel,CK3dEntity
*****************************************************************************/
class CKPlace : public CK3dEntity {
public:
#endif


//-----------------------------------------------------
// Camera


virtual  CKCamera * GetDefaultCamera() CK_PURE;

virtual  void	   SetDefaultCamera(CKCamera *cam) CK_PURE;

//-----------------------------------------------------
// Portals

/********************************************************
Summary: Adds a portal.

Arguments:
	place: A pointer to the CKPlace that is seen through the given portal.
	portal: A pointer to the CK3DEntity that represents the portal extents.
Remarks:
+ There can be more than one portal linking this place to a given place.
+ Adding a portal between one place and another automatically adds the 
portal from the other place to this one.
+ if portal is NULL, the given place is considered to be seen wherever 
the camera is in this place.
See Also:RemovePortal,GetPortalCount,GetPortal,ViewportClip
*********************************************************/
virtual  void		AddPortal(CKPlace* place,CK3dEntity* portal) CK_PURE;
/********************************************************
Summary: Removes a portal.

Arguments:
	place: A pointer to the CKPlace that is seen through the given portal.
	portal: A pointer to the CK3DEntity that represents the portal extents.
See Also:AddPortal,GetPortalCount,GetPortal,ViewportClip
*********************************************************/
virtual  void		RemovePortal(CKPlace* place,CK3dEntity* portal) CK_PURE;
/********************************************************
Summary: Returns the number of portals in this place.

Return Value:
	Number of portals in this place.
See Also:AddPortal,GetPortalCount,GetPortal,ViewportClip
*********************************************************/
virtual  int		GetPortalCount() CK_PURE;
/************************************************
Summary: Returns the place seen by a given portal.
Arguments:
	i: Index of the portal to return.
	portal: Address of a pointer to a CK3dEntity that will be filled with the result portal.
Return Value:
	A pointer to the place seen by the portal.

See Also:AddPortal,GetPortalCount,ViewportClip
************************************************/
virtual  CKPlace*	GetPortal(int i,CK3dEntity** portal) CK_PURE;
/************************************************
Summary: Clipping rectangle.
Return Value:
	A VxRect that contains the extents that can be seen when in this place.
Remarks:
+ The portal manager updates this clipping rect when parsing the places.
+ For example if a given place as only one portal the clipping rect will
be reduce to enclose only this portal so that rendering of the remaining 
places are clipped to this portal.
See Also:AddPortal,GetPortalCount,GetPortal
************************************************/
virtual  VxRect&	ViewportClip() CK_PURE;
/************************************************
Summary: Automatically computes a matrix standing for the portal boundaries 
between place1 and place2.
Arguments:
p2: Second place.
BBoxMatrix: a reference to a matrix in which the calculation will be put.
Return Value:
	TRUE if the bounding box matrix was generated correctly,
	FALSE if some problem occured.
Remarks:
+ The position and orientation of BBoxMatrix will match the best fitting bounding 
box between Place1 and Place2.
+ The scale of BBoxMatrix match the world half-size of the best fitting bounding 
box between Place1 and Place2.
+ More simply, if you set an unitary object's world matrix to BBoxMatrix 
after calling ComputeBestFitBBox() function, this object (if it's a cube) should
wrap up the common vertices between Place1 and Place2.
See Also:AddPortal
************************************************/
virtual CKBOOL ComputeBestFitBBox( CKPlace *p2, VxMatrix &BBoxMatrix ) CK_PURE;

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
static CKPlace* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_PLACE)?(CKPlace*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
