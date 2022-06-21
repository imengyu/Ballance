/*************************************************************************/
/*	File : VxMemoryMappedFile.h											 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef __VxMemoryMappedFile_H__
#define __VxMemoryMappedFile_H__

/***********************************************************************
Summary: Possible return value when opening a memory mapped file.

See Also: VxMemoryMappedFile::GetErrorType
***********************************************************************/
enum VxMMF_Error {  
	VxMMF_NoError,		// No error
	VxMMF_FileOpen,		// Cannot open file
    VxMMF_FileMapping,	// Cannot create file mapping
	VxMMF_MapView		// Cannot get a pointer to start of map
};

/***********************************************************************
Summary: Utility class for memory mapped file reading.

Remarks:
	The VxMemoryMappedFile can be used have a mapping of a file into a memory 
	buffer for reading purposes.
Example:
		VxMemoryMappedFile mappedFile(FileName);

		DWORD FileSize = mappedFile.GetFileSize();
		BYTE* buffer = (BYTE*)mappedFile.GetBase();
	
	  // buffer now contain the file content and can be read
			
See also: 
***********************************************************************/
class VxMemoryMappedFile
{
    public:
    
    VX_EXPORT VxMemoryMappedFile( char* pszFileName );
    
	VX_EXPORT ~VxMemoryMappedFile(void);
    
	/***********************************************************************
	Summary: Returns a pointer to the mapped memory buffer.
	Remarks: The returned pointer should not be deleted nor should it be 
	used for writing purpose.
	***********************************************************************/
    VX_EXPORT void*   GetBase( void ){ return m_pMemoryMappedFileBase; }

	/***********************************************************************
	Summary: Returns the file size in bytes.
	***********************************************************************/
	VX_EXPORT DWORD   GetFileSize( void ){ return m_cbFile; }

	/***********************************************************************
	Summary: Returns the file was successfully opened and mapped to a memory buffer.
	***********************************************************************/
	VX_EXPORT BOOL    IsValid( void ) { return VxMMF_NoError == m_errCode; } 

	/***********************************************************************
	Summary: Returns whether there was an error opening the file.
	***********************************************************************/
	VX_EXPORT VxMMF_Error  GetErrorType(){ return m_errCode; }

private:

#if defined(_LINUX)
	int      m_hFile;
	void*    m_pMemoryMappedFileBase;
	DWORD       m_cbFile;
	VxMMF_Error m_errCode;  
#else
    GENERIC_HANDLE      m_hFile;
    GENERIC_HANDLE      m_hFileMapping; // Handle of memory mapped file
    void*       m_pMemoryMappedFileBase;
    DWORD       m_cbFile;
    VxMMF_Error m_errCode;  
#endif

};


#endif
