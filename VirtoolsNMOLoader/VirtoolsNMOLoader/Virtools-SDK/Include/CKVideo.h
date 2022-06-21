/*************************************************************************/
/*	File : CKVideo.h													 */
/*	Author :  Leïla AIT	KACI											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2005, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKVIDEO_H

#define CKVIDEO_H "$Id:$"

#include "CKBeObject.h"
#include "VxThread.h"

class CKVideo;

/*******************************************************************
Name: CKVideoInfo
Summary: Information on a video file

See also: CKVideo
********************************************************************/
typedef struct CKVideoInfo
{
	float		m_Duration;

	// Video Specific
	int			m_Width;				// Image Width
	int			m_Height;				// Image Height
	CKBOOL		m_IsInterlaced;			// Is the video interlaced
	float		m_ImagePerSec;			// Number of Images per second
	XString		m_VideoFormat;			// Readable Video Format (Compression)

	// Sound Specific
	int			m_AudioChannel;			// Channel count if several available
	int			m_SamplingRate;			// Number of Samples per second
	int			m_SamplingSize;			// AvgBytesPerSec
	XString		m_AudioFormat;			// Readable Sound Format (Compression)

	CKBOOL		m_IsValid;				// Are the current info valid ?

	int			m_VideoInputFormatID;	// Format actually used for video input.
} CKVideoInfo;

/*******************************************************************
Name: CK_VIDEO_EVENT
Summary: Events that happens on a video

Remarks:
	+ This events may called from a different thread than Virtools
	  rendering thread. So, make sure to protect the objects you want
	  to access in these callbacks.
See also: CKVideoManager, CKVideo
*********************************************************************/
typedef enum CK_VIDEO_EVENT
{
	CK_VIDEOEVENT_LOADED,			// Not Supported Yet - The video has just been loaded [EventData = NULL]
	CK_VIDEOEVENT_UNLOADED,			// Not Supported Yet - The video has just unloaded [EventData = NULL]
	CK_VIDEOEVENT_NEWIMAGE,			// A new image is displayed [EventData = NULL]
	CK_VIDEOEVENT_PROCESSIMAGE,		// A new image is displayed [EventData = VxImageDescEx*]
	CK_VIDEOEVENT_COUNT				// Number of video events
} CK_VIDEO_EVENT;

/*******************************************************************
Name: CK_VIDEO_TYPE
Summary: Supported Video Types

Remarks: + Part of Video Flags
See also: CKVideoManager, CKVideo
*********************************************************************/
typedef enum CK_VIDEO_TYPE
{
	CK_VIDEO_FILE			= 0x10000000,	// The video is readed from a file
	CK_VIDEO_LIVE			= 0x20000000,	// The video is readed from a live source
	CK_VIDEO_STREAM			= 0x40000000	// The video comes from an internet stream			
} CK_VIDEO_TYPE;

/*******************************************************************
Name: CK_VIDEO_DISPLAY
Summary: On what support do we display the video ?

Remarks: + Part of Video Flags
See also: CKVideoManager, CKVideo
*********************************************************************/
typedef enum CK_VIDEO_DISPLAY
{
	CK_VIDEO_TEXTURE		= 0x01000000,	// The video is displayed on a texture
	CK_VIDEO_SCREEN			= 0x02000000	// The video is displayed on the screen
} CK_VIDEO_DISPLAY;

/*******************************************************************
Name: CK_VIDEO_SAVEOPTION
Summary: Possible saving options

See also: CKVideoManager
********************************************************************/
typedef enum CK_VIDEO_SAVEOPTION
{
	CK_VIDEOSAVE_GLOBAL		= 0x00100000,	// Use global saving options
	CK_VIDEOSAVE_INTERNAL	= 0x00200000,	// Save the video file within the .cmo
	CK_VIDEOSAVE_EXTERNAL	= 0x00400000	// Don't save the video file, keep it external
} CK_VIDEO_SAVEOPTION;

/*******************************************************************
Name: CK_VIDEO_FLAGS
Summary: Current video flags.

Remarks:
	+ Part of Video Flags
	+ Bit 1 is reserved for Type that is exclusive
	+ Bit 2 is reserved for Display that is exclusive
	+ Other flags are not exclusive
See also: 
*********************************************************************/
typedef enum CK_VIDEO_AUTHOR_FLAGS
{
	CK_VIDEO_TYPEMASK		= 0xF0000000,	// Mask for video type (CK_VIDEO_TYPE)
	CK_VIDEO_DISPLAYMASK	= 0x0F000000,	// Mask for video display (CK_VIDEO_DISPLAY)
	CK_VIDEO_SAVEOPTIONMASK	= 0x00F00000,	// Mask for saving options (CK_VIDEO_SAVEOPTION)
	CK_VIDEO_AUTHORMASK		= 0x000FFFFF,	// Mask for other options defined here
	
	CK_VIDEO_LOOP			= 0x00000001,	// Should the video loop (need to be able to seek)
	CK_VIDEO_SOUND			= 0x00000002,	// Should we play the video sound.
	CK_VIDEO_ASYNCLOAD		= 0x00000004,	// Should the video be loaded asynchronously
	CK_VIDEO_TEXFILL		= 0x00000008,	// Should the video be resized to fill the texture
	CK_VIDEO_TEXNONPOW2		= 0x00000010,	// Should we change the texture size for best performance
	CK_VIDEO_TEXAUTOSIZE	= 0x00000020,	// Not Implemented.
	CK_VIDEO_TEXAUTOFORMAT	= 0x00000040,	// Should we change the texture format for best performance

	CK_VIDEO_RESERVEDAFLAG0	= 0x00000080,	// Reserved for future use
	CK_VIDEO_RESERVEDAFLAG1	= 0x00000100,	// Reserved for future use
	CK_VIDEO_RESERVEDAFLAG2	= 0x00000200,	// Reserved for future use
	CK_VIDEO_RESERVEDAFLAG3	= 0x00000400,	// Reserved for future use
	CK_VIDEO_RESERVEDAFLAG4	= 0x00000800,	// Reserved for future use

} CK_VIDEO_AUTHOR_FLAGS;

