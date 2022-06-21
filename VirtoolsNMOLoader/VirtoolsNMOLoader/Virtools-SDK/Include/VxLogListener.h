#if !defined(VxLogListener_H)
#define VxLogListener_H


#include "VxLog.h"


/*************************************************
Summary: Define an object which listen to a log.



Remarks: A listener has a level which determines if it must print information.
The default value of the level is VxLog::ALL.

See also: VxLog.
*************************************************/
class VxLogListener
{

public:

	/*****************************************************************
	Summary: Describes how the date will be generated as a string.
	******************************************************************/
	enum TimeFormat
	{
		NODATE				= 0,
		TIME				= 1,
		DATE				= 2, // the system date like dd/mm/yy hh:mm:ss.
		DATEMILLI			= 3, // the system date like dd/mm/yy hh:mm:ss:ms.
		DATELONGYEAR		= 4, // the system date like dd/mm/yyyy hh:mm:ss.
		DATELONGYEARMILLI	= 5, // the system date like dd/mm/yyyy hh:mm:ss:ms.
		DATEMILLI2			= 6, // the system date like dd/mm/yy hh:mm:ss:ms (time).
	};

	/*****************************************************************
	Summary: Describes the behavior of the listener.
	******************************************************************/
	enum Flags
	{
		DATEINNAME	= 1, // the name of the listener will contains the date as ([yy mm dd]).
		FILEINFO	= 2, // the listener will add information about the file where the log was generated.
		FILEAPPEND	= 4, // (only for file) the file will be open in appen mode.
	};

		/*************************************************
		Summary: Destructor.
		*************************************************/
VX_EXPORT	virtual ~VxLogListener();

		/*************************************************
		Summary: Get the name of the listener.
		*************************************************/
VX_EXPORT	const XString&	GetName() const;

		/*************************************************
		Summary: Test if a module is already registered in the listener

		Input Arguments:
			iModule: identifier of the module returnedby VxLog::RegisterModule.

		Return value: Returns TRUE if the module is registered in the listener.

		See also: VxLog::RegisterModule, AddModule.
		*************************************************/
VX_EXPORT	XBOOL	IsModule(int iModule) const;

		/*************************************************
		Summary: Add a module in the listener.

		Input Arguments:
			iModule: identifier of the module returnedby VxLog::RegisterModule.

		See also: VxLog::RegisterModule, IsModule.
		*************************************************/
VX_EXPORT	void	AddModule(int iModule);

		/*************************************************
		Summary: Set the level of a listener.
		See also: VxLog::Level.
		*************************************************/
VX_EXPORT	void		SetLevel(int iLevel);

		/*************************************************
		Summary: Get the level of a listener.
		See also: VxLog::Level.
		*************************************************/
VX_EXPORT	int			GetLevel() const;

		/*************************************************
		Summary: Get timeformat of the listener.
		See also: SetTimeFormat.
		*************************************************/
VX_EXPORT	TimeFormat	GetTimeFormatValue() const;

		/*************************************************
		Summary: Set timeformat of the listener.
		See also: GetTimeFormatValue.
		*************************************************/
VX_EXPORT	void		SetTimeFormat(TimeFormat iTimeFormat);

		/*************************************************
		Summary: Get flags of the listener.
		See also: SetFlags.
		*************************************************/
VX_EXPORT	int			GetFlags() const;

		/*************************************************
		Summary: Set flags of the listener.
		See also: GetFlags.
		*************************************************/
VX_EXPORT	void		SetFlags(int iFlags);

		/*************************************************
		Summary: Convert the current date to a string.
		*************************************************/
VX_EXPORT	void		GetDateAsString(TimeFormat iFormat,XString& iStr);

	/*************************************************
	Summary: Returns the Guid of the Listener.
	*************************************************/
	virtual const XGUID& GetGuid() const = 0;

	/*************************************************
	Summary: Print information with the listener.
	Remarks: Function you must overload.

	Input parameters:
		iModule: the module which want to log the information.
		iLevel: the level of the informations to be displayed.
		iLine: the line the log occured.
		iFile: the file the log occured
		iLog: a string representing the informations to be displayed.

	See also: VxLog.
	*************************************************/
	virtual void Print(int iModule,VxLog::Level iLevel,int iLine,const char* iFile,const char* iLog) = 0;


protected:


