/*************************************************************************/
/*	File : CKVariableManager.h						 					 */
/*	Author :  Aymeric Bard												 */	
/*																		 */	
/*	Virtools SDK 														 */	 
/*	Copyright (c) Virtools 2002, All Rights Reserved.					 */	
/*************************************************************************/
#ifndef CKVARIABLEMANAGER_H

#define CKVARIABLEMANAGER_H "$Id:$"

#include "CKDefines.h"
#include "CKBaseManager.h"

#ifdef RUNTIME

#define VARDESCRIPTION(a) NULL
#else
#define VARDESCRIPTION(a) a
#endif

/************************************************************************
Name: CKVariableManager

Summary: This manager is used to expose some global (or unique) variables,
like floats, integers or strings by replacing the native type (be it int, float 
or XString) with a more complex one, CKVariableManager::VariableInt, Float or String
(VInt, VFloat or VString for short). This new type will works exactly as the
old one, except that you have to "bind" the variable to a unique name, of the
form "category/variablename" with the CKContext this variable refers to.

  Once a variable is bound, external users of the class containing
it can read and write (for non read-only) its value using the 
CKVariableManager and its unique name. The value can be read/write by its
native type or by a string, in a way similar to the CKParameters.

  Another way to access the variable is to create a CKParameterVariable registered
to the same unique name. This parameter will then works as a normal CKParameterLocal
except that it will address directly the real variable, without copying/synchronizing.

  A variable has always a default value and you can ask it if it is currently different
from it and you can also restore its default value, with the RestoreValue() function.


  There exists variant of variables:
  
	- They can be Read-Only, meaning that one can access its value only for 
  reading purpose. A variable of this type must returns TRUE when its
  virtual function IsReadOnly() is called. Implementation of the common type
  has been done in Read-Only version : they are VIntRO, VBoolRO, VFloatRO, VStringRO.
  
	- They can be Composition Depending: If a variable is Composition Depending, it
  means its value must be saved with the composition. If not, it is application 
  dependent, and it can take its value from a config file for example, like the
  one an application can set to the CKVariableManager using the function
  SetApplicationFile(). A variable can be natively Composition Depending by
  overriding the IsCompositionDepending() virtual function and returning TRUE
  (This has be done for the base types : CVInt, CVBool, CVFloat, CVString) or
  it can be marked as composition depending for a specific case. This is done
  on any application depending variable, by calling the MarkAsCompositionDepending()
  function. Once a variable is marked, it is bound with the current 
  composition but it will be automatically unmarked when the next clear all
  will occur.


  Concerning the edition of the variable, the basic types are edited in a straightforward
  way, but one can create INT variables with a different type of edition, like
  an enumeration of a flag list. This can be done by overriding the GetRepresentation()
  virtual function and returning a string that can be of the form:

- "enum:a=label1;b=label2;c=label3;..."  list of possible value a,b,c, 
with optional names in parenthesis

- "flag:a=label1;b=label2;c=label3;..."  same as enum. but represented 
as a flag. For the flag you're entitled to ensure that the value are not
crossing over themselves (like flag:box=1;sphere=2;cylinder=3)

  The Bool variable has overrided it so that you can choose between FALSE and TRUE
  value in a combo box, although its only an int that can be 0 or 1.

  One last thing you can do about variable is being notified when the variable
  is read or when it is written (only through the interface of the CKVariableManager)
  This is done by inheriting the variable type you want (VInt, CVFloat, VBoolrRO, etc..) 
  and overriding the PreRead() and PostWrite() virtual functions. This allow you to calculate 
  the real value of the variable before it is read for example, or force an action
  when some other variable has been changed.
	

See Also: 
*************************************************************************/
class CKVariableManager : public CKBaseManager 
{
friend class CKParameterVariable;
public :

	///
	// Types
	enum RestoreMode {
		ALL					= 1,
		COMPOSITION			= 2,
		NATIVECOMPOSITION	= 3
	};

