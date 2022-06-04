/*************************************************************************/
/*	File : XP.h															 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _XSMARTPTR_H_
#define _XSMARTPTR_H_

#include "XUtil.h"

#if !defined(_LINUX)
#pragma warning(disable : 4284)
#endif

/************************************************
Summary: a reference counting class. You must derive from this
class to use smart pointers.

Remarks:



See Also : XP,XSmartPtr
************************************************/
class XRefCount
{
public:
    // Reference counter
    mutable unsigned int m_RefCount;

    /// Destructor which release pinfo if necessary.
    ~XRefCount() {}
    /// Default constructor init XRefs to 0.
    XRefCount() : m_RefCount(0) {}
    /// operator= must NOT copy XRefs/pinfo!!
    XRefCount &operator=(const XRefCount &)
    {
        return *this;
    }
    /// copy cons must NOT copy XRefs/pinfo!!
    XRefCount(const XRefCount &) { m_RefCount = 0; }
};

/************************************************
Summary: Smart pointer class.

Remarks:
See Also : XP,XRefCount
************************************************/
template <class T>
class XSmartPtr
{
public:
    /************************************************
    Summary:
    ************************************************/
    XSmartPtr() : m_Pointee(NULL) {}

    /************************************************
    Summary:
    ************************************************/
    explicit XSmartPtr(T *p) : m_Pointee(p) { AddRef(); }

    /************************************************
    Summary: Releases the pointed object.
    ************************************************/
    ~XSmartPtr() { Release(); }

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XP.
    ************************************************/
    XSmartPtr(const XSmartPtr &a) : m_Pointee(a.m_Pointee) { AddRef(); }

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XP.
    ************************************************/
    XSmartPtr<T> &operator=(const XSmartPtr &a) { return operator=(a.m_Pointee); }

    XSmartPtr<T> &operator=(T *p)
    {
        if (p)
            ++p->m_RefCount;
        Release();
        m_Pointee = p;

        return *this;
    }

    /************************************************
    Summary: Direct access to the pointer.
    ************************************************/
    T *operator->() const { return m_Pointee; }

    /************************************************
    Summary: Direct access to the pointer.
    ************************************************/
    T &operator*() const { return *m_Pointee; }

    /************************************************
    Summary: Original pointer cast.
    ************************************************/
    operator T *() const { return m_Pointee; }

protected:
    void AddRef()
    {
        if (m_Pointee)
            ++m_Pointee->m_RefCount;
    }

    void Release()
    {
        if (m_Pointee && (--(m_Pointee->m_RefCount) == 0))
            delete m_Pointee;
    }

    ///
    // Members

    // The pointee object
    T *m_Pointee;
};

/************************************************
Summary: Strided pointers iterator class.

Remarks:
See Also : XP
************************************************/
template <class T>
class XPtrStrided
{
public:
    XPtrStrided() : m_Ptr(0), m_Stride(0) {}
    XPtrStrided(void *Ptr, int Stride) : m_Ptr((unsigned char *)Ptr), m_Stride(Stride) {}

    /************************************************
    Summary: Cast to the relevant type of pointer.
    ************************************************/
    operator T *() { return (T *)m_Ptr; }

    /************************************************
    Summary: Dereferencing operators.
    ************************************************/
    T &operator*() { return *(T *)m_Ptr; }
    const T &operator*() const { return *(T *)m_Ptr; }
    T *operator->() { return (T *)m_Ptr; }

    const T &operator[](unsigned short iCount) const { return *(T *)(m_Ptr + iCount * m_Stride); }
    T &operator[](unsigned short iCount) { return *(T *)(m_Ptr + iCount * m_Stride); }

    const T &operator[](int iCount) const { return *(T *)(m_Ptr + iCount * m_Stride); }
    T &operator[](int iCount) { return *(T *)(m_Ptr + iCount * m_Stride); }

    const T &operator[](unsigned int iCount) const { return *(T *)(m_Ptr + iCount * m_Stride); }
    T &operator[](unsigned int iCount) { return *(T *)(m_Ptr + iCount * m_Stride); }

    /************************************************
    Summary: Go to the next element.
    ************************************************/
    XPtrStrided &operator++()
    {
        m_Ptr += m_Stride;
        return *this;
    }
    XPtrStrided operator++(int)
    {
        XPtrStrided tmp = *this;
        m_Ptr += m_Stride;
        return tmp;
    }

    /************************************************
    Summary: Go to the n next element.
    ************************************************/
    XPtrStrided operator+(int n) { return XPtrStrided(m_Ptr + n * m_Stride, m_Stride); }
    XPtrStrided &operator+=(int n)
    {
        m_Ptr += n * m_Stride;
        return *this;
    }

private:
    unsigned char *m_Ptr;
    int m_Stride;
};

#endif
