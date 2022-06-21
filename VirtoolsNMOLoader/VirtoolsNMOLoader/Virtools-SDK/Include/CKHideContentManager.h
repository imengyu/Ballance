#ifndef CKHideContentManager_H
#define CKHideContentManager_H "$Id:$"

#include "CKDefines.h"
#include "CKDefines2.h"
#include "CKBaseManager.h"

#define CKHideContentManagerGUID CKGUID( 0xd6b354d7 , 0x9b751ca5 )

const CKDWORD HideChunkID = 1001;
const int maxPassSizeForCrypt = 6;

// Flags
enum HideManFlags {
	COMPOSITIONBOUND		= 0x0004, // Variable value will be saved when saving a composition (user decision in the interface)
	NATIVEONLY				= 0x0008, // Variable value will be saved when saving a composition (variable creater decision in code)
	NATIVECOMPOSITIONBOUND	= 0x000c, // Combination of COMPOSITIONBOUND & NATIVEONLY to indicate the value must be saved within a composition

};

class CKProtectable
{
protected:
	CKBOOL					m_Hide;
	XString					m_currentPass;
	int						m_passSize;
	CKBOOL					m_loaded;
	CKBOOL					m_setLoadedOnSave;
public:
	CKProtectable() {
		m_Hide = FALSE;
		m_loaded = FALSE;
		m_setLoadedOnSave = FALSE;
		m_currentPass = "empty";
		m_passSize = m_currentPass.Length();
	}

	CKProtectable(CKProtectable& prot) {
		m_Hide = prot.GetHideFlag();
		m_loaded = prot.Loaded();
		m_setLoadedOnSave = prot.SetLoadedOnSave();
		m_currentPass = prot.CurrentPass();
	}

	virtual void			SetHideFlag(CKBOOL hide) = 0;
	virtual CKBOOL			GetHideFlag() =0;
	virtual void			UnLock(const char* pass) = 0;
	virtual const XString	CurrentPass() {
		m_passSize = m_currentPass.Length();
		return m_currentPass; 
	}
	virtual void			CurrentPass(XString val) {
		m_currentPass = val;
		m_passSize = m_currentPass.Length();
	};

	void					SetCurrentPass(const char* pass);

	virtual const CKBOOL	Loaded() { return m_loaded; }
	virtual void			Loaded(CKBOOL val) { m_loaded = val; }

	virtual const CKBOOL	SetLoadedOnSave() { return m_setLoadedOnSave; }
	virtual void			SetLoadedOnSave(CKBOOL val) { m_setLoadedOnSave = val; }

	virtual const int		PassSize() { return m_passSize; }

	virtual	CKERROR			SimpleHideWithPass(char** dst, const char* src);
	virtual	CKERROR			SimpleClarifyWithPass(char** dst, const char* src);

	static	CKERROR			SimpleHideWithPass(CKProtectable* me, char** dst, const char* src) {return me->SimpleHideWithPass(dst, src); }
	static	CKERROR			SimpleClarifyWithPass(CKProtectable* me, char** dst, const char* src) {return me->SimpleClarifyWithPass(dst, src);}

	virtual void			WriteProtectable(int* hide, XString* currentPass) {
		*hide = m_Hide;
		*currentPass = m_currentPass;
	}

	virtual void			ReadProtectable(int* hide, XString* currentPass) {
		m_Hide = *hide;
		m_currentPass = *currentPass;
		m_loaded = TRUE;
		m_setLoadedOnSave = FALSE;
		m_passSize = m_currentPass.Length();
	}
};

class CKHideContentManager : public CKBaseManager 
{
	friend class CKProtectable;
//##############################################################
//                Public Part                      
//##############################################################
public :
	CKHideContentManager(CKContext* Context);
	~CKHideContentManager();	
	
