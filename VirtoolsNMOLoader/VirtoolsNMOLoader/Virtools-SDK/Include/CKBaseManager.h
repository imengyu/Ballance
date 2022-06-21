/**************************************************************************/
/*	File : CKBaseManager.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Base Class for Virtools Managers									 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKBASEMANAGER_H

#define CKBASEMANAGER_H "$Id:$"

#include "CKDefines.h"

class CKFile;
class CKStateChunk;


#define MAX_MANAGERFUNC_PRIORITY		30000

#define DEFAULT_MANAGERFUNC_PRIORITY	0

/*************************************************
Summary: Mask for Base Manager overridable functions.

Remarks:
	+ When implementing a manager the CKBaseManager::GetValidFunctionsMask 
	must be overriden to return a combination of these flag to indicate
	which methods are implemented.
See also: CKBaseManager,CKBaseManager::GetValidFunctionsMask
*************************************************/
typedef enum CKMANAGER_FUNCTIONS {
				CKMANAGER_FUNC_OnSequenceToBeDeleted		= 0x00000001,	// CKBaseManager::OnSequenceToBeDeleted
				CKMANAGER_FUNC_OnSequenceDeleted			= 0x00000002,	// CKBaseManager::OnSequenceDeleted
				CKMANAGER_FUNC_PreProcess					= 0x00000004,	// CKBaseManager::PreProcess
				CKMANAGER_FUNC_PostProcess					= 0x00000008,	// CKBaseManager::PostProcess
				CKMANAGER_FUNC_PreClearAll					= 0x00000010,	// CKBaseManager::PreClearAll
				CKMANAGER_FUNC_PostClearAll					= 0x00000020,	// CKBaseManager::PostClearAll
				CKMANAGER_FUNC_OnCKInit						= 0x00000040,	// CKBaseManager::OnCKInit
				CKMANAGER_FUNC_OnCKEnd						= 0x00000080,	// CKBaseManager::OnCKEnd
				CKMANAGER_FUNC_OnCKPlay						= 0x00000100,	// CKBaseManager::OnCKPlay
				CKMANAGER_FUNC_OnCKPause					= 0x00000200,	// CKBaseManager::OnCKPause
				CKMANAGER_FUNC_PreLoad						= 0x00000400,	// CKBaseManager::PreLoad
				CKMANAGER_FUNC_PreSave						= 0x00000800,	// CKBaseManager::PreSave
				CKMANAGER_FUNC_PreLaunchScene				= 0x00001000,	// CKBaseManager::PreLaunchScene
				CKMANAGER_FUNC_PostLaunchScene				= 0x00002000,	// CKBaseManager::PostLaunchScene
				CKMANAGER_FUNC_OnCKReset					= 0x00004000,	// CKBaseManager::OnCKReset
				CKMANAGER_FUNC_PostLoad						= 0x00008000,	// CKBaseManager::PostLoad
				CKMANAGER_FUNC_PostSave						= 0x00010000,	// CKBaseManager::PostSave
				CKMANAGER_FUNC_OnCKPostReset				= 0x00020000,	// CKBaseManager::OnCKPostReset
				CKMANAGER_FUNC_OnSequenceAddedToScene		= 0x00040000,	// CKBaseManager::OnSequenceAddedToScene
				CKMANAGER_FUNC_OnSequenceRemovedFromScene	= 0x00080000,	// CKBaseManager::OnSequenceRemovedFromScene
				CKMANAGER_FUNC_OnPreCopy					= 0x00100000,	// CKBaseManager::OnPreCopy
				CKMANAGER_FUNC_OnPostCopy					= 0x00200000,	// CKBaseManager::OnPostCopy
				CKMANAGER_FUNC_OnPreRender					= 0x00400000,	// CKBaseManager::OnPreRender
				CKMANAGER_FUNC_OnPostRender					= 0x00800000,	// CKBaseManager::OnPostRender
				CKMANAGER_FUNC_OnPostSpriteRender			= 0x01000000,	// CKBaseManager::OnPostSpriteRender
				CKMANAGER_FUNC_OnPreBackToFront				= 0x02000000,	// CKBaseManager::OnPreBackToFront
				CKMANAGER_FUNC_OnPostBackToFront			= 0x04000000,	// CKBaseManager::OnPostBackToFront
				CKMANAGER_FUNC_OnPreFullScreen				= 0x08000000,	// CKBaseManager::OnPreFullScreen
				CKMANAGER_FUNC_OnPostFullScreen				= 0x10000000,	// CKBaseManager::OnPostFullScreen
				CKMANAGER_FUNC_OnRasterizerEvent			= 0x20000000,	// CKBaseManager::OnRasterizerEvent
				CKMANAGER_FUNC_OnPreSpriteRender			= 0x40000000	// CKBaseManager::OnPreSpriteRender		
} CKMANAGER_FUNCTIONS;


