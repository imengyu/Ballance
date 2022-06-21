/*************************************************************************/
/*	File : CKPluginManager.h		 				 					 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2000, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKPLUGINMANAGER_H

#define CKPLUGINMANAGER_H "$Id:$"

#include "XClassArray.h"
#include "XString.h"
#include "CKDefines.h"
#include "CKDataReader.h"
#include "CKBitmapReader.h"
#include "CKSoundReader.h"
#include "CKModelReader.h"
#include "CKMovieReader.h"

#define CKPLUGIN_BITMAP			"Bitmap Readers"	 
#define CKPLUGIN_SOUND			"Sound Readers"		
#define CKPLUGIN_MODEL			"Model Readers"		
#define CKPLUGIN_MANAGER		"Managers"			
#define CKPLUGIN_BEHAVIOR		"BuildingBlocks"	
#define CKPLUGIN_RENDERENGINE	"Render Engines"	
#define CKPLUGIN_MOVIE			"Movie Readers"		
#define CKPLUGIN_EXTENSIONS		"Extensions"		

#ifdef CK_LIB
	/*******************************************
	+ There is only one function a rasterizer Dll is supposed
	to export :"CKRasterizerGetInfo", it will be used by the render engine 
	to retrieve information about the plugin :
	{secret}
	******************************************/
	struct	CKRasterizerInfo;
	
	typedef void (*CKRST_GETINFO)(CKRasterizerInfo*); 
#endif

/********************************************************
Summary: Short description of a DLL that declared plugins

Remarks:

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}

		struct CKPluginDll
		{
			XString			m_DllFileName;		
			INSTANCE_HANDLE	m_DllInstance;		
			int				m_PluginInfoCount;	
		};

{html:</td></tr></table>}

See Also: CKPluginManager::GetPluginDllInfo
*************************************************************/
struct CKPluginDll
{
	XString			m_DllFileName;		// DLL Path
	INSTANCE_HANDLE	m_DllInstance;		// Instance of the Loaded Dll (as HINSTANCE on windows)
	int				m_PluginInfoCount;	// Number of plugins declared by this DLL


	CKPluginDll()	{	
		m_DllFileName = "";	
		m_DllInstance=0; 
	}

//Summary: Returns a pointer to a function inside the plugin.
//Arguments:
//	FunctionName: Name of the function which should be returned			
//Return Value:
//	A Pointer to the function or NULL if the function was not found in the DLL.
	void* GetFunctionPtr(CKSTRING FunctionName) {
		VxSharedLibrary shl;
		shl.Attach(m_DllInstance);
		return shl.GetFunctionPtr(FunctionName);
	}
};


// Summary: Reader plugin creation function prototype
// 
// See Also:Creating New Plugins
typedef CKDataReader *(*CKReaderGetReaderFunction)(int);


// Summary: Data Reader (Movie,Bitmap,Sound,Models...) specific options
// 
struct CKPluginEntryReadersData
{
	CKGUID						m_SettingsParameterGuid;	// Parameter type for options
	int							m_OptionCount;				// Number of options for the reader
	CK_DATAREADER_FLAGS			m_ReaderFlags;				// Reader Save/Load options
	CKReaderGetReaderFunction	m_GetReaderFct;				// A pointer to the function that will create a reader.

	
	CKPluginEntryReadersData() {
		m_GetReaderFct = NULL;
		m_OptionCount = 0;
		m_ReaderFlags = (CK_DATAREADER_FLAGS)0;
		m_SettingsParameterGuid = CKGUID(0,0);
	}
};

// Summary: List of behavior GUID declared by a plugin
// 
//See Also:CKPluginEntry
struct CKPluginEntryBehaviorsData
{
	XArray<CKGUID>			m_BehaviorsGUID;
};

/******************************************************
Summary: Plugin Description structure

Remarks:
	+ This structure described a registred Virtools plugin :

{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}
  
				struct  CKPluginEntry
				{
					int				m_PluginDllIndex;	
					int				m_PositionInDll;	
					CKPluginInfo	m_PluginInfo;		
					(According to the type of plugin )
					CKPluginEntryReadersData*	m_ReadersInfo;		
					CKPluginEntryBehaviorsData* m_BehaviorsInfo;	
					CKBOOL			m_Active;			
					int				m_IndexInCategory;	
					CKBOOL			m_NeededByFile;		
				}

{html:</td></tr></table>}

See Also:CKPluginManager::GetPluginInfo
********************************************************/
struct  CKPluginEntry
{
	int				m_PluginDllIndex;	// Index of the owner Dll in the list of Dlls
	int				m_PositionInDll;	// Position of the PluginInfo inside the DLL (when thery are several plugins inside a same DLL)
	CKPluginInfo	m_PluginInfo;		// Base Info on the plugin (Type, Name,Description)

//--- According to the type of plugin 

