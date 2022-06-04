/*************************************************************************/
/*	File : XArray.h														 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _XARRAY_H_
#define _XARRAY_H_

#include "XUtil.h"
#include <string.h>

/************************************************
{filename:XArray}
Name: XArray

Summary: Class representation of an array.

Remarks:
The array has a notion of reserved size, which doubles each time it is
reached. The reserved size reduces only if the occupation of the array goes
below 30% of the reserved size.
You mustn't store classes of variable size into an XArray. Use XClassArray for this.



See Also : XClassArray, XSArray
************************************************/
template <class T>
class XArray
{
public:
    // Types
    typedef T *Iterator;
    typedef T Type;
    typedef T &Reference;

    /************************************************
    Summary: Constructors.

    Input Arguments:
        ss: Default number of reserved elements.
        a: An array to copy from.

    ************************************************/
    XArray(int ss = 0)
    {
        // Allocated
        if (ss > 0)
        {
            m_Begin = Allocate(ss);
            m_End = m_Begin;
            m_AllocatedEnd = m_Begin + ss;
        }
        else
        {
            m_AllocatedEnd = 0;
            m_Begin = m_End = 0;
        }
    }

    // copy Ctor
    XArray(const XArray<T> &a)
    {
        // the resize
        int size = a.Size();
        m_Begin = Allocate(size);
        m_End = m_Begin + size;
        m_AllocatedEnd = m_End;
        // The copy
        XCopy(m_Begin, a.m_Begin, a.m_End);
    }

    /************************************************
    Summary: Destructor.

    Remarks:
        Release the elements contained in the array. If
    you were storing pointers, you need first to iterate
    on the array and call delete on each pointer.
    ************************************************/
    ~XArray()
    {
        Free();
    }

    /************************************************
    Summary: Affectation operator.

    Remarks:
        The content of the array is enterely overwritten
    by the given array.
    ************************************************/
    XArray<T> &operator=(const XArray<T> &a)
    {
        if (this != &a)
        {
            if (Allocated() >= a.Size())
            { // No need to allocate
                // The copy
                XCopy(m_Begin, a.m_Begin, a.m_End);
                m_End = m_Begin + a.Size();
            }
            else
            {
                Free();
                // the resize
                int size = a.Size();
                m_Begin = Allocate(size);
                m_End = m_Begin + size;
                m_AllocatedEnd = m_End;
                // The copy
                XCopy(m_Begin, a.m_Begin, a.m_End);
            }
        }

        return *this;
    }

    /************************************************
    Summary: Appends the content of another array.

    Remarks:
        The content of the array is conserved.
    ************************************************/
    XArray<T> &operator+=(const XArray<T> &a)
    {
        int size = a.Size();
        if (size)
        {
            int oldsize = Size();
            int newsize = oldsize + size + 1;
            if (newsize <= Allocated())
            { // The new array fits in
                // we recopy the new array
                XCopy(m_End, a.m_Begin, a.m_End);
                m_End += size;
            }
            else
            { // the new array dosn't fit in
                T *temp = Allocate(newsize);

                // we recopy the old array
                XCopy(temp, m_Begin, m_End);
                m_End = temp + oldsize;

                // we copy the given array
                XCopy(m_End, a.m_Begin, a.m_End);
                m_End += size;
                m_AllocatedEnd = m_End + 1;

                // we free the old memory
                Free();

                // we set the new pointer
                m_Begin = temp;
            }
        }

        return *this;
    }

    /************************************************
    Summary: Removes the elements of the given array
    from the array.

    Remarks:
        The content of the array is conserved.
    ************************************************/
    XArray<T> &operator-=(const XArray<T> &a)
    {
        int size = a.Size();
        if (size)
        {
            int oldsize = Size();
            if (oldsize)
            {
                T *newarray = Allocate(oldsize + 1);
                T *temp = newarray;

                for (T *t = m_Begin; t != m_End; ++t)
                {
                    // we search for the element in the other array
                    if (a.Find(*t) == a.m_End)
                    {
                        // the element is not in the other array, we copy it to the newone
                        *temp = *t;
                        ++temp;
                    }
                }

                Free();
                // we set the new pointers
                m_Begin = newarray;
                m_End = temp;
                m_AllocatedEnd = m_Begin + oldsize + 1;
            }
        }

        return *this;
    }

