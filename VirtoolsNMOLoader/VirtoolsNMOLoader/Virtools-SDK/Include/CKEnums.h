/*************************************************************************/
/*	File : CKEnums.h				 				 					 */
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 1999, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKENUMS_H

#define CKENUMS_H "$Id:$"


/***************************************************
{filename:CK_OBJECT_FLAGS}
Summary: CKObject Flags

Remarks:
+ Flags specifying special settings for basic objects.
+ Some of this flags are shared with sub-classes such as CKParameterIn,CKParameterOut and CKBehaviorIO.
+ You rarely need to modify directly this flags through CKObject::SetFlags or CKObject::ModifyObjectFlags instead
you should always use the specific acces function (given between ()) which may need to perform additionnal operations.
See also: CKObject,CKObject::GetObjectFlags,CKObject::ModifyObjectFlags
*************************************************/
typedef enum CK_OBJECT_FLAGS {
	CK_OBJECT_INTERFACEOBJ			=	 0x00000001,		// Reserved for Inteface Use
	CK_OBJECT_PRIVATE				=	 0x00000002,		// The object must not be displayed in interface (Lists,Level view,etc...),nor should it be saved. (CKObject::IsPrivate()
	CK_OBJECT_INTERFACEMARK			=	 0x00000004,		
	CK_OBJECT_FREEID				=	 0x00000008,		// ID of this object can be released safely and is free to be reused by future CKobjects.
	CK_OBJECT_TOBEDELETED			=	 0x00000010,		// This object is being deleted
	CK_OBJECT_NOTTOBESAVED			=	 0x00000020,		// This object must not be saved 
	CK_OBJECT_VISIBLE				=	 0x00000040,		// This object is visible (CKObject::Show)
	CK_OBJECT_NAMESHARED			=	 0x00000080,		// This object has its name from another entity
	CK_OBJECT_DYNAMIC				=	 0x00000108,		// This object may be created or deleted at run-time, it also contails CK_OBJECT_FREEID. (CKObject::IsDynamic,CKContext::CreateObject)
	CK_OBJECT_HIERACHICALHIDE		=	 0x00000200,		// This object hides all its hierarchy (CKObject::Show)
	CK_OBJECT_UPTODATE				=	 0x00000400,		// (Camera,etc..)
	CK_OBJECT_TEMPMARKER			=	 0x00000800,		
	CK_OBJECT_ONLYFORFILEREFERENCE	=	 0x00001000,		
	CK_OBJECT_NOTTOBEDELETED		=	 0x00002000,		// This object must not be deleted in a clear all
	CK_OBJECT_APPDATA				=	 0x00004000,		// This object has app data	
	CK_OBJECT_SINGLEACTIVITY		=	 0x00008000,		// this object has an information of single activity (active at scene start,etc..)
	CK_OBJECT_LOADSKIPBEOBJECT		=	 0x00010000,		// When loading this object the CKBeObject part should be skipped
	CK_OBJECT_KEEPSINGLEACTIVITY	=	 0x00020000,		// this object must keep its information of single activity (active at scene start,etc..)
	CK_OBJECT_LOADREPLACINGOBJECT	=	 0x00040000,		// Indicates the object being loaded is being replaced
	
	CK_OBJECT_NOTTOBELISTEDANDSAVED	=	 0x00000023,		// Combination of Private and Not To Be Saved
	CK_OBJECT_SELECTIONSET			=	 0x00080000,		// if group, then it is a selection set, otherwise, temporary flag used for objects belonging to a selection set. Used by Virtools's Interface
	CK_OBJECT_VR_DISTRIBUTED		=	 0x00100000,		// distributed object for VR (ie mainly used for distributed parameters for VR)


// The following flags are specific to parameters (they are stored here for object's size purposes )
	CK_PARAMETEROUT_SETTINGS		=	0x00400000,			
	CK_PARAMETEROUT_PARAMOP			=	0x00800000,			// This parameter is the output of a CKParameterOperation (Automatically set by Engine)
	CK_PARAMETERIN_DISABLED			=	0x01000000,			// Parameter In or Out is disabled (CKBehavior::EnableInputParameter,CKBehavior::DisableInputParameter)	
	CK_PARAMETERIN_THIS				=	0x02000000,			// Special parameter type : its value and type are always equal to its owner (CKParameter::SetAsMyselfParameter)
	CK_PARAMETERIN_SHARED		    =	0x04000000,			
	CK_PARAMETEROUT_DELETEAFTERUSE 	=	0x08000000,			// When adding parameters to CKMessage, they can be automatically deleted when message is released (CKMessage::AddParameter)
	CK_OBJECT_PARAMMASK			  	=	0x0FC00000,			// Mask for options specific to parameters	

// The Following flags are specific	for Behavior ios (CKBehaviorIO)
	CK_BEHAVIORIO_IN				=	0x10000000,		// This BehaviorIO is a behavior input  (CKBehaviorIO::SetType}
	CK_BEHAVIORIO_OUT				=	0x20000000,		// This BehaviorIO is a behavior output (CKBehaviorIO::SetType)	
	CK_BEHAVIORIO_ACTIVE			=	0x40000000,		// This BehaviorIO is a currently active (CKBehaviorIO::Activate}
	CK_OBJECT_IOTYPEMASK		  	=	0x30000000,		
	CK_OBJECT_IOMASK			  	=	0xF0000000,		

	// The Following flags are specific	for Behavior ios (CKBehaviorIO)
	CKBEHAVIORLINK_RESERVED			=	0x10000000,		// This BehaviorIO is a behavior input  (CKBehaviorIO::SetType}
	CKBEHAVIORLINK_ACTIVATEDLASTFRAME =	0x20000000,		// This link had been activated last frame
	CK_OBJECT_BEHAVIORLINKMASK		=	0x30000000,		
} CK_OBJECT_FLAGS;


/*************************************************
{filename:CK_3DENTITY_FLAGS}
Summary: 3dEntity Flags
Remarks:
	+ Flags give user and engine more information about the 3dEntity.

See also: CK3dEntity::SetFlags,CK3dEntity::GetFlags,
*************************************************/
typedef enum CK_3DENTITY_FLAGS {
		CK_3DENTITY_DUMMY				=	0x00000001,		// Entity is a dummy used to represent a position
		CK_3DENTITY_FRAME				=	0x00000002,		// Entity is a frame used to represent an orientation
		CK_3DENTITY_RESERVED0			=	0x00000020,		// Obsolete Flag
		CK_3DENTITY_TARGETLIGHT			=	0x00000100,		// Entity is a target of a light
		CK_3DENTITY_TARGETCAMERA		=	0x00000200,		// Entity is a target of a camera
		CK_3DENTITY_IGNOREANIMATION		=	0x00000400,		// Animation using this entity can't modify it
		CK_3DENTITY_HIERARCHICALOBSTACLE=	0x00000800,		// Used by the Collision Manager
		CK_3DENTITY_UPDATELASTFRAME     =   0x00001000,		// Store the last world matrix for this Entity after each render
		CK_3DENTITY_CAMERAIGNOREASPECT  =   0x00002000,		// Ignore aspect ratio setting for cameras
		CK_3DENTITY_DISABLESKINPROCESS	=   0x00004000,		// Force skin processing to be disabled
		CK_3DENTITY_ENABLESKINOFFSET	=   0x00008000,		// If not set the skin stay attached to the bones the vertices are linked to, otherwise the skin can be freely rotated,translated or scaled according to its owner entity matrix.
		CK_3DENTITY_PLACEVALID			=   0x00010000,		// Used internally when saving 
		CK_3DENTITY_PARENTVALID			=	0x00020000,		// Used internally when saving 
		CK_3DENTITY_IKJOINTVALID        =   0x00030000,
		CK_3DENTITY_DEPRECATEDIK		=	0x00040000,		// secret Obsolete Flag
		CK_3DENTITY_PORTAL				=	0x00080000,		// The 3dEntity is a portal
		CK_3DENTITY_ZORDERVALID			=	0x00100000,		// The 3dEntity has a non-zero ZOrder

		CK_3DENTITY_HWSKINING			=	0x40000000,		// Special flag for Skins : Use a shader for skinning
		CK_3DENTITY_CHARACTERDOPROCESS	=	0x80000000,		// Special flag for Characters : Automatic process of animation
} CK_3DENTITY_FLAGS;


/*****************************************************************
{filename:VX_MOVEABLE_FLAGS}
Summary: 3dEntity additionnal flags Options

Remarks:
	+ The VX_MOVEABLE_FLAGS is used by CK3dEntity::SetMoveableFlags to specify different hints to the render engine about the entity.
	+ The (Engine) flags are set by the render engine and should not be modified by user. They can be checked with the CK3dEntity::GetMoveableFlags method.
	+ The (User) flags are to be set by the user or can be set by a specific method of CK3dEntity.

See Also: CK3dEntity::SetMoveableFlags
******************************************************************/
typedef enum VX_MOVEABLE_FLAGS { 
		VX_MOVEABLE_PICKABLE				=0x00000001,	// (User)If not set this entity cannot be returned by CKRenderContext::Pick() or CKRenderContext::RectPict() functions.
		VX_MOVEABLE_VISIBLE					=0x00000002,	// (Engine) See CKObject::Show,CK3dEntity::IsVisible
		VX_MOVEABLE_UPTODATE				=0x00000004,	// (Engine) Used to Notify change in the data of the entity.
		VX_MOVEABLE_USERBOX					=0x00000010,	// (Engine) When CK3dEntity::SetBoundingBox is called with a user box, this flag is set.
		VX_MOVEABLE_EXTENTSUPTODATE			=0x00000020,	// (Engine) Indicate that object 2D extents are up to date
		VX_MOVEABLE_BOXVALID				=0x00004000,	// (Engine) If not set the moveable has no mesh associated so its bounding box is irrelevant (a point).
		VX_MOVEABLE_RENDERLAST				=0x00010000,	// (User) If set the moveable will be rendered with the transparent objects (i.e in last) (CK3dEntity::SetRenderAsTransparent)
		VX_MOVEABLE_HASMOVED				=0x00020000,	// (Engine) Set when its position or orientation has changed. (Reset every frame when rendering starts) 
		VX_MOVEABLE_WORLDALIGNED			=0x00040000,	// (User) Hint for render engine : this object is aligned with world position and orientation.
		VX_MOVEABLE_NOZBUFFERWRITE			=0x00080000,	// (User) Set by the user to warn Render Engine that this object must not write information to Z buffer
		VX_MOVEABLE_RENDERFIRST				=0x00100000,	// (User) If set the moveable will be rendered within the firsts objects
		VX_MOVEABLE_NOZBUFFERTEST			=0x00200000,	// (User) Set by the user to warn Render Engine that this object must not test against Z buffer (This override settings of all materials used by this Entity)
		VX_MOVEABLE_INVERSEWORLDMATVALID	=0x00400000,	// (Engine) Inverse world matrix is not up to date and should be recomputed
		VX_MOVEABLE_DONTUPDATEFROMPARENT	=0x00800000,	// (User) This object will not be updated by parent (neither World nor Local matrix wil be updated) . This flags can be used by physic engine for example in which hierarchy is not relevant for physicalised objects
		VX_MOVEABLE_INDIRECTMATRIX			=0x01000000,	// (User/Engine) Set by the engine at load time  : The object matrix is in left hand referential, culling needs to be inverted
		VX_MOVEABLE_ZBUFONLY				=0x02000000,	// (User) The object will only be rendered in depth buffer
		VX_MOVEABLE_STENCILONLY				=0x04000000,	// (User) The object will only be rendered in stencil buffer
		VX_MOVEABLE_HIERARCHICALHIDE		=0x10000000,	// (Engine) If Object has this flags and is hidden its children won't be rendered
		VX_MOVEABLE_CHARACTERRENDERED		=0x20000000,	// (Engine) Set if a character was rendered last frame...
		VX_MOVEABLE_RESERVED2				=0x40000000,	// (Engine)
} VX_MOVEABLE_FLAGS;


/*****************************************************************
Summary:Mesh Flags Options

Remarks:
	+ The VXMESH_FLAGS is used by CKMesh::SetFlags to specify different hints to the render engine about the mesh.
	+ Most of this flags can be set or asked using the appropriate method of CKMesh (given between () in the members documentation).
See Also: CKMesh,CKMesh::SetFlags
******************************************************************/
typedef enum VXMESH_FLAGS
{
		VXMESH_BOUNDINGUPTODATE		= 0x00000001,	//	If set the bounding box is up to date (internal).
		VXMESH_VISIBLE				= 0x00000002,	//	If not set the mesh will not be rendered (CKMesh::Show)
		VXMESH_OPTIMIZED			= 0x00000004,	//	Set by the render engine if the mesh is optimized for rendering. Unset it to force to recreate optimized structures (when changing materials or face organization ) (CKMesh::VertexMove)
		VXMESH_FACENORMALSCOMPUTED	= 0x00000008,	//	Set by the render engine if the mesh face normal information is valid (internal)
		VXMESH_HASTRANSPARENCY		= 0x00000010,	//	If set indicates that one or more of the faces of this mesh use a transparent material (internal)
		VXMESH_PRELITMODE			= 0x00000080,	//	If set, no lightning should occur for this mesh, vertex color should be used instead (CKMesh::SetLitMode)
		VXMESH_WRAPU				= 0x00000100,	//	Texture coordinates wrapping among u texture coordinates. (CKMesh::SetWrapMode) 
		VXMESH_WRAPV				= 0x00000200,	//	Texture coordinates wrapping among v texture coordinates. (CKMesh::SetWrapMode)
		VXMESH_FORCETRANSPARENCY	= 0x00001000,	//	Forces this mesh to be considered as transparent even if no material is tranparent. (CKMesh::SetTransparent)
		VXMESH_TRANSPARENCYUPTODATE	= 0x00002000,	//	If set, the flags VXMESH_HASTRANSPARENCY is up to date. (internal) 
		VXMESH_UV_CHANGED			= 0x00004000,	//	Must be set if texture coordinates changed to enable the render engine to reconstruct potential display lists or vertex buffers. (CKMesh::UVChanged)
		VXMESH_NORMAL_CHANGED		= 0x00008000,	//	Must be set if normal coordinates changed to enable the render engine to reconstruct potential display lists or vertex buffers. (CKMesh::NormalChanged)
		VXMESH_COLOR_CHANGED		= 0x00010000,	//	Must be set if colors changed to enable the render engine to reconstruct potential display lists or vertex buffers.	(CKMesh::ColorChanged)
		VXMESH_POS_CHANGED			= 0x00020000,	//	Must be set if vertex position changed to enable the render engine to reconstruct potential display lists or vertex buffers. (CKMesh::VertexMove)
		VXMESH_HINTDYNAMIC			= 0x00040000,	//  Hint for render engine : Mesh geometry is updated frequently
		VXMESH_GENNORMALS			= 0x00080000,	//  Hint : Normals were generated by BuildNormals : Do not save	(internal)
		VXMESH_PROCEDURALUV			= 0x00100000,	//  Hint : UVs are generated : Do not save (internal)
		VXMESH_PROCEDURALPOS		= 0x00200000,	//  Hint : Vertices postions are generated : Do not save (internal)
		VXMESH_STRIPIFY				= 0x00400000,	//  If set the mesh will be stripified.
		VXMESH_MONOMATERIAL			= 0x00800000,	//  Set by the render engine if the mesh use only one material.
		VXMESH_PM_BUILDNORM			= 0x01000000,	//  Build normals when performing progressive meshing : Do not save (internal)
		VXMESH_BWEIGHTS_CHANGED		= 0x02000000,	//	Must be set if vertex blend weights have changed to enable the render engine to reconstruct potential display lists or vertex buffers. (CKMesh::VertexMove)
		VXMESH_GENTANSPACE			= 0x04000000,	//	Generates tangent space vectors.
		VXMESH_UPDATEINDEXBUF		= 0x08000000,	//	Index Buffer should be updated...
		VXMESH_USECOLORANDNORMAL	= 0x10000000,	//	Use Color AND Normal in the vertex buffer.
		VXMESH_ALLFLAGS				= 0x1FFFF39F
} VXMESH_FLAGS;



