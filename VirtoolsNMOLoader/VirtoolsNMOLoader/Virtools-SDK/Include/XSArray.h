/*************************************************************************/
/*	File : XSArray.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef _XSARRAY_H_
#define _XSARRAY_H_

#include "XUtil.h"

#ifdef DOCJET_DUMMY
#else

#ifdef PSP
	#define USE_OPT_ARRAY 0
#else
	#define USE_OPT_ARRAY 0
#endif

#endif


#if USE_OPT_ARRAY
	#include "XSArrayOpt.h"
#else

/************************************************
Name: XSArray

Summary: Class representation of an array.

Remarks:
	This array behaves exactly like the XArray,
exept that its size is exactly the number it contains,
so it is more slow for adding and removing elements,
but it occupies less memory.



See Also : XClassArray, XArray
************************************************/
template <class T>
class XSArray
{
public:
	
	XSArray()
	{
		// Init
		m_Begin = m_End = 0;
	}

	
	XSArray(const XSArray<T>& a)
	{
		// the resize
		int size = a.Size();
		m_Begin = Allocate(size);
		m_End = m_Begin+size;
		// The copy
		XCopy(m_Begin,a.m_Begin,a.m_End);
	}

	
	~XSArray()
	{
		Clear();
	}

	
	XSArray<T>& operator = (const XSArray<T>& a)
	{
		if(this != &a) {
			Free();
			// the resize
			int size = a.Size();
			m_Begin = Allocate(size);
			m_End = m_Begin+size;
			// The copy
			XCopy(m_Begin,a.m_Begin,a.m_End);
		}

		return *this;
	}

	
	XSArray<T>& operator += (const XSArray<T>& a)
	{
		int size = a.Size();
		if (size) {
			int oldsize = Size();
			T* temp = Allocate(oldsize+size);
			
			// we recopy the old array
			XCopy(temp,m_Begin,m_End);
			m_End = temp+oldsize;

			// we copy the given array
			XCopy(m_End,a.m_Begin,a.m_End);
			m_End += size;

			// we free the old memory
			Free();

			// we set the new pointer
			m_Begin = temp;
		}

		return *this;
	}

	
	XSArray<T>& operator -= (const XSArray<T>& a)
	{
		int size = a.Size();
		if (size) {
			int oldsize = Size();
			if (oldsize) {
				T* newarray = Allocate(oldsize);
				T* temp = newarray;

				for(T* t = m_Begin; t != m_End; ++t) {
					// we search for the element in the other array
					if(a.Find(*t) == a.m_End) {
						// the element is not in the other array, we copy it to the newone
						*temp = *t;
						++temp;
					}
				}
				
				// we free the memory
				Free();
				// the resize
				int size = temp - newarray;
				m_Begin = Allocate(size);
				m_End = m_Begin+size;
				// The copy
				XCopy(m_Begin,newarray,temp);
			}
		}

		return *this;
	}

	
	void Clear()
	{
		Free();
		m_Begin = 0;
		m_End = 0;
	}

	
	void Fill(const T& o)
	{
		for(T* t = m_Begin; t != m_End; ++t)
				*t=o;
	}

	
	void Resize(int size)
	{
		// No change ?
		if (size == Size()) return;

		// allocation of new size
		T* newdata = Allocate(size);
		
		// Recopy of old elements
		T* last = XMin(m_Begin+size,m_End);
		XCopy(newdata,m_Begin,last);

		// new Pointers
		Free();
		m_Begin = newdata;
		m_End = newdata+size;
	}

	
	void PushBack(const T& o)
	{
		XInsert(m_End,o);
	}

	
	void PushFront(const T& o)
	{
		XInsert(m_Begin,o);
	}

	
	void Insert(T* i, const T& o)
	{
		// TODO : s'assurer que i est dans les limites...
		if(i<m_Begin || i>m_End) return;

		// The Call
		XInsert(i,o);
	}

	
	void Insert(int pos, const T& o)
	{
		Insert(m_Begin+pos,o);
	}

	
	void Move(T* i, T* n)
	{
		// TODO : s'assurer que i est dans les limites...
		if(i<m_Begin || i>m_End) return;
		if(n<m_Begin || n>m_End) return;

		// The Call
		int insertpos = i - m_Begin;
		if (n < i) --insertpos;

		T tn = *n;
		XRemove(n);
		Insert(insertpos,tn);
	}

	
	void PopBack()
	{
		// we remove the last element only if it exists
		if(m_End > m_Begin)	XRemove(m_End-1);
	}

	
	void PopFront()
	{
		// we remove the first element only if it exists
		if(m_Begin != m_End) XRemove(m_Begin);
	}

	
	T* Remove(T* i)
	{
		// we ensure i is in boundary...
		if(i<m_Begin || i>=m_End) return 0;

		// the Call
		return XRemove(i);
	}

	
	XBOOL RemoveAt(unsigned int pos,T& old)
	{
		T* t = m_Begin+pos;
		// we ensure i is in boundary...
		if(t >= m_End) return 0;
		old = *t; // we keep the old value
		// the removal
		XRemove(t);
		return TRUE; // really removed
	}

	
	T* RemoveAt(int pos)
	{
		// we ensure i is in boundary...
		if((pos < 0) || (m_Begin+pos >= m_End)) return 0;

		// the Call
		return XRemove(m_Begin+pos);
	}
 
	
	int Remove(const T& o)
	{
		for(T* t = m_Begin; t != m_End; ++t) {
			if(*t == o) {
				XRemove(t);
				return 1;
			}
		}
		return 0;
	}

	
	T& operator [](unsigned int i) const
	{
		return *(m_Begin+i);
	}

	
	T* At(unsigned int i)  const
	{
		if (i >= (unsigned int) Size()) return m_End;
		return m_Begin+i;
	}
 
	
	T* Find(const T& o)  const
	{
		for(T* t = m_Begin; t != m_End; ++t) {
			if(*t == o) return t;
		}
		return m_End;
	}

	
	XBOOL IsHere(const T& o) const
	{
		T* t = m_Begin;
		while(t < m_End && *t != o) ++t;					
	
		return (t != m_End);
	}

	
	int GetPosition(const T& o) const
	{
		for(T* t = m_Begin; t != m_End; ++t) {
			if(*t == o) return (t-m_Begin);
		}
		return -1;
	}

