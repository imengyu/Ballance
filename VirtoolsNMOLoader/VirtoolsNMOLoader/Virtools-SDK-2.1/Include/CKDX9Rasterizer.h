#ifndef CKRASTERIZERDX9_H
#define CKRASTERIZERDX9_H "$Id:$"
/*************************************************************************/
/*	File : CKDX9Rasterizer.h											 */
/*	Author : Romain Sididris											 */
/*																		 */
/*	+ Direct X 9 Rasterizer declaration									 */
/*	+ Some methods of these classes are already implemented in the 		 */
/*	CKRasterizerLib	library	as they are common to all rasterizers		 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2004, All Rights Reserved.					 */
/*************************************************************************/

// Ensure we are using correct DirectDraw version
#define NBTEMPZBUFFER 256

#define _NOD3DX

#ifdef _DEBUG
#define D3D_DEBUG_INFO
#endif

#ifdef _XBOX
#include <xtl.h>
#if (_XBOX_VER >= 200)
#define NO_FIXED_PIPELINE
#endif
#else
#include "d3d9.h"
#include "d3dx9.h"
// DDraw is included here only to get the Total Video memory and Free Video Memory
#include "ddraw.h"
#endif

#include "CKRasterizer.h"
#include "XBitArray.h"

//---Implemented in CKDX9PixelFormatUtils.cpp :

D3DFORMAT VxPixelFormatToD3DFormat(VX_PIXELFORMAT pf);
VX_PIXELFORMAT D3DFormatToVxPixelFormat(D3DFORMAT ddpf);

D3DFORMAT TextureDescToD3DFormat(CKTextureDesc *desc);
void D3DFormatToTextureDesc(D3DFORMAT ddpf, CKTextureDesc *desc);

//------

class CKDX9RasterizerDriver;
class CKDX9RasterizerContext;
class CKDX9Rasterizer;

typedef BOOL (*SetDXRenderStateFunc)(CKDX9RasterizerContext *ctx, CKDWORD Value);
typedef BOOL (*GetDXRenderStateFunc)(CKDX9RasterizerContext *ctx, CKDWORD *Value);

#define PREPAREDXSTRUCT(x)        \
    {                             \
        memset(&x, 0, sizeof(x)); \
        x.dwSize = sizeof(x);     \
    }
#define SAFERELEASE(x)    \
    {                     \
        if (x)            \
            x->Release(); \
        x = NULL;         \
    }
#define SAFELASTRELEASE(x)           \
    {                                \
        int refCount = 0;            \
        if (x)                       \
            refCount = x->Release(); \
        x = NULL;                    \
        XASSERT(refCount == 0);      \
    }

// Store texture operation required to perform blending between two texture stages
typedef struct CKStageBlend
{
    D3DTEXTUREOP Cop;
    CKDWORD Carg1;
    CKDWORD Carg2;
    D3DTEXTUREOP Aop;
    CKDWORD Aarg1;
    CKDWORD Aarg2;
} CKStageBlend;

/*******************************************
 A texture object for DX9 : contains the
 texture surface pointer
*********************************************/
typedef struct CKDX9TextureDesc : public CKTextureDesc
{
public:
    union
    {
        LPDIRECT3DTEXTURE9 DxTexture;
        LPDIRECT3DCUBETEXTURE9 DxCubeTexture;
        LPDIRECT3DVOLUMETEXTURE9 DxVolumeTexture;
    };
    //----- For render target textures...
    LPDIRECT3DTEXTURE9 DxRenderTexture;
    //----- For non managed surface to be locked (DX9 does not support direct locking anymore)
    LPDIRECT3DSURFACE9 DxLockedSurface;
    DWORD LockedFlags;

public:
    CKDX9TextureDesc()
    {
        DxTexture = NULL;
        DxRenderTexture = NULL;
        DxLockedSurface = NULL;
    }
    ~CKDX9TextureDesc()
    {
        SAFERELEASE(DxTexture);
        SAFERELEASE(DxRenderTexture);
        SAFERELEASE(DxLockedSurface);
    }
} CKDX9TextureDesc;

/********************************************
Same override for vertex buffers
*************************************************/
typedef struct CKDX9VertexBufferDesc : public CKVertexBufferDesc
{
public:
    LPDIRECT3DVERTEXBUFFER9 DxBuffer;

public:
    CKDX9VertexBufferDesc() { DxBuffer = NULL; }
    ~CKDX9VertexBufferDesc() { SAFERELEASE(DxBuffer); }
} CKDX9VertexBufferDesc;

