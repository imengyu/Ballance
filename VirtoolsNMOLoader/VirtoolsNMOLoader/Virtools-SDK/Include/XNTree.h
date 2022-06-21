/*************************************************************************/
/*	File : XNTree.h												 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef NTREE_H

#define NTREE_H

#include "XHashFun.h"

/************************************************
Summary: Class representation of a N tree.

Remarks:
	The tree can contains different types on its internal nodes and on
	its leaf. The typename N represent the internal node type, and the
	typename L the leaf type.

Example:

class MyTrav
{
public:
	void TraverseNode(const int& a)
	{
		if (a == 4)
			int b = 0;
	}

	void TraverseLeaf(const float& a)
	{
		if (a == 4.0f)
			int b = 0;
	}
} myTrav;

void foo()
{
	XNTree<int,float> tree;
	XNTree<int,float>::INode* node = tree.InsertNode(NULL,4);
	tree.InsertLeaf(node,4.0f);
	tree.InsertLeaf(node,5.0f);
	tree.InsertLeaf(node,6.0f);
    
	tree.Traverse(NULL,myTrav);

	XNTree<int,float>::Leaf* lnode = tree.FindLeaf(NULL,6.0f);
	XNTree<int,float>::INode* inode = tree.FindNode(NULL,4);
}



See Also : XBinaryTree
************************************************/
template <class N, class L,class EqN = XEqual<N>, class EqL = XEqual<L> >
class XNTree
{
public:

// Inner Classes

	// Generic Struct of a Node
	class INode;
	
	class Node {

	friend class XNTree<N,L,EqN,EqL>;

	public:

		// node type enumeration
		enum Type
		{
			NODE,
			LEAF
		};

		INode*
		GetParent() const
		{
			return parent;
		}

		bool
		IsLeaf() const
		{
			return (type == LEAF);
		}

		// Ctor
		Node(Type iType):type(iType) {}

	protected:
		INode*			parent;
		Type			type;
	};

	class INode : public Node {

	friend class XNTree<N,L,EqN,EqL>;

	INode():Node(Node::NODE) {}

	public:
		int
		GetChildrenCount() const
		{
			return children.Size();
		}

		Node*
		GetChildren(int i) const
		{
			assert(i < children.Size());
			return children[i];
		}

		N& GetData()
		{
			return data;
		}

	private:
		XArray<Node*>	children;
		N				data;
	};

	class Leaf : public Node {

	friend class XNTree<N,L,EqN,EqL>;

	public:
		Leaf():Node(Node::LEAF) {}

		L& GetData()
		{
			return data;
		}
	private:
		L				data;
	};

// Methods


	// Dtor
	~XNTree()
	{
		Clear();
	}

	// Return the roots count
	int	GetRootsCount() const
	{
		return m_Roots.Size();
	}

	// Return the i th root
	Node*	GetRoots(const int i) const
	{
		assert(i < m_Roots.Size());
		return m_Roots[i];
	}

	template <class Trav>
	void				Traverse(Node* iRoot, Trav& iTraversal) const
	{
		if (!iRoot) { // From the roots
			
			for (Node** it = m_Roots.Begin(); it != m_Roots.End(); ++it) {
				_Traverse(*it,iTraversal);
			}
		} else { // from a specific subtree root

			_Traverse(iRoot,iTraversal);
		}

	}

	// Insert an internal Node into a tree
	INode*				InsertNode(INode* iRoot, const N& iData)
	{
		// We create the node to insert
		INode* n = AllocateInternalNode();
		n->parent	= iRoot;
		n->data		= iData;

		_InsertNode(iRoot,n);
		return n;
	}

	// Insert a leaf into the tree
	Leaf*				InsertLeaf(INode* iRoot, const L& iData)
	{
		// We create the node to insert
		Leaf* n		= AllocateLeaf();
		n->parent	= iRoot;
		n->data		= iData;

		_InsertNode(iRoot,n);
		return n;
	}

	// Find a Node in the substree defined by iRoot
	INode*				FindNode(Node* iRoot, const N& iData) const
	{
		if (!iRoot) { // we start at the root
			for (Node** it = m_Roots.Begin(); it != m_Roots.End(); ++it) {
				INode* i = FindNode(*it,iData);
				if (i) 
					return i;
			}

			return NULL;
		}

		if (iRoot->type != Node::NODE) // Can't find a node on a leaf
			return NULL;
		
		INode* inode = (INode*)iRoot;

		// Is it me ?
		EqN equal;
		if (equal(iData,inode->data))
			return inode;

		// Not found, we continue to the chidren
		for (Node** it = inode->children.Begin(); it != inode->children.End(); ++it) {
			INode* i = FindNode(*it,iData);
			if (i)
				return i;
		}

		// Really not found on this subtree
		return NULL;
	}

	// Find a Node in the children of iRoot
	INode*				FindDirectNode(Node* iRoot, const N& iData) const
	{
		if(!iRoot)
		{
			for(Node** it = m_Roots.Begin(); it != m_Roots.End(); ++it)
			{
				if(!(*it)->IsLeaf() && ((INode*)*it)->GetData()==iData)
					return (INode*)*it;
			}

			return NULL;
		}

		if (iRoot->type != Node::NODE) // Can't find a node on a leaf
			return NULL;
		
		INode* inode = (INode*)iRoot;

		EqN equal;
		for (Node** it = inode->children.Begin(); it != inode->children.End(); ++it)
		{
			//if(!(*it)->IsLeaf() && ((INode*)*it)->GetData()==iData)
			if(!(*it)->IsLeaf() && equal(((INode*)*it)->GetData(),iData))
				return (INode*)*it;
		}

		return NULL;
	}
	

