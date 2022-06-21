/*************************************************************************/
/*	File : CKRasterizerEnums.h											 */
/*	Author : Romain Sididris											 */	
/*																		 */	
/*	Common enumerations definitions for rasterizers						 */
/*																		 */
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/

#define INIT_OBJECTSLOTS 512

#define DEFAULT_VB_SIZE	 4096UL
#define RST_MAX_LIGHT	 128

/****************************************************************************
// ComputeBoxVisibility possible results
******************************************************************************/
#define CBV_OFFSCREEN 0		//  Box is entirely outside the viewing frustum
#define CBV_VISIBLE	  1		//  Box is partially inside the viewing frustum
#define CBV_ALLINSIDE 2		//  Box is entirely inside the viewing frustum

/******************************************************************************
//--- CKRasterizerContext::Clear Flags (equal to CK_RENDER_FLAGS in VxDefines.h to avoir conversion
*******************************************************************************/
typedef enum CKRST_CTXCLEAR_FLAGS {
	CKRST_CTXCLEAR_DEPTH	= 0x00000010,	// Clear Z buffer
	CKRST_CTXCLEAR_COLOR	= 0x00000020,	// Clear Color buffer
	CKRST_CTXCLEAR_STENCIL	= 0x00000040,	// Clear Stencil buffer
	CKRST_CTXCLEAR_VIEWPORT	= 0x00000100,	// Clear only viewport (otherwise clear entire context)
	CKRST_CTXCLEAR_ALL		= 0xFFFFFFFF,	
} CKRST_CTXCLEAR_FLAGS;

/******************************************************************************
//--- CKRasterizerContext::SetDrawBuffer Flags (for stereo rendering)
*******************************************************************************/
typedef enum CKRST_DRAWBUFFER_FLAGS {
	CKRST_DRAWBOTH	= 0x0000000,	// rendering is done to all back buffers.
	CKRST_DRAWLEFT	= 0x0000001,	// All rendering is done on the left render buffer
	CKRST_DRAWRIGHT	= 0x0000002,	// All rendering is done on the right render buffer
} CKRST_DRAWBUFFER_FLAGS;

/******************************************************************************
// CKRasterizerContext::SetTransformationMatrix flags (type of matrix)
*******************************************************************************/
typedef enum VXMATRIX_TYPE
{
	VXMATRIX_WORLD		= 1,		// World Transformation Matrix
	VXMATRIX_VIEW		= 2,		// View Transformaton Matrix
	VXMATRIX_PROJECTION = 3,		// Projection Transformaton Matrix
	VXMATRIX_TEXTURE0	= 16,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE1	= 17,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE2	= 18,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE3	= 19,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE4	= 20,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE5	= 21,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE6	= 22,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_TEXTURE7	= 23,		// Texture Stage 0 Transformaton Matrix
	VXMATRIX_WMAT		= 256,		// First world matrix for vertex blend modes
} VXMATRIX_TYPE;

#define VXMATRIX_TEXTURE(stage) (VXMATRIX_TYPE)(VXMATRIX_TEXTURE0 + stage)
#define VXMATRIX_WORLDMATRIX(index) (VXMATRIX_TYPE)(VXMATRIX_WMAT + index)

// Current state of the combined transformation matrices
#define MATRIX_TOTAL_UPTODATE	 1
#define MATRIX_VIEWPROJ_UPTODATE 2

/******************************************************************************
When enabling a user defined clipping plane , index of the clip plane being set
*******************************************************************************/
#define CKRST_CLIPPLANE(i) (1 << i) 

/****************************************************************
Index of a texture in a Cube Map texture
*****************************************************************/
typedef enum CKRST_CUBEFACE
{
	CKRST_CUBEFACE_XPOS		= 0,
	CKRST_CUBEFACE_XNEG		= 1,
	CKRST_CUBEFACE_YPOS		= 2,
	CKRST_CUBEFACE_YNEG		= 3,
	CKRST_CUBEFACE_ZPOS		= 4,
	CKRST_CUBEFACE_ZNEG		= 5
} CKRST_CUBEFACE;

