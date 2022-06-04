/*************************************************************************/
/*	File : CKRenderContext.h											 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKRENDERCONTEXT_H
#define CKRENDERCONTEXT_H "$Id:$"

#include "CKObject.h"
#include "XObjectArray.h"
#include "VxDefines.h"
#include "CKMesh.h"

/*********************************************************************
Name: CKPickResult

Summary: Structure storing results of a picking on screen

Remarks:
    When picking occurs the function Pick() returns the underlying object on screen
and fill this structure with additional information.

See also: CKRenderContext::Pick
*********************************************************************/
typedef struct CKPICKRESULT
{
    VxVector IntersectionPoint;	 // Intersection Point in the Object referential coordinates
    VxVector IntersectionNormal; // Normal  at the Intersection Point
    float TexU, TexV;			 // Textures coordinates
    float Distance;				 // Distance from Viewpoint to the intersection point
    int FaceIndex;				 // Index of the face where the intersection occured
    CK_ID Sprite;				 // If there was a sprite at the picked coordinates, ID of this sprite
} CKPICKRESULT;

/*************************************************************
Summary: Specify the type of overriding when rendering a scene.

See Also: CKRenderContext::SetOverriddenRendering
**************************************************************/
enum CK_OVERRIDE_RENDERING
{
    CK_OVERRIDE_NONE,		// No Override
    CK_OVERRIDE_MATERIAL,	// Entity materials are ignored and the material given to CKRenderContext::SetOverriddenRendering is used instead
    CK_OVERRIDE_USECURRENT, // Entity materials are ignored and the currently set render options are used instead...
    CK_OVERRIDE_PASS,		// Only the specific pass given to CKRenderContext::SetOverriddenRendering will be used when drawing object with effects.
};

/*********************************************************************
Summary: Managing device for rendering


Remarks:
{Image:RenderContext}
+ A CKRenderContext is where rendering occurs. It manages a list of objects to
be rendered.

+ At Creation time, a render context is attached to a window from which it
will take size information. At anytime the rendercontext can be switch forth and back
to fullscreen mode.

+ CKRenderContext provides methods for Fog management, Primitive drawing, ambient lighting
and cameras management.

+ Callbacks can be set to have functions called before and after rendering occurs.

+ The rendercontext contains a list of objects to be rendered. When creating a level all the objects
of the level will be added to its render contextes (attached through the function CKLevel::AddRenderContext).
If a rendercontext is used without a level the user must attach the objects he wants to be rendered.

+ CKRenderContext is created through the CKRenderManager::CreateRenderContext() method.

+ The class ID of CKRenderContext is CKCID_RENDERCONTEXT.
See also: CKRenderManager
*********************************************************************/
class CKRenderContext : public CKObject
{
public:
    /*************************************************
    Summary: Adds an object to the render context.

    Arguments:
        obj: A pointer to a CKRenderObject to attach to the rendercontext.
    Remarks:
    + This function does not need to be called if you are using scenes, since every objects
    in a scene are automatically added to the render contextes a level use.
    + But you may need to render objects without them being added to the level
    in which case you must attach them to the rendercontext.
    + Both 2dEntities (Sprites) and 3dEntities can be added to a rendercontext.
    See also:IsObjectAttached,RemoveObject,AddObjectWithHierarchy,DetachAll,AddRemoveSequence
    *************************************************/
    virtual void AddObject(CKRenderObject *obj) = 0;
    /*************************************************
    Summary: Adds an object to the rendercontext along with its hierarchy.

    Arguments:
        obj: A pointer to a CKRenderObject to attach to the rendercontext.
    Remarks:
        + Adds obj and all its children to the render context.
    See also:IsObjectAttached,RemoveObject,AddObject,DetachAll
    *************************************************/
    virtual void AddObjectWithHierarchy(CKRenderObject *obj) = 0;
    /*************************************************
    Summary: Removes an object from the rendercontext

    Arguments:
        obj: Object to be removed from the rendercontext.
    See also:IsObjectAttached,AddObjectWithHierarchy,AddObject,DetachAll
    *************************************************/
    virtual void RemoveObject(CKRenderObject *obj) = 0;
    /*************************************************
    Summary: Returns whether an object is attached to the render context.

    Arguments:
        obj: A pointer to a CKRenderObject
    Return Value:
        TRUE if the object is attached to the Rendercontext,
        FALSE otherwise.
    See also:AddObject,RemoveObject,AddObjectWithHierarchy,DetachAll
    *************************************************/
    virtual CKBOOL IsObjectAttached(CKRenderObject *obj) = 0;

    /*************************************************
    Summary: Computes the list of root 3D Entities.

    Return Value:
        A reference to an XObjectArray containing the list of root 3D Entities.
    Remarks:
    + Return list with the 3D Entities that do not have a parent.
    See also:Compute2dRootObjects,XObjectArray
    *************************************************/
    virtual const XObjectArray &Compute3dRootObjects() = 0;
    /*************************************************
    Summary: Computes the list of root 2D Entities.

    Return Value:
        A reference to an XObjectArray containing the list of root 2D Entities.
    Remarks:
        + Fills list with the 2D Entities that do not have a parent.
    See also:Compute3dRootObjects,XObjectArray
    *************************************************/
    virtual const XObjectArray &Compute2dRootObjects() = 0;
    /*************************************************
    Summary: Gets the root of the 2D entities.

    Return Value:
        A pointer to either the background or foreground 2D root CK2dEntity
    Arguments:
        background: either background or foreground root.
    Remarks:
    See also:Compute2dRootObjects
    *************************************************/
    virtual CK2dEntity *Get2dRoot(CKBOOL background) = 0;
    /*************************************************
    Summary: Removes all the objects from the rendercontext.

    See also:IsObjectAttached,AddObjectWithHierarchy,AddObject,RemoveObject
    *************************************************/
    virtual void DetachAll() = 0;

