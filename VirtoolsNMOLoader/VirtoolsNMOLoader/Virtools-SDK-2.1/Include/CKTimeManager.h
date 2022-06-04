/*************************************************************************/
/*	File : CKTimemanager.h												 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	A Manager to provide the same acces to time to behaviors			 */
/*	The time as seen by behaviors can be the real time or clamped		 */
/*	and stretched to slow or accelerate	"real" time						 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKTIMEMANAGER_H
#define CKTIMEMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKBaseManager.h"

#if _MSC_VER > 1000
#pragma warning(disable : 4786)
#endif

/*******************************************************
{filename:CK_FRAMERATE_LIMITS}
Summary:Time limits settings

Remarks:
+ The default settings for the process loop is to
execute one behavioral process then one rendering (with
a synchonization to screen refresh rate).
+ Behavioral process and Rendering can have a maximum frame rate
in which case it is guaranteed the rendering and behavioral loop
will not be called more than the given fps time per second. The asked frame rate
may not be achieved but it will not be exceed.
See also:CKTimeManager
********************************************************/
typedef enum CK_FRAMERATE_LIMITS
{
    CK_RATE_NOP        = 0x00000000,
    CK_BEHRATE_SYNC    = 0x00000001,	// Behavioral Rate is synchronized to frame rate
    CK_BEHRATE_LIMIT   = 0x00000004,	// Behavioral Rate is limited to a maximum value (see CKTimeManager::SetBehavioralRateLimit)
    CK_BEHRATE_MASK    = 0x0000000F,	// Mask for values concerning behavioral rate

    CK_FRAMERATE_SYNC  = 0x00000010,	// Frame Rate is synchronized to screen refresh rate (Render function will wait for vertical blank signal)
    CK_FRAMERATE_FREE  = 0x00000020,	// Frame Rate is free. Rendering occurs at the maximum possible rate.
    CK_FRAMERATE_LIMIT = 0x00000040,	// Frame Rate is limited to a maximum value (see CKTimeManager::SetFrameRateLimit)
    CK_FRAMERATE_MASK  = 0x000000F0,	// Mask for values concerning frame rate
} CK_FRAMERATE_LIMITS;

/**************************************************************************
Name: CKTimeManager

Summary: Story and execution time management


Remarks:

+ The Time manager stores and updates time for the story but also real time.

+ Each frame the time manager computes the time elapsed since last process loop. This time
is then multiplied by the scale factor and clamped to a minimum
and maximum value to avoid extreme values which could produces jump in the playback if its time dependant.
This delta value is added to the current time to compute the absolute time elapsed since
the playback started.

+ The time as it appears to behaviors can be multiplied by a scale
factor and also be clamped to minimum and maximum values.

+ The CKContext::Pause,Reset,Play automatically call
the appropriate functions in CKTimeManager to pause and restart the timers.

+ Since the time taken to render a scene and execute the scripts can greatly vary
from one frame to another, it is recommended to perform time dependant actions rather
the frame dependant.

+ The unique instance of the time manager can be retrieved through the  CKContext::GetTimeManager method

See also: Time Management,Creating a Standalone Application,CKContext::GetTimeManager
*****************************************************************************/
class CKTimeManager : public CKBaseManager
{
public:
    //*********** Processing **********************

    /**************************************************
    Summary: Returns the number of process loop (call to CKContext::Process()) since startup.

    See also: GetTime,GetAbsoluteTime,GetLastDeltaTime,SetLastDeltaTime
    ***************************************************/
    CKDWORD GetMainTickCount() { return m_MainTickCount; }

    /**************************************************
    Summary: Returns time elapsed since startup in milliseconds.

    Remarks:
    + Use a high-precision timer to return the time elapsed since creation of this context.
    + The returned time does NOT take scale factors,minimum and maximum values into account.
    + The behaviors should not use this function but rather GetAbsoluteTime which takes scale factor into account.
    Use only this function to be relative to the "real" time.
    See also: GetAbsoluteTime,GetLastDeltaTime,SetLastDeltaTime
    *****************************************************/
    float GetTime() { return m_StartupChrono.Current(); }

