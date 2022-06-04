/*************************************************************************/
/*	File : CKRasterizer.h												 */
/*	Author : Romain Sididris											 */
/*																		 */
/*	+ Base classes declaration for rasterizers							 */
/*	The default implementation of the render engine in Virtools use		 */
/*	rasterizers as a common interface to acces a rendering device.		 */
/*																		 */
/*	+ A Rasterizer DLL must provide the implemenation for the 3 base 	 */
/*	classes by overloading them:										 */
/*		- CKRasterizer: Top level class (One instance per rasterizer)    */
/*		which upon creation should check for availables drivers.		 */
/*																		 */
/*		- CKRasterizerDriver: One instance should be created for each	 */
/*		available driver (ie graphic card,software,hardware,etc.). It is */
/*		used to store capabilities and create rendering contextes		 */
/*																		 */
/*		- CKRasterizerContext: A rendering context with all the methods	 */
/*		to create/load textures, setup lights,materials,render states	 */
/*		and draw primitives.											 */
/*		 Many of these methods are similar to DirectX structures		 */
/*																		 */
/*																		 */
/*																		 */
/*	+ Some methods of these classes are already implemented in the 		 */
/*	CKRasterizerLib	library	as they are common to all rasterizers		 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKRASTERIZER_H
#define CKRASTERIZER_H "$Id:$"

#include "VxDefines.h"
#include "XString.h"
#include "XArray.h"
#include "CKRasterizerEnums.h"
#include "CKRasterizerTypes.h"

/**********************************************************
 The render engine will call the CKRasterizerGetInfo function (see below)
 to gain acces to rasterizer information. This information should be
 stored in CKRasterizerInfo structure.
***********************************************************/
struct CKRasterizerInfo
{
    XString DllName;			  // Filled by the render engine when parsing Dlls
    XString Desc;				  // Description for this rasterizer (eg: "Open GL Rasterizer")
    INSTANCE_HANDLE DllInstance;  // Filled by the render engine when loading the DLL
    CKRST_STARTFUNCTION StartFct; // A pointer to a function that will create the CKRasterizer Instance
    CKRST_CLOSEFUNCTION CloseFct; // A pointer to a function that will destroy the CKRasterizer Instance
    CKRST_OPTIONFCT OptionFct;
    CKRasterizerInfo()
    {
        DllInstance = NULL;
        StartFct = NULL;
        CloseFct = NULL;
        OptionFct = NULL;
    }
};

/*******************************************
+ There is only one function a rasterizer Dll is supposed
to export :"CKRasterizerGetInfo", it will be used by the render engine
to retrieve information about the plugin :
    - Description
******************************************/
typedef void (*CKRST_GETINFO)(CKRasterizerInfo *);

// secret
#ifdef _XBOX
#define MAX_FRAMES_AHEAD 3
#else
#define MAX_FRAMES_AHEAD 3
#endif

/****************************************************************
Main class for rasterizer declaration

  + A render engine is started by calling CKRasterizerStart which will try
  to create a CKRasterizer object and to initialize it.

  + It will enumerate and create  the available drivers on the system.

  + If no driver for 3D rendering is available the function should fail and return NULL.

  + The CKRasterizer object is only used to
        - generate texture,sprites and vertex buffer index
        ( which are simple CKDWORDs to identify a texture or VB across differents contexts or drivers ).
        - acces the list of available driver

  + Several rasterizers can work together in which case texture indices should be the same across
  them. Each rasterizer must warn the others of its existence through  LinkRasterizer
  (It is automatically called by the render engine upon registration of the rasterizers)
****************************************************************/
class CKRasterizer
{
public:
    CKRasterizer();
    virtual ~CKRasterizer();

    //---- Initialisation/Destruction
    virtual BOOL Start(WIN_HANDLE AppWnd);
    virtual void Close(void) {}

    //--- Available drivers access
    virtual int GetDriverCount() { return m_Drivers.Size(); }
    virtual CKRasterizerDriver *GetDriver(CKDWORD index) { return m_Drivers[index]; }

    //--- Texture,Sprite,Vertex buffer Index Creation (Implemented by Lib)
    //--- (Index are shared amongst all contexts
    virtual CKDWORD CreateObjectIndex(CKRST_OBJECTTYPE Type, BOOL WarnOthers = TRUE);
    virtual BOOL ReleaseObjectIndex(CKDWORD ObjectIndex, CKRST_OBJECTTYPE Type, BOOL WarnOthers = TRUE);

    //--- Implemented by lib : keep a track of other rasterizers
    void LinkRasterizer(CKRasterizer *rst);
    void RemoveLinkedRasterizer(CKRasterizer *rst);

    //--- A Mutex so that rasterizers can synchronize on critical data access
    //--- or NULL is no synchroniztion is needed
    void SetMutex(VxMutex *mutex) { m_Mutex = mutex; }

    //--- A pointer to a function that will get called each time a
    // event is launched by a rasterizer context....
    void SetEventWatcher(CKRST_EVENTFUNCTION fct, void *argument)
    {
        m_EventWatcher = fct;
        m_EventArgument = argument;
    }

    //----- Buggy driver information
    BOOL LoadVideoCardFile(const char *FileName);
    CKDriverProblems *FindDriverProblems(const XString &Vendor, const XString &Renderer, const XString &Version, const XString &DeviceDesc, int Bpp);

public:
    CKRasterizerContext *m_FullscreenContext; // If a context is currently fullscreen...
    XSArray<BYTE> m_ObjectsIndex;			  // A List of byte mask to keep track of the existing object
    CKDWORD m_FirstFreeIndex[8];			  // First Free Index for each type of objects...

