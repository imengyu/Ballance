/***************************************************************
CKLib Includes...
When CKEngine is compiled as a library , every behaviors
readers and managers are also compiled as library to be linked 
statically : This header file contains the definition of all
functions that we have to access from CK2 in these plugins
+ the functions needed to register plugins...
***************************************************************/
#pragma once

#ifdef WIN32
	#define CDECL_CALL __cdecl
#else
	#define CDECL_CALL
#endif


//----- Registration functions	
inline void RegisterAllStaticPlugins(CKPluginManager* pm);	
inline void RegisterAllBehaviors(CKPluginManager* pm);
inline void RegisterAllManagers(CKPluginManager* pm);
inline void RegisterAllReaders(CKPluginManager* pm);
inline void RegisterRenderEngine(CKPluginManager* pm);

//----- Behaviors
inline void Register3DTransfoBehaviors(CKPluginManager* pm);
inline void RegisterBBInProgressBehaviors(CKPluginManager* pm);
inline void RegisterBBAddonsBehaviors(CKPluginManager* pm);
inline void RegisterBBAddons2Behaviors(CKPluginManager* pm);
inline void RegisterBBAddons3Behaviors(CKPluginManager* pm);
inline void RegisterEvaluatorBehaviors(CKPluginManager* pm);
inline void RegisterCamerasBehaviors(CKPluginManager* pm);
inline void RegisterControllersBehaviors(CKPluginManager* pm);
inline void RegisterCharactersBehaviors(CKPluginManager* pm);
inline void RegisterCollisionsBehaviors(CKPluginManager* pm);
inline void RegisterGridsBehaviors(CKPluginManager* pm);
inline void RegisterInterfaceBehaviors(CKPluginManager* pm);
inline void RegisterLightsBehaviors(CKPluginManager* pm);
inline void RegisterLogicsBehaviors(CKPluginManager* pm);
inline void RegisterMaterialsBehaviors(CKPluginManager* pm);
inline void RegisterMeshesBehaviors(CKPluginManager* pm);
inline void RegisterMidiBehaviors(CKPluginManager* pm);
inline void RegisterNarrativesBehaviors(CKPluginManager* pm);
inline void RegisterParticleSystemsBehaviors(CKPluginManager* pm);
inline void RegisterPhysicsBehaviors(CKPluginManager* pm);
inline void RegisterSoundsBehaviors(CKPluginManager* pm);
inline void RegisterVideoBehaviors(CKPluginManager* pm);
inline void RegisterShaderBehaviors(CKPluginManager* pm);
inline void RegisterVisualsBehaviors(CKPluginManager* pm);
inline void RegisterWorldEnvBehaviors(CKPluginManager* pm);
inline void RegisterNetworkBehaviors(CKPluginManager* pm);
inline void RegisterNetworkServerBehaviors(CKPluginManager* pm);
inline void RegisterMultiPlayerBehaviors(CKPluginManager* pm);
inline void RegisterDownloadBehaviors(CKPluginManager* pm);
inline void RegisterDatabaseBehaviors(CKPluginManager* pm);
inline void RegisterDirectorBehaviors(CKPluginManager* pm);
inline void RegisterVSLBehaviors(CKPluginManager* pm);

//----- Managers 
inline void RegisterParamOpManager(CKPluginManager* pm);
inline void RegisterInputManager(CKPluginManager* pm);
inline void RegisterSoundManager(CKPluginManager* pm);
inline void RegisterVideoManager(CKPluginManager* pm);

#if defined(WIN32)
inline void RegisterDx8VideoManager(CKPluginManager* pm);
inline void RegisterDx9VideoManager(CKPluginManager* pm);
#endif

#if defined(WIN32) || defined(macintosh)
inline void RegisterXMLManager(CKPluginManager* pm);
#endif
//----- Readers 
inline void RegisterVirtoolsReader(CKPluginManager* pm);
inline void RegisterImageReader(CKPluginManager* pm);
inline void RegisterAVIReader(CKPluginManager* pm);
inline void RegisterPNGReader(CKPluginManager* pm);
inline void RegisterJPGReader(CKPluginManager* pm);
inline void RegisterDDSReader(CKPluginManager* pm);
inline void RegisterTIFFReader(CKPluginManager* pm);

inline void RegisterWAVReader(CKPluginManager* pm);

#ifdef macintosh
inline void RegisterQTReader(CKPluginManager* pm);	
inline void RegisterQTVideoManager(CKPluginManager* pm);
#endif

