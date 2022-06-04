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
typedef XHashTable<int, CK_ID> XFileObjectsTable;
typedef XFileObjectsTable::Iterator XFileObjectsTableIt;

typedef struct CKFileManagerData
{
    CKStateChunk *data;
    CKGUID Manager;
} CKFileManagerData;

/*************************************************
Summary: List of Plugins guids used by a file.
Remarks:
    + When saving a file, a list of the plugins needed


See also: CKFile::GetMissingPlugins
*************************************************/
class CKFilePluginDependencies
{
public:
    int m_PluginCategory;
    XArray<CKGUID> m_Guids;
    XBitArray ValidGuids;
};

/*************************************************
Summary: Description of a loaded/saved object.

Remarks:
    + When saving or loading a file, each object is assigned a description that can be used by managers
to do remapping or data modification.
See also: CKObject::Save,CKObject::Load
*************************************************/
typedef struct CKFileObject
{

    // Options that will be used to create this object...
    enum CK_FO_OPTIONS
    {
        CK_FO_DEFAULT = 0,	  // Default behavior : a new object will be created with the name stored in CKFileObject
        CK_FO_RENAMEOBJECT,	  // Renaming : a new object will be created with the name stored in CKFileObject + a integer value XXX to ensure its uniqueness
        CK_FO_REPLACEOBJECT,  // Do not create a new object, instead use an existing one which CK_ID is given by CreatedObject
                              // to load the chunk on
        CK_FO_DONTLOADOBJECT, // Object chunk will not be read either because it is a reference
                              // or because the loaded object already exist in the current level
                              // and the user choose to keep the existing one.
    };

    CK_ID Object;		   // ID of the object being load/saved (as it will be/was saved in the file)
    CK_ID CreatedObject;   // ID of the object being created
    CK_CLASSID ObjectCid;  // Class Identifier of the object
    CKObject *ObjPtr;	   // A pointer to the object itself (as CreatedObject when loading)
    CKSTRING Name;		   // Name of the Object
    CKStateChunk *Data;	   // A CKStateChunk that contains object information
    int PostPackSize;	   // When compressed chunk by chunk : size of Data after compression
    int PrePackSize;	   // When compressed chunk by chunk : size of Data before compression
    CK_FO_OPTIONS Options; // When loading an object it may be renamed , use to replace another object
    int FileIndex;		   // Position of the object data inside uncompressed file buffer
    CKDWORD SaveFlags;	   // Flags used when this object was saved.

    BOOL CanBeLoad()
    {
        return (ObjPtr && Data && (Options != CK_FO_DONTLOADOBJECT));
    }

    void CleanData()
    {
#ifndef USECHUNKTABLE // No need to delete if memory is taken from a global table...
        delete Data;
#endif
        Data = NULL;
    }

    CKFileObject()
    {
        memset(this, 0, sizeof(CKFileObject));
    }
} CKFileObject;

class CKFileChunk;

/*********************************************************
{secret}
Base class to parse a buffer (
two implementation exist using either
a memory buffer or a file...
*********************************************************/
class CKBufferParser
{
    friend class CKFile;

public:
    CKBufferParser(void *Buffer, int Size);
    ~CKBufferParser(){};

    //----- Read Write method
    BOOL Write(void *x, int size);
    BOOL Read(void *x, int size);
    char *ReadString();
    int ReadInt();

    //------ Cursor position
    void Seek(int Pos);
    void Skip(int Offset);

    //------- Is Buffer valid
    BOOL IsValid();
    int Size();
    int CursorPos();

    //----- Reading Utilities (always relative to current cursor position
    // Warning : All these methods advance the Cursor of Size bytes !

    // Create a CKStateChunk from the Size bytes
    // returns NULL if data was not valid
    CKStateChunk *ExtractChunk(int Size, CKFile *f);
    void ExtractChunk(int Size, CKFile *f, CKFileChunk *chnk);

    // Returns the CRC of the next Size bytes
    DWORD ComputeCRC(int Size, DWORD PrevCRC = 0);
    // Returns a new BufferParser containing the next size bytes or NULL
    // if Size is <=0
    CKBufferParser *Extract(int Size);
    // Saves the next Size bytes to a file
    BOOL ExtractFile(char *Filename, int Size);
    // Same version but with decoding
    CKBufferParser *ExtractDecoded(int Size, DWORD Key[4]);
    // Returns a new BufferParser containing the next PackSize bytes
    // unpacked to UnpackSize
    CKBufferParser *UnPack(int UnpackSize, int PackSize);

    //----- Writing Utilities (always relative to current cursor position
    // Warning : All these methods advance the Cursor of Size bytes !

    void InsertChunk(CKStateChunk *chunk);
    // Returns a new BufferParser containing the next Size bytes
    // packed with given compression level
    CKBufferParser *Pack(int Size, int CompressionLevel);

    //---- Others
    //- Encode the next Size bytes (This does not increment the cursor pointer)
    void Encode(int Size, DWORD Key[4]);

public:
    void *m_Buffer;
    int m_CursorPos;
    BOOL m_Valid;
    int m_Size;
};

CKDWORD GetCurrentFileLoadOption();
CKDWORD GetCurrentFileVersion();