    /*************************************************
    Summary: Updates render context settings according to the current camera.

    Remarks:
        + Every frame the settings of the current camera are used to update the current render context (Position,Fov,Aspect Ration,Clip Planes,etc...).
        + This function force the settings to be updated immediatly.
    See also:PrepareCameras
    *************************************************/
    virtual void ForceCameraSettingsUpdate() = 0;
    /*************************************************
    Summary: Prepares cameras positions before rendering

    Remarks:
    + This function updates the orientation of all target lights and cameras so that they point toward their targets.
    + If there is an active camera to which the viewpoint is attached, the rendercontext is updated according to the camera settings
    (Field of View,Type of projection (Perspective or Orthographic),Clip planes).
    + It finally updates the size of the viewport according to active camera aspect ratio.
    + This method is automatically called by the Render method.
    See also:Render,ForceCameraSettingsUpdate,SetViewRect,Understanding the Render Loop
    *************************************************/
    virtual void PrepareCameras(CK_RENDER_FLAGS Flags = CK_RENDER_USECURRENTSETTINGS) = 0;
    /*************************************************
    Summary: Clears the render context

    Arguments:
        Flags : A combination of CK_RENDER_FLAGS specifying which buffers should be cleared.
        Stencil: Stencil value to use to clear the stencil.
    Return Value:
        CK_OK if successful or an error code otherwise.
    Remarks:
    + The render engine uses the current background material settings (diffuse color and texture) to clear the background.
    + If Flags is CK_RENDER_USECURRENTSETTINGS (default) the rendering is done using
    the current render options (See GetCurrentRenderOptions).
    + See Understanding the Render Loop paper for more details on the rendering operations.
    See also:Render,DrawScene,BackToFront,Understanding the Render Loop
    *************************************************/
    virtual CKERROR Clear(CK_RENDER_FLAGS Flags = CK_RENDER_USECURRENTSETTINGS, CKDWORD Stencil = 0) = 0;
    /*************************************************
    Summary: Renders the scene.

    Return Value:
        CK_OK if successful or an error code otherwise.
    Remarks:
        + Rendering the scene consists in drawing the 2D and 3D entities attached to this render context.
        + See Understanding the Render Loop paper for more details on the rendering operations.
    See also:Clear,Render,BackToFront,Understanding the Render Loop
    *************************************************/
    virtual CKERROR DrawScene(CK_RENDER_FLAGS Flags = CK_RENDER_USECURRENTSETTINGS) = 0;
    /*************************************************
    Summary: Copies the backbuffer to the frontbuffer

    Arguments:
        Flags : A combination of CK_RENDER_FLAGS.
    Return Value:
        CK_OK if successful or an error code otherwise.
    See also:Clear,Render,DrawScene,Understanding the Render Loop
    *************************************************/
    virtual CKERROR BackToFront(CK_RENDER_FLAGS Flags = CK_RENDER_USECURRENTSETTINGS) = 0;

    /*******************************************************
    Summary: Render a single frame

    Arguments:
        Flags : A combination of CK_RENDER_FLAGS.
    Return Value:
        CK_OK if successful or an error code otherwise.
    Remarks:
    + If Flags is CK_RENDER_USECURRENTSETTINGS (default) the rendering is done using
    the current render options (See GetCurrentRenderOptions).
    + The main actions performed by this function is to call respectively :
            PrepareCameras(Flags);
            Clear(Flags);
            DrawScene(Flags);
            BackToFront(Flags);
    + See Understanding the Render Loop paper for more details about the operations done by this function.
    See also:Understanding the Render Loop,
    *************************************************/
    virtual CKERROR Render(CK_RENDER_FLAGS Flags = CK_RENDER_USECURRENTSETTINGS) = 0;

    /*************************************************
    Summary: Adds or removes a function to call before rendering begins.

    Arguments:
        Function : A pointer to a function that will be called before rendering begins.
        argument: A void pointer that will be passed as argument to the function.
    Remarks:
        + See Understanding the Render Loop paper for more information on when callbacks function are called.
    See also:AddPostRenderCallBack,AddPostSpriteRenderCallBack
    *************************************************/
    virtual void AddPreRenderCallBack(CK_RENDERCALLBACK Function, void *Argument, CKBOOL Temporary = FALSE) = 0;

    virtual void RemovePreRenderCallBack(CK_RENDERCALLBACK Function, void *Argument) = 0;
    /*************************************************
    Summary: Adds or removes a function to call after 3D rendering is done and before sprites are drawn.

    Arguments:
        Function : A pointer to a function that will be called after 3D rendering is done.
        argument: A void pointer that will be passed as argument to the function.
    Remarks:
        + See Understanding the Render Loop paper for more information on when callbacks function are called.
    See also:AddPreRenderCallBack,AddPostSpriteRenderCallBack
    *************************************************/
    virtual void AddPostRenderCallBack(CK_RENDERCALLBACK Function, void *Argument, CKBOOL Temporary = FALSE, CKBOOL BeforeTransparent = FALSE) = 0;

    virtual void RemovePostRenderCallBack(CK_RENDERCALLBACK Function, void *Argument) = 0;
    /*************************************************
    Summary: Adds or removes a callback to be called after the foreground sprites rendering.

    Arguments:
        Function: A pointer to CK_RENDERCALLBACK function to call.
        Argument: A void pointer that will be passed as argument when calling Function.
    Remarks:
        + See Understanding the Render Loop paper for more details on the rendering operations.
    See also:Understanding the Render Loop,AddPostRenderCallBack,AddPreRenderCallBack
    *************************************************/
    virtual void AddPostSpriteRenderCallBack(CK_RENDERCALLBACK Function, void *Argument, CKBOOL Temporary = FALSE) = 0;

    virtual void RemovePostSpriteRenderCallBack(CK_RENDERCALLBACK Function, void *Argument) = 0;

    //---------------------------------------------------------
    // Allocation Functions

    /******************************************************
    Summary: Returns a pre-allocated VxDrawPrimitive structure

    Return Value:
        A pointer to a VxDrawPrimitiveData structure that holds the data.
    Arguments:
        Flags: A CKRST_DPFLAGS enumeration specifying the type of
        VertexCount: Number of vertices to allocate.
    Remarks:
    + To avoid re-allocation of memory each time user primitives are to be drawn this method returns a preallocated vertex pool that grows according to vertex count.
    + According to the format flags the returned VxDrawPrimitiveData structure is ready to be use with the DrawPrimitive method : vertex count,vertex format and transformation,clipping,lighting flags are set.
    + If the flags CKRST_DP_VBUFFER is set and the current context supports vertex buffers,the returned structure will point to a vertex buffer. The returned pointers must not be kept and should only be used to fill up the vertex data. Once the data have been set the vertex buffer must be unlocked before any call to DrawPrimitive via the ReleaseCurrentVB method.
    + To see if the returned structure effectively points to a vertex buffer, check that the CKRST_DP_VBUFFER is present in the structure m_Flags member.
    + Using vertex buffers enable you to directly write in driver optimal memory and avoid the overead of a recopy in temporary system memory. When using vertex buffers one should take care of filling up the vertex data in a sequential manner and avoid random write access to the returned memory pointers.
    See also:Custom Rendering,GetDrawPrimitiveIndices,LockCurrentVB,ReleaseCurrentVB,CKRST_DPFLAGS,VxDrawPrimitiveData,DrawPrimitive
    *************************************************/
    virtual VxDrawPrimitiveData *GetDrawPrimitiveStructure(CKRST_DPFLAGS Flags, int VertexCount) = 0;
    /*************************************************
    Summary: Returns a pre-allocated list of indices

    Return Value:
        A pointer to an array of WORD.
    Remarks:
    + Returns a pre-allocated array of WORD to be filled with indices for primitives to be drawn with DrawPrimitive method.
    See also:Custom Rendering,GetDrawPrimitiveStructure,DrawPrimitive
    *************************************************/
    virtual CKWORD *GetDrawPrimitiveIndices(int IndicesCount) = 0;

    //-----------------------------------------------------------
    // Transformations

