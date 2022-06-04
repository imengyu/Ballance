#ifndef CKMEMORYPOOL_H
#define CKMEMORYPOOL_H "$Id:$"

#include <CKContext.h>

class CKMemoryPool
{
public:
    CKMemoryPool(CKContext *Context, size_t ByteCount = 0);
    ~CKMemoryPool();

    CKObject &operator=(const CKObject &);

    VxMemoryPool *Mem() const;

protected:
    CKContext *m_Context;
    VxMemoryPool *m_Memory;
    int m_Index;
};

#endif