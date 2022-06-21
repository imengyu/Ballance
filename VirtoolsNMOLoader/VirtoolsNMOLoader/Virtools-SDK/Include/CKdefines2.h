/*************************************************************************/
/*	File : CKDefines.h				 				 					 */
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKDEFINES2_H

#define CKDEFINES2_H "$Id:$"

//***********************************************************//
// Obsolete Flags are only kept for compatibilty purposes
// They cannot be used in the current version	
// 
// These flags are only used when saving an Object state not when saving to file
//***********************************************************//

#if defined(PSP) || defined(_XBOX) || defined(PSX2) 
	#define NO_POINT_CLOUDS	
	#define NO_VIDEO
#endif
#if defined(PSP) 
	#define NO_SHADER
#endif

#define CK_STATESAVE_ALL				0xFFFFFFFF			

//------------------------------------------------
// Object								
typedef enum CK_STATESAVEFLAGS_OBJECT {
		CK_STATESAVE_NAME				= 0x00000001,	// Obsolete
		CK_STATESAVE_ID					= 0x00000002,	// Obsolete
		CK_STATESAVE_OBJECTHIDDEN		= 0x00000004,	// The object is hidden
		CK_STATESAVE_OBJECTHIERAHIDDEN	= 0x00000018,	// The object is hidden hierarchically
		CK_STATESAVE_OBJECTALL			= 0x0000000F
} CK_STATESAVEFLAGS_OBJECT;


//------------------------------------------------
// Be Object							
typedef enum CK_STATESAVEFLAGS_BEOBJECT 
{
		 CK_STATESAVE_ATTRIBUTES		= 0x00000010,	// Obsolete
		 CK_STATESAVE_NEWATTRIBUTES		= 0x00000011,	// Save Attributes
		 CK_STATESAVE_GROUPS			= 0x00000020,	// Obsolete
		 CK_STATESAVE_DATAS				= 0x00000040,	// Save Flags and (Waiting for message) status
		 CK_STATESAVE_SOUNDS			= 0x00000080,	// Obsolete
		 CK_STATESAVE_BEHAVIORS			= 0x00000100,	// Obsolete
		 CK_STATESAVE_PARAMETERS		= 0x00000200,	// Obsolete
		 CK_STATESAVE_SINGLEACTIVITY	= 0x00000400,	// SINGLE ACTIVITY
		 CK_STATESAVE_SCRIPTS			= 0x00000800,	// Obsolete
		 CK_STATESAVE_BEOBJECTONLY		= 0x00000FF0,	// Save only BeObject specific datas
		 CK_STATESAVE_BEOBJECTALL		= 0x00000FFF	// Save All datas
} CK_STATESAVEFLAGS_BEOBJECT;


//------------------------------------------------									
// 3dEntity								
typedef enum CK_STATESAVEFLAGS_3DENTITY 
{
		 CK_STATESAVE_3DENTITYSKINDATANORMALS	= 0x00001000,	// Save Skin normals
		 CK_STATESAVE_ANIMATION			= 0x00002000,	// Obsolete
		 CK_STATESAVE_MESHS				= 0x00004000,	// Save List of mesh (and possibly blend shape weights for current mesh)
		 CK_STATESAVE_PARENT			= 0x00008000,	// Save Parent
		 CK_STATESAVE_3DENTITYFLAGS		= 0x00010000,	// Save Flags
		 CK_STATESAVE_3DENTITYMATRIX	= 0x00020000,	// Save Position/orientation/Scale
		 CK_STATESAVE_3DENTITYHIERARCHY	= 0x00040000,	// obsolete
		 CK_STATESAVE_3DENTITYPLACE		= 0x00080000,	// Save Place in which the Entity is referenced
		 CK_STATESAVE_3DENTITYNDATA		= 0x00100000,	// Reserved for future use 
		 CK_STATESAVE_3DENTITYSKINDATA	= 0x00200000,	// Save Skin data 
		 CK_STATESAVE_3DENTITYSKINDATA2 = 0x00220000,	// Save Skin data 
		 CK_STATESAVE_3DENTITYONLY		= 0x003FF000,	// Save only 3dEntity specific datas
		 CK_STATESAVE_3DENTITYALL		= 0x003FFFFF	// Save All datas for sub-classes
} CK_STATESAVEFLAGS_3DENTITY;
				
#define CK_STATESAVE_BLENDSHAPEWEIGHTS 0x4fae17d7 // Identifier for blend shape weights

//------------------------------------------------										
// Light								
typedef enum CK_STATESAVEFLAGS_LIGHT 
{
		 CK_STATESAVE_LIGHTDATA			= 0x00400000,	// Save Color,Type,Attenuation,Range and cone
		 CK_STATESAVE_LIGHTDATA2		= 0x00800000,	// Reserved for future use 
		 CK_STATESAVE_LIGHTRESERVED1	= 0x01000000,	// Reserved for future use 
		 CK_STATESAVE_LIGHTRESERVED2	= 0x02000000,	// Reserved for future use 
		 CK_STATESAVE_LIGHTRESERVED3	= 0x04000000,	// Reserved for future use 
		 CK_STATESAVE_LIGHTRESERVED4	= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_LIGHTONLY			= 0x0FC00000,	// Save only Light specific datas
		 CK_STATESAVE_LIGHTALL			= 0x0FFFFFFF,	// Save All datas for sub-classes
// Target Light
		 CK_STATESAVE_TLIGHTTARGET		= 0x80000000,	// Save Light Target
		 CK_STATESAVE_TLIGHTRESERVED0	= 0x10000000,	// Reserved for future use 
		 CK_STATESAVE_TLIGHTRESERVED1	= 0x20000000,	// Reserved for future use 
		 CK_STATESAVE_TLIGHTRESERVED2	= 0x40000000,	// Reserved for future use 
		 CK_STATESAVE_TLIGHTONLY		= 0xF0000000,	// Save only Target Light specific datas
		 CK_STATESAVE_TLIGHTALL			= 0xFFFFFFFF	// Save All datas for sub-classes
} CK_STATESAVEFLAGS_LIGHT;

