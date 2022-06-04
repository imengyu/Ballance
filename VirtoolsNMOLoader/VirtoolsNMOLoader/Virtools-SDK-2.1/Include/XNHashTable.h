/*************************************************************************/
/*	File : XNHashTable.h												 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _XNHashTable_H_
#define _XNHashTable_H_

#include "XArray.h"
#include "XHashFun.h"
#ifdef _WIN32
#pragma warning(disable : 4786)
#endif

template <class T, class K, class H, class Eq>
class XNHashTable;

template <class T, class K>
class XNHashTableEntry
{
    typedef XNHashTableEntry<T, K> *tEntry;

public:
    XNHashTableEntry(const K &k, const T &v) : m_Key(k), m_Data(v), m_Next(0) {}
    XNHashTableEntry(const XNHashTableEntry<T, K> &e) : m_Key(e.m_Key), m_Data(e.m_Data), m_Next(0) {}
    ~XNHashTableEntry() {}

    K m_Key;
    T m_Data;
    tEntry m_Next;
};

/************************************************
Summary: Iterator on a hash table.

Remarks: This iterator is the only way to iterate on
elements in a hash table. The iteration will be in no
specific order, not in the insertion order. Here is an example
of how to use it:

Example:

    XNHashTableIt<T,K,H> it = hashtable.Begin();
    while (it != hashtable.End()) {
        // access to the key
        it.GetKey();

        // access to the element
        *it;

        // next element
        ++it;
    }


************************************************/
template <class T, class K, class H = XHashFun<K>, class Eq = XEqual<K> >
class XNHashTableIt
{
    typedef XNHashTableEntry<T, K> *tEntry;
    typedef XNHashTableIt<T, K, H, Eq> tIterator;
    typedef XNHashTable<T, K, H, Eq> *tTable;
    friend class XNHashTable<T, K, H, Eq>;

public:
    /************************************************
    Summary: Default constructor of the iterator.
    ************************************************/
    XNHashTableIt() : m_Node(0), m_Table(0) {}

    /************************************************
    Summary: Copy constructor of the iterator.
    ************************************************/
    XNHashTableIt(const tIterator &n) : m_Node(n.m_Node), m_Table(n.m_Table) {}

    /************************************************
    Summary: Operator Equal of the iterator.
    ************************************************/
    int operator==(const tIterator &it) const { return m_Node == it.m_Node; }

    /************************************************
    Summary: Operator Not Equal of the iterator.
    ************************************************/
    int operator!=(const tIterator &it) const { return m_Node != it.m_Node; }

    /************************************************
    Summary: Returns a constant reference on the data
    pointed	by the iterator.

    Remarks:
        The returned reference is constant, so you can't
    modify its value. Use the other * operator for this
    purpose.
    ************************************************/
    const T &operator*() const { return (*m_Node).m_Data; }

    /************************************************
    Summary: Returns a reference on the data pointed
    by the iterator.

    Remarks:
        The returned reference is not constant, so you
    can modify its value.
    ************************************************/
    T &operator*() { return (*m_Node).m_Data; }

    /************************************************
    Summary: Returns a pointer on a T object.
    ************************************************/
    operator const T *() const { return &(m_Node->m_Data); }

    /************************************************
    Summary: Returns a pointer on a T object.
    ************************************************/
    operator T *() { return &(m_Node->m_Data); }

    /************************************************
    Summary: Returns a const reference on the key of
    the pointed entry.
    ************************************************/
    const K &GetKey() const { return m_Node->m_Key; }

    /************************************************
    Summary: Jumps to next entry in the hashtable.
    ************************************************/
    tIterator &operator++()
    { // Prefixe
        tEntry old = m_Node;
        // next element of the linked list
        m_Node = m_Node->m_Next;

        if (!m_Node)
        {
            // end of linked list, we have to find next filled bucket
            // OPTIM : maybe keep the index current : save a %
            int index = m_Table->Index(old->m_Key);
            while (!m_Node && ++index < m_Table->m_Table.Size())
                m_Node = m_Table->m_Table[index];
        }
        return *this;
    }

    /************************************************
    Summary: Jumps to next entry in the hashtable.
    ************************************************/
    tIterator operator++(int)
    {
        tIterator tmp = *this;
        ++*this;
        return tmp;
    }

    XNHashTableIt(tEntry n, tTable t) : m_Node(n), m_Table(t) {}

    tEntry m_Node;

    tTable m_Table;
};