#ifdef _XBOX
inline void RegisterD3DXReader(CKPluginManager* pm);	
inline void RegisterXBOXBehaviors(CKPluginManager* pm);	

// XACT
inline void RegisterXACTSoundManager(CKPluginManager* pm);	
CKPluginInfo* CKGet_XACTSoundManager_PluginInfo(int Index);

//XAUDIO2
inline void RegisterXAudio2SoundManager(CKPluginManager* pm);	
CKPluginInfo* CKGet_XAudio2SoundManager_PluginInfo(int Index);

//---- XInput Behaviors
CKPluginInfo*  CKGet_XInputController_PluginInfo(int index);
void Register_XInputController_BehaviorDeclarations(XObjectDeclarationArray *reg);
void Register_XAudio2_BehaviorDeclarations(XObjectDeclarationArray *reg);

//---- XBox Behaviors
CKPluginInfo*  CKGet_XBOXBBS_PluginInfo(int index);
void  Register_XBOXBBS_BehaviorDeclarations(XObjectDeclarationArray *reg);

#endif

#ifdef PSP
inline void RegisterPSPBehaviors(CKPluginManager* pm);	

//---- Psp Behaviors
CKPluginInfo* CDECL_CALL CKGet_PSPBBS_PluginInfo(int index);
void CDECL_CALL Register_PSPBBS_BehaviorDeclarations(XObjectDeclarationArray *reg);

#endif

//-----------------------------------------------------------
// When behaviors and plugins are compiled in a static library : 
// List of declaration functions for every possible plugins...

struct	CKRasterizerInfo;
struct	CKPluginInfo;
class	CKDataReader;

/*******************************************
+ There is only one function a rasterizer Dll is supposed
to export :"CKRasterizerGetInfo", it will be used by the render engine 
to retrieve information about the plugin :
	- Description 
******************************************/
typedef void  (*CKRST_GETINFO)(CKRasterizerInfo*); 

/***************************************************/
/**** RENDER ENGINE ********************************/
CKPluginInfo* CDECL_CALL CK2_3DGetPluginInfo(int Index);

/***************************************************/
/***** RASTERIZERS *********************************/
void CDECL_CALL CKDX9RasterizerGetInfo(CKRasterizerInfo* Info);
void CDECL_CALL CKDX8RasterizerGetInfo(CKRasterizerInfo* Info);
void CDECL_CALL CKDX7RasterizerGetInfo(CKRasterizerInfo* Info);
void CDECL_CALL CKGL15RasterizerGetInfo(CKRasterizerInfo* Info);

void CDECL_CALL CKAlchemyRasterizerGetInfo(CKRasterizerInfo* Info);

/***************************************************/
/**** READERS **************************************/
//---- Virtools Reader (4)
CKPluginInfo* CDECL_CALL CKGetVirtoolsPluginInfo(int index);
CKDataReader* CDECL_CALL CKGetVirtoolsReader(int index);

//---- Image Reader (3)
CKPluginInfo* CDECL_CALL CKGet_ImageReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_ImageReader_Reader(int pos);

//---- Avi Reader (1)
CKPluginInfo* CDECL_CALL CKGet_AviReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_AviReader_Reader(int pos);

//---- PNG Reader (1)
CKPluginInfo* CDECL_CALL CKGet_PngReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_PngReader_Reader(int pos);

//---- JPG Reader (1)
CKPluginInfo* CDECL_CALL CKGet_JpgReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_JpgReader_Reader(int pos);

//---- JPG Reader (1)
CKPluginInfo* CDECL_CALL CKGet_DDSReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_DDSReader_Static(int pos);

//---- Tif Reader (1)
CKPluginInfo* CDECL_CALL CKGet_TifReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_TifReader_Reader(int pos);

//---- Wav Reader (3)
CKPluginInfo* CDECL_CALL CKGet_WavReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_WavReader_Reader(int pos);


//---- D3DX Reader
CKPluginInfo* CDECL_CALL CKGet_D3DXReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_D3DXReader_Reader(int pos);


#ifdef PSX2

extern "C" {

//---- Alchemy Reader (1)
CKPluginInfo* CDECL_CALL CKGet_AlchemyReader_PluginInfo(int index);
int			  CDECL_CALL CKGet_AlchemyReader_PluginInfoCount();
CKDataReader* CDECL_CALL CKGet_AlchemyReader_Reader(int pos);

}

//---- IPU Reader (1)
CKPluginInfo* CDECL_CALL CKGet_IPUReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_IPUReader_Reader(int pos);

