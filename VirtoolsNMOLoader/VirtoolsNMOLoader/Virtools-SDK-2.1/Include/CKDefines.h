/*************************************************************************/
/*	File : CKDefines.h				 				 					 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKDEFINES_H
#define CKDEFINES_H "$Id:$"

// GetObject,LoadImage and GetClassName are #defined by windows.h which can cause unresolved externals
// when linking : to avoid this we use the same defines .... :(
#ifdef WIN32
#ifndef GetObject
#ifdef UNICODE
#define GetObject GetObjectW
#else
#define GetObject GetObjectA
#endif // !UNICODE
#endif // GetObject

#ifndef LoadImage
#ifdef UNICODE
#define LoadImage LoadImageW
#else
#define LoadImage LoadImageA
#endif // !UNICODE
#endif // LoadImage

#ifndef GetClassName
#ifdef UNICODE
#define GetClassName GetClassNameW
#else
#define GetClassName GetClassNameA
#endif // !UNICODE
#endif // LoadImage
#endif

//#define NO_OLDERVERSION_COMPATIBILITY

// #define FILE_CRC_TEST

//--------------------------------

// Current Version of CK Engine (Day/Month/Year)

#define CKVERSION 0x05082002

// Current Version of Dev

#define DEVVERSION 0x02050000

#define VIRTOOLS_GUID CKGUID(0x56495254, 0x4f4f4c53)

#define CKDEFINEGUID(x, y) CKGUID(x, y)

#define CK_ZERO 0.000001f

#define CKMAX_PATH 512

#define CKMAX_URL 4096

#define CKMAX_MANAGERFUNCTIONS 32

#define CKANIMATION_FORCESETSTEP 0xFFFFFFFF

#include "VxDefines.h"
#include "CKTypes.h"
#include "CKError.h"
#include "CKEnums.h"
#include "XBitArray.h"
#include "XString.h"
#include "XSArray.h"

/*************************************************
Summary: Returns the Unique Identifier for a CKObject or derivated.

Remarks:
+ Each object derived from the CKObject class has a unique ID.
+ This ID can be accessed through each instance of these classes, with the
CKObject::GetID method or through this macro.
See also: CKObject::GetID
*************************************************/
#define CKOBJID(x) (x ? x->GetID() : 0)
#define CKCHECKID(x) if (!m_Context->GetObject(x))	x = 0;
#define CKCHECK(x) if (x && x->IsToBeDeleted())		x = NULL;

//----------------------------------------------------------//
//		Default priority values								//
//----------------------------------------------------------//

#define CKOBJECT_PRIORITYMAX     32000
#define CKOBJECT_PRIORITYLEVEL   CKOBJECT_PRIORITYMAX
#define CKOBJECT_PRIORITYSCENE   30000
#define CKOBJECT_PRIORITYPLACE   20000
#define CKOBJECT_PRIORITYDEFAULT 0
#define CKOBJECT_PRIORITYMIN     -32000
#define CKBEHAVIOR_PRIORITYMAX   32000
#define CKBEHAVIOR_PRIORITYMIN   0

//----------------------------------------------------------//
//		Callback functions typedefs							//
//----------------------------------------------------------//

/***************************************************
Summary: Plugins initialization function

Remarks:
    + Some plugins (especially those implementing managers)
    may have to perform some initializations when a CKContext
    is created (for example to create a manager or register new parameter types).
    + In this case they should declare and implement the CKPluginInfo::m_InitInstanceFct pointer.
See Also:CKPluginInfo,CKPluginManager
****************************************************/
typedef CKERROR (*CK_INITINSTANCEFCT)(CKContext *context);

typedef CK_INITINSTANCEFCT CK_EXITINSTANCEFCT;

/*************************************************************
Summary: Behavior execution function.

See Also:, Writing the Execution Function,CKBEHAVIORCALLBACKFCT
***************************************************************/
typedef int (*CKBEHAVIORFCT)(const CKBehaviorContext &context);

