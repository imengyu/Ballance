// CMultiTreeCtrl.h : header file

#ifndef _CMULTITREECTRL_H_
#define _CMULTITREECTRL_H_

#include "CMHeaderCtrl.h"
#include "XArray.h"

/*
Doc:
-if no column (count==0), header uses treectrl->GetWindowText string

-Sort(item) in multicolumn won't work if you forgot to call
void SetColumnSortFct(int col, MULTITREESORTF sortf);
or
void SetCurrentSortFct(MULTITREESORTF sortf);
void SetColumnSort() is not enough
*/

#ifndef CUIK
namespace CKControl	//----------------------------------------
{
#endif

/////////////////////////////////////////////////////////////////////////////
// Special Edit for MT

class CMTEdit : public CEdit
{

	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam); 
};



/////////////////////////////////////////////////////////////////////////////
// CMultiTreeCtrl window

#define NTVS_SUBITEMSELECT		0x00000001
#define NTVS_DRAWHSEPARATOR		0x00000002
#define NTVS_DRAWVSEPARATOR		0x00000004
#define NTVS_NORIGHTSELECT		0x00000008
#define NTVS_ALLOWCOLUMNDROP	0x00000010
#define NTVS_SINGLESELECTION	0x00000020
#define NTVS_DRAWBORDER			0x00000040		//does not work yet
#define NTVS_DRAGRECT_AVAILABLE_1STCOLUMN		0x00000080	//enabledragrect(true), available if click after text
#define NTVS_DRAGRECT_AVAILABLE_1STCOLUMN_ALT	0x00000100	//enabledragrect(true), available if click after text
#define NTVS_OWNERDRAWN_BACKnTEXTCOLOR	0x00000200	//u'll receive a WM_DRAWITEM msg,dlgid,&DRAWITEMSTRUCT , please change
													//dis.itemAction	= BackColor;
													//dis.itemState	= TextColor;
#define MTN_CUSTOMCOLORS	WM_USER+150	//Multi Tree Notification: wParam = controlID, lParam = MTN_CUSTOMCOLORS_STRUCT*

/* defined later:
struct MTN_CUSTOMCOLORS_STRUCT
{
COLORREF	BackColor;
COLORREF	TextColor;
HNTVITEM	Item;	//HNTVITEM 
MTN_CUSTOMCOLORS_STRUCT(COLORREF bc,COLORREF tc,HNTVITEM i) : BackColor(bc),TextColor(tc),Item(i) {}
};
*/


#define NTVSI_NOP	 		0x00000001
#define NTVSI_TEXT	 		0x00000002
#define NTVSI_BITMAP 		0x00000004
#define NTVSI_WINDOW		0x00000008
#define NTVSI_WINDOWCLIPPED	0x00000010
#define NTVSI_OWNERDRAWN	0x00000020
#define NTVSI_EDITABLE 		0x00000040
#define NTVSI_TEXTNODELETE	0x00000080

#define NTVSI_TOP           0x00000000
#define NTVSI_LEFT          0x00000000
#define NTVSI_CENTER        0x00000100
#define NTVSI_RIGHT         0x00000200
#define NTVSI_VCENTER       0x00000400
#define NTVSI_BOTTOM        0x00000800

#define NTVSI_HIDDEN		0x00001000
#define NTVSI_STATENORMAL	0x00002000
#define NTVSI_STATEINTER	0x00004000
#define NTVSI_STATEPUSHED	0x00008000


#define NTVSI_DEFAULT		(NTVSI_TEXT | NTVSI_LEFT | NTVSI_VCENTER)

struct tagNTVITEM;
struct tagVITVSUBITEM;

typedef void (*NTVUpdateValueCB)(tagNTVITEM *it,int sub,tagVITVSUBITEM *subitem);