    /*************************************************
    Summary: Transform vertices to screen coordinates.

    Arguments:
        Dest: A pointer to a VxVector that will be filled with the screen coordinates of Src
        Src: A pointer to the VxVector to transform.
        Ref: A optionnal pointer to CK3dEntity which is the referential in which Src is taken. If NULL Transform uses the current World Transformation Matrix.
    Remarks:
    + If no referential is given Transform takes the current world transformation (See SetWorldTransformationMatrix)
    matrix when converting from model coordinates to screen coordinates.
    + If several vertices should be transformed use TransformVertices instead of this method.
    See also:TransformVertices,SetWorldTransformationMatrix,CK3dEntity
    *************************************************/
    virtual void Transform(VxVector *Dest, VxVector *Src, CK3dEntity *Ref = NULL) = 0;
    /*************************************************
    Summary: Transforms an array of vertices to screen coordinates

    Arguments:
        VertexCount: Number of vertices to transform
        data: A pointer to a VxTransformData that describes the vectors to transform.
        Ref:  A optionnal pointer to CK3dEntity which is the referential from which . If NULL TransformVertices uses the current World Transformation Matrix.
    Remarks:

    o If no referential is given Transform takes the current world transformation
    matrix for converting from model coordinates to screen coordinates

    o The VxTransformData structure stores the pointer to the source vectors to transform and
    the results homogenous and/or screen coordinates. It can also contain a pointer to a list
    of clipping flags indicating the clipping status of every transformed vector.

    See also:Transform,VxTransformData,SetWorldTransformationMatrix,CK3dEntity
    *************************************************/
    virtual void TransformVertices(int VertexCount, VxTransformData *data, CK3dEntity *Ref = NULL) = 0;

    //-----------------------------------------------------------
    // FullScreen

    /*************************************************
    Summary: Switches the context to FullScreen Mode.

    Arguments:
        Width: width of the window and screen (Default : 640).
        Height: height of the window and screen ( Default : 480).
        Bpp: Desired Bit per pixel in FullScreen mode (Default : -1 specifies to keep current desktop Bit per pixel).
        Driver: Index of the driver to use to create the fullscreen context (Default 0).
        RefreshRate: Refresh rate to set for the fullscreen mode or 0 to use the system settings for the given display mode.
    Return Value:
        CK_OK if successful or an error code.
    Remarks:
    See also: StopFullScreen,IsFullScreen
    *************************************************/
    virtual CKERROR GoFullScreen(int Width = 640, int Height = 480, int Bpp = -1, int Driver = 0, int RefreshRate = 0) = 0;
    /*************************************************
    Summary: Switches back from FullScreen mode to windowed mode

    Return Value:
        CK_OK if successful or an error code.
    Remarks:
    This function restores the previous display mode.
    See also:GoFullScreen,IsFullScreen
    *************************************************/
    virtual CKERROR StopFullScreen() = 0;
    /*************************************************
    Summary: Returns whether the context is in FullScreen mode.

    Return Value:
        TRUE if the context is in FullScreen mode, FALSE otherwise
    See also:StopFullScreen,GoFullScreen
    *************************************************/
    virtual CKBOOL IsFullScreen() = 0;
    /*****************************************************
    Summary:Returns the index of the driver used by this context.

    Return Value: Index of the driver.
    Remarks:
    Information about the driver can be retrieved by the CKRenderManager::GetRenderDriverDescription.
    See Also: ChangeDriver,CKRenderManager::GetRenderDriverDescription.
    *****************************************************/
    virtual int GetDriverIndex() = 0;
    /*****************************************************
    Summary:Changes the current driver used by a context.

    Arguments:
        DriverIndex: Index of new the driver to use or -1 to force the reconstruction of the current context.
    Return Value:
        TRUE if successful.
    Remarks:
    + This method can not change the driver of render context currently
    in fullscreen mode.
    + The context keeps all its current settings.
    See Also: GetDriverIndex,CKRenderManager::GetRenderDriverDescription.
    *****************************************************/
    virtual CKBOOL ChangeDriver(int NewDriver) = 0;

    //-----------------------------------------------------------
    // Window acces and position

    /*************************************************
    Summary: Returns the handle of the window used to create the context.

    Return Value:
        The handle of the render context window.
    Remarks:
        + The returned handle is the same that was given to CKRenderManager::CreateContext.
    See also:CKRenderManager::CreateContext
    *************************************************/
    virtual WIN_HANDLE GetWindowHandle() = 0;
    /*****************************************************
    Summary:Converts screen coordinates to render context window coordinates

    Arguments:
        pt: A pointer to Vx2DVector structure to convert.
    See Also:GetWindowHandle
    *****************************************************/
    virtual void ScreenToClient(Vx2DVector *ioPoint) = 0;

    virtual void ClientToScreen(Vx2DVector *ioPoint) = 0;

    // Context Dimensions

    /*************************************************
    Summary: Sets the context dimensions inside the window.

    Return Value:
        CK_OK.
    Arguments:
        rect: Rectangle defining the desired context dimensions.
        Flags: See Resize method comments.
    Remarks:
    + Coordinates are relative to top-left corner of the window containing the rendering context.
    + This method directly use the Resize method.
    See also:GetWindowRect,Resize,GetWidth,GetHeight,GetViewRect
    *************************************************/
    virtual CKERROR SetWindowRect(VxRect &rect, CKDWORD Flags = 0) = 0;
    /*************************************************
    Summary: Returns the render context dimensions.

    Arguments:
        rect: Rectangle to be filled context dimensions
        ScreenRelative: See remarks.
    Remarks:
    If ScreenRelative is FALSE coordinates are relative to top-left corner of the window containing the rendering context
    otherwise they are screen relative.
    See also:SetWindowRect,Resize,,GetWidth,GetHeight,GetViewRect
    *************************************************/
    virtual void GetWindowRect(VxRect &rect, CKBOOL ScreenRelative = FALSE) = 0;
    /*************************************************
    Summary: Returns the height of the context.

    Return Value:
        Height in pixels of the render context.
    See also:GetWidth,GetWindowRect,Resize,GetViewRect,
    *************************************************/
    virtual int GetHeight() = 0;
    /*************************************************
    Summary: Returns the width of the context.

    Return Value:
        Width in pixels of the render context.
    See also:GetHeight,GetWindowRect,Resize,GetViewRect,
    *************************************************/
    virtual int GetWidth() = 0;
    /*************************************************
    Summary: Changes the size of the render context.

    Arguments:
        PosX  : Position of the left corner of the context in the window.
        PosY  : Position of the top corner of the context in the window.
        SizeX : Optionnal new width of the context (0 to use the current size of the window).
        SizeY : Optionnal new height of the context  (0 to use the current size of the window).
        Flags : See remarks.
    Return Value:
        CK_OK if successful	or an error code otherwise.
    Remarks:
    + This function is usually called by the user to warn that the rendercontext window has changed its size.
    If the user does not specifies the new size the render engine will take the size of the client rect of the rendercontext window.
    + If Flags contains VX_RESIZE_NOSIZE SizeX and SizeY arguments are ignored.
    + If Flags contains VX_RESIZE_NOMOVE PosX and PosY arguments are ignored.
    See also:GetWindowHandle,SetWindowRect,GetWindowRect,SetViewRect,GetViewRect
    *************************************************/
    virtual CKERROR Resize(int PosX = 0, int PosY = 0, int SizeX = 0, int SizeY = 0, CKDWORD Flags = 0) = 0;
    // Viewport Dimensions
    /*************************************************
    Summary: Sets the viewport dimensions.

    Arguments:
        rect: Viewport size and position inside the context.
    Remarks:
    If the current render flags (see GetCurrentRenderOptions) contains CK_RENDER_USECAMERARATIO
    the viewport size is automatically set by PrepareCameras method according to camera aspect ratio
    and current size of the context at the beginning of each rendering loop.
    See also:GetViewRect,SetWindowRect,SetCurrentRenderOptions,PrepareCameras
    *************************************************/
    virtual void SetViewRect(VxRect &rect) = 0;
    /*************************************************
    Summary: Returns the viewport dimensions.

    Arguments:
        rect: Viewport size and position inside the context.
    Remarks:
    If the current render flags (see GetCurrentRenderOptions) contains CK_RENDER_USECAMERARATIO
    the viewport size is automatically set by PrepareCameras method according to camera aspect ratio
    and current size of the context at the beginning of each rendering loop.
    See also:SetViewRect,SetWindowRect,SetCurrentRenderOptions,PrepareCameras
    *************************************************/
    virtual void GetViewRect(VxRect &rect) = 0;

