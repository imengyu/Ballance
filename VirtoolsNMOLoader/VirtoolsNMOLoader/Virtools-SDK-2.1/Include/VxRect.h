/*************************************************************************/
/*	File : VxRect.h														 */
/*	Author :  Aymeric Bard												 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef VXRECT_H
#define VXRECT_H

#include "XUtil.h"
#include "Vx2dVector.h"
#include "VxMathDefines.h"

typedef enum VXRECT_INTERSECTION
{
    ALLOUTSIDE = 0,
    ALLINSIDE = 1,
    PARTINSIDE = 2
} VXRECT_INTERSECTION;

/**********************************************************
{filename:VxRect}
Name: VxRect

Remarks:
A Rect is defined by 4 floats and is used to represents
a 2D region.

A VxRect is defined as:

        class VxRect
        {
        public:
            float left;
            float top;
            float right;
            float bottom;
        };

Elements can be accessed directly or by using the accessors functions
for more sophisticate access.



See Also :
*********************************************************/
class VxRect
{
public:
    // Members
#if !defined(_MSC_VER)
    float left;
    float top;
    float right;
    float bottom;
#else
    union
    {
        struct
        {
            float left;
            float top;
            float right;
            float bottom;
        };
        struct
        {
            Vx2DVector m_TopLeft;
            Vx2DVector m_BottomRight;
        };
    };
#endif

    // Methods
    VxRect(){};

#if !defined(_MSC_VER)
    VxRect(Vx2DVector &topleft, Vx2DVector &bottomright) : left(topleft.x), top(topleft.y), right(bottomright.x), bottom(bottomright.y)
    {
    }
    VxRect(float l, float t, float r, float b) : left(l), top(t), right(r), bottom(b) {}
#else
    VxRect(Vx2DVector &topleft, Vx2DVector &bottomright) : m_TopLeft(topleft), m_BottomRight(bottomright)
    {
    }
    VxRect(float l, float t, float r, float b) : m_TopLeft(l, t), m_BottomRight(r, b) {}
#endif

    /************************************************
    Summary: Changes the width of a rectangle.

    Input Arguments:
        w: new width in float.
    ************************************************/
    void SetWidth(float w) { right = left + w; };

    /************************************************
    Summary: Returns the width of a rectangle.
    ************************************************/
    float GetWidth() const { return right - left; }

    /************************************************
    Summary: Changes the height of a rectangle.

    Input Arguments:
        h: new height in float.
    ************************************************/
    void SetHeight(float h) { bottom = top + h; }

    /************************************************
    Summary: Returns the height of a rectangle.
    ************************************************/
    float GetHeight() const { return bottom - top; }

    /************************************************
    Summary: Returns the horizontal center of the rect.
    ************************************************/
    float GetHCenter() const { return left + 0.5f * GetWidth(); }

    /************************************************
    Summary: Returns the vertical center of the rect.
    ************************************************/
    float GetVCenter() const { return top + 0.5f * GetHeight(); }

    /************************************************
    Summary: Changes the size of a rectangle.

    Input Arguments:
        v: new size in the form of a Vx2DVector.
    ************************************************/
    void SetSize(const Vx2DVector &v)
    {
        SetWidth(v.x);
        SetHeight(v.y);
    }

    /************************************************
    Summary: Returns the size of a rectangle.
    ************************************************/
    Vx2DVector GetSize() const { return Vx2DVector(GetWidth(), GetHeight()); }

    void SetHalfSize(const Vx2DVector &v)
    {
        Vx2DVector c = GetCenter();
        SetCenter(c, v);
    }
    Vx2DVector GetHalfSize() const { return Vx2DVector(0.5f * GetWidth(), 0.5f * GetHeight()); }

    void SetCenter(const Vx2DVector &v)
    {
        Vx2DVector hs = GetHalfSize();
        SetCenter(v, hs);
    }
    Vx2DVector GetCenter() const { return Vx2DVector(GetHCenter(), GetVCenter()); }

