/*************************************************************************/
/*	File : CKTypes.h													 */
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKTYPES_H
#define CKTYPES_H "$Id:$"

/*************************************************
{filename:CK_CLASSID}
Summary: Per Class Unique Identifier.

Remarks:
    + Each class derived from the CKObject class has a unique class ID.
    + This ID can be accessed through each instance of these classes, with the
    CKObject::GetClassID method.
    + This class ID is used internally for various matching operations, like matching behaviors on
    objects, etc..
See also: CKObject::GetClassID,CKIsChildClassOf,Class Identifiers
*************************************************/
typedef long CK_CLASSID;

/*************************************************
Summary: Unique Identifier for all Objects instanciated in a given CKContext

Remarks:
    + Each instance of CKObject and derived classes are automatically given a global unique
    ID at creation time. This ID can be accessed through the CKObject::GetID method.
    It is safer, though a bit slower, to reference object through their global ID than through
    a direct pointer reference. In any case the referenced object may be deleted even though 
    the client object has a ID for it.The client object should verify that the referenced object 
    still exists when used with the CKGetObject function.

    + The global ID for an instance remains unique and unchanged through a application session, but there 
    is no garanty that this ID will be the same when a level is saved and loaded back again.
See also: CKObject::GetID, CKContext::GetObject
*************************************************/
typedef unsigned long CK_ID;

//----------------------------------------------------------////
//		Type Definition										////
//----------------------------------------------------------////

typedef char* CKSTRING;			
typedef char CKCHAR;			
typedef int CKBOOL;				
typedef unsigned char CKBYTE;	
typedef unsigned long CKDWORD; 	
typedef unsigned short CKWORD;		
typedef long CKERROR;			
typedef int CKParameterType;	
typedef int CKOperationType;	
typedef int CKMessageType;		
typedef int CKAttributeType;	
typedef int CKAttributeCategory;	

//----------------------------------------------------------////
//		Class  List											////
//----------------------------------------------------------////

// Objects and derivated classes
class CKObject;
    class CKInterfaceObjectManager;
    class CKRenderContext;
    class CKParameterIn;
    class CKParameter;
        class CKParameterOut;
        class CKParameterLocal;
    class CKParameterOperation;
    class CKBehaviorLink;
    class CKBehaviorIO;
    class CKRenderContext;
    class CKSynchroObject;
    class CKStateObject;
    class CKCriticalSectionObject;
    class CKKinematicChain;
    class CKObjectAnimation;
    class CKLayer;
    class CKSceneObject;
        class CKBehavior;	
        class CKAnimation;
            class CKKeyedAnimation;
        class CKBeObject;
            class CKScene;	
            class CKLevel;
            class CKPlace;
            class CKGroup;
            class CKMaterial;
            class CKTexture;
            class CKMesh;
                class CKPatchMesh;
                class CKProgressiveMesh;
            class CKDataArray;
            class CKSound;
                class CKMidiSound;
                class CKWaveSound;
            class CKRenderObject;
                class CK2dEntity;
                    class CKSprite;
                    class CKSpriteText;
                class CK3dEntity;
                    class CKCamera;
                        class CKTargetCamera;
                    class CKCurvePoint;
                    class CKSprite3D;
                    class CKLight;
                        class CKTargetLight;
                    class CKCharacter;
                    class CK3dObject;
                        class CKBodyPart;
                    class CKCurve;
                    class CKGrid;

//---- Misc
class CKBehaviorPrototype;
class CKMessage;
class CK2dCurvePoint;
class CK2dCurve;
class CKStateChunk;
class CKFile;
class CKDependencies;
class CKDependenciesContext;
class CKPluginManager;
class CKDebugContext;
class CKObjectArray;
class CKObjectDeclaration;
class CKContext;
struct CKBitmapProperties;
class CKVertexBuffer;

//--- Managers
class CKBaseManager;
class CKSoundManager;
class CKTimeManager;
class CKRenderManager;
class CKBehaviorManager;
class CKMessageManager;
class CKParameterManager;
class CKAttributeManager;
class CKPathManager;
class CKSceneObjectDesc;