enum CKAXIS {
		CKAXIS_X,
		CKAXIS_Y,
		CKAXIS_Z
};


enum CKSUPPORT {
		CKSUPPORT_FRONT,
		CKSUPPORT_CENTER,
		CKSUPPORT_BACK
};

/*************************************************
{filename:CKSPRITETEXT_ALIGNMENT}
Summary: Sprite Text Alignment
Remarks: 
	+ Alignment attributes of the text in a CKSpriteText

See also: CKSpriteText, CKSpriteText::SetAlign
*************************************************/
typedef enum CKSPRITETEXT_ALIGNMENT {
			CKSPRITETEXT_CENTER		=0x00000001,		// Text is centered when written in the sprite
			CKSPRITETEXT_LEFT		=0x00000002,		// Text is aligned to the left of the sprite
			CKSPRITETEXT_RIGHT		=0x00000004,		// Text is aligned to the right of the sprite
			CKSPRITETEXT_TOP		=0x00000008,		// Text is aligned to the top of the sprite
			CKSPRITETEXT_BOTTOM		=0x00000010,		// Text is aligned to the bottom of the sprite
			CKSPRITETEXT_VCENTER	=0x00000020,		// Text is centered verticaly when written in the sprite
			CKSPRITETEXT_HCENTER 	=0x00000040,		// Text is centered horizontaly when written in the sprite
} CKSPRITETEXT_ALIGNMENT;



/****************************************************************
{filename:VXTEXTURE_WRAPMODE}
Summary: Wrapping Flags

Remarks:
	+ The VXTEXTURE_WRAPMODE is used by CKMesh::SetWrapMode() to specify how
	texture coordinates are interpolated.
See Also: Using Materials,CKMaterial,CKMesh::SetWrapMode.
****************************************************************/
typedef enum VXTEXTURE_WRAPMODE
{
	VXTEXTUREWRAP_NONE = 0x00000000,	// Flat texture addressing
	VXTEXTUREWRAP_U	   = 0x00000001,	// Vertical	cylinder mapping
	VXTEXTUREWRAP_V	   = 0x00000002,	// Horizontal cylinder mapping
	VXTEXTUREWRAP_UV   = 0x00000003,	// Spherical mapping
} VXTEXTURE_WRAPMODE;




/*****************************************************************
{filename:VXSPRITE3D_TYPE}
Summary: Sprite3D Orientation Options

Remarks:
	+ The VXSPRITE3D_TYPE is used by CKSprite3D::SetMode to specify how the orientation of the sprite3D is calculated.
See Also: CKSprite3D::SetMode,CKSprite3D::GetMode
******************************************************************/
typedef enum VXSPRITE3D_TYPE
{
	VXSPRITE3D_BILLBOARD = 0,		// Orientation is set to always face the camera
	VXSPRITE3D_XROTATE,				// Orientation is set to face the camera but X axis is locked.
	VXSPRITE3D_YROTATE,				// Orientation is set to face the camera but Y axis is locked (typically a tree for example).
	VXSPRITE3D_ORIENTABLE,			// Orientation of the sprite3D is free
} VXSPRITE3D_TYPE;


/******************************************************************
{filename:VXMESH_LITMODE}
Summary: Mesh lighting options

Remarks:
  + The VXMESH_LITMODE is used by CKMesh::SetLitMode to specify how lighting is done.
See Also: CKMaterial,CKMesh
******************************************************************/
typedef enum VXMESH_LITMODE
{
	 VX_PRELITMESH = 0,		// Lighting use color information store with vertices
	 VX_LITMESH	   = 1,		// Lighting is done by renderer using normals and face material information. 
} VXMESH_LITMODE;

/******************************************************************
{filename:VXSHADOWMAP_FORMAT}
Summary: Texture format used by shadow maps

******************************************************************/
enum VXSHADOWMAP_FORMAT
{	
	VXSHADOWMAP_DEBUGMATERIAL = 0,     // For debug : Display Triangles / Objects where shadows are received
	VXSHADOWMAP_HWSHADOWMAP24S8,       // Hardware shadow map with 24 bits depth buffer	
	VXSHADOWMAP_HWSHADOWMAP16,         // Hardware shadow map -> supports PCF in hardware (GeForce 3 or more)			
	VXSHADOWMAP_FLOAT32WBUFFER,        // Float32 shadow map with no PCF (Need filtering in the shader)	
	VXSHADOWMAP_FLOAT16WBUFFER,        // Float16 shadow map (Depth written in R component) with no PCF (Need filtering in the shader)	
	//	
	VXSHADOWMAP_FORMATCOUNT,
	VXSHADOWMAP_UNKNOWNFORMAT = VXSHADOWMAP_FORMATCOUNT
};

// Number of taps when sampling a shadow map
enum VXSHADOWMAP_TAPCOUNT
{		
	VXSHADOWMAP_1TAP = 0,
	VXSHADOWMAP_5TAPS,
	VXSHADOWMAP_9TAPS,
	VXSHADOWMAP_13TAPS
};

// Number of taps when sampling a shadow map
enum VXSHADOWMAP_ATTENUATIONMODE
{		
	VXSHADOWMAP_NOATTENUATION = 0,
	VXSHADOWMAP_LIGHTFACTORS,	        // use light attenuation factor and range
	VXSHADOWMAP_DIRECTIONALATTENUATION // directionnal attenuation starting from the light casting shadows
};

enum VXSHADOWMAP_FALLBACKPOLICY
{
	VXSHADOWMAP_NOFALLBACK = 0,
	VXSHADOWMAP_PREFERSAMPLING,
	VXSHADOWMAP_PREFERFILTERING,
};

/*****************************************************************
{filename:VXCHANNEL_FLAGS}
Summary:Mesh additionnal material channel options

Remarks:
	+ The VXCHANNEL_FLAGS is used by CKMesh::SetChannelFlags to give the behavior of 
	an additional material channel.
See Also: CKMesh,CKMesh::AddChannel,CKMesh::IsChannelLit,CKMesh::IsChannelActive
******************************************************************/
typedef enum VXCHANNEL_FLAGS {
	VXCHANNEL_ACTIVE	=	0x00000001,		// This channel is active
	VXCHANNEL_SAMENORMAL =	0x00000002,	    // Force to include normals from the mesh in this channel (if the channel has a material with an effect, then normals are added automatically, so specifying this flag is unecessary)
	VXCHANNEL_VERTEXCOLOR =	0x00000004,		// Force the channel to include its own vertex colors (diffuse color / vertex color 0)
	VXCHANNEL_HINT_USEORIGINALMESHVB = 0x00000008,  // Hint : try to use the mesh original vb when possible, instead of creating an additional vertex buffer for the channel. when this is possible,
	                                                // the shared vb used for that channel will be deallocated
	VXCHANNEL_KEEPFOG	=	0x00400000,		// By default fog is deactivated when rendering channels in multipass.. This flag prevent this from happening.
	VXCHANNEL_SAMEUV	=	0x00800000,		// This channel should use the texture coordinates of the base mesh.
	VXCHANNEL_NOTLIT	=	0x01000000,		// Additionnal Material Channel should not be lit (some channels may not be rendered in one pass with this option)
	VXCHANNEL_MONO		=	0x02000000,		// Internal use : Set at runtime by render engine to indicate whether this channel was rendered using multiple pass or not.(Dot Not Modify)
	VXCHANNEL_RESERVED1  =	0x04000000,		// Internal use : Reserved
	VXCHANNEL_LAST		=	0x08000000,		// Internal use : Set at runtime by render engine to indicate this channel isthe last to be rendered. Dot Not Modify 
	VXCHANNEL_UPTODATE	=	0x10000000,		// Internal use : Vertex data hold in vertex buffer is uptodate... 
	VXCHANNEL_SUBSET	=	0x20000000,		// Internal use : Channel should only use a subset of the face indices... If not set, then indices of the channel cover the whole mesh,
	                                        // as a result it may be possible to merge channel with material to do a single pass render
	VXCHANNEL_USEORIGINALMESHVB = 0x40000000, // Internal use : The channel can use the original mesh vb instead of creating its own
	VXCHANNEL_MATERIALSUBSET = 0x80000000     // Internal use : The channel match a material subset (that is, all triangles that use a given material for that mesh, for one or more materials)
	                                          // and so not modify the original mesh vb. As a result it can resue the index buffer of these material for rendering
	
} VXCHANNEL_FLAGS;

/****************************************************************
{filename:VxShadeType}
Summary: Global Shade Mode

Remarks:
	+ The VxShadeType describes a global render mode which is used for all objects in a context.
See Also: CKRenderContext::SetGlobalRenderMode
****************************************************************/
typedef enum VxShadeType
{
	WireFrame		=0,		// Rendering will use settings of materials for rendering
	FlatShading		=1,		// All Objects will be rendered	using flat shading
	GouraudShading	=2,		// All Objects will be rendered	using gouraud shading
	PhongShading	=3,		// All Objects will be rendered	using phong	shading
	MaterialDefault	=4		// All Objects will be rendered	using their own shade mode
} VxShadeType;




typedef enum CK_SCENE_FLAGS {
			CK_SCENE_LAUNCHEDONCE			=0x00000001,		// Set after the scene was initialised
			CK_SCENE_USEENVIRONMENTSETTINGS	=0x00000002,		// Use the env. values on scene activation
}	CK_SCENE_FLAGS;


/************************************************************
{filename:CK_BEHAVIOR_FLAGS}
Summary: Flags settings for behaviors.
Remarks:
	+ When creating a prototype, you can precise various flags
	about how your behavior will act: whether it will send or receive message, 
	does the user may add inputs,outputs or parameters, is it active, etc. 

See also: CKBehaviorPrototype::SetBehaviorFlags,Behavior Prototype Creation
**********************************************************/
typedef enum CK_BEHAVIOR_FLAGS {
			CKBEHAVIOR_NONE							=0x00000000,	// Reserved for future use
			CKBEHAVIOR_ACTIVE						=0x00000001,	// This behavior is active
			CKBEHAVIOR_SCRIPT						=0x00000002,	// This behavior is a script
			CKBEHAVIOR_RESERVED1					=0x00000004,	// Reserved for internal use.
			CKBEHAVIOR_USEFUNCTION					=0x00000008,	// Behavior uses a function and not a graph
			CKBEHAVIOR_RESERVED2					=0x00000010,	// Reserved for internal use.
			CKBEHAVIOR_CUSTOMSETTINGSEDITDIALOG		=0x00000020,	// Behavior has a custom Dialog Box for settings edition .
			CKBEHAVIOR_WAITSFORMESSAGE				=0x00000040,	// Behavior is waiting for a message to activate one of its outputs
			CKBEHAVIOR_VARIABLEINPUTS				=0x00000080,	// Behavior may have its inputs changed by editing them
			CKBEHAVIOR_VARIABLEOUTPUTS				=0x00000100,	// Behavior may have its outputs changed by editing them
			CKBEHAVIOR_VARIABLEPARAMETERINPUTS		=0x00000200,	// Behavior may have its number of input parameters changed by editing them
			CKBEHAVIOR_VARIABLEPARAMETEROUTPUTS		=0x00000400,	// Behavior may have its number of output parameters changed by editing them
			CKBEHAVIOR_TOPMOST						=0x00004000,	// No other Behavior includes this one
			CKBEHAVIOR_BUILDINGBLOCK				=0x00008000,	// This Behavior is a building block. Automatically set by the engine when coming from a DLL.
			CKBEHAVIOR_MESSAGESENDER				=0x00010000,	// Behavior may send messages during its execution.
			CKBEHAVIOR_MESSAGERECEIVER				=0x00020000,	// Behavior may check messages during its execution.
			CKBEHAVIOR_TARGETABLE					=0x00040000,	// Behavior may be owned by a different object that the one to which its execution will apply.
			CKBEHAVIOR_CUSTOMEDITDIALOG				=0x00080000,	// This Behavior have a custom Dialog Box for parameters edition .
			CKBEHAVIOR_RESERVED0					=0x00100000,	// Reserved for internal use.
			CKBEHAVIOR_EXECUTEDLASTFRAME			=0x00200000,	// This behavior has been executed during last process. (Available only in profile mode )
			CKBEHAVIOR_DEACTIVATENEXTFRAME			=0x00400000,	// Behavior will be deactivated next frame
			CKBEHAVIOR_RESETNEXTFRAME				=0x00800000,	// Behavior will be reseted next frame
			CKBEHAVIOR_INTERNALLYCREATEDINPUTS		=0x01000000,	// Behavior execution may create/delete inputs
			CKBEHAVIOR_INTERNALLYCREATEDOUTPUTS		=0x02000000,	// Behavior execution may create/delete outputs
			CKBEHAVIOR_INTERNALLYCREATEDINPUTPARAMS	=0x04000000,	// Behavior execution may create/delete input parameters or change their type
			CKBEHAVIOR_INTERNALLYCREATEDOUTPUTPARAMS=0x08000000,	// Behavior execution may create/delete output parameters or change their type
			CKBEHAVIOR_INTERNALLYCREATEDLOCALPARAMS	=0x40000000,	// Behavior execution may create/delete local parameters or change their type
			CKBEHAVIOR_ACTIVATENEXTFRAME			=0x10000000,	// Behavior will be activated next frame
			CKBEHAVIOR_LOCKED						=0x20000000,	// Behavior is locked for utilisation in Virtools
			CKBEHAVIOR_LAUNCHEDONCE					=0x80000000,	// Behavior has not yet been launched...
}	CK_BEHAVIOR_FLAGS;            


