
#ifndef XPRIORITYQUEUE_H
#define XPRIORITYQUEUE_H

template <class T>

struct XPriority
{
  int operator()(const T& iT) const { 
	  return iT; 
  }
};

/************************************************
Name: XPriorityQueue

Summary: Class representation of a priority queue.

Template Parameters:
	T : type of object to store into the queue
	PF : priority function, which is use to tell the priority (an int) 
	of a T object.

Remarks:
  This container is used to fastly pop an object, sorted by a criterion, called here the
  priority of the object. Insertion and Pop are in O(log(n)) and the objects are stored
  flat, in an array, avoiding many small allocations.
  You can not store objects with ctor and dtor in it because they won't be called.



See Also : XArray, XList
************************************************/
template <class T, class PF = XPriority<T> >
class XPriorityQueue
{
public:
	/************************************************
	Summary: Constructors.

	Input Arguments: 
		iBaseNumber: Default number of reserved elements.
	************************************************/
	XPriorityQueue(int iBaseNumber = 0):m_Cells(iBaseNumber) {}

	/************************************************
	Summary: Removes all the elements from the queue.

	Remarks:
		The memory allocated is not freed by this call.
	************************************************/
	void Clear()
	{
		if (m_Cells.Size()) { // already allocated : sets to the beginning
			m_Cells.Resize(1);
		}

	}

	void Insert(const T& iT)
	{
		if (!m_Cells.Size()) { // first insertion : init
			m_Cells.Reserve(2);
			m_Cells.Resize(1); // the first node is the root
		}

		int i = m_Cells.Size();
		m_Cells.PushBack(iT);
		
		T* cells = m_Cells.Begin();

		PF prio;
		int insertPrio = prio(iT);
		while (i > 1 && prio(cells[i / 2]) < insertPrio) {
			cells[i] = cells[i / 2];
			i /= 2;
		}
		cells[i] = iT;
	}
	
	/************************************************
	Summary: Removes the higher priority element of 
	the queue.

	Input Arguments: 
		oT: pointer to the T object that will be filled
		with the higher priority element.
		The pointer need to be valid.

	Return Value: TRUE if an object is popped.
	************************************************/
	XBOOL
	Pop(T* oT)
	{
		if (m_Cells.Size() <= 1)
			return 0;
		
		{
			T* cells = m_Cells.Begin();
			
			*oT = cells[1];
		}

		T tmp = m_Cells.PopBack();
		int size = m_Cells.Size();
		if (size == 1) // was the last one
			return 1;

		int i = 1;
		int j;


		PF prio;
		int lastPriority = prio(tmp);

		T* cells = m_Cells.Begin();

		while (i <= size / 2) {
			j = 2 * i;
			if (j < size && 
				prio(cells[j]) < prio(cells[j + 1])) {
				j++;
			}
			if (prio(cells[j]) <= lastPriority) {
				break;
			}
			cells[i] = cells[j];
			i = j;
		}
		
		cells[i] = tmp;
		return 1;	
	}

	/************************************************
	Summary: Peeks the higher priority element of 
	the queue.

	Input Arguments: 
		oT: pointer to the T object that will be filled
		with the higher priority element.
		The pointer need to be valid.

	Return Value: TRUE if an object is popped.
	************************************************/
	XBOOL
	Peek(T* oT) const
	{
		if (m_Cells.Size() <= 1)
			return 0;
		
		T* cells = m_Cells.Begin();

		*oT = cells[1];
		return 1;
	}


	/************************************************
	Summary: Returns the occupied size in memory in bytes
	
	Parameters:
		addstatic: TRUE if you want to add the size occupied
	by the class itself.
	************************************************/
	int GetMemoryOccupation(XBOOL iAddStatic = FALSE) const 
	{
		return m_Cells.GetMemoryOccupation(iAddStatic);
	}
	

protected:
	// array of queue cells
	XArray<T>	m_Cells;
};

#endif