		/*************************************************
		Summary: Constructor. Initialize a listener
		*************************************************/
VX_EXPORT	VxLogListener(const char* iName);


	
	int				m_Level;
	
	TimeFormat		m_TimeFormat;
	
	int				m_Flags;
	
	XString			m_Name;
	
	XArray<int>		m_Modules;
	
	VxTimeProfiler	m_Profiler;


private:

	
	VxLogListener(const VxLogListener&);
	
	VxLogListener& operator=(const VxLogListener&);

};

#ifndef _XBOX
/*************************************************
Summary: A Listener which display informations into a file.

Remarks: The name of the is:
	- directory + name + "." + ext or
	- directory + name + [yy mm dd] + "." + ext

Depending of the flags of the listener.

The log while be displayed like this: [MODULE] [LEVEL] [TIME] [FILE:LINE] : LOG.
	- [TIME] is displayed if timeformat!=NODATE.
	- [FILE:LINE] is displayed if flags&FILEINFO.

See also: VxLogListener. SetFlags, SetExt, VxLog::RegisterListener
*************************************************/
class VxFileListener : public VxLogListener
{
public:

	/*************************************************
	Summary: The Guid of the file listener.
	See also: VxLog::RegisterListenerFactory, VxLog::RegisterListener.
	*************************************************/
VX_EXPORT	static const XGUID	m_Guid;

VX_EXPORT	static VxLogListener* ListenerFactory(const char* iName);

		/*************************************************
		Summary: Destructor. The file is closed.
		*************************************************/
VX_EXPORT	~VxFileListener();

		const XGUID& GetGuid() const
		{
			return m_Guid;
		}

		/*************************************************
		Summary: Returns the directory of the file. Default
		is the directory of the executable.
		See also: SetDirectory.
		*************************************************/
VX_EXPORT	const char*	GetDirectory() const;

		/*************************************************
		Summary: Set the directory of the file.
		See also: GetDirectory.
		*************************************************/
VX_EXPORT	void		SetDirectory(const char* iDir);

		/*************************************************
		Summary: Returns the extension of the file. Default
		is "log".
		See also: SetExt.
		*************************************************/
VX_EXPORT	const char*	GetExt() const;

		/*************************************************
		Summary: Set the extension of the file.
		See also: GetExt.
		*************************************************/
VX_EXPORT	void		SetExt(const char* iExt);

	/*************************************************
	Summary: Display a log in a file.

	Remarks: The log while be displayed like this: [MODULE] [LEVEL] [TIME] [FILE:LINE] : LOG.
	[TIME] is displayed if timeformat!=NODATE.
	[FILE:LINE] is displayed if flags&FILEINFO. There is a cariage return after the log.
	*************************************************/
	void Print(int iModule,VxLog::Level iLevel,int iLine,const char* iFile,const char* iLog);

private:
	
	
	VxFileListener(const VxFileListener&);
	
	VxFileListener& operator=(const VxFileListener&);

	
	/*************************************************
	Summary: Constructor. Initialize a file Listener
	*************************************************/
	VxFileListener(const char* iName);

	
	/*************************************************
	Summary: Checks if the file is opened or should be reopen
	with another name.
	*************************************************/
	void	CheckFile();

	
	int				m_FileDay;
	
	XString			m_Ext;
	
	XString			m_Directory;
	
	VxFile			m_File;

};

