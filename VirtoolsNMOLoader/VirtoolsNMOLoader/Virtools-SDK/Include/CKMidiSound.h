/*************************************************************************/
/*	File : CKMidiSound.h												 */
/*	Author :  Francisco Cabrita											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKMIDISOUND_H

#define CKMIDISOUND_H "$Id:$"

#include "CKSound.h"


/**************************************************************************
Name: CKMidiSound
Summary: Midi sound.

Remarks:
+ CKMidiSound provides the basic methods for playing a midi sound file.
+ Its class id is CKCID_MIDISOUND
See also: CKWaveSound,CKSoundManager
******************************************************************************/
class CKMidiSound : public CKSound {
public :

//---------------------------------------------------
// Sound File
CKERROR  SetSoundFileName(CKSTRING filename);
CKSTRING GetSoundFileName();

//----------------------------------------------------
// Current Position In MilliSeconds
CKDWORD GetCurrentPos();

//-----------------------------------------------------
// Sound File
CKERROR Play();
CKERROR Stop();
CKERROR Pause(CKBOOL pause=TRUE);
CKBOOL IsPlaying();
CKBOOL IsPaused();

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

			CKMidiSound(CKContext *Context,CKSTRING name=NULL);					 
	virtual	~CKMidiSound();														 
	virtual CK_CLASSID GetClassID();											 

	virtual CKStateChunk*	Save(CKFile *file,CKDWORD flags);					
	virtual CKERROR			Load(CKStateChunk *chunk,CKFile* file);				

	virtual int				GetMemoryOccupation();								

	//--------------------------------------------
	// Class Registering 
	static CKSTRING  GetClassName();											
	static int		 GetDependenciesCount(int mode);							
	static CKSTRING  GetDependencies(int i,int mode);							
	static void		 Register();												
	static CKMidiSound* CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKMidiSound*);							
	static CK_CLASSID m_ClassID;												

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKMidiSound* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_MIDISOUND)?(CKMidiSound*)iO:NULL;
	}

	CKERROR OpenFile();															
	CKERROR CloseFile();																		
	CKERROR Prepare();															
	CKERROR Start();															
protected :
	void*			m_Source;
	CKMidiManager*	m_MidiManager;
#endif // Docjet secret macro
};

#endif