/*************************************************************
Summary: Behavior message callbacks function.

See Also:, Writing the Behavior Callback Function,CKBEHAVIORFCT
***************************************************************/
typedef CKERROR (*CKBEHAVIORCALLBACKFCT)(const CKBehaviorContext &context);

/*************************************************************
Summary: Parameter operation evaluation function

See Also: Creation of new Parameter Operations
*************************************************************/
typedef void (*CK_PARAMETEROPERATION)(CKContext *context, CKParameterOut *Res, CKParameterIn *p1, CKParameterIn *p2);

typedef XArray<CKObjectDeclaration *> XObjectDeclarationArray;

/*************************************************
Summary: Function filling a array of behaviors present in a DLL

Remarks:
+ When creating additional behaviors DLLs, a function named "RegisterBehaviorDeclarations"
must be present and exported to declare the behaviors that may be created using this Dll.
+ See the Creating Building Blocks and samples behaviors given in this SDK
to see how to declare a behavior
+ XObjectDeclarationArray is defined as a XArray<CKObjectDeclaration*>
See also : Creating Building Blocks,CKObjectDeclaration
*************************************************/
typedef void (*CKDLL_OBJECTDECLARATIONFUNCTION)(XObjectDeclarationArray *);

/*************************************************
Summary: Function creating a behavior prototype

See also : Behavior Prototype Creation,CKBehaviorPrototype
*************************************************/
typedef CKERROR (*CKDLL_CREATEPROTOFUNCTION)(CKBehaviorPrototype **);

/****************************************************************
Summary: Render context rendering callback function.

Remarks:
    + A function can be called before and after the rendering of a scene occurs.
See Also: CKRenderContext::AddPreRenderCallBack,CKRenderContext::AddPostRenderCallBack,The Virtools Render Loop
****************************************************************/
typedef void (*CK_RENDERCALLBACK)(CKRenderContext *context, void *Argument);

/*******************************************************
Summary: 3D or 2D entity rendering callback function

Remarks:
    + Both 2D and 3D entities can have fuctions called before and/or after
    their rendering occurs.
See also:CKRenderObject::AddPreRenderCallBack,CKRenderObject::AddPostRenderCallBack
*********************************************************/
typedef CKBOOL (*CK_RENDEROBJECT_CALLBACK)(CKRenderContext *Dev, CKRenderObject *ent, void *Argument);

/****************************************************************
Summary: Mesh rendering callback function.

Remarks:
    + A function can be called before and after the rendering of an object mesh occurs.
    It can also be used to replace the default rendering function.
See Also: CKMesh::SetRenderCallBack,CKMesh::AddPreRenderCallBack,,CKMesh::AddPostRenderCallBack,The Virtools Render Loop
****************************************************************/
typedef void (*CK_MESHRENDERCALLBACK)(CKRenderContext *Dev, CK3dEntity *Mov, CKMesh *Object, void *Argument);

/****************************************************************
Summary: Mesh sub part rendering callback function.

Remarks:
    + A function can be called before and after the rendering each part (per material) of a mesh occurs.
See Also: CKMesh::AddSubMeshPreRenderCallBack,,CKMesh::AddSubMeshPostRenderCallBack,The Virtools Render Loop
****************************************************************/
typedef void (*CK_SUBMESHRENDERCALLBACK)(CKRenderContext *Dev, CK3dEntity *Mov, CKMesh *Object, CKMaterial *mat, void *Argument);

/****************************************************************
Summary: Material rendering callback function.

Remarks:
    + A function can be called each time a material is set as current on a rendering context.
    + If the function returns 0 the material data is set on the rendercontext otherwise it is skipped
    (we suppose the callback function has already set the valid render states...)
See Also: CKMaterial::SetCallback,CKMaterial::SetAsCurrent,The Virtools Render Loop
****************************************************************/
typedef int (*CK_MATERIALCALLBACK)(CKRenderContext *Dev, CKMaterial *mat, void *Argument);

