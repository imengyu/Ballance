#ifndef _MACHEADER_H_
#define _MACHEADER_H_


//by nicolasp
#ifdef __MWERKS__
	#include <extras.h>
#endif	
	// Large enough compared to
	// standard define from MSL : 3 !!
	#define _MAX_DRIVE 256
	
	// Following defines found in 2.1
	// but already defined in MSL 8
	// (all values set to 256)
	// Check for non regression after
	// removal of these defines !!!
	
	
#define _MAX_FNAME 64
#define _MAX_DIR   256
#define _MAX_EXT   32

#define __min(x,y) ((x<y) ? x : y)
#define __max(x,y) ((x>y) ? x : y)

#define OutputDebugString(x)  printf(x);

	// sqrtf, cosf are defined in std::
	#include <math.h>
	using namespace std;

#endif