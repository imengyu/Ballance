#ifndef VXMUTEX_H
#define VXMUTEX_H

#include "VxMathDefines.h"

#if defined(_LINUX) || defined(macintosh)
	struct CMutexData;
#endif

/*************************************************
Summary: Represent an objet used for mutual-exclusion synchronization
between threads.

See also: VxThread,VxMutexLock
*************************************************/
class VxMutex 
{

public:


VX_EXPORT	VxMutex();
VX_EXPORT	~VxMutex();
    

VX_EXPORT	BOOL EnterMutex();
VX_EXPORT	BOOL LeaveMutex();

VX_EXPORT	BOOL operator++(int) {return EnterMutex();}
VX_EXPORT	BOOL operator--(int) {return LeaveMutex();}


private:

#if defined(_LINUX) || defined(macintosh)
	CMutexData*	m_MutexData;
#else
	
	void* m_Mutex;
#endif


	
	VxMutex(const VxMutex&);
	
	VxMutex& operator=(const VxMutex&);

};

/*************************************************
Summary: Represent an objet used for mutual-exclusion synchronization
between threads.

See also: VxThread,VxMutex
*************************************************/
class VxMutexLock
{
	VxMutex& m_Mutex;
public:
	VxMutexLock(VxMutex& Mutex):m_Mutex(Mutex) {m_Mutex++;}
	~VxMutexLock() {m_Mutex--;}
};

/*************************************************
Summary: This class ensure that a data is access by only one thread.

Remarks:
To ensure that the access to your data is thread safe you need
to create the VxDataMutexed. Then, if a thread want to modify or do anything on it,
you create its accessor in a scope. You can modify the value using the Value on the accessor.

Example

  VxDataMutexed<int> val;
  {
    VxDataMutexed<int>::Accessor acces(val);
    acces.Value() = 1;
  }


See also: VxThread,VxMutexLock,VxMutex
*************************************************/
template <class T> class VxDataMutexed
{
public:

	/*************************************************
	Summary: Default constructor. Nothing is done.
	*************************************************/
	VxDataMutexed()
	{
	}

	/*************************************************
	Summary: Initialization constructor. The internal value is intialized.
	Remarks: The type associated with "T" must have an affectation operator.
	*************************************************/
	VxDataMutexed(const T &value)
	{
		m_Value = value;
	}

	/*************************************************
	Summary: This class give you a thread safe access to the VxDataMutexed Value. Look the example in the VxDataMutexed.
	*************************************************/
	class Accessor
	{
	public:

		/*************************************************
		Summary: Construct an accessor on your data. The data is locked by its mutex.
		*************************************************/
		Accessor(VxDataMutexed<T> *dm)
		{
			m_DataM = dm;
			m_DataM->m_Mutex.EnterMutex();
		}

		/*************************************************
		Summary: Destruct the accessor on your data. The data is unlocked by its mutex.
		*************************************************/
		~Accessor()
		{
			m_DataM->m_Mutex.LeaveMutex();
		}

		/*************************************************
		Summary: access to the Value
		*************************************************/
		T& Value()
		{
			return m_DataM->m_Value;
		}

	private:

		
		Accessor();
		
		Accessor(const Accessor&);
		
		Accessor& operator=(const Accessor&);

		
		VxDataMutexed<T> *m_DataM;

	};

	/*************************************************
	Summary: This class give you a thread safe access to the VxDataMutexed Value. Look the example in the VxDataMutexed.
	*************************************************/
	class ConstAccessor
	{
	public:

		/*************************************************
		Summary: Construct an accessor on your data. The data is locked by its mutex.
		*************************************************/
		ConstAccessor(const VxDataMutexed<T> *dm)
		{
			m_DataM = dm;
			m_DataM->m_Mutex.EnterMutex();
		}

		/*************************************************
		Summary: Destruct the accessor on your data. The data is unlocked by its mutex.
		*************************************************/
		~ConstAccessor()
		{
			m_DataM->m_Mutex.LeaveMutex();
		}

		/*************************************************
		Summary: access to the Value
		*************************************************/
		const T& Value() const
		{
			return m_DataM->m_Value;
		}

	private:

		
		ConstAccessor();
		
		ConstAccessor(const ConstAccessor&);
		
		ConstAccessor& operator=(const ConstAccessor&);

		
		const VxDataMutexed<T> *m_DataM;

	};

	
	mutable VxMutex	m_Mutex;
	
	T				m_Value;


private:

	
	VxDataMutexed(const VxDataMutexed&);
	
	VxDataMutexed& operator=(const VxDataMutexed&);

};

#endif