    // indexes (textures,sprites,VBs)
    XSArray<CKRasterizer *> m_OtherRasterizers; // The list of other rasterizers..
    WIN_HANDLE m_MainWindow;					// Application main window used to preform initialisations

    XClassArray<CKDriverProblems> m_ProblematicDrivers; // List of driver with identified problems
    XArray<CKRasterizerDriver *> m_Drivers;
    VxMutex *m_Mutex;
    CKRST_EVENTFUNCTION m_EventWatcher;
    void *m_EventArgument;
    // Implementation specific data to follow....
};

/****************************************************************
+ Once a rasterizer is created and started it enumerates all availables drivers
 depending on the number of graphic adapters installed and their
 different implementation (Hardware,software,Hardware Transform & Lighting,etc...)

+ Once a driver is chosen (according to its capabilities for example) it can be use
 to create one more contexts for drawing (see CreateContext and CKRasterizerContext)
****************************************************************/
class CKRasterizerDriver
{
public:
    CKRasterizerDriver(CKRasterizer *Owner);
    virtual ~CKRasterizerDriver();

    //--- Contexts creation
    virtual CKRasterizerContext *CreateContext();
    virtual BOOL DestroyContext(CKRasterizerContext *Context);

public:
    void InitNULLRasterizerCaps();

    BOOL m_Hardware;							 // Hardware accelerated driver ?
    BOOL m_CapsUpToDate;						 // Driver Capabilities are up to date ?
    CKRasterizer *m_Owner;						 // Owner CKRasterizer object
    CKDWORD m_DriverIndex;						 // Index of this driver in the Rasterizer List of drivers
    XArray<VxDisplayMode> m_DisplayModes;		 // List of availables display modes
    XClassArray<CKTextureDesc> m_TextureFormats; // List of availables texture formats
    Vx3DCapsDesc m_3DCaps;						 // 3D capabilities
    Vx2DCapsDesc m_2DCaps;						 // 2D capabilities
    XString m_Desc;								 // Description string
    XArray<CKRasterizerContext *> m_Contexts;	 // Currently created render contextes
    BOOL m_Stereo;								 // Support steroscopic  ?
};

/***************************************************
 Utility : Convert the attenuation factors from  DX5 model
    A(d) = a0+ a1.(1-d/R) + a2.(1-d/R)�
 to DX7 or GL attenuation model
     A(d) = 1/(a0 + a1.d + a2.d�)
( The result is not exactly the same :(
*******************************************************/
void ConvertAttenuationModelFromDX5(float &_a0, float &_a1, float &_a2, float range);

/***************************************************
 Utility : From a given DrawprimitiveDataStructure :
 returns the corresponding Vertex Buffer Format (CKRST_VERTEXFORMAT) and the
 size of a vertex...
*******************************************************/
CKDWORD CKRSTGetVertexFormat(CKRST_DPFLAGS DpFlags, CKDWORD &VertexSize);

/***************************************************
 Utility : From a given CKRST_VERTEXFORMAT format :
 returns the corresponding size of a vertex...
*******************************************************/
CKDWORD CKRSTGetVertexSize(CKDWORD VertexFormat);

/**************************************************
Copy the content of a DrawPrimitive Data structure inside
a Vertex Buffer memory buffer ( must have been obtained by a call to Lock)
No Assumption or tests are made to check the coherence between the vertex format
and the incoming data
returns the new location of the current VBuffer memory pointer
To use in coordination with LockVertexBuffer
*****************************************************/
BYTE *CKRSTLoadVertexBuffer(BYTE *VBMem, CKDWORD VFormat, CKDWORD VSize, VxDrawPrimitiveData *data);

/**************************************************
Copy the content of a DrawPrimitive Data structure inside
another using the destination structure vertex count and
flags to take decision...
To use in coordination with LockVertexBuffer2
*****************************************************/
void CKRSTLoadVertexBuffer2(VxDrawPrimitiveData *Dest, VxDrawPrimitiveData *Src);

/**************************************************
Setup the DpData structure (data pointers and stride only) according
the DpData.Flags and VBMem pointer...
*****************************************************/
void CKRSTSetupDPFromVertexBuffer(BYTE *VBMem, CKVertexBufferDesc *VB, VxDrawPrimitiveData &DpData);

/**************************************************
When using CKRSTLoadVertexBuffer2 or CKRSTLoadVertexBuffer
vertex can be scaled by a given factor..
*****************************************************/
void SetRSTVertexScaling(const VxVector4 &scale);
void SetRSTVertexOffset(const VxVector4 &scale);

void SetRSTVertexUVScaling(int stage, const Vx2DVector &scale);

/****************************************************************
+ A context is used to identify where the rendering take place and
to specify how primitives should be drawn.

+ A context is created by its driver, either fullscreen or windowed.

+ A context is also used to create textures,sprites or vertex buffer
  and to load their content. When accessing to textures, sprites or vertex buffer
  one must use an index identifying the object, this object must have been
  created previously by CKRasterizer::CreateObjectIndex() (CKRST_OBJ_TEXTURE,CKRST_OBJ_SPRITE or
  CKRST_OBJ_VERTEXBUFFER). The object index is shared across all rasterizer object
  so there is no need to recreate it when destroying/creating contextes

+ Several methods are here to set the diferent parameters of the render
  engine : Lighting states( Material and Light), Viewport States,
  Transformation states, Render States and Texture Stage States
****************************************************************/
class CKRasterizerContext
{
public:
    //--- Construction/destruction
    CKRasterizerContext(CKRasterizerDriver *Driver);
    virtual ~CKRasterizerContext();