#ifdef DOCJETDUMMY  // Docjet duumy macro
#else
	#if !defined(VX_NO_LOG)
		#define VXREGLOGFILE(name)											VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,g_LogModule)
		#define VXREGLOGFILEEX(name,level,flags,timeformat)					{VxFileListener* li = (VxFileListener*)VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,g_LogModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
		#define VXREGLOGFILEEX2(name,level,flags,timeformat,dir,ext)		{VxFileListener* li = (VxFileListener*)VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,g_LogModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);li->SetDirectory(dir);li->SetExt(ext);}}

		#define VXREGLOGFILEV(var,name)										VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,var)
		#define VXREGLOGFILEEXV(var,name,level,flags,timeformat)			{VxFileListener* li = (VxFileListener*)VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,var);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
		#define VXREGLOGFILEEXV2(var,name,level,flags,timeformat,dir,ext)	{VxFileListener* li = (VxFileListener*)VxLog::GetLog().RegisterListener(VxFileListener::m_Guid,name,vargModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);li->SetDirectory(dir);li->SetExt(ext);}}
	#else
		#define VXREGLOGFILE(name)
		#define VXREGLOGFILEEX(name,level,flags,timeformat)
		#define VXREGLOGFILEEX2(name,level,flags,timeformat,dir,ext)
		#define VXREGLOGFILEV(var,name)
		#define VXREGLOGFILEEXV(var,name,level,flags,timeformat)
		#define VXREGLOGFILEEXV2(var,name,level,flags,timeformat,dir,ext)
	#endif

#endif // Docjet dummy macro
#endif // !_XBOX

#if defined(_WINDOWS) || defined(WIN32) || defined(_WIN32)
/*************************************************
Summary: A Listener which display informations into a console.

Remarks: The log while be displayed like this: [MODULE] [LEVEL] [TIME] [FILE:LINE] : LOG.
	- [TIME] is displayed if timeformat!=NODATE.
	- [FILE:LINE] is displayed if flags&FILEINFO

Only one console can be created per executable. So you must not create
more than one VxConsoleListener. It means you must not register
many console listener with different name. But you can register
many console listener with the name but with different module. See VxLog::RegisterListener.

See also: VxLogListener.
*************************************************/
class VxConsoleListener : public VxLogListener
{
public:

	/*************************************************
	Summary: The Guid of the console listener.
	See also: VxLog::RegisterListenerFactory, VxLog::RegisterListener.
	*************************************************/
VX_EXPORT	static const XGUID	m_Guid;

VX_EXPORT	static VxLogListener* ListenerFactory(const char* iName);

		/*************************************************
		Summary: Destructor. The console is released.
		*************************************************/
VX_EXPORT	~VxConsoleListener();

		const XGUID& GetGuid() const
		{
			return m_Guid;
		}

		/*************************************************
		Summary: Sets the number of rows (iWidth) and lines (iHeight)
		the console can display.
		*************************************************/
VX_EXPORT	XBOOL	SetSize(int iWidth,int iHeight);


	/*************************************************
	Summary: Display a log in the debuger.
	
	Remarks: The log while be displayed like this: [MODULE] [LEVEL] [TIME] [FILE:LINE] : LOG.
	[TIME] is displayed if flags&TIMEINFO
	[FILE:LINE] is displayed if flags&FILEINFO. There is a cariage return after the log.
	*************************************************/
	void Print(int iModule,VxLog::Level iLevel,int iLine,const char* iFile,const char* iLog);


private:

	
	VxConsoleListener(const VxConsoleListener&);
	
	VxConsoleListener& operator=(const VxConsoleListener&);

	
	/*************************************************
	Summary: Constructor. Initialize a console Listener

	See also: VxLogListener.
	*************************************************/
	VxConsoleListener(const char* title);

	
	/*************************************************
	Summary: Create the console if necessary.
	*************************************************/
	void	CreateConsole();

	
	GENERIC_HANDLE	m_Handle;

};

#ifdef DOCJETDUMMY  // Docjet duumy macro
#else

#if !defined(VX_NO_LOG)
	#define VXREGLOGCONSOLE()												VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",g_LogModule)
	#define VXREGLOGCONSOLEEX(level,flags,timeformat)						{VxConsoleListener* li = (VxConsoleListener*)VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",g_LogModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
	#define VXREGLOGCONSOLEEX2(level,flags,timeformat,width,height)			{VxConsoleListener* li = (VxConsoleListener*)VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",g_LogModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);li->SetSize(width,height);}}

	#define VXREGLOGCONSOLEV(var)											VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",var)
	#define VXREGLOGCONSOLEEXV(var,level,flags,timeformat)					{VxConsoleListener* li = (VxConsoleListener*)VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",var);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
	#define VXREGLOGCONSOLEEXV2(var,level,flags,timeformat,width,height)	{VxConsoleListener* li = (VxConsoleListener*)VxLog::GetLog().RegisterListener(VxConsoleListener::m_Guid,"CONSOLE LISTENER",var);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);li->SetSize(width,height);}}
