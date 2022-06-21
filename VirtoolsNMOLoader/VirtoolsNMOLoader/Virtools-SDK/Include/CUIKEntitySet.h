/*
 * Project:                Virtools Interface Plugin SDK
 * File:                   CUIKEntitySet.h
 * Author:                 Thomas Vissieres
 * Last check date:        11/19/2003 
 * optimized for Tab Size: 4	
 * Quick file summary:     CUIKEntitySet class used to store a list of objects(used by drag & drop to know what's dragged)
 */


/* CUIKEntitySet class
 * this class is used for storing dragged objects data during a drag & drop operation
 * It is an array of CUIKCKOnbjectDescription (see CUIKCKOnbjectDescription.h)
 * the CUIKCKObjectDescription is a generic object description : it can contains 
 * a CKObject, a text, an image or a resource file name
 * 
 * When you begin a drag & drop operation, create a new CUIKEntitySet and add 
 * the objects descriptions of what you want to drag & drop to it.
 */

#pragma once

#include "CUIKCKObjectDescription.h"
#include "XClassArray.h"

class DLLEDITORLIB_CLASS_DECL CUIKEntitySet : public XClassArray<CUIKCKObjectDescription>
{
public:
	void	SetClassID(int cid)	{m_ClassID = cid;}
	int		GetClassID()		{return m_ClassID;}

	void*	GetAppData()		{return m_AppData;}
	void	SetAppData(void* d)	{m_AppData = d;}

	CUIKEntitySet() {m_ClassID= CUIK_ENTITYSET_CLASS_ID;}
private:
	int		m_ClassID;
	void*	m_AppData;
};