    //--- Creation (destruction is done by deleting the context CKRasterizerDriver::DestroyContext)
    virtual BOOL Create(WIN_HANDLE Window, int PosX = 0, int PosY = 0, int Width = 0, int Height = 0, int Bpp = -1, BOOL Fullscreen = 0, int RefreshRate = 0, int Zbpp = -1, int StencilBpp = -1) { return TRUE; }

    //---
    virtual BOOL Resize(int PosX = 0, int PosY = 0, int Width = 0, int Height = 0, CKDWORD Flags = 0) { return TRUE; }
    virtual BOOL Clear(CKDWORD Flags = CKRST_CTXCLEAR_ALL, CKDWORD Ccol = 0, float Z = 1.0f, CKDWORD Stencil = 0, int RectCount = 0, CKRECT *rects = NULL) { return FALSE; }
    virtual BOOL BackToFront() { return FALSE; }

    //------------------------------------------------------
    //--- Starting/stopping primitive drawing
    virtual BOOL BeginScene();
    virtual BOOL EndScene();

    //----------------------------------------------------
    //--- Lighting & Material States
    virtual BOOL SetLight(CKDWORD LightIndex, CKLightData *data)
    {
        if (data && LightIndex < RST_MAX_LIGHT)
            m_CurrentLightData[LightIndex] = *data;
        return FALSE;
    }
    virtual BOOL EnableLight(CKDWORD LightIndex, BOOL Enable) { return FALSE; }
    virtual BOOL SetMaterial(CKMaterialData *mat);

    //-----------------------------------------------------
    //--- Viewport size and position
    virtual BOOL SetViewport(CKViewportData *data);

    //--- Transformaion Matrix (World,View or projection )
    virtual BOOL SetTransformMatrix(VXMATRIX_TYPE Type, const VxMatrix &Mat); // (default Implementation by Lib)

    //----------------------------------------------------------
    //--- Rendering  states
    //	To avoid redondant render state calls,a render state cache can be used by
    //	implementation of CKRasterizerContext by first calling CKRasterizerContext::InternalSetRenderState() before
    //	actually setting the render state (implmented in .h file to be inlined in implementations)
    virtual BOOL SetRenderState(VXRENDERSTATETYPE State, CKDWORD Value) { return InternalSetRenderState(State, Value); }
    virtual BOOL GetRenderState(VXRENDERSTATETYPE State, CKDWORD *Value) { return InternalGetRenderState(State, Value); }

    //-------------------------------------------------------------
    //--- Texture States control the texture and its aspects (filtering, etc.)
    //--- Set the current texture to be used for rendering (0 if no texturing)
    virtual BOOL SetTexture(CKDWORD Texture, int Stage = 0) { return FALSE; }
    virtual BOOL SetTextureStageState(int Stage, CKRST_TEXTURESTAGESTATETYPE Tss, CKDWORD Value)
    {
        return InternalSetTextureState(Stage, Tss, Value);
    }

    // This resets all the currently set texture stages
    virtual void DisableAllTextureStages(); // (Implementated by Lib)

    //-------------------------------------------------------------
    //--- Vertex & pixel shaders are created as textures and sprites
    //--- calling these functions with 0 as argument disables the
    //--- usage of programmable shaders and uses the default pipeline
    virtual BOOL SetVertexShader(CKDWORD VShaderIndex) { return FALSE; }
    virtual BOOL SetPixelShader(CKDWORD PShaderIndex) { return FALSE; }
    virtual BOOL SetVertexShaderConstant(CKDWORD Register, const void *Data, CKDWORD CstCount) { return FALSE; }
    virtual BOOL SetPixelShaderConstant(CKDWORD Register, const void *Data, CKDWORD CstCount) { return FALSE; }