CKPluginInfo* CDECL_CALL CKGet_VRLEReader_PluginInfo(int index);
CKDataReader* CDECL_CALL CKGet_VRLEReader_Reader(int pos);

#endif




/***************************************************/
/**** EXTENSIONS ***********************************/
CKPluginInfo* CDECL_CALL CKGet_ParamOp_PluginInfo(int Index);


/***************************************************/
/**** MANAGERS ***********************************/
CKPluginInfo* CDECL_CALL CKGet_InputManager_PluginInfo(int Index);
CKPluginInfo* CDECL_CALL CKGet_SoundManager_PluginInfo(int Index);
CKPluginInfo* CDECL_CALL CKGet_VideoManager_PluginInfo(int Index);

#if !defined(PSX2)
//-- WebServerManager (0:beh)
CKPluginInfo* CDECL_CALL CKGet_SelectionSetManager_PluginInfo(int Index);
void CDECL_CALL Register_SelectionSetManager_BehaviorDeclarations(XObjectDeclarationArray *reg);
#endif

#if defined(WIN32)
CKPluginInfo* CDECL_CALL CKGet_Dx8VideoManager_PluginInfo(int Index);
CKPluginInfo* CDECL_CALL CKGet_Dx9VideoManager_PluginInfo(int Index);
#endif

#if defined(macintosh)
CKPluginInfo* CDECL_CALL CKGet_QTVideoManager_PluginInfo(int Index);
#endif

#if defined(WIN32) || defined(macintosh)
CKPluginInfo* CDECL_CALL CKGet_XMLManager_PluginInfo(int Index);
void CDECL_CALL Register_XMLManager_BehaviorDeclarations(XObjectDeclarationArray *reg);

CKPluginInfo* CDECL_CALL CKGet_VideoManager_PluginInfo(int Index);
CKPluginInfo* CDECL_CALL CKGet_CKFEMgr_PluginInfo(int Index);
void CDECL_CALL Register_CKFEMgr_BehaviorDeclarations(XObjectDeclarationArray *reg);

#endif

/***************************************************/
/**** BEHAVIORS ***********************************/

