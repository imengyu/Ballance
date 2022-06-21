/*************************************************************************/
/*	File : CKParameterIn.h												 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKPARAMETERIN_H

#define CKPARAMETERIN_H

#include "CKObject.h"
#include "CKParameterOut.h"

/**********************************************************************
Name: CKParameterIn

Summary: Input parameters

Remarks:
{Image:ParameterIn}

+ Input parameters allow behaviors to receive values from other behaviors or operations. They have a type defining the type a data they expect.

+ Data is retrieved from an input parameter using the GetReadDataPtr or GetValue methods.

+ An input parameter never owns its data, it always gets it from another parameter,
whether in, out or local (which is a subset of out parameters). So data really is retrieved
from the first output parameter found in the chain of input parameters, named thus the Real Source.
This real source of	data may be accessed through the GetRealSource method.

+ The input paramter may be plugged directly into an output parameter from which it will get its data.
This parameter is thus called a DirectSource. This souce can be set using the SetDirectSource method and
accessed using GetDirectSource method.

+ It may also be plugged into another input parameter coming from the enclosing behavior. This input 
parameter from the enclosing behavior may be shared by many parameters from inside the behavior. 
The method to use to plug the input parameter to such a shared input is ShareSourceWith.

+ The input parameter can only be plugged into a parameter of a compatible type. 

+ A CKParameterIn is created with CKBehavior::CreateInputParameter or CKBehavior::InsertInputParameter.You can also use CKContext::CreateParameterIn though this is probably rare.

+ The class id of CKParameterIn is CKCID_PARAMETERIN.

See also: CKBehavior, CKParameterOut, CKParameterOperation
*************************************************/
class CKParameterIn : public CKObject {
friend class CKParameterManager;
friend class CKParameterOut;
public :

/*******************************************
Summary: Copies the input parameter's value into the given buffer

Remarks:
+Retrieves the value of the input parameter and copies it into the given buffer.
+The buffer must have been allocated to the correct size
+The value is retrieved using the usual system: if data has been modified since 
the last time it was fetched, the chain of inputs is walked up until an output parameter is found

Returns: CK_OK if success

See Also: GetReadDataPtr, GetLastUpdate 
********************************************/
	CKERROR GetValue(void *buf)
	{
		 CKParameter* p = GetRealSource();
		 if (!p) return CKERR_NOTINITIALIZED;
		 return p->GetValue(buf);
	}

/*************************************************
Summary: Retrieves a pointer to the buffer that contain the value.
Returns: A pointer to the data.
Remarks:
+The real pointer to the value is returned. 
+This pointer must not be destroyed.
+Using this method can faster than GetValue especially for big parameters
where a memcpy could be avoided.
See also: GetValue, GetLastUpdate
*************************************************/
	void *GetReadDataPtr()
	{
		CKParameter* p = GetRealSource();
		if (!p) return NULL;
		return p->GetReadDataPtr();
	}
	
	
	/*******************************************
	Summary: returns the shared source of the input parameter.
	
	Return Value:
		A pointer of the shared CKParameterIn if any, NULL otherwise.
	  
	See Also: ShareSourceWith, GetRealSource
	********************************************/
	CKParameterIn* GetSharedSource() 
	{
		return (!(m_ObjectFlags & CK_PARAMETERIN_SHARED))?NULL:m_InShared;		
	}


/*******************************************
Summary: Returns the real source of data of the input parameter
Remarks:

+If the input parameter gets its value from a shared source,
the shared source being an input parameter, it itself gets its value from another source. By walking up
the source chain, the real source is the first parameter that is not an input parameter. This is what
GetRealSource does. It returns the real source from which the input gets its value. This real source
is of course an output parameter.

+If the input parameter has a direct source, the real source is the direct source.
Return Value:
	A pointer to the CKParameter that stores the data for this input parameter.
See Also: ShareSourceWith
********************************************/
	CKParameter* GetRealSource()
	{
		if (m_ObjectFlags & CK_PARAMETERIN_SHARED) {
			if (m_InShared) return m_InShared->GetRealSource();
		} else
			return m_OutSource;

		return NULL;
	}

/*******************************************
Summary: Returns the direct source of the input parameter. 

Remarks:
+ If the input parameter is plugged into an output parameter, from which it will get its data, this output
parameter is called the direct source. 
+ An input parameter must have either a direct source or a shared source, it cannot have both.

Return Value:
	A pointer to the CKParameter that stores the data for this input parameter or NULL
	if has no direct source.

See Also: GetRealSource,SetDirectSource,ShareSourceWith
********************************************/
	CKParameter* GetDirectSource()
	{
		if (m_ObjectFlags & CK_PARAMETERIN_SHARED) return NULL;
		return m_OutSource;
	}

