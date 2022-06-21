#ifndef __CKGL15RASTERIZERCONTEXT_H__
#define __CKGL15RASTERIZERCONTEXT_H__

/*************************************************************************/
/*	File : CKRasterizerCtx.h											 */
/*  Author : Florian Delizy												 */
/*																		 */	
/*  OpenGL Context Renderer												 */
/*																		 */
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/


#ifdef WIN32 
	#include <windows.h>
	#include <GL/gl.h>
	#include <GL/glu.h>

#endif

#ifdef macintosh
#include "../Ck2_3d/Includes/RCKAll.h"
	#ifdef __GNUC__
		#include <AGL/agl.h>
		#include <OPENGL/glu.h>
	#else	
		#include "agl.h"
		#include "glu.h"
	#endif
#endif

#include "opengl/glext.h"

// While this work and reduce the number of state change in the case of shader, the performance gain is not measurable
// so disable it for now for safety ... there may be some gain, as all states in ManagerCG are not bound to the rasterizer state
//#define DEFER_RENDER_STATES

//#include "CKRasterizer.h"
#include "CKGL15Rasterizer.h"


namespace VCKGL15Rasterizer
{
	const CKDWORD CK_MAX_MRT_COUNT = 16;

	class CKGLRasterizerDriver;
	class CKGLContext;

	/*****************************************************************
	 CKGLRasterizerContext overload 
	******************************************************************/
	class CKGLRasterizerContext : public CKRasterizerContext
	{
		public:

			//--- Construction/destruction
			CKGLRasterizerContext(CKGLRasterizerDriver* Driver);
			virtual ~CKGLRasterizerContext();

			//--- Creation
			virtual BOOL Create(WIN_HANDLE Window,int PosX=0,int PosY=0,int Width=0,int Height=0,int Bpp=-1,BOOL Fullscreen=0,int RefreshRate=0,int Zbpp=-1,int StencilBpp=-1);
			
			//---  Window manipulating
			virtual BOOL Resize(int PosX=0,int PosY=0,int Width=0,int Height=0,CKDWORD Flags=0);
			virtual BOOL Clear(CKDWORD Flags=CKRST_CTXCLEAR_ALL,CKDWORD Ccol=0,float Z=1.0f,CKDWORD Stencil=0,int RectCount=0,CKRECT* rects=NULL);
			virtual BOOL BackToFront();

			//--- Scene 
			virtual BOOL BeginScene();
			virtual BOOL EndScene();

			//--- Lighting & Material States
			virtual BOOL SetLight(CKDWORD Light,CKLightData* data);
			virtual BOOL EnableLight(CKDWORD Light,BOOL Enable);
			virtual BOOL SetMaterial(CKMaterialData* mat);

			//--- Viewport State
			virtual BOOL SetViewport(CKViewportData* data);

			//--- Scissor rect
			virtual BOOL SetScissorRect(const CKRECT* rect);

			//--- Transform Matrix
			virtual BOOL SetTransformMatrix(VXMATRIX_TYPE Type,const VxMatrix& Mat);

			//--- Render states
			virtual BOOL SetRenderState(VXRENDERSTATETYPE State,CKDWORD Value) ; 
			virtual BOOL GetRenderState(VXRENDERSTATETYPE State,CKDWORD* Value);

			//--- Texture States
			virtual BOOL SetTexture(CKDWORD Texture,int Stage=0);
			virtual BOOL SetTextureStageState(int Stage,CKRST_TEXTURESTAGESTATETYPE Tss,CKDWORD Value);

			//--- Drawing
			virtual BOOL DrawPrimitive(VXPRIMITIVETYPE pType,WORD* indices,int indexcount,VxDrawPrimitiveData* data);

			// VBO Managing functions 
			virtual BOOL DrawPrimitiveVB(VXPRIMITIVETYPE pType,CKDWORD VertexBuffer,CKDWORD StartIndex,CKDWORD VertexCount,WORD* indices=NULL,int indexcount=NULL);
			virtual BOOL DrawPrimitiveVBIB(VXPRIMITIVETYPE pType,CKDWORD VB,CKDWORD IB,CKDWORD MinVIndex,CKDWORD VertexCount,CKDWORD StartIndex,int Indexcount);

