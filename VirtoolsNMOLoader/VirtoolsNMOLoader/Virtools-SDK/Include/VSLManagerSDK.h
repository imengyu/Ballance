/*******************************************************************************************************
File :		VSLManagerSDK.h
Author :	Edwin Razafimahatratra

Copyright (c) Virtools 2002, All Rights Reserved.					
*******************************************************************************************************/
//-------------------------------------------------------------------------------------------------------
// Simplified VSL Manager.
//-------------------------------------------------------------------------------------------------------
#ifndef VSLMANAGER
#define VSLMANAGER

//-------------------------------------------------------------------------------------------------------
#include "VxMath.h"
#include "CKBaseManager.h"
#include "Bind.h"
//-------------------------------------------------------------------------------------------------------
// Don't change this GUID !
#define VSLMANAGER_GUID						CKGUID(0x4f243100,0x28ab3f51)
//-------------------------------------------------------------------------------------------------------
namespace VSL {
	//-------------------------------------------------------------------------------------------------------
	enum VCallType {VCALLTYPE_CDECL, VCALLTYPE_STDCALL};
	//-------------------------------------------------------------------------------------------------------
	enum VClassOption
	{
		VCLASSOPTION_NULL						= 0x00000000,
		VCLASSOPTION_OBJECT						= 0x00000001,
		VCLASSOPTION_POINTERCOPY				= 0x00000002,
		VCLASSOPTION_MEMCOPY					= 0x00000004,
		VCLASSOPTION_SCRIPTSTRUCT				= 0x00000008,
		VCLASSOPTION_CONTAINCPP2CONSTRUCT		= 0x00000010,
		VCLASSOPTION_CONTAINCPP2DESTRUCT		= 0x00000020,
		VCLASSOPTION_CONTAINCPPOBJ				= 0x00000040,
		VCLASSOPTION_ENUM						= 0x00000080,
		VCLASSOPTION_RESERVEDMASK				= 0xFF000000,	// Mask reserved for user data...
	};
	//-------------------------------------------------------------------------------------------------------
	union										PRIMDATA
	{
		int										u_Int;
		float									u_Float;
		void									*u_VoidPtr;
		struct									s_OpParams
		{
			unsigned short						u_Opcode;
			unsigned short						u_NbParams;
		}										u_OpParams;
		PRIMDATA(){}
		PRIMDATA(int iVal):u_Int(iVal){}
		PRIMDATA(float iVal):u_Float(iVal){}
	};
	//-------------------------------------------------------------------------------------------------------
#if defined(macintosh) || defined(PSX2) || defined(PSP)  || (_XBOX_VER>=200)

	// Copy/Pasted from "CPlusLib.h"
	// must be synchro with this definition !!!
#if defined(macintosh) || defined(PSX2) || defined(_XBOX)
#ifdef __GNUC__
	typedef struct PTMF
	{
		// func_addr is even => direct pointer to code
		// func_addr is odd : func_addr-1 is the offset in the vtable	
		void *func_addr;			
		int	this_offset;
		inline bool operator ==(const PTMF &other) const
		{
			return (func_addr==other.func_addr)&&(this_offset==other.this_offset);	
		}
		inline bool IsValid() const
		{
			return func_addr!=NULL;
		}

	}PTMF;

#else
	typedef struct PTMF
	{
		int	this_delta;					//	delta to this pointer
		int	vtbl_offset;				//	offset of virtual function pointer in vtable (<0: non-virtual function address)
		union
		{
			void	*func_addr;				//	non-virtual function address
			int		ventry_offset;			//	offset of vtable pointer in class
		}func_data;
	}PTMF;


#endif
#endif

#ifdef PSP
#ifdef __MWERKS__
#include <ptmf.h>
#else
	typedef struct PTMF
	{
		short this_delta;				//	delta to this pointer
		short vtbl_offset;				//	offset of virtual function pointer in vtable (<0: non-virtual function address)
		union
		{
			void	*func_addr;				//	non-virtual function address
			int		ventry_offset;			//	offset of vtable pointer in class
		}func_data;
	}PTMF;

