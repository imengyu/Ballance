/*************************************************************************/
/*	File : CKDirectoryParser.h											 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKDIRECTORYPARSER_H
#define CKDIRECTORYPARSER_H  "$Id:$"


/***********************************************************************
Summary: Utility class for Directories parsing.

Remarks:
	CKDirectoryParser class useful for iterating inside a directory arborescence.
	It can be used to parse only a directory looking for files with an extension filter or
	to parse a complete arborescence.
	On Win32 platforms, suffers the extension mismatches limitation : if you want to parse the *.ext files, *.exta files
	will also be listed.

Example:
	// Counts and makes a list of files with extension .cpp 
	// in a directory tree
	CKDirectoryParser MyParser(Directory,"*.cpp",TRUE);	
	
	XClassArray<XString> FileList;
	char* str = NULL;
	while(str = parser.GetNextFile()) {
		// str contains the full path of the file.
		FileList.PushBack(XString(str));
	}

	 	


See also: CKPathMaker,CKPathSplitter
***********************************************************************/
class CKDirectoryParser  
{
public:

VX_EXPORT	CKDirectoryParser(char* dir,char* fileMask,BOOL recurse=FALSE, BOOL iReturnDirectory = FALSE);

VX_EXPORT	~CKDirectoryParser();
VX_EXPORT	char*	 GetNextFile();
VX_EXPORT	void	Reset(char* dir=NULL,char *fileMask=NULL,BOOL recurse=FALSE,BOOL iReturnDirectory = FALSE);

protected:

	void Clean();

	void*				m_FindData;  
#if defined(_LINUX) || defined(PSX2) || defined(PSP) 
	void*                           m_Dir;
#else
	int					m_hFile;
#endif
	char				m_FullFileName[_MAX_PATH];
	XString				m_StartDir;
	XString				m_FileMask;
	DWORD 				m_State;
	CKDirectoryParser	*m_SubParser;

};

#endif
