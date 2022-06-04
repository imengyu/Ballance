/*************************************************************************/
/*	File : CKRasterizerTypes.h											 */
/*	Author : Romain Sididris											 */
/*																		 */
/*	Common types definitions for rasterizers							 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */
/*************************************************************************/
class CKRasterizerDriver;
class CKRasterizerContext;
class CKRasterizer;

#include "VxDefines.h"
#include "CKTypes.h"

/**********************************************************
 Typedef for starting and ending function of a rasterizer
  These functions usually create and destroy a CKRasterizer instance
***********************************************************/
typedef CKRasterizer *(*CKRST_STARTFUNCTION)(WIN_HANDLE, VxMutex *);
typedef void (*CKRST_CLOSEFUNCTION)(CKRasterizer *);
typedef void (*CKRST_OPTIONFCT)(CKDWORD);

/**********************************************************
Typedef for event watching function, should be set externally
on the rasterizer object after creating it
***********************************************************/
typedef void (*CKRST_EVENTFUNCTION)(CKRasterizerContext *ctx, CKRST_EVENTS event, void *arg);

/**************************************************
 To enable changes in the list of cards
 that have identified driver problems , this structure
 holds information about problematic drivers.
 They are created at load time by reading
 the CKGLRasterizer.ini file present in the render engines directory
 for the OpenGL rasterizer
***************************************************/
typedef struct CKDriverProblems
{
    //--- Driver identification
    XString m_Vendor;
    XString m_Renderer;
    XString m_DeviceDesc;
    XString m_Version;

    //--- Special constraints (Os,Version,Bpp)
    // If none are set the bug is always there
    CKBOOL m_VersionMustBeExact;
    XArray<VX_OSINFO> m_ConcernedOS; // List of OS on which the bug appears..
    CKBOOL m_OnlyIn16;				 // Bug present only in 16 bpp display mode
    CKBOOL m_OnlyIn32;				 // Bug present only in 32 bpp display mode

    //--- Driver Bugs
    int m_RealMaxTextureWidth;						// Max texture width and height
    int m_RealMaxTextureHeight;						//
    CKBOOL m_ClampToEdgeBug;						// Driver propose the "ClampToEdge" extension without really supporting it...
    XArray<VX_PIXELFORMAT> m_TextureFormatsRGBABug; // Some pixel format in OpenGL require a swap of the R & B components
    CKDriverProblems()
    {
        m_ClampToEdgeBug = FALSE;
        m_VersionMustBeExact = FALSE;
        m_OnlyIn16 = FALSE;
        m_OnlyIn32 = FALSE;
        m_RealMaxTextureWidth = 0;
        m_RealMaxTextureHeight = 0;
    }
} CKDriverProblems;

/***********************************************************
 This structure must map the VxStats structure
*********************************************************/
typedef struct CKRasterizerStats
{
    int NbTrianglesDrawn;	 // Number of triangle primitives sent to rasterizer during one frame.
    int NbPointsDrawn;		 // Number of points primitives sent to rasterizer during one frame.
    int NbLinesDrawn;		 // Number of lines primitives sent to rasterizer during one frame.
    int NbVerticesProcessed; // Number of vertices transformed during one frame
} CKRasterizerStats;

/***********************************************************
 Light Structure passed to CKRasterizerContext::SetLight()
*********************************************************/
struct CKLightData
{
    VXLIGHT_TYPE Type;	// Point,Spot,Directionnal
    VxColor Diffuse;	// Diffuse Color
    VxColor Specular;	// Specular Color (Unused...)
    VxColor Ambient;	// Ambient Color (Unused...)
    VxVector Position;	// World Position
    VxVector Direction; // Direction
    float Range;		// Range
    float Falloff;
    float Attenuation0;
    float Attenuation1;
    float Attenuation2;
    float InnerSpotCone;	 // Only for spot lights
    float OuterSpotCone;	 // Only for spot lights
    CKBOOL DX5LightingModel; // TRUE if you want to use the DX5 lighting Model
};

/***********************************************************
//---- Material Structure passed to CKRasterizerContext::SetMaterial()
*********************************************************/
struct CKMaterialData
{
    VxColor Diffuse;
    VxColor Ambient;
    VxColor Specular;
    VxColor Emissive;
    float SpecularPower;
};

/***********************************************************
//---- Viewport Structure passed to CKRasterizerContext::SetViewport()
*********************************************************/
struct CKViewportData
{
    int ViewX;		// Viewport Top-Left Corner
    int ViewY;		//  "		"	"		"
    int ViewWidth;	// Viewport Size
    int ViewHeight; //
    float ViewZMin; // Viewport Z Clipping (should be 0..1)
    float ViewZMax; //
};