/********************************************
Same override for index buffers
*************************************************/
typedef struct CKDX9IndexBufferDesc : public CKIndexBufferDesc
{
public:
    LPDIRECT3DINDEXBUFFER9 DxBuffer;

public:
    CKDX9IndexBufferDesc() { DxBuffer = NULL; }
    ~CKDX9IndexBufferDesc() { SAFERELEASE(DxBuffer); }
} CKDX9IndexBufferDesc;

/********************************************
Vertex Shaders....
*************************************************/
typedef struct CKDX9VertexShaderDesc : public CKVertexShaderDesc
{
public:
    LPDIRECT3DVERTEXSHADER9 DxShader;
    CKDX9RasterizerContext *Owner;
    XArray<BYTE> m_FunctionData;

public:
    BOOL Create(CKDX9RasterizerContext *Ctx, CKVertexShaderDesc *Format);
    virtual ~CKDX9VertexShaderDesc();
    CKDX9VertexShaderDesc()
    {
        DxShader = 0;
        Owner = 0;
    }
} CKDX9VertexShaderDesc;

/********************************************
Pixel Shaders....
*************************************************/
typedef struct CKDX9PixelShaderDesc : public CKPixelShaderDesc
{
public:
    LPDIRECT3DPIXELSHADER9 DxShader;
    CKDX9RasterizerContext *Owner;

public:
    BOOL Create(CKDX9RasterizerContext *Ctx, CKDWORD *Function);
    virtual ~CKDX9PixelShaderDesc();
    CKDX9PixelShaderDesc()
    {
        DxShader = 0;
        Owner = 0;
    }

} CKDX9PixelShaderDesc;

/*****************************************************************
 CKDX9RasterizerContext override
******************************************************************/
class CKDX9RasterizerContext : public CKRasterizerContext
{
public:
    //--- Construction/destruction
    CKDX9RasterizerContext(CKDX9RasterizerDriver *Driver);
    virtual ~CKDX9RasterizerContext();

    //--- Creation
    virtual BOOL Create(WIN_HANDLE Window, int PosX = 0, int PosY = 0, int Width = 0, int Height = 0, int Bpp = -1, BOOL Fullscreen = 0, int RefreshRate = 0, int Zbpp = -1, int StencilBpp = -1);
    //---
    virtual BOOL Resize(int PosX = 0, int PosY = 0, int Width = 0, int Height = 0, CKDWORD Flags = 0);
    virtual BOOL Clear(CKDWORD Flags = CKRST_CTXCLEAR_ALL, CKDWORD Ccol = 0, float Z = 1.0f, CKDWORD Stencil = 0, int RectCount = 0, CKRECT *rects = NULL);
    virtual BOOL BackToFront();

    //--- Scene
    virtual BOOL BeginScene();
    virtual BOOL EndScene();

    //--- Lighting & Material States
    virtual BOOL SetLight(CKDWORD Light, CKLightData *data);
    virtual BOOL EnableLight(CKDWORD Light, BOOL Enable);
    virtual BOOL SetMaterial(CKMaterialData *mat);

    //--- Viewport State
    virtual BOOL SetViewport(CKViewportData *data);

    //--- Transform Matrix
    virtual BOOL SetTransformMatrix(VXMATRIX_TYPE Type, const VxMatrix &Mat);

    //--- Render states
    virtual BOOL SetRenderState(VXRENDERSTATETYPE State, CKDWORD Value);
    virtual BOOL GetRenderState(VXRENDERSTATETYPE State, CKDWORD *Value);

    //--- Texture States
    virtual BOOL SetTexture(CKDWORD Texture, int Stage = 0);
    virtual BOOL SetTextureStageState(int Stage, CKRST_TEXTURESTAGESTATETYPE Tss, CKDWORD Value);

    //--- Vertex & Pixel shaders
    virtual BOOL SetVertexShader(CKDWORD VShaderIndex);
    virtual BOOL SetPixelShader(CKDWORD PShaderIndex);
    virtual BOOL SetVertexShaderConstant(CKDWORD Register, const void *Data, CKDWORD CstCount);
    virtual BOOL SetPixelShaderConstant(CKDWORD Register, const void *Data, CKDWORD CstCount);

    //--- Drawing
    virtual BOOL DrawPrimitive(VXPRIMITIVETYPE pType, WORD *indices, int indexcount, VxDrawPrimitiveData *data);
    virtual BOOL DrawPrimitiveVB(VXPRIMITIVETYPE pType, CKDWORD VertexBuffer, CKDWORD StartIndex, CKDWORD VertexCount, WORD *indices = NULL, int indexcount = NULL);
    virtual BOOL DrawPrimitiveVBIB(VXPRIMITIVETYPE pType, CKDWORD VB, CKDWORD IB, CKDWORD MinVIndex, CKDWORD VertexCount, CKDWORD StartIndex, int Indexcount);

