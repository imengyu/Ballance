#if !defined(AFX_CKOBJECTCOMBOBOX_H__295405A6_24C4_11D3_BAE2_00A0C9CC72C3__INCLUDED_)
#define AFX_CKOBJECTCOMBOBOX_H__295405A6_24C4_11D3_BAE2_00A0C9CC72C3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// VIComboBox.h : header file
//

#ifdef CK2UI
	#define CKOBJECTCOMBOBOX_CLASS_DECL  _declspec(dllexport)
#else
#ifdef CUIK
	#define CKOBJECTCOMBOBOX_CLASS_DECL  
#else
	#define CKOBJECTCOMBOBOX_CLASS_DECL  _declspec(dllimport)
#endif
#endif

/////////////////////////////////////////////////////////////////////////////
// VIComboBox window

//must call SetContext first
//item data = CK_ID
class CKOBJECTCOMBOBOX_CLASS_DECL CKObjectComboBox : public VIComboBox
{
public:
	CKObjectComboBox() {m_pContext=NULL;m_ownerDrawHeight=6;}

	void SetContext(CKContext * ctxt);
	virtual void DrawItem( LPDRAWITEMSTRUCT lpDrawItemStruct );
	virtual void DoOnPaint(CDC* pDC=NULL,BOOL forceDraw=FALSE);

	/*
	void	StartEdit();
	void	EndEdit();
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	VIEdit	m_EditName;
	*/

	static void LoadIcons();

private:
	CKContext * m_pContext;
	static XArray<HICON>	m_IconArray;
	static BOOL				m_IconInit;
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CKOBJECTCOMBOBOX_H__295405A6_24C4_11D3_BAE2_00A0C9CC72C3__INCLUDED_)
