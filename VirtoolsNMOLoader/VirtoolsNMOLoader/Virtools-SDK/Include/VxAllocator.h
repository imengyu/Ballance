
#ifndef VXALLOCATOR_H
#define VXALLOCATOR_H

#include "XArray.h"
#include "XClassArray.h"
#include "XHashTable.h"
#include "VxMutex.h"

#if defined(_XBOX_VER)
	#include <windows.h>
#endif

// Visual Studio compiler was complainiog about a missing 
// delete operator on VxPoolObject structure
#ifdef _MSC_VER
#pragma warning(disable:4291)
#endif

class XString;
class XFixedSizeAllocator;

//---- Aligned memory allocation
VX_EXPORT void* VxNewAligned(int size,int align);
VX_EXPORT void  VxDeleteAligned(void* ptr);
VX_EXPORT BOOL  VxIsAllocatedByNewAligned(void* ptr,int alignement);

#undef new
#undef delete 

//-------------------------------------------------------- 
// To quickly identify the kind of allocation that is done :


#define def_new			new 

// this is just for information, the default new could be used 
#define pool_new		new		

#define fixeds_new(x)	VxAllocator::Instance().AllocatePoolObject((x*)NULL);	


// this is the standard delete
#define def_delete		delete 

// this is just for information, the default new could be used 
#define pool_delete		delete

#define fixeds_delete(x) VxAllocator::Instance().ReleasePoolObject(x);	

/****************************************************************
Summary: Memory buffer class.

Remarks:
	o This class is designed to be used as a local or global variable that
	handles a memory buffer that will be automatically deleted on destruction. 
	o The memory buffer is aligned on a 16 bytes boundary (64 on a PS2 system)
	o Do not call delete on the buffer returned by Buffer() method.
See Also: VxNewAligned,VxDeleteAligned
****************************************************************/
class VxMemoryPool {
public:
	
	~VxMemoryPool() {
		VxDeleteAligned(m_Memory);
		m_Memory = NULL;
	}
	// Constructs the class allocating the buffer to ByteCount bytes.
	VxMemoryPool(size_t ByteCount=0) {
		m_Memory = NULL;
		Allocated= 0;
		Allocate(ByteCount);
	}

	// Returns access to the memory buffer.
	void* Buffer() const {
		return m_Memory; 
	}
	
	// Returns allocated size of this memory pool.
	size_t AllocatedSize() const {
		return Allocated; 
	}
	
	// Allocates the number of bytes if not yet allocated.
	void Allocate(size_t ByteCount) {
		if (Allocated<ByteCount) {
			VxDeleteAligned(m_Memory);
			
#ifdef PSX2
			m_Memory = (DWORD *)VxNewAligned(ByteCount,64);
#else
			m_Memory = (DWORD *)VxNewAligned(ByteCount,16);
#endif
			Allocated = ByteCount;
		}
	}
protected:	
	
	DWORD* m_Memory;
	
	size_t Allocated;

};


class VxFreeSizeAllocator  
{
	friend class VxAllocator;
public:
	VxFreeSizeAllocator(const unsigned int iPageSize = 1048576); // Default to 1 Mo Pages
	~VxFreeSizeAllocator();

	// Methods
	void*	Allocate(const size_t iSize);
	void	Release(void* iPtr);

#ifdef PSX2
	void*   GetPageDataPointer();
	void    ResetPage();   
#endif

	// Profiling functions
	int		FreeQueueSize(int Page = -1);
	void	DumpFreeQueue();
	int		DumpPage(int iPage);
	void    SetMaxPageCount(int iMaxPageCount) { m_MaxPageCount = iMaxPageCount; }

private:
	// Memory Block Size : can be more if needed
	const unsigned int m_PageSize; 
	enum {
		PAGECOUNT = 256
	};

	// Types
	struct CPage {
		CPage():m_Begin(0),m_End(0) {}
		unsigned int*	Begin() const {return m_Begin;}			
		unsigned int*	End() const {return m_End;}
		unsigned int	Size() const {return m_End - m_Begin;}
		unsigned int*	m_Begin;
		unsigned int*	m_End;
	};


	// Class to repressent the starting data of an allocation
	struct CMemoryUnit
	{
		unsigned int*	End() {return (unsigned int*)this + m_Size;}
#ifdef PSX2	
		enum {
			SIZE = 4
		};	
// 16 Bytes long
		unsigned int	m_Size; // Size of the element (and offset to the next)
		int				m_PrevOffset;
		int				m_Free;
		int				m_Page;
#else		
		enum {
			SIZE = 2
		};	
		unsigned int	m_Size; // Size of the element (and offset to the next)
		int				m_PrevOffset:23;
		int				m_Free:1;
		int				m_Page:8;
#endif 
	}; // 2 dword of size
	


