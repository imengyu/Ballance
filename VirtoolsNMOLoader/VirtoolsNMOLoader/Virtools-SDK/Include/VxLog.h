#if !defined(VXLOG_H)
#define VXLOG_H

#ifdef DOCJET_DUMMY // Prevent doc processing
#else

#if defined(_DEBUG)
	#if !defined(VX_LOG_ALL_LEVEL) && !defined(VX_NO_LOG)
		#define VX_LOG_ALL_LEVEL
	#endif
#elif !defined(VX_NO_LOG)
	#if !defined(VX_LOG_ERROR)
		#define VX_LOG_ERROR
	#endif
	#if !defined(VX_LOG_WARNING)
		#define VX_LOG_WARNING
	#endif
#endif

#if defined(VX_LOG_ALL_LEVEL) && !defined(VX_NO_LOG)
	#if !defined(VX_LOG_DEBUG)
		#define VX_LOG_DEBUG
	#endif
	#if !defined(VX_LOG_TRACE)
		#define VX_LOG_TRACE
	#endif
	#if !defined(VX_LOG_WARNING)
		#define VX_LOG_WARNING
	#endif
	#if !defined(VX_LOG_ERROR)
		#define VX_LOG_ERROR
	#endif
	#if !defined(VX_LOG_INFO)
		#define VX_LOG_INFO
	#endif
#endif // VX_LOG_ALL_LEVEL

#endif // Prevent doc processing


class VxLogListener;


#if !defined(VX_NO_LOG)
	
	extern int g_LogModule;
#endif


/***********************************************************************
Summary: Utility class for logging information.

See also: VxLogListener
***********************************************************************/
class VxLog
{

public:

	/*****************************************************************
	Summary: Listener factory.
	******************************************************************/
	typedef VxLogListener* (*ListenerFactory)(const char* iName);

	/*****************************************************************
	Summary: Level of a log.
	Remarks: Used when calling VxLog::Print to defines the level of a log.
	Level is also used when a Listener is build. A Listener should
	defines which levels it wants to log.
	See Also: VxLog::Print, VxLogListener::SetLevel, VxLogListener::GetLevel
	******************************************************************/
	enum Level
	{
		VLDEBUG		= 0x00000001,
		VLTRACE		= 0x00000002,
		VLERROR		= 0x00000004,
		VLWARNING	= 0x00000008,
		VLINFO		= 0x00000010,
		VLALL		= 0x0000001F,
	};

		/*************************************************
		Summary: Get the string associated to a level.

		Input Arguments:
			iLevel: level to be converted.

		Remarks: The strings are : "DEBUG","TRACE","ERROR","WARNING","INFO".

		See also: VxLog::Level.
		*************************************************/
VX_EXPORT	static const char*	GetLevelDesc(Level iLevel);

		/*************************************************
		Summary: Return the unique instance of the log.
		*************************************************/
VX_EXPORT	static VxLog&		GetLog();

		/*************************************************
		Summary: Destructor.

		Remarks: Listener are destroyed.
		*************************************************/
		~VxLog();

		/*************************************************
		Summary: Set information about a log which will be print
		with Print.

		Input Arguments:
			iModule: module used to log.
			iLevel: level of the log.
			iLine: line in the file where the log is placed.
			iFile: file where the log is placed.

		Remarks: This function must be called before Print.

		See also: Print.
		*************************************************/
VX_EXPORT	VxLog&	SetInformation(int iModule,Level iLevel,int iLine,const char* iFile);

		/*************************************************
		Summary: Set information about a log which will be print
		with PrintMT.

		Input Arguments:
			iModule: module used to log.
			iLevel: level of the log.
			iLine: line in the file where the log is placed.
			iFile: file where the log is placed.

		Remarks: This function must be called before PrintMT.

		See also: PrintMT.
		*************************************************/
VX_EXPORT	VxLog&	SetInformationMT(int iModule,Level iLevel,int iLine,const char* iFile);

		/*************************************************
		Summary: Print informations (without cariage return).

		Input Arguments:
			iFormat: same arguments as a printf.

		Remarks: All registered logger will be called if their modules and levels are compatible
		with module and level.

		See also: SetInformation, RegisterListener,VxLogListener::IsModule,VxLogListener::GetLevel.
		*************************************************/
VX_EXPORT	void	Print(const char* iFormat,...);

		/*************************************************
		Summary: Print informations (without cariage return).
		This method is thread safe.

		Input Arguments:
			iFormat: same arguments as a printf.

		Remarks: All registered logger will be called if their modules and levels are compatible
		with module and level. SetInformationMT should be called just before calling
		PrintMT.

		See also: SetInformationMT, RegisterListener,VxLogListener::IsModule,VxLogListener::GetLevel.
		*************************************************/
VX_EXPORT	void	PrintMT(const char* iFormat,...);

		/*************************************************
		Summary: Register a new module.

		Input Arguments:
			iName: name of the module you are looking for.

		Return value: the identifier of the new module or -1
		if the name of module is invalide.
		*************************************************/
VX_EXPORT	int			RegisterModule(const char* iName);