			virtual BOOL DrawPrimitiveSharedVB(VXPRIMITIVETYPE pType,CKDWORD SVertexBuffer,CKDWORD StartIndex,CKDWORD VertexCount,WORD* indices=NULL,int indexcount=NULL);
			virtual BOOL DrawPrimitiveSharedVB(VXPRIMITIVETYPE pType,CKDWORD SVertexBuffer,CKDWORD StartIndex,CKDWORD VertexCount,DWORD* indices=NULL,int indexcount=NULL);
			virtual BOOL DrawPrimitiveSharedVBIB(VXPRIMITIVETYPE pType,CKDWORD SVB,CKDWORD SIB,CKDWORD MinVIndex,CKDWORD VertexCount,CKDWORD StartIndex,int Indexcount);

			// VBO Manipulating ... 
			virtual void* LockVertexBuffer(CKDWORD VB, CKDWORD StartVertex, CKDWORD VertexCount, CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT); 
			virtual BOOL UnlockVertexBuffer(CKDWORD VB);  
			virtual BOOL OptimizeVertexBuffer(CKDWORD VB) { return FALSE; }

			// Ensure that the current VB is not in use at this time

			virtual void EnsureVBBufferNotInUse(CKVertexBufferDesc* desc);
			virtual void EnsureIBBufferNotInUse(CKIndexBufferDesc* desc);

			// Index Buffer Manipulation (VBO)
			virtual void* LockIndexBuffer(CKDWORD IB,CKDWORD StartIndex,CKDWORD IndexCount,CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);  // (default Implementation by Lib)
			virtual BOOL UnlockIndexBuffer(CKDWORD IB);  // (default Implementation by Lib)


			// Lock/unlock shared objects (VBO) must be overriden here for GL rasterizer due to VBO functionning
			
			virtual BOOL LockSharedVB2(CKDWORD SVB,CKDWORD StartVertex,VxDrawPrimitiveData& Data,CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);
			virtual BOOL UnlockSharedVB(CKDWORD SVB);
			virtual CKSharedVBDesc* GetSharedVBData(CKDWORD SVB); 

			virtual void* LockSharedIB(CKDWORD SIB,CKDWORD StartIndex,CKDWORD IndexCount,CKRST_LOCKFLAGS Lock = CKRST_LOCK_DEFAULT);  
			virtual BOOL UnlockSharedIB(CKDWORD SIB);  
			virtual CKSharedIBDesc* GetSharedIBData(CKDWORD SIB); 



			//--- Creation of Textures, Sprites and Vertex Buffer
			virtual BOOL CreateObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type, void* DesiredFormat);
			virtual BOOL DeleteObject(CKDWORD ObjIndex, CKRST_OBJECTTYPE Type);

			//--- Textures
			virtual BOOL LoadTexture(CKDWORD Texture,const VxImageDescEx& SurfDesc,int miplevel=-1);
			virtual BOOL LoadCubeMapTexture(CKDWORD Texture,const VxImageDescEx& SurfDesc,CKRST_CUBEFACE Face,int miplevel=-1);
			virtual BOOL LoadVolumeMapTexture(CKDWORD Texture,const VxImageDescEx& SurfDesc,DWORD Depth,int miplevel);
			virtual	BOOL CopyToTexture(CKDWORD Texture,VxRect* Src,VxRect* Dest,CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS );

			//---- To Enable more direct creation of system objects	without
			//---- CK2_3D holding a copy of the texture
			virtual BOOL CreateTextureFromFile(CKDWORD Texture,const char* Filename,TexFromFile* param);
			virtual BOOL CreateCubeTextureFromFile(CKDWORD Texture,const char* Filename,TexFromFile* param);
			virtual BOOL CreateVolumeTextureFromFile(CKDWORD Texture,const char* Filename,TexFromFile* param);


			//--- Sprites
			virtual BOOL DrawSprite(CKDWORD Sprite,VxRect* src,VxRect* dst);
			
			//--- Utils
			virtual int   CopyToMemoryBuffer(CKRECT* rect,VXBUFFER_TYPE buffer,VxImageDescEx& img_desc);
			virtual int   CopyFromMemoryBuffer(CKRECT* rect,VXBUFFER_TYPE buffer,const VxImageDescEx& img_desc);

