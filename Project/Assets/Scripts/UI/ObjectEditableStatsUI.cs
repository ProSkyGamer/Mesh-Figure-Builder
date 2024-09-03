#region

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class ObjectEditableStatsUI : MonoBehaviour
{
    public static event EventHandler<OnObjectStatsEditedEventArgs> OnObjectStatsEdited;

    public class OnObjectStatsEditedEventArgs : EventArgs
    {
        public CreatedObject newCreatedObject;
    }

    public enum MaterialColors
    {
        Red,
        Blue,
        Green,
        Orange,
        Pink,
        Purple,
        Yellow
    }

    [Serializable]
    public class ObjectMaterials
    {
        public MaterialColors color;
        public Material material;
    }

    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_Dropdown objectTypeDropdown;
    [SerializeField] private TMP_Dropdown objectMaterialDropdown;
    [SerializeField] private List<ObjectMaterials> allAvailableMaterials;
    [SerializeField] private EditableStatsTypeUI editableStatsTypeUI;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closeButton;

    private CreatedObject createdObject;

    private void Awake()
    {
        objectTypeDropdown.options.Clear();
        var allMeshTypes = new List<string>();
        allMeshTypes.AddRange(Enum.GetNames(typeof(AllCreatedObjects.ObjectTypes)));
        objectTypeDropdown.AddOptions(allMeshTypes);

        objectMaterialDropdown.options.Clear();
        foreach (var objectMaterial in allAvailableMaterials)
        {
            objectMaterialDropdown.options.Add(new TMP_Dropdown.OptionData { text = objectMaterial.color.ToString() });
        }

        cancelButton.onClick.AddListener(Hide);
        closeButton.onClick.AddListener(Hide);

        confirmButton.onClick.AddListener(() =>
        {
            editableStatsTypeUI.GetStatsByObjectType((AllCreatedObjects.ObjectTypes)objectTypeDropdown.value,
                out var length, out var width, out var height, out var radius, out var edgesCount, out var objectPosition);
            AllCreatedObjects.Instance.ModifyObject(createdObject,
                (AllCreatedObjects.ObjectTypes)objectTypeDropdown.value, allAvailableMaterials[objectMaterialDropdown.value].material,
                playerNameInputField.text, objectPosition, length, width, height, radius, edgesCount, out var newCreatedObject);
            OnObjectStatsEdited?.Invoke(this, new OnObjectStatsEditedEventArgs
            {
                newCreatedObject = newCreatedObject
            });
            Hide();
        });

        objectTypeDropdown.onValueChanged.AddListener(newValue => { editableStatsTypeUI.ShowTab((AllCreatedObjects.ObjectTypes)newValue); });
    }

    private void Start()
    {
        AllCreatedObjectsUI.OnObjectSelected += AllCreatedObjectsUI_OnObjectSelected;

        Hide();
    }

    private void AllCreatedObjectsUI_OnObjectSelected(object sender, AllCreatedObjectsUI.OnObjectSelectedEventArgs e)
    {
        Show();
        createdObject = e.selectedObject;
        UpdateTab(e.selectedObject);
    }

    private void UpdateTab(CreatedObject createdObject)
    {
        editableStatsTypeUI.ShowTab(createdObject);
        objectTypeDropdown.value = (int)createdObject.GetObjectType();
        playerNameInputField.text = createdObject.gameObject.name;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        AllCreatedObjectsUI.OnObjectSelected -= AllCreatedObjectsUI_OnObjectSelected;
    }
}