	// Find a Leaf in the substree defined by iRoot
	Leaf*				FindLeaf(Node* iRoot, const L& iData) const
	{
		if (!iRoot) { // we start at the root
			for (Node** it = m_Roots.Begin(); it != m_Roots.End(); ++it) {
				Leaf* l = FindLeaf(*it,iData);
				if (l) 
					return l;
			}

			return NULL;
		}

		if (iRoot->type == Node::LEAF) { // Is it me ?
			Leaf* leaf = (Leaf*)iRoot;
			
			EqL equal;
			if (equal(iData,leaf->data))
				return leaf;

		} else { // Try the children
			INode* inode = (INode*)iRoot;

			// Not found, we continue to the chidren
			for (Node** it = inode->children.Begin(); it != inode->children.End(); ++it) {
				Leaf* l = FindLeaf(*it,iData);
				if (l)
					return l;
			}
		}

		// Really not found on this subtree
		return NULL;
	}

	// Find a Leaf in the chilren of iRoot
	Leaf*				FindDirectLeaf(Node* iRoot, const L& iData) const
	{
		if (!iRoot) { // we start at the root
			for (Node** it = m_Roots.Begin(); it != m_Roots.End(); ++it) {
				if((*it)->IsLeaf() && ((Leaf*)*it)->GetData()==iData)
					return (Leaf*)*it;
			}

			return NULL;
		}

		if (iRoot->type != Node::NODE) // Can't find a node on a leaf
			return NULL;

		INode* inode = (INode*)iRoot;

		for (Node** it = inode->children.Begin(); it != inode->children.End(); ++it)
		{
			if((*it)->IsLeaf() && ((Leaf*)*it)->GetData()==iData)
				return (Leaf*)*it;
		}

		return NULL;
	}

	// Insert a leaf into the tree
	void				MoveNode(Node* iNode, INode* iRoot)
	{
		assert(iNode);
		assert(iRoot->type == Node::NODE);

		if (iNode->parent) { // it was a child of something
			iNode->parent->children.Remove(iNode);
		} else {
			m_Roots.Remove(iNode);
		}

		iNode->parent = iRoot;
		_InsertNode(iRoot,iNode);
	}

	// Extract a node from the tree
	void				ExtractNode(Node* iNode)
	{
		assert(iNode);

		if (iNode->parent) { // it was a child of something
			iNode->parent->children.Remove(iNode);

			// Remove the parent
			iNode->parent = NULL;
		} else {
			m_Roots.Remove(iNode);
		}

	}

	// Clear the entire tree
	void				Clear()
	{
		// we delete all the roots
		for (Node** n = m_Roots.Begin(); n != m_Roots.End(); ++n)
			_DeleteNode(*n);

		// We clear the array
		m_Roots.Resize(0);
	}

	void				DeleteSubTree(Node* iRoot)
	{
		if (!iRoot) return;

		// We clear the node from the children of its parent

		if (iRoot->parent) { // it was a child of something
			iRoot->parent->children.Remove(iRoot);
		} else {
			m_Roots.Remove(iRoot);
		}
		
		// we delete the node and all its subnode
		_DeleteNode(iRoot);
	}

private:

// Methods
	void				_InsertNode(INode* iRoot, Node* iNode)
	{
		if (!iRoot) { // we insert at the root of the tree
			m_Roots.PushBack(iNode);
		} else { // we insert under an existing node
			iRoot->children.PushBack(iNode);
		}
	}

	void				_DeleteNode(Node* iRoot)
	{
		
		// Delete the children, if any
		if (iRoot->type == Node::NODE) { // If we are an intenral Node
			INode* inode = (INode*)iRoot;
			for (Node** n = inode->children.Begin(); n != inode->children.End(); ++n)
				_DeleteNode(*n);

			// delete the node
			ReleaseInternalNode(inode);

		} else { // It's a leaf

			// delete the leaf
			ReleaseLeaf((Leaf*)iRoot);
		}

	}

	template <class Trav>
	void				_Traverse(Node* iRoot, Trav& iTraversal) const
	{
		assert(iRoot); // Should not be called with a NULL root

		if (iRoot->type == Node::NODE) { // Internal Node

			INode* inode = (INode*)iRoot;
			
			// The processing function call
			iTraversal.TraverseNode(inode->data);

			// continue to the children
			for (Node** it = inode->children.Begin(); it != inode->children.End(); ++it)
				_Traverse(*it,iTraversal);

			// The processing function call
			iTraversal.ExitNode(inode->data);

		} else { // leaf

			iTraversal.TraverseLeaf(((Leaf*)iRoot)->data);
		}
	}

	INode*				AllocateInternalNode()
	{
		return new INode;
	}
	
	Leaf*				AllocateLeaf()
	{
		return new Leaf;
	}

	void				ReleaseInternalNode(INode* iNode)
	{
		delete iNode;
	}
	
	void				ReleaseLeaf(Leaf* iLeaf)
	{
		delete iLeaf;
	}

// Members
	XArray<Node*>		m_Roots;
};


#endif // NTREE_H