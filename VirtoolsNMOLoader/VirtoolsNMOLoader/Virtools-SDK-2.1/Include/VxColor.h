/*************************************************************************/
/*	File : VxColor.h													 */
/*	Author :  Romain SIDIDRIS											 */
/*																		 */
/*	Virtools SDK 														 */
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */
/*************************************************************************/
#ifndef VXCOLOR_H
#define VXCOLOR_H

#include "XUtil.h"

#ifdef DOCJETDUMMY // DOCJET secret macro
#else

#define R_SHIFT 16
#define G_SHIFT 8
#define B_SHIFT 0
#define A_SHIFT 24

#define R_MASK 0x00FF0000
#define G_MASK 0x0000FF00
#define B_MASK 0x000000FF
#define A_MASK 0xFF000000

#endif

struct VxColor;

/***************************************************
Summary: Converts 4 floats to a DWORD ARGB color

Remarks:
This function takes 4 floats R,G,B,A (in the range 0..1) and converts them into
a color stored in DWORD in 32 bit ARGB format.
*****************************************************/
VX_EXPORT unsigned long RGBAFTOCOLOR(float r, float g, float b, float a);
VX_EXPORT unsigned long RGBAFTOCOLOR(const VxColor *col);

/***************************************************
Summary: Converts 4 floats to a DWORD BGRA color

Remarks:
This function takes 4 floats R,G,B,A (in the range 0..1) and converts them into
a color stored in DWORD in 32 bit BGRA format.
*****************************************************/
VX_EXPORT unsigned long BGRAFTOCOLOR(const VxColor *col);

/***************************************************
Summary: Convers 4 integers to a DWORD ARGB color
Remarks:
This macro takes 4 integer R,G,B,A (in the range 0..255) and convert them
into a color stored in DWORD in 32 bit ARGB format.

******************************************************/
#define RGBAITOCOLOR(r, g, b, a) (((a) << A_SHIFT) | ((r) << R_SHIFT) | ((g) << G_SHIFT) | ((b) << B_SHIFT))

/***************************************************
Summary:  Extracts Red component from a 32 bit ARGB color.

******************************************************/
#define ColorGetRed(rgb) (((rgb) >> R_SHIFT) & 0xffL)

/***************************************************
Summary:  Extracts Alpha component from a 32 bit ARGB color.

******************************************************/
#define ColorGetAlpha(rgb) (((rgb) >> A_SHIFT) & 0xffL)

/***************************************************
Summary:  Extracts Green component from a 32 bit ARGB color.

******************************************************/
#define ColorGetGreen(rgb) (((rgb) >> G_SHIFT) & 0xffL)

/***************************************************
Summary:  Extracts Blue component from a 32 bit ARGB color.

******************************************************/
#define ColorGetBlue(rgb) (((rgb) >> B_SHIFT) & 0xffL)

/***************************************************
Summary:  Sets Alpha component in a 32 bit ARGB color and returns the color.

******************************************************/
#define ColorSetAlpha(rgba, x) (((x) << A_SHIFT) | ((rgba) & ~A_MASK))

/***************************************************
Summary:  Sets Red component in a 32 bit ARGB color and returns the color.

******************************************************/
#define ColorSetRed(rgba, x) (((x) << R_SHIFT) | ((rgba) & ~R_MASK))

/***************************************************
Summary:  Sets Green component in a 32 bit ARGB color and returns the color.

******************************************************/
#define ColorSetGreen(rgba, x) (((x) << G_SHIFT) | ((rgba) & ~G_MASK))

/***************************************************
Summary:  Sets Blue component in a 32 bit ARGB color and returns the color.

******************************************************/
#define ColorSetBlue(rgba, x) (((x) << B_SHIFT) | ((rgba) & ~B_MASK))

/*****************************************************************
{filename:VxColor}
Name: VxColor

Summary: Structure describing a color through 4 floats.

Remarks:
Structure describing a color through 4 floats for each component
Red, Green, Blue and Alpha.

Most methods are used to construct a VxColor or to convert it
to a 32 but ARGB format.

A VxColor is defined as:

             typedef struct VxColor {
                union {
                   struct {
                           float r,g,b,a;
                   };
                   float col[4];
                };
             }


*******************************************************************/
struct VxColor
{
    union
    {
        struct
        {
            float r, g, b, a;
        };
        float col[4];
    };

public:
    VxColor();

