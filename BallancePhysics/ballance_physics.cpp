#include "ballance_physics.h"
#include "ivp_physics.hxx"
#include "ivp_templates.hxx"
#include "ivp_mindist.hxx"
#include "ivp_core.hxx"
#include "ivp_friction.hxx"
#include "ivp_time.hxx"
#include "ivp_phantom.hxx"
#include "ivp_material.hxx"
#include "ivp_authenticity.hxx"
#include "ivp_collision_filter.hxx"
#include "ivp_anomaly_manager.hxx"
#include "ivu_linear_macros.hxx"
#include "ivu_string.hxx"
#include "ivp_listener_collision.hxx"
#include "ivp_listener_object.hxx"
#include "ivp_listener_psi.hxx"
#include "ivp_surman_polygon.hxx"
#include "ivp_surbuild_pointsoup.hxx"
#include "ivp_surbuild_ledge_soup.hxx"
#include "ivp_template_surbuild.hxx"
#include "ivp_controller_motion.hxx"
#include "ivp_ray_solver.hxx"
#include "ivp_cache_object.hxx"
#include "ivp_template_constraint.hxx"
#include "ivp_constraint.hxx"
#include "ivp_actuator.hxx"
#include "ivp_controller_stiff_spring.hxx"
#include "ivp_betterdebugmanager.hxx"
#include "ivu_geometry.hxx"
#include "ivu_float.hxx"

#include <list>
#include <map>
#include <string>

#include "SimpleLinkedList.hpp"
#include "SimpleStack.hpp"
#include <stdarg.h>
#include <stdio.h>

#if defined(_WINDOWS)
#	include <windows.h>
#	include "resource.h"
#elif defined(__linux__)
#	include <sstream>
#	include <unistd.h>
#elif defined(__APPLE__)
#	include <mach-o/dyld.h>
#endif

#define security_check_number 2123

#if _MSC_VER
#pragma comment(lib, "ivp_compactbuilder.lib")
#pragma comment(lib, "ivp_physics.lib")
#endif

#pragma region Define

#define check_nullptr(param) if(!param) { callback_err("%s is nullptr!", #param); return; }
#define check_nullptr_return(param, ret) if(!param) { callback_err("%s is nullptr!", #param); return ret; }

#define sassert(con) if(!(con)) { callback_err("assertion failure: %s(%d)", __FILE__, __LINE__); return; }
#define sassert2(con, message) if(!(con)) { callback_err("assertion failure: %s %s(%d)", message, __FILE__, __LINE__); return; }
#define sassert_return(con, ret) if(!(con)) { callback_err("assertion failure: %s(%d)", __FILE__, __LINE__); return ret; }
#define sassert2_return(con, message, ret) if(!(con)) { callback_err("assertion failure: %s %s(%d)", message, __FILE__, __LINE__); return ret; }
#define sKey 123451

#define sUnit unsigned int

#pragma region OS

#if defined(__APPLE__) && (defined(__GNUC__) || defined(__xlC__) || defined(__xlc__))
#  define OS_DARWIN
#  define OS_BSD4
#  ifdef __LP64__
#    define OS_DARWIN64
#  else
#    define OS_DARWIN32
#  endif
#elif !defined(SAG_COM) && (defined(WIN64) || defined(_WIN64) || defined(__WIN64__))
#  define OS_WIN32
#  define OS_WIN64
#elif !defined(SAG_COM) && (defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(__NT__))
#  if defined(WINCE) || defined(_WIN32_WCE)
#    define OS_WINCE
#  else
#    define OS_WIN32
#  endif
#elif defined(__linux__) || defined(__linux) || defined(LINUX)
#  define OS_LINUX
#elif !(defined(OS_IOS) || defined(OS_ANDROID))
#  error "Unsupport OS"
#endif

#if defined(OS_WIN32) || defined(OS_WIN64) || defined(OS_WINCE)
#  define OS_WIN
#endif

#if defined(OS_DARWIN)
#  define OS_MAC /* OS_MAC is mostly for compatibility, but also more clear */
#  define OS_MACX /* OS_MACX is only for compatibility.*/
#  if defined(OS_DARWIN64)
#     define OS_MAC64
#  elif defined(OS_DARWIN32)
#     define OS_MAC32
#  endif
#endif

#pragma endregion

#pragma endregion

#pragma region Callbacks

EventCallback eventCallback = nullptr;
char callbackBuffer[512];

void callback_err(const char* format, ...)
{
  va_list ap;
  va_start(ap, format);

  int len =
#ifdef _MSC_VER
  _vsnprintf(callbackBuffer, sizeof(callbackBuffer), format, ap);
#else
  vsprintf(callbackBuffer, format, ap);
#endif

  va_end(ap);

  if (eventCallback)
    eventCallback(sError, len, callbackBuffer);
}
void callback_warn(const char* format, ...)
{
  va_list ap;
  va_start(ap, format);

  int len =
#ifdef _MSC_VER
  _vsnprintf(callbackBuffer, sizeof(callbackBuffer), format, ap);
#else
  vsprintf(callbackBuffer, format, ap);
#endif

  va_end(ap);

  if (eventCallback)
    eventCallback(sWarning, len, callbackBuffer);
}
void callback_info(const char* format, ...)
{
  va_list ap;
  va_start(ap, format);

  int len = 
#ifdef _MSC_VER
  _vsnprintf(callbackBuffer, sizeof(callbackBuffer), format, ap);
#else
  vsprintf(callbackBuffer, format, ap);
#endif
  va_end(ap);

  if (eventCallback)
    eventCallback(sInfo, len, callbackBuffer);
}

#pragma endregion

#pragma region Structs

class MyGlobalListener;
class MyCollisionListener;
class MyPhantomListener;
class GroupFilter;
class sSurface
{
public:
  ~sSurface() {
    if (surface_manager)
      delete surface_manager;
    compact_surface = nullptr;
  }

  char name[32];
  IVP_SurfaceManager* surface_manager = nullptr;
  IVP_Compact_Surface* compact_surface = nullptr;
};
class sColDectInfo {
public:
  class sColDectInfo* next;
  class sColDectInfo* prev;
  int secrity_check;
  int col_id;
  float min_speed; 
  float max_speed;
  float sleep_afterwards;
  float speed_threadhold;
  double last_time;
  sPhysicsBody* body;
};
class sContractDectInfo {
public:
  class sContractDectInfo* next;
  class sContractDectInfo* prev;
  int secrity_check;
  int col_id;
  float time_delay_start;
  float time_delay_end;
  bool is_contract;
  bool is_contract_start_check;
  double last_contract_time;
  double last_contract_start_time;
  sPhysicsBody* body;
};

struct sPhysicsWorld
{
  IVP_Environment* environment;
  MyGlobalListener* global_listener;
  EventCallback eventCallback;
  GroupFilter* group_filter;
  std::map<sUint, sSurface*> createdSurfaces;
  bool disableSim;
  int securityCheck;
};
struct sPhysicsBody
{
  int id;
  float updated_pos[3];
  float updated_rot[4];
  struct sPhysicsBody* next;
  struct sPhysicsBody* prev;
  int layer;
  sUint filter_info;
  sBool is_phantom;
  sBool is_frozen;
  IVP_Real_Object* object;
  IVP_Material* material;
  PhantomEventCallback phantom_event_callback;
  CollisionEventCallback collision_event_callback;
  FrictionEventCallback friction_event_callback;
  MyPhantomListener* phantom_listener;
  MyCollisionListener* collision_listener;
  ContractEventCallback contract_event_callback;
  SimpleLinkedList<sColDectInfo> col_dections;
  SimpleLinkedList<sContractDectInfo> contract_dections;
  int col_id;
  float last_speed;
  sSurface* own_surface;
  sPhysicsWorld* own_world;
  char name[32];
  int securityCheck;
};
struct sMotionController
{
  IVP_Controller_Motion* controller_motion;
  IVP_Real_Object* object;
  sPhysicsBody* body;
  struct sMotionController* next;
  struct sMotionController* prev;
};
struct sPointBuffer
{
  IVP_U_Vector<IVP_U_Point> points;

  float* pointsBuffer;
  sUnit pointsCount;
  sUnit* trangles;
  sUnit trangleCount;
};
struct sConstant
{
  IVP_Constraint* handle;
};
struct sForce
{
  IVP_Actuator_Force* handle;
};
struct sSpring
{
  IVP_Controller_Stiff_Spring* stiff_spring_handle;
  IVP_Actuator_Spring* spring_handle;
};

#pragma endregion

#pragma region Forward

void do_physics_coll_detection_handler(sPhysicsBody* body, sPhysicsBody* other, IVP_U_Float_Point speed);
float do_physics_contact_detection(sPhysicsBody* body);
float physics_get_speed(sPhysicsBody* body);

#pragma endregion

#pragma region PoolUtils

void ConvertQuatToUnity(float* x, float* y, float* z, float* w) {
  //float iy = *y;
  //float iz = *z;

  //*x = -*x;
  //*y = -iz;
  //*z = -iy;
  //*w = *w;
}
void ConvertQuatToIVP(float* x, float* y, float* z, float* w) {
  //float iy = *y;
  //float iz = *z;

  //*x = *x;
  //*y = iz;
  //*z = iy;
  //*w = *w;
}
void ConvertPointToUnity(float* x, float* y, float* z) {
  /*float iy = *y;
  float iz = *z;

  *x = *x;
  *y = -iz;
  *z = iy;*/
}
void ConvertPointToIVP(float* x, float* y, float* z) {
  /*float iy = *y;
  float iz = *z;

  *x = *x;
  *y = iz;
  *z = -iy;*/
}

SimpleStack<IVP_U_Point> *poolPoints;
SimpleStack<IVP_U_Quat> *poolQuats;
int poolSmallSize = 0;

void init_small_pool(int smallPoolSize)
{
  poolSmallSize = smallPoolSize;

  if (poolSmallSize < 8) {
    callback_warn("smallPoolSize is too small! Use default 16.");
    poolSmallSize = 16;
  }

  poolPoints = new SimpleStack<IVP_U_Point>(poolSmallSize);
  poolQuats = new SimpleStack<IVP_U_Quat>(poolSmallSize);

  for (int i = 0; i < poolSmallSize; i++)
    poolPoints->push(new IVP_U_Point());
  for (int i = 0; i < poolSmallSize; i++)
    poolQuats->push(new IVP_U_Quat());
}
void destroy_small_pool()
{
  for (auto it = poolPoints->pop(); it != nullptr; it = poolPoints->pop())
    delete (it);  
  for (auto it = poolQuats->pop(); it != nullptr; it = poolQuats->pop())
    delete (it);

  delete poolPoints;
  delete poolQuats;
}

