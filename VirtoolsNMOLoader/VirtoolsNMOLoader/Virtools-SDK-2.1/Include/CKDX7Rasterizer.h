#ifndef CKRASTERIZERDX7_H
#define CKRASTERIZERDX7_H "$Id:$"

/*************************************************************************/
/*	File : CKDX7Rasterizer.h											 */
/*	Author : Romain Sididris											 */
/*																		 */
/*	+ Direct X 7 Rasterizer declaration									 */
/*	+ Some methods of these classes are already implemented in the 		 */
/*	CKRasterizerLib	library	as they are common to all rasterizers		 */
/*																		 */
/*  + D3DX Library in DX7 have a bug when enumerating drivers on a		 */
/*   ATI Rage Mobility that's why we use DX8 sdk to link even if we only */
/*	use DX7 interfaces													 */
/*																		 */
/*	See CKRasterizer.h 	for details on the base classes					 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/

// The DX7 rasterizer can be compiled without using D3DX
#define _NOD3DX

// Ensure we are using correct DirectDraw version
#define DIRECTDRAW_VERSION 0x0700

#include "CKRasterizer.h"
#include "XBitArray.h"
#ifndef _NOD3DX
#include "d3dx.h"
#endif
#include "ddraw.h"
#include "d3d.h"

#undef FAILED
#define FAILED(Err) (Err != S_OK)
#undef SUCCEEDED
#define SUCCEEDED(Err) (Err == S_OK)

class CKDX7RasterizerDriver;
class CKDX7RasterizerContext;
class CKDX7Rasterizer;

/*******************************************************************
Some render states need a specific implementation while others can
directly be forwarded to DX , for the first ones we use a array of function
pointers to call the overloading function...
*******************************************************************/
typedef BOOL (*SetDXRenderStateFunc)(CKDX7RasterizerContext *ctx, CKDWORD Value);
typedef BOOL (*GetDXRenderStateFunc)(CKDX7RasterizerContext *ctx, CKDWORD *Value);

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

// {secret}
extern CKDWORD g_FaceCaps[6];

/**********************************************************************
Store texture operations required to perform blending between two texture stages
for a given stage...
***********************************************************************/
typedef struct CKStageBlend
{
    D3DTEXTUREOP Cop;
    CKDWORD Carg1;
    CKDWORD Carg2;
    D3DTEXTUREOP Aop;
    CKDWORD Aarg1;
    CKDWORD Aarg2;
} CKStageBlend;

#ifdef _NOD3DX
/*******************************************
 Tempory storage structure for device enumeration
*********************************************/
struct D3DEnumInfo
{
    // DXObjects
    LPDIRECTDRAW7 m_DD7;
    LPDIRECT3D7 m_D3D7;
    // Rasterizer
    CKDX7Rasterizer *m_Rst;
    // D3D Device info
    XString m_Desc;
    XString m_Name;
    // DDraw Driver info
    GUID *m_DriverGUIDPtr;
    GUID m_DriverGUID;
    DDCAPS m_DriverCaps;
    DDCAPS m_HELCaps;
    HMONITOR m_Monitor;
    // D3D Device Description
    D3DDEVICEDESC7 m_DeviceDesc;
    // Available display modes
    XArray<VxDisplayMode> m_DModes;
};
#endif

