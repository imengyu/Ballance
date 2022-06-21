// CKControls.h : main header file for the CKCONTROLS DLL
//

#if !defined(AFX_CKCONTROLS_H__A1F768C1_E584_4280_A461_ABF8E1C38AD3__INCLUDED_)
#define AFX_CKCONTROLS_H__A1F768C1_E584_4280_A461_ABF8E1C38AD3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CCKControlsApp
// See CKControls.cpp for the implementation of this class
//

class CCKControlsApp : public CWinApp
{
public:
	CCKControlsApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCKControlsApp)
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CCKControlsApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CKCONTROLS_H__A1F768C1_E584_4280_A461_ABF8E1C38AD3__INCLUDED_)
