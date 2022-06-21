// CMultiListCtrl.h : header file

#ifndef _CMULTILISTCTRL_H_
#define _CMULTILISTCTRL_H_

#include "CMHeaderCtrl.h"

#ifndef CUIK
namespace CKControl	//----------------------------------------
{
#endif

/////////////////////////////////////////////////////////////////////////////
// Special Edit for MT

class CMLEdit : public CEdit
{

	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam); 
};



/////////////////////////////////////////////////////////////////////////////
// CMultiListCtrl window


#define NLVS_SUBITEMSELECT		0x00000001
#define NLVS_DRAWHSEPARATOR		0x00000002
#define NLVS_DRAWVSEPARATOR		0x00000004
#define NLVS_NORIGHTSELECT		0x00000008
#define NLVS_USESELECTEDCOLUMN	0x00000010
#define NLVS_ALLOWCOLUMNDROP	0x00000020
#define NLVS_FIXEDCOLUMNORDER	0x00000040
#define NLVS_SINGLESELECTION	0x00000080
#define NLVS_DRAWBORDER			0x00000100	//does not work yet
#define NLVS_OWNERDRAWROLLOVER	0x00000200	//owner drawn items are redrawn with user callback when mouse is on the (sub)item
#define NLVS_DRAGABLE_ON_HEADER	0x00000400	//window can be dragged with mouse from header


#define NLVSI_NOP	 		0x00000001
#define NLVSI_TEXT	 		0x00000002
#define NLVSI_BITMAP 		0x00000004
#define NLVSI_WINDOW		0x00000008
#define NLVSI_OWNERDRAWN	0x00000010
#define NLVSI_EDITABLE 		0x00000020

#define NLVSI_TOP           0x00000000
#define NLVSI_LEFT          0x00000000
#define NLVSI_CENTER        0x00000100
#define NLVSI_RIGHT         0x00000200
#define NLVSI_VCENTER       0x00000400
#define NLVSI_BOTTOM        0x00000800

#define NLVSI_HIDDEN		0x00010000
#define NLVSI_STATENORMAL	0x00020000
#define NLVSI_STATEINTER	0x00040000
#define NLVSI_STATEPUSHED	0x00080000


#define NLVSI_DEFAULT		(NLVSI_TEXT | NLVSI_VCENTER | NLVSI_CENTER)

struct tagNLVITEM;
struct tagVILVSUBITEM;

typedef void (*NLVUpdateValueCB)(tagNLVITEM *it,int sub,tagVILVSUBITEM *subitem);

typedef struct tagVILVSUBITEM{
	char *Text;
	int iIndex;
	int iInterIndex;
	int iPushedIndex;
	DWORD Flags;
	void *data;
	NLVUpdateValueCB UpdateCB;
	HWND hWnd;
	RECT rcItem; // used for Picking not copied
	int Height;
	int Width;
	public :
		tagVILVSUBITEM()
		{
			Text			= NULL;
			Flags			= NLVSI_DEFAULT;
			iIndex			= -1;
			iInterIndex		= -1;
			iPushedIndex	= -1;
			rcItem.top		= 0;
			rcItem.bottom	= 0;
			rcItem.left		= 0;
			rcItem.right	= 0;
			Height			= -1;
			Width			= -1;
			hWnd			= NULL;
			UpdateCB		= NULL;
			data			= NULL;
		}
		~tagVILVSUBITEM()
		{
			delete [] Text;

			if(hWnd)
				::DestroyWindow(hWnd);
		}
		tagVILVSUBITEM& operator=(const tagVILVSUBITEM& si)
		{
			Flags		= si.Flags;
			iIndex		= si.iIndex;
			iInterIndex	= si.iInterIndex;
			iPushedIndex= si.iPushedIndex;
			Height		= si.Height;
			Width		= si.Width;
			hWnd		= si.hWnd;
			UpdateCB	= si.UpdateCB;
			data		= si.data;
			
			delete [] Text;

			if(si.Text)
			{
				Text = new char[strlen(si.Text) + 1];
				strcpy(Text,si.Text);
			}
			else
			{
				Text = NULL;
			}

			UpdateCB = si.UpdateCB;
			hWnd = si.hWnd;

			return *this;
		}
} VILVSUBITEM;

