/*************************************************************************/
/*	File : XString.h													 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/


#ifndef _XSTRING_H_
#define _XSTRING_H_

#include "XUtil.h"

#ifndef _WIN32_WCE
	#include <cassert>
#endif

#ifndef _XBOX 
#else
	namespace XTools {
#endif




class XString;

/************************************************
{filename:XBaseString}
Summary: Wrapper around a C string (a char* or const char*).

Remarks:

	o This class does not duplicate the data it is given and does not delete 
	the string it points to when destroyed.
	o It calculates and store the length, for further faster access.



See Also : XBaseString.
************************************************/
class XBaseString
{
	friend class XString;
public:

	/************************************************
	Summary: Constructors.

	Arguments: 
		iString: the string to hold.
	************************************************/
	XBaseString():m_Buffer(0),m_Length(0),m_Allocated(0) {}

	// Ctor from a const string litteral
	// Warning : the size is calculated here
	XBaseString(const char* iString)
	{
		if (iString) {
			m_Buffer = (char*)iString;
			m_Length = 0xffffffff;
			while (m_Buffer[++m_Length]);
		} else {
			m_Buffer = NULL;
			m_Length = 0;
		}
	}

	// Dtor
	~XBaseString() {} // Nothing to do !

	/************************************************
	Summary: Returns the length of the string.
	************************************************/
	XDWORD	Length() const
	{
		return m_Length;
	}

	/************************************************
	Summary: Conversion to a const char* (read only).
	************************************************/
	const char* CStr() const
	{
		return (m_Buffer)?m_Buffer:"";
	}

	/************************************************
	Summary: Conversion to a int.
	************************************************/
	int ToInt() const
	{
		return (m_Buffer)?atoi(m_Buffer):0;
	}

	/************************************************
	Summary: Conversion to a float.
	************************************************/
	float ToFloat() const
	{
		return (m_Buffer)?(float)atof(m_Buffer):0.0f;
	}

	/************************************************
	Summary: Conversion to a double.
	************************************************/
	double ToDouble() const
	{
		return (m_Buffer)?atof(m_Buffer):0.0;
	}

	/************************************************
	Summary: access to a character (read only)

	Arguments: 
		i: index of the character to read.
	************************************************/
	const char	operator [](XDWORD i) const
	{
#ifndef _WIN32_WCE
		XASSERT(i>=0 && (DWORD) i < m_Length);
#endif		
		return m_Buffer[i];
	}

	const char	operator [](int i) const
	{
#ifndef _WIN32_WCE
		XASSERT(i>=0 && (DWORD) i < m_Length);
#endif		
		return m_Buffer[i];
	}

protected:
	// the string 
	char*	m_Buffer;
	// the length of the string
	XDWORD	m_Length;
	// the allocated size
	XDWORD	m_Allocated;
};

/************************************************
Summary: Class representation of a string (an array of character ended by a '\0').
{filename:XString}
Remarks:
	This class always duplicate the data it is given.



See Also : XBaseString.
************************************************/
class XString : public XBaseString
{
public:

	enum {
		NOTFOUND = 0xffffffff
	};

	// Default Ctor 
	XString():XBaseString() {}

	// Substring Ctor
	VX_EXPORT XString(const char* iString, const int iLength=0);

	// Reserving Ctor
	VX_EXPORT explicit XString(const int iLength);

	// Copy Ctor
	VX_EXPORT XString(const XString& iSrc);

	// Copy Ctor
	VX_EXPORT XString(const XBaseString& iSrc);

	// Dtor
	VX_EXPORT ~XString();

	// operator =
	VX_EXPORT XString& operator = (const XString& iSrc);

	// operator =
	VX_EXPORT XString& operator = (const char* iSrc);

	// operator =
	VX_EXPORT XString& operator = (const XBaseString& iSrc);

	// Create from a string and a length
	VX_EXPORT XString& Create(const char* iString, const int iLength=0);

	// Create from a string and a starting position
	VX_EXPORT XString& CreateTail(const XBaseString& iString, const int iStart=0);

	/************************************************
	Summary: Conversion to a char*.
	************************************************/
	char* Str()
	{
		return (m_Buffer)?m_Buffer:(char*)"";
	}

	/************************************************
	Summary: Conversion to a const char*.
	************************************************/
	const char* Str() const
	{
		return (m_Buffer)?m_Buffer:"";
	}

	/************************************************
	Summary: access to a character (read/write)

	Arguments: 
		i: index of the character to read/write.
	************************************************/
	char&	operator [](XDWORD i)
	{
#ifndef _WIN32_WCE
		XASSERT(i < m_Length);
#endif
		
		return m_Buffer[i];
	}
	
