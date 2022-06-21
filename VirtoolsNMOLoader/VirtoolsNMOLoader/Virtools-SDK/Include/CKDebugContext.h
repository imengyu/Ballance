/*************************************************************************/
/*	File : CKDebugContext.h			 			 						 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKDEBUGCONTEXT_H

#define CKDEBUGCONTEXT_H "$Id:$"

#include "CKObjectArray.h"


typedef enum CKDEBUG_STATE { 	
	CKDEBUG_NOP						= 0x00000000,
	CKDEBUG_BEHEXECUTE				= 0x00000001,
	CKDEBUG_BEHEXECUTEDONE			= 0x00000002,
	CKDEBUG_SCRIPTEXECUTEDONE		= 0x00000004,
} CKDEBUG_STATE;



class CKDebugContext {

public:
	float delta;

	CKBeObject* CurrentObject;
	CKBehavior* CurrentScript;
	CKBehavior* CurrentBehavior;
	CKBehavior* SubBehavior;
	CKObjectArray ObjectsToExecute;
	CKObjectArray ScriptsToExecute;
	CKObjectArray BehaviorStack;  
	CKContext*	  m_Context;	

// Behavior Part 
	CKDEBUG_STATE CurrentBehaviorAction;
	CKBOOL InDebug;

	void Init(XObjectPointerArray& array,float delta);
	void StepInto(CKBehavior *beh);
	void StepBehavior();
	CKBOOL DebugStep();
	void Clear();

	CKDebugContext(CKContext* context) 
	{
		m_Context=context;
		delta=0;
		CurrentObject=0;
		CurrentScript=0;
		CurrentBehavior=0;
		SubBehavior=0;

		InDebug = 0;
		CurrentBehaviorAction = CKDEBUG_NOP;
	}

};

#endif