/*************************************************************************/
/*	File : CKSoundManager.h												 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKSOUNDMANAGER_H
#define CKSOUNDMANAGER_H "$Id:$"

#include "CKBaseManager.h"

typedef void *CKSOUNDHANDLE;

#ifdef DOCJETDUMMY // Docjet secret macro
#else

struct SoundMinion
{
    // The A3D Source
    CKSOUNDHANDLE m_Source;
    // The A3D Source
    CKSOUNDHANDLE m_OriginalSource;
    // The Attached Entity
    CK_ID m_Entity;
    // Original Sound
    CK_ID m_OriginalSound;
    // The Position
    VxVector m_Position;
    // The Direction
    VxVector m_Direction;
    // The Old Position
    VxVector m_OldPosition;
    // Time Stamp
    float m_TimeStamp;
};

#endif

//
typedef enum CK_SOUNDMANAGER_CAPS
{
    CK_SOUNDMANAGER_ONFLYTYPE              = 0x00000001,	// Allows on the fly type changement
    CK_SOUNDMANAGER_OCCLUSION              = 0x00000002,	// Allows occlusions
    CK_SOUNDMANAGER_REFLECTION             = 0x00000004,	// Allows reflections
    CK_SOUNDMANAGER_ALL                    = 0x00000007,

    CK_WAVESOUND_SETTINGS_GAIN             = 0x00000010,
    CK_WAVESOUND_SETTINGS_EQUALIZATION     = 0x00000020,
    CK_WAVESOUND_SETTINGS_PITCH            = 0x00000040,
    CK_WAVESOUND_SETTINGS_PRIORITY         = 0x00000080,
    CK_WAVESOUND_SETTINGS_PAN              = 0x00000100,
    CK_WAVESOUND_SETTINGS_ALL              = 0x000001F0,

    CK_WAVESOUND_3DSETTINGS_CONE           = 0x00001000,
    CK_WAVESOUND_3DSETTINGS_MINMAXDISTANCE = 0x00002000,
    CK_WAVESOUND_3DSETTINGS_DISTANCEFACTOR = 0x00004000,
    CK_WAVESOUND_3DSETTINGS_DOPPLERFACTOR  = 0x00008000,
    CK_WAVESOUND_3DSETTINGS_POSITION       = 0x00010000,
    CK_WAVESOUND_3DSETTINGS_VELOCITY       = 0x00020000,
    CK_WAVESOUND_3DSETTINGS_ORIENTATION    = 0x00040000,
    CK_WAVESOUND_3DSETTINGS_HEADRELATIVE   = 0x00080000,
    CK_WAVESOUND_3DSETTINGS_ALL            = 0x000FF000,

    CK_LISTENERSETTINGS_DISTANCE           = 0x00100000,
    CK_LISTENERSETTINGS_DOPPLER            = 0x00200000,
    CK_LISTENERSETTINGS_UNITS              = 0x00400000,
    CK_LISTENERSETTINGS_ROLLOFF            = 0x00400000,
    CK_LISTENERSETTINGS_EQ                 = 0x00800000,
    CK_LISTENERSETTINGS_GAIN               = 0x01000000,
    CK_LISTENERSETTINGS_PRIORITY           = 0x02000000,
    CK_LISTENERSETTINGS_SOFTWARESOURCES    = 0x04000000,
    CK_LISTENERSETTINGS_ALL                = 0x07F00000,
} CK_SOUNDMANAGER_CAPS;

//
// Sound Settings
struct CKWaveSoundSettings
{
    CKWaveSoundSettings() : m_Gain(1.0f), m_Eq(1.0f), m_Pitch(1.0f), m_Priority(0.5f), m_Pan(0.0f) {}
    // Sets and gets the playback gain of a sourc. 0.....1.0
    float m_Gain;
    // Sets the tonal equalization of a source. 0....1.0
    float m_Eq;
    // Sets and gets the playback pitch bend of a source. 0.5....2.0
    float m_Pitch;
    // Sets and gets the playback pitch bend of a source. 0.0....1.0
    float m_Priority;

    // Sets the gains for multi-channel, non-spatialized sources. -1.0....1.0 default(0.0)
    float m_Pan;
};

// Summary: To Document
//
// 3D Sound settings
struct CKWaveSound3DSettings
{
    CKWaveSound3DSettings() : m_HeadRelative(0), m_MuteAfterMax(0), m_InAngle(360.0f), m_OutAngle(360.0f),
                              m_OutsideGain(0.0f), m_MinDistance(10.0f), m_MaxDistance(20.0f)
    {
        m_Position.x = m_Position.y = m_Position.z = 0;
        m_Velocity.x = m_Velocity.y = m_Velocity.z = 0;

        m_OrientationDir.x = 0.0f;
        m_OrientationDir.y = 0.0f;
        m_OrientationDir.z = 1.0f;

        m_OrientationUp.x = 0.0f;
        m_OrientationUp.y = 1.0f;
        m_OrientationUp.z = 0.0f;
    }
    CKWORD m_HeadRelative;
    CKWORD m_MuteAfterMax;

    float m_InAngle;
    float m_OutAngle;
    float m_OutsideGain;

    float m_MinDistance;
    float m_MaxDistance;

    VxVector m_Position;
    VxVector m_Velocity;
    VxVector m_OrientationDir;
    VxVector m_OrientationUp;
};

// Summary: To Document
//
// Listener Settings
struct CKListenerSettings
{
    CKListenerSettings() : m_DistanceFactor(1.0f), m_DopplerFactor(1.0f), m_RollOff(1.0f),
                           m_GlobalGain(1.0f), m_PriorityBias(1.0f), m_SoftwareSources(16) {}
    float m_DistanceFactor;
    float m_DopplerFactor;
    float m_RollOff;

    float m_GlobalGain;
    float m_PriorityBias;
    unsigned int m_SoftwareSources;
};

#define CKWAVE_FORMAT_PCM 1

// Summary: To Document
//
struct CKWaveFormat
{
    WORD wFormatTag;
    WORD nChannels;
    DWORD nSamplesPerSec;
    DWORD nAvgBytesPerSec;
    WORD nBlockAlign;
    WORD wBitsPerSample;
    WORD cbSize;
};

/*******************************************************************
Name: CKSoundManager
Summary: Sound Engine Manager.

Remarks:

+ The sound manager is implemented by an external plugin.The default implmentation Virtools is made using DirectSound.

+ Sound management is done by the SoundManager which process sounds to play and updates the listener (player ) position and settings.

+ The unique instance of this class may be retrieved through the CKContext::GetSoundManager()

See also: CKContext::GetSoundManager,CKMidiManager,CKWaveSound
*********************************************************************/
class CKSoundManager : public CKBaseManager
{
public:
    // Get the caps of the sound manager
    virtual CK_SOUNDMANAGER_CAPS GetCaps() = 0;

