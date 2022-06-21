/*************************************************************************/
/*	File: VxDefines.h													 */
/*																		 */	
/*	Render Engine specific data types and enumerations					 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef VXDEFINES_H

#define VXDEFINES_H "$Id:$"

#include "VxMath.h"

#if _MSC_VER > 1000
	#pragma warning( disable : 4251 )
#endif
 
class CKRasterizerContext;	
class CKRasterizer;			
class CKRasterizerDriver;	


/***********************************************
Summary: Texture Coordinates structure. 

Remarks:
	+ Textures coordinates determine which texel (Texture pixel) is assigned to a specific vertex.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}
		
		  class VxUV  {
			  float u;
			  float v;
		  };

{html:</td></tr></table>}  

See Also: CKPatchMesh::GetTVs
************************************************/
class VxUV
{
public:
	float u;	
	float v;	
public:
	VxUV(float _u=0,float _v=0):u(_u),v(_v) {}

	void Set(float _u,float _v) {
		u = _u;
		v = _v;
	}
	
	VxUV& operator += (const VxUV& uv) { u+=uv.u; v+=uv.v; return *this; }
	VxUV& operator -= (const VxUV& uv) { u-=uv.u; v-=uv.v; return *this; }
	VxUV& operator *= (float s) { u*=s; v*=s; return *this; }
	VxUV& operator /= (float s) { u/=s; v/=s; return *this; }

	// =====================================
	// Unary operators
	friend VxUV operator + (const VxUV& uv) { return uv; }
	friend VxUV operator - (const VxUV& uv) { return VxUV(-uv.u,-uv.v); }

	// =====================================
	// Binary operators
	// Addition and subtraction
	friend VxUV operator + (const VxUV& v1, const VxUV& v2) { return VxUV(v1.u+v2.u,v1.v+v2.v); }
	friend VxUV operator - (const VxUV& v1, const VxUV& v2) { return VxUV(v1.u-v2.u,v1.v-v2.v); }
	// Scalar multiplication and division
	friend VxUV operator * (const VxUV& uv, float s) { return VxUV(uv.u*s,uv.v*s); }
	friend VxUV operator * (float s, const VxUV& uv) { return VxUV(uv.u*s,uv.v*s); }
	friend VxUV operator / (const VxUV& uv, float s) { return VxUV(uv.u/s,uv.v/s); }
};


/**************************************************************
Summary: Rasterizer Events .

Remarks:
Rasterizer contexts used as render devices can be destroyed and recreated 
at runtime. 
According to implementation  (DirectX , OpenGL,etc.) some events can occur
such as LostDevice with DirectX that you might need to handle when directly using 
DirectX objects in a manager.
See Also: CKBaseManager::OnRasterizerEvent
****************************************************************/
typedef enum CKRST_EVENTS 
{
	CKRST_EVENT_CREATE	= 0x00000001UL,	// Rasterizer context (device) has just been created
	CKRST_EVENT_DESTROY	= 0x00000002UL,	// Rasterizer context (device) is being destroyed
	CKRST_EVENT_LOST	= 0x00000003UL,	// Device was lost 
	CKRST_EVENT_RESET	= 0x00000004UL,	// Device was reset
	CKRST_EVENT_RESIZING= 0x00000008UL,	// Device is about to be resized
	CKRST_EVENT_RESIZED	= 0x00000010UL,	// Device was resized
} CKRST_EVENTS;


/**************************************************************
Summary: Vertex format and draw primitive options.

Remarks:
 
+ This enumeration is used to specify the format of vertex used when drawing a
a primitive and the actions to perform.

+ It can also be used by the CKRenderContext::GetDrawPrimitiveStructure 
when asking to retrieve a pre-allocated VxDrawPrimitiveData structure. For example
calling RenderContext->GetDrawPrimitiveStructure(CKRST_DP_TR_CL_VNT,20);
returns a VxDrawPrimitiveData structure with a vertex count of 20. Normals and texture coordinates
will be present and when drawing the vertices will be transformed,lit and clipped.

+ If the flags CKRST_DP_VBUFFER is set and the current context supports Vertex Buffers,the 
returned structure will point to a vertex buffer. It  must be released before any rendering through 
the CKRenderContext::ReleaseCurrentVB method.

See Also: VxDrawPrimitiveData,CKRenderContext::DrawPrimitive,CKRenderContext::GetDrawPrimitiveStructure
****************************************************************/
typedef enum CKRST_DPFLAGS 
{
	CKRST_DP_TRANSFORM = 0x00000001UL,	// Transform vertices using the current transformations matrices (see CKRenderContext::SetWorldTransformationMatrix,..)
	CKRST_DP_LIGHT	   = 0x00000002UL,	// Enable lighting and lit vertices if normals information is present.
	CKRST_DP_DOCLIP	   = 0x00000004UL,	// Perfom frustum clipping
	CKRST_DP_DIFFUSE   = 0x00000010UL,	// Diffuse Color
#ifdef PSP
	CKRST_DP_SPECULAR  = 0x00000000UL,	// Specular Color
	CKRST_DP_TANSPACE  = 0x00000000UL,	// Tangent Space info (2x 3dcoords)
#else
	CKRST_DP_SPECULAR  = 0x00000020UL,	// Specular Color
	CKRST_DP_TANSPACE  = 0x00000040UL,	// Tangent Space info (2x 3dcoords)
#endif
	CKRST_DP_STAGESMASK= 0x0001FE00UL,	// Mask for texture coord sets		
	CKRST_DP_STAGES0   = 0x00000200UL,	// Texture coords up to Stage 0 		
	CKRST_DP_STAGES1   = 0x00000400UL,	// Texture coords up to Stage 1 		
	CKRST_DP_STAGES2   = 0x00000800UL,	// ...
	CKRST_DP_STAGES3   = 0x00001000UL,	// ...
	CKRST_DP_STAGES4   = 0x00002000UL,	// ...
	CKRST_DP_STAGES5   = 0x00004000UL,	// ...
	CKRST_DP_STAGES6   = 0x00008000UL,	// ...
	CKRST_DP_STAGES7   = 0x00010000UL,	// Texture coords up to Stage 7 		
	
	CKRST_DP_WEIGHTMASK= 0x01F00000UL,	// Mask for texture coord sets	
	CKRST_DP_WEIGHTS1  = 0x00100000UL,	// 1 Weight data is added to each vertex for HW vertex skinning or blending  		
	CKRST_DP_WEIGHTS2  = 0x00200000UL,	// 2 Weights data are added to each vertex for HW vertex skinning or blending
	CKRST_DP_WEIGHTS3  = 0x00400000UL,	// 3 Weights data are added to each vertex for HW vertex skinning or blending
	CKRST_DP_WEIGHTS4  = 0x00800000UL,	// 4 Weights data are added to each vertex for HW vertex skinning or blending
	CKRST_DP_WEIGHTS5  = 0x01000000UL,	// 5 Weights data are added to each vertex for HW vertex skinning or blending
	CKRST_DP_MATRIXPAL = 0x02000000UL,	// The last weight is a DWORD which contains the indices of matrix to which the weights are associated.
	
	CKRST_DP_VBUFFER   = 0x10000000UL,	// If a Vertex Buffer can be created, the returned structure should directly point to it.
	CKRST_DP_TRANSFORMW = 0x20000000UL,	// Vertex format contains transformed and clipped (x, y, z, w).  This constant is designed for, and can only be used with, the programmable vertex pipeline.

	CKRST_DP_TR_CL_VNT	   = 0x00000207UL,	// non-transformed vertices with normal and texture coords  		
	CKRST_DP_TR_CL_VCST	   = 0x00000235UL,	// non-transformed vertices with colors (diffuse+specular) and texture coords  		
	CKRST_DP_TR_CL_VCT	   = 0x00000215UL,	// non-transformed vertices with diffuse color only and texture coords  		
	CKRST_DP_TR_CL_VCS	   = 0x00000035UL,	// non-transformed vertices with colors (diffuse+specular) and no-texture coords
	CKRST_DP_TR_CL_VC	   = 0x00000015UL,	// non-transformed vertices with diffuse color only.  		
	CKRST_DP_TR_CL_V	   = 0x00000005U,	// non-transformed vertices (white).  		

	CKRST_DP_CL_VCST	   = 0x00000234UL,	// pre-transformed vertices with colors (diffuse+specular) and texture coords  		
	CKRST_DP_CL_VCT		   = 0x00000214UL,	// pre-transformed vertices with diffuse color and texture coords  		
	CKRST_DP_CL_VC		   = 0x00000014UL,	// pre-transformed vertices with diffuse color only. 
	CKRST_DP_CL_V		   = 0x00000004UL,	// pre-transformed vertices (white). 

	CKRST_DP_TR_VNT		   = 0x00000203UL,	// non-transformed vertices with normal and texture coords (No Clipping). 		
	CKRST_DP_TR_VCST	   = 0x00000231UL,	// non-transformed vertices with colors (diffuse+specular) and texture coords (No Clipping).	
	CKRST_DP_TR_VCT		   = 0x00000211UL,	// non-transformed vertices with diffuse color only and texture coords (No Clipping).  		
	CKRST_DP_TR_VCS		   = 0x00000031UL,	// non-transformed vertices with colors (diffuse+specular) and no-texture coords (No Clipping).
	CKRST_DP_TR_VC		   = 0x00000011UL,	// non-transformed vertices with colors (diffuse only) (No Clipping).
	CKRST_DP_TR_V		   = 0x00000001UL,	// non-transformed vertices (white,No Clipping).
	
	CKRST_DP_V			   = 0x00000000UL,	// pre-transformed vertices (White,No Clipping). 
	CKRST_DP_VC			   = 0x00000010UL,	// pre-transformed vertices with diffuse color only (No Clipping). 
	CKRST_DP_VCT		   = 0x00000210UL,	// pre-transformed vertices with diffuse color and texture coords only (No Clipping). 
	CKRST_DP_VCST		   = 0x00000230UL	// pre-transformed vertices with diffuse and specular color and texture coords only (No Clipping). 
} CKRST_DPFLAGS;