    //----------------------------------------------------------------
    //--- Primitive Drawing (From system memory data or Vertex buffer)
    // At least a system memory (DrawPrimitive) version must be supported by an implementation
    virtual BOOL DrawPrimitive(VXPRIMITIVETYPE pType, WORD *indices, int indexcount, VxDrawPrimitiveData *data) { return FALSE; }
    virtual BOOL DrawPrimitiveVB(VXPRIMITIVETYPE pType, CKDWORD VertexBuffer, CKDWORD StartIndex, CKDWORD VertexCount, WORD *indices = NULL, int indexcount = NULL); // (Default Implementation by Lib)
    virtual BOOL DrawPrimitiveVBIB(VXPRIMITIVETYPE pType, CKDWORD VB, CKDWORD IB, CKDWORD MinVIndex, CKDWORD VertexCount, CKDWORD StartIndex, int Indexcount);		 // (Default Implementation by Lib)
                                                                                                                                                                     //-------------------------------------------------------------
                                                                                                                                                                     //--- Creation of Textures, Sprites and Vertex Buffer
                                                                                                                                                                     //--- Once an object index exists and before it can be used a texture,sprite or VB
                                                                                                                                                                     //--- must be created with this method, The Desired format pointer must point to
                                                                                                                                                                     //--- a CKTextureDesc , CKSpriteDesc , CKVertexBufferDesc ,CKIndexBufferDesc structure according to
                                                                                                                                                                     //--- the type of object being created...
    virtual BOOL CreateObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type, void *DesiredFormat);																		 // (Default Implementation by Lib)
    virtual BOOL DeleteObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type);																								 // (Implemented by Lib)
    virtual BOOL FlushObjects(CKDWORD TypeMask);																													 // (Implemented by Lib)
    virtual void UpdateObjectArrays(CKRasterizer *rst, CKDWORD ObjMasks = CKRST_OBJ_ALL);																			 // (Implemented by Lib)

    //-------------------------------------------------------------
    //--- Textures
    virtual BOOL LoadTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, int miplevel = -1) { return FALSE; }
    virtual CKTextureDesc *GetTextureData(CKDWORD Texture); // (Implemented by Lib)
                                                            //--- Copy the content of this context to a texture
    virtual BOOL CopyToTexture(CKDWORD Texture, VxRect *Src, VxRect *Dest, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS) { return NULL; }
    //--- Try to set a texture as the target for rendering
    virtual BOOL SetTargetTexture(CKDWORD TextureObject, int Width = 0, int Height = 0, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS, BOOL GenerateMipMap = FALSE) { return FALSE; }

    //-------------------------------------------------------------
    //--- Sprites
    virtual BOOL LoadSprite(CKDWORD Sprite, const VxImageDescEx &SurfDesc); // (Implemented by Lib)
    virtual CKSpriteDesc *GetSpriteData(CKDWORD Sprite);					// (Implemented by Lib)
                                                                            //-- Draw the sprite on the context using the given source and destination rectangles
    virtual BOOL DrawSprite(CKDWORD Sprite, VxRect *src, VxRect *dst) { return FALSE; }

    //-------------------------------------------------------------
    //--- Vertex Buffers
    virtual void *LockVertexBuffer(CKDWORD VB, CKDWORD StartVertex, CKDWORD VertexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT); // (default Implementation by Lib)
    virtual BOOL UnlockVertexBuffer(CKDWORD VB);																					 // (default Implementation by Lib)
    virtual CKVertexBufferDesc *GetVertexBufferData(CKDWORD VB);																	 // (Implemented by Lib)
    virtual BOOL OptimizeVertexBuffer(CKDWORD VB) { return FALSE; }

    //-------------------------------------------------------------
    //--- Copy the content of this rendering context to a memory buffer	(CopyToMemoryBuffer)
    //--- or Updates this rendering context with the content of a memory buffer	(CopyFromMemoryBuffer)
    virtual int CopyToMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, VxImageDescEx &img_desc) { return 0; }
    virtual int CopyFromMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, const VxImageDescEx &img_desc) { return 0; }

    //-------------------------------------------------------------
    //--- Each implementation can return here a pointer
    //--- to a structure containing its specific data
    virtual void *GetImplementationSpecificData() { return NULL; }

    //-----------------------------------------------------------------
    //--- Transform a set of vertices using the current transformation matrices
    //--- from a local coordinate system to screen and/or homogenous coordinates
    //--- (Implemented by Lib)
    virtual BOOL TransformVertices(int VertexCount, VxTransformData *Data);

    //-----------------------------------------------------------------
    //--- Computes the visibility of a box in the current viewport
    //--- and also computes its screen extents if asked.
    //--- (Implemented by Lib)
    virtual CKDWORD ComputeBoxVisibility(const VxBbox &box, BOOL World = FALSE, VxRect *extents = NULL);

    //-----------------------------------------------------------------
    //--- When using threads one must warn the context before/after using its methods of the active calling thread.
    //--- (mainly for OpenGL implementation to work correctly on multi-thread applications)
    virtual BOOL WarnThread(BOOL Enter) { return FALSE; }

    //-----------------------------------------------------------------
    //--- For web content the render context can be transparent (no clear of backbuffer but instead
    //--- a copy of what is currently on screen)
    //--- The AddDirtyrect method warns the context that a rectangle had been redrawn
    //--- and should be updated (NULL to update the entire backbuffer)
    virtual void SetTransparentMode(BOOL Trans) { m_TransparentMode = Trans; }
    virtual void AddDirtyRect(CKRECT *Rect)
    {
        if (!Rect)
            m_CleanAllRects = TRUE;
        else
            m_DirtyRects.PushBack(*Rect);
    }
    virtual void RestoreScreenBackup() {}

    //-----------------------------------------------------------------
    //--- User Clip Plane Function
    virtual BOOL SetUserClipPlane(CKDWORD ClipPlaneIndex, const VxPlane &PlaneEquation) { return FALSE; }
    virtual BOOL GetUserClipPlane(CKDWORD ClipPlaneIndex, VxPlane &PlaneEquation) { return FALSE; }

    //--------------------------------------------------------------
    // Each rasterizer context
    virtual void InitDefaultRenderStatesValue();

    //--------- Load a cubemap texture face
    virtual BOOL LoadCubeMapTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, CKRST_CUBEFACE Face, int miplevel = -1) { return FALSE; }

    //--------- For rasteriszer that supports stereo rendering (OpenGL)
    // set the buffer on which next drawing operations should occur
    virtual BOOL SetDrawBuffer(CKRST_DRAWBUFFER_FLAGS Flags) { return FALSE; }

    //-------------------------------------------------------------
    //--- Index Buffers
    virtual void *LockIndexBuffer(CKDWORD IB, CKDWORD StartIndex, CKDWORD IndexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT); // (default Implementation by Lib)
    virtual BOOL UnlockIndexBuffer(CKDWORD IB);																					  // (default Implementation by Lib)
    virtual CKIndexBufferDesc *GetIndexBufferData(CKDWORD IB);																	  // (Implemented by Lib)

    // Some implementations may not support call to lock with a single buffer return
    // but prefer a VxDrawPrimitiveData to be filled with the pointers and stride of each elements...
    // For every rasterizer that support the LockVertexBuffer version there is no need to override this method
    // as the default implementation take care of everything
    // Data.Flags & Data.VertexCount must be initialized correctly before calling...
    virtual BOOL LockVertexBuffer2(CKDWORD VB, CKDWORD StartVertex, VxDrawPrimitiveData &Data, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT)
    {
        BYTE *Memory = (BYTE *)LockVertexBuffer(VB, StartVertex, Data.VertexCount, Lock);
        if (!Memory)
            return FALSE;
        CKRSTSetupDPFromVertexBuffer(Memory, m_VertexBuffers[VB], Data);
        return TRUE;
    }

    virtual BOOL DrawPrimitiveSharedVB(VXPRIMITIVETYPE pType, CKDWORD SVertexBuffer, CKDWORD StartIndex, CKDWORD VertexCount, WORD *indices = NULL, int indexcount = NULL); // (Default Implementation by Lib / or handled by the rasterizer eg OpenGL)
    virtual BOOL DrawPrimitiveSharedVBIB(VXPRIMITIVETYPE pType, CKDWORD SVB, CKDWORD SIB, CKDWORD MinVIndex, CKDWORD VertexCount, CKDWORD StartIndex, int Indexcount);		// (Default Implementation by Lib / or handled by the rasterizer eg OpenGL)

    virtual BOOL LockSharedVB2(CKDWORD SVB, CKDWORD StartVertex, VxDrawPrimitiveData &Data, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
    virtual BOOL UnlockSharedVB(CKDWORD SVB);
    virtual CKSharedVBDesc *GetSharedVBData(CKDWORD SVB);

    virtual void *LockSharedIB(CKDWORD SIB, CKDWORD StartIndex, CKDWORD IndexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
    virtual BOOL UnlockSharedIB(CKDWORD SIB);
    virtual CKSharedIBDesc *GetSharedIBData(CKDWORD SIB);

    //---------------------------------------------------------------------
    //---- New methods to lock video memory
    //---- either by giving a direct access to video buffer
    virtual BOOL LockTextureVideoMemory(CKDWORD Texture, VxImageDescEx &Desc, int MipLevel = 0, VX_LOCKFLAGS Flags = VX_LOCK_DEFAULT) { return FALSE; }
    virtual BOOL UnlockTextureVideoMemory(CKDWORD Texture, int MipLevel = 0) { return FALSE; }

    // secret
    struct TexFromFile
    {
        VX_PIXELFORMAT pf;
        int MipLevels;
        CKDWORD ColorKey; // 0 : No Color Key, 0xFFRRGGBB (R,G,B) color Key
        BOOL IsDynamic;
    };

    //---- To Enable more direct creation of system objects	without
    //---- CK2_3D holding a copy of the texture
    // secret
    virtual BOOL CreateTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param) { return FALSE; }
    // secret
    virtual BOOL CreateTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param) { return FALSE; }

    // secret
    virtual BOOL CreateCubeTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param) { return FALSE; }
    // secret
    virtual BOOL CreateCubeTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param) { return FALSE; }

    // secret
    virtual BOOL CreateVolumeTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param) { return FALSE; }
    // secret
    virtual BOOL CreateVolumeTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param) { return FALSE; }

    virtual BOOL LoadVolumeMapTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, DWORD Depth, int miplevel) { return FALSE; }

    virtual void EnsureVBBufferNotInUse(CKVertexBufferDesc *desc);
    virtual void EnsureIBBufferNotInUse(CKIndexBufferDesc *desc);