    /*****************************************************
    Summary:Returns the current pixel format of the context.

    Arguments:
        Bpp: Optionnal integer to be filled with the number of color buffer bits per pixel.
        Zbpp: Optionnal integer to be filled with the number of depth buffer bits per pixel.
        StencilBpp: Optionnal integer to be filled with the number of stencil bits per pixel.
    Return Value: VX_PIXELFORMAT of the color buffer.
    See Also: VX_PIXELFORMAT
    *****************************************************/
    virtual VX_PIXELFORMAT GetPixelFormat(int *Bpp = NULL, int *Zbpp = NULL, int *StencilBpp = NULL) = 0;

    //-----------------------------------------------------------
    // RenderStates

    /****************************************************************
    Summary: Sets a rendering state value.

    Arguments:
        State: VXRENDERSTATETYPE to modify.
        Value: New value for the state.
    Remarks:
    + Render states do not need to be modified if you are using materials
    since they are automatically set according to the material properties
    each time CKMaterial::SetAsCurrent is called (or an object with a given material is drawn).
    + Rendering states include filtering,blending,lighting,fog modes and many others, see VXRENDERSTATETYPE
    for more details.
    See also:GetState,CKMaterial::SetAsCurrent,VXRENDERSTATETYPE
    ****************************************************************/
    virtual void SetState(VXRENDERSTATETYPE State, CKDWORD Value) = 0;
    /*****************************************************
    Summary: Returns a rendering state value.

    Arguments:
        State: VXRENDERSTATETYPE to get the value of.
    Return Value:
        Current value of the given render state.
    Remarks:
        + Rendering states include filtering,blending,lighting,fog modes and many others, see VXRENDERSTATETYPE
        for more details.
    See also:SetState,CKMaterial::SetAsCurrent,VXRENDERSTATETYPE
    *****************************************************/
    virtual CKDWORD GetState(VXRENDERSTATETYPE State) = 0;
    /*************************************************************
    Summary: Specifies the current texture to use when drawing primitives.

    Return Value:
        TRUE if successful.
    Arguments:
        tex: A pointer to a CKTexture that will be used (a NULL texture disables the texturing).
        Clamped: Some implementation require additionnal processing when texture address mode is set to clamping. This value must be set to true if the texture is to be used in clamping mode.
        Stage: Texture stage on which this texture should be set, usually 0.
    Remarks:
    + When setting a texture as current the render engine will use it when drawing primitives.
    + If the texture is transparent alpha testing is enabled (SetState(VXRENDERSTATE_ALPHATESTENABLE,TRUE)
    + Setting a material as current automatically sets its texture as current.

    See also:DrawPrimitive,SetTextureStageState,SetState,SetCurrentMaterial,CKTexture::SetAsCurrent,CKMaterial::SetAsCurrent
    **************************************************************/
    virtual CKBOOL SetTexture(CKTexture *tex, CKBOOL Clamped = 0, int Stage = 0) = 0;
    /*****************************************************
    Summary:Sets the rendering state for the current texture.

    Return Value:
        TRUE if successful.
    Arguments:
        State: CKRST_TEXTURESTAGESTATETYPE to modify the value of.
        Value: New value.
        Stage: Index of the texture stage.
    Remarks:
    + The texture render states defines the texture filtering,blending or address modes
    for the currently active texture, so this method must be called after a call to CKTexture::SetAsCurrent
    or CKMaterial::SetAsCurrent.
    + These states are automatically set when setting a material as current.
    See Also:SetTexture,SetState,CKRST_TEXTURESTAGESTATETYPE,CKMaterial::SetAsCurrent
    *****************************************************/
    virtual CKBOOL SetTextureStageState(CKRST_TEXTURESTAGESTATETYPE State, CKDWORD Value, int Stage = 0) = 0;

    /*****************************************************
    Summary:Gets the rasterization context.

    Return Value: A pointer to a CKRasterizerContext.
    Remarks:
    + This method is specific to the Virtools implementation of
    the render engine. It gives acces to the low-level rasterizer
    object.
    + To use this object include the header files in CKRasterizerLib project
    given in the render engine source code.
    See Also:Available Source Code
    *****************************************************/
    virtual CKRasterizerContext *GetRasterizerContext() = 0;

    /*************************************************
    Summary: Specifies whether backbuffer should be cleared.

    Arguments:
        ClearBack : A boolean specifying if the back buffer should be cleared.
    Remarks:
    This function modifies the current render options and is a
    conveniency for ChangeCurrentRenderOptions(CK_RENDER_CLEARBACK)
    See also:SetClearZBuffer,ChangeCurrentRenderOptions
    *************************************************/
    virtual void SetClearBackground(CKBOOL ClearBack = TRUE) = 0;

    virtual CKBOOL GetClearBackground() = 0;
    /*************************************************
    Summary: Specifies whether backbuffer should be cleared.

    Arguments:
        ClearBack : A boolean specifying if the back buffer should be cleared.
    Remarks:
    + This function modifies the current render options and is a
    conveniency for ChangeCurrentRenderOptions(CK_RENDER_CLEARBACK)
    See also:SetClearZBuffer,ChangeCurrentRenderOptions
    *************************************************/
    virtual void SetClearZBuffer(CKBOOL ClearZ = TRUE) = 0;

    virtual CKBOOL GetClearZBuffer() = 0;

    virtual void GetGlobalRenderMode(VxShadeType *Shading, CKBOOL *Texture, CKBOOL *Wireframe) = 0;
    /*************************************************
    Summary: Forces render settings for all objects.

    Arguments:
        Shading: A global shade mode that all objects will use.
        Texture: Enable or disables texturing for all objects.
        Wireframe: Draws an additionnal wireframe layer on all the objects.
    Remarks:
    This method can be used to override the material settings of every objects.
    See also:VxShadeType
    *************************************************/
    virtual void SetGlobalRenderMode(VxShadeType Shading = GouraudShading, CKBOOL Texture = TRUE, CKBOOL Wireframe = FALSE) = 0;

    virtual void SetCurrentRenderOptions(CKDWORD flags) = 0;

