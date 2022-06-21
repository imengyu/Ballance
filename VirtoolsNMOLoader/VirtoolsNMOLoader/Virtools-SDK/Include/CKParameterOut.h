/*************************************************************************/
/*	File : CKParameterOut.h												 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKPARAMETEROUT_H

#define CKPARAMETEROUT_H

#include "CKParameter.h"

/**************************************************************************
Name: CKParameterOut

Summary: Output parameter providing a value

Remarks:
	{Image:ParameterOut}

	+ The type of the parameter defines the type of the data provided. These types are maintained by
the parameter manager. It defines the size of the buffer to use, and also decides what can be plugged
onto the output parameter. To have the list and definition of predefined parameter types see CKParameterManager.

	+ An output parameter may have destinations to which it pushes the data each time it is changed.
These destinations are other output parameters, which for example provide their value out of the enclosing
behavior, or local parameters which provide values to other parts of the graph of sub-behaviors.
These destinations are managed using the AddDestination and related methods.
When the data of the output parameter changes, it pushes the new value down to 
its destinations.

	+ An output parameter will probably also be plugged into input parameters. These input parameters will pull the
value from the output parameter when needed.

	+ An output parameter usually knows how to write its data from and to a string. This is useful for display
and debugging purposes. When you define a new type of parameter, you can specify the function that converts
to and from strings. 

	+ An output parameter can also have an edition window. When you define a new type of parameter, you can specify the 
function that will create the edition window when needed by the interface.

	+ A CKParameterOut is created with CKBehavior::CreateOutputParameter or CKContext::CreateCKParameterOut.

	+ The class id of CKParameterOut is CKCID_PARAMETEROUT.


See also: CKBehavior, CKParameterIn, CKParameterOperation
**********************************************************************************/
class CKParameterOut : public CKParameter {
friend class CKParameterIn;
friend class CKParameter;
friend class CKParameterFS;
friend class CKParameterLocalFS;
friend class CKParameterManager;
public :
//--------------------------------------------
// Value
	
virtual CKERROR				GetValue(void *buf, CKBOOL update = TRUE);
virtual CKERROR				SetValue(const void *buf,int size = 0);
virtual	CKERROR				CopyValue(CKParameter *param,CKBOOL UpdateParam=TRUE);
virtual	void*				GetReadDataPtr(CKBOOL update = TRUE); 
virtual	int  				GetStringValue(CKSTRING Value,CKBOOL update = TRUE);	 

		void				CheckClass(CKParameterTypeDesc* iType); 

//--------------------------------------------
// Destinations 

void			DataChanged(); 
CKERROR			AddDestination(CKParameter* param,CKBOOL CheckType=TRUE);
void			RemoveDestination(CKParameter* param);
int				GetDestinationCount();
CKParameter*	GetDestination(int pos);
void			RemoveAllDestinations();

//-------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // Docjet secret macro
#else

//--------------------------------------------
// Virtual functions 		  
	CKParameterOut(CKContext *Context,CKSTRING name = NULL);
	virtual	~CKParameterOut();												
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
	static CKParameterOut*	CreateInstance(CKContext *Context);				
	static void			ReleaseInstance(CKContext* iContext,CKParameterOut*);							
	static CK_CLASSID	m_ClassID;											

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKParameterOut* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_PARAMETEROUT)?(CKParameterOut*)iO:NULL;
	}
	
	void Update();
	
protected:

	XSObjectPointerArray	m_Destinations;
#endif // docjet secret macro
};

#endif
