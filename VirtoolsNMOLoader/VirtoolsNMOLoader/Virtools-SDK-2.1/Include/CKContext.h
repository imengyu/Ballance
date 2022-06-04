/*************************************************************************/
/*	File : CKContext.h				 				 					 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKCONTEXT_H
#define CKCONTEXT_H "$Id:$"

#include "VxDefines.h"
#include "CKDefines.h"
#include "XObjectArray.h"
#include "XHashTable.h"
#include "CKDependencies.h"

//-------------------------------------------------------------------------
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

typedef XClassArray<CKClassDesc> XClassInfoArray;
typedef XArray<CKBaseManager *> XManagerArray;

//-------- HashTable with CKGUID as Key for Managers and ObjectDeclarations
typedef XHashTable<CKBaseManager *, CKGUID> XManagerHashTable;
typedef XManagerHashTable::Iterator XManagerHashTableIt;
typedef XManagerHashTable::Pair XManagerHashTablePair;

typedef XHashTable<CKObjectDeclaration *, CKGUID> XObjDeclHashTable;
typedef XObjDeclHashTable::Iterator XObjDeclHashTableIt;
typedef XObjDeclHashTable::Pair XObjDeclHashTablePair;

#endif // Docjet secret macro

/***************************************************************************
{filename:CKContext}
Summary: Main Interface Object

Remarks:
    + The CKContext object is the heart of all Virtools based applications, It is the first object that should be created in order to
    use Virtools SDK. A CKContext is created with the global function CKCreateContext()

    + The CKContext object act as the central interface to create/destroy objects,to access managers, to load/save files.

    + Several CKContext can be created inside a same process (in multiple threads for example) but objects created
    by a specific CKContext must not be used in other contextes.


See also: CKContext::CreateObject, CKContext::GetObject, CKContext::DestroyObject
*******************************************************************************/
class CKContext
{
    friend class CKBehavior;

public:
    //------------------------------------------------------
    // Objects Management
    CKObject *CreateObject(CK_CLASSID cid, CKSTRING Name = NULL, CK_OBJECTCREATION_OPTIONS Options = CK_OBJECTCREATION_NONAMECHECK, CK_CREATIONMODE *Res = NULL);
    CKObject *CopyObject(CKObject *src, CKDependencies *Dependencies = NULL, CKSTRING AppendName = NULL, CK_OBJECTCREATION_OPTIONS Options = CK_OBJECTCREATION_NONAMECHECK);
    const XObjectArray &CopyObjects(const XObjectArray &SrcObjects, CKDependencies *Dependencies = NULL, CK_OBJECTCREATION_OPTIONS Options = CK_OBJECTCREATION_NONAMECHECK, CKSTRING AppendName = NULL);

    CKObject *GetObject(CK_ID ObjID);
    int GetObjectCount();
    int GetObjectSize(CKObject *obj);
    CKERROR DestroyObject(CKObject *obj, DWORD Flags = 0, CKDependencies *depoptions = NULL);
    CKERROR DestroyObject(CK_ID id, DWORD Flags = 0, CKDependencies *depoptions = NULL);
    CKERROR DestroyObjects(CK_ID *obj_ids, int Count, CKDWORD Flags = 0, CKDependencies *depoptions = NULL);
    void DestroyAllDynamicObjects(CKScene *iScene = NULL);
    void ChangeObjectDynamic(CKObject *iObject, CKBOOL iSetDynamic = TRUE);

    const XObjectPointerArray &CKFillObjectsUnused();

    // Object Access
    CKObject *GetObjectByName(CKSTRING name, CKObject *previous = NULL);
    CKObject *GetObjectByNameAndClass(CKSTRING name, CK_CLASSID cid, CKObject *previous = NULL);
    CKObject *GetObjectByNameAndParentClass(CKSTRING name, CK_CLASSID pcid, CKObject *previous);
    const XObjectPointerArray &GetObjectListByType(CK_CLASSID cid, CKBOOL derived);
    int GetObjectsCountByClassID(CK_CLASSID cid);
    CK_ID *GetObjectsListByClassID(CK_CLASSID cid);

