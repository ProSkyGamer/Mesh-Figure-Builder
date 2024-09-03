#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class CreatedObjectButtonSingleUI : MonoBehaviour
{
    public event EventHandler OnObjectDeleted;
    public event EventHandler OnObjectSelected;

    [SerializeField] private Button deleteObjectButton;
    [SerializeField] private TextMeshProUGUI objectNameText;
    private Button selectObjectButton;
    private CreatedObject createdObject;

    public void InitializeButton(CreatedObject newCreatedObject)
    {
        selectObjectButton = GetComponent<Button>();
        createdObject = newCreatedObject;
        objectNameText.text = newCreatedObject.gameObject.name;

        selectObjectButton.onClick.AddListener(() => { OnObjectSelected?.Invoke(this, EventArgs.Empty); });

        deleteObjectButton.onClick.AddListener(() => { OnObjectDeleted?.Invoke(this, EventArgs.Empty); });
    }

    public void ChangeCreatedObject(CreatedObject newCreatedObject)
    {
        createdObject = newCreatedObject;
        objectNameText.text = newCreatedObject.gameObject.name;
    }

    public CreatedObject GetCreatedObject()
    {
        return createdObject;
    }
}