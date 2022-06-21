/*************************************************************************/
/*	File : CKFile.h														 */
/*	Author :  Romain Sididris											 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/

#ifndef CKFILE_H

#define CKFILE_H "$Id:$"

#include "CKObjectArray.h"
#include "CKObject.h"
#include "XClassArray.h"
#include "CKStateChunk.h"

typedef XArray<int> XIntArray;								
typedef XHashTable<int,CK_ID> XFileObjectsTable;			
typedef XFileObjectsTable::Iterator XFileObjectsTableIt;	

struct CKFileObject;	

// Summary: Callback for customized reference resolution when loading a file.
// 
//See Also:CKFile::SetResolveReferenceCallback
typedef CKObject* (*CKRRCALLBACKFCT) (CKFileObject* iFileObject);	


#ifdef PSX2
// Default Loading system is to allocate 
// for each object in the file a CKStateChunk
// which in its turn allocate the appropriate memory 
// to load an object data, to avoid memory fragmentation on PSX2
// a unique buffer is created at the beginning of a load
// in which the chunks and their buffers are created 


#define USECHUNKTABLE
#endif


typedef struct CKFileManagerData 
{
	CKStateChunk *data;
	CKGUID Manager;
} CKFileManagerData;

/*************************************************
Summary: Handle the saving of .cmo/.nmo files in stream (memory,file,etc..).

Remarks: 
A CMO,NMO or NMS is saved using a standard file by default but this
the save operation can made in any stream (memory , file) provided that 
a instance of this class is given to handle the different operation.
The various method of this class should be overriden if a inheritated class
and will be called by the framework when trying to save a file.
Example:
		//-----------------------------------------
		//The default file saver for a CKFile
		struct CKFileStreamWriter : public CKStreamWriter {
			CKFileStreamWriter() {
				m_Filename = NULL;
			}
			virtual ~CKFileStreamWriter() {
			}
			virtual XBOOL Open(CKSTRING filename) {
				m_Filename = filename;
				return m_File.Open(filename,VxFile::WRITEONLY);
			}
			virtual XDWORD Write(const void* iBuffer,XDWORD iSize) {
				return m_File.Write(iBuffer,iSize);
			}
			virtual XBOOL Close() {
				m_Filename = NULL;
				return m_File.Close();
			}
			virtual XBOOL IsValid(CKSTRING filename) {
				if (!m_File.Open(filename,VxFile::APPEND)) {
					return FALSE;
				}
				m_File.Close();
				return TRUE;
			}
			CKSTRING m_Filename;
			VxFile 	 m_File;
		};
		
		//---------------------------------------
		//A writer to a memory buffer
		struct CKMemoryStreamWriter : public CKStreamWriter {
			CKMemoryStreamWriter() {
			}
			virtual ~CKMemoryStreamWriter() {
			}
			virtual XBOOL Open(CKSTRING filename) {
				m_Buffer.Resize(0);
			}
			virtual XDWORD Write(const void* iBuffer,XDWORD iSize) {
				XDWORD OldSize = m_Buffer.Size();
				m_Buffer.Resize(OldSize + iSize);
				memcpy(m_Buffer.Begin()+OldSize, iBuffer, iSize);
			}
			virtual XBOOL Close() {
			}
			virtual XBOOL IsValid(CKSTRING filename) {
				return TRUE;
			}
			XArray<BYTE> m_Buffer;
		};
		
		
		
		//Usage example
		//Saving a content in memory
		
		CKObjectArray* objectToSave;
		CKStreamWriter* myWriter = new CKMemoryStreamWriter;
		
		//Install our writer
		g_Context->SetStreamWriter(myWriter);
		
		//Save Data : No file will be created instead we will call
		//the writer methods
		g_Context->Save("testFile.cmo",objectToSave);
		
		//Restore default saving behavior
		g_Context->SetStreamWriter(NULL);
		//And free our saving object
		delete myWriter;

See also: CKContext::SetStreamWriter
*************************************************/
struct CKStreamWriter {
	virtual ~CKStreamWriter() {};
	/*************************************************
	Summary: Called by the framework when opening a stream to write to
	Remarks:
		The filename that given in argument is only useful if this writer handles writing
	to a file otherwise it can be ignored.
	***************************************************/
	virtual XBOOL Open(CKSTRING filename) = 0;
	/************************************************************
	Summary: Called by the framework to write data to the stream.
	Remarks:
		This method should return the number of bytes actually written.
	*************************************************************/
	virtual XDWORD Write(const void* iBuffer,XDWORD iSize) = 0;
	/************************************************************
	Summary: Called by the framework when the stream can be closed.
	*************************************************************/
	virtual XBOOL Close() = 0;
/***************************************************
Summary: Called by the framework when starting to save a composition.
Remarks:
This method is called to ensure that the save operation can continue.
For example to check that a file can be written. 
The filename that given in argument is only useful if this writer handles writing
to a file otherwise it can be ignored.
It should return TRUE if the save operation can continue.
*****************************************************/
	virtual XBOOL IsValid(CKSTRING filename) = 0;
};

