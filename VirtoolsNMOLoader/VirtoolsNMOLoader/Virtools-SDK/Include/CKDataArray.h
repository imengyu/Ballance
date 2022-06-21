/*************************************************************************/
/*	File : CKDataArray.h												 */
/*	Author :  Aymeric BARD												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKDATAARRAY_H

#define CKDATAARRAY_H "$Id:$"

#include "CKBeObject.h"


typedef XSArray<CKDWORD> CKDataRow;

typedef CKBOOL (*ArraySortFunction)(CKDataRow*,CKDataRow*);

typedef CKBOOL (*ArrayEqualFunction)(CKDataRow*);


class ColumnFormat {
public:
	ColumnFormat() 
	{
		m_Name			= NULL;
		m_Type			= CKARRAYTYPE_INT;
		m_ParameterType	= CKGUID(0,0);
		m_SortFunction	= NULL;
		m_EqualFunction	= NULL;
	}
	ColumnFormat(const ColumnFormat& c)
	{
		m_Name			= CKStrdup(c.m_Name);
		m_Type			= c.m_Type;
		m_ParameterType	= c.m_ParameterType;
		m_SortFunction	= c.m_SortFunction;
		m_EqualFunction	= c.m_EqualFunction;
	}

	// Column name
	char*				m_Name;
	// Column Type
	CK_ARRAYTYPE		m_Type;
	// Parameter Type
	CKGUID				m_ParameterType;
	// Sort Function
	ArraySortFunction	m_SortFunction;
	// Equal Function
	ArrayEqualFunction	m_EqualFunction;
};


typedef XArray<CKDataRow*>		CKDataMatrix;

typedef XSArray<ColumnFormat*>	CKFormatArray;

/*************************************************
{filename:CKDataArray}
Name: CKDataArray

Summary: Array that contains a collection of data, each column with a unique type of data

Remarks:
+ There exist four basic types of elements you can put in a data array. These types are defined in CK_ARRAYTYPE. 
+ The operations provided with the functions ColumnOperate and ColumnTransform only work with basic numeric types (CKARRAYTYPE_INT and CKARRAYTYPE_FLOAT) and not with a parameter of type "Integer" for example.
+ The comparaison, as used by the functions Sort,FindLine, TestRow, etc... fully works with basic types (CKARRAYTYPE_INT, CKARRAYTYPE_FLOAT and CKARRAYTYPE_STRING) and works with comparaison type EQUAL or NOTEQUAL with object types (CKARRAYTYPE_OBJECT and CKARRAYTYPE_PARAMETER) (it compares the ID of the object and it does a memcmp on the content of the two compared parameter.)
+ The class id of CKDataArray is CKCID_DATAARRAY.


See also: 
*************************************************/
class CKDataArray:public CKBeObject {
public :
// Column/Format Functions
// Insert Column before column cdest (-1 means move at the end)
void			InsertColumn(int cdest,CK_ARRAYTYPE type,char* name,CKGUID paramguid = CKGUID(0,0));
// Move Column csrc before column cdest (-1 means move at the end)
void			MoveColumn(int csrc,int cdest);
// Remove Column
void			RemoveColumn(int c);
// Set Column Name
void			SetColumnName(int c,char* name);
// Get Column Name
char*			GetColumnName(int c);
// Get Column Format
void			SetColumnType(int c,CK_ARRAYTYPE type,CKGUID paramguid = CKGUID(0,0));
// Get Column Format
CK_ARRAYTYPE	GetColumnType(int c);
// Get Column Format
CKGUID			GetColumnParameterGuid(int c);
// Get Key Column
int				GetKeyColumn();
// Set Key Column
void			SetKeyColumn(int c);
// Get Column Number
int				GetColumnCount();

// Elements Functions

// Get the elemnt pointer of the specified case
CKDWORD*	GetElement(int i,int c);
// Use to get an int, a float, a string or an object ID
CKBOOL		GetElementValue(int i,int c,void* value);

CKBOOL		GetElementValue_VSL(int i,int c,void* value);
// Use to get an CKObject
CKObject*	GetElementObject(int i,int c);

// Use to set an int, a float ,an object ID, a string 
CKBOOL		SetElementValue(int i,int c,void* value,int size = 0);

CKBOOL		SetElementValue_VSL(int i,int c,void* value,int size = 0);
// Use to set a value from a parameter
CKBOOL		SetElementValueFromParameter(int i,int c,CKParameter* pout);
// Use to set a value from a parameter
CKBOOL		SetElementValueFromValue(int i,int c,void* value,int type);
// Use to set an CKObject
CKBOOL		SetElementObject(int i,int c,CKObject* object);

// Parameters Shortcuts

// Paste a shortcut of a parameter on an existing compatible parameter
CKBOOL			PasteShortcut(int i,int c,CKParameter* pout);
// Remove a shortcut parameter (if it exists)
CKParameterOut* RemoveShortcut(int i,int c);

// String Value

// Set the value of an existing element
CKBOOL			SetElementStringValue(int i,int c,char* svalue);
// Set the value of an existing element
int				GetStringValue(CKDWORD key,int c,char* svalue);
// Set the value of an existing element
int				GetElementStringValue(int i,int c,char* svalue);

// Load / Write

// load elements into an array from a formatted file
CKBOOL			LoadElements(CKSTRING string,CKBOOL append,int column);
// write elements from an array to a file
CKBOOL			WriteElements(CKSTRING string,int column,int number,CKBOOL iAppend = FALSE);

// Rows Functions
// Get row Count
int				GetRowCount();
// Find the nth line
CKDataRow*		GetRow(int n);
// adds a row
void			AddRow();
// Insert a Row before another, -1 means after everything
CKDataRow*		InsertRow(int n=-1);
// Test a row on a column
CKBOOL			TestRow(int row,int c,CK_COMPOPERATOR op,CKDWORD key,int size=0);
// Find a line with the current key : search in the column c and return the line index (-1) if none
int				FindRowIndex(int c,CK_COMPOPERATOR op,CKDWORD key,int size=0,int startingindex=0);
// Find a line with the current key : search in the column c and return the line itself (NULL if none)
CKDataRow*		FindRow(int c,CK_COMPOPERATOR op,CKDWORD key,int size=0,int startindex=0);
// Remove the nth line
void			RemoveRow(int n);
// Move a row
void			MoveRow(int rsrc,int rdst);
// Swap 2 rows
void			SwapRows(int i1,int i2);
// Clear the entire array
void			Clear(CKBOOL Params=TRUE);


///////////////////////////
// Algorithm
///////////////////////////

// Find the highest value in the given column
CKBOOL GetHighest(int c,int& row);
// Find the lowest value in the given column
CKBOOL GetLowest(int c,int& row);
// Find the nearest value in the given column
CKBOOL GetNearest(int c,void* value,int& row);
// Transform the values by operating them with the given value
void ColumnTransform(int c,CK_BINARYOPERATOR op,CKDWORD value);
// Operate two colums into a third
void ColumnsOperate(int c1,CK_BINARYOPERATOR op,int c2,int cr);

// Sort the array on the column, ascending or descending
void Sort(int c,CKBOOL ascending);
// Remove the elements identical in the array
void Unique(int c);
// Shuffle the array
void RandomShuffle();
// Reverse the array
void Reverse();

// Get the sum of a specific column
CKDWORD Sum(int c);
// Get The product of a specific column
CKDWORD Product(int c);

// Get the count of elements verifying a condition (operator)
int GetCount(int c,CK_COMPOPERATOR op,CKDWORD key,int size=0);
// Create a group from elements matching a value
 void CreateGroup(int mc,CK_COMPOPERATOR op,CKDWORD key,int size,CKGroup* group,int ec=0);


//-------------------------------------------------------------------------
// Internal functions 
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

	//-------------------------------------------------------
	// Virtual functions	{Secret}
	CKDataArray(CKContext *Context,CKSTRING Name=NULL);							
	virtual ~CKDataArray();														
	virtual CK_CLASSID GetClassID();											

	virtual void PreSave(CKFile *file,CKDWORD flags);							
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);						
	virtual void PostLoad();													

	virtual void CheckPreDeletion();											
	virtual void CheckPostDeletion();											

	virtual int GetMemoryOccupation();											
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);						

	//--------------------------------------------
	// Dependencies Functions	{Secret}
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);			
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);			

	//--------------------------------------------
	// Class Registering	{Secret}
	static CKSTRING  GetClassName();											
	static int		 GetDependenciesCount(int mode);							
	static CKSTRING  GetDependencies(int i,int mode);							
	static void		 Register();												
	static CKDataArray* CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKDataArray*);							
	static  CK_ID	 m_ClassID;													

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKDataArray* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_DATAARRAY)?(CKDataArray*)iO:NULL;
	}
	//--------------------------------------------
	
#endif // Docjet secret macro

};

#endif

