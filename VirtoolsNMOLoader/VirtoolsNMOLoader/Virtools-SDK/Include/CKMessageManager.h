/*************************************************************************/
/*	File : CKMessageManager.h											 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 1999, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKMESSAGEMANAGER_H

#define CKMESSAGEMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKFile.h"
#include "CKBaseManager.h"


/*

struct CKMessageWaitingList;

struct CKMessageDesc;
*/


struct CKWaitingObject;

typedef XArray<CKWaitingObject> CKWaitingObjectArray;

typedef XClassArray<XString>	CKStringArray;

/****************************************************************************
Name: CKMessageHandler

Summary: A message handler used to override CKMessageManager::SendMessage
         behavior.

Remarks:

+ A message handler taking a CKMessage and returning a XBOOL
+ If no handler is set, the message manager will behave normally
+ If a handler is set, the CKMessageManager::SendMessage will call it,
  unless specified otherwise
+ If the handler returns true, the message won't be processed by SendMessage

See also: CKContext::GetMessageManager,CKMessage,CKMessageManager
****************************************************************************/
typedef XBOOL (CKMessageHandler)(CKMessage* msg);


/****************************************************************************
Name: CKMessageManager

Summary: Messages management

Remarks:

+ There is only one instance of the message manager per application. This instance receives
and dispatches the messages to the objects or the behaviors that have requested it. It also
manages a list of message types.

+ Behaviors that wish to wait for a message should register themselves using
the RegisterWait method specifying the input to activate when the message will be received.
They will then receive the messages sent with the SendMessage method. After the first message
received, they will be automatically unregistered. They can also unregister themselves 
at any time using the unregisterWait methods.

+ Another way to proceed is to specify that an object accept all messages using CKBeObject::SetAsWaitingForMessages.
This way all messages sent to an object will be stored for one frame. Behaviors may then
ask the object about how many and which messages did he received last frame.

+ Messages have a type. The list of accepted types is maintained by the message manager. If an object
or a behavior registers itself as waiting for a new type of message, this type is automatically added
to the list of types. One can also add a new type manually using the AddMessageType method. The list of types
can be accessed with the GetMessageTypeCount and GetMessageTypeName methods. In the registration methods,
the type can be referred to by its name, or for optimization reasons, by its number.

+ When the SendMessage method is called, the given message is stacked by the message manager. When the Process method 
of the message manager is called, it unstacks the messages and dispatches them to the behaviors or objects, and then
unregisters them. The messages to behaviors activate the provided input or output. The messages to the object
are stacked in the object's "last frame messages" list.

+ The unique instance of CKMessageManager is accessed through the CKContext::GetMessageManager function.


See also: CKContext::GetMessageManager,CKMessage,CKMessageHandler
****************************************************************************/
class CKMessageManager:public CKBaseManager {
friend class CKMessage;
public :

//---------------------------------------------
// Message Types 
CKMessageType AddMessageType(CKSTRING MsgName);
CKSTRING GetMessageTypeName(CKMessageType MsgType);
int	 GetMessageTypeCount();
void  RenameMessageType(CKMessageType MsgType,CKSTRING NewName);
void  RenameMessageType(CKSTRING OldName,CKSTRING NewName);

//---------------------------------------------
// register incoming message
CKERROR		SendMessage				(CKMessage *msg, XBOOL useHandler=true );
CKMessage* 	SendMessageSingle		(CKMessageType MsgType,CKBeObject *dest,CKBeObject *sender=NULL);			
CKMessage* 	SendMessageGroup		(CKMessageType MsgType,CKGroup* group,CKBeObject *sender=NULL);			
CKMessage* 	SendMessageBroadcast	(CKMessageType MsgType,CK_CLASSID id=CKCID_BEOBJECT,CKBeObject *sender=NULL);			

//----------------------------------------------
// Waiting behaviors
CKERROR RegisterWait(CKMessageType MsgType, CKBehavior *behav, int OutputToActivate,CKBeObject *obj);
CKERROR RegisterWait(CKSTRING MsgName, CKBehavior *behav, int OutputToActivate,CKBeObject *obj);
CKERROR UnRegisterWait(CKMessageType MsgType, CKBehavior *behav, int OutputToActivate);
CKERROR UnRegisterWait(CKSTRING MsgName, CKBehavior *behav, int OutputToActivate);
CKERROR RegisterDefaultMessages();

//----------------------------------------------
// Message handler
CKMessageHandler* SetMessageHandler( CKMessageHandler* msgHandler );

//-------------------------------------------------------------------
//-------------------------------------------------------------------
// Internal functions 
//-------------------------------------------------------------------
//-------------------------------------------------------------------

virtual CKStateChunk* SaveData(CKFile* SavedFile);					
virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile);	
virtual CKERROR PreClearAll();										
virtual CKERROR PostProcess();										
virtual CKERROR OnCKReset();						 				
virtual CKERROR SequenceToBeDeleted(CK_ID *objids,int count);		


virtual CKDWORD	GetValidFunctionsMask()	{ return CKMANAGER_FUNC_PreClearAll|	
												 CKMANAGER_FUNC_PostProcess|
												 CKMANAGER_FUNC_OnCKReset|
												 CKMANAGER_FUNC_OnSequenceToBeDeleted; }

	
	virtual ~CKMessageManager();
	
	CKMessageManager(CKContext *Context);
protected :
	void AddMessageToObject(CKObject* obj,CKMessage* msg,CKScene* currentscene,BOOL recurse);

	CKMessage*				CreateMessage();

	CKStringArray			m_RegistredMessageTypes;
	CKWaitingObjectArray**	m_MsgWaitingList;
	XArray<CKMessage*>		m_ReceivedMsgThisFrame;
	XObjectPointerArray		m_LastFrameObjects;
	XObjectPool<CKMessage>	m_MessagePool;
    CKMessageHandler*       m_MessageHandler;
};

#endif

