/*************************************************************************/
/*	File : VxMath.h														 */
/*																		 */	
/*	Main Header file for VxMath utility library							 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef VXMATH_H

#define VXMATH_H

//#pragma message ("VxMath::VxMath.h - #pragma warning(disable:4996)")
#pragma warning(disable:4996)

#if defined (PSX2) || defined(PSP)  
#include "VxMathPS2.h"
#endif

#ifdef _WIN32_WCE 
#include "VxMathCE.h"
#endif

#ifdef _XBOX
#define USE_PIX
#include <XTL.h>
#endif

#include <stdlib.h>
#include <string.h>
#include <math.h>	


#ifdef VX_LIB

#define VX_EXPORT  
#else
#ifdef VX_API
#if defined(WIN32) || defined(_XBOX)
#define VX_EXPORT __declspec(dllexport)	// VC++ export option  {secret}
#elif defined(macintosh)
	#ifdef __MWERKS_
		#define VX_EXPORT __declspec(export)	// CodeWarrior export pragma {secret}
	#else
		#define VX_EXPORT
	#endif
#elif _LINUX
#define VX_EXPORT
#else
#define VX_EXPORT
#endif	
#else
#if defined(WIN32) || defined(_XBOX)
#define VX_EXPORT __declspec(dllimport)	// VC++ export option  {secret}
#elif macintosh
//by nicolasp
	#ifdef __MWERKS__
		#define VX_EXPORT __declspec(import)	// CodeWarrior export pragma {secret}
	#else
		#define VX_EXPORT
	#endif
#elif _LINUX
#define VX_EXPORT
#else
#define VX_EXPORT
#endif	
#endif
#endif

#include "VxMathDefines.h"

#ifndef VX_MEM_RELEASE
#ifndef VX_LIB
#ifndef _XBOX
#ifndef _WIN32_WCE
#ifdef _DEBUG  
#if defined(WIN32)
#include "mmgr.h"	
#endif // WIN32
#endif // _DEBUG		
#endif	// WINCE
#endif	// XBOX
#endif
#endif


//*************** X86 ASM DEFINES *************************************************

#if defined(WIN32) || defined(_XBOX)

#ifndef _WIN32_WCE

#if (_XBOX_VER<200)
#define ASM_SUPPORTED

// This is defined in VxMath.h header file and should be removed
// if your compiler does not support SIMD (Intel Pentium III) instructions
// to compile some of the sample behaviors that use such instructions...
// otherwise you can install the Visual C++ Processor Pack (available in VC++ Service Pack 4 and 5)
// that supports these instructions
#define SIMD_SUPPORTED
#else
#pragma bitfield_order( lsb_to_msb ) 
#endif
#endif
#endif

#ifdef DOCJET_DUMMY // Prevent doc processing
#else


//*************** MACINTOSH DEFINES *************************************************


#ifdef macintosh
//by nicolasp
	#ifdef __MWERKS__
		#pragma warn_unusedvar off
		#pragma cpp_extensions on
		#pragma enumsalwaysint on
		
		#include <timer.h>		
	#endif
	
#include <time.h>

#include "VxMacHeader.h"
#undef SIMD_SUPPORTED
#endif

//*************** PSX2 DEFINES *************************************************

#ifdef PSX2

#endif

//*************** PSP DEFINES *************************************************

#ifdef PSP
#define VXTRY
#define VXCATCH(a) if(0)
#define VXTHROW  XASSERT(false);	
#endif

//*************** PSX2 DEFINES *************************************************


//*************** XBOX DEFINES *************************************************
#ifdef _XBOX

// Defines the level of pix profiling
#define PIX_RENDER_LEVEL	1
#define PIX_BEHAVIOR_LEVEL	1

#if (_XBOX_VER>=200)		
	class	PixEventMarker{
	public:
		PixEventMarker(const char*str ,DWORD Color ){
			PIXBeginNamedEvent( Color,str);					
		}
		~PixEventMarker(){
			PIXEndNamedEvent();
		}	
	};

	#define VXTRY
	#define VXCATCH(a) if(0)
	#define VXTHROW  XASSERT(false);	

#else
	#define HAVE_BOOLEAN
	#include <D3d8perf.h>

	class	PixEventMarker{
	public:
		PixEventMarker(const char*str ,DWORD Color ){
			D3DPERF_BeginEvent( Color,str);					
		}
		~PixEventMarker(){
			D3DPERF_EndEvent();
		}	
	};		
#endif // XBOX>=200

#define PIXE(a) PixEventMarker pixe##__COUNTER__(a,D3DCOLOR_ARGB(0xff,0,0,0xff));


#endif // XBOX

#if defined(_DEBUG) && defined(WIN32) && defined(VIRTOOLS_PIX)
#pragma message("PIXE FOR WIN32 enabled")
	#define PIXE(a) PixEventMarker pixe##__COUNTER__(a,D3DCOLOR_ARGB(0xff,0,0,0xff));
	#include "\\DevServervir\Dev\dxsdk9\Include\d3d9.h"
	typedef int  (__stdcall *LPD3DPERF_BEGINEVENT)  (D3DCOLOR, LPCWSTR);

	class	PixEventMarker{
	public:
		PixEventMarker(char* str ,DWORD Color ){			
			int sz = strlen(str)+1;	
			WCHAR *tmp = new WCHAR[sz];
			tmp[0] = '\0';
			MultiByteToWideChar(CP_ACP,0,str,-1,tmp,sz);
			D3DPERF_BeginEvent(Color,tmp)
			/*
			// That seems a bit overkill for a performance monitoring function !!!!
			char szPath[MAX_PATH+1];
			if( ::GetSystemDirectory( szPath, MAX_PATH+1 ) ) {
				LPD3DPERF_BEGINEVENT s_DynamicD3DPERF_BeginEvent = NULL;
				lstrcat( szPath, "\\d3d9.dll" );
				HMODULE s_hModD3D9 = NULL;
				s_hModD3D9 = LoadLibrary( szPath );
				if(s_hModD3D9) {
					s_DynamicD3DPERF_BeginEvent = (LPD3DPERF_BEGINEVENT)GetProcAddress( s_hModD3D9, "D3DPERF_BeginEvent" );
					if (s_DynamicD3DPERF_BeginEvent)
						s_DynamicD3DPERF_BeginEvent(Color,tmp);
				}				
			}			
			*/
			delete[] tmp;
			
		}
		~PixEventMarker(){
			D3DPERF_EndEvent();
		}	
	};		
