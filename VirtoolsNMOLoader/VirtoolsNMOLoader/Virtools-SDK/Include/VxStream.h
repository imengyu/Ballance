
#ifndef VXSTREAM_H
#define VXSTREAM_H
/****************************************************************************
Summary: Base class for file IO.
Remarks:
The VxFile and VxSubFile both derives from this base class.
It defines the base method to read/write data to a stream
*****************************************************************************/
class VX_EXPORT VxStream
{
public:
	/***********************************************************************
	Summary: Starting position and direction of a seek.
	See Also:VxStream::Seek
	***********************************************************************/
	enum SeekMode {
		START,		// From start of stream
		END,		// From end of strem
		CURRENT     // From current cursor position (See VxStream::Position())
	};

	/***********************************************************************
	Summary: Returns the current cursor position.
	***********************************************************************/
	virtual XDWORD Position() = 0;

	/***********************************************************************
	Summary: Returns the stream size in bytes.
	***********************************************************************/
	virtual XDWORD Size() = 0;

	/***********************************************************************
	Summary: Returns TRUE if the stream is still valid.
	***********************************************************************/
	virtual XBOOL IsValid() = 0;

	/***********************************************************************
	Summary: Seeks to a given position into the stream, starting from a given
	origin mode.
	***********************************************************************/
	virtual XBOOL Seek(int iOffset, SeekMode iOrigin) = 0;

	/***********************************************************************
	Summary: Reads given amount of data from the stream to the buffer.
	Return Value: The number of bytes actually read
	***********************************************************************/
	virtual XDWORD Read(void* oBuffer, XDWORD iSize) = 0;

	/***********************************************************************
	Summary: Writes given amount of data from the buffer to the stream.
	Return Value: The number of bytes actually written
	***********************************************************************/
	virtual XDWORD Write(const void* iBuffer, XDWORD iSize) = 0;

	/***********************************************************************
	Summary: Copy given amount of data from the source stream to the stream.
	A size of 0 means copy the whole stream.
	Return Value: The number of bytes actually written
	***********************************************************************/
	virtual XDWORD Copy(VxStream& iStream, XDWORD iSize = 0);
};


#endif