    //-----------------------------------------------------------
    // Engine runtime
    CKERROR Play();
    CKERROR Pause();
    CKERROR Reset();
    CKBOOL IsPlaying();
    CKBOOL IsInBreak();
    CKBOOL IsReseted();
    // Runtime mode
    CKERROR Process();

    CKERROR ClearAll();
    CKBOOL IsInClearAll();

    //----------------------------------------------------------
    // Current Level&Scene Functions
    CKLevel *GetCurrentLevel();
    CKRenderContext *GetPlayerRenderContext();
    CKScene *GetCurrentScene();
    void SetCurrentLevel(CKLevel *level);

    //----------------------------------------------------------
    // Object Management functions
    CKParameterIn *CreateCKParameterIn(CKSTRING Name, CKParameterType type, CKBOOL Dynamic = FALSE);
    CKParameterIn *CreateCKParameterIn(CKSTRING Name, CKGUID guid, CKBOOL Dynamic = FALSE);
    CKParameterIn *CreateCKParameterIn(CKSTRING Name, CKSTRING TypeName, CKBOOL Dynamic = FALSE);
    CKParameterOut *CreateCKParameterOut(CKSTRING Name, CKParameterType type, CKBOOL Dynamic = FALSE);
    CKParameterOut *CreateCKParameterOut(CKSTRING Name, CKGUID guid, CKBOOL Dynamic = FALSE);
    CKParameterOut *CreateCKParameterOut(CKSTRING Name, CKSTRING TypeName, CKBOOL Dynamic = FALSE);
    CKParameterLocal *CreateCKParameterLocal(CKSTRING Name, CKParameterType type, CKBOOL Dynamic = FALSE);
    CKParameterLocal *CreateCKParameterLocal(CKSTRING Name, CKGUID guid, CKBOOL Dynamic = FALSE);
    CKParameterLocal *CreateCKParameterLocal(CKSTRING Name, CKSTRING TypeName, CKBOOL Dynamic = FALSE);
    CKParameterOperation *CreateCKParameterOperation(CKSTRING Name, CKGUID opguid, CKGUID ResGuid, CKGUID p1Guid, CKGUID p2Guid);
    // CKParameterVariable* CreateCKParameterVariable(CKSTRING Name,CKBOOL Dynamic);

    CKFile *CreateCKFile();
    CKERROR DeleteCKFile(CKFile *);

    //-----------------------------------------------------
    // IHM

    void SetInterfaceMode(CKBOOL mode = TRUE, CKUICALLBACKFCT CallBackFct = NULL, void *data = NULL);
    CKBOOL IsInInterfaceMode();

    // Behaviors and CK can send messages to the console if errors occur
    CKERROR OutputToConsole(CKSTRING str, CKBOOL bBeep = TRUE);
    CKERROR OutputToConsoleEx(CKSTRING format, ...);
    CKERROR OutputToConsoleExBeep(CKSTRING format, ...);
    CKERROR OutputToInfo(CKSTRING format, ...);
    CKERROR RefreshBuildingBlocks(const XArray<CKGUID> &iGuids);

    CKERROR ShowSetup(CK_ID);
    CK_ID ChooseObject(void *dialogParentWnd);
    CKERROR Select(const XObjectArray &o, BOOL clearSelection = TRUE);
    CKDWORD SendInterfaceMessage(CKDWORD reason, CKDWORD param1, CKDWORD param2);

    CKERROR UICopyObjects(const XObjectArray &iObjects, CKBOOL iClearClipboard = TRUE);
    CKERROR UIPasteObjects(const XObjectArray &oObjects);