public:
    // Default Lib implementation for Vertex,Index buffers (create them in system memory)
    BOOL CreateVertexBuffer(CKDWORD VB, CKVertexBufferDesc *DesiredFormat);
    BOOL CreateIndexBuffer(CKDWORD IB, CKIndexBufferDesc *DesiredFormat);
    virtual BOOL CreateSharedVertexBuffer(CKDWORD VB, CKSharedVBDesc *DesiredFormat);
    virtual BOOL CreateSharedIndexBuffer(CKDWORD IB, CKSharedIBDesc *DesiredFormat);

    // Default Lib implementation for sprites (create them using sub-textures)
    BOOL CreateSprite(CKDWORD Sprite, CKSpriteDesc *DesiredFormat);

    //--- The complete projection matrix is not computed every time a transformation matrix is set...
    // calling this function
    void UpdateMatrices(CKDWORD Flags);

    //-------------------------------------------------------------------------------
    //--- Render state cache management (Implemented by Lib)
    //--- in inline functions
    //--- A rasterizer context implementation should always
    //--- call these methods to maintain the render state cache
    //--- which avoid redoundant render state calls
    inline void FlushRenderStateCache();
    inline void InvalidateStateCache(VXRENDERSTATETYPE State);
    inline CKDWORD GetRSCacheValue(VXRENDERSTATETYPE State);
    inline void LockRenderState(VXRENDERSTATETYPE State, BOOL Locked);
    inline void DisableRenderState(VXRENDERSTATETYPE State, BOOL Disabled = TRUE);
    inline void OverrideRenderState(VXRENDERSTATETYPE State, BOOL Overridden = TRUE);
    inline BOOL InternalSetRenderState(VXRENDERSTATETYPE State, CKDWORD Value);
    inline BOOL InternalGetRenderState(VXRENDERSTATETYPE State, CKDWORD *Value);

    //--- Same cache management for texture stage states...
    inline void FlushTextureStateCache();
    inline void InvalidateTextureStateCache(int Stage, CKRST_TEXTURESTAGESTATETYPE State);
    inline CKDWORD GetTSCacheValue(int Stage, CKRST_TEXTURESTAGESTATETYPE State);
    inline void LockTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, BOOL Locked);
    inline void DisableTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, BOOL Disabled = TRUE);
    inline BOOL InternalSetTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, CKDWORD Value);
    inline BOOL InternalGetTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, CKDWORD *Value);

    inline void FlushCaches()
    {
        FlushRenderStateCache();
        FlushTextureStateCache();
        memset(m_CurrentTextures, 0, CKRST_MAX_STAGES * sizeof(CKTextureDesc *));
    }

    //---
    void ResetDirtyRects()
    {
        m_CleanAllRects = FALSE;
        m_DirtyRects.Resize(0);
    }

    //-------------- Dynamic Vertex buffers --------------
    // This method will create a vertex buffer the first time it is called
    // with a given vertex format and will later on returns this same
    // vertex buffer so it can be used when drawing dynamic primitives
    // the returned buffer that can be filled and render with DrawPrimitiveVB
    // (Implemented by Lib)
    // AddKey is a value that must be < 255  that can be used to have different
    // dynamic vertex buffer with the same vertex format
    CKDWORD GetDynamicVertexBuffer(CKDWORD VertexFormat, CKDWORD VertexCount, CKDWORD VertexSize, CKDWORD AddKey);

    VxDrawPrimitiveData *DoIndexedVertexBlending(VxDrawPrimitiveData *data);

    void LaunchEvent(CKRST_EVENTS iEvent)
    {
        if (m_Owner && m_Owner->m_EventWatcher)
            m_Owner->m_EventWatcher(this, iEvent, m_Owner->m_EventArgument);
    }

    void _SetObject(CKDWORD Object, CKRST_OBJECTINDEX ObjectTypeIndex, CKRasterizerObjectDesc *ObjectData);

    virtual float GetSurfacesVideoMemoryOccupation(int *NbTextures, int *NbSprites, float *TextureSize, float *SpriteSize);
    virtual BOOL FlushPendingGPUCommands() { return FALSE; }

    void UpdateStats(VXPRIMITIVETYPE pType, int indexcount, CKDWORD VertexCount);