	struct CFreeMemoryUnit : public CMemoryUnit
	{
		CFreeMemoryUnit*	m_Next; // Next free element
		CFreeMemoryUnit*	m_Prev; // Previous free element
	}; // 4 dword of size

	///
	// Members

	// Array of allocated pages
	CPage				m_Pages[PAGECOUNT];
	int					m_PageCount;
	int					m_MaxPageCount;

	// Array of free memory pointers
	CFreeMemoryUnit*	m_FreeQueue;

	///
	// Methods
	CPage*				AllocateNewPage(const unsigned int iSize);
	void				UseUnit(CMemoryUnit* iUnit,const unsigned int iSize);
	void				FreeUnit(CMemoryUnit* iUnit,const CPage& iPage);
	
	///
	// Free Queue operations
	CFreeMemoryUnit*	Begin() {return m_FreeQueue->m_Next;}
	CFreeMemoryUnit*	End()	{return m_FreeQueue;}
	
	inline void				InsertFreeUnit(CFreeMemoryUnit* iUnit);
	inline void				RemoveFreeUnit(CFreeMemoryUnit* iUnit);
	inline CFreeMemoryUnit*	FindFreeUnit(const unsigned int iDwordSize);

	// Profiling
	void					_DumpNodePost(CFreeMemoryUnit* iNode);
	int						m_FreeIndex;
	int						m_FreeCount;
};


class VxSmallSizeAllocator  
{
	friend class VxAllocator;
	friend class RCKRenderManager;
public:
	VxSmallSizeAllocator(); // Default to 512 Ko Pages
	~VxSmallSizeAllocator();

	// Methods
	void*	Allocate(const size_t iSize);
	void	Release(void* iPtr);
	XBOOL	IsOwner(void* iPtr);

	// Profiling/Stats functions
	int		FreeQueueSize();
	void	FreeQueueStats(int& oCount, int& oSize) const;
	void	DumpFreeQueue();
	void	DumpPage(int iPage);
	
	int		GetPageCount() {
		return m_PageCount;
	}

	#define MUPREVSIZE	18
	#define MUSIZE		(24-MUPREVSIZE)
	enum {
		PAGESIZE		= 1 << (MUPREVSIZE-1),	// Pages size : directly related to the :19 in the MemoryUnit
		PAGEBITS		= 7,
		PAGECOUNT		= 1<<PAGEBITS,		  // Pages count : directly related to the :8 in the MemoryUnit
		MAXSIZE			= 1<<(MUSIZE+1),
		FREEQUEUESCOUNT	= MAXSIZE>>2
	};
 
private:

	///
	// Types

	// Class to repressent the starting data of an allocation
	struct CMemoryUnit
	{
		XDWORD*			UsedEnd() {
			return (unsigned int*)this + m_Size;
		}
		XDWORD*			FreeEnd() {
			return (unsigned int*)this + m_PrevSize;
		}
		XDWORD*			End() {
			if (m_Free)
				return (unsigned int*)this + m_PrevSize;
			else
				return (unsigned int*)this + m_Size;
		}
		XDWORD			GetSize() {
			if (m_Free)
				return m_PrevSize;
			else
				return m_Size;
		}
		XDWORD			GetPreviousSize() {
			if (m_Free)
				return m_Size;
			else
				return m_PrevSize;
		}

		XDWORD			m_PrevSize:MUPREVSIZE;		// Size of previous element or size of the free block
		XDWORD			m_Size:MUSIZE;			// Size of the element in DWORD so limited to 64 (and offset to the next) or size of the previous allocated unit
		XDWORD			m_Free:1;			// Marker free or not
		XDWORD			m_Page:PAGEBITS;	// Page index (128 pages Max)

	}; // 1 dword of size

	// class to represent a freed unit allocation
	struct CFreeMemoryUnit : public CMemoryUnit
	{
		CFreeMemoryUnit*	m_Next; // Next free element
	}; // 2 dword of size

	// Page structure
	struct CPage {
		CPage():m_Begin(0),m_End(0) {
			// Inits the queues
			for (int i=0; i<FREEQUEUESCOUNT+1; ++i)
				m_FreeUnits[i].m_Next = &m_FreeUnits[i];
		}
		unsigned int*	Begin() const {return m_Begin;}			
		unsigned int*	End() const {return m_End;}
		unsigned int	Size() const {return m_End - m_Begin;}
		unsigned int*	m_Begin;
		unsigned int*	m_End;
		unsigned int	m_Occupation; // Size occupied of the page