    virtual CKDWORD GetCurrentRenderOptions() = 0;
    /*************************************************
    Summary: Sets the current rendering options.

    Arguments:
        flags: A combination of CK_RENDER_FLAGS.
    Remarks:
    The default render options are:

    + Draw both foreground and background sprites (CK_RENDER_BACKGROUNDSPRITES & CK_RENDER_FOREGROUNDSPRITES)
    + To automatically adjust the viewport rectangle according to the current camera aspect ratio (CK_RENDER_USECAMERARATIO).
    + To clear Color,Z and stencil buffer at the beginning of each render loop (CK_RENDER_CLEARZ,CK_RENDER_CLEARBACK,CK_RENDER_CLEARSTENCIL)
    + To blit the content of the back buffer to the screen at the end (CK_RENDER_DOBACKTOFRONT)
    This settings can be changed or overriden when calling the Render,Clear,DrawScene or BackToFront method.
    See also:ChangeCurrentRenderOptions,GetCurrentRenderOptions,CK_RENDER_FLAGS
    *************************************************/
    virtual void ChangeCurrentRenderOptions(CKDWORD Add, CKDWORD Remove) = 0;

    virtual void SetCurrentExtents(VxRect &extents) = 0;

    virtual void GetCurrentExtents(VxRect &extents) = 0;

    //-----------------------------------------------------------
    // Ambient Light

    /*************************************************
    Summary: Sets or gets the ambient light parameters

    Arguments:
        R: Red component of ambient light (0..1)
        G: Green component of ambient light (0..1)
        B: Blue component of ambient light (0..1)
        Color: A packed ARGB dword containing the ambient light.
        Value: The new ambient light stored in ARGB format
    Remarks:
        + The current ambient light stored in a ARGB CKDWORD
        + The ambient light settings is set at the beginning of each render loop
        to change it at runtime during a rendering use SetState(VXRENDERSTATE_AMBIENT,Value);
    See also:
    *************************************************/
    virtual void SetAmbientLight(float R, float G, float B) = 0;
    virtual void SetAmbientLight(CKDWORD Color) = 0;

    virtual CKDWORD GetAmbientLight() = 0;

    //-----------------------------------------------------------
    //	Fog Access

    /*************************************************
    Summary: Sets or gets the fog mode.

    Arguments:
        Mode : A VXFOG_MODE value to enable or disable fog.
    Return Value:
        Current fog mode.
    Remarks:
    + The current implementation forces the fog to be in linear mode.
    + The fog render states are set at the beginning of each render loop
    according to the value given by these methods.
    + To modify the fog parameters at runtime during the rendering loop
    you must use the appropriate render states (VXRENDERSTATE_FOGENABLE,VXRENDERSTATE_FOGCOLOR,etc...)
    See also:SetFogStart,SetFogEnd,SetFogColor
    *************************************************/
    virtual void SetFogMode(VXFOG_MODE Mode) = 0;
    /*************************************************
    Summary: Sets or gets the depth at which the fog starts.

    Arguments:
        Start : Distance at which fog should start.
    Remarks:
    + The start and end distance are only used in linear fog mode.
    + The current implementation forces the fog to be in linear mode.
    + The fog render states are set at the beginning of each render loop
    according to the value given by these methods.
    + To modify the fog parameters at runtime during the rendering loop
    you must use the appropriate render states (VXRENDERSTATE_FOGENABLE,VXRENDERSTATE_FOGCOLOR,etc...)
    See also:SetFogMode,SetFogEnd,SetFogColor
    *************************************************/
    virtual void SetFogStart(float Start) = 0;
    /*************************************************
    Summary: Sets or gets the depth at which the fog is at its maximum.

    Arguments:
        End : Distance at which fog should be maximum.
    Remarks:
        + The start and end distance are only used in linear fog mode.
        + The current implementation forces the fog to be in linear mode.
        + The fog render states are set at the beginning of each render loop
        according to the value given by these methods.
        + To modify the fog parameters at runtime during the rendering loop
        you must use the appropriate render states (VXRENDERSTATE_FOGENABLE,VXRENDERSTATE_FOGCOLOR,etc...)
    See also:SetFogMode,SetFogStart,SetFogColor
    *************************************************/
    virtual void SetFogEnd(float End) = 0;
    /*************************************************
    Summary: Sets or gets the fog density

    Arguments:
        Density: Fog density (0..1)
    Return Value:
        Current fog density.
    Remarks:
    + The fog density is only used in exponential fog modes
    and is irrelevant otherwise.
    + The current implementation forces the fog to be in linear mode.
    + The fog render states are set at the beginning of each render loop
    according to the value given by these methods.
    + To modify the fog parameters at runtime during the rendering loop
    you must use the appropriate render states (VXRENDERSTATE_FOGENABLE,VXRENDERSTATE_FOGCOLOR,etc...)
    See also:SetFogColor,SetFogMode,SetFogStart,SetFogEnd
    *************************************************/
    virtual void SetFogDensity(float Density) = 0;
    /*************************************************
    Summary: Sets or gets the fog color

    Arguments:
        Color: CKDWORD ARGB fog color.
    Return Value:
        Current fog color.
    Remarks:
    + The current implementation forces the fog to be in linear mode.
    + The fog render states are set at the beginning of each render loop
    according to the value given by these methods.
    + To modify the fog parameters at runtime during the rendering loop
    you must use the appropriate render states (VXRENDERSTATE_FOGENABLE,VXRENDERSTATE_FOGCOLOR,etc...)
    See also:SetFogMode,SetFogStart,SetFogEnd
    *************************************************/
    virtual void SetFogColor(CKDWORD Color) = 0;

    virtual VXFOG_MODE GetFogMode() = 0;

    virtual float GetFogStart() = 0;

    virtual float GetFogEnd() = 0;

    virtual float GetFogDensity() = 0;

    virtual CKDWORD GetFogColor() = 0;

    //-----------------------------------------------------------
    // Primitives Drawing

