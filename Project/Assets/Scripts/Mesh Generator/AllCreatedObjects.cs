#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class AllCreatedObjects : MonoBehaviour
{
    public static AllCreatedObjects Instance { get; private set; }

    public enum ObjectTypes
    {
        Parallelepiped,
        Prism,
        Sphere,
        Capsule
    }

    [SerializeField] private CreatedObject parallelepipedPrefab;
    [SerializeField] private CreatedObject prismPrefab;
    [SerializeField] private CreatedObject spherePrefab;
    [SerializeField] private CreatedObject capsulePrefab;
    [SerializeField] private Vector3 maxObjectCoords = new Vector3(50f, 0f, 50f);
    [SerializeField] private Vector3 minObjectCoords = new Vector3(-50f, 0f, -50f);

    private readonly List<CreatedObject> allCreatedObjects = new();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void CreateObject(ObjectTypes objectType, out CreatedObject createdObject)
    {
        Transform creatingObjectPrefab;
        var creatingObjectOffset = Vector3.zero;
        var objectName = $"{objectType.ToString()} {Random.Range(0, 1000)}";

        switch (objectType)
        {
            default:
            case ObjectTypes.Parallelepiped:
                creatingObjectPrefab = parallelepipedPrefab.transform;
                creatingObjectOffset = creatingObjectPrefab.gameObject.GetComponent<ParallelepipedMesh>().GetObjectOffsetToGround();
                break;
            case ObjectTypes.Prism:
                creatingObjectPrefab = prismPrefab.transform;
                creatingObjectOffset = creatingObjectPrefab.gameObject.GetComponent<PrismMesh>().GetObjectOffsetToGround();
                break;
            case ObjectTypes.Sphere:
                creatingObjectPrefab = spherePrefab.transform;
                creatingObjectOffset = creatingObjectPrefab.gameObject.GetComponent<SphereMesh>().GetObjectOffsetToGround();
                break;
            case ObjectTypes.Capsule:
                creatingObjectPrefab = capsulePrefab.transform;
                creatingObjectOffset = creatingObjectPrefab.gameObject.GetComponent<CapsuleMesh>().GetObjectOffsetToGround();
                break;
        }

        var newCreatedObject = Instantiate(creatingObjectPrefab, creatingObjectOffset, Quaternion.identity, transform);
        newCreatedObject.gameObject.name = objectName;
        createdObject = newCreatedObject.GetComponent<CreatedObject>();

        allCreatedObjects.Add(createdObject);
    }

    public void DeleteObject(CreatedObject deletingObject)
    {
        Destroy(deletingObject.gameObject);
    }

    public void ModifyObject(CreatedObject modifyingObject, ObjectTypes newObjectType, Material newObjectMaterial, string objectName,
        Vector3 objectPosition, float length, float width, float height, float radius, int edgesCount, out CreatedObject newCreatedObject)
    {
        newCreatedObject = modifyingObject;
        var modifyingObjectIndex = allCreatedObjects.IndexOf(modifyingObject);
        objectPosition.y = 0f;

        if (objectPosition.x > maxObjectCoords.x)
            objectPosition.x = maxObjectCoords.x;
        else if (objectPosition.x < minObjectCoords.x)
            objectPosition.x = minObjectCoords.x;

        if (objectPosition.z > maxObjectCoords.z)
            objectPosition.z = maxObjectCoords.z;
        else if (objectPosition.z < minObjectCoords.z)
            objectPosition.z = minObjectCoords.z;

        if (modifyingObject.GetObjectType() != newObjectType)
        {
            Destroy(modifyingObject.gameObject);
            CreateObject(newObjectType, out newCreatedObject);
            allCreatedObjects[modifyingObjectIndex] = newCreatedObject;
        }

        newCreatedObject.gameObject.name = objectName;
        newCreatedObject.transform.position = Vector3.zero;
        var additionalObjectOffset = Vector3.zero;
        switch (newObjectType)
        {
            default:
            case ObjectTypes.Parallelepiped:
                var newCreatedObjectParallelepiped = newCreatedObject.gameObject.GetComponent<ParallelepipedMesh>();
                newCreatedObjectParallelepiped.ChangeMeshStat(length, width, height);
                newCreatedObjectParallelepiped.ChangeMeshMaterial(newObjectMaterial);
                additionalObjectOffset = newCreatedObjectParallelepiped.GetObjectOffsetToGround();
                break;
            case ObjectTypes.Prism:
                var newCreatedObjectPrism = newCreatedObject.gameObject.GetComponent<PrismMesh>();
                newCreatedObjectPrism.ChangeMeshStat(height, radius, edgesCount);
                newCreatedObjectPrism.ChangeMeshMaterial(newObjectMaterial);
                additionalObjectOffset = newCreatedObjectPrism.GetObjectOffsetToGround();
                break;
            case ObjectTypes.Sphere:
                var newCreatedObjectSphere = newCreatedObject.gameObject.GetComponent<SphereMesh>();
                newCreatedObjectSphere.ChangeMeshStat(edgesCount, radius);
                newCreatedObjectSphere.ChangeMeshMaterial(newObjectMaterial);
                additionalObjectOffset = newCreatedObjectSphere.GetObjectOffsetToGround();
                break;
            case ObjectTypes.Capsule:
                var newCreatedObjectCapsule = newCreatedObject.gameObject.GetComponent<CapsuleMesh>();
                newCreatedObjectCapsule.ChangeMeshStat(edgesCount, radius, height);
                newCreatedObjectCapsule.ChangeMeshMaterial(newObjectMaterial);
                additionalObjectOffset = newCreatedObjectCapsule.GetObjectOffsetToGround();
                break;
        }

        newCreatedObject.transform.position = objectPosition + additionalObjectOffset;
    }
}