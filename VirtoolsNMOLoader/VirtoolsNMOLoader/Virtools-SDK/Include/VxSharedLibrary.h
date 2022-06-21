/*************************************************************************/
/*	File : VxSharedLibrary.h											 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/

#ifndef VXSHAREDLIBRARY_H
#define VXSHAREDLIBRARY_H  "$Id:$"

/***********************************************************************
Summary: Utility class for loading Dlls.

Remarks:

Example:
		// To load a dll in order to retrieve a specific function and call it
		VxSharedLibrary Shl;
		INSTANCE_HANDLE DllInst = Shl.Load(DllName);	// INSTANCE_HANDLE = HMODULE of win32

		MyFunc = Shl.GetFunctionPtr("MyFunctionInTheDll");
		MyFunc(...);

		shl.ReleaseLibrary();

  

See also: 
***********************************************************************/
class VxSharedLibrary  
{
public:
	
	// Creates an unattached VxLibrary
	VX_EXPORT VxSharedLibrary();
	
	// Attaches a existing Library to a VxLibrary
	VX_EXPORT void Attach(INSTANCE_HANDLE LibraryHandle);
	
	// Loads the shared Library from disk
	VX_EXPORT INSTANCE_HANDLE Load(char *LibraryName);
	
	// Unloads the shared Library
	VX_EXPORT void ReleaseLibrary();

	// Retrieves a function pointer from the library
	VX_EXPORT void* GetFunctionPtr(char *FuntioncName);
	
protected:
	
	INSTANCE_HANDLE m_LibraryHandle;
};

#endif 