public:
    CKRasterizerDriver *m_Driver; // Driver that was used to create this context
    CKRasterizer *m_Owner;		  // Main rasterizer object

    //----- Size Info
    CKDWORD m_PosX;	  // Top left corner of the context (relative to its window)
    CKDWORD m_PosY;	  // ....
    CKDWORD m_Width;  // Size of the context
    CKDWORD m_Height; // ....

    //------ Pixel Format
    CKDWORD m_Bpp;				  // Color buffer bits per pixel
    CKDWORD m_ZBpp;				  // Depth buffer bits per pixel
    CKDWORD m_StencilBpp;		  // Stencil buffer bits per pixel
    VX_PIXELFORMAT m_PixelFormat; // Color buffer pixel format

    //------- Fullscreen Info
    CKDWORD m_Fullscreen;  // Currently fullscreen ?
    CKDWORD m_RefreshRate; // Fullscreen refresh rate

    WIN_HANDLE m_Window; // Window on which the rendering occurs.
    BOOL m_SceneBegined;

    //------- Transformation matrices (World, View, Projection)
    CKDWORD m_MatrixUptodate;		  // Are m_ViewProjMatrix & m_TotalMatrix up-to-date ?
    VxMatrix m_WorldMatrix;			  // Local->World transformation matrix
    VxMatrix m_ViewMatrix;			  // World->View transformation matrix
    VxMatrix m_ProjectionMatrix;	  // Projection matrix
    VxMatrix m_ModelViewMatrix;		  // World*View
    VxMatrix m_ViewProjMatrix;		  // View*Proj
    VxMatrix m_TotalMatrix;			  // World*View*Proj (from a local coordinate system to screen)
    XArray<VxMatrix> m_WorldMatrices; // vertex blend additionnal matrices

    //------- Current Viewport Size
    CKViewportData m_ViewportData; // Viewport position and size

    //------- Texture,sprites,Vertex & index buffers,vertex and pixel shaders objects arrays
    XArray<CKTextureDesc *> m_Textures;			  // Array of texture specific data (format,data,etc..)
    XArray<CKSpriteDesc *> m_Sprites;			  // sprites
    XArray<CKVertexBufferDesc *> m_VertexBuffers; // Vertex Buffers
    XArray<CKIndexBufferDesc *> m_IndexBuffers;	  // Index Buffers
    XArray<CKVertexShaderDesc *> m_VertexShaders; // Vertex Shaders
    XArray<CKPixelShaderDesc *> m_PixelShaders;	  // Pixel Shaders
    XArray<CKSharedVBDesc *> m_SharedVBs;		  // Shared Vertex Buffers
    XArray<CKSharedIBDesc *> m_SharedIBs;		  // Index Buffers
    int m_TotalObjectCounts[eOBJECTCOUNT];

    CKDWORD m_TargetTexture;

    struct VBPool : public XArray<SharedVBsHolder *>
    {
        ~VBPool();
    };

    //--- These structure holds the Vertex buffers that were created
    XHashTable<VBPool *, CKDWORD> m_Pool4SharedVBs[2]; // Dynamic or not...
    XArray<SharedIBsHolder *> m_Pool4SharedIBs[2];	   // Dynamic or not...

    void SharedVertexBufferDestroyed(CKDWORD ObjIndex);
    void SharedIndexBufferDestroyed(CKDWORD ObjIndex);

    SharedIBsHolder *GetSharedIndexBufferPool(CKSharedIBDesc *DesiredFormat);
    SharedVBsHolder *GetSharedVertexBufferPool(CKSharedVBDesc *DesiredFormat);
    void CleanPools();

    //--- Debug
    int NbVBPoolAllocation;
    int NbIBPoolAllocation;

    int NbVBPool;
    int NbIBPool;

    //------- Lighting data
    CKMaterialData m_CurrentMaterialData;
    CKLightData m_CurrentLightData[RST_MAX_LIGHT];

    //---- A special case for the VXRENDERSTATE_INVERSEWINDING render state
    //---- which is maintained by the context
    BOOL m_InverseWinding;

    //--- For web content the render context can be transparent (no clear of backbuffer but instead
    //--- a copy of what is currently on screen)
    BOOL m_TransparentMode;
    BOOL m_CleanAllRects;
    XArray<CKRECT> m_DirtyRects;

    //----- Currently set textures for each stages...
    CKTextureDesc *m_CurrentTextures[CKRST_MAX_STAGES];

    //-------------------------------------
    //  Special case : some render states can be disabled...
    BOOL m_Antialias;

    //----------------------------------------
    // A hash table that stores the index of a dynamic Vertex buffer
    // given a vertex format
    // 3 Frames of dynamic Vertex Buffers
    XHashTable<CKDWORD, CKDWORD> m_DynamicVertexBuffers[MAX_FRAMES_AHEAD];
    unsigned int m_CurrentFrame;

    XArray<CKSharedVBDesc *> m_SharedVBToFree[MAX_FRAMES_AHEAD];
    XArray<CKSharedIBDesc *> m_SharedIBToFree[MAX_FRAMES_AHEAD];

    //-------------------------------------
    // A cache of renderstate to avoid redoundant renderstate calls
    // for texture and render states
    CKRenderStateData m_StateCache[VXRENDERSTATE_MAXSTATE];
    CKRenderStateData m_TexStateCache[CKRST_MAX_STAGES + 1][CKRST_TSS_MAXSTATE];
    int m_RenderStateCacheHit;	// Render state already set
    int m_RenderStateCacheMiss; // Render state not in cache

    //----------------------------------------------------------------------
    //--- to avoid redoundant call to SetTransform with a unity matrix
    //--- we keep track of all possible matrix that are currently at the
    //--- identity (a combination of CKRST_MATMASK values )
    //--- the default value is 0 and it's the rasterizer implementation
    //--- responsability to update and use this value.
    CKDWORD m_UnityMatrixMask;

    //----------------------------------------------------------------------
    //--- Internal structure use in case of Indexed vertex blending
    //--- done by the rasterizer
    VxDrawPrimitiveData m_InternalDrawPrim;
    // VxScratch					m_ScratchPool;

    //--- Highest stage which has been set since the last call to DisableAllTextureStages
    // This must be updated by implementations in the implementation of the  SetTexture method
    int m_MaxSetStage;

    //--- Bool set to true if an effect shader is in use
    BOOL m_ShaderInUse;
    //--- Number of VBL to wait before performing
    // the "BackToFront". If (m_PresentInterval == 0) this means an immediate presentation
    // otherwise it is the number of VBL swap before rendering...
    // This value came in replacement for the WaitForVBL parameter of BackToFront
    DWORD m_PresentInterval;
    DWORD m_CurrentPresentInterval;

    CKRasterizerStats *m_Stats;
    CKRasterizerStats m_LocalStats;
};