    /**************************************************************
    Summary: Draws a list of primitives (point,line,triangle).

    Arguments:
        pType: Type of primitive to draw.
        indices: A pointer to a list of WORD that will be used as indices to vertices or NULL.
        indexcount: Number of indices
        data: A pointer to a VxDrawPrimitiveData structure that contains pointer to vertices to draw and transformation flags.
    Return Value:
        TRUE if successful.
    Remarks:
    + Instead of allocating memory for vertices each time primitives are to be drawn, you can use the GetDrawPrimitiveStructure and GetDrawPrimitiveIndices which return ready-to-use VxDrawPrimitiveData structure.
    See also:VXPRIMITIVETYPE,Custom Rendering,SetWorldTransformationMatrix,VxDrawPrimitiveData
    ************************************************************/
    virtual CKBOOL DrawPrimitive(VXPRIMITIVETYPE pType, CKWORD *indices, int indexcount, VxDrawPrimitiveData *data) = 0;
    /*****************************************************
    Summary:Sets the current world transformation matrix

    Arguments:
        M : A VxMatrix representation of the world transformation.
    Remarks:
    The world, view and projection matrices determine the way vertices are transformed respectively from
    local coordinate sytem to world, from world to camera and from camera to screen coordinates.

    When a scene is drawn the View (Camera) , Projection and World (Object) matrices are automatically set.

    If you need to use a custom matrix in a callback (for example a world identity matrix to draw primitives directly in world coordinates)
    you should take care of restoring the matrix afterwards for the other objects to be rendered correctly.

    This is especially true for view and projection matrices which are set once at the beginning of the render loop.

    See also: SetProjectionTransformationMatrix,SetViewTransformationMatrix
    *****************************************************/
    virtual void SetWorldTransformationMatrix(const VxMatrix &M) = 0;
    /*****************************************************
    Summary:Sets the current projection transformation matrix

    Arguments:
        M : A VxMatrix representation of the projection transformation.
    Remarks:
        + The world, view and projection matrices determine the way vertices are transformed
        respectively from local coordinate sytem to world, from world to camera and from
        camera to screen coordinates.
        + When a scene is drawn the View (Camera) , Projection and
        World (Object) matrices are automatically set.
        + If you need to use a custom matrix in a callback (for example a world identity matrix
        to draw primitives directly in world coordinates) you should take care
        of restoring the matrix afterwards for the other objects to be rendered correctly.
        This is especially true for view and projection matrices which are set once at the
        beginning of the render loop.

        + Perspective Projection:

                    A=Cos(Fov/2)/Sin(Fov/2)
                    W=ViewWidth
                    H=ViewHeight
                    F	=	Far clip plane
                    N	=	Near clip plane

                            [ A			0			0			0]
                            [ 0			A*W/H		0			0]
                        M=	[ 0			0			F/F-N		1]
                            [ 0			0			-F.N/F-N	0]

        + Orthographic Projection:

                    Z=Orthographic Zoom
                    W=ViewWidth
                    H=ViewHeight
                    F	=	Far clip plane
                    N	=	Near clip plane

                            [ Z			0			0			0]
                            [ 0			Z*W/H		0			0]
                        M=	[ 0			0			1/F-N		0]
                            [ 0			0			-N/F-N		1]

    See also: SetProjectionTransformationMatrix,SetViewTransformationMatrix,VxMatrix::Perspective
    *****************************************************/
    virtual void SetProjectionTransformationMatrix(const VxMatrix &M) = 0;
    /*****************************************************
    Summary:Sets the current view transformation matrix

    Arguments:
        M : A VxMatrix representation of the view transformation.
    Remarks:
    + The world, view and projection matrices determine the way vertices are transformed
    respectively from local coordinate sytem to world, from world to camera and from
    camera to screen coordinates.
    + When a scene is drawn the View (Camera) , Projection and
    World (Object) matrices are automatically set.
    + If you need to use a custom matrix in a callback (for example a world identity matrix
    to draw primitives directly in world coordinates) you should take care
    of restoring the matrix afterwards for the other objects to be rendered correctly.
    This is especially true for view and projection matrices which are set once at the
    beginning of the render loop.
    See also: SetProjectionTransformationMatrix,SetViewTransformationMatrix
    *****************************************************/
    virtual void SetViewTransformationMatrix(const VxMatrix &M) = 0;

    virtual const VxMatrix &GetWorldTransformationMatrix() = 0;

    virtual const VxMatrix &GetProjectionTransformationMatrix() = 0;

    virtual const VxMatrix &GetViewTransformationMatrix() = 0;

    /*****************************************************
    Summary:Sets a user clip plane equation
    Return Value:
        TRUE if successful.
    Remarks:
    + The plane equation must be given in world coordinates.
    + A clip plane must be enabled with the render state VXRENDERSTATE_CLIPPLANEENABLE: to enable
    the clip plane 0(1),1(2) and 3(8) one must set SetState(VXRENDERSTATE_CLIPPLANEENABLE,0xB);
    + Not all drivers support user clip planes, you should check
    the MaxClipPlanes member of Vx3DCapsDesc before using them.

    See also:GetUserClipPlane,SetState,VXRENDERSTATE_CLIPPLANEENABLE
    *****************************************************/
    virtual CKBOOL SetUserClipPlane(CKDWORD ClipPlaneIndex, const VxPlane &PlaneEquation) = 0;
    /*****************************************************
    Summary:Returns a user clip plane equation
    Remarks:
    + The plane equation is given in world coordinates.

    Return Value:
        TRUE if successful,FALSE otherwise.
    See also:GetUserClipPlane,SetState,VXRENDERSTATE_CLIPPLANEENABLE
    *****************************************************/
    virtual CKBOOL GetUserClipPlane(CKDWORD ClipPlaneIndex, VxPlane &PlaneEquation) = 0;

    //-----------------------------------------------------------
    // Picking

    /*************************************************
    Summary: Returns information about object at a specific position.

    Arguments:
        x: X coordinate (in context window coordinates).
        y: Y coordinate (in context window coordinates).
        res: A pointer to a VxIntersectionDesc which will be filled with additional information (Face,Normal,Texture coordinates...)
    Return Value:
        A pointer to the CKRenderObject at specified position or NULL if no such object exists.
    Remarks:
        + Picking is done by calculating the intersection between a ray from viewpoint to screen position and all the objects in the scene.
        + It uses the information from the last rendering to determines the list of objects that should be tested.
    See also:RectPick,Pick2D
    *************************************************/
    virtual CKRenderObject *Pick(int x, int y, CKPICKRESULT *oRes, CKBOOL iIgnoreUnpickable = FALSE) = 0;
    virtual CKRenderObject *Pick(CKPOINT pt, CKPICKRESULT *oRes, CKBOOL iIgnoreUnpickable = FALSE) = 0;
    /*************************************************
    Summary: Returns the list of objects contained in a rectangle.

    Arguments:
        Intersect : Specifies whether objects must be entirely inside rectangle r to be listed.
        res: A pointer to a CKObjectArray that will be filled with the objects.
        r: A VxRect giving the coordinates of the picking rectangle (in context window coordinates).
    Return Value:
        CK_OK if successful or an error code otherwise.
    See also:Pick
    *************************************************/
    virtual CKERROR RectPick(const VxRect &r, XObjectPointerArray &oObjects, CKBOOL Intersect = TRUE) = 0;

    //-----------------------------------------------------------
    // Viewpoint and Cameras

    /*************************************************
    Summary: Sets the active camera.

    Arguments:
        cam : A pointer to the CKCamera giving viewpoint information.
    Remarks:
        + Once the viewpoint is attached to a camera, it takes its position and orientation every frame.
        + The render context takes the active camera aspect ratio into account to resize the viewport.
    See also:DetachViewpointFromCamera,GetViewpoint,GetAttachedCamera
    *************************************************/
    virtual void AttachViewpointToCamera(CKCamera *cam) = 0;
    /*************************************************
    Summary: Detachs the viewpoint from its camera.

    Remarks:
        + Once detached the viewpoint does not follow anymore the position and orientation of the camera it was attached to.
        + The field of view,near plane,far plane settings are those of the last camera attached.
    See also:AttachViewpointToCamera,GetViewpoint,GetAttachedCamera
    *************************************************/
    virtual void DetachViewpointFromCamera() = 0;
    /*************************************************
    Summary: Returns the camera to which the viewpoint is attached.

    Return Value:
        A pointer to the CKCamera to which the viewpoint is attached or NULL.
    Remarks:
        + Most of the time the viewpoint is attached to a camera to specify
        other settings than position and orientation such as Field of view,Clip planes,Aspect ratio,etc...
    See also:AttachViewpointToCamera,GetViewpoint
    *************************************************/
    virtual CKCamera *GetAttachedCamera() = 0;
    /*************************************************
    Summary: Returns the 3dEntity which represents the viewpoint.

    Return Value:
        A pointer to the CK3dEntity which is used as viewpoint.
    Remarks:
    + The viewpoint is not a CKCamera. It is only a 3DEntity which position and orientation gives the current viewpoint position.
    + When the viewpoint is attached to a camera through AttachViewpointToCamera its position and orientation
    are set to the camera settings every frame.
    See also:GetAttachedCamera,AttachViewpointToCamera
    *************************************************/
    virtual CK3dEntity *GetViewpoint() = 0;

