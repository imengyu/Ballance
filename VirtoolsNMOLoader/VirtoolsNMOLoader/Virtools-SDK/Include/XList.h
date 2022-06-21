/*************************************************************************/
/*	File : XList.h														 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef _XLIST_H_
#define _XLIST_H_

#include "XUtil.h"


template <class T>
class XNode {
public:
	T			m_Data;
	XNode<T>*	m_Next;
	XNode<T>*	m_Prev;
};

/************************************************
Summary: Iterator on an XList.

Example:
Usage for itering on a list :
	for (XListIt<T> it = list.Begin(); it != list.End(); ++it) {
		// Do whatever you want with *it, a reference on a T
	}


************************************************/
template <class T>
class XListIt {
	typedef XNode<T>* tNode;
public:
	// Ctor
	XListIt():m_Node(0){}
	XListIt(XNode<T>* n):m_Node(n){}
	XListIt(const XListIt<T>& n):m_Node(n.m_Node){}
	
	// Operators
	int operator==(const XListIt<T>& it) const { return m_Node == it.m_Node; }
	int operator!=(const XListIt<T>& it) const { return m_Node != it.m_Node; }

	/************************************************
	Summary: Returns a reference on the current element.
	************************************************/
	T& operator*() const { return (*m_Node).m_Data; }
	
	/************************************************
	Summary: Go to the next element.
	************************************************/
	XListIt<T>& operator++() { // Prefixe
		m_Node = tNode(m_Node->m_Next);
		return *this;
	}
	XListIt<T> operator++(int) { // PostFixe
		XListIt<T> tmp = *this;
		++*this;
		return tmp;
	}
	/************************************************
	Summary: Go to the previous element.
	************************************************/
	XListIt<T>& operator--() { 
		m_Node = tNode(m_Node->m_Prev);
		return *this;
	}
	XListIt<T> operator--(int) { 
		XListIt<T> tmp = *this;
		--*this;
		return tmp;
	}

	XListIt<T> operator + (int iOffset) const { 
		XListIt<T> tmp = *this;
		while (iOffset--) {
			++tmp;
		}
		return tmp;
	}
	
	XListIt<T> operator - (int iOffset) const { 
		XListIt<T> tmp = *this;
		while (iOffset--) {
			--tmp;
		}
		return tmp;
	}

	
	XNode<T>*			m_Node;
};

/************************************************
Summary: Doubly linked list.

Remarks:
	You can only create a list of elements which
	can be constructed from nothing (default constructor
	with no argument) and the element should also
	possess an operator =.


************************************************/
template <class T>
class XList
{
	typedef XNode<T>* tNode;
public:
	typedef XListIt<T> Iterator;
	/************************************************
	Summary: Constructors.

	Input Arguments: 
		list: list to recopy in the new one.

	************************************************/
	XList()
	{
		m_Node			= new XNode<T>;
		m_Node->m_Prev	= m_Node;
		m_Node->m_Next	= m_Node;
		m_Count			= 0;
	}

	XList(const XList<T>& list)
	{
		m_Node = new XNode<T>;
		m_Node->m_Prev = m_Node;
		m_Node->m_Next = m_Node;
		m_Count = 0;
		for (XListIt<T> it=list.Begin(); it!=list.End(); ++it)
		{
			PushBack(*it);
		}
	}

	/************************************************
	Summary: Affectation operator.

	Remarks:
		The content of the list is enterely overwritten
	by the given one.
	************************************************/
	XList& operator = (const XList<T>& list)
	{
		if (&list != this) {
			Clear();
			for (XListIt<T> it=list.Begin(); it!=list.End(); ++it) {
				PushBack(*it);
			}
		}
		return *this;
	}

	/************************************************
	Summary: Destructor.

	Remarks:
		Release the elements contained in the array. If
	you were storing pointers, you need first to iterate 
	on the list and call delete on each pointer.
	************************************************/
	~XList()
	{
		Clear();
		delete m_Node;
	}

	/************************************************
	Summary: Removes all the elements of a list.
	************************************************/
	void Clear()
	{
		tNode tmp = XBegin();
		tNode del;
		while(tmp != XEnd()) {
			del = tmp;
			tmp = tmp->m_Next;
			delete del;
		}
		m_Node->m_Prev = m_Node;
		m_Node->m_Next = m_Node;
		m_Count = 0;
	}

	/************************************************
	Summary: Returns the elements number.
	************************************************/
	int Size() const
	{
		return m_Count;
	}

	/************************************************
	Summary: Returns a copy of the first element of an array.

	Remarks:
		No test are provided to see if there is an
	element.
	************************************************/
	T Front () const
	{
		return XBegin()->m_Data;
	}

	/************************************************
	Summary: Returns a reference on the first element of an array.

	Remarks:
		No test are provided to see if there is an
	element.
	************************************************/
	T& Front()
	{
		return XBegin()->m_Data;
	}