	/************************************************************************
	Summary: This class represent a bound variable, accessing by its name 
	with the VariableManager. You can't instantiate Variable directly but either 
	their simple derived classes for existing types VariableInt, VariableFloat or
	VariableString either one specific class you will derive from one of the 
	preceding class, to override some of the virtual functions presenting its
	functionality.

	
	See Also: CKVariableManager
	*************************************************************************/
	class Variable
	{
	friend class CKVariableManager;
	public:
		// Types
		enum  Type {
			INT		= 1,	// An Integer	
			FLOAT	= 2,	// A floating point value	
			STRING	= 3		// A XString 
		};

		// Flags
		enum Flags {
			READONLY				= 0x0001, // Variable can only be read , not written too.
			HIDDEN					= 0x0002, // Variable will not be shown in the interface	
			COMPOSITIONBOUND		= 0x0004, // Variable value will be saved when saving a composition (user decision in the interface)
			NATIVEONLY				= 0x0008, // Variable value will be saved when saving a composition (variable creater decision in code)
			NATIVECOMPOSITIONBOUND	= 0x000c, // Combination of COMPOSITIONBOUND & NATIVEONLY to indicate the value must be saved within a composition

		};

		// Dtor
		virtual ~Variable() {}

		/************************************************************************
		Summary: Returns the description of the variable. Can be NULL.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual const char* GetDescription() const {
#ifndef RUNTIME
			return m_Description;
#else
			return NULL;
#endif
		}

		/************************************************************************
		Summary: This function modifies the flags of the variable.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual void ModifyFlags(XDWORD iAdd, XDWORD iRemove)
		{
			m_Flags |= iAdd;
			m_Flags &= ~iRemove;
		}

		/************************************************************************
		Summary: This function gets the flags of the variable.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual XDWORD GetFlags()
		{
			return m_Flags;
		}

		/************************************************************************
		Summary: This function tells if the variable is different from its 
		default value.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual XBOOL IsDifferentFromDefault() const = 0;

		/************************************************************************
		Summary: This function restore the variable to its default value.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual void Restore() = 0;

		/************************************************************************
		Summary: This function gives the type of the variable. It can be any type
		of the VariableType enum : Variable::INT, Variable::FLOAT or 
		Variable::STRING.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual Type GetType() const = 0;

		/************************************************************************
		Summary: sets a variable from its string representation of a value.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual void SetStringValue(const char* iValue) = 0;

		/************************************************************************
		Summary: Fills a string with the string representation of the variable.

		
		See Also: CKVariableManager
		*************************************************************************/
		virtual void GetStringValue(XString& oValue) const = 0;

		/************************************************************************
		Summary: This function gives the representation (ie the input format) of
		a variable type.
		
		Remarks:
			By default this string is NULL, saying that the format 	is standard (an int is edited as an integer, float as a float, etc.), but you can 
		give a complex format for your customized types.
		3 different formats are supported:

			- "range:s=min;e=max;t=step" range from s to e, with an optional 
			step of t (default is 1) and an optionnal description in parenthesis.
			
			- "enum:a=toto;b=titi;c=tutu;..."  list of possible value a,b,c, 
			with optional names in parenthesis

			- "flag:a=toto;b=titi;c=tutu;..."  same as enum. but represented 
			as a flag
		See Also: CKVariableManager
		*************************************************************************/
		virtual const char*  GetRepresentation() const {
			return NULL;
		}

		/************************************************************************
		Summary: This function tells if the variable can be written from the 
		VariableManager. Default is false.

		
		See Also: CKVariableManager
		*************************************************************************/
		XBOOL IsReadOnly() const {
			return m_Flags&READONLY;
		}

		/************************************************************************
		Summary: This function tells if the variable must be saved with the current 
		composition. Default is true.

		
		See Also: CKVariableManager
		*************************************************************************/
		XBOOL IsCompositionDepending() const {
			return (m_Flags&COMPOSITIONBOUND);
		}

