// class CKVIPopupMenu
// interface to manipulate easily popup menu
// with CK pause added on popup menu show

#pragma once

#include "VIPopupMenu.h"

#ifdef CKCONTROLS_API
   #define CKCONTROLS_CLASS_DECL  __declspec(dllexport)
#else
   #define CKCONTROLS_CLASS_DECL  __declspec(dllimport)
#endif

namespace CKControl	//----------------------------------------
{

class CKCONTROLS_CLASS_DECL CKVIPopupMenu : public VIPopupMenu
{
public:
	enum {	VPF_CK_WAS_PLAYING					= 0x08000000
	}CKVIPOPUPMENUFLAG;

	CKVIPopupMenu(CWnd* parent);
	CKVIPopupMenu(CMenu* menu);
	CKVIPopupMenu();

	//show the menu at the specified position
	//pause CK engine
	virtual HRESULT	Show(long x,long y);

	//set context
	void			SetContext(CKContext* context);

protected:
	//var initialization
	virtual void	Init();

private:
	CKContext*	m_Context;
};

}	//namespace CKControl