struct CKRasterizerObjectDesc
{
    virtual ~CKRasterizerObjectDesc() {}
};

/**************************************************************
//--- Texture Object description structure
***************************************************************/
struct CKTextureDesc : public CKRasterizerObjectDesc
{
    CKDWORD Flags;		  // CKRST_TEXTUREFLAGS
    VxImageDescEx Format; // Width/Height/bpp etc...
    CKDWORD MipMapCount;  // Number of mipmap level in the texture
    CKTextureDesc() : Flags(0), MipMapCount(0) {}
    virtual ~CKTextureDesc() {}
};

/*******************************************************************
Sprites can be implemented as a set of square sub textures
This structure holds the position and size of such a sub texture
*******************************************************************/
struct CKSPRTextInfo
{
    CKDWORD IndexTexture; // Texture object
    short int x;		  //	Position
    short int y;		  //		"
    short int w;		  //	Size in the sprite
    short int h;		  //  "    "
    short int sw;		  //	Real texture size
    short int sh;		  //   "    "
};

/*************************************************************
Sprite description structure
A sprite is in fact a set of pow2 textures...
************************************************************/
struct CKSpriteDesc : public CKTextureDesc
{
    XArray<CKSPRTextInfo> Textures; // Sub textures making this sprite and their placement
    CKRasterizer *Owner;			//
    DWORD ModulateColor;			// New : Sprites that support modulation
    virtual ~CKSpriteDesc();
};

/*************************************************************
Vertex Shader description structure (Empty , only used in DX8)
************************************************************/
struct CKVertexShaderDesc : public CKRasterizerObjectDesc
{
    CKDWORD *m_Function;
    CKDWORD m_FunctionSize;

    CKVertexShaderDesc() : m_Function(NULL) {}
    virtual ~CKVertexShaderDesc() {}
};

/*************************************************************
Pixel Shader description structure (Empty , only used in DX8)
************************************************************/
struct CKPixelShaderDesc : public CKRasterizerObjectDesc
{
    CKDWORD *m_Function;
    CKDWORD m_FunctionSize;

    CKPixelShaderDesc() : m_Function(NULL) {}
    virtual ~CKPixelShaderDesc() {}
};

/***********************************************************
//---- Vertex Buffer Description :
************************************************************/
struct CKVertexBufferDesc : CKRasterizerObjectDesc
{
    CKDWORD m_Flags;		  // CKRST_VBFLAGS
    CKDWORD m_VertexFormat;	  // Vertex format : CKRST_VERTEXFORMAT
    CKDWORD m_MaxVertexCount; // Max number of vertices this buffer can contain
    CKDWORD m_VertexSize;	  // Size in bytes taken by a vertex..
    CKDWORD m_CurrentVCount;  // For dynamic buffers, current number of vertices taken in this buffer

    CKVertexBufferDesc()
    {
        m_Flags = m_VertexFormat = m_MaxVertexCount = m_VertexSize = m_CurrentVCount = 0;
    }
    virtual ~CKVertexBufferDesc() {}

    CKVertexBufferDesc &operator=(const CKVertexBufferDesc &b)
    {
        m_Flags = b.m_Flags;
        m_VertexFormat = b.m_VertexFormat;
        m_MaxVertexCount = b.m_MaxVertexCount;
        m_VertexSize = b.m_VertexSize;
        m_CurrentVCount = b.m_CurrentVCount;
        return *this;
    }
};

/***********************************************************
//---- Index Buffer Description :
************************************************************/
struct CKIndexBufferDesc : CKRasterizerObjectDesc
{
    CKDWORD m_Flags;		 // CKRST_VBFLAGS
    CKDWORD m_MaxIndexCount; // Max number of indices this buffer can contain
    CKDWORD m_CurrentICount; // For dynamic buffers, current number of indices taken in this buffer

    CKIndexBufferDesc()
    {
        m_Flags = m_MaxIndexCount = m_CurrentICount = 0;
    }
    virtual ~CKIndexBufferDesc() {}

    CKIndexBufferDesc &operator=(const CKIndexBufferDesc &b)
    {
        m_Flags = b.m_Flags;
        m_MaxIndexCount = b.m_MaxIndexCount;
        m_CurrentICount = b.m_CurrentICount;
        return *this;
    }
};