/*************************************************
{filename:CK_BEHAVIOR_CALLBACKMASK}
Summary: Mask for the messages the callback function of a behavior should be aware of.

Remarks: 
+The callback function of a behavior may ignore some messages using this mask. 
+Don't forget to set the mask for the behavior callback function if you use one since it
can improve performance not to receive useless messages.
See also: CKBehaviorPrototype::SetBehaviorCallbackFct,Behavior Prototype Creation
*************************************************/
typedef enum CK_BEHAVIOR_CALLBACKMASK {
		CKCB_BEHAVIORPRESAVE		=0x00000001,	// Callback will be called for CKM_BEHAVIORPRESAVE messages 
		CKCB_BEHAVIORDELETE			=0x00000002,	// Callback will be called for CKM_BEHAVIORDELETE messages
		CKCB_BEHAVIORATTACH			=0x00000004,	// Callback will be called for CKM_BEHAVIORATTACH messages
		CKCB_BEHAVIORDETACH			=0x00000008,	// Callback will be called for CKM_BEHAVIORDETACH messages
		CKCB_BEHAVIORPAUSE			=0x00000010,	// Callback will be called for CKM_BEHAVIORPAUSE messages
		CKCB_BEHAVIORRESUME			=0x00000020,	// Callback will be called for CKM_BEHAVIORRESUME messages
		CKCB_BEHAVIORCREATE			=0x00000040,	// Callback will be called for CKM_BEHAVIORCREATE messages
		CKCB_BEHAVIORRESET			=0x00001000,	// Callback will be called for CKM_BEHAVIORRESET messages
		CKCB_BEHAVIORPOSTSAVE		=0x00000100,	// Callback will be called for CKM_BEHAVIORPOSTSAVE messages
		CKCB_BEHAVIORLOAD			=0x00000200,	// Callback will be called for CKM_BEHAVIORLOAD messages
		CKCB_BEHAVIOREDITED			=0x00000400,	// Callback will be called for CKM_BEHAVIOREDITED messages
		CKCB_BEHAVIORSETTINGSEDITED	=0x00000800,	// Callback will be called for CKM_BEHAVIORSETTINGSEDITED messages
		CKCB_BEHAVIORREADSTATE		=0x00001000,	// Callback will be called for CKM_BEHAVIORREADSTATE messages
		CKCB_BEHAVIORNEWSCENE		=0x00002000,	// Callback will be called for CKM_BEHAVIORNEWSCENE messages
		CKCB_BEHAVIORACTIVATESCRIPT	=0x00004000,	// Callback will be called for CKM_BEHAVIORACTIVATESCRIPT messages
		CKCB_BEHAVIORDEACTIVATESCRIPT=0x00008000,	// Callback will be called for CKM_BEHAVIORDEACTIVATESCRIPT messages
		CKCB_BEHAVIORRESETINBREAKPOINT=0x00010000,	// Callback will be called for CKM_BEHAVIORRESETINBREAKPOINT messages
		CKCB_BEHAVIORRENAME			=0x00020000,	// Callback will be called for CKM_BEHAVIORRENAME messages

		CKCB_BEHAVIORBASE			=0x0000000E,	// Base flags :attach /detach /delete
		CKCB_BEHAVIORSAVELOAD		=0x00000301,	// Base flags for load and save
		CKCB_BEHAVIORPPR			=0x00000130,	// Base flags for play/pause/reset
		CKCB_BEHAVIOREDITIONS		=0x00000C00,	// Base flags for editions of settings or parameters
		CKCB_BEHAVIORALL			=0xFFFFFFFF,	// All flags 
} CK_BEHAVIOR_CALLBACKMASK;



/*************************************************
{filename:CK_BEHAVIOR_RETURN}
Summary: Return value for the behavior execution function.
Remarks: 
	+ Return value of a behavior execution function is used to activate it
	the next frame and handle errors.

	+ A return value with _RETRY (CKBR_ACTIVATENEXTFRAME bit set) forces the behavior to be reactivated next frame.

See also: CKBehaviorPrototype::SetFunction
*************************************************/
typedef enum CK_BEHAVIOR_RETURN
{
	CKBR_OK						= 0,		// Everything's ok. Behavior is deactivated unless one of its inputs is active.
	CKBR_ACTIVATENEXTFRAME		= 1,		// The behavior will be reactivated  next frame
	CKBR_ATTACHFAILED			= 2,		// The attach failed
	CKBR_DETACHFAILED			= 4,		// The attach failed
	CKBR_LOCKED					= 6,		// obsolete..
	CKBR_INFINITELOOP			= 8,		// The behavior has reached the infinite loop limit...
	CKBR_BREAK					= 10,		// Break the processing here => Keep on processing windows messages but keep on calling the behavior until 
											// it returns a different value (Used by script debugger )
	CKBR_GENERICERROR			= 0xA000,	// Something went wrong
	CKBR_BEHAVIORERROR			= 0xA002,	// The behavior gave to the code was wrong
	CKBR_OWNERERROR				= 0xA004,	// The owner isn't what it should be
	CKBR_PARAMETERERROR			= 0xA008,	// Some of the parameters are wrong
	CKBR_GENERICERROR_RETRY		= 0xA001,
	CKBR_BEHAVIORERROR_RETRY	= 0xA003,
	CKBR_OWNERERROR_RETRY		= 0xA005,
	CKBR_PARAMETERERROR_RETRY	= 0xA009,
	
	
} CK_BEHAVIOR_RETURN;


/*************************************************
{filename:CK_BEHAVIOR_TYPE}
Summary: Behavior Type.
Remarks: 
	+ Behaviors may be scripts, simpliest behaviors (a function) or elaborated graphs of sub-behaviors.

See also: CKBehavior::GetType
*************************************************/
typedef enum CK_BEHAVIOR_TYPE {
			CKBEHAVIORTYPE_BASE						=0x00000000,	// This Behavior is a most simple type of behavior ( a function )
			CKBEHAVIORTYPE_SCRIPT					=0x00000001,	// This Behavior is a script
			CKBEHAVIORTYPE_BEHAVIOR					=0x00000004,	// This Behavior is a simple one ( a graph )
}	CK_BEHAVIOR_TYPE;

/*************************************************
{filename:CK_2DENTITY_FLAGS}
Summary: 2DEntity Flags

Remarks:
	Flags specify how an entity should be drawn to screen.
See also: CK2dEntity, CK2dEntity::SetFlags
*************************************************/
typedef enum CK_2DENTITY_FLAGS {
			CK_2DENTITY_RESERVED3=			   0x00000001,	
			CK_2DENTITY_POSITIONCHANGED=	   0x00000002,	// Position changed since last frame (used internally)
			CK_2DENTITY_SIZECHANGED=		   0x00000004,	// Size changed since last frame (used internally) 
			CK_2DENTITY_USESIZE=			   0x00000008,	// Force entity to use given size
			CK_2DENTITY_VIRTOOLS=			   0x00000010,	// Entity created and used by the interface
			CK_2DENTITY_USESRCRECT=			   0x00000020,	// Force entity to use given rect when blitting to screen (CK2dEntity::UseSourceRect)
			CK_2DENTITY_BACKGROUND=			   0x00000040,	// Force entity to be rendered before 3d objects (CK2dEntity::SetBackground)
			CK_2DENTITY_NOTPICKABLE=		   0x00000080,	// This entity can not be picked through CKRenderContext::Pick (CK2dEntity::SetPickable)
			CK_2DENTITY_RATIOOFFSET=		   0x00000100,	// The position is relative to top-right screen or either to top-right camera surface (CK2dEntity::EnableRatioOffset)
			CK_2DENTITY_USEHOMOGENEOUSCOORD=   0x00000200,  // Use homogeneous coordinates (0..1) insted of pixel coordinates (CK2dEntity::SetHomogeneousCoordinates)
			CK_2DENTITY_CLIPTOCAMERAVIEW=	   0x00000400,	// Clip to camera view rather than to the screen (CK2dEntity::EnableClipToCamera)
			CK_2DENTITY_UPDATEHOMOGENEOUSCOORD=0x00000800,  // Need to update homogeneous coordinates (used internally)
			CK_2DENTITY_CLIPTOPARENT=		   0x00001000,	// Clip to Parent (CK2dEntity::SetClipToParent)
			CK_2DENTITY_RESERVED0			=  0x00010000,  
			CK_2DENTITY_RESERVED1			=  0x00020000,  
			CK_2DENTITY_RESERVED2			=  0x00040000,  
			CK_2DENTITY_STICKLEFT			=  0x00080000,  // stick left to the parent
			CK_2DENTITY_STICKRIGHT			=  0x00100000,  // stick right to the parent
			CK_2DENTITY_STICKTOP			=  0x00200000,  // stick top to the parent
			CK_2DENTITY_STICKBOTTOM			=  0x00400000   // stick bottom to the parent
} CK_2DENTITY_FLAGS;


/****************************************************************
{filename:CK_OBJECTANIMATION_FLAGS}
Summary: Special settings for object animations.

Remarks:
	+ These flags specify special settings for object animations such
	as ignoring position,scale or rotation information.

See also:	CKObjectAnimation::GetFlags
****************************************************************/
typedef enum CK_OBJECTANIMATION_FLAGS {
		CK_OBJECTANIMATION_IGNOREPOS	  = 0x00000004,	// Ignore Position information
		CK_OBJECTANIMATION_IGNOREROT	  = 0x00000008,	// Ignore Rotation information
		CK_OBJECTANIMATION_IGNORESCALE	  = 0x00000010,	// Ignore Scaling information
		CK_OBJECTANIMATION_IGNOREMORPH	  = 0x00000020,	// Ignore Morph information
		CK_OBJECTANIMATION_IGNORESCALEROT = 0x00000040,	// Ignore Shear information
		CK_OBJECTANIMATION_MERGED		  = 0x00000080, // This animation is a merged animation (CKObjectAnimation::CreateMergedAnimation)
		CK_OBJECTANIMATION_WARPER		  = 0x40000000,	// This animation is used for transitions (internal)
		CK_OBJECTANIMATION_RESERVED		  = 0x80000000,	// reserved for internal use
	} CK_OBJECTANIMATION_FLAGS;




/*************************************************
{filename:CK_ANIMATION_FLAGS}
Summary:  Flags settings for animations

Remarks: 
	These flags specify play options for the animations.
See also: CKAnimation, CKAnimation::SetFlags
*************************************************/
typedef enum CK_ANIMATION_FLAGS {
			CKANIMATION_LINKTOFRAMERATE	=			0x00000001,	// Interpolation is done accordind to real time (CKAnimation::LinkToFrameRate)
			CKANIMATION_CANBEBREAK		=			0x00000004,	// If not set, once started an animation will be played until its end (CKAnimation::SetCanBeInterrupt)
			CKANIMATION_ALLOWTURN		=			0x00000008,	// Character can be rotated while this animation is playing	(used internally)
			CKANIMATION_ALIGNORIENTATION=			0x00000010,	// Character will take animation orienation	(CKAnimation::SetCharacterOrientation)
			CKANIMATION_SECONDARYWARPER	=			0x00000020,	// This animation is used to made a transition for secondary animations (used internally)
			CKANIMATION_SUBANIMSSORTED	=			0x00000040,	// For keyed animations this flag is set when all the sub-objects animations have been sorted accorded to their importance (contribution) in the whole animation (used internally)
			CKANIMATION_HANDLELOD		=			0x00000080,	// When setting this flags, calling SetStep on a character the engine only animates the character root if it was not rendered last frame. 

// Transition Options... 

			CKANIMATION_TRANSITION_FROMNOW			= 0000000100,		// Play new animation right now without any transition
			CKANIMATION_TRANSITION_FROMWARPFROMCURRENT	= 0x00000200,	// Warp to next anim from current position

			CKANIMATION_TRANSITION_TOSTART			= 0x00000800,		//  Start next animation from the start

			CKANIMATION_TRANSITION_WARPTOSTART		= 0x00001000,	//  warp to the start of next animation
			CKANIMATION_TRANSITION_WARPTOBEST		= 0x00002000,	//  warp to the best suited position in the next animation
			CKANIMATION_TRANSITION_WARPTOSAMEPOS	= 0x00010000,	//  warp to the same current position in the source animation to the destination animation
			CKANIMATION_TRANSITION_WARPTOPOS		= 0x00040000,	//  warp to the given position of next animation
													
			CKANIMATION_TRANSITION_USEVELOCITY		= 0x00004000,	//  use current animation and next animation velocities to extrapolate a velocity for the root bodypart
			CKANIMATION_TRANSITION_LOOPIFEQUAL		= 0x00008000,	//  if current and next animation are the same act as a loop
														
			CKANIMATION_TRANSITION_WARPSTART		= 0x00001200,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOSTART)
			CKANIMATION_TRANSITION_WARPBEST			= 0x00002200,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOBEST)
			CKANIMATION_TRANSITION_WARPSAMEPOS		= 0x0001FB00,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOSAMEPOS)
			CKANIMATION_TRANSITION_WARPPOS			= 0x0004FB00,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOOS)

			CKANIMATION_TRANSITION_ALL				= 0x0001FF00,

// Secondary Animation Options ...
			CKANIMATION_SECONDARY_ONESHOT			= 0x00040000,	// Animation will be played once
			CKANIMATION_SECONDARY_LOOP				= 0x00080000,	// Animation will be played continously			
			CKANIMATION_SECONDARY_LASTFRAME			= 0x00200000,	// Animation will stay on last frame		
			CKANIMATION_SECONDARY_LOOPNTIMES		= 0x00400000,	// Animation will be played continously	for N loop (specified in PlaySecondaryAnimation)		
			CKANIMATION_SECONDARY_DOWARP			= 0x00800000,	// Creates A transition when starting or stopping a animation
			
			CKANIMATION_SECONDARY_ALL				= 0x00EC0000,	// Creates A transition when starting or stopping a animation

			CKANIMATION_TRANSITION_PRESET			= 0x01000000,	// Set by the Exporter or user to indicate to ignore flags specified in CKCharacter::SetNextActiveAnimation ( They should be set trough CKAnimation::SetTransitionMode )
			CKANIMATION_SECONDARY_PRESET			= 0x02000000,	// Set by the Exporter or user to indicate to ignore flags specified in CKCharacter::PlaySecondaryAnimation ( They should be set trough CKAnimation::SetSecondaryAnimationMode )
			
} CK_ANIMATION_FLAGS;


