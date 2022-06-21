/*************************************************************************/
/*	File : CKShader.h													 */
/*	Author : Francisco Cabrita											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2003, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKShader_H

#define CKShader_H "$Id:$"

#include "CKBeObject.h"
#include "CKHideContentManager.h"

#ifndef NO_SHADER

#ifdef _XBOX
#undef CompileShader
#endif


#define ShaderManagerGUID	CKGUID(0xBA495345,0x5252B45F)


#define	SHADERMANAGER_CHUNKID		1

#define	FIRSTSHADER_CHUNKID			2

#define	SHADERMANAGER_SAVE_VERSION	3
// Save version history:
// V3 18/04/2006 save/load F2 mark added

#define	SHADERMANAGER_FLAGS			0

//-----------------------------------------------------------------------------
// Parameters GUID

#define CKPGUID_SHADER		CKGUID(0x66a07e01,0xe5c468d)

#define CKPGUID_TECHNIQUE	CKGUID(0x44af1e63,0x3c2c0d3e)

#define CKPGUID_PASS		CKGUID(0x2d0168dd,0x1622074f)


#define CKPGUID_TECHENUM	CKGUID(0x177b5e3c,0x248c262a)

//--- Max Channel Count
enum {
	MaxChannelCount = 8
};

class	CKShader;


typedef void (*CK_PROCESSCALLBACK)( void *Argument );

//-----------------------------------------------------------------------------
// Callback Struct


class CallbackStruct {
public:
	CallbackStruct(){};
	CallbackStruct( CK_PROCESSCALLBACK iCallBack, void* iArgument ){
		callBack = iCallBack;
		arg = iArgument;
	}
	CK_PROCESSCALLBACK callBack;
	void* arg;
};


enum ShaderTSCreationMode { 
	ShaderTSCreationMode_DontWeldTangentSpaces = 0,
	ShaderTSCreationMode_WeldTangentSpaces
};


typedef enum CK_SHADER_MANAGER_TYPE{
	CKSM_HLSL = 0,
	CKSM_CGFX,
	CKSM_ENUM_SIZE
} CK_SHADER_MANAGER_TYPE;



/***************************************************************************
____  _               _
/ ___|| |__   __ _  __| | ___ _ __
\___ \| '_ \ / _` |/ _` |/ _ \ '__|
___) | | | | (_| | (_| |  __/ |
|____/|_| |_|\__,_|\__,_|\___|_|   _
|  _ \  ___  ___  ___ _ __(_)_ __ | |_ ___  _ __
| | | |/ _ \/ __|/ __| '__| | '_ \| __/ _ \| '__|
| |_| |  __/\__ \ (__| |  | | |_) | || (_) | |
|____/ \___||___/\___|_|  |_| .__/ \__\___/|_|
|_|
***************************************************************************/
namespace ShaderDescriptor_Base {

	//class Manager;
	//class Shader;

	//--- Semantics Enum
	enum {
		Sem_Unknown = 0,

		Sem_Ambient,
		Sem_Attenuation,
		Sem_BoundingBoxMax,
		Sem_BoundingBoxMin,
		Sem_BoundingBoxSize,
		Sem_BoundingCenter,
		Sem_BoundingSphereSize,
		Sem_BoundingSphereMin,
		Sem_BoundingSphereMax,
		Sem_Diffuse,
		Sem_ElapsedTime,
		Sem_Emissive,
		Sem_EnvironmentNormal,
		Sem_Height,
		Sem_Joint,
		Sem_JointWorld,
		Sem_JointWorldInverse,
		Sem_JointWorldInverseTranspose,
		Sem_JointWorldView,
		Sem_JointWorldViewInverse,
		Sem_JointWorldViewInverseTranspose,
		Sem_JointWorldViewProjection,
		Sem_JointWorldViewProjectionInverse,
		Sem_JointWorldViewProjectionInverseTranspose,
		Sem_LastTime,
		Sem_Normal,
		Sem_Opacity,
		Sem_Position,
		Sem_Projection,
		Sem_ProjectionInverse,
		Sem_ProjectionInverseTranspose,
		Sem_Random,
		Sem_Refraction,
		Sem_RenderColorTarget,
		Sem_RenderDepthStencilTarget,
		Sem_RenderTargetClipping,
		Sem_RenderTargetDimensions,
		Sem_Specular,
		Sem_SpecularPower,
		Sem_AlphaTestEnable,
		Sem_AlphaFunc,
		Sem_AlphaRef,
		Sem_StandardsGlobal,
		Sem_TextureMatrix,
		Sem_Time,
		Sem_UnitsScale,
		Sem_View,
		Sem_ViewInverse,
		Sem_ViewInverseTranspose,
		Sem_ViewProjection,
		Sem_ViewProjectionInverse,
		Sem_ViewProjectionInverseTranspose,
		Sem_World,
		Sem_WorldInverse,
		Sem_WorldInverseTranspose,
		Sem_WorldView,
		Sem_WorldViewInverse,
		Sem_WorldViewInverseTranspose,
		Sem_WorldViewProjection,
		Sem_WorldViewProjectionInverse,
		Sem_WorldViewProjectionInverseTranspose,