	/************************************************
	Summary: Returns a copy of the last element of an array.

	Remarks:
		No test are provided to see if there is an
	element.
	************************************************/
	T Back() const
	{
		XListIt<T> tmp = End();
		return *(--tmp);
	}

	/************************************************
	Summary: Returns a reference on the last element of an array.

	Remarks:
		No test are provided to see if there is an
	element.
	************************************************/
	T& Back()
	{
		XListIt<T> tmp = End();
		return *(--tmp);
	}


	/************************************************
	Summary: Inserts an element at the end of a list.

	Input Arguments: 
		o: object to insert.
	************************************************/
	void PushBack(const T& o)
	{
		XInsert(XEnd(),o);
	}

	/************************************************
	Summary: Inserts an element at the start of a list.

	Input Arguments: 
		o: object to insert.
	************************************************/
	void PushFront(const T& o)
	{
		XInsert(XBegin(),o);
	}

	/************************************************
	Summary: Inserts an element before another one.

	Input Arguments: 
		i: iterator on the element to insert before.
		o: object to insert.
	************************************************/
	void Insert(XListIt<T>& i,const T& o)
	{
		XInsert(i.m_Node,o);
	}

	/************************************************
	Summary: Removes an element at the end of a list.
	************************************************/
	void PopBack()
	{
		XListIt<T> tmp = End();
		XRemove((--tmp).m_Node);
	}

	/************************************************
	Summary: Removes an element at the beginning of 
	a list.
	************************************************/
	void PopFront()
	{
		XRemove(XBegin());
	}

	/************************************************
	Summary: Finds an element.

	Input Arguments: 
		o: object to find.
		start: iterator from which begin the search.
	Return Valuen : An iterator on the object found,
	End() if the object wasn't found.
	************************************************/
	XListIt<T> Find(const T& o) const
	{
		m_Node->m_Data = o;
		XListIt<T> it = Begin();
		while (*it != o) ++it;
		return it;
	}
	XListIt<T> Find(const XListIt<T>& start,const T& o) const
	{
		m_Node->m_Data = o;
		XListIt<T> it = start;
		while (*it != o) ++it;
		return it;
	}

	/************************************************
	Summary: Test the presence of an element.

	Input Arguments: 
		o: object to test.
	Return Value: TRUE if the object was found, 
	otherwise FALSE.
	************************************************/
	BOOL IsHere(const T& o) const
	{
		XListIt<T> it = Find(o);
		if (it != End()) return TRUE;
		return FALSE;
	}

	/************************************************
	Summary: Removes an element.

	Input Arguments: 
		o: object to remove.
	Return Value: TRUE if the object was removed, 
	otherwise FALSE.
	************************************************/
	BOOL Remove(const T& o)
	{
		XListIt<T> it = Find(o);
		if (it == End()) return FALSE;
		else {
			Remove(it);
			return TRUE;
		}
	}

	/************************************************
	Summary: Removes an element.

	Input Arguments: 
		i: iterator on the object to remove.
	Return Value: an iterator on the element following 
	the removed one.
	************************************************/
	XListIt<T> Remove(XListIt<T>& i)
	{
		return XRemove(i.m_Node);
	}

	/************************************************
	Summary: Returns an iterator on the first element.
	************************************************/
	XListIt<T> Begin() const {return XBegin();}

	/************************************************
	Summary: Returns an iterator after the last element.
	************************************************/
	XListIt<T> End() const {return XEnd();}

	/************************************************
	Summary: Swaps two lists.

	Input Arguments: 
		o: second list to swap with.
	************************************************/
	void Swap(XList<T>& a)
	{
		XSwap(m_Node,a.m_Node);
		XSwap(m_Count,a.m_Count);
	}

private:
	///
	// Methods
	
	
	XNode<T>* XBegin() const {return tNode(m_Node->m_Next);}

	
	XNode<T>* XEnd() const {return tNode(m_Node);}

	
	XNode<T>* XInsert(XNode<T>* i,const T& o)
	{
		XNode<T>* n = new XNode<T>;
		// Data
		n->m_Data = o;
		// Pointers
		n->m_Next = i;
		n->m_Prev = i->m_Prev;
		i->m_Prev->m_Next = n;
		i->m_Prev = n;
		m_Count++;

		return n;
	}
	
	
	XNode<T>* XRemove(XNode<T>* i)
	{
		XNode<T>* next = i->m_Next;
		XNode<T>* prev = i->m_Prev;
		prev->m_Next = next;
		next->m_Prev = prev;
		// we delete the old node
		delete i;
		m_Count--;
		// We return the element just after
		return next;
	}

	///
	// Members
	
	
	XNode<T>*	m_Node;
	
	int			m_Count;
};

#endif