#define CKRST_DP_WEIGHT(x)	(x ? (CKRST_DP_WEIGHTS1 << (x-1)) : 0)	
#define CKRST_DP_IWEIGHT(x)	(x ? (CKRST_DP_MATRIXPAL|(CKRST_DP_WEIGHTS1 << (x-1))) : 0)	
#define CKRST_DP_STAGE(i)	(CKRST_DP_STAGES0 << i)					
#define CKRST_DP_STAGEFLAGS(f)	((f & CKRST_DP_STAGESMASK) >> 9)	


#if defined(_XBOX) && (_XBOX_VER>=200)
	#define CKRST_MAX_STAGES 16											
#elif defined(PSP) 
	#define CKRST_MAX_STAGES 1											
#elif defined(_WIN32) || defined(macintosh)
	#define CKRST_MAX_STAGES 8											
#endif


/**************************************************************
Summary: The VxDrawPrimitiveData is used by CKRenderContext::DrawPrimitive to describe the vertices to render.

Remarks: 

+ If the vertices are already transformed, they should be provided in a VxVector4 form
in order the rasterizer to  have all information about the vertices(x,y,z,rhw) to draw 
(rhw = Reciprocal homogenous w used for texture correction).

+ The flags member precises which vertex data is available (see CKRST_DP_VBUFFER in CKRST_DPFLAGS) 
how vertices should be processed.

+ This structure can also points to a vertex buffer (see CKRST_DPFLAGS

See Also: CKRST_DPFLAGS,CKRenderContext::DrawPrimitive,CKRenderContext::GetDrawPrimitiveStructure
****************************************************************/
struct VxDrawPrimitiveDataSimple {
	int				VertexCount;		// Number of vertices to draw
	unsigned int	Flags;				// CKRST_DPFLAGS
	XPtrStrided<VxVector4>	Positions;
	XPtrStrided<VxVector>	Normals;
	XPtrStrided<DWORD>		Colors;
	XPtrStrided<DWORD>		SpecularColors;
	XPtrStrided<VxUV>		TexCoord;
};


struct VxDrawPrimitiveData : public VxDrawPrimitiveDataSimple
{
#if (CKRST_MAX_STAGES>1)
	XPtrStrided<VxUV>		TexCoords[CKRST_MAX_STAGES-1];
#endif	
	XPtrStrided<void*>		Weights;
	XPtrStrided<DWORD>		MatIndex;
};


/****************************************************************
Summary: Display Mode Description

Remarks:
	+ The VxDisplayMode contains the description of a fullscreen display mode.
See Also: CKRenderManager::GetRenderDriverDescription,CKRenderManager::GetRenderDriverCount,VxDriverDesc
****************************************************************/
typedef struct VxDisplayMode
{
	int Width;			// Width in pixel of the display mode.
	int Height;			// Height in pixel of the display mode.
	int	Bpp;			// Number of bits per pixel.
	int RefreshRate;	// Refresh rate in Hertz.
} VxDisplayMode;


/*****************************************************************
Summary: Vertices to transform to screen coordinates

Remarks

o The VxTransformData structure is used by CKRenderContext::TransformVertices function
to specify vertices to transform and 2 arrays that will received the tranformed homogenous
vertices and the screen coordinates of these vertices.

o All members except InVertices are optionnal.

o The ClipFlags member is an array of DWORD (VXCLIP_FLAGS) containing the clipping flags for each 
vertex: if one of the flags is set the vertex is outside the viewing frustrum.

See Also: CKRenderContext::TransformVertices,VXCLIP_FLAGS
******************************************************************/
typedef struct VxTransformData
{
	void* InVertices;		// VxVector (x,y,z) in model coordinate space
	unsigned int InStride;			// Amount in bytes separating two vertices in InVertices array
	void* OutVertices;		// VxVector4 Homogenous coordinates (xh,yh,zh,wh) 	
	unsigned int  OutStride;		// Amount in bytes separating two vertices in OutVertices array
	void* ScreenVertices;	// VxVector4 Screen coordinates (xs,ys,zs,rhw)	
	unsigned int  ScreenStride;		// Amount in bytes separating two vertices in ScreenVetices array
	unsigned int  *ClipFlags;		// Array of DWORD containing a combination of clipping flags (see VXCLIP_FLAGS )
	CKRECT m_2dExtents;		// Obsolete
	unsigned int  m_Offscreen;		// And combination of all the clipping flags : if !=0 all vertices are clipped by at least one clipping plane.
} VxTransformData;


/******************************************************************
Summary: DirectX specific data.

Remarks:
+ The data stored in the structure are only available when using a DirectX based rasterizer
(CKDX8Rasterizer,CKDX7Rasterizer or CKDX5Rasterizer)
+ The type and version of objects depends on the version of the rasterizer used.

+CKDX9Rasterizer:
		DDBackBuffer	- LPDIRECT3DSURFACE9
		DDPrimaryBuffer	- NULL		
		DDZBuffer		- LPDIRECT3DSURFACE9		
		DirectDraw		- NULL			
		Direct3D		- LPDIRECT3D9			
		DDClipper		- NULL			
		D3DDevice		- LPDIRECT3DDEVICE9
		D3DViewport		- NULL
		DxVersion		- 0x0900

+CKDX8Rasterizer:
		DDBackBuffer	- LPDIRECT3DSURFACE8
		DDPrimaryBuffer	- NULL		
		DDZBuffer		- LPDIRECT3DSURFACE8		
		DirectDraw		- NULL			
		Direct3D		- LPDIRECT3D8			
		DDClipper		- NULL			
		D3DDevice		- LPDIRECT3DDEVICE8
		D3DViewport		- NULL	
		DxVersion		- 0x0801    
  
+CKDX7Rasterizer:
		DDBackBuffer	- LPDIRECTDRAWSURFACE7
		DDPrimaryBuffer	- LPDIRECTDRAWSURFACE7		
		DDZBuffer		- LPDIRECTDRAWSURFACE7		
		DirectDraw		- LPDIRECTDRAW7			
		Direct3D		- LPDIRECT3D7			
		DDClipper		- LPDIRECTDRAWCLIPPER			
		D3DDevice		- LPDIRECT3DDEVICE7
		D3DViewport		- NULL	
		DxVersion		- 0x0700
	
+CKDX5Rasterizer:
		DDBackBuffer	- LPDIRECTDRAWSURFACE3
		DDPrimaryBuffer	- LPDIRECTDRAWSURFACE3		
		DDZBuffer		- LPDIRECTDRAWSURFACE3		
		DirectDraw		- LPDIRECTDRAW2			
		Direct3D		- LPDIRECT3D2			
		DDClipper		- LPDIRECTDRAWCLIPPER			
		D3DDevice		- LPDIRECT3DDEVICE2
		D3DViewport		- LPDIRECT3DVIEWPORT2				
		DxVersion		- 0x0500

See Also: CKRenderContext::GetDirectXInfo
******************************************************************/
typedef struct VxDirectXData {
	void*			DDBackBuffer;		// Pointer to the back buffer surface 
	void*			DDPrimaryBuffer;	// Pointer to the front buffer (primary) surface
	void*			DDZBuffer;			// Pointer to the depth buffer surface 
	void*			DirectDraw;			// Pointer to the IDirectDraw object
	void*			Direct3D;			// Pointer to the IDirect3D object	
	void*			DDClipper;			// Pointer to the clipper object	
	void*			D3DDevice;			// Pointer to the IDirect3DDevice object
	void*			D3DViewport;		// Pointer to the IDirect3DViewport object		
	DWORD			DxVersion;			// DirectX Version (0x0500 for DirectX 5.0 , 0x0700 for DirectX 7, etc..)
} VxDirectXData;



/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/************************ RENDER STATES *************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/


/*****************************************************************
{filename:VX_LOCKFLAGS}
Summary: Lock Flags.

Remarks:
+ When accessing a video memory surface, these flags should be
used to warn the drivers of the intended operation (Write or Read Only)
See also: CKTexture::LockVideoMemory,CKSprite::LockVideoMemory
******************************************************************/
typedef enum VX_LOCKFLAGS {
	VX_LOCK_DEFAULT		= 0x00000000,   // No assumption
	VX_LOCK_WRITEONLY	= 0x00000001,	// Write operation only.
	VX_LOCK_READONLY	= 0x00000002,	// Read operation only
	VX_LOCK_DISCARD		= 0x00000004,	// Write operation only , All current content can be discarded.
} VX_LOCKFLAGS;


typedef enum VX_RESIZE_FLAGS {
	VX_RESIZE_NOMOVE  = 0x0001UL,
	VX_RESIZE_NOSIZE  = 0x0002UL
} VX_RESIZE_FLAGS;


/******************************************************************
{filename:VXLIGHT_TYPE}
Summary: Light type.

Remarks:
	+ Used by CKLight::SetType to specify the type of a light. 
See also: CKLight::SetType,CKLight::GetType
******************************************************************/
typedef enum VXLIGHT_TYPE
{
	VX_LIGHTPOINT =	1UL,	//	The Light is a point of light
	VX_LIGHTSPOT  =	2UL,	//	The light is a spotlight
	VX_LIGHTDIREC =	3UL,	//	The light is directional light : Lights comes from an infinite point so only direction of light can be given
	VX_LIGHTPARA  =	4UL	// 	Obsolete, do not use
} VXLIGHT_TYPE;



/******************************************************************
{filename:VXPRIMITIVETYPE}
Summary: Type of primitive (Triangle,strips,line,points,etc.) to draw. 

Remarks: 
+ Used by CKRenderContext::DrawPrimitive to specify the type of primitive to be drawn. 
See also: CKRenderContext::DrawPrimitive
******************************************************************/
typedef enum VXPRIMITIVETYPE
{
	VX_POINTLIST     = 1UL,		// Draw a list of points, indices are not used to draw the points
    VX_LINELIST		 = 2UL,		// Draw a list of lines, if indices are given there must be a multiple of 2 indices
    VX_LINESTRIP     = 3UL,		// Draw a strip of lines
    VX_TRIANGLELIST  = 4UL,		// Draw a list of triangles, if indices are given there must be a multiple of 3 indices
	VX_TRIANGLESTRIP = 5UL,		// Draw a strip of triangles
	VX_TRIANGLEFAN	 = 6UL		// Draw a fan of triangles
} VXPRIMITIVETYPE;