/*******************************************************************
Name: CK_VIDEO_STATE
Summary: Current Video State

Remarks: + Part of Runtime Flags
See also: CKVideoManager, CKVideo
*********************************************************************/
typedef enum CK_VIDEO_STATE
{
	CK_VIDEO_INVALID		= 0x01000000,	// Video source is invalid
	CK_VIDEO_LOADING		= 0x02000000,	// Video source is currently loading
	CK_VIDEO_UNLOADING		= 0x04000000,	// Video source is currently unloading
	CK_VIDEO_PAUSED			= 0x08000000,	// Video is paused
	CK_VIDEO_PLAYING		= 0x10000000,	// Video is playing
	CK_VIDEO_STOPPED		= 0x20000000	// Video is stopped
} CK_VIDEO_STATE;

/*******************************************************************
Name: CK_VIDEO_FLAGS
Summary: Video runtime flags.
following flags.

Remarks:
	+ Part of Runtime Flags
	+ Bit 1 & 2 are reserved for Video State that is exclusive
	+ Other flags are not exclusive
	+ Runtime flags other than state are updated by the source directly.
See also: 
*********************************************************************/
typedef enum CK_VIDEO_RUNTIME_FLAGS
{
	CK_VIDEO_STATEMASK		= 0xFF000000,	// Mask for source state (CK_VIDEO_STATE)
	CK_VIDEO_PREVIEWMASK	= 0x00FF0000,	// Mask for preview state (CK_VIDEO_STATE)
	CK_VIDEO_RUNTIMEMASK	= 0x0000FFFF,	// Mask for other options defined here
		
	CK_VIDEO_FINISHED		= 0x00000001,	// Video is finished and paused
	CK_VIDEO_CANSEEK		= 0x00000002,	// Support video seeking
	CK_VIDEO_CANSPEED		= 0x00000004,	// Support video speed change
	CK_VIDEO_CANSPEEDBACK	= 0x00000008,	// Support video negative speed
	CK_VIDEO_CANLOOP		= 0x00000010,	// Can the video loop
	CK_VIDEO_HASSOUND		= 0x00000020,	// Does the video has sound
	CK_VIDEO_TEXTUREREADY	= 0x00000040,	// Private - Video texture is ready (initialized)
	CK_VIDEO_LOGOFF			= 0x00000080,	// Private - Video logging is stopped

	CK_VIDEO_RESERVEDRFLAG0	= 0x00000100,	// Reserved for future use
	CK_VIDEO_RESERVEDRFLAG1	= 0x00000200,	// Reserved for future use
	CK_VIDEO_RESERVEDRFLAG2	= 0x00000400,	// Reserved for future use
	CK_VIDEO_RESERVEDRFLAG3	= 0x00000800,	// Reserved for future use
	CK_VIDEO_RESERVEDRFLAG4	= 0x00001000,	// Reserved for future use
} CK_VIDEO_RUNTIME_FLAGS;


