/*************************************************************************/
/*	File : CKObjectArray.h												 */
/*	Author :  Nicolas Galinotti											 */	
/*	Last Modification : 28/09/99										 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKOBJECTARRAY_H

#define CKOBJECTARRAY_H "$Id:$"

#include "CKObject.h"




typedef int (*OBJECTARRAYCMPFCT)(CKObject *elem1, CKObject *elem2);


/********************************************************************************
{filename:CKObjectArray}
Name: CKObjectArray

Summary: Array of CKObject instances
Remarks:
+ This class is provided for compatibilty reasons and should not be used except for 
methods that require it explicitely. Use XObjectArray or XObjectPointerArray instead.

+ Provides utilities for storing, retrieving, iterating arrays of instances of
CKObject and derived classes. The CKObjectArray instance uses
a pointer to walk along to object it stores. Retrieval, deletion and insertions are done relatively
to this current object pointer.
+ The CKObjectArray is basically implemented as a linked list of global IDs.

+ Objects can be retrieved by position, ID, pointer or name. To get to an object,
use the corresponding method to move the current object pointer (PtrSeek, IDSeek, NameSeek, PositionSeek).
Then get the object using GetData.

+ As object may be deleted at any time, some object stored in CKObjectArray instances
may not be valid when they are retrieved with GetData. In this case, methods like GetData or Next
return NULL even though there are more objects in the array. Thus, in order to iterate
to the end of the list, you should use EndOfList to check if the end of the list is reached,
instead of using Next.

Do not use constructions like:

			array->Reset();
			while(tmp = array->Next())
			{
				[...use tmp ...]
			}

But use:

			for(array->Reset() ;!array->EndOfList(); array->Next())
			{
				tmp = array->GetData();
				[.. check and use tmp ...]
			}



+ The objects are stored linearly. So seek time speed is O(n), deletion and insertion speed are O(1).

+ CKObjectArray is not derived from CKObject,so it does not conform to the memory management scheme of CKObject.
CKObjectArray must be  created and deleted through the global functions CreateCKObjectArray and DeleteCKObjectArray
See also: CreateCKObjectArray, DeleteCKObjectArray
*************************************************/
class CKObjectArray{
public :
//----------------------------------------------------
// Return Elements count
	int GetCount();  
	int GetCurrentPos();  

//----------------------------------------------------
// Return current element
	CKObject *GetData(CKContext* context); 
CK_ID	  GetDataId(); 

CK_ID	  SetDataId(CK_ID id); 
CK_ID     SetData(CKObject* obj); 

//----------------------------------------------------
// Reset to start of list 
	void Reset(); 

//----------------------------------------------------
// Seek Functions, modifies the list current position pointer
	CKBOOL PtrSeek(CKObject *); 
	CKBOOL IDSeek(CK_ID id);
	CKBOOL PositionSeek(int Pos);
	CK_ID  Seek(int Pos);

//-----------------------------------------------------
// Iterating
	void Next();
	void Previous();
int GetPosition(CKObject *o);
int GetPosition(CK_ID id);

//------------------------------------------------------
// Find Functions, doesn't modify the list current position
// Check if an object belongs to the list
	CK_ID    PtrFind(CKObject *);
	CK_ID	 IDFind(CK_ID id);
	CK_ID	 PositionFind(int pos);


//----------------------------------------------------
// Insertion
	void InsertFront(CKObject *obj);
	void InsertRear(CKObject *obj);
	void InsertAt(CKObject *obj);
	CKBOOL AddIfNotHere(CKObject *obj);
	CKBOOL AddIfNotHereSorted(CKObject *obj,OBJECTARRAYCMPFCT CmpFct,CKContext* context);

	void InsertFront(CK_ID id);
	void InsertRear(CK_ID id);
	void InsertAt(CK_ID id);
	CKBOOL AddIfNotHere(CK_ID id);
	CKBOOL AddIfNotHereSorted(CK_ID id,OBJECTARRAYCMPFCT CmpFct,CKContext* context);

	CKERROR	Append(CKObjectArray *array);

//----------------------------------------------------
// Deletion
	CK_ID RemoveFront();
	CK_ID RemoveRear();
	CK_ID RemoveAt();
	CKBOOL Remove(CKObject *);
	CKBOOL Remove(CK_ID id);

//---------------------------------------------------
// Clears the list
	void Clear();	

//---------------------------------------------------
// Checks if we are at end of list
	CKBOOL EndOfList(); 
	CKBOOL ListEmpty(); 

	void SwapCurrentWithNext();
	void SwapCurrentWithPrevious();

//---------------------------------------------------
// Checks and removes invalidr references
	CKBOOL Check(CKContext* context); 

//---------------------------------------------------
// Sorting and SortedInsertion (Bubble Sort)
	void Sort(OBJECTARRAYCMPFCT CmpFct,CKContext* context); 
	void InsertSorted(CKObject *o,OBJECTARRAYCMPFCT CmpFct,CKContext* context); 
	void InsertSorted(CK_ID id,OBJECTARRAYCMPFCT CmpFct,CKContext* context); 



	
};



#endif