			//-- Sets the rendering to occur on a texture (reset the texture format to match )
			virtual BOOL SetTargetTexture(CKDWORD TextureObject,int Width = 0, int Height = 0,CKRST_CUBEFACE Face = CKRST_CUBEFACE_XPOS , BOOL  GenerateMipMap = FALSE);

			//--- Multiple threads must warn each other to take the hand on GL rendering.
			virtual BOOL WarnThread(BOOL Enter);

			virtual BOOL FlushObjects(CKDWORD TypeMask);

			//--- User clip planes
			virtual BOOL SetUserClipPlane(CKDWORD ClipPlaneIndex,const VxPlane& PlaneEquation);
			virtual BOOL GetUserClipPlane(CKDWORD ClipPlaneIndex,VxPlane& PlaneEquation);

			//--- stereoscopic rendering
			virtual BOOL SetDrawBuffer(CKRST_DRAWBUFFER_FLAGS Flags);
			
			#ifdef macintosh
			//--- Set the clipping coordinates  : this ensures the rect given 	
			// in resize will not be drawn outside this rect...
			//
			//		L/T----W-----
			//		|
			//		|
			//		|
			//		|
			//		H
			void SetClipRect(int left,int top,int width,int height) 
			{
				m_ClipX = left;
				m_ClipY = top;
				m_ClipWidth = width;
				m_ClipHeight = height;
			}
		#endif

			//---------------------------------------------------------------------
			//---- New methods to lock video memory (This use a temporary system copy...)
			virtual BOOL LockTextureVideoMemory(CKDWORD Texture,VxImageDescEx& Desc,int MipLevel = 0,VX_LOCKFLAGS Flags = VX_LOCK_DEFAULT);
			virtual BOOL UnlockTextureVideoMemory(CKDWORD Texture,int MipLevel = 0);
#ifdef macintosh
			BOOL CustomUploadTextureVideoMemory(CKDWORD Texture,VxImageDescEx &img, int MipLevel);
#endif
			virtual BOOL	FlushPendingGPUCommands();			
			virtual CKRST_STRETCHRECT_ERROR   StretchRect(CKDWORD destTexture, CKRECT* destRect, CKRST_CUBEFACE destFace,
										CKDWORD srcTexture, CKRECT* srcRect, CKRST_CUBEFACE srcFace,
										CKBOOL filter);

			virtual BOOL	ResolveMultisampling(CKBOOL resolveColorBuffer, CKBOOL resolveDepthStencilBuffer);
			virtual BOOL	ResumeMultisampling();
			virtual BOOL	IsMultisamplingResolved() const;

			// Get the gl texture bound to current stage for the given texture target
			static GLint GetGLCurrentTexture(GLint textureTarget);
			// Get the current activate texture stage 
			GLint GetGLCurrentStage();
			// Get the currently enabled texture target (GL_TEXTURE_1D, 2D ...) for the current texture stage
			GLint GetGLCurrentTextureTarget();			
			// Disable texturing for the current texture stage (virtual for access from other dll)
			virtual void DisableGLCurrentStageTexturing();
			// Enable GL texture target for current stage, ensuring that texture target with higher precedence are disabled
			// (virtual for access from other dll)
			virtual void EnableGLTextureTarget(GLint textureTarget);

		public:			
			void    InitRenderStates();
			#ifdef DEFER_RENDER_STATES
				void	ApplyDeferredRenderStates();
			#endif
			BOOL	CreateTexture(CKDWORD Texture,CKTextureDesc* DesiredFormat);
			BOOL	CreateVertexBuffer(CKDWORD VB, CKVertexBufferDesc* DesiredFormat);
			BOOL	CreateIndexBuffer(CKDWORD IB, CKIndexBufferDesc* DesiredFormat);
			virtual BOOL CreateSharedVertexBuffer(CKDWORD VB, CKSharedVBDesc * DesiredFormat);
			virtual BOOL CreateSharedIndexBuffer(CKDWORD IB, CKSharedIBDesc * DesiredFormat);

			// For need of specific Texture Stage Update ( Texture Combine )
			void			SpecificTextureStageUpdate(const int Stage);
			int				m_NeedTextureStageUpdate[8];