#endif

#if defined(PSP) && defined(NDEBUG)
#ifndef __MWERKS__
	#include <stdio.h>
	#include <snTuner.h>
	
	extern bool g_bVtDebugTrace;
	
	class	PixEventMarker{
	public:
		PixEventMarker(const char*str){			 
			 snStartMarker(s_snmark, str);
			 s_snmark++;
		}
		~PixEventMarker(){	
			s_snmark--;
			snStopMarker (s_snmark);			
		}	
		
		static DWORD s_snmark;
	};		
	#define PIXE(a) PixEventMarker pixe##__COUNTER__(a);
	#define SN_TUNER_ENABLED 1

#endif // !MWERKS
#endif // PSP

#ifndef PIXE
	#define PIXE(a)
#endif

#if PIX_RENDER_LEVEL >= 1
#define PIXE_RENDER_L1 PIXE
#else
#define PIXE_RENDER_L1(a) 
#endif

#if PIX_RENDER_LEVEL >= 2
#define PIXE_RENDER_L2 PIXE
#else
#define PIXE_RENDER_L2(a) 
#endif

#if PIX_RENDER_LEVEL >= 3
#define PIXE_RENDER_L3 PIXE
#else
#define PIXE_RENDER_L3(a) 
#endif

#if PIX_RENDER_LEVEL >= 4
#define PIXE_RENDER_L4 PIXE
#else
#define PIXE_RENDER_L4(a) 
#endif

