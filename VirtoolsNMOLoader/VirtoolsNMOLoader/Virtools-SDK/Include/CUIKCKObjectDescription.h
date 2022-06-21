/*
 * Project:                Virtools Interface Plugin SDK
 * File:                   CUIKCKObjectDescription.h
 * Author:                 Thomas Vissieres
 * Last check date:        11/19/2003 
 * optimized for Tab Size: 4	
 * Quick file summary:     CUIKCKObjectDescription class that store a generic object data to retrieve it later (used by drag&drop operation)
 */

/* CUIKCKObjectDescription class
 * this class is used for storing object data and is used in conjunction with the CUIKEntitySet 
 * for drag & drop operation (see CUIKEntitySet.h)
 * 
 * When you begin a drag & drop operation, create a new CUIKEntitySet and add 
 * the objects descriptions of what you want to drag & drop to it.
 */

#pragma once

#include "VEP_DllEditorLibDefines.h"

class DLLEDITORLIB_CLASS_DECL CUIKCKObjectDescription
{

// ****************
// FUNCTIONS
// ****************
public:

// --------------------------------------------------------
// ctors --------------------------------------------------
// --------------------------------------------------------

	//basic constructor
	CUIKCKObjectDescription();
	
	//
	CUIKCKObjectDescription(UINT classID,char* className,char * resourceName,CKObject* obj=NULL);
	CUIKCKObjectDescription(UINT classID,char* resourceName,CKGUID g);
	CUIKCKObjectDescription(UINT classID,CString resourceName);
	CUIKCKObjectDescription(char* resourceName);
	CUIKCKObjectDescription(CKObject* obj);
	~CUIKCKObjectDescription();

	CKObject*				GetObject(CKContext* context);
	void					SetObjectID(CK_ID id);
	CK_ID					GetObjectID();
	CString					GetResource();
	CString					GetClassName();
	CKGUID					GetGuid();
	UINT					GetObjectClassID();
	int						GetObjectIcon();
	CBitmap*				GetObjectBitmap();
	CKObjectDeclaration*	GetObjectDeclaration();
	
	void					SetObject(CKObject * object);
	void					SetObjectDeclaration(CKObjectDeclaration *od);
	void					SetResource(CString resource);
	void					SetClassName(CString className);
	void					SetGuid(CKGUID guid);
	void					SetObjectClassID(UINT classID);
	void					SetObjectIcon(int icon);
	void					SetObjectBitmap(CBitmap * b,BOOL deleteItOnDestructor=FALSE);

	void					SetClassID(int cid)	{m_ClassID = cid;}
	int						GetClassID()		{return m_ClassID;}

	BOOL operator==(const CUIKCKObjectDescription & desc) const;
	void operator=(const CUIKCKObjectDescription & desc);

#if _MSC_VER >= 1300
	BOOL operator!=(const CUIKCKObjectDescription & desc) const;
	int operator-(const CUIKCKObjectDescription & desc) const;
#endif

// ****************
// ATTRIBUTES
// ****************
private:
	void					Init();
	
	long		m_objectID;
	UINT		m_objetClassID;
	CString		m_className;
	CString		m_resource;
	CKGUID		m_guid;
	int			m_iconIndex;
	CBitmap *	m_bitmap;
	CKObjectDeclaration*	m_oDecl;
	BOOL		m_DeleteBitmap;
	int			m_ClassID;
};
