/*************************************************************************/
/*	File : CKInterfaceObjectManager.h									 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Base Class for Virtools Managers									 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKINTERFACE_H
#define CKINTERFACE_H "$Id:$"

#include "CKObject.h"

class CKInterfaceObjectManager : public CKObject
{
public:
    void SetGuid(CKGUID guid);
    CKGUID GetGuid();

    ////////////////////////////////////////
    // Datas
    void AddStateChunk(CKStateChunk *chunk);
    void RemoveStateChunk(CKStateChunk *chunk);
    int GetChunkCount();
    CKStateChunk *GetChunk(int pos);

    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    // Internal functions
    //-------------------------------------------------------------------
    //-------------------------------------------------------------------
    virtual CK_CLASSID GetClassID();
    virtual ~CKInterfaceObjectManager();

    //--------------------------------------------
    // Class Registering {secret}
    static CKSTRING GetClassName();
    static int GetDependenciesCount(int mode);
    static CKSTRING GetDependencies(int i, int mode);
    static void Register();
    static CKInterfaceObjectManager *CreateInstance(CKContext *Context);
    static void ReleaseInstance(CKContext *iContext, CKInterfaceObjectManager *);
    static CK_ID m_ClassID;
};

#endif