typedef struct tagVITVSUBITEM{
	char *Text;
	int iIndex;
	int iInterIndex;
	int iPushedIndex;
	union {
		DWORD flags;
		DWORD Flags;
	};
	void *data;
	NTVUpdateValueCB UpdateCB;
	HWND hWnd;
	RECT rcItem; // used for Picking not copied
	public :
		tagVITVSUBITEM()
		{
			Text			= NULL;
			flags			= NTVSI_DEFAULT;
			iIndex			= -1;
			iInterIndex		= -1;
			iPushedIndex	= -1;
			rcItem.top		= 0;
			rcItem.bottom	= 0;
			rcItem.left		= 0;
			rcItem.right	= 0;			
			hWnd			= NULL;
			UpdateCB		= NULL;	//update call back, called each redraw time
			data			= NULL;
		}
		~tagVITVSUBITEM()
		{
			if ((flags & NTVSI_TEXTNODELETE)==0)
				delete [] Text;

			if(hWnd)
				::DestroyWindow(hWnd);
		}

		tagVITVSUBITEM& operator=(const tagVITVSUBITEM& si)
		{
			flags			= si.flags;
			iIndex			= si.iIndex;
			iInterIndex		= si.iInterIndex;
			iPushedIndex	= si.iPushedIndex;
			hWnd			= si.hWnd;
			UpdateCB		= si.UpdateCB;
			data			= si.data;
			hWnd			= si.hWnd;

			delete [] Text;

			if(si.Text)
			{
				Text = new char[strlen(si.Text) + 1];
				strcpy(Text,si.Text);
			}
			else
				Text = NULL;
			
			return *this;
		}
} VITVSUBITEM;

// states Flags
#define NTVI_DISABLED		0x00000001 // Cannot be selected
#define NTVI_DONOTSORT		0x00000002 // Don't sort children
#define NTVI_ISSEPARATOR	0x00000004 // Flagged as a separator i.e. a black fat line will be drawn after this item
#define NTVI_HIDDEN			0x00000008 // Is hidden
#define NTVI_EXPANDED		0x00000010 // Is Expanded
#define NTVI_SELECTED		0x00000020 // Is Selected
#define NTVI_ISENTITY		0x00000040 // CkEntity Member  is valid
#define NTVI_HIDEEMPTY		0x00000080 // hidden if not children present
#define NTVI_EDITABLE		0x00000100 // Can edit name
#define NTVI_DRAGABLE		0x00000200 // Can be dragged
#define NTVI_ALPHAFIRST		0x00000400 // Must be in top regardless of the sort fct used 
#define NTVI_ALPHALAST		0x00000800 // Must be at bottom regardless of the sort fct used 
#define NTVI_PREALLOC		0x00001000 // Prealloc sub-childrens 
#define NTVI_OWNERDRAW		0x00002000 // Owner Drawn
#define NTVI_AFTEROWNERDRAW	0x00004000 // Owner Drawn after original draw
#define NTVI_FREE			0x00100000 // Free Flag for user

// SetItem Flags 
#define NTVIF_FLAGS			0x00000001 // Set flags
#define NTVIF_FONT			0x00000002 // Set font
#define NTVIF_CKENTITY		0x00000004 // Set entity
#define NTVIF_DATA			0x00000008 // Set data
#define NTVIF_ICON			0x00000010 // Set icon
#define NTVIF_REDRAW		0x00000020 // Redraw After Flag settings

// Hit Test return Flags

#define NTVHT_NOWHERE	  		0x00000001	// In the client area but below the last item.
#define NTVHT_ONITEMBUTTON		0x00000002  // On the button associated with an item.
#define NTVHT_ONITEMICON		0x00000004  // On the bitmap associated with an item.
#define NTVHT_ONITEMINDENT		0x00000008  // In the indentation associated with an item.
#define NTVHT_ONITEMLABEL		0x00000010  // On the label (string) associated with an item.
#define NTVHT_ONITEMRIGHT		0x00000020  // Right off the label.
#define NTVHT_ONITEMSUB			0x00000040  // On sub Item
#define NTVHT_ONITEMSUBICON		0x00000080  // On sub Item icon
#define NTVHT_ONHEADER			0x00000100  // On Header

#define NTVHT_ONITEM			( NTVHT_ONITEMICON | NTVHT_ONITEMLABEL )// On the bitmap or label associated with an item.


// Expand Flags 
#define NTVE_REDRAW	  		0x00000001	// Redraw after selection
#define NTVE_EXPAND			0x00000002  // Expand Item
#define NTVE_COLLAPSE		0x00000004  // Collapse Item
#define NTVE_TOGGLE			0x00000008  // Toggle Item
#define NTVE_CALCCOL		0x00000010  // Recalculate header size to match items sizes

// Select Flags
#define NTVS_SELECT			0x00000001 // Select Item
#define NTVS_UNSELECT		0x00000002 // Deselect Item
#define NTVS_TOGGLE			0x00000004 // Toggle Item Selection
#define NTVS_ADD			0x00000008 // Add to current Selection
#define NTVS_REDRAW			0x00000010 // Redraw after selection


