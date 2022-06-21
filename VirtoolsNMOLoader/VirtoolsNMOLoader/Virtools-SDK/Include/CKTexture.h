/*************************************************************************/
/*	File : CKTexture.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKTEXTURE_H

#define CKTEXTURE_H "$Id:$"

#include "CKBeObject.h"
#include "CKBitmapData.h"



/**************************************************************************
{filename:CKTexture}
Name: CKTexture

Summary: Class for managing textures applied to objects.

Remarks:
	+ CKTexture is used to described textures applied to objects. As a child
	class of CKBeObject, textures can also have behaviors.
	Images describing textures can be loaded from various file formats (GIF,JPG,BMP,TGA,TIFF,AVI,PCX).

	+ A texture is not bound to contain only one image. Additional images can be 
	added to a texture or loaded through  LoadMovie or LoadImage function.
	
	+ A texture surface information is stored in a buffer in system memory and load into video memory 
	when needed. You can acces system memory surface with LockSurfacePtr for procedural texturing.

	+ The class identifier of CKTexture is CKCID_TEXTURE.



See also: CKBitmapData,CKMaterial,CKRenderContext
***************************************************************************/
class CKTexture : public CKBeObject,public CKBitmapData {
public :
//-----------------------------------------------------------
// FILE IO
/*******************************************************
Summary: Creates an empty image.
Arguments:
	Width: width in pixel of the image to create
	Height: height in pixel of the image to create
	BPP: Bit per pixel of the image
	Slot: If there a multiple images, index of the image slot to create.
Return Value: TRUE if successful, FALSE otherwise.
Remarks:
+ The image is initialized to black with full alpha (all colors to 0xFF000000)
+ The name for the created slot is set to ""
+ If there was already some slots created but with a different width or height 
the method returns FALSE.

See Also:SaveImage,CKBitmapData::CreateImage
*******************************************************/
virtual	CKBOOL	Create(int Width,int Height,int BPP=32,int Slot=0) = 0;
/*************************************************
Summary: Loads a image slot from a file.

Arguments:
	Name: Name of the file to load.
	Slot: In a multi-images texture, index of the image slot to load. 
	(-1 means load all slots)
Return Value: 
	TRUE if successful, FALSE otherwise.
Remarks:
	+ The supported image formats depend on the image readers installed. The default available
	readers support BMP,TGA,JPG,PNG and GIF format
See also: SaveImage
*************************************************/
virtual CKBOOL	LoadImage(CKSTRING Name,int Slot=0) = 0;
/************************************************
Summary: Creates a multi-image texture from a movie file.

Arguments:
	Name: Movie file to load.
Return Value: TRUE if loading was successful, FALSE otherwise.
Remarks:
	+ The supported movie formats depend on the image readers and codecs installed. The default available
	reader support AVI files.
See also: LoadImage,Create
************************************************/
virtual CKBOOL	LoadMovie(CKSTRING Name) = 0;

/*************************************************
Summary: Sets this texture as the current to to use when drawing primitives.

Arguments:
	Dev: A pointer to the CKRenderContext on which this texture should be set as current.
	Clamping: TRUE if the texture addressing mode is clamp.	
	TextureStage: Texture stage on which the texture should be set.
Return Value:
	TRUE if successful.

Remarks:
	+ When setting a texture as current the render engine will use it when drawing primitives.
	+ This method is automatically called by CKMaterial::SetAsCurrent.
	+ If the texture is transparent alpha testing is enabled (SetState(VXRENDERSTATE_ALPHATESTENABLE,TRUE)

See also:CKRenderContext::SetCurrentMaterial,CKMaterial::SetAsCurrent,CKRenderContext::SetState
*************************************************/
virtual CKBOOL  SetAsCurrent(CKRenderContext* Dev,CKBOOL Clamping =FALSE,int TextureStage = 0) = 0;

//-------------------------------------------------------------
// VIDEO MEMORY MANAGEMENT 

/*************************************************
Summary: Restore texture video memory content

Return Value: 
	TRUE if successful, FALSE otherwise.
Remarks:
	+ If the texture is already in video memory, this function copies 
	the content of the system memory data to video memory otherwise it fails.
See also: SystemToVideoMemory
************************************************/
virtual	CKBOOL	Restore(CKBOOL Clamp = FALSE) = 0;
/*************************************************
Summary: Allocates and copies the texture image from system to video memory.

Arguments:
	Dev: A pointer to a CKRenderContext on which the texture is to be used.
Return Value: 
	TRUE if successful, FALSE otherwise.
Remarks:
+This method is automatically called by the framework when a texture
needs to be drawn and is not present in video memory.
+Use this function to creates the video memory copy.
+Usually, you can call FreeVideoMemory for textures that won't be visible for a long time. When one of
these sprites should be used again, calling SystemToVideoMemory ensures that the texture
will be stored into video memory.
See also: FreeVideoMemory,Restore
************************************************/
virtual CKBOOL	SystemToVideoMemory(CKRenderContext *Dev,CKBOOL Clamping =FALSE) = 0;

/*************************************************
Summary: Frees the texture video memory.

Return Value: 
	TRUE if successful, FALSE otherwise.
Remarks:
+ Use this function to manage which textures should be stored in video memory. Some video
cards may use textures stored in sytem memory (ie. AGP).
+ This method can be used in low video memory configuration
to flush textures that have not been used for a long time.
See also: SystemToVideoMemory
************************************************/
virtual CKBOOL	FreeVideoMemory() = 0;

/*************************************************
Summary: Returns whether the texture is present in video memory

Return Value: 
	TRUE if the sprite is in video memory FALSE otherwise.
See also: SystemToVideoMemory
*************************************************/
virtual CKBOOL	IsInVideoMemory() = 0;
/*************************************************
Summary: Copies the content of the back buffer to the texture video memory.

Arguments:
	ctx: A pointer to the CKRenderContext which content should be copied.
	Src: Source rectangle in the render context .
	Dest: Destination rectangle in the texture (Must be the same size than Src).
Return Value: 
	TRUE if successful, FALSE otherwise.
Remarks:
+ This method can be used to copy the content of the render context (back buffer)
to a texture.
+ This does not update the system memory copy.
See also: CKRenderContext::SetRenderTarget
************************************************/
virtual	CKBOOL	CopyContext(CKRenderContext* ctx,VxRect* Src,VxRect* Dest,int CubeMapFace =0) = 0;
/*************************************************
Summary: Generates the mipmap levels.

Return Value:
	TRUE if successful
Arguments:
	UseMipMap: TRUE if mipmap levels should be generated .
Remarks:
If generation of mipmaps levels is enabled they are automatically generated each time the texture video surface is restored (Restore or SystemToVideoMemory).
See Also: GetMipmapCount
*************************************************/
virtual CKBOOL	UseMipmap(int UseMipMap) = 0;
/*************************************************
Summary: Returns whether mipmaps levels are automatically generated.

Return Value:
	TRUE if mipmap levels are generated .
Remarks:
If generation of mipmaps levels is enabled they are automatically generated each
time the texture video surface is restored (Restore or SystemToVideoMemory).
See Also: UseMipmap
*************************************************/
virtual int		GetMipmapCount() = 0;

/*************************************************
Summary: Returns a information about how texture is stored in video memory.

Arguments:
	desc: A reference to a VxImageDescEx structure that will be filled with video memory format information.
Return Value: 
	FALSE if the texture is not in video memory or invalid, TRUE otherwise.
See Also: GetVideoPixelFormat,SetDesiredVideoFormat
*************************************************/
virtual CKBOOL			GetVideoTextureDesc(VxImageDescEx& desc) = 0;
/*************************************************
Summary: Returns the pixel format of the texture surface in video memory.

Return Value:
	Pixel format of video memory surface (VX_PIXELFORMAT) or VX_UNKNOWNPF 
	if the texture is not in video memory.
See Also: SetDesiredVideoFormat, GetVideoTextureDesc
*************************************************/
virtual VX_PIXELFORMAT	GetVideoPixelFormat() = 0;
/*************************************************
Summary: Gets Image format description

Return Value:
	TRUE
Arguments:
	desc: A reference to a VxImageDescEx structure that will be filled with system memory format information.
Remarks:
	+ The desc parameter will be filled with the size,pitch,bpp and mask information
	on how the texture is stored in system memory.
See Also: GetVideoPixelFormat,SetDesiredVideoFormat
*************************************************/
virtual CKBOOL			GetSystemTextureDesc(VxImageDescEx& desc) = 0;

/************************************************
Summary: Creates a set of mipmap level image the user can specify.

Return Value:
	TRUE if successful, FALSE otherwise
Arguments:
	UserMipmap: TRUE to use user specified images as mipmap FALSE otherwise.
Remarks:
+ The default behavior for a texture is to automatically generate
its mipmap level images when loaded on the video card. A user can
specify its own images using this method.
+ To set up the image to use for a particular level use GetMipMapLevelImageBuffer
+ This method only works for mono-slot textures.
See Also:GetUserMipMapLevel
************************************************/
virtual CKBOOL	SetUserMipMapMode(CKBOOL UserMipmap) = 0;

/************************************************
Summary: Gets access to a user mipmap level.

Return Value:
	TRUE if successful, FALSE otherwise
Remarks:
+ This method returns a VxImageDescEx structure which contain all details (included surface pointer in .Image member)
about a user specified level of mipmap.
+ This method must be called for every available level of mipmap before the
texture is used otherwise once every level of mipmap have been set, you must called 
FreeVideoMemory to ensure the texture in video memory reflects the changes you made to the
mipmaps level...
See Also:SetUserMipMapMode
************************************************/
virtual BOOL	GetUserMipMapLevel(int Level,VxImageDescEx& ResultImage) = 0;


/************************************************
Summary: Gets the texture index of this texture in the rasterizer context.

Return Value:
	Index of this texture in the rasterizer context
Remarks:
+ The default implementation of the render engine uses an underlying rasterizer to store
and render primitives.
+ This method returns the index of the texture as stored by the CKRasterizerContext.
+ With the render engine source code you can then access to the driver specific (DX5,7,8,OpenGL)
data for this texture.
See Also:GetRstTextureObject,CKRenderContext::GetRasterizerContext
************************************************/
virtual int	GetRstTextureIndex() = 0;

/************************************************
Summary: Gives access to the video memory surface pointer.

Return Value:
	TRUE if successful, FALSE if the texture is not currently in video memory
Remarks:
+ This method fills the Surface structure with information regarding the 
video memory surface. Surface pointer is stored in the Surface.Image member.
+ Once finish with the surface pointer you must release it by calling UnlockVideoMemory.
 Any rendering with a locked texture will fail.
+ On some implementations such as GL or DX8 that do not support direct access to video memory
the returned pointer can be on a system memory copy which can be very slow for partial write access.
See Also:VxImageDescEx,LockSurfacePtr,
************************************************/
virtual BOOL	LockVideoMemory(VxImageDescEx& Surface,int MipLevel = 0,VX_LOCKFLAGS Flags = VX_LOCK_DEFAULT) = 0;

virtual void	UnlockVideoMemory(int MipLevel = 0) = 0;


/************************************************
Summary: Transfers the content of the video memory to the system memory copy.

Return Value:
	TRUE if successful, FALSE if the texture is not currently in video memory
Remarks:
+ The System Caching mode must be CKBITMAP_PROCEDURAL or CKBITMAP_VIDEOSHADOW
for this method to work (that is a system copy must exist)
See Also:LockVideoMemory,LockSurfacePtr,
************************************************/
virtual BOOL	VideoToSystemMemory() = 0;

/************************************************
Summary: Gets the texture object of this texture in the rasterizer context.

Return Value:
A pointer to a structure that represents the texture object or NULL if the texture
is not yet stored in the rasterizer context.
Remarks:
+The default implementation of the render engine uses an underlying rasterizer to store
and render primitives.
+This method returns the object of the texture as stored by the CKRasterizerContext.
+With the render engine source code you can then access to the driver specific (DX7,8,9,OpenGL)
data for this texture by casting the returned pointer in the appropriate structure (CKDX8TextureDesc,CKDX9TextureDesc).
See Also:GetRstTextureIndex,CKRenderContext::GetRasterizerContext
************************************************/
virtual void*	GetRstTextureObject() = 0;

/************************************************
Summary: This function ensure the correct representation of the texture is in video memory.
Remarks:
+This method is automatically called by SetAsCurrent to ensure 
the correct data is present in video memory (in the case of a movie texture for example).
+It can be called by a shader manager to ensure the texture is ready in video memory.
See Also:SetAsCurrent,SystemToVideoMemory,Restore
************************************************/
virtual BOOL	EnsureVideoMemory(CKRenderContext* ctx,CKBOOL Clamping =FALSE) = 0;


CKTexture(CKContext *Context,CKSTRING name=NULL)  : CKBeObject(Context,name),CKBitmapData(Context) {}	

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
static CKTexture* Cast(CKObject* iO) 
{
	return CKIsChildClassOf(iO,CKCID_TEXTURE)?(CKTexture*)iO:NULL;
}

/************************************************
Summary: Swap this texture internal datas with another texture object. The object name is not swapped.
************************************************/
virtual void Swap(CKTexture& other) = 0;

/************************************************
Summary: Transfers the content of another texture (possibly this texture) into a target slot 

Return Value:
	TRUE if successful, FALSE if the texture is not currently in video memory, or if the target slot do not exist
Remarks:
+ The System Caching mode must be CKBITMAP_PROCEDURAL or CKBITMAP_VIDEOSHADOW
for this method to work (that is a system copy must exist)
See Also:LockVideoMemory,LockSurfacePtr,
************************************************/
virtual BOOL	VideoToSlotSystemMemory(CKTexture& src, CKDWORD destSlot) = 0;


};

#endif
