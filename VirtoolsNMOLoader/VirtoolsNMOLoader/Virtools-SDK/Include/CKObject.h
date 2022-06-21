/*************************************************************************/
/*	File : CKObject.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKOBJECT_H

#define CKOBJECT_H "$Id:$"

#include "CKDefines.h"
#include "CKDefines2.h"
#include "CKContext.h"

class CKFile;
class CKDependenciesContext;


typedef enum CK_OBJECT_SHOWOPTION {
	CKHIDE				=0,
	CKSHOW				=1,
	CKHIERARCHICALHIDE	=2
} CK_OBJECT_SHOWOPTION;

/***************************************************************************
{filename:CKObject}
Name: CKObject

Summary: Base class for most CK objects

Remarks:
	+ This is the base class for all objects of Virtools. You usually don't need to create an instance of
	CKObject, though this is possible. You usually create instances of classes derived of CKObject (See CKContext::CreateObject).
	CKObject provides functionnalities that are applicable to all its derived classes.

	+ CKObject and derived classes have no public (exported) constructors. Instances of CKObject and derived classes are created 
	with the CKContext::CreateObject method, and should be deleted with the CKContext::DestroyObject method. The CKContext::DestroyObject 
	function ensures that all Virtools objects using the to-be-deleted object take the necessary mesures to reflect the upcoming deletion. If you have
	created and used the object for your own needs and are sure that it has not been referenced anywhere else, you may delete the object without notifications
	spcifying CK_DESTROY_NONOTIFY.

	+ The CKObject class maintains a global ID for each of its instances. The IDs are globally unique and
	automatically created when the object is created. You obtain the global ID through the
	GetID method, and you can retrieve the object corresponding to an ID with the CKContext::GetObject
	method.

	+ Also, you should always reference CK objects through their global ID rather than with a direct pointer
	to them. Otherwise, you may keep a reference to an object that does not exist any more, as
	the object may have been deleted in the meantime. You can check that the object corresponding to the ID is still
	around when you need it by using the CKGetObject function to retrieve it.

	+ Each class derived from CKObject has a class ID associated to it. You can access this class ID through the instances.
	This is usefull for filtering or triggering different actions on objects according to their class. You access
	the class ID through the GetClassID method. 

	+ A name can be attached to each instance of the CKObject class and derived classes. This name is a CKSTRING.
	This functionnality is provided as a conveniency for debugging purposes. The Virtools library does not do any processing 
	on name except for loading purposes only (in the case of animation that should be attributed to objects for example).

	+ It is possible to associate arbitrary data to each instance of CKObject and derived classes. This data
	is not taken into account by the CK library. It is a service provided to the client application. The client
	application is responsible for the memory management of this data, for conflict resolution about the access to 
	the data and for its internal organization.
	You attach the data with the SetAppData method and you get it back with the GetAppData method.
	You can remove the data with by calling SetAppData with a NULL argument.




See also: CKContext::CreateObject, CKContext::GetObject, CKContext::DestroyObject
*******************************************************************************/
class CKObject{
public :
//----------------------------------------------------------
// Name
void SetName(CKSTRING Name,CKBOOL shared=FALSE);

//----------------------------------------------------------
// Application Data 	
void *GetAppData();
void SetAppData(void *Data);


void SetAppDataVSL(void *Data); //specific version of SetAppData for VSL binding

//----------------------------------------------------------
// Object Visibility
virtual	void	Show(CK_OBJECT_SHOWOPTION show = CKSHOW);
virtual	CKBOOL	IsHiddenByParent();
virtual	int		CanBeHide();

/*************************************************
Summary: Returns whether this object is visible. 
Return value: TRUE if the object is visible, FALSE otherwise.
Remarks:
+Only CKRenderObject and derived classes(CK2dEntity,CK3dEntity),CKMesh and CKGroup return relevant information
about their visibility state. Other classes may return any values.
+An object can return CKSHOW and still be hidden if its parent (see CK3dEntity::GetParent and CK2dEntity::GetParent)
is hierarchically hidden (see CKObject::Show)

See also: IsHiddenByParent,Show,IsHierarchicallyHide
*************************************************/
virtual CKBOOL IsVisible() { return (m_ObjectFlags & CK_OBJECT_VISIBLE)?CKSHOW:CKHIDE; }

/*************************************************
Summary:  Returns whether this object is hidden (and also hides its children).
Return Value:
	TRUE if hierarchically hidden.

Remarks:
	+ This methods returns if this object is hidden and also hides all its sub-hierarchy.
	+ See CKObject::Show for more details on hierarchically hidden objects.



See also: Show,IsVisible,IsHiddenByParent,CK3dEntity,CK2dEntity,CK_OBJECT_FLAGS
*************************************************/
CKBOOL IsHierarchicallyHide() { return (m_ObjectFlags & CK_OBJECT_HIERACHICALHIDE)?TRUE:FALSE; }

/**************************************************
Summary: Returns a pointer to the owner CKContext
Return Value: A pointer to the CKContext this object belongs to.
Remarks:
Each CKObject belongs to a given CKContext, this method gives acces to this CKContext.
See also: CKContext
**************************************************/
CKContext* GetCKContext()	{ return m_Context; }

/**************************************************
Summary: Returns the Identifier for this object
Return Value: CK_ID of this object
Remarks:
Returns the global ID for the object. The ID is globally unique.
It is automatically assigned to the object when it is created
(with CKContext::CreateObject and CKContext::CopyObject). It is safer to always store 
references to CKObject by storing its ID instead of pointers to these objects.
You can retrieve an object from its ID using the CKGetObject or CKContext::GetObject function.
See also: CreateObject, CreateCopy, CKContext::GetObject, Object Identifiers
**************************************************/
CK_ID	GetID() { 	return m_ID; }

/*************************************************
Summary: Returns the name of the object.
Return Value: A pointer to the object name or NULL if the object is unnamed.
Remarks:
+Provided as a conveniency for debugging purposes.
+The Virtools library does not do any processing corresponding to the name 
but it can be used when loading animation on objects for example.

See Also:GetName,CKContext::GetObjectByName,CKContext::GetObjectByNameAndClass
*************************************************/
CKSTRING GetName() { 
	return m_Name; 
}

/*************************************************
Summary: Returns the current object flags for this object. 
Return Value:
	A combination of CK_OBJECT_FLAGS for this objet.
Remarks:
	+ Many of these flags can be directly checked by using the appropriate method (see CK_OBJECT_FLAGS)
	instead of using this method.

See also: CK_OBJECT_FLAGS,ModifyObjectFlags
*************************************************/
CKDWORD GetObjectFlags() { return m_ObjectFlags; }


/*************************************************
Summary: Returns if the current object is dynamic (can be deleted or created at runtime). 
Return value:
	TRUE if object is dynamic.
Remarks:
	+ See Dynamic objects for more details.

See also: CK_OBJECT_FLAGS,ModifyObjectFlags,Dynamic Objects
*************************************************/
CKBOOL	IsDynamic()		{ return ((m_ObjectFlags & CK_OBJECT_DYNAMIC) == CK_OBJECT_DYNAMIC); } 

/*************************************************
Summary: Returns if the current object is to be deleted.
Return value:
	TRUE if object is about to be deleted.
Remarks:
	+ Managers and behaviors may be notified when objects are destroyed, this method
	enables them to check if an object they own is about to be destroyed.

See also: CK_OBJECT_FLAGS,ModifyObjectFlags
*************************************************/
CKBOOL	IsToBeDeleted() { return (m_ObjectFlags & CK_OBJECT_TOBEDELETED);}

/*************************************************
Summary: Adds or Removes flags for this object. 
Arguments:
	 add : A combination of CK_OBJECT_FLAGS to add.
  remove : A combination of CK_OBJECT_FLAGS to remove.
Remarks:
	+ You rarely need to modify directly this flags through CKObject::ModifyObjectFlags instead
	you should always use the specific acces function (given between () in CK_OBJECT_FLAGS documentation)
	which may need to perform additionnal operations.

See also: CK_OBJECT_FLAGS,GetObjectFlags
*************************************************/
void ModifyObjectFlags(CKDWORD add,CKDWORD remove) { m_ObjectFlags |= add;	m_ObjectFlags &= ~remove; }



//--------------------------------------------------------
////               Private Part                     
#ifdef DOCJETDUMMY // Docjet secret macro
#else

