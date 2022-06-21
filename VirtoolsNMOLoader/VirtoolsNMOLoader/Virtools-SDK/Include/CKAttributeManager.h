/*************************************************************************/
/*	File : CKAttributeManager.h											*/
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKATTRIBUTEMANAGER_H

#define CKATTRIBUTEMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKBaseManager.h"


struct CKAttributeDesc;

struct CKAttributeCategoryDesc;


/****************************************************************
Summary: Function called when an attribute is set or removed on an object.

Remarks:
	+A callback function can be set for a given attribute type with CKAttributeManager::SetAttributeCallbackFunction.
	+This function will be called each time an attribute with the given type is set (with Set = TRUE) or removed (with Set = FALSE)
	on an object.
See Also:CKAttributeManager::SetAttributeCallbackFunction,Using Attributes
*****************************************************/
typedef void	(*CKATTRIBUTECALLBACK)		(CKAttributeType AttribType,CKBOOL Set,CKBeObject *obj,void *arg);

	
/*********************************************************************
Name: CKAttributeManager


Summary: Object attributes management.
Remarks: 
+ Every CKBeObject may be given some attributes that behaviors or other
classes may ask for information.
+ An attribute is defined by :
	+ A Name
	+ A Category (Optionnal)
	+ A Parameter type (Optionnal)
+ New types of attributes may be registered by giving them a name and the type of parameters that will come along with them. Once registered this 
new type of attribute may be accessed by name or by the unique index returned by RegisterNewAttributeType.
+ Retrieving attributes by index is of course far more efficient than by Name.
+ The unique instance of CKAttributeManager can be retrieved by calling CKContext::GetAttributeManager() and its manager GUID is ATTRIBUTE_MANAGER_GUID.
+ See the Using attributes paper for more detail on how to use attributes.
See also: CKBeObject::SetAttribute,CKBeObject,CKParameterManager,Using Attributes
***********************************************************************/
class CKAttributeManager:public CKBaseManager {
friend class CKPluginManager;
friend class CKFile;
public:


//-------------------------------------------------------------------
// Registration
CKAttributeType	 RegisterNewAttributeType(CKSTRING Name,CKGUID ParameterType,CK_CLASSID CompatibleCid=CKCID_BEOBJECT,CK_ATTRIBUT_FLAGS flags = CK_ATTRIBUT_SYSTEM);
void UnRegisterAttribute(CKAttributeType AttribType);
void UnRegisterAttribute(CKSTRING atname);

CKSTRING		GetAttributeNameByType(CKAttributeType AttribType);
CKAttributeType	GetAttributeTypeByName(CKSTRING AttribName);

void SetAttributeNameByType(CKAttributeType AttribType,CKSTRING name);

int	GetAttributeCount();

//-------------------------------------------------------------------
// Attribute Parameter
CKGUID			 GetAttributeParameterGUID(CKAttributeType AttribType);
CKParameterType	 GetAttributeParameterType(CKAttributeType AttribType);


//-------------------------------------------------------------------
// Attribute Compatible Class Id
CK_CLASSID	 GetAttributeCompatibleClassId(CKAttributeType AttribType);

//-------------------------------------------------------------------
// Attribute Flags
CKBOOL	 IsAttributeIndexValid(CKAttributeType index);
CKBOOL	 IsCategoryIndexValid(CKAttributeCategory index);


//-------------------------------------------------------------------
// Attribute Flags
CK_ATTRIBUT_FLAGS	 GetAttributeFlags(CKAttributeType AttribType);

//-------------------------------------------------------------------
// Attribute Callback
void	SetAttributeCallbackFunction(CKAttributeType AttribType,CKATTRIBUTECALLBACK fct,void *arg);

//-------------------------------------------------------------------
// Attribute Default Value
void		SetAttributeDefaultValue(CKAttributeType AttribType,CKSTRING DefaultVal);
CKSTRING	GetAttributeDefaultValue(CKAttributeType AttribType);


//-------------------------------------------------------------------
// Group By Attributes
const XObjectPointerArray& GetAttributeListPtr(CKAttributeType AttribType);
const XObjectPointerArray& GetGlobalAttributeListPtr(CKAttributeType AttribType);

const XObjectPointerArray& FillListByAttributes(CKAttributeType *ListAttrib,int AttribCount);
const XObjectPointerArray& FillListByGlobalAttributes(CKAttributeType *ListAttrib,int AttribCount);

//----------------------------------------------------------------
// Categories...
int		 GetCategoriesCount();
CKSTRING GetCategoryName(CKAttributeCategory index);
CKAttributeCategory	 GetCategoryByName(CKSTRING Name);

void SetCategoryName(CKAttributeCategory catType,CKSTRING name);

CKAttributeCategory   AddCategory(CKSTRING Category,CKDWORD flags = 0);
void  RemoveCategory(CKSTRING Category);

CKDWORD GetCategoryFlags(CKAttributeCategory cat);
CKDWORD GetCategoryFlags(CKSTRING cat);

//-----------------------------------------------------------------
// Attribute Category
void  SetAttributeCategory(CKAttributeType AttribType,CKSTRING Category);
CKSTRING			GetAttributeCategory(CKAttributeType AttribType);
CKAttributeCategory GetAttributeCategoryIndex(CKAttributeType AttribType);

void	RemoveUnusedAttributes();

//-------------------------------------------------------------------
//-------------------------------------------------------------------
// Internal functions 
//-------------------------------------------------------------------
//-------------------------------------------------------------------

	
	virtual ~CKAttributeManager();
	
	virtual CKERROR PreClearAll();
	
	virtual CKERROR PostLoad();
	
	virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile);
	
	virtual CKStateChunk* SaveData(CKFile *SavedFile);
	
	virtual CKERROR SequenceAddedToScene(CKScene *scn,CK_ID *objid,int count);
	
	virtual CKERROR SequenceRemovedFromScene(CKScene *scn,CK_ID *objid,int count);
	
	virtual CKDWORD	GetValidFunctionsMask()	{ return CKMANAGER_FUNC_PreClearAll				|
															 CKMANAGER_FUNC_PostLoad		|
															 CKMANAGER_FUNC_OnSequenceAddedToScene  |
															 CKMANAGER_FUNC_OnSequenceRemovedFromScene; }	


};


#endif