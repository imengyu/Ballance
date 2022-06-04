#ifndef VXMEMORYPOOL_H
#define VXMEMORYPOOL_H

#include "XUtil.h"

//---- Aligned memory allocation
VX_EXPORT void *VxNewAligned(int size, int align);
VX_EXPORT void VxDeleteAligned(void *ptr);

/****************************************************************
Summary: Memory buffer class.

Remarks:
    o This class is designed to be used as a local or global variable that
    handles a memory buffer that will be automatically deleted on destruction.
    o The memory buffer is aligned on a 16 bytes boundary (64 on a PS2 system)
    o Do not call delete on the buffer returned by Buffer() method.
See Also: VxNewAligned,VxDeleteAligned
****************************************************************/
class VxMemoryPool
{
public:
    ~VxMemoryPool()
    {
        VxDeleteAligned(m_Memory);
        m_Memory = NULL;
    }
    // Constructs the class allocating the buffer to ByteCount bytes.
    VxMemoryPool(size_t ByteCount = 0)
    {
        m_Memory = NULL;
        Allocated = 0;
        Allocate(ByteCount);
    }

    // Returns access to the memory buffer.
    void *Buffer() const
    {
        return m_Memory;
    }

    // Returns allocated size of this memory pool.
    size_t AllocatedSize() const
    {
        return Allocated;
    }

    // Allocates the number of bytes if not yet allocated.
    void Allocate(size_t ByteCount)
    {
        if (Allocated < ByteCount)
        {
            VxDeleteAligned(m_Memory);

            m_Memory = (DWORD *)VxNewAligned(ByteCount, 16);
            Allocated = ByteCount;
        }
    }

protected:
    DWORD *m_Memory;

    size_t Allocated;
};

#endif