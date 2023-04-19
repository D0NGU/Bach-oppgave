using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTipPanel;
    public void ShowHideToolTip()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tooltip"))
        {
            if (go != this.toolTipPanel) go.SetActive(false);
        }

        toolTipPanel.SetActive(!toolTipPanel.activeSelf);
    }
}