IVP_U_Point* create_point(float x, float y, float z) {
  ConvertPointToIVP(&x, &y, &z);
  auto pt = poolPoints->pop();
  if (pt) {
    pt->set(x, y, z); //negate x-values and y-values for unity
    return pt;
  }
  else {
    pt = new IVP_U_Point(x, y, z); //negate x-values and y-values for unity
    return pt;
  }
}
IVP_U_Quat* create_quat(float x, float y, float z, float w) {
  ConvertQuatToIVP(&x, &y, &z, &z);
  IVP_U_Quat* pt = poolQuats->pop();
  if (!pt)
    pt = new IVP_U_Quat();

  pt->x = x;
  pt->y = y;
  pt->z = z;
  pt->w = w;

  return pt;
}

void destroy_point(IVP_U_Point* pt) {
  if (pt) {
    if (!poolPoints->push(pt))
       delete pt;
  }
}
void destroy_quat(IVP_U_Quat* pt) {
  if (pt) {
    if (!poolQuats->push(pt))
      delete pt;
  }
}
void get_point(IVP_U_Point* pt, float* buf) {
  if (!pt) {
    callback_err("pt is nullptr!");
    return;
  }

  buf[0] = pt->k[0];
  buf[1] = pt->k[1];
  buf[2] = pt->k[2];

  ConvertPointToUnity(buf, buf + sizeof(float) * 1, buf + sizeof(float) * 2);
}
void get_quat(IVP_U_Quat* quat, float* buf) {
  if (!quat) {
    callback_err("quat is nullptr!");
    return;
  }

  buf[0] = quat->x;
  buf[1] = quat->y;
  buf[2] = quat->z;
  buf[3] = quat->w;

  ConvertQuatToUnity(&buf[0], &buf[1], &buf[2], &buf[3]);
}
float get_pi() { return (float)IVP_PI; }
float get_pi2() { return (float)IVP_PI_2; }

sUint rs_hash(const char* str, sUint len = 0)
{
  sUint b = 378551;
  sUint a = 63689;
  sUint hash = 0;
  sUint i = 0;

  if (len == 0)
    len = (sUint)strlen(str);

  for (i = 0; i < len; str++, i++)
  {
    hash = hash * a + (*str);
    a = a * b;
  }

  return hash;
}

#if defined(OS_WIN)
#define DirectorySeparatorChar L'\\'
#define AltDirectorySeparatorChar  L'/'
#define VolumeSeparatorChar  L':'

bool check_invalid_path_chars(std::wstring path)
{
  for (size_t i = 0; i < path.size(); i++)
  {
    int num = (int)(path)[i];
    if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
    {
      return true;
    }
  }
  return false;
}
std::wstring get_file_name(std::wstring path)
{
  if (!path.empty())
  {
    if (check_invalid_path_chars(path))
      return path;

    size_t length = path.size();
    size_t num = length;
    while (--num >= 0)
    {
      wchar_t c = (path)[num];
      if (c == DirectorySeparatorChar || c == AltDirectorySeparatorChar || c == VolumeSeparatorChar)
      {
        return std::wstring(path.substr(num + 1, length - num - 1));
      }
    }
  }
  return path;
}
#else
#define DirectorySeparatorChar '\\'
#define AltDirectorySeparatorChar '/'
#define VolumeSeparatorChar ':'

bool check_invalid_path_chars(std::string path)
{
  for (size_t i = 0; i < path.size(); i++)
  {
    int num = (int)(path)[i];
    if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
    {
      return true;
    }
  }
  return false;
}
std::string get_file_name(std::string path)
{
  if (!path.empty())
  {
    if (check_invalid_path_chars(path))
      return path;

    size_t length = path.size();
    size_t num = length;
    while (--num >= 0)
    {
      char c = (path)[num];
      if (c == DirectorySeparatorChar || c == AltDirectorySeparatorChar || c == VolumeSeparatorChar)
      {
        return std::string(path.substr(num + 1, length - num - 1));
      }
    }
}
  return path;
}
#endif
#ifdef OS_LINUX 
size_t get_executable_path(char* processdir, char* processname, size_t len)
{
  char* path_end;
  if (readlink("/proc/self/exe", processdir, len) <= 0)
    return -1;
  path_end = strrchr(processdir, '/');
  if (path_end == NULL)
    return -1;
  ++path_end;
  strcpy(processname, path_end);
  *path_end = '\0';
  return (size_t)(path_end - processdir);
}
#endif

#pragma endregion

#pragma region Listeners

class MyPhantomListener : public IVP_Listener_Phantom
{
public:
  MyPhantomListener(sPhysicsBody* _body) {
    body = _body;
  }
  ~MyPhantomListener() {}
  void mindist_entered_volume(class IVP_Controller_Phantom* controller, class IVP_Mindist_Base* mindist) {
    IVP_Real_Object* objs[2];
    mindist->get_objects(objs);

    if (body->phantom_event_callback)
      body->phantom_event_callback(true, (sPhysicsBody*)objs[0]->client_data, (sPhysicsBody*)objs[1]->client_data);
  }
  void mindist_left_volume(class IVP_Controller_Phantom* controller, class IVP_Mindist_Base* mindist) {
    IVP_Real_Object* objs[2];
    mindist->get_objects(objs);

    if (body->phantom_event_callback)
      body->phantom_event_callback(false, (sPhysicsBody*)objs[0]->client_data, (sPhysicsBody*)objs[1]->client_data);
  }
  void core_entered_volume(class IVP_Controller_Phantom* controller, class IVP_Core* pCore) {}
  void core_left_volume(class IVP_Controller_Phantom* controller, class IVP_Core* pCore) {}
  void phantom_is_going_to_be_deleted_event(class IVP_Controller_Phantom* controller) {}
private:
  sPhysicsBody* body;
};
class MyCollisionListener : public IVP_Listener_Collision {
public:
  MyCollisionListener(sPhysicsBody* body) : IVP_Listener_Collision(IVP_LISTENER_COLLISION_CALLBACK_POST_COLLISION | IVP_LISTENER_COLLISION_CALLBACK_FRICTION) {
    this->body = body;
  }
  ~MyCollisionListener() {}
  void event_pre_collision(IVP_Event_Collision* evt) {}
  void event_post_collision(IVP_Event_Collision* evt) {
    auto o1 = evt->contact_situation->objects[0];
    auto o2 = evt->contact_situation->objects[1];

    if (body->contract_event_callback) {
      do_physics_coll_detection_handler(
        (sPhysicsBody*)o1->client_data,
        (sPhysicsBody*)o2->client_data,
        &evt->contact_situation->speed
      );
    }

    if (body->collision_event_callback) {
      if (evt->d_time_since_last_collision >= collision_call_sleep) {

        if (!o1 || !o2)
          return;

        auto pt1 = evt->contact_situation->contact_point_ws;
        auto pt2 = evt->contact_situation->speed;
        auto pt3 = evt->contact_situation->surf_normal;

        body->collision_event_callback(
          (sPhysicsBody*)o1->client_data,
          (sPhysicsBody*)o2->client_data,
          create_point(pt1.k[0], pt1.k[1], pt1.k[2]),
          create_point(pt2.k[0], pt1.k[1], pt1.k[2]),
          create_point(pt3.k[0], pt1.k[1], pt1.k[2])
        );
      }
    }
  }
  void event_collision_object_deleted(class IVP_Real_Object*) {}
  void event_friction_created(IVP_Event_Friction* evt) {
    if (body->friction_event_callback) {

      auto o1 = evt->contact_situation->objects[0];
      auto o2 = evt->contact_situation->objects[1];
      if (!o1 || !o2)
        return;

      auto pt1 = evt->contact_situation->contact_point_ws;
      auto pt2 = evt->contact_situation->speed;
      auto pt3 = evt->contact_situation->surf_normal;
      body->friction_event_callback(
        sTrue,
        (sPhysicsBody*)o1->client_data,
        (sPhysicsBody*)o2->client_data,
        evt->friction_handle,
        create_point(pt1.k[0], pt1.k[1], pt1.k[2]),
        create_point(pt2.k[0], pt1.k[1], pt1.k[2]),
        create_point(pt3.k[0], pt1.k[1], pt1.k[2])
      );
    }
  }
  void event_friction_deleted(IVP_Event_Friction* evt) {
    if (body->friction_event_callback) {

      auto o1 = evt->contact_situation->objects[0];
      auto o2 = evt->contact_situation->objects[1];
      if (!o1 || !o2)
        return;

      auto pt1 = evt->contact_situation->contact_point_ws;
      auto pt2 = evt->contact_situation->speed;
      auto pt3 = evt->contact_situation->surf_normal;
      body->friction_event_callback(
        sFalse,
        (sPhysicsBody*)o1->client_data,
        (sPhysicsBody*)o2->client_data,
        evt->friction_handle,
        create_point(pt1.k[0], pt1.k[1], pt1.k[2]),
        create_point(pt2.k[0], pt1.k[1], pt1.k[2]),
        create_point(pt3.k[0], pt1.k[1], pt1.k[2])
      );
    }
  }

  float collision_call_sleep = 0;
private:
  sPhysicsBody* body;
};

#pragma endregion

#pragma region Main

#define VER 2302

void init_functions(int i);
void init_crash_handler();
void init_info();
void test(int i);

bool disablePhysics = false;
bool initStatus = false;
void* functions[256];

void callback_ivp_message_callback(const char* message) {
  if (eventCallback) {
    if (eventCallback(sError, (int)strlen(message), message) != 0) 
      return;
  }
  printf("%s", message);
}

//Console redirect

#if defined(OS_WIN)
FILE* fileIn = nullptr;
FILE* file = nullptr;
FILE* fileErr = nullptr;
bool consoleOpen = false;
#endif

void init_console() {
#if defined(OS_WIN)
  if (!consoleOpen) {
    consoleOpen = true;

    if (AllocConsole()) {
      freopen_s(&fileIn, "CONIN$", "r", stdin);
      freopen_s(&fileErr, "CONOUT$", "w", stderr);
      freopen_s(&file, "CONOUT$", "w", stdout);
    }
    else {
      callback_err("AllocConsole failed : %d", GetLastError());
    }
  }
#endif
}
void destroy_console() {
#if defined(OS_WIN)
  if (consoleOpen) {
    consoleOpen = false;
    FreeConsole();

    if (fileIn) {
      fclose(fileIn);
      fileIn = nullptr;
    }
    if (fileErr) {
      fclose(fileErr);
      fileErr = nullptr;
    }
    if (file) {
      fclose(file);
      file = nullptr;
    }
  }
#endif
}

extern "C"
#if _MSC_VER 
__declspec(dllexport)
#else
__attribute__((visibility("default")))
#endif
void* ballance_physics_entry(sBool init, sInitStruct * options)
{
  if (init == 0) {
    if (initStatus) {
      ivp_set_message_callback(nullptr);
      destroy_small_pool();
      destroy_console();
      initStatus = false;
    }
  }
  else if (init == 1) {
    if (!initStatus) {
      if (options) {
        ivp_set_message_callback(callback_ivp_message_callback);

        if (options->showConsole) init_console();
        
        init_crash_handler();
        init_info();
        init_small_pool(options->smallPoolSize);
        init_functions(0);

        initStatus = true;
      }
      else {
        callback_err("PhysicsEngine entry: The options param must be provide.");
      }
    }
    return functions;
  }
  else if (init == 2) {
    destroy_console();
  }
  else if (init == 3) {
    init_console();
  }
  else if (init == 4) {
    return (void*)initStatus;
  }
  else if (init == 5) {
    return functions;
  }
  return nullptr;
}