    void SetTopLeft(const Vx2DVector &v)
    {
        left = v.x;
        top = v.y;
    }
#if defined(_MSC_VER)
    const Vx2DVector &GetTopLeft() const
    {
        return m_TopLeft;
    }
    Vx2DVector &GetTopLeft() { return m_TopLeft; }
#else
    const Vx2DVector &GetTopLeft() const
    {
        return (const Vx2DVector &)*(Vx2DVector *)&left;
    }
    Vx2DVector &GetTopLeft()
    {
        return (Vx2DVector &)*(Vx2DVector *)&left;
    }
#endif

    void SetBottomRight(const Vx2DVector &v)
    {
        right = v.x;
        bottom = v.y;
    }
#if defined(_MSC_VER)
    const Vx2DVector &GetBottomRight() const
    {
        return m_BottomRight;
    }
    Vx2DVector &GetBottomRight() { return m_BottomRight; }
#else
    const Vx2DVector &GetBottomRight() const
    {
        return (const Vx2DVector &)*(Vx2DVector *)&right;
    }
    Vx2DVector &GetBottomRight()
    {
        return (Vx2DVector &)*(Vx2DVector *)&right;
    }
#endif

    /************************************************
    Summary: Sets the rectangle as a NULL rectangle
    (position (0,0) ;  size (0,0).
    ************************************************/
    void Clear() { SetCorners(0, 0, 0, 0); }

    /*************************************************
    Summary: Creates a rectangle based on two corners.

    Input Arguments:
        topleft: a Vx2DVector containing the top left corner.
        bottomright: a Vx2DVector containing the bottom right corner.
        t: top in float.
        l: left in float.
        b: bottom in float.
        r: right in float.
    See also: VxRect::SetDimension,VxRect::SetCenter
    *************************************************/
    void SetCorners(const Vx2DVector &topleft, const Vx2DVector &bottomright)
    {
        left = topleft.x;
        top = topleft.y;
        right = bottomright.x;
        bottom = bottomright.y;
    }
    void SetCorners(float l, float t, float r, float b)
    {
        left = l;
        top = t;
        right = r;
        bottom = b;
    }

    /*************************************************
    Summary: Creates a rectangle based on the top left coner and the size.

    Input Arguments:
    position: a Vx2DVector containing the top left corner.
    size: a Vx2DVector containing the size.
    x: left position in float.
    y: top position in float.
    w: width in float.
    y: height in float.



    See also: VxRect::SetCorners,VxRect::SetCenter
    *************************************************/
    void SetDimension(const Vx2DVector &position, const Vx2DVector &size)
    {
        left = position.x;
        top = position.y;
        right = left + size.x;
        bottom = top + size.y;
    }
    void SetDimension(float x, float y, float w, float h)
    {
        left = x;
        top = y;
        right = x + w;
        bottom = y + h;
    }

    /*************************************************
    Summary: Creates a rectangle based on the center position and the half size.

    Input Arguments:
    position: a Vx2DVector containing the center position.
    halfsize: a Vx2DVector containing half size.
    cx: horizontal center position in float.
    cy: vertical center position in float.
    hw: half width in float.
    hy: half height in float.



    See also: VxRect::SetCorners,VxRect::SetDimension
    *************************************************/
    void SetCenter(const Vx2DVector &center, const Vx2DVector &halfsize)
    {
        left = center.x - halfsize.x;
        top = center.y - halfsize.y;
        right = center.x + halfsize.x;
        bottom = center.y + halfsize.y;
    }
    void SetCenter(float cx, float cy, float hw, float hh)
    {
        left = cx - hw;
        top = cy - hh;
        right = cx + hw;
        bottom = cy + hh;
    }

    /*************************************************
    Summary: Assign the VxRect to a CKRECT.

    Input Arguments:
    iRect: the rectangle to copy from.
    *************************************************/
    void CopyFrom(const CKRECT &iRect)
    {
        left = (float)iRect.left;
        top = (float)iRect.top;
        right = (float)iRect.right;
        bottom = (float)iRect.bottom;
    }

