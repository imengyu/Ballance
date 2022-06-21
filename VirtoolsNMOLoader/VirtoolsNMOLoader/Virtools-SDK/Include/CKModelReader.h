/*************************************************************************/
/*	File : CKModelReader.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2001, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef _CKMODELREADER_H_
#define _CKMODELREADER_H_

#include "CKDataReader.h"

/***********************************************
Summary: Base class for model readers
Remarks:
	+ Only the Load and Save method are used for a model reader

See Also:Creation of Model Media Plugins,Creation of Media Plugins,CKDataReader,CKPluginManager::GetModelReader
***********************************************/
class CKModelReader : public CKDataReader
{
public:
	
	void Release() = 0;
	
	
	virtual CK_DATAREADER_FLAGS GetFlags() {return (CK_DATAREADER_FLAGS) (CK_DATAREADER_FILELOAD);}

/************************************************
Summary: Loads a model file.
Return Value:
	CK_OK if successful, an error code otherwise
Arguments:
	context: A pointer to the CKContext to use to create objects
	FileName: Name of the file to load
	liste: A pointer to a CKObjectArray that will be filled with the loaded objects.
	LoadFlags: A combination of CK_LOAD_FLAGS
See Also:Creation of Model Media Plugins
************************************************/
	virtual CKERROR Load(CKContext* context,CKSTRING FileName,CKObjectArray *liste,CKDWORD LoadFlags,CKCharacter *carac=NULL) {	return CKERR_NOTIMPLEMENTED;	}

/************************************************
Summary: Loads a model file from memory.
Return Value:
	CK_OK if successful, an error code otherwise
Arguments:
	context: A pointer to the CKContext to use to create objects
	iBuffer: pointer on the buffer to parse
	iSize: size of the buffer to parse
	liste: A pointer to a CKObjectArray that will be filled with the loaded objects.
	LoadFlags: A combination of CK_LOAD_FLAGS
See Also:Creation of Model Media Plugins
************************************************/
	virtual CKERROR Load(CKContext* context,const void* iBuffer, XDWORD iSize,CKObjectArray *liste,CKDWORD LoadFlags,CKCharacter *carac=NULL) {	return CKERR_NOTIMPLEMENTED;	}

	/************************************************
Summary: Saves a model file.
Return Value:
	CK_OK if successful, an error code otherwise
Arguments:
	context: A pointer to the CKContext
	FileName: Name of the file to save
	liste: A pointer to a CKObjectArray that contains the objects to save
See Also:Creation of Model Media Plugins
************************************************/
	virtual CKERROR Save(CKContext* context,CKSTRING FileName,CKObjectArray *liste,CKDWORD SaveFlags)	 {	return CKERR_NOTIMPLEMENTED;	}

/************************************************
Summary: Loads a model file from a stream.
Return Value:
	CK_OK if successful, an error code otherwise
Arguments:
	context: A pointer to the CKContext to use to create objects
	inStream: reference to a VxStream object 
	liste: A pointer to a CKObjectArray that will be filled with the loaded objects.
	LoadFlags: A combination of CK_LOAD_FLAGS
See Also:VxStream,Creation of Model Media Plugins
************************************************/
	virtual CKERROR Load(CKContext* context,VxStream* stream,CKObjectArray *liste,CKDWORD LoadFlags,CKCharacter *carac=NULL) {	return CKERR_NOTIMPLEMENTED;	}
	
	
};

#endif