		/************************************************************************
		Summary: This function tells if the variable must be saved with the current 
		composition. Default is true.

		
		See Also: CKVariableManager
		*************************************************************************/
		XBOOL IsNativeCompositionDepending() const {
			return (m_Flags&NATIVEONLY);
		}

		/************************************************************************
		Summary: This function tells if the variable must be hidden in the interface
		of the application. Default is false.

		
		See Also: CKVariableManager
		*************************************************************************/
		XBOOL IsHidden() const {
			return m_Flags&HIDDEN;
		}

#ifndef RUNTIME
		/************************************************************************
		Summary: This function sets the description of the variable.

		
		See Also: CKVariableManager
		*************************************************************************/
		void SetDescription(const char* iDescription)
		{
			m_Description = iDescription;
		}
#endif

	protected:
		// Ctor 
		Variable():
			 m_Flags(0)
#ifndef RUNTIME
			,m_Description(NULL)
#endif
			{}

		///
		// Members

		// Flags
		XDWORD		m_Flags;
#ifndef RUNTIME
		// description
		const char*	m_Description;
#endif
	};


/************************************************************************
Summary: Variable modification watching class.
Remarks:
This clas is used to watch a variable and detect when it is written to 
or read from (by the use of the VariableManager only)


See Also: CKVariableManager
*************************************************************************/
	class Watcher
	{
	public:
		virtual ~Watcher() {};
	
/************************************************************************
Summary: Called before a variable is read by manager.
Remarks:
This function is called when a variable is read by the interface of the CKVariableManager, to let the owner of the variable be
notified and react accordingly.
To use this functionality, you'll have to derive a new class for your
specific variable and implement what has to be done in the update 
function.


See Also: CKVariableManager, PostWrite
*************************************************************************/
		virtual void PreRead(const char* iName) {
		}

/************************************************************************
Summary:  Called after a variable is written by manager.
Remarks:
This function is called when a variable is modified by the interface of the CKVariableManager, to let the owner of the variable be
notified and react accordingly.
To use this functionality, you'll have to derive a new class for your
specific variable and implement what has to be done in the update 
function.


See Also: CKVariableManager, PreRead
*************************************************************************/
		virtual void PostWrite(const char* iName) {
		}
	};

	// Add a watcher
	void		RegisterWatcher(const char* iName, Watcher* iWatcher);
	
	// Removes a watcher
	Watcher*	UnRegisterWatcher(const char* iName);

	// types
	typedef XHashTable<Variable*,const char*> HashName2Var;

	/************************************************************************
	Summary: This is the class that allow iteration on all variables registered 
	in the variable manager.
	*************************************************************************/
	class Iterator
	{
		friend class CKVariableManager;
	public:
		// Accessors to variable members

		/************************************************************************
		Summary: returns the unique name of the variable pointed by the iterator. 

		Return Value: a constant string on the variable name
		*************************************************************************/
		const char*		GetName() {
			return iterator.GetKey();
		}

		/************************************************************************
		Summary: returns the variable pointed to by the iterator. 

		Return Value: a pointer on a CKVariable.
		*************************************************************************/
		Variable*		GetVariable() {
			return (*iterator);
		}

		// Iteration management
		
		/************************************************************************
		Summary: iterates to the next variable. 
		*************************************************************************/
		Iterator&		operator++(int) { 
			++iterator;
			return *this;
		}

		/************************************************************************
		Summary: tells if the iteration is over. 

		Return Value: TRUE if the iteration is at its end, FALSE otherwise.
		*************************************************************************/
		XBOOL End() {
			return (iterator == endIterator);
		}

	private:
		Iterator(HashName2Var::Iterator iIterator, HashName2Var::Iterator iEndIterator):iterator(iIterator),endIterator(iEndIterator) {}

		// the iterator
		HashName2Var::Iterator iterator;
		HashName2Var::Iterator endIterator;
	};