typedef enum CKMANAGER_FUNCTIONS_INDEX {
				CKMANAGER_INDEX_OnSequenceToBeDeleted			= 0,
				CKMANAGER_INDEX_OnSequenceDeleted				= 1,
				CKMANAGER_INDEX_PreProcess						= 2,
				CKMANAGER_INDEX_PostProcess						= 3,
				CKMANAGER_INDEX_PreClearAll						= 4,
				CKMANAGER_INDEX_PostClearAll					= 5,
				CKMANAGER_INDEX_OnCKInit						= 6,
				CKMANAGER_INDEX_OnCKEnd							= 7,
				CKMANAGER_INDEX_OnCKPlay						= 8,
				CKMANAGER_INDEX_OnCKPause						= 9,
				CKMANAGER_INDEX_PreLoad							= 10,
				CKMANAGER_INDEX_PreSave							= 11,
				CKMANAGER_INDEX_PreLaunchScene					= 12,
				CKMANAGER_INDEX_PostLaunchScene					= 13,
				CKMANAGER_INDEX_OnCKReset						= 14,
				CKMANAGER_INDEX_PostLoad						= 15,
				CKMANAGER_INDEX_PostSave						= 16,
				CKMANAGER_INDEX_OnCKPostReset					= 17,
				CKMANAGER_INDEX_OnSequenceAddedToScene			= 18,
				CKMANAGER_INDEX_OnSequenceRemovedFromScene		= 19,
				CKMANAGER_INDEX_OnPreCopy						= 20,
				CKMANAGER_INDEX_OnPostCopy						= 21,
				CKMANAGER_INDEX_OnPreRender						= 22,
				CKMANAGER_INDEX_OnPostRender					= 23,
				CKMANAGER_INDEX_OnPostSpriteRender				= 24,
				CKMANAGER_INDEX_OnPreBackToFront				= 25,
				CKMANAGER_INDEX_OnPostBackToFront				= 26,
				CKMANAGER_INDEX_OnPreFullScreen					= 27,
				CKMANAGER_INDEX_OnPostFullScreen				= 28,
				CKMANAGER_INDEX_OnRasterizerEvent				= 29,
				CKMANAGER_INDEX_OnPreSpriteRender				= 30,				
} CKMANAGER_FUNCTIONS_INDEX;


/*************************************************************************
Summary: Base Class for managers.

Remarks: 
+ This class provides virtual methods that can be override by any managers. Any manager that inherits from CKBaseManager can override function to do some processing.

+ The instances of managers may be retrieved through the global function CKContext::GetManagerByGuid()

+ Some default managers implemented in Virtools  can be accessed directly : See Managers Access



See also: CKContext::RegisterNewManager,Implementing a external Manager
*************************************************************************/
class CKBaseManager {
public:
	
	CKBaseManager(CKContext *Context,CKGUID guid,CKSTRING Name);
	