		Sem_WorldViewProjectionTranspose,
		Sem_WorldTranspose,
		Sem_ViewTranspose,		
		Sem_ProjectionTranspose,
		Sem_WorldViewTranspose,	
		Sem_ViewProjectionTranspose,	
		Sem_EyePos,
		Sem_Texture,
		Sem_Texture0,
		Sem_Texture1,
		Sem_Texture2,
		Sem_Texture3,
		Sem_Texture4,
		Sem_Texture5,
		Sem_Texture6,
		Sem_Texture7,
		Sem_PassCount,
		Sem_PassIndex,
		Sem_Bones,		
		Sem_TBones,			
		Sem_RBones,			
		Sem_RTBones,			
		Sem_BonesPerVertex,
		Sem_ViewportPixelSize,
		Sem_TexelSize,
		Sem_TextureSize,
#if DIRECT3D_VERSION>=0x0900 || defined(macintosh)
		Sem_NearestLight,
		Sem_LightCount,
		Sem_GlobalAmbient,
#endif
#if DIRECT3D_VERSION<0x0900  || defined(macintosh)	
		Sem_TexelSize0,
		Sem_TexelSize1,
		Sem_TexelSize2,
		Sem_TexelSize3,
		Sem_BonesIndexConstant,
#endif
		Sem_WorldShadowViewProj,	

		Sem_AlphaBlendEnable,
		Sem_DoubleSided,
		Sem_SingleSided,

		SemCount
	};

	//--- Annotations Enum
	enum {

		Annot_Unknown = 0,

		Annot_Normalize,
		Annot_Object,
		Annot_Semantic,
		Annot_SemanticType,
		Annot_Space,
		Annot_Units,
		Annot_Usage,
		Annot_Dimensions,
		Annot_Discardable,
		Annot_Format,
		Annot_Function,
		Annot_MIPLevels,
		Annot_ResourceName,
		Annot_ResourceType,
		Annot_TargetPS, 
		Annot_TargetVS,
		Annot_ViewportRatio,
		Annot_UIHelp,
		Annot_UIMax,
		Annot_UIMin,
		Annot_UIName,
		Annot_UIObject,
		Annot_UIStep,
		Annot_UIStepPower,
		Annot_UIWidget,

		Annot_IsTexelSize,
		Annot_Name,
		Annot_Target,
		Annot_Width,
		Annot_Height,
		Annot_Depth,
		Annot_Type,
		Annot_Nearest,
		Annot_Sort,
		Annot_Default,
		Annot_NeedTangentSpace,
		Annot_NeedColorAndNormal,
		Annot_DirectionalLightHack,

		Annot_Script,	
		Annot_ScriptClass,
		Annot_ScriptOrder,
		Annot_ScriptOutput,


		AnnotationCount
	};

	//--- Annotation Types Enum
	enum {

		AnnotType_Unknown = 0,

		AnnotType_Bool,
		AnnotType_String,
		AnnotType_Float,
		AnnotType_Int,
		AnnotType_Vector4,

		AnnotTypeCount
	};