	CKERROR SetDirectSource(CKParameter* param);
	CKERROR ShareSourceWith(CKParameterIn *param);

	//-------------------------------------------------
	// Type of the data
	void SetType(CKParameterType type,CKBOOL UpdateSource=FALSE,CKSTRING NewName=NULL);
	void SetGUID(CKGUID guid,CKBOOL UpdateSource=FALSE,CKSTRING NewName=NULL);

	/*******************************************
	Summary: Returns the type of the input parameter

	Return Value:
		Returns the index of the type of the input parameter. These 
		types are maintained by the parameter manager.

	See Also: CKParameterIn::GetGUID,CKParameterManager
	********************************************/
	CKParameterType GetType() 
	{
		if (m_ParamType) return m_ParamType->Index;
		else return -1;
	}

/*******************************************
Summary: Returns the GUID of the input parameter type.

Return Value:
	Returns the GUID of the type of the input parameter.

See Also: CKParameterIn::GetType,CKParameterManager
********************************************/
	CKGUID GetGUID()
	{
		if (m_ParamType) return m_ParamType->Guid;
		else return CKGUID(0);
	}

/*******************************************
Summary: Sets the owner of the CKParameterIn.

Remarks:
	+ You normally shouldn't change the owner of 
a CKParameterIn this function being automatically by the framework.

See Also: CKParameterIn::GetOwner
********************************************/
	void SetOwner(CKObject *o) {m_Owner = o;}
	
/*******************************************
Summary: Gets the owner of the CKParameterIn.

Remarks:
	+ The owner of the parameter is the object with 
	wich the parameter will be saved. Typically, the owner 
	is a parameter operation of a behavior to which it is an input ( or the internal 
	behavior if the parameter is exported).
Return Value:
	A pointer to the CKObject that owns this input parameter usually a CKBehavior or a CKParameterOperation

See Also: CKParameterIn::SetOwner
********************************************/
	CKObject *GetOwner() {return m_Owner;}

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
	
	CKParameterIn(CKContext *Context,CKSTRING name = NULL,int type = NULL);	
	virtual	~CKParameterIn();												
	virtual CK_CLASSID GetClassID();										

	virtual void PreSave(CKFile *file,CKDWORD flags);						
	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);					

	virtual void PreDelete();												
	virtual void CheckPreDeletion();										

	virtual int GetMemoryOccupation();										
	virtual int IsObjectUsed(CKObject* o,CK_CLASSID cid);					

	//--------------------------------------------
	// Dependencies Functions 
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);		
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);		

	//--------------------------------------------
	// Class Registering
	static CKSTRING		GetClassName();										
	static int			GetDependenciesCount(int mode);						
	static CKSTRING		GetDependencies(int i,int mode);					
	static void			Register();											
	static CKParameterIn*	CreateInstance(CKContext *Context);				
	static void			ReleaseInstance(CKContext* iContext,CKParameterIn*);							
	static CK_CLASSID	m_ClassID;											

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKParameterIn* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_PARAMETERIN)?(CKParameterIn*)iO:NULL;
	}

	void Enable(CKBOOL act=TRUE) { 
		if (!act) m_ObjectFlags |= CK_PARAMETERIN_DISABLED;
		else m_ObjectFlags&=~CK_PARAMETERIN_DISABLED;
	}
	CKBOOL IsEnabled() {return ((m_ObjectFlags & CK_PARAMETERIN_DISABLED)!=CK_PARAMETERIN_DISABLED);}


protected:
	CKObject*			m_Owner;
	union {
	CKParameter*		m_OutSource;
	CKParameterIn*		m_InShared;	
	};
	CKParameterTypeDesc*	m_ParamType;

#endif // docjet secret macro
};

#endif
