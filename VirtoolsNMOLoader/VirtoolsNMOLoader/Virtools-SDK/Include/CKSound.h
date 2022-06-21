/*************************************************************************/
/*	File : CKSound.h													 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSOUND_H

#define CKSOUND_H "$Id:$"

#include "CKBeObject.h"

/*************************************************
{filename:CKSound}
Name: CKSound

Summary: Base Class for Sound management

Remarks:
	+ CKSound provides only common method for the save options of sounds.
See also: CKWaveSound,CKMidiSound
*************************************************/
class CKSound:public CKBeObject {
public :
//-------- Save format
CK_SOUND_SAVEOPTIONS	GetSaveOptions();
void	SetSaveOptions(CK_SOUND_SAVEOPTIONS Options);

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

			CKSound(CKContext *Context,CKSTRING name=NULL);						
	virtual	~CKSound();															
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
	static CKSound*  CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKSound*);							
	static CK_CLASSID m_ClassID;												

	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKSound* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_SOUND)?(CKSound*)iO:NULL;
	}

protected :
	CKSTRING					m_FileName;
	CK_SOUND_SAVEOPTIONS		m_SaveOptions;

#endif // docjet secret macro
};

#endif

