/*************************************************************************/
/*	File : XBinaryTree.h												 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef BINARYTREE_H

#define BINARYTREE_H


#include "XUtil.h"



template <class T>
class Compare
{
public:
	bool operator() (const T& iT1,const T& iT2) const {return (iT1 < iT2);}
};

/************************************************
Summary: Not Yet Documented


************************************************/
template <class T, class TCmpFunc = Compare<T>, bool TMulti = true>
class XBTree 
{
public:
	typedef T			Type;
	typedef T*			Pointer;
	typedef T&			Reference;
	typedef const T&	ConstReference;

protected:
	enum Color {RED, BLACK};

	struct Node
	{
		Node*	parent;
		Node*	left;
		Node*	right;
		Color	color;
		T		value;
	};

public:
	class Iterator;

	// Const iterator
	class ConstIterator
	{
	public:
        ConstIterator(){}
        ConstIterator ( Node* iP):m_Node(iP) {}
        ConstIterator(const Iterator& iCopy)
                : m_Node(iCopy.m_Node) {}
        ConstReference operator*() const
                {return m_Node->value; }
        Type* operator->() const
                {return &m_Node->value; }
        ConstIterator& operator++()
                {XInc();
                return (*this); }
        ConstIterator operator++(int)
                {ConstIterator _Tmp = *this;
                ++*this;
                return (_Tmp); }
        ConstIterator& operator--()
                {XDec();
                return (*this); }
        ConstIterator operator--(int)
                {ConstIterator _Tmp = *this;
                --*this;
                return (_Tmp); }
        bool operator==(const ConstIterator& iX) const
                {return (m_Node == iX.m_Node); }
        bool operator!=(const ConstIterator& iX) const
                {return (!(*this == iX)); }
	protected:
        void XDec() 
		{
			if (m_Node->color == RED && XParent(m_Node->parent) == m_Node) {
                        m_Node = m_Node->right;
			} else {
				if (m_Node->left != m_Nil) {
					m_Node = XMax(m_Node->left);
				} else {
					Node* n;
                    while (m_Node == XLeft(n = m_Node->parent))
						m_Node = n;
					m_Node = n; 
				}
			}
		}
        void XInc()
		{
			if (m_Node->right != m_Nil) {
                        m_Node = XMin(m_Node->right);
			} else {
				Node* n;
                while (m_Node == XRight(n = m_Node->parent))
					m_Node = n;
				
				if (m_Node->right != n) {
					m_Node = n;
				}
			}
		}

		Node*	m_Node;
	};
	friend class ConstIterator;

	// Non-Const Iterator
	class Iterator : public ConstIterator 
	{
	public:
        Iterator() {}
        Iterator(Node* iNode) : ConstIterator(iNode) {}
        Reference operator*() const {return this->m_Node->value;}
        Iterator& operator++() {
			this->XInc();
			return (*this); 
		}
        Iterator operator++(int) {
			Iterator _Tmp = *this;
            ++*this;
            return (_Tmp); 
		}
        Iterator& operator--() {
			this->XDec();
			return (*this); 
		}
        Iterator operator--(int) {
			Iterator _Tmp = *this;
			--*this;
			return (_Tmp); 
		}
        bool operator==(const Iterator& iA) const
                {return (this->m_Node == iA.m_Node); }
        bool operator!=(const Iterator& iA) const
                {return (!(*this == iA)); }

	};
	friend class Iterator;

	///
	// Methods

    explicit XBTree(const TCmpFunc& iCmp = TCmpFunc()):m_KeyCompare(iCmp)
	{
		XInit();
	}
    XBTree(const XBTree& iModel):m_KeyCompare(iModel.m_KeyCompare)
	{
		XInit();
        XCopy(iModel); 
	}
    ~XBTree()
	{
		Erase(Begin(), End());
		XFreeNode(m_Head);

		// Nil Node management
		// TODO : thread safe ? -> lock resource
		if (--m_NilRefCount == 0) {
			XFreeNode(m_Nil);
			m_Nil = 0; 
		}
	}
    