	char&	operator [](int i)
	{
#ifndef _WIN32_WCE
		XASSERT(i>=0 && (DWORD) i < m_Length);
#endif
		
		return m_Buffer[i];
	}

	/************************************************
	Summary: access to a character (read only)

	Arguments: 
		i: index of the character to read.
	************************************************/
	char	operator [](XDWORD i) const
	{
#ifndef _WIN32_WCE
		XASSERT(i < m_Length);
#endif		
		return m_Buffer[i];
	}

	char	operator [](int i) const
	{
#ifndef _WIN32_WCE
		XASSERT(i>=0 && (DWORD) i < m_Length);
#endif		
		return m_Buffer[i];
	}

	// Format the string sprintf style
	VX_EXPORT XString& Format(const char* iFormat,...);

	// Format the current time
	VX_EXPORT XString& FormatTime(const char* iFormat);

	// Format a given time 
	VX_EXPORT XString& FormatTime(int iTime, const char* iFormat);

	// Capitalize all the characters of the string
	VX_EXPORT XString& ToUpper();

	// Uncapitalize all the characters of the string
	VX_EXPORT XString& ToLower();

	// Compare the strings.
	/*************************************************
	Summary: Compare two strings.

	Arguments:
		iStr: string to compare with the current object.

	Return Value:
		< 0 the current string is lesser than iStr,
		0 the current string is identical to iStr,
		> 0 the current string is greater than iStr.

	See also: ICompare.
	*************************************************/
	int Compare(const XBaseString& iStr) const
	{
		if (!m_Length) return -(int)iStr.m_Length; // Null strings
		if (!iStr.m_Length) return m_Length;

#if defined(PSX2)  || defined(PSP)
		return strcmp(m_Buffer,iStr.m_Buffer);
#else
		char* s1 = m_Buffer;
		char* s2 = iStr.m_Buffer;

		int Lenght4 = 	(m_Length > iStr.m_Length) ? (iStr.m_Length >> 2) : (m_Length >> 2);
		//--- Compare dwords by dwords....
		while ((Lenght4-->0) && (*(XDWORD*)s1 == *(XDWORD*)s2))
			s1+=4,s2+=4;
	
		//----- remaining bytes...
		while ( (*s1 == *s2) && *s1)
			++s1,++s2;
		return (*s1-*s2);
#endif
	}
	
	/*************************************************
	Summary: Compare the n first character of two strings.

	Arguments:
		iStr: string to compare with the current object.
		iN: n first character to compare

	Return Value:
		< 0 the current string is lesser than iStr,
		0 the current string is identical to iStr,
		> 0 the current string is greater than iStr.

	See also: ICompare.
	*************************************************/
	int NCompare(const XBaseString& iStr, const int iN) const
	{
		if (!m_Length) return -(int)iStr.m_Length; // Null strings
		if (!iStr.m_Length) return m_Length;

		return strncmp(m_Buffer,iStr.m_Buffer,iN);
	}

	// Compare the strings (ignore the case).
	VX_EXPORT int ICompare(const XBaseString& iStr) const;

	///
	// Complex operations on strings

	// removes space ( ' ', '\t' and '\n') from the start and the end of the string	
	VX_EXPORT XString& Trim();

	// replaces space ( ' ', '\t' and '\n') sequences by one space
	VX_EXPORT XString& Strip();

	// finds a string in the string
	XBOOL Contains(const XBaseString& iStr) const
	{
		return Find(iStr) != NOTFOUND;
	}

	// finds a string in the string (ignore the case).
	XBOOL IContains(const XBaseString& iStr) const
	{
		return IFind(iStr) != NOTFOUND;
	}

	// finds a character in the string
	VX_EXPORT XDWORD Find(char iCar,XDWORD iStart=0) const;

	// finds a string in the string
	VX_EXPORT XDWORD Find(const XBaseString& iStr,XDWORD iStart=0) const;

	// finds a string in the string (ignore the case).
	VX_EXPORT XDWORD IFind(const XBaseString& iStr,XDWORD iStart=0) const;

	// finds a character in the string
	VX_EXPORT XDWORD RFind(char iCar,XDWORD iStart=NOTFOUND) const;

	// creates a substring
	VX_EXPORT XString Substring(XDWORD iStart,XDWORD iLength=0) const;

	// crops the string
	VX_EXPORT XString& Crop(XDWORD iStart,XDWORD iLength);

	// cuts the string
	VX_EXPORT XString& Cut(XDWORD iStart,XDWORD iLength);

	// replaces all the occurence of a character by another one
	VX_EXPORT int Replace(char iSrc,char iDest);

	// replaces all the occurence of a string by another string
	VX_EXPORT int Replace(const XBaseString& iSrc,const XBaseString& iDest);