//--------------------------------------------------
// Camera								
typedef enum CK_STATESAVEFLAGS_CAMERA 
{
		 CK_STATESAVE_CAMERAFOV				= 0x00400000,	// Save Camera Field of View
		 CK_STATESAVE_CAMERAPROJTYPE		= 0x00800000,	// Save Camera projection type
		 CK_STATESAVE_CAMERAOTHOZOOM		= 0x01000000,	// Save Camera orhographic zoom
		 CK_STATESAVE_CAMERAASPECT			= 0x02000000,	// Save Camera aspect ration
		 CK_STATESAVE_CAMERAPLANES			= 0x04000000,	// Save Camera near and far clip planes
		 CK_STATESAVE_CAMERARESERVED2		= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_CAMERAONLY			= 0x0FC00000,	// Save only camera specific datas	
		 CK_STATESAVE_CAMERAALL				= 0x0FFFFFFF,	// Save All datas for sub-classes	
// Target Camera
		 CK_STATESAVE_TCAMERATARGET			= 0x10000000,	// Save camera Target
		 CK_STATESAVE_TCAMERARESERVED1		= 0x20000000,	// Reserved for future use 
		 CK_STATESAVE_TCAMERARESERVED2		= 0x40000000,	// Reserved for future use 
		 CK_STATESAVE_TCAMERAONLY			= 0x70000000,	// Save only Target camera specific datas
		 CK_STATESAVE_TCAMERAALL			= 0x7FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_CAMERA;


//--------------------------------------------------
// Sprite3D								
typedef enum CK_STATESAVEFLAGS_SPRITE3D 
{
		 CK_STATESAVE_SPRITE3DDATA		= 0x00400000,		// Save offset,mapping,size and material
		 CK_STATESAVE_SPRITE3DRESERVED0	= 0x00800000,		// Reserved for future use 
		 CK_STATESAVE_SPRITE3DRESERVED1	= 0x01000000,		// Reserved for future use 
		 CK_STATESAVE_SPRITE3DRESERVED2	= 0x02000000,		// Reserved for future use 
		 CK_STATESAVE_SPRITE3DRESERVED3	= 0x04000000,		// Reserved for future use 
		 CK_STATESAVE_SPRITE3DRESERVED4	= 0x08000000,		// Reserved for future use 
		 CK_STATESAVE_SPRITE3DONLY		= 0x0FC00000,		// Save only Sprite3D specific datas
		 CK_STATESAVE_SPRITE3DALL		= 0x0FFFFFFF		// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SPRITE3D;

//--------------------------------------------------
// Object 3D								
typedef enum CK_STATESAVEFLAGS_3DOBJECT 
{
		 CK_STATESAVE_3DOBJECTATTRIBUTES	= 0x00400000,	// Obsolete		
		 CK_STATESAVE_3DOBJECTRESERVED		= 0x00800000,	// Reserved for future use 
		 CK_STATESAVE_3DOBJECTRONLY			= 0x00C00000,	// Save only 3dObject specific datas
		 CK_STATESAVE_3DOBJECTALL			= 0x03FFFFFF	// Save All datas for sub-classes			
} CK_STATESAVEFLAGS_3DOBJECT;

//--------------------------------------------------
// Object 3D								
typedef enum CK_STATESAVEFLAGS_3DPOINTCLOUD 
{
		 CK_STATESAVE_POINTCLOUD_DATA		= 0x00400000,		
		 CK_STATESAVE_POINTCLOUD_POINTS		= 0x00800000,		
		 CK_STATESAVE_POINTCLOUD_TREE		= 0x01000000,		
		 CK_STATESAVE_POINTCLOUD_ATTRIBUTES	= 0x02000000,		
		 CK_STATESAVE_POINTCLOUD_GROUPS		= 0x04000000,		
		 CK_STATESAVE_POINTCLOUD_SELECTIONS	= 0x08000000,		
		 CK_STATESAVE_POINTCLOUD_ONLY		= 0x0FC00000,	
		 CK_STATESAVE_POINTCLOUD_ALL		= 0x0FFFFFFF	// Save All datas for sub-classes			
} CK_STATESAVEFLAGS_3DPOINTCLOUD;

//--------------------------------------------------
// BodyPart							
typedef enum CK_STATESAVEFLAGS_BODYPART 
{
		 CK_STATESAVE_BODYPARTROTJOINT	= 0x01000000,		// Save rotation joint data
		 CK_STATESAVE_BODYPARTPOSJOINT	= 0x02000000,		// Save position joint data
		 CK_STATESAVE_BODYPARTCHARACTER	= 0x04000000,		// Save character owning this bodypart
		 CK_STATESAVE_BODYPARTRESERVED1	= 0x08000000,		// Reserved for future use 
		 CK_STATESAVE_BODYPARTRESERVED2	= 0x10000000,		// Reserved for future use 
		 CK_STATESAVE_BODYPARTRESERVED3	= 0x20000000,		// Reserved for future use 
		 CK_STATESAVE_BODYPARTRESERVED4	= 0x40000000,		// Reserved for future use 
		 CK_STATESAVE_BODYPARTONLY		= 0x7F000000,		// Save only bodypart specific datas
		 CK_STATESAVE_BODYPARTALL		= 0x7FFFFFFF		// Save All datas for sub-classes			
} CK_STATESAVEFLAGS_BODYPART;

//--------------------------------------------------
// Character							
typedef enum CK_STATESAVEFLAGS_CHARACTER 
{
		 CK_STATESAVE_CHARACTERBODYPARTS	= 0x00400000,	// Obsolete
		 CK_STATESAVE_CHARACTERKINECHAINS	= 0x00800000,	// Obsolete
		 CK_STATESAVE_CHARACTERANIMATIONS	= 0x01000000,	// Obsolete
		 CK_STATESAVE_CHARACTERROOT			= 0x02000000,	// Obsolete
		 CK_STATESAVE_CHARACTERSAVEANIMS	= 0x04000000,	// Save current and next active animations
		 CK_STATESAVE_CHARACTERSAVECHAINS	= 0x08000000,	// Obsolete
		 CK_STATESAVE_CHARACTERSAVEPARTS	= 0x10000000,	// Save sub bodyparts and sub-bodyparts data (saved with flag :CK_STATESAVE_BODYPARTALL)
		 CK_STATESAVE_CHARACTERFLOORREF		= 0x20000000,	// Save Character floor reference object
		 CK_STATESAVE_CHARACTERRESERVED2	= 0x40000000,	// Reserved for future use 
		 CK_STATESAVE_CHARACTERRESERVED3	= 0x80000000,	// Reserved for future use 
		 CK_STATESAVE_CHARACTERONLY			= 0xFFC00000,	// Save only character specific datas	
		 CK_STATESAVE_CHARACTERALL			= 0xFFFFFFFF	// Save All datas for sub-classes			
} CK_STATESAVEFLAGS_CHARACTER;

//--------------------------------------------------
// CURVE							
// && Curve Point							
typedef enum CK_STATESAVEFLAGS_CURVE 
{
		 CK_STATESAVE_CURVEFITCOEFF			= 0x00400000,	// Save fitting coef
		 CK_STATESAVE_CURVECONTROLPOINT		= 0x00800000,	// Save list of control points
		 CK_STATESAVE_CURVESTEPS			= 0x01000000,	// Save number of step setting
		 CK_STATESAVE_CURVEOPEN				= 0x02000000,	// Save Open/Close flag
		 CK_STATESAVE_CURVERESERVED1		= 0x04000000,	// Reserved for future use 
		 CK_STATESAVE_CURVERESERVED2		= 0x08000000,	// Reserved for future use 
// Control points
		 CK_STATESAVE_CURVEPOINTDEFAULTDATA	= 0x10000000,	// Save Control point setting and position
		 CK_STATESAVE_CURVEPOINTTCB			= 0x20000000,	// Save Control point tcb settings
		 CK_STATESAVE_CURVEPOINTTANGENTS	= 0x40000000,	// Save Control point tangents
		 CK_STATESAVE_CURVEPOINTCURVEPOS	= 0x80000000,	// Save Control point position in curve
		 CK_STATESAVE_CURVESAVEPOINTS		= 0xFF000000,	// Save control points data 
		 
		 CK_STATESAVE_CURVEONLY				= 0xFFC00000,	// Save only curve specific data	
		 CK_STATESAVE_CURVEALL				= 0xFFFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_CURVE;

//------------------------------------------------									
// 2dEntity								
typedef enum CK_STATESAVEFLAGS_2DENTITY 
{
		 CK_STATESAVE_2DENTITYSRCSIZE	= 0x00001000,		// Save source size
		 CK_STATESAVE_2DENTITYSIZE		= 0x00002000,		// Save size
		 CK_STATESAVE_2DENTITYFLAGS		= 0x00004000,		// Save Flags
		 CK_STATESAVE_2DENTITYPOS		= 0x00008000,		// Save position
		 CK_STATESAVE_2DENTITYZORDER    = 0x00100000,		// Save Z order
		 CK_STATESAVE_2DENTITYONLY		= 0x0010F000,		// Save only 2dEntity specific data 
		 CK_STATESAVE_2DENTITYMATERIAL  = 0x00200000,		// Save Material
		 CK_STATESAVE_2DENTITYHIERARCHY	= 0x00400000,		// Save Material
		 CK_STATESAVE_2DENTITYALL		= 0x0070FFFF		// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_2DENTITY;

//------------------------------------------------									
// Sprite								
typedef enum CK_STATESAVEFLAGS_SPRITE 
{
		 CK_STATESAVE_SPRITECURRENTIMAGE	= 0x00010000,	// Save current image
		 CK_STATESAVE_SPRITETRANSPARENT		= 0x00020000,	// Save transparency settings
		 CK_STATESAVE_SPRITEBITMAPS			= 0x00040000,	// Obsolete	
		 CK_STATESAVE_SPRITESHARED			= 0x00080000,	// Save shared sprite
		 CK_STATESAVE_SPRITEDONOTUSE		= 0x00100000,	// Reseved by CK_STATESAVEFLAGS_2DENTITY
		 CK_STATESAVE_SPRITEAVIFILENAME		= 0x00200000,	// Obsolete	
		 CK_STATESAVE_SPRITEFILENAMES		= 0x00400000,	// Obsolete
		 CK_STATESAVE_SPRITECOMPRESSED		= 0x00800000,	// Obsolete
		 CK_STATESAVE_SPRITEREADER			= 0x10000000,	// Reserved for future use 
		 CK_STATESAVE_SPRITEFORMAT			= 0x20000000,	// Reserved for future use 
		 CK_STATESAVE_SPRITEVIDEOFORMAT		= 0x40000000,	// Video Format 
		 CK_STATESAVE_SPRITESYSTEMCACHING	= 0x80000000,	// System Memory Caching
		 CK_STATESAVE_SPRITERENDEROPTIONS	= 0x80800000,	// Render options if any...
		 CK_STATESAVE_SPRITEONLY			= 0xF0EF0000,	// Save only sprite specific data 
		 CK_STATESAVE_SPRITEALL				= 0x70FFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SPRITE;


//------------------------------------------------									
// Sprite Text
typedef enum CK_STATESAVEFLAGS_SPRITETEXT 
{
		 CK_STATESAVE_SPRITETEXT			= 0x01000000,	// Save text
		 CK_STATESAVE_SPRITEFONT			= 0x02000000,	// Save font settings
		 CK_STATESAVE_SPRITETEXTCOLOR		= 0x04000000,	// Save text color
		 CK_STATESAVE_SPRITETEXTRESERVED	= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_SPRITETEXTDOTNOTUSE	= 0x10000000,	// Reserved by CK_STATESAVE_SPRITEREADER
		 CK_STATESAVE_SPRITETEXTDONOTUSED2	= 0x20000000,	// Reserved by CK_STATESAVE_SPRITEFORMAT
		 CK_STATESAVE_SPRITETEXTONLY		= 0x0F000000,	// Save only SpriteText specific data 	
		 CK_STATESAVE_SPRITETEXTALL			= 0x3FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SPRITETEXT;

//------------------------------------------------									
// Sound								
typedef enum CK_STATESAVEFLAGS_SOUND 
{
		 CK_STATESAVE_SOUNDFILENAME		= 0x00001000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED1	= 0x00002000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED2	= 0x00004000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED3	= 0x00008000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED4	= 0x00010000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED5	= 0x00020000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED6	= 0x00040000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDRESERVED7	= 0x00080000,	// Reserved for future use 
		 CK_STATESAVE_SOUNDONLY			= 0x000FF000,	// Save only Sound specific data 
		 CK_STATESAVE_SOUNDALL			= 0x000FFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SOUND;

//------------------------------------------------									
// Wave Sound								
typedef enum CK_STATESAVEFLAGS_WAVSOUND 
{
		 CK_STATESAVE_WAVSOUNDFILE		= 0x00100000,	// Save sound filename
		 CK_STATESAVE_WAVSOUNDDATA		= 0x00200000,	// Obsolete
		 CK_STATESAVE_WAVSOUNDDATA2		= 0x00400000,	// Save sound properties (3D/2D,pitch,gain,streaming,loop,etc..)
		 CK_STATESAVE_WAVSOUNDDURATION	= 0x00800000,	// Sound Length (in case it cannot be calculated latter)
		 CK_STATESAVE_WAVSOUNDRESERVED4	= 0x01000000,	// Reserved for future use 
		 CK_STATESAVE_WAVSOUNDRESERVED5	= 0x02000000,	// Reserved for future use 	
		 CK_STATESAVE_WAVSOUNDRESERVED6	= 0x04000000,	// Reserved for future use 
		 CK_STATESAVE_WAVSOUNDRESERVED7	= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_WAVSOUNDONLY		= 0x0FF00000,	// Save All datas for sub-classes	
		 CK_STATESAVE_WAVSOUNDALL		= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_WAVSOUND;


//------------------------------------------------									
// Wave Sound								
typedef enum CK_STATESAVEFLAGS_MIDISOUND 
{
		 CK_STATESAVE_MIDISOUNDFILE			= 0x00100000,	// Save sound filename
		 CK_STATESAVE_MIDISOUNDDATA			= 0x00200000,	// Save midi data 
		 CK_STATESAVE_MIDISOUNDRESERVED2	= 0x00400000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDRESERVED3	= 0x00800000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDRESERVED4	= 0x01000000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDRESERVED5	= 0x02000000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDRESERVED6	= 0x04000000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDRESERVED7	= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_MIDISOUNDONLY			= 0x0FF00000,
		 CK_STATESAVE_MIDISOUNDALL			= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_MIDISOUND;


//------------------------------------------------									
// Place								
typedef enum CK_STATESAVEFLAGS_PLACE 
{
		 CK_STATESAVE_PLACEPORTALS			= 0x00001000,	// Save level using the place
		 CK_STATESAVE_PLACECAMERA			= 0x00002000,	// Save attached camera	
		 CK_STATESAVE_PLACEREFERENCES		= 0x00004000,	// Save list of objects in the place
		 CK_STATESAVE_PLACELEVEL			= 0x00008000,	// Save level using the place
		 CK_STATESAVE_PLACEALL				= 0x0000FFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_PLACE;

//------------------------------------------------									
// Level
// CKSaveObjectState will not save any data
typedef enum CK_STATESAVEFLAGS_LEVEL 
{
		 CK_STATESAVE_LEVELRESERVED0	= 0x00001000,	// Reserved for future use 	
		 CK_STATESAVE_LEVELINACTIVEMAN	= 0x00002000,	// Reserved for future use 	
		 CK_STATESAVE_LEVELDUPLICATEMAN	= 0x00004000,	// Reserved for future use 	
		 CK_STATESAVE_LEVELDEFAULTDATA	= 0x20000000,	// Save Places,Scenes and Objects
		 CK_STATESAVE_LEVELSCENE		= 0x80000000,	// Default and active  scene
		 CK_STATESAVE_LEVELALL			= 0xFFFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_LEVEL;




//------------------------------------------------									
// GROUP
typedef enum CK_STATESAVEFLAGS_GROUP 
{
		 CK_STATESAVE_GROUPDATA			= 0x00001000,	// Save list of objects in the group 
		 CK_STATESAVE_GROUPRESERVED1	= 0x00002000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED2	= 0x00004000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED3	= 0x00008000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED4	= 0x00010000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED5	= 0x00020000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED6	= 0x00040000,	// Reserved for future use 
		 CK_STATESAVE_GROUPRESERVED7	= 0x00080000,	// Reserved for future use 
		 CK_STATESAVE_SELECTIONSET		= 0x00100000,	// save selection set data
		 CK_STATESAVE_GROUPALL			= 0x000FFFFF	// Save All datas for sub-classes
} CK_STATESAVEFLAGS_GROUP;


//------------------------------------------------									
// MESH
// CKSaveOjectSave will save all data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_MESH 
{
		 CK_STATESAVE_MESHRESERVED		= 0x00001000,	// Reserved for future use 
		 CK_STATESAVE_MESHFLAGS			= 0x00002000,	// Save flags
		 CK_STATESAVE_MESHCHANNELS		= 0x00004000,	// Save Channels 
		 CK_STATESAVE_MESHFACECHANMASK	= 0x00008000,	// Save face channel Mask
		 CK_STATESAVE_MESHFACES			= 0x00010000,	// Save face data
		 CK_STATESAVE_MESHFACES32		= 0x00011000,	// Save face data (32 bit)
		 CK_STATESAVE_MESHVERTICES		= 0x00020000,	// Save geometry
		 CK_STATESAVE_MESHLINES			= 0x00040000,	// Save line data
		 CK_STATESAVE_MESHLINES32		= 0x00041000,	// Save line data (32 bit)
		 CK_STATESAVE_MESHWEIGHTS		= 0x00080000,	// Save Vertex Weight info
		 CK_STATESAVE_MESHMATERIALS		= 0x00100000,	// Save Material Info
		 CK_STATESAVE_MESHCHANNELS_EXT	= 0x00200000,	// Extended Channels (float1-4)
		 CK_STATESAVE_MESHMATGROUPS		= 0x00400000,	// Save The mesh render groups (WORD Version)
		 CK_STATESAVE_MESHMATGROUPS32	= 0x00401000,	// Save The mesh render groups (DWORD Version)
		 CK_STATESAVE_PROGRESSIVEMESH	= 0x00800000,	// Progressive mesh info
		 CK_STATESAVE_MESHBLENDSHAPES	= 0x01000000,	// Progressive mesh info
		 CK_STATESAVE_MESHONLY			= 0x01FFF000,	// Save All datas for sub-classes	
		 CK_STATESAVE_MESHALL			= 0x01FFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_MESH;


//------------------------------------------------									
// PATCH MESH
// CKSaveOjectSave will save all data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_PATCHMESH 
{
		 CK_STATESAVE_PATCHMESHDATA			= 0x00800000,	// Obsolete
		 CK_STATESAVE_PATCHMESHDATA2		= 0x01000000,	// Obsolete
		 CK_STATESAVE_PATCHMESHSMOOTH		= 0x02000000,	// Obsolete
		 CK_STATESAVE_PATCHMESHMATERIALS	= 0x04000000,	// Obsolete
		 CK_STATESAVE_PATCHMESHDATA3		= 0x08000000,	// Save Patch Data
		 CK_STATESAVE_PATCHMESHONLY			= 0x0FF00000,	// Save All datas for sub-classes	
		 CK_STATESAVE_PATCHMESHALL			= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_PATCHMESH;

//-------------------------------------------------									
// Material
typedef enum CK_STATESAVEFLAGS_MATERIAL 
{
		 CK_STATESAVE_MATDATA			= 0x00001000,	// Save colors,blending modes,shade modes,fill modes etc...
		 CK_STATESAVE_MATDATA2			= 0x00002000,	// Additional texture objects...
		 CK_STATESAVE_MATDATA3			= 0x00004000,	// Effect Alone
		 CK_STATESAVE_MATDATA4			= 0x00008000,	// none
		 CK_STATESAVE_MATDATA5			= 0x00010000,	// Effect + parameter 
		 CK_STATESAVE_MATDATA6			= 0x00020000,	// MaterialShader (.fx)
		 CK_STATESAVE_MATRESERVED6		= 0x00040000,	// Reserved for future use 
		 CK_STATESAVE_MATRESERVED7		= 0x00080000,	// Reserved for future use		
		 CK_STATESAVE_MATERIALONLY		= 0x000FF000,	// Save All datas for sub-classes	
		 CK_STATESAVE_MATERIALALL		= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_MATERIAL;

//---------------------------------------------------									
// Texture
// CKSaveOjectSave will save all relevant data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_TEXTURE 
{
		 CK_STATESAVE_TEXAVIFILENAME	= 0x00001000,	// Save movie file name
		 CK_STATESAVE_TEXCURRENTIMAGE	= 0x00002000,	// Save current slot	
		 CK_STATESAVE_TEXBITMAPS		= 0x00004000,	// Obsolete
		 CK_STATESAVE_TEXTRANSPARENT	= 0x00008000,	// Save transparency data
		 CK_STATESAVE_TEXFILENAMES		= 0x00010000,	// Save texture slot filenames
		 CK_STATESAVE_TEXCOMPRESSED		= 0x00020000,	// Save raw texture data	
		 CK_STATESAVE_TEXVIDEOFORMAT	= 0x00040000,	// Save chosen video format
		 CK_STATESAVE_TEXSAVEFORMAT		= 0x00080000,	// Save chosen save format
		 CK_STATESAVE_TEXREADER			= 0x00100000,	// Save texture data using a specific BitmapReader
		 CK_STATESAVE_PICKTHRESHOLD		= 0x00200000,	// Save pick threshold
		 CK_STATESAVE_USERMIPMAP		= 0x00400000,	// User mipmap levels
		 CK_STATESAVE_TEXSYSTEMCACHING	= 0x00800000,	// System Memory Caching
		 CK_STATESAVE_OLDTEXONLY		= 0x002FF000,	// Kept for compatibility 
		 CK_STATESAVE_TEXONLY			= 0x00FFF000,	// Save Only Texture Data (Dot NOT MODIFY ! Texture loading/saving relies on this value)
		 CK_STATESAVE_TEXALL			= 0x002FFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_TEXTURE;


//---------------------------------------------------
// 2d CURVE							
// && 2d Curve Point							
typedef enum CK_STATESAVEFLAGS_2DCURVE 
{
		 CK_STATESAVE_2DCURVERESERVED0		 = 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_2DCURVERESERVED4		 = 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_2DCURVEFITCOEFF		 = 0x00000040,	// Obsolete
		 CK_STATESAVE_2DCURVECONTROLPOINT	 = 0x00000080,	// Obsolete
		 CK_STATESAVE_2DCURVENEWDATA		 = 0x00000100,	// Save All relevant data	
		 CK_STATESAVE_2DCURVERESERVED2		 = 0x00000200,	// Obsolete
		 CK_STATESAVE_2DCURVERESERVED3		 = 0x00000400,	// Obsolete
		 CK_STATESAVE_2DCURVEPOINTTCB		 = 0x00000800,	// Obsolete
		 CK_STATESAVE_2DCURVEPOINTTANGENTS	 = 0x00001000,	// Obsolete
		 CK_STATESAVE_2DCURVEPOINT2DCURVEPOS = 0x00002000,	// Obsolete
		 CK_STATESAVE_2DCURVEPOINTDEFAULTDATA= 0x00004000,	// Obsolete
		 CK_STATESAVE_2DCURVEPOINTNEWDATA	 = 0x00008000,	// Save All relevant data	
		 CK_STATESAVE_2DCURVEPOINTRESERVED1	 = 0x00010000,	// Reserved for future use 
		 CK_STATESAVE_2DCURVEPOINTRESERVED2	 = 0x00020000,	// Reserved for future use 
		 CK_STATESAVE_2DCURVESAVEPOINTS		 = 0x0003F800,	// Obsolete
		 CK_STATESAVE_2DCURVEALL			 = 0x0007FFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_2DCURVE;

//---------------------------------------------------
// Kinematic Chain

typedef enum CK_STATESAVEFLAGS_KINEMATICCHAIN 
{
		 CK_STATESAVE_KINEMATICCHAINDATA	  = 0x00000010,	// Save chain data
		 CK_STATESAVE_KINEMATICCHAINRESERVED1 = 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_KINEMATICCHAINRESERVED2 = 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_KINEMATICCHAINRESERVED3 = 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_KINEMATICCHAINALL		  = 0x000000FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_KINEMATICCHAIN;

//---------------------------------------------------
// Animation
typedef enum CK_STATESAVEFLAGS_ANIMATION 
{
		 CK_STATESAVE_ANIMATIONDATA				= 0x00000010,	// Save Flags & Framerate data
		 CK_STATESAVE_ANIMATIONRESERVED1		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_ANIMATIONLENGTH			= 0x00000040,	// Save animation Length
		 CK_STATESAVE_ANIMATIONBODYPARTS		= 0x00000080,	// Save root & list of bodypart
		 CK_STATESAVE_ANIMATIONCHARACTER		= 0x00000100,	// Save character	
		 CK_STATESAVE_ANIMATIONCURRENTSTEP		= 0x00000200,	// Save current step
		 CK_STATESAVE_ANIMATIONRESERVED5		= 0x00000400,	// Reserved for future use 
		 CK_STATESAVE_ANIMATIONRESERVED6		= 0x00000800,	// Reserved for future use 
		 CK_STATESAVE_ANIMATIONALL				= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_ANIMATION;

//---------------------------------------------------
// Keyed Anim
typedef enum CK_STATESAVEFLAGS_KEYEDANIMATION 
{
		 CK_STATESAVE_KEYEDANIMANIMLIST		= 0x00001000,	// Save list of object animations
		 CK_STATESAVE_KEYEDANIMLENGTH		= 0x00002000,	// Obsolete	
		 CK_STATESAVE_KEYEDANIMPOSKEYS		= 0x00004000,	// Obsolete
		 CK_STATESAVE_KEYEDANIMROTKEYS		= 0x00008000,	// Obsolete	
		 CK_STATESAVE_KEYEDANIMMORPHKEYS	= 0x00010000,	// Obsolete
		 CK_STATESAVE_KEYEDANIMSCLKEYS		= 0x00020000,	// Obsolete
		 CK_STATESAVE_KEYEDANIMFLAGS		= 0x00040000,	// Obsolete
		 CK_STATESAVE_KEYEDANIMENTITY		= 0x00080000,	// Obsolete
		 CK_STATESAVE_KEYEDANIMMERGE		= 0x00100000,	// Save merged animations
		 CK_STATESAVE_KEYEDANIMSUBANIMS		= 0x00200000,	// Save object animations data (using same flags than CKSaveObjectState)
		 CK_STATESAVE_KEYEDANIMRESERVED0	= 0x00400000,	// Reserved for future use 
		 CK_STATESAVE_KEYEDANIMRESERVED1	= 0x00800000,	// Reserved for future use 
		 CK_STATESAVE_KEYEDANIMRESERVED2	= 0x01000000,	// Reserved for future use 
		 CK_STATESAVE_KEYEDANIMRESERVED3	= 0x02000000	// Reserved for future use 	
} CK_STATESAVEFLAGS_KEYEDANIMATION;


//---------------------------------------------------
// Object Animation
// CKSaveOjectSave will save all relevant data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_OBJECTANIMATION 
{
		 CK_STATESAVE_OBJANIMNEWDATA		= 0x00001000,	// Save all relevant data
		 CK_STATESAVE_OBJANIMLENGTH			= 0x00002000,	// Not used
		 CK_STATESAVE_OBJANIMPOSKEYS		= 0x00004000,	// Not used
		 CK_STATESAVE_OBJANIMROTKEYS		= 0x00008000,	// Not used
		 CK_STATESAVE_OBJANIMMORPHKEYS		= 0x00010000,	// Not used
		 CK_STATESAVE_OBJANIMSCLKEYS		= 0x00020000,	// Not used
		 CK_STATESAVE_OBJANIMFLAGS			= 0x00040000,	// Not used
		 CK_STATESAVE_OBJANIMENTITY			= 0x00080000,	// Not used
		 CK_STATESAVE_OBJANIMMERGE			= 0x00100000,	// Not used
		 CK_STATESAVE_OBJANIMMORPHKEYS2		= 0x00200000,	// Not used
		 CK_STATESAVE_OBJANIMNEWSAVE1		= 0x00400000,	// Not used
		 CK_STATESAVE_OBJANIMMORPHNORMALS	= 0x00800000,	// Not used (Virtools 1.1)
		 CK_STATESAVE_OBJANIMMORPHCOMP		= 0x01000000,	// Not used (Virtools 1.1)
		 CK_STATESAVE_OBJANIMSHARED			= 0x02000000,	// Save Data for a shared animation
		 CK_STATESAVE_OBJANIMCONTROLLERS	= 0x04000000,	// (Virtools 1.5) Save All Controller information
		 CK_STATESAVE_OBJBLENDSHAPECONTROLLERS	= 0x08000000,	// (Virtools 1.5) Save All Controller information
		 CK_STATESAVE_OBJANIMONLY			= 0x0FFFF000,
		 CK_STATESAVE_OBJANIMALL			= 0x0FFFFFFF,
		 CK_STATESAVE_KEYEDANIMONLY			= 0x03FFF000,	// Save All datas for sub-classes	
		 CK_STATESAVE_KEYEDANIMALL			= 0x03FFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_OBJECTANIMATION;


//---------------------------------------------------
// IK Animation
typedef enum CK_STATESAVEFLAGS_IKANIMATION 
{
		 CK_STATESAVE_IKANIMATIONDATA		= 0x00001000,	// Save IK data
		 CK_STATESAVE_IKANIMATIONRESERVED2	= 0x00002000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED3	= 0x00004000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED4	= 0x00008000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED5	= 0x00010000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED6	= 0x00020000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED7	= 0x00040000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED8	= 0x00100000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONRESERVED9	= 0x00200000,	// Reserved for future use 
		 CK_STATESAVE_IKANIMATIONALL		= 0x003FFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_IKANIMATION;

//---------------------------------------------------
// BehaviorLink
typedef enum CK_STATESAVEFLAGS_BEHAV_LINK 
{
		 CK_STATESAVE_BEHAV_LINK_CURDELAY	= 0x00000004,	// Obsolete
		 CK_STATESAVE_BEHAV_LINK_IOS		= 0x00000008,	// Obsolete
		 CK_STATESAVE_BEHAV_LINK_DELAY		= 0x00000010,	// Obsolete	
		 CK_STATESAVE_BEHAV_LINK_NEWDATA	= 0x00000020,	// Save all relevant data (In,Out,Activation delay)	
		 CK_STATESAVE_BEHAV_LINKRESERVED5	= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_BEHAV_LINKRESERVED6	= 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_BEHAV_LINKONLY		= 0x000000F0,	// 
		 CK_STATESAVE_BEHAV_LINKALL			= 0x000000FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_BEHAV_LINK;

//---------------------------------------------------
// BehaviorIO
typedef enum CK_STATESAVEFLAGS_BEHAV_IO 
{
		 CK_STATESAVE_BEHAV_IOFLAGS			= 0x00000008,	// Save IO flags	
		 CK_STATESAVE_BEHAV_IORESERVED3		= 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_BEHAV_IORESERVED4		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_BEHAV_IORESERVED5		= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_BEHAV_IORESERVED6		= 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_BEHAVIOONLY			= 0x000000F0,	// 
		 CK_STATESAVE_BEHAVIOALL			= 0x000000FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_BEHAV_IO;
										
//---------------------------------------------------
// BehaviorPrototype
typedef enum CK_STATESAVEFLAGS_PROTOTYPE 
{
		 CK_STATESAVE_PROTORESERVED0		= 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_PROTORESERVED1		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_PROTOFLAGS			= 0x00000040,	// Save Flags
		 CK_STATESAVE_PROTOSUBPROTOS		= 0x00000080,	// Save sub prototypes
		 CK_STATESAVE_PROTOLINKS			= 0x00000100,	// Save links	
		 CK_STATESAVE_PROTOBEHAVFLAG		= 0x00000200,	// Save behavior flags
		 CK_STATESAVE_PROTOGUID				= 0x00000400,	// Save GUID
		 CK_STATESAVE_PROTOINPUTS			= 0x00000800,	// Save inputs
		 CK_STATESAVE_PROTOOUTPUTS			= 0x00001000,	// Save outputs
		 CK_STATESAVE_PROTOINPARAMS			= 0x00002000,	// Save input parameters
		 CK_STATESAVE_PROTOOUTPARAMS		= 0x00004000,	// Save output parameters
		 CK_STATESAVE_PROTOLOCALPARAMS		= 0x00008000,	// Save local parameters
		 CK_STATESAVE_PROTOOPERATIONS		= 0x00010000,	// Save parameter operations
		 CK_STATESAVE_PROTOPARAMETERLINKS	= 0x00020000,	// Save parameter links
		 CK_STATESAVE_PROTOAPPLYTO			= 0x00040000,	// Save ClassID of object to which it applies
		 CK_STATESAVE_PROTORESERVED14		= 0x00080000,	// Reserved for future use 
		 CK_STATESAVE_PROTOALL				= 0x000FFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_PROTOTYPE;
										

//---------------------------------------------------
// Behavior
typedef enum CK_STATESAVEFLAGS_BEHAVIOR 
{
		 CK_STATESAVE_BEHAVIORRESERVED0		= 0x00000010,	// Reserved for internal use
		 CK_STATESAVE_BEHAVIORNEWDATA		= 0x00000020,	// not used
		 CK_STATESAVE_BEHAVIORFLAGS			= 0x00000040,	// not used
		 CK_STATESAVE_BEHAVIORCOMPATIBLECID	= 0x00000080,	// not used
		 CK_STATESAVE_BEHAVIORSUBBEHAV		= 0x00000100,	// Save Sub-Behaviors
		 CK_STATESAVE_BEHAVIORINPARAMS		= 0x00000200,	// not used
		 CK_STATESAVE_BEHAVIOROUTPARAMS		= 0x00000400,	// not used
		 CK_STATESAVE_BEHAVIORINPUTS		= 0x00000800,	// not used
		 CK_STATESAVE_BEHAVIOROUTPUTS		= 0x00001000,	// not used
		 CK_STATESAVE_BEHAVIORINFO			= 0x00002000,	// not used
		 CK_STATESAVE_BEHAVIOROPERATIONS	= 0x00004000,	// not used
		 CK_STATESAVE_BEHAVIORTYPE			= 0x00008000,	// not used
		 CK_STATESAVE_BEHAVIOROWNER			= 0x00010000,	// not used
		 CK_STATESAVE_BEHAVIORLOCALPARAMS	= 0x00020000,	// Save local parameters
		 CK_STATESAVE_BEHAVIORPROTOGUID		= 0x00040000,	// not used
		 CK_STATESAVE_BEHAVIORSUBLINKS		= 0x00080000,	// not used
		 CK_STATESAVE_BEHAVIORACTIVESUBLINKS= 0x00100000,	// not used
		 CK_STATESAVE_BEHAVIORSINGLEACTIVITY= 0x00200000,	// SINGLE ACTIVITY
		 CK_STATESAVE_BEHAVIORSCRIPTDATA	= 0x00400000,	// not used
		 CK_STATESAVE_BEHAVIORPRIORITY		= 0x00800000,	// not used
		 CK_STATESAVE_BEHAVIORTARGET		= 0x01000000,	// not used
		 CK_STATESAVE_BEHAVIORONLY			= 0x01FFFFF0,	
		 CK_STATESAVE_BEHAVIORALL			= 0x01FFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_BEHAVIOR;




//---------------------------------------------------
// SCENE 
// CKSaveOjectSave will save all relevant data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_SCENE 
{
		 CK_STATESAVE_SCENERESERVED0			= 0x00001000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED8			= 0x00002000,	// Reserved for future use 
		 CK_STATESAVE_SCENEFLAGS				= 0x00004000,	
		 CK_STATESAVE_SCENELEVEL				= 0x00008000,	
		 CK_STATESAVE_SCENEOBJECTS				= 0x00010000,
		 CK_STATESAVE_SCENENEWDATA				= 0x00020000,	// every object description and initial conditions
		 CK_STATESAVE_SCENELAUNCHED				= 0x00040000,	// Scene was already launched once	
		 CK_STATESAVE_SCENERENDERSETTINGS		= 0x00080000,	// Background Color, Fog Color etc..
		 CK_STATESAVE_SCENERESERVED1			= 0x00100000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED2			= 0x00200000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED3			= 0x00400000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED4			= 0x00800000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED5			= 0x01000000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED12			= 0x02000000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED13			= 0x04000000,	// Reserved for future use 
		 CK_STATESAVE_SCENERESERVED14			= 0x08000000,	// Reserved for future use 
		 CK_STATESAVE_SCENEALL					= 0x0FFFFFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SCENE;


//---------------------------------------------------
// ParameterIn
typedef enum CK_STATESAVEFLAGS_PARAMETERIN 
{
		 CK_STATESAVE_PARAMETERIN_RESERVED4		= 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_PARAMETERIN_RESERVED0		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_PARAMETERIN_RESERVED1		= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_PARAMETERIN_OWNER			= 0x00000080,	// Obsolete
		 CK_STATESAVE_PARAMETERIN_INSHARED		= 0x00000100,	// Obsolete
		 CK_STATESAVE_PARAMETERIN_OUTSOURCE		= 0x00000200,	// Obsolete
		 CK_STATESAVE_PARAMETERIN_DEFAULTDATA	= 0x00000400,	// Obsolete
		 CK_STATESAVE_PARAMETERIN_DATASHARED	= 0x00000800,	// Save reference to shared inparameter
		 CK_STATESAVE_PARAMETERIN_DATASOURCE	= 0x00001000,	// Save reference to source outparameter
		 CK_STATESAVE_PARAMETERIN_DISABLED		= 0x00002000,	// The parameter was disabled
		 CK_STATESAVE_PARAMETERIN_ALL			= 0x0000FFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_PARAMETERIN;

//---------------------------------------------------
// ParameterLocal et ParameterOut
typedef enum CK_STATESAVEFLAGS_PARAMETEROUT 
{
		 CK_STATESAVE_PARAMETEROUT_RESERVED0		= 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_PARAMETEROUT_DESTINATIONS		= 0x00000020,	// Save destinations 
		 CK_STATESAVE_PARAMETEROUT_VAL				= 0x00000040,	// Save value
		 CK_STATESAVE_PARAMETEROUT_OWNER			= 0x00000080,	// Save Owner
		 CK_STATESAVE_PARAMETEROUT_MYSELF			= 0x00000200,	// 	
		 CK_STATESAVE_PARAMETEROUT_ISSETTING		= 0x00000400,	// Reserved for future use 
		 CK_STATESAVE_PARAMETEROUT_ALL				= 0x0000FFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_PARAMETEROUT;


//---------------------------------------------------
// Parameter Operation

typedef enum CK_STATESAVEFLAGS_OPERATION 
{
		 CK_STATESAVE_OPERATIONRESERVED0		= 0x00000010,	// Reserved for future use 
		 CK_STATESAVE_OPERATIONRESERVED1		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_OPERATIONINPUTS			= 0x00000040,
		 CK_STATESAVE_OPERATIONOUTPUT			= 0x00000080,
		 CK_STATESAVE_OPERATIONOP				= 0x00000100,
		 CK_STATESAVE_OPERATIONDEFAULTDATA		= 0x00000200,
		 CK_STATESAVE_OPERATIONNEWDATA			= 0x00000400,
		 CK_STATESAVE_OPERATIONALL				= 0x000007FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_OPERATION;


//---------------------------------------------------
// Synchro Object
// CKSaveOjectSave will save all relevant data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_SYNCHRO 
{
		 CK_STATESAVE_SYNCHRODATA				= 0x00000010,	// Save data
		 CK_STATESAVE_SYNCHRORESERVED0			= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_SYNCHRORESERVED1			= 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_SYNCHRORESERVED2			= 0x00000100,	// Reserved for future use 
		 CK_STATESAVE_SYNCHRORESERVED3			= 0x00000200,	// Reserved for future use 
		 CK_STATESAVE_SYNCHRONALL				= 0x000003FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_SYNCHRO;

//------------------------------------------------										
// Grid								
typedef enum CK_STATESAVEFLAGS_GRID 
{
		 CK_STATESAVE_GRIDDATA		= 0x00400000,		// Save Grid Data
		 CK_STATESAVE_GRIDRESERVED0	= 0x00800000,		// Reserved for future use 
		 CK_STATESAVE_GRIDRESERVED1	= 0x01000000,		// Reserved for future use 
		 CK_STATESAVE_GRIDRESERVED2	= 0x02000000,		// Reserved for future use 
		 CK_STATESAVE_GRIDRESERVED3	= 0x04000000,		// Reserved for future use 
		 CK_STATESAVE_GRIDRESERVED4	= 0x08000000,		// Reserved for future use 
		 CK_STATESAVE_GRIDONLY		= 0x0FC00000,		// 
		 CK_STATESAVE_GRIDALL		= 0x0FFFFFFF		// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_GRID;


//------------------------------------------------										
// Layer (For Grids)
typedef enum CK_STATESAVEFLAGS_LAYER 
{
		 CK_STATESAVE_LAYERDATA			= 0x00000010,	// Save Layer Data
		 CK_STATESAVE_LAYERRESERVED0	= 0x00800020,	// Reserved for future use 
		 CK_STATESAVE_LAYERRESERVED1	= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_LAYERRESERVED2	= 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_LAYERRESERVED3	= 0x00000100,	// Reserved for future use 
		 CK_STATESAVE_LAYERRESERVED4	= 0x00000200,	// Reserved for future use 
		 CK_STATESAVE_LAYERONLY			= 0x000003F0,	// 
		 CK_STATESAVE_LAYERALL			= 0x000003FF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_LAYER;

//------------------------------------------------									
// DataArray
// CKSaveOjectSave will save all relevant data and does not take flags into account
typedef enum CK_STATESAVEFLAGS_DATAARRAY
{
		 CK_STATESAVE_DATAARRAYFORMAT	= 0x00001000,	// Save format
		 CK_STATESAVE_DATAARRAYDATA		= 0x00002000,	// Save array data
		 CK_STATESAVE_DATAARRAYMEMBERS	= 0x00004000,	// Save members
		 CK_STATESAVE_DATAARRAYALL		= 0x0000FFFF	// Save All datas for sub-classes	
} CK_STATESAVEFLAGS_DATAARRAY;


//------------------------------------------------
// SceneObjectDesc	
typedef enum CK_STATESAVEFLAGS_SCENEOBJECTDESC {

		 CK_STATESAVE_SCENEOBJECTDESC		= 0x00000010,
		 CK_STATESAVE_SCENEOBJECTRES1		= 0x00000020,	// Reserved for future use 
		 CK_STATESAVE_SCENEOBJECTRES2		= 0x00000040,	// Reserved for future use 
		 CK_STATESAVE_SCENEOBJECTRES3		= 0x00000080,	// Reserved for future use 
		 CK_STATESAVE_SCENEOBJECTDESCALL	= 0x000000FF	// Save All datas for sub-classes	

} CK_STATESAVEFLAGS_SCENEOBJECTDESC;



#endif