			//--- For each stage : index of the tex coord pointer to use..
			CKDWORD					m_TexCoordIndex		[CKRST_MAX_STAGES];
			XPtrStrided<VxUV>		m_TexCoords			[CKRST_MAX_STAGES];
		
			
			CKGLContext*		m_GLContext;
			
			CKBOOL				m_TextureTarget; // TRUE if this render context is used to be render in a texture afterwards

			
			CKBOOL				m_RenderTargetsModified;

			// wanted MRT textures						
			GLint				m_RenderTargetsCKTexID[CK_MAX_MRT_COUNT];
			GLint				m_GLRenderTargets[CK_MAX_MRT_COUNT];			 // Currently wanted GL texture object for each MRT slot
			GLenum				m_GLRenderTargetTextureType[CK_MAX_MRT_COUNT]; // Type of wanted texture (or 0) for each MRT slot (GL_TEXTURE_2D etc ...)
			GLint				m_MRTDepthRenderBuffer;
			GLint				m_MRTStencilRenderBuffer;

			// Current MRT setup in the device			
			GLint				m_CurrentlyBoundFBO;
			GLint				m_GLDeviceRenderTargets[CK_MAX_MRT_COUNT]; // Currently bound GL texture object for each MRT slot
			GLenum				m_GLDeviceRenderTargetTextureType[CK_MAX_MRT_COUNT]; // type of bound texture (or 0) for each MRT slot (GL_TEXTURE_2D etc ...)			
			CKDWORD				m_MRTDrawBits;			
			GLint				m_MRTDeviceDepthRenderBuffer;
			GLint				m_MRTDeviceStencilRenderBuffer;
			

			
			GLenum				m_CurrentDrawBuffer;
			
			//--- StateBlocks
			BOOL 					m_SpecularEnabled;
			
			//---- For AGL contextes we need to clip out buffer to 
			// the size that is really drawn and add an offset to the viewport so 
			// that it is correctly placed...
		#ifdef macintosh
			int						m_ClipX,m_ClipY,m_ClipWidth,m_ClipHeight;
		#endif
			int						m_ViewportOffsetX, m_ViewportOffsetY;


			#ifdef DEFER_RENDER_STATES
				bool m_PreventDeferStateFlag[VXRENDERSTATE_MAXSTATE];
				bool m_InModifiedListState[VXRENDERSTATE_MAXSTATE]; // For each state tells, wether it has been modified, and wetherit is deferrable
															// If so, the state should be the deffered state list (applied just before a draw)
				XArray<VXRENDERSTATETYPE> m_DefferedStates;   // list of deffered states, they are applied just before a draw
			#endif

			// A function for each GetState,SetState,SetTextureStageState 
			static SetGLRenderStateFunc m_GLSetStateFunctions[VXRENDERSTATE_MAXSTATE];
			static GetGLRenderStateFunc m_GLGetStateFunctions[VXRENDERSTATE_MAXSTATE];
			static SetGLTextureStateFunc m_GLTextureStateFunctions[CKRST_TSS_MAXSTATE];
			
			//--- OpenGL Extension for secondary color, VBL sync and vertex array locks 
			void InitExtensions();
			void GetExtensions();

			BOOL m_CubeMapSupported;
			BOOL m_CombineTextureSupported;
			BOOL m_NV_Combine4TextureSupported;
			BOOL m_ATI_Combine3TextureSupported;

			
			PFNGLSECONDARYCOLORPOINTERPROC	m_glSecondaryColorPointer;
			PFNGLSECONDARYCOLOR3UBVPROC		m_glSecondaryColor3ubv;			
		
			// GL_ARB_multitexture
			PFNGLACTIVETEXTUREARBPROC			m_glActiveTextureARB;
			PFNGLCLIENTACTIVETEXTUREARBPROC		m_glClientActiveTextureARB;

			// Volume Textures (OpenGL 1.2 Spec)
			PFNGLTEXIMAGE3DPROC					m_glTexImage3D;

			// GL_ARB_texture_compression
			PFNGLCOMPRESSEDTEXIMAGE3DARBPROC    m_glCompressedTexImage3DARB;
			PFNGLCOMPRESSEDTEXIMAGE2DARBPROC    m_glCompressedTexImage2DARB;
			PFNGLCOMPRESSEDTEXIMAGE1DARBPROC    m_glCompressedTexImage1DARB;
		
