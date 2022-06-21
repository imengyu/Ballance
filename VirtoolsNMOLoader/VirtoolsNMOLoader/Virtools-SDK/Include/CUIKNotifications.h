/****************************************************************************************/
/*	 CUIKNOTIFICATIONS.H																		*/
/****************************************************************************************/

#ifndef _CUIKNOTIFICATIONS_H_
#define _CUIKNOTIFICATIONS_H_

//CUIK_NOTIFICATION-----------------------------------------------
typedef enum
{
	// refreshes variable manager
	CUIK_NOTIFICATION_REFRESHVARIABLEMANAGER,			
	// *****Private to Virtools Interface*****
	// notify a plugin editor that its fixed size has changed. Used by DllEditorDlg::UpdateEditorSize()
	//param1 : editor dlg HWND
	CUIK_NOTIFICATION_RESIZEDLLEDITOR,					
	// *****Private to Virtools Interface*****
	// ask a plugin editor to close. Used by DllEditorDlg::Close()
	// param1 : editor dlg HWND.
	CUIK_NOTIFICATION_CLOSEDLLEDITOR,					
	// *****Private to Virtools Interface*****
	// ask listening editors (3dLayout & Level View) to add their selection to ScriptInterfaceAction::Context g_ActionContext.
	// param1 : ScriptContextAction*
	CUIK_NOTIFICATION_FILLACTIONCONTEXTWITHSELECTION,
	// *****Private to Virtools Interface*****
	// ask listening editors (3dLayout & Level View) to update their selection from the ScriptInterfaceAction::Context
	// param1 : ScriptContextAction*
	CUIK_NOTIFICATION_UPDATESELECTIONFROMACTIONCONTEXT,
	//  before  a ck file is loaded
	// but before objects are added to scene/place/that kind of stuff.
	// param1 : CKObjectArray* list of object loaded
	CUIK_NOTIFICATION_PRECKFILELOADED,					
	// after a CK file has been loaded and objects have been added to scene/place/level/...
	// this is called after loading file such a .nms ,.nmo  files (BUT NOT CMO files, ),
	// from Virtools Dev's user interface
	// that is, this notification is NOT sent when a file is loaded through a building block
	// such as the ObjectLoad BB.
	// mostly by level view & 3d view
	// note : when loading a cmo file with a level, CUIK_NOTIFICATION_LEVEL_LOADED is called, not this notification
	// param1 : CKObjectArray* : list of loaded objects.
	CUIK_NOTIFICATION_CKFILELOADED,						
	// the field of view of a camera has changed
	// param1 : CK_ID of camera
	CUIK_NOTIFICATION_FOVCHANGED,						
	// the orthographic zoom of a camera has changed
	// param1 : CK_ID of camera
	CUIK_NOTIFICATION_ORTHOZOOMCHANGED,					
	// to attribute a new object to a setup and update the setup
	// param1 : CK_ID of entity
	CUIK_NOTIFICATION_ENTITYSET,						
	// curve was modified so its size changed
	// param1 : CK_ID of curve
	CUIK_NOTIFICATION_CURVELENGTHCHANGED,				
	// current curvepoint changed
	// param1 : CK_ID, 
	CUIK_NOTIFICATION_SELECTEDPOINTCHANGED,				
	// object has been renamed
	// param1 : CK_ID of object
	CUIK_NOTIFICATION_OBJECTRENAMED,					
	// before clear all (new composition)
	CUIK_NOTIFICATION_PRECLEARALL,
	// after clear all (new composition)
	CUIK_NOTIFICATION_POSTCLEARALL,
	// notify the schematic that a script has been added
	// param1 : CKBehavior* beh
	CUIK_NOTIFICATION_ADDSCRIPTBEHAVIOR,				
	// 3D object position has changed
	// param1 : CK_ID of object
	CUIK_NOTIFICATION_OBJECTPOSCHANGED,					
	// 3D object orientation has changed
	// param1 : CK_ID of object
	CUIK_NOTIFICATION_OBJECTROTCHANGED,					
	// 3D object scale has changed
	// param1 : CK_ID of object
	CUIK_NOTIFICATION_OBJECTSCALECHANGED,				
	// before level/objects is/are saved
	// param1 : CKObjectArray* , list of objects to save
	// return value : -1 or negate value to prevent saving
	CUIK_NOTIFICATION_PRESAVELEVEL,						
	// after level/objects has/have been saved
	// param1 : CKObjectArray* , list of objects saved
	CUIK_NOTIFICATION_POSTSAVELEVEL,					
	// trace mode has been activated or deactivated
	// mainly used for schematic, but can be used for other trace purposes
	// Param 1 : BOOL activation
	CUIK_NOTIFICATION_TRACE,
	// send by Virtools Dev main app each behavioral frame, when trace mode is active
	CUIK_NOTIFICATION_DEBUGTRACE,
	// redraws schematic
	// param1 : NULL or VPosition* (object to redraw) (Private to Virtools Interface)
	CUIK_NOTIFICATION_SCHEMATICREDRAW,		
	// update scripts position and size in schematic then redraw
	// to use when some scripts have been added/deleted(/hidden/showed/moved, private to Virtools Interface)
	CUIK_NOTIFICATION_SCHEMATICUPDATESCRIPTS,
	// to send before copying behaviors/scripts, in order for schematic data to be prepared for the copy to come
	// param1 : XObjectArray* , list of CKBehavior*
	CUIK_NOTIFICATION_PRECOPYBEHAVIOR,					
	// to send after copying behaviors, in order for schematic to build the graph associated to the copy 
	// and to clear the data used for building the graph
	// param1 : XObjectArray* , list of CKBehavior*
	CUIK_NOTIFICATION_POSTCOPYBEHAVIOR,					
	// focus script in schematic view
	// param1 : CKBehavior*	(script) or CKBeObject* (then schematic will set the focus on its first script 
	CUIK_NOTIFICATION_FOCUSSCRIPT,						
	// focus object in schematic
	// Param1 : CKBehavior* or CKParameter*
	// Param2 : BOOL, select object if TRUE
	CUIK_NOTIFICATION_FOCUSOBJECT_SCHEMATIC,			
	// focus object in level manager
	// list of object taken into account by level view
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_FOCUSOBJECT,						
	// called on reset
	CUIK_NOTIFICATION_REWIND,							
	// called on play
	CUIK_NOTIFICATION_PLAY,								
	// called on pause
	CUIK_NOTIFICATION_PAUSE,							
	// called on play step
	CUIK_NOTIFICATION_STEP,								
	// current selection in 3dLayout has changed
	CUIK_NOTIFICATION_SELECTIONCHANGED,					
	// Schematic Preferences have changed
	CUIK_NOTIFICATION_SCHEMATICPREFERENCESCHANGED,		
	// 3DLayout Preferences changed
	CUIK_NOTIFICATION_3DLAYOUTPREFERENCESCHANGED,		
	// Level Preferences changed
	CUIK_NOTIFICATION_LEVELCONTROLPREFERENCESCHANGED,	
	// Resource Preferences changed
	CUIK_NOTIFICATION_RESOURCEPREFERENCESCHANGED,		
	// current level's scene has changed (=active scene has changed)
	CUIK_NOTIFICATION_BEHAVIORAL_SCENE_CHANGED,
	// when a composition / level is loaded
	// param1 : CKObjectArray* list of object loaded
	CUIK_NOTIFICATION_LEVEL_LOADED,
	// Setups needs update
	// param1 : CK_ID* , array of CK_ID, list of object whose setups must be updated
	// param2 : array size
	//If param==param2==0, then update all setups
	CUIK_NOTIFICATION_UPDATESETUP,						
	// a new attribute has been registered in the CKAttributeManager
	// param1 : int, attribute index
	CUIK_NOTIFICATION_ATTRIBUTE_CREATED,				
	// an attribute has been unregistered from the CKAttributeManager
	// param1 : int, attribute index
	CUIK_NOTIFICATION_ATTRIBUTE_DELETED,				
	// an attribute category has been removed from the CKAttributeManager
	// param1 : const char*, category name
	CUIK_NOTIFICATION_ATTRIBUTE_CAT_DELETED,			
	// an attribute category has been added in the CKAttributeManager
	// param1 : const char*,category name
	CUIK_NOTIFICATION_ATTRIBUTE_CAT_CREATED,			
	// an attribute category has been renamed in the CKAttributeManager
	// param1 : int, category index
	CUIK_NOTIFICATION_ATTRIBUTE_CAT_RENAMED,			
	// an attribute has been renamed in the CKAttributeManager
	// param1 : int, attribute index
	CUIK_NOTIFICATION_ATTRIBUTE_RENAMED,				
	// the selection in the 3dLayout has been locked / unlocked (ie with the spacebar key when focus on 3dLayout)
	// param1 : BOOL, lock status
	CUIK_NOTIFICATION_SELECTIONLOCKCHANGED,				
	// show scripts in the schematic view (if they are hidden)
	// param1 : CKObjectArray*, filled with scripts
	CUIK_NOTIFICATION_SHOW_SCRIPT_IN_SCHEMATIC,			
	// hide scripts in the schematic view (if they are shown)
	// param1 : CKObjectArray*, filled with scripts
	CUIK_NOTIFICATION_HIDE_SCRIPT_IN_SCHEMATIC,			
	// *****Private to Virtools Interface*****
	// send message to debugging window
	// which is opened from the Schematic's toolbar left to the trace button
	// param1 : char *, the message to show
	CUIK_NOTIFICATION_DEBUGMESSAGE,						
	// *****Private to Virtools Interface*****
	// shortcut is removed
	// specific notification for schematic view
	// param1 : VPosition* of shortcut/source, specific notification for schematic view
	CUIK_NOTIFICATION_SCHEMATICSHORTCUTREMOVED,
	// *****Private to Virtools Interface*****
	// mark is removed
	// specific notification for schematic view
	// param1 : VPosition* of shortcut/source, specific notification for schematic view
	CUIK_NOTIFICATION_SCHEMATICMARKREMOVED,			
	// *****Private to Virtools Interface*****
	// breakpoint is removed
	// specific notification for schematic view
	// param1 = CUIK_ID of VPosition* of breakpointed object
	CUIK_NOTIFICATION_SCHEMATICBREAKPOINTREMOVED,		
	// sequence of ck objects is going to be deleted
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_PRESEQUENCEDELETED,				
	// sequence of ck objects has been deleted
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_POSTSEQUENCEDELETED,				
	// sequence of objects has been added to scene
	// param1 : CK_ID*
	// param2 : array count
	// parem3 : the scene in which objects have been added
	CUIK_NOTIFICATION_SEQUENCEADDEDTOSCENE,				
	// sequence of objects has been removed from scene
	// param1 : CK_ID*
	// param2 : array count
	// parem3 : the scene in which objects have been added
	CUIK_NOTIFICATION_SEQUENCEREMOVEDFROMSCENE,			
	// initial conditions have been added / changed on several objects
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_SEQUENCE_IC_CHANGED,				
	// several objects re restored in their initial conditions
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_SEQUENCE_IC_RESTORED,			
	// parameters have been modified
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_PARAMETERSEQUENCEMODIFIED,		
	// *****Private to Virtools Interface*****
	// the setup is about to loose its object
	// param1 : CK_ID of object
	CUIK_NOTIFICATION_LOOSINGOBJECT,
	// *****Private to Virtools Interface*****
	// some object specific params have changed (camera [with/without target], frame [dummy/not], ...)
	// param1 : CK_ID
	// param2 : DWORD (hints)
	CUIK_NOTIFICATION_OBJECTPARAMSCHANGED,				
	// current camera has changed (ie from the camera combo box in the 3dLayout)
	CUIK_NOTIFICATION_CAMERACHANGED,					
	// *****Private to Virtools Interface*****
	// a view is being closed
	// param 1 : CUIKView*
	CUIK_NOTIFICATION_VIEWISCLOSING,					
	// hierarchy of 3d/2d objects has changed
	// used by hierarchy manager,level view and some setups (Sprite2DSetup) for refresh
	// no parameter
	CUIK_NOTIFICATION_HIERARCHY_CHANGED,				
	// a new scene has been created
	// param1 : CK_ID of CKScene
	CUIK_NOTIFICATION_SCENE_CREATED,					
	// add list of object to 3dLayout selection
	// param1 : XObjectArray*, list of object to add to 3d Layout selection
	// param2 : BOOL, clear selection if TRUE
	CUIK_NOTIFICATION_SCENE_ADDTOSELECTION,				//XObjectArray *, BOOL pour clear selection (true=>clear asked)	
	// *****Private to Virtools Interface*****
	// when user unload or reload a plugin (such as a manager or a Building Block DLL)
	CUIK_NOTIFICATION_PLUGIN_LOAD_UNLOAD,				
	// *****Private to Virtools Interface*****
	// specific vor Virtools Vsl Script Editor
	// when a breakpoint in VSL script code is reached
	// param1 : a VSLEditor order
	// parem2 : data.
	CUIK_NOTIFICATION_VSLSCRIPTSCOMPILED,		
	// *****Private to Virtools Interface*****
	// specific vor Virtools Vsl Script Editor
	// vsl action scripts have just been compiled.
	// param1 : ScriptInterfaceMain**, array of (ScriptInterfaceMain*)
	// param2 : size of array
	CUIK_NOTIFICATION_VSLBREAKNOTIFY,					
	// *****Private to Virtools Interface*****
	// specific vor Virtools Vsl Script Editor
	// new vsl action scripts have been created.
	// param1 : ScriptInterface**, array of (ScriptInterface*)
	// param2 : size of array
	CUIK_NOTIFICATION_VSLINTERFACEACTIONCREATED,		
	// *****Private to Virtools Interface*****
	// specific vor Virtools Vsl Script Editor
	// vsl action scripts are going to be deleted.
	// param1 : ScriptInterface**, array of (ScriptInterface*)
	// param2 : size of array
	CUIK_NOTIFICATION_VSLINTERFACEACTIONDELETED,		
	// *****Private to Virtools Interface*****
	// specific vor Virtools Vsl Script Editor
	// vsl action scripts have been renamed.
	// param1 : ScriptInterface**, array of (ScriptInterface*)
	// param2 : size of array
	CUIK_NOTIFICATION_VSLINTERFACEACTIONRENAMED,		
	// create local parameters attached to the input parameters of a list of behaviors 
	// in the schematic view
	// the local parameters must already exists and must already have their owner set (should be the parent behavior)
	// param1 : CK_ID*, list of beh IDs
	// param2 : size of array
	CUIK_NOTIFICATION_ATTACHLOCALSINSCHEMATIC,			
	// for Virtools Interface Plugin Editors
	// to send to notify Virtools Dev Interface that the icon of the plugin editor has changed
	// param1 : HWND, plugin editor dlg HWND
	CUIK_NOTIFICATION_UPDATEDLLEDITORCUSTOMICON,		
	// render device has changed
	CUIK_NOTIFICATION_RENDERDEVICECHANGED,				
	// *****Specific for the Shader Editor*****
	// shader fx list changed
	CUIK_NOTIFICATION_SHADERLISTCHANGED,				
	// *****Specific for the Shader Editor*****
	// show shader editor and edit a shader
	// param1: CKShader*
	// param2: BOOL, compile shader
	// user want to see shader fx
	CUIK_NOTIFICATION_WANTTOSEESHADER,					
	// *****Specific for the Shader Editor*****
	// user just compiled a shader
	CUIK_NOTIFICATION_SHADERCOMPILED,					
	// *****Specific for the Shader Editor*****
	// user has deleted a shader
	CUIK_NOTIFICATION_SHADERREMOVED,					
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev)
	// send when the activation state of the source control plugin has changed
	// param1 : BOOL, TRUE  means source control has been enabled, FALSE means disabled
	CUIK_NOTIFICATION_SOURCECONTROLENABLED,
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev))
	// send when files are checked in
	// param1: const char**, array of filename (may be 0)
	// param2: size of array (may be 0)
	CUIK_NOTIFICATION_SOURCECONTROL_CHECKIN,
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev))
	// send when files are checked out
	// param1: const char**, array of filename (may be 0)
	// param2: size of array (may be 0)
	CUIK_NOTIFICATION_SOURCECONTROL_CHECKOUT,
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev))
	// send when files are unchecked out
	// param1: const char**, array of filename (may be 0)
	// param2: size of array (may be 0)
	CUIK_NOTIFICATION_SOURCECONTROL_UNDOCHECKOUT,
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev))
	// send when files are imported in source control
	// param1: const char**, array of filename (may be 0)
	// param2: size of array (may be 0)
	CUIK_NOTIFICATION_SOURCECONTROL_FILESIMPORTED,
	// send by the current Version/Source Control plugin (should be NxN Alienbrain plugin for Virtools Dev))
	// send when files are removed from source control
	// param1: const char**, array of filename (may be 0)
	// param2: size of array (may be 0)
	CUIK_NOTIFICATION_SOURCECONTROL_FILESREMOVED,
	//send when preference "Show Dynamic objects" changes
	//param1: bool, TRUE if show dynamic objects, FALSE if hide
	CUIK_NOTIFICATION_SHOWDYNAMICOBJECTS,
	//send when a parameter type description is registered or unregisterd(ie user flag/enum)
	//param1: CKGUID*
	//param2: BOOL, TRUE for register, FALSE for unregister
	CUIK_NOTIFICATION_REGISTERPARAMETERGUID,
	//send to vsl editor to refresh its type combo box in the iolist
	CUIK_NOTIFICATION_VSLEDITOR_REFRESHTYPECOMBO,
	// focus object in hierarchy manager
	// list of object taken into account by level view
	// param1 : CK_ID*
	// param2 : array count
	CUIK_NOTIFICATION_FOCUSOBJECT_HIERARCHY,
	//refresh vsl manager tree
	CUIK_NOTIFICATION_VSLEDITOR_REFRESHTREE,
	//refresh level manager
	CUIK_NOTIFICATION_LEVELMANAGER_REFRESH,
	//selection set has been modified (name/priority/options/dependencies/path/source control)
	// if Param1 == 0 , all sets must be refreshed
	// param1 : CK_ID*, array of selection set CK_IDs
	// Param2 : HIWORD = FSetPart, LOWORD = array size
	CUIK_NOTIFICATION_SELECTIONSETS_MODIFIED,
	/* useless
	//selection sets have been changed (created/delete)
	// editor should refresh all informations about sets
	// param1 : CK_ID*, list of created selection groups IDs (optional)
	// param2 : size of array (optional)
	CUIK_NOTIFICATION_SELECTIONSETS_UPDATEALL,
	*/
	CUIK_NOTIFICATION_INTERFACEOBJECTMANAGER_LOADED, // param1 = CK_ID of iom, param2 = BOOL dynamic. Send by objectloaded BB for instance

	CUIK_NOTIFICATION_REQUESTCLEARALL,	//clear all / new composition
	CUIK_NOTIFICATION_REQUESTLOADFILE,	//param1 = const char* filename&path, can be set to NULL to open dialog

	// Added for integration of Lua and Python
	CUIK_NOTIFICATION_SCRIPT_ADDED,
	CUIK_NOTIFICATION_SCRIPT_REMOVED,
	CUIK_NOTIFICATION_SCRIPT_PRINT,
	CUIK_NOTIFICATION_SCRIPT_PRINTERROR,
	CUIK_NOTIFICATION_SCRIPT_STOP_DEBUG,
	CUIK_NOTIFICATION_SCRIPT_START_DEBUG,
	CUIK_NOTIFICATION_GOTO_LINE,
	CUIK_NOTIFICATION_ADD_STACK_TRACE,
	CUIK_NOTIFICATION_CLEAR_CALLSTACK,
	CUIK_NOTIFICATION_WATCH_ADD_VARIABLE,
	CUIK_NOTIFICATION_CLEAR_WATCH,
	CUIK_NOTIFICATION_WATCH_ADD_LEVEL,
	CUIK_NOTIFICATION_WATCH_DEL_LEVEL,
	CUIK_NOTIFICATION_STEP_IN,
	CUIK_NOTIFICATION_SCRIPT_FOCUS, // param1 = char* name of the script on which to focus.
	CUIK_NOTIFICATION_LUA_FORCESTOPDEBUG,
	CUIK_NOTIFICATION_LUA_SCRIPT_FORCEREMOVE,
} CUIK_NOTIFICATION;

// Hints for CUIK_NOTIFICATION_OBJECTPARAMSCHANGED

typedef enum
{	
	CUIK_HINT_POSITION,
	CUIK_HINT_ORIENTATION,
	CUIK_HINT_SCALE,
	CUIK_HINT_TARGET,
	CUIK_HINT_PROJECTION,

} CUIK_NOTIFICATION_HINTS;

#endif