    //-----------------------------------------------------
    // Source Functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else
    // Creation
    virtual void *CreateSource(CK_WAVESOUND_TYPE flags, CKWaveFormat *wf, CKDWORD bytes, BOOL streamed) = 0;
    virtual void *DuplicateSource(void *source) = 0;
    virtual void ReleaseSource(void *source) = 0;

    // Control
    virtual void Play(CKWaveSound *, void *source, CKBOOL loop) = 0;
    virtual void Pause(CKWaveSound *, void *source) = 0;
    virtual void SetPlayPosition(void *source, int pos) = 0;
    virtual int GetPlayPosition(void *source) = 0;
    virtual void Stop(CKWaveSound *w, void *source)
    {
        Pause(w, source);
        SetPlayPosition(source, 0);
    }
    virtual CKBOOL IsPlaying(void *source) = 0;

    // PCM Buffer Informations
    virtual CKERROR SetWaveFormat(void *source, CKWaveFormat &wf) = 0;
    virtual CKERROR GetWaveFormat(void *source, CKWaveFormat &wf) = 0;
    virtual int GetWaveSize(void *source) = 0;

    // Buffer access
    virtual CKERROR Lock(void *source, CKDWORD WriteCursor, CKDWORD NumBytes, void **AudioPtr1, CKDWORD *dwAudioBytes1, void **pvAudioPtr2, CKDWORD *dwAudioBytes2, CK_WAVESOUND_LOCKMODE dwFlags) = 0;
    virtual CKERROR Unlock(void *source, void *AudioPtr1, CKDWORD NumBytes1, void *AudioPtr2, CKDWORD dwAudioBytes2) = 0;

