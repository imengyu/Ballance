using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BallancePhysics.Utils;
using BallancePhysics.Wapper;
using Unity.Mathematics;
using Unity.Physics.Editor;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace BallancePhysics.Editor
{

  [EditorTool("PhysicsObjectEditorTool", typeof(PhysicsObject))]
  public class PhysicsObjectEditorTool : EditorTool
  {
    private static readonly PhysicsSphereBoundsHandle s_Sphere = new PhysicsSphereBoundsHandle();

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

      var shape = target as PhysicsObject;
      if (shape == null)
        return;

      var sScale = shape.transform.lossyScale;

      var handleColor = shape.enabled ? k_ShapeHandleColor : k_ShapeHandleColorDisabled;
      var handleMatrix = new float4x4(MathUtils.DecomposeRigidBodyTransform(shape.transform.localToWorldMatrix));
      using (new Handles.DrawingScope(handleColor, handleMatrix))
      {
        if(shape.UseBall) {
          s_Sphere.center = Vector3.zero;
          s_Sphere.radius = shape.BallRadius;
          EditorGUI.BeginChangeCheck();
          {
            using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one))))
              s_Sphere.DrawHandle();
          }
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject(shape, GenericUndoMessage);
            shape.BallRadius = s_Sphere.radius;
          }
        } else {
          if (Event.current.type == EventType.Repaint) {
            var data = GetPreviewData(shape);
            if (data != null) {
              using (new Handles.DrawingScope(math.mul(Handles.matrix, float4x4.TRS(Vector3.zero, Quaternion.identity, sScale)))) {
                Handles.color = Color.green;
                foreach(var points in data.ConcaveEdges)
                  if (points != null && points.Length > 0)
                    Handles.DrawLines(points);
                Handles.color = Color.magenta;
                foreach(var points in data.ConvexEdges)
                  if (points != null && points.Length > 0)
                    Handles.DrawLines(points);
              }
            }
            
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
          lastGenerateConvexMesh = null;
          lastGenerateConcaveMesh = null;
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

      private Mesh[] lastGenerateConvexMesh = null;
      private Mesh[] lastGenerateConcaveMesh = null;

      private void GenerateEdges(Vector3[] vertices, int[] triangles)
      {
        Vector3[] m_Edges = new Vector3[triangles.Length * 2];
        int ie = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
          m_Edges[ie++] = vertices[triangles[i]];
          m_Edges[ie++] = vertices[triangles[i + 1]];
          m_Edges[ie++] = vertices[triangles[i + 1]];
          m_Edges[ie++] = vertices[triangles[i + 2]];
          m_Edges[ie++] = vertices[triangles[i + 2]];
          m_Edges[ie++] = vertices[triangles[i]];
        }
        ConcaveEdges.Add(m_Edges);
      }
      private void GenerateConvexHullEdges(Vector3[] vertices, int[] triangles) {
        List<int> ind = new List<int>(triangles);
        ConvexHullAlgorithm.Execute(ref ind, vertices, Vector3.up);
        int ie = 0;
        Vector3[] m_Edges = new Vector3[ind.Count * 2];
        for (int i = 0; i < ind.Count; i += 3)
        {
          if(i + 2 >= ind.Count || ind[i + 2] >= vertices.Length)
            break;
          m_Edges[ie++] = vertices[ind[i]];
          m_Edges[ie++] = vertices[ind[i + 1]];
          m_Edges[ie++] = vertices[ind[i + 1]];
          m_Edges[ie++] = vertices[ind[i + 2]];
          m_Edges[ie++] = vertices[ind[i + 2]];
          m_Edges[ie++] = vertices[ind[i]];
        }
        ConvexEdges.Add(m_Edges);
      }

      public void SchedulePreviewIfChanged(PhysicsObject obj)
      {
        if (!obj.UseBall) {
          if((obj.Concave.Count == 0 && obj.Convex.Count == 0))
          {
            var meshFilter = obj.GetComponent<MeshFilter>();
            if(meshFilter == null || meshFilter.sharedMesh == null) 
              return;
            var mesh = meshFilter.sharedMesh;
            if(obj.Fixed && (lastGenerateConcaveMesh == null || lastGenerateConcaveMesh.Length != 1 || lastGenerateConcaveMesh[0] != mesh)) {
              lastGenerateConcaveMesh = new Mesh[1] { mesh };
              ConcaveEdges.Clear();
              ConvexEdges.Clear();
              GenerateEdges(mesh.vertices, mesh.triangles);
            }
            else if(!obj.Fixed && (lastGenerateConvexMesh == null || lastGenerateConvexMesh.Length != 1 || lastGenerateConvexMesh[0] != mesh)) {
              lastGenerateConvexMesh = new Mesh[1] { mesh };
              ConcaveEdges.Clear();
              ConvexEdges.Clear();
              GenerateConvexHullEdges(mesh.vertices, mesh.triangles);
            }
          }
          else if((obj.Concave.Count > 0 && lastGenerateConcaveMesh == null) 
            || (obj.Convex.Count > 0 && lastGenerateConvexMesh == null) 
            || (lastGenerateConcaveMesh.Length != obj.Concave.Count || lastGenerateConvexMesh.Length != obj.Convex.Count))
          {
            ConcaveEdges.Clear();
            ConvexEdges.Clear();
            lastGenerateConcaveMesh = obj.Concave.ToArray();
            lastGenerateConvexMesh = obj.Convex.ToArray();

            foreach(var mesh in lastGenerateConcaveMesh)
              GenerateEdges(mesh.vertices, mesh.triangles);
            foreach(var mesh in lastGenerateConvexMesh)
              GenerateConvexHullEdges(mesh.vertices, mesh.triangles);
          }
        }
          
      }

      public List<Vector3[]> ConvexEdges { get; private set; } = new List<Vector3[]>();
      public List<Vector3[]> ConcaveEdges { get; private set; } = new List<Vector3[]>();
    }

    private Dictionary<PhysicsObject, PreviewMeshData> m_PreviewData = new Dictionary<PhysicsObject, PreviewMeshData>();

    public PreviewMeshData GetPreviewData(PhysicsObject shape)
    {
      if (shape.UseBall)
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
}