	//--- Annotation Strings Enum
	enum {
		AnnotStr_Unknown = 0,

		AnnotStr_Volume,
		AnnotStr_3d,
		AnnotStr_2d,
		AnnotStr_Cube,

		AnnotStr_Entity3D,
		AnnotStr_Vector,

		AnnotStr_Light,
		AnnotStr_Camera,
		AnnotStr_Local,
		AnnotStr_World,

		AnnotationStringsCount
	};


	//--- ExpositionType Enum
	enum {
		ExpositionType_Unknown = 0,

		ExpositionType_Automatic,
		ExpositionType_Exposed,

		ExpositionType_AutomaticPerPass,

		ExpositionTypeCount 
	};


}


/****************************************************************
Summary: The shader/Shader manager:

Remarks:
+ The Shader shader manages the list of available Shaders which can be retrieve by name or index
(GetShaderByName,GetShader,GetNumShaders)
+ The BeginShaders/EndShaders methods must be called to setup the default render states (GetDefaultShader)
before any rendering using Shaders take place.
+ Finally the Shader manager also returns whether the currently set rasterizer supports
drawing object with Shaders (IsSupported,GetVSPSVersion) 

+ The unique instance of this class may be retrieved through the CKContext::GetManagerByGuid(ShaderManagerGUID)

+ The engine automatically setup the shaders when they are used on meshes, sprite or sprite3D, but if
you want to draw your own primitive with a given shader, take a look at the following example.

Example:
// Drawing some triangles with a material that is using a shader
// needs to be done in a specific manner since the shader can contain 
// several passes

CKRenderContext* rdCtx;		// current rendering context
CK3dEntity*		 m_Entity;	// Target 3D entity that will be used for matrix information

CKMaterialShader* matShader = m_Material->GetMaterialShader(TRUE);
CKShaderManager* sMan = (CKShaderManager*)m_Context->GetManagerByGuid(ShaderManagerGUID);

// first check if our material is using a shader
// and is shaders are enabled
if (sMan && matShader && matShader->m_Technique) {

// Start shader rendering
sMan->BeginShaders(RdCtx);

CKShader*	fx = matShader->m_Shader;
XASSERT(fx != NULL);
fx->SetTechnique(matShader->m_Technique);
fx->SetParameters(matShader->m_Params);
RdCtx->SetValuesOfUsedAutomatics( *fx, m_Material, m_Entity);

// Begin this shader
int num_passes = fx->Begin(rdCtx);
fx->SetAutomaticValue( FXAP_PASSCOUNT, &num_passes, rdCtx);

// Set each pass and 
for (int i = 0; i < num_passes; ++i) {
fx->SetAutomaticValue( FXAP_PASSINDEX, &i, rdCtx);
fx->BeginPass(i,RdCtx);

// If some values are changed after a call to BeginPass (using SetAutomaticValue for example)
// we must warn the engine with 
// fx->CommitChanges(RdCtx);

RdCtx->DrawPrimitive(VX_TRIANGLEFAN,NULL,4,&DPData);
fx->EndPass(RdCtx);
}
// End of shaders
fx->End(RdCtx);
sMan->EndShaders(RdCtx);
} else {
// traditionnal rendering
m_Material->SetAsCurrent(RdCtx);			
RdCtx->DrawPrimitive(VX_TRIANGLEFAN,NULL,4,&DPData);
}


See Also:CKShader,CKMaterial 
****************************************************************/
class CKShaderManager :	public	CKBaseManager
{
public:
	
	CKShaderManager(CKContext *Context,CKGUID guid,CKSTRING Name)
		: CKBaseManager(Context,guid,Name),	m_DefaultShader(0), m_ShadowShader(0),
		m_LastTimeStamp_NearestLightRequested(-1)

#ifndef _XBOX		
		,	m_SavePreProcessed(false) 
#endif
	{}

	virtual ~CKShaderManager();

	// Inherited From BaseManager
	//---  Called before behavioral processing
	virtual CKERROR PreProcess();

	//---  Called after behavioral processing
	virtual CKERROR PostProcess();

	//---  Called before clear all
	virtual CKERROR PreClearAll();

