#region

using System.Collections.Generic;
using UnityEngine;

#endregion

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ParallelepipedMesh : MonoBehaviour
{
    private enum ParallelepipedMeshEdgeType
    {
        Front,
        Bottom,
        Left,
        Top,
        Right,
        Back
    }

    private class MeshEdgePrefab
    {
        public ParallelepipedMeshEdgeType parallelepipedMeshEdgeType;
        public Vector3 meshMainAxis;
        public Vector3 meshSecondaryAxis;
    }


    private readonly List<MeshEdgePrefab> parallelepipedMeshEdgePrefabs = new List<MeshEdgePrefab>
    {
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.X],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Y],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Front
        },
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Z],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.X],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Bottom
        },
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Z],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Y],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Left
        },
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Z],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.X],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Top
        },
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Z],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Y],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Right
        },
        new MeshEdgePrefab
        {
            meshMainAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.X],
            meshSecondaryAxis = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Y],
            parallelepipedMeshEdgeType = ParallelepipedMeshEdgeType.Back
        }
    };

    [SerializeField] private float length = 1.5f;
    [SerializeField] private float width = 1.5f;
    [SerializeField] private float height = 1.5f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void ChangeMeshStat(float newLength, float newWidth, float newHeight)
    {
        length = newLength;
        width = newWidth;
        height = newHeight;

        var newPrismMesh = GenerateParallelepipedMesh(length, width, height);

        meshFilter.mesh = newPrismMesh;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        var parallelepipedMesh = GenerateParallelepipedMesh(length, width, height);

        meshFilter.mesh = parallelepipedMesh;
    }

    private Mesh GenerateParallelepipedMesh(float length, float width, float height)
    {
        var generatingMesh = new Mesh();
        Dictionary<Vector3, int> associatedCorners = new();

        var allGeneratedEdgeCorners = new List<Vector3>();
        var allGeneratedTriangles = new List<int>();

        foreach (var parallelepipedMeshEdge in parallelepipedMeshEdgePrefabs)
        {
            //Front -> Bottom -> Left -> Top -> Right -> Back

            var meshCenter = Vector3.zero;
            var mainAxisLength = 0f;
            var secondaryAxisLength = 0f;

            switch (parallelepipedMeshEdge.parallelepipedMeshEdgeType)
            {
                default:
                case ParallelepipedMeshEdgeType.Front:
                    meshCenter += new Vector3(0f, 0f, -width / 2f);
                    mainAxisLength = length;
                    secondaryAxisLength = height;
                    break;
                case ParallelepipedMeshEdgeType.Bottom:
                    meshCenter += new Vector3(0f, -height / 2f, 0f);
                    mainAxisLength = width;
                    secondaryAxisLength = length;
                    break;
                case ParallelepipedMeshEdgeType.Left:
                    meshCenter += new Vector3(-length / 2f, 0f, 0f);
                    mainAxisLength = width;
                    secondaryAxisLength = height;
                    break;
                case ParallelepipedMeshEdgeType.Top:
                    meshCenter += new Vector3(0f, height / 2f, 0f);
                    mainAxisLength = width;
                    secondaryAxisLength = length;
                    break;
                case ParallelepipedMeshEdgeType.Right:
                    meshCenter += new Vector3(length / 2f, 0f, 0f);
                    mainAxisLength = width;
                    secondaryAxisLength = height;
                    break;
                case ParallelepipedMeshEdgeType.Back:
                    meshCenter += new Vector3(0f, 0f, width / 2f);
                    mainAxisLength = length;
                    secondaryAxisLength = height;
                    break;
            }

            GetMeshEdge(meshCenter, parallelepipedMeshEdge.meshMainAxis, mainAxisLength, parallelepipedMeshEdge.meshSecondaryAxis,
                secondaryAxisLength, parallelepipedMeshEdge.parallelepipedMeshEdgeType, out var edgeCorners, out var edgeTriangles);

            foreach (var edgeCorner in edgeCorners)
            {
                if (!associatedCorners.ContainsKey(edgeCorner))
                {
                    associatedCorners.TryAdd(edgeCorner, associatedCorners.Keys.Count);
                    allGeneratedEdgeCorners.Add(edgeCorner);
                }
            }

            foreach (var edgeTriangle in edgeTriangles)
            {
                var globalEdgeTriangleNumber = associatedCorners[edgeCorners[edgeTriangle]];
                allGeneratedTriangles.Add(globalEdgeTriangleNumber);
            }
        }

        generatingMesh.SetVertices(allGeneratedEdgeCorners);

        var trianglesArray = new int[allGeneratedTriangles.Count];
        for (var i = 0; i < allGeneratedTriangles.Count; i++)
        {
            var generatedTriangle = allGeneratedTriangles[i];
            trianglesArray[i] = generatedTriangle;
        }

        generatingMesh.triangles = trianglesArray;

        return generatingMesh;
    }

    private void GetMeshEdge(Vector3 meshCenter, Vector3 meshMainAxis, float mainAxisLength, Vector3 meshSecondaryAxis, float secondaryAxisLength,
        ParallelepipedMeshEdgeType meshEdgeType,
        out List<Vector3> meshCorners, out List<int> meshTriangles)
    {
        meshCorners = new();
        meshMainAxis = meshMainAxis.normalized;
        meshSecondaryAxis = meshSecondaryAxis.normalized;

        switch (meshEdgeType)
        {
            default:
            case ParallelepipedMeshEdgeType.Front:
            case ParallelepipedMeshEdgeType.Right:
            case ParallelepipedMeshEdgeType.Bottom:
                meshTriangles = new()
                {
                    0, 1, 2, 2, 1, 3
                };
                break;
            case ParallelepipedMeshEdgeType.Back:
            case ParallelepipedMeshEdgeType.Left:
            case ParallelepipedMeshEdgeType.Top:
                meshTriangles = new()
                {
                    2, 1, 0, 3, 1, 2
                };
                break;
        }

        var bottomRightCorner = meshCenter + meshMainAxis * mainAxisLength / 2 - meshSecondaryAxis * secondaryAxisLength / 2;
        var bottomLeftCorner = meshCenter - meshMainAxis * mainAxisLength / 2 - meshSecondaryAxis * secondaryAxisLength / 2;
        var topRightCorner = meshCenter + meshMainAxis * mainAxisLength / 2 + meshSecondaryAxis * secondaryAxisLength / 2;
        var topLeftCorner = meshCenter - meshMainAxis * mainAxisLength / 2 + meshSecondaryAxis * secondaryAxisLength / 2;

        meshCorners.Add(bottomRightCorner);
        meshCorners.Add(bottomLeftCorner);
        meshCorners.Add(topRightCorner);
        meshCorners.Add(topLeftCorner);
    }

    public Vector3 GetObjectOffsetToGround()
    {
        var offsetToGround = new Vector3(0f, height / 2, 0f);

        return offsetToGround;
    }

    public void GetMeshStats(out float length, out float width, out float height)
    {
        length = this.length;
        width = this.width;
        height = this.height;
    }

    public void ChangeMeshMaterial(Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }
}