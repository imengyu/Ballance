#ifndef _CMULTIHEADERCTRL_H_
#define _CMULTIHEADERCTRL_H_

/////////////////////////////////////////////////////////////////////////////
// CMHeaderCtrl window

typedef void (*MHeaderDrawCallback)(LPDRAWITEMSTRUCT dis);

#ifdef CUIK
class CMHeaderCtrl : public CHeaderCtrl
#else
#ifdef CKCONTROLS_API
#define CKCONTROLS_CLASS_DECL  __declspec(dllexport)
#else
#define CKCONTROLS_CLASS_DECL  __declspec(dllimport)
#endif
namespace CKControl	//----------------------------------------
{
class CKCONTROLS_CLASS_DECL CMHeaderCtrl : public CHeaderCtrl
#endif
{
public:
//enums
	enum CMHEADER_FLAGS {
		HF_NONE			= 0x00000000,
		HF_DRAWBORDER	= 0x00000001,		//does not work yet
	};

// Construction
public:
	CMHeaderCtrl();

// Attributes
public:

// Operations
public:
	void GetItemRect(int i,RECT *r);
	int GetColumnWidth(int i);
	void SetColumnWidth(int i,int w);
	int GetHeaderSize();
	int HitTest(CPoint pt,UINT *flags);

	void SetActiveColumn(int pos);
	int  GetActiveColumn();

	void SetColors(	
				COLORREF backColor,
				COLORREF textColor,
				COLORREF textBackColor,
				COLORREF textSelectedColor,
				COLORREF textBackSelectedColor,
				COLORREF borderColor = 0
				);


	void GetColors(	
				COLORREF *backColor,
				COLORREF *textColor,
				COLORREF *textBackColor,
				COLORREF *textSelectedColor,
				COLORREF *textBackSelectedColor,
				COLORREF *borderColor=NULL
				);

	//flags
	DWORD	GetFlags() {return m_Flags;};
	void	ModifyFlags(DWORD added,DWORD removed=0) {m_Flags &= ~removed;m_Flags |= added;}
	void	SetFlags(DWORD f) {m_Flags = f;}

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMHeaderCtrl)
	public:
		virtual void DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);
	protected:
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	//}}AFX_VIRTUAL
	
// Implementation
public:
	virtual ~CMHeaderCtrl();
	CWnd	*m_Owner;
	// Generated message map functions
protected:
	//{{AFX_MSG(CMHeaderCtrl)
	afx_msg void OnPaint();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	//}}AFX_MSG
		
	COLORREF m_backColor;
	COLORREF m_textColor;
	COLORREF m_textBackColor;
	COLORREF m_textSelectedColor;
	COLORREF m_textBackSelectedColor;
	COLORREF m_borderColor;

	int m_LastPicked;
	int m_ActiveCol;

	DWORD	m_Flags;

	DECLARE_MESSAGE_MAP()
};	//class CMHeaderCtrl

#ifndef CUIK
}	//namespace CKControl
#endif

#endif
