using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BallancePhysics.Api
{
  public class ApiStruct
  {
    public const int Version = 2301;
    
    public bool InitSuccess { get; private set; } = false;

    public IntPtr create_point(Vector3 pt) {
      return _create_point(pt.x, pt.y, pt.z);
    }
    public IntPtr create_quat(Quaternion q) {
      return _create_quat(q.x, q.y, q.z, q.w);
    }

    public IntPtr create_environment(Vector3 gravity, float suimRate, int layerMask, int[] layerToMask) {  
      var layerToMaskPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>() * 32);
      Marshal.Copy(layerToMask, 0, layerToMaskPtr, layerToMask.Length);
      var pt = create_point(gravity);
      var rs = _create_environment(pt, suimRate, layerMask, layerToMaskPtr);
      destroy_point(pt);
      Marshal.FreeHGlobal(layerToMaskPtr);
      return rs;
    }


    public fn_get_version get_version;
    private fn_create_environment _create_environment;
    public fn_destroy_environment destroy_environment;
    public fn_environment_simulate_dtime environment_simulate_dtime;
    public fn_environment_simulate_until environment_simulate_until;
    public fn_environment_reset_time environment_reset_time;
    public fn_environment_new_system_group environment_new_system_group;
    public fn_environment_set_collision_layer_masks environment_set_collision_layer_masks;
    public fn_create_point _create_point;
    public fn_create_quat _create_quat;
    public fn_destroy_point destroy_point;
    public fn_destroy_quat destroy_quat;
    public fn_get_point_x get_point_x;
    public fn_get_point_y get_point_y;
    public fn_get_point_z get_point_z;
    public fn_get_pi get_pi;
    public fn_get_pi2 get_pi2;
    private fn_physicalize physicalize;
    private fn_unphysicalize unphysicalize;
    private fn_physics_set_collision_listener physics_set_collision_listener;
    public fn_physics_remove_collision_listener physics_remove_collision_listener;
    public fn_physics_freeze physics_freeze;
    private fn_physics_wakeup physics_wakeup;
    private fn_physics_enable_collision_detection physics_enable_collision_detection;
    private fn_physics_disable_collision_detection physics_disable_collision_detection;
    private fn_physics_beam_object_to_new_position physics_beam_object_to_new_position;
    private fn_physics_get_speed physics_get_speed;
    private fn_physics_get_speed_vec physics_get_speed_vec;
    private fn_physics_get_rot_speed physics_get_rot_speed;
    public fn_physics_set_name physics_set_name;
    public fn_physics_set_layer physics_set_layer;
    public fn_physics_change_mass physics_change_mass;
    public fn_physics_change_unmovable_flag physics_change_unmovable_flag;
    private fn_physics_impluse physics_impluse;
    private fn_physics_torque physics_torque;
    private fn_physics_add_speed physics_add_speed;
    private fn_physics_convert_to_phantom physics_convert_to_phantom;
    private fn_physics_is_inside_phantom physics_is_inside_phantom;
    private fn_physics_is_contact physics_is_contact;
    private fn_physics_is_motion_enabled physics_is_motion_enabled;
    private fn_physics_is_controlling physics_is_controlling;
    private fn_physics_is_fixed physics_is_fixed;
    private fn_physics_is_gravity_enabled physics_is_gravity_enabled;
    private fn_physics_enable_gravity physics_enable_gravity;
    private fn_physics_enable_motion physics_enable_motion;
    private fn_physics_recheck_collision_filter physics_recheck_collision_filter;
    private fn_physics_transform_position_to_world_coords physics_transform_position_to_world_coords;
    private fn_physics_transform_position_to_object_coords physics_transform_position_to_object_coords;
    private fn_physics_transform_vector_to_object_coords physics_transform_vector_to_object_coords;
    private fn_physics_transform_vector_to_world_coords physics_transform_vector_to_world_coords;
    private fn_raycasting raycasting;
    private fn_raycasting_object raycasting_object;
    private fn_delete_raycast_result delete_raycast_result;
    private fn_create_points_buffer create_points_buffer;
    private fn_create_polyhedron create_polyhedron;
    public fn_delete_points_buffer delete_points_buffer;
    public fn_delete_all_surfaces delete_all_surfaces;
    public fn_do_update_all do_update_all;
    private fn_set_physics_ball_joint set_physics_ball_joint;
    private fn_set_physics_fixed_constraint set_physics_fixed_constraint;
    private fn_set_physics_hinge set_physics_hinge;
    private fn_set_physics_constraint set_physics_constraint;
    public fn_destroy_physics_constraint destroy_physics_constraint;
    private fn_create_physics_force create_physics_force;
    private fn_set_physics_force_value set_physics_force_value;
    public fn_destroy_physics_force destroy_physics_force;
    private fn_create_physics_spring create_physics_spring;
    public fn_destroy_physics_spring destroy_physics_spring;

    //获取所有函数指针
    internal void initAll(IntPtr apiArrayPtr, int len)
    {
      int i = 0;
      IntPtr[] apiArray = new IntPtr[len];
      Marshal.Copy(apiArrayPtr, apiArray, 0, len);

      get_version = Marshal.GetDelegateForFunctionPointer<fn_get_version>(apiArray[i++]);
      _create_environment = Marshal.GetDelegateForFunctionPointer<fn_create_environment>(apiArray[i++]);
      destroy_environment = Marshal.GetDelegateForFunctionPointer<fn_destroy_environment>(apiArray[i++]);
      environment_simulate_dtime = Marshal.GetDelegateForFunctionPointer<fn_environment_simulate_dtime>(apiArray[i++]);
      environment_simulate_until= Marshal.GetDelegateForFunctionPointer<fn_environment_simulate_until>(apiArray[i++]);
      environment_reset_time = Marshal.GetDelegateForFunctionPointer<fn_environment_reset_time>(apiArray[i++]);
      environment_new_system_group = Marshal.GetDelegateForFunctionPointer<fn_environment_new_system_group>(apiArray[i++]);
      environment_set_collision_layer_masks= Marshal.GetDelegateForFunctionPointer<fn_environment_set_collision_layer_masks>(apiArray[i++]);
      _create_point = Marshal.GetDelegateForFunctionPointer<fn_create_point>(apiArray[i++]);
      _create_quat = Marshal.GetDelegateForFunctionPointer<fn_create_quat>(apiArray[i++]);
      destroy_point = Marshal.GetDelegateForFunctionPointer<fn_destroy_point>(apiArray[i++]);
      destroy_quat = Marshal.GetDelegateForFunctionPointer<fn_destroy_quat>(apiArray[i++]);
      get_point_x = Marshal.GetDelegateForFunctionPointer<fn_get_point_x>(apiArray[i++]);
      get_point_y = Marshal.GetDelegateForFunctionPointer<fn_get_point_y>(apiArray[i++]);
      get_point_z= Marshal.GetDelegateForFunctionPointer<fn_get_point_z>(apiArray[i++]);
      get_pi = Marshal.GetDelegateForFunctionPointer<fn_get_pi>(apiArray[i++]);
      get_pi2 = Marshal.GetDelegateForFunctionPointer<fn_get_pi2>(apiArray[i++]);
      physicalize = Marshal.GetDelegateForFunctionPointer<fn_physicalize>(apiArray[i++]);
      unphysicalize = Marshal.GetDelegateForFunctionPointer<fn_unphysicalize>(apiArray[i++]);
      physics_set_collision_listener = Marshal.GetDelegateForFunctionPointer<fn_physics_set_collision_listener>(apiArray[i++]);
      physics_remove_collision_listener = Marshal.GetDelegateForFunctionPointer<fn_physics_remove_collision_listener>(apiArray[i++]);
      physics_freeze = Marshal.GetDelegateForFunctionPointer<fn_physics_freeze>(apiArray[i++]);
      physics_wakeup = Marshal.GetDelegateForFunctionPointer<fn_physics_wakeup>(apiArray[i++]);
      physics_enable_collision_detection = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_collision_detection>(apiArray[i++]);
      physics_disable_collision_detection = Marshal.GetDelegateForFunctionPointer<fn_physics_disable_collision_detection>(apiArray[i++]);
      physics_beam_object_to_new_position = Marshal.GetDelegateForFunctionPointer<fn_physics_beam_object_to_new_position>(apiArray[i++]);
      physics_get_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_get_speed>(apiArray[i++]);
      physics_get_speed_vec = Marshal.GetDelegateForFunctionPointer<fn_physics_get_speed_vec>(apiArray[i++]);
      physics_get_rot_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_get_rot_speed>(apiArray[i++]);
      physics_set_name = Marshal.GetDelegateForFunctionPointer<fn_physics_set_name>(apiArray[i++]);
      physics_set_layer = Marshal.GetDelegateForFunctionPointer<fn_physics_set_layer>(apiArray[i++]);
      physics_change_mass = Marshal.GetDelegateForFunctionPointer<fn_physics_change_mass>(apiArray[i++]);
      physics_change_unmovable_flag = Marshal.GetDelegateForFunctionPointer<fn_physics_change_unmovable_flag>(apiArray[i++]);
      physics_impluse = Marshal.GetDelegateForFunctionPointer<fn_physics_impluse>(apiArray[i++]);
      physics_torque = Marshal.GetDelegateForFunctionPointer<fn_physics_torque>(apiArray[i++]);
      physics_add_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_add_speed>(apiArray[i++]);
      physics_convert_to_phantom = Marshal.GetDelegateForFunctionPointer<fn_physics_convert_to_phantom>(apiArray[i++]);
      physics_is_inside_phantom = Marshal.GetDelegateForFunctionPointer<fn_physics_is_inside_phantom>(apiArray[i++]);
      physics_is_contact = Marshal.GetDelegateForFunctionPointer<fn_physics_is_contact>(apiArray[i++]);
      physics_is_motion_enabled = Marshal.GetDelegateForFunctionPointer<fn_physics_is_motion_enabled>(apiArray[i++]);
      physics_is_controlling = Marshal.GetDelegateForFunctionPointer<fn_physics_is_controlling>(apiArray[i++]);
      physics_is_fixed = Marshal.GetDelegateForFunctionPointer<fn_physics_is_fixed>(apiArray[i++]);
      physics_is_gravity_enabled = Marshal.GetDelegateForFunctionPointer<fn_physics_is_gravity_enabled>(apiArray[i++]);
      physics_enable_gravity = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_gravity>(apiArray[i++]);
      physics_enable_motion = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_motion>(apiArray[i++]);
      physics_recheck_collision_filter = Marshal.GetDelegateForFunctionPointer<fn_physics_recheck_collision_filter>(apiArray[i++]);
      physics_transform_position_to_world_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_position_to_world_coords>(apiArray[i++]);
      physics_transform_position_to_object_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_position_to_object_coords>(apiArray[i++]);
      physics_transform_vector_to_object_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_vector_to_object_coords>(apiArray[i++]);
      physics_transform_vector_to_world_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_vector_to_world_coords>(apiArray[i++]);
      raycasting = Marshal.GetDelegateForFunctionPointer<fn_raycasting>(apiArray[i++]);
      raycasting_object = Marshal.GetDelegateForFunctionPointer<fn_raycasting_object>(apiArray[i++]);
      delete_raycast_result = Marshal.GetDelegateForFunctionPointer<fn_delete_raycast_result>(apiArray[i++]);
      create_points_buffer = Marshal.GetDelegateForFunctionPointer<fn_create_points_buffer>(apiArray[i++]);
      create_polyhedron = Marshal.GetDelegateForFunctionPointer<fn_create_polyhedron>(apiArray[i++]);
      delete_points_buffer = Marshal.GetDelegateForFunctionPointer<fn_delete_points_buffer>(apiArray[i++]);
      delete_all_surfaces = Marshal.GetDelegateForFunctionPointer<fn_delete_all_surfaces>(apiArray[i++]);
      do_update_all = Marshal.GetDelegateForFunctionPointer<fn_do_update_all>(apiArray[i++]);
      set_physics_ball_joint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_ball_joint>(apiArray[i++]);
      set_physics_fixed_constraint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_fixed_constraint>(apiArray[i++]);
      set_physics_hinge = Marshal.GetDelegateForFunctionPointer<fn_set_physics_hinge>(apiArray[i++]);
      set_physics_constraint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_constraint>(apiArray[i++]);
      destroy_physics_constraint = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_constraint>(apiArray[i++]);
      create_physics_force = Marshal.GetDelegateForFunctionPointer<fn_create_physics_force>(apiArray[i++]);
      set_physics_force_value = Marshal.GetDelegateForFunctionPointer<fn_set_physics_force_value>(apiArray[i++]);
      destroy_physics_force = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_force>(apiArray[i++]);
      create_physics_spring = Marshal.GetDelegateForFunctionPointer<fn_create_physics_spring>(apiArray[i++]);
      destroy_physics_spring = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_spring>(apiArray[i++]);

      var v = get_version();
      if(v != Version)
        throw new Exception("Native lib version is not compatible with this (" + v + " !=" + Version + ")");
      
      InitSuccess = true;
    }

  }
}