    const XBTree& operator = (const XBTree& iModel)
	{
		if (this != &iModel) {
			Erase(Begin(), End());
			m_KeyCompare	= iModel.m_KeyCompare;
			XCopy(iModel); 
		}
		return *this;
	}

	/************************************************
	Summary: Returns an iterator on the first element.
	************************************************/
	Iterator Begin()	{return Iterator(this->XLMost());}
 	/************************************************
	Summary: Returns an iterator after the last element.
	************************************************/
    Iterator End()		{return Iterator(m_Head); }
    
	/************************************************
	Summary: Returns a const iterator on the first element.
	************************************************/
	ConstIterator Begin() const {return Iterator(this->XLmost());}
 	/************************************************
	Summary: Returns a const iterator after the last element.
	************************************************/
	ConstIterator End()	const	{return ConstIterator(m_Head); }
    
 	/************************************************
	Summary: Returns an iterator on the first 
	element for a reverse iteration.
	************************************************/
	Iterator RBegin() {return (Size())?Iterator(--End()):Iterator(m_Nil);}
 	/************************************************
	Summary: Returns a const iterator after the last 
	element for a reverse iteration.
	************************************************/
	Iterator REnd() {return (Size()>1)?Iterator(--Begin()):Iterator(--(--(Begin()))); }

	/************************************************
	Summary: Returns a const iterator on the first 
	element for a reverse iteration.
	************************************************/
	ConstIterator RBegin() const {return ConstIterator(End());}
 	/************************************************
	Summary: Returns a const iterator after the last 
	element for a reverse iteration.
	************************************************/
	ConstIterator REnd() const {return ConstIterator(Begin()); }

	/************************************************
	Summary: Returns the elements number.
	************************************************/
	unsigned int Size() const	{return m_Size; }
	

	Iterator Insert(Iterator iT, ConstReference iValue)
	{
		if (Size() == 0)
			;
		else if (iT == Begin()) {
			if (m_KeyCompare(iValue, iT.m_Node->value))
				return (XInsert(m_Head, iT.m_Node, iValue)); 
		} else if (iT == End()) {
			if (m_KeyCompare(XValue(XRMost()), iValue))
				return XInsert(m_Nil, XRMost(), iValue); 
		} else {
			Iterator i = iT;
			if (m_KeyCompare((--i).m_Node->value, iValue) && m_KeyCompare(iValue, iT.m_Node->value)) {
				if (i.m_Node->right == m_Nil)
					return XInsert(m_Nil, i.m_Node, iValue);
				else
					return XInsert(m_Head, iT.m_Node, iValue); 
			}
		}
		return Insert(iValue); 
	}

	Iterator Insert(ConstReference iValue)
	{
		Node* n = XRoot();
		Node* nodeBefore = m_Head;
		bool ans = true;
		while (n != m_Nil) {
			nodeBefore = n;
			ans = m_KeyCompare(iValue,n->value);
			n = ans ? n->left : n->right; 
		}

		if (TMulti) return XInsert(n,nodeBefore, iValue);
		Iterator it = Iterator(nodeBefore);
		if (!ans)
			;
		else if (it == Begin())
			return XInsert(n, nodeBefore, iValue);
		else
			--it;
		if (m_KeyCompare(*it, iValue))
			return XInsert(n, nodeBefore, iValue);
		
		// already in the tree... maybe do a pair ????
		return it; 
	}

	Iterator PushBack(ConstReference iValue) {return Insert(iValue);}

