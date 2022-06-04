/*************************************************************************/
/*	File : CKMovieReader.h												 */
/*	Author :  Romain Sididris											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2001, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _CKMOVIEREADER_H_
#define _CKMOVIEREADER_H_

#include "CKDataReader.h"

/********************************************
Summary: Movie Reader Error Values.

********************************************/
typedef enum CK_MOVIEREADER_ERROR
{
    CKMOVIEERROR_GENERIC               = 1,	// Generic Error
    CKMOVIEERROR_READERROR             = 2,	// File could not be read
    CKMOVIEERROR_UNSUPPORTEDFILE       = 3,	// Image Format is not supported for a load operation
    CKMOVIEERROR_FILECORRUPTED         = 4,	// File is corrupted (CRC error)
    CKMOVIEERROR_SAVEERROR             = 5,	// File could not be saved
    CKMOVIEERROR_UNSUPPORTEDSAVEFORMAT = 6	// Image Format is not supported for a save operation
} CK_MOVIEREADER_ERROR;

/*********************************************
Summary: Movie readers image description

Remarks:
+ A Movie reader method use this structure to describe images being
read or written.
+ It is also used to stored the save options some readers
can add. This is done by deriving from the CKMovieProperties class.
+ For a example of usage of CKMovieProperties you can refer to
the AVIReader sample available in the source code directory.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        struct CKMovieProperties {
            int					m_Size;			// Size of this structure in byte
            CKGUID  			m_ReaderGuid;	// CKGUID that uniquely identifies the reader that created this properties structure
            CKFileExtension		m_Ext;			// File Extension of the image being described by this structure
            VxImageDescEx		m_Format;		// Optionnal Image format
            void*				m_Data;			// Optionnal Image data pointer
        };

{html:</td></tr></table>}

See Also:Creation of Movie Media Plugins,CKMovieReader
*********************************************/
struct CKMovieProperties
{
    // Dtor
    CKMovieProperties()
    {
        m_Size = sizeof(CKMovieProperties);
        m_Data = NULL;
    };

    int m_Size;			 // Size of this structure in byte (should be initialized
                         // in constructor in derived classes)
    CKGUID m_ReaderGuid; // CKGUID that uniquely identifies the reader that created this properties structure

    CKFileExtension m_Ext; // File Extension of the image being described by this structure

    VxImageDescEx m_Format; // Optionnal Image format

    void *m_Data; // Optionnal Image data pointer
};

/*******************************************************
Summary: Base class for movie readers.

Remarks:
+ Every movie reader must derive from this class.
+ It declares the basic method for loading a movie
+ Refer to the AVIReader sample available in the source code directory for more details.

See Also:Creation of Movie Media Plugins,CKPluginManager::GetMovieReader
**********************************************************/
class CKMovieReader : public CKDataReader
{
public:
    void Release() = 0;

    virtual CK_DATAREADER_FLAGS GetFlags() { return (CK_DATAREADER_FLAGS)(CK_DATAREADER_FILELOAD | CK_DATAREADER_MEMORYLOAD); }

    //---------------------------------------------------
    // Movie Properties

    /************************************************
    Summary: Returns the number of image frames in the movie.
    Remarks:
        + A file must have been opened with OpenFile before this method can be used
    See Also: OpenFile,GetMovieLength,ReadFrame
    ************************************************/
    virtual int GetMovieFrameCount() = 0;

    /************************************************
    Summary: Returns the length in millseconds of the movie.
    Remarks:
        + A file must have been opened with OpenFile before this method can be used
    See Also: OpenFile,GetMovieFrameCount
    ************************************************/
    virtual int GetMovieLength() = 0;

    ///----------------------------------------------
    // Open Functions

    // Synchronous Reading from file or URL
    /************************************************
    Summary: Opens a movie file.
    Remarks:
        + Open the file which path is the name argument.
        + Return 0 (zero) if successful or an CK_MOVIEREADER_ERROR
        error.
        + Once the file is opened, images can be read with ReadFrame method
    See Also:ReadFrame,GetMovieFrameCount
    ************************************************/
    virtual CKERROR OpenFile(char *name) = 0;

    virtual CKERROR OpenMemory(char *name) = 0;

    virtual CKERROR OpenAsynchronousFile(char *name) = 0;

    ///----------------------------------------------
    // Decoding Functions

    /************************************************
    Summary: Decode a frame of the movie
    Remarks:
        + Fills up ppMovieProperties with a pointer to a description of an image,
        which corresponds to the frame number f of the movie.
        + Returns 0 (zero) if no error. Frame 0 (zero) is the first frame.
    See Also: OpenFile,GetMovieLength
    ************************************************/
    virtual CKERROR ReadFrame(int f, CKMovieProperties **ppMovieProperties) = 0;
};

#endif