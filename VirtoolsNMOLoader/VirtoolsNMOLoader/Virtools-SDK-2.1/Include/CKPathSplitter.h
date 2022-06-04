/*************************************************************************/
/*	File : CKPathSplitter.h												 */
/*	Author :  Nicolas Galinotti											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKPATHSPLITTER_H
#define CKPATHSPLITTER_H "$Id:$"

#include "XUtil.h"

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
    CKPathSplitter(char *file);

    ~CKPathSplitter();

    // Returns the optionnal drive letter, followed by a colon (:)
    char *GetDrive();
    // Returns the optionnal directory path, including trailing slash (\)
    char *GetDir();
    // Returns the file name without extension
    char *GetName();
    // Returns the file extension including period (.)
    char *GetExtension();

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
    CKPathMaker(char *Drive, char *Directory, char *Fname, char *Extension);
    // Returns the full path.
    char *GetFileName();

protected:
    char m_FileName[_MAX_PATH];
};

/***********************************************************************
Summary: Storage class for filename extensions




See also: CKPathSplitter,CKPathMaker
***********************************************************************/
struct CKFileExtension
{
    // Ctor
    CKFileExtension() { memset(m_Data, 0, 4); }
    CKFileExtension(char *s)
    {
        if (!s)
            m_Data[0] = 0;
        else
        {
            if (s[0] == '.')
                s = &s[1];
            int len = strlen(s);
            if (len > 3)
                len = 3;
            memcpy(m_Data, s, len);
            m_Data[len] = '\0';
        }
    }

    int operator==(const CKFileExtension &s)
    {
        return !_strcmpi(m_Data, s.m_Data);
    }

    operator char *() { return m_Data; }
    operator const char *() { return m_Data; }

    // Members secret
    char m_Data[4];
};

#endif