typedef CKERROR (*CKUICALLBACKFCT)(CKUICallbackStruct &param, void *data);

typedef CK_LOADMODE (*CK_USERLOADCALLBACK)(CK_CLASSID Cid, CKSTRING OldName, CKSTRING NewName, CKObject **newobj, void *Arg);

typedef CK_LOADMODE (*CK_LOADRENAMECALLBACK)(CK_CLASSID Cid, CKSTRING OldName, CKSTRING NewName, CKObject **newobj, void *Arg);

//----------------------------------------------------------//
//		Behavior callback messages							//
//----------------------------------------------------------//

#define CKM_BASE							0
#define CKM_BEHAVIORPRESAVE					CKM_BASE + 1
#define CKM_BEHAVIORDELETE					CKM_BASE + 2
#define CKM_BEHAVIORATTACH					CKM_BASE + 3
#define CKM_BEHAVIORDETACH					CKM_BASE + 4
#define CKM_BEHAVIORPAUSE					CKM_BASE + 5
#define CKM_BEHAVIORRESUME					CKM_BASE + 6
#define CKM_BEHAVIORCREATE					CKM_BASE + 7
#define CKM_BEHAVIORRESET					CKM_BASE + 9
#define CKM_BEHAVIORPOSTSAVE				CKM_BASE + 10
#define CKM_BEHAVIORLOAD					CKM_BASE + 11
#define CKM_BEHAVIOREDITED					CKM_BASE + 12
#define CKM_BEHAVIORSETTINGSEDITED			CKM_BASE + 13
#define CKM_BEHAVIORREADSTATE				CKM_BASE + 14
#define CKM_BEHAVIORNEWSCENE				CKM_BASE + 15
#define CKM_BEHAVIORACTIVATESCRIPT			CKM_BASE + 16
#define CKM_BEHAVIORDEACTIVATESCRIPT		CKM_BASE + 17
#define CKM_BEHAVIORRESETINBREAKBPOINT		CKM_BASE + 18
#define CKM_MAX_BEHAVIOR_CALLBACKS			CKM_BASE + 19

//----------------------------------------------------------//
//		StateChunk Versionning								//
//----------------------------------------------------------//

//-------------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

#define CHUNKDATA_OLDVERSION			0	// Before any version was saved
#define CHUNKDATA_BASEVERSION			1	// First version
#define CHUNK_WAVESOUND_VERSION2		2	// Changes in wavesound format
#define CHUNK_WAVESOUND_VERSION3		3	// Changes in wavesound format
#define CHUNK_MATERIAL_VERSION_ZTEST	4	// Change in material save format
#define CHUNK_MAJORCHANGE_VERSION		5	// Optimisations on many save functions	
#define CHUNK_MACCHANGE_VERSION			6	// Misc new Statechunk functions for macintosh (Big-Endian <-> Little Endian conversion )
#define CHUNK_WAVESOUND_VERSION4		7	// Changes in wavesound format (Added sound length)
#define CHUNK_SCENECHANGE_VERSION		8	// Changes in sceneObjectDesc format (Remove lasttimevalue)
#define CHUNK_MESHCHANGE_VERSION		9	// Changes in Mesh save format (primitives)
#define CHUNK_DEV_2_1				   10	// Changes in wavesound reading of inside, outside angles

#define CHUNKDATA_CURRENTVERSION CHUNK_DEV_2_1

// This object declaration declares a Behavior Prototype description.

#define CKDLL_BEHAVIORPROTOTYPE 4

#endif // Docjet secret macro

//----------------------------------------------------------//
//		Class Identifier List								//
//----------------------------------------------------------//