/*******************************************************************
Name: CK_VIDEO_ERRORS
Summary: Video possible errors

See also: CKVideoManager, CKVideo
********************************************************************/
typedef enum CK_VIDEO_ERRORS
{
	CK_VIDEOERR_GENERIC = 500,			// Support video files

	CK_VIDEOERR_NOPLUGIN,				// No video plugin available
	CK_VIDEOERR_PLUGINCAPS,				// Not supported by the current video engine
	CK_VIDEOERR_VIDEOCAPS,				// Not supported by the current video (compression format...)
	CK_VIDEOERR_TIMEOUT,				// The maximum wait time has been reached.

	CK_VIDEOERR_INPUTTYPE,				// Not supported by the current video input type
	CK_VIDEOERR_OUTPUTTYPE,				// Not supported by the current video output type
	CK_VIDEOERR_MAXCOUNT,				// The maximum number of playing videos has been reached. (1 for screen, no limit for texture one).
	CK_VIDEOERR_TEXTURE,				// The target texture is invalid.
	CK_VIDEOERR_TEXTURELOCK,			// A texture could not be locked (target or capture texture).
	CK_VIDEOERR_SEEKTIME,				// Seek times are not valid (>Duration)
	CK_VIDEOERR_INFO,					// Video info are not valid.

	CK_VIDEOERR_STATE,					// Not supported in the current video state
	CK_VIDEOERR_PREVIEWSTATE,			// Not supported in the current preview state
	CK_VIDEOERR_LOADED,					// Not supported when the video is loaded
	CK_VIDEOERR_LOADING,				// Not supported when the video is loading
	CK_VIDEOERR_NOTLOADED,				// Not supported when the video is not loaded
	CK_VIDEOERR_UNLOADING,				// Not supported when the video is unloading
	CK_VIDEOERR_TEXTUREUSED,			// The texture is already used
	CK_VIDEOERR_PROTOCOL,				// The stream protocol is unknown

	CK_VIDEOERR_RESERVED0,				// Reserved for future use
	CK_VIDEOERR_RESERVED1,				// Reserved for future use
	CK_VIDEOERR_RESERVED2,				// Reserved for future use
	CK_VIDEOERR_RESERVED3,				// Reserved for future use
	CK_VIDEOERR_RESERVED4,				// Reserved for future use
	CK_VIDEOERR_RESERVED5,				// Reserved for future use
	CK_VIDEOERR_RESERVED6,				// Reserved for future use
	CK_VIDEOERR_RESERVED7,				// Reserved for future use
	CK_VIDEOERR_RESERVED8,				// Reserved for future use
	CK_VIDEOERR_RESERVED9,				// Reserved for future use
	CK_VIDEOERR_IMPSTART,				// Error from here are implementation specific

} CK_VIDEO_ERRORS;


#define CK_VIDEOMSG_ENDED		"OnVideoEnded"

#define CK_VIDEOMSG_LOADED		"OnVideoLoaded"

#define CK_VIDEOMSG_UNLOADED	"OnVideoUnloaded"


typedef CKBOOL (*CK_VIDEOEVENTCALLBACK)	(CKVideo* iVideo, CK_VIDEO_EVENT iEvent, void *iEventData, void* iArg);


typedef enum CK_SAVEFLAGS_VIDEO
{
	CK_SAVEFLAGS_VIDEOBASICS		= 0x00001000,	// For common video information
	CK_SAVEFLAGS_VIDEOFILE			= 0x00002000,	// For video file specific information
	CK_SAVEFLAGS_VIDEOLIVE			= 0x00004000,	// For video live specific information
	CK_SAVEFLAGS_VIDEOSTREAM		= 0x00008000,	// For video stream specific information
	CK_SAVEFLAGS_VIDEOTEXTURE		= 0x00010000,	// For video texture specific information
	CK_SAVEFLAGS_VIDEOSCREEN		= 0x00020000,	// For video screen specific information
	CK_SAVEFLAGS_VIDEOUSERFILTER	= 0x00040000,	// Reserved for future use
	CK_SAVEFLAGS_VIDEOINTERNALS		= 0x00080000,	// Reserved for internal use

	CK_SAVEFLAGS_VIDEORESERVED0		= 0x00100000,	// Reserved for future use
	CK_SAVEFLAGS_VIDEORESERVED1		= 0x00200000,	// Reserved for future use
	CK_SAVEFLAGS_VIDEORESERVED2		= 0x00400000,	// Reserved for future use
	CK_SAVEFLAGS_VIDEORESERVED3		= 0x00800000,	// Reserved for future use
	CK_SAVEFLAGS_VIDEORESERVED4		= 0x01000000	// Reserved for future use
} CK_SAVEFLAGS_VIDEO;


typedef enum CK_SAVEVERSION_VIDEO
{
	CK_SAVEVERSION_VIDEO1		= 1,	// Saving version for 3.5
	CK_SAVEVERSION_VIDEO2		= 2,	// Saving version for 3.5 SP1 (Also save camera index)
	CK_SAVEVERSION_VIDEO3		= 3		// Reserved for future use
} CK_SAVEVERSION_VIDEO;

/*************************************************
{filename:CKVideo}
Name: CKVideo

Summary: Base Class for Video management

Remarks:
	+ CKVideo provides only common method for all video types.
See also: CKVideoManager
*************************************************/
class CKVideo : public CKBeObject  
{
friend
	class CKVideoManager;

public:

	/*************************************************
	Summary: Get video information
	Return value: The requested info
	Remarks: 
		+ The video need to be loaded to get these informations
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideoManager, CKVideo, GetLastError, GetErrorDescripion
	*************************************************/
	virtual XString GetErrorDescrition(int iError);

	/*************************************************
	Summary: Reset the last video error to CK_OK.
	
	See also: CKVideoManager, CKVideo, GetLastError, SetLastError, GetErrorDescripion
	*************************************************/
	virtual void ResetLastError() { SetLastError(CK_OK); };

	/*************************************************
	Summary: Get the last video error.
	Return value: last video error.
	
	See also: CKVideoManager, CKVideo, ResetLastError, SetLastError, GetErrorDescripion
	*************************************************/
	virtual int GetLastError();

