/*************************************************************************/
/*	File : CKBehaviorManager.h						 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKBEHAVIORMANAGER_H

#define CKBEHAVIORMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKBaseManager.h"
#include "CKBeObject.h"
#include "CKContext.h"
#include "XSHashTable.h"


typedef XSHashTable<int,CK_ID>					BeObjectTable;

typedef BeObjectTable::Iterator					BeObjectTableIt;

/************************************************************************
Name: CKBehaviorManager

Summary: Behavior execution management.

Remarks:

+ There is only one instance of the CKBehaviorManager class in a Context. 
+ This instance may be obtained with the CKContext::GetBehaviorManager() method.
+ The behavior manager manages the list of object which behaviors need to be executed at each process loop, and manages their execution. 
+ The behavior manager is automatically called by the process loop and by the scenes, but direct access is
provided for special behavior processing and direct access to the execution of behaviors.
See Also: CKBehavior, CKBeObject, CKScene
*************************************************************************/
class CKBehaviorManager:public CKBaseManager {
friend class CKBehavior;
public :

//----------------------------------------------
// Main Process
CKERROR Execute(float delta);

//----------------------------------------------
// Object Management
CKERROR		AddObject	(CKBeObject *b);
CKERROR		RemoveObject(CKBeObject *b);
int			GetObjectsCount();
CKERROR		RemoveAllObjects();
 CKBeObject * GetObject(int pos);

CKERROR		AddObjectNextFrame		(CKBeObject *b);
CKERROR		RemoveObjectNextFrame	(CKBeObject *b);


//-----------------------------------------------
// Setup
int		GetBehaviorMaxIteration();
void		SetBehaviorMaxIteration(int n);


//-------------------------------------------------------------------------
// Internal functions 
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

	virtual CKStateChunk* SaveData(CKFile* SavedFile);
	virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile);
	virtual CKERROR OnCKPlay();
	virtual CKERROR OnCKPause();
	virtual CKERROR PreProcess();
	virtual CKERROR PreClearAll() {  RemoveAllObjects();  return CK_OK; };
	virtual CKERROR SequenceDeleted(CK_ID *objids,int count);
	virtual CKERROR SequenceToBeDeleted(CK_ID *objids,int count);
	virtual CKDWORD	GetValidFunctionsMask()	{ return CKMANAGER_FUNC_PreClearAll							|
																  CKMANAGER_FUNC_OnCKPause				|
																  CKMANAGER_FUNC_PreProcess				|
																  CKMANAGER_FUNC_OnSequenceDeleted		|
																  CKMANAGER_FUNC_OnSequenceToBeDeleted	|
																  CKMANAGER_FUNC_OnCKPlay; 	}	


#endif
};

#endif

