/*************************************************************************/
/*	File : XSHashTable.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/


#ifndef _XSHashTable_H_
#define _XSHashTable_H_

#include "XArray.h"
#include "XHashFun.h"


template <class T,class K,class H,class Eq> 
class XSHashTable;


#define STATUS_FREE		0

#define STATUS_OCCUPIED	1

#define STATUS_DELETED	2


template <class T,class K>
class XSHashTableEntry
{
	typedef XSHashTableEntry<T,K>*	pEntry;
public:
	
	XSHashTableEntry():m_Status(STATUS_FREE) {}
	XSHashTableEntry(const XSHashTableEntry<T,K>& e) : m_Key(e.m_Key),m_Data(e.m_Data),m_Status(STATUS_OCCUPIED) {}
	~XSHashTableEntry() {}

	void Set(const K& key, const T& data) 
	{
		m_Key		= key;
		m_Data		= data;
		m_Status	= STATUS_OCCUPIED;
	}
	
	K					m_Key;
	T					m_Data;
	int					m_Status;
};

/************************************************
Summary: Iterator on a hash table.

Remarks: This iterator is the only way to iterate on
elements in a hash table. The iteration will be in no
specific order, not in the insertion order. Here is an example 
of how to use it:

Example:
	
	XSHashTableIt<T,K,H> it = hashtable.Begin();
	while (it != hashtable.End()) {
		// access to the key
		it.GetKey();

		// access to the element
		*it;

		// next element
		++it;
	}


************************************************/
template <class T,class K,class H = XHashFun<K>, class Eq = XEqual<K> >
class XSHashTableIt
{
	typedef XSHashTableEntry<T,K>*	pEntry;
	typedef XSHashTableIt<T,K,H,Eq>	tIterator;
	typedef XSHashTable<T,K,H,Eq>*		tTable;
	friend class XSHashTable<T,K,H,Eq>;
public:
	/************************************************
	Summary: Default constructor of the iterator.
	************************************************/
	XSHashTableIt():m_Node(0),m_Table(0){}
	/************************************************
	Summary: Copy constructor of the iterator.
	************************************************/
	XSHashTableIt(const tIterator& n):m_Node(n.m_Node),m_Table(n.m_Table){}
	
	/************************************************
	Summary: Operator Equal of the iterator.
	************************************************/
	int operator==(const tIterator& it) const { return m_Node == it.m_Node; }

	/************************************************
	Summary: Operator Equal of the iterator.
	************************************************/
	int operator!=(const tIterator& it) const { return m_Node != it.m_Node; }
	
	/************************************************
	Summary: Returns a constant reference on the data 
	pointed	by the iterator.

	Remarks:
		The returned reference is constant, so you can't 
	modify its value. Use the other * operator for this 
	purpose.
	************************************************/
	const T& operator*() const { return (*m_Node).m_Data; }

	/************************************************
	Summary: Returns a reference on the data pointed	
	by the iterator.

	Remarks:
		The returned reference is not constant, so you
	can	modify its value. 
	************************************************/
	T& operator*() { return (*m_Node).m_Data; }

	/************************************************
	Summary: Returns a pointer on a T object.
	************************************************/
	operator const T*() const { return &(m_Node->m_Data); }
	
	/************************************************
	Summary: Returns a const reference on the key of
	the pointed entry.
	************************************************/
	const K& GetKey() const {return m_Node->m_Key;}

	/************************************************
	Summary: Jumps to next entry in the hashtable.
	************************************************/
	tIterator& operator++() { // Prefixe
		++m_Node;
		pEntry end = m_Table->m_Table.End();
		while (m_Node != end) { // we're not at the end of the list yet
			if (m_Node->m_Status == STATUS_OCCUPIED) break;
			++m_Node;
		}
		return *this;
	}

	/************************************************
	Summary: Jumps to next entry in the hashtable.
	************************************************/
	tIterator operator++(int) { 
		tIterator tmp = *this;
		++*this;
		return tmp;
	}
	
protected:
	
	XSHashTableIt(pEntry n,tTable t):m_Node(n),m_Table(t){}
	// The Current Node {secret}
	pEntry		m_Node;
	// The Current Table {secret}
	tTable		m_Table;
};