/*************************************************
Summary: List of Plugins guids used by a file.
Remarks: 
	+ When saving a file, a list of the plugins needed 


See also: CKFile::GetMissingPlugins
*************************************************/
class CKFilePluginDependencies {
public:
	int				m_PluginCategory;
	XArray<CKGUID>  m_Guids;
	XBitArray		ValidGuids;
};

/*************************************************
Summary: Description of a loaded/saved object.

Remarks: 
	+ When saving or loading a file, each object is assigned a description that can be used by managers
to do remapping or data modification. 
See also: CKObject::Save,CKObject::Load
*************************************************/
typedef struct CKFileObject {

// Options that will be used to create this object...
	enum CK_FO_OPTIONS {
		CK_FO_DEFAULT = 0,		 // Default behavior : a new object will be created with the name stored in CKFileObject
		CK_FO_RENAMEOBJECT,		 // Renaming : a new object will be created with the name stored in CKFileObject + a integer value XXX to ensure its uniqueness
		CK_FO_REPLACEOBJECT,	 // Do not create a new object, instead use an existing one which CK_ID is given by CreatedObject   	
								 // to load the chunk on 
		CK_FO_DONTLOADOBJECT,	 // Object chunk will not be read either because it is a reference
								// or because the loaded object already exist in the current level 
								// and the user choose to keep the existing one.
	};


	CK_ID			Object;			// ID of the object being load/saved (as it will be/was saved in the file)
	CK_ID			CreatedObject;	// ID of the object being created
	CK_CLASSID		ObjectCid;		// Class Identifier of the object
	CKObject*		ObjPtr;			// A pointer to the object itself (as CreatedObject when loading)
	CKSTRING		Name;			// Name of the Object
	CKStateChunk*	Data;			// A CKStateChunk that contains object information
	int				PostPackSize;	// When compressed chunk by chunk : size of Data after compression
	int				PrePackSize;	// When compressed chunk by chunk : size of Data before compression
	CK_FO_OPTIONS	Options;		// When loading an object it may be renamed , use to replace another object 	
	int				FileIndex;		// Position of the object data inside uncompressed file buffer
	CKDWORD			SaveFlags;		// Flags used when this object was saved.	

	
	BOOL CanBeLoad() {
		return (ObjPtr && Data && (Options !=CK_FO_DONTLOADOBJECT));
	}
	
	void CleanData() {
#ifndef USECHUNKTABLE // No need to delete if memory is taken from a global table...
		delete Data;  
#endif
		Data = NULL;
	}
	
	CKFileObject() {
		memset(this,0,sizeof(CKFileObject));
	}
} CKFileObject;


class CKFileChunk;
class CKMemoryBufferParser;

/*********************************************************
{secret}
Base class to parse a buffer (
two implementation exist using either 
a memory buffer or a file...
*********************************************************/
class CKBufferParser : public VxPoolObject {
friend class CKFile;
public:
	virtual ~CKBufferParser() {};

//----- Read Write method
	virtual BOOL Write(void* x,int size) = 0;
	virtual BOOL Read(void* x,int size) = 0;
	virtual char* ReadString() = 0;
	virtual int ReadInt() = 0;

//------ Cursor position 
	virtual void Seek(int Pos) = 0;
	virtual void Skip(int Offset) = 0;

//------- Is Buffer valid
	virtual BOOL IsValid() = 0;
	virtual int  Size() = 0;
	virtual int  CursorPos() = 0;

//----- Reading Utilities (always relative to current cursor position
// Warning : All these methods advance the Cursor of Size bytes !	
	
	// Create a CKStateChunk from the Size bytes
	// returns NULL if data was not valid
	virtual CKStateChunk*	ExtractChunk(int Size,CKFile* f) = 0;		
	virtual void			ExtractChunk(int Size,CKFile* f,CKFileChunk* chnk) = 0;
	// Returns the CRC of the next Size bytes
	virtual DWORD			ComputeCRC(int Size,DWORD PrevCRC=0) = 0; 
	// Returns a new BufferParser containing the next size bytes or NULL
	// if Size is <=0 
	virtual CKMemoryBufferParser* Extract(int Size) = 0;
	// Saves the next Size bytes to a file 
	virtual BOOL ExtractFile(char* Filename,int Size) = 0;
	// Same version but with decoding 
	virtual CKMemoryBufferParser* ExtractDecoded(int Size,DWORD Key[4]) = 0;
	// Returns a new BufferParser containing the next PackSize bytes 
	// unpacked to UnpackSize
	virtual CKBufferParser*		UnPack(int UnpackSize,int PackSize) = 0;

//----- Writing Utilities (always relative to current cursor position
// Warning : All these methods advance the Cursor of Size bytes !		
	