#define  CKCID_OBJECT					1
    #define  CKCID_PARAMETERIN				2
    #define  CKCID_PARAMETEROPERATION		4
    #define  CKCID_STATE					5
    #define  CKCID_BEHAVIORLINK				6
    #define  CKCID_BEHAVIOR					8
    #define  CKCID_BEHAVIORIO				9
    #define  CKCID_RENDERCONTEXT			12
    #define  CKCID_KINEMATICCHAIN			13
    #define  CKCID_SCENEOBJECT				11
        #define  CKCID_OBJECTANIMATION			15
        #define  CKCID_ANIMATION				16
            #define  CKCID_KEYEDANIMATION		18
        #define  CKCID_BEOBJECT					19
            #define	 CKCID_DATAARRAY			52
            #define  CKCID_SCENE				10
            #define  CKCID_LEVEL				21
            #define  CKCID_PLACE				22
            #define  CKCID_GROUP				23
            #define  CKCID_SOUND				24
                #define  CKCID_WAVESOUND		25
                #define  CKCID_MIDISOUND		26
            #define  CKCID_MATERIAL				30
            #define  CKCID_TEXTURE				31
            #define  CKCID_MESH					32
                #define CKCID_PATCHMESH			53
            #define  CKCID_RENDEROBJECT			47
                #define  CKCID_2DENTITY			27
                    #define  CKCID_SPRITE		28
                    #define  CKCID_SPRITETEXT	29
            #define  CKCID_3DENTITY				33
                #define CKCID_GRID				50
                #define  CKCID_CURVEPOINT		36
                #define  CKCID_SPRITE3D			37
                #define  CKCID_CURVE			43
                #define  CKCID_CAMERA			34
                    #define  CKCID_TARGETCAMERA	35
                #define  CKCID_LIGHT			38
                    #define  CKCID_TARGETLIGHT	3
                #define  CKCID_CHARACTER		40
                #define  CKCID_3DOBJECT			41
                    #define  CKCID_BODYPART		42
    #define  CKCID_PARAMETER				46
        #define  CKCID_PARAMETERLOCAL		45
            #define  CKCID_PARAMETERVARIABLE	55
        #define  CKCID_PARAMETEROUT			3
    #define CKCID_INTERFACEOBJECTMANAGER	48
    #define CKCID_CRITICALSECTION			49
    #define CKCID_LAYER						51
    #define CKCID_PROGRESSIVEMESH			54
    #define CKCID_MAXCLASSID				56
    #define CKCID_SYNCHRO					20

//-------------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

//--------- Not CKObject derived classes
//--------- but reserved class IDs
#define  CKCID_OBJECTARRAY				80
#define  CKCID_SCENEOBJECTDESC			81
#define  CKCID_ATTRIBUTEMANAGER			82
#define  CKCID_MESSAGEMANAGER			83
#define  CKCID_COLLISIONMANAGER			84
#define  CKCID_OBJECTMANAGER			85
#define  CKCID_FLOORMANAGER				86
#define  CKCID_RENDERMANAGER			87
#define  CKCID_BEHAVIORMANAGER			88
#define  CKCID_INPUTMANAGER				89
#define  CKCID_PARAMETERMANAGER			90
#define  CKCID_GRIDMANAGER				91
#define  CKCID_SOUNDMANAGER				92
#define  CKCID_TIMEMANAGER				93
#define  CKCID_CUIKBEHDATA				-1

//----------------------------------------------------------//
//		Class registration utilities						//
//----------------------------------------------------------//
typedef void (*CKCLASSREGISTERFCT)();
typedef CKObject *(*CKCLASSCREATIONFCT)(CKContext *context);
typedef CKSTRING (*CKCLASSNAMEFCT)();
typedef CKSTRING (*CKCLASSDEPENDENCIESFCT)(int, int);
typedef int (*CKCLASSDEPENDENCIESCOUNTFCT)(int);

#define CK_GENERALOPTIONS_NODUPLICATENAMECHECK 1 // Classes that don't need to check for duplicate names	when created or loaded
#define CK_GENERALOPTIONS_CANUSECURRENTOBJECT 2	 // Classes that can use an existing object (Meshes,Materials for example)
#define CK_GENERALOPTIONS_AUTOMATICUSECURRENT 4	 // Classes that automatically use an existing object (Synchro objects...)

