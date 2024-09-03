#region

using UnityEngine;

#endregion

public class EditableStatsTypeUI : MonoBehaviour
{
    [SerializeField] private Transform parallelepipedStatsTab;
    [SerializeField] private InputFieldFilterUI parallelepipedLengthFiltratedInputField;
    [SerializeField] private InputFieldFilterUI parallelepipedWidthFiltratedInputField;
    [SerializeField] private InputFieldFilterUI parallelepipedHeightFiltratedInputField;

    [SerializeField] private Transform prismStatsTab;
    [SerializeField] private InputFieldFilterUI prismHeightFiltratedInputField;
    [SerializeField] private InputFieldFilterUI prismEdgesCountFiltratedInputField;
    [SerializeField] private InputFieldFilterUI prismRadiusFiltratedInputField;

    [SerializeField] private Transform sphereStatsTab;
    [SerializeField] private InputFieldFilterUI sphereEdgesCountFiltratedInputField;
    [SerializeField] private InputFieldFilterUI sphereRadiusFiltratedInputField;

    [SerializeField] private Transform capsuleStatsTab;
    [SerializeField] private InputFieldFilterUI capsuleHeightFiltratedInputField;
    [SerializeField] private InputFieldFilterUI capsuleEdgesCountFiltratedInputField;
    [SerializeField] private InputFieldFilterUI capsuleRadiusFiltratedInputField;

    [SerializeField] private InputFieldFilterUI objectPositionXFiltratedInputField;
    [SerializeField] private InputFieldFilterUI objectPositionZFiltratedInputField;

    public void ShowTab(CreatedObject createdObject)
    {
        parallelepipedStatsTab.gameObject.SetActive(false);
        prismStatsTab.gameObject.SetActive(false);
        sphereStatsTab.gameObject.SetActive(false);
        capsuleStatsTab.gameObject.SetActive(false);

        createdObject.GetMeshStats(out var length, out var width, out var height, out var radius, out var edgesCount);

        var objectPosition = createdObject.transform.position;

        objectPositionXFiltratedInputField.SetValue(objectPosition.x.ToString());
        objectPositionZFiltratedInputField.SetValue(objectPosition.z.ToString());

        switch (createdObject.GetObjectType())
        {
            default:
            case AllCreatedObjects.ObjectTypes.Parallelepiped:
                parallelepipedStatsTab.gameObject.SetActive(true);
                parallelepipedLengthFiltratedInputField.SetValue(length.ToString());
                parallelepipedWidthFiltratedInputField.SetValue(width.ToString());
                parallelepipedHeightFiltratedInputField.SetValue(height.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Prism:
                prismStatsTab.gameObject.SetActive(true);
                prismHeightFiltratedInputField.SetValue(height.ToString());
                prismEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                prismRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Sphere:
                sphereStatsTab.gameObject.SetActive(true);
                sphereEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                sphereRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Capsule:
                capsuleStatsTab.gameObject.SetActive(true);
                capsuleHeightFiltratedInputField.SetValue(height.ToString());
                capsuleEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                capsuleRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
        }
    }

    public void ShowTab(AllCreatedObjects.ObjectTypes objectType)
    {
        parallelepipedStatsTab.gameObject.SetActive(false);
        prismStatsTab.gameObject.SetActive(false);
        sphereStatsTab.gameObject.SetActive(false);
        capsuleStatsTab.gameObject.SetActive(false);

        var length = 1.5f;
        var width = 1.5f;
        var height = 1.5f;
        var radius = .5f;
        var edgesCount = 10;

        switch (objectType)
        {
            default:
            case AllCreatedObjects.ObjectTypes.Parallelepiped:
                parallelepipedStatsTab.gameObject.SetActive(true);
                parallelepipedLengthFiltratedInputField.SetValue(length.ToString());
                parallelepipedWidthFiltratedInputField.SetValue(width.ToString());
                parallelepipedHeightFiltratedInputField.SetValue(height.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Prism:
                prismStatsTab.gameObject.SetActive(true);
                prismHeightFiltratedInputField.SetValue(height.ToString());
                prismEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                prismRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Sphere:
                sphereStatsTab.gameObject.SetActive(true);
                sphereEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                sphereRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
            case AllCreatedObjects.ObjectTypes.Capsule:
                capsuleStatsTab.gameObject.SetActive(true);
                capsuleHeightFiltratedInputField.SetValue(height.ToString());
                capsuleEdgesCountFiltratedInputField.SetValue(edgesCount.ToString());
                capsuleRadiusFiltratedInputField.SetValue(radius.ToString());
                break;
        }
    }

    public void GetStatsByObjectType(AllCreatedObjects.ObjectTypes objectType, out float length, out float width, out float height, out float radius,
        out int edgesCount, out Vector3 objectPosition)
    {
        length = 0f;
        width = 0f;
        height = 0f;
        radius = 0f;
        edgesCount = 0;
        objectPosition = Vector3.zero;
        float.TryParse(objectPositionXFiltratedInputField.GetFiltratedValue(), out objectPosition.x);
        float.TryParse(objectPositionZFiltratedInputField.GetFiltratedValue(), out objectPosition.z);

        switch (objectType)
        {
            default:
            case AllCreatedObjects.ObjectTypes.Parallelepiped:
                float.TryParse(parallelepipedLengthFiltratedInputField.GetFiltratedValue(), out length);
                float.TryParse(parallelepipedWidthFiltratedInputField.GetFiltratedValue(), out width);
                float.TryParse(parallelepipedHeightFiltratedInputField.GetFiltratedValue(), out height);
                break;
            case AllCreatedObjects.ObjectTypes.Prism:
                float.TryParse(prismRadiusFiltratedInputField.GetFiltratedValue(), out radius);
                float.TryParse(prismHeightFiltratedInputField.GetFiltratedValue(), out height);
                int.TryParse(prismEdgesCountFiltratedInputField.GetFiltratedValue(), out edgesCount);
                break;
            case AllCreatedObjects.ObjectTypes.Sphere:
                float.TryParse(sphereRadiusFiltratedInputField.GetFiltratedValue(), out radius);
                int.TryParse(sphereEdgesCountFiltratedInputField.GetFiltratedValue(), out edgesCount);
                break;
            case AllCreatedObjects.ObjectTypes.Capsule:
                float.TryParse(capsuleRadiusFiltratedInputField.GetFiltratedValue(), out radius);
                float.TryParse(capsuleHeightFiltratedInputField.GetFiltratedValue(), out height);
                int.TryParse(capsuleEdgesCountFiltratedInputField.GetFiltratedValue(), out edgesCount);
                break;
        }
    }
}