/*******************************************
 A texture object for DX7 : contains the
 texture surface pointer
*********************************************/
typedef struct CKDX7TextureDesc : public CKTextureDesc
{
public:
    LPDIRECTDRAWSURFACE7 DxSurface;
    LPDIRECTDRAWSURFACE7 DxRenderSurface;
    CKDX7TextureDesc()
    {
        DxSurface = NULL;
        DxRenderSurface = NULL;
    }
    ~CKDX7TextureDesc()
    {
        DetachZBuffer();
        SAFERELEASE(DxSurface);
        SAFERELEASE(DxRenderSurface);
    }

    LPDIRECTDRAWSURFACE7 AttachZBuffer(LPDIRECTDRAWSURFACE7 iZBuffer, int iFace)
    {
        LPDIRECTDRAWSURFACE7 surf = DxRenderSurface;
        if (Flags & CKRST_TEXTURE_CUBEMAP)
        {

            DDSCAPS2 caps;
            memset(&caps, 0, sizeof(DDSCAPS2));
            caps.dwCaps = DDSCAPS_COMPLEX;
            caps.dwCaps2 = DDSCAPS2_CUBEMAP | g_FaceCaps[iFace];

            if (iFace != CKRST_CUBEFACE_XPOS)
            {
                DxSurface->GetAttachedSurface(&caps, &surf);
                if (surf)
                    surf->Release();
            }
            else
            {
                surf = DxSurface;
            }
        }

        if (!surf)
            return NULL;

        HRESULT res = surf->AddAttachedSurface(iZBuffer);
        if (res == S_OK)
        {
            Flags |= CKRST_TEXTURE_SURFATTACHED;
            return surf;
        }

        return NULL;
    }

    void DetachZBuffer()
    {
        if (Flags & CKRST_TEXTURE_SURFATTACHED)
        {
            Flags &= ~CKRST_TEXTURE_SURFATTACHED;

            if (Flags & CKRST_TEXTURE_CUBEMAP)
            {
                // si la texture est une cube map
                // il faut detacher le Z buf de la surface dans laquelle on vient de rendre = TODO !

                DDSCAPS2 caps;
                memset(&caps, 0, sizeof(DDSCAPS2));
                caps.dwCaps = DDSCAPS_ZBUFFER;

                if (DxSurface)
                {
                    LPDIRECTDRAWSURFACE7 zbuf = 0;
                    DxSurface->GetAttachedSurface(&caps, &zbuf);
                    if (zbuf)
                    {
                        zbuf->Release();
                        DxSurface->DeleteAttachedSurface(0, zbuf);
                        return;
                    }
                }

                // now we will iterate on all surfaces
                for (int i = 0; i < 6; ++i)
                {
                    caps.dwCaps = DDSCAPS_COMPLEX;
                    caps.dwCaps2 = DDSCAPS2_CUBEMAP | g_FaceCaps[i];

                    LPDIRECTDRAWSURFACE7 faceSurf = 0;
                    DxSurface->GetAttachedSurface(&caps, &faceSurf);
                    if (faceSurf)
                    {
                        faceSurf->Release();

                        caps.dwCaps = DDSCAPS_ZBUFFER;
                        caps.dwCaps2 = 0;

                        LPDIRECTDRAWSURFACE7 zbuf = 0;
                        faceSurf->GetAttachedSurface(&caps, &zbuf);
                        if (zbuf)
                        {
                            zbuf->Release();
                            faceSurf->DeleteAttachedSurface(0, zbuf);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (DxRenderSurface)
                    DxRenderSurface->DeleteAttachedSurface(0, NULL);
            }
        }
    }
} CKDX7TextureDesc;

/*******************************************
 A vertex buffer object for DX7 : contains the
 DIRECT3DVERTEXBUFFER7 pointer
*********************************************/
typedef struct CKDX7VertexBufferDesc : public CKVertexBufferDesc
{
    LPDIRECT3DVERTEXBUFFER7 DxBuffer;
    CKDX7VertexBufferDesc() { DxBuffer = NULL; }
    ~CKDX7VertexBufferDesc() { SAFERELEASE(DxBuffer); }
} CKDX7VertexBufferDesc;

/*****************************************************************
 CKDX7RasterizerContext overload , See CKRasterizer.h for
 methods description...
******************************************************************/
class CKDX7RasterizerContext : public CKRasterizerContext
{
public:
    //--- Construction/destruction
    CKDX7RasterizerContext(CKDX7RasterizerDriver *Driver);
    virtual ~CKDX7RasterizerContext();

    //-- Virtual overrides...

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

    //--- Drawing
    virtual BOOL DrawPrimitive(VXPRIMITIVETYPE pType, WORD *indices, int indexcount, VxDrawPrimitiveData *data);
    virtual BOOL DrawPrimitiveVB(VXPRIMITIVETYPE pType, CKDWORD VertexBuffer, CKDWORD StartIndex, CKDWORD VertexCount, WORD *indices = NULL, int indexcount = NULL);

    //--- Creation of Textures, Sprites and Vertex Buffer
    virtual BOOL CreateObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type, void *DesiredFormat);

    //--- Textures
    virtual BOOL LoadTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, int miplevel = -1);
    virtual BOOL LoadCubeMapTexture(CKDWORD Texture, const VxImageDescEx &SurfDesc, CKRST_CUBEFACE Face, int miplevel);
    virtual BOOL CopyToTexture(CKDWORD Texture, VxRect *Src, VxRect *Dest, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS);

    //--- Sprites
    virtual BOOL DrawSprite(CKDWORD Sprite, VxRect *src, VxRect *dst);

    //--- Vertex Buffers
    virtual void *LockVertexBuffer(CKDWORD VB, CKDWORD StartVertex, CKDWORD VertexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
    virtual BOOL UnlockVertexBuffer(CKDWORD VB);
    //----- Fills a vertex buffer either by locking and filling data or by using a DrawPrimitiveStruct
    virtual BOOL OptimizeVertexBuffer(CKDWORD VB);

    //--- For web content the render context can be transparent (no clear of backbuffer but instead
    //--- a copy of what is currently on screen)
    virtual void SetTransparentMode(BOOL Trans);
    virtual void RestoreScreenBackup();

    virtual void *GetImplementationSpecificData() { return &m_DirectXData; }

    //--- Utils
    virtual int CopyToMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, VxImageDescEx &img_desc);
    virtual int CopyFromMemoryBuffer(CKRECT *rect, VXBUFFER_TYPE buffer, const VxImageDescEx &img_desc);

    //-- Sets the rendering to occur on a texture (reset the texture format to match )
    virtual BOOL SetTargetTexture(CKDWORD TextureObject, int Width = 0, int Height = 0, CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS, BOOL GenerateMipMap = FALSE);

    virtual BOOL SetUserClipPlane(CKDWORD ClipPlaneIndex, const VxPlane &PlaneEquation);
    virtual BOOL GetUserClipPlane(CKDWORD ClipPlaneIndex, VxPlane &PlaneEquation);

    //---------------------------------------------------------------------
    //---- New methods to lock video memory (DX only)
    virtual BOOL LockTextureVideoMemory(CKDWORD Texture, VxImageDescEx &Desc, int MipLevel = 0, VX_LOCKFLAGS Flags = VX_LOCK_DEFAULT);
    virtual BOOL UnlockTextureVideoMemory(CKDWORD Texture, int MipLevel = 0);

    void EnsureVBBufferNotInUse(CKVertexBufferDesc *desc);

    virtual float GetSurfacesVideoMemoryOccupation(int *NbTextures, int *NbSprites, float *TextureSize, float *SpriteSize);

public:
    //-- Updates the  m_DirectXData according to the created DirectX interfaces
    void UpdateDirectXData();
    CKDWORD WaitForSurfaceAvailable();

#ifdef _NOD3DX
    //------ DX Object checks
    BOOL CheckDD();
    BOOL CheckPrimarySurface(BOOL FullScreen, int Width, int Height, int Bpp, int RefreshRate);
    BOOL RestoreDisplayMode();
    BOOL CreateBackBuffer();
    BOOL CreateZBuffer(int Zbpp);
    BOOL LoadSurface(BOOL CompressedFormat, LPDIRECTDRAWSURFACE7 TexSurface, const VxImageDescEx &SurfDesc);
#endif
    void Destroy(BOOL DestroyAll = TRUE);

    BOOL CreateTexture(CKDWORD Texture, CKTextureDesc *DesiredFormat);
    BOOL CreateVertexBuffer(CKDWORD VB, CKVertexBufferDesc *DesiredFormat);

    //----
    void ReleaseScreenBackup()
    {
        C_SECTION();
        if (m_ScreenBackup)
            m_ScreenBackup->Release();
        m_ScreenBackup = NULL;
    }

    void ReleaseTempZBuffers()
    {
        C_SECTION();
        for (int i = 0; i < 256; ++i)
        {
            SAFERELEASE(m_TempZBuffers[i]);
        }
    }

    LPDIRECTDRAWSURFACE7 GetTempZBuffer(int Width, int Height);

public:
//--- DirectX Data
#ifdef _NOD3DX
    LPDIRECTDRAW7 m_DD7;
    LPDIRECTDRAWSURFACE7 m_PrimarySurface;
    LPDIRECTDRAWSURFACE7 m_D3DBackBuffer;
    LPDIRECTDRAWSURFACE7 m_D3DZBuffer;
    LPDIRECTDRAWCLIPPER m_DDClipper;
    HWND m_BackupParent;
    RECT m_BackupWindowRect;
#else
    LPD3DXCONTEXT m_D3DXCtx; // D3DX Context
#endif
    LPDIRECT3DDEVICE7 m_Device7; //
    LPDIRECT3D7 m_D3D7;

    VxDirectXData m_DirectXData; // Direct X specific data
    volatile BOOL m_InCreateDestroy;

    //---- Render Target
    LPDIRECTDRAWSURFACE7 m_DefaultBackBuffer; // Backup pointer of previous back buffer

    //--- For web content the render context can be transparent (no clear of backbuffer but instead
    //--- a copy of what is currently on screen)
    LPDIRECTDRAWSURFACE7 m_ScreenBackup;

    //-----------------------------------------------------
    // + To do texture rendering, Z-buffers are created when
    // needed for any given size (power of two)
    // These Z buffers are stored in the rasterizer context
    // TempZbuffers array and are attached when doing
    // texture rendering
    LPDIRECTDRAWSURFACE7 m_TempZBuffers[256];
};

/*****************************************************************
 CKDX7RasterizerDriver overload
******************************************************************/
class CKDX7RasterizerDriver : public CKRasterizerDriver
{
public:
    CKDX7RasterizerDriver(CKDX7Rasterizer *Rst);
    virtual ~CKDX7RasterizerDriver();

    //--- Contexts
    virtual CKRasterizerContext *CreateContext();

#ifdef _NOD3DX
    //--- Initilisation
    BOOL InitializeCaps(D3DEnumInfo *rst);
    BOOL EnumerateTextures(D3DEnumInfo *d3ddesc);
#else
    BOOL InitializeCaps(CKDX7Rasterizer *rst);
#endif

    BOOL InitializeCommonCaps(D3DDEVICEDESC7 &DeviceDesc, DDCAPS &HalCaps, HMONITOR Monitor);

public:
    BOOL m_Inited;
    HMONITOR m_Monitor;
    BOOL m_OnPrimary;

#ifdef _NOD3DX
    // Available ZBuffer formats
    XArray<DDPIXELFORMAT> m_ZBufferFormats;

    // To Create DD Objects
    GUID *m_DriverGUIDPtr;
    GUID m_DriverGUID;
#else
    CKDWORD m_DxDeviceIndex;
#endif
    BOOL m_DXTFormatEnumerated; // DXT1 etc, available ?
    BOOL m_IsHTL;				// Transfom & Lighting available ?
};

/*****************************************************************
 CKDX7Rasterizer overload
******************************************************************/
class CKDX7Rasterizer : public CKRasterizer
{
public:
    CKDX7Rasterizer();
    virtual ~CKDX7Rasterizer();

    virtual BOOL Start(WIN_HANDLE AppWnd);
    virtual void Close(void);

public:
    BOOL m_Init;

    void InitBlendStages();
    // Stage Blend
    CKStageBlend *m_BlendStages[256];
    int m_MonitorCount;
};

#endif