/*************************************************
Shared Vertex Buffers (Shared index buffers) use
this class to allocate inside a common  big Vertex Buffer
This class does not perform any collating
*************************************************/
class PoolAllocator
{
public:
    struct FreeCell
    {
        FreeCell() : Next(NULL) {}
        CKDWORD Offset;
        CKDWORD Size;
        FreeCell *Next;
        // IMPLEMENT_POOL_CLASS(FreeCell)
    };

    PoolAllocator(CKDWORD MaxSize) : m_MaxSize(MaxSize)
    {
        m_FreeCells = new FreeCell;
        m_FreeCells->Offset = 0;
        m_FreeCells->Size = MaxSize;
        m_FreeSize = MaxSize;
        m_MaxContigous = MaxSize;
    }
    ~PoolAllocator()
    {
        while (m_FreeCells)
        {
            FreeCell *c = m_FreeCells->Next;
            delete m_FreeCells;
            m_FreeCells = c;
        }
    }
    //---------------------------------------
    // Find a cell for an allocation of size...
    FreeCell *FindAndRemoveFreeUnit(CKDWORD size)
    {
        if (!m_FreeCells)
            return NULL;
        FreeCell *c = m_FreeCells;
        FreeCell *prev = NULL;
        while (c && (c->Size < size))
        {
            prev = c;
            c = c->Next;
        }
        // Not Found
        if (!c)
        {
            //			Check();
            return NULL;
        }
        if (!c->Next)
        {
            // we are removing the largest block
            // the max contigoud block must be updated...
            if (prev)
                m_MaxContigous = prev->Size;
            else
                m_MaxContigous = 0;
        }
        // Found remove it
        if (prev)
            prev->Next = c->Next;
        else
            m_FreeCells = c->Next;
        c->Next = NULL;
        m_FreeSize -= c->Size;

        //		Check();
        return c;
    }
    //---------------------------------------
    // Insert a new cell in the list of free ranges
    // (list is sorted from smallest to largest)
    void InsertFreeUnit(FreeCell *cell)
    {
        if (!m_FreeCells)
        {
            m_FreeCells = cell;
            m_MaxContigous = cell->Size;
            m_FreeSize = cell->Size;
        }
        else
        {
            // New code, with collating
            FreeCell *c = m_FreeCells;
            FreeCell *prev = NULL;
            CKDWORD cellEnd = cell->Offset + cell->Size;
            while (c)
            {
                if ((c->Offset == cellEnd) || (cell->Offset == (c->Offset + c->Size)))
                {
                    // This cell is collatable with the new one
                    // -> update it
                    c->Offset = XMin(c->Offset, cell->Offset);

                    // sub the old size cause the cell is gonna be reinserted
                    m_FreeSize -= c->Size;

                    c->Size += cell->Size;

                    // remove the cell
                    if (prev)
                        prev->Next = c->Next;
                    else
                        m_FreeCells = c->Next;
                    c->Next = 0;

                    // delete the new one
                    delete cell;

                    // and reinsert it
                    InsertFreeUnit(c);
                    return;
                }
                prev = c;
                c = c->Next;
            }

            c = m_FreeCells;
            prev = NULL;
            while (c && (c->Size < cell->Size))
            {
                prev = c;
                c = c->Next;
            }
            cell->Next = c;
            if (prev)
            {
                prev->Next = cell;
            }
            else
            {
                m_FreeCells = cell;
            }
            m_FreeSize += cell->Size;
            m_MaxContigous = XMax((CKDWORD)cell->Size, m_MaxContigous);
        }
        //		Check();
    }
    /*	void Check() {
            // verify Freesize and contigous
            CKDWORD FreeSize = 0;
            CKDWORD MaxContigous = 0;
            if (!m_FreeCells) {
                FreeSize = 0;
                MaxContigous = 0;
            } else {
                FreeCell* c	= m_FreeCells;
                while (c) {
                    if (c->Size > MaxContigous) MaxContigous = c->Size;
                    FreeSize += c->Size;
                    c = c->Next;
                }
            }
            XASSERT(MaxContigous == m_MaxContigous);
            XASSERT(FreeSize == m_FreeSize);
        }
    */
    void ClearAllCells()
    {
        if (!m_FreeCells)
            m_FreeCells = new FreeCell;
        else
            while (m_FreeCells->Next)
            {
                FreeCell *c = m_FreeCells->Next;
                delete m_FreeCells;
                m_FreeCells = c;
            }
        m_FreeCells->Next = NULL;
        m_FreeCells->Offset = 0;
        m_FreeCells->Size = m_MaxSize;
        m_FreeSize = m_MaxSize;
        m_MaxContigous = m_MaxSize;
    }
    void RemoveShared(CKDWORD offset, CKDWORD size)
    {
        FreeCell *cell = new FreeCell;
        cell->Offset = offset;
        cell->Size = size;
        InsertFreeUnit(cell);
    }
    FreeCell *m_FreeCells;	// List of free ranges for allocation
    CKDWORD m_FreeSize;		// Total free size....
    CKDWORD m_MaxSize;		// Total size....
    CKDWORD m_MaxContigous; // Maximum contigous block....
};

