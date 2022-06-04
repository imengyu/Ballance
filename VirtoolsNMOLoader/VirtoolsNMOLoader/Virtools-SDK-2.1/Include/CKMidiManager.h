/*************************************************************************/
/*	File : CKMidiManager.h												 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKMidiManager_H
#define CKMidiManager_H "$Id:$"

#include "CKBaseManager.h"

/**************************************************************************
Name: CKMidiManager
Summary: Midi sound engine manager.

Remarks:
+ The midi manager is implemented as an external plugin.The default implmentation	in Virtools is made using the standard windows API.
+ The unique instance of this class may be retrieved through the CKContext::GetManagerByGuid(MIDI_MANAGER_GUID)
See also: CKContext::GetManagerByGuid,CKSoundManager,CKMidiSound
****************************************************************************/
class CKMidiManager : public CKBaseManager
{
public:
    virtual void *Create(void *hwnd) = 0;
    virtual void Release(void *source) = 0;

    virtual CKERROR SetSoundFileName(void *source, CKSTRING filename) = 0;
    virtual CKSTRING GetSoundFileName(void *source) = 0;

    virtual CKERROR Play(void *source) = 0;
    virtual CKERROR Restart(void *source) = 0;
    virtual CKERROR Stop(void *source) = 0;
    virtual CKERROR Pause(void *source, CKBOOL pause = TRUE) = 0;
    virtual CKBOOL IsPlaying(void *source) = 0;
    virtual CKBOOL IsPaused(void *source) = 0;

    virtual CKERROR OpenFile(void *source) = 0;
    virtual CKERROR CloseFile(void *source) = 0;
    virtual CKERROR Preroll(void *source) = 0;
    virtual CKERROR Time(void *source, CKDWORD *pTicks) = 0;
    virtual CKDWORD MillisecsToTicks(void *source, CKDWORD msOffset) = 0;
    virtual CKDWORD TicksToMillisecs(void *source, CKDWORD tkOffset) = 0;

    CKMidiManager(CKContext *Context, CKSTRING name) : CKBaseManager(Context, MIDI_MANAGER_GUID, name){};

    virtual ~CKMidiManager(){};
};

#endif