/****************************************************************
{filename:VXBUFFER_TYPE}
Summary: Video Buffer

Remarks:
	+ The VXBUFFER_TYPE is used by CKRenderContext::DumpToMemory() method to specify which 
	video buffer to copy.
See Also: CKRenderContext::CopyToVideo,CKRenderContext::DumpToMemory.
****************************************************************/
typedef enum VXBUFFER_TYPE
{
	VXBUFFER_BACKBUFFER		= 0x00000001UL,	// Back Buffer
	VXBUFFER_ZBUFFER		= 0x00000002UL,	// Depth Buffer
	VXBUFFER_STENCILBUFFER  = 0x00000004UL	// Stencil Buffer
} VXBUFFER_TYPE;


/*****************************************************************
{filename:VXTEXTURE_BLENDMODE}
Summary: Blend Mode Flags

Remarks:
+ The VXTEXTURE_BLENDMODE is used by CKMaterial::SetTextureBlendMode() to specify how
texture is applied on primitives.
+ Also used as value for CKRST_TSS_TEXTUREMAPBLEND texture stage state. 
See Also: Using Materials,CKMaterial,CKTexture,CKMaterial::SetTextureBlendMode,CKRST_TSS_TEXTUREMAPBLEND.
******************************************************************/
typedef enum VXTEXTURE_BLENDMODE
{
	VXTEXTUREBLEND_DECAL			= 1UL,	// Texture replace any material information
    VXTEXTUREBLEND_MODULATE			= 2UL,	// Texture and material are combine. Alpha information of the texture replace material alpha component.
    VXTEXTUREBLEND_DECALALPHA		= 3UL,	// Alpha information in the texture specify how material and texture are combined. Alpha information of the texture replace material alpha component. 
    VXTEXTUREBLEND_MODULATEALPHA	= 4UL,    // Alpha information in the texture specify how material and texture are combined
    VXTEXTUREBLEND_DECALMASK		= 5UL, 
    VXTEXTUREBLEND_MODULATEMASK		= 6UL, 
    VXTEXTUREBLEND_COPY				= 7UL,	// Equivalent to DECAL
    VXTEXTUREBLEND_ADD				= 8UL,		
	VXTEXTUREBLEND_DOTPRODUCT3		= 9UL,	// Perform a Dot Product 3 between texture (normal map)
											// and a referential vector given in VXRENDERSTATE_TEXTUREFACTOR.
	VXTEXTUREBLEND_MAX				= 10UL,
	VXTEXTUREBLEND_MASK				= 0xFUL
} VXTEXTURE_BLENDMODE; 


/******************************************************************
{filename:VXTEXTURE_FILTERMODE}
Summary: Filter Mode Options

Remarks:
+ The VXTEXTURE_FILTERMODE is used by CKMaterial::SetTextureMagMode and CKMaterial::SetTextureMinMode to specify how
texture is filtered when magnified.
+ Also used as value for CKRST_TSS_MAGFILTER and CKRST_TSS_MINFILTER texture stage state. 
See Also: Using Materials,CKMaterial,CKTexture,CKMaterial::SetTextureMagMode,CKMaterial::SetTextureMinMode,,CKRenderContext::SetTextureStageState
******************************************************************/
typedef enum VXTEXTURE_FILTERMODE
{
	VXTEXTUREFILTER_NEAREST          = 1UL,		// No Filter 	
    VXTEXTUREFILTER_LINEAR           = 2UL,		// Bilinear Interpolation
    VXTEXTUREFILTER_MIPNEAREST       = 3UL,		// Mip mapping 
    VXTEXTUREFILTER_MIPLINEAR        = 4UL,		// Mip Mapping with Bilinear interpolation
    VXTEXTUREFILTER_LINEARMIPNEAREST = 5UL,		// Mip Mapping with Bilinear interpolation between mipmap levels.
    VXTEXTUREFILTER_LINEARMIPLINEAR  = 6UL,		// Trilinear Filtering
	VXTEXTUREFILTER_ANISOTROPIC		 = 7UL,		// Anisotropic filtering 
	VXTEXTUREFILTER_MASK			 = 0xFUL
} VXTEXTURE_FILTERMODE; 

/******************************************************************
{filename:VXBLEND_MODE}
Summary: Blending Mode options

Remarks:
	+ The VXBLEND_MODE is used by CKMaterial::SetSourceBlend() and SetDestBlend() to specify the blend 
factors that are used when blending is enabled. (Rs,Gs,Bs,As) are color components of the source pixel (being drawn) and
(Rd,Gd,Bd,Ad) are color components of the destination pixel (current pixel on screen).
When blending is enabled the final pixel will be equal to : 
		
		  SrcBlendFactor * SrcPixel + DstBlendFactor * CurrentPixelOnScreen

	+ Also used as value for VXRENDERSTATE_SRCBLEND and VXRENDERSTATE_DESTBLEND render state. 

See Also: CKMaterial,CKTexture,CKMaterial::SetSourceBlend,CKMaterial::SetDestBlend,CKRenderContext::SetState,CKSprite::SetBlending,VXRENDERSTATE_SRCBLEND,VXRENDERSTATE_DESTBLEND
******************************************************************/
typedef enum VXBLEND_MODE
{
	VXBLEND_ZERO            = 1UL,		//Blend factor is (0, 0, 0, 0).
    VXBLEND_ONE             = 2UL,		//Blend factor is (1, 1, 1, 1).
    VXBLEND_SRCCOLOR        = 3UL,		//Blend factor is (Rs, Gs, Bs, As).
    VXBLEND_INVSRCCOLOR     = 4UL,		//Blend factor is (1-Rs, 1-Gs, 1-Bs, 1-As).
    VXBLEND_SRCALPHA        = 5UL,		//Blend factor is (As, As, As, As).
    VXBLEND_INVSRCALPHA     = 6UL,		//Blend factor is (1-As, 1-As, 1-As, 1-As).
    VXBLEND_DESTALPHA       = 7UL,		//Blend factor is (Ad, Ad, Ad, Ad).
    VXBLEND_INVDESTALPHA    = 8UL,		//Blend factor is (1-Ad, 1-Ad, 1-Ad, 1-Ad).
    VXBLEND_DESTCOLOR       = 9UL,		//Blend factor is (Rd, Gd, Bd, Ad).
    VXBLEND_INVDESTCOLOR    = 10UL,		//Blend factor is (1-Rd, 1-Gd, 1-Bd, 1-Ad).
    VXBLEND_SRCALPHASAT     = 11UL,		//Blend factor is (f, f, f, 1); f = min(As, 1-Ad).
    VXBLEND_BOTHSRCALPHA    = 12UL,		//Source blend factor is (As, As, As, As) and destination blend factor is (1-As, 1-As, 1-As, 1-As)
	VXBLEND_BOTHINVSRCALPHA = 13UL,		//Source blend factor is (1-As, 1-As, 1-As, 1-As) and destination blend factor is (As, As, As, As)
	VXBLEND_MASK			= 0xFUL		//Source blend factor is (1-As, 1-As, 1-As, 1-As) and destination blend factor is (As, As, As, As)
} VXBLEND_MODE;




/******************************************************************
{filename:VXTEXTURE_ADDRESSMODE}
Summary: Texture addressing modes.

Remarks:
+ The VXTEXTURE_ADDRESSMODE is used by CKMaterial::SetTextureAddresMode to specify texture coordinate are 
taken into account when they are outside the range [0.0 , 1.0].
+ Also used as value for CKRST_TSS_ADDRESS texture stage state. 
See Also: CKMaterial,CKTexture,CKRST_TSS_ADDRESS,CKRenderContext::SetTextureStageState
******************************************************************/
typedef enum VXTEXTURE_ADDRESSMODE
{
	VXTEXTURE_ADDRESSWRAP         = 1UL,	// Default mesh wrap mode is used (see CKMesh::SetWrapMode)
    VXTEXTURE_ADDRESSMIRROR       = 2UL,	// Texture coordinates outside the range [0..1] are flipped evenly.
    VXTEXTURE_ADDRESSCLAMP        = 3UL,	// Texture coordinates greater than 1.0 are set to 1.0, and values less than 0.0 are set to 0.0.
    VXTEXTURE_ADDRESSBORDER       = 4UL,	// When texture coordinates are greater than 1.0 or less than 0.0  texture is set to a color defined in CKMaterial::SetTextureBorderColor.		
	VXTEXTURE_ADDRESSMIRRORONCE   = 5UL,	// 
	VXTEXTURE_ADDRESSMASK         = 0x7UL	// mask for all values
} VXTEXTURE_ADDRESSMODE;


/******************************************************************
{filename:VXFILL_MODE}
Summary:  Fill Mode Options
  
Remarks:
	+ The VXFILL_MODE is used by CKMaterial::SetFillMode to specify how faces are drawn.
	+ Also used as value for VXRENDERSTATE_FILLMODE render state. 
See Also: CKMaterial::SetFillMode,VXRENDERSTATE_FILLMODE
******************************************************************/
typedef enum VXFILL_MODE
{
	VXFILL_POINT			= 1UL,		// Vertices rendering	
    VXFILL_WIREFRAME		= 2UL,		// Edges rendering
	VXFILL_SOLID			= 3UL,		// Face rendering
	VXFILL_MASK				= 3UL
}VXFILL_MODE; 


/******************************************************************
{filename:VXSHADE_MODE}
Summary: Shade Mode Options

Remarks:
+The VXSHADE_MODE is used by CKMaterial::SetShadeMode to specify how color 
interpolation is perform on faces when they are drawn.
+Also used as value for VXRENDERSTATE_SHADEMODE render state. 
See Also: CKMaterial::SetShadeMode,VXRENDERSTATE_SHADEMODE
******************************************************************/
typedef enum VXSHADE_MODE
{
	VXSHADE_FLAT			= 1UL,	// Flat Shading
	VXSHADE_GOURAUD			= 2UL,	// Gouraud Shading
	VXSHADE_PHONG			= 3UL,	// Phong Shading (Not yet supported by most implementation)
	VXSHADE_MASK			= 3UL	
} VXSHADE_MODE; 


/******************************************************************
{filename:VXCULL}
Summary: Backface culling options

Remarks:
	+ Used by CKRenderContext::SetState(VXRENDERSTATE_CULLMODE) to specify the type of backface culling. 
See also: CKRenderContext::SetState,VXRENDERSTATE_CULLMODE
******************************************************************/
typedef enum VXCULL
{
    VXCULL_NONE               = 1UL,	//	No backface culling occurs 
    VXCULL_CW                 = 2UL,	//	Clockwise made faces are rejected when rendering
    VXCULL_CCW                = 3UL,
	VXCULL_MASK               = 3UL
} VXCULL;


