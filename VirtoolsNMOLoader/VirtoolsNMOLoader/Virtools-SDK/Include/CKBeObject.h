/*************************************************************************/
/*	File : CKBeObject.h				 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKBEOBJECT_H

#define CKBEOBJECT_H "$Id:$"

#include "CKSceneObject.h"
#include "XObjectArray.h"
#include "XHashTable.h"

struct CKFileObject;

/*************************************************
Summary: Storage of couple (Attribute,Parameter)

Remarks:
	This structure is used to gain acces to the list of attribute on a CKBeObject through the
function CKBeObject::GetAttributeList. 



See also: CKBeObject::GetAttributeList,CKAttributeManager
*************************************************/
struct CKAttributeVal {
	CKAttributeType AttribType;	// Type of Attribute
	CK_ID			Parameter;	// ID of the associated parameter
};


typedef XHashTable<CK_ID,CK_ID>					XAttributeList;		


/*************************************************
{filename:CKBeObject}
Name: CKBeObject

Summary: Base class for objects with behaviors

Remarks:
	+ CKBeObject is the base class for all objects that can have scripts and attributes (BeObjects).

	+ Instances of BeObjects can have a set of scripts attached to them. The object owns the scripts.
	The scripts are instances of CKBehavior. 

	+ The scripts are the behaviors that are activated when the object is installed in the current scene. They are the
	main starting points of the composition. They declare what the object does at the "beginning".

	+ When an instance of CKBeObject registers itself as waiting for messages, the message manager
	will process the messages sent to the object by stacking it in the object's "last frame messages" list. Behaviors
	attached to the object may then dequeue the messages by using the GetLastFrameMessage 
	and GetLastFrameMessageCount methods. These messages are only stored during one "frame".

	+ Behavioral objects may be active or not in any scene they belong to. When they are active, all their active scripts, are
	processed during the process loop. By default, a behavioral object is activated at start.

	+ Behavioral objects can have "Attributes" attached to them, each attribute has a specific type (or name) and a type of 
	parameter that is associated to. For exemple one can create a new attribute type called "Health Point" with a associated parameter type "float"
	and attach it to any behavioral object using SetAttribute method.

	+ Behavioral objects may be grouped together using instance of CKGroup.

	+ The class id of CKBeObject is CKCID_BEOBJECT.

See also: CKBehavior, CKMessageManager,CKAttributeManager,CKScene
*************************************************/
class CKBeObject : public CKSceneObject {

public :

//-----------------------------------------------------------
// Execution
void ExecuteBehaviors(float delta);

//-----------------------------------------------------------
// Group Functions 
CKBOOL IsInGroup(CKGroup* group);

//----------------------------------------------------------
// Attribute Functions
CKBOOL HasAttribute(CKAttributeType AttribType);
CKBOOL SetAttribute(CKAttributeType AttribType,CK_ID parameter=0);
CKBOOL RemoveAttribute(CKAttributeType AttribType);
CKParameterOut* GetAttributeParameter(CKAttributeType AttribType);

int GetAttributeCount();
int	GetAttributeType(int index);
CKParameterOut* GetAttributeParameterByIndex(int index);
void GetAttributeList(CKAttributeVal* liste);
void RemoveAllAttributes();


//---------------------------------------------------------------
// Scripts Functions 
CKERROR	   AddScript(CKBehavior *ckb);
CKBehavior *RemoveScript(CK_ID id);
CKBehavior *RemoveScript(int pos);
CKERROR	   RemoveAllScripts();
CKBehavior *GetScript(int pos);
int		   GetScriptCount();

//-----------------------------------------------------------------
// Priority
int			GetPriority();
void		SetPriority(int priority);

//------------------------------------------------------------------
// Messages 
int GetLastFrameMessageCount();
CKMessage* GetLastFrameMessage(int pos);

void SetAsWaitingForMessages(CKBOOL wait=TRUE);
CKBOOL IsWaitingForMessages();


//--------------------------------------------------------------------
// Misc
int CallBehaviorCallbackFunction(CKDWORD Message,CKGUID* behguid=NULL);

//--------------------------------------------------------------------
// Profiling
float GetLastExecutionTime();


//-------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // Docjet secret macro
#else

	virtual void ApplyPatchForOlderVersion(int NbObject,CKFileObject* FileObjects);	
			CKBeObject() {}															
			CKBeObject(CKContext *Context,CKSTRING name=NULL);						 
	virtual	~CKBeObject();															 
	virtual CK_CLASSID GetClassID();												 

	virtual void			PreSave(CKFile *file,CKDWORD flags);					
	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);					

	virtual void			PreDelete();											
	
	virtual int				GetMemoryOccupation();									
	virtual CKBOOL			IsObjectUsed(CKObject* obj,CK_CLASSID cid);				

	//--------------------------------------------
	
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);				
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);				

	virtual void AddToScene		(CKScene *scene,CKBOOL dependencies = TRUE);		
	virtual void RemoveFromScene(CKScene *scene,CKBOOL dependencies = TRUE);		

	//--------------------------------------------
	// Class Registering
	static CKSTRING  GetClassName();												
	static int		 GetDependenciesCount(int mode);								
	static CKSTRING  GetDependencies(int i,int mode);								
	static void		 Register();													
	static CKBeObject* CreateInstance(CKContext *Context);							
	static void		 ReleaseInstance(CKContext* iContext,CKBeObject*);							
	static CK_CLASSID m_ClassID;													

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKBeObject* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_BEOBJECT)?(CKBeObject*)iO:NULL;
	}

	void AddToSelfScenes(CKSceneObject* o);			
	void RemoveFromSelfScenes(CKSceneObject* o);	

protected :
	XBitArray				m_Groups;				
	XObjectPointerArray*	m_ScriptArray;			
	XVoidArray*				m_LastFrameMessages;
	unsigned short			m_Waiting;
	signed short			m_Priority;
	float					m_LastExecutionTime;
	XAttributeList*			m_Attributes;

	void SortScripts();
	void RemoveFromAllGroups();
	CKERROR AddLastFrameMessage(CKMessage *msg);
	CKERROR RemoveAllLastFrameMessage();
	void ApplyOwner();
	void ResetExecutionTime();

static int BeObjectPrioritySort(const void* o1,const void* o2);

#endif // Docjet secret macro
};


#endif
