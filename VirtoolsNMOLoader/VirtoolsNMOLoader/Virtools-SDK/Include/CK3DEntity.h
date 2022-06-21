/*************************************************************************/
/*	File : CK3dEntity.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CK3DENTITY_H) || defined(CK_3DIMPLEMENTATION)

#define CK3DENTITY_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION


#include "CKRenderObject.h"
#include "CKSkin.h"
/*  #include "CKMesh.h" */


#undef CK_PURE

#define CK_PURE = 0


#define CKRENDER_UPDATEEXTENTS 0x00000FF

#define CKRENDER_DONTSETMATRIX 0x0000100

/*************************************************
{filename:CK3dentity}
Name: CK3dEntity

Summary: 3D objects with behaviors

Remarks:
	+ The CK3dEntity class is the base class for all behavioral 3D objects.	Basically it inherits its behaviors from CKBeObject. 
It also provides management for a set of potential meshes, and animations.

	+ In order to be visible a CK3dEntity must be added to the current level through CKLevel::AddObject.
	or if the Entity should not be referenced in the Level through CKRenderContext::AddObject.
	Adding a 3dEntity to a Level or Scene automatically adds its meshes to the Level/Scene.

	+ CK3dEntity class provides several functions for specifying position,orientation and scale of 
	 the entity plus an acces to local and world matrix.

	+ A CK3dEntity may have several callback functions called before and after rendering and
	the render function may also be replaced by an user function.

	+ A CK3dEntity is displayed using a mesh, called the current mesh.	At creation time,
	no mesh is defined. A mesh should be provided before any display is done.
	The current mesh can be changed at any time.

	+ A CK3dEntity also maintains a set of meshes that could be used for displaying. The current
	mesh does not need to be part of this set. This set may be used for example to store the representation
	of the 3D entity at different resolutions.

	+ The CK3dEntity maintains a list of CKObjectAnimation, which can be accessed and driven by the
	behaviors attached to the object. The animations can be undefined if no 
	behavior try to access it. The animations can be changed at any time, even
	while a behavior is active on it. An CKObjectAnimation may also be part of a CKKeyedAnimation
	(group of animations used to animate a character)  

	+ The CK3dEntity may be visible or not. This has no impact on its behaviors. Running behaviors
	continue to run even if the entity is hidden.

	+ Its class id is CKCID_3DENTITY




See also: Using 3D Entities,CKMesh,
*************************************************/
class CK3dEntity : public CKRenderObject {
public:
#endif
//-------------------------------------------------------
// HIERARCHY

/************************************************
Summary: Returns the number of children entity of this entity.

Return Value: The number of children.
Remarks:
See also: GetChildren,GetChild
***********************************************/	
virtual	int			GetChildrenCount() const CK_PURE;
/************************************************
Summary: Returns child entity according to its index in children list
Arguments:
	pos: Index of the child entity to return
Return Value: Child entity.
Remarks:

See also: GetChildrenCount,GetChildren
************************************************/
virtual	CK3dEntity*	GetChild(int pos) const CK_PURE;
/************************************************
Summary: Attaches the entity to a new parent in the scene hierarchy.
Arguments:
	Parent: New parent of the entity, if NULL this entity becomes a root object.
	KeepWorldPos: TRUE if this entity must keep the same world position and orientation.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
	+ If this entity was previously child of another entity, it is automatically detached
	from its previous parent.

See also: GetParent,AddChild,RemoveChild,GetChild
************************************************/	
virtual CKBOOL		SetParent(CK3dEntity *Parent,CKBOOL KeepWorldPos=TRUE) CK_PURE;
/*************************************************
Summary: Returns the parent entity.
Return Value: A pointer to the parent entity or NULL if this entity is a root of the hierarchy.

See also: AddChildren, RemoveChild, SetParent 
*************************************************/	
virtual CK3dEntity*	GetParent() const CK_PURE;
/************************************************
Summary: Attaches an entity as child of this entity.

Arguments:
	Child: Moveable to add as a child of this moveable in the hierarchy.
	KeepWorldPos: TRUE if this moveable must keep the same world position and orientation.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
o Trying to add a child twice to the same entity returns an error. If Child was already child of
another entity,it is detached from its previous parent.
o This method does the exact same thing than Child->SetParent(this)
See also: AddChildren, RemoveChild, SetParent, GetParent
************************************************/	
virtual CKBOOL	AddChild(CK3dEntity *Child,CKBOOL KeepWorldPos=TRUE) CK_PURE;
/************************************************
Summary: Attaches a list of entities as children of this entity.
Arguments:
	Children: An array of 3d entity to add as a children of this moveable in the hierarchy.
	KeepWorldPos: TRUE if this entity must keep the same world position and orientation.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
o This method takes a list of objects and attaches them as children of this entity.
o If some of the entities in the list are already hierarchised the method will not break
the current hierarchy amongst the entities (only top level entities will be attached).

See also: Addchild, RemoveChild, SetParent, GetParent
************************************************/	
virtual CKBOOL	AddChildren(const XObjectPointerArray& Children,CKBOOL KeepWorldPos=TRUE) CK_PURE;
/************************************************
Summary: Detaches a child entity from its parent.
Arguments:
	Mov: Entity to remove from the children list.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
	+ Trying to remove moveable which is not a child of this moveable returns FALSE.
	+ Once removed, Mov does not have any parent.

See also: AddChild, GetChildren, SetParent, GetParent
************************************************/
virtual CKBOOL	RemoveChild(CK3dEntity *Mov) CK_PURE;
/*************************************************
Summary: Compares the hierarchy of two entities.
Arguments:
	Mov: A CK3dentity to compare with this entity.
	SameOrder: Each children must be inthe same order in both entities.
Remarks:
	+This method compares the hierarchy of two entity.
	+If SameOrder is TRUE two hierarchy are considered the same if each child of Mov has an equal 
	number of child of its counterpart (ie  the first child of this entity has the same hierarchy than the first child of Mov,etc..)
	+If SameOrder is FALSE the order of child is less important.
	+This method can be used to compare the hierarchy of two characters, if they are 
	the same it is likely that animations could be shared from one to another.
Return Value:
	TRUE if the two entities have a similar hierarchy.

See Also: GetChild,GetChildren,GetParent
*************************************************/	
virtual CKBOOL		CheckIfSameKindOfHierarchy(CK3dEntity* Mov,CKBOOL SameOrder=FALSE) const CK_PURE;
/************************************************
Summary: Parses the sub hierarchy of an entity.

Arguments:
	current: Current object in the hierarchy (must start at NULL).
Return Value: Child entity.
Remarks:
	+ This method enables you to iterate among the complete hierarchy  of an 
	entity.


Example:
		// This sample iterates the hierarchy of Entity  
		// and search for a specific mesh...
			
		CK3dEntity* Child = NULL;
		CK3dEntity* Result = NULL;
			
		while (Child = Entity->HierarchyParser(Child)) {
			if (Child->GetCurrentMesh() == MeshToFound) {
				Result = Child;
			}
		}

See also: GetChildrenCount,GetChildren
************************************************/
virtual	CK3dEntity*		HierarchyParser(CK3dEntity* current) const CK_PURE;


//-------------------------------------------------------
// Flags settings 

/*************************************************
Summary: Returns the entity flags

Return Value:
	 A CKDWORD containing a combination of CK_3DENTITY_FLAGS flags
Remarks:
CK_3DENTITY_FLAGS flags contain information about special type of entity such as 
dummy objects,camera or light targets,etc..
See also: CK_3DENTITY_FLAGS,SetFlags
*************************************************/		
virtual CKDWORD GetFlags() const CK_PURE;

/*************************************************
Summary: Sets the entity flags

Arguments:
	Flags : A CKDWORD containing a combination of CK_3DENTITY_FLAGS flags
Remarks:
CK_3DENTITY_FLAGS flags contain information about special type of entity such as 
dummy objects,camera or light targets,etc..
See also: CK_3DENTITY_FLAGS,GetFlags
*************************************************/	
virtual void    SetFlags(CKDWORD Flags) CK_PURE;
/************************************************
Summary: Sets if the entity should be pickable

Arguments:
	Pick: TRUE if pickable, FALSE if not.
Remarks:
o If the flag is set to FALSE, this object is not taken into account by
CKRenderContext::Pick method. This way, if picking occurs on this object, nothing will be returnd.
o This method sets or removes the VX_MOVEABLE_PICKABLE moveable flag.

See also: VX_MOVEABLE_FLAGS,CKRenderContext::Pick, IsPickable
************************************************/
virtual void	SetPickable(CKBOOL Pick=TRUE) CK_PURE;
/************************************************
Summary: Returns the state of picking flag
Return Value: TRUE if pickable, FALSE if not.
Remarks:

See also: CKRenderContext::Pick, IsPickable
************************************************/
virtual CKBOOL	IsPickable() const CK_PURE;

/************************************************
Summary: Checks if the moveable and its associated mesh are in view frustrum.
Arguments:
	Dev: A CKRenderContext which will be used to test the moveable.
	Flags: CKRENDER_UPDATEEXTENTS is the screen extents of the object should be updated
				   and/or CKRENDER_DONTSETMATRIX if the world transformation matrix should not be modified
Return Value: TRUE if moveable is visible, FALSE if not.
Remarks:
	+This function tests the bounding box of the object against the viewing frustrum. It returns 
	TRUE if the object bounding box is partially visible.
	+if Flags contains CKRENDER_UPDATEEXTENTS the bounding box is projected onto the screen 
	to find the extents of the object on screen.
	+if Flags contains CKRENDER_DONTSETMATRIX the current world transformation matrix 
	(See CKRenderContext::SetWorldTransformationMatrix) is left as it is otherwise it is set 
	to the world matrix of this entity.

See also: IsInViewFrustrumHierarchic,IsAllOutsideFrustrum,IsAllInsideFrustrum
************************************************/	
virtual	CKBOOL	IsInViewFrustrum(CKRenderContext* Dev,CKDWORD Flags = 0) CK_PURE;
/************************************************
Summary: Checks if the moveable and its children are in the view frustrum.
Arguments:
	Dev: A CKRenderContext which will be used to test the moveable.
Return Value: TRUE if moveable and its hierarchy are partially is visible, FALSE if not.
Remarks:
	+ This function tests the bounding box of the object and all its hirearchy against the viewing frustrum. It returns 
	TRUE if the bounding box is partially visible.

See also: IsInViewFrustrum,IsAllOutsideFrustrum,IsAllInsideFrustrum
************************************************/	
virtual	CKBOOL	IsInViewFrustrumHierarchic(CKRenderContext* Dev) CK_PURE;
/*************************************************
Summary: Excludes this entity from animations.
Arguments:
	ignore: TRUE if animations should not modify this object.
Remarks:
o If ignore is set to TRUE the entity position and orientation will not be modifiable
anymore by animations.
o This method sets or removes the flag CK_3DENTITY_IGNOREANIMATION

*************************************************/	
virtual	void	IgnoreAnimations(CKBOOL ignore=TRUE) CK_PURE;

virtual	CKBOOL	AreAnimationIgnored() const CK_PURE;
/************************************************
Summary: Returns whether the object is entirely inside the view frustum
Return Value: TRUE if object is totally visible, FALSE if not.
Remarks:
	+ This method only tests the presence of the VX_MOVEABLE_ALLINSIDE moveable flag which
	is automatically set by the IsInViewFrustrum,IsInViewFrustrumHierarchic and Render methods. 

See also: IsInViewFrustrum, IsInViewFrustrumHierarchic,Render,IsAllInsideFrustrum
************************************************/	
virtual	CKBOOL	IsAllInsideFrustrum() const CK_PURE;
/************************************************
Summary: return whether the object is entirely outside the view frustum
Return Value: TRUE if object is totally clipped, FALSE if not.
Remarks:
	+ This method only tests the presence of the VX_MOVEABLE_OFFSCREEN moveable flag which
	is automatically set by the IsInViewFrustrum,IsInViewFrustrumHierarchic and Render methods. 

See also: IsInViewFrustrum, IsInViewFrustrumHierarchic,Render,IsAllInsideFrustrum
************************************************/	
virtual	CKBOOL	IsAllOutsideFrustrum() const CK_PURE;
/*************************************************
Summary: Makes this entity to be rendered as other transparents objects

Arguments:
	Trans: A Boolean indicating if it should be rendered as transparent objects.
Remarks:
o Render Engines may execute special processing for transparents object: In most
case they are sorted in Z order before being rendered and are rendered after all 
non-transparent object.
o This function sets VX_MOVEABLE_RENDERLAST moveable flags.
See also: VX_MOVEABLE_FLAGS,GetMoveableFlags,SetMoveableFlags,ModifyMoveableFlags
*************************************************/
virtual void	SetRenderAsTransparent(CKBOOL Trans=TRUE) CK_PURE;
/*************************************************
Summary: Sets the moveable flags

Return Value:
	A CKDWORD containing a combination of the entity VX_MOVEABLE_FLAGS flags
Arguments:
	flags : A CKDWORD containing a combination of VX_MOVEABLE_FLAGS flags
Remarks:
o Moveable Flags are used internally by the render engine 
o In order to change a flags you should rather use ModifyMoveableFlags
See also: VX_MOVEABLE_FLAGS,GetMoveableFlags,ModifyMoveableFlags
*************************************************/
virtual CKDWORD	GetMoveableFlags() const CK_PURE;

virtual void	SetMoveableFlags(CKDWORD flags) CK_PURE;
/*************************************************
Summary: Changes the entity flags
Return Value:
	A CKDWORD containing a combination of the entity new VX_MOVEABLE_FLAGS flags
Arguments:
	Add	   : A CKDWORD containing a combination of VX_MOVEABLE_FLAGS flags to add.
	Remove : A CKDWORD containing a combination of VX_MOVEABLE_FLAGS flags to add.
Remarks:
+ In order to change a flags you should rather use ModifyMoveableFlags than SetMoveableFlags
+ This flags contain hints or render settings that can be given to the render engine 

See also: VX_MOVEABLE_FLAGS,GetMoveableFlags,ModifyMoveableFlags
*************************************************/
virtual CKDWORD	ModifyMoveableFlags(CKDWORD Add,CKDWORD Remove) CK_PURE;

//-------------------------------------------------------
// MESHES 

/*************************************************
Summary: Returns the current mesh.

Return Value:
	The mesh that is currently used to render this entity.
See Also:Using Meshes,SetCurrentMesh,GetMeshCount,GetMesh,AddMesh
*************************************************/	
virtual CKMesh* GetCurrentMesh() const CK_PURE;
/*************************************************
Summary: Sets the current mesh of this entity.

Return Value:
	The mesh that was currently used to render this entity.
Arguments:
	m: A pointer to the CKMesh to use for rendering.
	add_if_not_here: If TRUE adds m to the list of meshes of this entity.
Remarks:
The current mesh of the entity does not have to be part of the set of potential
meshes (temporary representation for example), in which case add_if_not_here can be set to FALSE.
See Also:Using Meshes,GetCurrentMesh,GetMeshCount,GetMesh,AddMesh
*************************************************/	
virtual CKMesh* SetCurrentMesh(CKMesh *m,CKBOOL add_if_not_here=TRUE) CK_PURE;

/*************************************************
Summary: Returns the number of meshes in this entity.

Remarks:
	+ An entity can have several potential meshes attached to it, only can be used for rendering 
	but this list can be used to have level of detail on an entity.
Return Value: 
	Number of meshes on this entity.
See Also:Using Meshes,GetCurrentMesh,SetCurrentMesh,GetMesh,AddMesh
*************************************************/	
virtual int GetMeshCount() const CK_PURE;
/*************************************************
Summary: Returns a potential mesh according to its index.
Arguments:
	pos: Index of the mesh to return.	
Remarks:
	+ An entity has a set of potential meshes attached to it, only one can be used for rendering 
	but this set can be used by behavior to manage level of detail.
Return Value:
	A pointer to a CKMesh

See Also:Using Meshes,GetCurrentMesh,SetCurrentMesh,GetMesh,AddMesh
*************************************************/	
virtual CKMesh*  GetMesh(int pos) const CK_PURE;
/*************************************************
Summary: Adds the given mesh to the set of potential meshes
Arguments:
	mesh: A pointer to the CKMesh to add.
Return Value:
	CK_OK if success.
Remarks:
	+ An entity has a set of potential meshes attached to it, only one can be used for rendering 
	but this set can be used by behavior to manage level of detail.

See Also:Using Meshes,GetCurrentMesh,SetCurrentMesh,GetMesh,GetMeshCount
*************************************************/	
virtual CKERROR AddMesh(CKMesh *mesh) CK_PURE;
/*************************************************
Summary: Removes the given mesh from the set of potential meshes
Arguments:
	mesh: A pointer to the CKMesh to remove
Return Value:
	CK_OK if success.
Remarks:
	+ An entity has a set of potential meshes attached to it, only one can be used for rendering 
		but this set can be used by behavior to manage level of detail.

See Also:Using Meshes,GetCurrentMesh,SetCurrentMesh,GetMesh,GetMeshCount,AddMesh
*************************************************/	
virtual CKERROR RemoveMesh(CKMesh *mesh) CK_PURE;

//-------------------------------------------------------
// ORIENTATION POSITION SCALE 
// Default Referentiel : World										   
// Default  behavior: KeepChildren =FALSE  if an object moves its		
// children move also												
//-------------------------------------------------------

/************************************************
Summary: Changes the orientation of this entity so that it faces a point in space. 

Arguments:
	Pos: A VxVector which gives the position of the point to look at.
	Ref: The referential entity in which Pos is given.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
o If referential is NULL, the position is defined in world coordinates, otherwise the
position is defined in the moveable Ref referential.
o When KeepChildren is set to TRUE, children entities of this object will keep their current position.
See also: SetOrientation, GetOrientation
************************************************/
virtual	void	LookAt(const VxVector *Pos,CK3dEntity *Ref=NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
/************************************************
Summary: Rotates the entity around an axis.

Arguments:
	Axis: A VxVector defining the axis around which to rotate.
	Angle: Angle of rotation.
	Ref: The referential entity.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
o If the referential is NULL, the axis is defined in world coordinates, otherwise it's
defined in the moveable Ref referential.
o When KeepChildren is set to TRUE, children entities of this object will keep their current position.
See also: SetOrientation, GetOrientation
************************************************/	
virtual void	Rotate(const VxVector *Axis,float Angle,CK3dEntity *Ref = NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
virtual	void	Rotate(float X,float Y,float Z,float Angle,CK3dEntity *Ref = NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
/************************************************
Summary: Translates the entity.
Arguments:
	Vect: A vector defining the amount to translate the moveable
	Ref: The referential moveable.in which the translation vector is defined.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
o If referential is NULL, Vect is defined in world coordinates, otherwise the
position is defined in the moveable Ref referential.
o When KeepChildren is set to TRUE, children entities of this object will keep their current position/orientation.

See also: SetPosition, GetPosition.
************************************************/	
virtual void	Translate(const VxVector *Vect,CK3dEntity *Ref = NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
virtual void	Translate(float X,float Y,float Z,CK3dEntity *Ref = NULL,CKBOOL KeepChildren=FALSE) CK_PURE;

/************************************************
Summary: Adds scaling to the entity.

Arguments:
	Scale: A VxVector representing the scale factor for each axis.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
	+ When KeepChildren is set to TRUE, children entities of this object will keep their current position/orientation.
See also: GetScale, AddScale
************************************************/
virtual void	AddScale(const VxVector *Scale,CKBOOL KeepChildren=FALSE,CKBOOL Local=TRUE) CK_PURE;
virtual void	AddScale(float X,float Y,float Z,CKBOOL KeepChildren=FALSE,CKBOOL Local=TRUE) CK_PURE;
/************************************************
Summary: Sets the position of the moveable

Arguments:
	Pos: A vector defining the new position of the moveable
	Ref: The referential moveable.
	KeepChildren: TRUE if NOT applied to children, FALSE(Default) otherwise.
Remarks:
+ If referential is NULL, the position is given in world coordinates, otherwise the
position is defined in the entity Ref referential.
+ When KeepChildren is set to TRUE, children entity of this object will keep their current position.
See also: Translate
************************************************/	
virtual void	SetPosition(const VxVector *Pos,CK3dEntity *Ref=NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
virtual void	SetPosition(float X,float Y,float Z,CK3dEntity *Ref=NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
/*************************************************
Summary: Returns the entity position in a given referential.
Arguments:
	Pos: A pointer to a VxVector that will be filled with the position.
	Ref: An entity in which Pos will be defined.

See Also:SetPosition,GetWorldMatrix
*************************************************/	
virtual void	GetPosition(VxVector *Pos,CK3dEntity *Ref = NULL) const CK_PURE;
/************************************************
Summary: Sets the orientation of the entity.

Arguments:
	Dir: A vector defining the z axis (Direction) of the entity.
	Up: A vector defining the y axis ( Up Vector) of the entity.
	Right: A vector defining the x axis ( Right Vector) of the entity.
	Ref: The referential entity.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise

Remarks:
	+If referential is NULL, the vectors are defined in world coordinates, otherwise they are
	defined in the entity Ref referential.
	+When KeepChildren is set to TRUE, SetOrientation will not modify children current orientation.
	+Note that the vectors defining the axis do not need to be normalized.
See also: GetOrientation, Rotate
************************************************/	
virtual void	SetOrientation(const VxVector *Dir,const VxVector *Up,const VxVector *Right=NULL,CK3dEntity *Ref = NULL,CKBOOL KeepChildren=FALSE) CK_PURE;
/*************************************************
Summary: Returns the entity orientation axes.
Arguments:
	Dir: A vector that will receive the z axis (Direction) of the entity.
	Up: A vector that will receive the y axis ( Up Vector) of the entity.
	Right: A vector that will receive the x axis ( Right Vector) of the entity.
	Ref: The referential entity in which the vectors will be defined.
Reamrks:
	+ All arguments are optionnal and be set to NULL. 	

*************************************************/	
virtual void	GetOrientation(VxVector *Dir,VxVector *Up,VxVector *Right=NULL,CK3dEntity *Ref = NULL) CK_PURE;
/************************************************
Summary: Sets the orientation of the entity according to a quaternion.

Arguments:
	Quat: A VxQuaternion which gives the new orientation.
	Ref: The referential entity.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
	KeepScale: TRUE if the current scale should be kept,FALSE to reset scale to (1,1,1).
Remarks:
+ When KeepChildren is set to TRUE, SetQuaternion will not modify children current orientation.
+ If referential is NULL, the quaternion defines an orientation relative to the world
otherwise the orientation is relative to the moveable Ref referential.
See also: VxQuaternion,GetQuaternion,SetOrientation
************************************************/	
virtual void	SetQuaternion(const VxQuaternion *Quat,CK3dEntity *Ref=NULL,CKBOOL KeepChildren=FALSE,BOOL KeepScale=FALSE) CK_PURE;
/*************************************************
Summary: Returns the quaternion giving the entity orientation.
Arguments:
	Quat: A VxQuaternion that will receive the entity orientation.
	Ref:The referential entity.
Remarks:
	+ If referential is NULL, the quaternion defines an orientation relative to the world
	otherwise the orientation is relative to the moveable Ref referential.

See also: VxQuaternion,SetQuaternion,SetOrientation
*************************************************/	
virtual void	GetQuaternion(VxQuaternion *Quat,CK3dEntity *Ref=NULL) CK_PURE;
/************************************************
Summary: Sets the scale factors of the entity.
Arguments:
	Scale: A VxVector representing the scale factor for each axis.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
	Local: TRUE to set local axes scale (Default), FALSE to set world axis scale (can introduce shearing)
Remarks:
	+ When KeepChildren is set to TRUE, SetScale will not modify children current position/orientation/scale.


See also: GetScale, AddScale
************************************************/	
virtual void	SetScale(const VxVector *Scale,CKBOOL KeepChildren=FALSE,CKBOOL Local=TRUE) CK_PURE;
virtual void	SetScale(float X,float Y,float Z,CKBOOL KeepChildren=FALSE,CKBOOL Local=TRUE) CK_PURE;
/*************************************************
Summary: Returns the scale of this entity.
Arguments:
	Scale: A pointer to a VxVector that will be filled with the scale factors for each axis.
	Local: TRUE to get local axes scale (Default), FALSE to get world axes scale.
Remarks:

*************************************************/	
virtual void	GetScale(VxVector *Scale, CKBOOL Local=TRUE) CK_PURE;
/************************************************
Summary: Construct a new world or local matrix for the entity.
Arguments:
	Pos: Entity Position.
	Scale: Scale factors.
	Quat: Moveable orientation
	Shear: Scaling referential
Remarks:
	+ ConstructWorldMatrixEx and ConstructLocaldMatrixEx may generate not orthogonal matrices 
	due to the shear information resulting in deformations.
Return Value:
	TRUE if successful.

See also: SetPosition, SetScale, SetQuaternion
************************************************/	
virtual CKBOOL	ConstructWorldMatrix(const VxVector *Pos,const VxVector *Scale,const VxQuaternion *Quat) CK_PURE;

virtual CKBOOL	ConstructWorldMatrixEx(const VxVector *Pos,const VxVector *Scale,const VxQuaternion *Quat,const VxQuaternion *Shear,float Sign) CK_PURE;

virtual CKBOOL	ConstructLocalMatrix(const VxVector *Pos,const VxVector *Scale,const VxQuaternion *Quat) CK_PURE;

virtual CKBOOL	ConstructLocalMatrixEx(const VxVector *Pos,const VxVector *Scale,const VxQuaternion *Quat,const VxQuaternion *Shear,float Sign) CK_PURE;

//-------------------------------------------------------
// RENDERING

/************************************************
Summary: Renders the entity.

Arguments:
	Dev: A RenderContext which is used to render the entity.
	Flags: See remarks.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
+ This method is automatically called by the framework if the entity is referenced in the scene or 
render context.
+ This method works as following if entity is visible: first, it calls pre-render callbacks,
then it renders the mesh or it calls a user defined procedure and last, it calls post-render
callbacks. 
+ if Flags contains CKRENDER_UPDATEEXTENTS the bounding box is projected onto the screen 
to find the extents of the object on screen.
+ if Flags contains CKRENDER_DONTSETMATRIX the current world transformation matrix 
(See CKRenderContext::SetWorldTransformationMatrix) is left as it is otherwise it is set to the
current world matrix of the entity. It can be used to render this entity using a different matrix.
See also: CKRenderContext::Render,Understanding the Render Loop,IsInViewFrustrum
************************************************/
virtual 	CKBOOL	Render(CKRenderContext *Dev,CKDWORD Flags=CKRENDER_UPDATEEXTENTS) CK_PURE;

//-------------------------------------------------------
// INFO & MISC ACCES 

/************************************************
Summary: Checks if the entity is intersected by a given ray or segment.

Arguments:
	Pos1: A VxVector that gives the source of the ray.
	Pos2: Any point located on the ray, if Segment is TRUE this represent the ending point of the segment.
	Desc: The returned VxIntersectionDesc that describes the ray intersection results.
	Ref: The referential frame in which Pos1 and Pos2 are specified.
	iOptions: a combination of the CK_RAYINTERSECTION flags.
Return Value: 
	Returns 0 if not intersection occured or the number of faces intersected otherwise.

Remarks:
+Checks for intersection between the ray specified by points Pos1 and Pos2 where Pos1 is
the origin of the ray and Pos2 is a point on the ray (in Ref's coordinate system).
+Desc is the description of the intersection results (if there is one) in the entity's local coordinate
system.
+if NULL is given for Desc, then the first intersection found exits the function, returning 1 (and
not the real number of faces intersected) : This allow faster intersection when only occlusion
is looking for.
+If referential is NULL, the source vectos Pos1 and Pos2 are defined in world coordinates.
See also: VxIntersectionDesc
************************************************/	
virtual	int	RayIntersection(const VxVector *Pos1,const VxVector *Pos2,VxIntersectionDesc *Desc,CK3dEntity *Ref,CK_RAYINTERSECTION iOptions = CKRAYINTERSECTION_DEFAULT) CK_PURE;
/*************************************************
Summary: Returns the 2D extents of this entity on screen.

Arguments:
	rect: A reference to a VxRect that will be filled with the extents of the object on screen.
Remarks:
+Each time a object is rendered, its screen extents are computed, this method does not compute the
current extents but returns those that were computed the last time the object was rendered. 
*************************************************/	
virtual void	GetRenderExtents(VxRect& rect) const CK_PURE;

/****************************************************************************
Summary: Returns the world matrix of this object as it was last rendering frame.

Return Value:
	A reference to the saved matrix.
Remarks:
+The entity must have CK_3DENTITY_UPDATELASTFRAME flag set in order the matrix to be saved.
If the flag is set, the entity world matrix is saved each time a rendering occurs so that if someone
calls this method in a behavior it will have the position of this entity as it was last time it was rendered.
If the flag is not set and this method is called it will return the current world matrix.
See also: VxMatrix,SetLocalMatrix, SetWorldMatrix, GetWorldMatrix
***************************************************************************/
virtual const VxMatrix&	GetLastFrameMatrix() const CK_PURE;

//-------------------------------------------------------
// MATRIX ACCES 

/************************************************
Summary: Sets the local transformation matrix.

Arguments:
	Mat: A VxMatrix giving the orientation, position and scale relative to the entity's parent.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
+Setting the local transformation matrix automatically updates the world transformation
matrix of this entity and all its children (unless KeepChildren is set)
+When KeepChildren is set to TRUE, this method will not modify children current position/orientation.
See also: SetWorldMatrix,GetWorldMatrix,GetLocalMatrix,GetInverseWorldMatrix
************************************************/	
virtual	void	SetLocalMatrix(const VxMatrix& Mat,CKBOOL KeepChildren=FALSE) CK_PURE;
/*************************************************
Summary: Returns the local transformation matrix.

Return Value: A reference to the local matrix.
Remarks:
	+The local transformation matrix gives the position/orientation/scale of the 
	object relatively to its parent entity. If the entity does not have a parent its local
	matrix is equal to its world matrix.
See also: SetWorldMatrix,GetWorldMatrix,SetLocalMatrix,GetInverseWorldMatrix
*************************************************/	
virtual const VxMatrix&	GetLocalMatrix() const CK_PURE;
/************************************************
Summary: Sets the world transformation matrix.

Arguments:
	Mat: A VxMatrix giving the position,orientation and scale.
	KeepChildren: TRUE if NOT applied to children, FALSE otherwise
Remarks:
+ Setting the world transformation matrix automatically updates the local transformation
matrix of this entity and all its children (unless KeepChildren is set)
+ When KeepChildren is set to TRUE, this method will not modify children current position/orientation.
See also: SetLocalMatrix,GetLocalMatrix,GetWorldMatrix,GetInverseWorldMatrix
************************************************/	
virtual void	SetWorldMatrix(const VxMatrix& Mat,CKBOOL KeepChildren=FALSE) CK_PURE;
/*************************************************
Summary: Returns the world transformation matrix.

Return Value: A reference to the world matrix.
Remarks:
	+ The world transformation matrix gives the position/orientation/scale of the 
	object relatively to the world coordinate system.If the entity does not have a parent its local
	matrix is equal to its world matrix.
See also: SetLocalMatrix,GetLocalMatrix,GetWorldMatrix,GetInverseWorldMatrix
*************************************************/	
virtual const VxMatrix&	GetWorldMatrix() const CK_PURE;
/************************************************
Summary: Returns the inverse of the world transformation matrix.

Return Value: A reference to the inverse world matrix. 
See also: SetWorldMatrix, GetWorldMatrix, GetLocalMatrix, SetLocalMatrix
************************************************/	
virtual const VxMatrix&	GetInverseWorldMatrix() const CK_PURE;

//-------------------------------------------------------
// TRANSFORMATIONS 

/************************************************
Summary: Transforms a vector given in this coordinates system to another entity coordinates.

Arguments:
	Dest: A VxVector which will be filled with the transformed vector (can be the same pointer than Src).
	Src: The VxVector to be transformed.
	Ref: The referential entity.
Remarks:
+ If referential is NULL, the result vector Dest is defined in world coordinates.
+ The Transform method transforms a position vector (that is it takes entity position into account) while 
TransformVector transforms a normal vector (only rotation is taken).
See also: InverseTransform,TransformVector,TransformMany
************************************************/
virtual 	void	Transform(VxVector *Dest,const VxVector *Src,CK3dEntity *Ref=NULL) const CK_PURE;

/************************************************
Summary: Transforms a position from another entity coordinates system into this coordinates system.

Arguments:
	Dest: A VxVector which will be filled with the transformed vector (can be the same pointer than Src).
	Src: The VxVector to be transformed.
	Ref: The referential entity.
Remarks:
+ If referential is NULL, the source vector Src is defined in world coordinates.
+ The InverseTransform method transforms a position vector (that is it takes entity position into account) while 
InverseTransformVector transforms a normal vector (only rotation is taken).
See also: Transform,InverseTransformMany,InverseTransformVector
************************************************/	
virtual 	void	InverseTransform(VxVector *Dest,const VxVector *Src,CK3dEntity *Ref=NULL) const CK_PURE;

virtual 	void	TransformVector(VxVector *Dest,const VxVector *Src,CK3dEntity *Ref=NULL) const CK_PURE;

virtual 	void	InverseTransformVector(VxVector *Dest,const VxVector *Src,CK3dEntity *Ref=NULL) const CK_PURE;
/************************************************
Summary: Transforms vectors from this coordinates system into another entity coordinates system.

Arguments:
	Dest: An array of VxVector array which will be filled with the transformed vectors
	Src:  An array of VxVector to be transformed.
	Count:Number of Vector to process
	Ref: The referential entity (NULL means world coordinates).
Remarks:
	+ Use this function to convert an array of vector instead of calling several times InverseTransform for 
	better performances.
See also: InverseTransformMany
************************************************/	
virtual 	void	TransformMany(VxVector *Dest,const VxVector *Src,int count,CK3dEntity *Ref=NULL) const CK_PURE;
/************************************************
Summary: Transforms vectors from another entity coordinates system into this entity coordinates system.

Arguments:
	Dest: An array of VxVector array which will be filled with the transformed vectors
	Src:  An array of VxVector to be transformed.
	Count:Number of Vector to process
	Ref: The referential entity (NULL means world coordinates).
Remarks:
	+ Use this function to convert an array of vector instead of calling several times InverseTransform for 
	better performances.
See also: TransformMany
************************************************/
virtual 	void	InverseTransformMany(VxVector *Dest,const VxVector *Src,int count,CK3dEntity *Ref=NULL) const CK_PURE;

/************************************************
Summary: Changes the referential (pivot) of the entity without moving the vertices.

Arguments:
	Ref: Referential in which we would like to transform the vertices. NULL to express them in the world referential.
Remarks:
+ Transforms all the vertices positions and normals of all the meshes of the entity so that
their world position stays the same in the new given referential . 
+ This method can be used to change the pivot or referential of an entity without moving its vertices.
See also: SetRenderCallBack
************************************************/	
virtual 	void	ChangeReferential(CK3dEntity *Ref=NULL) CK_PURE;

//------------------------------------------------
// Places

/*************************************************
Summary: Returns the reference place of this entity

Return Value: A pointer to the reference place of this entity
Remarks:
	+ An entity can be attached to a place (as one of its children).
	+ Places can be used to perfom portal culling to removes hidden entities in a scene.
See also: Using Portals,CKPlace,CKPlace::AddEntity
*************************************************/	
virtual	CKPlace *GetReferencePlace() const CK_PURE;

//-------------------------------------------------
// Animations

/*************************************************
Summary: Adds an animation to the entity

Arguments:
	anim: The new animation to attach.
Remarks:
+ You should rather use the CKObjectAnimation::Set3dEntity method which automatically calls this method 
when necessary. 
+ Each 3D entity may have several animations attached to it. These animations can
be accessed by behaviors of the entity, to play, stop them, etc...
+ These animations are often part of a CKKeyedAnimation which control a whole character.
See also: CKObjectAnimation,GetObjectAnimation,GetObjectAnimationCount,RemoveObjectAnimation
*************************************************/
virtual 	void AddObjectAnimation(CKObjectAnimation *anim) CK_PURE;
/*************************************************
Summary: Removes an animation from the entity
Arguments:
	anim: the animation to remove
Remarks:
	+ Each 3D entity may have several animations attached to it. These animations can
	be accessed by behaviors of the entity, to play, stop them, etc...

See also: CKObjectAnimation,GetObjectAnimation,GetObjectAnimationCount,AddObjectAnimation
*************************************************/
virtual 	void RemoveObjectAnimation(CKObjectAnimation *anim) CK_PURE;
/*************************************************
Summary: Returns an animation by its index.
Arguments:
	index: Index of the animation to return.
Return Value:
	A pointer to the index th CKObjectAnimation.

See also:CKObjectAnimation,RemoveObjectAnimation,GetObjectAnimationCount,AddObjectAnimation
*************************************************/	
virtual 	CKObjectAnimation* GetObjectAnimation(int index) const CK_PURE;
/*************************************************
Summary: Returns the number of object animations on this entity.

Return Value:
	Number of animation on this entity.
See also:CKObjectAnimation,RemoveObjectAnimation,GetObjectAnimation,AddObjectAnimation
*************************************************/	
virtual 	int GetObjectAnimationCount() const CK_PURE;

//-------------------------------------------------
// Skin

/*************************************************
Summary: Creates a skin on this entity.
Return Value:
	A pointer to newly created CKSkin.
Remarks:
	+ A skin is a sort of modifier that can be applied to an entity. It will modify its current
	mesh vertices according to the position and orientation of a set of bones. This is used 
	to create characters that can be animated using less memory than standard morph animation. See the CKSkin class 
	for more details.

See Also:CKSkin,DestroySkin,UpdateSkin,GetSkin
*************************************************/	
virtual 	CKSkin* CreateSkin() CK_PURE;
/*************************************************
Summary: Removes the skin. 

Return Value:
	TRUE if successful, FALSE if the skin does not exist.
See Also:CKSkin,CreateSkin,UpdateSkin,GetSkin
*************************************************/	
virtual 	CKBOOL DestroySkin() CK_PURE;
/*************************************************
Summary: Updates the current mesh according to bones position.
Return Value:
	TRUE if successful, FALSE if the skin does not exist or is not configured properly.
Remarks:
	+ This method is automatically called before rendering this entity if it
	is visible. It can be use you to force the update of the skin.

See Also:CKSkin,CreateSkin,DestroySkin,GetSkin
*************************************************/	
virtual 	CKBOOL UpdateSkin() CK_PURE;
/*************************************************
Summary: Returns the current skin applied to this entity.
Return Value:	 A pointer to the CKSkin

See Also:CKSkin,CreateSkin,DestroySkin,UpdateSkin
*************************************************/	
virtual 	CKSkin* GetSkin() const CK_PURE;


virtual void	UpdateBox(CKBOOL World = TRUE) CK_PURE;

/************************************************
Summary: Returns the bounding box this entity.
Arguments:
	Local: TRUE for local bounding box, FALSE for world bounding box.
Return Value: A reference to the bounding box.
Remarks:
	+ If Local is set to TRUE the returned box is expressed in local axis aligned coordinates. Otherwise the box
	min and max coordinates are returned in world axis aligned coordinates which results in a larger box. 

See also: VxBbox, GetRadius, GetBaryCenter, GetHierarchicalBox
************************************************/	
virtual const VxBbox& GetBoundingBox(CKBOOL Local=FALSE) CK_PURE;

/************************************************
Summary: Specify a user bounding box for the entity.
Arguments:
	BBox: The VxBbox to set, a NULL value disable the usage of the user bounding box.
	Local: Indicate whether local or world bounding box should be set.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
	+ The default bounding box of an entity is the bounding box of its current mesh,
	you can specify your own bounding box (which will be used for visibility tests) with this
	method. It is especially useful when using render callback on dummy object (a point) in which case the 
	box should be set according to was it drawn. The particle systems behavior for example use
	this method to set the bounding box of their emitter as equal to the box that contains all particles. 

See also: VxBbox, GetRadius, GetBaryCenter, GetHierarchicalBox,GetBoundingBox
************************************************/
virtual CKBOOL	SetBoundingBox(const VxBbox *BBox,CKBOOL Local=FALSE) CK_PURE;

/************************************************
Summary: Returns the hierarchical bounding box of this entity and its children.
Arguments:
Local: This argument is kept only for compatibility reason and should 
be always set to FALSE.(TRUE for local bounding box, FALSE for world bounding box)
Return Value: A reference to the hierarchical bounding box.

Remarks:
+If Local is set to TRUE the returned box is expressed in local axis aligned coordinates. But this box
is computed from the extents of the world hierarchical box so it is not as precise as the 
world box could be.
To compute an accurate local hierarchy box, merge the "this" local box with "this" axis aligned
bounding boxes of the objects of the hierarchy of "this". 
This accurate method requires to parse the meshes of the objects of the hierarchy.
Another approximated method would be to merge "this" local box 
and its children local boxes transformed in "this" referential.
+ The box min and max coordinates are returned in world axis aligned coordinates. 

See also: VxBbox, GetRadius,GetBoundingBox,GetBaryCenter,SetBoundingBox
************************************************/	
virtual const VxBbox& GetHierarchicalBox(CKBOOL Local=FALSE) CK_PURE;
/************************************************
Summary: Returns the barycenter of the current mesh of this entity.
Return Value:
	FALSE if the 3D Entity does not have a mesh, TRUE otherwise.
Arguments:
	Pos: The returned VxVector which represents the barycenter of the mesh.
Remarks:
	+ The barycenter is computed as the average of all the mesh vertices.

See also: GetRadius, GetBoundingBox, GetHierarchicalBox
************************************************/
virtual CKBOOL	GetBaryCenter(VxVector *Pos) CK_PURE;
/************************************************
Summary: Returns the radius of the mesh currently used by this moveable.

  Return Value: The radius of the moveable, 0 there is no current mesh.
Remarks:
+ Returns the radius of the current mesh used by the entity. The radius is defined as
the distance from the midpoint of the mesh to a corner of the mesh's bounding box. This
method takes the entity scale factors into account.
NOTE: A Sprite3D returns a 0 radius.
See also: GetBaryCenter, GetBoundingBox, GetHierarchicalBox
************************************************/	
virtual float	GetRadius() CK_PURE;

/*************************************************
Summary: Set the weight value of a blend shape for the current mesh bound to that 3D entity. 
The blend shape is identified by its index. 
Arguments:
bsIndex: Index of the blend shape in the current mesh
weight: The weight value. This value will be clamped to [0, 1]
Remarks:
When CK3dEntity::SetCurrentMesh is called with a new mesh, all weights are set to 0

*************************************************/
virtual void SetBlendShapeWeightByIndex(int bsIndex, float weight) CK_PURE;

/*************************************************
Summary: Set the weight value of a blend shape for the current mesh bound to that 3D entity. 
The blend shape is identified by its name 
Arguments:
bsIndex: Name of the blend shape in the current mesh
weight: The weight value. This value will be clamped to [0, 1]
Remarks:
When CK3dEntity::SetCurrentMesh is called with a new mesh, all weights are set to 0

*************************************************/
virtual void SetBlendShapeWeightByName(const XString& name, float weight) CK_PURE;

/*************************************************
Summary: Set the weight value of a blend shape for the current mesh bound to that 3D entity, or 0
 if the blend shape index is invalid, or if no mesh was bound to that entity.
The blend shape is identified by its index
Arguments:
bsIndex: Name of the blend shape in the current mesh
Return Value:
The blend shape current weight

*************************************************/
virtual float GetBlendShapeWeightByIndex(int bsIndex) const CK_PURE;

/*************************************************
Summary: Set the weight value of a blend shape for the current mesh bound to that 3D entity, or 0
if the blend shape name is invalid, or if no mesh was bound to that entity.
The blend shape is identified by its name
Arguments:
bsIndex: Name of the blend shape in the current mesh
Return Value:
The blend shape current weight

*************************************************/
virtual float GetBlendShapeWeightByName(const XString& name) const CK_PURE;

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
static CK3dEntity* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_3DENTITY)?(CK3dEntity*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