	static void					Hide(CKProtectable* me);
	static void					UnLock(CKProtectable* me, const char* pass);
	static XString&				Hide(CKProtectable* me, const XString clearSrc);
	static XString&				Clarify(CKProtectable* me, const XString hiddenSrc);
	static CKERROR				Hide(CKProtectable* me, char** dst, const char* src);
	static CKERROR				Clarify(CKProtectable* me, char** dst, const char* src);
	static CKBOOL				CheckAgainst(CKProtectable* me, const char* pass);
	static const char*			GetClearPass();
	static const char*			GetTag();
	static void					SetLoaded(CKProtectable* me);
	static CKERROR				EndLoad();

//--------------------------------------------------------------
// Methods to implement
//--------------------------------------------------------------

//---  Called to load manager data.
	virtual CKERROR LoadData(CKStateChunk* chunk, CKFile* LoadedFile);

//---   Called to save manager data. return NULL if nothing to save.
	virtual CKStateChunk* SaveData(CKFile* SavedFile);

//---  Returns list of functions implemented by the manager.
	 virtual CKDWORD GetValidFunctionsMask() {
		return
			CKMANAGER_FUNC_PostClearAll;
	}

//--------------------------------------------------------------
// Unused methods
//--------------------------------------------------------------

/*
//---  Called at the beginning of each process loop.
	virtual CKERROR PreProcess();

//---  Called at the end of each process loop.
	virtual CKERROR PostProcess();

//---  Called after the composition has been restarted.
	virtual CKERROR OnCKPostReset();

//---  Called before the composition is reset.
	virtual CKERROR OnCKReset();

//---  Called when the process loop is started.
	virtual CKERROR OnCKPlay();

//---  Called when the process loop is paused.
	virtual CKERROR OnCKPause();

//---  Called before a scene becomes active.
	virtual CKERROR PreLaunchScene(CKScene* OldScene, CKScene* NewScene);

//---  Called after a scene became active.
	virtual CKERROR PostLaunchScene(CKScene* OldScene, CKScene* NewScene);

//---  Called at the beginning of a copy.
	virtual CKERROR OnPreCopy(CKDependenciesContext& context);

//---  Called at the end of a copy.
	virtual CKERROR OnPostCopy(CKDependenciesContext& context);

//---  Called when objects are added to a scene.
	virtual CKERROR SequenceAddedToScene(CKScene* scn, CK_ID* objids, int count);

//---  Called when objects are removed from a scene.
	virtual CKERROR SequenceRemovedFromScene(CKScene* scn, CK_ID* objids, int count);

//---  Called just before objects are deleted.
	virtual CKERROR SequenceToBeDeleted(CK_ID* objids, int count);

//---  Called after objects have been deleted.
	virtual CKERROR SequenceDeleted(CK_ID* objids, int count);

//---  Called before the rendering of the 3D objects.
	virtual CKERROR OnPreRender(CKRenderContext* dev);

//---  Called after the rendering of the 3D objects.
	virtual CKERROR OnPostRender(CKRenderContext* dev);

//---  Called after the rendering of 2D entities.
	virtual CKERROR OnPostSpriteRender(CKRenderContext* dev);

//---  Called before the backbuffer is presented.
	virtual CKERROR OnPreBackToFront(CKRenderContext* dev);

//---  Called after the backbuffer is presented.
	virtual CKERROR OnPostBackToFront(CKRenderContext* dev);

//---  Called before switching to/from fullscreen.
	virtual CKERROR OnPreFullScreen(BOOL Going2Fullscreen, CKRenderContext* dev);

//---  Called after switching to/from fullscreen.
	virtual CKERROR OnPostFullScreen(BOOL Going2Fullscreen, CKRenderContext* dev);

//---  Called at the end of the creation of a CKContext.
	virtual CKERROR OnCKInit();

//---  Called at deletion of a CKContext.
	virtual CKERROR OnCKEnd();

//---  Called at the beginning of a load operation.
	virtual CKERROR PreLoad();

//---  Called at the end of a load operation.
	virtual CKERROR PostLoad();

//---  Called at the beginning of a save operation.
	virtual CKERROR PreSave();


//---  Called at the end of a save operation.
	virtual CKERROR PostSave();

//---  Called at the beginning of a CKContext::ClearAll operation.
	virtual CKERROR PreClearAll();
*/
//---  Called at the end of a CKContext::ClearAll operation.
	virtual CKERROR PostClearAll();


	static void	Init();

	static CKERROR (*m_transformTo)(CKProtectable* me, char** dst, const char* src);
	static CKERROR (*m_transformFrom)(CKProtectable* me, char** dst, const char* src);

	static CKERROR SimpleHide(char** dst, const char* src);
	static CKERROR SimpleHide(char** dst, const char* src, BYTE offset);
	static CKERROR SimpleClarify(char** dst, const char* src);
	static CKERROR SimpleClarify(char** dst, const char* src, BYTE offset);

protected :
	static void		ProcessSetLoadedOnSave(CKProtectable* me);
	static CKBOOL	Hum();
	static void		LockThemAll();
	static void		UnlockThemAll(const char* pass);
	static CKBOOL	GetHideAll();
	static void		ClearProtectables();

	static	XString								m_tag;
	static	int									m_tagSize;
	static  const int							m_maxPassSize = 5;

	static	CKContext*							m_currentContext;
	static  XString								m_clearPass;

	static	XArray<CKProtectable*>				m_protectableObjects;

	static CKBOOL								m_loading;
	static CKBOOL								m_nmoOp;
	static CKBOOL								m_saving;
};

#endif

