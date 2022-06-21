/*************************************************************************/
/*	File : CKStateChunk.h												 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKSTATECHUNK_H
	
	#define CKSTATECHUNK_H "$Id:$"

#include "VxDefines.h"
#include "CKdefines.h"
#include "CKObject.h"
#include "CKDependencies.h"

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else
	struct ChunkIteratorData;
	typedef int (*ChunkIterateFct)(ChunkIteratorData* It);

	#define CHUNK_VERSIONBASE	0 
	#define CHUNK_VERSION1		4	// equal to file version : WriteObjectID => table
	#define CHUNK_VERSION2		5	// add Manager Data
	#define CHUNK_VERSION3		6	// New ConvertToBuffer / ReadFromBuffer (file system changed to reflect this )
	#define CHUNK_VERSION4		7	// New WriteObjectID when saving to a file 

	class IntListStruct:public XArray<int> {
	public:		
		void AddEntry(int pos) { 
			PushBack(pos);
		}
		void AddEntries(int pos) { 
			PushBack(-1);
			PushBack(pos);
		}
		void Append(int* list,int count,int StartPos) 
		{
			if (!list || !count) return; 
			for (int i = 0;i<count;++i) {
				if (list[i]>=0) {
					PushBack(list[i]+StartPos);
				} else
					PushBack(list[i]);
			}
		}
	};

	enum CHUNK_OPTIONS {
		CHNK_OPTION_IDS			= 0x01,		// IDS are stored inside chunk
		CHNK_OPTION_MAN			= 0x02,		// Managers ints are store inside chunk
		CHNK_OPTION_CHN			= 0x04,		// Sub chunk are stored	inside chunk
		CHNK_OPTION_FILE		= 0x08,		// Chunk was written with indices relative to a file....  
		CHNK_OPTION_ALLOWDYN	= 0x10,		// Dynamic object can be written in the chunk 
		CHNK_OPTION_LISTBIG		= 0x20,		// List are store in big Endian ? 
		CHNK_DONTDELETE_PTR		= 0x40,		// Data buffer stored in m_Buffer is not owned by CKStateChunk , it must not be deleted...
		CHNK_DONTDELETE_PARSER	= 0x80,		// m_Parser Ptr is not owned by CKStateChunk , it must not be deleted...
	};


	class ChunkParser : public VxPoolObject {
	public:
		CKDWORD  CurrentPos;
		CKDWORD  PrevIdentifierPos;
		virtual BOOL IsReader() { return  FALSE; }
		virtual BOOL IsWriter() { return  FALSE; }
	public:
		virtual ~ChunkParser() {}
	};

	//---------------------------------------------------------
	// When reading the content of a chunk this class 
	// keep track of the current read position
	class ChunkReader: public ChunkParser {
	public:
		virtual BOOL IsReader() { return  TRUE; }
		virtual BOOL IsWriter() { return  FALSE; }
		// These methods does not check that allocated size is correct
		inline CKDWORD Read();
		inline void Read(WORD& a,WORD& b);
		inline void Read(CKDWORD* v,int DwordCount);
		inline void Read_NoEndian(CKDWORD* v,int DwordCount);
	public:
			CKDWORD* Data;
			CKDWORD  DataSize;
			int* Ids;
			CKDWORD  IdsSize;
			int* Chunks;
			CKDWORD  ChunksSize;
			int* Managers;
			CKDWORD  ManagersSize;
	public:
		ChunkReader(CKStateChunk* chnk = NULL) { Init(chnk); }
		void Init(CKStateChunk* chnk);
	};

	//---------------------------------------------------------
	// When writing the content of a chunk this class 
	// keep track of the current write position
	// along with the list of positions IDs, Sub-Chunks and "Manager" Ints 
	// have been written
	class ChunkWriter :public ChunkParser {
	public:
		virtual BOOL IsReader() { return  FALSE; }
		virtual BOOL IsWriter() { return  TRUE; }

		CKDWORD& operator [](int i) { return Data[i]; }
		// These methods does not check that allocated size is correct
		inline void Write(CKDWORD v);
		inline void Write(WORD a,WORD b);
		inline void Write(CKDWORD* v,int DwordCount);
		inline void Write_NoEndian(CKDWORD* v,int DwordCount);
	public:
		CKDWORD			DataSize;
		XArray<CKDWORD>	Data;
		IntListStruct 	IDList;
		IntListStruct 	ChunkList;
		IntListStruct 	ManagerList;
	public:
		ChunkWriter(CKStateChunk* chnk = NULL);
		void WriteToChunk(CKStateChunk* chnk);
	};

#endif // Docjet secret macro

class CKFileChunk;
class CKBitmapReader;
struct CKBitmapProperties;


enum CK_READSUBCHUNK_FLAGS {
	CK_RSC_DEFAULT	= 0,  // Default Behavior	
	CK_RSC_SKIP		= 1,  // Just skip the sub chunk, returning NULL and advancing the read cursor.	
	CK_RSC_SCRATCH	= 2,  // Returned chunk will be allocated in a scratch pool (for temporary usages).	
};

/**************************************************************************
Name: CKStateChunk

Summary: Used to store CK object states 

Remarks:
The first use of CKStateChunk by the library is to store CK object states.( snapshot ) 
Instance of this class are returned by the CKSaveObjectState or  CreateCKStateChunk method that
is implemented for all instances of CKObject and of its derived classes.
They are used as argument to the CKReadObjectState method.

The second use is to create blocks of data that can be saved to the disk or in memory.
Methods of CKStateChunk provide easy ways to write and read any type of CkObjects, integer, float
array, buffer ,bitmap, string ,etc.. in a buffer.

Instances of CKStateChunk should be explicitely deleted with the DeleteCKStateChunk function.



See also: Using State Chunks, CKSaveObjectState, CKReadObjectState, DeleteCKStateChunk
**********************************************************************/
class CKStateChunk
{
	friend class CKFile;
	friend class ChunkReader;
	friend class ChunkWriter;
	friend class CKFileChunk;
	public:

		//----------------------------------------------------------
		// Initilisation function 
		void StartRead();
		void StartWrite();
		void CloseChunk();

		void Clear();
		void UpdateDataSize();
		CK_CLASSID	GetChunkClassID();
		void Clone(CKStateChunk *);

		//----------------------------------------------------------
		// Versions Functions
		short int  GetDataVersion();
		void  SetDataVersion(short int version);

		short int  GetChunkVersion();

		//----------------------------------------------------------
		// Parsing Functions
		// Identifiers must be unique within a chunk
		void		WriteIdentifier(CKDWORD id);
		CKBOOL 	SeekIdentifier(CKDWORD identifier);
		CKDWORD	ReadIdentifier();
		int		SeekIdentifierAndReturnSize(CKDWORD identifier);	// Return size until next identifier

		int		GetCurrentPos();
		void		Skip(int DwordCount);
		void		Goto(int DwordOffset);
		int		GetDataSize();

		//----------------------------------------------------------
		// Compression function
		
		void Pack(int CompressionLevel); 
		CKBOOL UnPack(int DestSize);		

		//----------------------------------------------------------
		//Writing functions
		void WriteByte(CKCHAR byte);
		void WriteWord(CKWORD data);
		void WriteDword(CKDWORD data);
		void WriteDwordAsWords(CKDWORD data);
		void WriteInt(int data);
		void WriteFloat(float data);
		void WriteString(const char* str);
		void WriteObjectID(CK_ID obj);
		void WriteObject(CKObject *obj);
		void WriteGuid(CKGUID data);
		void WriteVector(const VxVector& v);
		void WriteVector(const VxVector* v);
		void WriteMatrix(const VxMatrix& mat);
		void WriteObjectArray(CKObjectArray *array,CKContext* context = NULL); 
		void WriteSubChunk(CKStateChunk *sub);
		void WriteBitmap(BITMAP_HANDLE bitmap,CKSTRING ext=NULL);	// Obsolete
		void WriteReaderBitmap(const VxImageDescEx& desc,CKBitmapReader* reader,CKBitmapProperties* bp);
		void WriteManagerInt(CKGUID Manager,int val);

		// No assumptions are made about contents
		// items should be converted to little-endian (PC) format before calling these if necessary
		void WriteBuffer(int size,void *buf);
		void WriteBufferNoSize(int size,void *buf);	// Do not store size 

		// content is assumed to be dwords,int or floats
		// so it can safely be converted by the engine to little endian format if (necessary (Macintosh saving for ex...)

		void WriteBuffer_LEndian(int size,void *buf);
		void WriteBuffer_LEndian16(int size,void *buf);

		void WriteBufferNoSize_LEndian(int size,void *buf);	// Do not store size 
		void WriteBufferNoSize_LEndian16(int size,void *buf);	// Do not store size 

		//---- Sequence of Id's
		void StartObjectIDSequence(int count);
		void WriteObjectIDSequence(CK_ID id);
		void WriteObjectSequence(CKObject* obj);

		//---- Sequence of SubChunks
		void StartSubChunkSequence(int count);
		void WriteSubChunkSequence(CKStateChunk *sub);