	///////////////////////////////
	// Methods

	/************************************************************************
	Summary: Registers the variable to the CKVariableManager with an absolute 
	name. If a variable with the same absolute name is already registered, does
	nothing but producing an output error in the console.

	Arguments:
		iContext : a valid CKContext
		iName : the absolute name of the variable, in the form "category/varname"
		iDefaultValue : the default value (and initial value) of the variable
		iDescription: the description of the variable meaning and functionnality.
		For no runtime overhead, you must give your description surrounded by
		the VARDESCRIPTION() macro.

	
	See Also: CKVariableManager
	*************************************************************************/
	void Bind(const char* iName, int*	iAddress, int iDefaultValue, XDWORD iFlags, const char* iDescription);
	void Bind(const char* iName, int*	iAddress, int iDefaultValue, XDWORD iFlags, const char* Representation, const char* iDescription);
	void Bind(const char* iName, float*	iAddress, float iDefaultValue, XDWORD iFlags, const char* iDescription);
	void Bind(const char* iName, XString* iAddress, const XString& iDefaultValue, XDWORD iFlags, const char* iDescription);
	void UnBind(const char* iName);

	/************************************************************************
	Summary: Gets the value of a variable. 
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"
		oValue : address of data that will contain the variable value

	Return Value: TRUE if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL GetValue(const char* iName, int* oValue);

	/************************************************************************
	Summary: Sets the value of a variable. 
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"
		oValue : new value of the variable

	Return Value: TRUE if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL SetValue(const char* iName, int iValue);

	// float

	XBOOL GetValue(const char* iName, float* oValue);
	XBOOL SetValue(const char* iName, float iValue);

	// string

	XBOOL GetValue(const char* iName, XString& oValue);
	XBOOL SetValue(const char* iName, const char* iValue);

	/************************************************************************
	Summary: Sets the value of a variable as a string. 
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"
		iValue : string representation of the value of the variable

	Return Value: CK_OK if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL SetStringValue(const char* iName, const char* iValue);

	/************************************************************************
	Summary: Gets the value of a variable as a string. 
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"
		oValue : string representation of the value of the variable

	Return Value: CK_OK if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL GetStringValue(const char* iName,		XString& oValue);

	/************************************************************************
	Summary: Sets a variable that is not composition depending as a depending
	one. This variable will be saved in composition, overriding its application
	settings
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"
		iSaving : TRUE if the variable must be saved with composition, FALSE 
		otherwise.

	Return Value: CK_OK if the variable were existing and application dependent

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL		MarkAsCompositionDepending(const char* iName, XBOOL iSaving);

	// Get a bound variable
	Variable* GetVariable(const char* iName);
	

	/************************************************************************
	Summary: Restores a variable to its default value. 
	

	Arguments:
		iName : the absolute name of the variable, in the form "category/varname"

	Return Value: CK_OK if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	XBOOL RestoreValue(const char* iName);

	/************************************************************************
	Summary: Restores a defined set of variables to their default value. 
	

	Arguments:
		iMode : the set of variables that must be restored. This can be
		CKVariableManager::ALL : every variables
		CKVariableManager::COMPOSITION : every variables Composition Depending, 
		including the ones marked as so.
		CKVariableManager::NATIVECOMPOSITION : only variables natively Composition 
		Depending

	Return Value: CK_OK if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	void	   RestoreAll(RestoreMode iMode = ALL);

	/************************************************************************
	Summary: Import variables value from a given file. You can specify a set 
	of variables that must be read.
	

	Arguments:
		iFileName : the absolute file name of the variable file.
		iVariables: an array of variable names you want to be read. Specify NULL
		if you want all the variables in the file to be imported.

	Return Value: CK_OK if successfull

	See Also: CKVariableManager, Export
	*************************************************************************/
	XBOOL		Import(const char* iFileName, const XArray<const char*>* iVariables);

