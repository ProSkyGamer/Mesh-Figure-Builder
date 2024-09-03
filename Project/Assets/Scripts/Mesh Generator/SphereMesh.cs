#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class SphereMesh : MonoBehaviour
{
    [SerializeField] private int resolution = 10;
    [SerializeField] private float sphereRadius = 1.5f;
    [SerializeField] private List<SphereFace> allSphereFaces;

    public void ChangeMeshStat(int newResolution, float newRadius)
    {
        resolution = newResolution;
        sphereRadius = newRadius;

        if (resolution < 6)
            resolution = 6;

        foreach (var sphereFace in allSphereFaces)
        {
            sphereFace.ChangeMeshStat(resolution, sphereRadius, Vector3.zero);
        }
    }

    private void Awake()
    {
        InitializeSphereFaces();
    }

    private void InitializeSphereFaces()
    {
        foreach (var sphereFace in allSphereFaces)
        {
            sphereFace.InitializeSphereFace(resolution, sphereRadius, Vector3.zero);
        }
    }

    public Vector3 GetObjectOffsetToGround()
    {
        var offsetToGround = new Vector3(0f, sphereRadius, 0f);

        return offsetToGround;
    }

    public void GetMeshStats(out float radius, out int edgesCount)
    {
        radius = sphereRadius;
        edgesCount = resolution;
    }

    public void ChangeMeshMaterial(Material newMaterial)
    {
        foreach (var sphereFace in allSphereFaces)
        {
            sphereFace.ChangeMeshMaterial(newMaterial);
        }
    }
}