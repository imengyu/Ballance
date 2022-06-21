/*************************************************************************/
/*	File : CKBehaviorLink.h			 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKBEHAVIORLINK_H

#define CKBEHAVIORLINK_H "$Id:$"

#include "CKObject.h"

/***********************************************************************
Name: CKBehaviorLink

Summary: Links between behaviors Inputs/Outputs

Remarks:
{Image:BehaviorLink}

+ The behavior links provide activation propagation between behaviors in a graph.
Each link receives the activation from its input and propagates
the activation from its input to its output, immediately or with a given delay.

+ Its input can be the output of a behavior, or another input from the containing behavior.

+ Its output can be the input of another behavior, or the output of the containing behavior.

+ The class id of CKBehaviorLink is CKCID_BEHAVIORLINK.


See also: CKBehavior,CKBehaviorIO
**************************************************************************/
class CKBehaviorLink:public CKObject {
friend class CKBehavior;
friend class CKBehaviorIO;
public :

//-------------------------------------------------------
// Behavior IO functions
	CKERROR SetOutBehaviorIO(CKBehaviorIO *ckbioin); 
	CKERROR SetInBehaviorIO(CKBehaviorIO *ckbioout); 


CKBehaviorIO* GetOutBehaviorIO() {return m_OutIO;}

CKBehaviorIO* GetInBehaviorIO() {return m_InIO;}

/*************************************************
Summary: Management of the activation delay
Return Value:
	activation delay in number of frames for this link.
Remarks:
+ The activation delay is the number of frames after which the output of the link
is activated once the input of the link has triggered the link.

+ The GetActivationDelay returns the current activation delay. The SetActivationDelay sets
the new activation delay. This delay has to be >= 0. At creation time, the activation
delay is set to 1, that is that the output is activated one frame after the input
has been activated.

+ The ResetActivationDelay sets back the activation delay to its initial value, that
had been specified with SetInitialActivationDelay.

See also: SetInitialActivationDelay
*************************************************/
int  GetActivationDelay() {return m_ActivationDelay;}

void SetActivationDelay(int delay) {m_ActivationDelay = delay;}

void  ResetActivationDelay() {m_ActivationDelay = m_InitialActivationDelay;}

/*************************************************
Summary: Management of the initial activation delay.

Remarks:
	+ The initial activation delay is the default value of the activation delay.
	Even if the activation delay is changed, the initial activation delay remains the same. The
	activation delay can be reset to be equal to the initial activation delay with the ResetActivationDelay
	method.

	+ The GetInitialActivationDelay returns the current initial activation delay. At creation time, the initial
	activation delay is set to 1.

	+ The SetInitialActivationDelay sets a new value for the initial activation delay. This value must be >= 0 and <32765.

See also: ResetActivationDelay,GetActivationDelay
*************************************************/
void SetInitialActivationDelay(int delay) {m_InitialActivationDelay	= delay;}

int  GetInitialActivationDelay() {return m_InitialActivationDelay;}

//-------------------------------------------------------------------------
// Internal functions 
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

CKDWORD  GetFlags() { 
		return m_ObjectFlags&CK_OBJECT_BEHAVIORLINKMASK;
	}					
	void	 SetFlags(CKDWORD flags) { 
		m_ObjectFlags &= ~CK_OBJECT_BEHAVIORLINKMASK;
		m_ObjectFlags |= flags;
	}

			CKBehaviorLink(CKContext *Context,CKSTRING name=NULL);					
	virtual	~CKBehaviorLink();														
	virtual CK_CLASSID GetClassID();												

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);						
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);					
	virtual void			PostLoad();												

	virtual void			PreDelete();											

	virtual int				GetMemoryOccupation();									
	virtual CKBOOL			IsObjectUsed(CKObject* obj,CK_CLASSID cid);				

	//--------------------------------------------
	// Dependencies functions {secret}												
	virtual CKERROR RemapDependencies(CKDependenciesContext& context);				
	virtual CKERROR Copy(CKObject& o,CKDependenciesContext& context);				

	//--------------------------------------------
	// Class Registering {secret}
	static CKSTRING  GetClassName();												
	static int		 GetDependenciesCount(int mode);								
	static CKSTRING  GetDependencies(int i,int mode);								
	static void		 Register();													
	static CKBehaviorLink* CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKBehaviorLink*);							
	static CK_CLASSID m_ClassID;													

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKBehaviorLink* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_BEHAVIORLINK)?(CKBehaviorLink*)iO:NULL;
	}

protected :
	short int		m_ActivationDelay;
	short int		m_InitialActivationDelay;
	CKBehaviorIO*	m_InIO; 
	CKBehaviorIO*	m_OutIO; 
#endif // Docjet secret macro
};

#endif