// states Flags
#define NLVI_DISABLED		0x00000001 // Cannot be selected
#define NLVI_NOP			0x00000002 // not used
#define NLVI_ISSEPARATOR	0x00000004 // Flagged as a separator i.e. a black fat line will be drawn after this item
#define NLVI_HIDDEN			0x00000008 // Is hidden
#define NLVI_SELECTED		0x00000010 // Is Selected
#define NLVI_ISENTITY		0x00000020 // CkEntity Member  is valid
#define NLVI_EDITABLE		0x00000040 // Can edit name
#define NLVI_DRAGABLE		0x00000080 // Can be dragged
#define NLVI_ALPHAFIRST		0x00000100 // Must be in top regardless of the sort fct used 
#define NLVI_OWNERDRAW		0x00000200 // Owner Drawn (if on main HNTVITEM, then the whole row is custom draw)
#define NLVI_MAINOWNERDRAWN	0x00000400	//if on main HNTVITEM, then the whole column is custom draw only

// SetItem Flags 
#define NLVIF_Flags			0x00000001 // Set Flags
#define NLVIF_FONT			0x00000002 // Set font
#define NLVIF_CKENTITY		0x00000004 // Set entity
#define NLVIF_DATA			0x00000008 // Set data
#define NLVIF_ICON			0x00000010 // Set icon
#define NLVIF_REDRAW		0x00000020 // Redraw After Flag settings

// Hit Test return Flags

#define NLVHT_NOWHERE	  		0x00000001	// In the client area but below the last item.
#define NLVHT_ONITEMBUTTON		0x00000002  // On the button associated with an item.
#define NLVHT_ONITEMICON		0x00000004  // On the bitmap associated with an item.
#define NLVHT_ONITEMINDENT		0x00000008  // In the indentation associated with an item.
#define NLVHT_ONITEMLABEL		0x00000010  // On the label (string) associated with an item.
#define NLVHT_ONITEMRIGHT		0x00000020  // Right off the label.
#define NLVHT_ONITEMSUB			0x00000040  // On sub Item
#define NLVHT_ONITEMSUBICON		0x00000080  // On sub Item icon
#define NLVHT_ONHEADER			0x00000100  // On Header

#define NLVHT_ONITEM			( NLVHT_ONITEMICON | NLVHT_ONITEMLABEL )// On the bitmap or label associated with an item.


// Select Flags
#define NLVS_SELECT			0x00000001 // Select Item
#define NLVS_UNSELECT		0x00000002 // Deselect Item
#define NLVS_TOGGLE			0x00000004 // Toggle Item Selection
#define NLVS_ADD			0x00000008 // Add to current Selection
#define NLVS_REDRAW			0x00000010 // Redraw after selection

typedef void (*NLVUpdateItemCB)(tagNLVITEM *it);

typedef struct tagNLVITEM
{
	CString			Text;		// Text used for item that are not entities
	int				Icon;		// Icon Index Associated with item
	int				Font;		// Font Used to draw Item
	DWORD			CkEntity;	// Is either an object id or a CK class ID for subfolders (check for Flags & IS_ENTITY)
	DWORD			Flags;		// Flags
	LPVOID			Data;		// Associated Data
	VILVSUBITEM	**pSubs;	// Array of sub items
	int				nSubs ;		// Number of subs
	int				Height;			// height of item
	int				ToTop;			// Distance to top
	RECT			rcText;			// Text Rectangle
	RECT			rcClippedText;  // Clipped text rectangle
	NLVUpdateItemCB UpdateCB;
	tagNLVITEM*		PartOf;			// Part of 
	tagNLVITEM()
		{
			Icon		= 0;
			Font		= 0;
			CkEntity	= 0;
			Flags		= 0;
			Data		= NULL;
			pSubs		= NULL;
			nSubs		= 0;
			Height		= 0;
			ToTop		= 0;
			PartOf		= NULL;
			UpdateCB	= NULL;
			ZeroMemory(&rcText,sizeof(rcText));
		}
	~tagNLVITEM()
		{
			if(pSubs)
			{
				int i;
				for(i = 0; i < nSubs; i++ )
				{
					if(pSubs[i])
						delete pSubs[i];
				}
				delete [] pSubs;
			}
		}
} NLVITEM,*HNLVITEM;

typedef int (*MULTILISTSORTF)(const void* it1,const void* it2);

