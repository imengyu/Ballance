/*************************************************************************/
/*	File : XObjectArray.h												 */
/*	Author :  Aymeric BARD												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef _XOBJECTARRAY_H

#define _XOBJECTARRAY_H

#include "XArray.h"
#include "XSArray.h"

#include "CKGlobals.h"

class CKDependenciesContext;
class CKObject;

class XObjectArray;
class XObjectPointerArray;
class XSObjectArray;
class XSObjectPointerArray;



#define ITEROBJECTARRAY(array,obj_type,action)\
{\
	obj_type* obj;\
	for(CK_ID* it=array.Begin();it!=array.End();++it) {\
		if (obj = (obj_type*)m_Context->GetObject((*it)))\
		obj->action;\
	}\
}\


#define ITEROBJECTARRAYPTR(array,obj_type,action)\
{\
	obj_type* obj;\
	for(CKObject** it=array.Begin();it!=array.End();++it) {\
		 if (obj = (obj_type*)(*it)) obj->action;\
	}\
}\

/***************************************************************************
Summary: Container class for CKObject Id's

Remarks:
+ This class use the template container XSArray to contain object CK_ID's.
+ Therefore, the size occupied by the array is exactly the number of ids it
contains (whereas XObjectArray that can have a reserved size greater than
its number of IDs)
+ Supports for Check, Load, Save, Add, Remove, Find functions in the XObjectArray.
See Also: XSObjectPointerArray,XObjectArray,CKObjectArray, XSArray
****************************************************************************/
class XSObjectArray : public XSArray<CK_ID>
{
public:
/***************************************************************************
Summary: Converts an XSObjectArray into an XSArray<CKObject*>

Arguments:
	Context: A Pointer to CKContext.
	array: A reference to the XSArray<CKObject*> to fill.

Remarks:
+ Only valid objects are added to the XObjectPointerArray, so the arrays
may not have the same size if the XObjectArray wasn't checked before the
conversion.

See Also: ConvertFromObjects					
*******************************************************/
inline void ConvertToObjects(CKContext *Context,XSArray<CKObject *>&array)  const
{
	array.Clear();
	for (CK_ID* ids=Begin();ids!=End();++ids) {
		CKObject* obj = CKGetObject(Context,*ids);
		if (obj) array.PushBack(obj);
	}
}

/***************************************************************************
Summary: Converts an XSArray<CKObject *> into an XSObjectArray.

Arguments:
   array: a reference to the XSArray<CKObject *> to read from.

Remarks:
	+ If NULL pointers are in the XSArray<CKObject *>, ids of 0 are added to
	the XSObjectArray anyway.

See Also: ConvertFromObjects					
*******************************************************/
void ConvertFromObjects(const XSArray<CKObject *>& array);							

/*******************************************************
Summary: Checks the CK_ID's array to remove objects that do not
exist anymore.

Arguments:
	Context: A Pointer to CKContext.

Return Value: 
	TRUE if the objects have been deleted, FALSE otherwise.

Remarks:
	+ If a CK_ID does not reference a valid object anymore, it is removed
	from the array. You have to check array just after a deletion 
	because CK_IDs can be re-used for other objects, when initial
	object was dynamic or deleted with the flag CK_DESTROY_FREEID.

See Also: XObjectArray::Check
*******************************************************/
BOOL Check(CKContext *Context);

/*******************************************************
Summary: Adds given object's ID to the array only if it is not already present.

Arguments:
	id: CK_ID of the object to add.
	obj: Pointer to Object.

Return Value: 
	TRUE if the object id is added to array, FALSE otherwise.

Remarks:
	+ Addition is done only if the given Object is not already in the array.
See Also: FindObject, RemoveObject
*******************************************************/
BOOL AddIfNotHere(CK_ID id);
BOOL AddIfNotHere(CKObject* obj);

/*******************************************************
Summary: Gets an object from CK_ID's array.

Arguments:
	Context: A pointer to Context
	i:		 position from where object is to be extracted

Return Value: 
	Pointer to Object at the given position in the context.

See Also: GetObjectID
*******************************************************/
CKObject* GetObject(CKContext *Context,unsigned int i)  const;

/***************************************************************************
Summary: Gives Object Id at given position in the Object Id Array.

Arguments:
	i: Position at which object is to be obtained.

Return Value:
	CK_ID of the object at given position in the array.
		
See Also: GetObject
*******************************************************/
CK_ID GetObjectID(unsigned int i) const
{
	if (i<(unsigned int)(Size())) {
		return *(Begin()+i);
	}
	return 0;
}

/*******************************************************
Summary: Removes the given object id from CK_ID's array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	The position from where the Object id is removed.

See Also: FindObject, AddIfNotHere
*******************************************************/
BOOL RemoveObject(CKObject* obj);

/*******************************************************
Summary: Checks if given object is in CK_ID's array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	TRUE if the object id is in object id array, FALSE otherwise.

See Also: AddIfNotHere, RemoveObject, FindID
*******************************************************/
BOOL FindObject(CKObject* obj) const;

/***************************************************************************
Summary: Finds whether the given Object Id is in the Object Id Array.

Arguments:
	id: Id of the Object to find.

Return Value:
	TRUE if given object Id is found in array, FALSE otherwise.
		
See Also: FindObject					
*******************************************************/
BOOL FindID(CK_ID id) const { return Find(id) != End();}	
			
void Load(CKStateChunk *chunk);												
void Save(CKStateChunk *chunk,CKContext* ctx) const;										
void Prepare(CKDependenciesContext& context)  const;						
void Remap(CKDependenciesContext& context);									
};