    /*************************************************
    Summary: Assign the VxRect to a CKRECT.

    Input Arguments:
    oRect: the rectangle to copy to.
    *************************************************/
    void CopyTo(CKRECT *oRect) const
    {
        XASSERT(oRect);

        oRect->left = (int)left;
        oRect->top = (int)top;
        oRect->right = (int)right;
        oRect->bottom = (int)bottom;
    }

    /*************************************************
    Summary: Creates a rectangle bounding two given points.

    Input Arguments:
    p1: the first Vx2DVector.
    p2: the second Vx2DVector.



    See also: VxRect::SetCorners,VxRect::SetDimension
    *************************************************/
    void Bounding(const Vx2DVector &p1, const Vx2DVector &p2)
    {
        if (p1.x < p2.x)
        {
            left = p1.x;
            right = p2.x;
        }
        else
        {
            left = p2.x;
            right = p1.x;
        }
        if (p1.y < p2.y)
        {
            top = p1.y;
            bottom = p2.y;
        }
        else
        {
            top = p2.y;
            bottom = p1.y;
        }
    }

    /*************************************************
    Summary: Checks that the rectangle is valid. If not, turns it right.



    Remarks:
        Use this function when you create a rectangle with two
    corners without being sure that you provided the top left
    and top right corners, if this order.

    See also: VxRect::SetCorners,VxRect::SetDimension
    *************************************************/
    void Normalize()
    {
        // Check horizontally
        if (left > right)
            XSwap(right, left);

        // Check vertically
        if (top > bottom)
            XSwap(top, bottom);
    }

    /*************************************************
    Summary: Move a rectangle to a position.

    Input Arguments:
    pos: new position of the rectangle in Vx2DVector.



    See also: VxRect::Translate
    *************************************************/
    void Move(const Vx2DVector &pos)
    {
        right += (pos.x - left);
        bottom += (pos.y - top);
        left = pos.x;
        top = pos.y;
    }

    /*************************************************
    Summary: Translate a rectangle of an offset.

    Input Arguments:
    t: translation vector in Vx2DVector.



    See also: VxRect::Move
    *************************************************/
    void Translate(const Vx2DVector &t)
    {
        left += t.x;
        right += t.x;
        top += t.y;
        bottom += t.y;
    }

    void HMove(float h)
    {
        right += (h - left);
        left = h;
    }

    void VMove(float v)
    {
        bottom += (v - top);
        top = v;
    }

    void HTranslate(float h)
    {
        right += h;
        left += h;
    }

    void VTranslate(float v)
    {
        bottom += v;
        top += v;
    }

    /*************************************************
    Summary: Transform a point in homogeneous coordinates
    into a points in screen coordinates (the rect representing the screen).

    Input Arguments:
    dest: vector destination (in screen coordinates).
    srchom: src vector (in homogeneous coordinates).



    See also: VxRect::Move
    *************************************************/
    void TransformFromHomogeneous(Vx2DVector &dest, const Vx2DVector &srchom) const
    {
        dest.x = left + GetWidth() * srchom.x;
        dest.y = left + GetHeight() * srchom.y;
    }

    /*************************************************
    Summary: Scales a rectangle by a factor.

    Input Arguments:
    t: translation vector in Vx2DVector.



    See also: VxRect::Inflate
    *************************************************/
    void Scale(const Vx2DVector &s)
    {
        SetWidth(s.x * GetWidth());
        SetHeight(s.y * GetHeight());
    }

    /*************************************************
    Summary: Inflates a rectangle by an offset.

    Input Arguments:



    See also: VxRect::Scale
    *************************************************/
    void Inflate(const Vx2DVector &pt)
    {
        left -= pt.x;
        right += pt.x;
        top -= pt.y;
        bottom += pt.y;
    }