	//---  Returns list of functions implemented by the manager.
	virtual CKDWORD	GetValidFunctionsMask()	{
		return 	CKBaseManager::GetValidFunctionsMask() | 
			CKMANAGER_FUNC_PreProcess|
			CKMANAGER_FUNC_PostProcess|
			CKMANAGER_FUNC_PreClearAll;
	}	


	/******************************************************************
	Summary: Returns whether current rasterizer support Shaders.
	Remarks:
	Currently only DX9 rasterizer is Shader capable.
	*******************************************************************/	
	virtual	CKBOOL		IsSupported() const = 0;
	virtual	CKBOOL		IsSupportedAndWarnIfNot()=0;

	/*******************************************************************
	Summary: Return wether 'PreCleared' was called on the shader manager
	One should not attempt to create a shader between a call to PreClear and PostClear
	*******************************************************************/
	virtual CKBOOL		IsPreCleared() const = 0;

	/*******************************************************************
	Summary: Returns a CKShader given its name.
	*******************************************************************/
	virtual CKShader*	GetShaderByName(const XBaseString& name) = 0;
	/*******************************************************************
	Summary: Returns the default CKShader.
	Remarks:
	The default CKShader contains the list of default render states to 
	set on the rasterizer before rendering any object using Shaders.
	*******************************************************************/
	virtual	CKShader*	GetDefaultShader() = 0;
	/*******************************************************************
	Summary: Returns the built-in shadow shader	
	*******************************************************************/
	virtual	CKShader*	GetShadowShader() = 0;

	/********************************************************************
	Summary:Setups the default renderstates before rendering objects with Shaders.
	*********************************************************************/
	virtual void		BeginShaders( CKRenderContext* rc ) = 0;
	
	virtual void		EndShaders( CKRenderContext* rc ) = 0;
	/********************************************************************
	Summary:Gets the version of the Vertex Shader(vs) and Pixel Shader(ps) support.
	*********************************************************************/
	virtual	void		GetVSPSVersion(float& vs, float& ps) const = 0;
	/********************************************************************
	Summary:Returns the number of existing Shaders.
	*********************************************************************/
	virtual int			GetNumShaders() const = 0;
	/********************************************************************
	Summary:Returns an Shader given its index.
	*********************************************************************/
	virtual CKShader*	GetShader(int pos) = 0;


	virtual CKShader*	CreateShader(
		const XString* name = NULL, 
		const XString* text = NULL, 
		BOOL uniqueName		= TRUE )=0;

	virtual	void		DeleteShader(CKShader* fx)=0;
	virtual CKShader*	CreateShaderFromFile(const CKSTRING filename)=0;
	virtual CKShader*	CreateShaderFromFiles(const CKSTRING HLSLfilename, const CKSTRING CgFXfilename = NULL)=0;
	virtual bool		SaveShaderToFile(const XString& filename, CKShader* fx)=0;
	virtual bool		CompileShader(CKShader* fx, XClassArray<XString> &output)=0;
	virtual bool		CompileShaderOutput(CKShader* fx, const CKSTRING funcname, 
		const CKSTRING target, XClassArray<XString> &output, XArray<char>& text)=0;



	/*******************************************************************
	Summary: Set/Get a registered automatic name.
	Remarks: index must be smaller than FXAP_MAXSIZE and greater 
	or equal to 0.
	The SetRegAutomaticName method should be called to define 
	the name of all registered automatic params at init of the
	shader manager.
	*******************************************************************/

	//	virtual XString& GetRegSemanticName( int index ,int NameIndex = 0) = 0;
	//	virtual XString& GetRegSemanticDesc( int index ) = 0;

	/*******************************************************************
	Summary: Called when a RenderContext gets created or destroyed
	to update the Shader Manager's inner list of render contexts		
	*******************************************************************/
	virtual void OnRenderContextCreated( CKRenderContext* rc ){};
	virtual void OnRenderContextDestroyed( CKRenderContext* rc ){};

	/*******************************************************************
	Summary: Retrieve Registered Semantic/Annotation Infos
	*******************************************************************/
#if defined(_XBOX) && (_XBOX_VER<200)
	virtual int GetSemanticIndexFromFourCC( DWORD iFcc) = 0;
#else
	virtual int GetSemanticIndexFromString( XString& iStr ) = 0;
#endif

