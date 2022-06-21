/*************************************************************************/
/*	File : CKWaveSound.h												 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKWAVESOUND_H

#define CKWAVESOUND_H "$Id:$"

#include "CKSound.h"

class CKSoundReader;

/**************************************************************************
{filename:CKWaveSound}
Summary: Wave sound.

Remarks:
+ CKWaveSound provides methods for playing a wave sound file (Wav,etc..)

+ The sound can be played using 3D , stero , frequency control , 
panning and volume control.
+ Sound file are not loaded but read from disk by a separate thread.

+ Its class id is CKCID_WAVESOUND
See also: CKMidiSound,CKSoundManager
******************************************************************************/
class CKWaveSound : public CKSound {
	friend class CKSoundManager;
public :
#ifdef DOCJETDUMMY // DOCJET secret macro
#else
	
	//-----------------------------------------------------
	// Sound Duplication for Instance Playing
	CKSOUNDHANDLE PlayMinion(CKBOOL Background=TRUE,CK3dEntity* Ent=NULL,VxVector* Position=NULL,VxVector* Direction=NULL,float MinDelay = 0.0f,float gain=1.0f);

	//---------------------------------------------------------
	// Associated filename
	CKERROR SetSoundFileName(const CKSTRING FileName);
	CKSTRING GetSoundFileName();
	
	//----------------------------------------------------------	
	// Parameter PCM
	int GetSoundLength();
	CKERROR GetSoundFormat(CKWaveFormat &Format);
	//----------------------------------------------------------	
	// Sound Type 
	CK_WAVESOUND_TYPE GetType();
	void SetType(CK_WAVESOUND_TYPE Type);
	//----------------------------------------------------------	
	// Sound State
	CKDWORD GetState();
	void SetState(CKDWORD State);
	
	//----------------------------------------------------------	
	// Priority of the sound : between 0.0(lowest) and 1.0(highest). Default is 0.5 
	void SetPriority(float Priority);
	float GetPriority();
	
	//----------------------------------------------------------	
	// Activation of the loop mode : TOSETUP
	void SetLoopMode(CKBOOL Enabled);
	CKBOOL GetLoopMode();
	
	
	//----------------------------------------------------------	
	// File Streaming
	CKERROR SetFileStreaming(CKBOOL Enabled,BOOL RecreateSound=FALSE);
	CKBOOL GetFileStreaming();

	//-----------------------------------------------------	
	// PlayBack Control
	// plays the sound with a faded 
	void Play(float FadeIn=0,float FinalGain=1.0f);
	void Resume();
	void Rewind();
	
	//----------------------------------------------------------	
	// Stops the sound with a fade 
	void Stop(float FadeOut=0);

	void Pause();
	
	//----------------------------------------------------------	
	CKBOOL IsPlaying();
	CKBOOL IsPaused();
	
	//------------------------------------------------
	// 2D/3D Members Functions 
	
	// Sets and gets the playback gain of a source. 0.....1.0
	void SetGain(float Gain);
	float GetGain();
	//----------------------------------------------------------	
	// Sets and gets the playback pitch bend of a source. 0.5....2.0 
	void SetPitch(float Rate);
	float GetPitch();
	//----------------------------------------------------------	
	// Sets the gains for multi-channel, non-spatialized sources. -1.0....1.0 default(0.0) 
	void SetPan(float Pan);
	float GetPan();
	
	
	CKSOUNDHANDLE GetSource();
	//----------------------------------------------------------
	// 3D Members Functions
	
	//----------------------------------------------------------	
	// Attach the sound to an object : TOSETUP
	void PositionSound(CK3dEntity* Object,VxVector* Position=NULL,VxVector* Direction=NULL,CKBOOL Commit=FALSE);
	CK3dEntity* GetAttachedEntity();
	void GetPosition(VxVector& Pos);
	void GetDirection(VxVector& Dir);
	void GetSound3DInformation(VxVector& Pos,VxVector& Dir,float& DistanceFromListener);
	
	
	//----------------------------------------------------------	
	// Sets the directionality of a source cone. 0.....180 for the angles, gain 0....1.0 : TOSETUP
	void SetCone(float InAngle, float OutAngle, float OutsideGain); 
	void GetCone(float* InAngle, float* OutAngle, float* OutsideGain); 
	//----------------------------------------------------------	
	// Distance min/maximum de perception (maxdbehavior = 1(mute) ou 0(audible)), distances form 0.....n with max>min : TOSETUP
	void SetMinMaxDistance(float MinDistance,float MaxDistance,CKDWORD MaxDistanceBehavior = 1);
	void GetMinMaxDistance(float* MinDistance,float* MaxDistance,CKDWORD* MaxDistanceBehavior);