#ifndef NO_CRITICAL_SECTION
/*********************************************************
A simple class to automatically perform a Enter/Leave
critical section when used inside a code scope...
We use a critical section for each call to DX since it seems
it was not necesseraly always multi-thread safe
**********************************************************/
class CKRstCriticalSection
{
public:
    CKRstCriticalSection(CKRasterizer *Rst) : m_Rst(Rst)
    {
        if (Rst && Rst->m_Mutex)
        {
            Rst->m_Mutex->EnterMutex();
        }
    }
    ~CKRstCriticalSection()
    {
        if (m_Rst && m_Rst->m_Mutex)
        {
            m_Rst->m_Mutex->LeaveMutex();
        }
    }
    CKRasterizer *m_Rst;
};
#define C_SECTION_RST(x) CKRstCriticalSection CSection(x);
#define C_SECTION() CKRstCriticalSection CSection(m_Owner);

#else
/********************************************************
DO NOT USE CRITICAL SECTION
*********************************************************/
#define C_SECTION_RST(x)
#define C_SECTION()
#endif

/*******************************************************************************
Render State cache management
*******************************************************************************/
inline void CKRasterizerContext::FlushRenderStateCache()
{
    for (int i = 0; i < VXRENDERSTATE_MAXSTATE; ++i)
    {
        m_StateCache[i].Flags &= ~(RSC_VALID | RSC_LOCKED);
        m_StateCache[i].Value = m_StateCache[i].DefaultValue;
    }
}

inline void CKRasterizerContext::InvalidateStateCache(VXRENDERSTATETYPE State)
{
    m_StateCache[State].Flags &= ~RSC_VALID;
}

inline CKDWORD CKRasterizerContext::GetRSCacheValue(VXRENDERSTATETYPE State)
{
    return m_StateCache[State].Value;
}

inline void CKRasterizerContext::LockRenderState(VXRENDERSTATETYPE State, BOOL Locked)
{
    if (Locked)
    {
        m_StateCache[State].Flags |= RSC_LOCKED;
    }
    else
    {
        m_StateCache[State].Flags &= ~RSC_LOCKED;
    }
}