/*******************************************************************************
Summary: Structure containing informations about a .cmo or .nmo file.

Remarks:
    + The CKContext::GetFileInfo returns this structure about a given file 
    giving size,compression and version info.
See also: CKContext::GetFileInfo
*******************************************************************************/
typedef struct CKFileInfo
{
    CKDWORD ProductVersion; // Virtools Version (Dev/Creation). (CK_VIRTOOLS_VERSION)
    CKDWORD ProductBuild;	// Virtools Build Number.
    CKDWORD FileWriteMode;	// Options used to save this file. (CK_FILE_WRITEMODE)
    CKDWORD FileVersion;	// Version of file format when file was saved.
    CKDWORD CKVersion;		// Version of CK when file was saved.
    CKDWORD FileSize;		// Size of file in bytes.
    CKDWORD ObjectCount;	// Number of objects stored in the file.
    CKDWORD ManagerCount;	// Number of managers which saved data in the file.
    CKDWORD MaxIDSaved;		// Maximum Object identifier saved
    CKDWORD Crc;			// Crc of data
    CKDWORD Hdr1PackSize;	// Reserved
    CKDWORD Hdr1UnPackSize; // Reserved
    CKDWORD DataPackSize;	// Reserved
    CKDWORD DataUnPackSize; // Reserved
} CKFileInfo;

#define MAX_USER_PROFILE 8

/*******************************************************************************
Summary: Profiling Statistics.

Remarks:
+ The CKContext::GetProfileStats method can be called in order to retrieve statistic about 
last frame processing. 
+ Times are store in milliseconds.
See also: CKContext::EnableProfiling,CKContext::GetProfileStats,CKContext::UserProfileStart,CKContext::UserProfileEnd
*******************************************************************************/
typedef struct CKStats
{
    float TotalFrameTime;		  // Time elapsed since last frame
    float EstimatedInterfaceTime; // Estimated time for windows and interface (TotalFrameTime - (ProcessTime+RenderTime))
    float ProcessTime;			  // Time during last call to CKProcess
    float RenderTime;			  // Time for rendering last scene
    float ParametricOperations;	  // Time taken by parametric operations last frame
    float TotalBehaviorExecution; // Total behavior Execution time
    float AnimationManagement;	  // During Behavior Execution time : Time taken in character animations processing
    float IKManagement;			  // During Behavior Execution time : Time taken in IK solving
    float BehaviorCodeExecution;  // During Behavior Execution time : Time taken in by code execution of behaviors

    int ActiveObjectsExecuted; // Number of Objects executed last frame
    int BehaviorsExecuted;	   // TotalBehaviorExecuted last frame
    int BuildingBlockExecuted; // TotalBuildingBlock last frame
    int BehaviorLinksParsed;   // Total number of BehaviorLinks Parsed
    int BehaviorDelayedLinks;  // Total number of BehaviorLinks that have been stored as active in N frame

    float UserProfiles[MAX_USER_PROFILE];
} CKStats;
// Warning : Do not insert new values between existing ones CK_PROFILE_CATEGORY direcly refers to this struct by indexes



/****************************************************************
Summary: Render Driver description

Remarks:
+ The VxDriverDesc contains the description of a render driver. A render engine 
may provide several drivers according to the installed video cards.
+ Each driver supports differents pixel formats for the textures (TextureFormats)
and display modes for fullscreen rendering (Modes).
+ Caps2D & Caps3D members expose the different capabilities of the driver
to perform 3D and2D rendering (see  Vx2DCapsDesc & Vx3DCapsDesc)

See Also: CKRenderManager::GetRenderDriverDescription,CKRenderManager::GetRenderDriverCount
****************************************************************/
struct VxDriverDesc
{
    char DriverDesc[512]; // Driver Description
    char DriverName[512]; // Driver Name
    BOOL IsHardware;	  // Is Driver Hardware

    int DisplayModeCount;
    VxDisplayMode *DisplayModes;		   // Fullscreen display modes supported ( see  VxDisplayMode)
    XSArray<VxImageDescEx> TextureFormats; // Texture formats supported by the driver (see VxImageDescEx)

    Vx2DCapsDesc Caps2D; // 2d Capabilities
    Vx3DCapsDesc Caps3D; // 3d Capabilities
};

