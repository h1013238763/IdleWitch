using TMPro;
using UnityEngine;

public class GuiKeyTip : GuiBase
{

    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<TextMeshProUGUI>("tip_line_click").gameObject.SetActive(false);
        }
    }
}