	extern "C"
	{
		long __ptmf_cmpr(PTMF *ptmf1, PTMF *ptmf2);
		long __ptmf_test(PTMF *ptmf);
	}
#endif
#endif


#if defined(macintosh) || defined(PSX2)

	extern "C"
	{
		long __ptmf_cmpr(PTMF *ptmf1, PTMF *ptmf2);
		long __ptmf_test(PTMF *ptmf);
	}
#endif

	typedef unsigned int TPPCInstruction;

	class TPtrToRoutine
	{
	private :

		enum
		{
			eNoFlags 							= 0,
			eIsMethod							= 1,
			eIsReturnedObjectInMemory			= 2,
			eIsMethodThroughMultipleInheritance = 4

		};

		unsigned short 		m_Flags;
		short				m_NbUsedRegistersForParameters;// Debug info
		// to ensure at runtime
		// that the jumpcode fits the incoming parameters

		union
		{
			PTMF	m_PointerToMethod;
			void*	m_PointerToFunction;
		};

		TPPCInstruction*	m_JumpCode;

	public :
#if defined(__GNUC__)
		TPtrToRoutine(void);
		void CreateFunctionPtr(void** addrPtr);
		void CreateMethodPtr(void* addrPtr);
		inline bool operator==(const TPtrToRoutine& other) const;
		inline bool IsValid(void) const;

#else			
		// Constructors
		inline TPtrToRoutine(void) : m_Flags(eNoFlags), m_NbUsedRegistersForParameters(-1), m_PointerToFunction(NULL), m_JumpCode(NULL) {}
#ifdef _DEBUG
		void CreateMethodPtr(void* addrPtr);
		void CreateFunctionPtr(void** addrPtr);
#else		
		// Deferred Initializations
		void CreateMethodPtr(void* addrPtr)
		{
			m_Flags |= eIsMethod;
			m_PointerToMethod 	= *((PTMF*) addrPtr);	
		}

		void CreateFunctionPtr(void** addrPtr)		
		{
			m_Flags &= eIsMethod;
#if (defined(macintosh) && !defined(__GNUC__))
			m_PointerToFunction = reinterpret_cast<void*>(**(unsigned int**) addrPtr);	
#elif defined(PSP)
			m_PointerToFunction = reinterpret_cast<void*>(*(unsigned int*) addrPtr);	
#else
			m_PointerToFunction = reinterpret_cast<void*>(*(unsigned int**) addrPtr);	
#endif		
		}	
#endif	
		// Operator ==
		bool operator==(const TPtrToRoutine& other) const
		{
			if (((m_Flags ^ other.m_Flags) & eIsMethod) != 0)
				return false;
#if defined(macintosh) || defined(PSX2) || defined(PSP)
			if ((m_Flags & eIsMethod) != 0)
				return (__ptmf_cmpr((PTMF*) &m_PointerToMethod, (PTMF*) &other.m_PointerToMethod) == 0);
			else
#endif
				return m_PointerToFunction == other.m_PointerToFunction;
		}

		// Check for validity
		bool IsValid(void) const
		{
#if defined(macintosh) || defined(PSX2) || defined(PSP)
			if ((m_Flags & eIsMethod) != 0)
				return __ptmf_test((PTMF*) &m_PointerToMethod) != 0;
			else
#endif	
				return m_PointerToFunction != NULL;
		}
#endif	
		// Set the jump code
		void SetJumpCode(short nbParameters, const TPPCInstruction* code, unsigned int nbInstructions, bool isReturnedTypeInMemory = false);
		void FreeJumpCode(void);

		// Check the type
		bool IsPointerToMethod(void) const { return (m_Flags & eIsMethod) != 0; }
		bool IsPointerToFunction(void) const { return (m_Flags & eIsMethod) == 0; }