    /*******************************************************
    Summary: Returns time elapsed since last frame in milliseconds.

    Remarks:
        + The returned time takes scale factors,minimum and maximum values into account.
    See also: GetTime,GetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,SetTimeScaleFactor
    *********************************************************/
    float GetLastDeltaTime() { return m_DeltaTime; }

    /*******************************************************
    Summary: Returns real time elapsed since last frame in milliseconds.

    Remarks:
    + The returned time does NOT takes scale factors,minimum and maximum values into account.
    + The behaviors should not use this function but rather GetLastDeltaTime which takes scale factor into account.
    Use only this function to be relative to the "real" time. For example if between two frames an action takes
    a very long time, the GetLastDeltaTime() method will return the maximum allowed delta time (GetMaximumDeltaTime)
    while this method will return the exact time elapsed.
    See also: GetTime,GetLastDeltaTime,SetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,SetTimeScaleFactor
    *********************************************************/
    float GetLastDeltaTimeFree() { return m_DeltaTimeFree; }

    /*********************************************************
    Summary: Returns time elapsed since last call to CKContext::Reset() in milliseconds.

    Remarks:
        + The returned time takes scale factors,minimum and maximum values into account.
    See also: GetTime,GetLastDeltaTime,SetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,SetTimeScaleFactor
    **********************************************************/
    float GetAbsoluteTime() { return m_PlayTime; }

    //*********** Time scale factors **********************

    /*********************************************************
    Summary: Sets the time scaling factor.

    Arguments:
        mulfactor: A scale factor by which real time value will be multiplied.
    Remarks:
        + All time dependant operations in behaviors use the time manager methods,
        with a scale factor you can simulate a slower of faster time flow.
    See also: GetTime,GetLastDeltaTime,SetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,GetTimeScaleFactor
    *********************************************************/
    void SetTimeScaleFactor(float mulfactor) { m_TimeScaleFactor = mulfactor; }
    /*********************************************************
    Summary: Gets the time scaling factor.

    Return Value:	Current scale factor by which real time value is multiplied.
    Remarks:
        + All time dependant operations in behaviors use the time manager methods,
        with a scale factor you can simulate a slower of faster time flow.
    See also: GetTime,GetLastDeltaTime,SetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,SetTimeScaleFactor
    *********************************************************/
    float GetTimeScaleFactor() { return m_TimeScaleFactor; }

    //------------ Limits --------------------

