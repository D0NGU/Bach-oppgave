using UnityEngine;

/// <summary>
/// Responsible for rotating the player to face towards the test area.
/// This script must be a component of the parent object of the players VR camera.
/// </summary>
public class PlayerRotator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The VR camera of the player")]
    private GameObject VRCamera;

    /// <summary>
    /// Changes the rotation of the parent of the players VR camera to the inverse 
    /// of the players VR camera in order to counteract head rotation.
    /// </summary>
    public void RotatePerspetive()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z);
        var vrCamAngle = Quaternion.Inverse(VRCamera.transform.rotation);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, vrCamAngle.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}


