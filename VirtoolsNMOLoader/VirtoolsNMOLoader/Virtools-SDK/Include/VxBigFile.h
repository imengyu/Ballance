
#ifndef VXBIGFILE_H
#define VXBIGFILE_H

#include "FixedSizeAllocator.h"
#include "VxCachedFile.h"
#include "XUtil.h"

#ifdef macintosh
	#include <stdio.h>
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

/************************************************************
The VxSubFile is a special implementation of the VxStream
It gives access to a file stored inside a big file (concatenation of
several files in a single,bigger file).
See Also: VxBigFile
*************************************************************/
class VxSubFile : public VxStream
{
public:
	
	VxSubFile():m_File(NULL),m_Start(0),m_Size(0) {}
	VX_EXPORT void		Set(VxFile* iBigFile, XDWORD iOffset, XDWORD iSize);

	// Virtual overridings
	VX_EXPORT XDWORD	Position();
	VX_EXPORT XDWORD	Size();
	VX_EXPORT XBOOL		IsValid();
	VX_EXPORT XBOOL		Seek(int iOffset, SeekMode iOrigin);
	VX_EXPORT XDWORD	Read(void* oBuffer, XDWORD iSize);
	VX_EXPORT XDWORD	Write(const void* iBuffer, XDWORD iSize);
protected:
	void		EnsurePosition();
	VxFile*		m_File;
	XDWORD		m_Start;
	XDWORD		m_Size;
	XDWORD		m_Position;
	
};


/******************************************************************
A VxBigFile is used to store several files together in a single file.
Files stored in a big file can be accessed as normal files with the Find method
which fills a VxSubFile class with the appropriate data to read the sub file content.
*******************************************************************/
class VxBigFile
{
public:
	
	/*******************************************
	A Callback class that can be implemented to handle the progression
	of VxBigFile::Save operation.
	*******************************************/
	class ProgressionCallback
	{
	public:
		// Progression value from 0 to 100
		virtual void Progress(const int iProgression) = 0;
	};

#if (defined(PSX2) || defined(PSP))
	/***********************************************
	File buffers as stored inside the big file
	************************************************/
	struct Chunk
	{
		
		Chunk():buffersize(0),offset(0) {}
		XDWORD	offset;		// offset inside the bigfile
		int		buffersize; // if buffersize is <= 0, the buffer contains a file path, not yet loaded
	};
#else
	/***********************************************
	File buffers as stored inside the big file
	************************************************/
	struct Chunk
	{
		
		Chunk():buffer(NULL),buffersize(0),offset(0) {}
		
		~Chunk()
		{
			delete [] (XBYTE*)buffer;
		}
		XString	name;		// Filename
		XDWORD	offset;		// offset inside the bigfile
		void*	buffer;		// memory buffer or NULL if this chunk reference a not yet loaded file	
		int		buffersize; // if buffersize is <= 0, the buffer contains a file path, not yet loaded
	};
#endif

#if (defined(PSX2) || defined(PSP))
	typedef XHashTable<Chunk, const char*> ChunkTable;
#else
	typedef XHashTable<Chunk*, const char*> ChunkTable;
#endif

	
	VX_EXPORT ~VxBigFile();

	/*******************************************************************
	Opens an existing BigFile
	*******************************************************************/
	VX_EXPORT XBOOL Open(const char* iFileName, XBOOL iKeepOrder = TRUE);
	/*******************************************************************
	Creates a BigFile containing all the files appended.
	*******************************************************************/
	VX_EXPORT XBOOL Save(const char* iFileName = NULL, ProgressionCallback* iCallback = NULL);

	/*******************************************************************
	Loads an index file that contains the order in which the file should be
	stored inside the big file
	*******************************************************************/
	VX_EXPORT XBOOL	LoadIndex(const char* iFileName);
	/*******************************************************************
	Saves an index file that contains the order in which the file are
	stored inside the big file
	*******************************************************************/
	VX_EXPORT void	SaveIndex(const char* iFileName, XBOOL iDetails = FALSE);

	/***********************************************************************************
	Append a memory buffer or a file inside the big file.
	************************************************************************************/
	VX_EXPORT XBOOL Append(const char* iReference, const char* iFileName, XBOOL iPersistent = TRUE, XBOOL iReplace = TRUE);
	VX_EXPORT XBOOL Append(const char* iReference, const void* iBuffer, XDWORD iSize);

	/**********************************************************************
	Find and return a sub-file.
	***********************************************************************/
	VX_EXPORT XBOOL	Find(const char* iFileName, VxSubFile* oSubFile = NULL);
	/**********************************************************************
	Remove a sub-file.
	***********************************************************************/
	VX_EXPORT XBOOL	Remove(const char* iFileName);


	/**********************************************************************
	
	***********************************************************************/
	const XArray<Chunk*>& GetOrderedChunks() const
	{
		return m_Chunks;
	}

	/*********************************************************************
	
	**********************************************************************/
	const ChunkTable& GetIndex() const
	{
		return m_Index;
	}


	// Return the filename of an opened bigfile
	const XString& GetName() const
	{
		return m_FileName;
	}

	
	VxFile& GetFile()
	{
		return m_File;
	}

protected:

	// clear
	void		Clear();

	// Pool of chunk
	XObjectPool<Chunk>			m_ChunkPool;
	// list of all the files, in order
	XArray<Chunk*>				m_Chunks;
	// index of the file inside the big file
	ChunkTable					m_Index;
	// the big file path and file name
	XString						m_FileName;
	// the file
	VxCachedFile				m_File;

	// the current folder to look for file
	XString						m_CurrentFolder;
	
#if (defined(PSX2) || defined(PSP))
	XAP<XBYTE>					m_Header;
#endif

	// VxBigFile signature
	static char					m_Signature[4];

	VxFile						m_LogHistory;		

};

#endif