    VxColor(const float _r, const float _g, const float _b, const float _a);
    VxColor(const float _r, const float _g, const float _b);
    VxColor(const float _r);
    VxColor(const unsigned long col);
    VxColor(const int _r, const int _g, const int _b, const int _a);
    VxColor(const int _r, const int _g, const int _b);

    void Clear();
    // Ensure that every component is clamped to [0..1]
    void Check()
    {
        if (r > 1.0f)
            r = 1.0f;
        else if (r < 0.0f)
            r = 0.0f;
        if (g > 1.0f)
            g = 1.0f;
        else if (g < 0.0f)
            g = 0.0f;
        if (b > 1.0f)
            b = 1.0f;
        else if (b < 0.0f)
            b = 0.0f;
        if (a > 1.0f)
            a = 1.0f;
        else if (a < 0.0f)
            a = 0.0f;
    }
    void Set(const float _r, const float _g, const float _b, const float _a);
    void Set(const float _r, const float _g, const float _b);
    void Set(const float _r);
    void Set(const unsigned long col);
    void Set(const int _r, const int _g, const int _b, const int _a);
    void Set(const int _r, const int _g, const int _b);
    unsigned long GetRGBA() const;
    unsigned long GetRGB() const;
    float GetSquareDistance(const VxColor &color) const;

    VxColor &operator+=(const VxColor &v);
    VxColor &operator-=(const VxColor &v);
    VxColor &operator*=(const VxColor &v);
    VxColor &operator/=(const VxColor &v);
    VxColor &operator*=(float s);
    VxColor &operator/=(float s);

    friend VxColor operator-(const VxColor &col1, const VxColor &col2);
    friend VxColor operator*(const VxColor &col1, const VxColor &col2);
    friend VxColor operator+(const VxColor &col1, const VxColor &col2);
    friend VxColor operator/(const VxColor &col1, const VxColor &col2);
    friend VxColor operator*(const VxColor &col, float s);

    // Bitwise equality
    friend int operator==(const VxColor &col1, const VxColor &col2);
    friend int operator!=(const VxColor &col1, const VxColor &col2);

    static unsigned long Convert(float _r, float _g, float _b, float _a = 1.0f)
    {
        XThreshold(_r, 0.0f, 1.0f);
        XThreshold(_g, 0.0f, 1.0f);
        XThreshold(_b, 0.0f, 1.0f);
        XThreshold(_a, 0.0f, 1.0f);
        return RGBAFTOCOLOR(_r, _g, _b, _a);
    }
    static unsigned long Convert(int _r, int _g, int _b, int _a = 255)
    {
        XThreshold(_r, 0, 255);
        XThreshold(_g, 0, 255);
        XThreshold(_b, 0, 255);
        XThreshold(_a, 0, 255);
        return RGBAITOCOLOR(_r, _g, _b, _a);
    }
};

// Constructors
inline VxColor::VxColor(const float _r, const float _g, const float _b, const float _a)
{
    r = _r;
    g = _g;
    b = _b;
    a = _a;
}

inline VxColor::VxColor()
{
    r = g = b = a = 0.0f;
}

inline VxColor::VxColor(const int _r, const int _g, const int _b, const int _a)
{
    r = (float)_r / 255.0f;
    g = (float)_g / 255.0f;
    b = (float)_b / 255.0f;
    a = (float)_a / 255.0f;
}

inline VxColor::VxColor(const int _r, const int _g, const int _b)
{
    r = (float)_r / 255.0f;
    g = (float)_g / 255.0f;
    b = (float)_b / 255.0f;
    a = 1.0f;
}

inline VxColor::VxColor(const float _r, const float _g, const float _b)
{
    r = _r;
    g = _g;
    b = _b;
    a = 1.0f;
}

inline VxColor::VxColor(const float _r)
{
    r = _r;
    g = _r;
    b = _r;
    a = 1.0f;
}

inline VxColor::VxColor(const unsigned long colz)
{
    r = (float)(ColorGetRed(colz)) * 0.003921568627f; // 1/255
    g = (float)(ColorGetGreen(colz)) * 0.003921568627f;
    b = (float)(ColorGetBlue(colz)) * 0.003921568627f;
    a = (float)(ColorGetAlpha(colz)) * 0.003921568627f;
}