/*************************************************
{filename:CK_ANIMATION_TRANSITION_MODE}
Summary: Transition mode between animations

Remarks:
	Constants identifying way transition should occur when changing current animation in a character.
See also : CKCharacter::SetNextActiveAnimation,CKKeyedAnimation::CreateTransition
*************************************************/
typedef enum CK_ANIMATION_TRANSITION_MODE {
		CK_TRANSITION_FROMNOW				=0x00000001,	// play new animation right now without any transition
		CK_TRANSITION_FROMWARPFROMCURRENT	=0x00000002,	// Warp to next anim from current position

		CK_TRANSITION_TOSTART				=0x00000008,	//  start next animation from the start

		CK_TRANSITION_WARPTOSTART			=0x00000010,	//  warp to the start of next animation
		CK_TRANSITION_WARPTOBEST			=0x00000020,	//  warp to the best suited position in the next animation
		CK_TRANSITION_WARPTOSAMEPOS			=0x00000100,	//  warp to the same current position in the source animation to the destination animation
		CK_TRANSITION_WARPTOPOS				=0x00000400,	//  warp to a given position in the destination animation

		CK_TRANSITION_USEVELOCITY			=0x00000040,	//  use current animation and next animation velocities to extrapolate a velocity for the root bodypart
		CK_TRANSITION_LOOPIFEQUAL			=0x00000080,	//  if current and next animation are the same act as a loop

		CK_TRANSITION_FROMANIMATION			=0x00000200,	//  Use transition mode stored in the animation (See CKAnimation::SetTransitionMode)
		
		CK_TRANSITION_WARPSTART				=0x00000012,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOSTART)
		CK_TRANSITION_WARPBEST				=0x00000022,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOBEST)
		CK_TRANSITION_WARPSAMEPOS			=0x00000102,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOSAMEPOS)
		CK_TRANSITION_WARPPOS				=0x00000402,	//	(CK_TRANSITION_FROMWARPFROMCURRENT|CK_TRANSITION_WARPTOPOS)
		CK_TRANSITION_WARPMASK				=0x00000132		// Mask for all warping modes 
		
} CK_ANIMATION_TRANSITION_MODE;


#define CK_TRANSITION_BREAK		(CK_TRANSITION_FROMNOW)

#define CK_TRANSITION_LOOP		(CK_TRANSITION_TOSTART)


#define CK_TRANSITION_MODE_SHIFT 8

#define CK_SECONDARY_FLAGS_SHIFT 16

/*************************************************
{filename:CK_SECONDARYANIMATION_FLAGS}
Summary:  Flags settings for secondary animations

Remarks: 
+These flags specify play options for secondary animations on a character.
+They be given when playing a secondary animation in CKCharacter::PlaySecondaryAnimation
or set once on the animation using CKAnimation::SetSecondaryAnimationMode.
See also: CKAnimation, CKCharacter::PlaySecondaryAnimation,CKAnimation::SetSecondaryAnimationMode
*************************************************/
typedef enum CK_SECONDARYANIMATION_FLAGS {
			CKSECONDARYANIMATION_ONESHOT=	   0x00000004,	// Animation will be played once
			CKSECONDARYANIMATION_LOOP=		   0x00000008,	// Animation will be played continously			
			CKSECONDARYANIMATION_LASTFRAME=	   0x00000020,	// Animation will stay on last frame		
			CKSECONDARYANIMATION_LOOPNTIMES=   0x00000040,	// Animation will be played continously	for N loop (specified in CKCharacter::PlaySecondaryAnimation)		
			CKSECONDARYANIMATION_DOWARP=	   0x00000080,	// Creates a transition when starting or stopping a animation

			CKSECONDARYANIMATION_FROMANIMATION=0x00000100,	// Uses settings specified in CKAnimation::SetSecondaryAnimationMode
} CK_SECONDARYANIMATION_FLAGS;


/*************************************************
{filename:CK_RENDER_FLAGS}
Summary: Render options

Remarks:
	+ This flags specifies different render options when drawing the scenes. 

See also: CKRenderContext::ModifyCurrentRenderOptions,CKRenderContext::Render,CKRenderContext::DrawScene,CKRenderContext::Clear,CKRenderContext::BackToFront
*************************************************/
typedef enum CK_RENDER_FLAGS {
		CK_RENDER_BACKGROUNDSPRITES		=	0x00000001,		// Background sprites are to be rendered
		CK_RENDER_FOREGROUNDSPRITES		=	0x00000002,		// Foreground sprites are to be rendered
		CK_RENDER_USECAMERARATIO		=	0x00000008,		// Render Context size is automatically set using current camera aspect ratio and window size.
		CK_RENDER_CLEARZ				=	0x00000010,		// Clear ZBuffer
		CK_RENDER_CLEARBACK				=	0x00000020,		// Clear Back Buffer
		CK_RENDER_CLEARSTENCIL			=	0x00000040,		// Clear Stencil Buffer
		CK_RENDER_DOBACKTOFRONT			=	0x00000080,		// Swap buffers
		
		CK_RENDER_DEFAULTSETTINGS		=   0x000000FF,		// Default render context settings 

		CK_RENDER_CLEARVIEWPORT			=   0x00000100,		// Do not clear entire device, only viewport 		
		CK_RENDER_WAITVBL				=	0x00000200,		// Wait for Vertical Blank before blitting
		CK_RENDER_SKIPDRAWSCENE			=	0x00000400,		// Do not draw scene ( Both 3D and 2D objects)
		CK_RENDER_DONOTUPDATEEXTENTS	=	0x00000800,		// Do not update object extents
		CK_RENDER_SKIP3D				=	0x00001000,		// Do not draw 3D objects (nor device callbacks)
		CK_RENDER_SKIPTRANSPARENT		=	0x00002000,		// Do not draw transparent objects
		CK_RENDER_OPTIONSMASK			=   0x0000FFFF,		// All rendering flags

		CK_RENDER_PLAYERCONTEXT			=	0x00010000,		// Use by behaviors to know render contextes on which they should act.
		CK_RENDER_RESTORENEXTFRAME		=	0x00020000,		// Restores the clear flags and skipt draw scene next frame, and removes itself.

		CK_RENDER_HINTRENDERATTRIBTEX  =	0x00040000,		// An attribute texture render is current (z-texture, position of fragments for deferred shading etc): BBs such as "Shadow Stencil" should not render themselves

		CK_RENDER_USECURRENTSETTINGS	=	0x00000000		// When calling CKRenderContext::Render,CKRenderContext::DrawScene,CKRenderContext::Clear and CKRenderContext::BackToFront use this flag to use default options specified in SetCurrentRenderOptions
} CK_RENDER_FLAGS;



/*************************************************
{filename:CK_PARAMETERTYPE_FLAGS}
Summary:  Flags settings for new parameter types

Remarks:
+These flags specify special settings for a parameter type.
+Parameter may have a fixed or variable buffer size, some may be hidden
so that they are not displayed in the interface.
See also: CKParameterTypeDesc
*************************************************/
typedef enum CK_PARAMETERTYPE_FLAGS {
			CKPARAMETERTYPE_VARIABLESIZE =	   0x00000001,	// Size of the buffer stored by the parameter may change
			CKPARAMETERTYPE_RESERVED     =	   0x00000002,	// Reserved
			CKPARAMETERTYPE_HIDDEN		 =	   0x00000004,	// This parameter type should not be shown in the interface
			CKPARAMETERTYPE_FLAGS		 =	   0x00000008,	// This parameter type is a flag (See CKParameterManager::RegisterNewFlags)
			CKPARAMETERTYPE_STRUCT		 =	   0x00000010,	// This parameter type is a structure of parameters (See CKParameterManager::RegisterNewStructure)
			CKPARAMETERTYPE_ENUMS		 =	   0x00000020,	// This parameter type is an enumeration (See CKParameterManager::RegisterNewEnum)
			CKPARAMETERTYPE_USER		 =	   0x00000040,	// This parameter type is a user-defined one created in the interface
			CKPARAMETERTYPE_NOENDIANCONV =	   0x00000080,	// Do not try to convert this parameter buffer On Big-Endian processors (strings, void buffer have this flags)
			CKPARAMETERTYPE_TOSAVE		 =	   0x00000100,	// Temporary flag set in CKFile::EndSave(). This parameter type is to be saved in the SaveData callback of managers. Used in Parameter Manager (used user's flags & enums)
} CK_PARAMETERTYPE_FLAGS;



/***********************************************************
{filename:CK_SCENEOBJECTACTIVITY_FLAGS}
Summary: Flags overriding activity flags of objects and scripts in a scene.

Remarks: 
	+ When launching a scene objects and scripts are activated according to 
	their scene flags (CK_SCENEOBJECT_FLAGS). You can override this flags when launching a scene
	by specifying to force activation for example using CK_SCENEOBJECTACTIVITY_ACTIVATE.
See also: CKLevel::LaunchScene
***************************************************************/
typedef enum CK_SCENEOBJECTACTIVITY_FLAGS {
	CK_SCENEOBJECTACTIVITY_SCENEDEFAULT		= 0x00000000,	// The object or script will be activated according to its CK_SCENEOBJECT_FLAGS in the scene		
	CK_SCENEOBJECTACTIVITY_ACTIVATE			= 0x00000001,	// The objects and scripts will be activated
	CK_SCENEOBJECTACTIVITY_DEACTIVATE		= 0x00000002,	// The objects and scripts will be deactivated
	CK_SCENEOBJECTACTIVITY_DONOTHING		= 0x00000003,	// The objects and scripts stay in their current state (whether active or not)
} CK_SCENEOBJECTACTIVITY_FLAGS;


/***********************************************************
{filename:CK_SCENEOBJECTRESET_FLAGS}
Summary: Flags overriding reset flag of objects and scripts in a scene.

Remarks: 
	+ When launching a scene objects and scripts are reset according to 
	their scene flags (CK_SCENEOBJECT_FLAGS). You can override this flags when launching a scene
	by specifying to force objects not to return to their initial conditions for example using CK_SCENEOBJECTRESET_DONOTHING.
See also: CKLevel::LaunchScene,
***************************************************************/
typedef enum CK_SCENEOBJECTRESET_FLAGS {
	CK_SCENEOBJECTRESET_SCENEDEFAULT	= 0x00000000,	// The object or script will be reset according to its CK_SCENEOBJECT_FLAGS in the scene		
	CK_SCENEOBJECTRESET_RESET			= 0x00000001,	// Force objects to reset their initial conditions (if any) and scripts to be reset.
	CK_SCENEOBJECTRESET_DONOTHING		= 0x00000002,	// The objects and scripts stay in their current state
} CK_SCENEOBJECTRESET_FLAGS;

/***********************************************************
{filename:CK_SCENEOBJECT_FLAGS}
Summary: Flags describing state of an object in a scene.

Remarks: 
	+ Objects present in a CKScene use these flags to describe
	their state in the scene. Objects may be hidden or have an 
	initial state forced to a specific value.

See also: CKScene::SetObjectFlags,CKScene::ModifyObjectFlags
***************************************************************/
typedef enum CK_SCENEOBJECT_FLAGS {
			CK_SCENEOBJECT_START_ACTIVATE		= 0x0001,	// The object or script is activated when the scene starts (Default)
			CK_SCENEOBJECT_ACTIVE				= 0x0008,	// The object or script is currently active (behaviors will be executed)
			CK_SCENEOBJECT_START_DEACTIVATE		= 0x0010,	// The object or script is deactivated when the scene starts
			CK_SCENEOBJECT_START_LEAVE			= 0x0020,	// The object or script stay in their current state when the scene starts
			CK_SCENEOBJECT_START_RESET			= 0x0040,	// Objects : Reset their initial conditions if any, Scripts : Re-initialize script.( Default)
			CK_SCENEOBJECT_INTERNAL_IC			= 0x0080,	
} CK_SCENEOBJECT_FLAGS;


/*************************************************
{filename:CK_ATTRIBUT_FLAGS}
Summary: Attribute Flags

Remarks: 
	+ When created a new attribute type with CKAttributeManager::RegisterNewAttributeType , it can 
	be given special properties that will control its behavior in the Dev interface and other
	special features such as the ability for an attribute to be copied or saved along with the object
	it belongs to.
See also: CKAttributeManager::RegisterNewAttributeType,CKAttributeManager::GetAttributeFlags
*************************************************/
typedef enum CK_ATTRIBUT_FLAGS {
	CK_ATTRIBUT_CAN_MODIFY		 = 0x00000001,	// This Attribute type may be renamed
	CK_ATTRIBUT_CAN_DELETE		 = 0x00000002,	// This Attribute type may be deleted 		
	CK_ATTRIBUT_HIDDEN			 = 0x00000004,	// This Attribute type will not appear in the Virtools Interface		
	CK_ATTRIBUT_DONOTSAVE		 = 0x00000008,	// This Attribute type will not be saved		
	CK_ATTRIBUT_USER			 = 0x00000010,	// This Attribute type was created by a user (in the Dev interface for example)
	CK_ATTRIBUT_SYSTEM			 = 0x00000020,	// This Attribute type was created by the system (Behavior Dll,Manager,etc...)
	CK_ATTRIBUT_DONOTCOPY		 = 0x00000040,	// This Attribute type will not be copied when the object that holds it is copied		
	CK_ATTRIBUT_TOSAVE			 = 0x00000080,	// Temporary flag set in CKFile::EndSave(). This parameter type is to be saved in the SaveData callback of managers. Used in Attribute Manager (used attributes)
} CK_ATTRIBUT_FLAGS;


//----------------------------------------------------------////
// Behavior Prototype  Flags								////
//----------------------------------------------------------////

 
typedef enum CK_BEHAVIORPROTOTYPE_FLAGS {
				CK_BEHAVIORPROTOTYPE_NORMAL		=0x00000001,			
				CK_BEHAVIORPROTOTYPE_HIDDEN		=0x00000002,
				CK_BEHAVIORPROTOTYPE_OBSOLETE	=0x00000004,	
} CK_BEHAVIORPROTOTYPE_FLAGS;


/*************************************************
{filename:CK_LOADMODE}
Summary: Specify the way an object just loaded should be handled when it already exists in the level.
{secret}

See also: 
*************************************************/
typedef enum CK_LOADMODE 
{
	CKLOAD_INVALID=-1,// Use the existing object instead of loading 
	CKLOAD_OK=0,		// Ignore ( Name unicity is broken )
	CKLOAD_REPLACE=1,	// Replace the existing object (Not yet implemented)
	CKLOAD_RENAME=2,	// Rename the loaded object
	CKLOAD_USECURRENT=3,// Use the existing object instead of loading 
} CK_LOADMODE,CK_CREATIONMODE;

