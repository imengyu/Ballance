#pragma once

#include "XArray.h"
#include <cassert>

template <class T>
class XDictionnary
{
	// Internal nodes
	struct Node
	{
		Node():m_Flag(0),m_Parent(0)
		{}

		~Node()
		{
			if (m_Flag & SUFFIX) {
				delete [] Suffix.data;
				Suffix.data = NULL;
			} else {
				delete [] Children.data;
				Children.data = NULL;
			}
		}

		bool	HasChildren() {return !(m_Flag&SUFFIX);}

		void	Display(const T& iOffset)
		{
			printf("Symbol : %c\n",m_Symbol);
			if (m_Flag & SUFFIX) {
				if (Suffix.count > 0) {
					printf("Suffix : ");
					for (int i=0; i<Suffix.count; ++i) printf("%c",Suffix.data[i]);
					printf("\n");
				}
			} else {
				printf("Children : %d / Termination : %c\n",Children.count,(m_Flag&TERMINATION)?'Y':'N');
				int count = Children.count;
                int i = 0;
				while (count) {
                    Node* n = Children.data[i++];
					if (n) {
						--count;
						n->Display(iOffset);
					}
				}
			}
		}

		enum FLAGS {
			TERMINATION = 1,
			SUFFIX		= 2
		};

		void	FreeChildren()
		{
			delete [] Children.data;
			Children.data = NULL;

			Children.count = 0;
		}

		void	FreeSuffix()
		{
			delete [] Suffix.data;
			Suffix.data = NULL;

			Suffix.count = 0;
		}

		int		m_Flag;
		T		m_Symbol;
		Node*	m_Parent;
		void*	m_UserData;

		union {
			struct {
				T*		data;  
				int		count;
			} Suffix;
			struct {
				Node**	data;  
				int		count;
			} Children;
		};
	};
public:
	XDictionnary(const T& iMin, const T& iMax):m_SymbolOffset(iMin),m_SymbolCount(iMax-iMin+1)
	{
		// Creation of the root node
		m_Root = CreateNode(T(),NULL);
	}

	~XDictionnary(void)
	{
		DestroyNode(m_Root);
	}

	void	Clear()
	{
		DestroyNode(m_Root);
		m_Root = CreateNode(T());
	}

	// Insert a sequence into the dictionnary
	bool	InsertSequence(const T* iSequence, const int iCount,void* iUserData = NULL,bool iReplace=false)
	{
		return _InsertSequence(m_Root,iSequence,iCount,iUserData,iReplace);
	}

	void	RemoveSequence(const T* iSequence, const int iCount)
	{
		_RemoveSequence(m_Root,iSequence,iCount);
	}

	void	Display()
	{
		m_Root->Display(m_SymbolOffset);
	}

	void	CheckConsistency()
	{
		_CheckConsistency(m_Root);
	}

	///
	// Iterator on sequences starting with a given sequence
	class Iterator
	{
		friend class XDictionnary<T>;
	public:
		// default Ctor
		Iterator():m_Dictionnary(NULL),m_InitialDepth(0),m_Current(NULL),m_SuffixToGo(false)	{}

		Iterator(XDictionnary* iD, Node* iNode, const T* iSequence, const int iCount, const int iInitialCount):m_Dictionnary(iD),m_Current(iNode),m_SuffixToGo(false)
		{
			// Recopy the sequence
			m_Sequence.Resize(iInitialCount);
			for (int i=0; i<iInitialCount; ++i) {
				m_Sequence[i] = iSequence[i];
			}

			// Initial size
			m_InitialDepth = iInitialCount;

			m_Current = FillFirstSequence(m_Current);		

		}


		const XArray<T>& operator * ()
		{
			return m_Sequence;
		}

		bool operator == (const Iterator& iT)
		{
			return !(*this != iT);
		}

		bool operator != (const Iterator& iT)
		{
			// TODO : the real thing
			//return (m_Sequence.Size() != iT.m_Sequence.Size());
			return (m_Current != iT.m_Current);
		}