/***************************************************************************
Summary: Container class for CKObject pointers.

Remarks:
+ This class use the template container XArray to contain object pointers.
+ Exactly same as XObjectArray class, but uses XArray (Pre-allocated items) 
for storing pointers, and not IDs (more efficient to retrieve the objects).
+ Supports for Check, Load, Save, Add, Remove, Find functions in the CKObject Pointer array.
See Also:XSObjectPointerArray,XObjectArray,CKObjectArray
****************************************************************************/
class XObjectPointerArray : public XArray<CKObject*>
{
public:
	
	XObjectPointerArray(const int iSize = 0):XArray<CKObject*>(iSize) {}


/*******************************************************
Summary: Adds given object to object array only if it is not in array.

Arguments:
	obj: Pointer to Object.

Return Value: 
	TRUE if the object id is added to array, FALSE otherwise.

Remarks:
	+ Addition is done only if the given Object is not in the array.

See Also: RemoveObject, FindObject
*******************************************************/
BOOL AddIfNotHere(CKObject* obj)
{
	if (FindObject(obj)) return FALSE;
	PushBack(obj);
	return TRUE;
}

/*******************************************************
Summary: Gets an object from object array.

Arguments:
	i:	Position from where object is to be extracted

Return Value: 
	Object at the given position in the array if present, NULL otherwise.

See Also: GetObjectID
*******************************************************/
CKObject* GetObject(unsigned int i)  const 
{
	if (i<(unsigned int)(m_End-m_Begin)) {
		return *(m_Begin+i);
	}
	return NULL;
}

/*******************************************************
Summary: Removes the given object from object array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	The position from where the Object id is removed.

Remarks:
	+ The removal only shifts left the pointers after 
	the element removed. No memory is freed.

See Also: FindObject, AddIfNotHere
*******************************************************/
int	RemoveObject(CKObject* obj) {return (int) Remove(obj);}

/*******************************************************
Summary: Checks if given object is in object array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	TRUE if the object is in object array, FALSE otherwise.

See Also: RemoveObject, AddIfNotHere
*******************************************************/
BOOL FindObject(CKObject* obj)  const {return IsHere(obj);}

/*******************************************************
Summary: Gets the object id from object array at a given position.

Arguments:
	i:	Position from where object id is to be extracted

Return Value: 
	Id of the object at the given position in the array.

See Also: GetObject
*******************************************************/
CK_ID GetObjectID(unsigned int i) const;

/*******************************************************
Summary: Checks the CKObject*'s array to remove objects that don't
exist anymore.

Return Value: 
	TRUE if the objects have been deleted, FALSE otherwise.

Remarks:
	+ If a CKObject* is to be deleted (flagged), it is removed
	from the array. You have to check array after the objects 
	have been flagged, but before they are actually deleted 
	otherwise it will crashes (pointer pointing on nothing.)

See Also: XSObjectPointerArray::Check
*******************************************************/
BOOL Check();

void Load(CKContext *Context,CKStateChunk *chunk);		
void Save(CKStateChunk *chunk)  const;					
void Prepare(CKDependenciesContext& context)  const;	
void Remap(CKDependenciesContext& context);				
};