	Iterator Erase(Iterator iT)
	{
		Node* x;
		Node* y = (iT++).m_Node;
		Node* z = y;
		if (XLeft(y) == m_Nil) x = XRight(y);
		else if (XRight(y) == m_Nil) x = XLeft(y);
				else y = XMin(XRight(y)), x = XRight(y);
		
		{
//			_Lockit _Lk;
			if (y != z) {
				XParent(XLeft(z)) = y;
				XLeft(y) = XLeft(z);
				if (y == XRight(z)) XParent(x) = y;
				else {
					XParent(x) = XParent(y);
					XLeft(XParent(y)) = x;
					XRight(y) = XRight(z);
					XParent(XRight(z)) = y; 
				}
				if (XRoot() == z) {
					XRoot() = y;
				} else {
					if (XLeft(XParent(z)) == z) XLeft(XParent(z)) = y;
					else XRight(XParent(z)) = y;
				}
				XParent(y) = XParent(z);
				XSwap(XColor(y), XColor(z));
				y = z; 
			} else {
				XParent(x) = XParent(y);
				if (XRoot() == z) {
					XRoot() = x;
				} else {
					if (XLeft(XParent(z)) == z) {
						XLeft(XParent(z)) = x;
					} else {
						XRight(XParent(z)) = x;
					}
				}
				if (XLMost() != z) ;
				else {
					if (XRight(z) == m_Nil) {
						XLMost() = XParent(z);
					} else {
						XLMost() = XMin(x);
					}
				}

				if (XRMost() != z) ;
				else {
					if (XLeft(z) == m_Nil) {
						XRMost() = XParent(z);
					} else {
						XRMost() = XMax(x); 
					}
				}
			}
			if (XColor(y) == BLACK) {
				while (x != XRoot() && XColor(x) == BLACK) {
					if (x == XLeft(XParent(x))) {
						Node* _W = XRight(XParent(x));
						if (XColor(_W) == RED) {
							XColor(_W) = BLACK;
							XColor(XParent(x)) = RED;
							XLRotate(XParent(x));
							_W = XRight(XParent(x)); 
						}
						if (XColor(XLeft(_W)) == BLACK && XColor(XRight(_W)) == BLACK) {
							XColor(_W) = RED;
							x = XParent(x); 
						} else {
							if (XColor(XRight(_W)) == BLACK) {
								XColor(XLeft(_W)) = BLACK;
								XColor(_W) = RED;
								XRRotate(_W);_W = XRight(XParent(x)); 
							}
							XColor(_W) = XColor(XParent(x));
							XColor(XParent(x)) = BLACK;
							XColor(XRight(_W)) = BLACK;
							XLRotate(XParent(x));
							break;
						}
					} else {
						Node* _W = XLeft(XParent(x));
						if (XColor(_W) == RED) {
							XColor(_W) = BLACK;
							XColor(XParent(x)) = RED;
							XRRotate(XParent(x));
							_W = XLeft(XParent(x)); 
						}
						if (XColor(XRight(_W)) == BLACK && XColor(XLeft(_W)) == BLACK) {
							XColor(_W) = RED;
							x = XParent(x); 
						} else {
							if (XColor(XLeft(_W)) == BLACK) {
								XColor(XRight(_W)) = BLACK;
								XColor(_W) = RED;
								XLRotate(_W);
								_W = XLeft(XParent(x)); 
							}
							XColor(_W) = XColor(XParent(x));
							XColor(XParent(x)) = BLACK;
							XColor(XLeft(_W)) = BLACK;
							XRRotate(XParent(x));
							break;
						}
					}
					XColor(x) = BLACK; 
				}
			}
		}
		XDestval(&XValue(y));
		XFreeNode(y);
		--m_Size;
		return (iT); 
	}
	Iterator Erase(Iterator iFirst, Iterator iLast)
	{
		if (Size() == 0 || iFirst != Begin() || iLast != End()) {
			while (iFirst != iLast) {
				Erase(iFirst++);
			}
			return iFirst; 
		} else {
			XErase(XRoot());
			XRoot() = m_Nil;
			m_Size = 0;
			XLMost() = m_Head; 
			XRMost() = m_Head;
			return Begin(); 
		}
	}
    void Clear() {
		Erase(Begin(), End()); 
	}
    Iterator Find(ConstReference iValue) {
		Iterator i(XLBound(iValue));
		return ((i == End() || m_KeyCompare(iValue, i.m_Node->value))? End() : i); 
	}
    ConstIterator Find(ConstReference iValue) const {
		ConstIterator i(XLBound(iValue));
		return ((i == End() || m_KeyCompare(iValue, i.m_Node->value))? End() : i); 
	}
	