    // 2D/3D Members Functions
    virtual void SetType(void *source, CK_WAVESOUND_TYPE type) = 0;
    virtual CK_WAVESOUND_TYPE GetType(void *source) = 0;

    //----------------------------------------------------------
    // 2D/3D Settings
    virtual void UpdateSettings(void *source, CK_SOUNDMANAGER_CAPS settingsoptions, CKWaveSoundSettings &settings, CKBOOL set = TRUE) = 0;

    //----------------------------------------------------------
    // 3D Settings
    virtual void Update3DSettings(void *source, CK_SOUNDMANAGER_CAPS settingsoptions, CKWaveSound3DSettings &settings, CKBOOL set = TRUE) = 0;
#endif

    /*************************************************
    Summary: Sets the Listener settings such as global distance factor,doppler effect, rollof or gain.

    Arguments:
        settingsoptions : flags specifying wich member of the settings parameter must be set or retrieved
        settings : structure specifyig the new settings.
        set : if true the function sets the listener settings, otherwise it returns the current listener options in the "settings" parameter

    Remarks:
        + if no listener is specified, the listener is the current camera.
    See also: GetListener,SetListener,CKListenerSettings,CK_SOUNDMANAGER_CAPS
    *************************************************/
    virtual void UpdateListenerSettings(CK_SOUNDMANAGER_CAPS settingsoptions, CKListenerSettings &settings, CKBOOL set = TRUE) = 0;

    /*************************************************
    Summary: Sets the Listener

    Arguments:
        listener: The new listener
    Remarks:
        + if no listener is specified, the listener is the current camera.
    See also: GetListener,UpdateListenerSettings
    *************************************************/
    virtual void SetListener(CK3dEntity *listener);
    /*************************************************
    Summary: Gets the Listener

    Return Value:
        The currently set listener
    Remarks:
        + if no listener is specified, the listener is the current camera but the returned value is NULL.
    See also: SetListener,UpdateListenerSettings
    *************************************************/
    virtual CK3dEntity *GetListener();

    /*************************************************
    Summary: sets the internal buffer size used for streaming sounds

    Return Value:
        bsize : The length in milliseconds of the buffer used to play streamed sound.
    Remarks:
        + if a streamed sound length is smaller than this value, it will be fully loaded in memory.
    See also: GetStreamedBufferSize
    *************************************************/
    virtual void SetStreamedBufferSize(CKDWORD bsize);

    /*************************************************
    Summary: Gets the internal buffer size used for streaming sounds

    Arguments:
        The length in milliseconds of the buffer used to play streamed sound.
    Remarks:
        + if a streamed sound length is smaller than this value, it will be fully loaded in memory.
    See also: SetStreamedBufferSize
    *************************************************/
    virtual CKDWORD GetStreamedBufferSize();

#ifdef DOCJETDUMMY // DOCJET secret macro
#else
    virtual CKBOOL IsInitialized() = 0;

    SoundMinion *CreateMinion(CKSOUNDHANDLE source, float minimumdelay = 0.0f);
    void ReleaseMinions();
    void PauseMinions();
    void ResumeMinions();
    void ProcessMinions();

    CKSoundManager(CKContext *Context, CKSTRING smname);
    virtual ~CKSoundManager();
    void RegisterAttribute();

    CKERROR SequenceDeleted(CK_ID *objids, int count);

    CKERROR PostClearAll();

    virtual CKDWORD GetValidFunctionsMask() { return CKMANAGER_FUNC_OnSequenceDeleted | CKMANAGER_FUNC_PostClearAll; }

protected:
    virtual void InternalPause(void *source) = 0;
    virtual void InternalPlay(void *source, CKBOOL loop = FALSE) = 0;

    // Sound Obstacle Attribute
    //	int						m_SoundObstacleAttribute;
    // Listener Entity
    CK_ID m_ListenerEntity;
    // Streamed Buffers Size
    CKDWORD m_BufferSize;
    // Minions
    XArray<SoundMinion *> m_Minions;
    VxVector m_OldListenerPos;

#endif // Docjet secret macro
};

#endif