// Returns square distance between two colors.
inline float VxColor::GetSquareDistance(const VxColor &color) const
{
    VxColor col = color - *this;
    return (col.r * col.r + col.g * col.g + col.b * col.b);
}

// Sets all components to 0
inline void VxColor::Clear()
{
    r = g = b = a = 0.0f;
}

// Initializations
inline void VxColor::Set(const float _r, const float _g, const float _b, const float _a)
{
    r = _r;
    g = _g;
    b = _b;
    a = _a;
}

inline void VxColor::Set(const int _r, const int _g, const int _b, const int _a)
{
    r = (float)_r / 255.0f;
    g = (float)_g / 255.0f;
    b = (float)_b / 255.0f;
    a = (float)_a / 255.0f;
}

inline void VxColor::Set(const int _r, const int _g, const int _b)
{
    r = (float)_r / 255.0f;
    g = (float)_g / 255.0f;
    b = (float)_b / 255.0f;
    a = 1.0f;
}

inline void VxColor::Set(const float _r, const float _g, const float _b)
{
    r = _r;
    g = _g;
    b = _b;
    a = 1.0f;
}

inline void VxColor::Set(const float _r)
{
    r = _r;
    g = _r;
    b = _r;
    a = 1.0f;
}

inline void VxColor::Set(const unsigned long colz)
{
    r = (float)(ColorGetRed(colz)) / 255.0f;
    g = (float)(ColorGetGreen(colz)) / 255.0f;
    b = (float)(ColorGetBlue(colz)) / 255.0f;
    a = (float)(ColorGetAlpha(colz)) / 255.0f;
}

inline VxColor &VxColor::operator+=(const VxColor &v)
{
    r += v.r;
    g += v.g;
    b += v.b;
    a += v.a;
    return *this;
}

inline VxColor &VxColor::operator-=(const VxColor &v)
{
    r -= v.r;
    g -= v.g;
    b -= v.b;
    a -= v.a;
    return *this;
}

inline VxColor &VxColor::operator*=(const VxColor &v)
{
    r *= v.r;
    g *= v.g;
    b *= v.b;
    a *= v.a;
    return *this;
}

inline VxColor &VxColor::operator/=(const VxColor &v)
{
    r /= v.r;
    g /= v.g;
    b /= v.b;
    a /= v.a;
    return *this;
}

inline VxColor &VxColor::operator*=(float s)
{
    r *= s;
    g *= s;
    b *= s;
    a *= s;
    return *this;
}

inline VxColor &VxColor::operator/=(float s)
{
    r /= s;
    g /= s;
    b /= s;
    a /= s;
    return *this;
}

inline VxColor operator-(const VxColor &col1, const VxColor &col2)
{
    return VxColor(col1.r - col2.r, col1.g - col2.g, col1.b - col2.b, col1.a - col2.a);
}

inline VxColor operator+(const VxColor &col1, const VxColor &col2)
{
    return VxColor(col1.r + col2.r, col1.g + col2.g, col1.b + col2.b, col1.a + col2.a);
}

inline VxColor operator*(const VxColor &col1, const VxColor &col2)
{
    return VxColor(col1.r * col2.r, col1.g * col2.g, col1.b * col2.b, col1.a * col2.a);
}

inline VxColor operator/(const VxColor &col1, const VxColor &col2)
{
    return VxColor(col1.r / col2.r, col1.g / col2.g, col1.b / col2.b, col1.a / col2.a);
}

inline VxColor operator*(const VxColor &col, float s)
{
    return VxColor(s * col.r, s * col.g, s * col.b, s * col.a);
}

inline int operator==(const VxColor &col1, const VxColor &col2)
{
    return (col1.r == col2.r && col1.g == col2.g && col1.b == col2.b && col1.a == col2.a);
}

inline int operator!=(const VxColor &col1, const VxColor &col2)
{
    return (col1.r != col2.r || col1.g != col2.g || col1.b != col2.b || col1.a != col2.a);
}

// Returns the color in a DWORD in the 32 bit format ARGB
inline unsigned long VxColor::GetRGBA() const
{
    return RGBAFTOCOLOR(this);
}

// Returns the color in a DWORD in the 32 bit format ARGB and sets alpha to 255
inline unsigned long VxColor::GetRGB() const
{
    return RGBAFTOCOLOR(this) | A_MASK;
}

#endif