#if defined(OS_WIN)
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
  switch (ul_reason_for_call)
  {
  case DLL_PROCESS_ATTACH:
  case DLL_THREAD_ATTACH:
  case DLL_THREAD_DETACH:
  case DLL_PROCESS_DETACH:
    break;
  }
  return TRUE;
}
#endif

#if defined(OS_WIN)
std::wstring module0Path;
#else
std::string module0Path;
#endif
std::string buildUUID;
std::string buildUUID2;//666d ccad 4ae6 97b4 5aac 145f 18f4 9c5b

void init_info() {
#if defined(OS_WIN)
  wchar_t fileName[500];
  GetModuleFileNameW(0, fileName, 500);
  module0Path = fileName;

  const char* cmdLine = GetCommandLineA();
  if (strstr(cmdLine, "-disable-physics") != nullptr)
    disablePhysics = true;
#elif defined(OS_LINUX)
  char path[1024];
  char processname[64];
  get_executable_path(path, processname, sizeof(path));
  module0Path = path;
#elif defined(OS_DARWIN)
  char path[1024];
  uint32_t rawPathSize = (uint32_t)sizeof(path);
  _NSGetExecutablePath(path, &rawPathSize);
  module0Path = path;
#endif
}
void copy_str(char* buf, int len, short* instr, int inlen) {
  for (int i = 0; i < inlen && i < len; i++)
    buf[i] = (char)instr[i];
}
void copy_strw(wchar_t* buf, int len, short* instr, int inlen) {
  for (int i = 0; i < inlen && i < len; i++)
    buf[i] = (wchar_t)instr[i];
}
int get_version() { return VER; }

#pragma endregion

#pragma region CrashHandler

#if defined(OS_WIN)

#include <stdio.h>
#include <stdlib.h>
#include <DbgHelp.h>
#pragma comment(lib, "Dbghelp.lib")

BOOL GenerateCrashInfo(PEXCEPTION_POINTERS pExInfo, LPCWSTR info_file_name, LPCWSTR file_name, SYSTEMTIME tm, LPCWSTR dir) {

  FILE* fp = NULL;
  _wfopen_s(&fp, info_file_name, L"w");
  if (fp) {
    fwprintf_s(fp, L"=== Exception ===== %04d/%02d/%02d %02d:%02d:%02d ===========", tm.wYear, tm.wMonth, tm.wDay, tm.wHour, tm.wMinute, tm.wSecond);
    fwprintf_s(fp, L"\r\nAddress : 0x%IX  Code : 0x%08X  (0x%08X)",
      (ULONG_PTR)pExInfo->ExceptionRecord->ExceptionAddress, pExInfo->ExceptionRecord->ExceptionCode,
      pExInfo->ExceptionRecord->ExceptionFlags);
    fclose(fp);
    return TRUE;
  }
  return FALSE;
}
LONG GenerateMiniDump(PEXCEPTION_POINTERS pExInfo)
{
  TCHAR dmp_path[MAX_PATH];
  GetTempPath(MAX_PATH, dmp_path);

  SYSTEMTIME tm;
  GetLocalTime(&tm);
  TCHAR file_name[128];
  swprintf_s(file_name, L"%s\\BallancePhysicsCrashDump%d%02d%02d-%02d%02d%02d.dmp", dmp_path,
    tm.wYear, tm.wMonth, tm.wDay, tm.wHour, tm.wMinute, tm.wSecond);
  TCHAR info_file_name[128];
  swprintf_s(info_file_name, L"%s\\BallancePhysicsCrashDump%d%02d%02d-%02d%02d%02d.txt", dmp_path,
    tm.wYear, tm.wMonth, tm.wDay, tm.wHour, tm.wMinute, tm.wSecond);

  //Create file
  HANDLE hFile = CreateFile(file_name, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
    FILE_ATTRIBUTE_NORMAL, NULL);

  //Generate Crash info
  BOOL hasCrashInfo = GenerateCrashInfo(pExInfo, info_file_name, file_name, tm, dmp_path);

  //Gen Dump File and show dialog

  TCHAR expInfo[128];
  swprintf_s(expInfo, L"Exception !!! Address : 0x%IX  Code : 0x%08X  (0x%08X)",
    (ULONG_PTR)pExInfo->ExceptionRecord->ExceptionAddress, pExInfo->ExceptionRecord->ExceptionCode,
    pExInfo->ExceptionRecord->ExceptionFlags);

  if (hFile != INVALID_HANDLE_VALUE)
  {
    MINIDUMP_EXCEPTION_INFORMATION expParam;
    expParam.ThreadId = GetCurrentThreadId();
    expParam.ExceptionPointers = pExInfo;
    expParam.ClientPointers = FALSE;
    MiniDumpWriteDump(GetCurrentProcess(), GetCurrentProcessId(), hFile, MiniDumpWithDataSegs, (pExInfo ? &expParam : NULL), NULL, NULL);
    CloseHandle(hFile);

    TCHAR info[300];
    swprintf_s(info, L"Dump file: %s \nLast Error : %d", file_name, GetLastError());
    MessageBox(NULL, info, L"GenerateMiniDump", MB_ICONERROR | MB_SYSTEMMODAL);
  }
  else
  {
    TCHAR info[300];
    swprintf_s(info, L"Fail to create dump file: %s \nLast Error : %d", file_name, GetLastError());
    MessageBox(NULL, info, L"GenerateMiniDump", MB_ICONERROR | MB_SYSTEMMODAL);
  }
  return EXCEPTION_EXECUTE_HANDLER;
}
LONG WINAPI AppUnhandledExceptionFilter(PEXCEPTION_POINTERS pExInfo)
{
  if (IsDebuggerPresent())
    return EXCEPTION_CONTINUE_SEARCH;
  return GenerateMiniDump(pExInfo);
}

#endif // _WINDOWS

void init_crash_handler() {
#if defined(OS_WIN)
  SetUnhandledExceptionFilter(AppUnhandledExceptionFilter);
#endif // _WINDOWS
}

#pragma endregion

#pragma region Update

class MyGlobalListener : public IVP_Listener_Object {
public:
  SimpleLinkedList<sPhysicsBody> active_bodies;

  void event_object_deleted(IVP_Event_Object* obj) {
    active_bodies.remove((sPhysicsBody*)obj->real_object->client_data);
  }
  void event_object_created(IVP_Event_Object* obj) {
    active_bodies.add((sPhysicsBody*)obj->real_object->client_data);
  }
  void event_object_revived(IVP_Event_Object* obj) {
    ((sPhysicsBody*)obj->real_object->client_data)->is_frozen = false;
  }
  void event_object_frozen(IVP_Event_Object* obj) {
    ((sPhysicsBody*)obj->real_object->client_data)->is_frozen = true;
  }
};

void set_env_listeners(sPhysicsWorld* w) {
  w->global_listener = new MyGlobalListener();
  w->environment->add_listener_object_global(w->global_listener);
}
void get_stats(sPhysicsWorld* w, int* active_bodies, double* time) {
  check_nullptr(w);

  if (active_bodies) *active_bodies = w->global_listener->active_bodies.getSize();
  if (time) *time = w->environment->get_current_time().get_seconds();
}
void do_update_all(sPhysicsWorld* w) {
  if (w && w->environment)
  {
    IVP_U_Point position;
    IVP_U_Quat orientation;
    auto ptr = w->global_listener->active_bodies.begin;
    if (w->global_listener->active_bodies.getSize() == 0)
      return;

    while (ptr)
    {
      if (!ptr->is_frozen && !ptr->object->get_core()->physical_unmoveable)
      {
        ptr->object->get_quat_world_f_object_AT(&orientation, &position);
        ptr->updated_pos[0] = position.k[0];
        ptr->updated_pos[1] = position.k[1];
        ptr->updated_pos[2] = position.k[2];
        //ConvertPointToUnity(&ptr->updated_pos[0], &ptr->updated_pos[1], &ptr->updated_pos[2]);
        ptr->updated_rot[0] = orientation.x;
        ptr->updated_rot[1] = orientation.y;
        ptr->updated_rot[2] = orientation.z;
        ptr->updated_rot[3] = orientation.w;
        //ConvertQuatToUnity(&ptr->updated_rot[0], &ptr->updated_rot[1], &ptr->updated_rot[2], &ptr->updated_rot[3]);
      }
      ptr = ptr->next;
    }
  }
}
void do_update_all_physics_contact_detection(sPhysicsWorld* w) {
  if (w && w->environment)
  {
    auto ptr = w->global_listener->active_bodies.begin;
    if (w->global_listener->active_bodies.getSize() == 0)
      return;

    while (ptr)
    {
      if (ptr->contract_event_callback) {
        do_physics_contact_detection(ptr);
      }
      ptr = ptr->next;
    }
  }
}

#pragma endregion

#pragma region GroupFilter

class GroupFilter : public IVP_Collision_Filter
{
public:
  GroupFilter() {
    memset(m_collisionLookupTable, 0xffffffff, sizeof(m_collisionLookupTable));
  }
  ~GroupFilter() {}

  virtual IVP_BOOL check_objects_for_collision_detection(IVP_Real_Object* object0, IVP_Real_Object* object1) {
    auto body1 = ((sPhysicsBody*)object0->client_data);
    auto body2 = ((sPhysicsBody*)object1->client_data);
    if (body1 && body2)
      return (IVP_BOOL)isCollisionEnabled(body1->filter_info, body2->filter_info);
    return IVP_FALSE;
  }
  virtual void environment_will_be_deleted(IVP_Environment* environment) {}

