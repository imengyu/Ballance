/*************************************************************************/
/*	File : CKCamera.h				 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CKCAMERA_H) || defined(CK_3DIMPLEMENTATION)
#ifndef CK_3DIMPLEMENTATION

#define CKCAMERA_H "$Id:$"


#include "CK3dEntity.h"



#define CK_PERSPECTIVEPROJECTION  1

#define CK_ORTHOGRAPHICPROJECTION 2



#undef CK_PURE

#define CK_PURE = 0

/*************************************************
{filename:CKCamera}
Summary: Camera 

Remarks:
{Image:Camera}
+ A CKCamera represents a potential configuration for a viewpoint:
front plane, back plane, field of view, roll, projection type, zoom type, 
aspect ratio. As it is derived from CK3DEntity and CKBeObject, it has
all the potentialities from those classes, especially the possibility
to have behaviors attached to it. The orientation and position of the camera
is defined the same way a 3dEntity does.

+ For a camera to become active in a rendercontext you should attach the 
viewpoint to it using the function CKRenderContext::AttachViewpointToCamera()

+ Field of View (FOV): It controls how much of a scene is visible and is an angle
measured in radians. The Dev interface gives acces to the notion of focal length
used in photography.  For a standard 35 mm camera the conversion formula between
field of view and focal length is :

		Field Of View = 2 x ArcTan( (35/2) / Focal Length)

+ The class id of CKCamera is CKCID_CAMERA.
See also: CKRenderContext,CKTargetCamera
*************************************************/
class CKCamera : public CK3dEntity {
public:
#endif 

/*************************************************
Summary: Returns the front clipping plane distance

Return Value:
	Front clipping plane distance.
Remarks:
	+ The front plane distance is the distance below which nothing is seen by the camera.
	+ By default, at creation time, the front plane distance is equal to 1.0f.
See also: GetBackPlane,SetFrontPlane
*************************************************/
virtual float	GetFrontPlane() CK_PURE;
/*************************************************
Summary: Sets the front clipping plane distance

Arguments:
	front: The new front clipping plane distance
Remarks:
	+ The front plane distance is the distance below which nothing is seen by the camera.
	+ By default, at creation time, the front plane distance is equal to 1.0f.
See also: SetBackPlane,GetFrontPlane
*************************************************/
virtual void	SetFrontPlane(float front) CK_PURE;
/*************************************************
Summary: Returns the back clipping plane distance

Return Value:
	Back clipping plane distance.
Remarks:
	+ The back plane distance is the distance beyond which nothing is seen by the camera.
	+ By default, at creation time, the back clipping plane distance is equal to 4000.0f.
See also: SetBackPlane,SetFrontPlane
*************************************************/
virtual float	GetBackPlane() CK_PURE;
/*************************************************
Summary: Sets the back clipping plane distance

Arguments:
	back: The new back clipping plane distance
Remarks:
	+ The back plane distance is the distance beyond which nothing is seen by the camera.
	+ By default, at creation time, the back clipping plane distance is equal to 4000.0f.
See also: GetBackPlane,SetFrontPlane
*************************************************/
virtual void	SetBackPlane(float back) CK_PURE;

/*************************************************
Summary: Returns the field of view of the camera

Return Value:
	Field of view of the camera.
Remarks:
	+ By default, the field of view is equal to: 0.5f ( 30°)
See also: SetFov
*************************************************/
virtual float	GetFov() CK_PURE;
/*************************************************
Summary: Changes the field of view of the camera

Arguments:
	 fov: The new field of view of the camera
Remarks:
	+ By default, the field of view is equal to: 0.5f ( 30°)
See also: GetFov
*************************************************/
virtual void	SetFov(float fov) CK_PURE;


/*************************************************
Summary: Returns the current projection type

Return Value:
	CK_PERSPECTIVEPROJECTION : perspective projection (default)
	CK_ORTHOGRAPHICPROJECTION : orthographic projection
Remarks:
	+ The project type is either orthographic or perspective.
See also: SetProjectionType,SetOrthographicZoom
*************************************************/	
virtual int 	GetProjectionType()	 CK_PURE;
/*************************************************
Summary: Sets the projection type of the camera

Arguments:
	proj: The new projection type
Remarks:
+ The project type is either orthographic or perspective.
+ proj = CK_PERSPECTIVEPROJECTION : perspective projection (default)
+ proj = CK_ORTHOGRAPHICPROJECTION : orthographic projection
+ If the projection is orthographic, you can specify a zoom value.
See also: GetProjectionType,SetOrthographicZoom
*************************************************/	
virtual void	SetProjectionType(int proj) CK_PURE;

/*************************************************
Summary: Changes the zoom value in orthographic projection

Arguments:
	zoom: The new zoom value
Remarks:
+ Changes the zoom value. Valid only if in orthographic projection mode.
+ The bigger the value, the bigger the objects... 
+ By default, at creation time, the zoom of the camera is equal to: 1.0f
See also: GetOrthographicZoom,SetProjectionType
*************************************************/
virtual void	SetOrthographicZoom(float zoom) CK_PURE;
/*************************************************
Summary: Returns the zoom value in orthographic projection

Return value
	Zoom factor.
See also: SetOrthographicZoom,SetProjectionType
*************************************************/
virtual float	GetOrthographicZoom() CK_PURE;

//---------------------------------------
// AspectRatio		 


/*****************************************************
Summary:Sets the aspect ratio

Remarks:
	+ The given aspect ratio will be used to resize the 
	viewport of the render context on which this camera is 
	attached.
See also: GetAspectRatio,CKRenderContext::SetViewRect,CKRenderContext::AttachViewpointToCamera
*****************************************************/
virtual void SetAspectRatio(int width,int height) CK_PURE;
/*****************************************************
Summary:Gets the aspect ratio

Remarks:
	+ The given aspect ratio will be used to resize the 
	viewport of the render context on which this camera is 
	attached.
See also: SetAspectRatio,CKRenderContext::SetViewRect,CKRenderContext::AttachViewpointToCamera
*****************************************************/
virtual void GetAspectRatio(int& width,int& height) CK_PURE;

//-------------------------------------
// Result Projection Matrix

/*****************************************************
Summary: Computes the projection matrix

Remarks:
	+ This method uses the fov,aspect ratio, back and front clipping to compute
	a projection matrix.
See also: VxMatrix::Perspective,VxMatrix::Orthographic
*****************************************************/
virtual void	ComputeProjectionMatrix(VxMatrix& mat) CK_PURE;

//---------------------------------------
// Roll Angle				

/*****************************************************
Summary:Rolls back the camera to vertical.

Remarks:
+Rolls back the camera to vertical by aligning its Up axis (Y) 
with the world up axis.
+If the camera as a target this method as no effect as the orientation
of the camera will be forced toward its target.

See Also:CKTargetCamera,SetTarget,Roll
*****************************************************/
virtual void ResetRoll() CK_PURE;
/*****************************************************
Summary:Rolls the camera of the desired angle,

Arguments:
	angle: Angle of rotation around Z axis in radians.
Remarks:
+ If the camera as a target this method as no effect as the orientation
of the camera will be forced toward its target.
+ To align the camera back with the Up vector in the world use ResetRoll
See Also:CKTargetCamera,SetTarget,ResetRoll
*****************************************************/
virtual void Roll(float angle) CK_PURE;

//-------------------------------------------
// Target Access

/*****************************************************
Summary:Returns the target of this camera.

Return Value:
	A pointer to the CK3dEntity used as target for this camera.	
Remarks:
	+ This method is only implemented for a CKTargetCamera
	otherwise it returns NULL.
See Also:CKTargetCamera,SetTarget
*****************************************************/
virtual 	CK3dEntity *GetTarget() CK_PURE;
/*****************************************************
Summary:Sets the target of this camera.

Arguments:
	target: A pointer to the CK3dEntity to use as a target or NULL to remove the current target.
Remarks:
+ This method is only implemented for a CKTargetCamera.
+ If the camera has a target its orientation it always 
updated toward the given target.
See Also:CKTargetCamera,GetTarget
*****************************************************/
virtual 	void SetTarget(CK3dEntity *target) CK_PURE;

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
static CKCamera* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_CAMERA)?(CKCamera*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
