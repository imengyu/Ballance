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

#ifdef _MSC_VER
	#pragma warning (disable:4284)
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
	mutable	unsigned int	m_RefCount;	
	
	/// Destructor which release pinfo if necessary.
	~XRefCount() {}
	/// Default constructor init XRefs to 0.
    XRefCount():m_RefCount(0) {}
	/// operator= must NOT copy XRefs/pinfo!!
	XRefCount &operator=(const XRefCount &) {
		return *this;
	}
	/// copy cons must NOT copy XRefs/pinfo!!
	XRefCount(const XRefCount &) {m_RefCount = 0;}
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
	XSmartPtr() : m_Pointee(NULL)	{}

	/************************************************
	Summary: 
	************************************************/
	explicit XSmartPtr(T* p) : m_Pointee(p)	{AddRef();}

	/************************************************
	Summary: Releases the pointed object.
	************************************************/
	~XSmartPtr() {Release();}

	/************************************************
	Summary: Copy the pointer and release it the burden 
	from the source XP.
	************************************************/
	XSmartPtr(const XSmartPtr& a) : m_Pointee(a.m_Pointee) {AddRef();}

	/************************************************
	Summary: Copy the pointer and release it the burden 
	from the source XP.
	************************************************/
	XSmartPtr<T>& operator = (const XSmartPtr& a) {return operator=(a.m_Pointee);}

	XSmartPtr<T>& operator = (T* p) {
		if (p) ++p->m_RefCount;
		Release();
		m_Pointee = p;
	
		return *this;
	}

	/************************************************
	Summary: Direct access to the pointer.
	************************************************/
	T* operator-> () const {return m_Pointee;}

	/************************************************
	Summary: Direct access to the pointer.
	************************************************/
	T& operator* () const {return *m_Pointee;}

	/************************************************
	Summary: Original pointer cast.
	************************************************/
	operator T* () const {return m_Pointee;}

protected:

	
	void AddRef() {if (m_Pointee) ++m_Pointee->m_RefCount;}
	
	void Release() {if (m_Pointee && (--(m_Pointee->m_RefCount) == 0)) delete m_Pointee;}

	///
	// Members

	// The pointee object
	T*			m_Pointee;
};

/************************************************
Summary: Strided pointers iterator class.

Remarks:
See Also : XP
************************************************/
template <class T>
class XPtrStrided : public VxStridedData {
public:
	XPtrStrided() {}
	
	XPtrStrided(void* Ptr, unsigned int Stride):VxStridedData(Ptr,Stride) {}
	
	template <class U>
	XPtrStrided(const XPtrStrided<U>& copy) {
		Ptr		= copy.Ptr;
		Stride	= copy.Stride;
	}

	void Set(void* iPtr, unsigned int iStride) { Ptr = iPtr; Stride = iStride; }

	template <class U>
	XPtrStrided& operator = (const XPtrStrided<U>& copy) {
		Ptr		= copy.Ptr;
		Stride	= copy.Stride;
	}

	/************************************************
	Summary: Cast to the relevant type of pointer.
	************************************************/
	operator T* () {return (T*)Ptr;}

	/************************************************
	Summary: Dereferencing operators.
	************************************************/
	T& operator * () {return *(T*)Ptr;}
	const T& operator * () const {return *(T*)Ptr;}
	T* operator -> () {return (T*)Ptr;}

	const T& operator [] (unsigned short iCount) const  {return *(T*)(CPtr+iCount*Stride);}
	T& operator [] (unsigned short iCount) {return *(T*)(CPtr+iCount*Stride);}

	const T& operator [] (int iCount) const  {return *(T*)(CPtr+iCount*Stride);}
	T& operator [] (int iCount) {return *(T*)(CPtr+iCount*Stride);}

	const T& operator [] (unsigned int iCount) const  {return *(T*)(CPtr+iCount*Stride);}
	T& operator [] (unsigned int iCount) {return *(T*)(CPtr+iCount*Stride);}

	/************************************************
	Summary: Go to the next element.
	************************************************/
	XPtrStrided& operator ++() {CPtr += Stride;return *this;}
	XPtrStrided operator ++(int) {XPtrStrided tmp = *this; CPtr += Stride;return tmp;}

	/************************************************
	Summary: Go to the n next element.
	************************************************/
	XPtrStrided operator +(int n) {return XPtrStrided( CPtr+n*Stride,Stride);}
	XPtrStrided& operator +=(int n) { CPtr += n*Stride;return *this;}
};

#endif
