using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private GameObject objectSelectionController;

    public GameObject sphereChild;

    private GameObject fullSphere;
    private GameObject rightHalf;
    private GameObject leftHalf;


    private GameObject ghostSphere;
    private Rigidbody rb;
    private Rigidbody ghostRb;
    private LineRenderer lineRenderer;
    private Color originalColor;

    public string objectType = "fullsphere";
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed = 1;
    float t;

    public bool previewActive = false;
    private bool reverse = false;
    public bool loopMovement = false;
    public bool hasMovement = false;
    public bool verified = false;

    private Dictionary<string, float> testAreaBounds;

    void Awake()
    {
        objectSelectionController = GameObject.Find("ObjectSelectionController");

        testAreaBounds = GameObject.Find("TestArea").GetComponent<TestArea>().GetTestAreaBounds();

        ghostSphere = transform.parent.Find("Ghost Sphere").gameObject;
        ghostRb = ghostSphere.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();

        lineRenderer = GetComponent<LineRenderer>();

        fullSphere = transform.Find("FullSphere").gameObject;
        rightHalf = transform.Find("RightHalf").gameObject;
        leftHalf = transform.Find("LeftHalf").gameObject;

        if (sphereChild != null)
        {
            originalColor = sphereChild.GetComponent<Renderer>().material.color;
        }
    }


    private void Update()
    {
        
        var points = new Vector3[2];
        points[0] = rb.position;
        points[1] = ghostRb.position;
        if (lineRenderer != null) lineRenderer.SetPositions(points);
        
        if (previewActive && hasMovement)
        {
            if (Vector3.Distance(endPos, transform.position) < 0.01f && !reverse && loopMovement)
            {
                Debug.Log("reverse");
                reverse = true;
                t = 0;
            }
            if (Vector3.Distance(transform.position, startPos) < 0.01f && reverse && loopMovement)
            {
                reverse = false;
                Debug.Log("unreverse");
                t = 0;
            }
            if (!reverse)
            {
                t += Time.deltaTime / speed;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                // var step = speed * Time.deltaTime; // calculate distance to move
                // transform.position = Vector3.MoveTowards(transform.position, endPos, step);
            }
            else
            {
                t += Time.deltaTime / speed;
                transform.position = Vector3.Lerp(endPos, startPos, t);
                //var step = speed * Time.deltaTime; // calculate distance to move
                //transform.position = Vector3.MoveTowards(transform.position, startPos, step);
            }
        }

        // Clamps position of gameObject to stay within the bounds of the testArea
        float radius = GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z); ;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, testAreaBounds["minX"] + radius, testAreaBounds["maxX"] - radius),
            Mathf.Clamp(transform.position.y, testAreaBounds["minY"] + radius, testAreaBounds["maxY"] - radius),
            Mathf.Clamp(transform.position.z, testAreaBounds["minZ"] + radius, testAreaBounds["maxZ"] - radius));

        // Clamps position of ghost gameObject to stay within the bounds of the testArea
        float ghostRadius = ghostSphere.GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z); ;
        ghostSphere.transform.position = new Vector3(
            Mathf.Clamp(ghostSphere.transform.position.x, testAreaBounds["minX"] + ghostRadius, testAreaBounds["maxX"] - ghostRadius),
            Mathf.Clamp(ghostSphere.transform.position.y, testAreaBounds["minY"] + ghostRadius, testAreaBounds["maxY"] - ghostRadius),
            Mathf.Clamp(ghostSphere.transform.position.z, testAreaBounds["minZ"] + ghostRadius, testAreaBounds["maxZ"] - ghostRadius));

    }


    public void Selected()
    {
        // Checks if this gameobject is already selected
        if (!GameObject.ReferenceEquals(objectSelectionController.GetComponent<ObjectSelection>().selectedObject, this.gameObject))
        {

            objectSelectionController.GetComponent<ObjectSelection>().ChangeSelectedObject(this.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

        Debug.Log("Select");
    }

    public void DeSelected()
    {
       endPos = ghostSphere.transform.position;
       startPos = transform.position;

       Debug.Log("Deselect");
    }

    public void EditMovement()
    {
        if (!hasMovement) ghostSphere.transform.position = transform.position;

        ShowGhostSphere(true);

        hasMovement = true;

        //GetComponent<Collider>().isTrigger = false;
       //ghostObject.GetComponent<Collider>().isTrigger = false;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void RemoveMovement()
    {
        hasMovement = false;

        ShowGhostSphere(false);
    }

    public void DeleteObject()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ToggleLoopMovement()
    {
        if (loopMovement) reverse = false;

        loopMovement = !loopMovement;
    }

    public void ShowGhostSphere(bool enable)
    {
        ghostSphere.SetActive(enable);

        lineRenderer.enabled = enable;
    }

    public void Preview()
    {
        if (previewActive)
        {
            if (hasMovement)
            {
                lineRenderer.enabled = true;
                ghostSphere.SetActive(true);
            }

            transform.position = startPos;
            sphereChild.GetComponent<Collider>().isTrigger = false;
            ghostSphere.GetComponent<Collider>().isTrigger = false;
            sphereChild.GetComponent<Renderer>().material.color = originalColor;
        } 
        else
        {
            startPos = transform.position;
            endPos = ghostSphere.transform.position;
            sphereChild.GetComponent<Collider>().isTrigger = true;
            ghostSphere.GetComponent<Collider>().isTrigger = true;

            lineRenderer.enabled = false;
            ghostSphere.SetActive(false);

        }

        t = 0;
        previewActive = !previewActive;
    }

    public void ChangeToFullSphere()
    {
        DeactivateChildren();

        fullSphere.SetActive(true);

        sphereChild = fullSphere;
        objectType = "fullsphere";
    }

    public void ChangeToLeftHalf()
    {
        DeactivateChildren();

        leftHalf.SetActive(true);

        sphereChild = leftHalf;
        objectType = "lefthalf";
    }
    public void ChangeToRightHalf()
    {
        DeactivateChildren();

        rightHalf.SetActive(true);

        sphereChild = rightHalf;
        objectType = "righthalf";
    }


    private void DeactivateChildren()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
