/*************************************************************************/
/*	File : CKBehavior.h			 				 						 */
/*	Author :  Nicolas Galinotti											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKBEHAVIOR_H
#define CKBEHAVIOR_H "$Id:$"

#include "CKObject.h"
#include "XObjectArray.h"
#include "XPriorityQueue.h"

struct BehaviorBlockData;
struct BehaviorGraphData;

/**************************************************************************
{filename:CKBehavior}
Name: CKBehavior

Summary: Class for defining behaviors

Remarks:
    + The CKBehavior class provides functionnalities for defining a behavior as a function or as a graph,
    to set various parameters about how and when the behavior is activated,
    to link them to other behaviors through CKBehaviorLink instances or to create
    them from CKBehaviorPrototype, for creating CKBehaviorPrototype from a behavior.

    + Each behavior has an owner, which is an instance of CKBeObject. The owner saves its
    behaviors with itself when saved into a file, or with CKSaveObjectState. The behavior
    usually applies to its owner, or to parts of its owner, though it can apply to other
    objects as well. If the behavior is a graph, all of its sub-behaviors inherits the same owner.

    + A Behavior is considered to as a box with inputs/ouputs and input,output and local parameters.
    Inside this box the behavior may be implemented using a graph of sub-behaviors or using a C++ function,
    depending on which method UseGraph or UseFunction has been specified. For behaviors using a function,
    the function may be defined internally, or in a external code file (dll, or other depending on the OS).

    + If you want to implement the behavior with a C++ funtion without defining a prototype, you should specify the function with the
    SetFunction() method. This function will be called at each process cycle, with one argument:
    the elapsed time since the last call, in milliseconds.

    + Behaviors are real time pieces of algorithm. When a behavior is active and enabled or is waiting for a message, it gets
    called in an iterative fashion at each loop of the process cycle. Each time it is called, the behavior
    does the minimum amount of processing depending on the time elapsed since the last time it was called.
    No behavior should hold on the CPU more than necessary, as each other active behaviors have to be called
    in the process cycle.

    + Behavior execution is managed by its owner, and by the scene to which its owner belongs.
    A CKBeObject instance is declared in its scene as being active and/or enabled with the CKScene::SetObjectFlags or CKScene::Activate method.
    A disabled object's behaviors are never called. Setting an object as active has the effect of having
    all of its behaviors called during the next process cycle. If you add an object to a scene and
    want its behaviors to be called when the scene is started with CKLevel::LaunchScene, you should
    set the flags accordingly with CKScene::SetObjectFlags.
    If you don't use Scene or have only one scene (the level) setting the flags is not necessary.

    + If the behavior is implemented using an execution function, this function decides whether the
    behavior should remain active or not. This is done with the return value of the
    function. If the return value is CKBR_OK, the behavior is considered as being over and is set to be inactive
    at the next process cycle. It may become active again at a later time, if one of its entries gets
    activated.

    + If the behavior is implemented as a graph of sub-behaviors, it is considered active if at least one of its sub-behaviors is active, and
    will be called during the next process cycle.

    + Behavior prototypes are a sort of model of behaviors. One can create a behavior from a prototype.
    Prototypes are typically used in external pieces of code like dlls.	Prototypes are typically used
    for storing reusable behaviors.

    + Behaviors usually have inputs and outputs, which may be active or inactive. The inputs and outputs have
    no value, they are like switches which may be on or off. A behavior is activated if at least one of its entries is
    active. When all of the entries are inactive and all of its sub-behaviors are inactive and the behavior is not waiting for a message, the behavior
    is deactivated.

    + The outputs of a behavior are activated or deactivated by the behavior, as a result of its internal
    processing and states. Outputs of a behavior may be linked to other behaviors through links, in which case
    activating an output will trigger the activation of one or more other behaviors. This is the basic principle
    through which processing travels along behaviors.

    + A behavior may have zero or more inputs and zero or more outputs. The output of a behavior may be linked to
    an input of the same behavior.

    + Outputs of a behavior are linked to inputs or other behaviors through instances of CKBehaviorLink.
    These links have an activation delay, which may be 0. If it is zero, the input gets activated at the same
    process cycle which may introduce infinite loops. Such Loops are detected at execution time when exceding
    the value defined by CKSetBehaviorMaxIteration (Default : 8000)


    + Behaviors may exchange or store values using parameters, which are instances of
    CKParameterOut, CKParameterIn. These values may be arbitrarily complex, and
    you may define your own type of parameters. CKParameterOut instances are used by the behavior to export values,
    and CKParameterIn instances are used to get values from the outside.

    + Components of a behavior like parameters, I/O may be named, like all objects of CK. This name
    is only a conveniency and is not mandatory. These names are saved in the state or in the file
    with the behavior and may eat up space, so you should use them carefully if this is an issue for
    your application.

    + Behaviors may wait for messages. They specify that they wait for messages by registering
    themselves with the message manager, using the CKMessageManager::RegisterWait method and by providing
    an input or an output to activate when a corresponding message is received.

    + The class id of CKBehavior is CKCID_BEHAVIOR.




See also: CKBeObject, CKScene, CKParameterOut, CKParameterIn, CKBehaviorLink, CKBehaviorPrototype
************************************************************************************/
class CKBehavior : public CKSceneObject
{
    friend class CKBehaviorManager;
    friend class CKBeObject;
    friend class CKFile;
    friend class CKParameterIn;
    friend class CKParameter;
    friend class CKParameterLocal;
    friend class CKParameterOut;
    friend class CKParameterOperation;
    friend class CKBehaviorLink;
    friend class CKBehaviorIO;

public:
    //----------------------------------------------------------------
    // Behavior Typage
    CK_BEHAVIOR_TYPE GetType();
    void SetType(CK_BEHAVIOR_TYPE);