	virtual const XClassArray<XString>& GetSemanticOriginalNames() = 0;
	virtual void GetSemanticDesc( int iSemIndex, XString*& oSemDesc ) = 0;
#if !defined(_XBOX) || (_XBOX_VER>=200)
	virtual const XClassArray<XString>& GetAnnotationOriginalNames() = 0;
#endif
	/*********************************************************
	Summary: Retrieves the Include Manager   
	***********************************************************/
	virtual void*		GetIncludeManager() = 0;

	/************************************************************************
	Summary: Get the current type of the shader (HLSL, CgFX, ...)  
	*************************************************************************/
	virtual CK_SHADER_MANAGER_TYPE GetCurrentShaderManagerType() const = 0;

	/*******************************************************************
	Summary: Allows to perform preprocessing on the fx text before saving
	*******************************************************************/
#ifndef _XBOX
	virtual void SetSavePreProcessed(bool preProc);
	virtual void SetSavePreprocessOptions(const char* preProcOptions);
#endif

	/********************************************************************
	Summary:Get/Set the Tangent Space Creation Mode.
	Note: See ShaderTSCreationMode enum values.
	*********************************************************************/
	virtual	int		GetTangentSpaceCreationMode() const = 0;
	virtual	void	SetTangentSpaceCreationMode( ShaderTSCreationMode iTSCreationMode ) = 0;

	/*******************************************************************
	Summary: Add temporary post/pre render callback (used by shader BBs)
	*******************************************************************/
	void AddTemporaryPreProcessCallBack( CK_PROCESSCALLBACK Function,void *Argument);
	void AddTemporaryPostProcessCallBack( CK_PROCESSCALLBACK Function,void *Argument);

	/*******************************************************************
	Summary: Manage the Technique Enum Type
	*******************************************************************/
	void RebuildTechniqueEnum( CKShader* shader );

	/*******************************************************************
	Summary: Parse all materials and update params if it use the shader
	*******************************************************************/
	void MakeParamsUpToDateForAllConcernedMaterials( CKShader* shader );

	/*******************************************************************
	GetNearestLights

	Summary: Retrieves the Nth first nearest lights from an object

	Note: The returned array contains at least iRequestedLightCount Lights,
	but there can be more lights in this array (if some other shader
	already requested more light in the same rendering frame)
	*******************************************************************/
	struct NearestLight {
		CK_ID		lightID;
		float		squareDistance;
	};

	XArray<NearestLight>* GetNearestLights( CK3dEntity* iEnt, 
		int iRequestedLightCount,
		bool iSort=true );

public:
	CKShader*				m_DefaultShader;
	CKShader*				m_ShadowShader;
	XArray<CKShader*>		m_AllShaders;
	XClassArray<XString>	m_Output;
	CKDWORD					m_RenderOptionsBeforePlay;

	//--- Those members are meant to be accessed from Meanings 
	int						m_CurrentPassCount;
	int						m_CurrentPassIndex;
	CKTexture*				m_PseudoChannelTexture[MaxChannelCount];

#ifndef _XBOX
	bool				m_SavePreProcessed;
	XString				m_SavePreprocessedOptions;
#endif

protected:

	void _ExecuteAndRemoveTemporaryCallBacks( XArray<CallbackStruct>& cbs );

	XArray<CallbackStruct>	m_PreProcessCallBackStruct;
	XArray<CallbackStruct>	m_PostProcessCallBackStruct;

	XHashTable< XArray<NearestLight>*, CK_ID > m_NearestLightFromObjects;
	int m_LastTimeStamp_NearestLightRequested;

private:
	static int	_NearestLightSort( const void *elem1, const void *elem2 );
	void _DeAllocatedNearestLightsArrays();
};

/***********************************************************************
Summary: Shader Class.

Remarks:
The CKShader class holds the description of an Shader 

See also: CKShaderManager
***********************************************************************/
class CKShader : public CKProtectable
{
public:
	CKShader(){	}