#ifdef CUIK
class CMultiListCtrl : public CWnd
#else
class CKCONTROLS_CLASS_DECL CMultiListCtrl : public CWnd
#endif
{
// Construction
public:

// Ctors

	CMultiListCtrl(CKContext* context=0);
	virtual ~CMultiListCtrl();
	BOOL Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd,	UINT nID);
	BOOL CreateEx(DWORD dwExStyle, DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID);
	
	void SetCKContext(CKContext* context);	//set ck context if you plan to use ckentities inside tree / list
	
	void  SetStyle(DWORD Style);
	void  ModifyStyle(DWORD iAdded,DWORD iRemoved=0);
	DWORD GetStyle();

// List Functions :

	void UpdateHScroll();
	void UpdateVScroll();

// Image List management
	
	void SetButtonImageList(CImageList *il); // Image List used to Draw arraws next to the Item Name
	void SetItemImageList(CImageList *il);   // 
	void SetSubItemImageList(CImageList *il);

	CImageList *GetItemImageList();
	CImageList *GetSubItemImageList();
	CImageList *GetButtonImageList();
	
// Item Pre-allocation Size

	void SetPreAllocSize(int size);
	int  GetPreAllocSize();

// Picking

	HNLVITEM HitTest(CPoint point,int *sub,UINT *Flags);



// Apparence / Color / Font

	void SetFont( CFont* pFont, BOOL bRedraw);
	void SetColors(	COLORREF backColor,
				COLORREF textColor,
				COLORREF textBackColor,
				COLORREF textSelectedColor,
				COLORREF textBackSelectedColor,
				COLORREF textActivePartBackColor,
				COLORREF borderColor=0
				);
	void CMultiListCtrl::GetColors(	
				COLORREF *backColor,
				COLORREF *textColor,
				COLORREF *textBackColor,
				COLORREF *textSelectedColor,
				COLORREF *textBackSelectedColor,
				COLORREF *textActivePartBackColor,
				COLORREF *borderColor=NULL
				);

	
// Item Misc

	int GetItemCount();
	HNLVITEM GetItem(int pos);
	int GetItemPos(HNLVITEM item);
	
// Iterator

	
	int GetVisibleCount();	
	BOOL IsItemVisible(HNLVITEM item);

	HNLVITEM GetFirstVisibleItem();
	HNLVITEM GetLastVisibleItem();

	HNLVITEM GetNextVisibleItem(HNLVITEM item);
	HNLVITEM GetPreviousVisibleItem(HNLVITEM item);	
	HNLVITEM GetNextVisibleItem(int pos);
	HNLVITEM GetPreviousVisibleItem(int pos);

	
	HNLVITEM GetNextItem(HNLVITEM item);
	HNLVITEM GetNextItem(int pos);

	HNLVITEM GetPreviousItem(HNLVITEM item);
	HNLVITEM GetPreviousItem(int pos);

	int FindItemPosition(HNLVITEM item);

// Item Insertion  
	
	HNLVITEM InsertItem( LPCTSTR lpszItem, HNLVITEM hAfter = NULL, BOOL draw = TRUE);
	HNLVITEM InsertItem( LPCTSTR lpszItem, int IconId = -1,HNLVITEM hAfter = NULL, BOOL draw = TRUE);	
	HNLVITEM InsertItem( CK_ID ckid,HNLVITEM hAfter = NULL,BOOL draw = TRUE);
	HNLVITEM InsertItem( HNLVITEM it,HNLVITEM hAfter = NULL,BOOL draw = TRUE);

// Item Removal Functions

	void	DeleteItem(HNLVITEM item,BOOL redraw = TRUE);
	void	DeleteItem(int pos,BOOL redraw = TRUE);
	void	DeleteAllItem(BOOL redraw = TRUE);
	void	DeleteSelectedItems(BOOL redraw = TRUE);
	
//Move Selection
	HNLVITEM	RemoveItem(HNLVITEM item,BOOL redraw=FALSE);	//no deletion of the HNLVITEM
	HNLVITEM	RemoveItem(int pos,BOOL redraw=FALSE);	//no deletion of the HNLVITEM

	void	MoveSelectionUp(BOOL redraw=TRUE);
	void	MoveSelectionDown(BOOL redraw=TRUE);
	void	MoveSelectionToTop(BOOL redraw=TRUE);
	void	MoveSelectionToBottom(BOOL redraw=TRUE);
	
	