	CKPluginEntryReadersData*	m_ReadersInfo;		// Reader plugins specific info (optionnal settings + load/save capabilities) 
	CKPluginEntryBehaviorsData* m_BehaviorsInfo;	// Behavior plugins specific info (list of declared behavior GUIDS)

	CKBOOL			m_Active;			// For manager and Render engines TRUE if a manager was created, for other plugins this value is not used.
	int				m_IndexInCategory;	// Index of this entry in its category
	CKBOOL			m_NeededByFile;		// When saving a file TRUE if at least one object needs this plugin

	
	CKPluginEntry& operator = (const CKPluginEntry& ent);
	
	CKPluginEntry();
	
	CKPluginEntry(const CKPluginEntry& ent);
	
	~CKPluginEntry();
};



struct CKPluginCategory
{
	XString					m_Name;
	XArray<CKPluginEntry*>	m_Entries;

	CKPluginCategory(){}
	CKPluginCategory(const CKPluginCategory& s){
		m_Name		= s.m_Name;
		m_Entries	= s.m_Entries;
	}
	
	// Operator
	CKPluginCategory& operator=(const CKPluginCategory& s){ 
		if (&s != this){			
			m_Name		= s.m_Name;
			m_Entries	= s.m_Entries;
		}
		return *this;
	}
};


// Summary: Plugin declaration count function prototype
// 
// See Also:Creating New Plugins
typedef int (*CKPluginGetInfoCountFunction)();

// Summary: Plugin declaration function prototype
// 
// See Also:Creating New Plugins
typedef CKPluginInfo* (*CKPluginGetInfoFunction)(int Index);



/*****************************************************************************
Summary: Plugins Manager

Remarks:

+ CKPluginManager does not derive from CKBaseManager and there is
only one instance of the plugin per process (even if there are several CKContext)

+ PluginManager init functions must be call before creating any CKContext to 
find the available plugins (ParsePlugins or RegisterPlugin). 

+ The plugin manager can be retrieve with the CKGetPluginManager function.

+ Plugins are sorted by their category, many methods asks to be given a  
category which must be a valid CK_PLUGIN_TYPE identifier or -1 to use all categories.

+ Since a Dll can contain several plugins the plugin manager give acces to either 
the list of plugin per category (GetPluginCount,GetPluginInfo) or to the list of Dll
that were loaded (GetPluginDllCount,GetPluginDllInfo). A plugin is identified by its
CKPluginEntry which contains its type and other informations along with the index of 
the DLL that declared it.

See Also: CKPluginEntry,CKPluginDll,Creating New Plugins
**********************************************************************************/
class CKPluginManager {
public:
	CKPluginManager();			
	virtual ~CKPluginManager();	

//----- Parse a directory for plugins and returns the number of valid plugins enumerated
	int ParsePlugins(CKSTRING Directory);
//------ Registers a specific plugin Dll
	CKERROR RegisterPlugin(CKSTRING str);

	CKPluginEntry*	FindComponent(CKGUID Component,int catIdx=-1);	// Search for behaviors,managers,readers,etc.. to see if they exists


//---- Category Functions
	int			AddCategory(CKSTRING cat);				// Adds a category, category name must be unique
	CKERROR		RemoveCategory(int catIdx);				// Removes a category, category name must be unique
	int			GetCategoryCount();						// Gets the number of categories
	CKSTRING		GetCategoryName(int catIdx);			// Gets the category name at specified index
	int			GetCategoryIndex(CKSTRING cat);			// Gets the category Index in List
	CKERROR		RenameCategory(int catIdx, CKSTRING newName);	// Renames a category 

//---- PluginDll Functions 
	int				GetPluginDllCount();					// Gets the Plugin count
	CKPluginDll*		GetPluginDllInfo(int PluginDllIdx);		// Gets the Plugin at index
	CKPluginDll*		GetPluginDllInfo(CKSTRING PluginName,int *idx = NULL);	// Search for a Plugin by name
	CKERROR			UnLoadPluginDll(int PluginDllIdx);
	CKERROR			ReLoadPluginDll(int PluginDllIdx);

//---- Plugin Functions 
	int				GetPluginCount(int catIdx);						// Gets the Plugin count for a category	
	CKPluginEntry*	GetPluginInfo(int catIdx, int PluginIdx);		// Gets the Plugin at index PluginIdx for a category
	
//---- Bitmap,Sound,Model or Movie Reader Access
	BOOL				SetReaderOptionData(CKContext* context,void* memdata,CKParameterOut* Param,CKFileExtension ext,CKGUID* guid=NULL);
	CKParameterOut*	GetReaderOptionData(CKContext* context,void* memdata,CKFileExtension ext,CKGUID* guid=NULL);	

