#pragma once

#define sUint unsigned int
#define sBool int
#define sTrue 1
#define sFalse 0
#define sError 0
#define sWarning 1
#define sInfo 2

#ifdef _WINDOWS
#define CDECL __cdecl
#else
#define CDECL 
#endif

class IVP_U_Point;
class IVP_U_Quat;

//Output message callback
//    level: sError/sWarning/sInfo
//    message: message string
typedef int (CDECL*EventCallback)(int level, int len, const char* message);
typedef void (CDECL*PhantomEventCallback)(sBool enter, struct sPhysicsBody* self, struct sPhysicsBody* other);
typedef void (CDECL*CollisionEventCallback)(struct sPhysicsBody* self, struct sPhysicsBody* other, IVP_U_Point* contact_point_ws, IVP_U_Point* speed, IVP_U_Point* surf_normal);
typedef void (CDECL*FrictionEventCallback)(sBool create, struct sPhysicsBody* self, struct sPhysicsBody* other, void* friction_handle, IVP_U_Point* contact_point_ws, IVP_U_Point* speed, IVP_U_Point* surf_normal);
typedef void(CDECL* ContractEventCallback)(struct sPhysicsBody* self, int col_id, short type, float speed_precent, short isOn);

//Physics engine initialization options
struct sInitStruct
{
  //Alloc a console for physics engine outputs? (Only windows)
  sBool showConsole;
  //Small pool size for memory allocation of common data structures
  int smallPoolSize;
  //Secret key
  char key[33];
};
//The RayCast result struct
struct sRayCastResult
{
  //How many objects were hit by rays
  int n_hit_objects;
  //The array of objects were hit by rays, length = n_hit_objects
  sPhysicsBody** hit_objects;
  //An array of distances from the ray origin of the object hit by the ray, length = n_hit_objects
  float* hit_distances;
};

extern "C"
#if _MSC_VER 
__declspec(dllexport)
#else
__attribute__((visibility("default")))
#endif
//The entry function of Physics engine
//    init: initialization command
//        * 0 destroy
//        * 1 init, and options param must privide
//        * 2 close physics engine console (Only windows)
//        * 3 open physics engine console (Only windows)
//    options: physics engine initialization options
void* CDECL ballance_physics_entry(sBool init, sInitStruct * options);