	virtual void			InsertChunk(CKStateChunk* chunk) = 0;		
	// Returns a new BufferParser containing the next Size bytes 
	// packed with given compression level
	virtual CKMemoryBufferParser* Pack(int Size,int CompressionLevel) = 0;

//---- Others
	//- Encode the next Size bytes (This does not increment the cursor pointer)
	virtual void			Encode(int Size,DWORD Key[4]) = 0;		
	
	//secret
	void SkipString();
};



CKDWORD GetCurrentFileLoadOption();	
// returns TheHeader.FileVersion (from CKFileHader).
CKDWORD GetCurrentFileVersion();		
// returns TheHeader.CKVersion (from CKFileHader).
CKDWORD GetCurrentVersion();			
// set last ckfile version 0 => reset last ckfileloaded version with current dev's version
void SetCurrentVersion(CKDWORD iNewVersion=0);
/*************************************************
Summary: CKFile provides functions to save/load files in Virtools format.

Remarks: 
+ A CKFile is created through CreateCKFile and should be deleted after use using DeleteCKFile.
+ Once created a CKFile can be used to save one or more objects to disk or memory.
+ Normally you should not have to use this class directly since the CKContext::Load and CKContext::Save 
methods handle most cases.

See also: CreateCKFile,DeleteCKFile
*************************************************/
class CKFile {
friend class CKStateChunk;
public:

//------------------------------------------------				
// Loading (OpenFile  then LoadFileData )
CKERROR OpenFile(CKSTRING filename,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);
CKERROR OpenMemory(void* MemoryBuffer,int BufferSize,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);
CKERROR OpenStream(VxStream* stream,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);

CKERROR LoadFileData(CKObjectArray *list);

//------------------------------------------------				
// Direct Loading
CKERROR Load(CKSTRING filename,CKObjectArray *list,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);
CKERROR Load(void* MemoryBuffer,int BufferSize,CKObjectArray *list,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);
CKERROR Load(VxStream* stream,CKObjectArray *list,CK_LOAD_FLAGS Flags=CK_LOAD_DEFAULT);


void	UpdateAndApplyAnimationsTo(CKCharacter* character);

//------------------------------------------------
// Saving
CKERROR StartSave(CKSTRING filename,CKDWORD Flags=CK_STATESAVE_ALL);
void	SaveObject(CKObject *obj,CKDWORD flags=CK_STATESAVE_ALL);
void	SaveObjects(CKObjectArray *array,CKDWORD flags=CK_STATESAVE_ALL);
void	SaveObjects(CK_ID* ids,int count,CKDWORD flags=CK_STATESAVE_ALL);
void	SaveObjects(CKObject** obs,int count,CKDWORD flags=CK_STATESAVE_ALL);
// Add obj to the list of objects to be saved 
void	SaveObjectAsReference(CKObject *obj,CKDWORD flags=0);
// Add obj to the list of objects to be saved as references
CKERROR EndSave();

CKBOOL	IncludeFile(CKSTRING FileName,int SearchPathCategory = -1);

CKBOOL	IsObjectToBeSaved(CK_ID iID);


//-------------------------------------------------
// Used to update from old file formats
void LoadAndSave(CKSTRING filename,CKSTRING filename_new);
//-- Remap every chunks according to the conversion table
void RemapManagerInt(CKGUID Manager,int* ConversionTable,int TableSize);

// Sets a customized reference resolution function,
// which replaces CKFile::ResolveReference() implementation.
void SetResolveReferenceCallback(CKRRCALLBACKFCT iCallback);

//-------------------------------------------------
// Dependencies :
XClassArray<CKFilePluginDependencies> *GetMissingPlugins();

#ifdef DOCJETDUMMY	  // Docjet secret macro
#else


	
	CKFile(CKContext* Context);
	~CKFile();

//-----------------------------------------------
// find the created object ID, knowing the chunk read ID 	
//--------------------------------------------- 
	int FindCreatedID(CK_ID iReadIDFromChunk)
	{
		int count = m_FileObjects.Size();
		for (int i=0;i<count;++i)
		{
			if (m_FileObjects[i].Object==iReadIDFromChunk)
				return m_FileObjects[i].CreatedObject;
		}
		return 0;
	}

protected:
	void ClearData();	
	
