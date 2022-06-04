/*************************************************************************/
/*	File : XSMatrix.h													 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef _XSMATRIX_H_
#define _XSMATRIX_H_

#include "XUtil.h"

// Template class Describing a 2D Matrix of variable width and height.
template <class T>
class XMatrix
{
public:
    XMatrix(int iWidth = 0, int iHeight = 0) : m_Data(NULL), m_Width(0), m_Height(0)
    {
        Allocate(iWidth, iHeight);
    }

    ~XMatrix(void)
    {
        delete[] m_Data;
    }

    ///
    // Accessors

    // Returns the width of the matrix (Number of columns)
    int GetWidth() const
    {
        return m_Width;
    }

    // Returns the height of the matrix (Number of rows)
    int GetHeight() const
    {
        return m_Height;
    }

    // Returns the memory taken by the matrix in bytes
    int Size() const
    {
        return m_Width * m_Height * sizeof(T);
    }

    // Free the memory taken by the matrix.
    void Clear()
    {
        delete[] m_Data;
        m_Data = NULL;
        m_Width = 0;
        m_Height = 0;
    }

    // Creation of the matrix (automatically calls Clear)
    void Create(int iWidth, int iHeight)
    {
        Clear();
        Allocate(iWidth, iHeight);
    }

    // Access to an element of the matrix
    const T &operator()(const int iX, const int iY) const
    {
        XASSERT(iX < m_Width);
        XASSERT(iY < m_Height);
        return m_Data[iY * m_Width + iX];
    }

    T &operator()(const int iX, const int iY)
    {
        XASSERT(iX < m_Width);
        XASSERT(iY < m_Height);
        return m_Data[iY * m_Width + iX];
    }

private:
    // allocate the space for the matrix
    void Allocate(int iWidth, int iHeight)
    {
        int count = iWidth * iHeight;
        if (count)
        {
            m_Data = new T[count];
            XASSERT(m_Data); // No more free space ???
            m_Width = iWidth;
            m_Height = iHeight;
        }
    }

    ///
    // members

    // data
    T *m_Data;

    ///
    // dimensions

    // width
    int m_Width;
    // height
    int m_Height;
};

#endif