/******************************************************************
{filename:VXCMPFUNC}
Summary:  Comparison Function

Remarks:
+ Used by CKRenderContext::SetState with VXRENDERSTATE_ZFUNC, VXRENDERSTATE_ALPHAFUNC or VXRENDERSTATE_STENCILFUNC
to specify the type of Z or Alpha comparison function. 
+ The comparison function is used to compare the stencil,alpha or z reference value to a stencil,z or alpha entry.
See also: CKRenderContext::SetState,VXRENDERSTATETYPE,VXRENDERSTATE_ZFUNC,VXRENDERSTATE_ALPHAFUNC,
******************************************************************/
typedef enum VXCMPFUNC
{
    VXCMP_NEVER               = 1UL,	// Always fail the test.
    VXCMP_LESS                = 2UL,	// Accept if value if less than current value.
    VXCMP_EQUAL               = 3UL,	// Accept if value if equal than current value.
    VXCMP_LESSEQUAL           = 4UL,	// Accept if value if less or equal than current value.
    VXCMP_GREATER             = 5UL,	// Accept if value if greater than current value.
    VXCMP_NOTEQUAL            = 6UL,	// Accept if value if different than current value.
    VXCMP_GREATEREQUAL        = 7UL,	// Accept if value if greater or equal current value.
    VXCMP_ALWAYS              = 8UL,	// Always accept the test.
	VXCMP_MASK	              = 0xFUL	// Mask for all possible values.
} VXCMPFUNC;


/****************************************************************
{filename:VXSPRITE_RENDEROPTIONS}
Summary: Sprite rendering options

Remarks:
See Also: VxSpriteRenderOptions,CKSprite::SetRenderOptions,CKSprite::GetRenderOptions
****************************************************************/
typedef enum VXSPRITE_RENDEROPTIONS
{
	VXSPRITE_NONE			= 0x00000000UL,	// Default sprite rendering : no blending no filtering no modulation
	VXSPRITE_ALPHATEST		= 0x00000001UL,	// Alpha test is enabled, AlphaRefValue & AlphaTestFunc must be filled with correct values in VxSpriteRenderOptions
	VXSPRITE_BLEND			= 0x00000002UL,	// Blending is enabled, SrcBlendMode & DstBlendMode must be filled with correct values in VxSpriteRenderOptions
	VXSPRITE_MODULATE		= 0x00000004UL,	// Modulation is enabled, ModulateColor must be filled with correct values in VxSpriteRenderOptions
	VXSPRITE_FILTER			= 0x00000008UL	// Bi-linear Filtering is enabled.
	
} VXSPRITE_RENDEROPTIONS;

/****************************************************************
{filename:VXSPRITE_RENDEROPTIONS}
Summary: Sprite rendering options

Remarks:
See Also: VxSpriteRenderOptions,CKSprite::SetRenderOptions,CKSprite::GetRenderOptions
****************************************************************/
typedef enum VXSPRITE_RENDEROPTIONS2
{
	VXSPRITE2_NONE			= 0x00000000UL,	// Default sprite rendering : no blending no filtering no modulation
	VXSPRITE2_DISABLE_AA_CORRECTION			= 0x00000001UL// Disable Antialiasing special processing on the sprite (UV offset,...) .
} VXSPRITE_RENDEROPTIONS2;


/****************************************************************
{filename:VxSpriteRenderOptions}
Summary: Sprite rendering options structure

Remarks:
+ The default rendering of a sprite does not perform any blending, filtering or color modulation.
+ As for a CKMaterial some options can be controlled with the CKSprite::SetRenderOptions method
using the VxSpriteRenderOptions structure to describe the special settings
See Also: VXSPRITE_RENDEROPTIONS,CKSprite::SetRenderOptions,CKSprite::GetRenderOptions
****************************************************************/
struct VxSpriteRenderOptions {
	DWORD				   ModulateColor;	// A DWORD ARGB Color to use if VXSPRITE_MODULATE is enabled
											// to multiply the sprite pixel 			
	DWORD				   Options			: 4;	// A combinaison of VXSPRITE_RENDEROPTIONS used to render the sprite 
	VXCMPFUNC			   AlphaTestFunc	: 4;	// if VXSPRITE_ALPHATEST is enabled, the alpha test function.
	VXBLEND_MODE		   SrcBlendMode		: 4;	// If blending is enabled (VXSPRITE_BLEND), source blend mode
	DWORD				   Options2			: 4;	// A combinaison of VXSPRITE_RENDEROPTIONS2 used to render the sprite 
	DWORD				   DstBlendMode		: 8;		// If blending is enabled (VXSPRITE_BLEND), destination blend mode
	DWORD				   AlphaRefValue	: 8;		// If alpha test is enabled (VXSPRITE_ALPHATEST), reference value
};

/******************************************************************
{filename:VXSTENCILOP}
Summary: Stencil Operations

Remarks:
	+ This enumeration describes the stencil operations for 
	the VXRENDERSTATE_STENCILFAIL,VXRENDERSTATE_ZFAIL,VXRENDERSTATE_STENCILPASS render states.
See also: VXRENDERSTATETYPE,CKRenderContext::SetState
******************************************************************/
typedef enum VXSTENCILOP
{
    VXSTENCILOP_KEEP           = 1UL,	// Keep stencil value.
    VXSTENCILOP_ZERO           = 2UL,	// Set stencil value to zero 
    VXSTENCILOP_REPLACE        = 3UL,	// Set stencil value to reference (VXRENDERSTATE_STENCILREF)
    VXSTENCILOP_INCRSAT        = 4UL,	// Increment stencil value ( up to maximum value (2^n-1) where n is the number bits for stencil buffer)
    VXSTENCILOP_DECRSAT        = 5UL,	// Decrement stencil value ( down to 0) 
    VXSTENCILOP_INVERT         = 6UL,	// Invert the bits of the stencil value
    VXSTENCILOP_INCR           = 7UL,	// Increment stencil value (Wrap to 0 when above maximum value)
    VXSTENCILOP_DECR           = 8UL,	// Decrement stencil value.
	VXSTENCILOP_MASK           = 0xFUL	// Mask for all possible values.
} VXSTENCILOP;


/*****************************************************************
{filename:VXFOG_MODE}
Summary: Fog Options
  
Remarks:
	+ Used by CKRenderContext::SetFogMode to specify the type of fog to apply to the scene. 
	+ Also used as value for VXRENDERSTATE_FOGMODE render state. 
See also: CKRenderContext::SetFogMode,VXRENDERSTATE_FOGMODE,CKRenderContext::SetState.
******************************************************************/
typedef enum VXFOG_MODE
{
    VXFOG_NONE                = 0UL,	// No Fog  (Default)
    VXFOG_EXP                 = 1UL,	// Exponential Fog ()
    VXFOG_EXP2                = 2UL,	// Square Exponential Fog ()
    VXFOG_LINEAR              = 3UL		// Linear fog (most likely case )
} VXFOG_MODE;



//--- Texture operation for SetTextureStageState
//--- Kept here for usage with DX7 or DX8
 // 
typedef enum CKRST_TEXTUREOP { 
    CKRST_TOP_DISABLE						= 1UL,  
    CKRST_TOP_SELECTARG1					= 2UL,  
    CKRST_TOP_SELECTARG2					= 3UL,  
    CKRST_TOP_MODULATE						= 4UL,  
    CKRST_TOP_MODULATE2X					= 5UL,  
    CKRST_TOP_MODULATE4X					= 6UL,  
    CKRST_TOP_ADD							= 7UL,  
    CKRST_TOP_ADDSIGNED						= 8UL,  
    CKRST_TOP_ADDSIGNED2X					= 9UL,  
    CKRST_TOP_SUBTRACT						= 10UL,  
    CKRST_TOP_ADDSMOOTH						= 11UL,  
    CKRST_TOP_BLENDDIFFUSEALPHA				= 12UL,  
    CKRST_TOP_BLENDTEXTUREALPHA				= 13UL,  
    CKRST_TOP_BLENDFACTORALPHA				= 14UL,  
    CKRST_TOP_BLENDTEXTUREALPHAPM			= 15UL,  
    CKRST_TOP_BLENDCURRENTALPHA				= 16UL,  
    CKRST_TOP_PREMODULATE					= 17UL,  
    CKRST_TOP_MODULATEALPHA_ADDCOLOR		= 18UL,  
    CKRST_TOP_MODULATECOLOR_ADDALPHA		= 19UL,  
    CKRST_TOP_MODULATEINVALPHA_ADDCOLOR		= 20UL,  
    CKRST_TOP_MODULATEINVCOLOR_ADDALPHA		= 21UL,  
    CKRST_TOP_BUMPENVMAP					= 22UL, 
    CKRST_TOP_BUMPENVMAPLUMINANCE			= 23UL, 
    CKRST_TOP_DOTPRODUCT3					= 24UL,
	CKRST_TOP_MULTIPLYADD					= 25UL,
	CKRST_TOP_LERP							= 26UL
} CKRST_TEXTUREOP;


//--- Color argument for SetTextureStageState 
//--- Kept here for usage with DX7 or DX8
// 
typedef enum CKRST_TEXTUREARG { 
	CKRST_TA_DIFFUSE           =0x00000000UL,  // select diffuse color
	CKRST_TA_CURRENT           =0x00000001UL,  // select result of previous stage
	CKRST_TA_TEXTURE           =0x00000002UL,  // select texture color
	CKRST_TA_TFACTOR           =0x00000003UL,  // select RENDERSTATE_TEXTUREFACTOR
	CKRST_TA_SPECULAR          =0x00000004UL,  // select specular color
	CKRST_TA_TEMP		       =0x00000005UL,  // temp 
	CKRST_TA_COMPLEMENT        =0x00000010UL,  // take 1.0 - x
	CKRST_TA_ALPHAREPLICATE    =0x00000020UL  // replicate alpha to color components
} CKRST_TEXTUREARG;