/*************************************************************************************
Texture Flags (pixel format, hints, special)	
*************************************************************************************/
typedef enum CKRST_TEXTUREFLAGS {
	CKRST_TEXTURE_VALID					= 0x00000001,
	CKRST_TEXTURE_COMPRESSION			= 0x00000004,	// Compressed (DXTn ) texture
	CKRST_TEXTURE_MANAGED				= 0x00000080,	// Rasterizer handle memory managment of the texture (system - agp - video)
	CKRST_TEXTURE_HINTPROCEDURAL		= 0x00000100,	// Hint : This texture is likely to be frequently changed and loaded to video memory
	CKRST_TEXTURE_HINTSTATIC			= 0x00000200,	// Hint : This texture is likely to stay the same for a long time
	CKRST_TEXTURE_SPRITE				= 0x00000400,	// Not a texture => sprite (not pow2 conditionnal)
	
	CKRST_TEXTURE_RGB					= 0x00000800,	// Color Texture 
	CKRST_TEXTURE_ALPHA					= 0x00001000,	// Alpha information

	CKRST_TEXTURE_CUBEMAP				= 0x00002000,	// Cube map texture 
	CKRST_TEXTURE_BUMPDUDV				= 0x00004000,	// Bump map texture 
	CKRST_TEXTURE_FORCEPOW2				= 0x00008000,	
	CKRST_TEXTURE_HINTCOLORKEY			= 0x00010000,	
	CKRST_TEXTURE_HINTALPHAONE			= 0x00020000,	
	CKRST_TEXTURE_RLESPRITE				= 0x00040000,	
	CKRST_TEXTURE_SURFATTACHED			= 0x00080000,	
	
	CKRST_TEXTURE_VOLUMEMAP				= 0x00100000,	// Volume map texture 
	CKRST_TEXTURE_CONDITIONALNONPOW2	= 0x00200000,	// Do we try to create a non power of 2 conditionnal texture ?
	
	CKRST_TEXTURE_USERDEFINEDMIPMAPS	= 0x00400000,	// Mipmaps are defined manually by the user (don't use hardware mipmap)
	
	CKRST_TEXTURE_RENDERTARGET			= 0x10000000	// This texture is used as render target, format is forced by its render context 
	
} CKRST_TEXTUREFLAGS;

/****************************************************************
Types of objects a rasterizer can create
*****************************************************************/
typedef enum CKRST_OBJECTTYPE
{
	CKRST_OBJ_TEXTURE		= 0x01,
	CKRST_OBJ_SPRITE		= 0x02,
	CKRST_OBJ_VERTEXBUFFER	= 0x04,
	CKRST_OBJ_INDEXBUFFER	= 0x08,
	CKRST_OBJ_VERTEXSHADER	= 0x10,
	CKRST_OBJ_PIXELSHADER	= 0x20,
	CKRST_OBJ_SHAREDVB		= 0x40,	// Shared Vertex Buffer => several objects pointing to the same underlying (hardware) object
	CKRST_OBJ_SHAREDIB		= 0x80,	// Shared Index Buffer => several objects pointing to the same underlying (hardware) object
	CKRST_OBJ_VBIB			= 0xFC,
	CKRST_OBJ_ALL			= 0xFF
} CKRST_OBJECTTYPE;


// same enum than CKRST_OBJECTTYPE but giving index for each type of objects
// instead of flags...
enum CKRST_OBJECTINDEX
{
	eTEXTURE	  =0,
	eSPRITE			,
	eVERTEXBUFFER	,
	eINDEXBUFFER	,
	eVERTEXSHADER	,
	ePIXELSHADER	,
	eSHAREDVB		,
	eSHAREDIB		,
	eOBJECTCOUNT
};


