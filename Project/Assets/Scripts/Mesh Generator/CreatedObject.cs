#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class CreatedObject : MonoBehaviour
{
    public enum VectorDirectionTypes
    {
        X,
        Z,
        Y
    }

    public static readonly Dictionary<VectorDirectionTypes, Vector3> axisDirections = new()
    {
        { VectorDirectionTypes.X, new Vector3(1f, 0f, 0f) },
        { VectorDirectionTypes.Z, new Vector3(0f, 0f, 1f) },
        { VectorDirectionTypes.Y, new Vector3(0f, 1f, 0f) }
    };

    private AllCreatedObjects.ObjectTypes currentObjectType;
    private PrismMesh prismMesh;
    private ParallelepipedMesh parallelepipedMesh;
    private SphereMesh sphereMesh;
    private CapsuleMesh capsuleMesh;

    private void Awake()
    {
        prismMesh = GetComponent<PrismMesh>();
        if (prismMesh != null)
            currentObjectType = AllCreatedObjects.ObjectTypes.Prism;

        parallelepipedMesh = GetComponent<ParallelepipedMesh>();
        if (parallelepipedMesh != null)
            currentObjectType = AllCreatedObjects.ObjectTypes.Parallelepiped;

        sphereMesh = GetComponent<SphereMesh>();
        if (sphereMesh != null)
            currentObjectType = AllCreatedObjects.ObjectTypes.Sphere;

        capsuleMesh = GetComponent<CapsuleMesh>();
        if (capsuleMesh != null)
            currentObjectType = AllCreatedObjects.ObjectTypes.Capsule;
    }

    public AllCreatedObjects.ObjectTypes GetObjectType()
    {
        return currentObjectType;
    }

    public void GetMeshStats(out float length, out float width, out float height, out float radius,
        out int edgesCount)
    {
        length = 0f;
        width = 0f;
        height = 0f;
        radius = 0f;
        edgesCount = 0;

        switch (currentObjectType)
        {
            default:
            case AllCreatedObjects.ObjectTypes.Parallelepiped:
                parallelepipedMesh.GetMeshStats(out length, out width, out height);
                break;
            case AllCreatedObjects.ObjectTypes.Prism:
                prismMesh.GetMeshStats(out height, out radius, out edgesCount);
                break;
            case AllCreatedObjects.ObjectTypes.Sphere:
                sphereMesh.GetMeshStats(out radius, out edgesCount);
                break;
            case AllCreatedObjects.ObjectTypes.Capsule:
                capsuleMesh.GetMeshStats(out height, out radius, out edgesCount);
                break;
        }
    }
}