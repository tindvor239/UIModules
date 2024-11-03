using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropdownController : MonoBehaviour
{
    public delegate void OnToggleCreate<T>(T data, Toggle createdToggle);

    public Toggle prefabToggle;

    private Dropdown dropdown;

    #region PROPERTY
    public int Value
    {
        get => dropdown.Value;
        set => dropdown.Value = value;
    }
    public UnityEvent<int> OnValueChange
    {
        get
        {
            if (dropdown == null)
            {
                dropdown = GetComponent<Dropdown>();
            }
            return dropdown.OnValueChange;
        }
    }
    #endregion

    #region UNITY_METHODS
    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }
    #endregion

    public void AddOptions<T>(List<T> data, OnToggleCreate<T> onCreateCallback = null)
    {
        List<Toggle> toggles = new();
        for (int i = 0; i < data.Count; i++)
        {
            toggles.Add(CreateToggle(data[i], onCreateCallback));
        }
        dropdown.AddOptions(toggles);
        Canvas.ForceUpdateCanvases();
    }

    public void AddOptions<T>(T[] data, OnToggleCreate<T> onCreateCallback = null)
    {
        List<Toggle> toggles = new();
        for (int i = 0; i < data.Length; i++)
        {
            toggles.Add(CreateToggle(data[i], onCreateCallback));
        }
        dropdown.AddOptions(toggles);
        Canvas.ForceUpdateCanvases();
    }

    private Toggle CreateToggle<T>(T data, OnToggleCreate<T> onCreateCallback)
    {
        Toggle toggle = Instantiate(prefabToggle, dropdown.Content);
        onCreateCallback?.Invoke(data, toggle);
        return toggle;
    }

    public void ClearAllOptions()
    {
        dropdown.ClearAllOptions();
    }
}