// Item content management

	BOOL	SetItemData( HNLVITEM hItem, LPVOID dwData);
	LPVOID	GetItemData( HNLVITEM hItem);
	
	BOOL	SetItemFont( HNLVITEM hItem, int font);
	int		GetItemFont( HNLVITEM hItem);
	
	BOOL	SetItemFlags( HNLVITEM hItem, DWORD Flags);
	DWORD	GetItemFlags( HNLVITEM hItem);
	
	BOOL	SetItemIcon( HNLVITEM hItem, int Icon);
	int		GetItemIcon( HNLVITEM hItem);
	
	void SetItemVisibility(HNLVITEM it,BOOL vis);
	BOOL GetItemVisibility(HNLVITEM it);

	void SetItemDragable(HNLVITEM it,BOOL drag);
	BOOL GetItemDragability(HNLVITEM it);

	BOOL	SetItemProperties( HNLVITEM hItem,DWORD mask,NLVITEM *it);
	
	void SetItemHeight(int height);
	int  GetItemHeight();

	void SetItemHeight(HNLVITEM item,int height);
	int  GetItemHeight(HNLVITEM item);

// Selection Management

	int  GetCurSelRow();
	void Select(HNLVITEM item,DWORD Flags,BOOL notify = TRUE);

	void SelectRange(HNLVITEM itemStart,HNLVITEM itemEnd,DWORD Flags);	
	void SelectRange(int start,int end,DWORD Flags);

	BOOL IsItemSelected(HNLVITEM item);

	HNLVITEM GetLastSelected();
	HNLVITEM GetPrevSelected();
	
	CPtrList *GetSelectedItems();
	int GetSelectedItemsCount();

	void ClearSelection(BOOL redraw,BOOL sync = TRUE);

	void SelectItemByID(CK_ID id,HNLVITEM start);
	void SelectVisibleItemByID(CK_ID id,HNLVITEM partof);
	
	BOOL IsSelectionLocked();
	void LockSelection(BOOL bLock);


// Label Edition

	CEdit *GetEdit();

	CEdit* EditItemText(HNLVITEM it);
	HNLVITEM GetEditedEntity();
	
	CEdit* EditSubItemText(HNLVITEM it,int sub);
	int GetEditedSubItemIndex();

// Sub Item Functions 


	void SwapSubItems(HNLVITEM item,int p1,int p2);
	
	void AddSubItem(HNLVITEM item,int subItem, VILVSUBITEM *ntvsi);
	void SetSubItem(HNLVITEM item,int subItem, VILVSUBITEM *ntvsi);
	
	void  SetSubItemFlags(HNLVITEM item,int subItem, DWORD Flags);
	DWORD GetSubItemFlags(HNLVITEM item,int subItem);
	
	void  SetSubItemData(HNLVITEM item,int subItem, void *data);
	void* GetSubItemData(HNLVITEM item,int subItem);
	

	void  SetSubItemText(HNLVITEM item,int subItem, char *text);
	char *GetSubItemText(HNLVITEM item,int subItem);

	VILVSUBITEM *GetSubItem(HNLVITEM item,int subItem);
	void RemoveSubItem(HNLVITEM item,int subItem);

	int GetSubItemCount(HNLVITEM item);
	void GetItemRect(HNLVITEM item, int col,RECT *r, BOOL textOnly);

	void GetItemEditRect(HNLVITEM item, int col,RECT *r);

	int GetSubItemHeight(HNLVITEM item,int subItem);
	void SetSubItemHeight(HNLVITEM item,int subItem, int height);

// Column Functions

	// Set Selected Column

	void SetSelectedColumn(int Column);
	int  GetSelectedColumn();
	
	void SetColumnCount(int count);
	int  GetColumnCount();

	void InsertColumn(char *name,int pos=-1,int size = 100);
	void SwapColumns(int c1,int c2);
	
	void SetColumn(int pos,CString str,int image,int width);
	void SetActiveColumn(int pos);
	int GetActiveColumn();

	void SetColumnWidth(int pos, int w);
	void GetColumnInfo(int index,int *start, int *width);

	void SetColumnSortFct(int col, MULTILISTSORTF sortf);
	void SizeColumnToOptimalSize(int col);

	void SetColumnText(int pos,CString str);

	void SetColumnOrder(int *order);

	void AddColumn();
	void RemoveColumn(int pos);

	void SwapItems(int pos1, int pos2);

	void SetHeaderColors(	
				COLORREF backColor,
				COLORREF textColor,
				COLORREF textBackColor,
				COLORREF textSelectedColor,
				COLORREF textBackSelectedColor
				);