#if PIX_RENDER_LEVEL >= 5
#define PIXE_RENDER_L5 PIXE
#else
#define PIXE_RENDER_L5(a) 
#endif

// Pix Behavior

#if PIX_BEHAVIOR_LEVEL >= 1
#define PIXE_BEHAVIOR_L1(a) PIXE(a)
#else
#define PIXE_BEHAVIOR_L1(a) 
#endif

#if PIX_BEHAVIOR_LEVEL >= 2
#define PIXE_BEHAVIOR_L2 PIXE
#else
#define PIXE_BEHAVIOR_L2(a) 
#endif

#if PIX_BEHAVIOR_LEVEL >= 3
#define PIXE_BEHAVIOR_L3 PIXE
#else
#define PIXE_BEHAVIOR_L3(a) 
#endif

#if PIX_BEHAVIOR_LEVEL >= 4
#define PIXE_BEHAVIOR_L4 PIXE
#else
#define PIXE_BEHAVIOR_L4(a) 
#endif

#if PIX_BEHAVIOR_LEVEL >= 5
#define PIXE_BEHAVIOR_L5 PIXE
#else
#define PIXE_BEHAVIOR_L5(a) 
#endif

#ifdef PSP

	#include <malloc.h>
	#include <kernel.h>

	class VxMemoryReporter{
	public:
		VxMemoryReporter(const char* s){
			unsigned char GPI = sceKernelGetGPI();
			if(!(GPI & 0x1)){
				struct mallinfo mi = mallinfo();
				prevmem = mi.uordblks;
				infoString = s;
			}
		}

		~VxMemoryReporter(){
			unsigned char GPI = sceKernelGetGPI();
			if(!(GPI & 0x1)){
				struct mallinfo mi = mallinfo();
				char str[1024];
				sprintf(str,"%s %d->%d [%d]",infoString,prevmem/1024,mi.uordblks/1024, mi.uordblks-prevmem);
				PspOutputDebugString(str);
			}
		}

		const char *infoString;
		unsigned int prevmem;

	};

#define MEM_REPORT(a) VxMemoryReporter memrep##__COUNTER__(a);
#endif 

#ifndef MEM_REPORT
	#define MEM_REPORT(a)
#endif

#ifndef VXTRY
#define VXTRY try
#endif

#ifndef VXCATCH
#define VXCATCH(a) catch(a)
#endif

#ifndef VXTHROW
#define VXTHROW throw
#endif

#endif // DOCJET_DUMMY

//*************** EXPORT DEFINES FOR LIB / DLL VERSIONS *************************************************

//-- Roro: Do not modify these lines 

#ifndef CK_LIB
#ifdef CK_PRIVATE_VERSION_VIRTOOLS
#if defined(WIN32) || defined(_XBOX)
#define DLL_EXPORT __declspec(dllexport)	// VC++ export option 
#elif defined(macintosh)
//by nicolasp
	#ifdef __MWERKS__
		#define DLL_EXPORT __declspec(export)			// CodeWarrior export pragma
	#else
		#define DLL_EXPORT
	#endif
#endif			
#else 
#ifdef macintosh
//by nicolasp
	#ifdef __MWERKS__
		#define DLL_EXPORT __declspec(import)	// CodeWarrior import pragma {secret}		
	#else
		#define DLL_EXPORT
	#endif
#else
#define DLL_EXPORT 		
#endif
#endif
#else
#define DLL_EXPORT 
#endif

#ifndef CK_LIB
#ifdef macintosh

#define PLUGIN_EXPORT extern "C" __declspec(dllexport) 
#else
#define PLUGIN_EXPORT 
#endif
#else
#define PLUGIN_EXPORT 
#endif // CK_LIB

//*************** MACINTOSH ENDIANNES SWAPS *************************************************

#if defined(macintosh) && !defined(__i386__)
//by nicolasp
	#ifdef __MWERKS__
		#include <Endian.h>
	#endif