	void Swap(XBTree& iTree)
	{
		Swap(m_KeyCompare, iTree.m_KeyCompare);
		if (this->m_Allocator == iTree.m_Allocator) {
			Swap(m_Head, iTree.m_Head);
			Swap(m_Size, iTree.m_Size); 
		} else {
			XBTree _Ts = *this;
			*this = iTree;
			iTree = _Ts; 
		}
	}

protected:

	static Node*& XLeft(Node* iNode)	{return ((Node*&)(*iNode).left); }
	static Node*& XParent(Node* iNode)	{return ((Node*&)(*iNode).parent); }
	static Node*& XRight(Node* iNode)	{return iNode->right; }
	static Color& XColor(Node* iNode)	{return iNode->color; }
	static Reference XValue(Node* iNode){return ((Reference)(*iNode).value); }

	static Node* XMax(Node* iN) {while (iN->right != m_Nil) iN = iN->right; return iN; }
	static Node* XMin(Node* iN)	{while (iN->left != m_Nil) iN = iN->left; return iN; }

	Node*&	XRoot() const	{return m_Head->parent;}
	Node*&	XRMost() const	{return m_Head->right;}
	Node*&	XLMost() const	{return m_Head->left;}

	void XLRotate(Node* iN) {
		Node* y = iN->right;
		iN->right = y->left;
		if (y->left != m_Nil) XParent(y->left) = iN;
		y->parent = iN->parent;
		if (iN == XRoot()) {
			XRoot() = y; 
		} else {
			if (iN == XLeft(iN->parent)) {
				XLeft(iN->parent) = y;
			} else {
				XRight(iN->parent) = y;
			}
		}
		y->left = iN;
		iN->parent = y; 
	}

	void XRRotate(Node* iN) {
		Node* y = iN->left;
		iN->left = y->right;
		if (y->right != m_Nil) XParent(y->right) = iN;
		y->parent = iN->parent;
		if (iN == XRoot()) XRoot() = y;
		else if (iN == XRight(iN->parent))
				XRight(iN->parent) = y;
		else
				XLeft(iN->parent) = y;
		y->right = iN;
		iN->parent = y; 
	}

	Node* XLBound(ConstReference iValue) const
	{
		Node* x = XRoot();
		Node* y = m_Head;
		while (x != m_Nil) {
			if (m_KeyCompare(x->value, iValue)) {
				x = x->right;
			} else {
				y = x;
				x = x->left;
			}
		}
		return (y); 
	}
	
	Node* XUBound(ConstReference iValue) const
	{
		Node* x = XRoot();
		Node* y = m_Head;
		while (x != m_Nil) {
			if (m_KeyCompare(iValue, x->value)) {
						y = x, x = x->left;
			} else {
				x = x->right;
			}
		}
		return (y); 
	}

	void XInit()
	{
		Node* tmp = XBuynode(0, BLACK);
		{
			// _Lockit _Lk; :: TODO Thread locking
			if (m_Nil == 0) {
				m_Nil = tmp;
				tmp = 0;
				m_Nil->left = 0;
				m_Nil->right = 0; 
			}
			++m_NilRefCount; 
		}
		if (tmp != 0) {
			XFreeNode(tmp);
		}
		m_Head = XBuynode(m_Nil, RED);
		m_Size = 0;
		XLMost() = m_Head;
		XRMost() = m_Head; 
	}

	void XErase(Node* iNode)
	{
		for (Node* n = iNode; n != m_Nil; iNode = n) {
			XErase(n->right);
			n = n->left;
			XDestval(&XValue(iNode));
			XFreeNode(iNode); 
		}
	}

