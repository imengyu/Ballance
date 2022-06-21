/*************************************************************************/
/*	File : CKSynchroObject.h											 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSYNCOBJECT_H

#define CKSYNCOBJECT_H "$Id:$"

#include "CKObject.h"
#include "CKObjectArray.h"


/*************************************************
Name: CKSynchroObject

Summary: Class managing a rendez-vous mechanism

Remarks: 
	+ Synchro Object are used in the Interface to create a 
	Rendez-vous mechanism. Objects register themselves as arrived to
	a rendez-vous and ask for the permission to pass, this permission is
	granted when the specified number of object have arrived.

	+ This class is mainly used by the corresponding parameter type CKPGUID_SYNCHRO.

	+ The class id of CKSynchroObject is CKCID_SYNCHRO


See also: 
*************************************************/
class CKSynchroObject : public CKObject {
public :
void Reset();
//-------------------------------------------
// Rendez Vous
void SetRendezVousNumberOfWaiters(int waiters);
int GetRendezVousNumberOfWaiters();
CKBOOL CanIPassRendezVous(CKBeObject *asker);
int GetRendezVousNumberOfArrivedObjects();
CKBeObject* GetRendezVousArrivedObject(int pos);

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
			CKSynchroObject(CKContext *Context,CKSTRING name=NULL);				
	virtual	~CKSynchroObject();													
	virtual CK_CLASSID GetClassID();											

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);				
	virtual void			CheckPostDeletion();								

	virtual int				GetMemoryOccupation();								
	virtual CKBOOL			IsObjectUsed(CKObject* obj,CK_CLASSID cid);			

//--------------------------------------------
// Class Registering 
	static CKSTRING  GetClassName();										
	static int		 GetDependenciesCount(int mode);						
	static CKSTRING  GetDependencies(int i,int mode);						
	static void		 Register();											
	static CKSynchroObject* CreateInstance(CKContext *Context);				
	static void		 ReleaseInstance(CKContext* iContext,CKSynchroObject*);							
	static CK_CLASSID m_ClassID;											

protected :
	int m_MaxWaiters;
	CKObjectArray m_Arrived;
	CKObjectArray m_Passed;
#endif // Docjet secret macro
};



/*************************************************
Name: CKCriticalSectionObject

Summary: Class managing a critical section mechanism

Remarks:
	+ A CriticalSection enables objects to acces resources using behaviors without risks
	of being executed concurrently. An object ask for the right to use a section through
	EnterCriticalSection and warm that is has finished with LeaveCriticalSection

	+ This class is mainly used by the corresponding parameter type CKPGUID_CRITICALSECTION.

	+ The class id of CKCriticalSectionObject is CKCID_CRITICALSECTION


See also: 
*************************************************/
class CKCriticalSectionObject : public CKObject {
public :
void Reset();

CKBOOL EnterCriticalSection(CKBeObject *asker);
CKBOOL LeaveCriticalSection(CKBeObject *asker);

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
			CKCriticalSectionObject(CKContext *Context,CKSTRING name=NULL);		
	virtual	~CKCriticalSectionObject();											
	virtual CK_CLASSID GetClassID();											

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);				
	virtual void			CheckPostDeletion();								

	virtual int				GetMemoryOccupation();								
	virtual CKBOOL			IsObjectUsed(CKObject* obj,CK_CLASSID cid);			


//--------------------------------------------
// Class Registering 
	static CKSTRING  GetClassName();											
	static int		 GetDependenciesCount(int mode);							
	static CKSTRING  GetDependencies(int i,int mode);							
	static void		 Register();												
	static CKCriticalSectionObject* CreateInstance(CKContext *Context);			
	static void		 ReleaseInstance(CKContext* iContext,CKCriticalSectionObject*);							
	static CK_CLASSID m_ClassID;												


protected :
	CK_ID	m_ObjectInSection;
#endif // Docjet secret macro
};




/*************************************************
Name: CKStateObject

Summary: Class managing a global state mechanism

Remarks:
+ A StateObject describes a global state variable that can be TRUE if EnterState is called
or FALSE is ExitState is called. This variable is identified by a name and can 
be checked through IsStateActive.
+ This class is mainly used by the corresponding parameter type CKPGUID_STATE.

+ The class id of CKStateObject is CKCID_STATE

See also: 
*************************************************/
class CKStateObject : public CKObject {

public :
//-----------------------------------
// Event
CKBOOL IsStateActive();
void EnterState();
void LeaveState();


//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

			CKStateObject(CKContext *Context,CKSTRING name=NULL);			
	virtual	~CKStateObject();												
	virtual CK_CLASSID GetClassID();										

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);				
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);			

	virtual int				GetMemoryOccupation();							

//--------------------------------------------
// Class Registering 
	static CKSTRING  GetClassName();										
	static int		 GetDependenciesCount(int mode);						
	static CKSTRING  GetDependencies(int i,int mode);						
	static void		 Register();											
	static CKStateObject* CreateInstance(CKContext *Context);				
	static void		 ReleaseInstance(CKContext* iContext,CKStateObject*);							
	static CK_CLASSID m_ClassID;											

protected :
	CKBOOL m_Event;
#endif // Docjet secret macro
};


#endif