struct CKClassDesc
{
    int Done;
    // Initialized upon class registration
    CK_CLASSID Parent;							 // Class Identifier of parent class
    CKCLASSREGISTERFCT RegisterFct;				 // Pointer to Class Specific Registration function
    CKCLASSCREATIONFCT CreationFct;				 // Pointer to Class instance creation function
    CKCLASSNAMEFCT NameFct;						 // Pointer to Class name function
    CKCLASSDEPENDENCIESFCT DependsFct;			 // Pointer to Class dependencies function (Copy,delete,replace...)
    CKCLASSDEPENDENCIESCOUNTFCT DependsCountFct; // Pointer to Class dependencies Count function (Copy,delete,replace...)

    // Initialized by class specific registration function
    CKDWORD DefaultOptions; // Default options for this class
    CKDWORD DefaultCopyDependencies;
    CKDWORD DefaultDeleteDependencies;
    CKDWORD DefaultReplaceDependencies;
    CKDWORD DefaultSaveDependencies;
    CKGUID Parameter; // Associated parameter GUID

    // Initialized when building class hierarchy table
    int DerivationLevel;		  // O => CKObject , etc..
    XBitArray Parents;			  // Bit Mask of parents classes
    XBitArray Children;			  // Bit Mask of children classes
    XBitArray ToBeNotify;		  // Mask for Classes that should warn the objects of this class when they are deleted
    XBitArray CommonToBeNotify;	  // idem but merged with sub classes masks
    XSArray<CK_CLASSID> ToNotify; // List of ClassID to notify when an object of this class is deleted (inverse of ToBeNotify)

    CKClassDesc()
    {
        Done = DerivationLevel = 0;
        DefaultOptions = DefaultCopyDependencies = DefaultDeleteDependencies = DefaultReplaceDependencies = DefaultSaveDependencies = 0;
        Parent = 0;
        RegisterFct = NULL;
        CreationFct = NULL;
        NameFct = NULL;
        DependsFct = NULL;
        DependsCountFct = NULL;
    }
};

#define CKCID_MAXMAXCLASSID 128

#endif // Docjet secret macro
/*******************************************************************************
Summary: Plugin description.

Remarks:
+ The m_InitInstanceFct function is called (if present) when a CKContext
is created to enable plugin to register new types or create managers they implement.
+ The m_ExitInstanceFct function is called if the DLL that implements this plugin
is unloaded so that the plugin unregister all the  types it may have registered in m_InitInstanceFct.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        struct CKPluginInfo {
            CKGUID				m_GUID;				// Unique Identifier
            CKFileExtension		m_Extension;		// Supported file extension for a reader plugin
            XString				m_Description;		// Description string
            XString				m_Author;			// Author string
            XString				m_Summary;			// Quick description
            DWORD				m_Version;			// Version
            CK_INITINSTANCEFCT	m_InitInstanceFct;	// Initialization function
            CK_PLUGIN_TYPE		m_Type;				// Type of this plugin
            CK_EXITINSTANCEFCT	m_ExitInstanceFct;	// Exit function
        }

{html:</td></tr></table>}


See also: Creating New Plugins,CKPluginManager,CK_PLUGIN_TYPE
*******************************************************************************/
struct CKPluginInfo
{
    CKGUID m_GUID;
    CKFileExtension m_Extension;
    XString m_Description;
    XString m_Author;
    XString m_Summary;
    DWORD m_Version;
    CK_INITINSTANCEFCT m_InitInstanceFct;
    CK_PLUGIN_TYPE m_Type;
    CK_EXITINSTANCEFCT m_ExitInstanceFct;

    CKPluginInfo()
    {
        m_InitInstanceFct = NULL;
        m_ExitInstanceFct = NULL;
    }