/************************************************
Summary: Constant Iterator on a hash table.

Remarks: This iterator is the only way to iterate on
elements in a constant hash table. The iteration will be in no
specific order, not in the insertion order. Here is an example
of how to use it:

Example:

    void MyClass::MyMethod() const
    {
        XNHashTableConstIt<T,K,H> it = m_Hashtable.Begin();
        while (it != m_Hashtable.End()) {
            // access to the key
            it.GetKey();

            // access to the element
            *it;

            // next element
            ++it;
        }
    }


************************************************/
template <class T, class K, class H = XHashFun<K>, class Eq = XEqual<K> >
class XNHashTableConstIt
{
    typedef XNHashTableEntry<T, K> *tEntry;
    typedef XNHashTableConstIt<T, K, H, Eq> tConstIterator;
    typedef XNHashTable<T, K, H, Eq> const *tConstTable;
    friend class XNHashTable<T, K, H, Eq>;

public:
    /************************************************
    Summary: Default constructor of the iterator.
    ************************************************/
    XNHashTableConstIt() : m_Node(0), m_Table(0) {}

    /************************************************
    Summary: Copy constructor of the iterator.
    ************************************************/
    XNHashTableConstIt(const tConstIterator &n) : m_Node(n.m_Node), m_Table(n.m_Table) {}

    /************************************************
    Summary: Operator Equal of the iterator.
    ************************************************/
    int operator==(const tConstIterator &it) const { return m_Node == it.m_Node; }

    /************************************************
    Summary: Operator Not Equal of the iterator.
    ************************************************/
    int operator!=(const tConstIterator &it) const { return m_Node != it.m_Node; }

    /************************************************
    Summary: Returns a constant reference on the data
    pointed	by the iterator.

    Remarks:
        The returned reference is constant, so you can't
    modify its value. Use the other * operator for this
    purpose.
    ************************************************/
    const T &operator*() const { return (*m_Node).m_Data; }

    /************************************************
    Summary: Returns a pointer on a T object.
    ************************************************/
    operator const T *() const { return &(m_Node->m_Data); }

    /************************************************
    Summary: Returns a const reference on the key of
    the pointed entry.
    ************************************************/
    const K &GetKey() const { return m_Node->m_Key; }

    /************************************************
    Summary: Jumps to next entry in the hashtable.
    ************************************************/
    tConstIterator &operator++()
    { // Prefixe
        tEntry old = m_Node;
        // next element of the linked list
        m_Node = m_Node->m_Next;

        if (!m_Node)
        {
            // end of linked list, we have to find next filled bucket
            // OPTIM : maybe keep the index current : save a %
            int index = m_Table->Index(old->m_Key);
            while (!m_Node && ++index < m_Table->m_Table.Size())
                m_Node = m_Table->m_Table[index];
        }
        return *this;
    }

    /************************************************
    Summary: Jumps to next entry in the hashtable.
    ************************************************/
    tConstIterator operator++(int)
    {
        tConstIterator tmp = *this;
        ++*this;
        return tmp;
    }

    XNHashTableConstIt(tEntry n, tConstTable t) : m_Node(n), m_Table(t) {}

    tEntry m_Node;

    tConstTable m_Table;
};

/************************************************
Summary: Struct containing an iterator on an object
inserted and a BOOL determining if it were really
inserted (TRUE) or already there (FALSE).


************************************************/
template <class T, class K, class H = XHashFun<K>, class Eq = XEqual<K> >
class XNHashTablePair
{
public:
    XNHashTablePair(XNHashTableIt<T, K, H, Eq> it, int n) : m_Iterator(it), m_New(n){};

    XNHashTableIt<T, K, H, Eq> m_Iterator;
    XBOOL m_New;
};

/************************************************
Summary: Class representation of an Hash Table
container.

Remarks:
    T is the type of element to insert
    K is the type of the key
    H is the hash function to hash the key

    Several hash functions for basic types are
already defined in XHashFun.h

    This implementation of the hash table uses
Linked List in each bucket for element hashed to
the same index, so there are memory allocation
for each insertion. For a static implementation
without dynamic allocation, look at XSHashTable.

    There is a m_LoadFactor member which allow the
user to decide at which occupation the hash table
must be extended and rehashed.


************************************************/
template <class T, class K, class H = XHashFun<K>, class Eq = XEqual<K> >
class XNHashTable
{
    // Types
    typedef XNHashTable<T, K, H, Eq> tTable;
    typedef XNHashTableEntry<T, K> *tEntry;
    typedef XNHashTableIt<T, K, H, Eq> tIterator;
    typedef XNHashTableConstIt<T, K, H, Eq> tConstIterator;
    typedef XNHashTablePair<T, K, H, Eq> tPair;
    // Friendship
    friend class XNHashTableIt<T, K, H, Eq>;
    // Friendship
    friend class XNHashTableConstIt<T, K, H, Eq>;

public:
    typedef XNHashTablePair<T, K, H, Eq> Pair;
    typedef XNHashTableIt<T, K, H, Eq> Iterator;
    typedef XNHashTableConstIt<T, K, H, Eq> ConstIterator;