  //Enable the collision between layer layerA and layerB.
  void enableCollisionsBetween(int layerA, int layerB)
  {
    sassert(0 <= layerA && layerA < 32);
    sassert(0 <= layerB && layerB < 32);

    m_collisionLookupTable[layerA] |= (sUint)(1 << layerB);
    m_collisionLookupTable[layerB] |= (sUint)(1 << layerA);
  }
  //Disables collisions between the layers A and B.
  void disableCollisionsBetween(int layerA, int layerB)
  {
    sassert(0 <= layerA && layerA < 32);
    sassert(0 <= layerB && layerB < 32);
    sassert2(layerA > 0, "You are not allowed to disable collision of layer 0");
    sassert2(layerB > 0, "You are not allowed to disable collision of layer 0");

    m_collisionLookupTable[layerA] &= (sUint)(~(1 << layerB));
    m_collisionLookupTable[layerB] &= (sUint)(~(1 << layerA));
  }
  //Enables collisions between the specified layers.
  //layerBitsA and layerBitsB are bitfields, one bit for every layer.
  //e.g., to enable collisions between one layer and all other layers,
  //call enableCollisionsUsingBitfield( 1<< myLayer, 0xfffffffe)
  void enableCollisionsUsingBitfield(sUint layerBitsA, sUint layerBitsB)
  {
    sassert2((layerBitsA | layerBitsB) != 0, "layer bits not set");
    for (int i = 0; i < 32; i++)
    {
      int b = 1 << i;
      if (b & layerBitsA)
      {
        m_collisionLookupTable[i] |= layerBitsB;
      }
      if (b & layerBitsB)
      {
        m_collisionLookupTable[i] |= layerBitsA;
      }
    }
  }
  //Disables collisions between the specified collision layers.
  //See enableCollisionsUsingBitfield for how to use bitfields.
  void disableCollisionsUsingBitfield(sUint layerBitsA, sUint layerBitsB)
  {
    sassert2((layerBitsA | layerBitsB) != 0, "layer bits not set");
    sassert2((layerBitsA & 1) == 0, "You are not allowed to disable collision of layer 0");
    sassert2((layerBitsB & 1) == 0, "You are not allowed to disable collision of layer 0");
    for (int i = 0; i < 32; i++)
    {
      int b = 1 << i;
      if (b & layerBitsA)
      {
        m_collisionLookupTable[i] &= ~layerBitsB;
      }
      if (b & layerBitsB)
      {
        m_collisionLookupTable[i] &= ~layerBitsA;
      }
    }
  }
  //Combine the layer and systemGroup information into one 32 bit integer.
  //This resulting collision filter info can be used in entities and hkEntityCinfo.
  static inline sUint calcFilterInfo(int layer, int systemGroup = 0, int subSystemId = 0, int subSystemDontCollideWith = 0)
  {
    sassert_return(layer >= 0 && layer < 32, 0);
    sassert_return(subSystemId >= 0 && subSystemId < 32, 0);
    sassert_return(subSystemDontCollideWith >= 0 && subSystemDontCollideWith < 32, 0);
    sassert_return(systemGroup >= 0 && systemGroup < 0x10000, 0);

    return (sUint)((subSystemId << 5) | (subSystemDontCollideWith << 10) | (systemGroup << 16) | layer);
  }
  //Extract the layer from a given filterInfo.
  static inline int getLayerFromFilterInfo(sUint filterInfo)
  {
    return filterInfo & 0x1f;
  }
  //Returns the filter info provided with the layer replaced by newLayer.
  static inline int setLayer(sUint filterInfo, int newLayer)
  {
    sUint collisionLayerMask = 0xffffffff - 0x1f;
    return newLayer + (collisionLayerMask & filterInfo);
  }
  //Extract the system group from a given filterInfo.
  static inline int getSystemGroupFromFilterInfo(sUint filterInfo)
  {
    return filterInfo >> 16;
  }
  //Extract the subsystem id from a given filterInfo.
  static inline int getSubSystemIdFromFilterInfo(sUint filterInfo)
  {
    return (filterInfo >> 5) & 0x1f;
  }
  //Extract the subSystemDontCollideWith from a given filterInfo.
  static inline int getSubSystemDontCollideWithFromFilterInfo(sUint filterInfo)
  {
    return (filterInfo >> 10) & 0x1f;
  }
  //Creates a new unique identifier for system groups (maximum 65k).
  inline int getNewSystemGroup()
  {
    return ++m_nextFreeSystemGroup;
  }
  //The actual filter implementation between two sUint values. All the other isCollisionEnabled functions call this method.
  //Returns true if the objects are enabled to collide, based on their collision groups.
  sBool isCollisionEnabled(sUint infoA, sUint infoB) const {
    // If the objects are in the same system group, but not system group 0,
    // then the decision of whether to collide is based exclusively on the 
    // objects' SubSystemId and SubSystemDontCollideWith.
    // Otherwise, the decision is based exclusively on the objects' layers.

    sUint zeroIfSameSystemGroup = (infoA ^ infoB) & 0xffff0000;

    // check for identical system groups
    if (zeroIfSameSystemGroup == 0)
    {
      // check whether system group was set (nonzero)
      if ((infoA & 0xffff0000) != 0)
      {
        // check whether we allow collisions
        int idA = (infoA >> 5) & 0x1f;
        int dontCollideB = (infoB >> 10) & 0x1f;
        if (idA == dontCollideB)
        {
          return false;
        }

        int idB = (infoB >> 5) & 0x1f;
        int dontCollideA = (infoA >> 10) & 0x1f;
        if (idB == dontCollideA)
        {
          return false;
        }
        return true;
      }
    }

    if (infoA == 0 && infoB == 0)
      return true;

    // use the layers to decide
    sUint f = 0x1f;
    sUint layerBitsA = m_collisionLookupTable[infoA & f];
    sUint layerBitsB = (sUint)(1 << (infoB & f));

    return 0 != (layerBitsA & layerBitsB);
  }
private:
  sUint m_collisionLookupTable[32];
  int m_nextFreeSystemGroup = 0;
};

#pragma endregion

#pragma region Coll Detection

sColDectInfo* physics_coll_detection(sPhysicsBody* body, int col_id, float min_speed, float max_speed, float sleep_afterwards, float speed_threadhold) {
  check_nullptr_return(body, sFalse);

  if (body->is_phantom) {
    callback_err("Body 0x%X is phantom can not create coll detection", (IVP_PTR)body);
    return nullptr;
  }

  sColDectInfo* info = new sColDectInfo();
  info->col_id = col_id;
  info->min_speed = min_speed;
  info->sleep_afterwards = sleep_afterwards;
  info->max_speed = max_speed;
  info->body = body;
  info->secrity_check = 123;

  body->col_dections.add(info);

  return info;
}
sContractDectInfo* physics_contract_detection(sPhysicsBody* body, int col_id, float time_delay_start, float time_delay_end) {
  check_nullptr_return(body, sFalse);

  if (body->is_phantom) {
    callback_err("Body 0x%X is phantom can not create contract detection", (IVP_PTR)body);
    return nullptr;
  }

  sContractDectInfo* info = new sContractDectInfo();
  info->col_id = col_id;
  info->time_delay_start = time_delay_start;
  info->time_delay_end = time_delay_end;
  info->body = body;
  info->secrity_check = 123;

  body->contract_dections.add(info);

  return info;
}
void destroy_physics_coll_detection(sColDectInfo*info) {
  check_nullptr(info);
  if (info->secrity_check == 123) {
    info->body->col_dections.remove(info);
    delete info;
  }
}
void destroy_all_physics_coll_detection(sPhysicsBody* body) {
  check_nullptr(body);
  auto ptr = body->col_dections.begin;
  while (ptr)
  {
    auto next = ptr->next;
    delete ptr;
    ptr = next;
  }
  body->col_dections.clear();
}
void destroy_physics_contract_detection(sContractDectInfo* info) {
  check_nullptr(info);
  if (info->secrity_check == 123) {
    info->body->contract_dections.remove(info);
    delete info;
  }
}
void destroy_all_physics_contract_detection(sPhysicsBody* body) {
  check_nullptr(body);
  auto ptr = body->contract_dections.begin;
  while (ptr)
  {
    auto next = ptr->next;
    delete ptr;
    ptr = next;
  }
  body->contract_dections.clear();
}

void do_physics_coll_detection_handler(sPhysicsBody* body, sPhysicsBody* other, IVP_U_Float_Point speed) {
  //碰撞处理工具
  auto ptr = body->col_dections.begin;
  if (ptr) {
    auto now_time = body->own_world->environment->get_current_time().get_seconds();
    while (ptr)
    {
      auto this_col_id = ptr->col_id;
      if (other->col_id == this_col_id) {//当前碰撞层
        auto now_speed = speed.fast_real_length();
        //小于阈值，则不发出事件, 时间过短，不发出
        if (now_time - ptr->last_time > ptr->sleep_afterwards 
          && now_speed > ptr->min_speed 
          && abs(now_speed - body->last_speed) >= ptr->speed_threadhold) {
           ptr->last_time = now_time;
           body->contract_event_callback(body, ptr->col_id, 1, (now_speed - ptr->min_speed) / (ptr->max_speed - ptr->min_speed), 0);
        }
      }
      ptr = ptr->next;
    }
  }
}
float do_physics_contact_detection(sPhysicsBody* body) {
  //接触处理工具
  check_nullptr_return(body, sFalse);

  body->last_speed = physics_get_speed(body);

  auto now_time = body->own_world->environment->get_current_time().get_seconds();
  auto ptr = body->contract_dections.begin;
  while (ptr)
  {
    //时间过短
    if (ptr->is_contract)  
    {
      if (now_time - ptr->last_contract_time < ptr->time_delay_end) {
        ptr = ptr->next;
        continue;
      }      
    }
    else 
    {
      if (now_time - ptr->last_contract_start_time < ptr->time_delay_start) {
        ptr = ptr->next;
        continue;
      }
    }

    //检查碰撞物体
    int this_col_id = ptr->col_id, cur_test_id = 0; bool is_coll = false;
    auto* fr_esynapse = body->object->get_first_exact_synapse();
    IVP_Synapse_Friction* fr_synapse = body->object->get_first_friction_synapse();
    IVP_Contact_Point* fr_mindist;
    IVP_Real_Object* other;
    while (fr_synapse) {
      fr_mindist = fr_synapse->get_contact_point();
      if (fr_mindist) {
        other = fr_mindist->get_synapse(0)->get_object();
        if (other == body->object)
          other = fr_mindist->get_synapse(1)->get_object();
        
        auto obj = ((sPhysicsBody*)other->client_data);
        if (obj) {
          cur_test_id = obj->col_id;
          if (cur_test_id == this_col_id) {
            is_coll = true;
            break;
          }
        }
      }
      fr_synapse = fr_synapse->get_next();
    }


    //状态变更时
    if (is_coll != ptr->is_contract) 
    {
      //设置开始时间
      if (is_coll && !ptr->is_contract && !ptr->is_contract_start_check) {
        ptr->last_contract_start_time = now_time;
        ptr->is_contract_start_check = true;
        body->contract_event_callback(body, ptr->col_id, 2, 0, sTrue);
        return sFalse;
      }

      ptr->is_contract = is_coll;
      body->contract_event_callback(body, ptr->col_id, 2, 0, is_coll);

      ptr->is_contract_start_check = false;
      ptr->last_contract_time = now_time;
    }
    if (ptr->is_contract_start_check && !is_coll) {
      ptr->is_contract_start_check = false;
      body->contract_event_callback(body, ptr->col_id, 2, 0, false);
    }

    ptr = ptr->next;
  }
  return sFalse;
}

#pragma endregion

#pragma region BaseCreationAndDestruction

