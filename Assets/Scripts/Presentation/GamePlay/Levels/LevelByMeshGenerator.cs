using System.Collections.Generic;
using System.Linq;
using Presentation.GamePlay.Levels.Interface;
using UnityEngine;

namespace Presentation.GamePlay.Levels
{
    public class LevelByMeshGenerator : MonoBehaviour, ILevel, ILevelGenerator
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _pipeStartTransform;
        [SerializeField] private Transform _puzzleExitTransform;

        [SerializeField] int _sides;
        [SerializeField] float _radiusOne;
        [SerializeField] float _radiusTwo;
        [SerializeField] bool _useWorldSpace = true;
        [SerializeField] private Material _material;

        private Mesh _outsideMesh;
        private MeshFilter _outsideMeshFilter;
        private MeshRenderer _outsideMeshRenderer;

        private Mesh _insideMesh;
        private MeshFilter _insideMeshFilter;
        private MeshRenderer _insideMeshRenderer;

        private LineRenderer _lineRenderer;
        public Rigidbody LevelRigidbody => _rigidbody;

        public void GenerateLevel(Texture2D levelArt)
        {
            GameObject lineRendererObj = new GameObject("LineRenderer");
            _lineRenderer = lineRendererObj.AddComponent<LineRenderer>();

            List<Vector2> temp = new List<Vector2>();
            List<List<Color>> colors = new List<List<Color>>();
            for (int i = 0; i < levelArt.height; i++)
            {
                colors.Add(new List<Color>());
                for (int j = 0; j < levelArt.width; j++)
                {
                    var c = levelArt.GetPixel(j, i);
                    colors[^1].Add(c);
                }
            }

            for (int i = 0; i < colors.Count; i++)
            {
                for (int j = 0; j < colors[i].Count; j++)
                {
                    var c = colors[i][j];
                    if (c != Color.clear)
                    {
                        temp.Add(new Vector2(i, j));
                        for (int k = j; k < colors[i].Count; k++)
                        {
                            if (c != Color.clear)
                            {
                                colors[i].RemoveAt(k);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            _lineRenderer.positionCount = temp.Count;
            var offset = temp[0].y;
            for (int i = 0; i < temp.Count; i++)
            {
                var t = temp[i];
                t.y -= offset;
                temp[i] = t;

                _lineRenderer.SetPosition(i, new Vector3(temp[i].x, temp[i].y, 0));
            }

            _lineRenderer.Simplify(1);

            GameObject outsideObj = new GameObject("outsideObj");
            _outsideMeshFilter = outsideObj.AddComponent<MeshFilter>();
            _outsideMeshRenderer = outsideObj.AddComponent<MeshRenderer>();
            _outsideMeshRenderer.material = _material;
            _outsideMesh = new Mesh();
            _outsideMeshFilter.mesh = _outsideMesh;
            _outsideMeshRenderer.enabled = true;
            outsideObj.transform.SetParent(transform);

            GameObject insideObj = new GameObject("insideObj");
            _insideMeshFilter = insideObj.AddComponent<MeshFilter>();
            _insideMeshRenderer = insideObj.AddComponent<MeshRenderer>();
            _insideMeshRenderer.material = _material;
            _insideMesh = new Mesh();
            _insideMeshFilter.mesh = _insideMesh;
            _insideMeshRenderer.enabled = true;
            insideObj.transform.SetParent(transform);

            _sides = Mathf.Max(3, _sides);
            GenerateMesh(_radiusOne, _outsideMesh, _outsideMeshFilter);
            GenerateMesh(_radiusTwo, _insideMesh, _insideMeshFilter);

            _puzzleExitTransform.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
            var parent = _puzzleExitTransform.parent;

            _puzzleExitTransform.SetParent(outsideObj.transform);

            outsideObj.transform.localRotation = Quaternion.Euler(0, 0, 90);
            outsideObj.transform.localScale = Vector3.one * 0.006f;

            insideObj.transform.localRotation = Quaternion.Euler(0, 0, 90);
            insideObj.transform.localScale = Vector3.one * 0.006f;

            outsideObj.transform.SetParent(_pipeStartTransform);
            insideObj.transform.SetParent(_pipeStartTransform);

            outsideObj.transform.localPosition = Vector3.zero;
            insideObj.transform.localPosition = Vector3.zero;

            _puzzleExitTransform.SetParent(parent);
            _puzzleExitTransform.transform.localScale = Vector3.one;

            _insideMesh.triangles = _insideMesh.triangles.Reverse().ToArray();
            _insideMesh.RecalculateBounds();
            _insideMesh.RecalculateNormals();

            outsideObj.AddComponent<MeshCollider>();
            insideObj.AddComponent<MeshCollider>();

            Destroy(lineRendererObj);
        }

        private void GenerateMesh(float radius, Mesh mesh, MeshFilter meshFilter)
        {
            if (mesh == null || _lineRenderer.positionCount <= 1)
            {
                return;
            }

            var verticesLength = _sides * _lineRenderer.positionCount;
            var vertices = new Vector3[verticesLength];

            var indices = GenerateIndices();
            var uvs = GenerateUVs();

            if (verticesLength > mesh.vertexCount)
            {
                mesh.vertices = vertices;
                mesh.triangles = indices;
                mesh.uv = uvs;
            }
            else
            {
                mesh.triangles = indices;
                mesh.vertices = vertices;
                mesh.uv = uvs;
            }


            var currentVertIndex = 0;

            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                var circle = CalculateCircle(i, radius);
                foreach (var vertex in circle)
                {
                    vertices[currentVertIndex++] = _useWorldSpace ? transform.InverseTransformPoint(vertex) : vertex;
                }
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        private Vector2[] GenerateUVs()
        {
            var uvs = new Vector2[_lineRenderer.positionCount * _sides];

            for (int segment = 0; segment < _lineRenderer.positionCount; segment++)
            {
                for (int side = 0; side < _sides; side++)
                {
                    var vertIndex = (segment * _sides + side);
                    var u = side / (_sides - 1f);
                    var v = segment / (_lineRenderer.positionCount - 1f);

                    uvs[vertIndex] = new Vector2(u, v);
                }
            }

            return uvs;
        }

        private int[] GenerateIndices()
        {
            // Two triangles and 3 vertices
            var indices = new int[_lineRenderer.positionCount * _sides * 2 * 3];

            var currentIndicesIndex = 0;
            for (int segment = 1; segment < _lineRenderer.positionCount; segment++)
            {
                for (int side = 0; side < _sides; side++)
                {
                    var vertIndex = (segment * _sides + side);
                    var prevVertIndex = vertIndex - _sides;

                    // Triangle one
                    indices[currentIndicesIndex++] = prevVertIndex;
                    indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                    indices[currentIndicesIndex++] = vertIndex;


                    // Triangle two
                    indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
                    indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                    indices[currentIndicesIndex++] = prevVertIndex;
                }
            }

            return indices;
        }

        private Vector3[] CalculateCircle(int index, float radius)
        {
            var dirCount = 0;
            var forward = Vector3.zero;

            // If not first index
            if (index > 0)
            {
                forward += (_lineRenderer.GetPosition(index) - _lineRenderer.GetPosition(index - 1)).normalized;
                dirCount++;
            }

            // If not last index
            if (index < _lineRenderer.positionCount - 1)
            {
                forward += (_lineRenderer.GetPosition(index + 1) - _lineRenderer.GetPosition(index)).normalized;
                dirCount++;
            }

            // Forward is the average of the connecting edges directions
            forward = (forward / dirCount).normalized;
            var side = Vector3.Cross(forward, forward + new Vector3(.123564f, .34675f, .756892f)).normalized;
            var up = Vector3.Cross(forward, side).normalized;

            var circle = new Vector3[_sides];
            var angle = 0f;
            var angleStep = (2 * Mathf.PI) / _sides;

            var t = index / (_lineRenderer.positionCount - 1f);
            var r = radius;

            for (int i = 0; i < _sides; i++)
            {
                var x = Mathf.Cos(angle);
                var y = Mathf.Sin(angle);

                circle[i] = _lineRenderer.GetPosition(index) + side * x * r + up * y * r;

                angle += angleStep;
            }

            return circle;
        }
    }
}