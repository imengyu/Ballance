/*************************************************************************/
/*	File : CKDataReader.h												 */
/*	Author :  Aymeric BARD												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2001, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef _CKDATAREADER_H_
#define _CKDATAREADER_H_

#include "CKDefines.h"

/**********************************************************
Summary: Readers loading/saving capabilities.

Remarks:
	A Reader (either bitmap,sound or model) must implement the GetFlags method
to indicate its loading / saving capabilities by returning a combination 
of the following flags.
See Also:CKDataReader::GetFlags
**********************************************************/
typedef enum CK_DATAREADER_FLAGS {
	CK_DATAREADER_FILELOAD		= 0x00000001,		// This reader can load data from file
	CK_DATAREADER_FILESAVE		= 0x00000002,		// This reader can save data to file
	CK_DATAREADER_MEMORYLOAD	= 0x00000004,		// This reader can load data from memory
	CK_DATAREADER_MEMORYSAVE	= 0x00000008,		// This reader can save data to memory
	CK_DATAREADER_STREAMFILE	= 0x00000010,		// This reader can stream data from file
	CK_DATAREADER_STREAMURL		= 0x00000020,		// This reader can stream data from URL
	CK_DATAREADER_VXSTREAMLOAD	= 0x00000040,		// This reader can load data from a VxStream
} CK_DATAREADER_FLAGS;


/**********************************************************
Summary: Base class for all reader plugins

Remarks:
This class declares the common methods used by all reader plugins which are:

	+ Save Options (GetOptionDescription & GetOptionsCount) essentially used by bitmap plugins)
	+ Reader Description (GetReaderInfo)
	+ Readers loading/saving capabilities (GetFlags)

When done using a reader (normally returned by a call to the CKPluginManager), it must 
be released by calling its Release() method which will take care of deleting the reader object.

See Also:Creation of Media Plugins,CKBitmapReader,CKSoundReader,CKMovieReader,CKModelReader
*********************************************************************/
class CKDataReader
{
public:
	
	virtual ~CKDataReader() {}
	/************************************************
	Summary: Destroys the reader.
	Remarks:
		This method is often implemented as delete(this);
	as it should destroy the reader.
	************************************************/
	virtual void Release() = 0;
	/************************************************
	Summary: Returns a description of the reader.
	Return Value:
		A pointer to a CKPluginInfo structure that describes this reader. 
	See Also: CKPluginInfo
	************************************************/
	virtual CKPluginInfo* GetReaderInfo() = 0;

	/************************************************
	Summary: Returns the number of options.
	Return Value:
		Number of options this reader supports.
	Remarks:
		Refer to The Media Options part in the Creation of Bitmap Media Plugins chapter for more details on this method.
	See Also:Creation of Bitmap Media Plugins,GetOptionDescription
	************************************************/
	virtual int GetOptionsCount() = 0;

	/************************************************
	Summary: Returns a string describing a given option.
	Arguments:
		i: Index of option which name string should be returned.
	Return Value:
		String description of the i th option.
	Remarks:
		Refer to The Media Options part in the Creation of Bitmap Media Plugins	chapter for more details on this method.
	See Also:GetOptionsCount,Creation of Bitmap Media Plugins
	************************************************/
	virtual CKSTRING GetOptionDescription(int i) = 0;
	
	/************************************************
	Summary: Returns the reader loading/saving capabilities.
	Return Value:
		A combination of CK_DATAREADER_FLAGS reader capabilities.
	Remarks:
		A Reader can load or save  data from file,memory or an URL.
	This method returns a combination of CK_DATAREADER_FLAGS which indicates
	which of these functionnalities are supported.
	See Also: CK_DATAREADER_FLAGS
	************************************************/
	virtual CK_DATAREADER_FLAGS GetFlags() = 0;
	
};

#endif