/*************************************************
{filename:CK_OBJECTCREATION_OPTIONS}
Summary: Specify the way an object is created through CKCreateObject.

Remarks: 
	+ These flag controls the way an object is created, the most important of these flags
	being CK_OBJECTCREATION_DYNAMIC which, if set in CKCreateObject, make the newly created object 
	dynamic.
See also: CKContext::CreateObject,Dynamic Objects
*************************************************/
enum CK_OBJECTCREATION_OPTIONS
{
	CK_OBJECTCREATION_NONAMECHECK	= 0,	// Do not test for name unicity (may be overriden in special case)
	CK_OBJECTCREATION_REPLACE		= 1,	// Replace the current object by the object being loaded
	CK_OBJECTCREATION_RENAME		= 2,	// Rename the created object to ensure its uniqueness
	CK_OBJECTCREATION_USECURRENT	= 3,	// Do not create a new object, use the one with the same name instead
	CK_OBJECTCREATION_ASK			= 4,	//	If a duplicate name if found, opens a dialog box to ask the useror use automatic load mode if any.
	CK_OBJECTCREATION_FLAGSMASK		= 0x0000000F,	// Mask for previous values
	CK_OBJECTCREATION_DYNAMIC		= 0x00000010,	//	The object must be created dynamic
	CK_OBJECTCREATION_ACTIVATE		= 0x00000020,	//	The object will be copied/created active
	CK_OBJECTCREATION_NONAMECOPY	= 0x00000040	//	The object will take control of the string given to it directly, without copying it
};


#define CK_OBJECTCREATION_SameDynamic (IsDynamic() ? CK_OBJECTCREATION_DYNAMIC : CK_OBJECTCREATION_NONAMECHECK)

#define CK_OBJECTCREATION_Dynamic(x) (x ? CK_OBJECTCREATION_DYNAMIC : CK_OBJECTCREATION_NONAMECHECK)

/*************************************************
{filename:CK_DEPENDENCIES_OPMODE}
Summary: Specify the mode of operation of a CKDependenciesContext.

Remarks: 
	A CKDependenciesContext class is always used when it come to work
	with object dependencies (A mesh if a dependencies of a CK3dEntity for example)
See also: CKCreateObject
*************************************************/
typedef enum CK_DEPENDENCIES_OPMODE {
	CK_DEPENDENCIES_COPY			=	1,	// Copying objects
	CK_DEPENDENCIES_DELETE			=	2,	// Deleting objects
	CK_DEPENDENCIES_REPLACE			=	3,	
	CK_DEPENDENCIES_SAVE			=	4,	// Saving objects
	CK_DEPENDENCIES_BUILD			=	5,	// Building a list of dependencies
	CK_DEPENDENCIES_OPERATIONMODE	=	0xF
} CK_DEPENDENCIES_OPMODE;

/*************************************************
{filename:CK_FILE_WRITEMODE}
Summary: Specify the way files are saved to disk (compression)

Remarks :
	+ File write mode controls the format of a Virtools file when saved. More specifically it 
	controls whether compression is enabled and also if the Virtools Interface specific data
	should be stored in the file (if CKFILE_FORVIEWER flag is set , no interface data is saved)

See also: CKContext::SetFileWriteMode,CKContext::GetFileWriteMode,CKContext::SetCompressionLevel,CKContext::SetGlobalImagesSaveOptions,CKContext::SetGlobalSoundsSaveOptions
*************************************************/
typedef enum CK_FILE_WRITEMODE 
{
	CKFILE_UNCOMPRESSED	       =0,	// Save data uncompressed
	CKFILE_CHUNKCOMPRESSED_OLD =1,	// Obsolete
	CKFILE_EXTERNALTEXTURES_OLD=2,	// Obsolete : use CKContext::SetGlobalImagesSaveOptions instead.
	CKFILE_FORVIEWER           =4,	// Don't save Interface Data within the file, the level won't be editable anymore in the interface
	CKFILE_WHOLECOMPRESSED     =8,	// Compress the whole file
} CK_FILE_WRITEMODE;

/*************************************************
{filename:CK_BITMAP_SYSTEMCACHING}
Summary: Specify the way textures or sprites are stored in system memory

Remarks : 
See also: CKBitmapData::SetSystemCaching,CKContext::SetGlobalImagesSaveOptions
*************************************************/
enum CK_BITMAP_SYSTEMCACHING 
{
	CKBITMAP_PROCEDURAL				= 0,	// The texture is stored in a raw 32 bit per pixel format for easy read and write operations (needed for SetPixel and ReadPixel for ex.). 
	CKBITMAP_VIDEOSHADOW			= 1,	// The texture is stored in the same format as the desired video
	CKBITMAP_DISCARD				= 2		// The texture is not stored in system memory
};

/*************************************************
{filename:CK_TEXTURE_SAVEOPTIONS}
Summary: Specify the way textures or sprites will be saved

Remarks : 
	+ Textures can be stored inside Virtools files or kept as references to external files.
	+ These options can be used for a specific texture (or sprite) or as a global setting.
See also: CKBitmapData::SetSaveOptions,CKSprite::SetSaveOptions,CKContext::SetGlobalImagesSaveOptions
*************************************************/
typedef enum CK_TEXTURE_SAVEOPTIONS 
{
	CKTEXTURE_RAWDATA				=0,	// Save raw data inside file. The bitmap is saved in a raw 32 bit per pixel format. 
	CKTEXTURE_EXTERNAL				=1,	// Store only the file name for the texture. The bitmap file must be present in the bitmap paths 
										// when loading the composition.
	CKTEXTURE_IMAGEFORMAT			=2,	// Save using format specified. The bitmap data will be converted to the 
										// specified format by the correspondant bitmap plugin and saved inside file. 
	CKTEXTURE_USEGLOBAL				=3,	// Use Global settings, that is the settings given with CKContext::SetGlobalImagesSaveOptions. (Not valid when using CKContext::SetImagesSaveOptions). 
	CKTEXTURE_INCLUDEORIGINALFILE	=4,	// Insert original image file inside CMO file. The bitmap file that 
										// was used originally for the texture or sprite will be append to 
										// the composition file and extracted when the file is loaded.
} CK_TEXTURE_SAVEOPTIONS;

/*************************************************
{filename:CK_BITMAP_SAVEOPTIONS}
Summary: Specify the way textures or sprites will be saved


See also: CK_TEXTURE_SAVEOPTIONS
*************************************************/
typedef CK_TEXTURE_SAVEOPTIONS  CK_BITMAP_SAVEOPTIONS;

/*************************************************
{filename:CK_SOUND_SAVEOPTIONS}
Summary: Specify the way sounds will be saved

Remarks : 
+ Sounds can kept as references to external files or the original sound file can
be appended to the composition file.
+ These options can be used for a specific sound or as a global setting.
See also: CKSound::SetSaveOptions,CKContext::SetGlobalSoundSaveOptions
*************************************************/
typedef enum CK_SOUND_SAVEOPTIONS 
{
	CKSOUND_EXTERNAL				=0,	// Store only the file name for the sound. The sound file must be present
										// in one of the sound paths when the composition is loaded.
	CKSOUND_INCLUDEORIGINALFILE		=1,	// Insert original sound file inside the CMO file. The sound file that 
										// was used originally will be append to  the composition file and
										// extracted when the file is loaded.
	CKSOUND_USEGLOBAL				=2,	// Use Global settings. This flag is only valid for the CKSound::SetSaveOptions method.
} CK_SOUND_SAVEOPTIONS;



/*************************************************
{filename:CK_CONFIG_FLAGS}
Summary: Flags specifying start options for Virtools

Remarks:
	Flags specifying start options for Virtools



See also: CKCreateContext
*************************************************/
typedef enum CK_CONFIG_FLAGS {
		CK_CONFIG_DISABLEDSOUND=			1,		// Prevent CK from initializing Sound Engines
		CK_CONFIG_DISABLEDINPUT=			2,		// Prevent CK from initializing Input Engines
		CK_CONFIG_DOWARN=					4		// Set this flag for message box warning (sound init failed, etc.)
} CK_CONFIG_FLAGS;


/*************************************************
{filename:CK_DESTROY_FLAGS}
Summary: Objects destruction Flags

Remarks: Flags specifying how objects should be deleted.


See also: CKContext::DestroyObjects,CKContext::DestroyObject
*************************************************/
typedef enum CK_DESTROY_FLAGS {
	CK_DESTROY_FREEID		= 0x00000001,		// Release the CK_ID of the object so that it can be re-used bu new objects.
	CK_DESTROY_NONOTIFY		= 0x00000002,		// Managers and other objects won't be notified of this deletion 
	CK_DESTROY_TEMPOBJECT	= 0x00000003,		// Combination for temporary objects : Do not notify and release CK_ID
} CK_DESTROY_FLAGS;


/*************************************************
{filename:CK_WAVESOUND_TYPE}
Summary: Enums specifying the type of a sound

See also: CKSoundManager::CreateSource
*************************************************/
typedef enum CK_WAVESOUND_TYPE {
		CK_WAVESOUND_BACKGROUND =			1,		// The sound is in 2D
		CK_WAVESOUND_POINT	    =			2,		// The sound is in 3D
		CK_WAVESOUND_CONE       =			3,		// The sound is a 3D Cone : NOT YET IMPLEMENTED
} CK_WAVESOUND_TYPE;

/*************************************************
{filename:CK_WAVESOUND_LOCKMODE}
Summary: Enums specifying lock mode

Remarks: 
See also: CKSoundManager::Lock
*************************************************/
typedef enum CK_WAVESOUND_LOCKMODE {
		CK_WAVESOUND_LOCKFROMWRITE		=			1,		// Uses current cursor position and ignores dwWriteCursor
		CK_WAVESOUND_LOCKENTIREBUFFER	=			2		// Ignores dwWriteCursor and dwNumBytes - locks the whole buffer
} CK_WAVESOUND_LOCKMODE;

/*************************************************
{filename:CK_WAVESOUND_STATE}
Summary: Flags specifying current state of a sound

Remarks:	
See also: CKSoundManager,CKWaveSound
*************************************************/
typedef enum CK_WAVESOUND_STATE {
		CK_WAVESOUND_ALLTYPE			= 0x00000007, // The vave type part of the state
		CK_WAVESOUND_LOOPED				= 0x00000008, // The vave must loop
		CK_WAVESOUND_FADEIN				= 0x00000020, // The wave is in fadein
		CK_WAVESOUND_FADEOUT			= 0x00000040, // The wave is in fadeout
		CK_WAVESOUND_FADE				= 0x00000060, // The wave is in fade
		CK_WAVESOUND_COULDBERESTARTED	= 0x00000080, // The wave data could be restarted while playing
		CK_WAVESOUND_NODUPLICATE		= 0x00000100, // The wave data could not be duplicated
		CK_WAVESOUND_OCCLUSIONS			= 0x00000200, // The sound can be occluded
		CK_WAVESOUND_1ST_REFLECTIONS	= 0x00000400, // The sound can be reflected
		CK_WAVESOUND_MONO				= 0x00000800, // The sound is in mono
		CK_WAVESOUND_HASMOVED			= 0x00001000, // The sound must be updated
		CK_WAVESOUND_FILESTREAMED		= 0x00002000, // The Sound is streamed from an external file
		CK_WAVESOUND_STREAMOVERLAP		= 0x00004000, // The Streaming buffer is overlapping the current play position
		CK_WAVESOUND_STREAMFULLYLOADED	= 0x00008000, // The streamed sound is small enough to be loaded entierly in the buffer (do not modify this flags, used internally)
		CK_WAVESOUND_NEEDREWIND			= 0x00010000, // The sound has reached the end and needs to be rewinded before next playback
		CK_WAVESOUND_PAUSED				= 0x00020000, // The sound has reached the end and needs to be rewinded before next playback

} CK_WAVESOUND_STATE;


//----------------------------------------------------------////
// CollisionManager Flags and Enums							////
//----------------------------------------------------------////

/************************************************
{filename:CK_GEOMETRICPRECISION}
Summary: Geometric precision for collision tests

Remarks:
	This enum is also used when asking for a collision detection. In this case it can override the precision attributed to the 
obstacles (by asking for box testing, for example) or it can use this default values by using CKCOLLISION_NONE (the preferred way).



See Also: CKCollisionManager,CKCollisionManager::DetectCollision
************************************************/
typedef enum CK_GEOMETRICPRECISION {
	CKCOLLISION_NONE	=0x00000000,		// This value is used only when we want to use the default precision when testing objects
	CKCOLLISION_BOX		=0x00000001,		// Collision testing will occur at the Object Local Bounding Box level
	CKCOLLISION_FACE	=0x00000002,		// Collision testing will occur at the Object Faces level.
} CK_GEOMETRICPRECISION;

/************************************************
{filename:CK_RAYINTERSECTION}
Summary: Options for the RayIntersection

See Also: CKCollisionManager,CKCollisionManager::DetectCollision
************************************************/
enum CK_RAYINTERSECTION {
	CKRAYINTERSECTION_DEFAULT		= 0x00000000,		// Default Behavior
	CKRAYINTERSECTION_SEGMENT		= 0x00000001,		// The ray will be considered as a segment
	CKRAYINTERSECTION_IGNOREALPHA	= 0x00000002,		// The ray will ignore the pick threshold set in the textures
	CKRAYINTERSECTION_FIRSTCONTACT	= 0x00000004,		// The ray will stop at the first face touched, not the nearest
	CKRAYINTERSECTION_IGNORECULLING	= 0x00000008,		// The ray will ignore whether a face is clockwise or counterclockwise, otherwise we use the material "both sided" to determine if culling should done.
	CKRAYINTERSECTION_IGNOREBOTHSIDED	= 0x00000010		// The ray consider the face culling even if material's both sided flag is set to TRUE.
};

/************************************************
{filename:CK_IMPACTINFO}
Summary: Flags defining which informations should be returned from a collision.
Remarks:
	These flags are used to specify which members of the ImpactDesc structure should be 
	filled with collision information.

See Also: CKCollisionManager,CKCollisionManager::DetectCollision
************************************************/
typedef enum CK_IMPACTINFO {
	OBSTACLETOUCHED		=0x00000001,		// Fills the m_ObstacleTouched member of the structure
	SUBOBSTACLETOUCHED	=0x00000002,		// Fills the m_EntityTouched member of the structure
	TOUCHEDFACE			=0x00000004,		// Fills the m_TouchedFace member of the structure
	TOUCHINGFACE		=0x00000008,		// Fills the m_TouchingFace member of the structure
	TOUCHEDVERTEX		=0x00000010,		// Fills the m_TouchedVertex member of the structure
	TOUCHINGVERTEX		=0x00000020,		// Fills the m_TouchingVertex member of the structure
	IMPACTWORLDMATRIX	=0x00000040,		// Fills the m_ImpactWorldMatrix member of the structure
	IMPACTPOINT			=0x00000080,		// Fills the m_ImpactPoint member of the structure
	IMPACTNORMAL		=0x00000100,		// Fills the m_ImpactNormal member of the structure
	OWNERENTITY			=0x00000200,		// Fills the m_OwnerEntity member of the structure
}CK_IMPACTINFO;


//----------------------------------------------------------////
// FloorManager Flags and Enums								////
//----------------------------------------------------------////