    void SetFlags(CK_BEHAVIOR_FLAGS);
    CK_BEHAVIOR_FLAGS GetFlags();
    CK_BEHAVIOR_FLAGS ModifyFlags(CKDWORD Add, CKDWORD Remove);

    //---------- BuildingBlock or Graph
    void UseGraph();
    void UseFunction();
    int IsUsingFunction();

    //----------------------------------------------------------------
    // Targetable Behavior
    CKBOOL IsTargetable();
    CKBeObject *GetTarget();
    CKERROR UseTarget(CKBOOL Use = TRUE);
    CKBOOL IsUsingTarget();
    CKParameterIn *GetTargetParameter();
    void SetAsTargetable(CKBOOL target = TRUE);
    CKParameterIn *ReplaceTargetParameter(CKParameterIn *targetParam);
    CKParameterIn *RemoveTargetParameter();

    //----------------------------------------------------------------
    // Object type to which this behavior applies
    CK_CLASSID GetCompatibleClassID();
    void SetCompatibleClassID(CK_CLASSID);

    //----------------------------------------------------------------
    // Execution Function
    void SetFunction(CKBEHAVIORFCT fct);
    CKBEHAVIORFCT GetFunction();

    //----------------------------------------------------------------
    // Callbacks Function
    void SetCallbackFunction(CKBEHAVIORCALLBACKFCT fct);
    int CallCallbackFunction(CKDWORD Message);
    int CallSubBehaviorsCallbackFunction(CKDWORD Message, CKGUID *behguid = NULL);

    //--------------------------------------------------------------
    // RunTime Functions
    CKBOOL IsActive();
    int Execute(float deltat);

    //--------------------------------------------------------------
    //
    CKBOOL IsParentScriptActiveInScene(CKScene *scn);
    int GetShortestDelay(CKBehavior *beh);

    //--------------------------------------------------------------
    // Owner Functions
    CKBeObject *GetOwner();
    CKBehavior *GetParent();
    CKBehavior *GetOwnerScript();

    //----------------------------------------------------------------
    // Creation From prototype
    CKERROR InitFromPrototype(CKBehaviorPrototype *proto);
    CKERROR InitFromGuid(CKGUID Guid);
    CKERROR InitFctPtrFromGuid(CKGUID Guid);
    CKERROR InitFctPtrFromPrototype(CKBehaviorPrototype *proto);
    CKGUID GetPrototypeGuid();
    CKBehaviorPrototype *GetPrototype();
    CKSTRING GetPrototypeName();
    CKDWORD GetVersion();
    void SetVersion(CKDWORD version);

    //----------------------------------------------------------------
    // Outputs
    void ActivateOutput(int pos, CKBOOL active = TRUE);
    CKBOOL IsOutputActive(int pos);
    CKBehaviorIO *RemoveOutput(int pos);
    CKERROR DeleteOutput(int pos);
    CKBehaviorIO *GetOutput(int pos);
    int GetOutputCount();
    int GetOutputPosition(CKBehaviorIO *pbio);
    int AddOutput(CKSTRING name);
    CKBehaviorIO *ReplaceOutput(int pos, CKBehaviorIO *io);
    CKBehaviorIO *CreateOutput(CKSTRING name);