// Find Flags
#define NTVF_CHILD			0x00000001 // Get Child 
#define NTVF_BROTHER		0x00000002 // Get Brother
#define NTVF_ALL			0xffffffff  

typedef void (*NTVUpdateItemCB)(tagNTVITEM *it);

typedef struct tagNTVITEM
{
	CString					Text;		// Text used for item that are not entities
	int						Icon;		// Icon Index Associated with item
	int						Font;		// Font Used to draw Item
	DWORD					CkEntity;	// Is either an object id or a CK class ID for subfolders (check for flags & IS_ENTITY)
	DWORD					Flags;		// Flags
	LPVOID					Data;		// Associated Data
	XArray<VITVSUBITEM*>	Subs;	// Array of sub items
	tagNTVITEM*				hParent;	// Parent item
	XArray<tagNTVITEM*>		Children;	// Array of child
	int						Depth;			// number of parents
	int						Height;			// height of item
	int						ToTop;			// Distance to top
	RECT					rcText;			// Text Rectangle
	RECT					rcClippedText;  // Clipped text rectangle
	NTVUpdateItemCB			UpdateCB;
	tagNTVITEM*				ParentFolder;   // Item controled by
	tagNTVITEM*				PartOf;			// Part of 
	tagNTVITEM()
		{
			Icon		= 0;
			Font		= 0;
			CkEntity	= 0;
			Flags		= 0;
			Data		= NULL;
			hParent		= NULL;
			Depth		= 0;
			Height		= 0;
			ToTop		= 0;
			ParentFolder= NULL;
			PartOf		= NULL;
			UpdateCB	= NULL;
			ZeroMemory(&rcText,sizeof(rcText));
		}
	~tagNTVITEM()
		{
			int i,count = Subs.Size();
			for(i = 0; i < count; i++ )
				delete Subs[i];										
		}
} NTVITEM,*HNTVITEM;

struct MTN_CUSTOMCOLORS_STRUCT
{
	COLORREF	BackColor;
	COLORREF	TextColor;
	HNTVITEM	Item;	//HNTVITEM 
	MTN_CUSTOMCOLORS_STRUCT(COLORREF bc,COLORREF tc,HNTVITEM i) : BackColor(bc),TextColor(tc),Item(i) {}
};


#define tviIsDisabled(i) (i->Flags & NTVI_DISABLED)
#define tviIsNotSorted(i) (i->Flags & NTVI_DONOTSORT)
#define tviIsSeparator(i) (i->Flags & NTVI_ISSEPARATOR	)
#define tviIsHidden(i) (i->Flags & NTVI_HIDDEN)
#define tviIsExpanded(i) (i->Flags & NTVI_EXPANDED)
#define tviIsSelected(i) (i->Flags & NTVI_SELECTED)
#define tviIsEntity(i) (i->Flags & NTVI_ISENTITY)
#define tviIsHideEmpty(i) (i->Flags & NTVI_HIDEEMPTY)
#define tviIsEditable(i) (i->Flags & NTVI_EDITABLE)
#define tviIsDragable(i) (i->Flags & NTVI_DRAGABLE)
#define tviIsAlphaFirst(i) (i->Flags & NTVI_ALPHAFIRST)
#define tviIsPreAlloc(i) (i->Flags & NTVI_PREALLOC)
#define tviIsOwnerDrawn(i) (i->Flags & NTVI_OWNERDRAW);

typedef int (*MULTITREESORTF)(const void* it1,const void* it2);

#ifdef CUIK
class CMultiTreeCtrl : public CWnd
#else
class CKCONTROLS_CLASS_DECL CMultiTreeCtrl : public CWnd
#endif
{
// Construction
public:

// Ctors

	CMultiTreeCtrl(CKContext *context=0);
	virtual ~CMultiTreeCtrl();
	BOOL Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd,	UINT nID);
	
	void SetCKContext(CKContext* context);	//set ck context if you plan to use ckentities inside tree / list
	
	void SetStyle(DWORD Style);
	DWORD GetStyle();

	static void CompressArray(XArray<HNTVITEM>& ioArray);
	static void CompressArray(XArray<HNTVITEM>& iArray,XArray<HNTVITEM>& oResult);


// Image List management
	
	void SetButtonImageList(CImageList *il); // Image List used to Draw arraws next to the Item Name
	void SetItemImageList(CImageList *il,BOOL del=FALSE);   // 
	void SetSubItemImageList(CImageList *il);

	CImageList *GetItemImageList();
	CImageList *GetSubItemImageList();
	CImageList *GetButtonImageList();
	
