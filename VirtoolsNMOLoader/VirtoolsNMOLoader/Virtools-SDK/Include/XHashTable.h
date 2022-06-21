/*************************************************************************/
/*	File : XHashTable.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/


#ifndef _XHashTable_H_
#define _XHashTable_H_

#include "XClassArray.h"
#include "XArray.h"
#include "XSArray.h"
#include "XHashFun.h"

#ifdef _WIN32
#pragma warning(disable : 4786)
#endif


const float L = 0.75f;

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
template <class T,class K,class H = XHashFun<K>, class Eq = XEqual<K>/*, float L = 0.75f*/>
class XHashTable
{
	// Types
	
	
	struct Entry
	{
		K					key;
		T					data;
		Entry*				next;
	};

	typedef Entry*			pEntry;
public:

	typedef XHashTable				Table;
	typedef XHashTable*				pTable;
	typedef const XHashTable*		pConstTable;

	/************************************************
	Summary: Iterator on a hash table.

	Remarks: This iterator is the only way to iterate on
	elements in a hash table. The iteration will be in no
	specific order, not in the insertion order. Here is an example 
	of how to use it:

	Example:
		
		XHashTable<T,K,H>::Iterator it = hashtable.Begin();
		while (it != hashtable.End()) {
			// access to the key
			it.GetKey();

			// access to the element
			*it;

			// next element
			++it;
		}

	
	************************************************/
	class Iterator
	{
	public:

		/************************************************
		Summary: Default constructor of the iterator.
		************************************************/
		Iterator():m_It(0) {}

		/************************************************
		Summary: Copy constructor of the iterator.
		************************************************/
		Iterator(const Iterator& n):m_It(n.m_It) {}
		
		/************************************************
		Summary: Operator Equal of the iterator.
		************************************************/
		int operator==(const Iterator& it) const { return m_It == it.m_It; }

		/************************************************
		Summary: Operator Not Equal of the iterator.
		************************************************/
		int operator!=(const Iterator& it) const { return m_It != it.m_It; }
		
		/************************************************
		Summary: Returns a constant reference on the data 
		pointed	by the iterator.

		Remarks:
			The returned reference is constant, so you can't 
		modify its value. Use the other * operator for this 
		purpose.
		************************************************/
		const T& operator*() const { return m_It->data; }

		/************************************************
		Summary: Returns a reference on the data pointed	
		by the iterator.

		Remarks:
			The returned reference is not constant, so you
		can modify its value. 
		************************************************/
		T& operator*() { return m_It->data; }

		/************************************************
		Summary: Returns a pointer on a T object.
		************************************************/
//		operator const T*() const { return &m_It->data; }
		
		/************************************************
		Summary: Returns a pointer on a T object.
		************************************************/
//		operator T*() { return &m_It->data; }

		/************************************************
		Summary: Returns a const reference on the key of
		the pointed entry.
		************************************************/
		const K& GetKey() const {return m_It->key;}
		K& GetKey() {return m_It->key;}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		Iterator& operator++() { // Prefixe
			++m_It;
			return *this;
		}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		Iterator& operator--() { // Prefixe
			--m_It;
			return *this;
		}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		Iterator operator++(int) { 
			Iterator tmp = *this;
			++*this;
			return tmp;
		}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		Iterator operator--(int) { 
			Iterator tmp = *this;
			--*this;
			return tmp;
		}

		
		Iterator(Entry* iT):m_It(iT) {}
		
		Entry*	m_It;
	};
	friend class Iterator;

	/************************************************
	Summary: Constant Iterator on a hash table.

	Remarks: This iterator is the only way to iterate on
	elements in a constant hash table. The iteration will be in no
	specific order, not in the insertion order. Here is an example 
	of how to use it:

	Example:
		
		void MyClass::MyMethod() const
		{
			XHashTable<T,K,H>::ConstIterator it = m_Hashtable.Begin();
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
	class ConstIterator
	{
		friend class XHashTable;
	public:
		/************************************************
		Summary: Default constructor of the iterator.
		************************************************/
		ConstIterator():m_It(0) {}

		/************************************************
		Summary: Copy constructor of the iterator.
		************************************************/
		ConstIterator(const ConstIterator& n):m_It(n.m_It){}
		
		/************************************************
		Summary: Operator Equal of the iterator.
		************************************************/
		int operator==(const ConstIterator& it) const { return m_It == it.m_It; }

		/************************************************
		Summary: Operator Not Equal of the iterator.
		************************************************/
		int operator!=(const ConstIterator& it) const { return m_It != it.m_It; }
		
		/************************************************
		Summary: Returns a constant reference on the data 
		pointed	by the iterator.

		Remarks:
			The returned reference is constant, so you can't 
		modify its value. Use the other * operator for this 
		purpose.
		************************************************/
		const T& operator*() const { return m_It->data; }

