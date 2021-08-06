using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PhysicsRT;
using Unity.Mathematics;
using Unity.Physics.Editor;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsShapeEditorTool", typeof(PhysicsShape))]
public class PhysicsShapeEditorTool : EditorTool
{
  private static readonly BeveledBoxBoundsHandle s_Box = new BeveledBoxBoundsHandle();
  private static readonly PhysicsCapsuleBoundsHandle s_Capsule =
    new PhysicsCapsuleBoundsHandle { heightAxis = CapsuleBoundsHandle.HeightAxis.Z };
  private static readonly BeveledCylinderBoundsHandle s_Cylinder = new BeveledCylinderBoundsHandle();
  private static readonly PhysicsSphereBoundsHandle s_Sphere = new PhysicsSphereBoundsHandle();
  private static readonly BoxBoundsHandle s_Plane =
      new BoxBoundsHandle { axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Z };

  private static readonly Color k_ShapeHandleColor = new Color32(145, 244, 139, 210);
  private static readonly Color k_ShapeHandleColorDisabled = new Color32(84, 200, 77, 140);
  private int m_DraggingControlID = 0;

  private static readonly string GenericUndoMessage = L10n.Tr("Change Shape");

  public override GUIContent toolbarIcon
  {
    get { return EditorGUIUtility.IconContent("EditCollider"); }
  }