	virtual ~CKShader(){}

	/********************************************************
	Summary: Pass Description
	Note: Used in the Shader Editor
	*********************************************************/
	class	PassInfo
	{
	public:
		XString			name;			// Name given to this pass
		XString			desc;			// Description of the pass
	};

	/*********************************************
	Summary:Description of a technique
	Note: Used in the Shader Editor
	***********************************************/
	class	TechniqueInfo
	{
	public:

		XString				name;		// Name of the technique
		XString				desc;		// Description of the technique
		bool				isValid;
	};

	/************************************************************************
	F2 Marks
	*************************************************************************/
	virtual XArray<int>*	GetF2Marks() = 0;

	/************************************************************************
	Summary: Get the current type of the shader (HLSL, CgFX, ...)  
	*************************************************************************/
	virtual CK_SHADER_MANAGER_TYPE GetCurrentShaderType() const = 0;


	/****************************************************************
	Summary:Applies a Shader settings before rendering an object using the currently  set technique.
	*****************************************************************/
	virtual int		Begin( CKRenderContext* rc ) = 0;
	/****************************************************************
	Summary:Applies the settings of a given pass of a technique.
	Remarks:
	This method is intended to be used iteratively for each pass of a technique. 
	Each BeginPass called must be followed by a EndPass call after the rendering is done.
	If any value of a parameter is changed on the effect you must call CommitChanges
	*****************************************************************/
	virtual void	BeginPass( CKDWORD Num, CKRenderContext* rc ) = 0;
	/****************************************************************
	Summary:Ends the current pass .
	Remarks:
	This method must be called once the rendering for a given pass has been done.
	*****************************************************************/
	virtual void	EndPass( CKRenderContext* rc ) = 0;

	/****************************************************************
	Summary:Commits any change between a begin/end pass.
	Remarks:
	This method must be called if the value of a parameter is changed between 
	BeginPass and the rendering.
	*****************************************************************/
	virtual void	CommitChanges( CKRenderContext* rc ) = 0;
	/****************************************************************
	Summary:Ends the application of a technique .
	*****************************************************************/
	virtual void	End( CKRenderContext* rc ) = 0;

	/****************************************************************
	Summary:Sets the technique to use when applying this Shader.
	*****************************************************************/
	virtual void	SetTechnique( const XString &iTechName ) = 0;
	virtual void	SetTechnique( int iTechIndex ) = 0;

	virtual bool	FindNextValidTechnique( int& ioTechIndex, XString* oTechName=NULL ) const = 0;

	virtual CKBOOL	IsSupported() const = 0;

	// Techniques/Pass enumeration
	virtual int		GetTechniqueEnumValue( const XString& name ) const = 0;
	virtual void	GetTechniqueEnumString( int num, XString& oName ) const = 0;

	virtual int		GetPassCount( int iTechIndex ) const = 0;
	virtual void	GetPassInfo( int iTechIndex, int iPassIndex, PassInfo& oPassInfo ) const = 0;
	virtual bool	GetPassIndexByName( int iTechIndex, const char* iPassName, int& oPos ) const = 0;

	// Parameters creation
	virtual void	SetParameters( const XArray<CKParameterLocal*>& params ) = 0;
	// Returns true if it has changed the technique.
	virtual bool	LinkTechnique( int& CompID, const XString& tech, BOOL fnvt, int& oTechIndex ) = 0;
	// Returns true if it has changed the parameters.
	virtual bool	LinkParameters( int& CompID, XArray<CKParameterLocal*>& params, CKBOOL cleanup = FALSE ) = 0;


	/***********************************************************
	Summary: Returns the list of Techniques for this Shader.
	***********************************************************/
	virtual int		GetTechniquesCount() const = 0;
	virtual void	GetTechniqueInfo( int iTechIndex, TechniqueInfo& oTechInfo ) const = 0;
	virtual bool	GetTechIndexByName( const XString& iTechName, int& oPos ) const = 0;

	/***********************************************************
	Summary: Sets the entire Shader text.
	***********************************************************/
	virtual void SetText(const XString& Text) = 0;
	virtual void SetText(const XString& Text, CK_SHADER_MANAGER_TYPE type) = 0;