	virtual ~CKBaseManager();

/*************************************************
Summary: Acces to Manager GUID

Return Value:
	CKGUID of this manager.
Remarks:
	+ Each Manager is given an unique GUID. When creating a new manager it should
	assign itself a GUID and name before registering itsef.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

		CKAttributeManager::CKAttributeManager(CKContext *Context):CKBaseManager(Context,ATTRIBUTE_MANAGER_GUID,"Attribute Manager")
		{
			....
			....
			Context->RegisterNewManager(this);
		}

{html:</td></tr></table>}

See Also: CKContext::RegisterNewManager,GetName
*************************************************/	
	CKGUID		GetGuid()			{ return m_ManagerGuid; }
/*************************************************
Summary: Acces to Manager name

Return Value:
	Name of this manager.
Remarks:
	+ Each Manager can be given a name. When creating a new manager it should
	assign itself a GUID and name before registering itsef.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}	

		CKAttributeManager::CKAttributeManager(CKContext *Context):CKBaseManager(Context,ATTRIBUTE_MANAGER_GUID,"Attribute Manager")
		{
			....
			....
			Context->RegisterNewManager(this);
		}

{html:</td></tr></table>}

See Also: CKContext::RegisterNewManager,GetGuid
*************************************************/	
	CKSTRING	GetName()			{ return m_ManagerName; }

/*************************************************
Summary: Called to save manager data.

Arguments:
	SavedFile: A pointer to the CKFile being saved.
Return Value:
	This function should return a valid CKStateChunk that contain data to save or NULL if there is nothing to save.
Remarks:
+ During a save operation, each manager is given the opportunity to save its data in the file.
+ The file being saved is given for information only and must not be modified. It can be used to decide whether it is worth saving 
data for your manager.
See Also: CKStateChunk,LoadData
*************************************************/	
	virtual CKStateChunk* SaveData(CKFile* SavedFile) { return NULL; }

/*************************************************
Summary: Called to load manager data.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	chunk:	A pointer to a CKStateChunk that was saved in the file.
	LoadedFile: A pointer to the CKFile being loaded.
Remarks:
	+ During a load operation, each manager is automatically called if there was a chunk saved in the file with SaveData.
See Also: CKStateChunk,SaveData
*************************************************/	
	virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile) { return CK_OK; }

/*************************************************
Summary: Called at the beginning of a CKContext::ClearAll operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the beginning of a CKContext::ClearAll operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PreClearAll for this function to get called.

See Also:Main Virtools Events,PostClearAll,CKContext::ClearAll 
*************************************************/	
	virtual CKERROR PreClearAll()	{ return CK_OK; }
/*************************************************
Summary: Called at the end of a CKContext::ClearAll operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the end of a CKContext::ClearAll operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PostClearAll for this function to get called.

See Also:Main Virtools Events,PreClearAll,CKContext::ClearAll, 
*************************************************/	
	virtual CKERROR PostClearAll()  { return CK_OK; }

/*************************************************
Summary: Called at the beginning of each process loop.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the beginning  of a CKContext::Process operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PreProcess for this function to get called.
See Also:Main Virtools Events,PostProcess,CKContext::Process, 
*************************************************/	
	virtual CKERROR PreProcess()	{ return CK_OK; }
/*************************************************
Summary: Called at the end of each process loop.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the end  of a CKContext::Process operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PostProcess for this function to get called.

See Also:Main Virtools Events,PreProcess,CKContext::Process, 
*************************************************/	
	virtual CKERROR PostProcess()	{ return CK_OK; }


/*************************************************
Summary: Called when objects are added to a scene

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	scn: A pointer to the CKScene to which objects where added.
	objids: A pointer to a list of CK_ID of the objects being added to the scene.
	count: number of objects in objids list.
Remarks:
+ You can override these functions to add specific processing when adding or removing objects from scene
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnSequenceAddedToScene for this function to get called.

See Also:Main Virtools Events,SequenceRemovedFromScene
*************************************************/		
	virtual CKERROR SequenceAddedToScene(CKScene *scn,CK_ID *objids,int count)	{ return CK_OK; }

/*************************************************
Summary: Called when objects are removed from a scene

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	scn: A pointer to the CKScene to which objects where added.
	objids: A pointer to a list of CK_ID of the objects being added to the scene.
	count: number of objects in objids list.
Remarks:
+ You can override these functions to add specific processing when adding or removing objects from scene
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnSequenceRemovedFromScene for this function to get called.

See Also:Main Virtools Events,SequenceAddedToScene, 
*************************************************/		
	virtual CKERROR SequenceRemovedFromScene(CKScene *scn,CK_ID *objids,int count){ return CK_OK; }

/*************************************************
Summary: Called before a scene becomes active.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	OldScene: Previous active scene.
	NewScene: Scene to become active.
Remarks:
+ You can override these functions to add specific processing at the beginning and at the end of a LaunchScene operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PreLaunchScene for this function to get called.

See Also:Main Virtools Events,PostLaunchScene, 
*************************************************/	
	virtual CKERROR PreLaunchScene(CKScene* OldScene,CKScene* NewScene)	{ return CK_OK; }

/*************************************************
Summary: Called after a scene became active.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	OldScene: Previous active scene.
	NewScene: Scene that have been activated.
Remarks:
+ You can override these functions to add specific processing at the beginning and at the end of a LaunchScene operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PostLaunchScene for this function to get called.

See Also:Main Virtools Events,PreLaunchScene
*************************************************/	
	virtual CKERROR PostLaunchScene(CKScene* OldScene,CKScene* NewScene)	{ return CK_OK; }

