using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(changeColor);
    }

    // Changes color of the toggle and the label
    void changeColor(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        cb.normalColor = isOn ? Color.red : Color.black;

        toggle.GetComponentInChildren<Text>().color = isOn ? Color.red : Color.black;
        toggle.colors = cb;
    }
}