		//---- Sequence of Managers int
		void StartManagerSequence(CKGUID man,int count);
		void WriteManagerSequence(int val);


		//----------------------------------------------------------
		// Reading functions
		int 		StartReadSequence();	// Starts reading a sequence that was written usign StartObjectIDSequence or  StartSubChunkSequence functions ,return value is the count 

		int 		StartManagerReadSequence(CKGUID* guid);	// Starts reading a sequence that was written usign StartManagerSequence functions ,return value is the count 
		int		ReadManagerIntSequence();

		CK_ID	ReadObjectID();				// Returns an object ID  
		CKObject* ReadObject(CKContext* );				// same with a pointer 

		CKBYTE	ReadByte();
		CKWORD	ReadWord();
		CKGUID	ReadGuid();
		CKDWORD	ReadDword();
		CKDWORD	ReadDwordAsWords();
		int		ReadInt();
		float	ReadFloat();
		void		ReadVector(VxVector& v);
		void		ReadVector(VxVector* v);
		void		ReadMatrix(VxMatrix& mat);
		int		ReadManagerInt(CKGUID* guid);


		const XObjectArray&			ReadXObjectArray();		
		const XObjectPointerArray&	ReadXObjectArray(CKContext* context);
		void	ReadObjectArray(CKObjectArray *array);		
		CKObjectArray* ReadObjectArray();				

		void ReadAndFillBuffer(void* buffer);			// fills buffer (must be allocated )
		void ReadAndFillBuffer(int size,void* buffer);	// fills buffer with known size (must be allocated )

		void ReadAndFillBuffer_LEndian(void* buffer);			// fills buffer (must be allocated )
		void ReadAndFillBuffer_LEndian(int size,void* buffer);	// fills buffer with known size (must be allocated )
		void ReadAndFillBuffer_LEndian16(void* buffer);			// fills buffer (must be allocated )
		void ReadAndFillBuffer_LEndian16(int size,void* buffer);	// fills buffer with known size (must be allocated )

		CKStateChunk* ReadSubChunk(CK_READSUBCHUNK_FLAGS Flags = CK_RSC_DEFAULT);
		int ReadBuffer(void** buffer);			// returns the size in bytes of the allocated buffer (// Use CKDeletePointer to delete allocated pointer)
		int	ReadString(CKSTRING* str);			// returns the length of the string including the terminating null caracter (Use CKStrdelete to delete allocated string)	
		XString	ReadString();					// returns the length of the string including the terminating null caracter 
		int	ReadString(XString& oString);		// returns the length of the string including the terminating null caracter
		int ReadStringUntilZero(char* str, int maxSize);		// returns the length of the string including the terminating null caracter (str must already be allocated)	


		//----------------------------------------------------------
		// Bitmaps functions
		BITMAP_HANDLE	ReadBitmap();
		CKBOOL			ReadReaderBitmap(const VxImageDescEx& desc);


		//----------------------------------------------------------
		
		int RemapObject(CK_ID old_id,CK_ID new_id); 
		int RemapObjects(CKContext* context,CKDependenciesContext* Depcontext=NULL); 
		int RemapManagerInt(CKGUID Manager,int* ConversionTable,int NbEntries); 
		int RemapParameterInt(CKGUID ParameterType,int* ConversionTable,int NbEntries); 

		//----------------------------------------------------------
		// concat Chunks
		void AddChunk(CKStateChunk *);
		void AddChunkAndDelete(CKStateChunk *);


		//----------------------------------------------------------
		// Conversion to buffers for file i/o
		// Buffer must be allocated by user (call  ConvertToBuffer(NULL) to get the size of the buffer needed
#ifdef DOCJETDUMMY // Docjet secret macro
#else
		int		ConvertToBuffer	 (void* buffer);
		CKBOOL ConvertFromBuffer(void *buffer);
#endif
			void* LockWriteBuffer(int DwordCount);
			void* LockReadBuffer();

		BYTE*	ReadRawBitmap(VxImageDescEx& desc);
		void		WriteRawBitmap(const VxImageDescEx& desc);


		
		
//--------------------------------------------------------
////               Private Part                     
#ifdef DOCJETDUMMY // Docjet secret macro
#else
		BOOL		ReadRawBitmapHeader(VxImageDescEx& desc);
		BOOL		ReadRawBitmapData(VxImageDescEx& desc);

	

	//IMPLEMENT_POOL_CLASS(CKStateChunk)
#endif // Docjet secret macro
}; 


class CKFileChunk {
public:
	CKStateChunk chunk;
	ChunkReader	 reader;
	void InitFromBuffer(void* Buffer,CKFile* f);
};

#endif