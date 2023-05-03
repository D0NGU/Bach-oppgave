using UnityEngine;

/// <summary>
/// Controls the tooltip prefab used to
/// hold and display information about UI elements
/// </summary>
public class TooltipController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("UI Panel containing the tooltip information")]
    private GameObject toolTipPanel;

    /// <summary>
    /// Hides all tooltips in the scene, and 
    /// shows/hides the tooltip panel.
    /// </summary>
    public void ShowHideToolTip()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tooltip"))
        {
            if (go != this.toolTipPanel) go.SetActive(false);
        }

        toolTipPanel.SetActive(!toolTipPanel.activeSelf);
    }
}