    /************************************************
    Summary: Default Constructor.

    Input Arguments:
        initialsize: The default number of buckets
        (should be a power of 2, otherwise will be
        converted.)
        l: Load Factor (see Class Description).
        a: hash table to copy.

    ************************************************/
    XNHashTable(int initialsize = 16, float l = 0.75f)
    {
        int dec = -1;
        while (initialsize)
        {
            initialsize >>= 1;
            dec++;
        }
        if (dec > -1)
            initialsize = 1 << dec;
        else
            initialsize = 1; // No Zero size allowed

        m_Table.Resize(initialsize);
        m_Table.Memset(0);

        if (l <= 0.0)
            l = 0.75f;

        m_LoadFactor = l;
        m_Count = 0;
        m_Threshold = (int)(m_Table.Size() * m_LoadFactor);
    }

    /************************************************
    Summary: Copy Constructor.
    ************************************************/
    XNHashTable(const XNHashTable &a)
    {
        XCopy(a);
    }

    /************************************************
    Summary: Destructor.

    Remarks:
        Release the elements contained in the hash table. If
    you were storing pointers, you need first to iterate
    on the table and call delete on each pointer.
    ************************************************/
    ~XNHashTable()
    {
        Clear();
    }

    /************************************************
    Summary: Removes all the elements from the table.

    Remarks:
        The hash table remains with the same number
    of buckets after a clear.
    ************************************************/
    void Clear()
    {
        for (tEntry *it = m_Table.Begin(); it != m_Table.End(); it++)
        {
            // we destroy the linked list
            if (*it)
            {
                XDeleteList(*it);
                *it = NULL;
            }
        }
        m_Count = 0;
    }

    /************************************************
    Summary: Affectation operator.

    Remarks:
        The content of the table is enterely overwritten
    by the given table.
    ************************************************/
    tTable &operator=(const tTable &a)
    {
        if (this != &a)
        {
            // We clear the current table
            Clear();
            // we then copy the content of a
            XCopy(a);
        }

        return *this;
    }

    /************************************************
    Summary: Inserts an element in the table.

    Input Arguments:
        key: key of the element to insert.
        o: element to insert.
        override: if the key is already present, should
    the old element be overriden ?

    Remarks:
        Insert will automatically override the old value
    and InsertUnique will not replace the old value.
    TestInsert returns a XHashPair, which allow you to know
    if the element was already present.
    ************************************************/
    XBOOL Insert(const K &key, const T &o, XBOOL override)
    {
        int index = Index(key);

        // we look for existing key
        tEntry e = XFind(index, key);
        if (!e)
        {
            if (m_Count >= m_Threshold)
            { // Need Rehash
                Rehash(m_Table.Size() * 2);
                return Insert(key, o, override);
            }
            else
            { // No
                XInsert(index, key, o);
            }
        }
        else
        {
            if (!override)
                return FALSE;
            e->m_Data = o;
        }

        return TRUE;
    }

    tIterator Insert(const K &key, const T &o)
    {
        int index = Index(key);
        Eq equalFunc;

        // we look for existing key
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (equalFunc(e->m_Key, key))
            {
                e->m_Data = o;
                return tIterator(e, this);
            }
        }