/*************************************************
Summary: Called at the end of the creation of a CKContext.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function if you need to add specific processing at initialization.
+ If your manager is registered after the context has been created and if it implements the OnCKInit()
function then, this function will be called upon registration.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKInit for this function to get called.

See Also:Main Virtools Events,OnCKEnd,CKCreateContext 
*************************************************/	
	virtual CKERROR OnCKInit()			{ return CK_OK; }

/*************************************************
Summary: Called at deletion of a CKContext

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function if you need to clean up things at the end of the session.
+ This method is called at the beginning of the deletion of the CKContext, the manager 
is also deleted a short time after.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKEnd for this function to get called.

See Also:Main Virtools Events,OnCKInit,CKCloseContext
*************************************************/	
	virtual CKERROR OnCKEnd()			{ return CK_OK; }

/*************************************************
Summary: Called before the composition is reset.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKReset for this function to get called.

See Also:Main Virtools Events,CKContext::Reset,OnCKPostReset,OnCKPause,OnCKPlay.
*************************************************/	
	virtual CKERROR OnCKReset()			{ return CK_OK; }
/*************************************************
Summary: Called after the composition has been restarted.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKPostReset for this function to get called.
See Also:Main Virtools Events ,CKContext::Reset,OnCKReset,OnCKPause,OnCKPlay.
*************************************************/	
	virtual CKERROR OnCKPostReset()			{ return CK_OK; }
/*************************************************
Summary: Called when the process loop is paused.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKPause for this function to get called.
See Also:Main Virtools Events,CKContext::Reset,OnCKReset,OnCKPostReset,OnCKPlay.
*************************************************/	
	virtual CKERROR OnCKPause()			{ return CK_OK; }
/*************************************************
Summary: Called when the process loop is started .

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnCKPlay for this function to get called.
See Also:Main Virtools Events,CKContext::Reset,OnCKReset,OnCKPostReset,OnCKPause.
*************************************************/	
	virtual CKERROR OnCKPlay()			{ return CK_OK; }

/*************************************************
Summary: Called just before objects are deleted.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	objids: A pointer to a list of CK_ID of the objects being deleted.
	count: number of objects in objids list.
Remarks:
+ You can override this function if you need to add specific processing before objects are deleted.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnSequenceToBeDeleted for this function to get called.

See Also:Main Virtools Events,CKContext::DestroyObjects,SequenceDeleted
*************************************************/	
	virtual CKERROR SequenceToBeDeleted(CK_ID *objids,int count)	{ return CK_OK; }

/*************************************************
Summary: Called after objects have been deleted.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	objids: A pointer to a list of CK_ID of the objects being deleted.
	count: number of objects in objids list.
Remarks:
+ You can override this function if you need to add specific processing after objects have been deleted.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnSequenceDeleted for this function to get called.

See Also:Main Virtools Events,CKContext::DestroyObjects,SequenceToBeDeleted
*************************************************/	
	virtual CKERROR SequenceDeleted(CK_ID *objids,int count)		{ return CK_OK; }

/*************************************************
Summary: Called at the beginning of a load operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the beginning of a load operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PreLoad for this function to get called.

See Also:Main Virtools Events,PreSave,PostSave,PostLoad,CKContext::Load
*************************************************/	
	virtual CKERROR PreLoad()	{ return CK_OK; }

/*************************************************
Summary: Called at the end of a load operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the end of a load operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PostLoad for this function to get called.

See Also:Main Virtools Events,PreSave,PostSave,PreLoad,CKContext::Load
*************************************************/	
	virtual CKERROR PostLoad()	{ return CK_OK; }

/*************************************************
Summary: Called at the beginning of a save operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the beginning of a save operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PreSave for this function to get called.

See Also:Main Virtools Events,PreSave,PostSave,PreLoad,CKContext::Load
*************************************************/	
	virtual CKERROR PreSave()	{ return CK_OK; }

/*************************************************
Summary: Called at the end of a save operation.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing at the end of a save operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_PostSave for this function to get called.

See Also:Main Virtools Events,PreSave,PostSave,PreLoad,CKContext::Load
*************************************************/	
	virtual CKERROR PostSave()	{ return CK_OK; }

