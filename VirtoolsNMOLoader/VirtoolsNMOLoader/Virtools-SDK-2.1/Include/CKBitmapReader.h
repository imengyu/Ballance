/*************************************************************************/
/*	File : CKBitmapReader.h												 */
/*	Author :  Aymeric BARD												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2001, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _CKBITMAPREADER_H_
#define _CKBITMAPREADER_H_

#include "CKDataReader.h"

/********************************************
Summary: Bitmap Reader Error Values.

*************************************************/
typedef enum CK_BITMAPREADER_ERROR
{
    CKBITMAPERROR_GENERIC               = 1,	// Generic Error
    CKBITMAPERROR_READERROR             = 2,	// File could not be read
    CKBITMAPERROR_UNSUPPORTEDFILE       = 3,	// Image Format is not supported for a load operation
    CKBITMAPERROR_FILECORRUPTED         = 4,	// File is corrupted (CRC error)
    CKBITMAPERROR_SAVEERROR             = 5,	// File could not be saved
    CKBITMAPERROR_UNSUPPORTEDSAVEFORMAT = 6,	// Image Format is not supported for a save operation
    CKBITMAPERROR_UNSUPPORTEDFUNCTION   = 7		// Image Format is not supported for a save operation
} CK_BITMAPREADER_ERROR;

/************************************************************
Summary: Bitmap readers image description

Remarks:
+ A Bitmap reader method use this structure to describe images being
read or written.
+ It is also used to stored the save options some readers
can add. This is done by deriving from the CKBitmapProperties class.
+ For a example of usage of CKBitmapProperties you can refer to
the DDSReader sample available in the source code directory.

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

        struct CKBitmapProperties {
            int					m_Size;			// Size of this structure in byte (should be initialized )
            CKGUID  			m_ReaderGuid;	// CKGUID that uniquely identifies the reader that created this properties structure
            CKFileExtension		m_Ext;			// File Extension of the image being described by this structure
            VxImageDescEx		m_Format;		// Optionnal Image format
            void*				m_Data;			// Optionnal Image data pointer
        };

{html:</td></tr></table>}

See Also:Creation of Bitmap Media Plugins,CKBitmapReader
****************************************************************/
struct CKBitmapProperties
{

    CKBitmapProperties()
    {
        m_Size = sizeof(CKBitmapProperties);
        m_Data = NULL;
    };

    int m_Size;			 // Size of this structure in byte (should be initialized
                         // in constructor in derived classes)
    CKGUID m_ReaderGuid; // CKGUID that uniquely identifies the reader that created this properties structure

    CKFileExtension m_Ext; // File Extension of the image being described by this structure

    VxImageDescEx m_Format; // Optionnal Image format

    void *m_Data; // Optionnal Image data pointer
};

/***********************************************************
Summary: Base class for bitmap readers.

Remarks:
    + Every bitmap reader must derive from this class.
    + It declares the basic method for loading and saving a bitmap and managing the save options
See Also:Creation of Bitmap Media Plugins,CKPluginManager::GetBitmapReader
**************************************************************/
class CKBitmapReader : public CKDataReader
{
public:
    void Release() = 0;

    // Get Flags : by default, the bitmap readers can do it all...

    virtual CK_DATAREADER_FLAGS GetFlags() { return (CK_DATAREADER_FLAGS)(CK_DATAREADER_FILELOAD | CK_DATAREADER_FILESAVE | CK_DATAREADER_MEMORYLOAD | CK_DATAREADER_MEMORYSAVE); }

    ///------------------------------------------
    // Bitmap Properties