    /*************************************************
    Summary: Interpolates a rectangle with another one.

    Input Arguments:



    See also: VxRect::Scale
    *************************************************/
    void Interpolate(float value, const VxRect &a)
    {
        left += (a.left - left) * value;
        right += (a.right - right) * value;
        top += (a.top - top) * value;
        bottom += (a.bottom - bottom) * value;
    }

    /*************************************************
    Summary: Merge a rectangle with another one.

    Input Arguments:



    See also: VxRect::Scale
    *************************************************/
    void Merge(const VxRect &a)
    {
        if (a.left < left)
            left = a.left;
        if (a.right > right)
            right = a.right;
        if (a.top < top)
            top = a.top;
        if (a.bottom > bottom)
            bottom = a.bottom;
    }

    /*************************************************
    Summary: Tests a rectangle over a clipping rectangle.

    Input Arguments:
    cliprect: clipping rectangle.

    Return Value:
        ALLOUTSIDE if the two rectangles are distinct.
        PARTINSIDE if the two rectangles are crossing.
        ALLINSIDE  if the tested rectangle is inside the clipping rectangle.



    See also: VxRect::IsOutside
    *************************************************/
    int IsInside(const VxRect &cliprect) const
    {
        // entirely clipped
        if (left >= cliprect.right)
            return ALLOUTSIDE;
        if (right < cliprect.left)
            return ALLOUTSIDE;
        if (top >= cliprect.bottom)
            return ALLOUTSIDE;
        if (bottom < cliprect.top)
            return ALLOUTSIDE;

        // partially or not clipped
        if (left < cliprect.left)
            return PARTINSIDE;
        if (right > cliprect.right)
            return PARTINSIDE;
        if (top < cliprect.top)
            return PARTINSIDE;
        if (bottom > cliprect.bottom)
            return PARTINSIDE;

        return ALLINSIDE;
    }

    /*************************************************
    Summary: Tests a rectangle over a clipping rectangle.

    Input Arguments:
    cliprect: clipping rectangle.

    Return Value:
        TRUE if the rectangle is outside the clipping one,
    FALSE otherwise.



    See also: VxRect::IsInside
    *************************************************/
    BOOL IsOutside(const VxRect &cliprect) const
    {
        // entirely clipped
        if (left >= cliprect.right)
            return TRUE;
        if (right < cliprect.left)
            return TRUE;
        if (top >= cliprect.bottom)
            return TRUE;
        if (bottom < cliprect.top)
            return TRUE;

        return FALSE;
    }

    /*************************************************
    Summary: Tests a point for rectangle interiority.

    Input Arguments:
    pt: Vx2DVector to test.

    Return Value:
        TRUE if the point is inside, FALSE otherwise.



    See also: VxRect::IsOutside
    *************************************************/
    BOOL IsInside(const Vx2DVector &pt) const
    {
        if (pt.x < left)
            return FALSE;
        if (pt.x > right)
            return FALSE;
        if (pt.y < top)
            return FALSE;
        if (pt.y > bottom)
            return FALSE;
        return TRUE;
    }

    /*************************************************
    Summary: Tests a rectangle for validity.

    Return Value:
        TRUE if the rectangle is NULL, FALSE otherwise.



    See also: VxRect::IsEmpty, VxRect::IsOutside, VxRect::Clear
    *************************************************/
    BOOL IsNull() const { return (left == 0 && right == 0 && bottom == 0 && top == 0); }

    /*************************************************
    Summary: Tests if a rectangle is Empty (width==0 or height==0)

    Return Value:
        TRUE if the rectangle is Empty, FALSE otherwise.



    See also: VxRect::IsNull, VxRect::IsOutside, VxRect::Clear
    *************************************************/
    BOOL IsEmpty() const { return ((left == right) || (bottom == top)); }

