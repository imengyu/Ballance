#ifndef _VX_MATH_DEFINES_H_

#define _VX_MATH_DEFINES_H_

 
#ifndef EPSILON
	#define EPSILON		1.192092896e-07F	
#endif

#ifndef PI
#define PI		3.1415926535f	
#endif

#ifndef HALFPI
#define HALFPI	1.5707963267f	
#endif

#ifndef HALF_RANDMAX

#define HALF_RANDMAX		0x3fff
#endif

#ifndef INVHALF_RANDMAX

	#if (defined(macintosh) && defined(__GNUC__))
		#define INVHALF_RANDMAX	9.31322575e-10f
	#else
		#define INVHALF_RANDMAX	6.10389e-005f
	#endif
#endif

#ifndef INV_RANDMAX

	#if (defined(macintosh) && defined(__GNUC__))
		#define INV_RANDMAX		4.65661287e-10f	
	#else
		#define INV_RANDMAX		3.05185e-005f
	#endif
#endif

#define _8192ONPI		2607.594587617613f

#ifndef FALSE 

#define FALSE 0
#endif

#ifndef TRUE

#define TRUE 1
#endif

#ifndef NULL

#define NULL 0
#endif

#ifdef WIN32
	typedef unsigned long	DWORD;	
#endif

#ifdef macintosh
	typedef unsigned long	DWORD;  
#endif

#ifdef _XBOX
	typedef unsigned long	DWORD;	
#endif

#if defined(PSX2) || defined(PSP)
	typedef unsigned int	DWORD;	
#endif

typedef int				BOOL;	
typedef unsigned char	BYTE;	
typedef unsigned short	WORD;	
typedef long			LONG;	
typedef void*			LPVOID;	
typedef void* WIN_HANDLE;		
typedef void* INSTANCE_HANDLE;	
typedef void* GENERIC_HANDLE;	
typedef void* BITMAP_HANDLE;	
typedef void* FONT_HANDLE;		


typedef struct CKRECT
{
    int    left;
    int    top;
    int    right;
    int    bottom;
} CKRECT;

typedef struct {  int x,y; } CKPOINT;


class VxMatrix;
struct VxStridedData;

/************************************************
Summary: Structure for storage of strided data.


************************************************/
typedef struct VxStridedDataBase {
	union {
		void*			Ptr;
		unsigned char*	CPtr;
	};
	unsigned int	Stride;
	operator const VxStridedData& () const { return *(const VxStridedData*)this; }
} VxStridedDataBase;


/************************************************
Summary: Structure for storage of strided data.


************************************************/
typedef struct VxStridedData : public VxStridedDataBase  {
	VxStridedData() { Ptr = 0; Stride = 0; }
	VxStridedData(void* iPtr,unsigned int iStride) { Ptr = iPtr; Stride = iStride; }
} VxStridedData;


/************************************************
Name: ProcessorType

Summary: Enumerations of different processor types.

The processor type and frequency are detected at startup of any program using VxMath library.



See also: GetProcessorType
************************************************/
typedef enum ProcessorsType
{
		PROC_UNKNOWN		=-1,	// Processor type can not be detected
		PROC_PENTIUM		=0,		// Standard Pentium Processor
		PROC_PENTIUMMMX		=1,		// Standard Pentium Processor with MMX support
		PROC_PENTIUMPRO		=2,		// Intel Pentium Pro Processor
		PROC_K63DNOW		=3,		// AMD K6 + 3DNow ! support
		PROC_PENTIUM2		=4,		// Intel Pentium II Processor Model 3
		PROC_PENTIUM2XEON	=5,		// Intel Pentium II Processor Xeon or Celeron Model 5
		PROC_PENTIUM2CELERON=6,		// Intel Pentium II Processor Celeron Model 6
		PROC_PENTIUM3		=7,		// Intel Pentium III Processor
		PROC_ATHLON			=9,		// AMD K7 Athlon Processor
		PROC_PENTIUM4		=10,	// Intel Pentium 4  Processor
		PROC_PPC_ARM		=11,	// Pocket PC ARM Device
		PROC_PPC_MIPS		=12,	// Pocket PC MIPS Device
		PROC_PPC_G3			=13,	// Power  PC G3
		PROC_PPC_G4			=14,	// Power  PC G4
		PROC_PSX2			=15,	// MIPS PSX2
		PROC_XBOX2			=16,		// XBOX2 CPU
    PROC_PSP			  =17		// XBOX2 CPU
} ProcessorsType;


#define NB_STDPIXEL_FORMATS 19
#ifdef PSX2
	
	#define MAX_PIXEL_FORMATS	34
