/*************************************************************************/
/*	File : CKLevel.h													 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKLEVEL_H

#define CKLEVEL_H "$Id:$"

#include "CKBeObject.h"

/*************************************************
{filename:CKLevel}
Summary: Main composition management

Remarks:
+ A composition must contain an unique CKLevel object accessible through CKContext::GetCurrentLevel
+ The level manages a list of CKScene and provides the methods to switch from one scene to another.
+ All objects used in the composition must be referenced in the level in order to be saved.

+ The level also manages a list of CKRenderContext on which the rendering should be done. It uses this list 
to automatically add an object to the rendercontext when it is added to the level.

+ A Level automatically creates a scene it owns. This scene is called the Level Scene and is used to 
store the reference of every objects in the composition. This  scene contains additionnal informations about 
objects such as how do they behave at startup or initial conditions.

+ The class id of CKLevel is CKCID_LEVEL.


See also: CKContext::GetCurrentLevel,CKScene
*************************************************/
class CKLevel : public CKBeObject {
friend class CKScene;
friend class CKContext;
public :

//-----------------------------------------------------
//	Object Management
//  Object are directly inserted or removed from the level Scene
	CKERROR   AddObject(CKObject *obj);
	CKERROR   RemoveObject(CKObject *obj);
	CKERROR   RemoveObject(CK_ID obj);
void BeginAddSequence(CKBOOL Begin);
void BeginRemoveSequence(CKBOOL Begin);
			
//--------------------------------------------------------
// Return the list of object ( owned by this level )
 const XObjectPointerArray& ComputeObjectList(CK_CLASSID cid,CKBOOL derived=TRUE);

//----------------------------------------------------------
// Place Management
	CKERROR  AddPlace(CKPlace *pl);
	CKERROR  RemovePlace(CKPlace *pl);
	CKPlace* RemovePlace(int pos);
	CKPlace* GetPlace(int pos);
	int GetPlaceCount();

//-----------------------------------------------------------
// Scene Management
	CKERROR  AddScene(CKScene *scn);
	CKERROR  RemoveScene(CKScene *scn);
	CKScene* RemoveScene(int pos);
	CKScene* GetScene(int pos);
	int GetSceneCount();

//-----------------------------------------------------------
// Active Scene 
CKERROR SetNextActiveScene(CKScene *,CK_SCENEOBJECTACTIVITY_FLAGS Active = CK_SCENEOBJECTACTIVITY_SCENEDEFAULT,
												CK_SCENEOBJECTRESET_FLAGS Reset = CK_SCENEOBJECTRESET_RESET);

CKERROR LaunchScene(CKScene *,CK_SCENEOBJECTACTIVITY_FLAGS Active = CK_SCENEOBJECTACTIVITY_SCENEDEFAULT,
											CK_SCENEOBJECTRESET_FLAGS Reset = CK_SCENEOBJECTRESET_RESET);
CKScene* GetCurrentScene();

//----------------------------------------------------------
//	Render Context functions		  	
void  AddRenderContext(CKRenderContext *,CKBOOL Main=FALSE);
void  RemoveRenderContext(CKRenderContext *);
int	 GetRenderContextCount();
CKRenderContext *GetRenderContext(int count);

//-----------------------------------------------------------
//	Main Scene for this Level
CKScene* GetLevelScene();


//-----------------------------------------------------------
//	Merge
CKERROR Merge(CKLevel *mergedLevel,CKBOOL asScene);

#ifndef NO_OLDERVERSION_COMPATIBILITY

	virtual void ApplyPatchForOlderVersion(int NbObject,CKFileObject* FileObjects);	
#endif

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
	
	CKLevel(CKContext *Context,CKSTRING name = NULL);						
	virtual	~CKLevel();														
	virtual CK_CLASSID GetClassID();										

	virtual void PreSave(CKFile *file,CKDWORD flags);						
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);					

	virtual void CheckPreDeletion();										
	virtual void CheckPostDeletion();										

	virtual int GetMemoryOccupation();										
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);					

	//--------------------------------------------
	// Dependencies Functions
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);

	//--------------------------------------------
	// Class Registering
	static CKSTRING		GetClassName();										
	static int			GetDependenciesCount(int mode);						
	static CKSTRING		GetDependencies(int i,int mode);					
	static void			Register();											
	static CKLevel*		CreateInstance(CKContext *Context);						
	static void			ReleaseInstance(CKContext* iContext,CKLevel*);							
	static CK_CLASSID	m_ClassID;											

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKLevel* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_LEVEL)?(CKLevel*)iO:NULL;
	}

	virtual void AddToScene		(CKScene *scene,CKBOOL dependencies = TRUE);	
	virtual void RemoveFromScene(CKScene *scene,CKBOOL dependencies = TRUE);	

	void Reset();						
	void ActivateAllScript();			
	void CheckForNextScene();			
void CreateLevelScene();	
void PreProcess();			

protected :
	XObjectPointerArray	m_SceneList;
	XObjectPointerArray	m_RenderContextList;
	CK_ID				m_CurrentScene;
	CK_ID				m_DefaultScene;
	CK_ID				m_NextActiveScene;
	CK_SCENEOBJECTACTIVITY_FLAGS	m_NextSceneActivityFlags;
	CK_SCENEOBJECTRESET_FLAGS		m_NextSceneResetFlags;
	CKBOOL				m_IsReseted;

#endif // Docjet secret macro
};

#endif