	Iterator XInsert(Node* iNode, Node* iBefore, ConstReference iValue)
	{
		Node* z = XBuynode(iBefore, RED);
		z->left	= m_Nil;
		z->right= m_Nil;
		XConsval(&z->value, iValue);
		++m_Size;
		if (iBefore == m_Head || iNode != m_Nil || m_KeyCompare(iValue, iBefore->value)) {
			iBefore->left = z;
			if (iBefore == m_Head) {
				XRoot() = z;
				XRMost() = z; 
			} else {
				if (iBefore == XLMost()) XLMost() = z; 
			}
		} else {
			iBefore->right = z;
			if (iBefore == XRMost()) XRMost() = z; 
		}
		for (iNode = z; iNode != XRoot() && XColor(iNode->parent) == RED; ) {
			if (iNode->parent == XLeft(XParent(iNode->parent))) {
				iBefore = XRight(XParent(iNode->parent));
				if (iBefore->color == RED) {
					XColor(iNode->parent) = BLACK;
					iBefore->color = BLACK;
					XColor(XParent(iNode->parent)) = RED;
					iNode = XParent(iNode->parent); 
				} else {
					if (iNode == XRight(iNode->parent)) {
						iNode = iNode->parent;
						XLRotate(iNode); 
					}
					XColor(iNode->parent) = BLACK;
					XColor(XParent(iNode->parent)) = RED;
					XRRotate(XParent(iNode->parent)); 
				}
			} else {
				iBefore = XLeft(XParent(iNode->parent));
				if (iBefore->color == RED) {
					XColor(iNode->parent) = BLACK;
					iBefore->color = BLACK;
					XColor(XParent(iNode->parent)) = RED;
					iNode = XParent(iNode->parent); 
				} else {
					if (iNode == XLeft(iNode->parent)) {
						iNode = iNode->parent;
						XRRotate(iNode); 
					}
					XColor(iNode->parent) = BLACK;
					XColor(XParent(iNode->parent)) = RED;
					XLRotate(XParent(iNode->parent));
				}
			}
		}
		XColor(XRoot()) = BLACK;
		return Iterator(z); 
	}
	
	//
	// (De)Allocation methods

    Node* XBuynode(Node* iParent, Color iColor) {
		Node* n		= new Node;
		n->parent	= iParent;
		n->color	= iColor;
		return n; 
	}
    // Construction by placement new
	void XConsval(Pointer iP, ConstReference iValue) {new ((void*)iP) T(iValue); }
    void XDestval(Pointer iP) {(iP)->~T(); }
	void XFreeNode(Node* iN) {delete iN;}

	
	// Null reference
	static Node*	m_Nil;
	static int		m_NilRefCount;

	TCmpFunc		m_KeyCompare;
	Node*			m_Head;
	unsigned int	m_Size;


};

template <class T, class TCmpFunc, bool TMulti>
typename XBTree<T,TCmpFunc,TMulti>::Node*	XBTree<T,TCmpFunc,TMulti>::m_Nil = 0;

template <class T, class TCmpFunc, bool TMulti>
int	XBTree<T,TCmpFunc,TMulti>::m_NilRefCount = 0;

/************************************************
Summary: Not Yet Documented


************************************************/
template <class T>
class XBinaryTreeNode
{
public:
	// Ctors
	XBinaryTreeNode() : m_Left(NULL),m_Right(NULL){}
	XBinaryTreeNode( const T & E ) : m_Data( E ), m_Left( NULL ),m_Right( NULL ) { }
	XBinaryTreeNode( const T & E,XBinaryTreeNode* L, XBinaryTreeNode* R ):m_Data(E),m_Left(L),m_Right(R) { }
	// Dtor
	~XBinaryTreeNode() {}
	
	///
	// Members
	