    /*************************************************
    Summary: Clips a rectangle over a clipping rectangle.

    Input Arguments:
    cliprect: clipping rectangle.

    Return Value:
        TRUE if the rectangle is visible (partially clipped or not),
    FALSE otherwise.



    See also: VxRect::IsOutside,VxRect::IsInside
    *************************************************/
    BOOL Clip(const VxRect &cliprect)
    {
        // entirely clipped
        if (IsOutside(cliprect))
            return FALSE;

        // partially or not clipped
        if (left < cliprect.left)
            left = cliprect.left;
        if (right > cliprect.right)
            right = cliprect.right;
        if (top < cliprect.top)
            top = cliprect.top;
        if (bottom > cliprect.bottom)
            bottom = cliprect.bottom;

        return TRUE;
    }

    /*************************************************
    Summary: Clips a point over a rectangle.

    Input Arguments:
    pt: point to clip
    excluderightbottom: should the bottom and right line be considered as valid values for the point.



    See also: VxRect::IsOutside,VxRect::IsInside
    *************************************************/
    void Clip(Vx2DVector &pt, BOOL excluderightbottom = TRUE) const
    {
        if (pt.x < left)
            pt.x = left;
        else
        {
            if (pt.x >= right)
            {
                if (excluderightbottom)
                    pt.x = right - 1;
                else
                    pt.x = right;
            }
        }
        if (pt.y < top)
            pt.y = top;
        else
        {
            if (pt.y >= bottom)
            {
                if (excluderightbottom)
                    pt.y = bottom - 1;
                else
                    pt.y = bottom;
            }
        }
    }

    /*************************************************
    Summary: Transforms a rectangle from a screen referentiel to another.

    Input Arguments:
    destscreen: destination referential screen defined by a rectangle.
    srcscreen: source referential screen defined by a rectangle.
    destscreen: destination referential screen size.
    srcscreen: source referential screen size.



    See also: VxRect::TransformToHomogeneous
    *************************************************/
    VX_EXPORT void Transform(const VxRect &destScreen, const VxRect &srcScreen);
    VX_EXPORT void Transform(const Vx2DVector &destScreenSize, const Vx2DVector &srcScreenSize);

    /*************************************************
    Summary: Transforms a screen coordinate rectangle to an homogeneous rectangle.

    Input Arguments:
    screen: Vx2DVector screen size.



    See also: VxRect::Transform
    *************************************************/
    VX_EXPORT void TransformToHomogeneous(const VxRect &screen);

    /*************************************************
    Summary: Transforms an homogeneous rectangle to a screen coordinate rectangle.

    Input Arguments:
    screen: Vx2DVector screen size.



    See also: VxRect::Transform
    *************************************************/
    VX_EXPORT void TransformFromHomogeneous(const VxRect &screen);

    // Operator with 2DVectors

    VxRect &operator+=(const Vx2DVector &t)
    {
        left += t.x;
        right += t.x;
        top += t.y;
        bottom += t.y;
        return *this;
    }
    VxRect &operator-=(const Vx2DVector &t)
    {
        left -= t.x;
        right -= t.x;
        top -= t.y;
        bottom -= t.y;
        return *this;
    }
    VxRect &operator*=(const Vx2DVector &t)
    {
        left *= t.x;
        right *= t.x;
        top *= t.y;
        bottom *= t.y;
        return *this;
    }
    VxRect &operator/=(const Vx2DVector &t)
    {
        float x = 1.0f / t.x;
        float y = 1.0f / t.y;
        left *= x;
        right *= x;
        top *= y;
        bottom *= y;
        return *this;
    }

    //
    friend int operator==(const VxRect &r1, const VxRect &r2);

    //
    friend int operator!=(const VxRect &r1, const VxRect &r2);
};

//
inline int operator==(const VxRect &r1, const VxRect &r2)
{
    return (r1.left == r2.left && r1.right == r2.right && r1.top == r2.top && r1.bottom == r2.bottom);
}

//
inline int operator!=(const VxRect &r1, const VxRect &r2)
{
    return !(r1 == r2);
}

#endif