#define ENDIANSWAP16(x)	x = Endian16_Swap(x)

#define ENDIANSWAP32(x)	x = Endian32_Swap(x)	

#define ENDIANSWAPFLOAT(x)	(*(DWORD*)&x) = Endian32_Swap(*(DWORD*)&x)	
#elif (_XBOX_VER>=200)

void VxConvertEndianArray32(void*buf,unsigned int DwordCount);
void VxConvertEndianArray16(void*buf,unsigned int DwordCount);

unsigned short int VxEndian16Swap(unsigned short int x);
unsigned int VxEndian32Swap(unsigned int x);


#define ENDIAN_ARRAY(buf,DwordCount)		VxConvertEndianArray32(buf,DwordCount);	

#define ENDIAN_ARRAY16(buf,WordCount)		VxConvertEndianArray16(buf,WordCount);


#define ENDIANSWAP16(x)	x = VxEndian16Swap(x)

#define ENDIANSWAP32(x)	x = VxEndian32Swap(x)	

#define ENDIANSWAPFLOAT(x)	(*(DWORD*)&x) = VxEndian32Swap(*(DWORD*)&x)	


#define DWORD_LENDIAN(x)					VxEndian32Swap((unsigned int)x)							

#define WORD_LENDIAN(x)						VxEndian16Swap((unsigned short int)x)							


#define BIG_ENDIAN 1	

#else

#ifdef 	BIG_ENDIAN					
#undef  BIG_ENDIAN
#endif	

#ifdef 	LITTLE_ENDIAN					
#undef  LITTLE_ENDIAN
#endif	
#define LITTLE_ENDIAN 1	

#define ENDIANSWAP16(x)
#define ENDIANSWAP32(x)		
#define ENDIANSWAPFLOAT(x)
#endif


#if defined(_LINUX)

#define DLL_EXPORT
#endif

struct VxVector;
struct VxCompressedVector;

struct VxCompressedVectorOld;
struct Vx2DVector;
struct VxColor;
struct VxBbox;
struct VxQuaternion;
struct VxStridedData;
struct VxImageDescEx;

#include "XString.h"

#ifdef macintosh
#include "VxMac.h"
#endif

#ifdef _WIN32_WCE
#include "VxMathCE.h"
#else 
#define WCE_CHECKSTR(x)
#define WCE_STR(x)	x
#endif

#if defined(_LINUX)
#include "VxMathLinux.h"
#endif

#include "XUtil.h"
#include "XP.h"
// Port Class Utility is it the right place
#include "VxSharedLibrary.h"
#include "VxMeMoryMappedFile.h"
#include "CKPathSplitter.h"
#include "CKDirectoryParser.h"
#include "VxWindowFunctions.h"
#include "VxVector.h"
#include "Vx2dVector.h"
#include "VxMatrix.h"
#include "VxConfiguration.h"
#include "VxQuaternion.h"
#include "VxRect.h"
#include "VxOBB.h"
#include "VxRay.h"
#include "VxSphere.h"
#include "VxPlane.h"
#include "VxIntersect.h"
#include "VxDistance.h"
#include "VxFrustum.h"
#include "VxColor.h"
#include "VxAllocator.h"
#include "VxFile.h"
#include "VxBigFile.h"
#include "VxCone.h"

// Containers
#include "XArray.h"
#include "XSArray.h"
#include "XClassArray.h"
#include "XList.h"
#include "XHashTable.h"
#include "XSHashTable.h"
#include "XNTree.h"
#include "XSmartPtr.h"
#include "FixedSizeAllocator.h"


typedef XArray<void*> XVoidArray;

// Threads and Synchro
#include "VxMutex.h"
#ifndef macintosh
#include "VxThread.h"
#endif

//----- Automatically called in dynamic library...{secret}
#if defined(_LINUX)

void InitVxMath(const char* name); 
#else

void InitVxMath(); 
#endif