	CKBitmapReader*	GetBitmapReader(CKFileExtension& ext,CKGUID *preferedGUID = NULL);	
	CKSoundReader*	GetSoundReader(CKFileExtension& ext,CKGUID *preferedGUID = NULL);	
	CKModelReader*	GetModelReader(CKFileExtension& ext,CKGUID *preferedGUID = NULL);	
	CKMovieReader*	GetMovieReader(CKFileExtension& ext,CKGUID *preferedGUID = NULL);	

//---- Model and Virtools Loading access {secret}


	CKERROR Load(CKContext* context,CKSTRING FileName,CKObjectArray *liste,CK_LOAD_FLAGS LoadFlags,CKCharacter *carac=NULL,CKGUID* Readerguid=NULL);	//  {secret}
	CKERROR Save(CKContext* context,CKSTRING FileName,CKObjectArray *liste,CKDWORD SaveFlags,CKGUID* Readerguid=NULL);	//  {secret}

//---- Init {secret}
	void ReleaseAllPlugins();									
	void InitializePlugins(CKContext* context);					
	void DeInitializePlugins(CKContext* context);				
	void ComputeDependenciesList(CKFile* file);					
	void MarkComponentAsNeeded(CKGUID Component,int catIdx);	

#ifdef CK_LIB

//-------------------------------------------------------------------
#ifdef DOCJETDUMMY // Docjet secret macro
#else

//---------------- for additionnal plugins to link statically 
	void RegisterPluginInfo(int PositionInDll,CKPluginInfo* info,CKDLL_OBJECTDECLARATIONFUNCTION InfoFct,CKReaderGetReaderFunction GetReaderFunc);
	void RegisterNewStaticLibAsDll(char* Name,int PluginInfoCount);	
	void AddRenderEngineRasterizer(CKRST_GETINFO RasterizerInfoFunction) { m_StaticRasterizers.PushBack(RasterizerInfoFunction); } 
	const XArray<CKRST_GETINFO>& GetRegistredRasterizers() { return m_StaticRasterizers; }  
#endif // Docjet secret macro

#endif

protected:
//--- Utils
	void InitInstancePluginEntry(CKPluginEntry* entry,CKContext* context);
	void ExitInstancePluginEntry(CKPluginEntry* entry,CKContext* context);
	void InitializeBehaviors(VxSharedLibrary& lib,CKPluginEntry& entry);		
	void InitializeBehaviors(CKDLL_OBJECTDECLARATIONFUNCTION Fct,CKPluginEntry& entry);		
	void RemoveBehaviors(int PluginIndex);

#ifdef CK_LIB
	
	XArray<CKRST_GETINFO> m_StaticRasterizers;
#endif

	int				AddPlugin(int catIdx, CKPluginEntry& Plugin);	// Adds a Plugin to a category
	CKERROR			RemovePlugin(int catIdx, int PluginIdx);		// Removes a Plugin in a category

	CKPluginEntry* EXTFindEntry(CKFileExtension& ext,int Category=-1);
	CKDataReader *EXTFindReader(CKFileExtension& ext,int Category=-1);
	CKDataReader *GUIDFindReader(CKGUID& guid,int Category=-1);


	XClassArray<CKPluginCategory>	m_PluginCategories;
	XClassArray<CKPluginDll>		m_PluginDlls;
	XClassArray<VxSharedLibrary*>	m_RunTimeDlls;

	void Clean();
	void Init();
	
};

#endif
  