/*************************************************************************/
/*	File : XHashFun.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/


#ifndef _XHashFun_H_
#define _XHashFun_H_

#include "XString.h"
#include "XUtil.h"

#ifdef _WIN32_WCE
	#include "VxMathCE.H"
#else
	#include <cctype>
	#include <cstdlib>
#endif
/************************************************
Summary: A serie of comparison function

Remarks


************************************************/
template <class K>
struct XEqual 
{
  int operator()(const K& iK1,const K& iK2) const { 
	  return (iK1 == iK2); 
  }
};


template <>
struct XEqual<const char*>
{
  int operator()(const char* iS1,const char* iS2) const { return !strcmp(iS1,iS2); }
};


template <>
struct XEqual<char*>
{
  int operator()(const char* iS1,const char* iS2) const { return !strcmp(iS1,iS2); }
};


struct XEqualXStringI
{
  int operator()(const XString& iS1,const XString& iS2) const { return !iS1.ICompare(iS2); }
};


struct XEqualStringI
{
  int operator()(const char* iS1,const char* iS2) const { return !stricmp(iS1,iS2); }
};

/************************************************
Summary: A serie of hash functions

Remarks
These hash functions are designed to be used when declaring a hash table where the
key is one of the following type :

	o XHashFunString : hash function for char* key 
	o XHashFunXString : hash function for XString key 
	o XHashFunChar : hash function for char key 
	o XHashFunByte : hash function for BYTE key 
	o XHashFunWord : hash function for WORD key 
	o XHashFunDword : hash function for DWORD key 
	o XHashFunInt : hash function for int key 
	o XHashFunFloat : hash function for float key 
	o XHashFunPtr : hash function for void* key 


************************************************/
template <class K> 
struct XHashFun 
{
	int operator ()(const K& iK) 
	{
		return (int) iK;
	}
};


// NB hashing should return unsigned int
// and be multiplied by 2654435761 (Knuth method : golden ratio of 2^32)


inline int XHashString(const char* __s)
{
	unsigned int __h = 0; 
	for ( ; *__s; ++__s)
		__h = 5*__h + *__s;
	
	return int(__h);
}


inline int XHashStringI(const char* __s)
{
	unsigned int __h = 0; 

	for ( ; *__s; ++__s)
	{
		// GG : ANSI Compliant
		__h = 5*__h + tolower(*__s);
	}
	
	return int(__h);
}


template <>
struct XHashFun<char*>
{
	int operator()(const char* __s) const { return XHashString(__s); }
};


template <>
struct XHashFun<const char*>
{
	int operator()(const char* __s) const { return XHashString(__s); }
};


template <>
struct XHashFun<XString>
{
	int operator()(const XString& __s) const { return XHashString(__s.CStr()); }
};


template <>
struct XHashFun<float>
{
  int operator()(const float __x) const { return *(int*)&__x; }
};


template <>
struct XHashFun<void*>
{
  int operator()(const void* __x) const { return (int)__x>>8; }
};


template <>
struct XHashFun<XGUID> 
{
	int operator()(const XGUID& __s) const
	{
		return __s.d1;
	}
};
	

struct XHashFunStringI
{
	int operator()(const char* __s) const { return XHashStringI(__s); }
};


struct XHashFunXStringI
{
	int operator()(const XString& __s) const { return XHashStringI(__s.CStr()); }
};

#endif
