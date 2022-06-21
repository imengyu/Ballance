/*************************************************************************/
/*	File : CKSprite3d.h													 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#if !defined(CKSPRITE3D_H) || defined(CK_3DIMPLEMENTATION)

#define CKSPRITE3D_H "$Id:$"
#ifndef CK_3DIMPLEMENTATION

#include "CK3dEntity.h"


#undef CK_PURE

#define CK_PURE = 0

/****************************************************************************
{filename:CKSprite3D}
Summary: Sprite with 3d position and orientation.

Remarks:
{Image:Sprite3D_ex}
+ This class represents a sprite with a position and orientation in 3D.
The CKSprite3D class is derived from the CK3DEntity class.
+ The representation of the Sprite3D is given by a material which contain 
the texture and render settings.
+ The sprite3D orientation can be free or constrained (billboard or axis specific).
+ The Class id of a CKSprite3D is CKCID_SPRITE3D
+ Size and position of a sprite relatively to is referential (CK3dEntity) can be controlled using
SetSize and SetOffset.
{Image:Sprite3D_Offset}
+ The default size of a sprite is (1,1) and its offset is (0,0)

See also: CK3dEntity,CKMaterial
***************************************************************************/
class CKSprite3D : public CK3dEntity {
public:
#endif

/************************************************
Summary: Sets the material used by the sprite for rendering.
Arguments:
	mat: A pointer to a CKMaterial to use for rendering.  
Remarks:
	+ The material specify the texture and render options used to render the sprite.
	+ A Sprite3D with no material will not be rendered.

See also: GetMaterial,CKMaterial,CKTexture,Using Materials
************************************************/
virtual 	void		SetMaterial(CKMaterial *Mat)  CK_PURE;

/************************************************
Summary: Returns the material used by the sprite for rendering.
Return Value: A pointer to the CKMaterial used for rendering.  
Remarks:
	+ The material specify the texture and render options used to render the sprite.
	+ A Sprite3D with no material will not be rendered.

See also: SetMaterial,CKMaterial,CKTexture
************************************************/
virtual 	CKMaterial	*GetMaterial()  CK_PURE;

/************************************************
Summary: Sets the size of the rectangular shape of the sprite.
Arguments:
	vect: The size in local coordinates.
Remarks:
	+ The default size of a sprite3D is (1,1)

See also: GetOffset, GetSize
************************************************/
virtual  void		SetSize(Vx2DVector& vect)  CK_PURE;

/************************************************
Summary: Gets the size of the rectangular shape of the sprite.
Arguments:
	vect: A Vx2DVector that will be filled with the size of the Sprite3D in local coordinates, to be filled.
Remarks:
	+ The default size of a sprite3D is (1,1)

See also: SetSize
************************************************/
virtual  void		GetSize(Vx2DVector& vect)  CK_PURE;


/************************************************
Summary: Sets the offset between the 3dEntity center and the center of the rectangular shape of the sprite.
Arguments:
	vect: The offset in local coordinates.
Remarks:
  + Offset is used to specify offset between the sprite vertices and the 3dEntity center
   for example, if OffsetX = 0.0 and OffsetY = 1.0 the sprite rotates around  
  the center of its bottom edge. Values x = 1.0 and y = 1.0 specifies the lower left corner and
  values x = -1.0 and y = -1.0 specifies the top right corner. 

See also: GetOffset,SetSize,GetSize
************************************************/
virtual  void		SetOffset(Vx2DVector& vect)  CK_PURE;

/************************************************
Summary: Returns the offset between the 3dEntity center and the center of the rectangular shape of the sprite.
Arguments:
	vect: The offset in local coordinates.
Remarks:
  + Offset is used to specify offset between the sprite vertices and the 3dEntity center
   for example, if OffsetX = 0.0 and OffsetY = 1.0 the sprite rotates around  
  the center of its bottom edge. Values x = 1.0 and y = 1.0 specifies the lower left corner and
  values x = -1.0 and y = -1.0 specifies the top right corner. 

See also: GetOffset,SetSize,GetSize
************************************************/
virtual  void		GetOffset(Vx2DVector& vect)  CK_PURE;

/************************************************
Summary: Sets the texture mapping of the Sprite3D. 
Arguments:
	Rect: Homogenous Texture coordinates.
Remarks:
	+ Default texture coordinates are (0,0) <=> (1,1)

See also: GetUVMapping
************************************************/
virtual  void		SetUVMapping(VxRect& rect)  CK_PURE;

/************************************************
Summary: Returns the texture mapping of the Sprite3D. 
Arguments:
	Rect: A VxRect that will be filled with the Texture coordinates.
Remarks:
	+ Default texture coordinates are (0,0) <=> (1,1)

See also: GetUVMapping
************************************************/
virtual  void		GetUVMapping(VxRect& rect)  CK_PURE;

/************************************************
Summary: Sets the orientation mode of the Sprite3D.
Arguments:
	Mode:	A VXSPRITE3D_TYPE type.
Remarks:
+ This function sets the behavior of the sprite regarding to the camera (ie. you can specify
if the sprite is locked on one axis or not).
+ The default mode is VXSPRITE3D_BILLBOARD where the sprite3D always faces the current camera.

See also: VXSPRITE3D_TYPE,GetMode
************************************************/
virtual  void	SetMode(VXSPRITE3D_TYPE Mode)  CK_PURE;


/************************************************
Summary: Gets the orientation mode of the Sprite3D.
Return value: Orientation mode
Remarks:

See also: VXSPRITE3D_TYPE, SetMode
************************************************/
virtual  VXSPRITE3D_TYPE	GetMode()  CK_PURE;


/*************************************************
Summary: Dynamic cast operator. 
Arguments:
	iO: A pointer to a CKObject to cast.
Return Value:
	iO casted to the appropriate class or NULL if iO is not from the required class .
Example:
	  CKObject* Object;
	  CKSprite3D* anim = CKSprite3D::Cast(Object);
Remarks:

*************************************************/
static CKSprite3D* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_SPRITE3D)?(CKSprite3D*)iO:NULL;
}

#ifndef CK_3DIMPLEMENTATION
};

#endif
#endif