	/************************************************************************
	Summary: Export variables value from a given file. You can specify a set 
	of variables that must be written.
	

	Arguments:
		iFileName : the absolute file name of the variable file.
		iVariables: an array of variable names you want to be written. Specify NULL
		if you want all the variables in the file to be exported.

	Return Value: CK_OK if successfull

	See Also: CKVariableManager, Import
	*************************************************************************/
	XBOOL		Export(const char* iFileName, const XArray<const char*>* iVariables,BOOL iAddToCurrentFile=FALSE);

	/************************************************************************
	Summary: Gets the count of registered variables. 
	

	Return Value: the number of variables

	See Also: CKVariableManager
	*************************************************************************/
	int			GetVariableCount();

/************************************************************************
Summary: Gets an iterator on all registered variables.

Remarks:
{html:<table width="90%" border="1" align="center" bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" bgcolor="#FFFFFF" bordercolor="#FFFFFF"><tr bgcolor="#E6E6E6" bordercolor="#000000"><td>}	
{html:<pre>}
{html:	CKVariableManager::Iterator it = vm->GetVariableIterator();}
{html:		for (;!it.End(); it++) }
{html:			const char* name = it.GetName();}
{html:</pre>}
{html:</td></tr></table>}

Return Value: a CKVariableManager::Iterator

See Also: CKVariableManager
*************************************************************************/
	Iterator	GetVariableIterator();

	
	void WatcherPreRead(const char* iName);
	
	void WatcherPostWrite(const char* iName);

	/************************************************************************
	Summary: Sets a config file for the application depending variables. These 
	variables will be saved in this file at each clear all, and the values of
	the forced-composition variables will be restored according to this file
	too.
	

	Arguments:
		iName : the absolute file name of the config file.

	Return Value: CK_OK if the variable was found

	See Also: CKVariableManager
	*************************************************************************/
	CKERROR		SetApplicationFile(const char* iFileName);
	const char*	GetApplicationFile();

	/************************************************************************
	Summary: Synchronizes the application and the application file, either by 
	writing the variables values to the file, or by reading them from the file.
	You can shynchronize all the variables or only a subset of them.

	

	Arguments:
		iToFile : TRUE if the variables must be written to the file, FALSE if 
		the variables must be read from the file.
		
		iVariables : array of variable names to synchronize. NULL means all the 
		variables.

	Return Value: CK_OK if the synchronization is ok.

	See Also: CKVariableManager
	*************************************************************************/
	CKERROR		SynchronizeApplicationFile(CKBOOL iToFile,const XArray<const char*>* iVariables = NULL);
//-------------------------------------------------------------------------
// Internal functions 
#ifdef DOCJETDUMMY // DOCJET secret macro
#else

	virtual CKStateChunk* SaveData(CKFile* SavedFile);
	virtual CKERROR LoadData(CKStateChunk *chunk,CKFile* LoadedFile);
	virtual CKERROR PostClearAll();
	virtual CKDWORD	GetValidFunctionsMask()	{ 
		return CKMANAGER_FUNC_PostClearAll; 	
	}	


#endif
};


typedef CKVariableManager::Variable VxVar;


#define BIND(ctx,name,address,defaultvalue,flags,description) ctx->GetVariableManager()->Bind(name,address,defaultvalue,flags,VARDESCRIPTION(description));

#define BINDREP(ctx,name,address,defaultvalue,flags,representation,description) ctx->GetVariableManager()->Bind(name,(int*)address,defaultvalue,flags,representation,VARDESCRIPTION(description));

#define UNBIND(ctx,name) ctx->GetVariableManager()->UnBind(name);

#define ADDWATCHER(ctx,name,watcher) ctx->GetVariableManager()->RegisterWatcher(name,watcher);

#define REMOVEWATCHER(ctx,name) delete ctx->GetVariableManager()->UnRegisterWatcher(name);

#define REMOVEWATCHERNODELETE(ctx,name) ctx->GetVariableManager()->UnRegisterWatcher(name);

#endif