    CKPluginInfo(CKGUID guid, CKFileExtension ext, const char *iDesc, const char *iAuthor, const char *iSummary, DWORD version, CK_INITINSTANCEFCT Initfct, CK_EXITINSTANCEFCT Exitfct, CK_PLUGIN_TYPE type)
        : m_GUID(guid), m_Extension(ext), m_Description(iDesc), m_Author(iAuthor), m_Summary(iSummary), m_Version(version), m_InitInstanceFct(Initfct), m_ExitInstanceFct(Exitfct), m_Type(type)
    {
    }
};

//----------------------------------------------------------//
//		Parameter Types										//
//----------------------------------------------------------//

/********************************************************
Summary: Parameter type (Enumeration) description.

Remarks:
+ New parameter types can be defined as a set of enumeration values by the
CKParameterManager::RegisterNewEnum method.
+ The CKParameterManager::GetEnumDescByType returns this structure to describe
the detail of such a parameter type.
See also : CKParameterManager::RegisterNewEnum,CKParameterManager::GetEnumDescByType
*******************************************/

typedef struct CKEnumStruct
{
public:
    // Summary:Returns the number of enumeration values
    int GetNumEnums() { return NbData; }
    // Summary:Returns the value of the index th enumeration element
    int GetEnumValue(int index) { return Vals[index]; }
    // Summary:Returns the description string of the index th enumeration element.
    CKSTRING GetEnumDescription(int index) { return Desc[index]; }

public:
    int NbData;
    int *Vals;
    CKSTRING *Desc;
} CKEnumStruct;

/********************************************************
Summary: Parameter type (Flags) description.

Remarks:
    + New parameter types can be defined as a combination of flags by the
    CKParameterManager::RegisterNewFlags method.
    + The CKParameterManager::GetFlagsDescByType returns this structure to describe
    the detail of such a parameter type.
See also : CKParameterManager::RegisterNewFlags,CKParameterManager::GetFlagsDescByType
*******************************************/

typedef struct CKFlagsStruct
{
public:
    // Summary:Returns the number of flag values
    int GetNumFlags() { return NbData; }
    // Summary:Returns the value of the index th flag (usually 1,2,4,etc..)
    int GetFlagValue(int index) { return Vals[index]; }
    // Summary:Returns the description string of the index th flag
    CKSTRING GetFlagDescription(int index) { return Desc[index]; }

public:
    int NbData;
    int *Vals;
    CKSTRING *Desc;
} CKFlagsStruct;

/********************************************************
Summary: Parameter type (Structure of parameters) description.

Remarks:
+ New parameter types can be defined as structure of sub-parameters by the
CKParameterManager::RegisterNewStructure method.
+ The CKParameterManager::GetStructDescByType returns this structure to describe
the detail of such a parameter type.
See also : CKParameterManager::RegisterNewStructure,CKParameterManager::GetStructDescByType
*******************************************/

typedef struct CKStructStruct
{
public:
    // Summary:Returns the number of sub parameters in the structure
    int GetNumSubParam() { return NbData; }
    // Summary:Returns the CKGUID of the index th sub parameter in the structure
    CKGUID &GetSubParamGuid(int index) { return Guids[index]; }
    // Summary:Returns the description string of the index th sub parameter in the structure
    CKSTRING GetSubParamDescription(int index) { return Desc[index]; }

public:
    int NbData;
    CKGUID *Guids;
    CKSTRING *Desc;
} CKStructStruct;

//----------------------------------------------------------//
//		Parameter type functions							//
//----------------------------------------------------------//

struct CKPluginEntry;

typedef CKERROR (*CK_PARAMETERCREATEDEFAULTFUNCTION)(CKParameter *);
typedef void (*CK_PARAMETERDELETEFUNCTION)(CKParameter *);
typedef void (*CK_PARAMETERCHECKFUNCTION)(CKParameter *);
typedef void (*CK_PARAMETERREMAPFUNCTION)(CKParameter *, CKDependenciesContext &);
typedef void (*CK_PARAMETERCOPYFUNCTION)(CKParameter *, CKParameter *);
typedef void (*CK_PARAMETERSAVELOADFUNCTION)(CKParameter *param, CKStateChunk **chunk, CKBOOL load);
typedef int (*CK_PARAMETERSTRINGFUNCTION)(CKParameter *param, CKSTRING ValueString, CKBOOL ReadFromString);
typedef WIN_HANDLE (*CK_PARAMETERUICREATORFUNCTION)(CKParameter *param, WIN_HANDLE ParentWindow, CKRECT *rect);