	// Data
	T					m_Data;
	// Children
	XBinaryTreeNode*	m_Left;
	XBinaryTreeNode*	m_Right;
};

template <class T>
class XBinaryTree;


/************************************************
Summary: Not Yet Documented


************************************************/
template <class T>
class XBinaryTreeIt
{
	typedef XBinaryTreeNode<T>	tNode;
	typedef XBinaryTree<T>*		tTree;
	typedef XBinaryTreeIt<T>	tIterator;
public:
	// Ctors
	XBinaryTreeIt(const tNode* n, const tTree tree) : m_Tree(tree),m_Node(n){}
	XBinaryTreeIt(tNode* n=NULL):m_Node(n) {}
	// Dtor
	~XBinaryTreeIt() {}
	
	// Operators
	int operator==(const tIterator& it) const { return m_Node == it.m_Node; }
	int operator!=(const tIterator& it) const { return m_Node != it.m_Node; }
	
	const T& operator*() { return (*m_Node).m_Data; }
//	T* operator*() { return &(m_Node->m_Data); }

	tIterator& operator++() { // Prefixe
		// TODO
		return *this;
	}
	tIterator operator++(int) { 
		tIterator tmp = *this;
		++*this;
		return tmp;
	}

	///
	// Members
	
	// The Binary Tree
	tTree		m_Tree;
	// Current Node
	tNode*		m_Node;

};

/************************************************
Summary: Not Yet Documented


************************************************/
template <class T>
class XBinaryTree
{
	typedef XBinaryTreeIt<T>	tIterator;
	typedef XBinaryTreeNode<T>	tNode;
	friend class XBinaryTreeIt<T>;
public:
	XBinaryTree() 
	{
		m_Root = m_NullNode = new tNode;
	}

	~XBinaryTree()
	{
		XRemove(m_Root);
		delete m_NullNode;
	}

	void Clear()
	{
		XRemove(m_Root);
		m_Root = m_NullNode;
	}

	int	GetMemoryOccupation() const
	{
		// Childs + this + NullNode
		return XMemory(m_Root) + sizeof(XBinaryTree<T>) + sizeof(tNode);
	}

	tIterator Begin() const
	{
		return End();

	}

	tIterator End() const
	{
		return tIterator(m_NullNode);
	}

	// Infixed iteration
	void Iterate(void(*f)(const T&,void *),void* arg = NULL) const {
		XInfixe(m_Root,f,arg);
	}

	// Postfixed iteration
	void BackIterate(void(*f)(const T&,void *),void* arg = NULL) const {
		XPostfixe(m_Root,f,arg);
	}

	// Prefixed iteration
	void Prefixe(void(*f)(const T&,void *),void* arg = NULL) const {
		XPrefixe(m_Root,f,arg);
	}

	static
	int XCompare(const void *elem1, const void *elem2 ) 
	{
		return (*(T*)elem2 - *(T*)elem1);
	}

	// Get The Smallest element 
	T GetSmallest() const
	{
		if (m_Root == m_NullNode) return (T)0;
		return XGetSmallest(m_Root);
	}

	// Insert an element in the tree (return 0 if it was already there)
	int Insert(const T& o,VxSortFunc compare  = XCompare)
	{
		m_NullNode->m_Data = o;
		tNode** parent = &m_Root;
		tNode* t = m_Root;
		while(compare(&t->m_Data,&o) != 0) {
			if(compare(&o,&t->m_Data)>0) {
				parent = &t->m_Left;
				t = t->m_Left;
			} else {
				parent = &t->m_Right;
				t = t->m_Right;
			}
		}
		// The value wasn't found, we insert it
		if(t == m_NullNode) {
			*parent = new tNode(o,m_NullNode,m_NullNode);; 
			return 1;
		}
		// already in the tree
		return 0;
	}

