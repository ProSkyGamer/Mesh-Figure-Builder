#region

using System;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class AllCreatedObjectsUI : MonoBehaviour
{
    public static event EventHandler<OnObjectSelectedEventArgs> OnObjectSelected;

    public class OnObjectSelectedEventArgs : EventArgs
    {
        public CreatedObject selectedObject;
    }

    [SerializeField] private Transform createdObjectButtonPrefab;
    [SerializeField] private Transform allCreatedObjectsContainer;
    [SerializeField] private Button createObjectButton;
    [SerializeField] private AllCreatedObjects.ObjectTypes creatingObjectType = AllCreatedObjects.ObjectTypes.Parallelepiped;

    private CreatedObjectButtonSingleUI currentSelectedButton;

    private void Awake()
    {
        createdObjectButtonPrefab.gameObject.SetActive(false);

        createObjectButton.onClick.AddListener(AddNewCreatedObject);
    }

    private void Start()
    {
        ObjectEditableStatsUI.OnObjectStatsEdited += ObjectEditableStatsUI_OnObjectStatsEdited;
    }

    private void ObjectEditableStatsUI_OnObjectStatsEdited(object sender, ObjectEditableStatsUI.OnObjectStatsEditedEventArgs e)
    {
        currentSelectedButton.ChangeCreatedObject(e.newCreatedObject);
    }

    private void AddNewCreatedObject()
    {
        AllCreatedObjects.Instance.CreateObject(creatingObjectType, out var createdObject);
        AddCreatedObjectButton(createdObject);
        OnObjectSelected?.Invoke(this, new OnObjectSelectedEventArgs
        {
            selectedObject = createdObject
        });
    }

    private void AddCreatedObjectButton(CreatedObject createdObject)
    {
        var newCreatedObjectButton = Instantiate(createdObjectButtonPrefab, allCreatedObjectsContainer);
        newCreatedObjectButton.gameObject.SetActive(true);
        var newCreatedObjectButtonSingleUI = newCreatedObjectButton.GetComponent<CreatedObjectButtonSingleUI>();
        newCreatedObjectButtonSingleUI.InitializeButton(createdObject);

        currentSelectedButton = newCreatedObjectButtonSingleUI;

        newCreatedObjectButtonSingleUI.OnObjectSelected += NewCreatedObjectButtonSingleUI_OnObjectSelected;
        newCreatedObjectButtonSingleUI.OnObjectDeleted += NewCreatedObjectButtonSingleUI_OnObjectDeleted;
    }

    private void NewCreatedObjectButtonSingleUI_OnObjectSelected(object sender, EventArgs e)
    {
        var createdObjectButtonSingleUI = sender as CreatedObjectButtonSingleUI;

        if (createdObjectButtonSingleUI == null) return;

        currentSelectedButton = createdObjectButtonSingleUI;

        OnObjectSelected?.Invoke(this, new OnObjectSelectedEventArgs
        {
            selectedObject = createdObjectButtonSingleUI.GetCreatedObject()
        });
    }

    private void NewCreatedObjectButtonSingleUI_OnObjectDeleted(object sender, EventArgs e)
    {
        var createdObjectButtonSingleUI = sender as CreatedObjectButtonSingleUI;

        if (createdObjectButtonSingleUI == null) return;

        var createdObject = createdObjectButtonSingleUI.GetCreatedObject();

        Destroy(createdObjectButtonSingleUI.gameObject);

        AllCreatedObjects.Instance.DeleteObject(createdObject);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}