// Item Pre-allocation Size

	void SetPreAllocSize(int size);
	int  GetPreAllocSize();

// Picking

	HNTVITEM HitTest(CPoint point,int *sub,UINT *flags);
	
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
	void CMultiTreeCtrl::GetColors(	
				COLORREF *backColor,
				COLORREF *textColor,
				COLORREF *textBackColor,
				COLORREF *textSelectedColor,
				COLORREF *textBackSelectedColor,
				COLORREF *textActivePartBackColor,
				COLORREF *borderColor=NULL
				);

	int GetIndent();
	void SetIndent(int indent);

// Item Misc

	int GetItemCount();

	BOOL ItemHasChildren(HNTVITEM item);
	BOOL ItemHasVisibleChildren(HNTVITEM item);

// Iterator

	
	int GetVisibleCount();	
	BOOL IsItemVisible(HNTVITEM item);

	HNTVITEM GetFirstVisibleItem();
	HNTVITEM GetNextVisibleItem(HNTVITEM item);
	HNTVITEM GetPreviousVisibleItem(HNTVITEM item);
	HNTVITEM GetLastVisibleItem();

	
	HNTVITEM GetRoot();
	HNTVITEM GetLastItem(HNTVITEM it = NULL);
	HNTVITEM GetNextItem(HNTVITEM item);
	HNTVITEM GetNextSibling(HNTVITEM item);
	HNTVITEM GetPreviousSibling(HNTVITEM item);
	HNTVITEM GetParentItem(HNTVITEM item);

// Item Insertion  
	
	HNTVITEM InsertItem( LPCTSTR lpszItem, HNTVITEM hParent = NULL, BOOL draw = TRUE);
	HNTVITEM InsertItem( LPCTSTR lpszItem, int IconId = -1,HNTVITEM hParent = NULL, BOOL draw = TRUE);	
	HNTVITEM InsertItem( CK_ID ckid,HNTVITEM hParent = NULL,BOOL draw = TRUE);
	HNTVITEM InsertItem( HNTVITEM it,HNTVITEM Parent = NULL,BOOL draw = TRUE);

// Item Removal Functions

	void	DeleteItem(HNTVITEM item,BOOL redraw = TRUE);
	void	DeleteAllItem(BOOL redraw = TRUE);
	void	DeleteChildren(HNTVITEM it,BOOL redraw = TRUE);
	void	DeleteBranch(HNTVITEM it,BOOL redraw = TRUE);
	void	RemoveChild(HNTVITEM hParent,HNTVITEM hChild);
	void	DeleteSelectedItems(BOOL draw = TRUE);
	void	DeleteFind(XArray<CK_ID>& ids);
	void	DeleteCheck(); 

// Hierarchy Management

	void SetItemParent(HNTVITEM hChild ,HNTVITEM hParent);
	
	void Expand(HNTVITEM item,DWORD flags);
	void ExpandAll(HNTVITEM item,DWORD flags);

// Item content management

	BOOL	SetItemData( HNTVITEM hItem, LPVOID dwData);
	LPVOID	GetItemData( HNTVITEM hItem);
	
	BOOL	SetItemFont( HNTVITEM hItem, int font);
	int		GetItemFont( HNTVITEM hItem);
	
	BOOL	SetItemFlags( HNTVITEM hItem, DWORD Flags);
	DWORD	GetItemFlags( HNTVITEM hItem);
	
	BOOL	SetItemIcon( HNTVITEM hItem, int Icon);
	int		GetItemIcon( HNTVITEM hItem);
	
	BOOL	SetItemProperties( HNTVITEM hItem,DWORD mask,NTVITEM *it);
	
	void SetItemHeight(int height);
	int  GetItemHeight();

	void SetItemHeight(HNTVITEM item,int height);
	int  GetItemHeight(HNTVITEM item);

	void SetItemVisibility(HNTVITEM it,BOOL vis);
	BOOL GetItemVisibility(HNTVITEM it);

	void SetItemDragable(HNTVITEM it,BOOL drag);
	BOOL GetItemDragability(HNTVITEM it);

	void SetItemDepth(HNTVITEM item,int depth);
	int  GetItemDepth(HNTVITEM item);

	void SetItemSortable(HNTVITEM it,BOOL sort);
	BOOL GetItemSortability(HNTVITEM it);