	/*************************************************
	Summary: Set the last video error.
	Input Argument: 
		+ iError: video error to set as last.
	
	See also: CKVideoManager, CKVideo, ResetLastError, GetLastError, GetErrorDescripion
	*************************************************/
	virtual void SetLastError(int iError);

	/*************************************************
	Summary: Get the current video state.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual DWORD GetAuthorFlags();

	/*************************************************
	Summary: Get the current video options.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual DWORD GetRuntimeFlags();

	/*************************************************
	Summary: Get the current video state.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual CK_VIDEO_STATE GetState();

	/*************************************************
	Summary: Get Video Type: File, Live or Streamed
	
	See also: CKVideoManager, CKVideo
	*************************************************/
	virtual CK_VIDEO_TYPE GetType();

	/*************************************************
	Summary: Set the video Type: File, Live or Streamed
	Input Arguments:
		iType: video type
	Remarks:
		+ The video must be unloaded (state = invalid).
	
	See also: CKVideoManager, CKVideo
	*************************************************/
	virtual CKERROR SetType(CK_VIDEO_TYPE iType);

	/*************************************************
	Summary: Get the current display mode.
	Remarks:
		  + This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual CK_VIDEO_DISPLAY GetDisplayMode();

	/*************************************************
	Summary: Set the video display mode: screen or texture
	Input Arguments:
		iMode: display mode
	Remarks:
		+ The video must be unloaded (state = invalid).	
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetLoop
	*************************************************/
	virtual CKERROR SetDisplayMode(CK_VIDEO_DISPLAY iMode);

	/*************************************************
	Summary: Get video saving options
	
	See also: CKVideoManager, CKVideo
	*************************************************/
	virtual CK_VIDEO_SAVEOPTION GetSaveOptions();

	/*************************************************
	Summary: Set the video saving options
	Input Arguments:
		iOption: saving option to set
	
	See also: CKVideoManager, CKVideo
	*************************************************/
	virtual CKERROR SetSaveOptions(CK_VIDEO_SAVEOPTION iOption);

/*************************************************
Summary: Set the video File name.
Input Arguments:
	iFileName: file name (absolute or relative).
Return Value:
	+ CK_OK if success,
	+ Any video error otherwise.
Remarks:
+ For video file type only
+ The system tries to locate the file in the bitmap searche directories.
It has to exist, otherwise, we return CKERR_INVALIDFILE.
+ The video must be unloaded (state = invalid).
+ This method is thread safe. Never access the variable directly.

See also: CKVideo::GetFileName
*************************************************/
	virtual CKERROR	SetFileName(XString& iFileName);

	/*************************************************
	Summary: Get the video File name.
	Input Arguments:
		Current file name. Path is not in the result.
	Remarks:
		+ For video file type only
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetFilePath, CKVideo::SetFileName
	*************************************************/
	virtual XString	GetFileName();

	/*************************************************
	Summary: Get the video file full path.
	Remarks:
		+ For video file type only
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetFilePath, CKVideo::SetFileName
	*************************************************/
	virtual XString	GetFilePath();

	/*************************************************
	Summary: Set current input ID for the video
	Input Arguments:
		iInputID: ID of the live source to use.
	Return Value:
		+ CK_OK if success,
		+ CKERR_INVALIDFILE if the file is invalid
	Remarks:
		+ For video live only
		+ The possible live source ID are given by the video manager
		+ The video must be unloaded (state = invalid).	
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR SetVideoInputID(int iInputID);

	/*************************************************
	Summary: Get current input ID for the video
	Return Value:
		+ ID of the live source used.
	Remarks:
		+ For video live only
		+ The possible live source ID are given by the video manager
		+ This method has no effect when the video is loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual int GetVideoInputID();

	/*************************************************
	Summary: Set current input ID for the video
	Input Arguments:
		iInputID: ID of the live source to use.
	Return Value:
		+ CK_OK if success,
		+ CKERR_INVALIDFILE if the file is invalid
	Remarks:
		+ For video live only
		+ The possible live source ID are given by the video manager
		+ The video must be unloaded (state = invalid).	
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR SetAudioInputID(int iInputID);

	/*************************************************
	Summary: Get current input ID for the video
	Return Value:
		+ ID of the live source used.
	Remarks:
		+ For video live only
		+ The possible live source ID are given by the video manager
		+ This method has no effect when the video is loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual int GetAudioInputID();

	/*************************************************
	Summary: Set the video url path.
	Input Arguments:
		iUrl: url to the video (http or mms protocol).
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ For video stream only
		+ The video must be unloaded (state = invalid).		
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetFileName
	*************************************************/
	virtual CKERROR	SetUrl(XString& iUrl);

	/*************************************************
	Summary: Get the video url path.
	Remarks:
		+ For video stream only
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetFilePath, CKVideo::SetFileName
	*************************************************/
	virtual XString	GetUrl();

	/*************************************************
	Summary: Set if the video should be loaded asynchronously
	Input Arguments:
		iAsynch: load asynchronously if TRUE, synchronously if FALSE
	Remarks:
		+ The video must be unloaded (state = invalid).				
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::IsLoadingAsynchronous
	*************************************************/
	virtual CKERROR SetAsynchronous(CKBOOL iAsynch);