    /************************************************
    Summary: Removes all the elements from an array.

    Remarks:
        There is no more space reserved after this call.
    ************************************************/
    void Clear()
    {
        Free();
        m_Begin = 0;
        m_End = 0;
        m_AllocatedEnd = 0;
    }

    /************************************************
    Summary: Ensures than the reserved size equals the real size..
    ************************************************/
    void Compact()
    {
        if (m_AllocatedEnd > m_End)
        {
            int size = Size();
            if (!size)
                return;

            T *newdata = Allocate(size);

            // copy before insertion point
            XCopy(newdata, m_Begin, m_End);

            Free();
            m_Begin = newdata;
            m_AllocatedEnd = m_End = newdata + size;
        }
    }

    /************************************************
    Summary: Reserves n elements fot an array.

    Remarks:
        The elements beyond the reserved limit are
    discarded.
    ************************************************/
    void Reserve(int size)
    {
        // allocation of new size
        T *newdata = Allocate(size);

        // Recopy of old elements
        T *last = XMin(m_Begin + size, m_End);
        XCopy(newdata, m_Begin, last);

        // new Pointers
        Free();
        m_End = newdata + (last - m_Begin);
        m_Begin = newdata;
        m_AllocatedEnd = newdata + size;
    }

    /************************************************
    Summary: Resizes the array.
    Input Arguments:
        size: New size of the array.
    Remarks:
    After Resize(n) (n>0), you can address elements from [0] to [n-1].
    No constructors are called (use an XClassArray if it is  desired).
    If the size is greater than the reserved size, the array is reallocated at the exact needed size.
    If not, there is no reallocation at all. Resize(0) is faster than Clear() if you know you will probably
    push some more elements after.
    See Also:Reserve
    ************************************************/
    void Resize(int size)
    {
        XASSERT(size >= 0);
        // we check if the array has enough capacity
        int oldsize = (m_AllocatedEnd - m_Begin);
        // If not, we allocate extra data
        if (size > oldsize)
            Reserve(size);
        // if (size+1 > oldsize) Reserve(size+1); // +1 for research
        // We set the end cursor
        m_End = m_Begin + size;
    }

    /************************************************
    Summary: Expands an array of e elements.

    Input Arguments:
        e: size to expand.
    ************************************************/
    void Expand(int e = 1)
    {
        // we check if the array has enough capacity

        // If not, we allocate extra data
        while (Size() + e > Allocated())
        {
            Reserve(Allocated() ? Allocated() * 2 : 2);
        }
        // We set the end cursor
        m_End += e;
    }

    /************************************************
    Summary: Compress an array of e elements.

    Input Arguments:
        e: size to compress.
    ************************************************/
    void Compress(int e = 1)
    {
        if (m_Begin + e <= m_End)
        {
            // We set the end cursor
            m_End -= e;
        }
        else
        {
            m_End = m_Begin;
        }
    }

    /************************************************
    Summary: Inserts an element at the end of an array.

    Input Arguments:
        o: object to insert.
    ************************************************/
    void PushBack(const T &o)
    {
        if (m_End == m_AllocatedEnd)
        {
            Reserve(Size() ? Size() * 2 : 2);
        }

        *(m_End++) = o;
    }

    /************************************************
    Summary: Inserts an element at the start of an array.

    Input Arguments:
        o: object to insert.

    Remarks:
        This function can be slow if your array is
    well filled.
    ************************************************/
    void PushFront(const T &o)
    {
        XInsert(m_Begin, o);
    }

    /************************************************
    Summary: Inserts an element before another one.

    Input Arguments:
        i: iterator on the element to insert before.
        pos: position to insert the object
        o: object to insert.

    Remarks:
        The element to insert before is given as
    an iterator on it, i.e. a pointer on it in
    this case.
    ************************************************/
    void Insert(T *i, const T &o)
    {
        if (i < m_Begin || i > m_End)
            return;

        // The Call
        XInsert(i, o);
    }
    void Insert(int pos, const T &o)
    {
        Insert(m_Begin + pos, o);
    }

    /************************************************
    Summary: Moves the element n before the element i.

    Input Arguments:
        n: iterator on the element to move.
        i: iterator on the element to insert before.

    Remarks:
        The elements are given by iterators on them,
    i.e. pointer on them in this case.
    ************************************************/
    void Move(T *i, T *n)
    {
        if (i < m_Begin || i > m_End)
            return;
        if (n < m_Begin || n > m_End)
            return;

        // The Call
        int insertpos = i - m_Begin;
        if (n < i)
            --insertpos;

        T tn = *n;
        XRemove(n);
        Insert(insertpos, tn);
    }