    //-----------------------------------------------------------
    // Misc

    /*************************************************
    Summary: Returns the material used to clear the background.

    Return Value:
        A pointer to the CKMaterial used to clear the background.
    Remarks:
        + When clearing the backbuffer, if the background material has a texture the backbuffer is filled
        with this texture otherwise the backbuffer is clear with the material diffuse color.
    See also:Clear
    *************************************************/
    virtual CKMaterial *GetBackgroundMaterial() = 0;
    /*************************************************
    Summary: Returns the bounding box of every objects referenced in the context.

    Arguments:
        box : A pointer to a VxBbox that will contain the extents of all the objects.
    Remarks:
        + This method computes the bounding box from the list of all the objects attached to the
        rendercontext. Computing this bounding box may be a slow operation especially if there is
        a great number of objects.
    See also:VxBbox,CK3dEntity::GetBoundingBox,CK3dEntity::GetHierarchicalBox
    *************************************************/
    virtual void GetBoundingBox(VxBbox *BBox) = 0;
    /*************************************************
    Summary: Returns statistics about the last rendered frame .

    Arguments:
        stats : A pointer to a VxStats structure to be filled.
    Remarks:
    + The number of Objects,faces,lines and vertices drawn during one frame
    can be retrieved though this method.
    + The VxStats structure also contains profiling results of the time spent
    during the last render loop.
    See also:VxStats
    *************************************************/
    virtual void GetStats(VxStats *stats) = 0;
    /*************************************************
    Summary: Specifies the current material to use when drawing primitives.

    Arguments:
        mat: A pointer to a CKMaterial that will be used (a NULL material disables the texturing).
        Lit: TRUE to set the color settings of the material or FALSE if they will not be used (when rendering prelitted primitives for example)
    Remarks:
    When setting a material as current the lighting engine will use its parameters (Diffuse Color ,etc.)
    for lighting calculation if Lit is TRUE. But this function also sets the render state according to the material properties:

    + If the material has a texture it is set as current texture.
    + If material is two-sided the culling is disabled : SetState(VXRENDERSTATE_CULLMODE,VXCULL_NONE) otherwise it is set to counter-clockwise SetState(VXRENDERSTATE_CULLMODE, VXCULL_CCW).
    + The Texture Blend Mode is set : SetState(VXRENDERSTATE_TEXTUREMAPBLEND,TextureBlendMode);
    + The Shade Mode is set : SetState(VXRENDERSTATE_SHADEMODE,ShadeMode) unless the render context has a global shade mode in which case this is the one used.
    + The Filtering Mode is set :SetState(VXRENDERSTATE_TEXTUREMAG,TextureMagMode) and SetState(VXRENDERSTATE_TEXTUREMIN,TextureMinMode)
    + Texture Border Color and Address Mode (Wrap,Clamp,etc.) are set : Dev->SetState(VXRENDERSTATE_BORDERCOLOR,BorderColor ) and
    Dev->SetState(VXRENDERSTATE_TEXTUREADDRESS,TextureAddressMode )
    + If Blending is enabled the blending is enabled through SetState(VXRENDERSTATE_BLENDENABLE, ...)
    Z buffer writing is disabled according to hte current flag SetState(VXRENDERSTATE_ZWRITEENABLE, ...)
    and the Blending Factors are set SetState(VXRENDERSTATE_SRCBLEND, SrcBlendMode) and SetState(VXRENDERSTATE_DESTBLEND, DestBlendMode).
    + Alpha testing is set,SetState(VXRENDERSTATE_ALPHATESTENABLE,AlphaTestEnabled),SetState(VXRENDERSTATE_ALPHAFUNC, AlphaFunc),SetState(VXRENDERSTATE_ALPHAREF, AlphaRef);
    See also:DrawPrimitive,SetState,SetTexture,CKTexture::SetAsCurrent,CKMaterial::SetAsCurrent
    *************************************************/
    virtual void SetCurrentMaterial(CKMaterial *mat, BOOL Lit = TRUE) = 0;

    virtual void Activate(CKBOOL active = TRUE) = 0;

    //-----------------------------------------------------------
    // Buffers Acces

    /*************************************************
    Summary: Creates an image from the specified video buffer (Back or Z ).

    Arguments:
        rect: Rectangle in the buffer from which image should be copied.
        desc: A reference to a VxImageDescEx giving the format in which image data will be stored.
        buffer: Buffer from which copy should be done can be either VXBUFFER_BACKBUFFER or VXBUFFER_ZBUFFER
    Return Value:
        The size of the resultant buffer.
    Remarks:
        First call this function with a desc.Image set to NULL to get the size of the buffer. You can then allocate
        your data buffer and call the function again with this parameter.
    Example:
            VxImageDescEx ImgDesc;
            BYTE* ImgBuffer = NULL;

            int ImageSize = rdctx->DumpToMemory(NULL,VXBUFFER_BACKBUFFER,ImgDesc);
            if (ImageSize) {
                ImgBuffer = new BYTE[ImageSize];
                ImgDesc.Image = ImgBuffer;

                rdctx->DumpToMemory(NULL,VXBUFFER_BACKBUFFER,ImgDesc);
            }

            // ImgDesc now contain the description (size and pixel format) of the image
            // and ImgDesc.Image contains the image pixels...

    See also:CopyToVideo,DumpToFile
    *************************************************/
    virtual int DumpToMemory(const VxRect *iRect, VXBUFFER_TYPE buffer, VxImageDescEx &desc) = 0;

    /*************************************************
    Summary: Copies an image to the specified video buffer (Back or Z )

    Arguments:
        rect: rectangle in the buffer to which image should be copied.
        desc: A pointer to a VxImageDescEx giving the format in which image data was stored.Must not be NULL.
        buffer: buffer to which copy should be done can be either VXBUFFER_BACKBUFFER or VXBUFFER_ZBUFFER
    Return Value:
        The size of the resultant buffer or 0 if an error occured.
    Remarks:
    + The desc parameter must contain information about image format. If the image stored in data does not
    have the same format than the video buffer the function will fail and return 0.
    + In the same manner the size of the source image (as given in desc) must be the same that the destination rectangle.
    See also:CopyFromBuffer,DumpTo
    *************************************************/
    virtual int CopyToVideo(const VxRect *iRect, VXBUFFER_TYPE buffer, VxImageDescEx &desc) = 0;

    /*************************************************
    Summary: Saves the content of a video-buffer to the disk.

    Arguments:
        filename: Filename of the bitmap file to save.
        rect: A pointer to a VxRect specifiyng which part of the buffer should be saved. A NULL parameter takes the whole buffer.
        buffer: Video buffer to save (can be back or depth buffer).
    Return Value:
        CK_OK if successful or an error code otherwise.
    Remarks:
    + DumpToFile locks the specified buffer and writes its data into a bitmap file.
    + The file format can be of any type supported by Virtools but the extension must
    be given.
    See also:DumpToMemory
    *************************************************/
    virtual CKERROR DumpToFile(CKSTRING filename, const VxRect *rect, VXBUFFER_TYPE buffer) = 0;