    //--- Creation of Textures, Sprites and Vertex Buffer
    virtual BOOL CreateObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type, void *DesiredFormat);

    //--- Vertex Buffers
    virtual void *LockVertexBuffer(CKDWORD VB, CKDWORD StartVertex, CKDWORD VertexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
    virtual BOOL UnlockVertexBuffer(CKDWORD VB);

    //--- Textures
    virtual BOOL LoadCubeMapTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, CKRST_CUBEFACE Face, int miplevel = -1);
    virtual BOOL LoadTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, int miplevel = -1);
    virtual BOOL CopyToTexture(CKDWORD Texture, VxRect *Src, VxRect *Dest, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS);

    //--- Sprites
    virtual BOOL DrawSprite(CKDWORD Sprite, VxRect *src, VxRect *dst);

    virtual void *GetImplementationSpecificData() { return &m_DirectXData; }

    //--- Utils
    virtual int CopyToMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, VxImageDescEx &img_desc);
    virtual int CopyFromMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, const VxImageDescEx &img_desc);

    //-- Sets the rendering to occur on a texture (reset the texture format to match )
    virtual BOOL SetTargetTexture(CKDWORD TextureObject, int Width = 0, int Height = 0, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS, BOOL GenerateMipMap = FALSE);

    virtual BOOL SetUserClipPlane(CKDWORD ClipPlaneIndex, const VxPlane &PlaneEquation);
    virtual BOOL GetUserClipPlane(CKDWORD ClipPlaneIndex, VxPlane &PlaneEquation);

    virtual void *LockIndexBuffer(CKDWORD IB, CKDWORD StartIndex, CKDWORD IndexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
    virtual BOOL UnlockIndexBuffer(CKDWORD IB);

    //---------------------------------------------------------------------
    //---- New methods to lock video memory (DX only)
    virtual BOOL LockTextureVideoMemory(CKDWORD Texture, VxImageDescEx &Desc, int MipLevel = 0, VX_LOCKFLAGS Flags = VX_LOCK_DEFAULT);
    virtual BOOL UnlockTextureVideoMemory(CKDWORD Texture, int MipLevel = 0);

    //---- To Enable more direct creation of system objects	without
    //---- CK2_3D holding a copy of the texture
    virtual BOOL CreateTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param);
    virtual BOOL CreateTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param);

    virtual BOOL CreateCubeTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param);
    virtual BOOL CreateCubeTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param);

    virtual BOOL CreateVolumeTextureFromFile(CKDWORD Texture, const char *Filename, TexFromFile *param);
    virtual BOOL CreateVolumeTextureFromFileInMemory(CKDWORD Texture, void *mem, DWORD sz, TexFromFile *param);

    virtual BOOL LoadVolumeMapTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, DWORD Depth, int miplevel);

    virtual void EnsureVBBufferNotInUse(CKVertexBufferDesc *desc);
    virtual void EnsureIBBufferNotInUse(CKIndexBufferDesc *desc);
    virtual float GetSurfacesVideoMemoryOccupation(int *NbTextures, int *NbSprites, float *TextureSize, float *SpriteSize);
    virtual BOOL FlushPendingGPUCommands();

protected:
    //-----------------------
    void UpdateDirectXData();
    BOOL InternalDrawPrimitiveVB(VXPRIMITIVETYPE pType, CKDX9VertexBufferDesc *VB, CKDWORD StartIndex, CKDWORD VertexCount, WORD *indices, int indexcount, BOOL Clip);
    void SetupStreams(LPDIRECT3DVERTEXBUFFER9 Buffer, CKDWORD VFormat, CKDWORD VSize);

    //--- Objects creation
    BOOL CreateTexture(CKDWORD Texture, CKTextureDesc *DesiredFormat);
    BOOL CreateVertexShader(CKDWORD VShader, CKVertexShaderDesc *DesiredFormat);
    BOOL CreatePixelShader(CKDWORD PShader, CKPixelShaderDesc *DesiredFormat);
    BOOL CreateVertexBuffer(CKDWORD VB, CKVertexBufferDesc *DesiredFormat);
    BOOL CreateIndexBuffer(CKDWORD IB, CKIndexBufferDesc *DesiredFormat);

    BOOL TextureCanUseAutoGenMipMap(D3DFORMAT TextureFormat);

    void UpdateTextureBPL(CKDWORD Texture);

    //---- Cleanup
    void FlushNonManagedObjects();
    void ReleaseIndexBuffers();
    void ClearStreamCache();
    void ReleaseScreenBackup();
    void ResetDevice();
    CKDWORD DX9PresentInterval(DWORD PresentInterval);