/******************************************************
Summary: Intersection description.

Remarks:
+ This structure is filled by the CK3dEntity::RayIntersection and the
CKRenderContext::Pick to give the details of the intersection between 
a ray and a mesh.
+ Sprite is only relevant as a result of a pick.
+ The returned distance is the distance between the point of intersection and 
the point of origin of the ray (in the referential given in CK3dEntity::RayIntersection)
See Also: CK3dEntity::RayIntersection, CKRenderContext::Pick
*****************************************************/
struct VxIntersectionDesc
{
    CKRenderObject *Object;		 // Object intersected
    VxVector IntersectionPoint;	 // Intersection point in local coordinates
    VxVector IntersectionNormal; // Normal at the intersection point
    float TexU;					 // U texture coordinate at intersection point
    float TexV;					 // V texture coordinate at intersection point
    float Distance;				 // Distance
    int FaceIndex;				 // Index of the face intersecting with ray or picking
};

/******************************************************
Summary: Rendering Profiling results.

Remarks:
    + The VxStats structure is updated each frame with the number of primitives
    drawn and the differents time taken by 
    is used by CKRenderContext::GetStats to monitor the number of primitives processed.

See Also: CKRenderContext::GetStats
*****************************************************/
typedef struct VxStats
{
    int NbTrianglesDrawn;			  // Number of triangle primitives sent to rasterizer during one frame.
    int NbPointsDrawn;				  // Number of points primitives sent to rasterizer during one frame.
    int NbLinesDrawn;				  // Number of lines primitives sent to rasterizer during one frame.
    int NbVerticesProcessed;		  // Number of vertices transformed during one frame
    int NbObjectDrawn;				  // Number of objects drawn during one frame
    float SmoothedFps;				  // Frame Per Second average.
    int RenderStateCacheHit;		  // Number of render state changes that hit the cache  (redondant state)
    int RenderStateCacheMiss;		  // Number of render state changes that missed the cache
    float DevicePreCallbacks;		  // Time taken by render context pre render callbacks
    float SceneTraversalTime;		  // Time taken to iterate the hierarchy and perform culling
    float TransparentObjectsSortTime; // Time taken to sort transparent 3d objects
    float ObjectsRenderTime;		  // Time taken to draw 3D Objects
                                      // (this time can be very short (especially with T&L cards) as it does not
                                      // reflect the real rendering time , but time taken to send the commands to the card...
    float ObjectsCallbacksTime;		  // Time taken by 3D objects pre and post render callbacks
    float SkinTime;					  // Time taken to compute skins
    float SpriteTime;				  // Time taken to render 2D Entities
    float SpriteCallbacksTime;		  // Time taken by 2D entities pre and post render callbacks
    float DevicePostCallbacks;		  // Time taken by render context post render callbacks
    float BackToFrontTime;			  // Time taken to perfom the back buffer presentation on screen.
                                      // It can be longer due to 3d objects asynchornous drawing(see ObjectsRenderTime)
    float ClearTime;				  // Time taken by clear back,z,(stencil) buffers
    DWORD Reserved1;
} VxStats;

// DO NOT CHANGE THE ORDER of this structure !!!!

/*******************************************************************************
Summary: Global Unique Identifier Struture.

Remarks:
+ Guids are used to uniquely identify plugins,operation types, parameter types and behavior prototypes.
+ Its defined as 

        typedef struct CKGUID {
            union {
                struct { CKDWORD d1,d2; };
                CKDWORD d[2];
                };
        };

+ Comparison operators are defined so CKGUIDS can be compared with 
==,!= ,<,> operators.

See also: Pre-Registred Parameter Types,ParameterOperation Types
*******************************************************************************/

struct CKGUID
{
    union
    {
        struct
        {
            CKDWORD d1, d2;
        };

        CKDWORD d[2];
    };

public:
    explicit CKGUID(CKDWORD gd1 = 0, CKDWORD gd2 = 0)
    {
        d[0] = gd1;
        d[1] = gd2;
    }

    friend CKBOOL operator==(const CKGUID &v1, const CKGUID &v2)
    {
        return ((v1.d[0] == v2.d[0]) && (v1.d[1] == v2.d[1]));
    }

    friend CKBOOL operator!=(const CKGUID &v1, const CKGUID &v2)
    {
        return ((v1.d[0] != v2.d[0]) || (v1.d[1] != v2.d[1]));
    }