// Selection Management

	void Select(HNTVITEM item,DWORD flags,BOOL notify = TRUE);
	void SelectRange(HNTVITEM itemStart,HNTVITEM itemEnd,DWORD flags);
	
	BOOL IsItemSelected(HNTVITEM item);

	HNTVITEM GetLastSelected();
	HNTVITEM GetPrevSelected();
	
	int GetSelectedItemsCount();
	HNTVITEM GetSelectedItem(int i);

	XArray<HNTVITEM> *GetSelectedItems();

	void ClearSelection(BOOL redraw,BOOL sync = TRUE);

	void SelectItemByID(CK_ID id,HNTVITEM start);
	void SelectVisibleItemByID(CK_ID id,HNTVITEM partof);
	
	BOOL IsSelectionLocked();
	void LockSelection(BOOL bLock);


// Label Edition

	CEdit *GetEdit();

	CEdit* EditItemText(HNTVITEM it);
	HNTVITEM GetEditedEntity();
	
	CEdit* EditSubItemText(HNTVITEM it,int sub);
	int GetEditedSubItemIndex();

// Sub Item Functions 

	void AddSubItem(HNTVITEM item,int subItem, VITVSUBITEM *ntvsi);	//note 1st subitem index is 0, but its column index is 1
	void SetSubItem(HNTVITEM item,int subItem, VITVSUBITEM *ntvsi);	//note 1st subitem index is 0, but its column index is 1
	
	void  SetSubItemFlags(HNTVITEM item,int subItem, DWORD flags);
	DWORD GetSubItemFlags(HNTVITEM item,int subItem);
	
	void  SetSubItemText(HNTVITEM item,int subItem, char *text);
	char *GetSubItemText(HNTVITEM item,int subItem);


	VITVSUBITEM *GetSubItem(HNTVITEM item,int subItem);
	void RemoveSubItem(HNTVITEM item,int subItem);

	int GetSubItemCount(HNTVITEM item);
	void GetItemRect(HNTVITEM item, int col,RECT *r, BOOL textOnly);

	void GetItemEditRect(HNTVITEM item, int col,RECT *r);

	// Column Functions
	
	void SetColumnCount(int count);	//note columns indexes from 1 to n are connected to subitem index from 0 to n-1
	int  GetColumnCount();	//note columns indexes from 1 to n are connected to subitem index from 0 to n-1

	void SetColumnOrder(int *order);
	void SetColumn(int pos,CString str,int image,int width,MHeaderDrawCallback iCustomDrawCallback=0);

	void SetColumnWidth(int pos, int w);
	void GetColumnInfo(int index,int *start, int *width);
	
	void SetColumnText(int pos,CString str);

	void SetColumnSortFct(int col, MULTITREESORTF sortf);
	void SetCurrentSortFct(MULTITREESORTF sortf);

	void SetHeaderColors(	
				COLORREF backColor,
				COLORREF textColor,
				COLORREF textBackColor,
				COLORREF textSelectedColor,
				COLORREF textBackSelectedColor,
				COLORREF borderColor=0
				);


// Sorting

	void Sort(HNTVITEM parent = NULL,BOOL recurse = FALSE);
	void SetCurrentSort(int col);

// ID Searching Function 

	HNTVITEM ChildIdFind(CK_ID id, HNTVITEM startat = NULL);
	HNTVITEM IdFind(CK_ID id, HNTVITEM startat,BOOL goBack = FALSE,HNTVITEM blocker = NULL); 
	
// DnD Management

	CImageList *CreateDragImage(HNTVITEM item); // It up to you to destroy img list after use

// Misc

	void Check(HNTVITEM start = NULL); // Check for every item if the corresopnding CKObject still exists, if not, deletes the item

	CMHeaderCtrl*	GetHeaderCtrl() {return m_Header;}

// Show/Hide Layer

	void ToggleShowHideLayer();
	BOOL HideLayerOn();

// Active Part

	void SetActivePart(HNTVITEM it);
	HNTVITEM GetActivePart();
	

