using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{
    [Space]
    [SerializeField]
    private Toggle dropDownToggle;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
    private GameObject dropdown;
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
    public RectTransform Content => content;

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
        Setup();
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
        dropDownToggle.onValueChanged.AddListener(dropdown.SetActive);
        dropdown.SetActive(dropDownToggle.isOn);
        Setup();
        SetValue(value, false);
    }

    private void OnDestroy()
    {
        dropDownToggle.onValueChanged.RemoveListener(dropdown.SetActive);
    }
    #endregion

    public void AddOptions(List<Toggle> toggles)
    {
        this.toggles = toggles;
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            AddOption(toggles[i], index);
        }
    }

    public void ClearAllOptions()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
            Destroy(toggle.gameObject);
        }
        toggles.Clear();
    }

    private void SetValue(int value, bool isCallback)
    {
        if (value >= toggles.Count || value < 0)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    toggle.isOn =  false;
                }
            }
            this.value =  -1;
            return;
        }

        if (isCallback)
        {
            toggles[value].isOn = true;
        }
        else
        {
            toggles[value].SetIsOnWithoutNotify(true);
            this.value = value;
        }
    }

    private void Setup()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i] == null)
            {
                continue;
            }
            
            SetupToggle(toggles[i], i);
        }
    }

    private void AddOption(Toggle toggle, int index)
    {
        toggle.transform.SetParent(content);
        toggle.group = toggleGroup;
        SetupToggle(toggle, index);
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
            Debug.Log(index);
        }
        else
        {
            value = IsAllTogglesFalse() ? -1 : value;
        }
    }

    private bool IsAllTogglesFalse()
    {
        int count = 0;
        foreach (Toggle toggle in toggles)
        {
            if (!toggle.isOn) count++;
        }
        return toggles.Count == count;
    }
}