    /****************************************************
    Summary: Gets the rendering and behavioral frame rate limits.

    Remarks:
        + Returns a combination of CK_FRAMERATE_LIMITS flags
        specifying the behavioral and frame rate limits.
    See Also:CK_FRAMERATE_LIMITS,ChangeLimitOptions
    *****************************************************/
    CKDWORD GetLimitOptions() { return m_LimitOptions; }
    /****************************************************
    Summary: Gets the frame rate limit

    Return Value:
        Maximum render loops per second.
    Remarks:
        + This value is only used when the frame rate is limited (GetLimitOptions() & CK_FRAMERATE_LIMIT)
    See Also:SetFrameRateLimit,GetLimitOptions,ChangeLimitOptions
    *****************************************************/
    float GetFrameRateLimit() { return m_LimitFrameRate; }
    /****************************************************
    Summary: Gets the behavioral rate limit

    Return Value:
        Maximum process loops per second.
    Remarks:
        + This value is only used when the behavioral rate is limited (GetLimitOptions() & CK_BEHRATE_LIMIT)
    See Also:SetBehavioralRateLimit,GetLimitOptions,ChangeLimitOptions
    *****************************************************/
    float GetBehavioralRateLimit() { return m_LimitBehRate; }
    /****************************************************
    Summary: Gets the minimum delta time.

    Remarks:
    + Each frame the time manager computes the time elapsed since last process loop. This time
    is then multiplied by the scale factor. This time are also be clamped to a minimum
    and maximum value to avoid extreme values.
    + Default minimum delta time = 1 millisecond.
    See Also:SetMinimumDeltaTime,GetMaximumDeltaTime
    *****************************************************/
    float GetMinimumDeltaTime() { return m_MinimumDeltaTime; }
    /****************************************************
    Summary: Gets the maximum delta time.

    Remarks:
    + Each frame the time manager computes the time elapsed since last process loop. This time
    is then multiplied by the scale factor. This time are also be clamped to a minimum
    and maximum value to avoid extreme values.
    + Default maximum delta time = 200 millisecond.
    See Also:GetMinimumDeltaTime,SetMaximumDeltaTime
    *****************************************************/
    float GetMaximumDeltaTime() { return m_MaximumDeltaTime; }
    /****************************************************
    Summary: Changes the rendering and behavioral frame rate limits.

    Arguments:
        FpsOptions:  CK_FRAMERATE_SYNC to synchronize to screen refresh rate (Render
        function will wait for vertical blank signal),CK_FRAMERATE_FREE to render
        at the maximum possible rate or CK_FRAMERATE_LIMIT to set a maximum frame rate.
        BehOptions: CK_BEHRATE_SYNC to synchronise the process loop with rendering (a behavioral processing is done
        before each rendering) or CK_BEHRATE_LIMIT to set a maximum behavioral rate.
    Remarks:
        + If the frame or behavioral rate is limited, the limits can be set by the
        SetFrameRateLimit or SetBehavioralRateLimit methods.
    See Also:CK_FRAMERATE_LIMITS,SetBehavioralRateLimit,SetFrameRateLimit,GetLimitOptions
    *****************************************************/
    void ChangeLimitOptions(CK_FRAMERATE_LIMITS FpsOptions, CK_FRAMERATE_LIMITS BehOptions = CK_RATE_NOP);
    /****************************************************
    Summary: Sets the frame rate limit

    Arguments:
        FRLimit: Maximum render loops per second.
    Remarks:
        + This value is only used when the frame rate is limited (GetLimitOptions() & CK_FRAMERATE_LIMIT)
    See Also:GetFrameRateLimit,GetLimitOptions,ChangeLimitOptions
    *****************************************************/
    void SetFrameRateLimit(float FRLimit);
    /****************************************************
    Summary: Sets the behavioral rate limit

    Arguments:
        BRLimit: Maximum process loops per second.
    Remarks:
        + This value is only used when the behavioral rate is limited (GetLimitOptions() & CK_BEHRATE_LIMIT)
    See Also:GetBehavioralRateLimit,GetLimitOptions,ChangeLimitOptions
    *****************************************************/
    void SetBehavioralRateLimit(float BRLimit);
    /****************************************************
    Summary: Sets the minimum delta time.

    Arguments:
        DtMax: Minimum value for the delta time in milliseconds.
    Remarks:
    + Each frame the time manager computes the time elapsed since last process loop. This time
    is then multiplied by the scale factor. This time are also be clamped to a minimum
    and maximum value to avoid extreme values.
    + Default minimum delta time = 1 millisecond.
    See Also:SetMaximumDeltaTime,GetMinimumDeltaTime
    *****************************************************/
    void SetMinimumDeltaTime(float DtMin);
    /****************************************************
    Summary: Sets the maximum delta time.

    Arguments:
        DtMax: Maximum value for the delta time in milliseconds.
    Remarks:
    + Each frame the time manager computes the time elapsed since last process loop. This time
    is then multiplied by the scale factor. This time are also be clamped to a minimum
    and maximum value to avoid extreme values.
    + Default maximum delta time = 200 millisecond.
    See Also:SetMinimumDeltaTime,GetMaximumDeltaTime
    *****************************************************/
    void SetMaximumDeltaTime(float DtMax);