#ifdef _NOD3DX
    BOOL LoadSurface(const D3DSURFACE_DESC &ddsd, const D3DLOCKED_RECT &LockRect, const VxImageDescEx &SurfDesc);
#endif

    //--- Temp Z-Buffers for texture rendering...
    void ReleaseTempZBuffers()
    {
        for (int i = 0; i < 256; ++i)
        {
            SAFELASTRELEASE(m_TempZBuffers[i]);
        }
    }

    LPDIRECT3DSURFACE9 GetTempZBuffer(int Width, int Height);

public:
    LPDIRECT3DDEVICE9 m_Device;
    D3DPRESENT_PARAMETERS m_PresentParams;
    VxDirectXData m_DirectXData;
    BOOL m_SoftwareVertexProcessing;
    BOOL m_SoftwareShader;
    BOOL m_ResetLastFrame;

    //----------------------------------------------------
    //--- Index buffer filled when drawing primitives
    CKDX9IndexBufferDesc *m_IndexBuffer[2]; // Clip/unclipped

    //-----------------------------------------------------
    //--- Render Target if rendering is redirected to a texture
    LPDIRECT3DSURFACE9 m_DefaultBackBuffer;	 // Backup pointer of previous back buffer
    LPDIRECT3DSURFACE9 m_DefaultDepthBuffer; // Backup pointer of previous depth buffer

    volatile BOOL m_InCreateDestroy;

    //--- For web content the render context can be transparent (no clear of backbuffer but instead
    //--- a copy of what is currently on screen)
    LPDIRECT3DSURFACE9 m_ScreenBackup;

    //-------------------------------------------------
    //--- to avoid redoundant calls to SetVertexShader & SetStreamSource :
    //--- a cache with the current vertex format and source VB
    CKDWORD m_CurrentVertexShaderCache;
    CKDWORD m_CurrentVertexFormatCache;
    LPDIRECT3DVERTEXBUFFER9 m_CurrentVertexBufferCache;
    CKDWORD m_CurrentVertexSizeCache;

    //--------------------------------------------------
    // Render states which must be disabled or which values must be translated...
    CKDWORD *m_TranslatedRenderStates[VXRENDERSTATE_MAXSTATE];

    //-----------------------------------------------------
    // + To do texture rendering, Z-buffers are created when
    // needed for any given size (power of two)
    // These Z buffers are stored in the rasterizer context
    // TempZbuffers array and are attached when doing
    // texture rendering
    LPDIRECT3DSURFACE9 m_TempZBuffers[NBTEMPZBUFFER];
};

/*****************************************************************
 CKDX9RasterizerDriver overload
******************************************************************/
class CKDX9RasterizerDriver : public CKRasterizerDriver
{
public:
    CKDX9RasterizerDriver(CKDX9Rasterizer *rst);
    virtual ~CKDX9RasterizerDriver();

    //--- Contexts
    virtual CKRasterizerContext *CreateContext();

    BOOL InitializeCaps(int AdapterIndex, D3DDEVTYPE DevType);
    BOOL IsTextureFormatOk(D3DFORMAT TextureFormat, D3DFORMAT AdapterFormat, DWORD Usage = 0);

    D3DFORMAT FindNearestTextureFormat(CKTextureDesc *desc);
    D3DFORMAT FindNearestRenderTargetFormat(int Bpp, BOOL Windowed);
    D3DFORMAT FindNearestDepthFormat(D3DFORMAT pf, int ZBpp, int StencilBpp);

public:
    BOOL m_Inited;
    UINT m_AdapterIndex;
    D3DDEVTYPE m_DevType;
    D3DCAPS9 m_D3DCaps;
    D3DADAPTER_IDENTIFIER9 m_D3DIdentifier;
    XArray<D3DFORMAT> m_RenderFormats;
    BOOL m_IsHTL; // Transfom & Lighting
};

/*****************************************************************
 CKDX9Rasterizer overload
******************************************************************/
class CKDX9Rasterizer : public CKRasterizer
{
public:
    CKDX9Rasterizer();
    virtual ~CKDX9Rasterizer();

    virtual BOOL Start(WIN_HANDLE AppWnd);
    virtual void Close(void);

public:
    BOOL m_Init;
    LPDIRECT3D9 m_D3D9;

    // Stage Blends
    void InitBlendStages();
    CKStageBlend *m_BlendStages[256];
};

#endif