/************************************************
{filename:CK_FLOORGEOMETRY}
Summary: Geometric precision for floor-intersection tests.

Remarks:
	Enum defining the geometric precision that should be use by default for an entity during the floor test
See Also: CKFloorManager
************************************************/
typedef enum CK_FLOORGEOMETRY {
	CKFLOOR_FACES		=0x00000000,		// Floors testing will occur at the Object Faces level.
	CKFLOOR_BOX			=0x00000001,		// Floors testing will occur at the Object Local Bounding Box level
} CK_FLOORGEOMETRY;

/************************************************
{filename:CK_FLOORNEAREST}
Summary: Enum defining the floors found when retrieving the floors around a position

See Also: CKFloorManager::GetNearestFloors
************************************************/
typedef enum CK_FLOORNEAREST{
	CKFLOOR_NOFLOOR	=0x00000000,		// No Floors were found 
	CKFLOOR_DOWN	=0x00000001,		// There is a floor below the asked position
	CKFLOOR_UP		=0x00000002,		// There is a floor above the asked position
} CK_FLOORNEAREST;


//----------------------------------------------------------////
// GridManager Flags and Enums              								////
//----------------------------------------------------------////

/************************************************
{filename:CK_GRIDORIENTATION}
Summary: Enum defining the orientation of a Grid

See Also: CKGrid::GetOrientationMode, CKGrid::SetOrientationMode
************************************************/
typedef enum CK_GRIDORIENTATION{
	CKGRID_FREE = 0x00000000,	// Free Orientation
	CKGRID_XZ	= 0x00000001,   // Aligned along the X and Z world axis
	CKGRID_XY	= 0x00000002,   // Aligned along the X and Y world axis
	CKGRID_YZ = 0x00000003,		// Aligned along the Y and Z world axis
} CK_GRIDORIENTATION;


/************************************************
{filename:CKGRID_LAYER_FORMAT}
Summary: Enum defining the format of a Grid Layer

See Also: CKGrid,CKLayer::GetFormat, CKLayer::SetFormat
************************************************/
typedef enum CKGRID_LAYER_FORMAT{
	CKGRID_LAYER_FORMAT_NORMAL = 0x00000000,   // Free Orientation
} CKGRID_LAYER_FORMAT;


//----------------------------------------------------------////
// Data Array Flags and Enums              								////
//----------------------------------------------------------////

/************************************************
{filename:CK_BINARYOPERATOR}
Summary: Available operations between colums of a DataArray

See Also: CKDataArray::ColumnTransform, CKDataArray::ColumnsOperate
************************************************/
typedef enum CK_BINARYOPERATOR{
	CKADD	= 1,	// Addition
	CKSUB	= 2,	// Substraction
	CKMUL	= 3,	// Multiplication	
	CKDIV	= 4		// Division
} CK_BINARYOPERATOR;

/************************************************
{filename:CK_COMPOPERATOR}
Summary: Available comparisons between colums of a DataArray
Remarks:

See Also: CKDataArray::CreateGroup, CKDataArray::FindLine
************************************************/
typedef enum CK_COMPOPERATOR{
	CKEQUAL			= 1,	
	CKNOTEQUAL		= 2,
	CKLESSER		= 3,
	CKLESSEREQUAL	= 4,
	CKGREATER		= 5,
	CKGREATEREQUAL	= 6
} CK_COMPOPERATOR;



typedef enum CK_SETOPERATOR{
	CKUNION	= 1,			// Addition
	CKINTERSECTION = 2,		// Intersection
	CKSUBTRACTION	= 3,	// Subtraction	
} CK_SETOPERATOR;

/************************************************
{filename:CK_ARRAYTYPE}
Summary: Enum defining the format of a Data Array element.
Remarks:
	+ In a data array each column can contain data
	of the given type.

See Also: CKDataArray::InsertColumn, CKDataArray::GetColumnType
************************************************/
typedef enum CK_ARRAYTYPE{
	CKARRAYTYPE_INT = 1,		// an integer
	CKARRAYTYPE_FLOAT = 2,		// a float
	CKARRAYTYPE_STRING = 3,		// a pointer to a string
	CKARRAYTYPE_OBJECT = 4,		// a CKObject ID (CK_ID)
	CKARRAYTYPE_PARAMETER = 5	// a CKParameter ID
} CK_ARRAYTYPE;


/*************************************************
{filename:CK_MOUSEBUTTON}
Summary:  Mouse Buttons Constants
Remarks:
	+ Constants identifying mouse buttons.

See also: CKInputManager::IsMouseButtonDown()
*************************************************/
typedef enum CK_MOUSEBUTTON {
			CK_MOUSEBUTTON_LEFT		= 0,			// Left Mouse Button
			CK_MOUSEBUTTON_RIGHT	= 1,			// Right Mouse Button
			CK_MOUSEBUTTON_MIDDLE   = 2,			// Middle Mouse Button
			CK_MOUSEBUTTON_4		= 3				
} CK_MOUSEBUTTON;



/*************************************************
{filename:CK_LOAD_FLAGS}
Summary: Load Options.

Remarks:
+ This options apply when loading a Virtools file
or a importing a 3D Model file. 
+ They defines whether object geometry,only animations 
or only behaviors should be loaded.
+ One can specify (using the CK_LOAD_AS_DYNAMIC_OBJECT) if 
created CKObjects should be created as dynamic (See also Dynamic Objects)
See also : CKContext::Load,CKContext::CKSave
*************************************************/
typedef enum CK_LOAD_FLAGS {
			CK_LOAD_ANIMATION					=1<<0,	// Load animations
			CK_LOAD_GEOMETRY					=1<<1,	// Load geometry.
			CK_LOAD_DEFAULT						=CK_LOAD_GEOMETRY|CK_LOAD_ANIMATION,	// Load animations & geometry
			CK_LOAD_ASCHARACTER					=1<<2, // Load all the objects and create a character that contains them all .
			CK_LOAD_DODIALOG					=1<<3, // Check object name unicity and warns the user with a dialog box when duplicate names are found. 
			CK_LOAD_AS_DYNAMIC_OBJECT			=1<<4, // Objects loaded from this file may be deleted at run-time or are temporary
			CK_LOAD_AUTOMATICMODE				=1<<5, // Check object name unicity and automatically rename or replace according to the options specified in CKContext::SetAutomaticLoadMode
			CK_LOAD_CHECKDUPLICATES				=1<<6, // Check object name unicity (The list of duplicates is stored in the CKFile class after a OpenFile call
			CK_LOAD_CHECKDEPENDENCIES			=1<<7, // Check if every plugins needed are availables
			CK_LOAD_ONLYBEHAVIORS				=1<<8, // 
			CK_LOAD_REPLACEALL_WITHSCRIPT		=1<<9, // Replace all duplicates names, scripts included (old scripts are deleted)
			CK_LOAD_KEEP_CKFILE_OBJECT_AFTER_LOAD	=1<<10,	//ask the reader not to delete the CKFile & store it with CKContext::SetLoadedFile for interface ui purpose
} CK_LOAD_FLAGS;


/*************************************************
{filename:CK_PLUGIN_TYPE}
Summary: Type identifier for a Virtools plugin.

Remarks: 
+Each plugin must be given a type.
+This enumeration is used to identify a specific catagory 
of plugin when using the CKPluginManager.
See also: CKPluginManager,Creating New Plugins
*************************************************/
typedef enum CK_PLUGIN_TYPE {
	CKPLUGIN_BITMAP_READER		= 0,	// The plugin is bitmap (textures,sprites) loader
	CKPLUGIN_SOUND_READER		= 1,	// Sound Reader Plugin
	CKPLUGIN_MODEL_READER		= 2,	// 3D Model Reader 
	CKPLUGIN_MANAGER_DLL		= 3,	// The plugin implements a Manager
	CKPLUGIN_BEHAVIOR_DLL		= 4,	// The plugin implements one or more behaviors
	CKPLUGIN_RENDERENGINE_DLL	= 5,	// Render Engine plugin
	CKPLUGIN_MOVIE_READER		= 6,	// Movie (AVI,Mpeg) reader		
	CKPLUGIN_EXTENSION_DLL		= 7,	// Generic extension (definition of new parameter types or operations for ex.)	
} CK_PLUGIN_TYPE;


//----------------------------------------------------------////
//		Preregistred Managers								////
//----------------------------------------------------------////
//---- Virtools Managers GUID second data is 0


#define OBJECT_MANAGER_GUID1	0x7cbb3b91 

#define ATTRIBUTE_MANAGER_GUID1 0x3d242466 

#define MESSAGE_MANAGER_GUID1	0x466a0fac

#define FLOOR_MANAGER_GUID1		0x420936f9

#define COLLISION_MANAGER_GUID1	0x38244712

#define GRID_MANAGER_GUID1		0x7f004791

#define TIME_MANAGER_GUID1		0x89ce7b32

#define BEHAVIOR_MANAGER_GUID1	0x58d621ae

#define INPUT_MANAGER_GUID1		0xf787c904

#define SOUND_MANAGER_GUID1		0xdce135f6

#define MIDI_MANAGER_GUID1		0x594154a6

#define INTERFACE_MANAGER_GUID1	0x9a4b8e3d

#define RENDER_MANAGER_GUID1	0xa213c8d5

#define PARAMETER_MANAGER_GUID1	0x9ce57ab6

#define PATH_MANAGER_GUID1		0x15fd54b9

#define VARIABLE_MANAGER_GUID1	0x98cc3cc9



#define OBJECT_MANAGER_GUID		CKGUID(OBJECT_MANAGER_GUID1		,0) 

#define ATTRIBUTE_MANAGER_GUID  CKGUID(ATTRIBUTE_MANAGER_GUID1	,0) 

#define MESSAGE_MANAGER_GUID	CKGUID(MESSAGE_MANAGER_GUID1	,0)

#define TIME_MANAGER_GUID		CKGUID(TIME_MANAGER_GUID1		,0)

#define SOUND_MANAGER_GUID		CKGUID(SOUND_MANAGER_GUID1		,0)

#define MIDI_MANAGER_GUID		CKGUID(MIDI_MANAGER_GUID1		,0)

#define INPUT_MANAGER_GUID		CKGUID(INPUT_MANAGER_GUID1		,0)

#define BEHAVIOR_MANAGER_GUID	CKGUID(BEHAVIOR_MANAGER_GUID1	,0)

#define FLOOR_MANAGER_GUID		CKGUID(FLOOR_MANAGER_GUID1		,0)

#define COLLISION_MANAGER_GUID	CKGUID(COLLISION_MANAGER_GUID1	,0)

#define GRID_MANAGER_GUID		CKGUID(GRID_MANAGER_GUID1		,0)

#define INTERFACE_MANAGER_GUID	CKGUID(INTERFACE_MANAGER_GUID1	,0)

#define RENDER_MANAGER_GUID		CKGUID(RENDER_MANAGER_GUID1		,0)

#define PARAMETER_MANAGER_GUID	CKGUID(PARAMETER_MANAGER_GUID1	,0)

#define PATH_MANAGER_GUID		CKGUID(PATH_MANAGER_GUID1		,0)

#define VARIABLE_MANAGER_GUID	CKGUID(VARIABLE_MANAGER_GUID1	,0)



//----------------------------------------------------------////
//		Preregistred parameter types						////
//----------------------------------------------------------////


#define CKPGUID_NONE			CKDEFINEGUID(0x1cb10760,0x419f50c5)

#define CKPGUID_VOIDBUF 		CKDEFINEGUID(0x4d082c90,0x0c8339a2)

#define CKPGUID_FLOAT			CKDEFINEGUID(0x47884c3f,0x432c2c20)		

#define CKPGUID_ANGLE			CKDEFINEGUID(0x11262cf5, 0x30b0233a)		

#define CKPGUID_PERCENTAGE		CKDEFINEGUID(0xf3c84b4e,0x0ffacc34)		

#define CKPGUID_FLOATSLIDER		CKDEFINEGUID(0x429d42cf,0x211c0cc2)		

#define CKPGUID_INT 			CKDEFINEGUID(0x5a5716fd,0x44e276d7)

#define CKPGUID_KEY 			CKDEFINEGUID(0xfa6e1bdd,0x62d2abd7)

#define CKPGUID_BOOL			CKDEFINEGUID(0x1ad52a8e,0x5e741920)

#define CKPGUID_STRING			CKDEFINEGUID(0x6bd010e2,0x115617ea)

#define CKPGUID_RECT			CKDEFINEGUID(0x7ab20d20,0x693044a9)

#define CKPGUID_VECTOR			CKDEFINEGUID(0x48824eae,0x2fe47960)

#define CKPGUID_VECTOR4			CKDEFINEGUID(0x6c439ee0,0x2fe47960)

#define CKPGUID_2DVECTOR		CKDEFINEGUID(0x4efcb34a,0x6079e42f)

#define CKPGUID_QUATERNION		CKDEFINEGUID(0x6c439ee,0x45b50fc2)

#define CKPGUID_EULERANGLES		CKDEFINEGUID(0x13b01b3c,0x1942583e)

#define CKPGUID_MATRIX			CKDEFINEGUID(0x643f046e,0x65211b71)

#define CKPGUID_COLOR			CKDEFINEGUID(0x57d42fee,0x7cbb3b91)

#define CKPGUID_BOX				CKDEFINEGUID(0x668649c8,0x283e2ee1)

#define CKPGUID_OBJECTARRAY		CKDEFINEGUID(0x71df7142,0xc437133a)

#define CKPGUID_OBJECT			CKDEFINEGUID(0x30ec20ab,0x6df6517d)

#define CKPGUID_BEOBJECT		CKDEFINEGUID(0x71d80779,0x402f42f3)

#define CKPGUID_MESH			CKDEFINEGUID(0x24535345,0x65d15229)

#define CKPGUID_MATERIAL		CKDEFINEGUID(0x55ab12cd,0x22ae6a8b)

#define CKPGUID_TEXTURE			CKDEFINEGUID(0x155b2870,0x183679f8)

#define CKPGUID_SPRITE			CKDEFINEGUID(0x577e2ea8,0x5b901969)

#define CKPGUID_3DENTITY		CKDEFINEGUID(0x5b8a05d5,0x31ea28d4)

#define CKPGUID_CURVEPOINT		CKDEFINEGUID(0x5cc5194f,0x72342879)

#define CKPGUID_LIGHT			CKDEFINEGUID(0x4b6d412f,0x5d1e1416)

#define CKPGUID_TARGETLIGHT		CKDEFINEGUID(0x4e5f22fc,0x194709d4)

#define CKPGUID_ID				CKDEFINEGUID(0x71653557,0x2d1b2e97)

#define CKPGUID_CAMERA			CKDEFINEGUID(0x3cf24d6f,0x216204f9)