	/*************************************************
	Summary: Is the video loading asynchronous
	Remarks:
		+ Return TRUE is loading is asynchronous, FALSE otherwise
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::LoadAsynchronously
	*************************************************/
	virtual CKBOOL IsAsynchronous();

	/*************************************************
	Summary: Set the target texture.
	Input Arguments:
		iMode: display mode
	Remarks:
		+ Has no effect if current display mode is screen
		+ The video must be unloaded (state = invalid).		
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetLoop
	*************************************************/
	virtual CKERROR SetTexture(CKTexture* iTarget);

	/*************************************************
	Summary: Get the current target texture
	Remarks:
		+ Return NULL if current display mode is screen
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual CKTexture* GetTexture();

	/*************************************************
	Summary: Set the texture options
	Input Arguments:
		iAutoFormat: should the texture format be choosen by the video engine
		  automatically.
		iAllowConditionalTexture: do we allow the system to create conditional
		  non pow2 textures for this video ?
	Remarks:
		+ Has no effect if current display mode is screen
		+ The video must be unloaded (state = invalid).		
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetLoop
	*************************************************/
	virtual CKERROR SetTextureOptions(CKBOOL iAllowFormatChange, CKBOOL iAllowConditionalTexture);

	/*************************************************
	Summary: Get the current target texture
	Remarks:
		+ Return NULL if current display mode is screen
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetState
	*************************************************/
	virtual void GetTextureOptions(CKBOOL &oAllowFormatChange, CKBOOL &oAllowConditionalTexture);

	/*************************************************
	Summary: Load the video with the current settings.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Load();

	/*************************************************
	Summary: Unload a video.
	Input Arguments:
		iForceSync: when this parameter is true, we force a synchronous 
	      unloading whatever syncrhonous parameter is set on the video.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Unload(CKBOOL iForceSync=FALSE);

	/*************************************************
	Summary: Play the video.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ If the video is not loaded, it loads it. As it may take some time, it
		  is advised to load video before to play.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Play();

	/*************************************************
	Summary: Pause the video.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Pause();

	/*************************************************
	Summary: Stop the video.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Stop();

	/*************************************************
	Summary: Seek a video file.
	Input Arguments:
		iValue: Position where to seek
		iHomogeneous: are we seeking with an homogenous value (between 0 and 1)
		  or with a time in ms.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
		+ Once rewinded, the video is paused
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	Seek(float iValue, CKBOOL iHomogeneous=FALSE);

	/*************************************************
	Summary: Capture the current video image in the given texture.
	Input Arguments:
		iVideo: video to get image from,
		iTexture: texture where to save the captures image
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ This method should not change the target texture format, if needed,
		  we must warn the user by documenting it or returning a value saying
		  so.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	CaptureImage(CKTexture* iTarget);

	/*************************************************
	Summary: Get the current video time.
	Input Arguments:
		iHomogeneous: are we geting an homogenous value (between 0 and 1)
		  or a time in ms.	  
	Return Value:
		+ -1 if current time is not available. >=0 otherwise.
	Remarks:
		+ This method is thread safe.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual float GetPlayingTime(CKBOOL iHomogeneous=FALSE);

	/*************************************************
	Summary: Get the current video frame.
	Return Value:
		+ -1 if current frame is not available. >=0 otherwise.
	Remarks:
		+ This method is thread safe.
	
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual int GetPlayingFrame();

	/*************************************************
	Summary: Set the video destination rect
	Input Arguments:
		iDestination: destination rect,
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetDestination
	*************************************************/
	virtual CKERROR SetDestination(VxRect& iDestination);

	/*************************************************
	Summary: Get the video destination rect
	Return Value:
		Desctination rect.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetDestination
	*************************************************/
	virtual VxRect GetDestination();

/*************************************************
Summary: Set the video 'Fill Texture' option
Input Arguments:
iFill: option value
Remarks:
+ When the video must fill the texture, it is resized each frame to
fit the texture video memory size. This may be usefull, but is less
less efficient.
+ This method is thread safe. Never access the variable directly.

See also: CKVideo::GetFillTexture
*************************************************/
	virtual CKERROR SetFillTexture(CKBOOL iFill);

/*************************************************
Summary: Get the video 'Fill Textrure' option.
Return Value:
Destination rect.
Remarks:
+ When the video must fill the texture, it is resized each frame to
fit the texture video memory size. This may be useful, but is less
less efficient.
+ This method is thread safe. Never access the variable directly.

See also: CKVideo::SetFillTexture
*************************************************/
	virtual CKBOOL GetFillTexture();

	/*************************************************
	Summary: Should we play the video sound ?
	Input Arguments:
		iEnable: TRUE to play sound, FALSE not to.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetVolume, CKVideo::SetBalance, CKVideo::GetBalance
	*************************************************/
	virtual CKERROR EnableAudio(CKBOOL iEnable);

	/*************************************************
	Summary: Is the video sound played ?
	Return Value:
		TRUE if sound is player, FALSE otherwise
	Remarks:
		+ If the video has no sound channed, the method will return FALSE
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetVolume, CKVideo::SetBalance, CKVideo::GetBalance
	*************************************************/
	virtual CKBOOL IsAudioEnabled();