/***************************************************************************
Summary: Container class for CKObject Id's.

Remarks:
	+ This class use the template container XArray to contain object CK_ID's.
	+ Exactly same as XSObjectArray class, but uses XArray (Pre-allocated items).
	+ Supports for Check, Load, Save, Add, Remove, Find functions in the Object CK_ID array.
See Also: XSObjectPointerArray,XObjectArray,CKObjectArray
****************************************************************************/
class XObjectArray : public XArray<CK_ID>
{
public:
	
	XObjectArray(const int iSize = 0):XArray<CK_ID>(iSize) {}

/***************************************************************************
Summary: Converts an XObjectArray into an XObjectPointerArray.

Arguments:
	Context: A Pointer to CKContext.
	array:   a reference to the XObjectPointerArray to fill.

Remarks:
	+ Only valid objects are added to the XObjectPointerArray, so the arrays
	may not have the same size if the XObjectArray wasn't checked before the
	conversion.

See Also: ConvertFromObjects					
*******************************************************/
inline void ConvertToObjects(CKContext *Context,XObjectPointerArray& array)  const
{
	array.Resize(Size());
	array.Resize(0);
	for (CK_ID* ids=m_Begin;ids!=m_End;++ids) {
		CKObject* obj=CKGetObject(Context,*ids);
		if (obj) array.PushBack(obj);
	}
}

/***************************************************************************
Summary: Converts an XObjectPointerArray into an XObjectArray.

Arguments:
	array: a reference to the XObjectPointerArray to read from.

Remarks:
	+ If NULL pointers are in the XObjectPointerArray, ids of 0 are added to
	the XObjectArray anyway.

See Also: ConvertFromObjects					
*******************************************************/
void ConvertFromObjects(const XObjectPointerArray& array);

/*******************************************************
Summary: Checks the CK_ID's array to remove objects that do not
exist anymore.

Arguments:
	Context: A Pointer to CKContext.

Return Value: 
	TRUE if the objects have been deleted, FALSE otherwise.

Remarks:
	+ If a CK_ID doesn't reference an object anymore, it is removed
	from the array. You have to check array just after a deletion 
	because CK_IDs can be reused for other objects, when initial
	object was dynamic or deleted with the flag CK_DESTROY_FREEID.

See Also: XSObjectArray::Check
*******************************************************/
BOOL Check(CKContext *Context);

/*******************************************************
Summary: Adds given object's ID to the array if it is not already present..

Arguments:
	id: CK_ID of the object to add.
	obj: Pointer to Object.

Return Value: 
	TRUE if the object id is added to array, FALSE otherwise.

Remarks:
	+ Addition is done only if the given Object is not in the array.

See Also: FindObject, RemoveObject
*******************************************************/
BOOL AddIfNotHere(CK_ID id);
BOOL AddIfNotHere(CKObject* obj);

/*******************************************************
Summary: Gets an object from CK_ID's array.

Arguments:
	Context: A pointer to Context
	i:		 position from where object is to be extracted

Return Value: 
	Pointer to Object at the given position in the context.

See Also: GetObjectID
*******************************************************/
CKObject* GetObject(CKContext *Context,unsigned int i)  const;

/***************************************************************************
Summary: Returns Object Id at given position in the Object Id Array.

Arguments:
	i: Position at which object is to be obtained.

Return Value:
	CK_ID of the object at given position in the array.
		
See Also: GetObject
*******************************************************/
CK_ID GetObjectID(unsigned int i)  const 
{
	if (i<(unsigned int)(m_End-m_Begin)) {
		return *(m_Begin+i);
	}
	return 0;
}

/*******************************************************
Summary: Removes the given object id from CK_ID's array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	The position from where the Object id is removed.

See Also: FindObject, AddIfNotHere
*******************************************************/
BOOL RemoveObject(CKObject* obj);	

/*******************************************************
Summary: Checks if given object is in CK_ID's array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	TRUE if the object id is in object id array, FALSE otherwise.

See Also: AddIfNotHere, RemoveObject, FindID
*******************************************************/
BOOL FindObject(CKObject* obj)  const;

/***************************************************************************
Summary: Finds whether the given Object Id is in the Object Id Array.

Arguments:
	id: Id of the Object to find.

Return Value:
	TRUE if given object Id is found in array, FALSE otherwise.
		
See Also: FindObject
*******************************************************/
BOOL FindID(CK_ID id) const { return Find(id) != m_End;}			

void Load(CKStateChunk *chunk);													
void Save(CKStateChunk *chunk,CKContext* ctx)  const;											
void Prepare(CKDependenciesContext& context)  const;							
void Remap(CKDependenciesContext& context);										
};