    //------------------------------------------------------
    // Common Managers functions
    CKRenderManager *GetRenderManager();
    CKBehaviorManager *GetBehaviorManager();
    CKParameterManager *GetParameterManager();
    CKMessageManager *GetMessageManager();
    CKTimeManager *GetTimeManager();
    CKAttributeManager *GetAttributeManager();
    CKPathManager *GetPathManager();
    XManagerHashTableIt GetManagers();
    CKBaseManager *GetManagerByGuid(CKGUID guid);
    CKBaseManager *GetManagerByName(CKSTRING ManagerName);

    int GetManagerCount();
    CKBaseManager *GetManager(int index);

    CKBOOL IsManagerActive(CKBaseManager *bm);
    CKBOOL HasManagerDuplicates(CKBaseManager *bm);
    void ActivateManager(CKBaseManager *bm, CKBOOL active);
    int GetInactiveManagerCount();
    CKBaseManager *GetInactiveManager(int index);

    CKERROR RegisterNewManager(CKBaseManager *manager);

    //---------------------------------------------------------------
    // Profiling functions
    void EnableProfiling(CKBOOL enable);
    CKBOOL IsProfilingEnable();
    void GetProfileStats(CKStats *stats);
    void UserProfileStart(CKDWORD UserSlot);
    float UserProfileEnd(CKDWORD UserSlot);
    float GetLastUserProfileTime(CKDWORD UserSlot);

    //-------------------------------------------------------
    // Utils
    CKGUID GetSecureGuid();
    CKDWORD GetStartOptions();
    WIN_HANDLE GetMainWindow();
    int GetSelectedRenderEngine();

    //----------------------------------------------------------------
    // File Save/Load Options
    void SetCompressionLevel(int level);
    int GetCompressionLevel();

    void SetFileWriteMode(CK_FILE_WRITEMODE mode);
    CK_FILE_WRITEMODE GetFileWriteMode();

    CK_TEXTURE_SAVEOPTIONS GetGlobalImagesSaveOptions();
    void SetGlobalImagesSaveOptions(CK_TEXTURE_SAVEOPTIONS Options);

    CKBitmapProperties *GetGlobalImagesSaveFormat();
    void SetGlobalImagesSaveFormat(CKBitmapProperties *Format);

    CK_SOUND_SAVEOPTIONS GetGlobalSoundsSaveOptions();
    void SetGlobalSoundsSaveOptions(CK_SOUND_SAVEOPTIONS Options);

    //---------------------------------------------------------------
    // Save/Load functions
    CKERROR Load(CKSTRING FileName, CKObjectArray *liste, CK_LOAD_FLAGS LoadFlags = CK_LOAD_DEFAULT, CKGUID *ReaderGuid = NULL);
    CKERROR Load(int BufferSize, void *MemoryBuffer, CKObjectArray *ckarray, CK_LOAD_FLAGS LoadFlags = CK_LOAD_DEFAULT);
    CKSTRING GetLastFileLoaded();

    CKSTRING GetLastCmoLoaded();
    void SetLastCmoLoaded(CKSTRING str);

    CKSTRING GetLastFileSaved();
    void SetLastFileSaved(CKSTRING str);

    CKERROR GetFileInfo(CKSTRING FileName, CKFileInfo *FileInfo);
    CKERROR GetFileInfo(int BufferSize, void *MemoryBuffer, CKFileInfo *FileInfo);
    CKERROR Save(CKSTRING FileName, CKObjectArray *liste, CKDWORD SaveFlags, CKDependencies *dependencies = NULL, CKGUID *ReaderGuid = NULL);
    CKERROR LoadAnimationOnCharacter(CKSTRING FileName, CKObjectArray *liste, CKCharacter *carac, CKGUID *ReaderGuid = NULL, BOOL AsDynamicObjects = FALSE);
    CKERROR LoadAnimationOnCharacter(int BufferSize, void *MemoryBuffer, CKObjectArray *ckarray, CKCharacter *carac, BOOL AsDynamicObjects = FALSE);

