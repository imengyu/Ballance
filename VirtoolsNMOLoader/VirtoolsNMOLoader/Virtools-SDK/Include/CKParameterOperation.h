/*************************************************************************/
/*	File : CKParameterOperation.h										 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKPARAMETEROPERATION_H

#define CKPARAMETEROPERATION_H "$Id:$"


#include "CKObject.h"

/****************************************************************************
Name: CKParameterOperation

Summary: Performing operations on parameters

Remarks:
	{Image:ParameterOperation}

	+ An operation encapsulates a C++ function that applies on zero, one or two input parameters,
	and outputs one parameter. Certain operations are predefined in Virtools and you can define your own. 
	The function is called only when necessary (GetValue on the output parameter for example).

	+ All the operation's characteristics are defined by the CKParameterManager, and by
	the GUID of the operation. You can defined your own operation with CKParameterManager::RegisterOperationType
	and your own operation function with CKParameterManager::RegisterOperationFunction.

	+ An operation has zero, one or two input parameters that it owns. These parameters can be plugged into
	output parameters to retrieve their value for example. The output parameter is also owned by the operation,
	and is used to make the result of the operation available to the rest of the graph. One can access
	the parameters with the GetInParameter1, GetInParameter2 and GetOutParameter methods.

	+ The execution of the operation is automatically done when needed. You can force it by using the DoOperation method.
	In any case, the operation checks the update times of the inputs parameters and only does the computation if necessary.

	+ The class id of CKParameterOperation is CKCID_PARAMETEROPERATION.


See also: CKParameterManager, CKParameterOut, CKParameterIn, CKBehaviorPrototype,ParameterOperation Types
*********************************************************************************/
class CKParameterOperation:public CKObject {
friend class CKBehavior;
friend class CKParameterIn;
friend class CKParameterOut;
public :

	/*************************************************
	Summary: Accessing the first input parameter of the operation
	Return Value:
		A pointer to the first input parameter.
	Remarks:
		The parameters of the operation are created with the operation, and are owned by it. These methods
	give access to these parameters.
	See Also: GetInParameter2,GetOutParameter
	*************************************************/
	CKParameterIn* GetInParameter1() {return m_In1;}
	
	/*************************************************
	Summary: Accessing the second input parameter of the operation
	Return Value:
		A pointer to the second input parameter.
  	Remarks:
		The parameters of the operation are created with the operation, and are owned by it. These methods
	give access to these parameters.
	See Also: GetInParameter1,GetOutParameter
	*************************************************/
	CKParameterIn* GetInParameter2() {return m_In2;}

	/*************************************************
	Summary: Accessing the output parameter of the operation
	Return Value:
		A pointer to the output parameter.  
	Remarks:
		The parameters of the operation are created with the operation, and are owned by it. These methods
	give access to these parameters.
	See Also: GetInParameter1,GetInParameter2
	*************************************************/
	CKParameterOut* GetOutParameter() {return m_Out;}

	/*************************************************
	Summary: Returns the owner behavior.
	Return Value:
		A pointer to the CKBehavior that owns this parameter operation.
	Remarks:
		+ The owner is usually the behavior which owns the graph of which the operation is part of.
	  
	See also: CKBehavior::AddParameterOperation	
	*************************************************/
	CKBehavior*	GetOwner() {
		return m_Owner;
	}

	/*************************************************
	Summary: Owner management
	
	Remarks:
		The owner is usually the behavior which owns the graph of which the operation is part of.
	  
	See also: CKBehavior::AddParameterOperation	
	*************************************************/
	void SetOwner(CKBehavior* beh) {m_Owner = beh;}

	//------------------------------------------------
	// Execution
		CKERROR DoOperation();

	//------------------------------------------------
	// Guid
	 CKGUID	GetOperationGuid();
	 void Reconstruct(CKSTRING Name,CKGUID opguid,CKGUID ResGuid, CKGUID p1Guid, CKGUID p2Guid);	
	
//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
	CK_PARAMETEROPERATION GetOperationFunction();	

	//------------------------------------------------
	// Virtual functions 			  
	CKParameterOperation(CKContext *Context,CKSTRING name=NULL);			
	CKParameterOperation(CKContext *Context,CKSTRING name,CKGUID OpGuid,CKGUID ResGuid,CKGUID P1Guid,CKGUID P2Guid); 
	virtual	~CKParameterOperation();											
	virtual CK_CLASSID GetClassID();											

	virtual void PreSave(CKFile *file,CKDWORD flags);							
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);						
	virtual void PostLoad();													

	virtual void PreDelete();													

	virtual int GetMemoryOccupation();											
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);						
	//--------------------------------------------
	// Dependencies Functions  
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);			
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);			

	//--------------------------------------------
	// Class Registering  
	static CKSTRING  GetClassName();											
	static int		 GetDependenciesCount(int mode);							
	static CKSTRING  GetDependencies(int i,int mode);							
	static void		 Register();												
	static CKParameterOperation*	CreateInstance(CKContext *Context);			
	static void		 ReleaseInstance(CKContext* iContext,CKParameterOperation*);							
	static CK_ID	 m_ClassID;													

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKParameterOperation* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_PARAMETEROPERATION)?(CKParameterOperation*)iO:NULL;
	}
	
	protected :
	CKParameterIn*			m_In1;
	CKParameterIn*			m_In2;
	CKParameterOut*			m_Out;		
	CKBehavior*				m_Owner;
	static CKSTRING			m_In1Name;
	static CKSTRING			m_In2Name;
	static CKSTRING			m_OutName;

#endif // docjet secret macro
};

#endif