			// GL_ARB_vertex_buffer_object
			PFNGLBINDBUFFERARBPROC				m_glBindBufferARB;
			PFNGLDELETEBUFFERSARBPROC			m_glDeleteBuffersARB;
			PFNGLGENBUFFERSARBPROC				m_glGenBuffersARB;
			PFNGLISBUFFERARBPROC				m_glIsBufferARB;
			PFNGLBUFFERDATAARBPROC				m_glBufferDataARB;
			PFNGLBUFFERSUBDATAARBPROC			m_glBufferSubDataARB;
			PFNGLGETBUFFERSUBDATAARBPROC		m_glGetBufferSubDataARB;
			PFNGLMAPBUFFERARBPROC				m_glMapBufferARB;
			PFNGLUNMAPBUFFERARBPROC				m_glUnmapBufferARB;
			PFNGLGETBUFFERPARAMETERIVARBPROC	m_glGetBufferParameterivARB;
			PFNGLGETBUFFERPOINTERVARBPROC		m_glGetBufferPointervARB;

			// GL_EXT_framebuffer_objectFrame Buffer Object (to replace Pbuffer + Render to Texture)
			//PFNGLISRENDERBUFFEREXTPROC				m_glIsRenderbufferEXT;
			PFNGLBINDRENDERBUFFEREXTPROC			m_glBindRenderbufferEXT;
			PFNGLDELETERENDERBUFFERSEXTPROC			m_glDeleteRenderbuffersEXT;
			PFNGLGENRENDERBUFFERSEXTPROC			m_glGenRenderbuffersEXT;
			PFNGLRENDERBUFFERSTORAGEEXTPROC			m_glRenderbufferStorageEXT;
			PFNGLGETRENDERBUFFERPARAMETERIVEXTPROC	m_glGetRenderbufferParameterivEXT;

			//PFNGLISFRAMEBUFFEREXTPROC				m_glIsFramebufferEXT;
			PFNGLBINDFRAMEBUFFEREXTPROC				m_glBindFramebufferEXT;
			PFNGLDELETEFRAMEBUFFERSEXTPROC			m_glDeleteFramebuffersEXT;
			PFNGLGENFRAMEBUFFERSEXTPROC				m_glGenFramebuffersEXT;
			PFNGLCHECKFRAMEBUFFERSTATUSEXTPROC		m_glCheckFramebufferStatusEXT;

			PFNGLFRAMEBUFFERTEXTURE1DEXTPROC		m_glFramebufferTexture1DEXT;
			PFNGLFRAMEBUFFERTEXTURE2DEXTPROC		m_glFramebufferTexture2DEXT;
			PFNGLFRAMEBUFFERTEXTURE3DEXTPROC		m_glFramebufferTexture3DEXT;
			PFNGLFRAMEBUFFERRENDERBUFFEREXTPROC		m_glFramebufferRenderbufferEXT;
			PFNGLGENERATEMIPMAPEXTPROC				m_glGenerateMipmapEXT;
			PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVEXTPROC	m_glGetFramebufferAttachmentParameterivEXT;
			PFNGLRENDERBUFFERSTORAGEMULTISAMPLEEXTPROC m_glRenderbufferStorageMultisampleEXT;
			PFNGLBLITFRAMEBUFFEREXTPROC m_glBlitFramebufferEXT;
			PFNGLDRAWBUFFERSARBPROC m_glDrawBuffersARB;			

			// GL_EXT_framebuffer_blit

			// Vertex Attributes
			// GL_ARB_vertex_blend
			PFNGLWEIGHTPOINTERARBPROC			m_glWeightPointerARB;
			// GL_ARB_vertex_program
			PFNGLVERTEXATTRIBPOINTERPROC		m_glVertexAttribPointer;
			PFNGLENABLEVERTEXATTRIBARRAYPROC	m_glEnableVertexAttribArray;
			PFNGLDISABLEVERTEXATTRIBARRAYPROC	m_glDisableVertexAttribArray;
			PFNGLVERTEXATTRIB4FPROC				m_glVertexAttrib4f;
			PFNGLPROGRAMLOCALPARAMETER4FVARBPROC m_glProgramLocalParameter4fvARB;
			PFNGLGETPROGRAMENVPARAMETERFVARBPROC m_glGetProgramLocalParameterfvARB;
			PFNGLBINDPROGRAMARBPROC	             m_glBindProgramARB;