	// These functions are for direct access to the source : You don't have to use them normally.
	//----------------------------------------------------------	
	// Velocity of the Source
	void SetVelocity(VxVector& Pos);
	void GetVelocity(VxVector& Pos);
	//----------------------------------------------------------	
	// Orientation of the source
	void SetOrientation(VxVector& Dir,VxVector& Up);
	void GetOrientation(VxVector& Dir,VxVector& Up);
	
	//----------------------------------------------------------	
	// Write Data in the sound buffer
	 CKERROR WriteData(BYTE* Buffer,int Buffersize);

	//----------------------------------------------------------	
	// Buffer access
	CKERROR Lock(CKDWORD WriteCursor,CKDWORD NumBytes,void **Ptr1,CKDWORD *Bytes1,void **Ptr2,CKDWORD *Bytes2,CK_WAVESOUND_LOCKMODE Flags);
	CKERROR Unlock(void *Ptr1,CKDWORD Bytes1,void *Ptr2,CKDWORD Bytes2);

	//----------------------------------------------------------	
	// Position in the sound buffer
	 CKDWORD GetPlayPosition();

	 int GetPlayedMs();

	//----------------------------------------------------------	
	// Creation of the buffer and 
	CKERROR Create(CKBOOL FileStreaming,CKSTRING Filename);
	CKERROR Create(CK_WAVESOUND_TYPE Type,CKWaveFormat* Format,int Size);
	CKERROR SetReader(CKSoundReader* Reader);
	CKSoundReader* GetReader();

	void SetDataToRead(int Size);

	CKERROR Recreate(BOOL Safe = FALSE);
	void Release();		

	
	CKERROR TryRecreate();					

	//----------------------------------------------------------	
	// Update the position, according to the attached object 
	
	void UpdatePosition(float deltaT);

//-------------------------------------------------------------------------
// Internal functions 

	void UpdateFade();								
	CKERROR WriteDataFromReader();					
	void FillWithBlanks(CKBOOL IncBf = FALSE);		
	void InternalStop();								


			CKWaveSound(CKContext *Context,CKSTRING Name=NULL);						
	virtual	~CKWaveSound();															
	virtual CK_CLASSID GetClassID();												

	virtual CKStateChunk*	Save(CKFile *File,CKDWORD Flags);						
	virtual CKERROR			Load(CKStateChunk *Chunk,CKFile* File);					

	virtual CKERROR	RemapDependencies(CKDependenciesContext& context);				
	virtual CKERROR	Copy(CKObject& o,CKDependenciesContext& context);				

	virtual void			CheckPostDeletion();									

	virtual int				GetMemoryOccupation();									
	//--------------------------------------------
	// Class Registering 
	static CKSTRING  GetClassName();												
	static int		 GetDependenciesCount(int Mode);								
	static CKSTRING  GetDependencies(int i,int Mode);								
	static void		 Register();													
	static CKWaveSound*  CreateInstance(CKContext *Context);						
	static void		 ReleaseInstance(CKContext* iContext,CKWaveSound*);							
	static CK_CLASSID m_ClassID;													
	
	// Dynamic Cast method (returns NULL if the object can't be casted)
	static CKWaveSound* Cast(CKObject* iO) 
	{
		return CKIsChildClassOf(iO,CKCID_WAVESOUND)?(CKWaveSound*)iO:NULL;
	}

// protected:	
	CKSoundManager* m_SoundManager;		
	void*			m_Source;			
	CKDWORD			m_State;			
	CK_ID			m_AttachedObject;	
	VxVector		m_OldPosition;		
	VxVector		m_Position;			
	VxVector		m_Direction;		
	float			m_FinalGain;		
	float			m_FadeTime;			
	float			m_CurrentTime;		
	int				m_BufferPos;		
	int				m_BufferSize;		
	CKSoundReader*	m_SoundReader;		
	int				m_DataRead;			
	int				m_DataToRead;		
	int				m_DataPlayed;		
	int				m_OldCursorPos;		
	int				m_Duration;			// Duration in milliseconds		
	CKWaveFormat	m_WaveFormat;		
	
	int GetDistanceFromCursor();		
	void InternalSetGain(float Gain);	
	
	void SaveSettings();				
	void RestoreSettings();				
	
	
	void * ReallocSource(void *oSource,int alreadyWritten,int newSize);


	// Save Information From Current source
	CKWaveSoundSettings m_2DSetting;		
	CKWaveSound3DSettings m_3DSetting;	

#endif // Docjet secret macro

};

#endif