		 // Prefixe ++ Operator
		Iterator& operator++() 
		{
			// we first descend the tree
			Node* n = m_Current;
	
			if (m_SuffixToGo) {
				m_SuffixToGo = false;
				
				for (int j=0; j<n->Suffix.count; ++j) {
					m_Sequence.PushBack(n->Suffix.data[j]);
				}

                return *this;
			}

			// there s more under this branch
			if (!(n->m_Flag & Node::SUFFIX)) {
				
				for (;;) {
					Node* child = m_Dictionnary->GetFirstChild(n);
					assert(child);
					
					m_Sequence.PushBack(child->m_Symbol);
					
					n = child;

					// we now fill the string
					if (child->m_Flag & Node::SUFFIX) {
						// add the rest of the suffixe
						for (int j=0; j<child->Suffix.count; ++j) {
							m_Sequence.PushBack(child->Suffix.data[j]);
						}
						
						break;
					}
					

					if (n->m_Flag & Node::TERMINATION) {
						break;
					}
				}
			} else { // it s a leaf
				while (true) {
					Node* parent = n->m_Parent;
					// we pop back the node symbol
					m_Sequence.PopBack();
					// we popback the suffixes too
					if (n->m_Flag & Node::SUFFIX) {
						for (int i=0; i<n->Suffix.count; ++i) {
							m_Sequence.PopBack();
						}
					}


					// we went too high, exit there
					if (m_Sequence.Size() < m_InitialDepth) {
						// warning dependent of the != test
						m_Current = NULL;
						return *this;
					}

					Node* child = m_Dictionnary->GetNextChild(parent,n->m_Symbol);
					if (child) {
						// add the symbol of the child
						m_Sequence.PushBack(child->m_Symbol);

						m_Current = FillFirstSequence(child);
						return *this;

					} else {
						// we continue the going up
						n = parent;
					}
				}
			}

			m_Current = n;
			return *this;
		}
		
		Iterator operator++(int) { 
			Iterator tmp = *this;
			++*this;
			return tmp;
		}

		void*		GetUserData()
		{
			return m_Current->m_UserData;
		}

	protected:
		
		Node*				FillFirstSequence(Node* iNode)
		{
			Node* n = iNode;
			while (n->HasChildren()) {
				if (n->m_Flag & Node::TERMINATION) break;
				
				// there must be children
				assert(n->Children.count);

				Node* child = NULL;
				for (int i=0; i<m_Dictionnary->m_SymbolCount; ++i) {
					child		= n->Children.data[i];
					if (child)	break;
				}

				assert(child);

				// we a_d the symbol of the child
				m_Sequence.PushBack(child->m_Symbol);

				// we continue the descent
				n = child;				

			}

			if (!(n->m_Flag & Node::TERMINATION)) { // if the node is not a termination, we add its suffix
				// we now add the suffix
				if (n->m_Flag & Node::SUFFIX) {
					for (int j=0; j<n->Suffix.count; ++j) {
						m_Sequence.PushBack(n->Suffix.data[j]);
					}
				}
			} else {
				if ((n->m_Flag & Node::SUFFIX) && n->Suffix.count) m_SuffixToGo = true;
			}

			return n;
		}

		XDictionnary*		m_Dictionnary;
		int					m_InitialDepth;
		bool				m_SuffixToGo;
		XArray<T>			m_Sequence;
		Node*				m_Current;
	};

	// Friendship
	friend class Iterator;

	Iterator		Begin(const T* iSequence, const int iCount)
	{
		return First(iSequence,iCount);
	}

	Iterator		End()
	{
		return Iterator();
	}