	/***********************************************************
	Summary: Gets the entire Shader text.
	***********************************************************/
	virtual const XString& GetText() const = 0;
	virtual const XString& GetText(CK_SHADER_MANAGER_TYPE type) = 0;


	/*********************************************************
	Summary: Description of a parameter as used in a Shader.
	Note: Used in the Shader Editor
	***********************************************************/
	struct  ParamInfo
	{
		XString		name;	// Name of the parameter
		CKGUID		guid;	// Type of data stored (matrix, vector, color, array, etc.)
	};

	/*********************************************************
	Summary: Returns Infos on Auto Params (used in the Shader Editor)
	***********************************************************/
	virtual int		GetAutoParameterCount( CKRenderContext* rc ) = 0;
	virtual void	GetAutoParameterInfo( int paramIndex, ParamInfo& paramInfo, CKRenderContext* rc ) = 0;

	/*********************************************************
	Summary: Returns Infos on Exposed Params (used in the Shader Editor)
	***********************************************************/
	virtual int		GetExposedParameterCount( CKRenderContext* rc ) = 0;
	virtual void	GetExposedParameterInfo( int paramIndex, ParamInfo& paramInfo, CKRenderContext* rc ) = 0;

	/*********************************************************
	Summary: Give a value to one of the shader's automatics
	***********************************************************/
	virtual void ExecuteMeanings( CK3dEntity* iEnt, CKMaterial* iMat, class CKMaterialShader* iMatShader, CKRenderContext* rc ) = 0;
	virtual void ExecutePerPassMeanings( CK3dEntity* iEnt, CKMaterial* iMat, class CKMaterialShader* iMatShader, CKRenderContext* rc ) = 0;

	/*********************************************************
	Summary: To Support Hide Content functionality
	***********************************************************/
	virtual void			SetHideFlag(CKBOOL hide) = 0;
	virtual CKBOOL			GetHideFlag() {return m_Hide; }
	virtual CKProtectable*	GetProtectable() = 0;
	virtual void			UnLock(const char* pass) {	/* We do nothing in base class.*/ }

	/*********************************************************
	Summary: Support for hard coded, externally created shaders
	***********************************************************/
	virtual CKBOOL GetSaveFlag() const = 0;
	virtual void SetSaveFlag(CKBOOL saveFlag) = 0;
	virtual CKBOOL GetListInEditorFlag() const = 0;
	virtual void SetListInEditorFlag(CKBOOL listInEditor) = 0;

	/*********************************************************
	Summary: Retrieve Input Stream Format
	***********************************************************/
	enum StreamParamUsage 
	{
		FXINPUTSTREAM_POSITION = 0,
		FXINPUTSTREAM_BLENDWEIGHT,   // 1
		FXINPUTSTREAM_BLENDINDICES,  // 2
		FXINPUTSTREAM_NORMAL,        // 3
		FXINPUTSTREAM_PSIZE,         // 4
		FXINPUTSTREAM_TEXCOORD,      // 5
		FXINPUTSTREAM_TANGENT,       // 6
		FXINPUTSTREAM_BINORMAL,      // 7
		FXINPUTSTREAM_TESSFACTOR,    // 8
		FXINPUTSTREAM_POSITIONT,     // 9
		FXINPUTSTREAM_COLOR,         // 10
		FXINPUTSTREAM_FOG,           // 11
		FXINPUTSTREAM_DEPTH,         // 12
		FXINPUTSTREAM_SAMPLE,        // 13
	};
	struct StreamParam 
	{
		unsigned int usage;		// Stream Param's Usage
		WORD usageIndex;		// Stream Param's Usage Index
		WORD size;				// Stream Param's Size (DWORD count... float=1, uvw=3...)
	};

	inline XArray<StreamParam>& GetInputStreamFormat(){ return m_InputStreamFormat; };

public:
	virtual void SetName(const XString& Name) = 0;
	virtual	const XString&	GetName() const = 0;

protected:
	XArray<StreamParam> m_InputStreamFormat;
};

#endif // NO_SHADER

#endif	//	CKShader_H
