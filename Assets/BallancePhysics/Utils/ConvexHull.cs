namespace BallancePhysics.Utils
{
  /**
  *  ============================================================================
  *  MIT License
  *
  *  Copyright (c) 2016 Eric Phillips
  *
  *  Permission is hereby granted, free of charge, to any person obtaining a
  *  copy of this software and associated documentation files (the "Software"),
  *  to deal in the Software without restriction, including without limitation
  *  the rights to use, copy, modify, merge, publish, distribute, sublicense,
  *  and/or sell copies of the Software, and to permit persons to whom the
  *  Software is furnished to do so, subject to the following conditions:
  *
  *  The above copyright notice and this permission notice shall be included in
  *  all copies or substantial portions of the Software.
  *
  *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
  *  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
  *  DEALINGS IN THE SOFTWARE.
  *  ============================================================================
  *
  *
  *  This file implements a 3D generalization of the convex hulling algorithm
  *  known as the "QuickHull" algorithm. This algorithm generates a surface for a
  *  point cloud which contains all of the points and all of the surface features
  *  possible using only a convex mesh.
  *
  *  The procedure followed here is based on the following example.
  *  http://thomasdiewald.com/blog/?p=1888
  *
  *  Created by Eric Phillips on October 23, 2016.
  */

  using System.Collections.Generic;
  using UnityEngine;

  public static class ConvexHull
  {
      /// <summary>
      /// Class for holding faces and their data.
      /// </summary>
      private class FaceData
      {
          public Vector4 Plane;               // Equation of a face's plane
          public int[] FaceIndices;           // Indices of the face's points
          public List<int> VisibleIndices;    // Indices of points visible to face
          private Vector3[] _vertices;        // Main vertex array


          /// <summary>
          /// Create a new data structure for a face.
          /// </summary>
          /// <param name="pt1Idx">Index of the first point on the face.</param>
          /// <param name="pt2Idx">Index of the second point on the face.</param>
          /// <param name="pt3Idx">Index of the third point on the face.</param>
          /// <param name="vertices">Array of vertices.</param>
          public FaceData(int pt1Idx, int pt2Idx, int pt3Idx, Vector3[] vertices)
          {
              Plane = Points2Plane(vertices[pt1Idx], vertices[pt2Idx],
                  vertices[pt3Idx]);
              FaceIndices = new int[] { pt1Idx, pt2Idx, pt3Idx };
              VisibleIndices = new List<int>();
              _vertices = vertices;
          }

          /// <summary>
          /// Get the point farthest from the plane in the positive direction.
          /// </summary>
          /// <returns>The index of the point.</returns>
          public int GetFurthestPoint()
          {
              int maxIndex = 0;
              float maxDistance = float.NegativeInfinity;
              foreach (int index in VisibleIndices)
              {
                  // Calculate something linearly equivalent to the plane distance
                  Vector3 v = _vertices[index] - _vertices[FaceIndices[0]];
                  float distance = Vector3.Dot(v, Plane);
                  if (distance > maxDistance)
                  {
                      maxIndex = index;
                      maxDistance = distance;
                  }
              }
              return maxIndex;
          }
      }


      /// <summary>
      /// Compute indices of triangles which form a convex hull around the given
      /// array of vertices. This is formatted so that a set of mesh vertices can
      /// be passed in, and the output can be directly assigned to the triangles
      /// property of the mesh.
      /// </summary>
      /// <param name="vertices">Array of vertices passed in.</param>
      /// <returns>Array of triangles as indices into the input array.</returns>
      public static int[] Generate(Vector3[] vertices)
      {
          // Create initial simplex
          int[] extremePoints = FindExtremePoints(vertices);
          int[] initialSimplex = CreateInitialSimplex(extremePoints, vertices);
          // Assign each vertex to the first face to which it is visible
          List<FaceData> faceStack = AssignInitialPointsToFaces(initialSimplex, vertices);
          // Iterate until all faces have been processed
          FaceData face;
          int i = 0;
          while ((face = GetUnprocessedFace(faceStack)) != null && i < 0xFF)
          {
              // Get the point farthest from the plane in the positive direction
              int maxPtIndex = face.GetFurthestPoint();
              // Get all faces which are visible to this point and pop from stack
              List<FaceData> visibleFaces = ExtractVisibleFaces(
                  vertices[maxPtIndex], faceStack);
              // Extract the horizon edges of the visible faces
              // All faces will be connected
              Dictionary<string, int[]> horizonEdges =
                  ExtractHorizonEdges(visibleFaces);
              // Create new points from horizon edges and max point
              List<FaceData> newFaces = CreateNewFaces(maxPtIndex, visibleFaces,
                  vertices, horizonEdges);
              // Add new faces to stack and repeat
              faceStack.AddRange(newFaces);
              i++;
          }
          // Compile one array of all the triangle indices
          int[] indices = new int[faceStack.Count * 3];
          for (int ii = 0; ii < faceStack.Count; ii++)
              for (int jj = 0; jj < 3; jj++)
                  indices[ii * 3 + jj] = faceStack[ii].FaceIndices[jj];
          return indices;
      }

      /// <summary>
      /// Find the indices of the points in the vertices array which are at a
      /// minimum or maximum on each of the three axis. So find the min and max
      /// X, the min and max Y and the min and max Z points, and return their
      /// indices.
      /// </summary>
      /// <param name="vertices">Array of vertices passed in.</param>
      /// <returns>Array of six indices into the vertices array.</returns>
      private static int[] FindExtremePoints(Vector3[] vertices)
      {
          // Setup variables
          int[] extremePoints = new int[6];
          float[] extremePointValues = new float[6];
          for (int ii = 0; ii < extremePoints.Length - 1; ii += 2)
          {
              extremePoints[ii] = 0;
              extremePoints[ii + 1] = 0;
              extremePointValues[ii] = float.PositiveInfinity;
              extremePointValues[ii + 1] = float.NegativeInfinity;
          }
          // Search point cloud
          for (int ii = 0; ii < vertices.Length; ii++)
              for (int jj = 0; jj < extremePoints.Length - 1; jj += 2)
              {
                  float val = vertices[ii][jj / 2];
                  if (val < extremePointValues[jj])
                  {
                      extremePoints[jj] = ii;
                      extremePointValues[jj] = val;
                  }
                  else if (val > extremePointValues[jj + 1])
                  {
                      extremePoints[jj + 1] = ii;
                      extremePointValues[jj + 1] = val;
                  }
              }
          return extremePoints;
      }

      /// <summary>
      /// Create a tetrahedron of maximum volume out of the extreme points.
      /// This returns four indices in right-handed manner. The first three
      /// define the base of the tetrahedron, and face away from the third point.
      /// </summary>
      /// <param name="extremePoints">The indices of the extreme points.</param>
      /// <param name="vertices">Array of vertices passed in.</param>
      /// <returns>The indices of the initial tetrahedron.</returns>
      private static int[] CreateInitialSimplex(int[] extremePoints,
          Vector3[] vertices)
      {
          int[] initialSimplex = new int[4];
          // Find two most distent extreme points (base line of tetrahedron)
          float maxDistance = float.NegativeInfinity;
          for (int ii = 0; ii < extremePoints.Length; ii++)
              for (int jj = ii + 1; jj < extremePoints.Length; jj++)
              {
                  float distance = (vertices[extremePoints[ii]] -
                      vertices[extremePoints[jj]]).sqrMagnitude;
                  if (distance > maxDistance)
                  {
                      initialSimplex[0] = extremePoints[ii];
                      initialSimplex[1] = extremePoints[jj];
                      maxDistance = distance;
                  }
              }
          // Find the extreme point most distent from the line
          maxDistance = float.NegativeInfinity;
          Vector3 normal = vertices[initialSimplex[0]] -
              vertices[initialSimplex[1]];
          for (int ii = 0; ii < extremePoints.Length; ii++)
          {
              Vector3 v = vertices[extremePoints[ii]] -
                  vertices[initialSimplex[0]];
              Vector3 rejection = Vector3.ProjectOnPlane(v, normal);
              float distance = rejection.sqrMagnitude;
              if (distance > maxDistance)
              {
                  initialSimplex[2] = extremePoints[ii];
                  maxDistance = distance;
              }
          }
          // Find the most distant of all the points from the plane of the
          // triangle formed from the first three "initialSimplex" points
          maxDistance = float.NegativeInfinity;
          Vector3 v1 = vertices[initialSimplex[1]] - vertices[initialSimplex[0]];
          Vector3 v2 = vertices[initialSimplex[2]] - vertices[initialSimplex[0]];
          normal = Vector3.Cross(v1, v2);
          for (int ii = 0; ii < vertices.Length; ii++)
          {
              Vector3 v = vertices[ii] - vertices[initialSimplex[0]];
              float distance = Mathf.Abs(Vector3.Dot(v, normal));
              if (distance > maxDistance)
              {
                  initialSimplex[3] = ii;
                  maxDistance = distance;
              }
          }
          // Swap the two first vertices if the final point is in front of the
          // triangular base plane (this makes all faces of the tetrahedron point)
          // outward
          Vector4 baseFace = Points2Plane(vertices[initialSimplex[0]],
              vertices[initialSimplex[1]],
              vertices[initialSimplex[2]]);
          if (PointAbovePlane(vertices[initialSimplex[3]], baseFace))
          {
              int t = initialSimplex[0];
              initialSimplex[0] = initialSimplex[1];
              initialSimplex[1] = t;
          }
          return initialSimplex;
      }

      /// <summary>
      /// Create the first four faces and assign all the points to the first face
      /// to which they are visible.
      /// </summary>
      /// <param name="initialSimplex">The indices of the initial simplex.</param>
      /// <param name="vertices">Array of vertices.</param>
      /// <returns>The initial list of faces with their data.</returns>
      private static List<FaceData> AssignInitialPointsToFaces(
          int[] initialSimplex, Vector3[] vertices)
      {
          // Create the first four faces
          List<FaceData> faceData = new List<FaceData>();
          faceData.Add(new FaceData(initialSimplex[0], initialSimplex[1],
              initialSimplex[2], vertices));
          faceData.Add(new FaceData(initialSimplex[0], initialSimplex[2],
              initialSimplex[3], vertices));
          faceData.Add(new FaceData(initialSimplex[1], initialSimplex[3],
              initialSimplex[2], vertices));
          faceData.Add(new FaceData(initialSimplex[1], initialSimplex[0],
              initialSimplex[3], vertices));
          // Assign each point to the first face to which it is visible
          for (int ii = 0; ii < vertices.Length; ii++)
              foreach (FaceData face in faceData)
                  if (PointAbovePlane(vertices[ii], face.Plane))
                  {
                      face.VisibleIndices.Add(ii);
                      break;
                  }
          return faceData;
      }

      /// <summary>
      /// Get a face from the stack which still needs to be processed.
      /// </summary>
      /// <param name="faceStack">The list of faces.</param>
      /// <returns>The face to process next.</returns>
      private static FaceData GetUnprocessedFace(List<FaceData> faceStack)
      {
          foreach (FaceData face in faceStack)
              if (face.VisibleIndices.Count > 0)
                  return face;
          return null;
      }

      /// <summary>
      /// Find all the faces which can be seen from the given point. This is the
      /// same as if the point were a light and we were looking for all faces
      /// which the point illuminated. Faces pointing the opposite direction are
      /// ignored. These faces are removed from the stack.
      /// </summary>
      /// <param name="pt">The point in question.</param>
      /// <param name="faceStack">The list of faces to search.</param>
      /// <returns>A new list of faces.</returns>
      private static List<FaceData> ExtractVisibleFaces(Vector3 pt,
          List<FaceData> faceStack)
      {
          List<FaceData> visibleFaces = new List<FaceData>();
          foreach (FaceData face in faceStack)
              if (PointAbovePlane(pt, face.Plane))
                  visibleFaces.Add(face);
          foreach (FaceData face in visibleFaces)
              faceStack.Remove(face);
          return visibleFaces;
      }

      /// <summary>
      /// Get pairings of all the edges around the set of visible faces.
      /// This basically finds one outline around all the given faces.
      /// This is returned as a dictionary to reduce data copying between
      /// functions.
      /// </summary>
      /// <param name="visibleFaces">A list of the visible faces.</param>
      /// <returns>A dictionary with the edge pairs.</returns>
      private static Dictionary<string, int[]> ExtractHorizonEdges(
          List<FaceData> visibleFaces)
      {
          Dictionary<string, int[]> edges = new Dictionary<string, int[]>();
          foreach (FaceData face in visibleFaces)
              for (int ii = 0; ii < face.FaceIndices.Length; ii++)
              {
                  // Get indices of triangle vertices
                  int idx1 = face.FaceIndices[ii];
                  int idx2 = face.FaceIndices[(ii + 1) % face.FaceIndices.Length];
                  // Use keys to easily detect and remove duplicate edges
                  // If an edge appears twice, it is shared by two triangles,
                  // and not really an edge
                  string key = idx1 + "," + idx2;
                  string keyReversed = idx2 + "," + idx1;
                  if (edges.ContainsKey(key) || edges.ContainsKey(keyReversed))
                  {
                      edges.Remove(key);
                      edges.Remove(keyReversed);
                  }
                  else
                      edges.Add(key, new int[] { idx1, idx2 });
              }
          return edges;
      }

      /// <summary>
      /// Create new faces between the horizon edges and the maximum point.
      /// This is somewhat like extruding the edges out to the maximum point to
      /// form new faces.
      /// </summary>
      /// <param name="maxPtIndex">Point farthest from current face.</param>
      /// <param name="visibleFaces">List of current visible faces.</param>
      /// <param name="vertices">Array of vertices.</param>
      /// <param name="horizonEdges">Dictionary of pairs of indices.</param>
      /// <returns>A new list of faces.</returns>
      private static List<FaceData> CreateNewFaces(int maxPtIndex,
          List<FaceData> visibleFaces, Vector3[] vertices,
          Dictionary<string, int[]> horizonEdges)
      {
          List<FaceData> newFaces = new List<FaceData>();
          // Create new faces from horizon edges and max point
          foreach (int[] edge in horizonEdges.Values)
              newFaces.Add(new FaceData(edge[0], edge[1], maxPtIndex, vertices));
          // Assign all points off all visible faces to the first of the new
          // faces to which the point is visible
          foreach (FaceData face in visibleFaces)
              foreach (int idx in face.VisibleIndices)
                  foreach (FaceData newFace in newFaces)
                      if (PointAbovePlane(vertices[idx], newFace.Plane))
                      {
                          newFace.VisibleIndices.Add(idx);
                          break;
                      }
          return newFaces;
      }

      /// <summary>
      /// Convert three points to a plane equation of the form
      /// Ax + By + Cz + D = 0. The returned Vector4 contains A, B, C and D.
      /// </summary>
      /// <param name="pt1">The first point.</param>
      /// <param name="pt2">The second point.</param>
      /// <param name="pt3">The third point.</param>
      /// <returns>A new Vector4.</returns>
      private static Vector4 Points2Plane(Vector3 pt1, Vector3 pt2, Vector3 pt3)
      {
          Vector4 v = Vector3.Cross(pt2 - pt1, pt3 - pt1).normalized;
          v.w = -Vector3.Dot(v, pt1);
          return v;
      }

      /// <summary>
      /// Checks if the point is on the front face of the plane.
      /// This is the same as the point being able to "see" the front of the
      /// plane.
      /// </summary>
      /// <param name="point">The point in question.</param>
      /// <param name="plane">The plane in question.</param>
      /// <returns>A boolean value.</returns>
      private static bool PointAbovePlane(Vector3 point, Vector4 plane)
      {
          return Vector3.Dot(point, plane) + plane.w > 0.0000001f;
      }
  }
}