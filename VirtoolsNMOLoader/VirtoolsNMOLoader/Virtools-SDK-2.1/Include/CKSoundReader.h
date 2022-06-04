/*************************************************************************/
/*	File : CKSoundReader.h												 */
/*	Author :  Nicolas Galinotti											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef CKSOUNDREADER_H
#define CKSOUNDREADER_H

#include "CKDataReader.h"
#include "CKSoundManager.h"

//
typedef enum CK_SOUNDREADER_ERROR
{
    CKSOUND_READER_OK            = 0,
    CKSOUND_READER_GENERICERR    = -1,
    CKSOUND_READER_EOF           = -2,
    CKSOUND_READER_NO_DATA_READY = -3
} CK_SOUNDREADER_ERROR;

//
class CKSoundReader : public CKDataReader
{
protected:
    CK_DATAREADER_FLAGS m_Flags;

public:
    void *m_SubFileMem; // Used to keep data in memory when read from VxBigfile files when sound is streamed
public:
    virtual ~CKSoundReader() {}

    virtual CK_DATAREADER_FLAGS GetFlags() { return m_Flags; }

    virtual CKERROR OpenFile(char *file) = NULL;
    virtual CKERROR Decode() = NULL;

    virtual CKERROR GetDataBuffer(BYTE **buf, int *size) = NULL;

    virtual CKERROR GetWaveFormat(CKWaveFormat *wfe) = NULL;

    virtual int GetDataSize() = NULL;

    virtual int GetDuration() = NULL;

    virtual CKERROR Play() = NULL;

    virtual CKERROR Stop() = NULL;

    virtual CKERROR Pause() = NULL;

    virtual CKERROR Resume() = NULL;

    virtual CKERROR Seek(int pos) = NULL;

    virtual CKERROR ReadMemory(void *memory, int size) { return CK_OK; };
};

#endif
