/*************************************************************************/
/*	File : CKSceneObject.h												 */
/*	Author :  Aymeric BARD												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSCENEOBJECT_H

#define CKSCENEOBJECT_H "$Id:$"

#include "CKObject.h"
#include "XBitArray.h"

/*************************************************
{filename:CKSceneObject}
Summary: Base class for objects which can be referenced in a scene

Remarks:
	+ CKSceneObject is the base class for all objects that can be referenced in a scene.

	+ It defines methods to see if an object is present and/or active in a specific scene
	and methods to know in which scenes the object is referenced.

	+ The class id of CKSceneObject is CKCID_SCENEOBJECT.



See also: CKBeObject, CKScene
*************************************************/
class CKSceneObject : public CKObject {

public :
//----------------------------------------------------------
// Scene Activity
	CKBOOL  IsActiveInScene(CKScene *);
	CKBOOL  IsActiveInCurrentScene();
 
//----------------------------------------------------------
// Scene Presence
	CKBOOL	IsInScene(CKScene *scene);
	int		GetSceneInCount();
	CKScene* GetSceneIn(int index);

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

	CKSceneObject() {}													
	CKSceneObject(CKContext *Context,CKSTRING name=NULL);				
	virtual	~CKSceneObject();											
	virtual CK_CLASSID GetClassID();									

	virtual int		GetMemoryOccupation();								

	//--------------------------------------------
	// Class Registering 
	static CKSTRING GetClassName();								
	static int		GetDependenciesCount(int mode);				
	static CKSTRING GetDependencies(int i,int mode);			
	static void		Register();									
	static CKSceneObject* CreateInstance(CKContext *Context);	
	static void		 ReleaseInstance(CKContext* iContext,CKSceneObject*);							
	static	CK_ID	m_ClassID;									

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKSceneObject* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_SCENEOBJECT)?(CKSceneObject*)iO:NULL;
	}

	//----------------------------------------------
	// Do Not use this functions to add an object to a scene
	// Use CKScene::AddObjectToScene Method instead 
	virtual void AddToScene		(CKScene *scene,CKBOOL dependencies = TRUE);
	virtual void RemoveFromScene(CKScene *scene,CKBOOL dependencies = TRUE);
	void		AddSceneIn(CKScene *scene);										
	void		RemoveSceneIn(CKScene *scene);										
	void		RemoveFromAllScenes();										

protected:
	XBitArray	m_Scenes;

#endif // docjet secret macro
};


#endif

