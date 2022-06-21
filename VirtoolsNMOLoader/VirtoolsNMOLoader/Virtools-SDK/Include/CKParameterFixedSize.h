
/*************************************************************************/
/*	File : CKParameterFixedSize.h										 */
/*	Author :  Aymeric BARD												 */	
/*																		 */
/*    This file contains all the declarations of the Fixed size parameter*/
/*  classes, used for memory footprint reductions reasons.				 */	
/*																		 */
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CK_PARAMETERFIXEDSIZE_H

#define CK_PARAMETERFIXEDSIZE_H "$Id:$"

#include "CKParameter.h"
#include "CKParameterLocal.h"
#include "CKParameterOut.h"



class CKParameterFS : public CKParameter 
{
public:
	// virtual destructor
	virtual	~CKParameterFS();
	
	// CKParameter virtual surdefinitions
	virtual CKERROR			GetValue(void* oBuffer, CKBOOL iUpdate = TRUE);
	virtual	CKERROR			SetValue(const void* iBuffer, int iSize = 0);
	virtual	CKERROR			CopyValue(CKParameter* iParam, CKBOOL iUpdateParam=TRUE);
	virtual	void*			GetReadDataPtr(CKBOOL iUpdate = TRUE); 
	virtual	void*			GetWriteDataPtr(); 

	static	void			ApplyVTable(CKParameter& iParam, CKBOOL iFixedSize);
	
private:
	CKParameterFS():CKParameter(NULL) {} // No Ctor provided
};


class CKParameterLocalFS : public CKParameterLocal
{
public:
	// virtual destructor
	virtual	~CKParameterLocalFS();
	
	// CKParameter Local virtual surdefinitions
	virtual CKERROR			GetValue(void* oBuffer, CKBOOL iUpdate = TRUE);
	virtual	CKERROR			SetValue(const void* iBuffer, int iSize = 0);
	virtual	CKERROR			CopyValue(CKParameter* iParam, CKBOOL iUpdateParam=TRUE);
	virtual	void*			GetReadDataPtr(CKBOOL iUpdate = TRUE); 
	virtual	void*			GetWriteDataPtr(); 

	static	void			ApplyVTable(CKParameterLocal& iParam, CKBOOL iFixedSize);
	
private:
	CKParameterLocalFS():CKParameterLocal(NULL) {} // No Ctor provided
};


class CKParameterOutFS : public CKParameterOut
{
public:
	// virtual destructor
	virtual	~CKParameterOutFS();
	
	// CKParameter Local virtual surdefinitions
	virtual CKERROR			GetValue(void* oBuffer, CKBOOL iUpdate = TRUE);
	virtual	CKERROR			SetValue(const void* iBuffer, int iSize = 0);
	virtual	CKERROR			CopyValue(CKParameter* iParam, CKBOOL iUpdateParam=TRUE);
	virtual	void*			GetReadDataPtr(CKBOOL iUpdate = TRUE); 
	virtual	void*			GetWriteDataPtr(); 

	static	void			ApplyVTable(CKParameterOut& iParam, CKBOOL iFixedSize);
	
private:
	CKParameterOutFS():CKParameterOut(NULL) {} // No Ctor provided
};

#endif

