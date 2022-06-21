#ifndef _XSARRAY_OPT_H_
#define _XSARRAY_OPT_H_


#define SZ_ONE_ELEMENT(a) (sizeof(T)==sizeof(T*) && (a <= 1)) 
#define SET_DATA(a) ( (*(T*)&m_Data) = a)

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
		m_Data = NULL;
		m_Size = 0;
	}

	
	XSArray(const XSArray<T>& a)
	{
		// the resize
		int size = a.Size();
		if(SZ_ONE_ELEMENT(size)){
			m_Data = a.m_Data;
		}else{
			m_Data = Allocate(size);
			// The copy
			XCopy(m_Data,a.Begin(),a.End());			
		}
		m_Size = size;		
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
			if(SZ_ONE_ELEMENT(size)){
				m_Data = a.m_Data;
			}else{
				m_Data = Allocate(size);
				// The copy
				XCopy(m_Data,a.Begin(),a.End());				
			}
			m_Size = size;				
		}

		return *this;
	}

	
	XSArray<T>& operator += (const XSArray<T>& a)
	{
		int size = a.Size();
		if (size) {
			int oldsize = Size();
			if(SZ_ONE_ELEMENT(oldsize+size)){
				// Empty += Single Element
				m_Data = a.m_Data;				
			}else{
				T* temp = Allocate(oldsize+size);
				// we recopy the old array
				XCopy(temp,Begin(),End());				
				// we copy the given array
				XCopy(temp+oldsize,a.Begin(),a.End());

				// we free the old memory
				Free();
				// we set the new pointer
				m_Data = temp;
			}
			m_Size = oldsize+size;
		}

		return *this;
	}

	
	XSArray<T>& operator -= (const XSArray<T>& a)
	{
		int size = a.Size();
		if (size) {
			int oldsize = Size();
			if (oldsize) {
				if(SZ_ONE_ELEMENT(oldsize)){
					T* it = Begin();
					if(a.Find(*it) != a.End()) {
						// Our only Item is in the array 
						// we clear the array
						Clear();
					}				
				}else{

					T* newarray = Allocate(oldsize);
					T* temp = newarray;

					for(T* t = Begin(); t != End(); ++t) {
						// we search for the element in the other array
						T* iEnd = a.End();
						if(a.Find(*t) == iEnd) {
							// the element is not in the other array, we copy it to the new one
							*temp = *t;
							++temp;
						}
					}

					// we free the memory
					Free();
					// the resize
					int size = temp - newarray;
					if(size){
						if(SZ_ONE_ELEMENT(size)){
							SET_DATA(*newarray);
						}else{
							m_Data = Allocate(size);
							// The copy
							XCopy(m_Data,newarray,temp);
						}
						m_Size = size;
					}else{
						m_Data = NULL;
						m_Size = 0;
					}
					XFree(newarray);
				}
			}
		}

		return *this;
	}

	
	void Clear()
	{
		Free();		
	}

	
	void Fill(const T& o)
	{
		T* iEnd = End();
		for(T* t = Begin(); t !=iEnd; ++t)
			*t=o;
	}

	
	void Resize(int size)
	{
		// No change ?
		if (size == Size()) 
			return;

		if(0 == size){
			Clear();
			return;
		}

		if(SZ_ONE_ELEMENT(size)){
			// the New size is 1
			// And the array was not of size 1 already
			if(m_Size){
				T dat = m_Data[0];
				SET_DATA(dat);
			}else{
				(int)m_Data = 0xcdcdcdcd;
			}
			m_Size = 1;
		}else{
			// the new data does not fit in one slot
			// [0..N] -> N
			// allocation of new size
			T* newdata = Allocate(size);

			// Recopy of old elements
			T* last = XMin(Begin()+size,End());
			XCopy(newdata,Begin(),last);

			// new Pointers
			Free();
			m_Data = newdata;			
		}
		m_Size = size;
	}

	
	void PushBack(const T& o)
	{
		XInsert(End(),o);
	}

	
	void PushFront(const T& o)
	{
		XInsert(Begin(),o);
	}

	
	void Insert(T* i, const T& o)
	{
		// TODO : s'assurer que i est dans les limites...
		if(i<Begin() || i>End())
			return;

		// The Call
		XInsert(i,o);
	}

	
	void Insert(int pos, const T& o)
	{
		Insert(Begin()+pos,o);
	}

	
	void Move(T* i, T* n)
	{
		// TODO : s'assurer que i est dans les limites...
		if(i<Begin() || i>End()) return;
		if(n<Begin() || n>End()) return;

		// The Call
		int insertpos = i - Begin();
		if (n < i) 
			--insertpos;

		T tn = *n;
		XRemove(n);
		Insert(insertpos,tn);
	}

	
	void PopBack()
	{
		// we remove the last element only if it exists
		if(Size())	
			XRemove(End()-1);
	}

	
	void PopFront()
	{
		// we remove the first element only if it exists
		if(Size()) 
			XRemove(Begin());
	}

	
	T* Remove(T* i)
	{
		// we ensure i is in boundary...
		if(i<Begin() || i>=End())
			return 0;
		// the Call
		return XRemove(i);
	}

	
	XBOOL RemoveAt(unsigned int pos,T& old)
	{
		T* t = Begin()+pos;
		// we ensure i is in boundary...
		if(t >= End()) return 0;
		old = *t; // we keep the old value
		// the removal
		XRemove(t);
		return TRUE; // really removed
	}

	
	T* RemoveAt(int pos)
	{
		// we ensure i is in boundary...
		if((pos < 0) || (Begin()+pos >= End())) return 0;

		// the Call
		return XRemove(Begin()+pos);
	}

	
	int Remove(const T& o)
	{
		for(T* t = Begin(); t != End(); ++t) {
			if(*t == o) {
				XRemove(t);
				return 1;
			}
		}
		return 0;
	}

	
	T& operator [](unsigned int i) const
	{
		return *(Begin()+i);
	}

	
	T* At(unsigned int i)  const
	{
		return Begin()+i;
	}

	
	T* Find(const T& o)  const
	{
		T* tEnd = End();
		for(T* t = Begin(); t != tEnd; ++t) {
			if(*t == o) return t;
		}
		return tEnd;
	}

	
	XBOOL IsHere(const T& o) const
	{
		T* t = Begin();
		T* tEnd = End();
		while(t < tEnd && *t != o) ++t;					

		return (t != tEnd);
	}

	
	int GetPosition(const T& o) const
	{
		T* tEnd = End();
		for(T* t = Begin(); t != tEnd; ++t) {
			if(*t == o) 
				return (t-Begin());
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
		if(sizeof(T) == sizeof(void*)){
			T* t1 = Begin()+pos1;
			T* t2 = Begin()+pos2;
			T temp = *t1;
			*t1=*t2;
			*t2 = temp;
		}else{
			char buffer[sizeof(T)];
			memcpy(buffer,Begin()+pos1,sizeof(T));
			memcpy(Begin()+pos1,Begin()+pos2,sizeof(T));
			memcpy(Begin()+pos2,buffer,sizeof(T));
		}
	}

	
	void Swap(XSArray<T>& a)
	{
		XSwap(m_Data,a.m_Data);
		XSwap(m_Size,a.m_Size);
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
		if (Size() > 1) qsort(Begin(),Size(),sizeof(T),compare);
	}

	
	void BubbleSort( VxSortFunc compare = XCompare)
	{
		if	(!compare) return;
		if  ((Size())<=1) return;

		BOOL Noswap=TRUE;

		T* iEnd0 = End();
		T* iEnd1 = End()-1;

		for (T* it1=Begin()+1;it1<iEnd0;it1++)	{
			for (T* it2=iEnd1;it2>=it1;it2--)	 {
				T* t2= it2-1;
				if (compare(it2,t2) < 0)  { 
					XSwap(*it2,*t2);
					Noswap=FALSE;
				}
			}
			if (Noswap) 
				break;
			Noswap=TRUE;
		}
	}

	
	T* Begin() const {
		if(SZ_ONE_ELEMENT(m_Size))
			return m_Size ? (T*) &m_Data : NULL;
		return m_Data;
	}

	
	T* End()  const {
		if(SZ_ONE_ELEMENT(m_Size)){
			return m_Size ? ((T*)&m_Data)+1 : 0;
		}
		return m_Data+m_Size;
	}

	
	int Size() const {
		return m_Size;
	}

	
	int GetMemoryOccupation(XBOOL addstatic=FALSE) const {
		if(SZ_ONE_ELEMENT(m_Size)){
			return addstatic?sizeof(this):0;
		}
		return Size()*sizeof(T)+addstatic?sizeof(this):0;
	}

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
		int newsize = Size()+1;
		if(SZ_ONE_ELEMENT(newsize)){
			//0->1
			m_Size = newsize;
			SET_DATA(o);
		}else{
			//[1..N]->[2..N+1] 
			T* newdata = Allocate(newsize);
			// copy before insertion point
			XCopy(newdata,Begin(),i);

			// copy the new element
			T* insertionpoint = newdata+(i-Begin());
			*(insertionpoint) = o;

			// copy after insertion point
			XCopy(insertionpoint+1,i,End());

			// New Pointers
			Free();
			m_Size = newsize;
			m_Data = newdata;
		}
	}

	
	T* XRemove(T* i)
	{
		if(0 == m_Size)
			return i;

		int newsize = m_Size-1;

		if(SZ_ONE_ELEMENT(newsize)){
			if(newsize){
				// [2]->[1]
				T data;
				if(Begin() == i){
					data = *Begin();
				}else{
					data = *End();
				}
				Free();
				m_Size = newsize;
				SET_DATA(data);
			}else{
				// 1->0
				Free();
			}
		}else{
			T* newdata = Allocate(newsize);

			// copy before insertion point
			XCopy(newdata,Begin(),i);

			// copy after insertion point
			T* deletionpoint = newdata+(i-Begin());
			XCopy(deletionpoint,i+1,End());
			i = deletionpoint;

			// New Pointers
			Free();
			m_Size = newsize;
			m_Data = newdata;
		}
		return i;
	}

	///
	// Allocation and deallocation methods : to override for alignment purposes

	
	T* Allocate(int size)
	{
		if(size) 
			return (T*) XAllocate(sizeof(T)*size);
		else{
			return 0;
		}
	}

	
	void Free()	{
		if(!SZ_ONE_ELEMENT(m_Size)){			
			XFree(m_Data);
		}
		m_Size = 0;
		m_Data = NULL;
	}

	
	void XFree(T* data){
		free(data);
	}

	
	void* XAllocate(unsigned int size){
		return malloc(size);
	}

	// Members
private:
	
	T*			m_Data;
	
	unsigned int m_Size;
};

#endif