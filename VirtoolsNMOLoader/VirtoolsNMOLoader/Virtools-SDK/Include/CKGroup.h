/*************************************************************************/
/*	File : CKGroup.h						 		 					 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKGROUP_H

#define CKGROUP_H "$Id:$"


#include "CKBeObject.h"

/**************************************************************************
{filename:CKGroup}
Summary: Management of group of objects

Remarks: 
	+ Behavioral Objects can be grouped together because they share common attributes
	or for better understanding of a level. A group is simply a list of objects that
	can be removed or added.

	+ The class id of CKGroup is CKCID_GROUP.
See also: CKBeObject
**************************************************************************/
class CKGroup:public CKBeObject {
friend class CKBeObject;
public :
//---------------------------------------
// Insertion Removal
		CKERROR  AddObject(CKBeObject *o);
		CKERROR  AddObjectFront(CKBeObject *o);
		CKERROR  InsertObjectAt(CKBeObject *o,int pos);

		CKBeObject* RemoveObject(int pos);
		void		RemoveObject(CKBeObject *obj);
		void		Clear();

//---------------------------------------
// Order
		void MoveObjectUp(CKBeObject *o);
		void MoveObjectDown(CKBeObject *o);

//---------------------------------------
// Object Access
		CKBeObject *GetObject(int pos);
		int		 GetObjectCount();

		CK_CLASSID GetCommonClassID();

//-------------------------------------------------------------------
//-------------------------------------------------------------------
// Internal functions 
//-------------------------------------------------------------------
//-------------------------------------------------------------------

//--------------------------------------------------------
////               Private Part                     
#ifdef DOCJETDUMMY // Docjet secret macro
#else

	virtual	int CanBeHide();	

	//----------------------------------------------------------
	// Object Visibility 
	void	Show(CK_OBJECT_SHOWOPTION Show=CKSHOW);							

	//-------------------------------------------------
	// Internal functions	{secret}			  
	CKGroup(CKContext *Context,CKSTRING Name=NULL);							
	virtual	~CKGroup();														
	virtual CK_CLASSID GetClassID();										

	virtual void AddToScene(CKScene *scene,CKBOOL dependencies);			
	virtual void RemoveFromScene(CKScene *scene,CKBOOL dependencies);		

	virtual void PreSave(CKFile *file,CKDWORD flags);						
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);					
	virtual void PostLoad();												

	virtual void PreDelete();												
	virtual void CheckPreDeletion();										

	virtual int GetMemoryOccupation();										
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);					

	//--------------------------------------------
	// Dependencies Functions {secret}
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);		
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);		

	//--------------------------------------------
	// Class Registering {secret}
	static CKSTRING		GetClassName();										
	static int			GetDependenciesCount(int mode);						
	static CKSTRING		GetDependencies(int i,int mode);					
	static void			Register();											
	static CKGroup*		CreateInstance(CKContext *Context);					
	static void			ReleaseInstance(CKContext* iContext,CKGroup*);							
	static CK_ID		m_ClassID;											

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKGroup* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_GROUP)?(CKGroup*)iO:NULL;
	}

	void ComputeClassID();													

protected :
	
	XObjectPointerArray	m_ObjectArray;
	
	CK_CLASSID			m_CommonClassId;
	
	CKBOOL				m_ClassIdUpated;
	
	DWORD				m_GroupIndex;

#endif // Docjet secret macro
};

#endif