#else
	#define MAX_PIXEL_FORMATS	40
#endif
/*****************************************************************
Name: VX_OSINFO

Summary: Operating systems enumeration. 
See Also: VxGetOs
******************************************************************/
typedef enum VX_OSINFO {
	VXOS_UNKNOWN,
	VXOS_WIN31,
	VXOS_WIN95,
	VXOS_WIN98,
	VXOS_WINME,
	VXOS_WINNT4,
	VXOS_WIN2K,
	VXOS_WINXP,
	VXOS_MACOS9,
	VXOS_MACOSX,
	VXOS_XBOX,
	VXOS_LINUXX86,
	VXOS_WINCE1,
	VXOS_WINCE2,
	VXOS_WINCE3,
	VXOS_PSX2,
	VXOS_XBOX2,
	VXOS_WINVISTA,
	VXOS_PSP,
	VXOS_XBOX360,
	VXOS_WII,
	VXOS_WINSEVEN
} VX_OSINFO; 

/*****************************************************************
{filename:VX_PLATFORMINFO}
Name: VX_PLATFORMINFO

Summary: Platform enumeration. 
See Also: VxGetPlatform
******************************************************************/
typedef enum VX_PLATFORMINFO {
	VXPLATFORM_UNKNOWN = -1,
	VXPLATFORM_WINDOWS = 0,
	VXPLATFORM_MAC = 1,
	VXPLATFORM_XBOX = 2,
	VXPLATFORM_WINCE = 3,
	VXPLATFORM_LINUX = 4,
	VXPLATFORM_PSX2 = 5,
	VXPLATFORM_XBOX2 = 6,
	VXPLATFORM_PSP = 7,
	VXPLATFORM_WII = 8,
} VX_PLATFORMINFO; 


/*****************************************************************
{filename:VX_PIXELFORMAT}
Name: VX_PIXELFORMAT

Summary: Pixel format types. 
See Also: VxImageDesc2PixelFormat,VxPixelFormat2ImageDesc
******************************************************************/
typedef enum VX_PIXELFORMAT {
	UNKNOWN_PF	  = 0,			// Unknown pixel format
	_32_ARGB8888  = 1,			// 32-bit ARGB pixel format with alpha
	_32_RGB888	  = 2,			// 32-bit RGB pixel format without alpha
	_24_RGB888	  = 3,			// 24-bit RGB pixel format
	_16_RGB565	  = 4,			// 16-bit RGB pixel format
	_16_RGB555	  = 5,			// 16-bit RGB pixel format (5 bits per color)
	_16_ARGB1555  = 6,			// 16-bit ARGB pixel format (5 bits per color + 1 bit for alpha)
	_16_ARGB4444  = 7,			// 16-bit ARGB pixel format (4 bits per color)	
	_8_RGB332     = 8,			// 8-bit  RGB pixel format
	_8_ARGB2222   = 9,			// 8-bit  ARGB pixel format
	_32_ABGR8888  = 10,			// 32-bit ABGR pixel format
	_32_RGBA8888  = 11,			// 32-bit RGBA pixel format
	_32_BGRA8888  = 12,			// 32-bit BGRA pixel format
	_32_BGR888	  = 13,			// 32-bit BGR pixel format
	_24_BGR888	  = 14,			// 24-bit BGR pixel format
	_16_BGR565	  = 15,			// 16-bit BGR pixel format
	_16_BGR555	  = 16,			// 16-bit BGR pixel format (5 bits per color)
	_16_ABGR1555  = 17,			// 16-bit ABGR pixel format (5 bits per color + 1 bit for alpha)
	_16_ABGR4444  = 18,			// 16-bit ABGR pixel format (4 bits per color)	
	_DXT1		  = 19,			// S3/DirectX Texture Compression 1	
	_DXT2		  = 20,			// S3/DirectX Texture Compression 2	
	_DXT3		  = 21,			// S3/DirectX Texture Compression 3	
	_DXT4		  = 22,			// S3/DirectX Texture Compression 4	
	_DXT5		  = 23,			// S3/DirectX Texture Compression 5	
	_16_V8U8	  = 24,			// 16-bit Bump Map format format (8 bits per color)	
	_32_V16U16	  = 25,			// 32-bit Bump Map format format (16 bits per color)	
	_16_L6V5U5	  = 26,			// 16-bit Bump Map format format with luminance
	_32_X8L8V8U8  = 27,			// 32-bit Bump Map format format with luminance

// Floating Textures

	_16_R16F		  = 28,     // 1*16 bits floating point
	_32_GR16F		  = 29,		// 2*16 bits floating point
	_64_ABGR16F		  = 30,		// 4*16 bits floating point	

	_32_R32F		  = 31,     // 1*32 bits floating point IEEE
	_64_GR32F		  = 32,		// 2*32 bits floating point IEEE
	_128_ABGR32F	  = 33,		// 4*32 bits floating point	IEEE

// Clut Formats
	_8_ABGR8888_CLUT  = 34,		// 8 bits indexed CLUT (ABGR)
	_8_ARGB8888_CLUT  = 35,		// 8 bits indexed CLUT (ARGB)
	_4_ABGR8888_CLUT  = 36,		// 4 bits indexed CLUT (ABGR)
	_4_ARGB8888_CLUT  = 37,		// 4 bits indexed CLUT (ARGB)

// Hardware shadow maps formats
	_32_D24_S8 = 38, // 24 bits shadow map with 8 bits for stencil
	_16_D16 = 39     // 16 bits shadow map

} VX_PIXELFORMAT;