  public override void OnActivated()
  {
    base.OnActivated();
  }
  public override void OnWillBeDeactivated()
  {
    base.OnWillBeDeactivated();
  }
  public override void OnToolGUI(EditorWindow window)
  {
    var hotControl = GUIUtility.hotControl;
    switch (Event.current.GetTypeForControl(hotControl))
    {
      case EventType.MouseDrag:
        m_DraggingControlID = hotControl;
        break;
      case EventType.MouseUp:
        m_DraggingControlID = 0;
        break;
    }

    var shape = target as PhysicsShape;
    if(shape == null)
      return;

    var sScale = Vector3.Scale(shape.Wrap == ShapeWrap.TransformShape ? shape.ShapeScale : Vector3.one, shape.transform.localScale);
    var sOrientation = shape.Wrap == ShapeWrap.TransformShape ? Quaternion.Euler(shape.ShapeRotation) : Quaternion.Euler(shape.ShapeType == ShapeType.Box ? 0 : 90, 0, 0);
    var sTranslation = shape.Wrap == ShapeWrap.None ? Vector3.zero : shape.ShapeTranslation;

    var handleColor = shape.enabled ? k_ShapeHandleColor : k_ShapeHandleColorDisabled;
    var handleMatrix = new float4x4(MathUtils.DecomposeRigidBodyTransform(shape.transform.localToWorldMatrix));
    using (new Handles.DrawingScope(handleColor, handleMatrix))
    {
      switch (shape.ShapeType)
      {
        case ShapeType.Box:
          s_Box.bevelRadius = shape.ShapeConvexRadius;
          s_Box.center = sTranslation;
          s_Box.size = shape.ShapeSize;
          EditorGUI.BeginChangeCheck();
          {
            using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(sTranslation, sOrientation, sScale))))
              s_Box.DrawHandle();
          }
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject(shape, GenericUndoMessage);
            shape.ShapeSize = s_Box.size;
            shape.ShapeConvexRadius = s_Box.bevelRadius;
          }
          break;
        case ShapeType.Capsule:
          s_Capsule.center = sTranslation;
          s_Capsule.height = shape.ShapeHeight;
          s_Capsule.radius = shape.ShapeRadius;
          EditorGUI.BeginChangeCheck();
          {
            using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(sTranslation, sOrientation, sScale))))
              s_Capsule.DrawHandle();
          }
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject(shape, GenericUndoMessage);
            shape.ShapeHeight = s_Capsule.height;
            shape.ShapeRadius = s_Capsule.radius;
          }
          break;
        case ShapeType.Sphere:
          s_Sphere.center = sTranslation;
          s_Sphere.radius = shape.ShapeRadius;
          EditorGUI.BeginChangeCheck();
          {
            using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(sTranslation, sOrientation, sScale))))
              s_Sphere.DrawHandle();
          }
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject(shape, GenericUndoMessage);
            shape.ShapeRadius = s_Sphere.radius;
          }
          break;
        case ShapeType.Cylinder:
          s_Cylinder.center = float3.zero;
          s_Cylinder.height = shape.ShapeHeight;
          s_Cylinder.radius = shape.ShapeRadius;
          s_Cylinder.sideCount = shape.ShapeSideCount;
          s_Cylinder.bevelRadius = shape.ShapeConvexRadius;
          EditorGUI.BeginChangeCheck();
          {
            using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(sTranslation, sOrientation, sScale))))
              s_Cylinder.DrawHandle();
          }
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject(shape, GenericUndoMessage);
            shape.ShapeHeight = s_Cylinder.height;
            shape.ShapeRadius = s_Cylinder.radius;
            shape.ShapeConvexRadius = s_Cylinder.bevelRadius;
          }
          break;
        case ShapeType.Plane:
          {
            var size2 = shape.ShapeSize;
            s_Plane.center = float3.zero;
            s_Plane.size = new float3(size2.x, size2.y, size2.z);
            EditorGUI.BeginChangeCheck();
            {
              var m = math.mul(shape.transform.localToWorldMatrix, float4x4.TRS(sTranslation, sOrientation, sScale));
              using (new Handles.DrawingScope(m))
                s_Plane.DrawHandle();
              var right = math.mul(m, new float4 { x = 1f }).xyz;
              var forward = math.mul(m, new float4 { z = 1f }).xyz;
              var normal = math.cross(math.normalizesafe(forward), math.normalizesafe(right))
                  * 0.5f * math.lerp(math.length(right) * size2.x, math.length(forward) * size2.y, 0.5f);

              using (new Handles.DrawingScope(float4x4.identity))
                Handles.DrawLine(m.c3.xyz, m.c3.xyz + normal);
            }
            if (EditorGUI.EndChangeCheck())
            {
              Undo.RecordObject(shape, GenericUndoMessage);
              shape.ShapeSize = s_Plane.size;
            }
            break;
          }
        case ShapeType.ConvexHull:
        case ShapeType.Mesh:
          {
            if (Event.current.type != EventType.Repaint)
              break;
            var points = GetPreviewData(shape).Edges;
            if (points != null && points.Length > 0)
              Handles.DrawLines(points);
            break;
          }
      }
    }
    base.OnToolGUI(window);
  }

  public class PreviewMeshData : IDisposable
  {
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        lastGenerateMesh = null;
        m_Edges = null;
        disposedValue = true;
      }
    }

    ~PreviewMeshData()
    {
      Dispose(disposing: false);
    }

    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    private Mesh lastGenerateMesh = null;
    private float lastGenerateMeshConvexRadius = 0;
    private ShapeType lastGenerateMeshShapeType = ShapeType.Box;
    private Vector3[] m_Edges = null;

    private void GenerateEdges(Vector3[] vertices, int[] triangles) {
      m_Edges = new Vector3[triangles.Length * 2];
      int ie = 0;
      for(int i = 0; i < triangles.Length; i += 3) {
        m_Edges[ie++] = vertices[triangles[i]];
        m_Edges[ie++] = vertices[triangles[i + 1]];
        m_Edges[ie++] = vertices[triangles[i + 1]];
        m_Edges[ie++] = vertices[triangles[i + 2]];
        m_Edges[ie++] = vertices[triangles[i + 2]];
        m_Edges[ie++] = vertices[triangles[i]];
      }
    }

    public void SchedulePreviewIfChanged(PhysicsShape shape) {
      if(shape.ShapeMesh != null && (lastGenerateMesh != shape.ShapeMesh || lastGenerateMeshConvexRadius != shape.ShapeConvexRadius || lastGenerateMeshShapeType != shape.ShapeType)) {
        lastGenerateMesh = shape.ShapeMesh;
        lastGenerateMeshConvexRadius = shape.ShapeConvexRadius;
        lastGenerateMeshShapeType = shape.ShapeType;

        if(lastGenerateMeshShapeType == ShapeType.ConvexHull)  {

          if(lastGenerateMesh.vertices.Length > 8192)
          {
            Debug.LogWarning("Mesh vertices too large " + lastGenerateMesh.vertices.Length + " > 8192");
            return;
          }

          if(!PhysicsApi.API.InitSuccess) 
            PhysicsApi.PhysicsApiInit();

          IntPtr convexHullResultPtr = PhysicsApi.API.Build3DPointsConvexHull(lastGenerateMesh.vertices);
          var convexHullResult = Marshal.PtrToStructure<sConvexHullResult>(convexHullResultPtr);

          //Read convex hull result vertices
          float[] verticesArrResult = new float[convexHullResult.verticesCount * 3];
          Vector3[] verticesVArrResult = new Vector3[convexHullResult.verticesCount];
          int[] trianglesArrResult = new int[convexHullResult.trianglesCount * 3];

          int bufferSize = Marshal.SizeOf<float>() * verticesArrResult.Length;
          IntPtr verticesArrResultBuffer = Marshal.AllocHGlobal(bufferSize);

          PhysicsApi.API.GetConvexHullResultVertices(convexHullResultPtr, verticesArrResultBuffer, convexHullResult.verticesCount);
          Marshal.Copy(verticesArrResult, 0, verticesArrResultBuffer, verticesArrResult.Length);
          Marshal.FreeHGlobal(verticesArrResultBuffer);

          bufferSize = Marshal.SizeOf<float>() * trianglesArrResult.Length;
          IntPtr trianglesArrResultBuffer = Marshal.AllocHGlobal(bufferSize);

          PhysicsApi.API.GetConvexHullResultTriangles(convexHullResultPtr, trianglesArrResultBuffer, convexHullResult.trianglesCount);
          Marshal.Copy(trianglesArrResult, 0, trianglesArrResultBuffer, verticesArrResult.Length);
          Marshal.FreeHGlobal(trianglesArrResultBuffer);

          //float to Vector3
          for(int i = 0; i < convexHullResult.verticesCount; i++) 
            verticesVArrResult[i] = new Vector3(
              verticesArrResult[i * 3],
              verticesArrResult[i * 3 + 1],
              verticesArrResult[i * 3 + 2]
            );

          PhysicsApi.API.CommonDelete(convexHullResultPtr);

          //Convert to lines
          GenerateEdges(verticesVArrResult, trianglesArrResult);
        } else if(lastGenerateMeshShapeType == ShapeType.Mesh) {
          GenerateEdges(lastGenerateMesh.vertices, lastGenerateMesh.triangles);
        }
      }
    }

    public Vector3[] Edges => m_Edges;
  }

  private Dictionary<PhysicsShape, PreviewMeshData> m_PreviewData = new Dictionary<PhysicsShape, PreviewMeshData>();

  public PreviewMeshData GetPreviewData(PhysicsShape shape)
  {
    if (shape.ShapeType != ShapeType.ConvexHull && shape.ShapeType != ShapeType.Mesh)
      return null;

    if (!m_PreviewData.TryGetValue(shape, out var preview))
    {
      preview = m_PreviewData[shape] = new PreviewMeshData();
      preview.SchedulePreviewIfChanged(shape);
    }

    // do not generate a new preview until the user has finished dragging a control handle (e.g., scale)
    if (m_DraggingControlID == 0 && !EditorGUIUtility.editingTextField)
      preview.SchedulePreviewIfChanged(shape);

    return preview;
  }
}