//--- Transform argument for SetTextureStageState 
//--- Kept here for usage with DX7 or DX8
// 
typedef enum CKRST_TEXTURETRANSFORMFLAGS { 
		CKRST_TTF_NONE           =0x00000000UL,  // Texture coordinates are passed directly to the rasterizer
		CKRST_TTF_COUNT1         =0x00000001UL,  // The rasterizer should expect 1-D texture coordinates
		CKRST_TTF_COUNT2         =0x00000002UL,  // The rasterizer should expect 2-D texture coordinates
		CKRST_TTF_COUNT3         =0x00000003UL,  // The rasterizer should expect 3-D texture coordinates
		CKRST_TTF_COUNT4         =0x00000004UL,  // The rasterizer should expect 4-D texture coordinates
		CKRST_TTF_PROJECTED      =0x00000100UL,  // divide all coordinates by the last one...
		
} CKRST_TEXTURETRANSFORMFLAGS;


#define STAGEBLEND(Src,Dst) ((Src<<4)|Dst)

/*****************************************************************
{filename:CKRST_TEXTURESTAGESTATETYPE}
Summary: Texture rendering states.

Remarks:
+ The texture render states defines the texture filtering,blending or address modes
for the currently active texture.
+ Each time a mesh,sprite3d or 2dEntity is drawn it automatically sets the appropriate render
states for its rendering (See CKMaterial::SetAsCurrent). 
See Also: CKRenderContext::SetTextureStageState, CKRenderContext::SetState
*****************************************************************/
typedef enum CKRST_TEXTURESTAGESTATETYPE { 
	CKRST_TSS_OP			  = 1UL,	//CKRST_TEXTUREOP (Not yet supported on all implementations, left for DirectX7/8 OpenGL Usage Only)
	CKRST_TSS_ARG1			  = 2UL,	//CKRST_TEXTUREARG (Not yet supported on all implementations, left for DirectX Usage Only)
	CKRST_TSS_ARG2			  = 3UL,	//CKRST_TEXTUREARG (Not yet supported on all implementations, left for DirectX Usage Only)
	CKRST_TSS_AOP			  = 4UL,	//CKRST_TEXTUREOP (Not yet supported on all implementations, left for DirectX Usage Only)
	CKRST_TSS_AARG1			  = 5UL,	//CKRST_TEXTUREARG (Not yet supported on all implementations, left for DirectX Usage Only)	
	CKRST_TSS_AARG2			  = 6UL,	//CKRST_TEXTUREARG (Not yet supported on all implementations, left for DirectX Usage Only)
    CKRST_TSS_BUMPENVMAT00	  = 7UL,  // float *(DWORD *)&Value	(Not yet supported on all implementations, left for DirectX Usage Only)
    CKRST_TSS_BUMPENVMAT01	  = 8UL,  // float (Not yet supported on all implementations, left for DirectX Usage Only)			
    CKRST_TSS_BUMPENVMAT10	  = 9UL,	// float (Not yet supported on all implementations, left for DirectX Usage Only)	
    CKRST_TSS_BUMPENVMAT11	  = 10UL, // float (Not yet supported on all implementations, Left for DirectX Usage Only)
	CKRST_TSS_TEXCOORDINDEX	  = 11UL,	// int (Not yet supported on all implementations, left for DirectX Usage Only)
    CKRST_TSS_ADDRESS         = 12UL,	// VXTEXTURE_ADDRESSMODE
    CKRST_TSS_ADDRESSU        = 13UL,	// VXTEXTURE_ADDRESSMODE
    CKRST_TSS_ADDRESSV        = 14UL,	// VXTEXTURE_ADDRESSMODE
    CKRST_TSS_BORDERCOLOR     = 15UL, // DWORD RGBA
    CKRST_TSS_MAGFILTER		  = 16UL, // VXTEXTURE_FILTERMODE	
    CKRST_TSS_MINFILTER		  = 17UL, // VXTEXTURE_FILTERMODE

    CKRST_TSS_MIPMAPLODBIAS	  = 19UL, // float 
	CKRST_TSS_MAXMIPMLEVEL	  = 20UL, // 
    CKRST_TSS_MAXANISOTROPY	  = 21UL, // float 
	CKRST_TSS_BUMPENVLSCALE	  = 22UL, // float 
	CKRST_TSS_BUMPENVLOFFSET  = 23UL, // float


	CKRST_TSS_TEXTURETRANSFORMFLAGS  = 24UL,	//CKRST_TEXTURETRANSFORMFLAGS

	CKRST_TSS_ADDRESW		  = 25UL, 
	CKRST_TSS_COLORARG0		  = 26UL, 
	CKRST_TSS_ALPHAARG0		  = 27UL, 
	CKRST_TSS_RESULTARG0	  = 28UL, 
	


	CKRST_TSS_TEXTUREMAPBLEND = 39UL, // VXTEXTURE_BLENDMODE		
	CKRST_TSS_STAGEBLEND	  = 40UL, // Use STAGEBLEND(SrcBlendMode,DstBlendMode) macro. This state is used to test if we can perform mono-pass multitexturing. For example 
									// to draw a texture with a shadow-map in mono-pass we will try to set the CKRST_TSS_STAGEBLEND state of the second stage (1) to STAGEBLEND(VXBLEND_DESTCOLOR,0) or STAGEBLEND(0,VXBLEND_SRCCOLOR)
									// if the return value is OK (0) then we can render in mono-pass otherwise we will need to render in two passes.
	CKRST_TSS_MAXSTATE		  = 41UL	
} CKRST_TEXTURESTAGESTATETYPE; 



// If SetTextureStageState is called with CKRST_TSS_TEXCOORDINDEX
// the value is the index of texture coordinate to use.
// This value can be combined with one of the following value (<< 16) 
// to have an automatic coordinate generation
enum VXTEXCOORD_GEN {
	VXTEXCOORD_SKIP			= 0,
	VXTEXCOORD_PROJNORMAL	= 0x1,	// Texcoords = vertex normal, transformed to camera space
	VXTEXCOORD_PROJPOSITION	= 0x2,	// Texcoords = vertex position, transformed to camera space
	VXTEXCOORD_PROJREFLECT	= 0x3,	// Texcoords = reflection vector, transformed to camera space. The reflection vector is computed from the input vertex position and normal vector. 
	VXTEXCOORD_MASK			= 0x3
};

#ifndef _XBOX

// {filename:VXWRAP_MODE}
// Summary : Texture coordinates wrapping mode
// 
typedef enum VXWRAP_MODE
{
	VXWRAP_U = 0x00000001UL,	//	Texture coordinates wrapping among u texture coordinates
	VXWRAP_V = 0x00000002UL,	//	Texture coordinates wrapping among v texture coordinates
	VXWRAP_S = 0x00000004UL,	//	Texture coordinates wrapping among s texture coordinates
	VXWRAP_T = 0x00000008UL,	//	Texture coordinates wrapping among t texture coordinates
	VXWRAP_MASK = 0x000FUL	//	
} VXWRAP_MODE;

#else

typedef enum VXWRAP_MODE
{
	VXWRAP_U	= 0x00000010UL,	//	Texture coordinates wrapping among u texture coordinates
	VXWRAP_V	= 0x00001000UL,	//	Texture coordinates wrapping among v texture coordinates
	VXWRAP_S	= 0x00100000UL,	//	Texture coordinates wrapping among s texture coordinates
	VXWRAP_T	= 0x01000000UL,	//	Texture coordinates wrapping among t texture coordinates
	VXWRAP_MASK = 0x01101010UL	//	
} VXWRAP_MODE;

#endif



typedef enum VXBLENDOP
{
	VXBLENDOP_ADD			= 0x00000001L,	
	VXBLENDOP_SUBTRACT		= 0x00000002L,	
	VXBLENDOP_REVSUBTRACT	= 0x00000003L,	
	VXBLENDOP_MIN			= 0x00000004L,	
	VXBLENDOP_MAX			= 0x00000005L,	
	VXBLENDOP_MASK			= 0x00000007UL		
} VXBLENDOP;



typedef enum VXVERTEXBLENDFLAGS
{
	VXVBLEND_DISABLE		= 0x00000000UL,	
	VXVBLEND_1WEIGHTS		= 0x00000001UL,	
	VXVBLEND_2WEIGHTS		= 0x00000002UL,	
	VXVBLEND_3WEIGHTS		= 0x00000003UL,	
	VXVBLEND_TWEENING		= 0x000000FFUL,	
	VXVBLEND_0WEIGHTS		= 0x00000100UL		
} VXVERTEXBLENDFLAGS;


