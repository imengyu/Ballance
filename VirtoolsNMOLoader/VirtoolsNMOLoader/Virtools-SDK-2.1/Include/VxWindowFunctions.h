/*************************************************************************/
/*	File : VxWindowFunction.h											 */
/*	Author :  Nicolas Galinotti											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef __VXWINDOWFUNCTION_H__
#define __VXWINDOWFUNCTION_H__

#include "XUtil.h"
#include "VxMathDefines.h"

class XString;
struct VxImageDescEx;

// KeyBoard Functions
VX_EXPORT char VxScanCodeToAscii(DWORD scancode, unsigned char keystate[256]);
VX_EXPORT int VxScanCodeToName(DWORD scancode, char *keyName);

// Cursor function
/**************************************************
{filename:VXCURSOR_POINTER}
Summary:Appearance of mouse cursor.

See Also:VxSetCursor
***************************************************/
typedef enum VXCURSOR_POINTER
{
    VXCURSOR_NORMALSELECT = 1,	// Display the standard arrow cursor
    VXCURSOR_BUSY         = 2,	// Display the busy (hourglass) cursor
    VXCURSOR_MOVE         = 3,	// Display the move cursor
    VXCURSOR_LINKSELECT   = 4	// Display the link select (hand) cursor
} VXCURSOR_POINTER;

VX_EXPORT int VxShowCursor(BOOL show);
VX_EXPORT BOOL VxSetCursor(VXCURSOR_POINTER cursorID);

VX_EXPORT WORD VxGetFPUControlWord();
VX_EXPORT void VxSetFPUControlWord(WORD Fpu);
// Disable exceptions,round to nearest,single precision
VX_EXPORT void VxSetBaseFPUControlWord();

//-------Library Function

VX_EXPORT void VxAddLibrarySearchPath(char *path);
VX_EXPORT BOOL VxGetEnvironmentVariable(char *envName, XString &envValue);
VX_EXPORT BOOL VxSetEnvironmentVariable(char *envName, char *envValue);

VX_EXPORT DWORD VxEscapeURL(char *InURL, XString &OutURL);
VX_EXPORT void VxUnEscapeUrl(XString &str);

//------ Window Functions
VX_EXPORT WIN_HANDLE VxWindowFromPoint(CKPOINT pt);
VX_EXPORT BOOL VxGetClientRect(WIN_HANDLE Win, CKRECT *rect);
VX_EXPORT BOOL VxGetWindowRect(WIN_HANDLE Win, CKRECT *rect);
VX_EXPORT BOOL VxScreenToClient(WIN_HANDLE Win, CKPOINT *pt);
VX_EXPORT BOOL VxClientToScreen(WIN_HANDLE Win, CKPOINT *pt);

VX_EXPORT WIN_HANDLE VxSetParent(WIN_HANDLE Child, WIN_HANDLE Parent);
VX_EXPORT WIN_HANDLE VxGetParent(WIN_HANDLE Win);
VX_EXPORT BOOL VxMoveWindow(WIN_HANDLE Win, int x, int y, int Width, int Height, BOOL Repaint);
VX_EXPORT XString VxGetTempPath();
VX_EXPORT BOOL VxMakeDirectory(char *path);
VX_EXPORT BOOL VxRemoveDirectory(char *path);
VX_EXPORT BOOL VxDeleteDirectory(char *path);
VX_EXPORT BOOL VxGetCurrentDirectory(char *path);
VX_EXPORT BOOL VxSetCurrentDirectory(char *path);
VX_EXPORT BOOL VxMakePath(char *fullpath, char *path, char *file);
VX_EXPORT BOOL VxTestDiskSpace(const char *dir, DWORD size);

VX_EXPORT int VxMessageBox(WIN_HANDLE hWnd, char *lpText, char *lpCaption, DWORD uType);

//------ Process access {secret}
VX_EXPORT DWORD VxGetModuleFileName(INSTANCE_HANDLE Handle, char *string, DWORD StringSize);
VX_EXPORT INSTANCE_HANDLE VxGetModuleHandle(const char *filename);

//------ Recreates the whole  file path (not the file itself) {secret}
VX_EXPORT BOOL VxCreateFileTree(char *file);

//------ URL Download {secret}
VX_EXPORT DWORD VxURLDownloadToCacheFile(char *File, char *CachedFile, int szCachedFile);

//------ Bitmap Functions
VX_EXPORT BITMAP_HANDLE VxCreateBitmap(const VxImageDescEx &desc);
VX_EXPORT BYTE *VxConvertBitmap(BITMAP_HANDLE Bitmap, VxImageDescEx &desc);
VX_EXPORT BOOL VxCopyBitmap(BITMAP_HANDLE Bitmap, const VxImageDescEx &desc);
VX_EXPORT void VxDeleteBitmap(BITMAP_HANDLE BitmapHandle);
VX_EXPORT BITMAP_HANDLE VxConvertBitmapTo24(BITMAP_HANDLE _Bitmap);

VX_EXPORT VX_OSINFO VxGetOs();

typedef struct VXFONTINFO
{
    XString FaceName;
    int Height;
    int Weight;
    BOOL Italic;
    BOOL Underline;
} VXFONTINFO;

typedef enum VXTEXT_ALIGNMENT
{
    VXTEXT_CENTER  = 0x00000001,	// Text is centered when written
    VXTEXT_LEFT    = 0x00000002,	// Text is aligned to the left of the rectangle
    VXTEXT_RIGHT   = 0x00000004,	// Text is aligned to the right of the rectangle
    VXTEXT_TOP     = 0x00000008,	// Text is aligned to the top of the rectangle
    VXTEXT_BOTTOM  = 0x00000010,	// Text is aligned to the bottom of the rectangle
    VXTEXT_VCENTER = 0x00000020,	// Text is centered verticaly
    VXTEXT_HCENTER = 0x00000040,	// Text is centered horizontaly
} VXTEXT_ALIGNMENT;

//------ Font  Functions
VX_EXPORT FONT_HANDLE VxCreateFont(char *FontName, int FontSize, int Weight, BOOL italic, BOOL underline);
VX_EXPORT BOOL VxGetFontInfo(FONT_HANDLE Font, VXFONTINFO &desc);
VX_EXPORT BOOL VxDrawBitmapText(BITMAP_HANDLE Bitmap, FONT_HANDLE Font, char *string, CKRECT *rect, DWORD Align, DWORD BkColor, DWORD FontColor);
VX_EXPORT void VxDeleteFont(FONT_HANDLE Font);

#endif // __VXWINDOWFUNCTION_H__