sPhysicsWorld* create_environment(IVP_U_Point* gravity, float suimRate, int layerMask, int* layerToMask, EventCallback eventCallback)
{
  IVP_Environment_Manager* pEnvManager;
  pEnvManager = IVP_Environment_Manager::get_environment_manager();

  IVP_Application_Environment appl_env;

  if (functions[255] == (void*)1)
    appl_env.collision_filter = nullptr;
  else
    appl_env.collision_filter = new GroupFilter();
  auto m_penvironment = pEnvManager->create_environment(&appl_env, "ballance", 0x1221f635);

  if (gravity)
    m_penvironment->set_gravity(gravity);
  else {
    IVP_U_Point _gravity(0, -9.81, 0);
    callback_info("Use default gravity");
    m_penvironment->set_gravity(&_gravity);
  }
  if (suimRate >= IVP_MIN_DELTA_PSI_TIME && suimRate < IVP_MAX_DELTA_PSI_TIME)
    m_penvironment->set_delta_PSI_time(suimRate);

  sPhysicsWorld* world = new sPhysicsWorld();
  world->environment = m_penvironment;
  world->group_filter = (GroupFilter*)appl_env.collision_filter;

  if (eventCallback) {
    if(::eventCallback == nullptr)
      ::eventCallback = eventCallback;
    world->eventCallback = eventCallback;
  }

  //Set layer masks
  for (int i = 1; i < 32; i++) {
    int mask = layerToMask[i];
    for (int j = i; j < 32; j++) {
      if ((mask >> j) & 0x01)
        world->group_filter->enableCollisionsBetween(i, j);
      else
        world->group_filter->disableCollisionsBetween(i, j);
    }
  }

  set_env_listeners(world);

  world->securityCheck = security_check_number;
  return world;
}
void destroy_environment(sPhysicsWorld* world)
{
  check_nullptr(world);

  if (world->securityCheck != security_check_number) {
    callback_err("World delete twice!");
    return;
  }

  if (world->eventCallback && eventCallback == world->eventCallback)
    eventCallback = nullptr;

  world->environment->remove_listener_object_global(world->global_listener);
  delete world->environment;

  delete world->group_filter;
  delete world->global_listener;
  delete world;
}

void environment_simulate_dtime(sPhysicsWorld* world, float dtime) {
  if (world && world->environment && !world->disableSim) {
    if (world->securityCheck != security_check_number) {
      callback_err("World security check failed!");
      return;
    }
#if DEBUG
    __try
    {
#endif
      world->environment->simulate_dtime(dtime);
#if DEBUG
    }
    __except (EXCEPTION_EXECUTE_HANDLER)
    {
      world->disableSim = true;
      callback_err("Env EXCEPTION ! Simulate disabled");
    }
#endif
  }
}
void environment_simulate_variable_time_step(sPhysicsWorld* world, float dtime) {
  if (world && world->environment && !world->disableSim) {
    if (world->securityCheck != security_check_number) {
      callback_err("World security check failed!");
      return;
    }
#if DEBUG
    __try
    {
#endif
      world->environment->simulate_variable_time_step(dtime);
#if DEBUG
    }
    __except (EXCEPTION_EXECUTE_HANDLER)
    {
      world->disableSim = true;
      callback_err("Env EXCEPTION ! Simulate disabled");
    }
#endif
  }
}
void environment_simulate_until(sPhysicsWorld* world, float time) {
  if (world && world->environment && !world->disableSim) {
    if (world->securityCheck != security_check_number) {
      callback_err("World security check failed!");
      return;
    }
#if DEBUG
    __try
    {
#endif
      world->environment->simulate_until(time);
#if DEBUG
    }
    __except (EXCEPTION_EXECUTE_HANDLER)
    {
      world->disableSim = true;
      callback_err("Env EXCEPTION ! Simulate disabled");
    }
#endif
  }
}
void environment_reset_time(sPhysicsWorld* world) {
  if (world && world->environment)
    world->environment->reset_time();
}
int environment_new_system_group(sPhysicsWorld* world) {
  check_nullptr_return(world, 0);
  return world->group_filter->getNewSystemGroup();
}
void environment_set_collision_layer_masks(sPhysicsWorld* world, unsigned int layerId, unsigned int toMask, int enable) {

  check_nullptr(world);

  if (enable)
    world->group_filter->enableCollisionsUsingBitfield(1 << layerId, toMask);
  else
    world->group_filter->disableCollisionsUsingBitfield(1 << layerId, toMask);
}

sPointBuffer* create_points_buffer(int point_count, float* pt) {
  sPointBuffer* b = new sPointBuffer();
  for (int i = 0; i < point_count; i++) {
    ConvertPointToIVP(&pt[i * 3], & pt[i * 3 + 1], & pt[i * 3 + 2]);
    auto p = new IVP_U_Point(pt[i * 3], pt[i * 3 + 1], pt[i * 3 + 2]);
    b->points.add(p);
  }
  return b;
}
sPointBuffer* create_polyhedron(int point_count, int trangle_count, float* points, int* trangles) {
  sPointBuffer* b = new sPointBuffer();

  b->pointsBuffer = (float*)malloc(point_count * 3 * sizeof(float));
  b->pointsCount = (sUnit)point_count;
  memcpy(b->pointsBuffer, points, point_count * 3 * sizeof(float));

  b->trangles = (sUnit*)malloc(trangle_count * sizeof(sUnit));
  b->trangleCount = (sUnit)trangle_count;
  memcpy(b->trangles, trangles, trangle_count * sizeof(sUnit));

  return b;
}
void delete_points_buffer(sPointBuffer* b) {
  if (b) {
    for (int i = b->points.len() - 1; i >= 0; i--)
      delete b->points.element_at(i);
    if (b->trangles)
      delete b->trangles;
    if (b->pointsBuffer)
      delete b->pointsBuffer;

    delete b;
  }
}

void add_surface(sPhysicsWorld* world, const char* name, sSurface* surface) {
  world->createdSurfaces.insert(std::pair<sUint, sSurface*>(rs_hash(name), surface));
}
IVP_SurfaceManager* get_surface_by_name(sPhysicsWorld* world, const char* name) {
  auto it = world->createdSurfaces.find(rs_hash(name));
  if (it != world->createdSurfaces.end()) {
    auto p = *it;
    return p.second->surface_manager;
  }
  return nullptr;
}
sBool surface_exist_by_name(sPhysicsWorld* world, const char* name) {
  return world->createdSurfaces.find(rs_hash(name)) != world->createdSurfaces.end();
}
void delete_all_surfaces(sPhysicsWorld* world) {
  int len = 0;
  for (auto it = world->createdSurfaces.begin(); it != world->createdSurfaces.end(); it++) {
    delete (*it).second;
    len++;
  }
  world->createdSurfaces.clear();
  callback_info("%d surfaces deleted", len);
}

int physics_get_id(sPhysicsBody* body) {
  check_nullptr_return(body, 0);
  return body->id;
}
void physics_set_name(sPhysicsBody* body, char* name) {
  strcpy(body->name, name);
}
void physics_set_layer(sPhysicsWorld* world, sPhysicsBody* body, int layer, int systemGroup, int subSystemId, int subSystemDontCollideWith) {
  check_nullptr(body);
  body->filter_info = world->group_filter->calcFilterInfo(layer, systemGroup, subSystemId, subSystemDontCollideWith);
}

void physics_set_collision_listener(sPhysicsBody* body, CollisionEventCallback callback, float collision_call_sleep, FrictionEventCallback friction_event_callback) {
  check_nullptr(body);

  body->collision_event_callback = callback;
  body->friction_event_callback = friction_event_callback;

  if (body->collision_listener == nullptr) {
    body->collision_listener = new MyCollisionListener(body);
    body->collision_listener->collision_call_sleep = collision_call_sleep;
    body->object->add_listener_collision(body->collision_listener);
  }
}
void physics_remove_collision_listener(sPhysicsBody* body) {
  check_nullptr(body);

  body->collision_event_callback = nullptr;
  body->friction_event_callback = nullptr;

  if (body->collision_listener) {
    body->object->remove_listener_collision(body->collision_listener);
    delete body->collision_listener;
    body->collision_listener = nullptr;
  }
}

void physics_set_contract_listener(sPhysicsBody* body, ContractEventCallback callback) {
  check_nullptr(body);
  body->contract_event_callback = callback;
}
void physics_remove_contract_listener(sPhysicsBody* body) {
  check_nullptr(body);
  body->contract_event_callback = nullptr;
}

sBool physics_is_contact(sPhysicsBody* body, sPhysicsBody* other) {
  check_nullptr_return(body, sFalse);
  check_nullptr_return(other, sFalse);

  if (!body->is_phantom) {
    IVP_Synapse_Friction* fr_synapse = body->object->get_first_friction_synapse(), * next_syn;
    while (fr_synapse) {
      next_syn = fr_synapse->get_next();
      if (((sPhysicsBody*)fr_synapse->get_object()->client_data) == body)
        return sTrue;
    }
  }
  return sFalse;
}
sBool physics_is_motion_enabled(sPhysicsBody* body)
{
  check_nullptr_return(body, sFalse);

  return body->object->get_core()->pinned ? sFalse : sTrue;
}
sBool physics_is_controlling(sPhysicsBody* body, IVP_Controller* pController)
{
  IVP_Core* pCore = body->object->get_core();
  for (int i = 0; i < pCore->controllers_of_core.len(); i++)
  {
    // already controlling this core?
    if (pCore->controllers_of_core.element_at(i) == pController)
      return true;
  }
  return false;
}
sBool physics_is_fixed(sPhysicsBody* body)
{
  check_nullptr_return(body, sFalse);

  if (body->object->get_core()->physical_unmoveable)
    return sTrue;
  return sFalse;
}
sBool physics_is_gravity_enabled(sPhysicsBody* body)
{
  check_nullptr_return(body, sFalse);
  if (!physics_is_fixed(body))
    return physics_is_controlling(body, body->object->get_core()->environment->get_gravity_controller());
  return sFalse;
}
sBool physics_is_phantom(sPhysicsBody* body)
{
  check_nullptr_return(body, sFalse);
  return body->is_phantom;
}
void physics_enable_gravity(sPhysicsBody* body, sBool enable) {
  if (physics_is_fixed(body))
    return;
  bool isEnabled = physics_is_gravity_enabled(body);
  if ((bool)enable == isEnabled)
    return;

  IVP_Controller* pGravity = body->object->get_core()->environment->get_gravity_controller();
  if (enable)
    body->object->get_core()->add_core_controller(pGravity);
  else
    body->object->get_core()->rem_core_controller(pGravity);
}
void physics_enable_collision_detection(sPhysicsBody* body) {
  check_nullptr(body);
  body->object->enable_collision_detection();
}
void physics_disable_collision_detection(sPhysicsBody* body) {
  check_nullptr(body);
  body->object->enable_collision_detection(IVP_FALSE);
}
void physics_recheck_collision_filter(sPhysicsBody* body) {
  check_nullptr(body);
  body->object->recheck_collision_filter();
}
void physics_enable_motion(sPhysicsBody* body, sBool enable)
{
  check_nullptr(body);

  bool isMoveable = physics_is_motion_enabled(body);
  if (isMoveable == (bool)enable)
    return;

  body->object->set_pinned(enable ? IVP_FALSE : IVP_TRUE);
  physics_recheck_collision_filter(body);
}