	/*************************************************
	Summary: Set the sound volume
	Input Arguments:
		iVolume: percentage value between 0 and 1
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetVolume, CKVideo::SetBalance, CKVideo::GetBalance
	*************************************************/
	virtual CKERROR SetVolume(float iVolume);

	/*************************************************
	Summary: Get the sound volume.
	Return Value:
		Sound Volume between 0 and 1.
	Remarks:
		  + This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetVolume, CKVideo::SetBalance, CKVideo::GetBalance
	*************************************************/
	virtual float GetVolume();

	/*************************************************
	Summary: Set the sound balance
	Input Arguments:
		iBalance: value between -1 (left) and 1 (right)
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetBalance, CKVideo::SetVolume, CKVideo::GetVolume
	*************************************************/
	virtual CKERROR SetBalance(float iBalance);

	/*************************************************
	Summary: Get the sound balance.
	Return Value:
		Sound Balance between -1 and 1.
	Remarks:
		  + This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetBalance, CKVideo::SetVolume, CKVideo::GetVolume
	*************************************************/
	virtual float GetBalance();

	/*************************************************
	Summary: Set the video 'Loop' option value
	Input Arguments:
		iLoop: should the video loop ?
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::GetLoop
	*************************************************/
	virtual CKERROR SetLoop(CKBOOL iLoop);

	/*************************************************
	Summary: Get the video 'Loop' option value
	Return Value:
		+ iLoop: should the video loop ?
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetLoop
	*************************************************/
	virtual CKBOOL GetLoop();

/*************************************************
Summary: Change the speed of the speed.
Input Arguments:
iSpeed: speed to set.
Return Value:
+ CK_OK if success,
+ Any video error otherwise.
Remarks:
+ Speed cannot be changed on every video. It depends on the codec it
uses. Check the video capabilities to know if you can modify the
speed.
+ The video must be loaded.
+ This method is thread safe. Never access the variable directly.

See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
*************************************************/
	virtual CKERROR	SetSpeed(float iSpeed);

	/*************************************************
	Summary: Get the video 'Loop' option value
	Return Value:
		+ The video speed
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetLoop
	*************************************************/
	virtual float GetSpeed();

	/*************************************************
	Summary: Set the video new seeking time for next restart
	Remarks:
		+ When value is <0, no change is done to the value
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetLoop
	*************************************************/
	virtual CKERROR SetLimitTimes(float iStartTime, float iStopTime);

	/*************************************************
	Summary: Get the video starting time
	Output Arguments:
		+ oStartTime: variable receiving the start time. May be NULL.
		+ oStopTime: variable receiving the stop time. May be NULL.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideo::SetLoop
	*************************************************/
	virtual void GetLimitTimes(float* oStartTime, float* oStopTime);

	/*************************************************
	Summary: Set the loading thread priority
	Return value: The requested info
	Remarks: 
		+ The video need to be loaded to get these informations
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual void SetVideoPriority(VXTHREAD_PRIORITY iPriority);

	/*************************************************
	Summary: Get the loading thread priority
	Return value: The requested info
	Remarks: 
		+ The video need to be loaded to get these informations
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual VXTHREAD_PRIORITY GetVideoPriority();

	/*************************************************
	Summary: Get video information
	Return value: The requested info
	Remarks: 
		+ The video need to be loaded to get these informations
		+ This method is thread safe. Never access the variable directly.
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual const CKVideoInfo* GetVideoInfo();

	/*************************************************
	Summary: Add / Remove a callback on a given event
	Return value: The requested info
	Remarks: 
		+ The new image callbacks are only supported when playing the video on a
		texture, not on the screen.
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual CKBOOL AddEventCallBack(CK_VIDEO_EVENT iEvent, CK_VIDEOEVENTCALLBACK iFunction, void *iArgument);
	
	virtual CKBOOL RemoveEventCallBack(CK_VIDEO_EVENT iEvent, CK_VIDEOEVENTCALLBACK iFunction);

	CKVideoManager*			m_VideoManager;		// Current Video Manager

////////////////////////////////////////////////////////////////////////
#ifdef DOCJETDUMMY // DOCJET secret macro
#else
	DWORD					m_AuthorFlags;		// Current Video Author Flags (including Type, Target)
	VxRect					m_Destination;		// Destination rect for screen and texture display
	VxColor					m_BorderColor;		// Color used for video borders
	CKTexture*				m_Texture;			// Target for texture display	
	float					m_Volume;			// Sound Volume
	float					m_Balance;			// Sound Balance
	float					m_Speed;			// Video Speed
	float					m_StartTime;		// Video Start Time in ms
	float					m_StopTime;			// Video Stop Time in ms

	XString					m_FileName;			// Video File Path
	XString					m_Url;				// Video Stream Address. Must contain protocole.
	int						m_VideoInputID;		// Video Live Source ID
	int						m_AudioInputID;		// Video Live Source ID
		
	// Runtime Informations
	CKVideoInfo				m_VideoInfo;		// Information on the video
	DWORD volatile 			m_RuntimeFlags;		// Current Video Runtime Flags (include States, Caps...)
	float					m_SourceTime;		// Current source time
	float					m_PreviewTime;		// Current preview time
	int						m_LastError;		// Last video error

	/*************************************************
	Summary: Verify if the current video plugin supports our video
	
	Return Value: video caps
	See also: CKVideoPlugin,CK_VIDEOMANAGER_CAPS
	*************************************************/
	virtual CKERROR VerifyPluginCaps();

