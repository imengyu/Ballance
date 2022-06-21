#if !defined(VXMATHPS2_H)
#define VXMATHPS2_H

#ifdef PSX2
	#include <sys/time.h>
	#include <sys/param.h>
#endif

#if defined(PSP)

#if defined(SN_TARGET_PSP_HW) || defined(SN_TARGET_PSP_PRX)
	#include <stdio.h>
	#include <sys/time.h>
	#include <sys/param.h>
	#include <rtcsvc.h>
	#include <Kernel.h>

	void PspOutputDebugString(const char* str);

#endif

#ifdef __MWERKS__
	#pragma warn_possunwant off
	#pragma warn_unusedarg off
#elif defined(SN_TARGET_PSP) || defined(SN_TARGET_PSP_HW)|| defined(SN_TARGET_PSP_PRX)

	#pragma diag_suppress=237 // remark 0237: controlling expression is constant
	#pragma diag_suppress=817 // warning 0817: type qualifier on return type is meaningless
	#pragma diag_suppress=178 // warning 0178: variable "totalDist" was declared but never referenced
	#pragma diag_suppress=9   //warning 0009: nested comment is not allowed
	#pragma diag_suppress=828   // remark 0828: parameter "PlaneEquation" was never referenced
	#pragma diag_suppress=403 // remark 0403: destructor for base class "XBaseString" is not virtual
	#pragma diag_suppress=341 // remark 0341: value copied to temporary, reference to temporary used
	#pragma diag_suppress=83 // remark 0083: storage class is not first
	#pragma diag_suppress=229 // remark 0229: trailing comma is nonstandard
	#pragma diag_suppress=194 // remark 0194: zero used for undefined preprocessing identifier
	#pragma diag_suppress=481 // remark 0481: function "VxVector::VxVector()" redeclared "inline" after being called	
#endif




#endif


#ifndef PATH_MAX
#define PATH_MAX	256
#endif


#ifndef _MAX_PATH
#define _MAX_PATH PATH_MAX
#endif


#ifndef _MAX_DRIVE

#define _MAX_DRIVE 10
#endif

#ifndef _MAX_DIR

#define _MAX_DIR _MAX_PATH
#endif

#ifndef _MAX_EXT

#define _MAX_EXT 10
#endif

#ifndef _MAX_FNAME

#define _MAX_FNAME _MAX_PATH
#endif

#ifndef stricmp 
#define stricmp strcmpi
#endif

#ifndef strnicmp 
#define strnicmp strncmpi
#endif

#define OutputDebugString(x)  printf(x);


int strcmpi(const char *dst, const char *src);
#ifndef PSP
	int strncmpi(const char *dst, const char *src,size_t count);
#else
	int strncmpi(const char *dst, const char *src,unsigned int count);
#endif


#endif // VXMATHPS2_H