sBool physics_freeze(sPhysicsBody* body) {
  check_nullptr_return(body, sFalse);
  return (sBool)body->object->disable_simulation();
}
void physics_wakeup(sPhysicsBody* body) {
  check_nullptr(body);
  if (physics_is_fixed(body) == 0)
    body->object->ensure_in_simulation();
}


int idPool = 0;

sPhysicsBody* physicalize(sPhysicsWorld* world, const char* name, int layer, int systemGroup, int subSystemId, int subSystemDontCollideWith, float mass, float friction, float elasticity,
  float linear_speed_damp, float rot_speed_damp, float ball_radius, sBool use_ball, sBool enable_convex_hull, sBool auto_mass_center, sBool enable_collision, sBool start_frozen, sBool physical_unmoveable,
  IVP_U_Point* position, IVP_U_Point* shfit_mass_center, IVP_U_Quat* rotation,
  sBool use_exists_surface, const char* surface_name,
  int convex_count, sPointBuffer** convex_data, int concave_count, sPointBuffer** concave_data,
  float extra_radius, int col_id)
{
  if (!world || !world->environment) {
    callback_err("World is nullptr!");
    return nullptr;
  }

  sPhysicsBody* body = new sPhysicsBody();
  body->material = new IVP_Material_Simple(friction, elasticity);
  body->layer = layer;
  body->filter_info = world->group_filter->calcFilterInfo((layer < 0 || layer > 31) ? 0 : layer, systemGroup, subSystemId, subSystemDontCollideWith);
  if (strlen(name) >= 31) strncpy(body->name, name, 31);
  else strcpy(body->name, name);

  IVP_Template_Real_Object template_real_object;
  template_real_object.mass = mass;
  template_real_object.material = body->material;
  template_real_object.set_name(name);
  template_real_object.physical_unmoveable = (IVP_BOOL)physical_unmoveable;
  template_real_object.client_data = body;
  template_real_object.speed_damp_factor = linear_speed_damp;
  template_real_object.rot_speed_damp_factor = IVP_U_Point(rot_speed_damp, rot_speed_damp, rot_speed_damp);
  template_real_object.extra_radius = extra_radius;

  if (!auto_mass_center && shfit_mass_center) {
    //Custom mass_center
    IVP_U_Matrix m;
    m.init();
    m.vv.set(shfit_mass_center);
    template_real_object.mass_center_override = &m;
  }

  if (use_ball) {
    //Create ball
    IVP_Template_Ball template_ball;
    template_ball.radius = ball_radius; // the sphere's radius (unit: m)

    body->object = world->environment->create_ball(&template_ball, &template_real_object, rotation, position);
  }
  else {
    //Use mesh
    IVP_SurfaceManager* surface_manager = nullptr;
              
    if (use_exists_surface && surface_name)
      surface_manager = get_surface_by_name(world, surface_name);//use exists surface

    if (surface_manager == nullptr) {
      //Create Surface
      IVP_Compact_Surface* compact_surface = nullptr;

      if (convex_count == 1 && concave_count == 0) {
        compact_surface = IVP_SurfaceBuilder_Pointsoup::convert_pointsoup_to_compact_surface(&convex_data[0]->points);
      }
      else if (convex_count == 0 && concave_count > 0) {

        IVP_SurfaceBuilder_Ledge_Soup ledge_soup;
        for (int i = 0; i < concave_count; i++) {

          //copy point buffers
          sPointBuffer* concave = concave_data[i];
          IVP_U_BigVector<IVP_U_Point> points;

          //load polyhedron
          for (sUint i = 0; i < concave->pointsCount * 3; i += 3)
            points.add(
              new IVP_U_Point(
                concave->pointsBuffer[i],
                concave->pointsBuffer[i + 1],
                concave->pointsBuffer[i + 2]
              )

            );
          for (sUint i = 0; i < concave->trangleCount; i += 3) {
            ledge_soup.insert_ledge(IVP_SurfaceBuilder_Pointsoup::convert_triangle_to_compace_ledge(
              points.element_at(concave->trangles[i + 2]),
              points.element_at(concave->trangles[i + 1]),
              points.element_at(concave->trangles[i + 0])
            ));
          }

          for (int i = points.len() - 1; i >= 0; i--)
            delete points.element_at(i);
        }

        IVP_Template_Surbuild_LedgeSoup tls;
        tls.build_root_convex_hull = (IVP_BOOL)enable_convex_hull;
        tls.merge_points = IVP_SLMP_MERGE_AND_REALLOCATE;                                  

        compact_surface = ledge_soup.compile(&tls);
      }
      else if (convex_count > 0 && concave_count == 0) {
        IVP_SurfaceBuilder_Ledge_Soup ledge_soup;

        for (int i = 0; i < convex_count; i++)
          ledge_soup.insert_ledge(IVP_SurfaceBuilder_Pointsoup::convert_pointsoup_to_compact_ledge(&convex_data[i]->points));

        IVP_Template_Surbuild_LedgeSoup tls;
        tls.build_root_convex_hull = (IVP_BOOL)enable_convex_hull;
        tls.merge_points = IVP_SLMP_MERGE_AND_REALLOCATE;

        compact_surface = ledge_soup.compile(&tls);
      }

      if (compact_surface == nullptr) {
        callback_err("Create surface \"%s\" for \"%s\" failed!", surface_name, name);
        delete body->material;
        delete body;
        return nullptr;
      }

      surface_manager = new IVP_SurfaceManager_Polygon(compact_surface);

      //Add surface to temp
      sSurface *s = new sSurface();
      if(strlen(surface_name) >= 31) strncpy(s->name, surface_name, 31);
      else strcpy(s->name, surface_name);
      s->surface_manager = surface_manager;
      s->compact_surface = compact_surface;

      body->own_surface = s;

      if (surface_name) {
        if (get_surface_by_name(world, surface_name) != nullptr)
          callback_warn("The surface \"%s\" alreday exists!", surface_name);
        else {
          body->own_surface = nullptr;
          add_surface(world, surface_name, s);
        }
      }
    }

    body->object = world->environment->create_polygon(surface_manager, &template_real_object, rotation, position);
  }
   body->own_world = world;

  if (enable_collision)
    physics_enable_collision_detection(body);
  if (!physical_unmoveable && !start_frozen)
    physics_wakeup(body);

  body->col_id = col_id;
  body->securityCheck = security_check_number;
  body->id = idPool++;
  if (idPool > 0xfffe) idPool = 0;

  return body;
}
void unphysicalize(sPhysicsWorld* world, sPhysicsBody* body, sBool silently) {
  check_nullptr(world);
  check_nullptr(body);

  if (body->securityCheck != security_check_number) {
    callback_err("Object %ld delete twice!", body);
    return;
  }

  if (silently)
    body->object->delete_silently();
  else
    body->object->delete_and_check_vicinity();

  destroy_all_physics_coll_detection(body);
  destroy_all_physics_contract_detection(body);

  delete body->material;
  if (body->own_surface)
    delete body->own_surface;
  if (body->phantom_listener)
    delete body->phantom_listener;
  if (body->collision_listener)
    delete body->collision_listener;
  delete body;
}

void physics_beam_object_to_new_position(sPhysicsBody* body, IVP_U_Quat* rotation, IVP_U_Point* pos, sBool optimize_for_repeated_calls) {
  check_nullptr(body);
  check_nullptr(rotation);
  check_nullptr(pos);
  body->object->beam_object_to_new_position(rotation, pos, (IVP_BOOL)optimize_for_repeated_calls);
}
float physics_get_speed(sPhysicsBody* body) {
  check_nullptr_return(body, 0);
  return body->object->get_geom_center_speed();
}
void physics_get_speed_vec(sPhysicsBody* body, IVP_U_Point* speed_ws_out) {
  check_nullptr(body);

  body->object->get_geom_center_speed_vec(speed_ws_out);
}
void physics_get_rot_speed(sPhysicsBody* body, IVP_U_Point* normized_axis_cs_out) {
  check_nullptr(body);

  IVP_U_Float_Point normized_axis_cs;
  body->object->get_core()->get_rot_speed_cs(&normized_axis_cs);
  normized_axis_cs_out->set(&normized_axis_cs);
}

void physics_change_mass(sPhysicsBody* body, float mass) {
  check_nullptr(body);
  body->object->change_mass(mass);
}
void physics_change_unmovable_flag(sPhysicsBody* body, sBool unmovable_flag) {
  check_nullptr(body);
  if(body->object->get_core()->physical_unmoveable != unmovable_flag)
    body->object->change_unmovable_flag((IVP_BOOL)unmovable_flag);
}

void physics_impluse(sPhysicsBody* body, IVP_U_Point* pos_ws, IVP_U_Point* impulse_ws) {
  check_nullptr(body);

  IVP_U_Float_Point _impulse_ws; _impulse_ws.set(impulse_ws);
  body->object->async_push_object_ws(pos_ws, &_impulse_ws);
}
void physics_torque(sPhysicsBody* body, IVP_U_Point* rotation_vec) {
  check_nullptr(body);

  IVP_U_Float_Point _rotation_vec; _rotation_vec.set(rotation_vec);
  body->object->async_add_rot_speed_object_cs(&_rotation_vec);
}
void physics_add_speed(sPhysicsBody* body, IVP_U_Point* speed_ws) {
  check_nullptr(body);

  IVP_U_Float_Point _speed_ws; _speed_ws.set(speed_ws);
  body->object->async_add_speed_object_ws(&_speed_ws);
}
void physics_set_col_id(sPhysicsBody* body, int col_id) {
  check_nullptr(body);
  body->col_id = col_id;
}

void physics_transform_position_to_world_coords(sPhysicsBody* body, IVP_U_Point* pos_cs, IVP_U_Point* out_ws) {
  check_nullptr(body);

  IVP_U_Float_Point pt; pt.set(pos_cs);
  IVP_Cache_Object* cache = body->object->get_cache_object();
  cache->transform_position_to_world_coords(&pt, out_ws);
  cache->remove_reference();
}
void physics_transform_position_to_object_coords(sPhysicsBody* body, IVP_U_Point* pos_ws, IVP_U_Point* out_os) {
  check_nullptr(body);

  IVP_Cache_Object* cache = body->object->get_cache_object();
  cache->transform_position_to_object_coords(pos_ws, out_os);
  cache->remove_reference();
}
void physics_transform_vector_to_object_coords(sPhysicsBody* body, IVP_U_Point* vec_ws, IVP_U_Point* out_os) {
  check_nullptr(body);

  IVP_Cache_Object* cache = body->object->get_cache_object();
  cache->transform_vector_to_object_coords(vec_ws, out_os);
  cache->remove_reference();
}
void physics_transform_vector_to_world_coords(sPhysicsBody* body, IVP_U_Point* vec_cs, IVP_U_Point* out_ws) {
  check_nullptr(body);

  IVP_Cache_Object* cache = body->object->get_cache_object();
  cache->transform_vector_to_world_coords(vec_cs, out_ws);
  cache->remove_reference();
}