	/*************************************************
	Summary: Set the video border color
	Input Arguments:
		iColor: border color,
	Remarks:
		+ This method is thread safe. Never access the variable directly.
		+ Not supported yet
	
	See also: CKVideo::GetBorderColor
	*************************************************/
	virtual CKERROR SetBorderColor(VxColor& iColor);

	/*************************************************
	Summary: Get the video border color
	Return Value:
		Video border color.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
		+ Not supported yet
	
	See also: CKVideo::SetBorderColor
	*************************************************/
	virtual VxColor GetBorderColor();

	/*************************************************
	Summary: Set the video source and preview
	
	Remarks:
		+ This method is thread safe.
	See also: CKVideo::GetState
	*************************************************/
	virtual void SetSource(void* iSource);
	virtual void SetPreview(void* iPreview);
	
	/*************************************************
	Summary: Get the current video major state.
	
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	See also: CKVideo::GetState
	*************************************************/
	virtual inline CKBOOL IsSourceValid();
	virtual inline CKBOOL IsPreviewValid();

	/*************************************************
	Summary: Modify the current .
	
	Input Arguments:
		iAdd: flags to add. Can be one or more of CK_VIDEO_AUTHOR_FLAGS.
		iRemove: flags to remove. Can be one or more of CK_VIDEO_AUTHOR_FLAGS.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	See also: CKVideo::GetState
	*************************************************/
	virtual void ModifyAuthorFlags(DWORD iAdd, DWORD iRemove);

	/*************************************************
	Summary: Modify the current runtime flags.
	
	Input Arguments:
		iAdd: flags to add. Can be one or more of CK_VIDEO_RUNTIME_FLAGS.
		iRemove: flags to remove. Can be one or more of CK_VIDEO_RUNTIME_FLAGS.
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	See also: CKVideo::GetState
	*************************************************/
	virtual void ModifyRuntimeFlags(DWORD iAdd, DWORD iRemove);

	/*************************************************
	Summary: Get the current preview state.
	
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	See also: CKVideo::SetState
	*************************************************/
	virtual CK_VIDEO_STATE GetPreviewState();

	/*************************************************
	Summary: Set current video state
	
	Input Arguments:
		iState: video state
	Remarks:
		+ This method is thread safe. Never access the variable directly.
	See also: CKVideo::GetState
	*************************************************/
	virtual void SetState(CK_VIDEO_STATE iState);
	virtual void SetPreviewState(CK_VIDEO_STATE iState);

	/*************************************************
	Summary: Load the video with the current settings.
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	LoadPreview(void* iPreviewWindow=NULL);

	/*************************************************
	Summary: Unload a video.
	
	Input Arguments:
		iForceSync: when this parameter is true, we force a synchronous
		  unloading whatever syncrhonous parameter is set on the video.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	UnloadPreview(CKBOOL iForceSync=FALSE);

	/*************************************************
	Summary: Play the video.
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ If the video is not loaded, it loads it. As it may take some time, it
		  is advised to load video before to play.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	PlayPreview();

	/*************************************************
	Summary: Pause the video.
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	PausePreview();

	/*************************************************
	Summary: Stop the video.
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	StopPreview();

	/*************************************************
	Summary: Seek a video file.
	
	Input Arguments:
		iValue: Position where to seek
		iHomogeneous: are we seeking with an homogeneous value (between 0 and 1)
		  or with a time in ms.
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ The video must be loaded.
		+ Once rewinded, the video is paused
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	SeekPreview(float iValue, CKBOOL iHomogeneous=FALSE);

	/*************************************************
	Summary: Get the current preview time.
	
	Input Arguments:
		iHomogeneous: are we geting an homogenous value (between 0 and 1)
		  or a time in ms.
	Return Value:
		+ -1 if current time is not available. A value in ms otherwise.
	Remarks:
		+ This method is thread safe.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual float GetPreviewTime(CKBOOL iHomogeneous=FALSE);

	/*************************************************
	Summary: Post a message to the video
	
	Input Arguments:
		iMsg: message to post
	See also: CKVideoManager
	*************************************************/
	virtual void PostVideoMsg(CKSTRING iMsg);

	/*************************************************
	Summary: Does the video has a callback defined for the given event.
	Return value: TRUE if has at least one callback, FALSE otherwise
	Remarks: 
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual CKBOOL HasEventCallBacks(CK_VIDEO_EVENT iEvent);

	/*************************************************
	Summary: Add / Remove a callback on a given event
	Return value: The requested info
	Remarks: 
	
	See also: CKVideoManager, CKVideo, CKVideoLive, CKVideo
	*************************************************/
	virtual void ExecuteEventCallBacks(CK_VIDEO_EVENT iEvent, void *iEventData);

