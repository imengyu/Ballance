/*************************************************************************/
/*	File : CKParameter.h												 */
/*	Author :  Nicolas Galinotti											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKParameter_H
#define CKParameter_H

#include "CKObject.h"

/**************************************************************************
{filename:CKParameter}
Name: CKParameter

Summary: Base class for output and local parameter

Remarks:

+ The type of the parameter defines the type of the data provided. These types are maintained by
the parameter manager. It defines the size of the buffer to use, and also decides what can be plugged
onto the output parameter. To have the list and definition of predefined parameter types see CKParameterManager.

+ The class id of CKParameter is CKCID_PARAMETER.

See also: CKBehavior, CKParameterIn, CKParameterOperation
**********************************************************************************/
class CKParameter : public CKObject
{
    friend class CKParameterIn;
    friend class CKParameterManager;

public:
    //--------------------------------------------
    // Value

    CKObject *GetValueObject(CKBOOL update = TRUE);

    virtual CKERROR GetValue(void *buf, CKBOOL update = TRUE);
    virtual CKERROR SetValue(const void *buf, int size = 0);
    virtual CKERROR CopyValue(CKParameter *param, CKBOOL UpdateParam = TRUE);

    CKBOOL IsCompatibleWith(CKParameter *param);

    //--------------------------------------------
    // Data pointer
    int GetDataSize();
    virtual void *GetReadDataPtr(CKBOOL update = TRUE);
    virtual void *GetWriteDataPtr();

    //--------------------------------------------
    // Convertion from / to string
    virtual CKERROR SetStringValue(CKSTRING Value);
    virtual int GetStringValue(CKSTRING Value, CKBOOL update = TRUE);

    //--------------------------------------------
    // Type
    CKParameterType GetType();
    void SetType(CKParameterType type);
    CKGUID GetGUID();
    void SetGUID(CKGUID guid);
    CK_CLASSID GetParameterClassID();

    //--------------------------------------------
    // Owner
    virtual void SetOwner(CKObject *o);
    CKObject *GetOwner();

//--------------------------------------------
// Disabled parameters in behaviors

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

    void Enable(CKBOOL act = TRUE);
    CKBOOL IsEnabled();

    //--------------------------------------------
    // Virtual functions
    CKParameter(CKContext *Context, CKSTRING name = NULL);
    virtual ~CKParameter();
    virtual CK_CLASSID GetClassID();

    virtual void PreSave(CKFile *file, CKDWORD flags);
    virtual CKStateChunk *Save(CKFile *file, CKDWORD flags);
    virtual CKERROR Load(CKStateChunk *chunk, CKFile *file);

    virtual void CheckPostDeletion();

    virtual int GetMemoryOccupation();
    virtual int IsObjectUsed(CKObject *o, CK_CLASSID cid);

    //--------------------------------------------
    // Dependencies Functions
    virtual CKERROR PrepareDependencies(CKDependenciesContext &context, CKBOOL iCaller = TRUE);
    virtual CKERROR RemapDependencies(CKDependenciesContext &context);
    virtual CKERROR Copy(CKObject &o, CKDependenciesContext &context);

    //--------------------------------------------
    // Class Registering
    static CKSTRING GetClassName();
    static int GetDependenciesCount(int mode);
    static CKSTRING GetDependencies(int i, int mode);
    static void Register();
    static CKParameter *CreateInstance(CKContext *Context);
    static void ReleaseInstance(CKContext *iContext, CKParameter *);
    static CK_CLASSID m_ClassID;

    // Dynamic Cast method (returns NULL if the object can't be casted)
    static CKParameter *Cast(CKObject *iO)
    {
        return CKIsChildClassOf(iO, CKCID_PARAMETER) ? (CKParameter *)iO : NULL;
    }

    CKERROR CreateDefaultValue();
    void MessageDeleteAfterUse(CKBOOL act);
    CKParameterTypeDesc *GetParameterType() { return m_ParamType; }

    CKBOOL IsCandidateForFixedSize(CKParameterTypeDesc *iType);

protected:
    CKObject *m_Owner;
    CKParameterTypeDesc *m_ParamType;
    int m_Size;

    union
    {
        CKBYTE *m_Buffer;
        CKDWORD m_Value;
    };

    friend void CK_ParameterCopier_Memcpy(CKParameter *Dst, CKParameter *Src);
    friend void CK_ParameterCopier_SaveLoad(CKParameter *Dst, CKParameter *Src);
    friend void CK_ParameterCopier_SetValue(CKParameter *Dst, CKParameter *Src);
    friend void CK_ParameterCopier_Dword(CKParameter *Dst, CKParameter *Src);

    friend void CKObjectArrayRemapfunc(CKParameter *param, CKDependenciesContext &);
    friend void CKObjectRemapFunc(CKParameter *param, CKDependenciesContext &);
    friend void CKStructRemapFunc(CKParameter *param, CKDependenciesContext &);

#endif // docjet secret macro
};

#endif