    friend CKBOOL operator<(const CKGUID &v1, const CKGUID &v2)
    {
        if (v1.d[0] < v2.d[0])
        {
            return TRUE;
        }

        if (v1.d[0] == v2.d[0])
        {
            return (v1.d[1] < v2.d[1]);
        }

        return FALSE;
    }

    friend CKBOOL operator<=(const CKGUID &v1, const CKGUID &v2)
    {
        return (v1.d[0] <= v2.d[0]);
    }

    friend CKBOOL operator>(const CKGUID &v1, const CKGUID &v2)
    {
        if (v1.d[0] > v2.d[0])
        {
            return TRUE;
        }

        if (v1.d[0] == v2.d[0])
        {
            return (v1.d[1] > v2.d[1]);
        }

        return FALSE;
    }

    friend CKBOOL operator>=(const CKGUID &v1, const CKGUID &v2)
    {
        return (v1.d[0] >= v2.d[0]);
    }

    CKBOOL inline IsValid() { return d[0] && d[1]; }
};

/*

typedef struct CKGUID
{
    union
    {
        struct
        {
            CKDWORD d1, d2;
        };
        CKDWORD d[2];
    };

public:
    explicit CKGUID(CKDWORD gd1 = 0, CKDWORD gd2 = 0)
    {
        d1 = gd1;
        d2 = gd2;
    }

    friend CKBOOL operator==(const CKGUID &v1, const CKGUID &v2)
    {
        return ((v1.d1 == v2.d1) && (v1.d2 == v2.d2));
    }

    friend CKBOOL operator!=(const CKGUID &v1, const CKGUID &v2)
    {
        return ((v1.d1 != v2.d1) || (v1.d2 != v2.d2));
    }

    friend CKBOOL operator<(const CKGUID &v1, const CKGUID &v2)
    {
        if (v1.d1 < v2.d1)
            return TRUE;
        if (v1.d1 == v2.d1)
            return (v1.d2 < v2.d2);
        return FALSE;
    }

    friend CKBOOL operator<=(const CKGUID &v1, const CKGUID &v2)
    {
        return (v1.d1 <= v2.d1);
    }

    friend CKBOOL operator>(const CKGUID &v1, const CKGUID &v2)
    {
        if (v1.d1 > v2.d1)
            return TRUE;
        if (v1.d1 == v2.d1)
            return (v1.d2 > v2.d2);
        return FALSE;
    }

    friend CKBOOL operator>=(const CKGUID &v1, const CKGUID &v2)
    {
        return (v1.d1 >= v2.d1);
    }

    CKBOOL inline IsValid()
    {
        return d1 && d2;
    }
} CKGUID;

*/

/******************************************************************
Summary:  Material special effects

Remarks:
o Effects provide additionnal functionnalities to take advantage of graphic features such as bump mapping,cube maps etc... 
o When an effect is enabled on a material (CKMaterial::SetEffect) it may override the default settings of mesh channels or material blend options
o New effects can be created by providing a callback function (see CKRenderManager::AddEffect)
o This enumeration provides the list of hardcoded existing effects.
o Most of this effect are heavily hardware and device (DX8,DX7,etc..) dependant 
See also: CKMaterial::SetEffect,CKMaterial::GetEffect,CKRenderManager::AddEffect
******************************************************************/
typedef enum VX_EFFECT
{
    VXEFFECT_NONE      = 0UL,	// No Effect
    VXEFFECT_TEXGEN    = 1UL,	// Texture coordinate generation using current viewpoint as referential
    VXEFFECT_TEXGENREF = 2UL,	// texture generation generation with an optionnal referential
    VXEFFECT_BUMPENV   = 3UL,	// Environment Bump Mapping
    VXEFFECT_DP3       = 4UL,	// Dot Product 3 bump mapping
    VXEFFECT_2TEXTURES = 5UL,	// Blend 2 Textures
    VXEFFECT_3TEXTURES = 6UL,	// Blend 3 Textures
    VXEFFECT_MASK      = 0xFUL	// Mask for all possible values.
} VX_EFFECT;