/******************************************************************
For vertex buffer, Vertex format description
Data inside the vertex buffer is organised in the same order
as in this enumeration...
*******************************************************************/
#define CKRST_VF_TEXSHIFT  8
#define CKRST_VF_TEXCOUNT(count)	((count) << CKRST_VF_TEXSHIFT) 
#define CKRST_VF_GETTEXCOUNT(vf) (((vf) &  CKRST_VF_TEXMASK) >> CKRST_VF_TEXSHIFT) 
#define CKRST_VF_WCOUNT(count)		(CKRST_VF_POSITION1W + ((count)-1)*2)

// convert a CKRST_VF_POSITION1W .. CKRST_VF_POSITION5W to 1..5
#define CKRST_VF_GETWCOUNT(Flags)	((((int)((Flags) & CKRST_VF_POSITIONMASK)-4) > 0) ? (((int)((Flags) & CKRST_VF_POSITIONMASK)-4)>>1) : 0)

//--- Specify the number of float taken by a texture coordinate (default : 2 )
// eg : 
#define CKRST_VF_TEXFORMAT1 3
#define CKRST_VF_TEXFORMAT2 0
#define CKRST_VF_TEXFORMAT3 1
#define CKRST_VF_TEXFORMAT4 2

#define CKRST_VF_TEXSIZE_F1(CoordIndex)		(CKRST_VF_TEXFORMAT1 << ((CoordIndex)*2 + 16))
#define CKRST_VF_TEXSIZE_F2(CoordIndex)		(CKRST_VF_TEXFORMAT2)
#define CKRST_VF_TEXSIZE_F3(CoordIndex)		(CKRST_VF_TEXFORMAT3 << ((CoordIndex)*2 + 16))
#define CKRST_VF_TEXSIZE_F4(CoordIndex)		(CKRST_VF_TEXFORMAT4 << ((CoordIndex)*2 + 16))


//Alias of DirectX vertex format flags 
typedef enum CKRST_VERTEXFORMAT
{
	CKRST_VF_POSITION         =0x0002,	// Position				(X,Y,Z)
	CKRST_VF_RASTERPOS        =0x0004,	// Transformed Position (X,Y,Z,W) (exclusive with Position)
	CKRST_VF_POSITION1W       =0x0006,	// Position	(X,Y,Z) + 1 weight
	CKRST_VF_POSITION2W       =0x0008,	// Position	(X,Y,Z) + 2 weights
	CKRST_VF_POSITION3W       =0x000a,	// Position	(X,Y,Z) + 3 weights
	CKRST_VF_POSITION4W       =0x000c,	// Position	(X,Y,Z) + 4 weights
	CKRST_VF_POSITION5W       =0x000e,	// Position	(X,Y,Z) + 5 weights
	CKRST_VF_POSITIONMASK	  =0x000F,	// Mask for position flags  
	CKRST_VF_PSIZE            =0x0020,	// Point Size for point sprite
	CKRST_VF_NORMAL           =0x0010,	// Normal (X,Y,Z) (exclusive with Diffuse and Specular)
	CKRST_VF_DIFFUSE          =0x0040,	// Diffuse Color (CKDWORD ARGB) (exclusive with Normal)
	CKRST_VF_SPECULAR         =0x0080,	// Specular Color (CKDWORD ARGB) (exclusive with Normal)
	CKRST_VF_TEX1             =0x0100,	// 1 Texture coords (default 2 floats, can be less or more with TEXSIZE )
	CKRST_VF_TEX2             =0x0200,	// 
	CKRST_VF_TEX3             =0x0300,	// 
	CKRST_VF_TEX4             =0x0400,	// 
	CKRST_VF_TEX5             =0x0500,	// 
	CKRST_VF_TEX6             =0x0600,	// 
	CKRST_VF_TEX7             =0x0700,	// 
	CKRST_VF_TEX8             =0x0800,	// 8 Texture coords : 
	CKRST_VF_TEXMASK          =0x0F00,	// 
	CKRST_VF_MATRIXPAL		  =0x1000,	// When giving vertex weight last float is a dword 
										// containing matrix indices sotred as 4 bytes
	CKRST_VF_POSITIONW		  =0x4002,	// Position				(X,Y,Z,W) 								

	CKRST_VF_VERTEX           =0x0112,	// Standard Vertex with 1 set of texture coords (2 floats)
	CKRST_VF_LVERTEX          =0x01C2,	// Standard Pre Lit Vertex "  "  "   "
	CKRST_VF_TLVERTEX         =0x01C4,	// Standard Pre Transform and Lit Vertex "  "   "  "
		
	CKRST_VF_TEX0_1FLOAT	 = 0x00030000,	// texture coordinates 0 use 1 float	
	CKRST_VF_TEX0_2FLOAT	 = 0x00000000, 	// texture coordinates 0 use 2 floats (default)
	CKRST_VF_TEX0_3FLOAT	 = 0x00010000, 	// texture coordinates 0 use 3 floats
	CKRST_VF_TEX0_4FLOAT	 = 0x00020000, 	// texture coordinates 0 use 4 floats

	//---- etc.... use CKRST_VF_TEXSIZE_Fn to setthe size of any stage of texture...

} CKRST_VERTEXFORMAT;