/************************************************
Summary: Constant iterator on a hash table.

Remarks: This iterator is the only way to iterate on
elements in a constant hash table. The iteration will be in no
specific order, not in the insertion order. Here is an example 
of how to use it:

Example:
	
	void MyClass::MyMethod() const
	{
		XSHashTableConstIt<T,K,H> it = m_Hashtable.Begin();
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
template <class T,class K,class H = XHashFun<K>, class Eq = XEqual<K> >
class XSHashTableConstIt
{
	typedef XSHashTableEntry<T,K>*			pEntry;
	typedef XSHashTableConstIt<T,K,H,Eq>	tConstIterator;
	typedef XSHashTable<T,K,H,Eq>const*		tConstTable;
	friend class XSHashTable<T,K,H,Eq>;
public:
	/************************************************
	Summary: Default constructor of the iterator.
	************************************************/
	XSHashTableConstIt():m_Node(0),m_Table(0){}
	/************************************************
	Summary: Copy constructor of the iterator.
	************************************************/
	XSHashTableConstIt(const tConstIterator& n):m_Node(n.m_Node),m_Table(n.m_Table){}
	
	/************************************************
	Summary: Operator Equal of the iterator.
	************************************************/
	int operator==(const tConstIterator& it) const { return m_Node == it.m_Node; }

	/************************************************
	Summary: Operator Equal of the iterator.
	************************************************/
	int operator!=(const tConstIterator& it) const { return m_Node != it.m_Node; }
	
	/************************************************
	Summary: Returns a constant reference on the data 
	pointed	by the iterator.

	Remarks:
		The returned reference is constant, so you can't 
	modify its value. Use the other * operator for this 
	purpose.
	************************************************/
	const T& operator*() const { return (*m_Node).m_Data; }

	/************************************************
	Summary: Returns a pointer on a T object.
	************************************************/
	operator const T*() const { return &(m_Node->m_Data); }
	
	/************************************************
	Summary: Returns a const reference on the key of
	the pointed entry.
	************************************************/
	const K& GetKey() const {return m_Node->m_Key;}

	/************************************************
	Summary: Jumps to next entry in the hashtable.
	************************************************/
	tConstIterator& operator++() { // Prefixe
		++m_Node;
		pEntry end = m_Table->m_Table.End();
		while (m_Node != end) { // we're not at the end of the list yet
			if (m_Node->m_Status == STATUS_OCCUPIED) break;
			++m_Node;
		}
		return *this;
	}

	/************************************************
	Summary: Jumps to next entry in the hashtable.
	************************************************/
	tConstIterator operator++(int) { 
		tConstIterator tmp = *this;
		++*this;
		return tmp;
	}
	
protected:
	
	XSHashTableConstIt(pEntry n,tConstTable t):m_Node(n),m_Table(t){}
	// The Current Node {secret}
	pEntry		m_Node;
	// The Current Table {secret}
	tConstTable	m_Table;
};

/************************************************
Summary: Struct containing an iterator on an object
inserted and a BOOL determining if it were really 
inserted (TRUE) or already there (FALSE).


************************************************/
template <class T,class K,class H = XHashFun<K>, class Eq = XEqual<K> >
class XSHashTablePair
{
	public:
		XSHashTablePair(XSHashTableIt<T,K,H> it,int n) : m_Iterator(it),m_New(n) {};

		XSHashTableIt<T,K,H>		m_Iterator;
		BOOL					m_New;
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
template <class T,class K,class H = XHashFun<K>, class Eq = XEqual<K> >
class XSHashTable
{
	// Types
	typedef XSHashTable<T,K,H,Eq>			tTable;
	typedef XSHashTableEntry<T,K>			tEntry;
	typedef tEntry*							pEntry;
	typedef XSHashTableIt<T,K,H,Eq>			tIterator;
	typedef XSHashTableConstIt<T,K,H,Eq>	tConstIterator;
	typedef XSHashTablePair<T,K,H,Eq>		tPair;
	// Friendship
	friend class XSHashTableIt<T,K,H,Eq>;
	friend class XSHashTableConstIt<T,K,H,Eq>;
public:
	typedef XSHashTableIt<T,K,H,Eq>			Iterator;
	typedef XSHashTableConstIt<T,K,H,Eq>	ConstIterator;

	/************************************************
	Summary: Constructors.

	Input Arguments: 
		initialsize: The default number of buckets 
		(should be a power of 2, otherwise will be 
		converted.)
		l: Load Factor (see Class Description).
		a: hash table to copy.

	************************************************/
	XSHashTable(int initialsize = 8,float l = 0.75f)
	{
		int dec = -1;
		while (initialsize) { initialsize>>=1; dec++; } 
		if (dec > -1) initialsize = 1<<dec;
		else initialsize = 1; // No 0 size allowed
		m_Table.Resize(initialsize);

		if(l<=0.0) l = 0.75f;
		
		m_LoadFactor	= l;
		m_Count			= 0;
		m_Occupation	= 0;
		m_Threshold		= (int)(m_Table.Size() * m_LoadFactor);
	}