		// Infos about the object returned by function/method
		bool IsReturnedObjectInMemory(void) const { return (m_Flags & eIsReturnedObjectInMemory) != 0; }
		bool IsReturnedObjectInRegister(void) const { return (m_Flags & eIsReturnedObjectInMemory) == 0; }

		// Const Getters

		short GetNbUsedRegistersForParameters(void) const { return m_NbUsedRegistersForParameters; }
		const TPPCInstruction* GetJumpCode(void) const { return m_JumpCode; }

		const void* GetCodeAddress(void) const
		{
			XASSERT(IsPointerToFunction());
			return m_PointerToFunction;
		}

		const PTMF* GetPTMF(void) const
		{
			XASSERT(IsPointerToMethod());
			return &m_PointerToMethod;
		}


	};

#else

	// Win32
	// Must be copied in VSLManagerSDK too...
	class TPtrToRoutine
	{
	private :
		void* 	m_PointerToFunction;

	public :

		TPtrToRoutine(void) : m_PointerToFunction(NULL) {}

		void CreateMethodPtr(void* addrPtr)
		{
			m_PointerToFunction =  (void *)*(int *)addrPtr;	
		}

		void CreateFunctionPtr(void** addrPtr)
		{
			m_PointerToFunction =  (void *)*(int *)addrPtr ;	
		}

		const void* GetCodeAddress(void) const { return m_PointerToFunction; }
		bool operator==(const TPtrToRoutine& other) const { return m_PointerToFunction == other.m_PointerToFunction; }

		bool IsValid(void) const { return m_PointerToFunction != NULL; }
	};

#endif
	//-------------------------------------------------------------------------------------------------------
} // namespace VSL
//-------------------------------------------------------------------------------------------------------
class										VSLManager : public CKBaseManager, CKVariableManager::Watcher
{
public:
	// Bind tools (interface for class manager).
	virtual CKBOOL							RegisterSpace();
	virtual CKBOOL							RegisterFunction(const char *iFuncName, const VSL::TPtrToRoutine& iPtr, VSL::VCallType iCallType, int iNbParam, const char *iReturnClassName, ...);
	virtual CKBOOL							RegisterClass(const char *iClassName, int iSize, const char *iAliasClassName = NULL, DWORD iOption = VSL::VCLASSOPTION_NULL);
	virtual CKBOOL							RegisterClass(const char *iClassName, const char *iReferenceClass, const char *iAliasClassName = NULL);
	virtual CKBOOL							RegisterHeritage(const char *iClassName, const char *iParentClassName, ...);
	virtual CKBOOL							RegisterMember(const char *iClassName, const char *iMemberName, const char *iReturnClassName, int iOffset);
	virtual CKBOOL							RegisterMethod(const char *iClassName, const char *iMethodName, CKBOOL iStatic, XBOOL constFunct, const VSL::TPtrToRoutine& iPtr, VSL::VCallType iCallType, unsigned int iNbParam, const char *iReturnClassName, ...);
	virtual CKBOOL							RegisterEnum(const char *iEnumName);
	virtual CKBOOL							RegisterEnumMember(const char *iEnumName, const char *iEnumMemberName, int iValue);
	virtual CKBOOL							RegisterConstant(const char *iConstName, BindedConstType iConstType, VSL::PRIMDATA iData);
	virtual CKBOOL							RegisterGUID(CKGUID iGUID, const char *iClassName);
	virtual CKBOOL							UnregisterGUID(CKGUID iGUID);
#ifdef _DEBUG
	virtual CKBOOL							RegisterClassDocSDKLink(const char *iClassName, const char* iDocSDKLink, unsigned int iDocFlags = 0);
#endif

	// Constructor/Destructor.
	virtual ~VSLManager();
};
//-------------------------------------------------------------------------------------------------------
#endif