/*************************************************************************/
/*	File : CKSoundReader.h												 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSOUNDREADER_H

#define CKSOUNDREADER_H


#include "CKDataReader.h"
#include "CKSoundManager.h"


// 
typedef enum CK_SOUNDREADER_ERROR {
	CKSOUND_READER_OK			=	0,
	CKSOUND_READER_GENERICERR	=	-1,
	CKSOUND_READER_EOF			=	-2,
	CKSOUND_READER_NO_DATA_READY=	-3
} CK_SOUNDREADER_ERROR;


// 
class CKSoundReader: public CKDataReader  
{
protected:
	CK_DATAREADER_FLAGS m_Flags;
public:
	void* m_SubFileMem;			//Used to keep data in memory when read from VxBigfile files when sound is streamed
public:
	
	virtual ~CKSoundReader(){}
	
	virtual CK_DATAREADER_FLAGS GetFlags() {return m_Flags; }

	virtual CKERROR OpenFile(char *file) = 0;
	virtual CKERROR Decode() = 0;

	virtual CKERROR GetDataBuffer(BYTE **buf, int *size) = 0;
	
	virtual CKERROR GetWaveFormat(CKWaveFormat *wfe) = 0;

	virtual int GetDataSize() = 0;
	
	virtual int GetDuration() = 0;

	virtual CKERROR Play() = 0;

	virtual CKERROR Stop() = 0;

	virtual CKERROR Pause() = 0;

	virtual CKERROR Resume() = 0;
	
	virtual CKERROR Seek(int pos) = 0;
	
	virtual CKERROR ReadMemory(void* memory, int size){return CK_OK;};
	

};

#endif 