		/*************************************************
		Summary: Test if a module is already registered.

		Input Arguments:
			iName: name of the module you are looking for.

		Return value: identifier of the module or -1 if the
		module is not registered.

		See also: RegisterModule
		*************************************************/
VX_EXPORT	int			IsModule(const char* iName) const;

		/*************************************************
		Summary: Get the name of a module.

		Input Arguments:
			iModule: identifier of the module returned by RegisterModule.

		Return value: the name of the module or NULL.

		See also: RegisterModule
		*************************************************/
VX_EXPORT	const char*	GetModule(int iModule) const;

		/*************************************************
		Summary: Register a new listener factory.

		Input Arguments:
			iGuid: guid of the listener.
			iFactory: factory to create the listener.

		Return value: Return FALSE if a factory with the same guid
		has already been registered.

		Remarks: Listener factories are used to create a listener
		(if necessary) by RegisterListener.

		See also: RegisterListener.
		*************************************************/
VX_EXPORT	BOOL					RegisterListenerFactory(const XGUID& iGuid,ListenerFactory iFactory);

		/*************************************************
		Summary: Register a listener.

		Input Arguments:
			iGuid: guid of the listener.
			iName: name of the listener. This name must be unique
			even for listener with a different guid.
			iModule: module to add to the listener.

		Remarks: The name of the listener must be unique even for different
		guid.

		If a listener is already registered with the same name the function check if 
		guids are equal (see VxLogListener::GetGuid). If guids are different
		the function return NULL. Else iModule is added to the listener (see VxLogListener::AddModule)
		and the pointer to the listener is returned.

		If no listener is registered with this name, a new listener is created
		and iModule is added to this listener. Then the pointer of the new listener
		is returned.

		After calling RegisterListener it is adviced to call VxLogListener::SetLevel
		on the listener.

		Return value: A pointer to the listener or NULL if the function failed.

		See also: RegisterListenerFactory, VxLogListener::AddModule,
		VxLogListener::GetGuid, VxLogListener::AddModule,
		VxLogListener::SetLevel
		*************************************************/
VX_EXPORT	VxLogListener*			RegisterListener(const XGUID& iGuid,const char* iName,int iModule);

		/*************************************************
		Summary: Returns a listener with its name.

		Return value: A pointer to the listener or NULL if no
		listener is registered with this name.
		*************************************************/
VX_EXPORT	VxLogListener*			GetListener(const char* iName);

		/*************************************************
		Summary: Returns a listener with its name.

		Return value: A constant pointer to the listener or NULL if no
		listener is registered with this name.
		*************************************************/
VX_EXPORT	const VxLogListener*	GetListener(const char* iName) const;
VX_EXPORT	BOOL					UnregisterListener(const char* iName);

private:

	
	/*************************************************
	Summary: Constructor. Build a object to log informations.
	Remarks: Default listener factory are registered.
	*************************************************/
	VxLog();

	
	VxLog(const VxLog&);
	
	VxLog& operator=(const VxLog&);

	
	void PPrint(int module,Level iLevel,int iLine,const char* iFile,const char* iFormat,va_list iArgs);

	
	int									m_Size;
	
	char*								m_Buffer;
	
	int									m_Module;
	
	int									m_Line;
	
	Level								m_Level;
	
	XString								m_File;
	
	XHashTable<VxLogListener*,XString>	m_Listeners;
	
	XHashTable<ListenerFactory,XGUID>	m_ListenerFactories;
	
	XClassArray<XString>				m_Modules;
	
	VxMutex								m_Mutex;

};

#ifdef DOCJET_DUMMY // Prevent doc processing
#else

#if !defined(VX_NO_LOG)
	#define	VXIMPLOGMODULEEX()					int	g_LogModule;
	#define	VXIMPLOGMODULEEX2(var)				int	var;
	#define	VXREGLOGMODULEEX(name)				g_LogModule = VxLog::GetLog().RegisterModule(name);
	#define	VXREGLOGMODULEEX2(var,name)			var = VxLog::GetLog().RegisterModule(name);
	
	#define	VXREGLOGMODULE(name)				int g_LogModule = VxLog::GetLog().RegisterModule(name);
	#define	VXREGLOGMODULE2(var,name)			int var = VxLog::GetLog().RegisterModule(name);
	
	#define VXREGLOGFACTORY(guid,factory)		VxLog::GetLog().RegisterListenerFactory(guid,factory);
	#define VXREGLOGLISTENER(guid,name)			VxLog::GetLog().RegisterListener(guid,name,g_LogModule);
	#define VXREGLOGLISTENER2(var,guid,name)	VxLog::GetLog().RegisterListener(guid,name,var);
	#define VXUNREGLOGLISTENER(name)			VxLog::GetLog().UnregisterListener(name);
