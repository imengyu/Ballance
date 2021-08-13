using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PhysicsRT
{  
    [SLua.CustomLuaClass]
    public enum ShapeType
    {
        Box,
        Sphere,
        Capsule,
        Cylinder,
        Plane,
        ConvexHull,
        Mesh,
        BvCompressedMesh,
        List,
        StaticCompound,
    }
    [SLua.CustomLuaClass]
    public enum ShapeWrap
    {
        None = 0,
        TranslateShape = 1,
        TransformShape = 2,
    }

    [AddComponentMenu("PhysicsRT/Physics Shape")]
    [DefaultExecutionOrder(210)]
    [DisallowMultipleComponent]
    [SLua.CustomLuaClass]
    public class PhysicsShape : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("为碰撞检测目的定义对象的形状和大小。")]
        private ShapeType m_ShapeType = ShapeType.Box;
        [SerializeField]
        [Tooltip("使用附加变换包裹其他凸面形状。")]
        private ShapeWrap m_Wrap = ShapeWrap.None;

        [SerializeField]
        private Vector3 m_Translation = Vector3.zero;
        [SerializeField]
        private Vector3 m_Rotation = Vector3.zero;
        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        
        [SerializeField]
        private CustomPhysicsMaterialTags m_CustomTags = CustomPhysicsMaterialTags.Nothing;

        [SerializeField]
        private Mesh m_ShapeMesh = null;
        [SerializeField]
        private Vector3 m_ShapeSize = Vector3.one;
        [SerializeField]
        private float m_ShapeRadius = 0.5f;
        [SerializeField]
        private float m_ShapeConvexRadius = 0.0f;
        [SerializeField]
        private float m_ShapeHeight = 1.0f;
        [SerializeField]
        private int m_ShapeSideCount = 32;
        [SerializeField]
        [Tooltip("指定指定给此形状的蒙皮顶点的最小权重和/或自动检测所需的变换子级。值为0时，将包括所有具有指定给此形状层次的任何权重的点。")]
        [Range(0f, 1f)]
        private float m_MinimumSkinnedVertexWeight = 0.1f;
        private IntPtr ptr = IntPtr.Zero;
        private IntPtr shapeRealPtr = IntPtr.Zero;

        [SerializeField]
        PhysicsLayerTags m_BelongsToCategories = PhysicsLayerTags.Everything;
        [SerializeField]
        PhysicsLayerTags m_CollidesWithCategories = PhysicsLayerTags.Everything;
        [SerializeField]
        CustomPhysicsMaterialTags m_CustomMaterialTags = new CustomPhysicsMaterialTags();

        public int StaticCompoundChildId { get; private set; }
        public ShapeType ShapeType => m_ShapeType;
        public ShapeWrap Wrap => m_Wrap;
        public Mesh ShapeMesh { get => m_ShapeMesh; set => m_ShapeMesh = value; }
        public Vector3 ShapeSize { get => m_ShapeSize; set => m_ShapeSize = value; }
        public float ShapeRadius { get => m_ShapeRadius; set => m_ShapeRadius = value; }
        public float ShapeConvexRadius { get => m_ShapeConvexRadius; set => m_ShapeConvexRadius = value; }
        public float ShapeHeight { get => m_ShapeHeight; set => m_ShapeHeight = value; }
        public int ShapeSideCount { get => m_ShapeSideCount; set => m_ShapeSideCount = value; }
        public Vector3 ShapeScale {
            get {
                switch(m_Wrap) {
                    case ShapeWrap.TransformShape: 
                        return m_Scale;
                    default:
                    case ShapeWrap.None:
                    case ShapeWrap.TranslateShape: 
                        return Vector3.one;
                }
            }
            set => m_Scale = value; 
        }
        public Vector3 ShapeRotation { get => m_Rotation; set => m_Rotation = value; }
        public Vector3 ShapeTranslation { get => m_Translation; set => m_Translation = value; }
        public float MinimumSkinnedVertexWeight { get => m_MinimumSkinnedVertexWeight; set => m_MinimumSkinnedVertexWeight = value; }
        public CustomPhysicsMaterialTags CustomTags
        {
            get => m_CustomMaterialTags;
            set => m_CustomMaterialTags = value;
        }

        private void Start() {
            if(m_ShapeMesh == null) {
                var m = GetComponent<MeshFilter>();
                if(m != null) m_ShapeMesh = m.mesh;
            }
        }
        private void OnValidate()
        {
            m_ShapeConvexRadius = Mathf.Max(m_ShapeConvexRadius, 0f);
            m_ShapeRadius = Mathf.Max(m_ShapeRadius, 0f);
            m_ShapeSideCount = (int)Mathf.Max(m_ShapeSideCount, 0f);
            m_ShapeHeight = Mathf.Max(m_ShapeHeight, 0f);
        }

        public IntPtr GetPtr() { return ptr; }
        public IntPtr ComputeMassProperties(float mass)
        {
            IntPtr result = IntPtr.Zero;
            switch (ShapeType)
            {
                case ShapeType.Box:
                    {
                        result = PhysicsApi.API.ComputeBoxVolumeMassProperties(ShapeSize, mass);
                        break;
                    }
                case ShapeType.Sphere:
                    {
                        result = PhysicsApi.API.ComputeSphereVolumeMassProperties(ShapeRadius, mass);
                        break;
                    }
                case ShapeType.Capsule:
                    {
                        result = PhysicsApi.API.ComputeCapsuleVolumeMassProperties(new Vector3(0, ShapeHeight / 2, 0), new Vector3(0, -ShapeHeight / 2, 0), ShapeRadius, mass);
                        break;
                    }
                case ShapeType.Cylinder:
                    {
                        result = PhysicsApi.API.ComputeCylinderVolumeMassProperties(new Vector3(0, ShapeHeight / 2, 0), new Vector3(0, -ShapeHeight / 2, 0), ShapeRadius, mass);
                        break;
                    }
                case ShapeType.Plane:
                    {
                        result = PhysicsApi.API.ComputeBoxVolumeMassProperties(new Vector3(ShapeSize.x, 0, ShapeSize.z), mass);
                        break;
                    }
                case ShapeType.ConvexHull:
                case ShapeType.List:
                    {
                        result = PhysicsApi.API.ComputeShapeVolumeMassProperties(ptr, mass);
                        break;
                    }
                case ShapeType.Mesh:
                case ShapeType.BvCompressedMesh:
                case ShapeType.StaticCompound:
                    break;
            }
            return result;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct sPhysicsShape {
            public IntPtr shape;
            public UInt16 type;
            public IntPtr staticCompoundShapeRetIds;
            public int staticCompoundShapeRetIdsCount;
        };

        private void CreateShape(bool forceRecreate, int layout) {

            //Create base shape
            switch (m_ShapeType)
            {
                case ShapeType.Box:
                    {
                        shapeRealPtr = PhysicsApi.API.CreateBoxShape(new Vector3(ShapeSize.x / 2, ShapeSize.y / 2, ShapeSize.z / 2), ShapeConvexRadius);
                        break;
                    }
                case ShapeType.Plane:
                    {
                        shapeRealPtr = PhysicsApi.API.CreateBoxShape(new Vector3(ShapeSize.x / 2, 0.01f, ShapeSize.z / 2), ShapeConvexRadius);
                        break;
                    }
                case ShapeType.Capsule:
                    {
                        shapeRealPtr = PhysicsApi.API.CreateCapsuleShape(new Vector3(0, ShapeHeight / 2, 0), new Vector3(0, -ShapeHeight / 2, 0), ShapeRadius);
                        break;
                    }
                case ShapeType.Cylinder:
                    {
                        shapeRealPtr = PhysicsApi.API.CreateCylindeShape(new Vector3(0, ShapeHeight / 2, 0), new Vector3(0, -ShapeHeight / 2, 0), ShapeRadius, ShapeConvexRadius);
                        break;
                    }
                case ShapeType.Sphere:
                    {
                        shapeRealPtr = PhysicsApi.API.CreateSphereShape(ShapeRadius);
                        break;
                    }
                case ShapeType.ConvexHull:
                    {
                        Mesh mesh = ShapeMesh;
                        if (mesh == null)
                        {
                            Debug.LogWarning("ConvexHull need a mesh! " + name);
                            return;
                        }

                        IntPtr convexHullResult = PhysicsApi.API.Build3DPointsConvexHull(mesh.vertices);
                        shapeRealPtr = PhysicsApi.API.CreateConvexVerticesShapeByConvexHullResult(convexHullResult, ShapeConvexRadius);
                        PhysicsApi.API.CommonDelete(convexHullResult);
                        break;
                    }
                case ShapeType.Mesh:
                    {
                        Mesh mesh = ShapeMesh;
                        if (mesh == null)
                        {
                            Debug.LogWarning("Shape " + name + " need a mesh");
                            return;
                        }

                        shapeRealPtr = PhysicsApi.API.CreateSimpleMeshShape(mesh.vertices, mesh.triangles, ShapeConvexRadius);
                        break;
                    }
                case ShapeType.BvCompressedMesh:
                    {
                        Mesh mesh = ShapeMesh;
                        if (mesh == null)
                        {
                            Debug.LogWarning("Shape " + name + " need a mesh");
                            return;
                        }

                        shapeRealPtr = PhysicsApi.API.CreateBvCompressedMeshShape(mesh.vertices, mesh.triangles, ShapeConvexRadius);
                        break;
                    }
                case ShapeType.List:
                    {
                        List<IntPtr> childernTransforms = null;
                        IntPtr childTransforms = IntPtr.Zero;
                        IntPtr childs = GetChildernShapes(forceRecreate, layout, false, out int childCount, ref childTransforms, ref childernTransforms);
                        shapeRealPtr = PhysicsApi.API.CreateListShape(childs, childCount);
                        Marshal.FreeHGlobal(childs);
                        break;
                    }
                case ShapeType.StaticCompound:
                    {
                        List<IntPtr> childernTransforms = new List<IntPtr>();
                        IntPtr childTransforms = IntPtr.Zero;
                        IntPtr childs = GetChildernShapes(forceRecreate, layout, true, out int childCount, ref childTransforms, ref childernTransforms);
                        IntPtr retStruct = PhysicsApi.API.CreateStaticCompoundShape(childs, childTransforms, childCount, layout);
                        shapeRealPtr = retStruct;
                        //更新ID至每个shape
                        sPhysicsShape str = (sPhysicsShape)Marshal.PtrToStructure(retStruct, typeof(sPhysicsShape));
                        int[] staticCompoundShapeRetIds = new int[str.staticCompoundShapeRetIdsCount];
                        Marshal.Copy(str.staticCompoundShapeRetIds, staticCompoundShapeRetIds, 0, str.staticCompoundShapeRetIdsCount);
                        for (int i = 0, ia = 0, c = transform.childCount; i < c; i++) {
                            var shape = transform.GetChild(i).gameObject.GetComponent<PhysicsShape>();
                            if (shape != null) {
                                shape.StaticCompoundChildId = staticCompoundShapeRetIds[ia];
                                ia++;
                            }
                        }

                        Marshal.FreeHGlobal(childs);
                        Marshal.FreeHGlobal(childTransforms);

                        foreach (var p in childernTransforms)
                            PhysicsApi.API.DestroyTransform(p);
                        childernTransforms.Clear();
                        break;
                    }
            }

            //获取父级，如果父级是容器的话，那么需要使用TransformShape包装
            var parentShape = transform.parent != null ? transform.parent.GetComponent<PhysicsShape>() : null;
            if(parentShape != null && parentShape.m_ShapeType == ShapeType.List) {
                if(transform.localPosition != Vector3.zero && transform.localRotation.eulerAngles == Vector3.zero && transform.localScale == Vector3.one) {
                    //只有位置更改，选择TranslateShape
                    ptr = PhysicsApi.API.CreateConvexTranslateShape(shapeRealPtr, ShapeTranslation + transform.localPosition);
                } else if(transform.localPosition != Vector3.zero || transform.localRotation.eulerAngles != Vector3.zero || transform.localScale != Vector3.one)  {
                    //三个任意一个更改，选择TransformShape
                    var pos = ShapeTranslation + transform.localPosition;
                    var quaternion = Quaternion.Euler(ShapeRotation + transform.localRotation.eulerAngles);
                    var scale = GetScaleMaxSame(Vector3.Scale(ShapeScale, transform.localScale));

                    IntPtr posPtr = PhysicsApi.API.CreateTransform(
                        pos.x, pos.y, pos.z,
                        quaternion.x, quaternion.y, quaternion.z, quaternion.w,
                        scale.x, scale.y, scale.z
                    );
                    ptr = PhysicsApi.API.CreateConvexTransformShape(shapeRealPtr, posPtr);
                    PhysicsApi.API.DestroyTransform(posPtr);
                } else CreateDefaultWrapShape();
            } else CreateDefaultWrapShape();
        }
        private Vector3 GetScaleMaxSame(Vector3 v) {
            if((v.x == v.y && v.y == v.z) ||  m_ShapeType == ShapeType.Box || m_ShapeType == ShapeType.ConvexHull) 
                return v;
            else {
                //其他种类的碰撞体不允许不正的缩放系数，这里改成最大的相同的缩放系数
                var max = Mathf.Max(v.x, v.y, v.z);
                return new Vector3(max, max, max);
            }
        }
        //创建默认包装
        private void CreateDefaultWrapShape() {
            //如果当前shape直接由PhysicsBody控制，并且缩放不为1，则需要包裹TransformShape以调整缩放比例 
            var body = GetComponent<PhysicsBody>();
            if(body != null && transform.lossyScale != Vector3.one) {
                var scale = GetScaleMaxSame(transform.lossyScale);
                var rot = Quaternion.Euler(0,0,0);
                IntPtr posPtr = PhysicsApi.API.CreateTransform(
                    0, 0, 0, rot.x, rot.y, rot.z, rot.w,
                    scale.x, scale.y, scale.z
                );
                ptr = PhysicsApi.API.CreateConvexTransformShape(shapeRealPtr, posPtr);
                PhysicsApi.API.DestroyTransform(posPtr);
            } else {
                //Create Wrap shape
                switch(m_Wrap) {
                    case ShapeWrap.None:
                        ptr = shapeRealPtr;
                        break;
                    case ShapeWrap.TransformShape:
                        {
                            var pos = ShapeTranslation;
                            var quaternion = Quaternion.Euler(ShapeRotation);
                            var scale = ShapeScale;

                            IntPtr posPtr = PhysicsApi.API.CreateTransform(
                                pos.x, pos.y, pos.z,
                                quaternion.x, quaternion.y, quaternion.z, quaternion.w,
                                scale.x, scale.y, scale.z
                            );
                            ptr = PhysicsApi.API.CreateConvexTransformShape(shapeRealPtr, posPtr);
                            PhysicsApi.API.DestroyTransform(posPtr);
                            break;
                        }
                    case ShapeWrap.TranslateShape:
                        {
                            ptr = PhysicsApi.API.CreateConvexTranslateShape(shapeRealPtr, ShapeTranslation);
                            break;
                        }
                }
            }
        }
        //释放
        private void DestroyShape(bool forceRecreate = false) {
            if(ptr != shapeRealPtr) {
                if(shapeRealPtr != IntPtr.Zero) {                
                    PhysicsApi.API.DestroyShape(shapeRealPtr);
                    shapeRealPtr = IntPtr.Zero;
                }
            } else {
                shapeRealPtr = IntPtr.Zero;
            }

            if(ptr != IntPtr.Zero) {
                PhysicsApi.API.DestroyShape(ptr);
                ptr = IntPtr.Zero;
            }
        }
        //获取子级Shape
        private IntPtr GetChildernShapes(bool forceRecreate, int layout, bool withChildTransforms, out int childCount, ref IntPtr outChildTransforms, ref List<IntPtr> childernTransforms)
        {
            //获取子级 PhysicsShape 的指针

            List<IntPtr> childernShapes = new List<IntPtr>();
            for (int i = 0, c = transform.childCount; i < c; i++) {
                var child = transform.GetChild(i);
                var shape = child.gameObject.GetComponent<PhysicsShape>();
                if (shape != null)
                {
                    var ptr = shape.GetShapeBody(forceRecreate, layout);
                    if(ptr == IntPtr.Zero)
                        continue;
                    childernShapes.Add(ptr);

                    if (withChildTransforms)// Child Transforms
                    {
                        childernTransforms.Add(PhysicsApi.API.CreateTransform(
                            shape.transform.localPosition.x, shape.transform.localPosition.y, shape.transform.localPosition.z,
                            shape.transform.localRotation.x, shape.transform.localRotation.y, shape.transform.localRotation.z, shape.transform.localRotation.w,
                            shape.transform.localScale.x, shape.transform.localScale.y, shape.transform.localScale.z
                        ));
                    }
                }
            }

            childCount = childernShapes.Count;

            var outArr = childernShapes.ToArray(); 
            IntPtr outArrBuf = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * outArr.Length);
            Marshal.Copy(outArr, 0, outArrBuf, outArr.Length);

            if (withChildTransforms)
            {
                var outArr2 = childernTransforms.ToArray();
                IntPtr outArrBuf2 = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * outArr2.Length);
                Marshal.Copy(outArr2, 0, outArrBuf2, outArr2.Length);
                outChildTransforms = outArrBuf2;
            }

            return outArrBuf;
        }

        public void ReleaseShapeBody() {
            DestroyShape(false);

            if(ShapeType == ShapeType.List || ShapeType == ShapeType.StaticCompound)
                for (int i = 0, c = transform.childCount; i < c; i++) {
                    var shape = transform.GetChild(i).gameObject.GetComponent<PhysicsShape>();
                    if (shape != null)
                        shape.ReleaseShapeBody();
                }
        }
        public IntPtr GetShapeBody(bool forceRecreate, int layout)
        {
            if (forceRecreate && ptr != IntPtr.Zero)
                DestroyShape(true);
            if (ptr == IntPtr.Zero)
                CreateShape(forceRecreate, layout);
            return ptr;
        }
        public void FitToEnabledRenderMeshes(float f) {
            if(ShapeMesh == null)
            {
                Debug.LogWarning("Not found mesh in this gameObject");
                return;
            }
            Bounds bounds = ShapeMesh.bounds;
            switch (m_ShapeType)
            {
                case ShapeType.Box:
                case ShapeType.Plane:
                    ShapeSize = bounds.size;
                    ShapeTranslation = bounds.center;
                    break;
                case ShapeType.Capsule:
                case ShapeType.Cylinder:
                    ShapeRadius = bounds.size.x / 2;
                    ShapeHeight = bounds.size.y;
                    break;
                case ShapeType.Sphere:
                    ShapeRadius = bounds.size.x / 2;
                    break;
            }
        }

        /// <summary>
        /// 设置 StaticCompound 子shape的启用状态
        /// </summary>
        /// <param name="id">Id，存储在Shape的StaticCompoundChildId字段</param>
        /// <param name="enable">是否启用</param>
        public void SetChildInstanceEnable(int id, bool enable)
        {
            if(m_ShapeType != ShapeType.StaticCompound)
            {
                Debug.LogError("Only StaticCompoundShape can use this action");
                return;
            }

            PhysicsApi.API.StaticCompoundShapeSetInstanceEnabled(shapeRealPtr, id, PhysicsApi.API.BoolToInt(enable));
        }
        /// <summary>
        /// 获取 StaticCompound 子shape的启用状态
        /// </summary>
        /// <param name="id">Id，存储在Shape的StaticCompoundChildId字段</param>
        /// <returns></returns>
        public bool GetChildInstanceEnable(int id)
        {
            if (m_ShapeType != ShapeType.StaticCompound)
            {
                Debug.LogError("Only StaticCompoundShape can use this action");
                return false;
            }
            return PhysicsApi.API.StaticCompoundShapeIsInstanceEnabled(shapeRealPtr, id) > 0;
        }
        /// <summary>
        /// 启用 StaticCompound 的所有子shape
        /// </summary>
        public void EnableAllChildInstance()
        {
            if (m_ShapeType != ShapeType.StaticCompound)
            {
                Debug.LogError("Only StaticCompoundShape can use this action");
                return;
            }

            PhysicsApi.API.StaticCompoundShapeEnableAllInstancesAndShapeKeys(shapeRealPtr);
        }
    }

}