    //--- Specific to D3D engine :

    /*************************************************
    Summary: Returns pointers to the DirectX objects.

    Return Value:
        A pointer to a VxDirectXData that contain the pointers to the DirectDraw and Direct3D objects.
    Remarks:
        + When using a DirectX based rasterizers, one can access the DirectDraw and Direct3D objects through this method.
    See also:VxDirectXData
    *************************************************/
    virtual VxDirectXData *GetDirectXInfo() = 0;

    //--- Thread specific functions
    /*****************************************************
    Summary: Multi thread

    Remarks:
        + Some rasterizers (OpenGL) functions will failed
        if called from 2 differents threads without changing
        the active thread before calling them.
        + In a multi-thread application where more than
        one thread may access a render context you need to
        calls these methods before and after any other methods if you are not in
        the thread that created the render context.
    *****************************************************/
    virtual void WarnEnterThread() = 0;

    virtual void WarnExitThread() = 0;

    /*************************************************
    Summary: Returns information about 2D entities at a specific position.

    Arguments:
        p: A Vx2DVector containing the position in context window coordinates.
    Return Value:
        A pointer to the CK3dEntity at specified position or NULL if no such object exists.
    Remarks:
        + This method only test the 2D Entities (sprites,..) and returns the 2D Entity
        under the given position with the highest Z-order.
    See also:RectPick,Pick
    *************************************************/
    virtual CK2dEntity *Pick2D(const Vx2DVector &v) = 0;
    /*****************************************************
    Summary:Sets a texture as target for the rendering

    Arguments:
        texture: A CKTexture on which rendering should be targeted.
        CubeMapFace: For a cube map texture index of the face on which the rendering should occur.
    Remarks:

    + Depending on the implementation, when a texture is set as the target for rendering, it either occurs directly in the texture video memory surface or is copied at the end of the frame from the back buffer to the texture video surface (which is a little bit slower)
    + The texture system memory image is left unchanged and does not reflect any more what is in video memory.
    + When dealing with cube map, a face index (0 : positive X axis, 1 negative X axis,2 : position Y axis, 3 negative Y axis, etc...) must be given to indicate which face to render to)

    Return Value:
        TRUE if successful,FALSE otherwise.
    See Also:CKTexture
    *****************************************************/
    virtual CKBOOL SetRenderTarget(CKTexture *texture = NULL, int CubeMapFace = 0) = 0;

    /*****************************************************
    Summary: Warns the engine that several objects are being added or removed.

    Arguments:
        Start: TRUE to warn the start of the sequence, FALSE to end it.
    Remarks:

    + When an object is added to the render context it is inserted the scene graph. This operation can be slow if it must be repeated for each object.
    + This method enables the user to warn the engine that several call to AddObjects or RemoveObjects are about to occur so that it can update the scene graph only once when all the objects have been added or removed.
    See Also:AddObject,RemoveObject
    *****************************************************/
    virtual void AddRemoveSequence(CKBOOL Start) = 0;

    virtual void SetTransparentMode(CKBOOL Trans) = 0;

    virtual void AddDirtyRect(CKRECT *Rect) = 0;

    virtual void RestoreScreenBackup() = 0;

    CKRenderContext(CKContext *Context, CKSTRING name = NULL);

    virtual ~CKRenderContext();

    virtual CKDWORD GetStencilFreeMask() = 0;

    virtual void UsedStencilBits(CKDWORD stencilBits) = 0;

    virtual int GetFirstFreeStencilBits() = 0;

    /******************************************************
    Summary: Gets write access to the latest allocated vertex buffer

    Return Value:

    Remarks:
    + After a successful call to GetDrawPrimitiveStructure with CKRST_DP_VBUFFER flag set the returned pointers point to a vertex buffer which must be released with ReleaseCurrentVB before any rendering can occur.
    + This method returns a pointer to a VxDrawPrimitiveData structure ( pointing to a vertex buffer) that can be used to update vertex data.
    + ReleaseCurrentVB must be called once the data have been written.

    A typical usage of this mehod is for users who want to render the same geometry twice
    and change only a subset of data between the two calls to draw primitive. Such a technique would look like

    {html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

                DataPtr = GetDrawPrimitiveStructure(Flags | CKRST_DP_VBUFFER...)

                //.. Fill vertex data
                //...............

                DrawPrimitive(DataPtr);

                if (DataPtr->m_Flags & CKRST_DP_VBUFFER) {
                    //.. Returned pointer was a vertex buffer we must lock it before writting again
                    LockCurrentVB()
                }

                //.. Fill vertex data
                //......

                if (DataPtr->m_Flags & CKRST_DP_VBUFFER) {
                    //.. Returned pointer was effectively a vertex buffer we must release it
                    ReleaseCurrentVB()
                }

                DrawPrimitive(DataPtr);

    {html:</td></tr></table>}

    Return Value:
        A pointer to a VxDrawPrimitiveData containing the vertex buffer data.

    See also:Custom Rendering,GetDrawPrimitiveStructure,VxDrawPrimitiveData,DrawPrimitive,CKVertexBuffer
    *************************************************/
    virtual VxDrawPrimitiveData *LockCurrentVB(CKDWORD VertexCount) = 0;

    virtual BOOL ReleaseCurrentVB() = 0;

    /*****************************************************
    Summary:Sets the current texture transformation matrix

    Arguments:
        M : A VxMatrix representation of the texture transformation.
        Stage: Index of the texture stage (0..7) which matrix should be set.
    Remarks:
    See also: SetProjectionTransformationMatrix,SetViewTransformationMatrix,SetWorldTransformationMatrix
    *****************************************************/
    virtual void SetTextureMatrix(const VxMatrix &M, int Stage = 0) = 0;

    /*****************************************************
    Summary:Sets the stereo parameters

    Arguments:
        EyeSeparation : Distance between eyes in world referential.
        FocalLength: Focal Distance.
    Remarks:
    OpenGL devices that have hardware support for stereo rendering are
    detected has potential driver and be used as render context driver.
    Two rendering are then performed each time CKRenderContext::Render is
    called using asymmetric frustums given the eye separation and focal length.
    *******************************************************/
    virtual void SetStereoParameters(float EyeSeparation, float FocalLength) = 0;

    virtual void GetStereoParameters(float &EyeSeparation, float &FocalLength) = 0;

    /*****************************************************
    Summary:Sets the viewport to the window

    Arguments:
        iRect : Rectangle of the viewport to set.
    Remarks:
    See also: SetViewRect
    *****************************************************/
    virtual void SetViewport(const VxRect &iRect) = 0;

    /*****************************************************
    Summary:Calculate a 3D point from a 2D Point, located on the
    near plane of the camera.

    Arguments:
        i2DPosition: Position in the screen coordinates (or in the viewport).
        o3DPosition: Position in the viewpoint system coordinates.
        iScreen: TRUE if the position is given in the screen coordinates,
        FALSE if it is given in client coordinates.
    Remarks:
    See also: SetViewRect
    *****************************************************/
    virtual void ScreenToViewpoint(const Vx2DVector &i2DPosition, VxVector *o3DPosition, CKBOOL iScreen = TRUE) = 0;
};

#endif
