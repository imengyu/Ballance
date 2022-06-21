/*************************************************************************/
/*	File : XBitArray.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef XBITARRAY_H
#define XBITARRAY_H  "$Id:$"

#include "XArray.h"
#include "VxMath.h"

/************************************************
{filename:XBitArray}
Summary: Set of bit flags.

Remarks:

	o This class  define a set of bit flags that may be treated as a virtual array but are stored in an efficient manner. 
	o The class has methods to set, clear and return the i-th bit, resize the array, etc.
************************************************/
class XBitArray  
{
public:
	XBitArray(int initialsize = 1) {
		if (initialsize<1) initialsize=1;
		m_Size = (initialsize<<5);
		if (m_Size > 32) { // we allocate only if > 32
			m_Data = Allocate(initialsize);
			Clear();
		} else {
			m_Flags = 0;
		}
	}
	
	~XBitArray() {
		if (m_Size > 32)
			Free();
	}

	// copy Ctor
	XBitArray(const XBitArray& a)
	{
		m_Size = a.m_Size;
		if (m_Size <= 32)
			m_Flags = a.m_Flags;
		else {
			m_Data = Allocate(m_Size>>5);
			memcpy(m_Data,a.m_Data,m_Size>>3);
		}
	}

	// operator =
	XBitArray& operator = (const XBitArray& a)
	{
		if (this != &a) {
			if (m_Size != a.m_Size) {
				if (m_Size > 32)
					Free();
				
				m_Size = a.m_Size;
				if (m_Size > 32) { // we allocate only if > 32
					m_Data = Allocate(m_Size>>5);
					memcpy(m_Data,a.m_Data,m_Size>>3);
				} else {
					m_Flags = a.m_Flags;
				}
			} else {
				if (m_Size>32)
					memcpy(m_Data,a.m_Data,m_Size>>3);
				else 
					m_Flags = a.m_Flags;

			}
		}
		return *this;
	}

	
	// Reallocation if necessary
	void CheckSize(int n)
	{
		while (n >= m_Size) { // m_Size >= 32, always
			
			int size = (m_Size>>5);
			XDWORD* temp = Allocate(size+size);
			
			// Copy the old bits
			if (m_Size > 32)
				memcpy(temp,m_Data,size*sizeof(XDWORD));
			else 
				*temp = m_Flags;
			
			// Clear the new bits
			memset(temp+size,0,size*sizeof(XDWORD));
			
			if (m_Size > 32) // we free only if we were allocated
				Free();
			
			m_Data = temp;
			m_Size += m_Size;
		}
	}
	
	void CheckSameSize(XBitArray& a)
	{
		if (m_Size < a.m_Size) {
			if (a.m_Size > 32) {
				int size = a.m_Size>>5;
				int oldsize = m_Size>>5;
				XDWORD* temp = Allocate(size);
				// Copy the old bits
				if (m_Size > 32)
					memcpy(temp,m_Data,oldsize*sizeof(XDWORD));
				else 
					*temp = m_Flags;

				// Clear the new bits
				memset(temp+oldsize,0,(size-oldsize)*sizeof(XDWORD));
				
				if (m_Size > 32) // we free only if we were allocated
					Free();

				m_Data = temp;
				m_Size = a.m_Size;
			}
		}
	}

	
	// Summary: Returns if the n-th bit is set to 1
	int IsSet(int n) { 
		if (n >= m_Size) 
			return 0; // Out of range
		if (m_Size <= 32) 
			return (m_Flags & (1<<n)); // Not allocated 

		return (m_Data[n>>5] & (1<<(n&31)));  // Allocated after the first DWORD
	}

	// Summary: Returns if the n-th bit is set to 1
	int operator [] (int n) {
		return IsSet(n);
	}

	// Summary: Appends the bitcount first bits of integer v to the array
	void AppendBits(int n,int v,int bitcount)
	{
		int mask = 1;
		bitcount += n;
		for(int i=n;i<bitcount; ++i,mask<<=1) {
			if (mask&v) 
				Set(i);
			else 
				Unset(i);
		}
	}

	// Summary: Sets the n-th bit to 1
	void Set(int n) { 
		if (n < m_Size) {
			if (m_Size <= 32) { // Only one dword long
				m_Flags |= (1<<n);
			} else { // more than one dword
				m_Data[n>>5] |= 1<<(n&31);
			}
		} else { // we have to reallocate
			CheckSize(n);
			m_Data[n>>5] |= 1<<(n&31);
		}
	}
	