	// Insert an element in the tree (return 0 if it was already there)
	int Remove(const T& o,VxSortFunc compare  = XCompare)
	{
		m_NullNode->m_Data = o;
		tNode** parent = &m_Root;
		tNode* t = m_Root;
		while(compare(&t->m_Data,&o) != 0) {
			if (compare(&o,&t->m_Data)>0) {
				parent = &t->m_Left;
				t = t->m_Left;
			} else {
				parent = &t->m_Right;
				t = t->m_Right;
			}
		}
		// The value was found, we remove it
		if(t != m_NullNode) {
			// No left child ?
			if(t->m_Left == m_NullNode) {
				// if it's a leaf ?
				if (t->m_Right == m_NullNode) {
					delete t;
					*parent = m_NullNode;
				} else { // we reup the right tree in place
					*parent = t->m_Right;
					delete t;
				}
			} else { // There's a left child
				// No right Child
				if (t->m_Right == m_NullNode) { // we reup the left tree in place
					*parent = t->m_Left;
					delete t;
				} else { // we have to put the max of the left tree in place
					tNode* left = t->m_Left;
					parent = &t;
					while(left->m_Right != m_NullNode) {
						parent = &left;
						left = left->m_Right;
					}
					*parent = left->m_Left;
					t->m_Data = left->m_Data;
					delete left;
				}
			}
			return 1;
		}
		// already in the tree
		return 0;
	}

	// Search an element in the tree
	int Find(const T& o,VxSortFunc compare  = XCompare) const
	{
		m_NullNode->m_Data = o;
		tNode* t = m_Root;
		while(compare(&t->m_Data,&o) != 0) {
			if (compare(&o,&t->m_Data)>0) {
				t = t->m_Left;
			} else {
				t = t->m_Right;
			}
		}

		return t != m_NullNode;
	}

	// Search an element in the tree
	T* FindPtr(const T& o,VxSortFunc compare  = XCompare) const
	{
		m_NullNode->m_Data = o;
		tNode* t = m_Root;
		while(compare(&t->m_Data,&o) != 0) {
			if (compare(&o,&t->m_Data)>0) {
				t = t->m_Left;
			} else {
				t = t->m_Right;
			}
		}

		return (t != m_NullNode) ? &t->m_Data : NULL;
	}

private:
	// Methods
	void XRemove(tNode* t)
	{
		if( t != m_NullNode) {
			XRemove( t->m_Left );
			XRemove( t->m_Right );
			delete t;
		}
	}

	int XMemory(tNode* t)
	{
		
		if( t != m_NullNode) {
			return XMemory( t->m_Left ) + XMemory( t->m_Right ) + sizeof(tNode);
		} else return 0;

	}

	// Get The Smallest element 
	T XGetSmallest(tNode* t) const
	{
		if ((t->m_Left == m_NullNode) && (t->m_Right == m_NullNode)) return t->m_Data;
		if (t->m_Left != m_NullNode) return XGetSmallest(t->m_Left);
		if (t->m_Right!= m_NullNode) return XGetSmallest(t->m_Right);
		return t->m_Data;
	}

	void XInfixe(tNode* t,void(*f)(const T&,void *),void* arg) const {
		if( t != m_NullNode) {
			XInfixe(t->m_Left,f,arg);
			f(t->m_Data,arg);
			XInfixe(t->m_Right,f,arg);
		}
	}

	void XPrefixe(tNode* t,void(*f)(const T&,void *),void* arg) const {
		if( t != m_NullNode) {
			f(t->m_Data,arg);
			XPrefixe(t->m_Left,f,arg);
			XPrefixe(t->m_Right,f,arg);
		}
	}
	
	void XPostfixe(tNode* t,void(*f)(const T&,void *),void* arg) const {
		if( t != m_NullNode) {
			XPostfixe(t->m_Right,f,arg);
			f(t->m_Data,arg);
			XPostfixe(t->m_Left,f,arg);
		}
	}

	// Members
	// Root
	tNode*		m_Root;
	// Null Node
	tNode*		m_NullNode;
};



#endif // BINARYTREE_H
