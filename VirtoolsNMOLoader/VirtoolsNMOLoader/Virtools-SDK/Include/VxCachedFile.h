/*************************************************************************/
/*	File : VxCachedFile.h														 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef __VXCACHEDFILE_H__
#define __VXCACHEDFILE_H__

#include "VxFile.h"


class VX_EXPORT VxCachedFile : public VxFile
{
public:
	// Ctor
	VxCachedFile(const XDWORD iCacheSize = 32768); 

	// Dtor
	~VxCachedFile();

	enum {
		UNCACHED = 0xffffffff
	};

	/***********************************************************************
	Summary: Returns the cache size.
	See also: SetCacheSize.
	***********************************************************************/
	void SetCacheSize(const XDWORD iCacheSize);

	/***********************************************************************
	Summary: Returns the cache size.
	See also: SetCacheSize.
	***********************************************************************/
	XDWORD GetCacheSize()
	{
		return m_CacheSize;
	}
	
	/***********************************************************************
	Summary: Returns the file size in bytes.
	***********************************************************************/
	XDWORD Position();

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

	// VxFile Override

	/***********************************************************************
	Summary: Opens () and close the currently opened one) a file for 
	reading or writing.
	***********************************************************************/
    XBOOL Open(const char* iFileName, OpenMode iMode = READONLY);

	/***********************************************************************
	Summary: Returns TRUE if the file is successfully closed.
	***********************************************************************/
	virtual XBOOL Close();

	/***********************************************************************
	Summary: Gets a string from a file.
	***********************************************************************/
	virtual XBOOL GetString(char* oString, XDWORD iMax);

	/***********************************************************************
	Summary: Flushes a file (force update).
	***********************************************************************/
	virtual XDWORD Flush();

	/***********************************************************************
	Summary: Reads given amount of items from the file to the buffer.
	Return Value: The number of items actually read
	***********************************************************************/
	virtual XDWORD ReadItems(void* oBuffer, XDWORD iSize, XDWORD iCount);


protected:
	// Current Position in the file
	XDWORD	m_Position;

	// the cache size
	XDWORD	m_CacheSize;
	// the cache
	XBYTE*	m_Cache;
	// Cache position
	XDWORD	m_CachePosition;
};


#endif