/*************************************************
Summary: Description of a parameter type.

Remarks:
+ This structure is used to register a new parameter type or can be retrieve later
with the CKParameterManager::GetParameterTypeDescription to get the description of
an existing parameter type.
+ Members marked as (used internally) are read only
+ If used to create a new parameter type at least Guid,TypeName and DefaultSize must
be given (See Creating New Parameter Types papers for  sample usages )
+ The only thing that can be changed on a parameter type after its creation is its dwFlags member

Callback function prototypes:

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        typedef CKERROR (*CK_PARAMETERCREATEDEFAULTFUNCTION)(CKParameter*);
        typedef void	(*CK_PARAMETERDELETEFUNCTION)(CKParameter*);
        typedef void	(*CK_PARAMETERCHECKFUNCTION)(CKParameter*);
        typedef void	(*CK_PARAMETERREMAPFUNCTION)(CKParameter*,CKDependenciesContext&);
        typedef void	(*CK_PARAMETERCOPYFUNCTION)(CKParameter*,CKParameter*);
        typedef void	(*CK_PARAMETERSAVELOADFUNCTION)(CKParameter* param,CKStateChunk **chunk,CKBOOL load);
        typedef int		(*CK_PARAMETERSTRINGFUNCTION)(CKParameter* param,CKSTRING ValueString,CKBOOL ReadFromString);
        typedef WIN_HANDLE	(*CK_PARAMETERUICREATORFUNCTION)(CKParameter* param,WIN_HANDLE ParentWindow,CKRECT *rect);

{html:</td></tr></table>}

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        struct CKParameterTypeDesc {
            CKParameterType		Index;
            CKGUID				Guid;
            CKGUID				DerivedFrom;
            XString				TypeName;
            int					Valid;
            int					DefaultSize;
            CK_PARAMETERCREATEDEFAULTFUNCTION	CreateDefaultFunction;
            CK_PARAMETERDELETEFUNCTION			DeleteFunction;
            CK_PARAMETERSAVELOADFUNCTION		SaveLoadFunction;
            CK_PARAMETERCHECKFUNCTION			CheckFunction;
            CK_PARAMETERCOPYFUNCTION			CopyFunction;
            CK_PARAMETERSTRINGFUNCTION			StringFunction;
            CK_PARAMETERUICREATORFUNCTION		UICreatorFunction;
            CKPluginEntry*		CreatorDll;
            CKDWORD				dwParam;
            CKDWORD				dwFlags;
            CKDWORD				Cid;
            XBitArray			DerivationMask;
            CKGUID				Saver_Manager;
        };

{html:</td></tr></table>}

See also: Creating New Parameter Types,CKParameterManager::RegisterParameterType,Pre-Registred Parameter Types
*************************************************/
typedef struct CKParameterTypeDesc
{
    // Index in the parameter array (used internally)
    CKParameterType Index;
    // Glocal Unique identifier to identify this type
    CKGUID Guid;
    // GUID of the parameter type from which this type is derivated
    CKGUID DerivedFrom;
    // Name of this type
    XString TypeName;
    // (used internally)
    int Valid;
    // Default size (in bytes)	of parameters ofthis type
    int DefaultSize;
    // Creation function called each time a parameter of this type is created.
    CK_PARAMETERCREATEDEFAULTFUNCTION CreateDefaultFunction;
    // Deletion function called each time a parameter of this type is deleted.
    CK_PARAMETERDELETEFUNCTION DeleteFunction;
    // Function use to save or load parameters of this type. Only needed if special processing should be done during load and save operations.
    CK_PARAMETERSAVELOADFUNCTION SaveLoadFunction;
    // Function use to check parameters for object utilisation
    CK_PARAMETERCHECKFUNCTION CheckFunction;
    // Function use to copy the value from a parameter to another (Optionnal).
    CK_PARAMETERCOPYFUNCTION CopyFunction;
    // Function to convert a parameter to or from a string.
    CK_PARAMETERSTRINGFUNCTION StringFunction;
    // Function called to create the dialog box when editing this type of parameter.
    CK_PARAMETERUICREATORFUNCTION UICreatorFunction;

    // An index to the registred Dlls from which this type was declared (used internally)
    CKPluginEntry *CreatorDll;
    // An application reserved DWORD for placing parameter type specific data.
    CKDWORD dwParam;
    // Flags specifying special settings for this parameter type (CK_PARAMETERTYPE_FLAGS)
    CKDWORD dwFlags;
    // Special case for parameter types that refer to CKObjects => corresponding class ID of the object
    CKDWORD Cid;
    // Updated by parameter manager...: Bitmask for all class this type can be derived from directly or indirectly (used internally)
    XBitArray DerivationMask;
    // Int Manager GUID
    CKGUID Saver_Manager;

    CKParameterTypeDesc()
    {
        Cid = dwFlags = dwParam = DefaultSize = Valid = 0;
        CreatorDll = NULL;
        UICreatorFunction = 0;
        StringFunction = 0;
        SaveLoadFunction = 0;
        CheckFunction = 0;
        DeleteFunction = 0;
        CopyFunction = 0;
        CreateDefaultFunction = 0;
        DerivedFrom = CKGUID(0, 0);
        Guid = CKGUID(0, 0);
        TypeName = "";
        Saver_Manager = CKGUID(0, 0);
    }
} CKParameterTypeDesc;