/*************************************************
Summary: CKFile provides functions to save/load files in Virtools format.

Remarks:
    + A CKFile is created through CreateCKFile and should be deleted after use using DeleteCKFile.
    + Once created a CKFile can be used to save one or more objects to disk or memory.
    + Normally you should not have to use this class directly since the CKContext::Load and CKContext::Save
    methods handle most cases.

See also: CreateCKFile,DeleteCKFile
*************************************************/
class CKFile
{
    friend class CKStateChunk;

public:
    //------------------------------------------------
    // Loading (OpenFile  then LoadFileData )
    CKERROR OpenFile(CKSTRING filename, CK_LOAD_FLAGS Flags = CK_LOAD_DEFAULT);
    CKERROR OpenMemory(void *MemoryBuffer, int BufferSize, CK_LOAD_FLAGS Flags = CK_LOAD_DEFAULT);

    CKERROR LoadFileData(CKObjectArray *list);

    //------------------------------------------------
    // Direct Loading
    CKERROR Load(CKSTRING filename, CKObjectArray *list, CK_LOAD_FLAGS Flags = CK_LOAD_DEFAULT);
    CKERROR Load(void *MemoryBuffer, int BufferSize, CKObjectArray *list, CK_LOAD_FLAGS Flags = CK_LOAD_DEFAULT);

    void UpdateAndApplyAnimationsTo(CKCharacter *character);

    //------------------------------------------------
    // Saving
    CKERROR StartSave(CKSTRING filename, CKDWORD Flags = CK_STATESAVE_ALL);
    void SaveObject(CKObject *obj, CKDWORD flags = CK_STATESAVE_ALL);
    void SaveObjects(CKObjectArray *array, CKDWORD flags = CK_STATESAVE_ALL);
    void SaveObjects(CK_ID *ids, int count, CKDWORD flags = CK_STATESAVE_ALL);
    void SaveObjects(CKObject **obs, int count, CKDWORD flags = CK_STATESAVE_ALL);
    // Add obj to the list of objects to be saved
    void SaveObjectAsReference(CKObject *obj);
    CKERROR EndSave();

    CKBOOL IncludeFile(CKSTRING FileName, int SearchPathCategory = -1);

    CKBOOL IsObjectToBeSaved(CK_ID iID);

    //-------------------------------------------------
    // Used to update from old file formats
    void LoadAndSave(CKSTRING filename, CKSTRING filename_new);
    //-- Remap every chunks according to the conversion table
    void RemapManagerInt(CKGUID Manager, int *ConversionTable, int TableSize);

    //-------------------------------------------------
    // Dependencies :
    XClassArray<CKFilePluginDependencies> *GetMissingPlugins();

#ifdef DOCJETDUMMY // Docjet secret macro
#else

    CKFile(CKContext *Context);
    ~CKFile();

protected:
    void ClearData();

    CKERROR ReadFileHeaders(CKBufferParser **ParserPtr);
    CKERROR ReadFileData(CKBufferParser **ParserPtr);
    void FinishLoading(CKObjectArray *list, DWORD flags);

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
    int SaveFindObjectIndex(CK_ID obj)
    {
        int Index = -1;
        m_ObjectsHashTable.LookUp(obj, Index);
        return Index;
    }
    //-----------------------------------------------
    // when loading an object we can then convert an index inside the
    // file object table into the CK_ID of the object that as been created
    //---------------------------------------------
    CK_ID LoadFindObject(int index)
    {
        return (index >= 0) ? m_FileObjects[index].CreatedObject : 0;
    }

    //-----------------------------------------------
    // The object stored in this file object is only a reference to
    // another object that is supposed to be already existing
    // This method is only supported for parameters for the moment
    // to enable an automatic shortcut remapping
    //---------------------------------------------
    CKObject *ResolveReference(CKFileObject *Data);

    //-----------------------------------------------
    // This method check every loaded object name unicity
    // and is called at the end of OpenFile according
    // to the load options. It builds a list of
    // duplicates and store it in m_DuplicateNameFounds
    // (List of indices in the m_FileObjects table)
    //---------------------------------------------
    void CheckDuplicateNames();

public:
    int m_SaveIDMax;									// Maximum CK_ID found when saving or loading objects  {secret}
    XArray<CKFileObject> m_FileObjects;					// List of objects being saved / loaded   {secret}
    XArray<CKFileManagerData> m_ManagersData;			// Manager Data loaded  {secret}
    XClassArray<CKFilePluginDependencies> m_PluginsDep; // Plugins dependencies for this file  {secret}
    XClassArray<XIntArray> m_IndexByClassId;			// List of index in the m_FileObjects table sorted by ClassID  {secret}
    XClassArray<XString> m_IncludedFiles;				// List of files that should be inserted in the CMO file.  {secret}
    CKFileInfo m_FileInfo;								// Headers summary  {secret}
    CKBOOL m_SceneSaved;
    XBitArray m_AlreadySavedMask;						// BitArray of IDs already saved  {secret}
    CKDWORD m_Flags;									// Flags used to save file {secret}
    CKSTRING m_FileName;								// Current file name  {secret}
    CKContext *m_Context;								// CKContext on which file is loaded/Saved  {secret}
    CKBufferParser *m_Parser;
    VxMemoryMappedFile *m_MappedFile;
    XFileObjectsTable m_ObjectsHashTable;
    CKBOOL m_ReadFileDataDone;
    XBitArray m_AlreadyReferencedMask;					// BitArray of IDs already referenced  {secret}
    XObjectPointerArray m_ReferencedObjects;

#endif // Docjet secret macro
};

#endif