	CKERROR ReadFileHeaders(CKBufferParser** ParserPtr);
	CKERROR ReadFileData(CKBufferParser** ParserPtr);
	void	FinishLoading(CKObjectArray *list,DWORD flags);

//-----------------------------------------------
// Debug ouput : 
// File statistic on file size and memory taken by each
// class of objects...
//--------------------------------------------- 
	void WriteStats(int InterfaceDataSize);

//-----------------------------------------------
// When writing the ID of an object inside a chunk, 	
// we instead write its index inside the file which was
// stored in the m_ObjectsHashTable	
//--------------------------------------------- 
	int SaveFindObjectIndex(CK_ID obj) {
		int Index=-1;
		m_ObjectsHashTable.LookUp(obj,Index);
		return Index;
	}

//-----------------------------------------------
// when loading an object we can then convert an index inside the
// file object table into the CK_ID of the object that as been created
//--------------------------------------------- 
	CK_ID LoadFindObject(int index) {
		return (index>=0) ? m_FileObjects[index].CreatedObject : 0;
	}

//-----------------------------------------------
// The object stored in this file object is only a reference to 
// another object that is supposed to be already existing
// This method is only supported for parameters for the moment
// to enable an automatic shortcut remapping
//--------------------------------------------- 
	CKObject* ResolveReference(CKFileObject* Data);

//-----------------------------------------------
// This method check every loaded object name unicity 
// and is called at the end of OpenFile according
// to the load options. It builds a list of 
// duplicates and store it in m_DuplicateNameFounds
// (List of indices in the m_FileObjects table)
//--------------------------------------------- 
	void CheckDuplicateNames();
	
//This method saves a thumbnail for Windows explorer in the CKFile if the 
//File Options/Save Thumbnail of the variable manager is TRUE.
	CKBOOL SaveThumbnail();

public:
	int							m_SaveIDMax;			// Maximum CK_ID found when saving or loading objects  {secret}
	XArray<CKFileObject>		m_FileObjects;			// List of objects being saved / loaded   {secret}
	XArray<CKFileManagerData>	m_ManagersData;			// Manager Data loaded  {secret}
	XClassArray<CKFilePluginDependencies> m_PluginsDep;	// Plugins dependencies for this file  {secret}
	XClassArray<XIntArray>		m_IndexByClassId;		// List of index in the m_FileObjects table sorted by ClassID  {secret}
	XClassArray<XString>		m_IncludedFiles;		// List of files that should be inserted in the CMO file.  {secret}
	CKFileInfo					m_FileInfo;				// Headers summary  {secret}
	CKDWORD						m_Flags;				// Flags used to save file {secret}
	CKSTRING					m_FileName;				// Current file name  {secret}
	CKContext*					m_Context;				// CKContext on which file is loaded/Saved  {secret}
	CKBufferParser*				m_Parser;
	VxMemoryMappedFile*			m_MappedFile;
	XFileObjectsTable			m_ObjectsHashTable;
#ifdef USECHUNKTABLE
	XClassArray<CKFileChunk>	m_FileChunks;			// Instead of allocating chunk per chunk a whole memory buffer is allocated to store all chunks and their readers
	CKFileChunk*				m_ObjectChunks;
	CKFileChunk*				m_ManagersChunks;
	VxMemoryPool				m_ChunkBuffers;			// Store all decompressed file buffer in memory so that all chunks directly points to it...
														// can only work for recent files ( > 2.0)
	BYTE*						m_CurrentChunkBufferPtr;
#endif					

	CKBOOL						m_ReadFileDataDone;
	CKBOOL						m_SceneSaved;
//-----------
	XIntArray					m_DuplicateNameFounds;	// A List of file object index for which a existing object with the same name has been
														// found, this list is build if the load option contains CK_LOAD_AUTOMATICMODE or CK_LOAD_DODIALOG

//----- 	
	XBitArray					m_AlreadySavedMask;			// BitArray of IDs already saved  {secret}
	XBitArray					m_AlreadyReferencedMask;	// BitArray of IDs already referenced  {secret}
	XObjectPointerArray			m_ReferencedObjects;
	VxTimeProfiler				m_Chrono;

protected:
	CKRRCALLBACKFCT				m_ResolveReferenceCallbackFct;	// callback called in place of ResolveReference() implementation

public:
	static char m_CIDNeedName[CKCID_MAXCLASSID];

#endif // Docjet secret macro
}; 


#endif