#else
	#define	VXIMPLOGMODULEEX()
	#define	VXIMPLOGMODULEEX2(var)
	#define	VXREGLOGMODULEEX(name)
	#define	VXREGLOGMODULEEX2(var,name)
	#define	VXREGLOGMODULE(name)
	#define	VXREGLOGMODULE2(var,name)
	#define VXREGLOGFACTORY(guid,factory)
	#define VXREGLOGLISTENER(guid,name)
	#define VXREGLOGLISTENER2(var,guid,name)
	#define VXUNREGLOGLISTENER(name)
#endif

#if defined(VX_LOG_DEBUG)
	#define VXLOGD			VxLog::GetLog().SetInformation(g_LogModule,VxLog::VLDEBUG,__LINE__,__FILE__).Print
	#define VXLOGDMT		VxLog::GetLog().SetInformationMT(g_LogModule,VxLog::VLDEBUG,__LINE__,__FILE__).PrintMT
	#define VXLOGDEX(var)	VxLog::GetLog().SetInformation(var,VxLog::VLDEBUG,__LINE__,__FILE__).Print
	#define VXLOGDMTEX(var)	VxLog::GetLog().SetInformationMT(var,VxLog::VLDEBUG,__LINE__,__FILE__).PrintMT
#else
	#define VXLOGD			if(0)
	#define VXLOGDMT		if(0)
	#define VXLOGDEX(var)	if(0)
	#define VXLOGDMTEX(var)	if(0)
#endif // VX_LOG_DEBUG

#if defined(VX_LOG_TRACE)
	#define VXLOGT			VxLog::GetLog().SetInformation(g_LogModule,VxLog::VLTRACE,__LINE__,__FILE__).Print
	#define VXLOGTMT		VxLog::GetLog().SetInformationMT(g_LogModule,VxLog::VLTRACE,__LINE__,__FILE__).PrintMT
	#define VXLOGTEX(var)	VxLog::GetLog().SetInformation(var,VxLog::VLTRACE,__LINE__,__FILE__).Print
	#define VXLOGTMTEX(var)	VxLog::GetLog().SetInformationMT(var,VxLog::VLTRACE,__LINE__,__FILE__).PrintMT
#else
	#define VXLOGT			if(0)
	#define VXLOGTMT		if(0)
	#define VXLOGTEX(var)	if(0)
	#define VXLOGTMTEX(var)	if(0)
#endif // VX_LOG_TRACE

#if defined(VX_LOG_ERROR)
	#define VXLOGE			VxLog::GetLog().SetInformation(g_LogModule,VxLog::VLERROR,__LINE__,__FILE__).Print
	#define VXLOGEMT		VxLog::GetLog().SetInformationMT(g_LogModule,VxLog::VLERROR,__LINE__,__FILE__).PrintMT
	#define VXLOGEEX(var)	VxLog::GetLog().SetInformation(var,VxLog::VLERROR,__LINE__,__FILE__).Print
	#define VXLOGEMTEX(var)	VxLog::GetLog().SetInformationMT(var,VxLog::VLERROR,__LINE__,__FILE__).PrintMT
#else
	#define VXLOGE			if(0)
	#define VXLOGEMT		if(0)
	#define VXLOGEEX(var)	if(0)
	#define VXLOGEMTEX(var)	if(0)
#endif // VX_LOG_ERROR

#if defined(VX_LOG_WARNING)
	#define VXLOGW			VxLog::GetLog().SetInformation(g_LogModule,VxLog::VLWARNING,__LINE__,__FILE__).Print
	#define VXLOGWMT		VxLog::GetLog().SetInformationMT(g_LogModule,VxLog::VLWARNING,__LINE__,__FILE__).PrintMT
	#define VXLOGWEX(var)	VxLog::GetLog().SetInformation(var,VxLog::VLWARNING,__LINE__,__FILE__).Print
	#define VXLOGWMTEX(var)	VxLog::GetLog().SetInformationMT(var,VxLog::VLWARNING,__LINE__,__FILE__).PrintMT
#else
	#define VXLOGW			if(0)
	#define VXLOGWMT		if(0)
	#define VXLOGWEX(var)	if(0)
	#define VXLOGWMTEX(var)	if(0)
#endif // VX_LOG_WARNING

#if defined(VX_LOG_INFO)
	#define VXLOGI			VxLog::GetLog().SetInformation(g_LogModule,VxLog::VLINFO,__LINE__,__FILE__).Print
	#define VXLOGIMT		VxLog::GetLog().SetInformationMT(g_LogModule,VxLog::VLINFO,__LINE__,__FILE__).PrintMT
	#define VXLOGIEX(var)	VxLog::GetLog().SetInformation(var,VxLog::VLINFO,__LINE__,__FILE__).Print
	#define VXLOGIMTEX(var)	VxLog::GetLog().SetInformationMT(var,VxLog::VLINFO,__LINE__,__FILE__).PrintMT
#else
	#define VXLOGI			if(0)
	#define VXLOGIMT		if(0)
	#define VXLOGIEX(var)	if(0)
	#define VXLOGIMTEX(var)	if(0)
#endif // VX_LOG_INFO

#endif // DOCJET_DUMMY 

#endif // VXLOG_H
