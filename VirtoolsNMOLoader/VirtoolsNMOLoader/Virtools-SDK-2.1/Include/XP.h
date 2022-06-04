/*************************************************************************/
/*	File : XP.h															 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _XP_H_
#define _XP_H_

#include "XUtil.h"

/************************************************
Name: XP

Summary: auto_ptr like class.

Remarks:
    XP objects will allow you to keep a reference
on pointers, without having to delete them
explicitely. The pointers will be deleted by
the XP object when leaving the its scope. This class
use a delete operator, so to handle pointers
allocated with new [], use XAP.



See Also : XAP
************************************************/
template <class T>
class XP
{
public:
    /************************************************
    Summary: Constructs an XP object.
    ************************************************/
    explicit XP(T *p) : m_Pointee(p) {}

    /************************************************
    Summary: Releases the pointed object.
    ************************************************/
    ~XP() { delete m_Pointee; }

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XP.
    ************************************************/
    XP(XP<T> &a) : m_Pointee(a.Release()) {}

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XP.
    ************************************************/
    XP<T> &operator=(XP<T> &a)
    {
        if (this != &a)
            Set(a.Release());
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

    /************************************************
    Summary: Release the pointer from sure deletion.
    ************************************************/
    T *Release()
    {
        T *oldp = m_Pointee;
        m_Pointee = 0;
        return oldp;
    }

    /************************************************
    Summary: Sets a new pointer to be handled, deleting
    the previously handled one.
    ************************************************/
    void Set(T *p = 0)
    {
        if (m_Pointee != p)
        {
            delete m_Pointee;
            m_Pointee = p;
        }
    }

protected:
    ///
    // Members

    // The pointee object
    T *m_Pointee;
};

/************************************************
Name: XAP

Summary: auto_ptr like class.

Remarks:
    XAP objects will allow you to keep a reference
on pointers, without having to delete them
explicitely. The pointers will be deleted by
the XP object when leaving the its scope. This class
use a delete [] operator, so to handle pointers
allocated with new , use XP.



See Also : XP
************************************************/
template <class T>
class XAP
{
public:
    /************************************************
    Summary: Constructs an XAP object.
    ************************************************/
    explicit XAP(T *p = NULL) : m_Pointee(p) {}

    /************************************************
    Summary: Releases the pointed object.
    ************************************************/
    ~XAP() { delete[] m_Pointee; }

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XAP.
    ************************************************/
    XAP(XAP<T> &a) : m_Pointee(a.Release()) {}

    /************************************************
    Summary: Copy the pointer and release it the burden
    from the source XAP.
    ************************************************/
    XAP<T> &operator=(XAP<T> &a)
    {
        if (this != &a)
            Set(a.Release());
        return *this;
    }

    /************************************************
    Summary: Direct access to the pointer.
    ************************************************/
    T &operator*() const { return *m_Pointee; }

    /************************************************
    Summary: Original pointer cast.
    ************************************************/
    operator T *() const { return m_Pointee; }

    /************************************************
    Summary: Release the pointer from sure deletion.
    ************************************************/
    T *Release()
    {
        T *oldp = m_Pointee;
        m_Pointee = 0;
        return oldp;
    }

    /************************************************
    Summary: Sets a new pointer to be handled, deleting
    the previously handled one.
    ************************************************/
    void Set(T *p = 0)
    {
        if (m_Pointee != p)
        {
            delete[] m_Pointee;
            m_Pointee = p;
        }
    }

protected:
    ///
    // Members

    // The pointee object
    T *m_Pointee;
};
#endif