		// Single linked of free memory pointers
		CFreeMemoryUnit		m_FreeUnits[FREEQUEUESCOUNT+1];
	};

	///
	// Members

	// Array of allocated pages
	CPage				m_Pages[PAGECOUNT];
	// Range of used pages (not necessary means that all the pages
	// in this range are allocated)
	XDWORD				m_PageCount;
	// array of pages containing the page index by occupation
	// (most occupied comes first, less occupied comes last)
	XDWORD				m_PageMostOccupied[PAGECOUNT];
	//we need a count because we do not store fully occupied or deleted pages
	XDWORD				m_PageMostOccupiedCount;

	///
	// Methods
	CPage*				AllocateNewPage(const unsigned int iSize);
	void				UseUnit(CMemoryUnit* iUnit,const unsigned int iSize);
	void				FreeUnit(CMemoryUnit* iUnit,const CPage& iPage);
	void				ValidatePage(const unsigned int iPage);
	void				Check();

	void				SortMostOccupied(XBOOL iAfterAllocation);
	void				AddFullPageToMostOccupied(XDWORD iPageIndex);
	void				RemovePageFromMostOccupied(XDWORD iPageIndex);

	///
	// Free Queue operations
	
	// Begin
	CFreeMemoryUnit*	Begin(CPage& iPage, const unsigned int iSize) {
		//XASSERT(iSize < FREEQUEUESCOUNT);
		// we normally don't need to test because we always know when we call for size 0
		// except in the removefreeunit !!! to optimize !
		if (iSize > FREEQUEUESCOUNT)
			return iPage.m_FreeUnits[0].m_Next;
		else
			return iPage.m_FreeUnits[iSize].m_Next;
	}
	// End
	CFreeMemoryUnit*	End(CPage& iPage, const unsigned int iSize)	{
		// XASSERT(iSize < FREEQUEUESCOUNT);
		// we normally don't need to test because we always know when we call for size 0
		// except in the removefreeunit !!! to optimize !
		if (iSize > FREEQUEUESCOUNT)
			return &(iPage.m_FreeUnits[0]);
		else 
			return &(iPage.m_FreeUnits[iSize]);
	}
	// InsertFreeUnit
	inline void			InsertFreeUnit(CPage& iPage, CFreeMemoryUnit* iUnit);
	// RemoveFreeUnit
	void				RemoveFreeUnit(CFreeMemoryUnit* iUnit) {
		XASSERT(iUnit->m_Page < m_PageCount);
		CPage& page = m_Pages[iUnit->m_Page];

		CFreeMemoryUnit* it		= Begin(page, iUnit->m_PrevSize);
		CFreeMemoryUnit* itend	= End(page, iUnit->m_PrevSize);

		CFreeMemoryUnit* prev	= itend;

		while (it != itend) {
			if (it == iUnit) { // found !
				prev->m_Next = it->m_Next;
				return;
			}
			prev	= it;
			it		= it->m_Next;
		}
	}
	// FindFreeUnit
	inline CFreeMemoryUnit*	FindAndRemoveFreeUnit(const unsigned int iDwordSize);
};

/***************************************************
Summary:Memory management  


****************************************************/
class VxAllocator
{
	friend class VxScratch;
	friend class RCKRenderManager;
	friend class RCKTriangles;
	// Allocation Mutex
	VxMutex							m_Mutex;
public:
	// CTor
	VxAllocator();
	// Dtor
	~VxAllocator();

	// Standard Allocation
VX_EXPORT	void* Allocate(size_t iSize);

	// Standard Deallocation
VX_EXPORT	void Release(void* iMem);

VX_EXPORT	void DeleteAllAllocatedDatas();

	// Gets an instance on the allocator
VX_EXPORT	static VxAllocator& Instance();

	// Updates the debug symbols
	void	UpdateSymbols(const char* iModule);

	///
	// String Public Part

	// allocate a string
VX_EXPORT	char*	AllocateString(const char* iString);
	// allocate a string
VX_EXPORT	char*	AllocateString(unsigned int iSize);
	// Release a string
VX_EXPORT	void	ReleaseString(char* iString);

	///
	// Scratch Public Part

