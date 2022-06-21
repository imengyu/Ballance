/*************************************************************************/
/*	File : CKDependenciesConstants.h									 */
/*	Author:  Romain Sididris											 */	
/*																		 */	
/*	Default Delete,Copy,Replace and Save options for Virtools Classes		 */	
/*	dependencies														 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef _DEPENDENCIESCONSTANTS_H

#define _DEPENDENCIESCONSTANTS_H

// Destroy Dependencies
// Defines which dependencies of an object  should also be destroyed
#define CK_DEPENDENCIES_DESTROY_BEOBJECT_SCRIPTS		1	// Destroy Object scripts, Default : On
#define CK_DEPENDENCIES_DESTROY_MATERIAL_TEXTURE		1	// Destroy Material texture, Default : Off
#define CK_DEPENDENCIES_DESTROY_MESH_MATERIAL			1   // Destroy Mesh materials, Default : Off
#define CK_DEPENDENCIES_DESTROY_3DENTITY_MESH			1	// Destroy 3DEntity meshes, Default : Off	
#define CK_DEPENDENCIES_DESTROY_3DENTITY_CHILDREN		2	// Destroy 3DEntity hierarchy, Default : Off
#define CK_DEPENDENCIES_DESTROY_3DENTITY_ANIMATIONS		4	// Destroy 3DEntity animations, Default : On
#define CK_DEPENDENCIES_DESTROY_DATAARRAY_OBJECTS		1	// Destroy objects referenced by a DataArray, Default: Off
#define CK_DEPENDENCIES_DESTROY_PLACE_PORTALS			1	// Destroy portals referenced by a Plane, Default : Off
#define CK_DEPENDENCIES_DESTROY_SPRITE3D_MATERIAL		1	// Destroy Sprite3D material, Default : Off
#define CK_DEPENDENCIES_DESTROY_TARGETLIGHT_TARGET		1	// Destroy TargetLight target 3DEntity, Default : Off
#define CK_DEPENDENCIES_DESTROY_TARGETCAMERA_TARGET		1	// Destroy TargetCamera target 3DEntity, Default : Off
#define CK_DEPENDENCIES_DESTROY_GROUP_OBJECTS			1	// Destroy every objects referenced by the Group, Default : Off

// Copy Dependencies
// Defines which dependencies of an object should also be copied
#define CK_DEPENDENCIES_COPY_OBJECT_NAME			1		// Copy object name, Default : On
#define CK_DEPENDENCIES_COPY_OBJECT_UNIQUENAME		2		// Ensure name uniqueness when copying, Default : On
#define CK_DEPENDENCIES_COPY_SCENEOBJECT_SCENES		1		// Add copy to every scenes the source was belonging to, Default : On
#define CK_DEPENDENCIES_COPY_BEOBJECT_SCRIPTS		1		// Copy BeObject scripts, Default : On
#define CK_DEPENDENCIES_COPY_BEOBJECT_ATTRIBUTES	2		// Copy BeObject attributes, Default : On
#define CK_DEPENDENCIES_COPY_BEOBJECT_GROUPS		4		// If BeObject belongs to a group, adds its copy to the group, Default : On
#define CK_DEPENDENCIES_COPY_MATERIAL_TEXTURE		1		// Copy Material texture, Default : Off 
#define CK_DEPENDENCIES_COPY_MESH_MATERIAL			1		// Copy Mesh materials, Default : Off
#define CK_DEPENDENCIES_COPY_3DENTITY_MESH			1		// Copy 3DEntity meshes, Default : On
#define CK_DEPENDENCIES_COPY_3DENTITY_CHILDREN		2		// Copy 3DEntity hierarchy, Default : On
#define CK_DEPENDENCIES_COPY_3DENTITY_ANIMATIONS	4		// Copy 3DEntity animation, Default : On
#define CK_DEPENDENCIES_COPY_CHARACTER_SHAREANIMS	1		// When copying a character shares its animation data, Default : On
#define CK_DEPENDENCIES_COPY_DATAARRAY_OBJECTS		1		// When copying a DataArray, this options also copies the objects referenced, Default : Off
#define CK_DEPENDENCIES_COPY_DATAARRAY_DATA			2		// Copy DataArray contents, Default : On
#define CK_DEPENDENCIES_COPY_PLACE_PORTALS			1		// Copy portals referenced by a Plane, Default : On
#define CK_DEPENDENCIES_COPY_SPRITE_SHAREBITMAP		1		// Sprite copy will share its bitmap data with the source sprite, Default : On
#define CK_DEPENDENCIES_COPY_SPRITE3D_MATERIAL		1		// Copy Sprite3D material, Default : On
#define CK_DEPENDENCIES_COPY_TARGETLIGHT_TARGET		1		// Copy TargetLight target 3DEntity, Default : On
#define CK_DEPENDENCIES_COPY_TARGETCAMERA_TARGET	1		// Copy TargetCamera target 3DEntity, Default : On
#define CK_DEPENDENCIES_COPY_GROUP_OBJECTS			1		// Copy every objects referenced by the Group, Default : On
#define CK_DEPENDENCIES_COPY_GRID_LAYERS			1		// Copy Grid layers, Default : On
#define CK_DEPENDENCIES_COPY_GRIDLAYER_DATA			1		// Copy Layer Data, Default : On

// Save Dependencies
// Defines which dependencies of an object should also be saved
#define CK_DEPENDENCIES_SAVE_OBJECT_NAME			1		// Save Object name, Default : On
#define CK_DEPENDENCIES_SAVE_BEOBJECT_SCRIPTS		1		// Save BeObject scripts, Default : On
#define CK_DEPENDENCIES_SAVE_BEOBJECT_ATTRIBUTES	2		// Save BeObject attributes, Default : On
#define CK_DEPENDENCIES_SAVE_BEOBJECT_GROUPS		4		// Save groups this object belongs to, Default : On
#define CK_DEPENDENCIES_SAVE_MATERIAL_TEXTURE		1		// Save Material texture, Default : On 
#define CK_DEPENDENCIES_SAVE_MESH_MATERIAL			1		// Save Mesh materials, Default : On
#define CK_DEPENDENCIES_SAVE_3DENTITY_MESH			1		// Save 3DEntity meshes, Default : On
#define CK_DEPENDENCIES_SAVE_3DENTITY_CHILDREN		2		// Save 3DEntity children, Default : On
#define CK_DEPENDENCIES_SAVE_3DENTITY_ANIMATIONS	4		// Save 3DEntity animations, Default : On
#define CK_DEPENDENCIES_SAVE_PLACE_PORTALS			1		// Save portals referenced by a Plane, Default : On
#define CK_DEPENDENCIES_SAVE_SPRITE3D_MATERIAL		1		// Save Sprite3D material, Default : On
#define CK_DEPENDENCIES_SAVE_TARGETLIGHT_TARGET		1		// Save TargetLight target 3DEntity, Default : On
#define CK_DEPENDENCIES_SAVE_TARGETCAMERA_TARGET	1		// Save TargetCamera target 3DEntity, Default : On
#define CK_DEPENDENCIES_SAVE_GROUP_OBJECTS			1		// Save every objects referenced by the Group, Default : On

#endif