/*************************************************
Summary: Called at the beginning of a copy.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	context: A CKDependenciesContext containing the objects being copied.
Remarks:
+ You can override this function to add specific processing at the beginning of a copy operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPreCopy for this function to get called.

See Also:Main Virtools Events,OnPostCopy,CKContext::CopyObjects
*************************************************/	
	virtual CKERROR OnPreCopy(CKDependenciesContext& context) { return CK_OK; }

/*************************************************
Summary: Called at the end of a copy.

Return Value:
	CK_OK if successful or an error code otherwise.
Arguments:
	context: A CKDependenciesContext containing the objects being copied.
Remarks:
+ You can override this function to add specific processing at the beginning of a copy operation.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostCopy for this function to get called.

See Also:Main Virtools Events,OnPreCopy,CKContext::CopyObjects
*************************************************/	
	virtual CKERROR OnPostCopy(CKDependenciesContext& context) { return CK_OK; }

/*************************************************
Summary: Called before the rendering of the 3D objects.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing before the rendering of the scene occured.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPreRender for this function to get called.
See Also:OnPostRender,OnPostSpriteRender,CKRenderContext::Render,Understanding the Render Loop
*************************************************/	
	virtual CKERROR OnPreRender(CKRenderContext* dev) { return CK_OK; }

/*************************************************
Summary: Called after the rendering of the 3D objects .

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing after the rendering of the 3D scene occured.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostRender for this function to get called.
See Also:OnPreRender,OnPostSpriteRender,CKRenderContext::Render,Understanding the Render Loop
*************************************************/	
	virtual CKERROR OnPostRender(CKRenderContext* dev) { return CK_OK; }

/*************************************************
Summary: Called Before the rendering of background 2D entities.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing before the rendering of the background 2D entities occured.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostSpriteRender for this function to get called.
See Also:OnPostRender,OnPreRender,CKRenderContext::Render,Understanding the Render Loop
*************************************************/	
	virtual CKERROR OnPreSpriteRender(CKRenderContext* dev) { return CK_OK; }

/*************************************************
Summary: Called after the rendering of foreground 2D entities.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing after the rendering of the foreground 2D entities occured.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostSpriteRender for this function to get called.
See Also:OnPostRender,OnPreRender,CKRenderContext::Render,Understanding the Render Loop
*************************************************/	
	virtual CKERROR OnPostSpriteRender(CKRenderContext* dev) { return CK_OK; }

/*************************************************
Summary: Returns the priority of the specified manager function.

Arguments:
	Function: A CKMANAGER_FUNCTIONS to get the priority of.
Return Value:  A integer giving priority for the specified function.
Remarks:
	+ Override this function if you want to specify a priority for one or several functions of your manager.
	+ The default implementation returns a priority of 0 for all functions.

Example:
			// To ensure that your PostProcess() function will be one of the first to be called amongst all managers:
			
			int MyManager::GetFunctionPriority(CKMANAGER_FUNCTIONS Function)
			{
				if (Function==CKMANAGER_FUNC_PostProcess) return 10000;	// High Priority
				return 0;
			}
See Also:  CKMANAGER_FUNCTIONS
*************************************************/	
	virtual int		GetFunctionPriority(CKMANAGER_FUNCTIONS Function)	{ return 0; }	

/*************************************************
Summary: Returns list of functions implemented by the manager.

Return Value: A combination of CKMANAGER_FUNCTIONS.
Remarks.
+ You must override this function to indicate which functions your manager implements.
Example:
			// The attribute manager implements four CKBaseManager functions :PreClearAll,PostLoad,OnSequenceAddedToscene
			// and OnSequenceRemovedFromScene so its GetValidFunctionsMask looks like this:
			
			virtual CKDWORD	GetValidFunctionsMask()	{ return CKMANAGER_FUNC_PreClearAll				|
														CKMANAGER_FUNC_PostLoad				|
														CKMANAGER_FUNC_OnSequenceAddedToScene  |
														CKMANAGER_FUNC_OnSequenceRemovedFromScene;	 }	

See Also:  CKMANAGER_FUNCTIONS
*************************************************/	
	virtual CKDWORD	GetValidFunctionsMask()								{ return 0; }	


/*************************************************
Summary: Called before the backbuffer is presented.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing before the backbuffer is presented.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPreBackToFront for this function to get called.
See Also:OnPostBackToFront,CKRenderContext::Render,Understanding the Render Loop,GetValidFunctionsMask
*************************************************/	
	virtual CKERROR	 OnPreBackToFront(CKRenderContext* dev)		{ return CK_OK; }