    /************************************************
    Summary: Removes the last element of an array.

    Remarks:
        You must be sure that the array isn't empty.

    Return Value: object removed.
    ************************************************/
    T PopBack()
    {
        // If there is no last element, it will crash
        T t = *(m_End - 1);
        XRemove(m_End - 1);
        return t;
    }

    /************************************************
    Summary: Removes the first element of an array.
    ************************************************/
    void PopFront()
    {
        // we remove the first element only if it exists
        if (m_Begin != m_End)
            XRemove(m_Begin);
    }

    /************************************************
    Summary: Removes an element at a given position.

    Input Arguments:
        pos: position of the object to remove.
        old: value of the element removed.

    Return Value: an iterator on the next
    element after the element removed (to go on with
    an iteration).
    ************************************************/
    XBOOL RemoveAt(unsigned int pos, T &old)
    {
        T *t = m_Begin + pos;
        // we ensure i is in boundary...
        if (t >= m_End)
            return 0;
        old = *t; // we keep the old value
        // the removal
        XRemove(t);
        return TRUE; // really removed
    }

    BOOL EraseAt(int pos)
    {
        return (BOOL)Remove(m_Begin + pos);
    }

    T *RemoveAt(int pos)
    {
        return Remove(m_Begin + pos);
    }

    /************************************************
    Summary: Removes an element.

    Input Arguments:
        i: iterator on the element to remove.
        o: object to remove.

    Return Value: An iterator on the next
    element after the element removed (to go on with
    an iteration).

    Remarks:
        The elements are given by iterators on them,
    i.e. pointer on them in this case.
    ************************************************/
    T *Remove(T *i)
    {
        // we ensure i is in boundary...
        if (i < m_Begin || i >= m_End)
            return 0;

        // the Call
        return XRemove(i);
    }

    T *Remove(const T &o)
    {
        T *t = Find(o);
        // we ensure i is in boundary...
        if (t < m_Begin || t >= m_End)
            return 0;

        // the Call
        return XRemove(t);
    }

    BOOL Erase(const T &o)
    {
        T *t = Find(o);
        // we ensure i is in boundary...
        if (t < m_Begin || t >= m_End)
            return FALSE;

        // the Call
        return (BOOL)XRemove(t);
    }

    void FastRemove(const T &o)
    {
        FastRemove(Find(o));
    }

    void FastRemove(const Iterator &iT)
    {
        // we ensure i is in boundary...
        if (iT < m_Begin || iT >= m_End)
            return;

        m_End--;
        if (iT < m_End)
            *iT = *m_End;
    }

    /************************************************
    Summary: Fills an array with a value.

    Input Arguments:
        o: value to fill.
    ************************************************/
    void Fill(const T &o)
    {
        for (T *t = m_Begin; t != m_End; ++t)
            *t = o;
    }

    /************************************************
    Summary: Fills an array with a BYTE value.

    Input Arguments:
        val: byte value to fill.
    ************************************************/
    void Memset(XBYTE val)
    {
        memset(m_Begin, val, (m_End - m_Begin) * sizeof(T));
    }

    /************************************************
    Summary: Access to an array element.

    Input Arguments:
        i: index of the element to access.

    Return Value: a reference on the object accessed.

    Remarks:
        No test are provided on i.
    ************************************************/
    const T &operator[](int i) const
    {
        XASSERT(i >= 0 && i < Size());
        return *(m_Begin + i);
    }

    T &operator[](int i)
    {
        XASSERT(i >= 0 && i < Size());
        return *(m_Begin + i);
    }

    /************************************************
    Summary: Access to an array element.

    Input Arguments:
        i: index of the element to access.

    Return Value: a pointer on the object accessed.

    Remarks:
        End() is returned if i is outside the array
    limits.
    ************************************************/
    T *At(unsigned int i)
    {
        T *t = m_Begin + i;
        if ((t >= m_Begin) && (t < m_End))
            return t;
        else
            return m_End;
    }

    const T *At(unsigned int i) const
    {
        T *t = m_Begin + i;
        if ((t >= m_Begin) && (t < m_End))
            return t;
        else
            return m_End;
    }