	/************************************************
	Summary: Swaps two items in array.

	Input Arguments: 
		pos1: position of first item to swap
		pos2: position of second item to swap.
	************************************************/
	void Swap(int pos1,int pos2)
	{
		char buffer[sizeof(T)];
		memcpy(buffer,m_Begin+pos1,sizeof(T));
		memcpy(m_Begin+pos1,m_Begin+pos2,sizeof(T));
		memcpy(m_Begin+pos2,buffer,sizeof(T));
	}

	
	void Swap(XSArray<T>& a)
	{
		XSwap(m_Begin,a.m_Begin);
		XSwap(m_End,a.m_End);
	}


	static
	int XCompare(const void *elem1, const void *elem2 )
	{
		return *(T*)elem1 < *(T*)elem2;
	}

	
	//			compare func should return :
	//			< 0 elem1 less than elem2 
	//			0 elem1 equivalent to elem2 
	//			> 0 elem1 greater than elem2 
	//			elem1 & elem2 T*
	void Sort( VxSortFunc compare = XCompare )
	{
		if (Size() > 1) qsort(m_Begin,Size(),sizeof(T),compare);
	}

	
	void BubbleSort( VxSortFunc compare = XCompare)
	{
		if	(!compare) return;
		if  ((m_End-m_Begin)<=1) return;

		
		BOOL Noswap=TRUE;
		for (T* it1=m_Begin+1;it1<m_End;it1++)	{
			for (T* it2=m_End-1;it2>=it1;it2--)	 {
				T* t2= it2-1;
				if (compare(it2,t2) < 0)  { 
					XSwap(*it2,*t2);
					Noswap=FALSE;
				}
			}
			if (Noswap) break;
			Noswap=TRUE;
		}
	}

	
	T* Begin() const {return m_Begin;}

	
	T* End()  const {return m_End;}

	
	int Size() const {return m_End-m_Begin;}

	
	int GetMemoryOccupation(XBOOL addstatic=FALSE) const {return Size()*sizeof(T)+addstatic?sizeof(*this):0;}

protected:
	///
	// Methods

	
	void XCopy(T* dest, T* start, T* end)
	{
		int size = ((XBYTE*)end - (XBYTE*)start);
		if(size)
			memcpy(dest,start,size);
	}
	
	
	void XMove(T* dest, T* start, T* end)
	{
		int size = ((XBYTE*)end - (XBYTE*)start);
		if(size)
			memmove(dest,start,size);
	}

	
	void XInsert(T* i, const T& o)
	{
		// Reallocation
		int newsize = (m_End-m_Begin)+1;
		T* newdata = Allocate(newsize);
		
		// copy before insertion point
		XCopy(newdata,m_Begin,i);
		
		// copy the new element
		T* insertionpoint = newdata+(i-m_Begin);
		*(insertionpoint) = o;
		
		// copy after insertion point
		XCopy(insertionpoint+1,i,m_End);
		
		// New Pointers
		m_End = newdata+newsize;
		Free();
		m_Begin = newdata;
	}

	
	T* XRemove(T* i)
	{
		// Reallocation
		int newsize = (m_End-m_Begin)-1;
		T* newdata = Allocate(newsize);
		
		// copy before insertion point
		XCopy(newdata,m_Begin,i);
		
		// copy after insertion point
		T* deletionpoint = newdata+(i-m_Begin);
		XCopy(deletionpoint,i+1,m_End);
		i = deletionpoint;
		
		// New Pointers
		m_End = newdata+newsize;
		Free();
		m_Begin = newdata;

		return i;
	}

	///
	// Allocation and deallocation methods : to be override for alignement purposes

	
	T* Allocate(int size)
	{
		if(size) return (T*)VxNew(sizeof(T) * size);
		else return 0;
	}

	
	void Free()
	{
		VxDelete(m_Begin);
	}



	///
	// Members

	
	T*			m_Begin;
	
	T*			m_End;
};
#endif // USE_OPT_ARRAY
#endif