/*****************************************************************
{filename:VXCLIP_FLAGS}
Summary: Vertex clipping flags. 

Remarks:
When using functions such as VxTransformBox2D which performs frustum tests, vertices are assigned
clipping flags to indicate in which part of the viewing frustum they are.
See Also: VxTransformBox2D
******************************************************************/
typedef enum VXCLIP_FLAGS 
{
	VXCLIP_LEFT		=	0x00000010,			// Vertex is clipped by the left viewing plane 
	VXCLIP_RIGHT	=	0x00000020,			// Vertex is clipped by the right viewing plane 
	VXCLIP_TOP		=	0x00000040,			// Vertex is clipped by the top viewing plane 
	VXCLIP_BOTTOM	=	0x00000080,			// Vertex is clipped by the bottom viewing plane 
	VXCLIP_FRONT	=	0x00000100,			// Vertex is clipped by the front viewing plane 
	VXCLIP_BACK		=	0x00000200,			// Vertex is clipped by the rear viewing plane 
	VXCLIP_BACKFRONT=	0x00000300,			// Combination of BACK and FRONT flags 
	VXCLIP_ALL		=	0x000003F0,			// All flags Combined
} VXCLIP_FLAGS;


enum VXCLIP_BOXFLAGS {
	VXCLIP_BOXLEFT		= 0x01,
	VXCLIP_BOXBOTTOM	= 0x02,
	VXCLIP_BOXBACK		= 0x04,
	VXCLIP_BOXRIGHT		= 0x08,
	VXCLIP_BOXTOP		= 0x10,
	VXCLIP_BOXFRONT		= 0x20
};

/************************************************
Name: ProcessorFeatures

Summary: Enumerations of different processor features.

The processor type and frequency are detected at startup of any program using VxMath library.



See also: GetProcessorFeatures,ModifyProcessorFeatures
************************************************/
typedef enum ProcessorsFeatures
{
	PROC_HASFPU		= 0x00000001,	// Fpu on Chip
	PROC_V86		= 0x00000002,	// 
	PROC_DE			= 0x00000004,	// Debugging Extensions
	PROC_PSE		= 0x00000008,	// Page size Extensions
	PROC_TIMESTAMP	= 0x00000010,	// Time stamp counter available
	PROC_MSR		= 0x00000020,	// Model specific register 
	PROC_PAE		= 0x00000040,	// 
	PROC_MCE		= 0x00000080,	// 
	PROC_CMPXCHG8B	= 0x00000100,	// CMPXCHG8B instruction available
	PROC_APIC		= 0x00000200,	// APIC on Chip
	PROC_RESERVED	= 0x00000400,	// 
	PROC_SEP		= 0x00000800,	// 
	PROC_MTRR		= 0x00001000,	//
	PROC_PGE		= 0x00002000,	//
	PROC_MCA		= 0x00004000,	//
	PROC_CMOV		= 0x00008000,	//  CMOVcc instructions
	PROC_PAT		= 0x00010000,	//
	PROC_PST32		= 0x00020000,	//
	PROC_PN			= 0x00040000,	// Processor Number
	PROC_MMX		= 0x00800000,	// MMX instructions available
	PROC_FXSR		= 0x01000000,	// Fast state save/restore
	PROC_SIMD		= 0x02000000,	// SIMD instructions available
	PROC_WNI		= 0x04000000,	// Willamette new instructions available
	PROC_SS			= 0x08000000,	// Self snoop
	PROC_HTT		= 0x10000000,	// Hyper Threading Technology
	PROC_TM			= 0x20000000,	// Thermal Moitoring
} ProcessorsFeatures;

#endif