// Item Drawing	

	void SetRedraw(BOOL redraw = TRUE);

	BOOL	DrawItem(HNTVITEM item,CDC *cdc = NULL,BOOL draw = TRUE);
	void	RedrawItems(HNTVITEM first = NULL,HNTVITEM last = NULL,BOOL draw = TRUE);
	
	void    UpdateVisibleList(HNTVITEM start = NULL);
	void	RemoveEntity(HNTVITEM start,CK_ID id);
	
	void EnsureVisible(HNTVITEM it,BOOL noredraw=FALSE,BOOL iUpdateScrollPos=TRUE);
	HNTVITEM Parser(HNTVITEM root,HNTVITEM it);
	HNTVITEM InvParser(HNTVITEM root,HNTVITEM it);

	//drag rect
	void StartDragRect();
	void StopDragRect();
	void EnableDragRect(BOOL iEnable);

// Operations
public:
	CKContext* m_Context;
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMultiTreeCtrl)
	public:
	virtual BOOL PreTranslateMessage(MSG *pMsg);	//for tooltip
	protected:
	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	//}}AFX_VIRTUAL

	// Generated message map functions
protected:
	//{{AFX_MSG(CMultiTreeCtrl)
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

	int					m_NbColumns;
	
	CBrush				m_BackBrush;
	CBrush				m_BackSelBrush;
	CBrush				m_BackActiveBrush;
	
	CMHeaderCtrl*		m_Header;
	int					m_HeaderSize;	
	DWORD				m_Style;

	CImageList			*m_SIL;
	CImageList			*m_IL;
	CImageList			*m_SubIL;

	BOOL				m_HideLayerOn;
	BOOL				m_bDrag;
	CPoint				m_LastPickedPoint;
	
	BOOL	m_lDown;
	BOOL	m_mDown;
	BOOL	m_rDown;

// Tree data :

	NTVITEM m_hRoot;
	int m_ItemCount;
	int m_Indent;
	int m_StartIndent;
	int m_ItemHeight;
	int m_IconSize;
	int m_ButtonSize;
	int m_IconSpace;	
	
	XArray<HNTVITEM> m_VisibleItems;
	XArray<HNTVITEM>  m_SelectedItems;

	HNTVITEM m_LastPicked;
	
	int m_HScrollPos;
	int m_VScrollPos;

	int m_TreeHeight;
	BOOL m_bNeedUpdate;
	BOOL m_Redraw;

	HNTVITEM m_ActivePart;
	int m_WheelSize;

	VxMutex m_Mutex;


	MULTITREESORTF m_SortFct;
	MULTITREESORTF *m_SortFctArray;
	
	CMTEdit *m_Edit;

	HNTVITEM m_EditedItem;
	int m_EditedSub;

// Colors :

	DWORD m_backColor;
	DWORD m_textColor;
	DWORD m_textBackColor;
	DWORD m_textSelectedColor;
	DWORD m_textBackSelectedColor;
	DWORD m_textActivePartBackColor;
	DWORD m_backDynColor;
	DWORD m_textDynColor;
	DWORD m_borderColor;

	int		m_PreallocSize;
	CFont*	m_Font;
	BOOL	m_Locked;
	BOOL	m_bDelItemImageList;
	int		m_DragRect;//0=>not enabled, 1=>enabled 2=>currently detecting drag, 3=>rect is currently used, ie button down & mouse move
	POINT	m_DragRectLastPoint;

	float	m_ScrollFactor;

// Tree Functions :
	
	void UpdateHScroll();
	void UpdateVScroll();


	
/*****************************************/
// Members Function

	void	InitSubItem1(HNTVITEM item);
	void	InitSubItem2(HNTVITEM item);

	HRESULT ProcessHeaderNotification(LPNMHDR nm);
	
	HNTVITEM LocalGetNextVisibleItem(HNTVITEM item);
	HNTVITEM LocalGetPreviousVisibleItem(HNTVITEM item);

	int		XToColumn(int x);	 
	int		GetColumnOptimalSize(int c);
	void	CalcColumnRect(int c,RECT *r);

	void OnVKUp(BOOL shift, BOOL ctrl);
	void OnVKDown(BOOL shift, BOOL ctrl);
	void OnVKRight(BOOL shift, BOOL ctrl);
	void OnVKLeft(BOOL shift, BOOL ctrl);

	void OnVKPgUp(BOOL shift, BOOL ctrl);
	void OnVKPgDown(BOOL shift, BOOL ctrl);

	void OnVKHome(BOOL shift, BOOL ctrl);
	void OnVKEnd(BOOL shift, BOOL ctrl);

	void ScrollUp();
	void ScrollDown();

	
	DECLARE_MESSAGE_MAP()
	
};

#ifndef CUIK
}	//namespace CKControl
#endif

#endif