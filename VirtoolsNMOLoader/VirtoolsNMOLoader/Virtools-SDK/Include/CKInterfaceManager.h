/*************************************************************************/
/*	File : CKInterfaceManager.h											*/
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKINTERFACEMANAGER_H
#define CKINTERFACEMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKBaseManager.h"


class CKInterfaceManager:public CKBaseManager {

public:
		virtual CK_PARAMETERUICREATORFUNCTION	GetEditorFunctionForParameterType(CKParameterTypeDesc* param);
		virtual	int			CallBehaviorEditionFunction(CKBehavior* beh,void* arg);
		virtual	int			CallBehaviorSettingsEditionFunction(CKBehavior* beh,void* arg);
		virtual	int			CallEditionFunction(CK_CLASSID id,void * arg);
		virtual	int			DoRenameDialog(char* Name,CK_CLASSID cid);
		virtual ~CKInterfaceManager();
		CKInterfaceManager(CKContext *Context);
		
		virtual CKERROR OnCKInit();
		virtual CKDWORD	GetValidFunctionsMask()	{ return CKMANAGER_FUNC_OnCKInit; }

		CK_CLASSID			m_TheCid;			// Used as argument for rename dialog {secret}
		char				m_TheName[128];		// Used as argument for rename dialog {secret}
};


#endif