void physics_convert_to_phantom(sPhysicsBody* body, float extra_radius, PhantomEventCallback callback) {
  check_nullptr(body);

  if (body->is_phantom) {
    callback_warn("object %d is already phantom", body->id);
    return;
  }

  IVP_Template_Phantom tpl;
  tpl.exit_policy_extra_radius = extra_radius;
  tpl.dont_check_for_unmoveables = IVP_TRUE;
  tpl.manage_intruding_objects = IVP_TRUE;
  body->object->convert_to_phantom(&tpl);
  body->phantom_event_callback = callback;
  if (callback) {
    body->phantom_listener = new MyPhantomListener(body);
    body->object->get_controller_phantom()->add_listener_phantom(body->phantom_listener);
  }
}
sBool physics_is_inside_phantom(sPhysicsBody* body, sPhysicsBody* other) {
  check_nullptr_return(body, sFalse);
  check_nullptr_return(other, sFalse);
  sassert2_return(body->is_phantom, "object is not a phantom", sFalse);

  auto v = body->object->get_controller_phantom()->get_intruding_objects();
  return v->find_element(other->object) != nullptr;
}

sMotionController* create_motion_controller(sPhysicsBody* body, sPhysicsBody* targetBody, IVP_U_Point* max_translation_force, float max_torque, float force_factor, float damp_factor, float angular_damp_factor, float torque_factor) {

  check_nullptr_return(body, nullptr);
  check_nullptr_return(targetBody, nullptr);

  IVP_Template_Controller_Motion m;
  m.max_translation_force.set(max_translation_force);
  m.max_torque = max_torque;
  m.force_factor = force_factor;
  m.damp_factor = damp_factor;
  m.angular_damp_factor = angular_damp_factor;
  m.torque_factor = torque_factor;

  auto rs = new sMotionController();
  rs->controller_motion = new IVP_Controller_Motion(body->object, &m);
  rs->object = body->object;
  rs->body = body;

  return rs;
}
void destroy_motion_controller(sMotionController* controller) {
  check_nullptr(controller);
  delete controller->controller_motion;
  delete controller;
}
void motion_controller_set_target_pos(sMotionController* controller, IVP_U_Point* pos_ws) {
  check_nullptr(controller);
  controller->controller_motion->set_target_position_ws(pos_ws);
}
void motion_controller_set_max_translation_force(sMotionController* controller, IVP_U_Point* max_translation_force) {
  check_nullptr(controller);
  auto pt = IVP_U_Float_Point(max_translation_force);
  controller->controller_motion->set_max_translation_force(&pt);
}
void motion_controller_set_max_torque(sMotionController* controller, IVP_U_Point* max_torque) {
  check_nullptr(controller);
  auto pt = IVP_U_Float_Point(max_torque);
  controller->controller_motion->set_max_torque(&pt);
}
void motion_controller_set_force_factor(sMotionController* controller, float force_factor) {
  check_nullptr(controller);
  controller->controller_motion->set_force_factor(force_factor);
}
void motion_controller_set_damp_factor(sMotionController* controller, float damp_factor) {
  check_nullptr(controller);
  controller->controller_motion->set_damp_factor(damp_factor);
}
void motion_controller_set_angular_damp_factor(sMotionController* controller, float angular_damp_factor) {
  check_nullptr(controller);
  controller->controller_motion->set_angular_damp_factor(angular_damp_factor);
}
void motion_controller_set_torque_factor(sMotionController* controller, float torque_factor) {
  check_nullptr(controller);
  controller->controller_motion->set_torque_factor(torque_factor);
}


#pragma endregion

#pragma region Raycasting

void delete_raycast_result(sRayCastResult* ptr) {
  if (ptr) {
    if (ptr->hit_objects)
      delete ptr->hit_objects;
    if (ptr->hit_distances)
      delete ptr->hit_distances;
    delete ptr;
  }
}
sRayCastResult* raycasting(sPhysicsWorld* world, int flag, IVP_U_Point* start_point, IVP_U_Point* direction, float ray_length) {
  check_nullptr_return(world, nullptr);

  IVP_Ray_Solver_Template ray_template;
  ray_template.ray_length = ray_length; // [m]
  ray_template.ray_flags = (IVP_RAY_SOLVER_FLAGS)flag;
  ray_template.ray_start_point.set(start_point);
  ray_template.ray_normized_direction.set(direction);

  IVP_Ray_Solver_Min_Hash ray_solver(&ray_template);
  ray_solver.check_ray_against_all_objects_in_sim(world->environment);

  IVP_U_Min_Hash* all_hits = ray_solver.get_result_min_hash();
  int n_hit_objects = all_hits->counter;

  sRayCastResult* rs = new sRayCastResult();
  rs->n_hit_objects = n_hit_objects;

  if (n_hit_objects > 0) {
    rs->hit_objects = new sPhysicsBody * [n_hit_objects];
    rs->hit_distances = new float[n_hit_objects];

    IVP_Ray_Hit* hit;
    int i = 0;
    while ((hit = (IVP_Ray_Hit*)all_hits->find_min_elem())) {
      float nearest_hit_distance = all_hits->find_min_value();
      IVP_Ray_Hit* hit = (IVP_Ray_Hit*)all_hits->find_min_elem();
      IVP_Real_Object* object = hit->hit_real_object;

      all_hits->remove_min();
      rs->hit_objects[i] = (sPhysicsBody*)object->client_data;
      rs->hit_distances[i] = nearest_hit_distance;
    }
  }

  return rs;
}
int raycasting_one(sPhysicsWorld* world, IVP_U_Point* start_point, IVP_U_Point* direction, float ray_length, float* distance_out) {
  check_nullptr_return(world, 0);

  IVP_Ray_Solver_Template ray_template;
  ray_template.ray_length = ray_length; // [m]
  ray_template.ray_flags = IVP_RAY_SOLVER_ALL;
  ray_template.ray_start_point.set(start_point);
  ray_template.ray_normized_direction.set(direction);

  IVP_Ray_Solver_Min_Hash ray_solver(&ray_template);
  ray_solver.check_ray_against_all_objects_in_sim(world->environment);

  IVP_Ray_Hit* hit = (IVP_Ray_Hit*)ray_solver.get_result_min_hash()->find_min_elem();
  if (hit) {
    IVP_Real_Object* o = hit->hit_real_object;

    if (distance_out)
      *distance_out = hit->hit_distance;
    return ((sPhysicsBody*)o->client_data)->id;
  }
  return 0;
}
sBool raycasting_object(sPhysicsBody* object, IVP_U_Point* start_point, IVP_U_Point* direction, float ray_length, float* distance_out) {
  check_nullptr_return(object, 0);

  IVP_Ray_Solver_Template ray_template;
  ray_template.ray_length = ray_length; // [m]
  ray_template.ray_flags = IVP_RAY_SOLVER_ALL;
  ray_template.ray_start_point.set(start_point);
  ray_template.ray_normized_direction.set(direction);

  IVP_Ray_Solver_Min_Hash ray_solver(&ray_template);
  ray_solver.check_ray_against_object(object->object);

  IVP_Ray_Hit* hit = (IVP_Ray_Hit*)ray_solver.get_result_min_hash()->find_min_elem();
  if (hit) {
    IVP_Real_Object* o = hit->hit_real_object;

    if (distance_out)
      *distance_out = hit->hit_distance;
    return object->object == o;
  }
  return false;
}

#pragma endregion

#pragma region ConstraintAndHingeCreationAndDestruction

void constant_delete_hook(IVP_Constraint* s) {
  if (s->client_data) {
    ((sConstant*)s->client_data)->handle = nullptr;
    s->callback = nullptr;
  }
}
void set_constant_delete_hook(sConstant* rs) {
  rs->handle->client_data = rs;
  rs->handle->set_delete_callback(constant_delete_hook);
}
sConstant* set_physics_ball_joint(sPhysicsBody* body, sPhysicsBody* other, IVP_U_Point* joint_position_ws) {
  check_nullptr_return(body, nullptr);

  auto rs = new sConstant();

  IVP_Template_Constraint bs;
  bs.set_ballsocket_ws(body->object, joint_position_ws, other ? other->object : nullptr);
  rs->handle = body->object->get_environment()->create_constraint(&bs);
  set_constant_delete_hook(rs);
  return rs;
}
sConstant* set_physics_fixed_constraint(sPhysicsBody* body, sPhysicsBody* other) {
  check_nullptr_return(body, nullptr);

  auto rs = new sConstant();

  IVP_Template_Constraint bs;
  bs.set_fixed(body->object, other ? other->object : nullptr);
  rs->handle = body->object->get_environment()->create_constraint(&bs);
  set_constant_delete_hook(rs);
  return rs;
}
sConstant* set_physics_hinge(sPhysicsBody* body, sPhysicsBody* other, IVP_U_Point* anchor_ws, IVP_U_Point* free_axis_ws) {
  check_nullptr_return(body, nullptr);

  auto rs = new sConstant();

  IVP_Template_Constraint bs;
  bs.set_hinge_ws(body->object, anchor_ws, free_axis_ws, other ? other->object : nullptr);
  rs->handle = body->object->get_environment()->create_constraint(&bs);
  set_constant_delete_hook(rs);
  return rs;
}
sConstant* set_physics_constraint(sPhysicsBody* body, sPhysicsBody* other, float force_factor, float damp_factor, int translation_limit, IVP_U_Point* translation_freedom_min, IVP_U_Point* translation_freedom_max, int rotation_limit, IVP_U_Point* rotation_freedom_min, IVP_U_Point* rotation_freedom_max) {
  check_nullptr_return(body, nullptr);

  auto rs = new sConstant();

  IVP_Template_Constraint bs;

  bs.objectR = body->object;
  bs.objectA = other ? other->object : nullptr;

  if ((translation_limit & IVP_INDEX_X) == IVP_INDEX_X) {
    if (translation_freedom_min->k[0] == translation_freedom_max->k[0] && translation_freedom_max->k[0] == 0) bs.fix_translation_axis(IVP_INDEX_X);
    else bs.limit_translation_axis(IVP_INDEX_X, translation_freedom_min->k[0], translation_freedom_max->k[0]);
  }
  else
    bs.free_translation_axis(IVP_INDEX_X);

  if ((translation_limit & IVP_INDEX_Y) == IVP_INDEX_Y) {
    if (translation_freedom_min->k[1] == translation_freedom_max->k[1] && translation_freedom_max->k[1] == 0) bs.fix_translation_axis(IVP_INDEX_Y);
    else bs.limit_translation_axis(IVP_INDEX_Y, translation_freedom_min->k[1], translation_freedom_max->k[1]);
  }
  else
    bs.free_translation_axis(IVP_INDEX_Y);

  if ((translation_limit & IVP_INDEX_Z) == IVP_INDEX_Z) {
    if (translation_freedom_min->k[2] == translation_freedom_max->k[2] && translation_freedom_max->k[2] == 0) bs.fix_translation_axis(IVP_INDEX_Z);
    else bs.limit_translation_axis(IVP_INDEX_Z, translation_freedom_min->k[2], translation_freedom_max->k[2]);
  }
  else
    bs.free_translation_axis(IVP_INDEX_Z);

  if ((rotation_limit & IVP_INDEX_X) == IVP_INDEX_X) {
    if (rotation_freedom_min->k[0] == rotation_freedom_max->k[0] && rotation_freedom_max->k[0] == 0) bs.fix_rotation_axis(IVP_INDEX_X);
    else bs.limit_rotation_axis(IVP_INDEX_X, rotation_freedom_min->k[0], rotation_freedom_max->k[0]);
  }
  else
    bs.free_rotation_axis(IVP_INDEX_X);

  if ((rotation_limit & IVP_INDEX_Y) == IVP_INDEX_Y) {
    if (rotation_freedom_min->k[1] == rotation_freedom_max->k[1] && rotation_freedom_max->k[1] == 0) bs.fix_rotation_axis(IVP_INDEX_Y);
    else bs.limit_rotation_axis(IVP_INDEX_Y, rotation_freedom_min->k[1], rotation_freedom_max->k[1]);
  }
  else
    bs.free_rotation_axis(IVP_INDEX_Y);

  if ((rotation_limit & IVP_INDEX_Z) == IVP_INDEX_Z) {
    if (rotation_freedom_min->k[2] == rotation_freedom_max->k[2] && rotation_freedom_max->k[2] == 0) bs.fix_rotation_axis(IVP_INDEX_Z);
    else bs.limit_rotation_axis(IVP_INDEX_Z, rotation_freedom_min->k[2], rotation_freedom_max->k[2]);
  }
  else
    bs.free_rotation_axis(IVP_INDEX_Z);

  bs.force_factor = force_factor;
  bs.damp_factor = damp_factor;

  rs->handle = body->object->get_environment()->create_constraint(&bs);
  set_constant_delete_hook(rs);
  return rs;
}