        if (m_Count >= m_Threshold)
        { // Need Rehash
            Rehash(m_Table.Size() * 2);
            return Insert(key, o);
        }
        else
        { // No
            return tIterator(XInsert(index, key, o), this);
        }
    }

    tPair TestInsert(const K &key, const T &o)
    {
        int index = Index(key);
        Eq equalFunc;

        // we look for existing key
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (equalFunc(e->m_Key, key))
            {
                return tPair(tIterator(e, this), 0);
            }
        }

        // Need Rehash
        if (m_Count >= m_Threshold)
        { // Yes
            Rehash(m_Table.Size() * 2);
            return TestInsert(key, o);
        }
        else
        { // No
            return tPair(tIterator(XInsert(index, key, o), this), 1);
        }
    }

    tIterator InsertUnique(const K &key, const T &o)
    {
        int index = Index(key);
        Eq equalFunc;

        // we look for existing key
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (equalFunc(e->m_Key, key))
            {
                return tIterator(e, this);
            }
        }

        // Need Rehash
        if (m_Count >= m_Threshold)
        { // Yes
            Rehash(m_Table.Size() * 2);
            return InsertUnique(key, o);
        }
        else
        { // No
            tEntry newe = new XNHashTableEntry<T, K>(key, o);
            newe->m_Next = m_Table[index];
            m_Table[index] = newe;
            m_Count++;
            return tIterator(newe, this);
        }
    }

    /************************************************
    Summary: Removes an element.

    Input Arguments:
        key: key of the element to remove.
        it: iterator on the object to remove.

    Return Value: iterator on the lement next to
    the one just removed.
    ************************************************/
    void Remove(const K &key)
    {
        int index = Index(key);
        Eq equalFunc;

        // we look for existing key
        tEntry old = NULL;
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (equalFunc(e->m_Key, key))
            {
                // This is the element to remove
                if (old)
                {
                    old->m_Next = e->m_Next;
                    delete e;
                }
                else
                {
                    m_Table[index] = e->m_Next;
                    delete e;
                }
                --m_Count;
                break;
            }
            old = e;
        }
    }

    tIterator Remove(const tIterator &it)
    {
        int index = Index(it.m_Node->m_Key);
        if (index >= m_Table.Size())
            return tIterator(0, this);

        // we look for existing key
        tEntry old = NULL;
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (e == it.m_Node)
            {
                // This is the element to remove
                if (old)
                {
                    old->m_Next = e->m_Next;
                    delete e;
                    old = old->m_Next;
                }
                else
                {
                    m_Table[index] = e->m_Next;
                    delete e;
                    old = m_Table[index];
                }
                --m_Count;
                break;
            }
            old = e;
        }
        // There is an element in the same column, we return it
        if (!old)
        { // No element in the same bucket, we parse for the next
            while (!old && ++index < m_Table.Size())
                old = m_Table[index];
        }

        return tIterator(old, this);
    }

    /************************************************
    Summary: Access to an hash table element.

    Input Arguments:
        key: key of the element to access.

    Return Value: a copy of the element found.

    Remarks:
        If no element correspond to the key, an element
    constructed with 0.
    ************************************************/
    T &operator[](const K &key)
    {
        int index = Index(key);

        // we look for existing key
        tEntry e = XFind(index, key);
        if (!e)
        {
            if (m_Count >= m_Threshold)
            { // Need Rehash
                Rehash(m_Table.Size() * 2);
                return operator[](key);
            }
            else
            { // No
                e = XInsert(index, key, T());
            }
        }

        return e->m_Data;
    }

    /************************************************
    Summary: Access to an hash table element.

    Input Arguments:
        key: key of the element to access.

    Return Value: an iterator of the element found. End()
    if not found.

    ************************************************/
    tIterator Find(const K &key)
    {
        return tIterator(XFindIndex(key), this);
    }

    /************************************************
    Summary: Access to a constant hash table element.

    Input Arguments:
        key: key of the element to access.

    Return Value: a constant iterator of the element found. End()
    if not found.

    ************************************************/
    tConstIterator Find(const K &key) const
    {
        return tConstIterator(XFindIndex(key), this);
    }

    /************************************************
    Summary: Access to an hash table element.

    Input Arguments:
        key: key of the element to access.

    Return Value: a pointer on the element found. NULL
    if not found.

    ************************************************/
    T *FindPtr(const K &key) const
    {
        tEntry e = XFindIndex(key);
        if (e)
            return &e->m_Data;
        else
            return 0;
    }

    /************************************************
    Summary: search for an hash table element.

    Input Arguments:
        key: key of the element to access.
        value: value to receive the element found value.

    Return Value: TRUE if the key was found, FALSE
    otherwise..

    ************************************************/
    XBOOL LookUp(const K &key, T &value) const
    {
        tEntry e = XFindIndex(key);
        if (e)
        {
            value = e->m_Data;
            return TRUE;
        }
        else
            return FALSE;
    }

    /************************************************
    Summary: test for the presence of a key.

    Input Arguments:
        key: key of the element to access.

    Return Value: TRUE if the key was found, FALSE
    otherwise..

    ************************************************/
    XBOOL IsHere(const K &key) const
    {
        return (XBOOL)XFindIndex(key);
    }

    /************************************************
    Summary: Returns an iterator on the first element.

    Example:
        Typically, an algorithm iterating on an hash table
    looks like:

        XNHashTableIt<T,K,H> it = h.Begin();
        XNHashTableIt<T,K,H> itend = h.End();

        for(; it != itend; ++it) {
            // do something with *t
        }

    ************************************************/
    tIterator Begin()
    {
        for (tEntry *it = m_Table.Begin(); it != m_Table.End(); it++)
        {
            if (*it)
                return tIterator(*it, this);
        }
        return End();
    }

    tConstIterator Begin() const
    {
        for (tEntry *it = m_Table.Begin(); it != m_Table.End(); it++)
        {
            if (*it)
                return tConstIterator(*it, this);
        }
        return End();
    }

    /************************************************
    Summary: Returns an iterator out of the hash table.
    ************************************************/
    tIterator End()
    {
        return tIterator(0, this);
    }

    tConstIterator End() const
    {
        return tConstIterator(0, this);
    }

    /************************************************
    Summary: Returns the index of the given key.

    Input Arguments:
        key: key of the element to find the index.
    ************************************************/
    int Index(const K &key) const
    {
        H hashfun;
        return XIndex(hashfun(key), m_Table.Size());
    }

    /************************************************
    Summary: Returns the elements number.
    ************************************************/
    int Size() const
    {
        return m_Count;
    }

    /************************************************
    Summary: Return the occupied size in bytes.

    Parameters:
        addstatic: TRUE if you want to add the size occupied
    by the class itself.
    ************************************************/
    int GetMemoryOccupation(XBOOL addstatic = FALSE) const
    {
        return m_Table.GetMemoryOccupation() + m_Count * sizeof(XNHashTableEntry<T, K>) + (addstatic ? sizeof(*this) : 0);
    }