    /************************************************
    Summary: Finds an element.

    Input Arguments:
        o: element to find.

    Return Value: a pointer on the first object found
    or End() if the object is not found.
    ************************************************/
    T *Find(const T &o) const
    {
        // FRom roro : To Check !!!!!!
        // If the array is empty
        // if(!(m_End - m_Begin)) return m_End;
        //		*m_End = o;
        //		T* t = m_Begin;
        //		while(*t != o) ++t;
        T *t = m_Begin;
        while (t < m_End && *t != o)
            ++t;

        // Some times faster : To Disassemble
        // T* t = m_Begin-1;
        // do ++t; while(*t != o);

        return t;
    }

    /************************************************
    Summary: Tests for an element presence.

    Input Arguments:
        o: element to test.

    Return Value: TRUE if the element is present.
    ************************************************/
    XBOOL IsHere(const T &o) const
    {
        T *t = m_Begin;
        while (t < m_End && *t != o)
            ++t;

        return (t != m_End);
    }

    /************************************************
    Summary: Returns the position of an element.

    Input Arguments:
        o: element to find.

    Return Value: position or -1 if not found.
    ************************************************/
    int GetPosition(const T &o) const
    {
        T *t = Find(o);
        // If the element is not found
        if (t == m_End)
            return -1;
        // else return the position
        return t - m_Begin;
    }

    /************************************************
    Summary: Swaps two items in array.

    Input Arguments:
        pos1: position of first item to swap
        pos2: position of second item to swap.
    ************************************************/
    void Swap(int pos1, int pos2)
    {
        char buffer[sizeof(T)];
        memcpy(buffer, m_Begin + pos1, sizeof(T));
        memcpy(m_Begin + pos1, m_Begin + pos2, sizeof(T));
        memcpy(m_Begin + pos2, buffer, sizeof(T));
    }

    /************************************************
    Summary: Swaps two arrays.

    Input Arguments:
        o: second array to swap.
    ************************************************/
    void Swap(XArray<T> &a)
    {
        XSwap(m_Begin, a.m_Begin);
        XSwap(m_End, a.m_End);
        XSwap(m_AllocatedEnd, a.m_AllocatedEnd);
    }

    /************************************************
    Summary: Returns the first element of an array.

    Remarks:
        No test are provided to see if there is an
    element.
    ************************************************/
    T &Front() { return *Begin(); }

    const T &Front() const { return *Begin(); }

    /************************************************
    Summary: Returns the last element of an array.

    Remarks:
        No test are provided to see if there is an
    element.
    ************************************************/
    T &Back() { return *(End() - 1); }

    const T &Back() const { return *(End() - 1); }

    /************************************************
    Summary: Returns an iterator on the first element.

    Example:
        Typically, an algorithm iterating on an array
    looks like:

            for(T* t = a.Begin(); t != a.End(); ++t) {
                // do something with *t
            }
    ************************************************/
    T *Begin() const { return m_Begin; }

    /************************************************
    Summary: Returns an iterator on the last element.
    ************************************************/
    T *RBegin() const { return m_End - 1; }

    /************************************************
    Summary: Returns an iterator after the last element.
    ************************************************/
    T *End() const { return m_End; }

    /************************************************
    Summary: Returns an iterator before the first element.
    ************************************************/
    T *REnd() const { return m_Begin - 1; }

    /************************************************
    Summary: Returns the elements number.
    ************************************************/
    int Size() const { return m_End - m_Begin; }

    /************************************************
    Summary: Returns whether the array is empty (it is faster to use
    IsEmpty than Size when testing for array size...)
    ************************************************/
    bool IsEmpty() const { return m_End == m_Begin; }

    /************************************************
    Summary: Returns the occupied size in memory in bytes

    Parameters:
        addstatic: TRUE if you want to add the size occupied
    by the class itself.
    ************************************************/
    int GetMemoryOccupation(XBOOL addstatic = FALSE) const { return Allocated() * sizeof(T) + (addstatic ? sizeof(*this) : 0); }

    /************************************************
    Summary: Returns the elements allocated.
    ************************************************/
    int Allocated() const { return m_AllocatedEnd - m_Begin; }

    static int XCompare(const void *elem1, const void *elem2)
    {
        return *(T *)elem1 - *(T *)elem2;
    }

