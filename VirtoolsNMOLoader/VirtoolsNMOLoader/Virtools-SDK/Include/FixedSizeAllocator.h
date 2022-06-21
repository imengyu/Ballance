#ifndef FIXEDSIZEALLOCATOR_H
#define FIXEDSIZEALLOCATOR_H


/////////////////////////////////////
// XFixedSizeAllocator
// created  : AGoTH
// Date		: 05/12/01
//
// This class os to allocate fixed size object
// with a constant time allocation and deallocation
#ifndef _WIN32_WCE 
	#include <limits.h>

	#undef new
	#undef delete

	#include <new.h>
#endif

#include "XBitArray.h"

// 
class XFixedSizeAllocator
{
public:
	enum {
		DEFAULT_CHUNK_SIZE = 4096
	};

	VX_EXPORT XFixedSizeAllocator(const int iBlockSize,const int iPageSize = DEFAULT_CHUNK_SIZE);
	VX_EXPORT ~XFixedSizeAllocator();
	
	// return the number of allocated chunks
	int			GetChunksCount()
	{
		return m_Chunks.Size();
	}

	// return the number of allocated chunks
	VX_EXPORT int			GetChunksTotalSize();
	
	// return the number of allocated chunks
	VX_EXPORT int			GetChunksOccupation();

	template <class T>
	void		CallDtor(T* iDummy)
	{
		// we clear all the chunks
		for (Chunks::Iterator it = m_Chunks.Begin(); it != m_Chunks.End(); ++it) {
			it->CallDtor(iDummy,m_BlockSize,m_BlockCount);
		}
	}
	
	VX_EXPORT void		Clear();

	VX_EXPORT void*		Allocate();

	VX_EXPORT void		Free(void* iP);

private:
	class Chunk
	{
	public:
		Chunk() {}

		void	Init(size_t iBlockSize, unsigned int iBlockCount);
		
		template <class T>
		void		CallDtor(T* iDummy,size_t iBlockSize, unsigned int iBlockCount)
		{
			// everything is clear -> nothing to do
			if (m_BlockAvailables == iBlockCount) 
				return;
			
			// else we have some cleaning todo

			if (!m_BlockAvailables) { // we need to clean everything

				{ // we call the dtor of the used blocks
					for (unsigned int i=0; i<iBlockCount; ++i) {

						unsigned char* p = m_Data + i*iBlockSize;
						((T*)p)->~T();
					}
					
				}

			} else { // only some are used

				XBitArray freeBlocks;

				{ // we mark the objects used
					int freeb = m_FirstAvailableBlock;
					for (unsigned int i=0; i<m_BlockAvailables-1; ++i) {

						freeBlocks.Set(freeb);
						
						unsigned char* p = m_Data + freeb*iBlockSize;
						freeb = *(int*)p;
					}
					freeBlocks.Set(freeb);
				}

				{ // we call the dtor of the used blocks
					for (unsigned int i=0; i<iBlockCount; ++i) {

						if (freeBlocks.IsSet(i))
							continue;
						
						unsigned char* p = m_Data + i*iBlockSize;
						((T*)p)->~T();
					}
					
				}
			}

		}

		void	Destroy();
		
		void*	Allocate(size_t iBlockSize);
		
		void	Deallocate(void* iP, size_t iBlockSize);
		
		unsigned char*	m_Data;
		unsigned int	m_FirstAvailableBlock;
		unsigned int	m_BlockAvailables;
		unsigned int	m_BlockCount;
	};

	// types
	typedef XArray<Chunk>	Chunks;

	// function to find the chunk containing the ptr
	Chunk*			FindChunk(void* iP);

	// members
	size_t			m_PageSize;
	// Block size
	size_t			m_BlockSize;
	// Blocks Count (per Chunk)
	unsigned int	m_BlockCount;

	// the chunks
	Chunks			m_Chunks;

	// Allocating and deallocating chunks
	Chunk*			m_AChunk;
	Chunk*			m_DChunk;

};

// 
template <class T>
class XObjectPool
{
public:
	XObjectPool(XBOOL iCallDtor = TRUE):m_Allocator(sizeof(T)),m_CallDtor(iCallDtor) {}

	T*		Allocate()
	{
		return new (m_Allocator.Allocate()) T;
	}

	void	Free(T* iP)
	{
		if (m_CallDtor)
			iP->~T();

		m_Allocator.Free(iP);
	}

	void	Clear()
	{
		if (m_CallDtor)
			m_Allocator.CallDtor((T*)NULL);

		m_Allocator.Clear();
	}

private:
	XFixedSizeAllocator m_Allocator;
	XBOOL				m_CallDtor;
};

/*
#define	new		(m_setOwner  (__FILE__,__LINE__,__FUNCTION__),false) ? NULL : new
#define	delete		(m_setOwner  (__FILE__,__LINE__,__FUNCTION__),false) ? m_setOwner("",0,"") : delete
*/

#endif // FIXEDSIZEALLOCATOR_H