#region

using System.Collections.Generic;
using UnityEngine;

#endregion

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PrismMesh : MonoBehaviour
{
    [SerializeField] private float height = 1.5f;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private int edgesCount = 6;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void ChangeMeshStat(float newHeight, float newRadius, int newEdgesCount)
    {
        height = newHeight;
        radius = newRadius;
        edgesCount = newEdgesCount;

        if (edgesCount < 3)
            edgesCount = 3;

        var newPrismMesh = GeneratePrismMesh(height, edgesCount, radius);

        meshFilter.mesh = newPrismMesh;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        var prismMesh = GeneratePrismMesh(height, edgesCount, radius);

        meshFilter.mesh = prismMesh;
    }

    private Mesh GeneratePrismMesh(float height, int edgesCount, float radius)
    {
        var generatingMesh = new Mesh();
        Dictionary<Vector3, int> associatedCorners = new();

        var allGeneratedEdgeCorners = new List<Vector3>();
        var allGeneratedTriangles = new List<int>();

        var bottomCenterPoint = new Vector3(0f, -height / 2, 0f);
        var topCenterPoint = new Vector3(0f, height / 2, 0f);
        var fullAnglesSumm = (edgesCount - 2) * 180;
        var currentAngle = 0f;
        var rotatingAngle = 180 - (float)fullAnglesSumm / edgesCount;
        var rotatingAngleRadiant = (180 - (float)fullAnglesSumm / edgesCount) / 2 * Mathf.PI / 180;
        var currentAngleInRadians = (fullAnglesSumm - rotatingAngle) * Mathf.PI / 180;
        var secondaryAxisLength = radius * Mathf.Sqrt(2 * (1 - Mathf.Cos(currentAngleInRadians)));
        var smallRadius = radius * Mathf.Cos(rotatingAngleRadiant);

        associatedCorners.TryAdd(bottomCenterPoint, associatedCorners.Keys.Count);
        allGeneratedEdgeCorners.Add(bottomCenterPoint);
        associatedCorners.TryAdd(topCenterPoint, associatedCorners.Keys.Count);
        allGeneratedEdgeCorners.Add(topCenterPoint);

        for (var i = 0; i < edgesCount; i++)
        {
            var mainAxisDirection = CreatedObject.axisDirections[CreatedObject.VectorDirectionTypes.Y];

            currentAngleInRadians = currentAngle * Mathf.PI / 180;

            var secondaryAxisDirection = new Vector3(Mathf.Cos(currentAngleInRadians), 0f, Mathf.Sin(currentAngleInRadians));
            secondaryAxisDirection = secondaryAxisDirection.normalized;

            var edgeCenter = secondaryAxisDirection * smallRadius;

            secondaryAxisDirection = new Vector3(secondaryAxisDirection.z, 0f, -secondaryAxisDirection.x);

            GetMeshEdge(edgeCenter, mainAxisDirection, height,
                secondaryAxisDirection, secondaryAxisLength,
                out var edgeCorners, out var edgeTriangles);

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

            var bottomTriangles = new List<int>
            {
                associatedCorners[edgeCorners[1]],
                associatedCorners[bottomCenterPoint],
                associatedCorners[edgeCorners[3]]
            };

            var topTriangles = new List<int>
            {
                associatedCorners[edgeCorners[2]],
                associatedCorners[topCenterPoint],
                associatedCorners[edgeCorners[0]]
            };

            allGeneratedTriangles.AddRange(bottomTriangles);
            allGeneratedTriangles.AddRange(topTriangles);

            currentAngle += rotatingAngle;
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
        out List<Vector3> meshCorners, out List<int> meshTriangles)
    {
        meshCorners = new();
        meshTriangles = new()
        {
            0, 1, 2, 2, 1, 3
        };
        meshMainAxis = meshMainAxis.normalized;
        meshSecondaryAxis = meshSecondaryAxis.normalized;

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

    public void GetMeshStats(out float height, out float radius, out int edgesCount)
    {
        height = this.height;
        radius = this.radius;
        edgesCount = this.edgesCount;
    }

    public void ChangeMeshMaterial(Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }
}