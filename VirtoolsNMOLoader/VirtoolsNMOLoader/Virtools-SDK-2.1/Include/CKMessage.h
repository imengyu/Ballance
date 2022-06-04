/*************************************************************************/
/*	File : CKMessage.h													 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKMESSAGE_H
#define CKMESSAGE_H "$Id:$"

#include "CKObject.h"

typedef enum CK_MESSAGE_SENDINGTYPE
{
    CK_MESSAGE_BROADCAST = 1,	// Send message to all objects of a specific class ID
    CK_MESSAGE_SINGLE    = 2,	// Send message to a single object
    CK_MESSAGE_GROUP     = 3,	// Send message to an object and its dependencies (CKCharacter and CKGroup)
} CK_MESSAGE_SENDINGTYPE;

/*************************************************
{filename:CKMessage}
Summary: Messages between behaviors and objects


Remarks:
+ Important : CKMessage DOES NOT derive from CKObject... !

+ Instances of CKMessage can be sent to objects or to behaviors. Behaviors input or outputs
can be automatically activated when messages are dispatched.

+ This class is to be used in cooperation with the CKMessageManager. Output parameters can be attached to messages in order to provide
additional values to the recipients. Messages can be sent to a single recipient, to a list of recipients
or to all instances of a class hierarchy.

+ Messages are versatile: They are often created automatically by the SendMessage.. functions and
destroyed at the end of the next frame. DO NOT KEEP pointers to CKMessage.

+ Most of the CKMessage methods (type,sender,recipient,sending type) are automatically called
when creating a message with the SendMessage methods of the CKMessageManager

See also: CKMessageManager, CKMessageManager::SendMessageSingle, CKMessageManager::SendMessageGroup, CKMessageManager::SendMessageBroadcast
***********************************************************************/
class CKMessage
{
    friend class CKMessageManager;

public:
    //--------------------------------------------------------------
    // In case of typed broadcast, type of objects to send the message to
    CKERROR SetBroadcastObjectType(CK_CLASSID type = CKCID_BEOBJECT);

    //--------------------------------------------------------------
    // Associated Parameters
    CKERROR AddParameter(CKParameter *, CKBOOL DeleteParameterWithMessage = FALSE);
    CKERROR RemoveParameter(CKParameter *);
    int GetParameterCount();
    CKParameter *GetParameter(int pos);

    //---------------------------------------------------------
    // Sender

    /*******************************************
    Summary: Specify the sender of this message

    Arguments:
        obj: Sender of this message (Optionnal).
    See Also:GetSender
    ********************************************/
    void SetSender(CKBeObject *obj) { m_Sender = CKOBJID(obj); }
    /*******************************************
    Summary: Returns the sender of this message
    Return Value:
        A pointer to the CKBeObject that sends this message.

    See Also:SetSender
    ********************************************/
    CKBeObject *GetSender() { return (CKBeObject *)m_Context->GetObject(m_Sender); }

    //--------------------------------------------------------------
    // In case of single sending
    /*************************************************
    Summary: Returns the recipient of this message.

    Remarks:
        + The recipient is not taken into account when using a broadcast message.
    See Also: GetSendingType, SetRecipient, SetBroadcastObjectType
    *************************************************/
    void SetRecipient(CKObject *recipient) { m_Recipient = CKOBJID(recipient); }

    /*************************************************
    Summary: Returns the recipient of this message.

    Return Value:
        A pointer to the CKObject that is recipientof this message.
    Remarks:
    The recipient is not taken into account when using a broadcast message.
    See Also: GetSendingType, SetRecipient, SetBroadcastObjectType
    *************************************************/
    CKObject *GetRecipient() { return m_Context->GetObject(m_Recipient); }

    //-------------------------------------------------------------
    // Sending Type
    // (set according to the function used for the recipient(s))

    /*******************************************
    Summary: Returns the sending type of the message

    Return Value:
        Sending type.
    Remarks:
    The sending type refers to the kind of destinations of the message. The sending types are:

    - CK_MESSAGE_BROADCAST: sent to all instances of a class hierarchy
    - CK_MESSAGE_SINGLE: sent to a single recipient
    - CK_MESSAGE_GROUP: sent to a single recipient and its dependencies (CKGroup and CKCharacter only)

    See Also: SetSendingType,GetRecipient, SetRecipient
    ********************************************/
    CK_MESSAGE_SENDINGTYPE GetSendingType() { return m_SendingType; }
    /*******************************************
    Summary: Sets the sending type of the message

    Remarks:
        + The sending type refers to the kind of destinations of the message. The sending types are:
                    + CK_MESSAGE_BROADCAST: sent to all instances of a class hierarchy
                    + CK_MESSAGE_SINGLE: sent to a single recipient
                    + CK_MESSAGE_GROUP: sent to a single recipient and its dependencies (CKGroup and CKCharacter only)
        + This method is automatically called by CKMessageManager::SendMessage.. methods.
    See Also: GetSendingType,GetRecipient, SetRecipient
    ********************************************/
    void SetSendingType(CK_MESSAGE_SENDINGTYPE Type) { m_SendingType = Type; }

    /*************************************************************************
    Summary: Sets the type of this message.

    Remarks:
        + Each message has a type("On Click",etc..) , which corresponds to an index in a table
        maintained by the message manager. This type is used as a filter for dispatching
        messages.
     See Also: GetMsgType,CKMessageManager::AddMessageType
    *********************************************************************************/
    void SetMsgType(CKMessageType type) { m_MessageType = type; }

    /*************************************************************************
    Summary: Returns the type of this message.

    Return Value:
        Message Type.
    Remarks:
    Each message has a type("On Click",etc..) , which corresponds to an index in a table
    maintained by the message manager. This type is used as a filter for dispatching
    messages.
    See Also: SetMsgType,CKMessageManager::AddMessageType
    *********************************************************************************/
    CKMessageType GetMsgType() { return m_MessageType; }

    //----------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    // Internal functions
    //-------------------------------------------------------------------------
    //----------------------------------------------------------------------------

    CKMessage();
    ~CKMessage();
    int AddRef();
    int Release();

protected:
    int m_RefCount;
    CKMessageType m_MessageType;
    XObjectArray *m_Parameters;
    CK_MESSAGE_SENDINGTYPE m_SendingType;
    CK_ID m_Sender;
    union
    {
        CK_CLASSID m_BroadcastCid;
        CK_ID m_Recipient;
    };
    CKContext *m_Context;
};

#endif