	/*************************************************
	Summary: Update all times
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remarks:
		+ Update m_SourceTime,
		+ Update m_PreviewTime.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual CKERROR	InternalUpdate();
	
	/*************************************************
	Summary: Free the video memory of the target texture
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual void FreeTargetTexture();

	/*************************************************
	Summary: Check that the video texture do not belong to the ToDelete
	         input list. If it does, unload the video.
	
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	See also: CKVideoManager::CreateSource, CKVideoManager::HasSource, CKVideo
	*************************************************/
	virtual void IsTextureToBeDeleted(XArray<CKTexture*> &iToDelete);

	//-------------------------------------------------
	// Virtools Play/Pause Management: no state update
	virtual void			InternalPlay			();
	virtual void			InternalStop			();
	virtual void			InternalPause			();
	virtual void			InternalContinue		();
	
	//-------------------------------------------------
	// Constructor / Destructor
	CKVideo(CKContext *iCtx,CKSTRING iName=NULL);
	virtual ~CKVideo();

	//-------------------------------------------------
	// VSL Binding
	static void				BindForVSL(CKContext* iCtx);

	//-------------------------------------------------
	// Class General
	virtual CK_CLASSID		GetClassID();											
	virtual int				GetMemoryOccupation();								
	
	//-------------------------------------------------
	// Load / Save
	virtual CKStateChunk*	Save(CKFile *iFile, CKDWORD iFlags);
	virtual CKERROR			Load(CKStateChunk *iChunk, CKFile* iFile);
	virtual CKERROR			Copy(CKObject& o,CKDependenciesContext& context);

	//-------------------------------------------------
	// Class Registering 
	static CKSTRING			GetClassName();
	static int				GetDependenciesCount(int iMode);
	static CKSTRING			GetDependencies(int i,int iMode);
	static void				Register();
	static CKVideo*			CreateInstance(CKContext *iCtx);
	static void				ReleaseInstance(CKContext* iCtx, CKVideo* iVideo);
	static CK_CLASSID		m_ClassID;
	
	//-------------------------------------------------
	// Dynamic Cast method. Returns NULL if the object can't be casted
	static CKVideo* Cast(CKObject* iObj) 
	{
		return CKIsChildClassOf(iObj,CKCID_VIDEO)?(CKVideo*)iObj:NULL;
	}


////////////////////////////////////////////////////////////////////////
// User Undocumented Variables
////////////////////////////////////////////////////////////////////////
	VxMutex				m_VideoLock;		// A mutex to protect some video members access.
	void*				m_Source;			// Source used by the Video Manager
	void*				m_Preview;			// Source used by the Preview Window
	void*				m_PreviewWindow;	// Preview Window
	int					m_VideoPriority;	// Asynchronous Video Priority
	DWORD				m_EngineData;		// Data that can be used by the implementation Engine.

	
	typedef struct CallbkContainer {
		CK_VIDEOEVENTCALLBACK	m_Callbk; 
		void*					m_Argument;
	} CallbkContainer;
	XArray<CallbkContainer>		m_EventCallbacks[CK_VIDEOEVENT_COUNT];

////////////////////////////////////////////////////////////////////////
// Methods added after 3.5.0.24
////////////////////////////////////////////////////////////////////////
	int			m_VideoInputFormatID;	// Video Live Input Format. Keep default value when =-1
	float		m_VideoInputFrequency;	// Video Live Input Frequency. Keep default value when =-1
	VxMutex		m_CallbackLock;			// A mutex to protect the video callback array.

	/*************************************************
	Summary: Get the video format to use for live input.
	Argument:
		+ oFormatID: format used (<0 means default). This value can be NULL.
		+ oFrequency: frequency used (<=0 means default). This value can be NULL.
	
	See also: SetVideoInputFormat, CKVideoPlugin::GetVideoInputCount, CKVideoPlugin::GetVideoInputFormatCount
	*************************************************/
	virtual void GetVideoInputFormat(int* oFormatID, float* oFrequency=NULL);

	/*************************************************
	Summary: Set the video format to use for live input.
	Argument:
		+ iFormatID: index of the format to set (<0 not to modify it)
		+ iFrequency: frequency to use (<=0 not to modify it)
	Return Value:
		+ CK_OK if success,
		+ Any video error otherwise.
	Remark:
		+ When the video is unloaded, there is no restriction, you can set any
		available format.
		+ When the video is loaded, it has to be stopped to be able to change
		the input format and switching from one format to another is not always
		possible (this depends on your camera drivers). For instance, it is often
		impossible to switch from a RGB format to a YUV format while the video is
		loaded. You should therefore test the capabilities of your camera if you 
		need to forbid the non working format changes.
	
	See also: GetVideoInputFormat, CKVideoPlugin::GetVideoInputCount, CKVideoPlugin::GetVideoInputFormatDesc
	*************************************************/
	virtual CKERROR SetVideoInputFormat(int iFormatID, float iFrequency=-1);

	
	virtual CKERROR PrepareDependencies(CKDependenciesContext& context, CKBOOL iCaller = TRUE);

#endif // DOCJET secret macro
};

#endif