    /*******************************************************
    Summary: Set the DeltaTime for custom use.

    Remarks:
        +This is usefull for special purposes.
    See also: GetTime,GetLastDeltaTime,SetLastDeltaTime,SetMinimumDeltaTime,SetMaximumDeltaTime,SetTimeScaleFactor
    *********************************************************/
    void SetLastDeltaTime(float DtSet) { m_DeltaTime = DtSet; }

    //----------------------------------------------------
    //--  Runtime

    /***************************************************************
    Summary: Returns the time to wait to respect the time manager limits.

    Arguments:
        TimeBeforeRender: Time in milliseconds to wait before performing
        a rendering.
        TimeBeforeBeh: Time in milliseconds to wait before performing
        a process loop.
    Remarks:
    + This method returns the time to wait in milliseconds
    to perfom a rendering or a process loop so that the
    time manager limits are respected.
    + If the rendering or the behavioral process are not limited to given frame rate the
    method returns 0 values.
    Example
        // This is a standard loop that takes potential frame rate limitats
        // into account

        CKTimeManager* TheTimeManager = TheContext->GetTimeManager();
        while(1) {
                float TimeBeforeRender=0,TimeBeforeBeh=0;

                // Ask the time manager the time to wait before
                // doing a render and a process
                // if the time is positive we keep on looping
                // without doing anything
                TheTimeManager->GetTimeToWaitForLimits(TimeBeforeRender,TimeBeforeBeh);

                BOOL DoBehaviors	= (TimeBeforeBeh <=0);
                BOOL DoRender		= (TimeBeforeRender  <=0);

                // If a rendering or a behavioral process is done
                // we restart the chronos for the next iteration
                TheTimeManager->ResetChronos(DoRender,DoBehaviors);

                if (DoBehaviors)	TheCKContext->Process();
                if (DoRender) 		TheRenderContext->Render();
        }
    See Also:Creating a Standalone Application
    *****************************************************/
    void GetTimeToWaitForLimits(float &TimeBeforeRender, float &TimeBeforeBeh);

    void ResetChronos(CKBOOL RenderChrono, CKBOOL BehavioralChrono);

//-------------------------------------------------------------------------
// Internal functions
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

    virtual CKERROR PreProcess();
    virtual CKERROR OnCKPlay();
    virtual CKERROR OnCKPause();
    virtual CKERROR OnCKReset();
    virtual CKERROR PostClearAll();
    virtual CKBOOL IsPaused();
    virtual CKERROR LoadData(CKStateChunk *chunk, CKFile *LoadedFile);
    virtual CKStateChunk *SaveData(CKFile *SavedFile);

    virtual CKDWORD GetValidFunctionsMask()
    {
        return CKMANAGER_FUNC_OnCKPause |
               CKMANAGER_FUNC_OnCKPlay |
               CKMANAGER_FUNC_OnCKReset |
               CKMANAGER_FUNC_PostClearAll |
               CKMANAGER_FUNC_PreProcess;
    }

    virtual int GetFunctionPriority(CKMANAGER_FUNCTIONS Function)
    {
        if (Function != CKMANAGER_FUNC_PreProcess)
            return DEFAULT_MANAGERFUNC_PRIORITY;
        else
            return MAX_MANAGERFUNC_PRIORITY;
    }

    CKTimeManager(CKContext *Context);

protected:
    CKDWORD m_MainTickCount;
    float m_LastTime, m_CurrentTime;
    float m_PlayTime, m_DeltaTime;

    //----------- Limits
    float m_LimitFrameRate;
    float m_LimitBehRate;
    float m_MaximumDeltaTime;
    float m_MinimumDeltaTime;
    CKDWORD m_LimitOptions;

    //--- Calculated from FPS Limitations
    float m_LimitFPSDeltaTime;
    float m_LimitBehDeltaTime;

    CKBOOL m_Paused;
    float m_TimeScaleFactor;

    VxTimeProfiler m_BehChrono;
    VxTimeProfiler m_FpsChrono;
    VxTimeProfiler m_StartupChrono;

    float m_DeltaTimeFree;
#endif // Docjet secret macro
};

#endif
