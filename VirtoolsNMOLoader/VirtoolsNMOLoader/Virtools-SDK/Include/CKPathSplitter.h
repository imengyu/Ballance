/*************************************************************************/
/*	File : CKPathSplitter.h												 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKPATHSPLITTER_H
#define CKPATHSPLITTER_H  "$Id:$"

#include "string.h"
#include "stdlib.h"

#ifdef _WIN32_WCE
	#include "VxMathCE.h"
	#include "windows.h"
#endif



/***********************************************************************
Summary: Utility class for filenames extraction

Remarks:
	CKPathSplitter & CKPathMaker are useful for manipulation of filenames and paths.
	CKPathSplitter class is used to break a path into its four components (Drive, Directory Filename and extension).
	CKPathMaker class creates a path string from its four components..


Example:
		// for example to change the name of a file :
		CKPathSplitter	splitter(OldFilename);
		
		char* OldName = splitter.GetName();
		char* newname = ChangeName(OldName);
		CKPathMaker maker(splitter.GetDrive(),splitter.GetDir(),newname,splitter.GetExtension());
		char* NewFilename = maker.GetFileName();
	 	


See also: CKPathMaker
***********************************************************************/
class VX_EXPORT CKPathSplitter
{
public:
	// Constructs the object from a full path.
	CKPathSplitter(char* file);
	
	~CKPathSplitter();

	// Returns the optionnal drive letter, followed by a colon (:)
	char *GetDrive();
	// Returns the optionnal directory path, including trailing slash (\)
	char *GetDir();
	// Returns the file name without extension
	char *GetName();
	// Returns the file extension including period (.)
	char *GetExtension();

	// create the objet from a full path.
	CKPathSplitter& operator=(const char* file);

protected:
	char m_Drive[_MAX_DRIVE];
	char m_Dir[_MAX_DIR];
	char m_Fname[_MAX_FNAME];
	char m_Ext[_MAX_EXT];

};

/***********************************************************************
Summary: Utility class for directory and filenames concatenation

Remarks:
	CKPathSplitter & CKPathMaker are useful for manipulation of filenames and paths.
	CKPathMaker class creates a path string from its four components..


Example:
		// for example to change the name of a file :
		CKPathSplitter	splitter(OldFilename);
		
		char* OldName = splitter.GetName();
		char* newname = ChangeName(OldName);
		CKPathMaker maker(splitter.GetDrive(),splitter.GetDir(),newname,splitter.GetExtension());
		char* NewFilename = maker.GetFileName();
	 	


See also: CKPathSplitter
***********************************************************************/
class VX_EXPORT CKPathMaker
{
public:
	// Constructs the object from an optionnal Drive letter,Directory,Filename and Extension.
	CKPathMaker(char* Drive,char* Directory,char* Fname,char* Extension);
	// Returns the full path. 
	char *GetFileName();

protected:
	char m_FileName[_MAX_PATH];

};

#define VIRTOOLS_MAX_EXTENSION_SIZE	68

/***********************************************************************
Summary: Storage class for filename extensions

	 	


See also: CKPathSplitter,CKPathMaker
***********************************************************************/
struct CKFileExtension
{
	// Ctor
	CKFileExtension()
	{
		memset(m_ExtLong,0,VIRTOOLS_MAX_EXTENSION_SIZE);
	}

	CKFileExtension(char* s)
	{
		if(!s) {
			m_ExtLong[0] = 0;
		} else {
			if(s[0] == '.') {
				s = &s[1];
			}
			int len = (int)strlen(s);
			if(len>(VIRTOOLS_MAX_EXTENSION_SIZE-1)) {
				len = VIRTOOLS_MAX_EXTENSION_SIZE-1; 
			}
			memcpy(m_ExtLong,s,len);
			m_ExtLong[len]= '\0';
		}
	}

	int operator==(const CKFileExtension& s)
	{ 		
		return !strcmpi(m_ExtLong,s.m_ExtLong);
	}

	operator char*()
	{
		return m_ExtLong;
	}

	operator const char*()
	{
		return m_ExtLong;
	}

	// Members secret
	char m_ExtLong[VIRTOOLS_MAX_EXTENSION_SIZE];
};

struct CKFileExtensionShort
{
	// Ctor
	CKFileExtensionShort() {memset(m_ExtShort,0,4);}
	CKFileExtensionShort(char* s) {
		if(!s) m_ExtShort[0] = 0;
		else {
			if(s[0] == '.')
				s = &s[1];
			int len = (int)strlen(s);
			if(len>3) len = 3; 
			memcpy(m_ExtShort,s,len);
			m_ExtShort[len]= '\0';
		}
	}

	int operator==(const CKFileExtensionShort& s){ 		
		return !strcmpi(m_ExtShort,s.m_ExtShort);
	}

	operator char*(){return m_ExtShort;}
	operator const char*(){return m_ExtShort;}

	// Members secret
	char m_ExtShort[4];
};


#endif 