    /************************************************
    Summary: Returns if the alpha channel will be saved.
    Return Value:
        TRUE if format saves the alpha channel.
    Arguments:
        bp: A pointer to CKBitmapProperties containing the desired save format.
    Remarks:
        This method returns whether the reader is capable of saving
    an alpha channel according to the given option in bp.
    ************************************************/
    virtual CKBOOL IsAlphaSaved(CKBitmapProperties *bp) { return FALSE; }
    /************************************************
    Summary: Returns the current default bitmap options.
    See Also:SetBitmapDefaultProperties
    ************************************************/
    virtual void GetBitmapDefaultProperties(CKBitmapProperties **Bp) = 0;
    /************************************************
    Summary: Sets the current default bitmap options.
    See Also:GetBitmapDefaultProperties
    ************************************************/
    virtual void SetBitmapDefaultProperties(CKBitmapProperties *Bp) = 0;

    //---------------------------------------------
    // Loading Functions

    /************************************************
    Summary: Loads a bitmap file.
    Return Value:
        Returns 0 if successful,an error code (CK_BITMAPREADER_ERROR) otherwise.
    Remarks:
        + Reads a file and fill up the ppBitmapProperties with a pointer to the properties of the read image.
        + The name argument is the full path of the file, and can be an URL.
    See Also:ReadMemory
    ************************************************/
    virtual int ReadFile(char *name, CKBitmapProperties **ppBitmapProperties) = 0;
    // Synchronous Reading from memory
    /************************************************
    Summary: Loads a bitmap file stored in memory.
    Return Value:
        Returns 0 if successful,an error code (CK_BITMAPREADER_ERROR) otherwise.
    Remarks:
        + Reads an image from the memory pointer and fill up the ppBitmapProperties with a pointer to the properties of the read image.
    See Also:ReadFile
    ************************************************/
    virtual int ReadMemory(void *memory, int size, CKBitmapProperties **) = 0;
    /************************************************
    Summary: Not Yet Supported.
    ************************************************/
    virtual int ReadASynchronousFile(char *name, CKBitmapProperties **) = 0;

    //--------------------------------------------------
    // Saving Functions
    // Synchronous Reading from file or URL

    /************************************************
    Summary: Saves a image to a file
    Return Value:
        Returns the number of written bytes if successful.
    Remarks:
        + Saves the image described by pBitmapProperties to the file whose path is name.
        + A simple implementation of this method can be done using the SaveMemory method and then writing the content
        of the memory to a file.
    See Also:SaveFile
    ************************************************/
    virtual int SaveFile(char *name, CKBitmapProperties *pBitmapProperties) = 0;

    /************************************************
    Summary: Saves an image into a memory block.
    Return Value:
        Returns the number of written bytes if successful, 0 otherwise.
    Remarks:
        + Allocate memory and writes the image corresponding to the pBitmapProperties argument into this memory.
        + The pMemory pointer is filled with a pointer to the newly created memory.
        + This is typically called by Virtools when storing an image inside a Cmo file.
        + When the caller is done with this memory, it must call the ReleaseMemory method, so that the Plugin releases the memory.
    See Also:ReleaseMemory,SaveFile
    ************************************************/
    virtual int SaveMemory(void **memory, CKBitmapProperties *pBitmapProperties) = 0;

    /************************************************
    Summary: Deletes a block of memory allocated by a previous SaveMemory call.
    Remarks:
        If the compilation options of a reader plugin and the program
    that actually uses it are not the same it can occur that the new operator
    used to create the file buffer in SaveMemory is not compatible with the delete operator
    in the program. For this reason the reader must implement a ReleaseMemory method that
    actually only calls delete operator on the block of memory it is given.
    See Also:SaveMemory
    ************************************************/
    virtual void ReleaseMemory(void *memory) = 0;

    /************************************************
    Summary: Loads a bitmap file from a VxStream.
    Return Value:
        Returns 0 if successful,an error code (CK_BITMAPREADER_ERROR) otherwise.
    Remarks:
        + Reads an image from the iStream  and fill up the ppBitmapProperties with a pointer to the properties of the read image.
    See Also:ReadFile
    ************************************************/
    // virtual int ReadStream(VxStream& iStream,CKBitmapProperties** oBitmapProperties) {return CKBITMAPERROR_UNSUPPORTEDFUNCTION;}
};

#endif