    //----------------------------------------------------------------
    // Inputs
    void ActivateInput(int pos, CKBOOL active = TRUE);
    CKBOOL IsInputActive(int pos);
    CKBehaviorIO *RemoveInput(int pos);
    CKERROR DeleteInput(int pos);
    CKBehaviorIO *GetInput(int pos);
    int GetInputCount();
    int GetInputPosition(CKBehaviorIO *pbio);
    int AddInput(CKSTRING name);
    CKBehaviorIO *ReplaceInput(int pos, CKBehaviorIO *io);
    CKBehaviorIO *CreateInput(CKSTRING name);

    //----------------------------------------------------------------
    // inputs Parameters
    CKERROR ExportInputParameter(CKParameterIn *p);
    CKParameterIn *CreateInputParameter(CKSTRING name, CKParameterType type);
    CKParameterIn *CreateInputParameter(CKSTRING name, CKGUID guid);
    void AddInputParameter(CKParameterIn *in);
    int GetInputParameterPosition(CKParameterIn *);
    CKParameterIn *GetInputParameter(int pos);
    CKParameterIn *RemoveInputParameter(int pos);
    CKParameterIn *ReplaceInputParameter(int pos, CKParameterIn *param);
    int GetInputParameterCount();
    CKERROR GetInputParameterValue(int pos, void *buf);
    void *GetInputParameterReadDataPtr(int pos);
    CKObject *GetInputParameterObject(int pos);
    CKBOOL IsInputParameterEnabled(int pos);
    void EnableInputParameter(int pos, CKBOOL enable);

    //----------------------------------------------------------------
    // outputs Parameters
    CKERROR ExportOutputParameter(CKParameterOut *p);
    CKParameterOut *CreateOutputParameter(CKSTRING name, CKParameterType type);
    CKParameterOut *CreateOutputParameter(CKSTRING name, CKGUID guid);
    CKParameterOut *GetOutputParameter(int pos);
    int GetOutputParameterPosition(CKParameterOut *);
    CKParameterOut *ReplaceOutputParameter(int pos, CKParameterOut *p);
    CKParameterOut *RemoveOutputParameter(int pos);
    void AddOutputParameter(CKParameterOut *out);
    int GetOutputParameterCount();
    CKERROR GetOutputParameterValue(int pos, void *buf);
    CKERROR SetOutputParameterValue(int pos, const void *buf, int size = 0);
    void *GetOutputParameterWriteDataPtr(int pos);
    CKERROR SetOutputParameterObject(int pos, CKObject *obj);
    CKObject *GetOutputParameterObject(int pos);
    CKBOOL IsOutputParameterEnabled(int pos);
    void EnableOutputParameter(int pos, CKBOOL enable);

    void SetInputParameterDefaultValue(CKParameterIn *pin, CKParameter *plink);

    //----------------------------------------------------------------
    // Local Parameters
    CKParameterLocal *CreateLocalParameter(CKSTRING name, CKParameterType type);
    CKParameterLocal *CreateLocalParameter(CKSTRING name, CKGUID guid);
    CKParameterLocal *GetLocalParameter(int pos);
    CKParameterLocal *RemoveLocalParameter(int pos);
    void AddLocalParameter(CKParameterLocal *loc);
    int GetLocalParameterPosition(CKParameterLocal *);
    int GetLocalParameterCount();
    CKERROR GetLocalParameterValue(int pos, void *buf);
    CKERROR SetLocalParameterValue(int pos, const void *buf, int size = 0);
    void *GetLocalParameterWriteDataPtr(int pos);
    void *GetLocalParameterReadDataPtr(int pos);
    CKObject *GetLocalParameterObject(int pos);
    CKERROR SetLocalParameterObject(int pos, CKObject *obj);
    CKBOOL IsLocalParameterSetting(int pos);

    //----------------------------------------------------------------
    // Run Time Graph : Activation
    void Activate(CKBOOL Active = TRUE, CKBOOL breset = FALSE);

    //----------------------------------------------------------------
    // Run Time Graph : Sub Behaviors
    CKERROR AddSubBehavior(CKBehavior *cbk);
    CKBehavior *RemoveSubBehavior(CKBehavior *cbk);
    CKBehavior *RemoveSubBehavior(int pos);
    CKBehavior *GetSubBehavior(int pos);
    int GetSubBehaviorCount();

    //----------------------------------------------------------------
    // Run Time Graph : Links between sub behaviors
    CKERROR AddSubBehaviorLink(CKBehaviorLink *cbkl);
    CKBehaviorLink *RemoveSubBehaviorLink(CKBehaviorLink *cbkl);
    CKBehaviorLink *RemoveSubBehaviorLink(int pos);
    CKBehaviorLink *GetSubBehaviorLink(int pos);
    int GetSubBehaviorLinkCount();