#if defined (macintosh)
// InitVxMath must be declared 
#ifndef InitVxMath
void InitVxMath(); 
#endif
void ShutDownVxMath();
#endif


#if defined(_LINUX)

void ShutDownVxMath();

const char* GetExeName();
#endif


void VxDetectProcessor(); 

//------ Interpolation
VX_EXPORT void InterpolateFloatArray(void* Res,void* array1,void *array2,float factor,int count);
VX_EXPORT void InterpolateVectorArray(void* Res,void* Inarray1,void *Inarray2,float factor,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT void MultiplyVectorArray(void* Res,void* Inarray1,const VxVector& factor,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT void MultiplyVector2Array(void* Res,void* Inarray1,const Vx2DVector& factor,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT void MultiplyVector4Array(void* Res,void* Inarray1,const VxVector4& factor,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT void MultiplyAddVectorArray(void* Res,void* Inarray1,const VxVector& factor,const VxVector& offset,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT void MultiplyAddVector4Array(void* Res,void* Inarray1,const VxVector4& factor,const VxVector4& offset,int count,DWORD StrideRes,DWORD StrideIn);
VX_EXPORT BOOL VxTransformBox2D(const VxMatrix& World_ProjectionMat,const VxBbox& box,VxRect* ScreenSize,VxRect* Extents,VXCLIP_FLAGS& OrClipFlags,VXCLIP_FLAGS& AndClipFlags); 
VX_EXPORT void VxProjectBoxZExtents(const VxMatrix& World_ProjectionMat,const VxBbox& box,float& ZhMin,float& ZhMax); 

//------- Structure copying
VX_EXPORT BOOL VxFillStructure(int Count,void* Dst,DWORD Stride,DWORD SizeSrc,void* Src); 
VX_EXPORT BOOL VxCopyStructure(int Count,void* Dst,DWORD OutStride,DWORD SizeSrc,void* Src,DWORD InStride);
VX_EXPORT BOOL VxFillStructure(int Count,const VxStridedData& Dst,DWORD SizeSrc,void* Src);
VX_EXPORT BOOL VxCopyStructure(int Count,const VxStridedData& Dst,DWORD SizeSrc,const VxStridedData& Src);


VX_EXPORT BOOL VxIndexedCopy(const VxStridedData& Dst,const VxStridedData& Src,DWORD SizeSrc,DWORD* Indices,int IndexCount);
VX_EXPORT BOOL VxIndexedCopy(const VxStridedData& Dst,const VxStridedData& Src,DWORD SizeSrc,WORD* Indices,int IndexCount);

VX_EXPORT void VxCopyDwords2Words(WORD* iDest,DWORD* iSrc,int iCount);
VX_EXPORT void VxCopyWords2Dwords(DWORD* iDest,WORD* iSrc,int iCount);

//---- Graphic Utilities
VX_EXPORT 	void VxDoBlit(const VxImageDescEx& src_desc,const VxImageDescEx& dst_desc);
VX_EXPORT 	void VxDoBlitUpsideDown(const VxImageDescEx& src_desc,const VxImageDescEx& dst_desc);

VX_EXPORT 	void VxDoBlitDeInterleaved(const VxImageDescEx& src_desc,const VxImageDescEx& dst_desc, const BOOL iField1First);
VX_EXPORT 	void VxDoBlitDeInterleavedUpsideDown(const VxImageDescEx& src_desc,const VxImageDescEx& dst_desc, const BOOL iField1First);

VX_EXPORT 	void VxDoAlphaBlit(const VxImageDescEx& dst_desc,BYTE AlphaValue);
VX_EXPORT 	void VxDoAlphaBlit(const VxImageDescEx& dst_desc,BYTE* AlphaValues);

VX_EXPORT 	void VxGetBitCounts(const VxImageDescEx& desc,DWORD& Rbits,DWORD& Gbits,DWORD& Bbits,DWORD& Abits);
VX_EXPORT 	void VxGetBitShifts(const VxImageDescEx& desc,DWORD& Rshift,DWORD& Gshift,DWORD& Bshift,DWORD& Ashift);

VX_EXPORT 	void VxGenerateMipMap(const VxImageDescEx& src_desc,BYTE* DestBuffer);
VX_EXPORT 	void VxResizeImage32(const VxImageDescEx& src_desc,const VxImageDescEx& dst_desc);

VX_EXPORT	BOOL VxConvertToNormalMap(const VxImageDescEx& image,DWORD ColorMask);
VX_EXPORT	BOOL VxConvertToBumpMap(const VxImageDescEx& image);

VX_EXPORT 	DWORD GetBitCount(DWORD dwMask);
VX_EXPORT 	DWORD GetBitShift(DWORD dwMask);

VX_EXPORT 	VX_PIXELFORMAT  VxImageDesc2PixelFormat(const VxImageDescEx& desc);
VX_EXPORT 	void			VxPixelFormat2ImageDesc(VX_PIXELFORMAT Pf,VxImageDescEx& desc);
VX_EXPORT 	const char*		VxPixelFormat2String(VX_PIXELFORMAT Pf);

VX_EXPORT	void	VxBppToMask(VxImageDescEx& desc);

VX_EXPORT	int		GetQuantizationSamplingFactor();
VX_EXPORT	void	SetQuantizationSamplingFactor(int sf);

//---- Processor features
VX_EXPORT	char*			GetProcessorDescription();
VX_EXPORT	int				GetProcessorFrequency();
VX_EXPORT	DWORD			GetProcessorFeatures();
VX_EXPORT	void			ModifyProcessorFeatures(DWORD Add,DWORD Remove);
VX_EXPORT	ProcessorsType	GetProcessorType();

VX_EXPORT BOOL VxPtInRect(CKRECT *rect, CKPOINT *pt);

// Summary: Compute best Fit Box for a set of points
// 
VX_EXPORT BOOL VxComputeBestFitBBox(const BYTE *Points, const DWORD Stride, const int Count, VxMatrix& BBoxMatrix, const float AdditionnalBorder );

// Path Conversion
// 
#if defined(macintosh)
/****************************************************************
Summary: Defines the string representing a directory separator
on the current system.
Remarks:
Depending on the system the character can be
Macintosh - ":"
PC/XBox	- "\"
Linux/PS2	- "/"
****************************************************************/
#define DIRECTORY_SEP_STRING  	":"
/****************************************************************
Summary: Defines the character representing a directory separator
on the current system.
Remarks:
Depending on the system the character can be
Macintosh - ":"
PC/XBox	- "\"
Linux/PS2	- "/"
****************************************************************/
#define DIRECTORY_SEP_CHAR  	':'
#elif defined(WIN32) || defined(_XBOX)
#define DIRECTORY_SEP_STRING  	"\\"
#define DIRECTORY_SEP_CHAR  	'\\'
#elif defined(_LINUX) || defined(PSX2) || defined(PSP)
#define DIRECTORY_SEP_STRING  	"/"
#define DIRECTORY_SEP_CHAR  	'/'
#endif


/****************************************************************
Summary: Adds the character representing a directory separator on the current system.
Arguments:
	path: a Xstring to add a directory separator to. 
Remarks:
Depending on the system the character can be
Macintosh - ":"
PC/XBox	- "\"
Linux/PS2	- "/"
****************************************************************/
VX_EXPORT void VxAddDirectorySeparator(XString& path);

/****************************************************************
Summary: Converts a path to be compatible with current system.
Remarks:
Process a string representing a directory or a file
to be compatible with the current system. It means directory seperators
are converted to match the current system.
See Also:DIRECTORY_SEP_STRING,VxAddDirectorySeparator
****************************************************************/
VX_EXPORT void VxConvertPathToSystemPath(XString& path);

/*************************************************
{filename:VxTimeProfiler}
Name: VxTimeProfiler
Summary: Class for timing purposes

Remarks: 
This class provides methods to accurately compute
the time elapsed. On PC platform it uses the Windows QueryPerformanceCounter
function to be compatible with processor with the Intel SpeedStep feature.

The VxRDTSCProfiler class is best suited when profiling small chunk of code
has it has smaller overhead than VxTimeProfiler. 

Example:
// To profile several items :

VxTimeProfiler MyProfiler;
...
float delta_time=MyProfiler.Current();
MyProfiler.Reset();
...
float delta_time2=MyProfiler.Current();
See also: VxRDTSCProfiler
*************************************************/
class VX_EXPORT  VxTimeProfiler {
public:
	/*************************************************
	Name: VxTimeProfiler
	Summary: Starts profiling
	*************************************************/
	VxTimeProfiler() { Reset(); }
	VxTimeProfiler(BOOL iReset) { if (iReset) Reset(); }

#if defined(PSX2) || defined(PSP)
#ifndef PSP
	~VxTimeProfiler();
	void Reset();

#else
	void Reset(){
	}

	float Current()
	{
		return 0.f;

	}

	inline float GetCurrent() {
		return 0.0f;
	}
#endif
#else

	/*************************************************
	Summary: Restarts the timer
	*************************************************/
	void Reset();

	/*************************************************
	Summary: Returns the current time elapsed (in milliseconds)
	*************************************************/
	float Current();

#endif // PSX2

	/*************************************************
	Summary: Returns the current time elapsed (in milliseconds)
	*************************************************/
	float Split()
	{
		float c = Current();
		Reset();
		return c;
	}

protected:
#ifdef _XBOX
	/*
	union{
		DWORD Times[4];
		__int64 TimesLL[2];
	 };
	*/
	DWORD Times[4];
#else
	DWORD Times[4];
#endif
};

#ifdef DOCJETDUMMY // DOCJET secret macro
#else
#ifdef PSP
class VX_EXPORT  VxTimeProfilerPSP : public VxTimeProfiler {
public:
	VxTimeProfilerPSP() { 
		Reset(); 
	}

	VxTimeProfilerPSP(BOOL iReset) { 
		if (iReset) Reset(); 
	}

	void Reset(){
		sceRtcGetCurrentTick((SceRtcTick*) Times);		
	}

	float Current()
	{
		return GetCurrent();
	}

	inline float GetCurrent() {
		SceRtcTick cur;
		sceRtcGetCurrentTick(&cur);
		cur.tick -= (*(SceRtcTick*) Times).tick;
		unsigned int tick32 = (unsigned int)cur.tick;
		return ((float)tick32)*0.001f;	
	}

};
#endif
#endif


/*************************************************
{filename:VxRDTSCProfiler}
Name: VxRDTSCProfiler
Summary: Class for profiling purposes

Remarks: 
This class provides methods to accurately compute
the time elapsed. Its implementation is the same 
from VxTimeProfiler except on PC platform where it uses the RDTSC
instruction to profile precisely the number of cycles. 

On Intel processor with the Speedstep technology, the 
processor frequency can change over time so the result 
of the profiler may not be reliable. 
Example:
// To profile several items :

VxRDTSCProfiler MyProfiler;
...
float delta_time = MyProfiler.Current();
int   cycle_count = MyProfiler.CurrentCycleCount();

MyProfiler.Reset();
...
float delta_time2=MyProfiler.Current();
See also: VxTimeProfiler
*************************************************/
#if defined(WIN32) || (defined(_XBOX) && (_XBOX_VER<200))
#define VXRDTSCPROFILER
class VX_EXPORT  VxRDTSCProfiler {
public:
	/*************************************************
	Name: VxTimeProfiler
	Summary: Starts profiling
	*************************************************/
	VxRDTSCProfiler() { Reset(); }
	VxRDTSCProfiler(BOOL iReset) { if (iReset) Reset(); }

	/*************************************************
	Summary: Restarts the timer
	*************************************************/
	void Reset();

	/*************************************************
	Summary: Returns the current time elapsed (in milliseconds)
	*************************************************/
	float Current();

	/*************************************************
	Summary: Returns the number of cycles elapsed...
	*************************************************/
	int   CurrentCycleCount();

	/*************************************************
	Summary: Returns the current time elapsed (in milliseconds)
	*************************************************/
	float Split()
	{
		float c = Current();
		Reset();
		return c;
	}

protected:
	DWORD Times[4];
};
#else 
typedef VxTimeProfiler VxRDTSCProfiler;
#endif


#include "VxLog.h"
#include "VxLogListener.h"


/****************************************************************
Name: VxImageDescEx

Summary: Enhanced Image description

Remarks:
The VxImageDescEx holds basically an VxImageDesc with additionnal support for
Colormap, Image pointer and is ready for future enhancements.


****************************************************************/
struct VxImageDescEx {
	int		Size;				// Size of the structure
	DWORD	Flags;				// Reserved for special formats (such as compressed ) 0 otherwise

	int		Width;				// Width in pixel of the image
	int		Height;				// Height in pixel of the image
	union   {
		int		BytesPerLine;		// Pitch (width in bytes) of the image
		int		TotalImageSize;		// For compressed image (DXT1...) the total size of the image
	};
	int		BitsPerPixel;		// Number of bits per pixel
	union {
		DWORD	RedMask;			// Mask for Red component
		DWORD   BumpDuMask;			// Mask for Bump Du component
	};
	union {
		DWORD	GreenMask;			// Mask for Green component	
		DWORD	BumpDvMask;			// Mask for Bump Dv component
	};
	union {
		DWORD	BlueMask;			// Mask for Blue component
		DWORD   BumpLumMask;		// Mask for Luminance component

	};
	DWORD	AlphaMask;			// Mask for Alpha component
	union{
		short	BytesPerColorEntry;	// ColorMap Stride
		short	Depth;				// Depth for Volume Texture
	};
	short	ColorMapEntries;	// If other than 0 image is palletized

	BYTE*	ColorMap;			// Palette colors
	BYTE*	Image;				// Image

public:
	VxImageDescEx() 
	{
		Size=sizeof(VxImageDescEx);
		memset((BYTE*)this+4,0,Size-4);
	}

	void Set(const VxImageDescEx& desc)
	{
		Size=sizeof(VxImageDescEx);
		if (desc.Size<Size) memset((BYTE*)this+4,0,Size-4);
		if (desc.Size>Size) return;
		memcpy((BYTE*)this+4,(BYTE*)&desc+4,desc.Size-4);
	}
	BOOL HasAlpha() {
		return ((AlphaMask!=0) || (Flags>=_DXT1));
	}


	int operator == (const VxImageDescEx& desc)
	{
		return (Size==desc.Size &&
			Height==desc.Height && Width==desc.Width &&
			BitsPerPixel==desc.BitsPerPixel && BytesPerLine==desc.BytesPerLine &&
			RedMask==desc.RedMask && GreenMask==desc.GreenMask &&
			BlueMask==desc.BlueMask && AlphaMask==desc.AlphaMask &&
			BytesPerColorEntry==desc.BytesPerColorEntry && ColorMapEntries==desc.ColorMapEntries);
	}

	int operator != (const VxImageDescEx& desc)
	{
		return (Size!=desc.Size ||
			Height!=desc.Height || Width!=desc.Width ||
			BitsPerPixel!=desc.BitsPerPixel || BytesPerLine!=desc.BytesPerLine ||
			RedMask!=desc.RedMask || GreenMask!=desc.GreenMask ||
			BlueMask!=desc.BlueMask || AlphaMask!=desc.AlphaMask ||
			BytesPerColorEntry!=desc.BytesPerColorEntry || ColorMapEntries!=desc.ColorMapEntries);
	}

};

/************************************************
Summary: Returns the current platform.

************************************************/
VX_EXPORT VX_PLATFORMINFO VxGetPlatform();

#endif