	//replaces all invalid characters :*?"<>| into ' ' (with the exception of C:\ allowed - [letter]:[\ or /])
	VX_EXPORT void CheckFileNameValidity();

	///
	// Comparison operators

	/************************************************
	Summary: Operators for strings comparisons.
	************************************************/
	int operator == (const char* iStr) const
	{
		return !Compare(iStr);
	}

	int operator != (const char* iStr) const
	{
		return Compare(iStr);
	}

	int operator < (const char* iStr) const
	{
		return Compare(iStr)<0;
	}

	int operator <= (const char* iStr) const
	{
		return Compare(iStr)<=0;
	}

	int operator > (const char* iStr) const
	{
		return Compare(iStr)>0;
	}

	int operator >= (const char* iStr) const
	{
		return Compare(iStr)>=0;
	}

	int operator - (const char* iStr) const
	{
		return Compare(iStr);
	}

	int operator == (const XBaseString& iStr) const
	{
		return !Compare(iStr);
	}

	int operator != (const XBaseString& iStr) const
	{
		return Compare(iStr);
	}

	int operator < (const XBaseString& iStr) const
	{
		return Compare(iStr)<0;
	}

	int operator <= (const XBaseString& iStr) const
	{
		return Compare(iStr)<=0;
	}

	int operator > (const XBaseString& iStr) const
	{
		return Compare(iStr)>0;
	}

	int operator >= (const XBaseString& iStr) const
	{
		return Compare(iStr)>=0;
	}

	int operator - (const XBaseString& iStr) const
	{
		return Compare(iStr);
	}
	

	/************************************************
	Summary: Creteas a new string that is the concatenation of the 
	left and right operand.
	************************************************/

	///
	// Stream operators
	
	// Concatenation operator
	VX_EXPORT XString& operator << (const char* iString);

	// Concatenation operator
	VX_EXPORT XString& operator << (const XBaseString& iString);

	// Concatenation operator
	VX_EXPORT XString& operator << (const char iValue);

	// Concatenation operator
	VX_EXPORT XString& operator << (const int iValue);

	// Concatenation operator
	VX_EXPORT XString& operator << (const unsigned int iValue);

	// Concatenation operator
	VX_EXPORT XString& operator << (const float iValue);

	// Concatenation operator
	VX_EXPORT XString& operator << (const void* iValue);

	///
	// + operators

	VX_EXPORT XString operator + (const char* iString)	const
	{
		XString tmp = *this;
		return tmp<<iString;
	}
	VX_EXPORT XString operator + (const XBaseString& iString) const
	{
		XString tmp = *this;
		return tmp<<iString;
	}
	VX_EXPORT XString operator + (const char iValue) const
	{
		XString tmp = *this;
		return tmp<<iValue;
	}
	VX_EXPORT XString operator + (const int iValue) const
	{
		XString tmp = *this;
		return tmp<<iValue;
	}
	VX_EXPORT XString operator + (const unsigned int iValue) const
	{
		XString tmp = *this;
		return tmp<<iValue;
	}
	VX_EXPORT XString operator + (const float iValue) const
	{
		XString tmp = *this;
		return tmp<<iValue;
	}




	///
	// Capacity functions
	
	/************************************************
	Summary: Returns the capacity (the allocated size)
	of the string.
	See also: Reserve.
	************************************************/
	XDWORD	Capacity() 
	{
		return m_Allocated;
	}

	// Sets the capacity of the string
	VX_EXPORT void	Reserve(XDWORD iLength);
	
	void	Resize(XDWORD iLength)
	{
		Reserve(iLength);
		if (iLength)
			m_Buffer[iLength] = 0;
		else 
			if (m_Buffer)
				m_Buffer[0]		= 0;
		m_Length = iLength;
	}

	///
	// Back Compatibility functions
	
	
	XString& operator+=(const XString& v)	{return (*this) << v;}
	
	XString& operator+=(const char* v)		{return (*this) << v;}
	
	XString& operator+=(const char v)		{return (*this) << v;}
	
	XString& operator+=(const int v)		{return (*this) << v;}
	
	XString& operator+=(const float v)		{return (*this) << v;}

	// conversion operator (for script purpose only) (cast operator NYI)
	// operator char*() 
	// {
	// 	return (m_Buffer)?m_Buffer:(char*)"";
	// }

protected:
	
	// Check a string size to see if it fits in
	
	void CheckSize(int iLength);

	// Assign a string to the current string
	
	void Assign(const char* iBuffer,int iLength);

};

inline XString operator+(const char* lhs, const XString &rhs)
{
	XString tmp = lhs;
	return tmp<<rhs;
}

#ifndef _XBOX
#else
	} // end namespace XTools

	using XTools::XString;
	using XTools::XBaseString;
#endif

#endif // _XSTRING_H_