    //----------------------------------------------------------------
    // Run Time Graph : Parameter Operation
    CKERROR AddParameterOperation(CKParameterOperation *op);
    CKParameterOperation *GetParameterOperation(int pos);
    CKParameterOperation *RemoveParameterOperation(int pos);
    CKParameterOperation *RemoveParameterOperation(CKParameterOperation *op);
    int GetParameterOperationCount();

    //----------------------------------------------------------------
    // Run Time Graph : Behavior Priority
    int GetPriority();
    void SetPriority(int priority);

    //------- Profiling ---------------------
    float GetLastExecutionTime();

//-------------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

    CKERROR SetOwner(CKBeObject *, CKBOOL callback = TRUE);
    CKERROR SetSubBehaviorOwner(CKBeObject *o, CKBOOL callback = TRUE);

    void NotifyEdition();
    void NotifySettingsEdition();

    CKStateChunk *GetInterfaceChunk();
    void SetInterfaceChunk(CKStateChunk *state);

    CKBehavior(CKContext *Context, CKSTRING name = NULL);
    virtual ~CKBehavior();
    virtual CK_CLASSID GetClassID();

    virtual void PreSave(CKFile *file, CKDWORD flags);
    virtual CKStateChunk *Save(CKFile *file, CKDWORD flags);
    virtual CKERROR Load(CKStateChunk *chunk, CKFile *file);
    virtual void PostLoad();

    virtual void PreDelete();

    virtual int GetMemoryOccupation();
    virtual CKBOOL IsObjectUsed(CKObject *obj, CK_CLASSID cid);

    //--------------------------------------------
    // Dependencies functions	{Secret}
    virtual CKERROR PrepareDependencies(CKDependenciesContext &context, CKBOOL iCaller = TRUE);
    virtual CKERROR RemapDependencies(CKDependenciesContext &context);
    virtual CKERROR Copy(CKObject &o, CKDependenciesContext &context);

    //--------------------------------------------
    // Class Registering {secret}
    static CKSTRING GetClassName();
    static int GetDependenciesCount(int mode);
    static CKSTRING GetDependencies(int i, int mode);
    static void Register();
    static CKBehavior *CreateInstance(CKContext *Context);
    static void ReleaseInstance(CKContext *iContext, CKBehavior *);
    static CK_CLASSID m_ClassID;

    // Dynamic Cast method (returns NULL if the object can't be casted)
    static CKBehavior *Cast(CKObject *iO)
    {
        return CKIsChildClassOf(iO, CKCID_BEHAVIOR) ? (CKBehavior *)iO : NULL;
    }

    void Reset();
    void ErrorMessage(CKSTRING Error, CKSTRING Context, CKBOOL ShowOwner = TRUE, CKBOOL ShowScript = TRUE);
    void ErrorMessage(CKSTRING Error, CKDWORD Context, CKBOOL ShowOwner = TRUE, CKBOOL ShowScript = TRUE);
    void SetPrototypeGuid(CKGUID ckguid);

protected:
    CK_ID m_Owner;
    CK_ID m_BehParent;
    XSObjectPointerArray m_InputArray;
    XSObjectPointerArray m_OutputArray;
    XSObjectPointerArray m_InParameter;
    XSObjectPointerArray m_OutParameter;
    XSObjectPointerArray m_LocalParameter;
    CK_CLASSID m_CompatibleClassID;
    int m_Priority;
    CKDWORD m_Flags;
    CKStateChunk *m_InterfaceChunk;
    BehaviorGraphData *m_GraphData;
    BehaviorBlockData *m_BlockData;
    float m_LastExecutionTime;
    CK_ID m_InputTargetParam;

    void SetParent(CKBehavior *parent);
    void SortSubs();
    void ResetExecutionTime();
    void ExecuteStepStart();
    int ExecuteStep(float delta, CKDebugContext *Context);
    void WarnInfiniteLoop();
    int InternalGetShortestDelay(CKBehavior *beh, XObjectPointerArray &behparsed);
    void CheckIOsActivation();
    void CheckBehaviorActivity();
    int ExecuteFunction();
    void FindNextBehaviorsToExecute(CKBehavior *beh);
    void HierarchyPostLoad();

    static int BehaviorPrioritySort(CKObject *o1, CKObject *o2);
    static int BehaviorPrioritySort(const void *elem1, const void *elem2);

    void ApplyPatchLoad();

#endif // Docjet secret macro
};

#endif