			// GL_NV_vertex_program
			PFNGLPROGRAMPARAMETER4FVNVPROC		m_glProgramParameter4fvNV;
			PFNGLBINDPROGRAMNVPROC				m_glBindProgramNV;

			// GL_NV_fragment_program
			PFNGLPROGRAMNAMEDPARAMETER4FVNVPROC m_glProgramNamedParameter4fvNV;

			
			// GL_ARB_point_parameters
			PFNGLPOINTPARAMETERFARBPROC			m_glPointParameterfARB;
			PFNGLPOINTPARAMETERFVARBPROC		m_glPointParameterfvARB;

			
			#ifdef GL_EXT_compiled_vertex_array
				PFNGLLOCKARRAYSEXTPROC				m_glLockArrayEXT;
				PFNGLUNLOCKARRAYSEXTPROC			m_glUnlockArraysEXT;
			#endif
				
			#ifdef GL_NV_vertex_array_range
				PFNGLFLUSHVERTEXARRAYRANGENVPROC	m_glFlushVertexArrayRangeNV;
				PFNGLVERTEXARRAYRANGENVPROC			m_glVertexArrayRangeNV;
			#endif
					
		#ifdef WIN32
			PFNWGLSWAPINTERVALEXTPROC			m_wglSwapIntervalEXT;


			#ifdef 	WGL_NV_allocate_memory
				PFNWGLALLOCATEMEMORYNVPROC			m_wglAllocateMemoryNV;
				PFNWGLFREEMEMORYNVPROC				m_wglFreeMemoryNV;

				BYTE*								m_AgpMem;
			#endif
				
		#endif

			
			PFNGLBLENDEQUATIONPROC			m_glBlendEquation;

			// gl matrixs
			VxMatrix m_GLModelViewMatrix;
			VxMatrix m_GLProjectionMatrix;			
			

			//  Point Scale test 

			BOOL m_PointSpriteEnabled;



			//-----------------------------------------------------
			// + To do texture rendering, Z-buffers are created when
			// needed for any given size (power of two)
			// These Z buffers are stored in the rasterizer context 
			// TempZbuffers array and are attached when doing 
			// texture rendering
			//--- Temp Z-Buffers for texture rendering...
			#define NBTEMPZBUFFER 256
			CKGLRenderBuffer*	GetTempZBuffer(int Width,int Height);			
		private :
			CKGLRenderBuffer*	m_TempZBuffers[NBTEMPZBUFFER];
			void ReleaseTempZBuffers();



			GLuint  m_VBOLockednb;
			GLuint  m_VBOIBLockednb;			
			
			#if 0 //(nicolasp) //ifdef macintosh
				XArray< Byte > m_scratch; // Used only for not calling glMapBufferARB (Vertex Buffer)
			#endif

			CKGLRasterizerDriver * m_GLDriver;

			void DumpTextureStageCombineState();
	public:
			virtual void DumpTextureStageStates(); // virtual for access by shader dll
	private:

			// light cache (added for fast restore after shader pass)
			CKBYTE	  m_GLLightEnableBits;
	public:
			CKDWORD	  m_GLLightAmbientColor;
	private:
			CKBYTE	  m_GLLightModifiedBits;
			CKBYTE	  m_GLLightEnableBitsBackup;
			VxVector4 m_GLLightCache[8][GL_QUADRATIC_ATTENUATION - GL_AMBIENT];			
			VxVector4 m_GLLightCacheBackup[8][GL_QUADRATIC_ATTENUATION - GL_AMBIENT];			
	public:
			// 'virtual' here for calls by shader.dll (quite rare) -> will bounce to the corresponding 'Internal' methods
			virtual void GLLightFV4(int lightIndex, GLenum pname, const GLfloat* value);
			virtual void GLLightFV3(int lightIndex, GLenum pname, const GLfloat* value);
			virtual void GLLightF(int lightIndex, GLenum pname, const GLfloat value);
			// Signal that calls to glEnableLight/glDisableLight may have been done between a pair
			// of BackupLightSetup/RestoreLightSetup
			// -> Light setup then bypass the cache until "RestoreLightSetup" is called
			// Usually called by shader.dll when a CG shader could not be optimized (that is if may change lights without
			// the cache in this object knowing it)
			virtual void InvalidateLightSetupCache() { m_GLLightModifiedBits = 0xff; }			
			// Backup enabled lights (for shader.dll usage)
			virtual void BackupLightSetup();
			// Restore enabled lights (for shader.dll usage at the end of a shader pass)
			virtual void RestoreLightSetup();