private:
    ///
    // Methods

    tEntry *GetFirstBucket() const { return m_Table.Begin(); }

    void
    Rehash(int size)
    {
        int oldsize = m_Table.Size();
        m_Threshold = (int)(size * m_LoadFactor);

        // Temporary table
        XArray<tEntry> tmp;
        tmp.Resize(size);
        tmp.Memset(0);

        for (int index = 0; index < oldsize; ++index)
        {
            tEntry first = m_Table[index];
            while (first)
            {
                H hashfun;
                int newindex = XIndex(hashfun(first->m_Key), size);
                m_Table[index] = first->m_Next;
                first->m_Next = tmp[newindex];
                tmp[newindex] = first;
                first = m_Table[index];
            }
        }
        m_Table.Swap(tmp);
    }

    int XIndex(int key, int size) const
    {
        return key & (size - 1);
    }

    void XDeleteList(tEntry e)
    {
        tEntry tmp = e;
        tEntry del;

        while (tmp != NULL)
        {
            del = tmp;
            tmp = tmp->m_Next;
            delete del;
        }
    }

    tEntry XCopyList(tEntry e)
    {
        tEntry tmp = e;
        tEntry newone;
        tEntry oldone = NULL;
        tEntry firstone = NULL;

        while (tmp != NULL)
        {
            newone = new XNHashTableEntry<T, K>(*tmp);
            if (oldone)
                oldone->m_Next = newone;
            else
                firstone = newone;
            oldone = newone;
            tmp = tmp->m_Next;
        }

        return firstone;
    }

    void XCopy(const XNHashTable &a)
    {
        m_Table.Resize(a.m_Table.Size());
        m_LoadFactor = a.m_LoadFactor;
        m_Count = a.m_Count;
        m_Threshold = a.m_Threshold;

        tEntry *it2 = a.GetFirstBucket();
        for (tEntry *it = m_Table.Begin(); it != m_Table.End(); ++it, ++it2)
        {
            *it = XCopyList(*it2);
        }
    }

    tEntry XFindIndex(const K &key) const
    {
        int index = Index(key);
        return XFind(index, key);
    }

    tEntry XFind(int index, const K &key) const
    {
        Eq equalFunc;

        // we look for existing key
        for (tEntry e = m_Table[index]; e != 0; e = e->m_Next)
        {
            if (equalFunc(e->m_Key, key))
            {
                return e;
            }
        }
        return NULL;
    }

    tEntry XInsert(int index, const K &key, const T &o)
    {
        tEntry newe = new XNHashTableEntry<T, K>(key, o);
        newe->m_Next = m_Table[index];
        m_Table[index] = newe;
        ++m_Count;
        return newe;
    }

    ///
    // Members

    // the hash table data {secret}
    XArray<tEntry> m_Table;
    // The entry count {secret}
    int m_Count;
    // Rehashes the table when count exceeds this threshold. {secret}
    int m_Threshold;
    // The load factor for the hashtable. {secret}
    float m_LoadFactor;
};

#endif
