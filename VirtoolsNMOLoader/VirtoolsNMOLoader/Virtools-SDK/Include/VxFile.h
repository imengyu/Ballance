/*************************************************************************/
/*	File : VxFile.h														 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef __VXFILE_H__
#define __VXFILE_H__

#include "VxStream.h"

#if defined(macintosh)
	#include <stdio.h>
	// Define it to use standard ANSI routines
	// Undef it to use Carbon Toolbox API
	#define MAC_USE_ANSI_FOR_VXFILE
#else
	#ifndef _WIN32_WCE
		#include <cstdio>	
		#include <stdarg.h>
		#include <sys/stat.h>
	#else
		#include <stdio.h>
		#include <stdarg.h>
	#endif	
#endif


/***********************************************************************
Summary: Utility class for file reading/writing.

Remarks:
Example:
		VxFile file;
		
		file.Open("TestFile.txt",VxFile::TEXTWRITEONLY);
		file.Printf("dummy string : %s","example");
		file.Close();
			
See also: 
***********************************************************************/
class VX_EXPORT VxFile : public VxStream
{
    public:

	enum OpenMode {
		READONLY		= 1,	
		WRITEONLY		= 2,
		READWRITE		= 3,
		APPEND			= 4,
		RWAPPEND		= 5,
		TEXTREADONLY	= 9,
		TEXTWRITEONLY	= 10,
		TEXTREADWRITE	= 11,
		TEXTAPPEND		= 12,
		TEXTRWAPPEND	= 13
	};
    
#if defined(macintosh)
	enum FileMode
	{
		INVALID			= 0,
		REGULAR_FILE	= 1<<13,
		DIRECTORY		= 1<<12,
		READPERMISSION	= 1<<8,
		WRITEPERMISSION = 1<<7,
		EXECPERMISSION	= 1<<6,
	};   
#elif !defined(PSX2) && !defined(_WIN32_WCE) 
	enum FileMode {
		INVALID			= 0,
		REGULAR_FILE	= S_IFREG,  // Regular File
		DIRECTORY		= S_IFDIR,	// File is a directory
		READPERMISSION	= S_IREAD,	// Read Permissions
		WRITEPERMISSION = S_IWRITE,	// Write Permissions
		EXECPERMISSION	= S_IEXEC,  // Excution permissions
	};
#else
	enum FileMode {
		INVALID			= 0,
		REGULAR_FILE	= 1<<13,
		DIRECTORY		= 1<<12,
		READPERMISSION	= 1<<8,
		WRITEPERMISSION = 1<<7,
		EXECPERMISSION	= 1<<6,
	};
#endif
	


	/***********************************************************************
	Summary: Returns the file mode.
	See also: FileMode.
	***********************************************************************/
	static XDWORD GetFileMode(const char* iFileName);

	/***********************************************************************
	Summary: Default Ctor.
	***********************************************************************/
    VxFile();

	/***********************************************************************
	Summary: Closes the file if it was open.
	***********************************************************************/
	virtual ~VxFile();
    
	//////////////////////////////////////
	// Virtuals Override

	/***********************************************************************
	Summary: Returns the current cursor position.
	***********************************************************************/
	XDWORD Position();

	/***********************************************************************
	Summary: Returns the file size in bytes.
	***********************************************************************/
	XDWORD Size();

	/***********************************************************************
	Summary: Returns TRUE if the file is still valid.
	***********************************************************************/
	XBOOL IsValid();

	/***********************************************************************
	Summary: Seeks to a given position into the file, starting from a given
	origin mode.
	***********************************************************************/
	XBOOL Seek(int iOffset, SeekMode iOrigin);

	/***********************************************************************
	Summary: Reads given amount of data from the file to the buffer.
	Return Value: The number of bytes actually read
	***********************************************************************/
	XDWORD Read(void* oBuffer, XDWORD iSize);

	/***********************************************************************
	Summary: Writes given amount of data from the buffer to the file.
	Return Value: The number of bytes actually written
	***********************************************************************/
	XDWORD Write(const void* iBuffer, XDWORD iSize);

	// New virtuals

	/***********************************************************************
	Summary: Opens () and close the currently opened one) a file for 
	reading or writing.
	***********************************************************************/
    virtual XBOOL Open(const char* iFileName, OpenMode iMode = READONLY);

	/***********************************************************************
	Summary: Returns TRUE if the file is successfully closed.
	***********************************************************************/
	virtual XBOOL Close();

	/***********************************************************************
	Summary: Gets a string from a file.
	***********************************************************************/
	virtual XBOOL GetString(char* oString, XDWORD iMax);

	/***********************************************************************
	Summary: Writes a formatted text buffer to the file.
	Return Value: The number of bytes actually written
	***********************************************************************/
	virtual XDWORD Printf(const char* iFormat, ...);

	/***********************************************************************
	Summary: Flushes a file (force update).
	***********************************************************************/
	virtual XDWORD Flush();

	/***********************************************************************
	Summary: Checks for the end of file (TRUE is end of file).
	***********************************************************************/
	virtual XBOOL EndOfFile();

	/***********************************************************************
	Summary: Reads data from the current position of stream into the locations given by argument (if any).
	Return Value: The number of fiels succesfully converted and assigned.
	A return value of 0 indicates that no fields were assigned. 
	If an error occurs, or if the end of the file stream is reached 
	before the first conversion, the return value is EOF
	***********************************************************************/
	virtual XDWORD Scanf(const char* iFormat, ...);

	/***********************************************************************
	Summary: Reads given amount of items from the file to the buffer.
	Return Value: The number of items actually read
	***********************************************************************/
	virtual XDWORD ReadItems(void* oBuffer, XDWORD iSize, XDWORD iCount);

	protected:

#if defined(macintosh)

	#if defined(MAC_USE_ANSI_FOR_VXFILE)
		FILE*		m_File;
	#else
		SInt16		m_File;
	#endif

#elif defined(PSX2) || defined(PSP)
	int			m_File;
#else
   	union
	{
	    FILE*		m_File;
		int			m_IFile;
	};
	
	#if defined(WIN32) || defined(_XBOX)
		int m_savedESP;
		int m_savedESI;
		int m_savedEAX;
		int m_savedRET;
		int m_savedThis;
	#endif
#endif

};


#endif