	// allocate a scratch (temporary usage) memory
VX_EXPORT	void*	AllocateScratch(size_t iSize);
	// Release a scratch memory
VX_EXPORT	void	ReleaseScratch(void* iMemory);
	// Returns the total memory allocated for scratch
VX_EXPORT	size_t	GetAllocatedScratchMemory();
	// Returns the count of scratch pool
VX_EXPORT	int		GetAllocatedScratchCount();
	// Free the memory allocated by unused scratch pools
VX_EXPORT	void	ReleaseUnusedScratch();

	///
	// Scratch Strings

	// allocate a scratch (temporary usage) string 
VX_EXPORT	XString*	AllocateScratchString();
	// Release the scratch string
VX_EXPORT	void		ReleaseScratchString(XString* iString);


	///
	// Generic Allocation Part

	// allocate the general memory
VX_EXPORT	void*		AllocateMemory(size_t iSize);
	// Release the general memory
VX_EXPORT	void		ReleaseMemory(void* iMem);

	///
	// Pool Allocation Part

	// allocate an object from a fixed size pool
	template <class T>
	T*			AllocatePoolObject(T* iDummy)
	{
		return new (AllocatePool(sizeof(T))) T;
	}
	// Release an object from a fixed size pool
	template <class T>
	void		ReleasePoolObject(T* iMem)
	{
		iMem->~T();
		ReleasePool(sizeof(T),iMem);
	}
	// allocate memory in a fixed size pool
VX_EXPORT	void*		AllocatePool(size_t iSize);
	// Release memory allocated by a fixed size pool
VX_EXPORT	void		ReleasePool(size_t iSize, void* iMem);

	///
	// Stats

	// dump the stats into a file
VX_EXPORT	void		DumpStats(const char* iFile);
	
	// Types
	struct Stats
	{
		Stats():total(0),peak(0),accumulated(0) {}
		int		total;
		int		peak;
		int		accumulated;
		int		size;
	
		int		operator < (const Stats& iS) const
		{
			return accumulated*size > iS.accumulated*iS.size;
		}

		void Add(int i=1)
		{
			accumulated	+= i;
			total		+= i;
			if (total > peak) {
				peak = total;
			}
		}
		void Remove(int i=1)
		{
			total -= i;
			if (total < 0)
				total = 0;
		}
	};
///////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////
VX_EXPORT const Stats& GetGlobalStats() const;

private:
	


	///
	// Generic Allocation Part

	typedef XHashTable<size_t,XString>	HashCallStack2Size;
	typedef XHashTable<Stats,size_t>	HashSize2Stats;
	typedef XHashTable<size_t,void*>	HashAddress2Size;
	
	static	int CompareAllocated(const void *elem1, const void *elem2 );

	// Hash table of memory stats
#if defined(_XBOX_VER)
	struct					AllocationSentinel
	{
		AllocationSentinel() {
			InterlockedIncrement(&count);
		}
		~AllocationSentinel() {
			InterlockedDecrement(&count);			
		}
		
		static volatile LONG count;
	};
#else
	struct					AllocationSentinel
	{
		AllocationSentinel() {
			count++;
		}
		~AllocationSentinel() {
			--count;
		}
		
		static volatile int count;
	};
#endif
	HashSize2Stats			m_MemoryStats;
	HashAddress2Size		m_MemorySize;
	HashCallStack2Size		m_MemoryCallStacks;
	XString					m_TempCallStack;

	///
	// Scratch Part
	
	// allocate the scratch memory
VX_EXPORT	VxMemoryPool*	XAllocateScratch(size_t iSize);

	typedef XArray<VxMemoryPool*>	ScratchPoolArray;

	// Used scratch pool
	ScratchPoolArray				m_UsedScratch;
	// Free scratch pool
	ScratchPoolArray				m_FreeScratch;

	///
	// Scratch String Part
	
	typedef XArray<XString*>		ScratchStringPoolArray;

	// Used scratch pool
	ScratchStringPoolArray			m_UsedScratchStrings;
	// Free scratch pool
	ScratchStringPoolArray			m_FreeScratchStrings;

	///
	// String part
	
	// strings count statistics
	Stats m_StringsCount;
	// strings allocated size statistics
	Stats m_StringsSize;
	
	// Storage For global allocation stats
	mutable Stats m_GlobalAllocation;

public:
	
	int m_SharedStringsCount;
	
	int m_UnSharedStringsCount;
private:

	// temp array
	XClassArray<XString>	m_AllocatedStrings;
	XArray<const char*>		m_AllocatedStrings2;

	///
	// Pool Part

