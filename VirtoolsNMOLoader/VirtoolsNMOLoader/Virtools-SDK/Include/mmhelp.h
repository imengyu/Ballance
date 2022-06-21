#pragma once

///////////////////////////////////////////////////////////
// ------------ Multi Monitor aware tool functions --------
///////////////////////////////////////////////////////////
//
//  GetMonitorRect
//
//  gets the "screen" or work area of the monitor that the passed
//  window is on.  this is used for apps that want to clip or
//  center windows.
//
//  the most common problem apps have with multimonitor systems is
//  when they use GetSystemMetrics(SM_C?SCREEN) to center or clip a
//  window to keep it on screen.  If you do this on a multimonitor
//  system the window we be restricted to the primary monitor.
//
//  this is a example of how you used the new Win32 multimonitor APIs
//  to do the same thing.
//
void GetMonitorRect(HWND hwnd, LPRECT prc, BOOL fWork);

//
// ClipRectToMonitor
//
// uses GetMonitorRect to clip a rect to the monitor that
// the passed window is on.
//
void ClipRectToMonitor(HWND hwnd, RECT *prc, BOOL fWork);

//
// CenterRectToMonitor
//
// uses GetMonitorRect to center a rect to the monitor that
// the passed window is on.
//
void CenterRectToMonitor(HWND hwnd, RECT *prc, BOOL fWork);

//
// CenterWindowToMonitor
//
void CenterWindowToMonitor(HWND hwndP, HWND hwnd, BOOL fWork);

//
// ClipWindowToMonitor
//
void ClipWindowToMonitor(HWND hwndP, HWND hwnd, BOOL fWork);

//
// IsWindowOnScreen
//
BOOL IsWindowOnScreen(HWND hwnd);

//
// MakeSureWindowIsVisible
//
void MakeSureWindowIsVisible(HWND hwnd);
