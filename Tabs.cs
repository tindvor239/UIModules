using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Tabs : MonoBehaviour
{
    public Toggle prefabToggle;
    [Space]
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private List<Toggle> toggles;
    [SerializeField]
    private int value;
    [Space]
    [SerializeField]
    private UnityEvent<int> onValueChange = new();

    #region PROPERTIES
    public int Value
    {
        get => value;
        set
        {
            SetValue(value, true);
        }
    }

    public UnityEvent<int> OnValueChange => onValueChange;
    #endregion

    #region UNITY METHODS
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetValue(value, true);
        }
        else
        {
            SetValue(value, false);
        }
    }
#endif

    private void Awake()
    {
        Setup();
        SetValue(value, false);
    }
    #endregion

    public void AddOptions(List<Toggle> toggles)
    {
        foreach (Toggle toggle in toggles)
        {
            AddOption(toggle);
        }
    }

    public void AddOption(Toggle toggle)
    {
        toggle.transform.SetParent(content);
        toggles.Add(toggle);
        SetupToggle(toggle, toggles.Count - 1);
    }

    public void ClearAllOptions()
    {
        foreach (Toggle toggle in toggles)
        {
            Destroy(toggle);
        }
        toggles.Clear();
    }

    private void SetValue(int value, bool isCallback)
    {
        if (value >= toggles.Count || value < 0)
        {
            return;
        }

        this.value = value;
        if (isCallback)
        {
            toggles[value].isOn = true;
        }
        else
        {
            toggles[value].SetIsOnWithoutNotify(true);
        }
    }

    private void Setup()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            SetupToggle(toggles[i], index);
        }
    }

    private void SetupToggle(Toggle toggle, int index)
    {
        toggle.onValueChanged.AddListener((isOn) =>
        {
            OnClickToggle(isOn, index);
        });
    }

    private void OnClickToggle(bool isOn, int index)
    {
        if (isOn)
        {
            onValueChange?.Invoke(index);
            value = index;
        }
    }
}