/***************************************************************************
Summary: Container class for CKObject pointers.

Remarks:
+ This class use the template container XSArray to contain object pointers.
+ Exactly same as XObjectPointerArray class, but uses XSArray for storing pointers,
thus the allocated size of the array always exactly match what it contains, each 
insertion and removal generating an allocation and a deallocation.
+ Supports for Check, Load, Save, Add, Remove, Find functions in the CKObject Pointer array.
See Also:XObjectPointerArray,XObjectArray,CKObjectArray
****************************************************************************/
class XSObjectPointerArray : public XSArray<CKObject*>
{
public:
/*******************************************************
Summary: Adds given object to object array only if it is not in array.

Arguments:
	obj: Pointer to Object.

Return Value: 
	TRUE if the object id is added to array, FALSE otherwise.

Remarks:
	+ Addition is done only if the given Object is not in the array.

See Also: RemoveObject, FindObject
*******************************************************/
BOOL AddIfNotHere(CKObject* obj)
{
	if (FindObject(obj)) return FALSE;
	PushBack(obj);
	return TRUE;
}

/*******************************************************
Summary: Gets an object from object array.

Arguments:
	i:	Position from where object is to be extracted

Return Value: 
	Object at the given position in the array if present, NULL otherwise.

See Also: GetObjectID
*******************************************************/
CKObject* GetObject(unsigned int i)  const
{
	if (i<(unsigned int)(Size())) {
		return *(Begin()+i);
	}
	return NULL;
}

/*******************************************************
Summary: Removes the given object from object array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	The position from where the Object id is removed.

Remarks:
	The removal actually delete and reallocate the 
	whole array. Use XObjectPointerArray to avoid
	this.

See Also: AddIfNotHere, FindObject
*******************************************************/
int	RemoveObject(CKObject* obj) {return (int) Remove(obj);}

/*******************************************************
Summary: Checks if given object is in object array.

Arguments:
	obj: A pointer to Object.

Return Value: 
	TRUE if the object is in object array, FALSE otherwise.

See Also: RemoveObject, AddIfNotHere
*******************************************************/
BOOL FindObject(CKObject* obj)  const {return IsHere(obj);}

/*******************************************************
Summary: Gets the object id from object array at a given position.

Arguments:
	i:	Position from where object id is to be extracted

Return Value: 
	Id of the object at the given position in the array.

See Also: GetObject
*******************************************************/
CK_ID GetObjectID(unsigned int i)  const;

/*******************************************************
Summary: Checks the CKObject*'s array to remove objects that don't
exist anymore.

Return Value: 
	TRUE if the objects have been deleted, FALSE otherwise.

Remarks:
	If a CKObject* is to be deleted (flagged), it is removed
	from the array. You have to check array after the objects 
	have been flagged, but before they are actually deleted 
	otherwise it will crashes (pointer pointing on nothing.)

See Also: XObjectPointerArray::Check
**************************************************************/
BOOL Check();

void Load(CKContext *Context,CKStateChunk *chunk);		
void Save(CKStateChunk *chunk)  const;					
void Prepare(CKDependenciesContext& context)  const;	
void Remap(CKDependenciesContext& context);				
};

#endif