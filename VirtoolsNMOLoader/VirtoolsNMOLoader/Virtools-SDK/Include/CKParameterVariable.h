/*************************************************************************/
/*	File : CKParameterVariable.h										 */
/*	Author :  Aymeric BARD												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKParameterVariable_H

#define CKParameterVariable_H

#include "CKParameterLocal.h"

/**************************************************************************
Summary: Base class for parameterized access of the variables registered in the
variable manager.
Remarks:

The class id of CKParameter is CKCID_PARAMETERVARIABLE.
See also: CKVariableManager, CKParameterIn
**********************************************************************************/
class CKParameterVariable : public CKParameterLocal {
public :

///
// New Functions

// Method to bind the parameter with a registered variable
CKERROR	Bind(const char* iName);

///
// Virtuals Overriding

	CKERROR			GetValue(void *buf, CKBOOL update = TRUE);
	CKERROR			SetValue(const void *buf,int size = 0);
	CKERROR			CopyValue(CKParameter *param,CKBOOL UpdateParam=TRUE);

//--------------------------------------------
// Data pointer
	void*			GetReadDataPtr(CKBOOL update = TRUE); 
	void*			GetWriteDataPtr(); 
	void			CheckClass(CKParameterTypeDesc* iType);

//--------------------------------------------
// Convertion from / to string
	CKERROR			SetStringValue(CKSTRING Value);
	int  			GetStringValue(CKSTRING Value,CKBOOL update = TRUE);	 

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

	//--------------------------------------------
	// Virtual functions 			  
	CKParameterVariable(CKContext *Context,CKSTRING name = NULL);		
	virtual	~CKParameterVariable();													
	virtual CK_CLASSID GetClassID();										

	virtual CKStateChunk *Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR Load(CKStateChunk *chunk,CKFile* file);					

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
	static CKParameterVariable*	CreateInstance(CKContext *Context);					
	static void			ReleaseInstance(CKContext* iContext,CKParameterVariable*);							
	static CK_CLASSID	m_ClassID;											

	struct PostWrite { // Call the post write
		PostWrite(CKContext* iCtx,const char* iName):name(iName),ctx(iCtx) {}
		~PostWrite() {
			if (name) 
				ctx->GetVariableManager()->WatcherPostWrite(name);
		}
		const char* name;
		CKContext*	ctx;
	};

	CKVariableManager::Variable*	m_Variable;

#endif // docjet secret macro

};

#endif