			void GLLightFV4Internal(int lightIndex, GLenum pname, const GLfloat* value);
			void GLLightFV3Internal(int lightIndex, GLenum pname, const GLfloat* value);
			void GLLightFInternal(int lightIndex, GLenum pname, const GLfloat value);

	public:
#ifdef _DEBUG
		int m_nbVboAllocated; // Used to keep track of creation/deletion of vbo debug only
		int m_nbIboAllocated; // Used to keep track of creation/deletion of ibo debug only
#endif // #ifdef _DEBUG				
	public:
		inline BOOL _SetTextureInternal(CKGLTextureDesc* currentTex, int Stage);
		void  _StretchRectEmulate(const CKGLTextureDesc* destTexture, const CKRECT* destRect, CKDWORD srcTextureIndex, CKGLTextureDesc& srcTexture, CKRST_CUBEFACE srcFace, const CKRECT* srcRect, CKBOOL filter);
		CKBOOL  _SetupStretchRectFrameBuffer(CKDWORD texture, CKRECT* wantedRect, CKRST_CUBEFACE cubeFace, CKRECT &finalRect, GLuint &fbIndex);
		CKBOOL	_BindOffscreenMultisampledBuffer();
		//
		void    _CheckMRTIntegrity();
		// Update current FBO for MRT (possibly a single buffer) rendering
		// This is done in a deferred fashion for performance reasons
		void	UpdateMRTFBO();
		//void    _UnbindRenderTarget(CKDWORD mrtSlot);
		//void    _BeginMRTSetup();
		//void    _EndMRTSetup();
		//CKGLTextureDesc* _GetMRTTexDesc(CKDWORD mrtSlot);
		inline void _BindFBO(GLint fbo);
		void	ReleaseOffscreenMultisampledBuffer();
		CKBOOL  InitOffscreenMultisampledBuffer(int width, int height, int sampleCount);			
		void	CheckFBOCompletness(CKDWORD texture);
		
		// offscreen multisampled framebuffer
		GLuint m_MultiSampledFBO;
		GLuint m_MultiSampledFBOSampleCount;
		GLuint m_MultiSampledColorBuffer;
		GLuint m_MultiSampledDepthStencilBuffer;
		CKBOOL m_UseMultiSampledFBO;		

		// main framebuffer object for multiple render target support
		GLuint	m_MainFBO;
		GLint	m_MRTSlotCount;
	};

	// Utility class to backup and restore the texture bound to the current stage
	// Useful when creating a new texture -> this avoid to trash an already setupped material,
	// This object assumes that the current active stage will not change while it remains in the scope.
	// (else an assert is raised in debug build)
	class CGLTextureBindingBackup
	{
	public:
		CGLTextureBindingBackup(CKGLRasterizerContext &rst);
		~CGLTextureBindingBackup();
	private:
		CKGLRasterizerContext &m_Rasterizer;
		GLint m_TextureName;
		GLint m_ActiveTarget;
		#ifdef _DEBUG
			int m_CurrentActiveStage;
		#endif
	};

	// Utility class to backup and restore the current framebuffer object binding
	class CGLFrameBufferBindingBackup
	{
	public:
		CGLFrameBufferBindingBackup(CKGLRasterizerContext &rst);
		~CGLFrameBufferBindingBackup();
	private:
		CKGLRasterizerContext &m_Rasterizer;
		GLuint m_FBOIndex;		
	};






} // namespace VCKGL15Rasterizer


#endif // #ifndef __CKGL15RASTERIZERCONTEXT_H__