		/************************************************
		Summary: Returns a pointer on a T object.
		************************************************/
//		operator const T*() const { return &(m_It->data); }
		
		/************************************************
		Summary: Returns a const reference on the key of
		the pointed entry.
		************************************************/
		const K& GetKey() const {return m_It->key;}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		ConstIterator& operator++() { // Prefixe
			++m_It;
			return *this;
		}

		/************************************************
		Summary: Jumps to next entry in the hashtable.
		************************************************/
		ConstIterator operator++(int) { 
			ConstIterator tmp = *this;
			++*this;
			return tmp;
		}

		
		ConstIterator(Entry* iT):m_It(iT) {}
		
		Entry* m_It;
	};
	friend class ConstIterator;

	/************************************************
	Summary: Struct containing an iterator on an object
	inserted and a BOOL determining if it were really 
	inserted (TRUE) or already there (FALSE).

	
	************************************************/
	struct Pair
	{
		public:
			Pair(Iterator it,int n) : m_Iterator(it),m_New(n) {};

			Iterator				m_Iterator;
			XBOOL					m_New;
	};

	/************************************************
	Summary: Default Constructor.

	Input Arguments: 
		initialsize: The default number of buckets 
		(should be a power of 2, otherwise will be 
		converted.)
		l: Load Factor (see Class Description).
		a: hash table to copy.

	************************************************/
	XHashTable(int initialsize = 16)
	{
		initialsize = Near2Power(initialsize);
		if (initialsize < 4) initialsize = 4;

		m_Table.Resize(initialsize);
		m_Table.Fill(0);
		m_Pool.Reserve((int)(initialsize*L));
	}

	/************************************************
	Summary: Copy Constructor.
	************************************************/
	XHashTable(const XHashTable& a)
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
	~XHashTable()
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
		// we clear all the allocated entries
		m_Pool.Resize(0);
		// we clear the table
		m_Table.Fill(0);
	}

	/************************************************
	Summary: Calculates the average occupation for the 
	buckets by filling an array with the population for
	different bucket size (represented by the index of 
	the array)

	************************************************/
	void GetOccupation(XArray<int>& iBucketOccupation) const
	{
		iBucketOccupation.Resize(1);
		iBucketOccupation[0] = 0;

		for(pEntry* it = m_Table.Begin();it != m_Table.End();it++) {
			if (!*it) { // there is someone there
				iBucketOccupation[0]++;
			} else {
				// count the number of occupant
				int count = 1;
				pEntry e = *it;
				while (e->next) {
					e = e->next;
					count++;
				}
				
				int oldsize = iBucketOccupation.Size();
				if (oldsize <= count) { // we need to resize
					iBucketOccupation.Resize(count+1);

					// and we init to 0
					for (int i=oldsize; i<=count; ++i)
						iBucketOccupation[i] = 0;
				}
		
				// the recensing
				iBucketOccupation[count]++;

			}
		}

	
	}

	/************************************************
	Summary: Affectation operator.

	Remarks:
		The content of the table is enterely overwritten
	by the given table.
	************************************************/
	Table& operator = (const Table& a)
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

	Remarks:
		Insert will automatically override the old value
	and InsertUnique will not replace the old value.
	TestInsert returns a XHashPair, which allow you to know
	if the element was already present.
	************************************************/
	XBOOL Insert(const K& key,const T& o,XBOOL override)
	{
		int index = Index(key);

		// we look for existing key
		pEntry e = XFind(index,key);
		if (e == m_Pool.End()) {
			if (m_Pool.Size() == m_Pool.Allocated()) { // Need Rehash
				Rehash(m_Table.Size()*2);
				return Insert(key,o,override);
			} else { // No
				XInsert(index,key,o);
			}
		} else {
			if (!override) return FALSE;
			e->data = o;
		}

		return TRUE;
	}

	Iterator Insert(const K& key,const T& o)
	{
		int index = Index(key);
		Eq equalFunc;

		// we look for existing key
		for(pEntry e=m_Table[index];e != 0;e = e->next) {
			if (equalFunc(e->key,key)) {
				e->data = o;
				return Iterator(e);
			}
		}
		
		if (m_Pool.Size() == m_Pool.Allocated()) { // Need Rehash
			Rehash(m_Table.Size()*2);
			return Insert(key,o);
		} else { // No
			return Iterator(XInsert(index,key,o));
		}
	}

	
	Pair
	TestInsert(const K& key,const T& o)
	{
		int index = Index(key);
		Eq equalFunc;

		// we look for existing key
		for(pEntry e=m_Table[index];e != 0;e = e->next) {
			if (equalFunc(e->key,key)) {
				return Pair(Iterator(e),0);
			}
		}
		
		// Need Rehash
		if (m_Pool.Size() == m_Pool.Allocated()) { // Need Rehash
			Rehash(m_Table.Size()*2);
			return TestInsert(key,o);
		} else { // No
			return Pair(Iterator(XInsert(index,key,o)),1);
		}
	}

	
	Iterator InsertUnique(const K& key,const T& o)
	{
		int index = Index(key);
		Eq equalFunc;

		// we look for existing key
		for(pEntry e=m_Table[index];e != 0;e = e->next) {
			if (equalFunc(e->key,key)) {
				return Iterator(e);
			}
		}
		
		// Need Rehash
		if (m_Pool.Size() == m_Pool.Allocated()) { // Need Rehash
			Rehash(m_Table.Size()*2);
			return InsertUnique(key,o);
		} else { // No
			return Iterator(XInsert(index,key,o));
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
		int index = Index(key);
		Eq equalFunc;

		// we look for existing key
		pEntry old = NULL;
		for(pEntry e=m_Table[index];e != 0;e = e->next) {
			if (equalFunc(e->key,key)) {
				// This is the element to remove
				
				// change the pointers to it
				if (old) {
					old->next = e->next;
				} else {
					m_Table[index] = e->next;
				}

				// then removed it from the pool
				m_Pool.FastRemove(e);
				if (e != m_Pool.End()) { // wasn't the last one... we need to remap
					RemapEntry(m_Pool.End(), e);
				}


				break;
			}
			old = e;
		}
	}

	Iterator Remove(const Iterator& it)
	{
		int index = Index(it.m_It->key);
		if(index >= m_Table.Size()) 
			return Iterator(m_Pool.End());

		if (!m_Pool.Size())
			return End();

		// we look for existing key
		pEntry old = NULL;
		for (pEntry e=m_Table[index];e != 0;e = e->next) {
			if (e == (it.m_It)) {
				// This is the element to remove
				if (old) {
					old->next = e->next;
					old = old->next;
				} else {
					m_Table[index] = e->next;
					old = m_Table[index];
				}

				// then removed it from the pool
				m_Pool.FastRemove(e);
				if (e != m_Pool.End()) { // wasn't the last one... we need to remap
					RemapEntry(m_Pool.End(), e);
					if (old == m_Pool.End())
						old = e;
				}

				break;
			}
			old = e;
		}

		return it;
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
	T& operator [] (const K& key)
	{
		int index = Index(key);

		// we look for existing key
		pEntry e = XFind(index,key);
		if (e == m_Pool.End()) {
			if (m_Pool.Size() == m_Pool.Allocated()) { // Need Rehash
				Rehash(m_Table.Size()*2);
				return operator [] (key);
			} else { // No
				e = XInsert(index,key,T());
			}
		}

		return e->data;
	}

	/************************************************
	Summary: Access to an hash table element.

	Input Arguments: 
		key: key of the element to access.

	Return Value: an iterator of the element found. End()
	if not found.

	************************************************/
	Iterator Find(const K& key)
	{
		return Iterator(XFindIndex(key));
	}

	/************************************************
	Summary: Access to a constant hash table element.

	Input Arguments: 
		key: key of the element to access.

	Return Value: a constant iterator of the element found. End()
	if not found.

	************************************************/
	ConstIterator Find(const K& key) const
	{
		return ConstIterator(XFindIndex(key));
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
		if (e != m_Pool.End())	
			return &e->data;
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
	XBOOL LookUp(const K& key,T& value) const
	{
		pEntry e = XFindIndex(key);
		if (e != m_Pool.End()) {
			value = e->data;
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
	XBOOL IsHere(const K& key) const
	{
		return (XFindIndex(key) != m_Pool.End());
	}

	/************************************************
	Summary: Returns an iterator on the first element.

	Example:
		Typically, an algorithm iterating on an hash table 
	looks like:

		XHashTable<T,K,H>::Iterator it = h.Begin();
		XHashTable<T,K,H>::Iterator itend = h.End();

		for(; it != itend; ++it) {
			// do something with *t
		}

	************************************************/
	Iterator Begin()
	{
		return Iterator(m_Pool.Begin());
	}

	ConstIterator Begin() const
	{
		return ConstIterator(m_Pool.Begin());
	}

	/************************************************
	Summary: Returns an iterator out of the hash table.
	************************************************/
	Iterator End()
	{
		return Iterator(m_Pool.End());
	}

	ConstIterator End() const
	{
		return ConstIterator(m_Pool.End());
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
	Summary: Returns the index of the given key.

	Input Arguments: 
		key: key of the element to find the index.
	************************************************/
	Iterator GetIteratorByIndex(int index) const
	{
		XASSERT(index < m_Pool.Size());
		return Iterator(m_Pool.Begin()+index);
	}

	/************************************************
	Summary: Returns the elements number.
	************************************************/
	int Size() const
	{
		return m_Pool.Size();
	}

	/************************************************
	Summary: Return the occupied size in bytes.

	Parameters:
		addstatic: TRUE if you want to add the size occupied
	by the class itself.
	************************************************/
	int GetMemoryOccupation(XBOOL addstatic=FALSE) const 
	{
		return m_Table.GetMemoryOccupation(addstatic) +
			m_Pool.Allocated()*sizeof(Entry) +
			(addstatic?sizeof(*this):0);
	}

	/************************************************
	Summary: Reserve an estimation of future hash occupation.

	Parameters:
		iCount: count of elements to reserve
	Remarks:
		you need to call this function before populating 
	the hash table.
	************************************************/
	void Reserve(const int iCount) 
	{
		// Reserve the elements
		m_Pool.Reserve(iCount);

		int tableSize = Near2Power((int)(iCount/L));
		m_Table.Resize(tableSize);
		m_Table.Fill(0);
	}
	
	Iterator GetByIndex(const int iIndex) 
	{
		return(m_Pool.Begin() + iIndex);
	}
	
	

private:
	// Types
	
	///
	// Methods

	
	pEntry* GetFirstBucket() const {return m_Table.Begin();}

	
	void
	Rehash(int iSize) {
		int oldsize = m_Table.Size();

		// we create a new pool
		XClassArray<Entry> pool((int)(iSize*L));
		pool = m_Pool;

		// Temporary table
		XSArray<pEntry> tmp;
		tmp.Resize(iSize);
		tmp.Fill(0);
		
		for (int index = 0; index < oldsize; ++index) {
			Entry* first = m_Table[index];
			while (first) {
				H hashfun;
				int newindex	= XIndex(hashfun(first->key),iSize);

				Entry* newe		= pool.Begin() + (first - m_Pool.Begin());

				// insert new entry in new table
				newe->next		= tmp[newindex];
				tmp[newindex]	= newe;
				
				first			= first->next;          
			}
		}
		m_Table.Swap(tmp);
		m_Pool.Swap(pool);
	}

	
	int XIndex(int key,int size) const
	{
		return key&(size-1);
	}
	
	
	void XCopy(const XHashTable& a)
	{
		int size = a.m_Table.Size();
		m_Table.Resize(size);
		m_Table.Fill(0);

		m_Pool.Reserve(a.m_Pool.Allocated());
		m_Pool = a.m_Pool;

		// remap the address in the table
		for (int i=0; i<size; ++i) {
			if (a.m_Table[i])
				m_Table[i] = m_Pool.Begin() + (a.m_Table[i] - a.m_Pool.Begin());
		}

		// remap the adresses in the entries
		for (Entry* e = m_Pool.Begin(); e != m_Pool.End(); ++e) {
			if (e->next) {
				e->next = m_Pool.Begin() + (e->next - a.m_Pool.Begin());
			}
		}
	}

	
	pEntry XFindIndex(const K& key) const
	{
		int index = Index(key);
		return XFind(index,key);
	}

	
	pEntry XFind(int index,const K& key) const
	{
		Eq equalFunc;

		// we look for existing key
		for(pEntry e=m_Table[index];e != 0;e = e->next) {
			if (equalFunc(e->key,key)) {
				return e;
			}
		}
		return m_Pool.End();
	}

	
	pEntry XInsert(int index,const K& key,const T& o)
	{
		Entry* newe		= GetFreeEntry();
		newe->key		= key; 
		newe->data		= o;
		newe->next		= m_Table[index];
		m_Table[index]	= newe;
		return newe;
	}

	
	pEntry GetFreeEntry()
	{
		// We consider when we arrive here that we have space
		m_Pool.Resize(m_Pool.Size()+1);
		return (m_Pool.End()-1);
	}

	void	RemapEntry(pEntry iOld, pEntry iNew)
	{
		int index = Index(iNew->key);
		XASSERT(m_Table[index]);

		if (m_Table[index] == iOld) { // It was the first of the bucket
			m_Table[index] = iNew;
		} else {
			for (pEntry n = m_Table[index]; n->next != NULL; n = n->next) {
				if (n->next == iOld) { // found one
					n->next = iNew;
					break; // only one can match
				}
			}
		}
		
	}

	///
	// Members
	
	// the hash table data {secret}
	XSArray<pEntry>		m_Table;
	// the entry pool {secret}
	XClassArray<Entry>	m_Pool;
};

#endif