#define CKPGUID_TARGETCAMERA	CKDEFINEGUID(0x11d87565,0x5d0b7989)

#define CKPGUID_SPRITE3D		CKDEFINEGUID(0x23c8061b,0x70033a27)

#define CKPGUID_OBJECT3D		CKDEFINEGUID(0x362e4df8,0x17443539)

#define CKPGUID_BODYPART		CKDEFINEGUID(0x4ba1010e,0x7abd6d02)

#define CKPGUID_CHARACTER		CKDEFINEGUID(0x35985c64,0x51af1372)

#define CKPGUID_CURVE			CKDEFINEGUID(0x1072130b,0x1f9572ba)

#define CKPGUID_2DCURVE			CKDEFINEGUID(0x20ad345d,0x1afb25b1)

#define CKPGUID_LEVEL			CKDEFINEGUID(0x10584787,0x76932f77)

#define CKPGUID_PLACE			CKDEFINEGUID(0x249530ed,0x4c0865f3)

#define CKPGUID_GROUP			CKDEFINEGUID(0x5c0f151b,0x6f0547fd)

#define CKPGUID_2DENTITY		CKDEFINEGUID(0x181671e3,0x1bdc1503)

#define CKPGUID_RENDEROBJECT	CKDEFINEGUID(0x65706358,0x63627b50)

#define CKPGUID_SPRITETEXT		CKDEFINEGUID(0x5c2e69e3,0xfe156f09)

#define CKPGUID_SOUND			CKDEFINEGUID(0x40194410,0x3a773f80)

#define CKPGUID_WAVESOUND		CKDEFINEGUID(0x4bf74e5e,0x45f409ef)

#define CKPGUID_MIDISOUND		CKDEFINEGUID(0x4b81218b,0x2eb7145e)

#define CKPGUID_VIDEO			CKDEFINEGUID(0x1a2146f4,0x28d2155d)

#define CKPGUID_OBJECTANIMATION	CKDEFINEGUID(0x43476550,0x11e52c0e)

#define CKPGUID_ANIMATION		CKDEFINEGUID(0x5bfa474d,0x19e60236)

#define CKPGUID_KINEMATICCHAIN	CKDEFINEGUID(0x305b4e1c,0x3da07ae6)

#define CKPGUID_SCENE			CKDEFINEGUID(0x25ae6bd7,0x7c8f73a2)

#define CKPGUID_BEHAVIOR		CKDEFINEGUID(0x286b19db,0x21185d4f)

#define CKPGUID_MESSAGE			CKDEFINEGUID(0x3881e12,0x5ba34e2b)

#define CKPGUID_SYNCHRO			CKDEFINEGUID(0x2314a763,0xc2f49238)

#define CKPGUID_CRITICALSECTION	CKDEFINEGUID(0x9438212a,0xc2f4312c)

#define CKPGUID_STATE			CKDEFINEGUID(0xabe13353,0xa2834124)

#define CKPGUID_ATTRIBUTE		CKDEFINEGUID(0x3ea34ee9,0x9fa5366)

#define CKPGUID_CLASSID			CKDEFINEGUID(0x19644e43,0x4d6f6123)

#define CKPGUID_DIRECTION		CKDEFINEGUID(0x286652d,0x5ea709c2)

#define CKPGUID_BLENDMODE		CKDEFINEGUID(0x3c2a6aca,0x6f0e5865)

#define CKPGUID_FILTERMODE		CKDEFINEGUID(0x2bd753b3,0x563e5165)

#define CKPGUID_BLENDFACTOR		CKDEFINEGUID(0x89e28d4,0x2ef031d2)
	
#define CKPGUID_FILLMODE		CKDEFINEGUID(0x2b0f1617,0x53ce1b0e)

#define CKPGUID_LITMODE			CKDEFINEGUID(0x6dd30f0a,0x74671d2e)

#define CKPGUID_SHADEMODE		CKDEFINEGUID(0x7990258f,0x60326351)

#define CKPGUID_GLOBALEXMODE		CKDEFINEGUID(0x259699f,0x6eda4960)

#define CKPGUID_ZFUNC			CKDEFINEGUID(0x51cd1cf5,0x1d5c2d9b)

#define CKPGUID_ADDRESSMODE		CKDEFINEGUID(0x80d38d5,0x2cf7a30)

#define CKPGUID_WRAPMODE		CKDEFINEGUID(0x14e14bd8,0x32a738e7)

#define CKPGUID_3DSPRITEMODE	CKDEFINEGUID(0x1d1a3403,0x535e7252)

#define CKPGUID_FOGMODE			CKDEFINEGUID(0x686f28ef,0x5ecc7766)

#define CKPGUID_LIGHTTYPE		CKDEFINEGUID(0x4f0f4891, 0x0340a05d1)

#define CKPGUID_SPRITEALIGN		CKDEFINEGUID(0xb3272aa,0x7f1a3572)

#define CKPGUID_SCRIPT			CKDEFINEGUID(0x7ea4176d, 0x1b405d30)

#define CKPGUID_LAYERTYPE		CKDEFINEGUID(0x4a39553c, 0x1ac94677)

#define CKPGUID_STATECHUNK		CKDEFINEGUID(0x9fb5e33c, 0x134cb77e)

#define CKPGUID_DATAARRAY		CKDEFINEGUID(0x24d52f1,0x678223b2)

#define CKPGUID_COMPOPERATOR	CKDEFINEGUID(0x79b90856,0x75d070ff)

#define CKPGUID_BINARYOPERATOR	CKDEFINEGUID(0x2ce46c85,0x4b791332)

#define CKPGUID_SETOPERATOR		CKDEFINEGUID(0x2011b99,0x1efb0b91)

#define CKPGUID_SPRITETEXTALIGNMENT	CKDEFINEGUID(0x6e49509a,0x2f067425)

#define CKPGUID_OBSTACLEPRECISION		CKDEFINEGUID(0x5f1b2b3c,0x32ae26b6)

#define CKPGUID_OBSTACLEPRECISIONBEH	CKDEFINEGUID(0x5f3b233c,0x33ae36b6)

#define CKPGUID_OBSTACLE				CKDEFINEGUID(0x1238843,0xff881c6e)

#define CKPGUID_PATCHMESH				CKDEFINEGUID(0x75fd45b5,0x59e70e4b)

#define CKPGUID_POINTER					CKDEFINEGUID(0x50766059,0x159d4bde)

#define CKPGUID_ENUMS				CKDEFINEGUID(0x4dd37f6b,0x240f5fa2)

#define CKPGUID_STRUCTS				CKDEFINEGUID(0x38df566a,0x30f77b9e)

#define CKPGUID_FLAGS				CKDEFINEGUID(0x2b49245d,0x582d60d6)

#define	CKPGUID_FILTER				CKDEFINEGUID(0x4a5d4963,0x4d28883f)

#define CKPGUID_TIME				CKDEFINEGUID(0x54b4422b,0x730f0f4f)

#define CKPGUID_OLDTIME				CKDEFINEGUID(0x4a4d4867,0x3c28773f)

#define	CKPGUID_COPYDEPENDENCIES	CKDEFINEGUID(0x748d4e0d,0x3bf7195b)

#define	CKPGUID_DELETEDEPENDENCIES	CKDEFINEGUID(0x69590c80,0x30b61630)

#define	CKPGUID_SAVEDEPENDENCIES	CKDEFINEGUID(0xc187c6f,0x54f87691)

#define	CKPGUID_REPLACEDEPENDENCIES	CKDEFINEGUID(0x7f2e61bd,0x7d6890)

#define	CKPGUID_SCENEACTIVITYFLAGS	CKDEFINEGUID(0x1cd24241,0x533c0f8f)

#define	CKPGUID_SCENEOBJECT			CKDEFINEGUID(0x5bdf2d37,0x64ea731f)

#define	CKPGUID_SCENERESETFLAGS		CKDEFINEGUID(0x18170afb,0x687d5da7)

#define CKPGUID_ARRAYTYPE			CKDEFINEGUID(0x615d017c,0x54a9524f)

#define CKPGUID_RENDEROPTIONS		CKDEFINEGUID(0x745655a9,0x61bb29a1)

#define CKPGUID_PARAMETERTYPE		CKDEFINEGUID(0x34517df5,0x45e4965)

#define CKPGUID_MATERIALEFFECT		CKDEFINEGUID(0x62c563e3,0x7323470d)

#define CKPGUID_TEXGENEFFECT		CKDEFINEGUID(0x7dbc28f5,0x3f161e80)

#define CKPGUID_TEXGENREFEFFECT		CKDEFINEGUID(0x724b7421,0x7213add)

#define CKPGUID_COMBINE2TEX			CKDEFINEGUID(0x154264ea,0x1eb15971)

#define CKPGUID_COMBINE3TEX			CKDEFINEGUID(0x235e15cc,0x65903824)

#define CKPGUID_BUMPMAPPARAM		CKDEFINEGUID(0x36da22d9,0x4ac44b4c)

#define CKPGUID_TEXCOMBINE			CKDEFINEGUID(0x21e87a54,0x65d37993)

#define CKPGUID_PIXELFORMAT			CKDEFINEGUID(0x79465229,0x61af2af1)

#define CKPGUID_AXIS				CKDEFINEGUID(0x79465230,0x62a88af1)

#define CKPGUID_SUPPORT					CKDEFINEGUID(0x71119230,0x62abeacc)

#define CKPGUID_BITMAP_SYSTEMCACHING	CKDEFINEGUID(0x75894231,0x12afe226)

#define CKPGUID_OLDMESSAGE			CKDEFINEGUID(0x213661cc,0x7d1a2d54)

#define CKPGUID_OLDATTRIBUTE		CKDEFINEGUID(0x13b97e4c,0x982e8b4f)

#define CKPGUID_3DPOINTCLOUD		CKDEFINEGUID(0x4d111cea,0x1ab12e02)

#define CKPGUID_HOST_PLATFORM		CKDEFINEGUID(0x74e426fa,0xa3e48963)

#define CKPGUID_UNICODESTRING		CKDEFINEGUID(0x2893670,0x482d5151)

#define CKPGUID_FOCUSLOST_BEHAVIOR	CKDEFINEGUID(0x37944a65,0x477e3666)

#define CKPGUID_KERNELERROR			CKDEFINEGUID(0x3f35054,0xcf137c5)

#define CKPGUID_SHADOWMAPFORMAT	    CKDEFINEGUID(0x6125717e, 0x652c20a6)

#define CKPGUID_SHADOWMAPTAPCOUNT   CKDEFINEGUID(0xb7116af, 0x32624773)

#define CKPGUID_SHADOWMAPATTENUATIONMODE CKDEFINEGUID(0x54ac4786, 0x41845f5d)

#define CKPGUID_SHADOWMAPFALLBACKPOLICY CKDEFINEGUID(0x6d4f72b2, 0x23c04463)

#define CKPGUID_CUSTOMIZEDPARAM		CKDEFINEGUID( 0x91757c44 , 0xf18c37c7 )



typedef enum CK_VIRTOOLS_VERSION {
	CK_VIRTOOLS_DEV			= 0,
	CK_VIRTOOLS_CREATION	= 1,
	CK_VIRTOOLS_DEV_EVAL	= 2,
	CK_VIRTOOLS_CREA_EVAL	= 3,
	CK_VIRTOOLS_DEV_NFR		= 4,
	CK_VIRTOOLS_CREA_NFR	= 5,
	CK_VIRTOOLS_DEV_EDU		= 6,
	CK_VIRTOOLS_CREA_EDU	= 7,
	CK_VIRTOOLS_DEV_TB		= 8,
	CK_VIRTOOLS_CREA_TB		= 9,
	CK_VIRTOOLS_DEV_NCV		= 10,
	CK_VIRTOOLS_DEV_SE		= 11,
	CK_VIRTOOLS_MAXVERSION	= 12,
} CK_VIRTOOLS_VERSION;

//------------------------------------------
// Joystick codes 


#define CKJOY_1	0	

#define CKJOY_2	1

#define CKJOY_3	2

#define CKJOY_4	3

#define CKMAX_JOY 4

#ifdef DOCJETDUMMY // Docjet secret macro
#else

#ifdef _XBOX