void destroy_physics_constraint(sConstant* constant, bool forceDelete) {
  check_nullptr(constant);
  if(constant->handle)
    delete constant->handle;
  delete constant;
}

#pragma endregion

#pragma region ForceCreationAndDestruction

sForce* create_physics_force(sPhysicsBody* body1, sPhysicsBody* body2, IVP_U_Point* pos1_ws, IVP_U_Point* pos2_ws, float force_value, sBool push_object2) {
  check_nullptr_return(body1, nullptr);

  sForce* rs = new sForce();

  IVP_Template_Anchor anchor_cube1_template, anchor_cube2_template;
  IVP_Template_Force force_actuator_force_template;

  anchor_cube1_template.set_anchor_position_ws(body1->object, pos1_ws);
  anchor_cube2_template.set_anchor_position_ws(body2 == nullptr ? body1->own_world->environment->get_static_object() : body2->object, pos2_ws);

  force_actuator_force_template.anchors[0] = &anchor_cube1_template;
  force_actuator_force_template.anchors[1] = &anchor_cube2_template;

  // init some force values
  force_actuator_force_template.force = force_value;
  force_actuator_force_template.push_first_object = (IVP_BOOL)(!push_object2);
  force_actuator_force_template.push_second_object = (IVP_BOOL)push_object2;

  IVP_Actuator_Force* actuator_force = body1->object->get_environment()->create_force(&force_actuator_force_template);

  rs->handle = actuator_force;
  return rs;
}
void set_physics_force_value(sForce* f, float force_value) {
  check_nullptr(f);
  f->handle->set_force(force_value);
}
void destroy_physics_force(sForce* f) {
  check_nullptr(f);
  delete f->handle;
  delete f;
}

#pragma endregion

#pragma region SpringCreationAndDestruction

sSpring* create_physics_spring(sPhysicsBody* body1, sPhysicsBody* body2, IVP_U_Point* pos1_ws, IVP_U_Point* pos2_ws, float length, float constant, float spring_damping, float global_damping, sBool use_stiff_spring, sBool values_are_relative, sBool force_only_on_stretch) {
  check_nullptr_return(body1, nullptr);

  sSpring* rs = new sSpring();

  IVP_Template_Anchor anchor1;
  IVP_Template_Anchor anchor2;
  anchor1.set_anchor_position_ws(body1->object, pos1_ws);
  anchor2.set_anchor_position_ws(body2 == nullptr ? body1->own_world->environment->get_static_object() : body2->object, pos2_ws);

  if (use_stiff_spring) {
    IVP_Template_Stiff_Spring stiff_spring_template;

    stiff_spring_template.anchors[0] = &anchor1;
    stiff_spring_template.anchors[1] = &anchor2;

    // init some spring values
    stiff_spring_template.spring_constant = length; // range 0��1
    stiff_spring_template.spring_len = length;
    stiff_spring_template.spring_damp = spring_damping;  // range 0��1

    // create spring
    rs->stiff_spring_handle = new IVP_Controller_Stiff_Spring(body1->object->get_environment(), &stiff_spring_template);
  }
  else {

    IVP_Template_Spring spring_template;
    spring_template.spring_values_are_relative = (IVP_BOOL)values_are_relative;
    spring_template.spring_force_only_on_stretch = (IVP_BOOL)force_only_on_stretch;
    // multiply the spring constant with the mass of the objects
    spring_template.spring_constant = constant;
    spring_template.spring_len = length;
    spring_template.spring_damp = spring_damping;
    // dampening of spring in spring direction
    spring_template.rel_pos_damp = global_damping;
    // dampening of spring in all 3 directions
    spring_template.anchors[0] = &anchor1;
    spring_template.anchors[1] = &anchor2;

    rs->spring_handle = body1->object->get_environment()->create_spring(&spring_template);
  }

  return rs;
}
void destroy_physics_spring(sSpring* f) {
  check_nullptr(f);
  if (f->spring_handle)
    delete f->spring_handle;
  if (f->stiff_spring_handle)
    delete f->stiff_spring_handle;
  delete f;
}

#pragma endregion

#pragma region FunctionsInit

void init_functions(int i) {
  memset(functions, 0, sizeof(functions));

  functions[i++] = (void*)get_version;
  functions[i++] = (void*)create_environment;
  functions[i++] = (void*)destroy_environment;
  functions[i++] = (void*)environment_simulate_dtime;
  functions[i++] = (void*)environment_simulate_until;
  functions[i++] = (void*)environment_simulate_variable_time_step;
  functions[i++] = (void*)environment_reset_time;
  functions[i++] = (void*)environment_new_system_group;
  functions[i++] = (void*)environment_set_collision_layer_masks;
  functions[i++] = (void*)create_point;
  functions[i++] = (void*)create_quat;
  functions[i++] = (void*)destroy_point;
  functions[i++] = (void*)destroy_quat;
  functions[i++] = (void*)get_point;
  functions[i++] = (void*)nullptr;
  functions[i++] = (void*)physics_set_col_id;
  functions[i++] = (void*)get_pi;
  functions[i++] = (void*)get_pi2;
  functions[i++] = (void*)physicalize;
  functions[i++] = (void*)unphysicalize;
  functions[i++] = (void*)physics_set_collision_listener;
  functions[i++] = (void*)physics_remove_collision_listener;
  functions[i++] = (void*)physics_freeze;
  functions[i++] = (void*)physics_wakeup;
  functions[i++] = (void*)physics_enable_collision_detection;
  functions[i++] = (void*)physics_disable_collision_detection;
  functions[i++] = (void*)physics_beam_object_to_new_position;
  functions[i++] = (void*)physics_get_speed;
  functions[i++] = (void*)physics_get_speed_vec;
  functions[i++] = (void*)physics_get_rot_speed;
  functions[i++] = (void*)physics_set_name;
  functions[i++] = (void*)physics_set_layer;
  functions[i++] = (void*)physics_change_mass;
  functions[i++] = (void*)physics_change_unmovable_flag;
  functions[i++] = (void*)physics_impluse;
  functions[i++] = (void*)physics_torque;
  functions[i++] = (void*)physics_add_speed;
  functions[i++] = (void*)physics_convert_to_phantom;
  functions[i++] = (void*)physics_is_inside_phantom;
  functions[i++] = (void*)physics_is_contact;
  functions[i++] = (void*)physics_is_motion_enabled;
  functions[i++] = (void*)physics_is_fixed;
  functions[i++] = (void*)physics_is_gravity_enabled;
  functions[i++] = (void*)physics_enable_gravity;
  functions[i++] = (void*)physics_enable_motion;
  functions[i++] = (void*)physics_recheck_collision_filter;
  functions[i++] = (void*)physics_transform_position_to_world_coords;
  functions[i++] = (void*)physics_transform_position_to_object_coords;
  functions[i++] = (void*)physics_transform_vector_to_object_coords;
  functions[i++] = (void*)physics_transform_vector_to_world_coords;
  functions[i++] = (void*)raycasting;
  functions[i++] = (void*)raycasting_object;
  functions[i++] = (void*)delete_raycast_result;
  functions[i++] = (void*)create_points_buffer;
  functions[i++] = (void*)create_polyhedron;
  functions[i++] = (void*)delete_points_buffer;
  functions[i++] = (void*)delete_all_surfaces;
  functions[i++] = (void*)do_update_all;
  functions[i++] = (void*)set_physics_ball_joint;
  functions[i++] = (void*)set_physics_fixed_constraint;
  functions[i++] = (void*)set_physics_hinge;
  functions[i++] = (void*)set_physics_constraint;
  functions[i++] = (void*)destroy_physics_constraint;
  functions[i++] = (void*)create_physics_force;
  functions[i++] = (void*)set_physics_force_value;
  functions[i++] = (void*)destroy_physics_force;
  functions[i++] = (void*)create_physics_spring;
  functions[i++] = (void*)destroy_physics_spring;
  functions[i++] = (void*)surface_exist_by_name;
  functions[i++] = (void*)physics_get_id;
  functions[i++] = (void*)raycasting_one;
  functions[i++] = (void*)physics_is_phantom;
  functions[i++] = (void*)motion_controller_set_target_pos;
  functions[i++] = (void*)motion_controller_set_max_translation_force;
  functions[i++] = (void*)motion_controller_set_max_torque;
  functions[i++] = (void*)motion_controller_set_force_factor;
  functions[i++] = (void*)motion_controller_set_damp_factor;
  functions[i++] = (void*)motion_controller_set_angular_damp_factor;
  functions[i++] = (void*)motion_controller_set_torque_factor;
  functions[i++] = (void*)get_quat;
  functions[i++] = (void*)physics_coll_detection;
  functions[i++] = (void*)physics_contract_detection;
  functions[i++] = (void*)destroy_physics_coll_detection;
  functions[i++] = (void*)destroy_all_physics_coll_detection;
  functions[i++] = (void*)destroy_physics_contract_detection;
  functions[i++] = (void*)destroy_all_physics_contract_detection;
  functions[i++] = (void*)physics_set_contract_listener;
  functions[i++] = (void*)physics_remove_contract_listener;
  functions[i++] = (void*)do_update_all_physics_contact_detection;
  functions[i++] = (void*)get_stats;

}

#pragma endregion