	Iterator		First(const T* iSequence, const int iCount)
	{
		int count	= iCount;

 		Node* n		= FindNode(iSequence,count);
		if (!n) return End();
		//if (!n->HasChildren() && !n->Suffix.count) return End();

		// Create the iterator
		Iterator it(this,n,iSequence,iCount,count);

		return it;

		/*
		Node* n = m_Root;

		// create the iterator
		Iterator it = Iterator(this,iSequence,iCount);

		for (int i=0; i<iCount; ++i) {
			const T& symbol = iSequence[i];
		
			// This a final node
			if (n->m_Flag & Node::SUFFIX) {
				// add the rest of the suffixe
				for (int j=iCount-i; j<n->Suffix.count; ++j) {
					it.m_Sequence.PushBack(n->Suffix.data[j]);
				}

				return it;
			}

			n = n->Children.data[symbol - m_SymbolOffset];
			
			// This a branch has no child
			if (!n) return End();
		}


		if (n->m_Flag & Node::TERMINATION) { // This node is a valid sequence (the sequence given is inside)
			return it;
		}

		// we search for a longer string
		while (true) {
			if (n->m_Flag & Node::SUFFIX) {

				//it.m_Sequence.PushBack(n->m_Symbol);

				// add the rest of the suffixe
				for (int j=0; j<n->Suffix.count; ++j) {
					it.m_Sequence.PushBack(n->Suffix.data[j]);
				}
				break;

			} else {
				// search for the first child
				int i = 0;
				for (i=0; i<m_SymbolCount; ++i) {
					Node* nn = n->Children.data[i];
					if (nn) break;
				}

				// No children found
				if (i == m_SymbolCount) break;

				it.m_Sequence.PushBack(n->Children.data[i]->m_Symbol);
				n = n->Children.data[i];
			}
		}

		return it;
		*/
	}

private:

	Node*	FindNode(const T* iSequence, int& ioCount)
	{
		if (m_Root->m_Flag & Node::SUFFIX) // the dictionnary is empty : exit
			return NULL;

		Node* n = m_Root;

		for (int i=0; i<ioCount; ++i) {
			const T& symbol = iSequence[i];
		
			// discard the non awaited symbol
			if ((symbol < m_SymbolOffset) || (symbol > m_SymbolOffset + m_SymbolCount)) 
				return NULL;
			
			// we retreive the symbol node
			n = n->Children.data[symbol - m_SymbolOffset];

			// This a branch has no child
			if (!n) return NULL;

			// This a final node
			if (n->m_Flag & Node::SUFFIX) {
				
				// we have to test if the suffix is compatible with the sequence
				int next = i+1;

				// first we test if the suffix can contain the rest of the sequence
				if (n->Suffix.count < (ioCount-next)) 
					return NULL;
				
				// then we test if it s compatible
				for (int j=next; j<ioCount; ++j) {
					if (iSequence[j] != n->Suffix.data[j-next]) 
						return NULL;
				}

				ioCount = next;

				return n;
			}

			// this is a termination
			if (n->m_Flag & Node::TERMINATION) {
				
				if (i == (ioCount-1)) // this the sequence we were looking for
					return n;

			}
			
		}

		return n;
				
	}

	Node*	GetNextChild(Node* iNode, int iSymbol)
	{
		if (iNode->m_Flag & Node::SUFFIX) {
			return NULL;
		} else {
			// There must be children
			if (iNode->Children.count > 1) { // need to child for having a brother
				for (int i=iSymbol+1-m_SymbolOffset; i<m_SymbolCount; ++i) {
					Node* n = iNode->Children.data[i];
					if (n)	return n;
				}
			}
		}

		return NULL;
	}

	Node*	GetFirstChild(Node* iNode)
	{
		if (iNode->m_Flag & Node::SUFFIX) {
			return NULL;
		} else {
			// There must be children
			assert(iNode->Children.count > 0) ;

			for (int i=0; i<m_SymbolCount; ++i) {
				Node* n = iNode->Children.data[i];
				if (n)	return n;
			}
		}

		return NULL;
	}

	int		GetTerminationCount(Node* iNode)
	{
		if (iNode->m_Flag & Node::SUFFIX) {
			return 1;
		} else {
			// There must be children
			assert(iNode->Children.count > 0) ;

			int childcount = 0;
			
			// the current character is valid
			if (iNode->m_Flag & Node::TERMINATION)
				childcount++;

			for (int i=0; i<m_SymbolCount; ++i) {
				Node* n = iNode->Children.data[i];
				if (n)	
					childcount += GetTerminationCount(n);
			}
			return childcount;
		}

	}