typedef enum VX_EFFECTTEXGEN
{
    VXEFFECT_TGNONE				= 0UL,	// No TexGen
    VXEFFECT_TGTRANSFORM		= 1UL,	// Normal texture coordinates are used transformed by the referential matrix
    VXEFFECT_TGREFLECT			= 2UL,	// Use vertex position and normal to compute a reflection vector used as tex coords
    VXEFFECT_TGCHROME			= 3UL,	// Use vertex normal as tex coords
    VXEFFECT_TGPLANAR			= 4UL,	// Use vertex position as tex coords

    VXEFFECT_TGCUBEMAP_REFLECT	= 31UL,	// Cube Map : Use vertex position and normal to compute a reflection vector used as tex coords
    VXEFFECT_TGCUBEMAP_SKYMAP	= 32UL,	// Cube Map : Use vertex position as tex coords
    VXEFFECT_TGCUBEMAP_NORMALS	= 33UL,	// Cube Map : Use vertex normal as tex coords
    VXEFFECT_TGCUBEMAP_POSITIONS= 34UL,	// Cube Map : Use vertex position a tex coords
} VX_EFFECTTEXGEN;

class CKMaterial;
class CKRenderContext;

/******************************************************************
Summary:  Effect callback return value

Remarks:
o Effects provide additionnal functionnalities to take advantage of graphic features such as bump mapping,cube maps etc... 
o When an effect is enabled on a material (CKMaterial::SetEffect) it may override the default settings of mesh channels or material blend options
o New effects can be created by providing a callback function (see CKRenderManager::AddEffect)
o The return value of the callback function determines which material settings should be set by the default function
See also: CKMaterial::SetEffect,CKMaterial::GetEffect,CKRenderManager::AddEffect,VxEffectDescriptio
******************************************************************/
typedef enum VX_EFFECTCALLBACK_RETVAL
{
    VXEFFECTRETVAL_SKIPNONE		= 0UL,	
    VXEFFECTRETVAL_SKIPTEXMAT	= 1UL,			// Texture matrix has been set by the callback , material must not set it	   
    VXEFFECTRETVAL_SKIPALLTEX	= 2UL,			// Texture stages has been set by the callback , material must not set them	   
    VXEFFECTRETVAL_SKIPALL		= 0xFFFFFFFFUL,	// The Effect has set all the needed render states , no other processing should be done by the material
} VX_EFFECTCALLBACK_RETVAL;

/****************************************************************
Summary: Material effect callback function.

Remarks:
+ A function can be called each time a material is set as current on a rendering context
to set up a special effect.
+ The return value of this callback determines if other render states or texture stages should be set
by the material
See Also: CKMaterial::SetEffect,VxEffectDescription,The Virtools Render Loop,VX_EFFECTCALLBACK_RETVAL
****************************************************************/
typedef VX_EFFECTCALLBACK_RETVAL (*CK_EFFECTCALLBACK)(CKRenderContext *Dev, CKMaterial *mat, int Stage, void *Argument);

/******************************************************************
Summary:  Material special effects

Remarks:
o Effects provide additionnal functionnalities to take advantage of graphic features such as bump mapping,cube maps etc...
o When an effect is enabled on a material (CKMaterial::SetEffect) it may override the default settings of mesh channels or material blend options
o This structure holds a description of an effect to display to the user.
See also: VX_EFFECT,CKRenderManager::AddEffect,CKRenderManager::GetEffectDescription,CKMaterial::SetEffect,CKMaterial::GetEffect
******************************************************************/
typedef struct VxEffectDescription
{
    VX_EFFECT EffectIndex;		   // Index of this effect ( when using CKRenderManager::AddEffect the return value is the index of the newly added effect) so this member is ignored.
    XString Summary;			   // A short name that will be used for enumeration creation (eg. "Bump Map")
    XString Description;		   // A longer description that will appear in the interface to explain the effect to the user...
    XString DescImage;			   // A image file that can be use to be displayed in the interface
    int MaxTextureCount;		   // Number of textures that can be set on the material (CKMaterial::SetTexture)
    int NeededTextureCoordsCount;  // Number of texture coordinates that must be set on the mesh  for this effect to work (this is directly related to the number of channel you should set on the mesh that will use this effect)
    XString Tex1Description;	   // A short description for texture 1
    XString Tex2Description;	   // A short description for texture 2
    XString Tex3Description;	   // A short description for texture 3
    CK_EFFECTCALLBACK SetCallback; // A callback function that will be called to setup the effect if NULL the effect is likely to be hardcoded in render engine
    void *CallbackArg;			   // Arguments that will be given to the callback function
    CKGUID ParameterType;
    XString ParameterDescription;
    XString ParameterDefaultValue;
    VxEffectDescription() : CallbackArg(NULL), SetCallback(NULL), MaxTextureCount(0), EffectIndex(VXEFFECT_NONE), ParameterType(0, 0) {}
} VxEffectDescription;

