/*************************************************************************/
/*	File : CKScene.h													 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSCENE_H

#define CKSCENE_H

#include "CKBeObject.h"
#include "CKSceneObjectDesc.h"
#include "XHashTable.h"

typedef XHashTable<CKSceneObjectDesc,CK_ID>		CKSODHash;		
typedef CKSODHash::Iterator						CKSODHashIt;	

/*************************************************
Summary: Iterators on objects in a scene.

Remarks:
+ Objects in a scene are stored in a Hash table, this class can be used with CKScene::GetObjectIterator
to iterate on objects in a scene.

For example if can be use in following way:

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

		CKSceneObjectIterator it = scene->GetObjectIterator();
		CKObject* obj;
		
		while(!it.End()) {
			obj = context->GetObject(it.GetObjectID());
			....
			...
			it++;
		}

{html:</td></tr></table>}

See Also:CKScene::GetObjectIterator
*************************************************/
class CKSceneObjectIterator {
public:

	CKSceneObjectIterator(CKSODHashIt it, CKSODHash& iHash):m_Iterator(it),m_Hash(iHash) {}
//Summary:Returns the ID of the current object.
//Return Value
//	CK_ID of the current object.
	CK_ID				GetObjectID() {return m_Iterator.GetKey();}

	CKSceneObjectDesc*	GetObjectDesc() {return &(*m_Iterator);}
//Summary:Reset iterator to start of the list.
	void				Rewind() 
	{
		m_Iterator = m_Hash.Begin();
	}

	void				RemoveAt() 
	{
		m_Iterator = m_Hash.Remove(m_Iterator);
	}

//Summary:Indicates if end of list is reached.
//Return Value
//	Returns TRUE if the iterator is at the end of the list of objects.
	int					End() {return m_Iterator == m_Hash.End();}

	CKSceneObjectIterator& operator++(int) { 
		++m_Iterator;
		return *this;
	}

	CKSODHashIt m_Iterator;
	CKSODHash&  m_Hash;
};

/*****************************************************************************
Summary: Narrative management. 

Remarks: 
	+ A level in Virtools can counts several scenes or no scene at all. 
	A scene is used to described what objects should be present,
	and how should they act (which behaviors should be active).

	+ An object in the scene can have an initial value stored (called Initial Conditions)

	+ A scene can also stored some render options that can be set when the scene becomes 
	active such as background color,fog options or starting camera. Changing these options at runtime
	does not affect the current rendering, they are only use when the scene becomes active.
	
	+ The class id of CKScene is CKCID_SCENE.

See also:CKLevel::LaunchScene
******************************************************************************/
class CKScene : public CKBeObject {
friend class CKSceneObject;
public :

//-----------------------------------------------------------
// Objects functions
// Adds an Object to this scene with all dependant objects
	void	AddObjectToScene(CKSceneObject *o,CKBOOL dependencies = TRUE);
	void	RemoveObjectFromScene(CKSceneObject *o,CKBOOL dependencies = TRUE);
	CKBOOL	IsObjectHere(CKObject *o);
void BeginAddSequence(CKBOOL Begin);
void BeginRemoveSequence(CKBOOL Begin);

//-----------------------------------------------------------
// Object List
	int							GetObjectCount();
	const XObjectPointerArray&	ComputeObjectList(CK_CLASSID cid, CKBOOL derived=TRUE);

//-----------------------------------------------------------
// Object Settings by index in list 
	CKSceneObjectIterator	GetObjectIterator();

//---- BeObject and Script Activation/deactivation
	void				 Activate(CKSceneObject *o,CKBOOL Reset);
	void				 DeActivate(CKSceneObject *o);

//-----------------------------------------------------------
// Object Settings by object
	void				 SetObjectFlags			(CKSceneObject *o,CK_SCENEOBJECT_FLAGS flags);
	CK_SCENEOBJECT_FLAGS GetObjectFlags			(CKSceneObject *o);
	CK_SCENEOBJECT_FLAGS ModifyObjectFlags		(CKSceneObject *o,CKDWORD Add,CKDWORD Remove);
	CKBOOL				 SetObjectInitialValue	(CKSceneObject *o,CKStateChunk *chunk);
	CKStateChunk*		 GetObjectInitialValue	(CKSceneObject *o);
	CKBOOL				 IsObjectActive			(CKSceneObject *o);

//-----------------------------------------------------------
// Render Settings
 void		ApplyEnvironmentSettings(XObjectPointerArray* renderlist=NULL);
 void		UseEnvironmentSettings(BOOL use=TRUE);
 CKBOOL		EnvironmentSettings();

//-----------------------------------------------------------
// Ambient Light	
 void		SetAmbientLight(CKDWORD Color);
 CKDWORD	GetAmbientLight();

//-----------------------------------------------------------
//	Fog Access
 void	SetFogMode(VXFOG_MODE Mode);
 void	SetFogStart(float Start);
 void	SetFogEnd(float End);
 void	SetFogDensity(float Density);
 void	SetFogColor(CKDWORD Color);

VXFOG_MODE		GetFogMode();
float			GetFogStart();
float			GetFogEnd();
float			GetFogDensity();
CKDWORD			GetFogColor();

//---------------------------------------------------------
// Background 
void		SetBackgroundColor(CKDWORD Color);
CKDWORD		GetBackgroundColor();

void		SetBackgroundTexture(CKTexture* texture);
CKTexture*	GetBackgroundTexture();

//--------------------------------------------------------
// Active camera
void		SetStartingCamera(CKCamera* camera);
CKCamera*	GetStartingCamera();

//-----------------------------------------------------------
// Level functions
	CKLevel*	GetLevel();

//-----------------------------------------------------------
// Merge functions


	CKERROR	 Merge(CKScene *mergedScene,CKLevel *fromLevel=NULL);


//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
	//-----------------------------------------------------------
	// Virtual functions 				  
	CKScene(CKContext *Context,CKSTRING name=NULL);							
	virtual ~CKScene();														
	virtual CK_CLASSID GetClassID();										

	virtual void PreSave(CKFile *file,CKDWORD flags);						
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);					

	virtual void PreDelete();												
	virtual void CheckPostDeletion();										

	virtual int GetMemoryOccupation();										
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);					

	//--------------------------------------------
	// Dependencies Functions 
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);		
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);		

	//--------------------------------------------
	// Class Registering 
	static CKSTRING		GetClassName();										
	static int			GetDependenciesCount(int mode);						
	static CKSTRING		GetDependencies(int i,int mode);					
	static void			Register();											
	static CKScene*		CreateInstance(CKContext *Context);						
	static void			ReleaseInstance(CKContext* iContext,CKScene*);							
	static CK_CLASSID	m_ClassID;											

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKScene* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_SCENE)?(CKScene*)iO:NULL;
	}

		CKERROR		ComputeObjectList(CKObjectArray* array,CK_CLASSID cid, CKBOOL derived=TRUE);	
	void	AddObject	(CKSceneObject *o);	
	void	RemoveObject(CKSceneObject *o);	


#endif // Docjet secret macro
};

#endif

