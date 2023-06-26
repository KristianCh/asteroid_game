using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Combat.Asteroid
{
    public class MeshGenerator : MonoBehaviour
    {
        public int Subdivisions = 1;
        public float Size = 0.5f;

        private struct TriangleIndices
        {
            public int v1;
            public int v2;
            public int v3;

            public TriangleIndices(int v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }
        }

        // return index of point in the middle of p1 and p2
        private static int GetMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
        {
            // first check if we have it already
            var firstIsSmaller = p1 < p2;
            long smallerIndex = firstIsSmaller ? p1 : p2;
            long greaterIndex = firstIsSmaller ? p2 : p1;
            var key = (smallerIndex << 32) + greaterIndex;

            int ret;
            if (cache.TryGetValue(key, out ret))
            {
                return ret;
            }

            // not in cache, calculate it
            var point1 = vertices[p1];
            var point2 = vertices[p2];
            var middle = new Vector3(
                (point1.x + point2.x) / 2f,
                (point1.y + point2.y) / 2f,
                (point1.z + point2.z) / 2f
            );

            // add vertex makes sure point is on unit sphere
            var i = vertices.Count;
            vertices.Add(middle.normalized * radius);

            // store it, return index
            cache.Add(key, i);

            return i;
        }

        public void Create()
        {
            var filter = gameObject.GetComponent<MeshFilter>();
            var mesh = filter.mesh;
            mesh.Clear();

            var vertList = new List<Vector3>();
            var middlePointIndexCache = new Dictionary<long, int>();

            // create 12 vertices of a icosahedron
            var t = (1f + Mathf.Sqrt(5f)) / 2f;

            vertList.Add(new Vector3(-1f, t, 0f).normalized * Size);
            vertList.Add(new Vector3(1f, t, 0f).normalized * Size);
            vertList.Add(new Vector3(-1f, -t, 0f).normalized * Size);
            vertList.Add(new Vector3(1f, -t, 0f).normalized * Size);

            vertList.Add(new Vector3(0f, -1f, t).normalized * Size);
            vertList.Add(new Vector3(0f, 1f, t).normalized * Size);
            vertList.Add(new Vector3(0f, -1f, -t).normalized * Size);
            vertList.Add(new Vector3(0f, 1f, -t).normalized * Size);

            vertList.Add(new Vector3(t, 0f, -1f).normalized * Size);
            vertList.Add(new Vector3(t, 0f, 1f).normalized * Size);
            vertList.Add(new Vector3(-t, 0f, -1f).normalized * Size);
            vertList.Add(new Vector3(-t, 0f, 1f).normalized * Size);


            // create 20 triangles of the icosahedron
            List<TriangleIndices> faces = new List<TriangleIndices>
            {
                // 5 faces around point 0
                new TriangleIndices(0, 11, 5),
                new TriangleIndices(0, 5, 1),
                new TriangleIndices(0, 1, 7),
                new TriangleIndices(0, 7, 10),
                new TriangleIndices(0, 10, 11),
                // 5 adjacent faces
                new TriangleIndices(1, 5, 9),
                new TriangleIndices(5, 11, 4),
                new TriangleIndices(11, 10, 2),
                new TriangleIndices(10, 7, 6),
                new TriangleIndices(7, 1, 8),
                // 5 faces around point 3
                new TriangleIndices(3, 9, 4),
                new TriangleIndices(3, 4, 2),
                new TriangleIndices(3, 2, 6),
                new TriangleIndices(3, 6, 8),
                new TriangleIndices(3, 8, 9),
                // 5 adjacent faces
                new TriangleIndices(4, 9, 5),
                new TriangleIndices(2, 4, 11),
                new TriangleIndices(6, 2, 10),
                new TriangleIndices(8, 6, 7),
                new TriangleIndices(9, 8, 1)
            };


            // refine triangles
            for (var i = 0; i < Subdivisions; i++)
            {
                List<TriangleIndices> faces2 = new List<TriangleIndices>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    var a = GetMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, Size);
                    var b = GetMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, Size);
                    var c = GetMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, Size);

                    faces2.Add(new TriangleIndices(tri.v1, a, c));
                    faces2.Add(new TriangleIndices(tri.v2, b, a));
                    faces2.Add(new TriangleIndices(tri.v3, c, b));
                    faces2.Add(new TriangleIndices(a, b, c));
                }
                faces = faces2;
            }

            var vertListOffset = new List<Vector3>();
            foreach (var vert in vertList)
            {
                vertListOffset.Add(vert + vert * Random.Range(-Size / 2, Size / 2));
            }

            mesh.vertices = vertListOffset.ToArray();

            var triList = new List<int>();
            for (var i = 0; i < faces.Count; i++)
            {
                triList.Add(faces[i].v1);
                triList.Add(faces[i].v2);
                triList.Add(faces[i].v3);
            }
            mesh.triangles = triList.ToArray();
            mesh.uv = new Vector2[mesh.vertices.Length];

            var normals = new Vector3[vertList.Count];
            for (var i = 0; i < normals.Length; i++)
                normals[i] = vertList[i].normalized;


            mesh.normals = normals;

            mesh.RecalculateBounds();
            mesh.Optimize();
        }
    }
}