struct CKSharedIBDesc;
struct CKSharedVBDesc;

/******************************************
*******************************************/
struct SharedIBsHolder : public PoolAllocator
{
    SharedIBsHolder(CKRasterizerContext *ctx, CKSharedIBDesc *DesiredFormat);
    ~SharedIBsHolder();
    void CreateShared(CKSharedIBDesc *desc);
    void RemoveShared(CKDWORD offset, CKDWORD size);
    // If the pool becomes too fragmented we
    // will force all the shared IB to be recreated...
    // by cleaning the pool. This can happen during an allocation (CreateShared)
    void FlushContent();
    CKDWORD IB; // Index buffer that is holding the data....
    CKRasterizerContext *RstContext;
};

struct SharedVBsHolder : public PoolAllocator
{
    SharedVBsHolder(CKRasterizerContext *ctx, CKSharedVBDesc *DesiredFormat);
    ~SharedVBsHolder();

    void CreateShared(CKSharedVBDesc *desc);
    void RemoveShared(CKDWORD offset, CKDWORD size);
    // If the pool becomes too fragmented we
    // will force all the shared VB to be recreated...
    // by cleaning the pool. This can happen during an allocation (CreateShared)
    void FlushContent();

    CKDWORD VB; // Vertex buffer that is holding the data....
    CKRasterizerContext *RstContext;
};

/***********************************************************
//---- Shared Vertex Buffer Description :
************************************************************/
struct CKSharedVBDesc : CKRasterizerObjectDesc
{
    CKDWORD m_Flags;		// CKRST_VBFLAGS
    CKDWORD m_VertexFormat; // Vertex format : CKRST_VERTEXFORMAT
    CKDWORD m_VertexSize;	// Vertex size
    CKDWORD m_VertexCount;	// Max number of vertices this buffer can contain

    SharedVBsHolder *m_VB; // Internal Data : Vertex Buffer that is really is holding the data...

    CKDWORD m_Offset; // Internal Data : Index of the first vertex inside the vertex buffer

    CKDWORD m_LRF; // Internal Data : Index Last Frame this buffer was sent to render
    CKSharedVBDesc()
    {
        m_VB = 0;
        m_Offset = m_VertexCount = m_VertexFormat = 0;
        m_LRF = -1;
    }
    virtual ~CKSharedVBDesc()
    {
        if (m_VB)
            m_VB->RemoveShared(m_Offset, m_VertexCount);
    }
};

/***********************************************************
//---- Shared Index Buffer Description :
************************************************************/
struct CKSharedIBDesc : CKRasterizerObjectDesc
{
    CKDWORD m_Flags;	   // CKRST_VBFLAGS
    CKDWORD m_IndexCount;  // Max number of indices this buffer can contain
    SharedIBsHolder *m_IB; // Index Buffer that is really is holding the data...

    CKDWORD m_Offset; // Index of the first vertex inside the vertex buffer

    CKDWORD m_LRF; // Internal Data : Index Last Frame this buffer was sent to render

    CKSharedIBDesc()
    {
        m_IB = 0;
        m_Offset = m_IndexCount = 0;
        m_LRF = -1;
    }
    virtual ~CKSharedIBDesc()
    {
        if (m_IB)
            m_IB->RemoveShared(m_Offset, m_IndexCount);
    }
};

//--- Default format of a prelit vertex (Position,Colors,and texture coordinates)
struct CKVertex
{
    VxVector4 V;
    CKDWORD Diffuse;
    CKDWORD Specular;
    float tu, tv;
};

/***************************
Render State cache values
***************************/
enum RSCache
{
    RSC_VALID      = 1,	// Current render state value is valid in cache
    RSC_LOCKED     = 2,	// Render state can not be changed
    RSC_DISABLED   = 4,	// Render state is disabled (if it does not exist in a given implementation )
    RSC_OVERRIDDEN = 8	// Implementation can have special processing todo for a given render state
};

/**************************************************
//--- Used by the render state cache
***************************************************/
struct CKRenderStateData
{
    CKDWORD Value;		  // Current Value for this render state
    CKDWORD DefaultValue; // Default Value for this render state
    CKDWORD Flags;		  // Can value be modified...
};
