using UnityEngine;

/// <summary>
/// Responsible for controlling the players distance to the test area when running a test.
/// </summary>
public class PlayerDistanceController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The origin object of the XR Rig")]
    private GameObject origin;

    private void Awake()
    {
        origin = GameObject.Find("XR Origin");

        TestDataStatic.playerDistance = default(float);
    }

    /// <summary>
    /// Sets the player origins distance to the test area.
    /// </summary>
    /// <param name="distance">The new z position of the player origin</param>
    public void SetPlayerDistance(float distance)
    {
        TestDataStatic.playerDistance = distance;
        origin.transform.position = new Vector3(origin.transform.position.x, origin.transform.position.y, distance);
    }

}