	enum {
		POOLMAXOBJECTSIZE	= 1024			// max size of objects in pool
		// BIGMARKER			= 0xbee9bee9	// Marker preceding the allocation not related to the SmallSizeAllocator
	};

	// hash table size -> pool of fixed size
	XFixedSizeAllocator*	m_Pools[POOLMAXOBJECTSIZE>>2];

	// Small Size (<64) Allocator
	VxSmallSizeAllocator	m_SmallAllocator;
	// Free Size allocator in replacement for malloc/free
	VxFreeSizeAllocator     m_FreeAllocator;
};


//-------------------------------
// Summary: Ensures a virtual class is allocated inside a fixed size memory pool.
// Remarks:
// Adding this macro to the class definition ensure
// the memory allocation will be done in a fixed size 
// pool. 
// This macro only works for class with a virtual table
// use IMPLEMENT_POOL_CLASS otherwise
#define IMPLEMENT_POOL()\
void* operator new(size_t st) {\
	return VxAllocator::Instance().AllocatePool(st);\
}\
void operator delete(void* mem,size_t st) {\
	VxAllocator::Instance().ReleasePool(st,mem);\
}\
	

//------------------------------------
// Summary: Ensures a class is allocated inside a fixed size memory pool.
// Adding this macro to the class definition ensure
// the memory allocation will be done in a fixed size 
// pool 
// For class that do not have a virtual table
// use this macro with the name of the class
// inside the class declaration 
#define IMPLEMENT_POOL_CLASS(classname)\
void* operator new(size_t st) {\
	return VxAllocator::Instance().AllocatePool(st);\
}\
void operator delete(void* mem,size_t st) {\
	VxAllocator::Instance().ReleasePool(sizeof(classname),mem);\
}\



/*************************************************************
Summary: A base class for fixed sized object that should be placed in fixed size pools. 

Remarks:
This class force the usage of a virtual destructor so it should not be used with
small structures of known size that does not need a virtual table.	Instead for a class 
to be automatically placed in a fixed size pool use the IMPLEMENT_POOL macro that will 
redefines the new and delete operator for this class
*************************************************************/
class VxPoolObject {
public:
	IMPLEMENT_POOL()
public:
	virtual ~VxPoolObject() {}
};


/*************************************************************
Summary: Class to allocate some scratch memory.
Remarks 

	Allocates a scratch for temporary needs. 
	This class release the memory when it has finished
*************************************************************/
class VxScratch
{
public:
	VxScratch(size_t iSize=0)
	{
		m_Memory = VxAllocator::Instance().XAllocateScratch(iSize);
	}

	~VxScratch()
	{
		if (m_Memory)
			VxAllocator::Instance().ReleaseScratch(m_Memory->Buffer());
	}

	void*	Check(size_t iSize)
	{
		if (!m_Memory || (iSize > m_Memory->AllocatedSize())) { // we need to reallocate
			if (m_Memory)
				VxAllocator::Instance().ReleaseScratch(m_Memory->Buffer());

			m_Memory = VxAllocator::Instance().XAllocateScratch(iSize);
		}

		return Mem();
	}

	void*	Mem() 
	{
		if (m_Memory)
			return m_Memory->Buffer();

		return 0;
	}

protected:
	// the memory
	VxMemoryPool*	m_Memory;
	
private:

	
	VxScratch(const VxScratch&);
	
	VxScratch& operator=(const VxScratch&);

};

/*************************************************************
Summary: Class to allocate some scratch memory for a string.
Remarks 

	Allocates a scratch for temporary needs. 
	This class release the memory when it has finished
*************************************************************/
class VxScratchString
{
public:
	VxScratchString(const char* iString = "")
	{
		m_String = VxAllocator::Instance().AllocateScratchString();
		*m_String = iString;		
	}

	~VxScratchString()
	{
		VxAllocator::Instance().ReleaseScratchString(m_String);
	}

	XString&	operator* () const
	{
		return *m_String;
	}

	XString*	operator-> () const
	{
		return m_String;
	}
	
protected:
	// the string
	XString*	m_String;

private:

	
	VxScratchString(const VxScratchString&);
	
	VxScratchString& operator=(const VxScratchString&);
};


/*************************************************************
Summary: Tokenize a string.
Remarks 

*************************************************************/
class XStringTokenizer
{
public:
	XStringTokenizer(const char* iString, const char* iSeparators):m_String(iString),m_Separators(iSeparators) {}

VX_EXPORT		const char* NextToken(const char* iPrevToken);

private:
	VxScratchString		m_String;
	const char*			m_Separators;
};

#endif