			CKObject() {}															
			CKObject(CKContext *Context,CKSTRING name=NULL);						
	virtual	~CKObject();															
	virtual CK_CLASSID GetClassID();

	virtual void			PreSave(CKFile *file,CKDWORD flags);					
	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);					
	virtual void			PostLoad();												

	virtual void			PreDelete();											
	virtual void			CheckPreDeletion();										
	virtual void			CheckPostDeletion();									

	virtual int				GetMemoryOccupation();									
	virtual CKBOOL			IsObjectUsed(CKObject* obj,CK_CLASSID cid);				

	//--------------------------------------------
	// Dependencies functions														
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);				
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);
	XString& SmartRenameForCopy(const CKSTRING oldName, const XString& copyAppendString);

	//--------------------------------------------
	// Class Registering
	static CKSTRING  GetClassName();												
	static int		 GetDependenciesCount(int mode);								
	static CKSTRING  GetDependencies(int i,int mode);								
	static void		 Register();													
	static CKObject* CreateInstance(CKContext *Context);							
	static void		 ReleaseInstance(CKContext* iContext,CKObject*);							
	static CK_CLASSID m_ClassID;													
	static CKObject* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_OBJECT)?(CKObject*)iO:NULL;
	}
	
public:
		CK_ID		m_ID;										
		CKSTRING	m_Name;										
		CKDWORD		m_ObjectFlags;								
		CKContext	*m_Context;									

// Flags acces
		CKBOOL	IsUpToDate() { return (m_ObjectFlags & CK_OBJECT_UPTODATE);}		
		CKBOOL	IsPrivate() { return (m_ObjectFlags & CK_OBJECT_PRIVATE);}			
		CKBOOL	IsNotToBeSaved() { return (m_ObjectFlags & CK_OBJECT_NOTTOBESAVED);}
		CKBOOL	IsInterfaceObj() { return (m_ObjectFlags & CK_OBJECT_INTERFACEOBJ);}

// Util acces to CKContext functions 
		CKERROR	CKDestroyObject(CKObject *obj,DWORD Flags=0,CKDependencies* depoptions=NULL) { return m_Context->DestroyObject(obj,Flags,depoptions); }	
		CKERROR	CKDestroyObject(CK_ID id,DWORD Flags=0,CKDependencies* depoptions=NULL) { return m_Context->DestroyObject(id,Flags,depoptions); }		
		CKERROR	CKDestroyObjects(CK_ID* obj_ids,int Count,DWORD Flags=0,CKDependencies* depoptions=NULL) { return m_Context->DestroyObjects(obj_ids,Count,Flags,depoptions); }	
		CKObject *CKGetObject(CK_ID id) { return m_Context->GetObject(id); }
		CKObject *GetCKObject(CK_ID id){	return m_Context->GetObject(id);}
#endif // Docjet secret macro
};


#endif

