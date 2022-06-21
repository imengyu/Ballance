/*************************************************************************/
/*	File : CKPathManager.h			 				 					 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKPATHMANAGER_H

#define CKPATHMANAGER_H "$Id:$"

/****************************************************
{filename:CK_PATHMANAGER_CATEGORY}
Summary: Enumeration of pre-registred path categories

Remarks 
	+ The path manager pre-registers 3 categories of path (sound,bitmap and misc. data).
See Also: CKPathManager
*************************************************/
typedef enum CK_PATHMANAGER_CATEGORY {
	BITMAP_PATH_IDX		=0,	// Category index for bitmaps paths
	DATA_PATH_IDX		=1,	// Category index for datas paths
	SOUND_PATH_IDX		=2	// Category index for sounds paths
} CK_PATHMANAGER_CATEGORY;

#include "XClassArray.h"
#include "XString.h"


typedef XClassArray<XString> CKPATHENTRYVECTOR;


typedef struct CKPATHCATEGORY
{
	XString m_Name;
	CKPATHENTRYVECTOR m_Entries;
} CKPATHCATEGORY;


typedef XClassArray<CKPATHCATEGORY> CKPATHCATEGORYVECTOR;

/*************************************************
Summary: Files paths management
Remarks:

+ The path manager holds a set of paths that everybody can use to
retrieve a file.

+ These paths are put into relevant categories (bitmap,sound,datas) but
new categories for application specific data can be defined.

+ The Path manager also provides some utility methods to work on paths.

+ At startup (creation of the CKContext) , the path manager always registers
three default category:

		"Bitmap Paths"	: Index 0
		"Data Paths"	: Index 1
		"Sound Paths"	: Index 2


See Also:CKContext::GetPathManager
****************************************************/
class CKPathManager : public CKBaseManager  
{
public:
 
	
	CKPathManager(CKContext *Context);
	
	virtual ~CKPathManager();

// Category Functions

	// Adds a category, category name must be unique
	int AddCategory(XString& cat);	
	// Removes a category using its name or its index in category list
	CKERROR RemoveCategory(int catIdx);

	// Gets the number of categories
	int GetCategoryCount();

	// Gets the category name at specified index
	CKERROR GetCategoryName(int catIdx,XString& catName);

	// Gets the category Index in List
	int GetCategoryIndex(XString& cat);

	// Renames a category 
	CKERROR RenameCategory(int catIdx, XString& newName);

// Paths Functions
	
	// Adds a path to a category
	int AddPath(int catIdx, XString& path);
	// Removes a path in a category
	CKERROR RemovePath(int catIdx, int pathIdx);

	// Swap paths
	CKERROR SwapPaths(int catIdx, int pathIdx1, int pathIdx2);

	// Gets the path count for a category
	int GetPathCount(int catIdx);
	
	// Gets the path at index pathIdx for a category
	CKERROR GetPathName(int catIdx, int pathIdx,XString& path);
	
	// Gets the path at index pathIdx for a category
	int GetPathIndex(int catIdx, XString& path);
	
	// Changes a path in a category
	CKERROR RenamePath(int catIdx, int pathIdx, XString& path);

//--- Finding a file
	
	// Resolve File Name in the given category
	CKERROR ResolveFileName(XString& file,int catIdx,int startIdx = -1);


//--- Utilities

	// Path Type 
	BOOL PathIsAbsolute(XString& file);
	BOOL PathIsUNC(XString& file);
	BOOL PathIsURL(XString& file);
	BOOL PathIsFile(XString& file);
	
	// Converts '%20' characters to ' '
	void RemoveEscapedSpace(char *str);

	// Converts '' characters to '%20'
	void AddEscapedSpace(XString &str);

	// Virtools tempory storage folder...
	XString GetVirtoolsTemporaryFolder();

//--- Big Files

	// Register a BigFile : Absolute path
	CKERROR		AddBigFile(const char* iFileName);
	// Gets a registered BigFile by index
	const char*	GetBigFile(int iIndex);
	// Gets the count of registered BigFiles
	int			GetBigFileCount();
	// remove a BigFile by index
	CKERROR		RemoveBigFile(int iIndex);

	// Is the given path reference a file in a BigFile
	CKBOOL		IsBigFilePath(const char* iPath);
	// Is the given path reference a file in a BigFile
	XBOOL		OpenSubFile(const char* iPath,VxSubFile* oSubFile);
	
	
	static CKPathManager* Cast(CKBaseManager* iM) { return (CKPathManager*)iM; }
	
protected :

	void Clean();	

	// Do not use, use RemoveEscapedSpace !
	void RemoveSpace(char *str);

	BOOL TryOpenAbsolutePath(XString& file);
	BOOL TryOpenFilePath(XString& file);
	BOOL TryOpenURLPath(XString&file);

	
	CKPATHCATEGORYVECTOR	m_Categories;
	XString					m_TemporaryFolder;	
	CKBOOL					m_TemporaryFolderExist;

	// Array of big files
	XArray<VxBigFile*>		m_BigFiles;
	// Cache index of the last big file accessed
	int						m_LastBigFile;

};

#endif