/*********************************************
*********************************************/
typedef	enum CKRST_MATMASK {
	WORLD_TRANSFORM		= 1<<0,
	VIEW_TRANSFORM		= 1<<1,
	PROJ_TRANSFORM		= 1<<2,
	TEXTURE0_TRANSFORM	= 1<<3,
	TEXTURE1_TRANSFORM	= 1<<4,
	TEXTURE2_TRANSFORM	= 1<<5,
	TEXTURE3_TRANSFORM	= 1<<6,
	TEXTURE4_TRANSFORM	= 1<<7,
	TEXTURE5_TRANSFORM	= 1<<8,
	TEXTURE6_TRANSFORM	= 1<<9,
	TEXTURE7_TRANSFORM	= 1<<10,
} CKRST_MATMASK;

/******************************************************************
//--- Vertex or Index Buffer flags
*******************************************************************/
typedef enum CKRST_VBFLAGS {
	CKRST_VB_VALID				= 0x00000001,	
	CKRST_VB_WRITEONLY			= 0x00000004,	// Hint : Vertex buffer will only be written to (should always be there)
	CKRST_VB_DYNAMIC			= 0x00000008,	// Hint : Dynamic vertex buffer...	
	CKRST_VB_SHARED				= 0x00000010,	// This vertex buffer is being used to hold other shared vertex buffers
} CKRST_VBFLAGS;

/*****************************************************************
When locking a vertex buffer to write new data : behavior
******************************************************************/
typedef enum CKRST_LOCKFLAGS {
	CKRST_LOCK_DEFAULT		= 0x00000000,   // No assumption
	CKRST_LOCK_NOOVERWRITE	= 0x00000001,	// Write operation will not overwrite any vertex used in a pending drawing operation.
	CKRST_LOCK_DISCARD		= 0x00000002,	// No need for the previous memory
} CKRST_LOCKFLAGS;


#ifdef _XBOX
#include <XGraphics.h>

#if (_XBOX_VER>=200)
	inline BOOL VxIsSwizzledFormat(D3DFORMAT fmt){
		return XGIsTiledFormat(fmt) || XGIsCompressedFormat(fmt);
	}	
#else
	inline BOOL VxIsSwizzledFormat(D3DFORMAT fmt){
		return XGIsSwizzledFormat(fmt) 
			|| (fmt ==  D3DFMT_DXT1) || (fmt ==  D3DFMT_DXT3) || (fmt ==  D3DFMT_DXT5);
	}
#endif
inline BOOL VxIsSignedFormat(D3DFORMAT fmt){

	return	D3DFMT_V8U8			== fmt       
			|| D3DFMT_LIN_V8U8			== fmt;	
}
#endif // _XBOX