	/************************************************
	Summary: Copy Constructor.
	************************************************/
	XSHashTable(const XSHashTable& a)
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
	~XSHashTable()
	{
	}

	/************************************************
	Summary: Removes all the elements from the table.

	Remarks:
		The hash table remains with the same number 
	of buckets after a clear.
	************************************************/
	void Clear()
	{
		for(pEntry it = m_Table.Begin();it != m_Table.End();it++) {
			// we destroy the linked list
			(*it).m_Status = STATUS_FREE;
		}
		m_Count	= 0;
		m_Occupation = 0;
	}

	/************************************************
	Summary: Affectation operator.

	Remarks:
		The content of the table is enterely overwritten
	by the given table.
	************************************************/
	tTable& operator = (const tTable& a)
	{
		if(this != &a) {
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
	
	Return Value: TRUE if the value was really inserted, 
	else otherwise.

	Remarks:
		Insert will automatically override the old value
	and InsertUnique will not replace the old value.
	TestInsert returns a XHashPair, which allow you to know
	if the element was already present.
	************************************************/
	BOOL Insert(const K& key,const T& o,BOOL override)
	{
		// Insert x as active
        int index = XFindPos( key );
        
		if (m_Table[index].m_Status != STATUS_OCCUPIED) {
			
			// If the element was deleted, we remove an element
			if ((m_Table[index].m_Status != STATUS_DELETED)) {
				++m_Occupation; 
				++m_Count;
			} else {
				++m_Count;
			}
		} else { // Occupied
			if (override) {
				// no count or occupation change
			} else  return FALSE;
		}
		m_Table[index].Set(key,o);

		// Test the rehash need
		if( m_Occupation < m_Threshold ) return TRUE;
		
		Rehash(m_Table.Size()*2);
		return TRUE;
	}

	
	tIterator InsertUnique(const K& key,const T& o)
	{
		// Insert x as active
        int index = XFindPos( key );

		if (m_Table[index].m_Status != STATUS_OCCUPIED) {
			
			// If the element was deleted, we remove an element
			if ((m_Table[index].m_Status != STATUS_DELETED)) {
				++m_Occupation; 
				++m_Count;
			} else {
				++m_Count;
			}
		} else { // Occupied
			return tIterator(&m_Table[index],this);
		}
		
		// Need Rehash
		if (m_Count >= m_Threshold) { // Yes
			Rehash(m_Table.Size()*2);
			return InsertUnique(key,o);
		} else { // No
			m_Table[index].Set(key,o);
			return tIterator(&m_Table[index],this);
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
	void Remove(const K& key)
	{
        int index = XFindPos( key );
		if (m_Table[index].m_Status == STATUS_OCCUPIED) {
			m_Table[index].m_Status = STATUS_DELETED;
			--m_Count;
		}
	}

	tIterator Remove(const tIterator& it)
	{
        // may be not necessary
		pEntry e = it.m_Node; 
		if (e == m_Table.End()) return it;
		
		if (e->m_Status == STATUS_OCCUPIED) {
			e->m_Status = STATUS_DELETED;
			--m_Count;
		}

		++e;
		while (e != m_Table.End()) { // we're not at the end of the list yet
			if (e->m_Status == STATUS_OCCUPIED) break;
			++e;
		}

		return tIterator(e,this);
	}

	/************************************************
	Summary: Access to an hash table element.

	Input Arguments: 
		key: key of the element to access.

	Return Value: a reference of the element found.

	Remarks:
		If no element correspond to the key, an exception.
	************************************************/
	T& operator [] (const K& key)
	{
		// Insert x as active
        int index = XFindPos( key );
        
		if (m_Table[index].m_Status != STATUS_OCCUPIED) {
			
			// If the element was deleted, we remove an element
			if ((m_Table[index].m_Status != STATUS_DELETED)) {
				++m_Occupation; 
				++m_Count;
			} else {
				++m_Count;
			}
			m_Table[index].m_Status = STATUS_OCCUPIED;
			m_Table[index].m_Key	= key;
		}

		// Test the rehash need
		if( m_Occupation < m_Threshold ) return m_Table[index].m_Data;

		Rehash(m_Table.Size()*2);
		return m_Table[XFindPos( key )].m_Data;
	}

	/************************************************
	Summary: Access to an hash table element.

	Input Arguments: 
		key: key of the element to access.

	Return Value: an iterator of the element found. End()
	if not found.

	************************************************/
	tIterator Find(const K& key) const
	{
		return tIterator(XFindIndex(key),this);
	}

	/************************************************
	Summary: Access to an hash table element.

	Input Arguments: 
		key: key of the element to access.

	Return Value: a pointer on the element found. NULL
	if not found.

	************************************************/
	T *FindPtr(const K& key) const
	{
		pEntry e = XFindIndex(key);
		if (e)	return &e->m_Data;
		else	return 0;
	}

	/************************************************
	Summary: search for an hash table element.

	Input Arguments: 
		key: key of the element to access.
		value: value to receive the element found value.

	Return Value: TRUE if the key was found, FALSE 
	otherwise..

	************************************************/
	BOOL LookUp(const K& key,T& value) const
	{
		pEntry e = XFindIndex(key);
		if (e) {
			value = e->m_Data;
			return TRUE;
		} else 
			return FALSE;
	}

	/************************************************
	Summary: test for the presence of a key.

	Input Arguments: 
		key: key of the element to access.

	Return Value: TRUE if the key was found, FALSE 
	otherwise..

	************************************************/
	int IsHere(const K& key) const
	{
		return (int)XFindIndex(key);
	}

	/************************************************
	Summary: Returns an iterator on the first element.

	Example:
		Typically, an algorithm iterating on an hash table 
	looks like:

		XSHashTableIt<T,K,H> it = h.Begin();
		XSHashTableIt<T,K,H> itend = h.End();

		for(; it != itend; ++it) {
			// do something with *t
		}

	************************************************/
	tIterator Begin()
	{
		for(pEntry it = m_Table.Begin();it != m_Table.End();it++) {
			if (it->m_Status == STATUS_OCCUPIED) return tIterator(it,this);
		}
		return End();

	}

	tConstIterator Begin() const
	{
		for(pEntry it = m_Table.Begin();it != m_Table.End();it++) {
			if (it->m_Status == STATUS_OCCUPIED) return tConstIterator(it,this);
		}
		return End();

	}

	/************************************************
	Summary: Returns an iterator out of the hash table.
	************************************************/
	tIterator End()
	{
		return tIterator(m_Table.End(),this);
	}

	/************************************************
	Summary: Returns an iterator out of the hash table.
	************************************************/
	tConstIterator End() const
	{
		return tConstIterator(m_Table.End(),this);
	}

	/************************************************
	Summary: Returns the index of the given key.

	Input Arguments: 
		key: key of the element to find the index.
	************************************************/
	int Index(const K& key) const
	{
		H hashfun;
		return XIndex(hashfun(key),m_Table.Size());
	}

	/************************************************
	Summary: Returns the elements number.
	************************************************/
	int Size() const
	{
		return m_Count;
	}

private:
	///
	// Methods
	pEntry* GetFirstBucket() const {return m_Table.ConstBegin();}

	void
	Rehash(int size) {
		int oldsize = m_Table.Size();
		m_Threshold = (int)(size * m_LoadFactor);

		// Temporary table
		XClassArray<tEntry> tmp;
		tmp.Resize(size);
		
		m_Table.Swap(tmp);
		m_Count = 0;
		m_Occupation = 0;

		for (int index = 0; index < oldsize; ++index) {
			pEntry first = &tmp[index];

			if (first->m_Status == STATUS_OCCUPIED) {
				Insert(first->m_Key,first->m_Data,TRUE);
			}
		}
	}

	int XIndex(int key,int size) const
	{
		return key&(size-1);
	}

	
	void XCopy(const XSHashTable& a)
	{
		m_Table			= a.m_Table;
		m_Occupation	= a.m_Occupation;
		m_LoadFactor	= a.m_LoadFactor;
		m_Count			= a.m_Count;
		m_Threshold		= a.m_Threshold;
	}
	
	
	pEntry XFindIndex(const K& key) const
	{
        int index = XFindPos( key );
		if (index < 0) return NULL;
		pEntry e = &m_Table[index];
		if (e->m_Status == STATUS_OCCUPIED)	return e;
		else return NULL;
	}

	
	int XFindPos(const K& key) const
	{
		int index = Index(key);
		int oldindex = index;

		Eq eqaulFunc;

        while (m_Table[index].m_Status == STATUS_OCCUPIED) {
			if (eqaulFunc(m_Table[index].m_Key,key)) 
				return index;
            ++index;  // Compute ith probe
            if (index == m_Table.Size()) index=0;
			if (index == oldindex) return -1;
        }
		return index;	
	}

	///
	// Members
	
	// the hash table data
	XClassArray<tEntry>		m_Table;
	// The entry count
	int						m_Count;
	// The entry count
	int						m_Occupation;
    // Rehashes the table when count exceeds this threshold.
    int						m_Threshold;
    // The load factor for the hashtable.
    float					m_LoadFactor;


};

#endif