	// Summary: Sets the n-th bit to 1 and return 0 if it was set, 1 otherwise
	int TestSet(int n) { 
		if(n < m_Size) {
			if (m_Size <= 32) { // Only one dword long
				int mask = 1<<n;
				
				if (m_Flags&mask) return 0;
				m_Flags |= mask;
				return 1;
			
			} else { // more than one dword
				int pos = n>>5;
				int mask = 1<<(n&31);
				
				if (m_Data[pos]&mask) return 0;
				m_Data[pos] |= mask;
				return 1;
			}
		} else { // we have to reallocate
			CheckSize(n);
			m_Data[n>>5] |= 1<<(n&31);
			return 1;
		}
	}

	// Summary: Sets the n-th bit to 0
	void Unset(int n) { 
		if(n < m_Size) {
			if (m_Size <= 32) {
				m_Flags &= ~(1<<n);
			} else {
				m_Data[n>>5] &= ~(1<<(n&31));
			}
		}
	}

	// Summary: Sets the n-th bit to 0 and return 1 if it was set, 0 otherwise
	int TestUnset(int n) { 
		if(n < m_Size) {
			if (m_Size <= 32) {
				int mask	= 1<<n;
				if (m_Flags &   mask) {
					m_Flags &= ~mask;
					return 1;
				} 
				return 0;
			} else {
				int pos		= n>>5;
				int mask	= 1<<(n&31);
				if (m_Data[pos] &   mask) {
					m_Data[pos] &= ~mask;
					return 1;
				} 
				return 0;
			}
		} else return 0;
	}

	// Summary: Returns the number of bits
	int Size() const {
		return m_Size;
	}

	// Summary: Resets the array
	void Clear() {
		if (m_Size<=32)
			m_Flags = 0;
		else
			memset(m_Data,0,(m_Size>>3));
	}

	// Summary: Sets all bits of the array to 1 
	// Warning : this functions sets all the allocated bits, not only the 
	// used bits
	void Fill() {
		if (m_Size<=32)
			m_Flags = 0xffffffff;
		else
			memset(m_Data,0xff,(m_Size>>3));
	}

	// Summary: Performs a binary AND with another array
	void And(XBitArray& a)
	{
		if (a.m_Size <= 32) {
			if (m_Size <= 32)
				m_Flags &= a.m_Flags;
			else {
				m_Data[0] &= a.m_Flags;

				// clear the remaining bytes
				int rsize = m_Size>>5;
				for (int i=1;i<rsize;++i) {
					m_Data[i] = 0;
				}
			}
		} else {
			if (m_Size <= 32)
				m_Flags &= a.m_Data[0];
			else {
				int size = a.m_Size>>5;
				int i = 0;
				for (;i<size;++i) {
					m_Data[i] &= a.m_Data[i];
				}
				// clear the remaining bytes
				int rsize = m_Size>>5;
				for (;i<rsize;++i) {
					m_Data[i] = 0;
				}
			}
		}
	}

	// Summary: subtract bits from another bitarray
	XBitArray& operator -= (XBitArray& a)
	{
		if (a.m_Size <= 32) {
			if (m_Size <= 32)
				m_Flags &= ~a.m_Flags;
			else {
				m_Data[0] &= ~a.m_Flags;
			}
		} else {
			if (m_Size <= 32)
				m_Flags &= ~a.m_Data[0];
			else {
				int size = a.m_Size>>5;
				int i = 0;
				for (;i<size;++i) {
					m_Data[i] &= ~a.m_Data[i];
				}
			}
		}
		return *this;
	}

	// Summary: Returns TRUE if at least one common bit is set in two arrays
	BOOL CheckCommon(XBitArray& a)
	{
		if (a.m_Size <= 32) {
			if (m_Size <= 32)
				return (m_Flags & a.m_Flags);
			else 
				return (m_Data[0] & a.m_Flags);
		} else {
			if (m_Size <= 32)
				return (m_Flags & a.m_Data[0]);
			else {
				int size = a.m_Size>>5;
				for (int i=0;i<size;++i) {
					if (m_Data[i] & a.m_Data[i])
						return TRUE;
				}
			}
		}

		return FALSE;
	}

	// Summary: Performs a binary OR with another array
	void Or(XBitArray& a)
	{
		CheckSameSize(a);

		if (a.m_Size <= 32) {
			if (m_Size <= 32)
				m_Flags |= a.m_Flags;
			else 
				m_Data[0] |= a.m_Flags;
		} else {
			if (m_Size <= 32)
				m_Flags |= a.m_Data[0];
			else {
				int size = a.m_Size>>5;
				for (int i=0;i<size;++i) {
					m_Data[i] |= a.m_Data[i];
				}
			}
		}
	}