//--- 3D Transfo (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_3DTransfo_PluginInfo(int index);
void CDECL_CALL Register_3DTransfo_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Behavior In Progress (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_BBInProgress_PluginInfo(int index);
void CDECL_CALL Register_BBInProgress_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ BuildingBlock Addons (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_BBAddons_PluginInfo(int Index);
void CDECL_CALL Register_BBAddons_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ BuildingBlock Addons2 (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_BBAddons2_PluginInfo(int Index);
void CDECL_CALL Register_BBAddons2_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ BuildingBlock Addons2 (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_BBAddons3_PluginInfo(int Index);
void CDECL_CALL Register_BBAddons3_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Cameras (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Cameras_PluginInfo(int Index);
void CDECL_CALL Register_Cameras_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Controllers (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Controllers_PluginInfo(int Index);
void CDECL_CALL Register_Controllers_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Characters (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Characters_PluginInfo(int Index);
void CDECL_CALL Register_Characters_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Collisions (1: Coll Manager, 2 : Floor Manager , 3 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Collisions_PluginInfo(int Index);
void CDECL_CALL Register_Collisions_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Grids (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_Grids_PluginInfo(int index);
void CDECL_CALL Register_Grids_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Evaluator (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_Evaluator_PluginInfo(int index);
void CDECL_CALL Register_Evaluator_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Interface (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_Interface_PluginInfo(int index);
void CDECL_CALL Register_Interface_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Lights (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Lights_PluginInfo(int Index);
void CDECL_CALL Register_Lights_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Logics (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Logics_PluginInfo(int Index);
void CDECL_CALL Register_Logics_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Materials (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Materials_PluginInfo(int Index);
void CDECL_CALL Register_Materials_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Meshes (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_MeshModifiers_PluginInfo(int Index);
void CDECL_CALL Register_MeshModifiers_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Midi  (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_MidiBehaviors_PluginInfo(int Index);
void CDECL_CALL Register_MidiBehaviors_BehaviorDeclarations(XObjectDeclarationArray *reg);

//------ Narratives (1 : Beh)
CKPluginInfo* CDECL_CALL CKGet_Narratives_PluginInfo(int Index);
void CDECL_CALL Register_Narratives_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Particles (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_Particles_PluginInfo(int Index);
void CDECL_CALL Register_Particles_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Physics (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_Physics_PluginInfo(int Index);
void CDECL_CALL Register_Physics_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Sounds (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_Sounds_PluginInfo(int Index);
void CDECL_CALL Register_Sounds_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Video (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_Video_PluginInfo(int Index);
void CDECL_CALL Register_Video_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- Visuals (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_Visuals_PluginInfo(int Index);
void CDECL_CALL Register_Visuals_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- World Env (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_WorldEnvironment_PluginInfo(int Index);
void CDECL_CALL Register_WorldEnvironment_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- VSManager (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_VSManager_PluginInfo(int Index);
void CDECL_CALL Register_VSManager_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- VSServerManager (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_VSServerManager_PluginInfo(int Index);
void CDECL_CALL Register_VSServerManager_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- MultiPlayer (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_MP_PluginInfo(int Index);
void CDECL_CALL Register_MP_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- DownloadMedia (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_DLM_PluginInfo(int Index);
void CDECL_CALL Register_DLM_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- DownloadMedia (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_DBC_PluginInfo(int Index);
void CDECL_CALL Register_DBC_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- DownloadMedia (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_CryptedLoader_PluginInfo(int Index);
void CDECL_CALL Register_CryptedLoader_BehaviorDeclarations(XObjectDeclarationArray *reg);


//--- Director (1: Beh)
CKPluginInfo* CDECL_CALL CKGet_Director_PluginInfo(int Index);
void CDECL_CALL Register_Director_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- VSLManager (1: Beh, 2 : Manager)
CKPluginInfo* CDECL_CALL CKGet_VSLManager_PluginInfo(int Index);
void CDECL_CALL Register_VSLManager_BehaviorDeclarations(XObjectDeclarationArray *reg);

//--- ShaderManager ( 1 : Manager, 2 : Beh) 

CKPluginInfo* CDECL_CALL CKGet_Shaders_PluginInfo(int Index);
void CDECL_CALL Register_Shaders_BehaviorDeclarations(XObjectDeclarationArray *reg);


#if !defined(PSX2)
//-- WebManager (0:beh)
CKPluginInfo* CDECL_CALL CKGet_WebManager_PluginInfo(int Index);
void CDECL_CALL Register_WebManager_BehaviorDeclarations(XObjectDeclarationArray *reg);
#endif

#if !defined(PSX2)
//-- WebServerManager (0:beh)
CKPluginInfo* CDECL_CALL CKGet_WebServerManager_PluginInfo(int Index);
void CDECL_CALL Register_WebServerManager_BehaviorDeclarations(XObjectDeclarationArray *reg);
#endif

//--------------------- Implementation -------------------------------------//
//---------------------		  of       -------------------------------------//
//--------------------- registration functions -----------------------------//


/****************************************************************************
 BEHAVIORS
*******************************************************************************/

inline void Register3DTransfoBehaviors(CKPluginManager* pm)
{
//--- 3D Tranfo
	pm->RegisterPluginInfo(0,CKGet_3DTransfo_PluginInfo(0),Register_3DTransfo_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_3DTransfo_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("3DTransfo",2);
}

inline void RegisterBBInProgressBehaviors(CKPluginManager* pm)
{
//--- Behavior In Progress (1: Beh, 2 : Manager)
	pm->RegisterPluginInfo(0,CKGet_BBInProgress_PluginInfo(0),Register_BBInProgress_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_BBInProgress_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("BBInProgress",2);
}


inline void RegisterBBAddonsBehaviors(CKPluginManager* pm)
{
//--- BuildingBlock Addons (1 : Beh)
	pm->RegisterPluginInfo(0,CKGet_BBAddons_PluginInfo(0),Register_BBAddons_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("BBAddons",1);
}

inline void RegisterBBAddons2Behaviors(CKPluginManager* pm)
{
//--- BuildingBlock Addons 2 (1 : Beh)
	pm->RegisterPluginInfo(0,CKGet_BBAddons2_PluginInfo(0),Register_BBAddons2_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("BBAddons2",1);
}

inline void RegisterBBAddons3Behaviors(CKPluginManager* pm)
{
//--- BuildingBlock Addons 3 (1 : Beh)
	pm->RegisterPluginInfo(0,CKGet_BBAddons3_PluginInfo(0),Register_BBAddons3_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("BBAddons3",1);
}

inline void RegisterCamerasBehaviors(CKPluginManager* pm)
{
//-- Cameras
	pm->RegisterPluginInfo(0,CKGet_Cameras_PluginInfo(0),Register_Cameras_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Cameras",1);
}


inline void RegisterControllersBehaviors(CKPluginManager* pm)
{
//-- Controllers
	pm->RegisterPluginInfo(0,CKGet_Controllers_PluginInfo(0),Register_Controllers_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Controllers",1);
}


inline void RegisterCharactersBehaviors(CKPluginManager* pm)
{
//-- Characters
	pm->RegisterPluginInfo(0,CKGet_Characters_PluginInfo(0),Register_Characters_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Character",1);
}



inline void RegisterCollisionsBehaviors(CKPluginManager* pm)
{
//-- Collisions
	pm->RegisterPluginInfo(0,CKGet_Collisions_PluginInfo(0),NULL,NULL);
	pm->RegisterPluginInfo(1,CKGet_Collisions_PluginInfo(1),NULL,NULL);
	pm->RegisterPluginInfo(2,CKGet_Collisions_PluginInfo(2),Register_Collisions_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Collision",3);
}


inline void RegisterGridsBehaviors(CKPluginManager* pm)
{
//--- Grids
	pm->RegisterPluginInfo(0,CKGet_Grids_PluginInfo(0),Register_Grids_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_Grids_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Grids",2);
}

inline void RegisterEvaluatorBehaviors(CKPluginManager* pm)
{
//--- Evaluator
	pm->RegisterPluginInfo(0,CKGet_Evaluator_PluginInfo(0),Register_Evaluator_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_Evaluator_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Evaluator",2);

}


inline void RegisterInterfaceBehaviors(CKPluginManager* pm)
{
//--- Interface
	pm->RegisterPluginInfo(0,CKGet_Interface_PluginInfo(0),Register_Interface_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_Interface_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Interface",2);
}


inline void RegisterLightsBehaviors(CKPluginManager* pm)
{
//-- Lights
	pm->RegisterPluginInfo(0,CKGet_Lights_PluginInfo(0),Register_Lights_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Lights",1);
}



inline void RegisterLogicsBehaviors(CKPluginManager* pm)
{
//-- Logics
	pm->RegisterPluginInfo(0,CKGet_Logics_PluginInfo(0),Register_Logics_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Logics",1);
}


inline void RegisterMaterialsBehaviors(CKPluginManager* pm)
{
//-- Materials
	pm->RegisterPluginInfo(0,CKGet_Materials_PluginInfo(0),Register_Materials_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Materials",1);
}


inline void RegisterMeshesBehaviors(CKPluginManager* pm)
{
//--- Meshes
	pm->RegisterPluginInfo(0,CKGet_MeshModifiers_PluginInfo(0),Register_MeshModifiers_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_MeshModifiers_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Meshes",2);
}



inline void RegisterMidiBehaviors(CKPluginManager* pm)
{
//--- Midi
	pm->RegisterPluginInfo(0,CKGet_MidiBehaviors_PluginInfo(0),Register_MidiBehaviors_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_MidiBehaviors_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Midi",2);
}



inline void RegisterNarrativesBehaviors(CKPluginManager* pm)
{
//-- Narratives
	pm->RegisterPluginInfo(0,CKGet_Narratives_PluginInfo(0),Register_Narratives_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Narratives",1);
}


inline void RegisterParticleSystemsBehaviors(CKPluginManager* pm)
{
//-- Particle systems 
	pm->RegisterPluginInfo(0,CKGet_Particles_PluginInfo(0),Register_Particles_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_Particles_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("ParticleSystems",2);
}


inline void RegisterPhysicsBehaviors(CKPluginManager* pm)
{
//-- Physics
	pm->RegisterPluginInfo(0,CKGet_Physics_PluginInfo(0),Register_Physics_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_Physics_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Physics",2);
}

inline void RegisterSoundsBehaviors(CKPluginManager* pm)
{
//-- Sounds
	pm->RegisterPluginInfo(0,CKGet_Sounds_PluginInfo(0),Register_Sounds_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Sounds",1);
}

inline void RegisterVideoBehaviors(CKPluginManager* pm)
{
//-- Video
	pm->RegisterPluginInfo(0,CKGet_Video_PluginInfo(0),Register_Video_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Videos",1);
}

inline void RegisterShaderBehaviors(CKPluginManager* pm)
{
//-- Shader Behaviors
	pm->RegisterPluginInfo(0,CKGet_Shaders_PluginInfo(0),NULL,NULL);
	pm->RegisterPluginInfo(1,CKGet_Shaders_PluginInfo(1),Register_Shaders_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Shaders",2);
}

inline void RegisterVisualsBehaviors(CKPluginManager* pm)
{
//-- Visuals
	pm->RegisterPluginInfo(0,CKGet_Visuals_PluginInfo(0),Register_Visuals_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Visuals",1);
}

inline void RegisterWorldEnvBehaviors(CKPluginManager* pm)
{
//-- World Env
	pm->RegisterPluginInfo(0,CKGet_WorldEnvironment_PluginInfo(0),Register_WorldEnvironment_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("WorldEnv",1);
}


inline void RegisterNetworkBehaviors(CKPluginManager* pm)
{
//-- Network Manager
	pm->RegisterPluginInfo(0,CKGet_VSManager_PluginInfo(0),Register_VSManager_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_VSManager_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Network",2);
	
#ifndef _XBOX
	pm->RegisterPluginInfo(0,CKGet_DLM_PluginInfo(0),Register_DLM_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Network_Download",1);
	
	pm->RegisterPluginInfo(0,CKGet_DBC_PluginInfo(0),Register_DBC_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Network_Database",1);	

	pm->RegisterPluginInfo(0,CKGet_CryptedLoader_PluginInfo(0),Register_CryptedLoader_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("CryptedLoader",1);
#endif

}

inline void RegisterNetworkServerBehaviors(CKPluginManager* pm)
{
//-- Network Server Manager
	pm->RegisterPluginInfo(0,CKGet_VSServerManager_PluginInfo(0),Register_VSServerManager_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_VSServerManager_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("NetworkServer",2);
}

inline void RegisterMultiPlayerBehaviors(CKPluginManager* pm)
{
//-- MultiPlayer
	pm->RegisterPluginInfo(0,CKGet_MP_PluginInfo(0),Register_MP_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_MP_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("MultiPlayer",2);
}

inline void RegisterDownloadBehaviors(CKPluginManager* pm)
{
//-- DownloadMedia
	pm->RegisterPluginInfo(0,CKGet_DLM_PluginInfo(0),Register_DLM_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("DownloadMedia",1);
}

inline void RegisterDatabaseBehaviors(CKPluginManager* pm)
{
//-- Database
	pm->RegisterPluginInfo(0,CKGet_DBC_PluginInfo(0),Register_DBC_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Database",1);
}

inline void RegisterDirectorBehaviors(CKPluginManager* pm)
{
//-- Director
	pm->RegisterPluginInfo(0,CKGet_Director_PluginInfo(0),Register_Director_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("Director",1);
}

inline void RegisterVSLBehaviors(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_VSLManager_PluginInfo(0),Register_VSLManager_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_VSLManager_PluginInfo(1),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("VSL",2);
}

//--- 
inline void RegisterAllBehaviors(CKPluginManager* pm)
{
	Register3DTransfoBehaviors(pm);
	RegisterBBInProgressBehaviors(pm);
	RegisterBBAddonsBehaviors(pm);
	RegisterBBAddons2Behaviors(pm);
	RegisterBBAddons3Behaviors(pm);	
	RegisterCamerasBehaviors(pm);
	RegisterCamerasBehaviors(pm);
	RegisterControllersBehaviors(pm);
	RegisterCharactersBehaviors(pm);
	RegisterCollisionsBehaviors(pm);
	RegisterGridsBehaviors(pm);
	RegisterInterfaceBehaviors(pm);
	RegisterLightsBehaviors(pm);
	RegisterLogicsBehaviors(pm);
	RegisterMaterialsBehaviors(pm);
	RegisterMeshesBehaviors(pm);
	RegisterMidiBehaviors(pm);
	RegisterNarrativesBehaviors(pm);
	RegisterParticleSystemsBehaviors(pm);
	RegisterPhysicsBehaviors(pm);
	RegisterSoundsBehaviors(pm);
	RegisterVideoBehaviors(pm);
	RegisterShaderBehaviors(pm);
	RegisterVisualsBehaviors(pm);
	RegisterWorldEnvBehaviors(pm);
	RegisterNetworkBehaviors(pm);
	RegisterNetworkServerBehaviors(pm);
	RegisterMultiPlayerBehaviors(pm);
	RegisterVSLBehaviors(pm);
}

/****************************************************************************
 MANAGERS
*******************************************************************************/



inline void RegisterParamOpManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_ParamOp_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("CKParamOp",1);
}

inline void RegisterInputManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_InputManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("InputManager",1);
}

inline void RegisterSoundManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_SoundManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("SoundManager",1);
}

inline void RegisterVideoManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_VideoManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("VideoManager",1);
}

#if defined(WIN32)
inline void RegisterDx9VideoManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_Dx9VideoManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Dx9VideoManager",1);
}

inline void RegisterDx8VideoManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_Dx8VideoManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Dx8VideoManager",1);
}
#endif

#if defined(macintosh)
inline void RegisterQTVideoManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_QTVideoManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("QtVideoManager",1);
}
#endif

#if defined(WIN32) || defined(macintosh)
inline void RegisterWebManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_WebManager_PluginInfo(0),Register_WebManager_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_WebManager_PluginInfo(1),NULL,NULL);	
	pm->RegisterNewStaticLibAsDll("WebManager",2);
}

inline void RegisterWebServerManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_WebServerManager_PluginInfo(0),Register_WebServerManager_BehaviorDeclarations,NULL);
	pm->RegisterPluginInfo(1,CKGet_WebServerManager_PluginInfo(1),NULL,NULL);	
	pm->RegisterNewStaticLibAsDll("WebServerManager",2);
}

inline void RegisterSelectionSetManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_SelectionSetManager_PluginInfo(0),NULL,NULL);
	pm->RegisterPluginInfo(1,CKGet_SelectionSetManager_PluginInfo(1),Register_SelectionSetManager_BehaviorDeclarations,NULL);	
	pm->RegisterNewStaticLibAsDll("SelectionSetManager",2);
}

inline void RegisterFEManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_CKFEMgr_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("CKFEMrg",1);
}

inline void RegisterXMLManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(1,CKGet_XMLManager_PluginInfo(1),NULL,NULL);	
	pm->RegisterPluginInfo(0,CKGet_XMLManager_PluginInfo(0),Register_XMLManager_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("XMLManager",2);
}

#endif

inline void RegisterAllManagers(CKPluginManager* pm)
{
	RegisterParamOpManager(pm);
	RegisterInputManager(pm);
	RegisterSoundManager(pm);
	RegisterVideoManager(pm);

#if defined(WIN32) || defined(macintosh)
	RegisterWebManager(pm);
	RegisterWebServerManager(pm);
	RegisterSelectionSetManager(pm);
	RegisterXMLManager(pm);
	RegisterFEManager(pm);
#endif

#if defined(WIN32)
	RegisterDx8VideoManager(pm);
	RegisterDx9VideoManager(pm);
#endif

#if defined(macintosh)
	RegisterQTVideoManager(pm);
#endif

}


/****************************************************************************
 READERS
*******************************************************************************/
inline void RegisterVirtoolsReader(CKPluginManager* pm)
{
//--- Virtools Reader
	pm->RegisterPluginInfo(0,CKGetVirtoolsPluginInfo(0),NULL,CKGetVirtoolsReader);
	pm->RegisterPluginInfo(1,CKGetVirtoolsPluginInfo(1),NULL,CKGetVirtoolsReader);
	pm->RegisterPluginInfo(2,CKGetVirtoolsPluginInfo(2),NULL,CKGetVirtoolsReader);
	pm->RegisterPluginInfo(3,CKGetVirtoolsPluginInfo(3),NULL,CKGetVirtoolsReader);
	pm->RegisterNewStaticLibAsDll("Virtools Reader",4);
}



inline void RegisterImageReader(CKPluginManager* pm)
{
//--- Image Reader
	pm->RegisterPluginInfo(0,CKGet_ImageReader_PluginInfo(0),NULL,CKGet_ImageReader_Reader);
	pm->RegisterPluginInfo(1,CKGet_ImageReader_PluginInfo(1),NULL,CKGet_ImageReader_Reader);
	pm->RegisterPluginInfo(2,CKGet_ImageReader_PluginInfo(2),NULL,CKGet_ImageReader_Reader);
#ifdef PSX2
	pm->RegisterPluginInfo(3,CKGet_ImageReader_PluginInfo(3),NULL,CKGet_ImageReader_Reader);	
	pm->RegisterNewStaticLibAsDll("Image Reader",4);
#else	
	pm->RegisterNewStaticLibAsDll("Image Reader",3);
#endif	
}

inline void RegisterAVIReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_AviReader_PluginInfo(0),NULL,CKGet_AviReader_Reader);
	pm->RegisterNewStaticLibAsDll("AVI Reader",1);
}

inline void RegisterPNGReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_PngReader_PluginInfo(0),NULL,CKGet_PngReader_Reader);
	pm->RegisterNewStaticLibAsDll("PNG Reader",1);
}

inline void RegisterJPGReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_JpgReader_PluginInfo(0),NULL,CKGet_JpgReader_Reader);
	pm->RegisterNewStaticLibAsDll("JPG Reader",1);
}

inline void RegisterDDSReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_DDSReader_PluginInfo(0),NULL,CKGet_DDSReader_Static);
	pm->RegisterNewStaticLibAsDll("DDS Reader",1);
}

inline void RegisterTIFFReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_TifReader_PluginInfo(0),NULL,CKGet_TifReader_Reader);
	pm->RegisterNewStaticLibAsDll("Tiff Reader",1);
}


inline void RegisterWAVReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_WavReader_PluginInfo(0),NULL,CKGet_WavReader_Reader);
	pm->RegisterPluginInfo(1,CKGet_WavReader_PluginInfo(1),NULL,CKGet_WavReader_Reader);
	pm->RegisterPluginInfo(2,CKGet_WavReader_PluginInfo(2),NULL,CKGet_WavReader_Reader);
	pm->RegisterNewStaticLibAsDll("Wav Reader",3);
}

#ifdef _XBOX
inline void RegisterD3DXReader(CKPluginManager* pm){
	int i=0;
	pm->RegisterPluginInfo(0,CKGet_D3DXReader_PluginInfo(i++),NULL,CKGet_D3DXReader_Reader); 
	pm->RegisterPluginInfo(0,CKGet_D3DXReader_PluginInfo(i++),NULL,CKGet_D3DXReader_Reader); 
	pm->RegisterPluginInfo(0,CKGet_D3DXReader_PluginInfo(i++),NULL,CKGet_D3DXReader_Reader); 
	
	pm->RegisterNewStaticLibAsDll("D3DX Reader",i);
}


inline void RegisterXBOXBehaviors(CKPluginManager* pm){
	pm->RegisterPluginInfo(0,CKGet_XBOXBBS_PluginInfo(0),Register_XBOXBBS_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("XBOXBBS",1);

}

inline void RegisterXInputBehaviors(CKPluginManager* pm){
	pm->RegisterPluginInfo(0,CKGet_XInputController_PluginInfo(0),Register_XInputController_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("XInputController",1);
}

inline void RegisterXACTSoundManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_XACTSoundManager_PluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("XACTSoundManager",1);
}

inline void RegisterXAudio2SoundManager(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_XAudio2SoundManager_PluginInfo(0),NULL,NULL);
	pm->RegisterPluginInfo(0,CKGet_XAudio2SoundManager_PluginInfo(1),Register_XAudio2_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("XAudio2SoundManager",2);
}


#endif

#ifdef PSP

void CDECL_CALL CKPSPRasterizerGetInfo(CKRasterizerInfo* Info);

inline void RegisterPSPBehaviors(CKPluginManager* pm){
	pm->RegisterPluginInfo(0,CKGet_PSPBBS_PluginInfo(0),Register_PSPBBS_BehaviorDeclarations,NULL);
	pm->RegisterNewStaticLibAsDll("PSPBBS",1);

}
#endif 

#ifdef PSX2

inline void RegisterAlchemyReader(CKPluginManager* pm)
{
	int count = CKGet_AlchemyReader_PluginInfoCount();
	for(int i=0;i<count;i++){
		pm->RegisterPluginInfo(i,CKGet_AlchemyReader_PluginInfo(i),NULL,CKGet_AlchemyReader_Reader);
	}
	pm->RegisterNewStaticLibAsDll("Alchemy Reader",count);
}

inline void RegisterVRLEReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_VRLEReader_PluginInfo(0),NULL,CKGet_VRLEReader_Reader);
	pm->RegisterNewStaticLibAsDll("VRLE Reader",1);
}

inline void RegisterIPUReader(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CKGet_IPUReader_PluginInfo(0),NULL,CKGet_IPUReader_Reader);
	pm->RegisterNewStaticLibAsDll("IPU Reader",1);
}

#endif

inline void RegisterAllReaders(CKPluginManager* pm)
{
	RegisterVirtoolsReader(pm);
	RegisterImageReader(pm);
	RegisterAVIReader(pm);
	RegisterPNGReader(pm);
	RegisterJPGReader(pm);
	RegisterDDSReader(pm);
	RegisterWAVReader(pm);
	RegisterTIFFReader(pm);
}


/****************************************************************************
 RENDER ENGINE
*******************************************************************************/
inline void RegisterRenderEngine(CKPluginManager* pm)
{
	pm->RegisterPluginInfo(0,CK2_3DGetPluginInfo(0),NULL,NULL);
	pm->RegisterNewStaticLibAsDll("Render Engine",1);
}



inline void RegisterAllStaticPlugins(CKPluginManager* pm)
{
	RegisterRenderEngine(pm);
	RegisterAllReaders(pm);
	RegisterAllManagers(pm);
	RegisterAllBehaviors(pm);
}