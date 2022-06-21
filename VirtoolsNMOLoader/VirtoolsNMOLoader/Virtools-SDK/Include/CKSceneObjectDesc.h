/*************************************************************************/
/*	File : CKSceneObjectDesc.h											 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSCENEOBJECTDESC_H
#define CKSCENEOBJECTDESC_H "$Id:$"

#include "CKObject.h"

/////////////////////////////////////////////////////
// All private


class CKSceneObjectDesc {

public :
	CKSceneObjectDesc() { m_Object=0; m_InitialValue=NULL; m_Global=0; };
/////////////////////////////////////////
// Virtual functions				  	
	
	CKERROR ReadState(CKStateChunk *chunk);
	int GetSize();
	void Clear();
	void Init (CKObject *obj=NULL);
	
	BOOL ActiveAtStart() { return m_Flags & CK_SCENEOBJECT_START_ACTIVATE; }
	BOOL DeActiveAtStart() { return m_Flags & CK_SCENEOBJECT_START_DEACTIVATE; }
	BOOL NothingAtStart() { return m_Flags & CK_SCENEOBJECT_START_LEAVE; }
	BOOL ResetAtStart() { return m_Flags & CK_SCENEOBJECT_START_RESET; }
	BOOL IsActive()		{ return m_Flags & CK_SCENEOBJECT_ACTIVE; }

public :
	CK_ID			m_Object;
	CKStateChunk*	m_InitialValue;
	union {
		DWORD		m_Global;
		DWORD		m_Flags;
	  };
};

#endif
