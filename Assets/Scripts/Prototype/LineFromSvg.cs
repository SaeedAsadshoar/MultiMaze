using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class LineFromSvg : MonoBehaviour
    {
        LineRenderer LR;

        // Use this for initialization
        void Start()
        {
            LineRenderer LR = gameObject.AddComponent<LineRenderer>();
            LR.positionCount = 10;
            //LR.startWidth = 0.1f;
            //LR.endWidth = 0.1f;

            for (int i = 0; i < 10; i++)
            {
                LR.SetPosition(i, new Vector3(Random.Range(0,10), i, 0));
            }

            generateMesh(LR);
        }

        /// <summary>
        /// Return a mesh based on a linerenderer
        /// </summary>
        private Mesh generateMesh(LineRenderer LR)
        {
            Mesh mesh = new Mesh();

            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

            Vector3[] vertices = generateVertices(LR, 8, 1);
            int[] triangles = generateTriangles(vertices);

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }

        /// <summary>
        /// return an array of vertices based on a Linerenderer
        /// </summary>
        private Vector3[] generateVertices(LineRenderer LR, int nbVerticesPerLevel, float radius)
        {
            List<Vector3> result = new List<Vector3>();
            float angle;

            for (int i = 0; i < LR.positionCount; i++)
            {
                for (int j = 0; j < nbVerticesPerLevel; j++)
                {
                    angle = j * Mathf.PI * 2f / nbVerticesPerLevel;
                    result.Add(new Vector3(Mathf.Cos(angle) * radius, LR.GetPosition(i).y, Mathf.Sin(angle) * radius));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// return an array of triangle based on an array of vertices
        /// </summary>
        private int[] generateTriangles(Vector3[] Vertices)
        {
            if (Vertices.Length % 8 != 0)
            {
                Debug.LogError("Wrong vertices number input !?");
                return null;
            }

            int nbLevel = Vertices.Length / 8;

            List<int> triangles = new List<int>();

            for (int i = 0; i < nbLevel - 1; i++) //iterate over the different Level (1 lvl = 8 vertices)
            {
                for (int j = 0; j < 7; j++) //iterate over the vertices from 0 to 6
                {
                    // triangle 1 :
                    triangles.Add(i * 8 + j + 1);
                    triangles.Add(i * 8 + j);
                    triangles.Add(i * 8 + j + 8);

                    // triangle 2:
                    triangles.Add(i * 8 + j + 9);
                    triangles.Add(i * 8 + j + 1);
                    triangles.Add(i * 8 + j + 8);
                }

                //create the last 2 triangles of the lvl (vertex 7)
                // triangle 1 :
                triangles.Add(i * 8 + 0);
                triangles.Add(i * 8 + 7);
                triangles.Add(i * 8 + 15);

                // triangle 2:
                triangles.Add(i * 8 + 8);
                triangles.Add(i * 8 + 0);
                triangles.Add(i * 8 + 15);
            }

            return triangles.ToArray();
        }
    }
}