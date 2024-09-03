#region

using UnityEngine;

#endregion

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SphereFace : MonoBehaviour
{
    [SerializeField] private Vector3 sphereFaceDirection;
    [SerializeField] private bool isHalfSphere;
    [SerializeField] private bool isAxisX;
    [SerializeField] private bool isBottomHalf;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private int sphereResolution;
    private float sphereRadius;

    public void ChangeMeshStat(int resolution, float radius, Vector3 additionalOffset)
    {
        sphereResolution = resolution;
        sphereRadius = radius;

        InitializeSphereFace(sphereResolution, sphereRadius, additionalOffset);
    }

    public void InitializeSphereFace(int resolution, float radius, Vector3 additionalOffset)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh.Clear();
        sphereResolution = resolution;
        sphereRadius = radius;

        var sphereFaceMesh = isHalfSphere
            ? GetHalfSphereFaceMesh(sphereResolution, sphereFaceDirection, isAxisX, isBottomHalf, additionalOffset)
            : GetSphereFaceMesh(sphereResolution, sphereFaceDirection, additionalOffset);

        meshFilter.mesh = sphereFaceMesh;
    }

    private Mesh GetSphereFaceMesh(int resolution, Vector3 localUp, Vector3 additionalOffset)
    {
        // localUp - Direction from which creating the face
        var creatingMesh = new Mesh();
        var axisA = new Vector3(localUp.y, localUp.z, localUp.x); //Second axis for direction creating face
        var axisB = Vector3.Cross(localUp, axisA); //Perpendicular for previous axis

        var vertices = new Vector3[resolution * resolution];
        var trianglesInSquare = 2;
        var trianglesVertices = 3;
        var triangles = new int[(resolution - 1) * (resolution - 1) * trianglesInSquare * trianglesVertices];
        var triIndex = 0;

        for (var y = 0; y < resolution; y++)
        for (var x = 0; x < resolution; x++)
        {
            var i = x + y * resolution;
            var percent = new Vector2(x, y) / (resolution - 1);

            var pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
            var pointOnUnitSphere = pointOnUnitCube.normalized * sphereRadius + additionalOffset;
            vertices[i] = pointOnUnitSphere;

            if (x != resolution - 1 && y != resolution - 1)
            {
                triangles[triIndex] = i;
                triangles[triIndex + 1] = i + resolution + 1;
                triangles[triIndex + 2] = i + resolution;

                triangles[triIndex + 3] = i;
                triangles[triIndex + 4] = i + 1;
                triangles[triIndex + 5] = i + resolution + 1;
                triIndex += 6;
            }
        }

        creatingMesh.vertices = vertices;
        creatingMesh.triangles = triangles;
        creatingMesh.RecalculateNormals();

        return creatingMesh;
    }

    private Mesh GetHalfSphereFaceMesh(int resolution, Vector3 localUp, bool isAxisX, bool isBottomHalf, Vector3 additionalOffset)
    {
        // localUp - Direction from which creating the face
        var creatingMesh = new Mesh();
        var axisA = new Vector3(localUp.y, localUp.z, localUp.x); //Second axis for direction creating face
        var axisB = Vector3.Cross(localUp, axisA); //Perpendicular for previous axis

        var vertices = new Vector3[resolution * resolution];
        var trianglesInSquare = 2;
        var trianglesVertices = 3;
        var triangles = new int[(resolution - 1) * (resolution - 1) * trianglesInSquare * trianglesVertices];
        var triIndex = 0;

        var startingY = isAxisX ? 0 : isBottomHalf ? resolution / 2 : 0;
        var comparingY = isAxisX ? resolution : isBottomHalf ? resolution : resolution / 2;

        var startingX = !isAxisX ? 0 : isBottomHalf ? resolution / 2 : 0;
        var comparingX = !isAxisX ? resolution : isBottomHalf ? resolution : resolution / 2;

        for (var y = startingY; y < comparingY; y++)
        for (var x = startingX; x < comparingX; x++)
        {
            var i = x + y * resolution;
            var percent = new Vector2(x, y) / (resolution - 1);

            var pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
            var pointOnUnitSphere = pointOnUnitCube.normalized * sphereRadius + additionalOffset;
            vertices[i] = pointOnUnitSphere;

            if (x != comparingX - 1 && y != comparingY - 1)
            {
                triangles[triIndex] = i;
                triangles[triIndex + 1] = i + resolution + 1;
                triangles[triIndex + 2] = i + resolution;

                triangles[triIndex + 3] = i;
                triangles[triIndex + 4] = i + 1;
                triangles[triIndex + 5] = i + resolution + 1;
                triIndex += 6;
            }
        }

        creatingMesh.vertices = vertices;
        creatingMesh.triangles = triangles;
        creatingMesh.RecalculateNormals();

        return creatingMesh;
    }

    public void ChangeMeshMaterial(Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }
}