#else
	#define VXREGLOGCONSOLE()
	#define VXREGLOGCONSOLEEX(level,flags,timeformat)
	#define VXREGLOGCONSOLEEX2(level,flags,timeformat,width,height)
	#define VXREGLOGCONSOLEV(var)
	#define VXREGLOGCONSOLEEXV(var,level,flags,timeformat)
	#define VXREGLOGCONSOLEEXV2(var,level,flags,timeformat,width,height)
#endif // VX_NO_LOG
#endif // Docjet duumy macro

#else // _WINDOWS
	#ifdef DOCJETDUMMY  // Docjet duumy macro
	#else
	#define VXREGLOGCONSOLE()
	#define VXREGLOGCONSOLEEX(level,flags,timeformat)
	#define VXREGLOGCONSOLEEX2(level,flags,timeformat,width,height)
	#define VXREGLOGCONSOLEV(var)
	#define VXREGLOGCONSOLEEXV(var,level,flags,timeformat)
	#define VXREGLOGCONSOLEEXV2(var,level,flags,timeformat,width,height)
	#endif // Docjet duumy macro
#endif // _WINDOWS


/*************************************************
Summary: A Listener which display informations into the debug output of your debbuger.
See also: VxLogListener.
*************************************************/
class VxOutputDebugListener : public VxLogListener
{
public:

	/*************************************************
	Summary: The Guid of the outputdebug listener.
	See also: VxLog::RegisterListenerFactory, VxLog::RegisterListener.
	*************************************************/
VX_EXPORT	static const XGUID	m_Guid;

VX_EXPORT	static VxLogListener* ListenerFactory(const char* iName);

	const XGUID& GetGuid() const
	{
		return m_Guid;
	}

	/*************************************************
	Summary: Display a log in the debuger.
	Remarks: The log while be displayed like this: [MODULE] [LEVEL] [TIME] [FILE:LINE] : LOG.
	[TIME] is displayed if flags&TIMEINFO
	[FILE:LINE] is displayed if flags&FILEINFO. There is a cariage return after the log.
	*************************************************/
	void Print(int iModule,VxLog::Level iLevel,int iLine,const char* iFile,const char* iLog);


private:

	
	/*************************************************
	Summary: Constructor. Initialize a debug Listener

	See also: VxLogListener.
	*************************************************/
	VxOutputDebugListener(const char* iName);

	
	VxOutputDebugListener(const VxOutputDebugListener&);
	
	VxOutputDebugListener& operator=(const VxOutputDebugListener&);

};

#ifdef DOCJETDUMMY  // Docjet duumy macro
#else
#if !defined(VX_NO_LOG)
	#define VXREGLOGDEBUG()							VxLog::GetLog().RegisterListener(VxOutputDebugListener::m_Guid,"OUTPUTDEBUG LISTENER",g_LogModule);
	#define VXREGLOGDEBUGEX(level,flags,timeformat)	{VxOutputDebugListener* li = (VxOutputDebugListener*)VxLog::GetLog().RegisterListener(VxOutputDebugListener::m_Guid,"OUTPUTDEBUG LISTENER",g_LogModule);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
	#define VXREGLOGDEBUGV(var)							VxLog::GetLog().RegisterListener(VxOutputDebugListener::m_Guid,"OUTPUTDEBUG LISTENER",var);
	#define VXREGLOGDEBUGEXV(var,level,flags,timeformat)	{VxOutputDebugListener* li = (VxOutputDebugListener*)VxLog::GetLog().RegisterListener(VxOutputDebugListener::m_Guid,"OUTPUTDEBUG LISTENER",var);if(li){li->SetLevel(level);li->SetFlags(flags);li->SetTimeFormat(timeformat);}}
#else
	#define VXREGLOGDEBUG()
	#define VXREGLOGDEBUGEX(level,flags,timeformat)
	#define VXREGLOGDEBUGV(var)
	#define VXREGLOGDEBUGEXV(var,level,flags,timeformat)
#endif
#endif // Docjet duumy macro

#endif