/*****************************************************************
{filename:VXRENDERSTATETYPE}
Summary: Rendering states.

Remarks:
+Through VXRENDERSTATETYPE , one can specify various mode for rendering, most of the time this settings are 
automatically set by render engine according to textures, objects and materials properties. Using SetState
enable to override the default behavior especially in render callbacks.
+Each time a mesh,sprite3d or 2dEntity is drawn, it automatically sets the appropriate render
states for its rendering according to the material used (See CKMaterial::SetAsCurrent). 

See Also: CKRenderContext, CKRenderContext::SetState
*****************************************************************/
typedef enum VXRENDERSTATETYPE { 
	VXRENDERSTATE_ANTIALIAS          = 2,    //Antialiasing mode (TRUE/FALSE)
    VXRENDERSTATE_TEXTUREPERSPECTIVE = 4,    //Enable Perspective correction (TRUE/FALSE)
    VXRENDERSTATE_ZENABLE            = 7,    //Enable z test (TRUE/FALSE)  
    VXRENDERSTATE_FILLMODE           = 8,    //Fill mode  (VXFILL_MODE)
    VXRENDERSTATE_SHADEMODE          = 9,    //Shade mode  (VXSHADE_MODE)
    VXRENDERSTATE_LINEPATTERN        = 10,   //Line pattern (bit pattern in a DWORD)
    VXRENDERSTATE_ZWRITEENABLE       = 14,   //Enable z writes (TRUE/FALSE)
    VXRENDERSTATE_ALPHATESTENABLE    = 15,   //Enable alpha tests  (TRUE/FALSE)
    VXRENDERSTATE_SRCBLEND           = 19,   //Blend factor for source (VXBLEND_MODE)
    VXRENDERSTATE_DESTBLEND          = 20,   //Blend factor for destination (VXBLEND_MODE)
	VXRENDERSTATE_CULLMODE           = 22,   //Back-face culling mode (VXCULL)
    VXRENDERSTATE_ZFUNC              = 23,   //Z-comparison function  (VXCMPFUNC)
    VXRENDERSTATE_ALPHAREF           = 24,   //Reference alpha value  (DWORD (0..255) )
    VXRENDERSTATE_ALPHAFUNC          = 25,   //Alpha-comparison function (VXCMPFUNC)
    VXRENDERSTATE_DITHERENABLE       = 26,   //Enable dithering (TRUE/FALSE)
    VXRENDERSTATE_ALPHABLENDENABLE   = 27,   //Enable alpha blending  (TRUE/FALSE)
    VXRENDERSTATE_FOGENABLE          = 28,   //Enable fog (TRUE/FALSE)
    VXRENDERSTATE_SPECULARENABLE     = 29,   //Enable specular highlights (TRUE/FALSE)
    VXRENDERSTATE_FOGCOLOR           = 34,   //Fog color (DWORD ARGB) 
    VXRENDERSTATE_FOGPIXELMODE       = 35,	 //Fog mode for per pixel fog (VXFOG_MODE) 
	VXRENDERSTATE_FOGSTART           = 36,   //Fog start (for both vertex and pixel fog) 
    VXRENDERSTATE_FOGEND             = 37,   //Fog end (for both vertex and pixel fog)   
    VXRENDERSTATE_FOGDENSITY         = 38,   //Fog density (for both vertex and pixel fog)
    VXRENDERSTATE_EDGEANTIALIAS      = 40,   //Antialias edges (TRUE/FALSE)
    VXRENDERSTATE_ZBIAS              = 47,   //Z-bias (DWORD 0..16)
    VXRENDERSTATE_RANGEFOGENABLE     = 48,   //Enables range-based fog 
    VXRENDERSTATE_STENCILENABLE      = 52,   //Enable or disable stenciling (TRUE/FALSE)
    VXRENDERSTATE_STENCILFAIL        = 53,   //Stencil operation (VXSTENCILOP)
    VXRENDERSTATE_STENCILZFAIL       = 54,   //Stencil operation (VXSTENCILOP)
    VXRENDERSTATE_STENCILPASS        = 55,   //Stencil operation (VXSTENCILOP)
    VXRENDERSTATE_STENCILFUNC        = 56,   //Stencil comparison function  (VXCMPFUNC)
    VXRENDERSTATE_STENCILREF         = 57,   //Reference value for stencil test (DWORD (0..255))
    VXRENDERSTATE_STENCILMASK        = 58,   //Mask value used in stencil test (DWORD (0..255))
    VXRENDERSTATE_STENCILWRITEMASK   = 59,   //Stencil buffer write mask  
    VXRENDERSTATE_TEXTUREFACTOR      = 60,   //Texture factor 
    VXRENDERSTATE_WRAP0              = 128,  // Wrap flags for 1st texture coord set (VXWRAP_MODE)
	VXRENDERSTATE_WRAP1              = 129,  // Wrap flags for 2nd texture coord set (VXWRAP_MODE)       
	VXRENDERSTATE_WRAP2              = 130,  // Wrap flags for 3rd texture coord set (VXWRAP_MODE)  
	VXRENDERSTATE_WRAP3              = 131,	 //	Wrap flags for 4th texture coord set (VXWRAP_MODE)  
	VXRENDERSTATE_WRAP4              = 132,  // Wrap flags for 5th texture coord set (VXWRAP_MODE)  
	VXRENDERSTATE_WRAP5		         = 133,  // Wrap flags for 6th texture coord set (VXWRAP_MODE)  
	VXRENDERSTATE_WRAP6              = 134,  // Wrap flags for 7th texture coord set (VXWRAP_MODE)  
    VXRENDERSTATE_WRAP7              = 135,  // Wrap flags for last texture coord set 
    VXRENDERSTATE_CLIPPING           = 136, //Enable or disable primitive clipping (TRUE/FALSE)
    VXRENDERSTATE_LIGHTING           = 137, //Enable or disable lighting (TRUE/FALSE)
    VXRENDERSTATE_AMBIENT            = 139, //Ambient color for scene (DWORD ARGB)
    VXRENDERSTATE_FOGVERTEXMODE      = 140, //Fog mode for per vertex fog (VXFOG_MODE) 
    VXRENDERSTATE_COLORVERTEX        = 141, //Enable or disable per-vertex color 
	VXRENDERSTATE_LOCALVIEWER        = 142, //Camera relative specular highlights (TRUE/FALSE)
    VXRENDERSTATE_NORMALIZENORMALS   = 143, //Enable automatic normalization of vertex normals 
    VXRENDERSTATE_DIFFUSEFROMVERTEX  = 145, //If VXRENDERSTATE_COLORVERTEX is TRUE this flags indicate whether diffuse color is taken from the vertex color (TRUE) or from the currently set material (FALSE)
    VXRENDERSTATE_SPECULARFROMVERTEX = 146, //If VXRENDERSTATE_COLORVERTEX is TRUE this flags indicate whether specular color is taken from the vertex color (2) or from the currently set material (0)
    VXRENDERSTATE_AMBIENTFROMVERTEX  = 147, //If VXRENDERSTATE_COLORVERTEX is TRUE this flags indicate whether ambient color is taken from the vertex color (TRUE) or from the currently set material (FALSE)
    VXRENDERSTATE_EMISSIVEFROMVERTEX = 148, //If VXRENDERSTATE_COLORVERTEX is TRUE this flags indicate whether emissive color is taken from the vertex color (TRUE) or from the currently set material (FALSE)

    VXRENDERSTATE_VERTEXBLEND		 = 151, //Enable vertex blending and set the number of matrices to use (VXVERTEXBLENDFLAGS)
	VXRENDERSTATE_SOFTWAREVPROCESSING= 153, //When using a T&L driver in mixed mode, for the usage of software processing 
    
	VXRENDERSTATE_POINTSIZE          = 154, //Size of point when drawing point sprites. This value is in screen space units if VXRENDERSTATE_POINTSCALEENABLE is FALSE; otherwise this value is in world space units.
    VXRENDERSTATE_POINTSIZE_MIN      = 155, //Specifies the minimum size of point primitives. If below 1 the points drawn will disappear when smaller than a pixel 
	VXRENDERSTATE_POINTSIZE_MAX      = 166,	//Specifies the maximum size of point primitives. If below 1 the points drawn will disappear when smaller than a pixel 
    VXRENDERSTATE_POINTSPRITEENABLE  = 156, // 

    VXRENDERSTATE_POINTSCALEENABLE   = 157,	//If true the size of point will be attenuated according to distance:
											//Size = pointSize * sqrt(1/ (a + b*dist + c * dist*dist)) where dist
											//is the distance from viewpoint to point.
    VXRENDERSTATE_POINTSCALE_A       = 158, // constant attenuation factor for point size computation (see VXRENDERSTATE_POINTSCALEENABLE)
    VXRENDERSTATE_POINTSCALE_B       = 159, // linear attenuation factor for point size computation (see VXRENDERSTATE_POINTSCALEENABLE)
    VXRENDERSTATE_POINTSCALE_C       = 160,	// quadratic attenuation factor for point size computation (see VXRENDERSTATE_POINTSCALEENABLE)

	VXRENDERSTATE_CLIPPLANEENABLE    = 152, //Enable one or more user-defined clipping planes ( DWORD mask of planes)
	VXRENDERSTATE_INDEXVBLENDENABLE  = 167, //Enable indexed vertex blending (to use with VXRENDERSTATE_VERTEXBLEND)
	VXRENDERSTATE_COLORWRITEENABLE   = 168, //Per channel masking. Bit 0 enables Red, Bit 1 enables Green, Bit 2 enables Blue, Bit 3 enables Alpha
	VXRENDERSTATE_BLENDOP			 = 171, //Set blending operation VXBLENDOP
	VXRENDERSTATE_SCISSORTESTENABLE  = 174, // Enable / Disable the scissor test
	VXRENDERSTATE_TWOSIDEDLIGHTING   = 175,

// Virtools Specific Render States	
	VXRENDERSTATE_LOCKMATERIALSTATES	= 252,	// if Enabled, subsequent calls to CKRasterizerContext::SetMaterial are ignored	
	VXRENDERSTATE_TEXTURETARGET			= 253,	// Hint: context is used to render on a texture	
	VXRENDERSTATE_INVERSEWINDING		= 254,	// Invert Cull CW and cull CCW (TRUE/FALSE)
	VXRENDERSTATE_MAXSTATE				= 256,	
    VXRENDERSTATE_FORCE_DWORD        = 0x7fffffff 
} VXRENDERSTATETYPE; 


/****************************************************************************/
/****************************************************************************/
/****************************************************************************/
/***************** DRIVER INFO AND CAPS *************************************/
/****************************************************************************/
/****************************************************************************/
/****************************************************************************/



/****************************************************************
{filename:VxBpps}
Summary: Bits per pixel constants

Remarks:
	+ This enumeration is used to return the available Bit per pixel modes
	for a particular type of buffer (Depth,Color or Stencil).
See Also: Vx3DCapsDesc
****************************************************************/
typedef enum VxBpps {
	VX_BPP1 =  0x00004000UL,	// 1 bit per pixel
	VX_BPP2 =  0x00002000UL,	// 2 bits per pixel
	VX_BPP4 =  0x00001000UL,	// 4 bits per pixel
	VX_BPP8 =  0x00000800UL,	// 8 bits per pixel
	VX_BPP16 = 0x00000400UL,	// 16 bits per pixel
	VX_BPP24 = 0x00000200UL,	// 24 bits per pixel
	VX_BPP32 = 0x00000100UL,  // 32 bits per pixel
} VxBpps;



typedef enum CKRST_RSTFAMILY {
	CKRST_DIRECTX = 0UL,	
	CKRST_OPENGL  = 1UL,
	CKRST_SOFT	  = 3UL,
	CKRST_ALCHEMY = 5UL, 
	CKRST_UNKNOWN = 4UL
} CKRST_RSTFAMILY;