    /************************************************
    Summary: Sorts an array with a quick sort.

    Input Arguments:
        compare: The function comparing two elements.

    Remarks:
        Two sorts algorithm are available : BubbleSort
    and (quick)Sort.
    ************************************************/
    void Sort(VxSortFunc compare = XCompare)
    {
        if (Size() > 1)
            qsort(m_Begin, Size(), sizeof(T), compare);
    }

    /************************************************
    Summary: Sorts an array with a bubble sort.

    Input Arguments:
        compare: The function comparing two elements.

    Remarks:
        Two sorts algorithm are available : BubbleSort
    and (quick)sort.
    ************************************************/
    void BubbleSort(T *rangestart, T *rangeend, VxSortFunc compare = XCompare)
    {
        if (!compare)
            return;
        if ((rangeend - rangestart) <= 1)
            return;

        XBOOL Noswap = TRUE;
        for (T *it1 = rangestart + 1; it1 < rangeend; it1++)
        {
            for (T *it2 = rangeend - 1; it2 >= it1; it2--)
            {
                T *t2 = it2 - 1;
                if (compare(it2, t2) < 0)
                {
                    XSwap(*it2, *t2);
                    Noswap = FALSE;
                }
            }
            if (Noswap)
                break;
            Noswap = TRUE;
        }
    }
    /************************************************
    Summary: Sorts an array with a bubble sort.

    Input Arguments:
        compare: The function comparing two elements.

    Remarks:
        Two sorts algorithm are available : BubbleSort
    and (quick)sort.
    ************************************************/
    void BubbleSort(VxSortFunc compare = XCompare)
    {
        if (!compare)
            return;
        if ((m_End - m_Begin) <= 1)
            return;

        XBOOL Noswap = TRUE;
        for (T *it1 = m_Begin + 1; it1 < m_End; it1++)
        {
            for (T *it2 = m_End - 1; it2 >= it1; it2--)
            {
                T *t2 = it2 - 1;
                if (compare(it2, t2) < 0)
                {
                    XSwap(*it2, *t2);
                    Noswap = FALSE;
                }
            }
            if (Noswap)
                break;
            Noswap = TRUE;
        }
    }

protected:
    ///
    // Methods

    // Copy {secret}
    void XCopy(T *dest, T *start, T *end)
    {
        int size = ((XBYTE *)end - (XBYTE *)start);
        if (size)
            memcpy(dest, start, size);
    }

    // Move {secret}
    void XMove(T *dest, T *start, T *end)
    {
        int size = ((XBYTE *)end - (XBYTE *)start);
        if (size)
            memmove(dest, start, size);
    }

    // Insert {secret}
    void XInsert(T *i, const T &o)
    {
        assert(i >= m_Begin);
        assert(i <= m_End);

        // Test For Reallocation
        if (m_End == m_AllocatedEnd)
        {
            int newsize = (m_AllocatedEnd - m_Begin) * 2; //+m_AllocationSize;
            if (!newsize)
                newsize = 2;
            T *newdata = Allocate(newsize);

            // copy before insertion point
            XCopy(newdata, m_Begin, i);

            // copy the new element
            T *insertionpoint = newdata + (i - m_Begin);
            *(insertionpoint) = o;

            // copy after insertion point
            XCopy(insertionpoint + 1, i, m_End);

            // New Pointers
            m_End = newdata + (m_End - m_Begin);
            Free();
            m_Begin = newdata;
            m_AllocatedEnd = newdata + newsize;
        }
        else
        {
            // copy after insertion point
            XMove(i + 1, i, m_End);
            // copy the new element
            *i = o;
        }
        m_End++;
    }

    // Remove {secret}
    T *XRemove(T *i)
    {
        // copy after insertion point
        XMove(i, i + 1, m_End);

        m_End--;
        return i;
    }

    ///
    // Allocation and deallocation methods : to be override for alignement purposes

    // Allocation {secret}
    T *Allocate(int size)
    {
        if (size)
            return (T *)new T[size];
        else
            return 0;
    }

    // Free {secret}
    void Free()
    {
        delete[] m_Begin;
    }

    ///
    // Members

    // elements start {secret}
    T *m_Begin;
    // elements end {secret}
    T *m_End;
    // reserved end {secret}
    T *m_AllocatedEnd;
};

#endif
