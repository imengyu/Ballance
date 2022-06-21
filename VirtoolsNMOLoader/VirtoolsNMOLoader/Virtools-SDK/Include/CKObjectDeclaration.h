/*************************************************************************/
/*	File : CKObjectDeclaration.h										 */
/*	Author :  Nicolas Galinotti											 */	
/*																		 */	
/*	Virtools SDK 															 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.						 */	
/*************************************************************************/
#ifndef CKOBJECTDECLARATION_H

#define CKOBJECTDECLARATION_H "$Id:$"

#include "CKObject.h"

/*****************************************************************************
Summary: External object declaration

Remarks:
+A CKObjectDeclaration is used by Virtools engine to store a list of available extensions objects.
+Only declarations of CKBehaviorPrototype are supported in this version. The CKObjectDeclaration contains 
a short description about the behavior prototype and its author and also contains a pointer to the function 
that will be used to create the behavior prototype when needed.
+A CKObjectDeclaration is created with CreateCKObjectDeclaration.



Example:
	// The following sample creates an object declaration for the "Rotate" behavior. 
	// This object declaration is given :
	//	- The type of object declaration (Must Be CKDLL_BEHAVIORPROTOTYPE)
	//	- A short description of what the behavior is supposed to do.
	//	- The category in which this behavior will appear in the Virtools interface.
	//	- A unique CKGUID
	//	- Author and Version info
	//	- The class identifier of objects to which the behavior can be applied to.
	//	- The function that will create the CKBehaviorPrototype for this behavior. 

	CKObjectDeclaration	*FillBehaviorRotateDecl()
	{
		CKObjectDeclaration *od = CreateCKObjectDeclaration("Rotate");	

		od->SetType(CKDLL_BEHAVIORPROTOTYPE);
		od->SetDescription("Rotates the 3D Entity.");
		od->SetCategory("3D Transformations/Basic");
		od->SetGuid(CKGUID(0xffffffee, 0xeeffffff));
		od->SetAuthorGuid(VIRTOOLS_GUID);
		od->SetAuthorName("Virtools");
		od->SetVersion(0x00010000);
		od->SetCompatibleClassId(CKCID_3DENTITY);

		od->SetCreationFunction(CreateRotateProto);
		return od;
	}	


See also: CKBehaviorPrototype,
********************************************************************************/
class CKObjectDeclaration {
public :
//////////////////////////////////////////////////
////	CKObjectDeclaration Member Functions   ///
//////////////////////////////////////////////////

//-----------------------------------------------------
// Description
void SetDescription(CKSTRING Description);
CKSTRING GetDescription();

//-----------------------------------------------------
// Behavior GUID
void SetGuid(CKGUID guid);
CKGUID GetGuid();


void SetType(int type);

int GetType();

//-----------------------------------------------------
// Dependencie on Managers
void NeedManager(CKGUID Manager);

//-----------------------------------------------------
// Creation function (function that will create the prototype )
void SetCreationFunction(CKDLL_CREATEPROTOFUNCTION f);
CKDLL_CREATEPROTOFUNCTION GetCreationFunction();

//-----------------------------------------------------
// Author information
void SetAuthorGuid(CKGUID guid);
CKGUID GetAuthorGuid();

void SetAuthorName(CKSTRING Name);
CKSTRING GetAuthorName();

//-----------------------------------------------------
// Version information
void SetVersion(CKDWORD verion);
CKDWORD GetVersion();

//-----------------------------------------------------
// Class Id of object to which the declared behavior can apply
void SetCompatibleClassId(CK_CLASSID id);
CK_CLASSID GetCompatibleClassId();

//-----------------------------------------------------
// Category in which the behavior will be presented
void SetCategory(CKSTRING cat);
CKSTRING GetCategory();


/*************************************************
Summary: Gets the name of behavior Prototype.
Return Value:
	A pointer to Behavious Prototype name.
Remarks:
	+ The object declaration name is always the same
	than the behavior prototype.
********************************************/
CKSTRING GetName() { return m_Name.Str(); }

/**************************************************
Summary:Returns the index of the DLL that has declared this object.
Return Value:
	Index of the plugin that registered this object in the plugin manager.
Remarks:
+ The return value is the index of the plugin in the behavior
category of the plugin manager. 
+ To retrieve information about the plugin use :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

CKPluginManager::GetPluginDllInfo(CKPLUGIN_BEHAVIOR_DLL,Index);

{html:</td></tr></table>}

See Also:CKPluginManager
****************************************************/
int		GetPluginIndex() { return m_PluginIndex; }	


#ifdef DOCJETDUMMY // Docjet secret macro
#else
//-------------------------------------------------------------------
//-------------------------------------------------------------------
// Internal functions 
//-------------------------------------------------------------------
//-------------------------------------------------------------------


CKBehaviorPrototype* GetProto() { return m_Proto; }			
void		SetProto(CKBehaviorPrototype* proto) { m_Proto=proto; }	


		CKObjectDeclaration(CKSTRING Name);	
		virtual ~CKObjectDeclaration();		
		void	SetPluginIndex(int Index) 
		{ m_PluginIndex=Index; }	
		int		GetManagerNeededCount() { return m_ManagersGuid.Size(); }	
		CKGUID	GetManagerNeeded(int Index) { return m_ManagersGuid[Index]; }	

		CKGUID						m_Guid;					
		CK_CLASSID					m_CompatibleClassID;	
		CKDLL_CREATEPROTOFUNCTION	m_CreationFunction;		
		CKDWORD						m_Version;				
		CKSTRING					m_Description;			
		CKBehaviorPrototype*		m_Proto;				
		int							m_Type;					
		CKGUID						m_AuthorGuid;			
		CKSTRING					m_AuthorName;			
		CKSTRING					m_Category;				
		XString						m_Name;					
		int							m_PluginIndex;			
		XArray<CKGUID>				m_ManagersGuid;			
#endif
};

#endif