inline void CKRasterizerContext::DisableRenderState(VXRENDERSTATETYPE State, BOOL Disabled)
{
    if (Disabled)
    {
        m_StateCache[State].Flags |= RSC_DISABLED;
    }
    else
    {
        m_StateCache[State].Flags &= ~RSC_DISABLED;
    }
}

inline void CKRasterizerContext::OverrideRenderState(VXRENDERSTATETYPE State, BOOL Overridden)
{
    if (Overridden)
    {
        m_StateCache[State].Flags |= RSC_OVERRIDDEN;
    }
    else
    {
        m_StateCache[State].Flags &= ~RSC_OVERRIDDEN;
    }
}

inline BOOL CKRasterizerContext::InternalSetRenderState(VXRENDERSTATETYPE State, CKDWORD Value)
{
    if (m_StateCache[State].Flags & (RSC_LOCKED | RSC_DISABLED))
        return TRUE;
    if ((m_StateCache[State].Flags & RSC_VALID) && (m_StateCache[State].Value == Value))
    {
        m_RenderStateCacheHit++;
        return TRUE;
    }
    else
    {
        m_RenderStateCacheMiss++;
        m_StateCache[State].Value = Value;
        m_StateCache[State].Flags |= RSC_VALID;
        return FALSE;
    }
}

// Returns the value of cached render state or FALSE if the
// cache is invalid
inline BOOL CKRasterizerContext::InternalGetRenderState(VXRENDERSTATETYPE State, CKDWORD *Value)
{
    if (m_StateCache[State].Flags & RSC_VALID)
    {
        *Value = m_StateCache[State].Value;
        return TRUE;
    }
    else
    {
        *Value = m_StateCache[State].DefaultValue;
        return FALSE;
    }
}

/*******************************************************************************
Texture  State cache management
*******************************************************************************/
inline void CKRasterizerContext::FlushTextureStateCache()
{
    for (int s = 0; s < CKRST_MAX_STAGES; ++s)
    {
        for (int i = 0; i < CKRST_TSS_MAXSTATE; ++i)
        {
            m_TexStateCache[s][i].Flags &= ~(RSC_VALID | RSC_LOCKED);
            m_TexStateCache[s][i].Value = m_TexStateCache[s][i].DefaultValue;
        }
    }
}

inline void CKRasterizerContext::InvalidateTextureStateCache(int Stage, CKRST_TEXTURESTAGESTATETYPE State)
{
    m_TexStateCache[Stage][State].Flags &= ~RSC_VALID;
}

inline CKDWORD CKRasterizerContext::GetTSCacheValue(int Stage, CKRST_TEXTURESTAGESTATETYPE State)
{
    return m_TexStateCache[Stage][State].Value;
}

inline void CKRasterizerContext::LockTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, BOOL Locked)
{
    if (Locked)
    {
        m_TexStateCache[Stage][State].Flags |= RSC_LOCKED;
    }
    else
    {
        m_TexStateCache[Stage][State].Flags &= ~RSC_LOCKED;
    }
}

inline void CKRasterizerContext::DisableTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, BOOL Disabled)
{
    if (Disabled)
    {
        m_TexStateCache[Stage][State].Flags |= RSC_DISABLED;
    }
    else
    {
        m_TexStateCache[Stage][State].Flags &= ~RSC_DISABLED;
    }
}

inline BOOL CKRasterizerContext::InternalSetTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, CKDWORD Value)
{
    if (Stage >= CKRST_MAX_STAGES)
    {
        return TRUE;
    }
    if (m_TexStateCache[Stage][State].Flags & (RSC_LOCKED | RSC_DISABLED))
        return TRUE;
    if ((m_TexStateCache[Stage][State].Flags & RSC_VALID) && (m_TexStateCache[Stage][State].Value == Value))
    {
        m_RenderStateCacheHit++;
        return TRUE;
    }
    else
    {
        m_MaxSetStage = XMax(Stage, m_MaxSetStage);
        m_RenderStateCacheMiss++;
        m_TexStateCache[Stage][State].Value = Value;
        m_TexStateCache[Stage][State].Flags |= RSC_VALID;
        return FALSE;
    }
}

// Returns the value of cached render state or FALSE if the
// cache is invalid
inline BOOL CKRasterizerContext::InternalGetTextureState(int Stage, CKRST_TEXTURESTAGESTATETYPE State, CKDWORD *Value)
{
    if (Stage >= CKRST_MAX_STAGES)
    {
        *Value = 0;
        return FALSE;
    }
    if (m_TexStateCache[Stage][State].Flags & RSC_VALID)
    {
        *Value = m_TexStateCache[Stage][State].Value;
        return TRUE;
    }
    else
    {
        *Value = m_TexStateCache[Stage][State].DefaultValue;
        return FALSE;
    }
}

#ifdef WIN32
#pragma warning(disable : 4035)
/**************************************************
Small util to get the position of a given bit...
*****************************************************/
inline int GetFirstBitpos(CKDWORD data)
{
    __asm {
            mov		eax,data
            and     eax,eax
            jz		exitnow
            bsf		eax,eax
            inc		eax
    exitnow:
    }
}
#pragma warning(default : 4035)
#else
inline int GetFirstBitpos(CKDWORD data)
{
    CKDWORD Lsb = 0;
    if (!data)
        return 0;
    while (!(data & 1))
    {
        data >>= 1;
        Lsb++;
    }
    return Lsb + 1;
}
#endif

#endif