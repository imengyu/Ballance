/*************************************************************************/
/*	File : CKKeyedAnimation.h						 					 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/

#ifndef CKANIMATION_H

#define CKANIMATION_H "$Id:$"

#include "CKObject.h"

/*********************************************************************
{filename:CKanimation}
Name: CKAnimation

Summary: Base Class for character animations or group of object animations.

Remarks:
	+ CKAnimation is a virtual class that provides the base methods
	used for character animations. Miscellaneous flags are accessible to 
	specify whereas the animation should be played at a fixed framerate, if the 
	character should align its orientation with the orientation of the root contained in 
	the animation.
	For more details about characters and their animation management see CKCharacter. 

	+ An Animation may be maded up of two other blended animations, the resulting 
	animation depends on a float factor between 0 and 1. If 0 the merged animation is equal
	to the first animation and if 1 it is equal to the second. A factor of 0.5 gives a smooth 
	blending between the two initial animations. One can use this to create a universal grab animation 
	for example maded up of 4 initial animations ( grab right,grab left,grab high,grab low) then according
	to where the grabbing occurs we calculate the two merge factors (right-left,low-high) so that
	the resulting animation grab at the right place.

	+ For a character, animation transitions often occur. These transitions may be brutal or
	smooth using motion warping. Motion warping creates on the fly a transition animation
	between the current character animation and the desired animation. 

	+ An animation has a length which can be time dependend or not. For example an animation
	of 60 frames can be played at a fixed frame rate (eg with LinkToFrameRate(TRUE,60) the animation
	will be played in exactly 1 second (60 frame per second)) or can be completly process dependendant
	in which case the animation is played at 1 frame per (behavior+render) process which is never sure to give a constant speed
	according to what is on screen.

	+ The class id of CKAnimation is CKCID_ANIMATION.




See also: CKKeyedAnimation,CKCharacter,Using Animations,Using Characters
***********************************************************************/
class CKAnimation:public CKSceneObject {
public :
//--------------------------------------
// Stepping along

/*************************************************
Summary: Returns the total length.
Return Value:
	Length of the animation.
Remarks:

See Also:SetLength,GetFrame,SetStep,SetFrame
*************************************************/
virtual	float GetLength() = 0;
/*************************************************
Summary: Returns the current position of the animation.
Return Value:
	Current Frame
Remarks:

See Also:SetStep,GetStep,SetFrame,Getlength
*************************************************/
virtual	float GetFrame() = 0;
/*************************************************
Summary: Returns the expected next frame.

Arguments:
	delta_t: Time in milliseconds to next process.
Return Value:
	Espected next frame if delta_t milliseconds elapse.
Remarks:
	+ According to the animation settings (Linked to frame rate,etc..)
	the return value is the expected next frame of animation if delta_t milliseconds elapse.
See Also:SetStep,GetFrame,SetFrame,Getlength
*************************************************/
virtual	float GetNextFrame(float delta_t) = 0;
/*************************************************
Summary: Returns the current progression of the animation.
Return Value:
	+ Current progression of the animation between 0..1, this value
	is equivalent to GetFrame()/GetLength()

See Also:SetStep,GetFrame,SetFrame,GetNextFrame,Getlength
*************************************************/
virtual	float GetStep() = 0;

//---------------------------------------------
// Current Pos functions 

/*************************************************
Summary: Sets the current position of animation
Arguments:
	frame: A float between 0 and the length of the animation specifying 
	the current position 
Remarks:
	+ Calling this method sets the position/orientation/scale
	of all the entities concerned by this animation.

See Also:SetStep,GetFrame,GetStep,GetNextFrame,Getlength
*************************************************/
virtual void SetFrame(float frame) = 0;
/*************************************************
Summary: Sets the current progression of the animation
Arguments:
	step: A float in the range 0..1.
Remarks:
	+ SetStep(f) is equivalent to SetFrame(f*GetLength());

See Also:SetStep,GetFrame,GetStep,GetNextFrame,Getlength,SetCurrentStep
*************************************************/
virtual void SetStep(float step) = 0;
/*************************************************
Summary: Sets the total length of the animation.
Arguments:
	nbframe: Length in number of frames of the animation.
Remarks:
	+ Changing the length of an animation does not rescale it.
	For example changing the length of an animation that was previously 40 frames long 
	to 60 frames will only results in nothing happening in the last 20 frames.

See Also:GetLength,SetFrame,SetStep
*************************************************/
virtual void SetLength(float nbframe) = 0;

//--------------------------------------
// Character functions	
		  	
/*************************************************
Summary: Returns the owner character of the animation.
Return Value:
	A pointer to the character.
Remarks:
	+ If an animation is not assigned to a character it is called a global animation.

*************************************************/
virtual	CKCharacter *GetCharacter() = 0;

//-------------------------------------
// animation frame rate link

/*************************************************
Summary: Links the play back of the animation to the frame rate.
Arguments:
	link : TRUE if animation is linked to frame rate, FALSE otherwise.
	fps: Frame rate per second.
Remarks:
	+Once an animation is linked to a given frame rate it will be played
	at the speed given by fps otherwise the default behavior is to play
	the animation at 1 frame per behavior process which is never sure to be at constant speed.
	+This method sets or removes the CKANIMATION_LINKTOFRAMERAT flag.

See Also: GetLinkedFrameRate, IsLinkedToFrameRate,GetFlags
*************************************************/
virtual void LinkToFrameRate(CKBOOL link,float fps=30.0f) = 0;

/*************************************************
Summary: Returns the linked frame rate value.
Return Value:
	Value of the frame rate for the animation.
Remarks:
	+ The returned value indicates the speed at which 
	the animation will be played.

See Also: LinkToFrameRate, IsLinkedToFrameRate
*************************************************/
virtual float GetLinkedFrameRate() = 0;

/*************************************************
Summary: Checks whether the animation is linked to frame rate.
Return Value:
	TRUE if the animation is linked to a given frame rate.
Remarks:

See Also: LinkToFrameRate, GetLinkedFrameRate
*************************************************/
virtual CKBOOL IsLinkedToFrameRate() = 0;

//-------------------------------------
// transition Mode

/*************************************************
Summary: Sets the transition mode for primary animations.
Arguments: 
	mode: Transition mode to be set.
Remarks:
	+ When an animation is to become active on a character
	the transition options (Warping,break,loop,etc..) can be given at transition time (CKCharacter::SetNextActiveAnimation)
	or set once to the animation using this method.

See Also: GetTransitionMode, CKCharacter::SetNextActiveAnimation,CK_ANIMATION_TRANSITION_MODE
*************************************************/
virtual void SetTransitionMode(CK_ANIMATION_TRANSITION_MODE mode) = 0;

virtual CK_ANIMATION_TRANSITION_MODE GetTransitionMode() = 0;

//-------------------------------------
// Secondary Animation transition Mode

/*************************************************
Summary: Sets the transition mode for secondary animations.
Arguments: 
	mode: Transition mode to be set.
Remarks:
	+ When an animation is to used as a secondary animation on a character
	the play options (One Shot,Loop,Loop N times,Warping) can be given at play time (CKCharacter::PlaySecondaryAnimation)
	or set to the animation using this method in which case when playing the secondary animation 
	with CKCharacter::PlaySecondaryAnimation use CKSECONDARYANIMATION_FROMANIMATION.

See Also: GetSecondaryAnimationMode, CKCharacter::PlaySecondaryAnimation,CK_SECONDARYANIMATION_FLAGS
*************************************************/
virtual void SetSecondaryAnimationMode(CK_SECONDARYANIMATION_FLAGS mode) = 0;

virtual CK_SECONDARYANIMATION_FLAGS GetSecondaryAnimationMode() = 0;

//-------------------------------------
// Priority

/*************************************************
Summary: Forces the animation to be played entirely. 
Arguments: 
	can: TRUE indicates that animation can be interrupted, FALSE indicates that it cannot be interrupted.
Remarks:
+ When dealing with character animations, there are often transitions
between an animation and another for example stand,walk,run etc.. but some
animations need to be played entirely to be correct such as jump animation
for example. Setting this flags ensures no animations can be launched on a 
character while this one is playing.
+ This method sets or removes the CKANIMATION_CANBEBREAK flag.

See Also: SetFlags,CKCharacter::SetNextActiveAnimation
*************************************************/
virtual void SetCanBeInterrupt(CKBOOL can=TRUE) = 0;

virtual CKBOOL CanBeInterrupt() = 0;

//-------------------------------------
// Orientation

/*************************************************
Summary: Sets whether the character should take orientation from this animation.

Arguments:
	orient: TRUE sets charater takes orientation, FALSE otherwise.
Remarks:
+ When a animation modifies the orientation of the root entity 
for example a animation to turn a character to the left you need to set this
flag to the character to force it to realign itself with the root entity at 
the end of the animation otherwise it will keep its current orientation.
+ This method sets or removes the CKANIMATION_ALIGNORIENTATION flag
See Also: SetFlags
*************************************************/
virtual void SetCharacterOrientation(CKBOOL orient=TRUE) = 0;

virtual CKBOOL DoesCharacterTakeOrientation() = 0;

//-------------------------------------
// Flags

/*************************************************
Summary: Sets the animation flags.
Arguments:
	flags: CK_ANIMATION_FLAGS Flags to be set.
Remarks:
	+ Most of the flags can be directly asked or set by te appropriate
	method of CKAnimation.


See Also: GetFlags,CK_ANIMATION_FLAGS
*************************************************/
virtual void SetFlags(CKDWORD flags) = 0;

/*************************************************
Summary: Returns the animation flags.
Return Value:
	CK_ANIMATION_FLAGS Flags set for the animation.

Remarks:
	+ Most of the flags can be directly asked or set by te appropriate
	method of CKAnimation.

See Also: SetFlags,CK_ANIMATION_FLAGS
*************************************************/
virtual CKDWORD GetFlags() = 0;

//-------------------------------------
// Root Entity

/*************************************************
Summary: Returns the root entity of the animation.
Return Value: 
	A pointer to root entity.
Remarks:
	+ When dealing with character animations there must always 
	be one and only one root object that will be used to identify 
	character movements.

*************************************************/
virtual	CK3dEntity* GetRootEntity() = 0;

//--------------------------------------------------------------
// Replacement 
/*************************************************
Summary: Recenter the animation data.
Remarks:
	+ This method translates the animation data for the
	root entity so that, at the given fram, the local position
	of the root is (0,0,0).

*************************************************/
virtual void CenterAnimation(float fram) {}

//---------------------------------------------------------------
// Merged Animations

/*************************************************
Summary: Returns the blending factor of blended animation.
Return Value: 
	Merging factor of blended animation.
Remarks:
	+ An Animation may be maded up of two other blended animations, the resulting 
	animation depends on a float factor between 0 and 1.

See Also: SetMergeFactor, IsMerged, CreateMergedAnimation
*************************************************/
virtual float GetMergeFactor() { return 0; }

/*************************************************
Summary: Sets the blending factor of blended animation.
Arguments: 
	factor: Merging factor of blended animation to be set.
Remarks:
	+ If the factor is 0 the animation is equal to the first animation used for creation,
	if its 1 the animation is equal to the second animation.

See Also: GetMergeFactor, IsMerged, CreateMergedAnimation
*************************************************/
virtual void  SetMergeFactor(float factor) { }

/*************************************************
Summary: Checks if the animation is blended or not.
Return Value:
	Returns TRUE if animation is blended, FALSE otherwise.
Remarks:
	+ An Animation may be maded up of two other blended animations, the resulting 
	animation depends on a float factor between 0 and 1.

See Also: GetMergeFactor, SetMergeFactor, CreateMergedAnimation
*************************************************/
virtual CKBOOL  IsMerged() { return FALSE; }

/*************************************************
Summary: Blend another animation with the current animation.

Arguments:
	anim2: A pointer to the animation to be merged with current animation.
	dynamic: If created animation should be dynamic.
Return Value:
	A pointer to the blended animation object. 
Remarks:
+ An Animation may be maded up of two other blended animations, the resulting 
animation depends on a float factor between 0 and 1.

See Also: GetMergeFactor, SetMergeFactor, IsMerged
*************************************************/
virtual	CKAnimation*	CreateMergedAnimation(CKAnimation *anim2,CKBOOL dynamic=FALSE) { return NULL; }

/*************************************************
Summary: Sets the current progression.
Remarks:
	+ This methods only sets the current progression without actually applying the 
	animation to the concerned entities.

See Also:SetStep
*************************************************/
virtual	void  SetCurrentStep(float Step) = 0;
//------------------------------------------------------------------
// Transition Animation

/*************************************************
Summary: Creates this animation as a transition between two others.

Arguments:
	in: A pointer to the first animation.
	out: A pointer to the second animation.
	OutTransitionMode: A CK_ANIMATION_TRANSITION_MODE Transition mode.
	length: Length of transition animation.
	FrameTo: Destination frame in second animation.

Return Value:
	A value indicating the frame on which the next animation will start.
	
Remarks:
	+ This animation will be a transition animation between in and out.
	+ The return value is where (in term of number of frame ) to start the next animation.
	+ CK_ANIMATION_TRANSITION_MODE precise the type of transition that will done. 


See Also: CK_ANIMATION_TRANSITION_MODE
*************************************************/
virtual	float CreateTransition(CKAnimation *in,CKAnimation *out,CKDWORD OutTransitionMode,float length=6.0f,float FrameTo=0) {return 0.0f;} //by nicolasp return NULL; }


CKAnimation(CKContext *Context=NULL,CKSTRING name=NULL) : CKSceneObject(Context,name) {} 

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
static CKAnimation* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_ANIMATION)?(CKAnimation*)iO:NULL;
}

};

#endif