	// Summary: Performs a binary XOR with another array
	void XOr(XBitArray& a)
	{
		CheckSameSize(a);

		if (a.m_Size <= 32) {
			if (m_Size <= 32)
				m_Flags ^= a.m_Flags;
			else 
				m_Data[0] ^= a.m_Flags;
		} else {
			if (m_Size <= 32)
				m_Flags ^= a.m_Data[0];
			else {
				int size = a.m_Size>>5;
				for (int i=0;i<size;++i) {
					m_Data[i] ^= a.m_Data[i];
				}
			}
		}
	}

	// Summary: Inverts all bits value in the array
	void Invert()
	{		
		if (m_Size <= 32) {
			m_Flags = ~m_Flags;
		} else {
			int size = m_Size>>5;
			for(int i=0;i<size;++i) {
				m_Data[i] = ~m_Data[i];
			}
		}
	}



	// Summary: Returns the number of bits set
	int BitSet()
	{
		int set = 0;
		if (m_Size <= 32) {
			int mask = 1;
			for (int j=0;j<32;++j) {
				if (m_Flags & mask) 
					++set;
				mask<<=1;
			}
		} else {
			int size = m_Size>>5;
			for (int i=0;i<size;++i) {
				int mask = 1;
				for(int j=0;j<32;++j) {
					if(m_Data[i]&mask) ++set;
					mask<<=1;
				}
			}
		}
		return set;
	}

	// Summary: Returns the position of the n-th set(1) bit
	int GetSetBitPosition(int n)
	{
		int set = 0;
		int pos = 0;
		if (m_Size <= 32) {
			int mask = 1;
			for (int j=0; j<32; ++j, ++pos) {
				if (m_Flags & mask) {
					if (set == n) 
						return pos;
					++set;
				}
				mask<<=1;
			}

		} else {
			int size = m_Size>>5;
			for (int i=0;i<size;++i) {
				int mask = 1;
				for (int j=0; j<32; ++j, ++pos) {
					if (m_Data[i] & mask) {
						if (set == n) 
							return pos;
						++set;
					}
					mask<<=1;
				}
			}
		}
		return -1;
	}

	// Summary: Returns the position of the n-th unset(0) bit
	int GetUnsetBitPosition(int n)
	{
		int unset = 0;
		int pos = 0;
		if (m_Size <= 32) {
			int mask = 1;
			for (int j=0; j<32; ++j, ++pos) {
				if (!(m_Flags & mask)) {
					if (unset == n) 
						return pos;
					++unset;
				}
				mask<<=1;
			}

		} else {
			int size = m_Size>>5;
			for (int i=0;i<size;++i) {
				int mask = 1;
				for (int j=0;j<32;++j,++pos) {
					if (!(m_Data[i]&mask)) {
						if (unset == n) 
							return pos;
						++unset;
					}
					mask<<=1;
				}
			}
		}
		// We haven't found an unsetted bit yet : we reallocate
		CheckSize(pos);
		return pos;
	}

	
	char * ConvertToString(char * buffer)
	{
		if (buffer)
		{
			int count=0;
			if (m_Size <= 32) {
				for (int j=0;j<32;j++)
				{
					if (m_Flags & (1<<j))
						buffer[count]='1';
					else
						buffer[count]='0';
					count++;
				}
			} else {
				for (int i=0;i<(m_Size>>5);i++)
				{
					for (int j=0;j<32;j++)
					{
						if (m_Data[i] & (1<<j))
							buffer[count]='1';
						else
							buffer[count]='0';
						count++;
					}
				}
			}
			buffer[m_Size]=0;
		}
		return buffer;
	}

	// Summary: Returns the occupied size in memory in bytes
	int GetMemoryOccupation(BOOL addstatic=FALSE) const {
		if (m_Size<=32)
			return addstatic?sizeof(*this):0;
		else
			return (m_Size>>5)*sizeof(XDWORD)+(addstatic?sizeof(*this):0);
	}

private:
	
	XDWORD* Allocate(int size) 
	{
		return new XDWORD[size];
	}

	
	void Free() 
	{
		delete [] m_Data;
	}

	// the array itself {secret}
	// If the size is inferior strict to 32
	// the data array is not allocated, we 
	// just use the sapce of the pointer to store the 
	// first bits
	union {
	XDWORD*	m_Data;
	XDWORD	m_Flags;
	};
	// real size already allocated {secret}
	int		m_Size;
};

#endif // BITARRAY_H