    void SetAutomaticLoadMode(CK_LOADMODE GeneralMode, CK_LOADMODE _3DObjectsMode, CK_LOADMODE MeshMode, CK_LOADMODE MatTexturesMode);
    void SetUserLoadCallback(CK_USERLOADCALLBACK fct, void *Arg);
    CK_LOADMODE LoadVerifyObjectUnicity(CKSTRING OldName, CK_CLASSID Cid, const CKSTRING NewName, CKObject **newobj);

    CKBOOL IsInLoad();
    CKBOOL IsInSave();
    CKBOOL IsRunTime();

    //----------------------------------------------------
    //	Render Engine Implementation Specific

    void ExecuteManagersOnPreRender(CKRenderContext *dev);
    void ExecuteManagersOnPostRender(CKRenderContext *dev);
    void ExecuteManagersOnPostSpriteRender(CKRenderContext *dev);

    void AddProfileTime(CK_PROFILE_CATEGORY cat, float time);

    //------- Runtime Debug Mode

    CKERROR ProcessDebugStart(float delta_time = 20.0f);
    CKBOOL ProcessDebugStep();
    CKERROR ProcessDebugEnd();
    CKDebugContext *GetDebugContext();

    void SetVirtoolsVersion(CK_VIRTOOLS_VERSION ver, CKDWORD Build);
    int GetPVInformation();
    CKBOOL IsInDynamicCreationMode();

// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

#endif // Docjet secret macro
};

//-------------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

/*************************************************
Name: CKTimeProfiler
Summary: Class for profiling purposes

See also:
*************************************************/
class CKTimeProfiler
{
public:
    /*************************************************
    Name: VxMultiTimeProfiler
    Summary: Starts profiling
    *************************************************/
    CKTimeProfiler(const char *iTitle, CKContext *iContext, int iStartingCount = 4) : m_Title(iTitle), m_Context(iContext), m_Marks(iStartingCount) {}

    ~CKTimeProfiler()
    {
        char buffer[512];
        Dump(buffer);
        if (strlen(buffer))
            m_Context->OutputToConsoleEx((CKSTRING)"[%s] : %s", m_Title, buffer);
        else
            m_Context->OutputToConsoleEx((CKSTRING)"[%s] : %g", m_Title, m_Profiler.Current());
    }

    /*************************************************
    Summary: Restarts the timer
    *************************************************/
    void Reset()
    {
        m_Profiler.Reset();
        m_Marks.Resize(0);
    }

    /*************************************************
    Summary: add a split time with a name
    *************************************************/
    void operator()(const char *iString)
    {
        Mark m;
        m.name = iString;
        m.time = m_Profiler.Current();
        m_Marks.PushBack(m);
        m_Profiler.Reset();
    }

    /*************************************************
    Summary: Restarts the timer
    *************************************************/
    void Dump(char *oBuffer, char *iSeparator = (char*)" | ")
    {
        char buffer[64];
        *oBuffer = '\0';

        float sum = 0.0f;

        for (XArray<Mark>::Iterator it = m_Marks.Begin(); it != m_Marks.End(); ++it)
        {

            sum += (*it).time;

            sprintf(buffer, "%s = %.3g", (*it).name, (*it).time);
            strcat(oBuffer, buffer);

            if (it != (m_Marks.End() - 1)) // we don't add the separator for the last mark
                strcat(oBuffer, iSeparator);
        }

        if (sum != 0.0f)
        {
            sprintf(buffer, "=> %g ms", sum);
            strcat(oBuffer, buffer);
        }
    }

protected:
    // types
    struct Mark
    {
        const char *name;
        float time;
    };

    VxTimeProfiler m_Profiler;
    const char *m_Title;
    CKContext *m_Context;
    XArray<Mark> m_Marks;
};

#endif

#endif
