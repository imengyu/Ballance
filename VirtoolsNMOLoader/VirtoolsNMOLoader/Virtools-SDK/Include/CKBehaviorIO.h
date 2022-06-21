/*************************************************************************/
/*	File : CKBehaviorIO.h			 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKBEHAVIORIO_H

#define CKBEHAVIORIO_H "$Id:$"

#include "CKObject.h"

/**************************************************************************
Summary: Behaviors Input and Outputs

Remarks:
{Image:BehaviorIO}
	
+ This class is rarely used since all the methods needed to check or trigger inputs/outputs
activity are available in CKBehaviorClass

+ A CKBehaviorIO is created by CKBehavior::CreateInput,CKBehavior::CreateOutput methods.

+ The class id of CKBehaviorIO is CKCID_BEHAVIORIO.


See also: CKBehavior
***************************************************************************/
class CKBehaviorIO : public CKObject {
friend class CKBehavior;
friend class CKBehaviorLink;
public :

/*************************************************
Summary: Specifies if the behavior IO is an input or output.
Arguments:
	Type: CK_BEHAVIORIO_IN if this Io is an input or CK_BEHAVIORIO_OUT if it is an output.
See also: GetType
*************************************************/
void SetType(int Type) { 	m_ObjectFlags &= ~CK_OBJECT_IOTYPEMASK; m_ObjectFlags |= Type; }
/*************************************************
Summary: Returns whether the behavior IO is an input or output.
Return Value:
	Returns CK_BEHAVIORIO_IN if this Io is an input.
	Returns CK_BEHAVIORIO_OUT if it is an output.
See also: GetType
*************************************************/
int  GetType()		   { 	return m_ObjectFlags & CK_OBJECT_IOTYPEMASK; }

/*************************************************
Summary: Activates or deactivates the behavior IO.
Arguments:
	Active: TRUE to activate the behavior IO.
See also:IsActive
*************************************************/
void Activate(CKBOOL Active=TRUE) { if (Active)	m_ObjectFlags |= CK_BEHAVIORIO_ACTIVE; else m_ObjectFlags &= ~CK_BEHAVIORIO_ACTIVE; }
/*************************************************
Summary: Returns whether the behavior IO is active.
Return Value:
	TRUE if IO is curerntly active, FALSE otherwise
See also: Activate
*************************************************/
CKBOOL IsActive() { 	return (m_ObjectFlags & CK_BEHAVIORIO_ACTIVE);  }

/*************************************************
Summary: Returns the owner behavior.
Return Value:
	A pointer to the CKBehavior this IO belongs to.
See also: SetOwner
*************************************************/
CKBehavior *GetOwner()			{ return m_OwnerBehavior; } 

//-------------------------------------------------------------------------
// Internal functions 
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

	void SetOwner(CKBehavior *b) {	m_OwnerBehavior = b; }							

			CKBehaviorIO(CKContext *Context,CKSTRING name=NULL);					
	virtual	~CKBehaviorIO();														
	virtual CK_CLASSID GetClassID();												

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);					

	virtual void			PreDelete();											

	virtual int				GetMemoryOccupation();									

	//--------------------------------------------
	// Dependencies functions	{secret}											
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);				
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);				

	//--------------------------------------------
	
	static CKSTRING  GetClassName();												
	static int		 GetDependenciesCount(int mode);								
	static CKSTRING  GetDependencies(int i,int mode);								
	static void		 Register();													
	static CKBehaviorIO* CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKBehaviorIO*);							
	static CK_CLASSID m_ClassID;													

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKBehaviorIO* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_BEHAVIORIO)?(CKBehaviorIO*)iO:NULL;
	}

	//--- Ensure Links that are on the same behavior are the last in the list
	void SortLinks();
	bool ActiveIOAndBB();


protected :
	XSObjectPointerArray	m_Links; 
	CKBehavior*				m_OwnerBehavior; 

	void	SetOldFlags(int Type);
	int		GetOldFlags();
#endif // Docjet secret macro
};

#endif