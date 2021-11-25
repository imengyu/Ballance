using System;
using System.Runtime.InteropServices;
using UnityEngine;
using BallancePhysics;
using System.Collections.Generic;

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

    public fn_get_version get_version;
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
    public fn_get_point get_point;
    public fn_get_pi get_pi;
    public fn_get_pi2 get_pi2;
    public fn_physics_recheck_collision_filter physics_recheck_collision_filter;
    public fn_physics_remove_collision_listener physics_remove_collision_listener;
    public fn_physics_enable_collision_detection physics_enable_collision_detection;
    public fn_physics_disable_collision_detection physics_disable_collision_detection;
    public fn_physics_wakeup physics_wakeup;
    public fn_physics_get_speed physics_get_speed;    
    public fn_physics_set_name _physics_set_name;
    public fn_physics_set_layer physics_set_layer;
    public fn_physics_change_mass physics_change_mass;
    public fn_delete_points_buffer delete_points_buffer;
    public fn_delete_all_surfaces delete_all_surfaces;
    public fn_do_update_all do_update_all;
    public fn_destroy_physics_constraint destroy_physics_constraint;
    public fn_destroy_physics_force destroy_physics_force;
    public fn_destroy_physics_spring destroy_physics_spring;
    public fn_set_physics_force_value set_physics_force_value;
    public fn_set_physics_fixed_constraint set_physics_fixed_constraint;
    public fn_physics_get_id physics_get_id;
    public fn_get_quat get_quat;
    private fn_delete_raycast_result delete_raycast_result;
    private fn_surface_exist_by_name _surface_exist_by_name;

    public void physics_set_name(IntPtr body, string name) {
      var namePtr = Marshal.StringToHGlobalAnsi(name);
      _physics_set_name(body, namePtr);
      Marshal.FreeHGlobal(namePtr);
    }

    private fn_create_environment _create_environment;
    public IntPtr create_environment(Vector3 gravity, float suimRate, int layerMask, int[] layerToMask) {  
      var layerToMaskPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>() * 32);
      Marshal.Copy(layerToMask, 0, layerToMaskPtr, layerToMask.Length);
      var pt = create_point(gravity);
      var rs = _create_environment(pt, suimRate, layerMask, layerToMaskPtr);
      destroy_point(pt);
      Marshal.FreeHGlobal(layerToMaskPtr);
      return rs;
    }

    private fn_physicalize _physicalize;
    public IntPtr physicalize(IntPtr world, string name, int layer, int systemGroup, int subSystemId, int subSystemDontCollideWith, float mass, float friction, float elasticity, float linear_speed_damp, float rot_speed_damp, float ball_radius, bool use_ball, bool enable_convex_hull, bool auto_mass_center, bool enable_collision, bool start_frozen, bool physical_unmoveable, Vector3 position, Vector3 shfit_mass_center, Quaternion rotation, bool use_exists_surface, string surface_name, int convex_count, List<IntPtr> convex_data, int concave_count, List<IntPtr> concave_data, float extra_radius) {
      
      var pt_position = create_point(position);
      var pt_shfit_mass_center = create_point(shfit_mass_center);
      var pt_rotation = create_quat(rotation);

      var p_name = Marshal.StringToHGlobalAnsi(name);
      var p_surface_name = Marshal.StringToHGlobalAnsi(surface_name);
      var p_convex_data = Marshal.AllocHGlobal(convex_data.Count * Marshal.SizeOf<IntPtr>());
      var p_concave_data = Marshal.AllocHGlobal(concave_data.Count * Marshal.SizeOf<IntPtr>());

      Marshal.Copy(convex_data.ToArray(), 0, p_convex_data, convex_data.Count);
      Marshal.Copy(concave_data.ToArray(), 0, p_concave_data, concave_data.Count);

      var rs = _physicalize(world, p_name, layer, systemGroup, subSystemId, subSystemDontCollideWith, mass, friction, 
        elasticity, linear_speed_damp, rot_speed_damp, ball_radius, PhysicsApi.boolToSbool(use_ball), PhysicsApi.boolToSbool(enable_convex_hull), PhysicsApi.boolToSbool(auto_mass_center), 
        PhysicsApi.boolToSbool(enable_collision), PhysicsApi.boolToSbool(start_frozen), PhysicsApi.boolToSbool(physical_unmoveable), pt_position, pt_shfit_mass_center,
        pt_rotation, PhysicsApi.boolToSbool(use_exists_surface), p_surface_name, convex_count, p_convex_data, concave_count, p_concave_data, extra_radius);
      
      Marshal.FreeHGlobal(p_convex_data);
      Marshal.FreeHGlobal(p_surface_name);
      Marshal.FreeHGlobal(p_name);
      Marshal.FreeHGlobal(p_concave_data);
      destroy_point(pt_position);
      destroy_point(pt_shfit_mass_center);
      destroy_quat(pt_rotation);

      return rs;
    }
    
    private fn_unphysicalize _unphysicalize;
    public void unphysicalize(IntPtr world, IntPtr body, bool silently) {
      _unphysicalize(world, body, PhysicsApi.boolToSbool(silently));
    }
    
    private fn_physics_set_collision_listener _physics_set_collision_listener;
    public void physics_set_collision_listener(IntPtr body, CollisionEventCallback callback, float collision_call_sleep, FrictionEventCallback friction_event_callback) {
      _physics_set_collision_listener(body, Marshal.GetFunctionPointerForDelegate(callback), collision_call_sleep, Marshal.GetFunctionPointerForDelegate(friction_event_callback));
    }

    private fn_physics_freeze _physics_freeze;
    public bool physics_freeze(IntPtr body) { return _physics_freeze(body) > 0; }
    
    private fn_physics_beam_object_to_new_position _physics_beam_object_to_new_position;
    public void physics_beam_object_to_new_position(IntPtr body, Quaternion rotation, Vector3 position, bool optimize_for_repeated_calls) {
      var pt_position = create_point(position);
      var pt_rotation = create_quat(rotation); 
      _physics_beam_object_to_new_position(body, pt_rotation, pt_position, PhysicsApi.boolToSbool(optimize_for_repeated_calls));
      destroy_point(pt_position);
      destroy_quat(pt_rotation);
    }

    private fn_physics_get_speed_vec _physics_get_speed_vec;
    public void physics_get_speed_vec(IntPtr body, out Vector3 speed_ws_out) {
      var pt_speed_ws_out = create_point(Vector3.zero);
      _physics_get_speed_vec(body, pt_speed_ws_out);
      speed_ws_out = ptr_to_vec3(pt_speed_ws_out);
      destroy_point(pt_speed_ws_out);
    }
    
    private fn_physics_get_rot_speed _physics_get_rot_speed;
    public void physics_get_rot_speed(IntPtr body, out Vector3 normized_axis_cs_out) {
      var pt_normized_axis_cs_out = create_point(Vector3.zero);
      _physics_get_rot_speed(body, pt_normized_axis_cs_out);
      normized_axis_cs_out = ptr_to_vec3(pt_normized_axis_cs_out);
      destroy_point(pt_normized_axis_cs_out);
    }

    private fn_physics_change_unmovable_flag _physics_change_unmovable_flag;
    public void physics_change_unmovable_flag(IntPtr body, bool unmovable_flag) { _physics_change_unmovable_flag(body, PhysicsApi.boolToSbool(unmovable_flag)); }

    private fn_physics_impluse _physics_impluse;
    public void physics_impluse(IntPtr body, Vector3 pos_ws, Vector3 impulse_ws) { 
      var pt_pos_ws = create_point(pos_ws);
      var pt_impulse_ws = create_point(impulse_ws);
      _physics_impluse(body, pt_pos_ws, pt_impulse_ws);
      destroy_point(pt_pos_ws);
      destroy_point(pt_impulse_ws);
    }

    private fn_physics_torque _physics_torque;
    public void physics_torque(IntPtr body, Vector3 rotation_vec) {
      var pt_rotation_vec = create_point(rotation_vec);
      _physics_torque(body, pt_rotation_vec);
      destroy_point(pt_rotation_vec);
    }

    private fn_physics_add_speed _physics_add_speed;
    public void physics_add_speed(IntPtr body, Vector3 speed_ws) {
      var pt_speed_ws = create_point(speed_ws);
      _physics_add_speed(body, pt_speed_ws);
      destroy_point(pt_speed_ws);
    }

    private fn_physics_convert_to_phantom _physics_convert_to_phantom;
    public void physics_convert_to_phantom(IntPtr body, float extra_radius, PhantomEventCallback phantomEventCallback) {
      _physics_convert_to_phantom(body, extra_radius, Marshal.GetFunctionPointerForDelegate(phantomEventCallback));
    }

    private fn_physics_is_inside_phantom _physics_is_inside_phantom;
    public bool physics_is_inside_phantom(IntPtr phantom, IntPtr other) { return _physics_is_inside_phantom(phantom, other) > 0; }

    private fn_physics_is_contact _physics_is_contact;
    public bool physics_is_contact(IntPtr body, IntPtr other) { return _physics_is_contact(body, other) > 0; }

    private fn_physics_is_motion_enabled _physics_is_motion_enabled;
    public bool physics_is_motion_enabled(IntPtr body) { return _physics_is_motion_enabled(body) > 0; }

    private fn_physics_is_phantom _physics_is_phantom;
    public bool physics_is_phantom(IntPtr body) { return _physics_is_phantom(body) > 0; }

    private fn_physics_is_fixed _physics_is_fixed;
    public bool physics_is_fixed(IntPtr body) { return _physics_is_fixed(body) > 0; }

    private fn_physics_is_gravity_enabled _physics_is_gravity_enabled;
    public bool physics_is_gravity_enabled(IntPtr body) { return _physics_is_gravity_enabled(body) > 0; }

    private fn_physics_enable_gravity _physics_enable_gravity;
    public void physics_enable_gravity(IntPtr body, bool enable) { _physics_enable_gravity(body, PhysicsApi.boolToSbool(enable)); }

    private fn_physics_enable_motion _physics_enable_motion;
    public void physics_enable_motion(IntPtr body, bool enable) { _physics_enable_motion(body, PhysicsApi.boolToSbool(enable)); }
    
    private fn_physics_transform_position_to_world_coords _physics_transform_position_to_world_coords;
    public void physics_transform_position_to_world_coords(IntPtr body, Vector3 pos_cs, Vector3 out_ws) {
      var pt_pos_cs = create_point(pos_cs);
      var pt_out_ws = create_point(out_ws);
      _physics_transform_position_to_world_coords(body, pt_pos_cs, pt_out_ws);
      destroy_point(pt_pos_cs);
      destroy_point(pt_out_ws);
    }

    private fn_physics_transform_position_to_object_coords _physics_transform_position_to_object_coords;
    public void physics_transform_position_to_object_coords(IntPtr body, Vector3 pos_ws, Vector3 out_os) {
      var pt_pos_ws = create_point(pos_ws);
      var pt_out_os = create_point(out_os);
      _physics_transform_position_to_object_coords(body, pt_pos_ws, pt_out_os);
      destroy_point(pt_pos_ws);
      destroy_point(pt_out_os);
    }

    private fn_physics_transform_vector_to_object_coords _physics_transform_vector_to_object_coords;
    public void physics_transform_vector_to_object_coords(IntPtr body, Vector3 vec_ws, Vector3 out_os) {
      var pt_vec_ws = create_point(vec_ws);
      var pt_out_os = create_point(out_os);
      _physics_transform_vector_to_object_coords(body, pt_vec_ws, pt_out_os);
      destroy_point(pt_vec_ws);
      destroy_point(pt_out_os);
    }

    private fn_physics_transform_vector_to_world_coords _physics_transform_vector_to_world_coords;
    public void physics_transform_vector_to_world_coords(IntPtr body, Vector3 pos_cs, Vector3 out_ws) {
      var pt_pos_cs = create_point(pos_cs);
      var pt_out_ws = create_point(out_ws);
      _physics_transform_vector_to_world_coords(body, pt_pos_cs, pt_out_ws);
      destroy_point(pt_pos_cs);
      destroy_point(pt_out_ws);
    }

    private fn_motion_controller_set_target_pos _motion_controller_set_target_pos;
    public void motion_controller_set_target_pos(IntPtr controller, Vector3 pos_ws) {
      var pt_out_ws = create_point(pos_ws);
      _motion_controller_set_target_pos(controller, pt_out_ws);
      destroy_point(pt_out_ws);
    }

    private fn_raycasting _raycasting;
    public RayCastResult raycasting(IntPtr world, int flag, Vector3 start_point, Vector3 direction, float rayLength) {
      var pt_start_point = create_point(start_point);
      var pt_direction = create_point(direction);
      var rsPtr = _raycasting(world, flag, pt_start_point, pt_direction, rayLength);
      destroy_point(pt_start_point);
      destroy_point(pt_direction);

      var rsStruct = Marshal.PtrToStructure<sRayCastResult>(rsPtr);

      var rs = new RayCastResult();
      rs.hit_distances = new float[rsStruct.n_hit_objects];
      rs.hit_objects = new IntPtr[rsStruct.n_hit_objects];
      Marshal.Copy(rsStruct.hit_distances, rs.hit_distances, 0, rsStruct.n_hit_objects);
      Marshal.Copy(rsStruct.hit_objects, rs.hit_objects, 0, rsStruct.n_hit_objects);

      delete_raycast_result(rsPtr);
      return rs;
    }

    private fn_raycasting_object _raycasting_object;
    public bool raycasting_object(IntPtr objectPtr, Vector3 start_point, Vector3 direction, float rayLength, ref float distance_out) {
      var pt_start_point = create_point(start_point);
      var pt_direction = create_point(direction);
      var rs = _raycasting_object(objectPtr, pt_start_point, pt_direction, rayLength, ref distance_out);
      destroy_point(pt_start_point);
      destroy_point(pt_direction);
      return rs > 0;
    }

    private fn_raycasting_one _raycasting_one;
    public int raycasting_one(IntPtr world, Vector3 start_point, Vector3 direction, float rayLength, ref float distance_out) {
      var pt_start_point = create_point(start_point);
      var pt_direction = create_point(direction);
      var rs = _raycasting_object(world, pt_start_point, pt_direction, rayLength, ref distance_out);
      destroy_point(pt_start_point);
      destroy_point(pt_direction);
      return rs;
    }

    private fn_create_points_buffer _create_points_buffer;
    public IntPtr create_points_buffer(Vector3[] points, Vector3 scale) {

      float[] pointFloats = new float[points.Length * 3];
      for(int i = 0, c = points.Length; i < c; i++) {
        var p = points[i];
        pointFloats[i * 3] = p.x * scale.x;
        pointFloats[i * 3 + 1] = p.y * scale.y;
        pointFloats[i * 3 + 2] = p.z * scale.z;
      }

      var pointsPtr = Marshal.AllocHGlobal(points.Length * 3 * Marshal.SizeOf<float>());

      Marshal.Copy(pointFloats, 0, pointsPtr, pointFloats.Length);

      var rs = _create_points_buffer(points.Length, pointsPtr);

      Marshal.FreeHGlobal(pointsPtr);

      return rs;
    }

    private fn_create_polyhedron _create_polyhedron;
    public IntPtr create_polyhedron(Vector3[] points, int[] indices, Vector3 scale) {
      float[] pointFloats = new float[points.Length * 3];
      for(int i = 0, c = points.Length; i < c; i++) {
        var p = points[i];
        pointFloats[i * 3] = p.x * scale.x;
        pointFloats[i * 3 + 1] = p.y * scale.y;
        pointFloats[i * 3 + 2] = p.z * scale.z;
      }

      var pointsPtr = Marshal.AllocHGlobal(points.Length * 3 * Marshal.SizeOf<float>());
      var indicesPtr = Marshal.AllocHGlobal(indices.Length * Marshal.SizeOf<int>());

      Marshal.Copy(pointFloats, 0, pointsPtr, pointFloats.Length);
      Marshal.Copy(indices, 0, indicesPtr, indices.Length);

      var rs = _create_polyhedron(points.Length, indices.Length, pointsPtr, indicesPtr);

      Marshal.FreeHGlobal(pointsPtr);
      Marshal.FreeHGlobal(indicesPtr);

      return rs;
    }

    private fn_set_physics_ball_joint _set_physics_ball_joint;
    public IntPtr set_physics_ball_joint(IntPtr body, IntPtr other, Vector3 joint_position_ws) {
      var pt_joint_position_ws = create_point(joint_position_ws);
      var rs = _set_physics_ball_joint(body, other, pt_joint_position_ws);
      destroy_point(pt_joint_position_ws);
      return rs;
    }

    private fn_set_physics_hinge _set_physics_hinge;
    public IntPtr set_physics_hinge(IntPtr body, IntPtr other, Vector3 anchor_ws, Vector3 free_axis_ws) {
      var pt_anchor_ws = create_point(anchor_ws);
      var pt_free_axis_ws = create_point(free_axis_ws);
      var rs = _set_physics_hinge(body, other, pt_anchor_ws, pt_free_axis_ws);
      destroy_point(pt_anchor_ws);
      destroy_point(pt_free_axis_ws);
      return rs;
    }

    private fn_set_physics_constraint _set_physics_constraint;
    public IntPtr set_physics_constraint(IntPtr body, IntPtr other, float force_factor, float damp_factor, int translation_limit, Vector3 translation_freedom_min, Vector3 translation_freedom_max, int rotation_limit, Vector3 rotation_freedom_min, Vector3 rotation_freedom_max) {
      var pt_translation_freedom_min = create_point(translation_freedom_min);
      var pt_translation_freedom_max = create_point(translation_freedom_max);
      var pt_rotation_freedom_min = create_point(rotation_freedom_min);
      var pt_rotation_freedom_max = create_point(rotation_freedom_max);
      var rs = _set_physics_constraint(body, other, force_factor, damp_factor, translation_limit, pt_translation_freedom_min, pt_translation_freedom_max, rotation_limit, pt_rotation_freedom_min, pt_rotation_freedom_max);
      destroy_point(pt_translation_freedom_min);
      destroy_point(pt_translation_freedom_max);
      destroy_point(pt_rotation_freedom_min);
      destroy_point(pt_rotation_freedom_max);
      return rs;
    }
  
    private fn_create_physics_force _create_physics_force;
    public IntPtr create_physics_force(IntPtr body1, IntPtr body2, Vector3 pos1_ws, Vector3 pos2_ws, float force_value, bool push_object2) {
      var pt_pos1_os = create_point(pos1_ws);
      var pt_pos2_os = create_point(pos2_ws);
      var rs = _create_physics_force(body1, body2, pt_pos1_os, pt_pos2_os, force_value, PhysicsApi.boolToSbool(push_object2));
      destroy_point(pt_pos1_os);
      destroy_point(pt_pos2_os);
      return rs;
    }

    private fn_create_physics_spring _create_physics_spring;
    public IntPtr create_physics_spring(IntPtr body1, IntPtr body2, Vector3 pos1_ws, Vector3 pos2_ws, float length, float constant, float spring_damping, float global_damping, bool use_stiff_spring) {
      var pt_pos1_os = create_point(pos1_ws);
      var pt_pos2_os = create_point(pos2_ws);
      var rs = _create_physics_spring(body1, body2, pt_pos1_os, pt_pos2_os, length, constant, spring_damping, global_damping, PhysicsApi.boolToSbool(use_stiff_spring));
      destroy_point(pt_pos1_os);
      destroy_point(pt_pos2_os);
      return rs;
    }
  
    public bool surface_exist_by_name(IntPtr world, string name) {
      var p_name = Marshal.StringToHGlobalAnsi(name);
      var rs = _surface_exist_by_name(world, p_name) > 0;
      Marshal.FreeHGlobal(p_name);
      return rs;
    }

    public Vector3 ptr_to_vec3(IntPtr p) {
      
      IntPtr buf = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 3);
      get_point(p, buf);
      float[] bufOut = new float[3];
      Marshal.Copy(buf, bufOut, 0, 3);
      Marshal.FreeHGlobal(buf);

      return new Vector3(bufOut[0], bufOut[1], bufOut[2]);
    }
    public Quaternion ptr_to_quat(IntPtr p) {
      
      IntPtr buf = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 4);
      get_quat(p, buf);
      float[] bufOut = new float[4];
      Marshal.Copy(buf, bufOut, 0, 4);
      Marshal.FreeHGlobal(buf);

      return new Quaternion(bufOut[0], bufOut[1], bufOut[2], bufOut[3]);
    }

    //获取所有函数指针
    internal void initAll(IntPtr apiArrayPtr, int len)
    {
      int i = 0;
      IntPtr[] apiArray = new IntPtr[len];
      Marshal.Copy(apiArrayPtr, apiArray, 0, len);

      get_version = Marshal.GetDelegateForFunctionPointer<fn_get_version>(apiArray[i++]);
      
      var v = get_version();
      if(v != Version)
        throw new Exception("[BallancePhysics] Native lib version is not compatible with this (" + v + " !=" + Version + ")");

      _create_environment = Marshal.GetDelegateForFunctionPointer<fn_create_environment>(apiArray[i++]);
      destroy_environment = Marshal.GetDelegateForFunctionPointer<fn_destroy_environment>(apiArray[i++]);
      environment_simulate_dtime = Marshal.GetDelegateForFunctionPointer<fn_environment_simulate_dtime>(apiArray[i++]);
      environment_simulate_until= Marshal.GetDelegateForFunctionPointer<fn_environment_simulate_until>(apiArray[i++]);
      environment_reset_time = Marshal.GetDelegateForFunctionPointer<fn_environment_reset_time>(apiArray[i++]);
      environment_new_system_group = Marshal.GetDelegateForFunctionPointer<fn_environment_new_system_group>(apiArray[i++]);
      environment_set_collision_layer_masks = Marshal.GetDelegateForFunctionPointer<fn_environment_set_collision_layer_masks>(apiArray[i++]);
      _create_point = Marshal.GetDelegateForFunctionPointer<fn_create_point>(apiArray[i++]);
      _create_quat = Marshal.GetDelegateForFunctionPointer<fn_create_quat>(apiArray[i++]);
      destroy_point = Marshal.GetDelegateForFunctionPointer<fn_destroy_point>(apiArray[i++]);
      destroy_quat = Marshal.GetDelegateForFunctionPointer<fn_destroy_quat>(apiArray[i++]);
      get_point = Marshal.GetDelegateForFunctionPointer<fn_get_point>(apiArray[i++]);
      i++;
      i++;
      get_pi = Marshal.GetDelegateForFunctionPointer<fn_get_pi>(apiArray[i++]);
      get_pi2 = Marshal.GetDelegateForFunctionPointer<fn_get_pi2>(apiArray[i++]);
      _physicalize = Marshal.GetDelegateForFunctionPointer<fn_physicalize>(apiArray[i++]);
      _unphysicalize = Marshal.GetDelegateForFunctionPointer<fn_unphysicalize>(apiArray[i++]);
      _physics_set_collision_listener = Marshal.GetDelegateForFunctionPointer<fn_physics_set_collision_listener>(apiArray[i++]);
      physics_remove_collision_listener = Marshal.GetDelegateForFunctionPointer<fn_physics_remove_collision_listener>(apiArray[i++]);
      _physics_freeze = Marshal.GetDelegateForFunctionPointer<fn_physics_freeze>(apiArray[i++]);
      physics_wakeup = Marshal.GetDelegateForFunctionPointer<fn_physics_wakeup>(apiArray[i++]);
      physics_enable_collision_detection = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_collision_detection>(apiArray[i++]);
      physics_disable_collision_detection = Marshal.GetDelegateForFunctionPointer<fn_physics_disable_collision_detection>(apiArray[i++]);
      _physics_beam_object_to_new_position = Marshal.GetDelegateForFunctionPointer<fn_physics_beam_object_to_new_position>(apiArray[i++]);
      physics_get_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_get_speed>(apiArray[i++]);
      _physics_get_speed_vec = Marshal.GetDelegateForFunctionPointer<fn_physics_get_speed_vec>(apiArray[i++]);
      _physics_get_rot_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_get_rot_speed>(apiArray[i++]);
      _physics_set_name = Marshal.GetDelegateForFunctionPointer<fn_physics_set_name>(apiArray[i++]);
      physics_set_layer = Marshal.GetDelegateForFunctionPointer<fn_physics_set_layer>(apiArray[i++]);
      physics_change_mass = Marshal.GetDelegateForFunctionPointer<fn_physics_change_mass>(apiArray[i++]);
      _physics_change_unmovable_flag = Marshal.GetDelegateForFunctionPointer<fn_physics_change_unmovable_flag>(apiArray[i++]);
      _physics_impluse = Marshal.GetDelegateForFunctionPointer<fn_physics_impluse>(apiArray[i++]);
      _physics_torque = Marshal.GetDelegateForFunctionPointer<fn_physics_torque>(apiArray[i++]);
      _physics_add_speed = Marshal.GetDelegateForFunctionPointer<fn_physics_add_speed>(apiArray[i++]);
      _physics_convert_to_phantom = Marshal.GetDelegateForFunctionPointer<fn_physics_convert_to_phantom>(apiArray[i++]);
      _physics_is_inside_phantom = Marshal.GetDelegateForFunctionPointer<fn_physics_is_inside_phantom>(apiArray[i++]);
      _physics_is_contact = Marshal.GetDelegateForFunctionPointer<fn_physics_is_contact>(apiArray[i++]);
      _physics_is_motion_enabled = Marshal.GetDelegateForFunctionPointer<fn_physics_is_motion_enabled>(apiArray[i++]);
      _physics_is_fixed = Marshal.GetDelegateForFunctionPointer<fn_physics_is_fixed>(apiArray[i++]);
      _physics_is_gravity_enabled = Marshal.GetDelegateForFunctionPointer<fn_physics_is_gravity_enabled>(apiArray[i++]);
      _physics_enable_gravity = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_gravity>(apiArray[i++]);
      _physics_enable_motion = Marshal.GetDelegateForFunctionPointer<fn_physics_enable_motion>(apiArray[i++]);
      physics_recheck_collision_filter = Marshal.GetDelegateForFunctionPointer<fn_physics_recheck_collision_filter>(apiArray[i++]);
      _physics_transform_position_to_world_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_position_to_world_coords>(apiArray[i++]);
      _physics_transform_position_to_object_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_position_to_object_coords>(apiArray[i++]);
      _physics_transform_vector_to_object_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_vector_to_object_coords>(apiArray[i++]);
      _physics_transform_vector_to_world_coords = Marshal.GetDelegateForFunctionPointer<fn_physics_transform_vector_to_world_coords>(apiArray[i++]);
      _raycasting = Marshal.GetDelegateForFunctionPointer<fn_raycasting>(apiArray[i++]);
      _raycasting_object = Marshal.GetDelegateForFunctionPointer<fn_raycasting_object>(apiArray[i++]);
      delete_raycast_result = Marshal.GetDelegateForFunctionPointer<fn_delete_raycast_result>(apiArray[i++]);
      _create_points_buffer = Marshal.GetDelegateForFunctionPointer<fn_create_points_buffer>(apiArray[i++]);
      _create_polyhedron = Marshal.GetDelegateForFunctionPointer<fn_create_polyhedron>(apiArray[i++]);
      delete_points_buffer = Marshal.GetDelegateForFunctionPointer<fn_delete_points_buffer>(apiArray[i++]);
      delete_all_surfaces = Marshal.GetDelegateForFunctionPointer<fn_delete_all_surfaces>(apiArray[i++]);
      do_update_all = Marshal.GetDelegateForFunctionPointer<fn_do_update_all>(apiArray[i++]);
      _set_physics_ball_joint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_ball_joint>(apiArray[i++]);
      set_physics_fixed_constraint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_fixed_constraint>(apiArray[i++]);
      _set_physics_hinge = Marshal.GetDelegateForFunctionPointer<fn_set_physics_hinge>(apiArray[i++]);
      _set_physics_constraint = Marshal.GetDelegateForFunctionPointer<fn_set_physics_constraint>(apiArray[i++]);
      destroy_physics_constraint = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_constraint>(apiArray[i++]);
      _create_physics_force = Marshal.GetDelegateForFunctionPointer<fn_create_physics_force>(apiArray[i++]);
      set_physics_force_value = Marshal.GetDelegateForFunctionPointer<fn_set_physics_force_value>(apiArray[i++]);
      destroy_physics_force = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_force>(apiArray[i++]);
      _create_physics_spring = Marshal.GetDelegateForFunctionPointer<fn_create_physics_spring>(apiArray[i++]);
      destroy_physics_spring = Marshal.GetDelegateForFunctionPointer<fn_destroy_physics_spring>(apiArray[i++]);
      _surface_exist_by_name = Marshal.GetDelegateForFunctionPointer<fn_surface_exist_by_name>(apiArray[i++]);
      physics_get_id = Marshal.GetDelegateForFunctionPointer<fn_physics_get_id>(apiArray[i++]);
      _raycasting_one = Marshal.GetDelegateForFunctionPointer<fn_raycasting_one>(apiArray[i++]);
      _physics_is_phantom = Marshal.GetDelegateForFunctionPointer<fn_physics_is_phantom>(apiArray[i++]);
      _motion_controller_set_target_pos = Marshal.GetDelegateForFunctionPointer<fn_motion_controller_set_target_pos>(apiArray[i++]);
      get_quat = Marshal.GetDelegateForFunctionPointer<fn_get_quat>(apiArray[i++]);
      
      InitSuccess = true;
    }
      
    [StructLayout(LayoutKind.Sequential)]
    private struct sRayCastResult {
      public int n_hit_objects;
      public IntPtr hit_objects; // sPhysicsBody** hit_objects;
      public IntPtr hit_distances; // float* hit_distances; 
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct sInitStruct
    {
      public IntPtr eventCallback; //EventCallback eventCallback;
      public int showConsole; //sBool showConsole;
      public int smallPoolSize;
    }
  }

  /// <summary>
  /// Raycast 结果
  /// </summary>
  public struct RayCastResult {
    /// <summary>
    /// 碰撞的物体指针
    /// </summary>
    public IntPtr[] hit_objects;
    /// <summary>
    /// 碰撞的物体距离
    /// </summary>
    public float[] hit_distances;
  }

}