/****************************************************************
Summary: 2D Capabilities of a render driver

See Also : VxDriverDesc,CKRenderManager::GetRenderDriverDescription
*****************************************************************/
typedef struct Vx2DCapsDesc
{
	CKRST_RSTFAMILY	Family;						// Precises the type of driver implementation : CKRST_DIRECTX,CKRST_OPENGL,CKRST_SOFT or CKRST_UNKNOWN
	DWORD 			MaxVideoMemory;				// Maximum video memory (Minus size already allocated for current display) 
	DWORD 			AvailableVideoMemory;		// Available video memory
	DWORD			Caps;						// General Caps
} Vx2DCapsDesc;



/****************************************************************
Summary: 3D Capabilities of a render driver

Remarks:
+The Vx3DCapsDesc contains the 3D capabilities of a render driver.
+3D Capabilities concerns texture size limitations, texture filtering capabilities,
blending capabilities, monopass multi-texturing capabilities,etc.
+The StencilBpps member is 0 for all render drivers except Directx7 and OpenGL based ones
See Also: VxDriverDesc,CKRenderManager::GetRenderDriverDescription,CKRenderManager::CreateRenderContext
****************************************************************/
typedef struct Vx3DCapsDesc
{
	DWORD	DevCaps;							// Unused
	DWORD	RenderBpps;							// Supported pixel formats for a 3D device (combination of VxBpps)
	DWORD	ZBufferBpps;						// Supported pixel format for Zbuffer (combination of VxBpps)
	DWORD	StencilBpps;						// Supported pixel format for Stencil buffer (combination of VxBpps)
	DWORD   StencilCaps;						// Stencil Caps CKRST_STENCILCAPS
	DWORD	MinTextureWidth;					// Min Width allowed for a texture
	DWORD	MinTextureHeight;					// Min Height allowed for a texture
	DWORD	MaxTextureWidth;					// Max Width allowed for a texture
	DWORD	MaxTextureHeight;					// Max Height allowed for a texture
	DWORD	MaxClipPlanes;						// Max number of clip planes
	DWORD	VertexCaps;							// Vertex Processing Caps : CKRST_VTXCAPS
	DWORD	MaxActiveLights;					// Max simulteanous active lights
	DWORD	MaxNumberBlendStage;				// Max number of blend Stages
	DWORD	MaxNumberTextureStage;				// Max simulteanous textures
	DWORD	MaxTextureRatio;					// Max W/H ratio

	DWORD   TextureFilterCaps;					// Texture Filtering Caps CKRST_TFILTERCAPS		
	DWORD   TextureAddressCaps;					// Texture Addressing Caps CKRST_TADDRESSCAPS
	DWORD   TextureCaps;						// Texture Caps	CKRST_TEXTURECAPS
	DWORD	MiscCaps;							// Misc Caps CKRST_MISCCAPS
	DWORD	AlphaCmpCaps;						// Alpha compare function caps CKRST_CMPCAPS
	DWORD	ZCmpCaps;							// Z compare function caps	CKRST_CMPCAPS
	DWORD	RasterCaps;							// Rasterization Caps CKRST_RASTERCAPS
	DWORD	SrcBlendCaps;						// Source Blend Caps CKRST_BLENDCAPS
	DWORD	DestBlendCaps;						// Destination Blend Caps CKRST_BLENDCAPS
	DWORD   CKRasterizerSpecificCaps;			// Specific to CKRasterizers CKRST_SPECIFICCAPS
	DWORD   MaxIndexedBlendMatrices;			// Number of indexed Matrices	
	DWORD   MaxVertexCountPerCall;				// Max Number of vertex that can be stored in a vertex buffer	
	BOOL    StretchRectSupport;                 // Is StretchRect supported by rasterizer ?
} Vx3DCapsDesc;




/****************************************************************
{filename:CKRST_SPECIFICCAPS}
Summary: CKRasterizer specific caps

Remarks:
	+ This enumeration is used to know special implementation
	details of a given rasterizer.
See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_SPECIFICCAPS {
	CKRST_SPECIFICCAPS_SPRITEASTEXTURES		=0x00000001UL,		// Sprites are rendered using textured primitives.
	CKRST_SPECIFICCAPS_CLAMPEDGEALPHA		=0x00000002UL,		// Texture border alpha needs to be set to zero for Texture clamping to work correctly (OpenGL implementation of clamping for ex.)
	CKRST_SPECIFICCAPS_CANDOVERTEXBUFFER	=0x00000004UL,		// Vertex buffer can be lock to be read or write
	CKRST_SPECIFICCAPS_GLATTENUATIONMODEL	=0x00000008UL,		// GL attenuation (or DX 7) light attenuation model
	CKRST_SPECIFICCAPS_SOFTWARE				=0x00000010UL,		// Software transformations,lighting and rasterization.
	CKRST_SPECIFICCAPS_HARDWARE				=0x00000020UL,		// Software transformations and lighting,hardware rasterization.
	CKRST_SPECIFICCAPS_HARDWARETL			=0x00000040UL,		// Hardware transformations,lighting and rasterization.
	CKRST_SPECIFICCAPS_COPYTEXTURE			=0x00000080UL,		// Can copy back buffer parts into a texture ? (GL..) 

	CKRST_SPECIFICCAPS_DX5					=0x00000100UL,		// DX 5 implementation (if Family ==  CKRST_DIRECTX)
	CKRST_SPECIFICCAPS_DX7					=0x00000200UL,		// DX 7 implementation (if Family ==  CKRST_DIRECTX)
	CKRST_SPECIFICCAPS_DX8					=0x00000400UL,		// DX 8.1 implementation (if Family ==  CKRST_DIRECTX)
	CKRST_SPECIFICCAPS_DX9					=0x00000800UL,		// DX 9 implementation (if Family ==  CKRST_DIRECTX)

	CKRST_SPECIFICCAPS_SUPPORTSHADERS		=0x00001000UL,		// CKShaders are supported by this implementation
	CKRST_SPECIFICCAPS_POINTSPRITES			=0x00002000UL,		// Point sprites are supported
	
	CKRST_SPECIFICCAPS_VERTEXCOLORABGR		=0x00004000UL,		// OGL implementation : if set, CK2_3D will send colors of Vertex Buffers into the appropriate format
	CKRST_SPECIFICCAPS_BLENDTEXTEFFECT		=0x00008000UL,		// OGL implementation : if set, CK2_3D do BlendTexturesEffect (Texture Combine Effect)

	CKRST_SPECIFICCAPS_CANDOINDEXBUFFER		=0x00010000UL,		// Index buffers can be lock to be read or write
	CKRST_SPECIFICCAPS_HW_SKINNING			=0x00020000UL,		// Implementation can perform hardware accelerated skinning

	CKRST_SPECIFICCAPS_AUTGENMIPMAP			=0x00040000UL,		// Graphics card supports automatic mipmap generation

} CKRST_SPECIFICCAPS;

/****************************************************************
Summary: Texture Filtering Capabilities

Remarks:
	+ This enumeration gives the supported filter mode supported
	by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,CKMaterial::SetTextureMinMode,CKMaterial::SetTextureMagMode,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_TFILTERCAPS {
	CKRST_TFILTERCAPS_NEAREST				=0x00000001UL,	// Point sampling supported
	CKRST_TFILTERCAPS_LINEAR				=0x00000002UL,	// Bilinear filtering supported
	CKRST_TFILTERCAPS_MIPNEAREST			=0x00000004UL,	// Mipmapping supported with point sampling
	CKRST_TFILTERCAPS_MIPLINEAR				=0x00000008UL,	// Mipmapping supported with bilinear filtering
	CKRST_TFILTERCAPS_LINEARMIPNEAREST		=0x00000010UL,	// Bilinear interpolation between mipmaps with point sampling
	CKRST_TFILTERCAPS_LINEARMIPLINEAR		=0x00000020UL,	// Trilinear filtering supported
	CKRST_TFILTERCAPS_ANISOTROPIC			=0x00000400UL	// Anisotropic filtering supported
} CKRST_TFILTERCAPS;


/****************************************************************
Summary: Texture Addressing Capabilities

Remarks:
	+ This enumeration gives the texture addressing mode supported
	by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,CKMaterial::SetTextureAddressMode,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_TADDRESSCAPS {
	CKRST_TADDRESSCAPS_WRAP           =0x00000001UL,	// Texture wrapping supported
	CKRST_TADDRESSCAPS_MIRROR         =0x00000002UL,	// Texture mirroring supported
	CKRST_TADDRESSCAPS_CLAMP          =0x00000004UL,	// Texture clamping supported
	CKRST_TADDRESSCAPS_BORDER         =0x00000008UL,	// A border color can be used outside the [0..1] range
	CKRST_TADDRESSCAPS_INDEPENDENTUV  =0x00000010UL	// Address mode can be set separately on U and V components (see CKRST_TSS_ADDRESS,CKRST_TSS_ADDRESSU,CKRST_TSS_ADDRESSV)
} CKRST_TADDRESSCAPS;

/****************************************************************
{filename:CKRST_TEXTURECAPS}
Summary: Texture Capabilities

Remarks:
	+ This enumeration gives the texture operation supported
	by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,CKMaterial::PerspectiveCorrectionEnabled,CKMaterial::SetTextureBlendMode,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_TEXTURECAPS {
	CKRST_TEXTURECAPS_PERSPECTIVE			=0x00000001UL	// Perspective correction is supported
	,CKRST_TEXTURECAPS_POW2					=0x00000002UL	// Texture size must be powers of 2 
	,CKRST_TEXTURECAPS_ALPHA				=0x00000004UL	// Supports texture with alpha values with VXTEXTUREBLEND_DECAL and VXTEXTUREBLEND_MODULATE blending modes.
	,CKRST_TEXTURECAPS_SQUAREONLY			=0x00000020UL	// Textures must have the same witdh than height. 
	,CKRST_TEXTURECAPS_CONDITIONALNONPOW2	=0x00000100UL	// Device support conditionnal pow2 textures
	,CKRST_TEXTURECAPS_PROJECTED			=0x00000400UL	// Device supports the CKRST_TTF_PROJECTED transform flags: If this capability is present, then the projective divide occurs per pixel
	,CKRST_TEXTURECAPS_CUBEMAP				=0x00000800UL	// Device can do cube maps
	,CKRST_TEXTURECAPS_VOLUMEMAP			=0x00002000UL	// Device can do volume maps.
} CKRST_TEXTURECAPS;


/****************************************************************
Summary: Stencil operations supported

Remarks:
	+ This enumeration gives the stencil operations supported
	by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,VXSTENCILOP,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_STENCILCAPS {
	CKRST_STENCILCAPS_KEEP     =0x00000001UL,	// VXSTENCILOP_KEEP operation is supported
	CKRST_STENCILCAPS_ZERO     =0x00000002UL,	// VXSTENCILOP_ZERO operation is supported
	CKRST_STENCILCAPS_REPLACE  =0x00000004UL,	// VXSTENCILOP_REPLACE operation is supported
	CKRST_STENCILCAPS_INCRSAT  =0x00000008UL,	// VXSTENCILOP_INCRSAT operation is supported
	CKRST_STENCILCAPS_DECRSAT  =0x00000010UL,	// VXSTENCILOP_DECRSAT operation is supported
	CKRST_STENCILCAPS_INVERT   =0x00000020UL,	// VXSTENCILOP_INVERT operation is supported
	CKRST_STENCILCAPS_INCR     =0x00000040UL,	// VXSTENCILOP_INCR operation is supported
	CKRST_STENCILCAPS_DECR     =0x00000080UL		// VXSTENCILOP_DECR operation is supported
} CKRST_STENCILCAPS;


/****************************************************************
Summary: Miscellaneous capabilities

Remarks:
See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_MISCCAPS {
	CKRST_MISCCAPS_MASKZ              =0x00000002UL,	// Driver can enable and disable modification of the depth buffer (Zwrite Enable) 
	CKRST_MISCCAPS_CONFORMANT         =0x00000008UL,	// Driver is conformant to the OpenGL standard. 
	CKRST_MISCCAPS_CULLNONE           =0x00000010UL,	// no triangle culling 
	CKRST_MISCCAPS_CULLCW             =0x00000020UL, // The driver supports clockwise triangle culling.
	CKRST_MISCCAPS_CULLCCW            =0x00000040UL	// The driver supports counter clockwise triangle culling.
} CKRST_MISCCAPS;

/****************************************************************
Summary: Rasterization capabilities

See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_RASTERCAPS {
	CKRST_RASTERCAPS_DITHER                   =0x00000001UL, // Driver can perform dithering 
	CKRST_RASTERCAPS_ZTEST                    =0x00000010UL, // Z-tests operations are supported
	CKRST_RASTERCAPS_SUBPIXEL                 =0x00000060UL, // Subpixel placement supported
	CKRST_RASTERCAPS_FOGVERTEX                =0x00000080UL, // Per vertex fog is supported
	CKRST_RASTERCAPS_FOGPIXEL                 =0x00000100UL, // Per Pixel fog is supported
	CKRST_RASTERCAPS_ZBIAS                    =0x00004000UL, // Coplanar polygon can be rendered using a Z-bias to avoid Z-fight problems (VXRENDERSTATE_ZBIAS)
	CKRST_RASTERCAPS_ZBUFFERLESSHSR           =0x00008000UL, // Hidden surface removal can be performed without a Z-buffer
	CKRST_RASTERCAPS_FOGRANGE                 =0x00010000UL, // Support range based fog.
	CKRST_RASTERCAPS_ANISOTROPY               =0x00020000UL, // Anisotropic filtering is supported	
	CKRST_RASTERCAPS_WBUFFER                  =0x00040000UL, // Depth buffering can be perfomed using w values (more precise).
	CKRST_RASTERCAPS_WFOG			          =0x00100000UL, // The device supports w-based pixel fog.
	CKRST_RASTERCAPS_ZFOG			          =0x00200000UL, // The device supports z-based pixel fog.
} CKRST_RASTERCAPS;


/****************************************************************
Summary: Supported blending factors.

Remarks:
	+ This enumeration gives the blending factors (VXBLEND_MODE) supported by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription,VXBLEND_MODE.
****************************************************************/
typedef enum CKRST_BLENDCAPS {
	CKRST_BLENDCAPS_ZERO               =0x00000001UL	// Driver supports VXBLEND_ZERO
	,CKRST_BLENDCAPS_ONE               =0x00000002UL	// Driver supports VXBLEND_ONE
	,CKRST_BLENDCAPS_SRCCOLOR          =0x00000004UL	// Driver supports VXBLEND_SRCCOLOR
	,CKRST_BLENDCAPS_INVSRCCOLOR       =0x00000008UL	// Driver supports VXBLEND_INVSRCCOLOR
	,CKRST_BLENDCAPS_SRCALPHA          =0x00000010UL	// Driver supports VXBLEND_SRCALPHA
	,CKRST_BLENDCAPS_INVSRCALPHA       =0x00000020UL	// Driver supports VXBLEND_INVSRCALPHA
	,CKRST_BLENDCAPS_DESTALPHA         =0x00000040UL	// Driver supports VXBLEND_DESTALPHA
	,CKRST_BLENDCAPS_INVDESTALPHA      =0x00000080UL	// Driver supports VXBLEND_INVDESTALPHA
	,CKRST_BLENDCAPS_DESTCOLOR         =0x00000100UL	// Driver supports VXBLEND_DESTCOLOR
	,CKRST_BLENDCAPS_INVDESTCOLOR      =0x00000200UL	// Driver supports VXBLEND_INVDESTCOLOR
	,CKRST_BLENDCAPS_SRCALPHASAT       =0x00000400UL	// Driver supports VXBLEND_SRCALPHASAT
	,CKRST_BLENDCAPS_BOTHSRCALPHA      =0x00000800UL	// Driver supports VXBLEND_BOTHSRCALPHA
	,CKRST_BLENDCAPS_BOTHINVSRCALPHA   =0x00001000UL	// Driver supports VXBLEND_BOTHINVSRCALPHA
} CKRST_BLENDCAPS;