// Sorting

	void Sort(HNLVITEM start = NULL,HNLVITEM end = NULL,BOOL redraw = FALSE);
	void SetCurrentSort(int col);

// ID Searching Function 

	HNLVITEM IdFind(CK_ID id, HNLVITEM startat); 
	HNLVITEM IdFind(CK_ID id, int pos); 
	
// DnD Management

	CImageList *CreateDragImage(HNLVITEM item); // It up to you to destroy img list after use

// Misc

	void Check(HNLVITEM start = NULL); // Check for every item if the corresopnding CKObject still exists, if not, deletes the item

// Show/Hide Layer

	void ToggleShowHideLayer();
	BOOL HideLayerOn();

// Active Part

	void SetActivePart(HNLVITEM it);
	HNLVITEM GetActivePart();
	

// Item Drawing	

	BOOL DrawItem(HNLVITEM item,CDC *cdc = NULL,BOOL draw = TRUE);
	BOOL DrawOwnerDrawItem(HNLVITEM item,int sub,BOOL highlight=FALSE);	//sub =-1=> main, otherwise subitems

	void RedrawItems(HNLVITEM first = NULL,HNLVITEM last = NULL,BOOL draw = TRUE);
	
	void UpdateVisibleList(HNLVITEM start = NULL);
	void RemoveEntity(HNLVITEM start,CK_ID id);
	
	void EnsureVisible(HNLVITEM it);

//Clipboard
	void CopyContentToClipboard();
	
// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMultiListCtrl)
	protected:
	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	//}}AFX_VIRTUAL

	// Generated message map functions
protected:
	//{{AFX_MSG(CMultiListCtrl)
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnDestroy();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
	afx_msg BOOL OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message);
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnMButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

// Members Datas


	CKContext			*m_Context;

	int					m_NbColumns;

	CBrush				m_Brush;
	
	CMHeaderCtrl		*m_Header;
	int					m_HeaderSize;	
	DWORD				m_Style;
	COLORREF			m_GreyedColor;
	CImageList			*m_SIL;
	CImageList			*m_IL;
	CImageList			*m_SubIL;

	BOOL				m_HideLayerOn;
	BOOL				m_bDrag;
	CPoint				m_LastPickedPoint;
	
// List data :

	int m_PreallocSize;
	int m_ItemCount;
	int m_AllocatedItemCount;

	HNLVITEM *m_ItemArray;

	int m_StartIndent;
	int m_ItemHeight;
	int m_IconSize;
	int m_IconSpace;	
	
	CPtrList m_SelectedItems;

	HNLVITEM m_LastPicked;
	
	int m_HScrollPos;
	int m_VScrollPos;

	int m_ListHeight;
	BOOL m_bNeedUpdate;
	
	HNLVITEM m_ActivePart;
	int m_WheelSize;

	MULTILISTSORTF m_SortFct;
	MULTILISTSORTF *m_SortFctArray;
	
	CMLEdit *m_Edit;

	HNLVITEM m_EditedItem;
	int m_EditedSub;
// Colors :

	DWORD m_backColor;
	DWORD m_textColor;
	DWORD m_textBackColor;
	DWORD m_textSelectedColor;
	DWORD m_textBackSelectedColor;
	DWORD m_textActivePartBackColor;
	DWORD m_borderColor;

	CFont *m_Font;
	BOOL m_Locked;	//use not implemented

	int m_SelectedColumn;

//rollover with NLVS_OWNERDRAWROLLOVER
	HNLVITEM	m_rolloverItem;
	int			m_rolloverSubitem;

	//
	void _Create(DWORD dwStyle, const RECT& rect);

	
/*****************************************/
// Members Function

	HRESULT ProcessHeaderNotification(LPNMHDR nm);
	
	int		XToColumn(int x);	 
	int		GetColumnOptimalSize(int c);
	void	CalcColumnRect(int c,RECT *r);

	void OnVKUp(BOOL extend= FALSE);
	void OnVKDown(BOOL extend= FALSE);
	
	void OnVKPgUp(BOOL extend= FALSE);
	void OnVKPgDown(BOOL extend = FALSE);

	void OnVKHome(BOOL extend= FALSE);
	void OnVKEnd(BOOL extend= FALSE);

	void ScrollUp();
	void ScrollDown();
	
	DECLARE_MESSAGE_MAP()
	
};

#ifndef CUIK
}	//namespace CKControl
#endif

#endif