/******************************************************
Summary : Structure passed to behavior execution and callback functions

Remarks:
+ The execution function of a behavior is given this structure as a argument
to have acces to frequently used global objects (context,level,manager,etc...)
+ the callback function is also passed this structure in which the reason
for callback is given in CallbackMessage.
See Also: Writing the Execution Function,Writing the Behavior Callback Function,CKBehavior::SetCallbackFunction
*******************************************************/
struct CKBehaviorContext
{
    CKBehavior *Behavior;	// Pointer to executed behavior
    float DeltaTime;		// Last Delta-Time in milliseconds since previous frame.
    CKContext *Context;		// The CKContext this behavior belongs to.
    CKLevel *CurrentLevel;	// A Pointer to the current CKLevel
    CKScene *CurrentScene;	// A Pointer to the current CKScene
    CKScene *PreviousScene; // A Pointer to the previous CKScene if available

    CKRenderContext *CurrentRenderContext; // A Pointer to the main CKRenderContext
    CKParameterManager *ParameterManager;  // A Pointer to the ParameterManager
    CKMessageManager *MessageManager;	   // A Pointer to the MessageManager
    CKAttributeManager *AttributeManager;  // A Pointer to the AttributeManager
    CKTimeManager *TimeManager;			   // A Pointer to the TimeManager
    CKDWORD CallbackMessage;			   // Specific to behavior callbck function, reason for
    void *CallbackArg;					   //

    CKBehaviorContext()
    {
        CurrentLevel = NULL;
        CurrentScene = NULL;
        PreviousScene = NULL;
        CurrentRenderContext = NULL;
    }
};

class XObjectArray;

typedef enum CK_UICALLBACK_REASON
{
    CKUIM_LOADSAVEPROGRESS      = 1,	// NbObjectLoaded,NbObjetsToLoad
    CKUIM_DEBUGMESSAGESEND      = 2,	// DebugMessageSent
    CKUIM_OUTTOCONSOLE          = 3,	// DoBeep,ConsoleString
    CKUIM_OUTTOINFOBAR          = 4,	// ConsoleString
    CKUIM_SHOWSETUP             = 5,	// Objects
    CKUIM_SELECT                = 6,	// ObjectID
    CKUIM_CHOOSEOBJECT          = 7,	// Return value in ObjectID
    CKUIM_EDITOBJECT            = 8,	// ID of object in Param1 / ObjectID
    CKUIM_CREATEINTERFACECHUNK  = 9,	// Objects
    CKUIM_COPYOBJECTS           = 10,	// Copy CK_ID objects
    CKUIM_PASTEOBJECTS          = 11,	// Paste CK_ID objects
    CKUIM_SCENEADDEDTOLEVEL     = 12,	// Scene ID
    CKUIM_REFRESHBUILDINGBLOCKS = 13,	// param1 = CKGUID*, param2 = count
    CKUIM_DOMESSAGELOOP         = 14,	// Do a standard loop of windows message processing
    CKUIM_VSLBREAKNOTIFY        = 15,	// First param is a VSLEditor order, second is data.
    CKUIM_DUPLICATEOBJECTS      = 16,	// First param is a CKfile being load with duplicate objects.
    CKUIM_OPENVSLSCRIPTEDITOR   = 17,	// Request to open VSL Script Editor, or focus it
} CK_UICALLBACK_REASON;

typedef struct CKUICallbackStruct
{
    CKDWORD Reason;
    union
    {
        CKDWORD Param1;
        int NbObjetsLoaded;
        CKBOOL DoBeep;
        CKMessage *DebugMessageSent;
        CK_ID ObjectID;
        XObjectArray *Objects;
        CKFile *File;
    };
    union
    {
        CKDWORD Param2;
        int NbObjetsToLoad;
        CKSTRING ConsoleString;
        int ClearSelection;
    };
    CKDWORD Param3;
} CKUICallbackStruct;

#endif