/****************************************************************
Summary: Supported comparison functions.

Remarks:
	+ This enumeration gives the comparison function (VXCMPFUNC) supported by a render driver.
See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription,VXCMPFUNC
****************************************************************/
typedef enum CKRST_CMPCAPS {
	CKRST_CMPCAPS_NEVER               =0x00000001UL, // Driver supports VXCMP_NEVER
	CKRST_CMPCAPS_LESS                =0x00000002UL,	// Driver supports VXCMP_LESS
	CKRST_CMPCAPS_EQUAL               =0x00000004UL,	// Driver supports VXCMP_EQUAL
	CKRST_CMPCAPS_LESSEQUAL           =0x00000008UL,	// Driver supports VXCMP_LESSEQUAL
	CKRST_CMPCAPS_GREATER             =0x00000010UL,	// Driver supports VXCMP_GREATER
	CKRST_CMPCAPS_NOTEQUAL            =0x00000020UL,	// Driver supports VXCMP_NOTEQUAL
	CKRST_CMPCAPS_GREATEREQUAL        =0x00000040UL,	// Driver supports VXCMP_GREATEREQUAL
	CKRST_CMPCAPS_ALWAYS              =0x00000080UL	// Driver supports VXCMP_ALWAYS
} CKRST_CMPCAPS;


/****************************************************************
Summary: Supported vertex processing caps.

See Also: VxDriverDesc,Vx3DCapsDesc,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_VTXCAPS {
	CKRST_VTXCAPS_TEXGEN              =0x00000001UL,		// Device can generate texture coordinates. 
	CKRST_VTXCAPS_MATERIALSOURCE	  =0x00000002UL,		// Device supports selectable vertex color sources (material or vertex).  
	CKRST_VTXCAPS_VERTEXFOG           =0x00000004UL,		// Device supports vertex fog. 
	CKRST_VTXCAPS_DIRECTIONALLIGHTS   =0x00000008UL,		// Device supports directional lights. 
	CKRST_VTXCAPS_POSITIONALLIGHTS    =0x00000010UL,		// Device supports positional lights (including point lights and spotlights). 
	CKRST_VTXCAPS_LOCALVIEWER         =0x00000020UL		// Device supports orthogonal specular highlights, enabled by setting the VXRENDERSTATE_LOCALVIEWER render state to FALSE. 
} CKRST_VTXCAPS;

/****************************************************************
Summary: Supported 2D caps.

See Also: VxDriverDesc,Vx2DCapsDesc,CKRenderManager::GetRenderDriverDescription
****************************************************************/
typedef enum CKRST_2DCAPS {
	CKRST_2DCAPS_WINDOWED		  =0x00000001UL,		// Driver supports windowed rendering
	CKRST_2DCAPS_3D				  =0x00000002UL,		// Driver supports 3d acceleration
	CKRST_2DCAPS_GDI		      =0x00000004UL		// Driver shared with GDI
} CKRST_2DCAPS;


/****************************************************************
Summary: Return from the stretch rect method
****************************************************************/
enum CKRST_STRETCHRECT_ERROR {
	CKRST_STRETCHRECT_NO_ERROR = 0,
	CKRST_STRETCHRECT_UNSUPPORTED,								// Not supported by rasterizer
	CKRST_STRETCHRECT_DEST_EQUAL_SOURCE_ERROR,					// Can't blit inside the same texture/backbuffer
	CKRST_STRETCHRECT_BLIT_FROM_MULTISAMPLED_BACKBUFFER_ERROR, // Tried to blit from a multisampled rendertarget. See CKRasterizerContext::ResumeMultisampling
	CKRST_STRETCHRECT_BLIT_TO_MULTISAMPLED_BACKBUFFER_ERROR,  // Tried to into a multisampled rendertarget. See CKRasterizerContext::ResumeMultisampling
	CKRST_STRETCHRECT_NONPOW2_SRC_TEX_ERROR,                    // The source texture dimensions should be powers of 2 (no such restriction when blitting from backbuffer)
	CKRST_STRETCHRECT_NONPOW2_DEST_TEX_ERROR,                   // The destination texture dimensions should be powers of 2 (no such restriction when blitting to backbuffer)
	CKRST_STRETCHRECT_SRC_RECT_ERROR,                           // Source rect is outside the source texture / backbuffer
	CKRST_STRETCHRECT_DEST_RECT_ERROR,                          // Dest rect is outside the dest texture / backbuffer
	CKRST_STRETCHRECT_BAD_CUBE_FACE_ERROR,						// Dest/Src cube face is invalid
	CKRST_STRETCHRECT_OPERATION_FAILED_ERROR					// Operation failed under current multisampling / texture format conditions
};








#endif