//----------------------------------------------------------//
//			Windows messages {Secret}						//
//----------------------------------------------------------//

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

#define CKWM_BASE                 0x600				//
//------------ Parameter Edit Dialog specific messages
#define CKWM_OK                   CKWM_BASE + 1		// Sent by framework (No Param)
#define CKWM_CANCEL               CKWM_BASE + 2		// Sent by framework (No Param)
#define CKWM_SETVALUE             CKWM_BASE + 3		// lParam = Pointer to CKParameter
#define CKWM_GETVALUE             CKWM_BASE + 4		// lParam = Pointer to CKParameter
#define CKWM_INIT                 CKWM_BASE + 5		// Sent by framework (No Param)
#define CKWM_PARAMPICK            CKWM_BASE + 6		//	WParam: screen LPPOINT lParam: Parameter Type
#define CKWM_PICK                 CKWM_BASE + 7		//	WParam: client LPPOINT lParam:CK_CLASSID
#define CKWM_SETPARAMTEXT         CKWM_BASE + 8		// Sent by framework lParam contain a pointer on the text (should return the Handle (HWND) to the control containing the parameter name
#define CKWM_SIZECHANGED          CKWM_BASE + 9		// No Param (Sent by dialog box to warn of change in size)
#define CKWM_PARAMMODIFIED        CKWM_BASE + 11	// No Param (Sent by dialog box to warn of modification of the parameter value)
#define CKWM_CREATEBEHAVIORLOCALS CKWM_BASE + 12	// lParam = CK_ID of Behavior to construct local params
#define CKWM_SETMARGIN            CKWM_BASE + 13	//	WParam: client LPPOINT lParam:CK_CLASSID
#define CKWM_GETMARGIN            CKWM_BASE + 14	//	WParam: client LPPOINT lParam:CK_CLASSID
#define CKWM_STARTPICK            CKWM_BASE + 15	// 
#define CKWM_ENDPICK              CKWM_BASE + 16	// 

#endif // Docjet secret macro
//----------------------------------------------------------//
//			CKGUID HASH Function {Secret}					//
//----------------------------------------------------------//

template <>
struct XHashFun<CKGUID>
{

    int operator()(const CKGUID &__s) const { return __s.d1; }
};

#endif
