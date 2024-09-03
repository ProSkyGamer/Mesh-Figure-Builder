#region

using System.Collections.Generic;
using UnityEngine;

#endregion

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CapsuleMesh : MonoBehaviour
{
    [SerializeField] private int resolution = 10;
    [SerializeField] private float capsuleRadius = .5f;
    [SerializeField] private float capsuleHeight = 2.5f;
    [SerializeField] private List<SphereFace> topSphereFaces;
    [SerializeField] private List<SphereFace> bottomSphereFaces;
    private Vector3 spheresOffset;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private const float ADDITIONAL_SPHERE_OFFSET_MULTIPLIER = 1.2f;

    public void ChangeMeshStat(int newResolution, float newRadius, float newHeight)
    {
        resolution = newResolution;
        capsuleRadius = newRadius;
        capsuleHeight = newHeight;

        if (resolution < 6)
            resolution = 6;

        spheresOffset = new Vector3(0f, (capsuleHeight - capsuleRadius) / 2, 0f);

        foreach (var sphereFace in topSphereFaces)
        {
            sphereFace.ChangeMeshStat(resolution, capsuleRadius, spheresOffset * (ADDITIONAL_SPHERE_OFFSET_MULTIPLIER + 1f));
        }

        foreach (var sphereFace in bottomSphereFaces)
        {
            sphereFace.ChangeMeshStat(resolution, capsuleRadius, spheresOffset * (ADDITIONAL_SPHERE_OFFSET_MULTIPLIER - 1f));
        }

        var centralMesh = GetCentralMesh(capsuleRadius, resolution);

        meshFilter.mesh = centralMesh;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        InitializeCapsuleMesh();
    }

    private void InitializeSphereFaces()
    {
        foreach (var sphereFace in topSphereFaces)
        {
            sphereFace.InitializeSphereFace(resolution, capsuleRadius, spheresOffset * (ADDITIONAL_SPHERE_OFFSET_MULTIPLIER + 1f));
        }

        foreach (var sphereFace in bottomSphereFaces)
        {
            sphereFace.InitializeSphereFace(resolution, capsuleRadius, spheresOffset * (ADDITIONAL_SPHERE_OFFSET_MULTIPLIER - 1f));
        }
    }

    private void InitializeCapsuleMesh()
    {
        spheresOffset = new Vector3(0f, (capsuleHeight - capsuleRadius) / 2, 0f);
        InitializeSphereFaces();

        var centralMesh = GetCentralMesh(capsuleRadius, resolution);

        meshFilter.mesh = centralMesh;
    }

    private Mesh GetCentralMesh(float radius, int circleIterations)
    {
        var generatingMesh = new Mesh();

        var vertexColumns = circleIterations + 1;
        var vertexRows = 2;

        var verticesCount = vertexColumns * vertexRows;
        var trianglesCount = circleIterations * 2;
        var numCapTris = circleIterations - 2;
        var trisArrayLength = (trianglesCount + numCapTris * 2) * 3;

        var allVertices = new Vector3[verticesCount];
        var allTriangles = new int[trisArrayLength];

        var heightStep = (capsuleHeight - capsuleRadius) * ADDITIONAL_SPHERE_OFFSET_MULTIPLIER;
        var angleStep = 2 * Mathf.PI / circleIterations;

        for (var y = 0; y < vertexRows; y++)
        for (var x = 0; x < vertexColumns; x++)
        {
            var angle = x * angleStep;

            if (x == vertexColumns - 1)
                angle = 0;

            allVertices[y * vertexColumns + x] = new Vector3(radius * Mathf.Cos(angle), y * heightStep, radius * Mathf.Sin(angle));

            if (y != 0 && x < vertexColumns - 1)
            {
                var baseIndex = numCapTris * 3 + (y - 1) * circleIterations * 6 + x * 6;

                allTriangles[baseIndex + 0] = y * vertexColumns + x;
                allTriangles[baseIndex + 1] = y * vertexColumns + x + 1;
                allTriangles[baseIndex + 2] = (y - 1) * vertexColumns + x;

                allTriangles[baseIndex + 3] = (y - 1) * vertexColumns + x;
                allTriangles[baseIndex + 4] = y * vertexColumns + x + 1;
                allTriangles[baseIndex + 5] = (y - 1) * vertexColumns + x + 1;
            }
        }

        generatingMesh.SetVertices(allVertices);

        generatingMesh.triangles = allTriangles;

        return generatingMesh;
    }

    public Vector3 GetObjectOffsetToGround()
    {
        var offsetToGround = new Vector3(0f, capsuleRadius, 0f);

        return offsetToGround;
    }

    public void GetMeshStats(out float height, out float radius, out int edgesCount)
    {
        height = capsuleHeight;
        radius = capsuleRadius;
        edgesCount = resolution;
    }

    public void ChangeMeshMaterial(Material newMaterial)
    {
        meshRenderer.material = newMaterial;

        foreach (var sphereFace in topSphereFaces)
        {
            sphereFace.ChangeMeshMaterial(newMaterial);
        }

        foreach (var sphereFace in bottomSphereFaces)
        {
            sphereFace.ChangeMeshMaterial(newMaterial);
        }
    }
}