/*************************************************
Summary: Called after the backbuffer is presented.

Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing after the backbuffer is presented.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostBackToFront for this function to get called.
See Also:OnPreBackToFront,CKRenderContext::Render,Understanding the Render Loop,GetValidFunctionsMask
*************************************************/	
	virtual CKERROR	 OnPostBackToFront(CKRenderContext* dev)	{ return CK_OK; }
/*************************************************
Summary: Called before switching to/from fullscreen.

Arguments:
	Going2Fullscreen: TRUE if we are going to fullscreen , FALSE if returning to windowed mode.
	dev: A Pointer to the CKRenderContext that is currently changing mode.
Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing before a context switch to/from fullscreen.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPreFullScreen for this function to get called.

See Also:OnPreBackToFront,CKRenderContext::Render,Understanding the Render Loop,GetValidFunctionsMask
*************************************************/	
	virtual CKERROR	 OnPreFullScreen(BOOL Going2Fullscreen, CKRenderContext* dev)		{ return CK_OK; }

/*************************************************
Summary: Called after switching to/from fullscreen.

Arguments:
	Going2Fullscreen: TRUE if we are going to fullscreen , FALSE if returning to windowed mode.
	dev: A Pointer to the CKRenderContext that has just changed of video mode.
Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing after a context switch to/from fullscreen.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnPostFullScreen for this function to get called.

See Also:OnPreBackToFront,CKRenderContext::Render,Understanding the Render Loop,GetValidFunctionsMask
*************************************************/	
	virtual CKERROR	 OnPostFullScreen(BOOL Going2Fullscreen,CKRenderContext* dev)		{ return CK_OK; }

/*************************************************
Summary: Called for specific rasterizer events.

Arguments:
	Event: Event that was sent by the rasterizer.
	dev: A Pointer to the CKRenderContext that has just changed of video mode.
Return Value:
	CK_OK if successful or an error code otherwise.
Remarks:
+ You can override this function to add specific processing when a render context
is created/destroyed or resized.
+ According to implementation  (DirectX , OpenGL,etc.) some events can occur
such as LostDevice with DirectX that you might need to handle when directly using 
DirectX objects in a manager.
+ You must override GetValidFunctionsMask and return a value including 
CKMANAGER_FUNC_OnRasterizerEvent for this function to get called.

See Also:CKRenderContext::Render,Understanding the Render Loop,GetValidFunctionsMask
*************************************************/	
	virtual CKERROR	 OnRasterizerEvent(CKRST_EVENTS Event,CKRenderContext* dev)		{ return CK_OK; }



	CKERROR	CKDestroyObject(CKObject *obj,DWORD Flags=0,CKDependencies* depoptions=NULL);				
	CKERROR	CKDestroyObject(CK_ID id,DWORD Flags=0,CKDependencies* depoptions=NULL);					
	CKERROR	CKDestroyObjects(CK_ID* obj_ids,int Count,DWORD Flags=0,CKDependencies* depoptions=NULL);	
	CKObject *CKGetObject(CK_ID id);																	

/*******************************************
Summary: Starts profiling. 
Remarks:

+ The time taken by the manager between the 
StartProfile / StopProfile sequence will be added 
to m_ProcessingTime member of CKBaseManager and 
will appear in the Profiler in the Virtools Interface.

+ The time taken by a manager on the PreProcess,PostProcess,PreRender,PostRender and PostSpriteRender
method is automatically computed so you should use these methods if there are other parts
of your manager you would like to be profiled.

See Also:StopProfile
**********************************************/
	void StartProfile();

/*******************************************
Summary: Stops profiling. 
Remarks:
+ The time taken by the manager between the 
StartProfile / StopProfile sequence will be added 
to m_ProcessingTime member of CKBaseManager and 
will appear in the Profiler in the Virtools Interface.
+ The time taken by a manager on the PreProcess,PostProcess,PreRender,PostRender and PostSpriteRender
method is automatically computed so you should use these methods if there are other parts
of your manager you would like to be profiled.
See Also:StopProfile
**********************************************/
	void StopProfile();

public:
	CKGUID			m_ManagerGuid;		// Manager GUID
	CKSTRING		m_ManagerName;		// Manager Name
	CKContext*		m_Context;			// A pointer to the CKContext on which this manager is valid.
	float			m_ProcessingTime;	// Time elapsed during profiling. (Reset each frame before behavioral processing starts)
	VxRDTSCProfiler m_Timer;			

};

#endif