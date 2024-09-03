#region

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#endregion

public class InputFieldFilterUI : MonoBehaviour
{
    [SerializeField] private string allowedCharacters = "0123456789";
    private readonly List<char> allAllowedCharacters = new();
    [SerializeField] private bool isPointAllowed;
    [SerializeField] private bool isMinusAllowed;

    private TMP_InputField inputField;
    private bool isFiltrated;

    private void Awake()
    {
        allAllowedCharacters.AddRange(allowedCharacters);

        inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(newValue =>
        {
            if (isFiltrated)
            {
                isFiltrated = false;
                return;
            }

            var filtratedString = GetFiltratedString(newValue);

            if (filtratedString != newValue)
            {
                isFiltrated = true;
                inputField.text = filtratedString;
            }
        });
    }

    private string GetFiltratedString(string originalString)
    {
        var filtratedString = "";
        var isHasPoint = false;
        for (var i = 0; i < originalString.Length; i++)
        {
            var charOfString = originalString[i];

            if (charOfString == '-' && i == 0)
                filtratedString += '-';

            if (charOfString == '.' || charOfString == ',')
            {
                if (!isHasPoint)
                {
                    filtratedString += '.';
                    isHasPoint = true;
                }

                continue;
            }

            if (allAllowedCharacters.Contains(charOfString)) filtratedString += charOfString;
        }

        return filtratedString;
    }

    public string GetFiltratedValue()
    {
        var filtratedString = GetFiltratedString(inputField.text);

        return filtratedString;
    }

    public void SetValue(string newValue)
    {
        inputField.text = newValue;
    }
}