	// Node Creation
	Node* CreateNode(const T& iSymbol,void* iUserData=NULL)
	{
		Node* node				= new Node;
		node->m_Flag			|= Node::SUFFIX;
		
		node->m_UserData		= iUserData;
		
		node->Suffix.data		= NULL;
		node->Suffix.count		= 0;

		node->m_Symbol			= iSymbol;

		return node;
	}

	void	DestroyNode(Node* iNode)
	{
		if (!(iNode->m_Flag & Node::SUFFIX)) { // destroy the children
			
			int count = iNode->Children.count;

			for (int i=0; (i<m_SymbolCount) && count; ++i) {
				Node* child = iNode->Children.data[i];
				if (child) {
					--count; // child suppressed
					DestroyNode(child);
				}
			}
		}

        delete iNode;            
	}

	void	CreateChildren(Node* iNode)
	{
		if (iNode->m_Flag & Node::SUFFIX) { // we transform a final node with suffix in an internal node with children

			iNode->m_Flag &= ~Node::SUFFIX;

			// create the children array
			Node** children		= new Node*[m_SymbolCount];
			int childrencount	= 0;
			memset(children,0,m_SymbolCount*sizeof(Node*));

			// there is a suffix
			if (iNode->Suffix.count > 0) {
			
				Node* n  = CreateNode(iNode->Suffix.data[0],iNode->m_UserData);

				if (iNode->Suffix.count > 1) {
					n->Suffix.count = iNode->Suffix.count-1;
					n->Suffix.data	= new T[n->Suffix.count];

					// recopy the rest of suffix
					for (int i=0; i<n->Suffix.count; ++i) {
						n->Suffix.data[i] = iNode->Suffix.data[1+i];
					}
				}

				// we delete the old suffixes data
				delete [] iNode->Suffix.data;
				iNode->Suffix.data	= NULL;
				iNode->m_UserData	= NULL;

				// we store the new child
				n->m_Parent								= iNode;
				children[n->m_Symbol-m_SymbolOffset]	= n;
				childrencount							= 1;
			} else {

				if (iNode->m_Parent) // Don't make the root a termination !
					iNode->m_Flag |= Node::TERMINATION;
			}
			
			iNode->Children.data	= children;
			iNode->Children.count	= childrencount;

		} // else nothing to do, already internal
	}

	bool _InsertSequence(Node* iNode, const T* iSequence, const int iCount,void* iUserData,bool iReplace)
	{
		Node* n = iNode;

		// We iterate on the sequence
		for (int i=0; i<iCount; ++i) {
            
			const T& symbol = iSequence[i];
			bool terminal = (i == (iCount-1));

			// we prepare the current node to receive the sequence
			CreateChildren(n);

			// we check if the path already exist
			int index = symbol-m_SymbolOffset;
			if (index<0)
			{
				index=0;
			}
			Node*& child = n->Children.data[index];

			if (child) { // yes
				if (terminal) {
				
					// If you hit this assert, that means that the sequence 
					// was already inside the dictionnary : you're going to
					// step over the user data
					//XASSERT(!(child->m_Flag&Node::TERMINATION));
					
					if (child->m_Flag & Node::SUFFIX) { 
						
						// same problem as above
						if (child->Suffix.count == 0) {
							// XASSERT(0);

							//Thomas : je suis vraiment pas sur du tout que ce soit ca a faire sur un replace
							if (iReplace)
							{
								child->m_Flag	|= Node::TERMINATION;
								child->m_UserData = iUserData;
								return true;
							}
							return false;
						}
						
						// there is already a suffix on this node
						// we have to make it go down
						CreateChildren(child);
					}
					
					child->m_Flag	|= Node::TERMINATION;
					child->m_UserData = iUserData;
				} else {
					n = child;
				}
			} else {
				// we create the node
				Node* nn = CreateNode(symbol,iUserData);
				
				// we add the child
				nn->m_Parent	= n;
				child			= nn;
				n->Children.count++;
				
				if (!terminal) {
					// we store the suffix and stop there
					nn->m_Flag |= Node::SUFFIX;
					
					nn->Suffix.count = iCount-(i+1);
					nn->Suffix.data = new T[nn->Suffix.count];
					
					// we fill the suffix
					for (int j=0; j<nn->Suffix.count; ++j) {
						nn->Suffix.data[j] = iSequence[i+1+j];
					}

					break;
				}
			}
		}
		return true;
	}

