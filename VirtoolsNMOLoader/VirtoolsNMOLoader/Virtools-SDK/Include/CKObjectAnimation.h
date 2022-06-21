/*************************************************************************/
/*	File : CKObjectAnimation.h											 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 1999, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKOBJECTANIMATION_H

#define CKOBJECTANIMATION_H "$Id:$"

#include "CKObject.h"
#include "CKKeyFrameData.h"



typedef struct CKAnimKey {
	VxVector	 m_Pos;
	VxVector	 m_Scl;
	VxQuaternion m_Rot;
	VxQuaternion m_SclRot;
} CKAnimKey;

/***********************************************************************
{filename:CKObjectAnimation}
Summary: Animation of a 3d Object.

Remarks:
	+ An ObjectAnimation is made up of keys describing position,orientation or scale at 
	specific times. Animation a 3dObject consists in interpolating between those
	keys to calculate position/orientation/scale at  a given time.

	+ The most frequent use of a CKObjectAnimation is as a part of a CKKeyedAnimation which
	describes animation for a whole character or for a group of objects.
	
	+ It contains keyframe data for position,scale and rotation. Each of this data is driven by
	a given controller which performs the interpolation according to its type (linear,TCB or bezier)

	+ It can also contain keyframe data for morphing in which case a key is the set of vertices (optionnaly normals)
	to set to the target mesh.
	
	+ If often occurs that several characters have common animations, to avoid data to be reproduced the common animation
	can share their keyframe data and only keep target (CK3dEntity) specific data. 

	+ Its class id is CKCID_OBJECTANIMATION



See also: CKKeyedAnimation
*************************************************************************/
class CKObjectAnimation : public CKSceneObject {
friend class CKKeyedAnimation;
public :


/**************************************************************
Summary: Creates an animation controller which performs the interpolation according to its type

Arguments:
	ControlType: a CKANIMATION_CONTROLLER enum to identify the desired controller type.
	TargetBlendShapeName: If controller is a float controller, give the blend shape name to which that controller must be bound
Return value: The newly created CKAnimController
Remarks:
+If a animation controller of the same type already existed, it is deleted and replaced
by the newly created.
+If no specific type of interpolation is given, a linear controller is created.
See also: CKAnimController, CKANIMATION_CONTROLLER
****************************************************************/
virtual CKAnimController* CreateController(CKANIMATION_CONTROLLER ControlType, const char* TargetBlendShapeName = NULL) = 0; 

/*************************************************
Summary: Deletes a controller of specified type.

Arguments:
	ControlType: a CKANIMATION_CONTROLLER enum to identify the controller to delete.
	TargetBlendShapeName: If controller is a float controller, give the blend shape name
Return value: TRUE if successful.
Remarks:
	+ Only the type of controller (position,rotation,scale,...) in ControlType is 
	used to determine which controller should be deleted.
See also: CKANIMATION_CONTROLLER,CreateController
*************************************************/
virtual CKBOOL DeleteController(CKANIMATION_CONTROLLER ControlType, const char* TargetBlendShapeName = NULL) = 0; 

/*************************************************
Summary: Deletes a controller by its pointer

Arguments:
controller: a pointer to the controller to be removed
Return value: TRUE if successful.
See also: CKANIMATION_CONTROLLER,CreateController
*************************************************/
virtual CKBOOL DeleteController(CKAnimController *controller) = 0; 

/*************************************************
Summary: Gets the position controller

Return value:  A pointer to a CKAnimController or NULL is not available.
See also: CKAnimController,CreateController,GetScaleController,GetRotationController
*************************************************/
virtual CKAnimController* GetPositionController() = 0;

/*************************************************
Summary: Gets the scale controller

Return value:  A pointer to a CKAnimController or NULL is not available.
See also: CKAnimController,CreateController,GetPositionController,GetRotationController
*************************************************/
virtual CKAnimController* GetScaleController() = 0; 

/*************************************************
Summary: Gets the rotation controller

Return value:  A pointer to a CKAnimController or NULL is not available.
See also: CKAnimController,CreateController,GetPositionController,GetScaleController
*************************************************/
virtual CKAnimController* GetRotationController() = 0; 

/*************************************************
Summary: Gets the off-axis scale controller

Return value:  A pointer to a CKAnimController or NULL is not available.
See also: CKAnimController,CreateController,GetPositionController,GetScaleController,GetRotationController
*************************************************/
virtual CKAnimController* GetScaleAxisController() = 0; 

/*************************************************
Summary: Gets the Morph controller

Return value:  A pointer to a CKMorphController or NULL is not available.
See also: CKMorphController,CreateController,GetPositionController,GetScaleController,GetRotationController
*************************************************/
virtual CKMorphController* GetMorphController() = 0; 

/*************************************************
Summary: Gets the Number of blend shape controllers. There may be less controllers than blend shape in the target,
 if all blend shapes are not animated

Return value:  The Number of blend shape controllers
*************************************************/
virtual int GetBlendShapeControllerCount() const = 0; 

/*************************************************
Summary: Gets the nth blend shape controller

Return value:  A pointer to a CKMorphController or NULL is not available.
*************************************************/
virtual CKAnimController* GetBlendShapeController(int index) = 0; 

/*************************************************
Summary: Gets the name of the blend shape affected by the nth blend shape controller

Return value:  A string
*************************************************/
virtual const XString& GetBlendShapeControllerTargetBlendShapeName(int index) const = 0; 

/*************************************************
Summary: Evaluates position value at given time

Arguments: 
	Time: Time at which the position has to be evaluated.
	Pos: A VxVector, to be filled with the position.
Return value: TRUE if successful, FALSE otherwise (No Position controller).
Remarks:
	+ If the animation is merged with another animation, then position is evaluated on
the basis of both animations, else the position of the individual animation is evaluated.
See also: EvaluateScale,EvaluateRotation,EvaluateScaleAxis,EvaluateKeys
*************************************************/
virtual CKBOOL EvaluatePosition(float Time,VxVector& Pos) = 0;

/*************************************************
Summary: Evaluates scale value at given time

Arguments: 
	Time: Time at which the Scale has to be evaluated.
	Scl: A  VxVector to be filled with the Scale value.
Return value: TRUE if successful, FALSE otherwise (No Scale controller).
See also: EvaluatePosition,EvaluateRotation,EvaluateScaleAxis,EvaluateKeys
*************************************************/
virtual CKBOOL EvaluateScale(float Time,VxVector& Scl) = 0;

/*************************************************
Summary: Evaluates rotation value at given time

Arguments: 
	Time: Time at which the rotation has to be evaluated.
	Rot: A VxQuaternion, to be filled with Rotation vector.
Return value: TRUE if successful, FALSE otherwise (No Rotation controller).
Remarks:
	+ If the animation is merged with another animation, then rotation 
vector is evaluated on the basis of both animations, else the rotation of 
the individual animation is evaluated.
See also: EvaluatePosition,EvaluateScale,EvaluateScaleAxis,EvaluateKeys
*************************************************/
virtual CKBOOL EvaluateRotation(float Time,VxQuaternion& Rot) = 0;

/*************************************************
Summary: Evaluates scale axis at given time

Arguments: 
	Time: Time at which the Scale Axis has to be evaluated.
	ScaleAxis: Scale Axis to be filled in VxQuaternion
Return value: TRUE if successful, FALSE otherwise (No off-axis scale controller).
See also: EvaluatePosition,EvaluateRotation,EvaluateScale,EvaluateKeys
*************************************************/
virtual CKBOOL EvaluateScaleAxis(float Time,VxQuaternion& ScaleAxis) = 0;

/*************************************************
Summary: Evaluates morph target at given time

Arguments: 
	Time: Time at which the morph target has to be evaluated.
	VertexCount: Number of vertices to be returned.
	Vertices: Vertices to be filled in VxVector.
	VStride: Amount in bytes between positions in the Vertices array. 
	Normals: A pointer to a VxCompressedVector array that will be filled with normals.
Return value: TRUE if successful, FALSE otherwise (No morph controller).
See also: GetMorphController
*************************************************/
virtual CKBOOL EvaluateMorphTarget(float Time,int VertexCount,VxVector* Vertices,CKDWORD VStride,VxCompressedVector* Normals) = 0;

/*************************************************
Summary: Evaluates the keys (rotation, position and scale) at given step

Arguments:
	step: Step at which the keys has to be evaluated (in the interval 0..1).
	rot: A VxQuaternion pointer, to be filled with Rotation. 
	pos: A VxVector pointer, to be filled with Position.
	scale: A VxVector pointer, to be filled with Scale.
	ScaleRot: A VxQuaternion pointer, to be filled with off-axis Scale.
Return value: TRUE, if successful, FALSE otherwise.
Remarks:
+All pointers parameter are optionnal and can be set to NULL is some
values are not desired.
+This method is a combination of services provided by EvaluatePosition, 
EvaluateRotation, EvaluateScale, EvaluateScaleAxis.
See also: EvaluatePosition,EvaluateRotation, EvaluateScale, EvaluateScaleAxis.
*************************************************/
virtual	CKBOOL  EvaluateKeys(float step,VxQuaternion *rot,VxVector *pos,VxVector *scale,VxQuaternion *ScaleRot=NULL) = 0;

/*************************************************
Summary: Checks whether morph has normal information.

Remarks:
+For a standard animation this method checks if a Morph controller is 
available and have normals information.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has a morph controller with normals information.
See also: HasMorphInfo,GetMorphController
*************************************************/
virtual  CKBOOL HasMorphNormalInfo() = 0;

/*************************************************
Summary: Checks whether animation has morph information

Remarks:
+For a standard animation this method checks if a Morph controller is 
available.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has a morph controller.
See also: HasMorphNormalInfo,GetMorphController
*************************************************/
virtual  CKBOOL HasMorphInfo() = 0;

/*************************************************
Summary: Checks whether animation has scale information.

Remarks:
+For a standard animation this method checks if a Scale controller is 
available.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has Scale information.
See also: HasPositionInfo,HasRotationInfo,GetScaleController
*************************************************/
virtual  CKBOOL HasScaleInfo() = 0;

/*************************************************
Summary: Checks whether animation has position information.

Remarks:
+For a standard animation this method checks if a Position controller is 
available.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has Position information.
See also: HasScaleInfo,HasRotationInfo,GetPositionController
*************************************************/
virtual  CKBOOL HasPositionInfo() = 0;

/*************************************************
Summary: Checks whether animation has rotation information.

Remarks:
+For a standard animation this method checks if a Rotation controller is 
available.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has Rotation information.
See also: HasScaleInfo,HasPositionInfo,GetRotationController
*************************************************/
virtual  CKBOOL HasRotationInfo() = 0;

/*************************************************
Summary: Checks whether animation has off-axis scale information.

Remarks:
+For a standard animation this method checks if a off-axis scale controller is 
available.
+If the animation is a merged animation, the method returns TRUE
if any one of the merged animations has off-axis scale information.
See also: HasScaleInfo,HasPositionInfo,GetScaleAxisController
*************************************************/
virtual  CKBOOL HasScaleAxisInfo() = 0;

//------- Construction using linear controllers;

/*************************************************
Summary: Adds a linear position key

Arguments: 
	TimeStep: Time step at which the new position key has to be added.
	pos: A VxVector pointer specifying the position.
Remarks:
+Adds new position key to the Key frame data with the given time step and position
+This method automatically creates a linear position controller 
if it was not available.
+This method is left here as a shortcut to create linear keys to use other types of interpolation, create
the appropriate controller with CreateController and use the AddKey method on the created CKAnimController.
See also: CKAnimController,CreateController
*************************************************/
virtual void AddPositionKey	(float TimeStep,VxVector* pos) = 0;

/*************************************************
Summary: Adds rotation key

Arguments: 
	TimeStep: Time step at which the new rotation key has to be added.
	rot: A VxQuaternion pointer specifying the rotation.
Remarks:
+Adds new rotation key to the Key frame data with the given time step and rotation
+This method automatically creates a linear rotation controller 
if it was not available.
+This method is left here as a shortcut to create linear keys to use other types of interpolation, create
the appropriate controller with CreateController and use the AddKey method on the created CKAnimController.
See also: CKAnimController,CreateController,AddPositionKey,AddScaleKey,AddScaleAxisKey
*************************************************/
virtual void AddRotationKey	(float TimeStep,VxQuaternion* rot) = 0;

/*************************************************
Summary: Adds scale key

Arguments: 
	TimeStep: Time step at which the new scale key has to be added.
	scl: A VxVector pointer specifying the Scale.
Remarks:
+Adds new scale key to the Key frame data with the given time step and scale
+This method automatically creates a linear scale controller 
if it was not available.
+This method is left here as a shortcut to create linear keys to use other types of interpolation, create
the appropriate controller with CreateController and use the AddKey method on the created CKAnimController.
See also: CKAnimController,CreateController,AddPositionKey,AddRotationKey,AddScaleAxisKey
*************************************************/
virtual void AddScaleKey		(float TimeStep,VxVector* scl) = 0;

/*************************************************
Summary: Adds off-axis scale key

Arguments: 
	TimeStep: Time step at which the new off-axis scale key has to be added.
	sclaxis: A VxQuaternion pointer specifying the off-axis scale.
Remarks:
+Adds new scale axis key to the Key frame data with the given time step and scale axis
+This method automatically creates a linear off-axis scale controller 
if it was not available.
+This method is left here as a shortcut to create linear keys to use other types of interpolation, create
the appropriate controller with CreateController and use the AddKey method on the created CKAnimController.
See also: CKAnimController,CreateController,AddPositionKey,AddRotationKey,AddScaleKey
*************************************************/
virtual void AddScaleAxisKey	(float TimeStep,VxQuaternion* sclaxis) = 0;

//-------------------------------------------------------------


/*************************************************
Summary: Compares current animation object with a given animation.
Arguments:
	anim: A Pointer to the animation object to compare.
	threshold: Threshold value.
Return value: TRUE, if both are equal, FALSE otherwise.
Remarks:
	+ The threshold is used to determine the accepted error between
	two vector to say they are equal.


See also: RCKObjectAnimation
*************************************************/
virtual	CKBOOL	 Compare(CKObjectAnimation* anim,float threshold=0.0f) = 0;

/*************************************************
Summary: Uses the keyframe data from another animation.

Arguments:
	anim: A Pointer to the animation object to use the keyframe data of.
Remarks:
+Using this methods deletes all the existing data and controllers to
use the same data than anim.
+The two animations will only differ in the entity to which they apply.
See also: Shared
*************************************************/
virtual	CKBOOL	 ShareDataFrom(CKObjectAnimation* anim) = 0;
/*************************************************
Summary: Checks if the animation uses the keyframe data from another animation.

Return Value: A pointer to the shared object animation or NULL is this animation
uses its own keyframe data.
See also: ShareDataFrom
*************************************************/
virtual	CKObjectAnimation*	 Shared() = 0;

//-------------------------------------------------------------
// Flags

/*************************************************
Summary: Sets the flags to the animation object

Arguments: 
	flags: A CK_OBJECTANIMATION_FLAGS enumeration combination.
See also: GetFlags,CK_OBJECTANIMATION_FLAGS
*************************************************/
virtual	void SetFlags(CKDWORD flags) = 0;

/*************************************************
Summary: Gets specflags of the animation object
  
Return Value: 
	A CK_OBJECTANIMATION_FLAGS enumeration combination.
See also: SetFlags,CK_OBJECTANIMATION_FLAGS
*************************************************/
virtual	CKDWORD	GetFlags() = 0;

//-------------------------------------------------------------
// Clearing


virtual	void Clear() = 0;

/*************************************************
Summary: Clears all data (Keyframe data + merge info).

*************************************************/
virtual	void ClearAll() = 0;

//-------------------------------------------------------------
// Merged Animations

/*************************************************
Summary: Gets merge factor of a blended animation.

Return value: 
	Merge factor.
Remarks:
	+ The merge factor is the amount deciding how much of each source animations is used to produce 
	the blended animation. If the merge factor is 0 this animation is equal to the first animation, if it's
	1 it is equal to the second animation , other values result in a smooth interpolation between the 
	two sources animations.
See also: CreateMergedAnimation,SetMergeFactor,IsMerged,
*************************************************/
virtual  float GetMergeFactor() = 0;

/*************************************************
Summary: Sets merge factor for animation object

Arguments:
	factor: Merge factor to be set.
See also: GetMergeFactor,CreateMergedAnimation
*************************************************/
virtual  void  SetMergeFactor(float factor) = 0;

/*************************************************
Summary: Checks whether this animation is a blending of two animations 
 
Return Value:
	TRUE if the animation is a composition between two animations,
	FALSE otherwise.
See also: CreateMergedAnimation,GetMergeFactor
*************************************************/
virtual  CKBOOL  IsMerged() = 0;

/*************************************************
Summary: Creates a new object animation by blending this animation with another one .
 		
Arguments:
	subanim2: Pointer to second animation object.
Return value: 
	A Pointer to the created blended animation, NULL if method failed.
Example:
	// Creates a merged object animation out of two
	// CKObjectAnimation *Anim1,*Anim2 are supposed to exist and be valid.

	CKObjectAnimation* Merged = Anim1->CreateMergedAnimation(Anim2);

	// This sets Merged to be the same animation than Anim1
	Merged->SetMergeFactor(0.0f);

	// This sets Merged to be the same animation than Anim2
	Merged->SetMergeFactor(1.0f);

	// This sets Merged to be a smooth interpolation between Anim1 and Anim2
	Merged->SetMergeFactor(0.5f);

See also: IsMerged,SetMergeFactor
*************************************************/
virtual  CKObjectAnimation* CreateMergedAnimation(CKObjectAnimation *subanim2,CKBOOL Dynamic = FALSE) = 0;

//-------------------------------------------------------------
// Animation Length

/*************************************************
Summary: Sets te length of the animation.

Arguments:
	nbframe: Animation length
Remarks:
	+ Changing the length of an object animation does not rescale it.
	For example changing the length of an animation that was previously 40 frames long 
	to 60 frames will only results in nothing happening in the last 20 frames.
See also: GetLength
*************************************************/
virtual	void   SetLength(float nbframe) = 0;

/*************************************************
Summary: Returns the length of the animation.

See also: SetLength,SetFrame
*************************************************/
virtual	float  GetLength() = 0;


virtual  void GetVelocity(float step,VxVector *vel) = 0;

//-------------------------------------------------------------
// Anim's current pos

/*************************************************
Summary: Applies the animation to its entity at the given step.

Arguments:
	frame: Frame (0.. Animation length)
	step: Step (0..1)
	anim: A pointer to the higher level animation (CKKeyedAnimation) asking to set the current frame.
Remarks:
	+ The anim pointer is used to identify the high level animation asking to compute
	this object animation in case the target entity is the target of another exclusive animation
	(a character secondary animation for example) See CKBodyPart::GetExclusiveAnimation).
	It can be set to NULL or CKANIMATION_FORCESETSTEP to force the object animation to be processed
	even if the entity is flagged as ignoring animations (CK3dEntity::AreAnimationIgnored).
See also: GetCurrentStep,CKAnimation::SetStep
*************************************************/
virtual	CKERROR SetStep	(float step,CKKeyedAnimation *anim=NULL) = 0;

virtual	CKERROR SetFrame(float frame,CKKeyedAnimation *anim=NULL) = 0;

/*************************************************
Summary: Gets the current step of animation.

Return value: current step of animation (0..1).
Remarks:
	+ The animation step is the percentage of the current frame on its 
	overal length.
See also: SetStep
*************************************************/
virtual	float	GetCurrentStep() = 0;

//-------------------------------------------------------------
// Anim's 3d entity

/*************************************************
Summary: Sets the 3DEntity to which this animation applies

Arguments:
	ent: A pointer to the 3dEntity to which this animation should apply.
See also: CK3dEntity, Get3dEntity
*************************************************/
virtual	void Set3dEntity(CK3dEntity *ent) = 0;

/********************************************************
Summary: Returns the 3DEntity to which this animation applies

Return Value:
	A pointer to the 3dEntity to which this animation applies
See also: CK3dEntity, Set3dEntity
*********************************************************/
virtual	CK3dEntity *Get3dEntity() = 0;

//---------------------------------------------------------
// Expected Number of Vertices in the Mesh

/*******************************************************
Summary: Gets the number of vertices in morph animation.

Return Value: Number of vertices in a morph key.
Remarks:
	+ The number of vertices in the morph animation must be
	the same than the mesh of the target entity
See also: CKMorphController::SetMorphVerteCount,Get3dEntity
*******************************************************/
virtual	int  GetMorphVertexCount() = 0;

/*************************************************
Summary: Makes this animation a transition animation between two others.

Arguments:
	length: Length of transition animation.
	AnimIn: A pointer to the first animation.
	StepFrom: Beginning step (0..1) in first animation.
	AnimOut: A pointer to the second animation.
	StepTo: Destination step (0..1) in second animation.
	Veloc: TRUE if Velocity is to be set for the animation, FALSE otherwise.
	DontTurn: if TRUE takes the rotation from out otherwise create a transition from rotation in AnimIn to rotation in AnimOut.
	startingset: this parameter is used internally and should be left to NULL.
Remarks:
	+ This methods creates the key frame data to have a transition between AnimIn and AnimOut
	at the given step.

See also: RCKObjectAnimation
*************************************************/
virtual  void CreateTransition(float length,CKObjectAnimation* AnimIn,float StepFrom,CKObjectAnimation * AnimOut,float StepTo,CKBOOL Veloc,CKBOOL DontTurn,CKAnimKey* startingset=NULL) = 0; 

/*************************************************
Summary: Copies an object animation data.

Arguments:
	anim: Pointer to animation object to be copied.
*************************************************/
virtual  void Clone(CKObjectAnimation *anim) = 0;


CKObjectAnimation(CKContext *Context,CKSTRING name=NULL) : CKSceneObject(Context,name) {} 

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
static CKObjectAnimation* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_OBJECTANIMATION)?(CKObjectAnimation*)iO:NULL;
}
};

#endif