// {filename:CKKEYBOARD}
// Summary : Enumeration of keyboard constants
//
typedef enum CKKEYBOARD {
  CKKEY_CANCEL         = 0x03
, CKKEY_BACK           = 0x08
, CKKEY_TAB            = 0x09
, CKKEY_CLEAR          = 0x0C
, CKKEY_RETURN         = 0x0D
, CKKEY_SHIFT          = 0x10
, CKKEY_CONTROL        = 0x11
, CKKEY_MENU           = 0x12
, CKKEY_PAUSE          = 0x13
, CKKEY_CAPITAL        = 0x14

, CKKEY_KANA           = 0x15
, CKKEY_HANGEUL        = 0x15
, CKKEY_HANGUL         = 0x15
, CKKEY_JUNJA          = 0x17
, CKKEY_FINAL          = 0x18
, CKKEY_HANJA          = 0x19
, CKKEY_KANJI          = 0x19

, CKKEY_ESCAPE         = 0x1B

, CKKEY_CONVERT        = 0x1C
, CKKEY_NONCONVERT     = 0x1D
, CKKEY_ACCEPT         = 0x1E
, CKKEY_MODECHANGE     = 0x1F

, CKKEY_SPACE          = 0x20
, CKKEY_PRIOR          = 0x21
, CKKEY_NEXT           = 0x22
, CKKEY_END            = 0x23
, CKKEY_HOME           = 0x24
, CKKEY_LEFT           = 0x25
, CKKEY_UP             = 0x26
, CKKEY_RIGHT          = 0x27
, CKKEY_DOWN           = 0x28
, CKKEY_SELECT         = 0x29
, CKKEY_PRINT          = 0x2A
, CKKEY_EXECUTE        = 0x2B
, CKKEY_SNAPSHOT       = 0x2C
, CKKEY_INSERT         = 0x2D
, CKKEY_DELETE         = 0x2E
, CKKEY_HELP           = 0x2F

, CKKEY_0				= 0x30
, CKKEY_1				= 0x31
, CKKEY_2				= 0x32
, CKKEY_3				= 0x33
, CKKEY_4				= 0x34
, CKKEY_5				= 0x35
, CKKEY_6				= 0x36
, CKKEY_7				= 0x37
, CKKEY_8				= 0x38
, CKKEY_9				= 0x39

, CKKEY_A				= 0x41
, CKKEY_B
, CKKEY_C
, CKKEY_D
, CKKEY_E
, CKKEY_F
, CKKEY_G
, CKKEY_H
, CKKEY_I
, CKKEY_J
, CKKEY_K
, CKKEY_L
, CKKEY_M
, CKKEY_N
, CKKEY_O
, CKKEY_P
, CKKEY_Q
, CKKEY_R
, CKKEY_S
, CKKEY_T
, CKKEY_U
, CKKEY_V
, CKKEY_W
, CKKEY_X
, CKKEY_Y
, CKKEY_Z

, CKKEY_LWIN           = 0x5B
, CKKEY_RWIN           = 0x5C
, CKKEY_APPS           = 0x5D
, CKKEY_SLEEP          = 0x5F
, CKKEY_NUMPAD0        = 0x60
, CKKEY_NUMPAD1        = 0x61
, CKKEY_NUMPAD2        = 0x62
, CKKEY_NUMPAD3        = 0x63
, CKKEY_NUMPAD4        = 0x64
, CKKEY_NUMPAD5        = 0x65
, CKKEY_NUMPAD6        = 0x66
, CKKEY_NUMPAD7        = 0x67
, CKKEY_NUMPAD8        = 0x68
, CKKEY_NUMPAD9        = 0x69
, CKKEY_MULTIPLY       = 0x6A
, CKKEY_ADD            = 0x6B
, CKKEY_SEPARATOR      = 0x6C
, CKKEY_SUBTRACT       = 0x6D
, CKKEY_DECIMAL        = 0x6E
, CKKEY_DIVIDE         = 0x6F
, CKKEY_F1             = 0x70
, CKKEY_F2             = 0x71
, CKKEY_F3             = 0x72
, CKKEY_F4             = 0x73
, CKKEY_F5             = 0x74
, CKKEY_F6             = 0x75
, CKKEY_F7             = 0x76
, CKKEY_F8             = 0x77
, CKKEY_F9             = 0x78
, CKKEY_F10            = 0x79
, CKKEY_F11            = 0x7A
, CKKEY_F12            = 0x7B
, CKKEY_F13            = 0x7C
, CKKEY_F14            = 0x7D
, CKKEY_F15            = 0x7E
, CKKEY_F16            = 0x7F
, CKKEY_F17            = 0x80
, CKKEY_F18            = 0x81
, CKKEY_F19            = 0x82
, CKKEY_F20            = 0x83
, CKKEY_F21            = 0x84
, CKKEY_F22            = 0x85
, CKKEY_F23            = 0x86
, CKKEY_F24            = 0x87
, CKKEY_NUMLOCK        = 0x90
, CKKEY_SCROLL         = 0x91
, CKKEY_OEM_NEC_EQUAL  = 0x92   // '=' key on numpad
, CKKEY_OEM_FJ_JISHO   = 0x92   // 'Dictionary' key
, CKKEY_OEM_FJ_MASSHOU = 0x93   // 'Unregister word' key
, CKKEY_OEM_FJ_TOUROKU = 0x94   // 'Register word' key
, CKKEY_OEM_FJ_LOYA    = 0x95   // 'Left OYAYUBI' key
, CKKEY_OEM_FJ_ROYA    = 0x96   // 'Right OYAYUBI' key
, CKKEY_LSHIFT         = 0xA0
, CKKEY_RSHIFT         = 0xA1
, CKKEY_LCONTROL       = 0xA2
, CKKEY_RCONTROL       = 0xA3
, CKKEY_LMENU          = 0xA4
, CKKEY_RMENU          = 0xA5
, CKKEY_BROWSER_BACK        = 0xA6
, CKKEY_BROWSER_FORWARD     = 0xA7
, CKKEY_BROWSER_REFRESH     = 0xA8
, CKKEY_BROWSER_STOP        = 0xA9
, CKKEY_BROWSER_SEARCH      = 0xAA
, CKKEY_BROWSER_FAVORITES   = 0xAB
, CKKEY_BROWSER_HOME        = 0xAC
, CKKEY_VOLUME_MUTE         = 0xAD
, CKKEY_VOLUME_DOWN         = 0xAE
, CKKEY_VOLUME_UP           = 0xAF
, CKKEY_MEDIA_NEXT_TRACK    = 0xB0
, CKKEY_MEDIA_PREV_TRACK    = 0xB1
, CKKEY_MEDIA_STOP          = 0xB2
, CKKEY_MEDIA_PLAY_PAUSE    = 0xB3
, CKKEY_LAUNCH_MAIL         = 0xB4
, CKKEY_LAUNCH_MEDIA_SELECT = 0xB5
, CKKEY_LAUNCH_APP1         = 0xB6
, CKKEY_LAUNCH_APP2         = 0xB7
, CKKEY_OEM_1          = 0xBA 
, CKKEY_OEM_PLUS       = 0xBB 
, CKKEY_OEM_COMMA      = 0xBC 
, CKKEY_OEM_MINUS      = 0xBD 
, CKKEY_OEM_PERIOD     = 0xBE 
, CKKEY_OEM_2          = 0xBF 
, CKKEY_OEM_3          = 0xC0 
, CKKEY_OEM_4          = 0xDB  
, CKKEY_OEM_5          = 0xDC  
, CKKEY_OEM_6          = 0xDD  
, CKKEY_OEM_7          = 0xDE  
, CKKEY_OEM_8          = 0xDF
, CKKEY_OEM_AX         = 0xE1  
, CKKEY_OEM_102        = 0xE2  
, CKKEY_ICO_HELP       = 0xE3  
, CKKEY_ICO_00         = 0xE4  
, CKKEY_PROCESSKEY     = 0xE5
, CKKEY_ICO_CLEAR      = 0xE6
, CKKEY_PACKET         = 0xE7

} CKKEYBOARD;

#else
typedef enum CKKEYBOARD {
	  CKKEY_ESCAPE			= 0x01         // Escape
	, CKKEY_1				= 0x02		  // 1 through 0 on main keyboard		
	, CKKEY_2				= 0x03     
	, CKKEY_3				= 0x04     
	, CKKEY_4				= 0x05     
	, CKKEY_5				= 0x06     
	, CKKEY_6				= 0x07     
	, CKKEY_7				= 0x08     
	, CKKEY_8				= 0x09     
	, CKKEY_9				= 0x0A     
	, CKKEY_0				= 0x0B     
	, CKKEY_MINUS			= 0x0C       // - on main keyboard 
	, CKKEY_EQUALS			= 0x0D		 // = on main keyboard 
	, CKKEY_BACK			= 0x0E       // backspace 
	, CKKEY_TAB				= 0x0F				
	, CKKEY_Q				= 0x10         
	, CKKEY_W				= 0x11         
	, CKKEY_E				= 0x12       
	, CKKEY_R				= 0x13       
	, CKKEY_T				= 0x14       
	, CKKEY_Y				= 0x15       
	, CKKEY_U				= 0x16       
	, CKKEY_I				= 0x17       
	, CKKEY_O				= 0x18       
	, CKKEY_P				= 0x19       
	, CKKEY_LBRACKET		= 0x1A       // [  
	, CKKEY_RBRACKET		= 0x1B		 // ]
	, CKKEY_RETURN			= 0x1C       // Enter on main keyboard 
	, CKKEY_LCONTROL		= 0x1D		 // Left Control 	
	, CKKEY_A				= 0x1E       
	, CKKEY_S				= 0x1F       
	, CKKEY_D				= 0x20       
	, CKKEY_F				= 0x21       
	, CKKEY_G				= 0x22       
	, CKKEY_H				= 0x23       
	, CKKEY_J				= 0x24       
	, CKKEY_K				= 0x25       
	, CKKEY_L				= 0x26       
	, CKKEY_SEMICOLON		= 0x27		  // ;	
	, CKKEY_APOSTROPHE		= 0x28		  // '	
	, CKKEY_GRAVE			= 0x29        // accent grave 
	, CKKEY_LSHIFT			= 0x2A		  // Left shift		
	, CKKEY_BACKSLASH		= 0x2B		  //	 
	, CKKEY_Z				= 0x2C       
	, CKKEY_X				= 0x2D       
	, CKKEY_C				= 0x2E       
	, CKKEY_V				= 0x2F       
	, CKKEY_B				= 0x30       
	, CKKEY_N				= 0x31       
	, CKKEY_M				= 0x32       
	, CKKEY_COMMA			= 0x33		 // ,	 
	, CKKEY_PERIOD			= 0x34       // . on main keyboard 
	, CKKEY_SLASH			= 0x35       // / on main keyboard 
	, CKKEY_RSHIFT			= 0x36		 // Right Shift
	, CKKEY_MULTIPLY		= 0x37       // * on numeric keypad 
	, CKKEY_LMENU			= 0x38       // left Alt 
	, CKKEY_SPACE			= 0x39		 // Space Bar 
	, CKKEY_CAPITAL			= 0x3A		 // Caps Lock	
	, CKKEY_F1				= 0x3B		
	, CKKEY_F2				= 0x3C       
	, CKKEY_F3				= 0x3D       
	, CKKEY_F4				= 0x3E       
	, CKKEY_F5				= 0x3F       
	, CKKEY_F6				= 0x40       
	, CKKEY_F7				= 0x41       
	, CKKEY_F8				= 0x42       
	, CKKEY_F9				= 0x43       
	, CKKEY_F10				= 0x44       
	, CKKEY_NUMLOCK			= 0x45			  // Num Lock	
	, CKKEY_SCROLL			= 0x46           // Scroll Lock 
	, CKKEY_NUMPAD7			= 0x47       
	, CKKEY_NUMPAD8			= 0x48       
	, CKKEY_NUMPAD9			= 0x49       
	, CKKEY_SUBTRACT		= 0x4A           // - on numeric keypad 
	, CKKEY_NUMPAD4			= 0x4B       
	, CKKEY_NUMPAD5			= 0x4C       
	, CKKEY_NUMPAD6			= 0x4D       
	, CKKEY_ADD				= 0x4E           // + on numeric keypad 
	, CKKEY_NUMPAD1			= 0x4F       
	, CKKEY_NUMPAD2			= 0x50       
	, CKKEY_NUMPAD3			= 0x51       
	, CKKEY_NUMPAD0			= 0x52       
	, CKKEY_DECIMAL			= 0x53           // . on numeric keypad 
	, CKKEY_F11				= 0x57       
	, CKKEY_F12				= 0x58       
								
	, CKKEY_F13				= 0x64                 //   (NEC PC98) 
	, CKKEY_F14				= 0x65                 //   (NEC PC98) 
	, CKKEY_F15				= 0x66                 //   (NEC PC98) 
										
	, CKKEY_KANA			= 0x70       // (Japanese keyboard)            
	, CKKEY_CONVERT			= 0x79       // (Japanese keyboard)            
	, CKKEY_NOCONVERT		= 0x7B       // (Japanese keyboard)            
	, CKKEY_YEN				= 0x7D       // (Japanese keyboard)            
	, CKKEY_NUMPADEQUALS	= 0x8D       // = on numeric keypad (NEC PC98) 
	, CKKEY_CIRCUMFLEX		= 0x90       // (Japanese keyboard)            
	, CKKEY_AT				= 0x91       //    (NEC PC98) 
	, CKKEY_COLON			= 0x92       //    (NEC PC98) 
	, CKKEY_UNDERLINE		= 0x93       //    (NEC PC98) 
	, CKKEY_KANJI			= 0x94       // (Japanese keyboard)            
	, CKKEY_STOP			= 0x95       //    (NEC PC98) 
	, CKKEY_AX				= 0x96       //    (Japan AX) 
	, CKKEY_UNLABELED		= 0x97       //       (J3100) 
	, CKKEY_NUMPADENTER		= 0x9C       // Enter on numeric keypad 
	, CKKEY_RCONTROL		= 0x9D   
	, CKKEY_NUMPADCOMMA		= 0xB3       // , on numeric keypad (NEC PC98) 
	, CKKEY_DIVIDE			= 0xB5       // / on numeric keypad 
	, CKKEY_SYSRQ			= 0xB7   
	, CKKEY_RMENU			= 0xB8       // right Alt 
	, CKKEY_HOME			= 0xC7       // Home on arrow keypad 
	, CKKEY_UP				= 0xC8       // UpArrow on arrow keypad 
	, CKKEY_PRIOR			= 0xC9       // PgUp on arrow keypad 
	, CKKEY_LEFT			= 0xCB       // LeftArrow on arrow keypad 
	, CKKEY_RIGHT			= 0xCD       // RightArrow on arrow keypad 
	, CKKEY_END				= 0xCF       // End on arrow keypad 
	, CKKEY_DOWN			= 0xD0       // DownArrow on arrow keypad 
	, CKKEY_NEXT			= 0xD1       // PgDn on arrow keypad 
	, CKKEY_INSERT			= 0xD2       // Insert on arrow keypad 
	, CKKEY_DELETE			= 0xD3       // Delete on arrow keypad 
	, CKKEY_LWIN			= 0xDB       // Left Windows key 
	, CKKEY_RWIN			= 0xDC       // Right Windows key 
	, CKKEY_APPS			= 0xDD,        // AppMenu key 

} CKKEYBOARD;
#endif

#endif

typedef enum CK_PARAMETER_FLAGS {
				CKPARAMETER_LOCAL	=0,  // The parameter is local to a CKBehavior
				CKPARAMETER_IN		=1,	 // The parameter is an input of a CKBehavior	
				CKPARAMETER_OUT		=2,	 // The	parameter is an output of a CKBehavior or a parameter of a CKBeObject
				CKPARAMETER_SETTING =3,	 // The	parameter is used as a setting parameter by a CKBehavior.
} CK_PARAMETER_FLAGS;



typedef enum CK_PROFILE_CATEGORY {
	CK_PROFILE_RENDERTIME		= 3,
	CK_PROFILE_IKTIME			= 7,
	CK_PROFILE_ANIMATIONTIME	= 6,
} CK_PROFILE_CATEGORY;

/**********************************************************
{filename:CK_BONES_REFERENTIAL}
Summary : Type of bone matrices send to the skinning shader

***************************************************************/
typedef enum CK_BONES_REFERENTIAL {
	
	CK_BONE_LOCAL			=	0x00000000,	// Matrices transformed from bones referential to skin holder referential
	CK_BONE_WORLD			,				// Matrices transformed from bones referential to world referential
	CK_BONE_WORLDVIEW		,				// Matrices transformed from bones referential to camera referential 

}CK_BONES_REFERENTIAL;


/**********************************************************
{filename:CK_FOCUSLOST_BEHAVIOR}
Summary : web player's behavior when it losts focus 
(used by webplayer)

***************************************************************/
typedef enum CK_FOCUSLOST_BEHAVIOR {
	CK_FOCUSLOST_NONE				=	0,
	CK_FOCUSLOST_PAUSEINPUT_MAIN	=	1,
	CK_FOCUSLOST_PAUSEINPUT_PLAYER	=	2,
	CK_FOCUSLOST_PAUSEALL			=	3,
	CK_FOCUSLOST_PAUSEINPUT_WHENOUT	=	4
} CK_FOCUSLOST_BEHAVIOR;



#endif