	// Return 1 if the given node is to be deleted too
	int _RemoveSequence(Node* iNode, const T* iSequence, const int iCount)
	{
		// 2 solutions : the node is a suffix and is the sequence searched
		// or it s an internal node and one of it's children contains
		// what we re looking for.

		if(!iNode)
			return 0;

		if (iCount && iNode->HasChildren()) {

			const T& symbol = iSequence[0];

			Node*& child = iNode->Children.data[symbol-m_SymbolOffset];

			if (_RemoveSequence(child,iSequence+1,iCount-1)) { // my child is no good anymore
				DestroyNode(child);
				child = NULL;
				
				iNode->Children.count--;
			}

			// If no more children, my parent can destroy me
			return (iNode->Children.count == 0);

		} else { // Suffix
			
			// we check for termination
			if (!iCount) {

				if (iNode->Suffix.count) { // more suffix inside

					// Must be marked as a termination then
					XASSERT(iNode->m_Flag & Node::TERMINATION);

					iNode->m_Flag &= ~Node::TERMINATION;
					return 0; // Not any more a termination but still a valid suffix
				} else {
					
					// No suffix other than the one searched
					// this node is not relevant anymore

					return 1;
				}
			} else {
				// we need to check if the string we search matches the string of the suffix

				// first the size
				XASSERT(iNode->Suffix.count == iCount);
				
				// then the content
				for (int i=0; i<iCount; ++i) {
					XASSERT(iNode->Suffix.data[i] == iSequence[i]);
				}

				if (iNode->m_Flag & Node::TERMINATION) { // the node was also a termination

					// so we can not delete it, but still free its suffix memory
					iNode->FreeSuffix();

					return 0; // Can not destroy it
				} else {

					// can freely destroy

					return 1;

				}


			}

		}
	}
	

	void	_CheckConsistency(Node* iNode)
	{
		XASSERT(iNode);

		XASSERT(!iNode->m_Symbol || (iNode->m_Symbol >= m_SymbolOffset));
		XASSERT(iNode->m_Symbol < (m_SymbolOffset+m_SymbolCount));
		

		if (iNode->HasChildren()) { // It's an internal node
			
			int childrenCount = iNode->Children.count;

			XASSERT(childrenCount > 0);
			XASSERT(childrenCount < m_SymbolCount);

			int childrenFound = 0;

			
			for (int i=0; i<m_SymbolCount; ++i) {
				Node* child = iNode->Children.data[i];
				if (child) {
					++childrenFound;

					XASSERT(childrenFound <= childrenCount);

					XASSERT(child->m_Parent == iNode);
					XASSERT(child->m_Symbol == i+m_SymbolOffset);
					
					_CheckConsistency(child);

				}
			}

			XASSERT(childrenCount == childrenFound);

		} else { // it's only a suffix

			// can't be a termination too ?
			//XASSERT(!(iNode->m_Flag & Node::TERMINATION));
			
			int suffixCount = iNode->Suffix.count;

			XASSERT(suffixCount >= 0);

			for (int i=0; i<suffixCount; ++i) {

				T s = iNode->Suffix.data[i];

				XASSERT(s >= m_SymbolOffset);
				XASSERT(s < (m_SymbolOffset+m_SymbolCount));
			}

		}
	}

	///
	// Members

	// Offset to subtract from the given symbols to store in the symbol table
	T		m_SymbolOffset;
	// Size of the symbol table
	T		m_SymbolCount;
	// The root node
	Node*	m_Root;
};


class XCharDictionnary : public XDictionnary<char>
{
public:
	XCharDictionnary():XDictionnary<char>(' ','}') {}
};

