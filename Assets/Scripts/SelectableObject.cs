using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for holding and handling all parameters and properties of a
/// selectable object.
/// </summary>
public class SelectableObject : MonoBehaviour
{
    /// <summary>
    /// Game object with the <see cref="ObjectMenuController"/> in the scene
    /// </summary>
    private GameObject objectSelectionController;

    /// <summary>
    /// The currently active child of the game object this script is attached to.
    /// </summary>
    public GameObject sphereChild;

    private GameObject fullSphere;
    private GameObject rightHalf;
    private GameObject leftHalf;

    /// <summary>
    /// Game object that represent the end position of the selectable objects movement path
    /// </summary>
    private GameObject ghostSphere;
    /// <summary>
    /// The rigid body of the selectable object
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// The rigid body of the ghost sphere
    /// </summary>
    private Rigidbody ghostRb;
    /// <summary>
    /// Renders a line between the selectable object and the ghost sphere
    /// </summary>
    private LineRenderer lineRenderer;
    /// <summary>
    /// The original color of the selectable object
    /// </summary>
    private Color originalColor;

    private Coroutine testCoroutine;

    // All changable parameters of the seletable object
    public string objectType = "fullsphere";
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 positionWhenSeen;
    public float moveTime = 1;
    public Vector3 scale;
    public float startDelay = 0.0f;
    public bool loopMovement = false;
    public bool hasMovement = false;

    public bool hasBeenSeen = false;
    public float timePassedBeforeSeen;

    /// <summary>
    /// Whether the selectable object should move
    /// </summary>
    public bool move = false;
    /// <summary>
    /// Whether the test has been started and is active
    /// </summary>
    public bool testActive = false;
    /// <summary>
    /// Whether the selectable object is moving in reverse
    /// </summary>
    private bool reverse = false;
    /// <summary>
    /// Keeps track of time spent in movement one way
    /// </summary>
    private float t;

    private Dictionary<string, float> testAreaBounds;

    void Awake()
    {
        objectSelectionController = GameObject.Find("SelectableObjectController");

        testAreaBounds = GameObject.Find("TestArea").GetComponent<TestArea>().GetTestAreaBounds();

        ghostSphere = transform.parent.Find("Ghost Sphere").gameObject;
        ghostRb = ghostSphere.GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();

        lineRenderer = GetComponent<LineRenderer>();

        scale = transform.localScale;

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
        
        // Object will move is the test is active, if it has movement, and if the initial start delay is over
        if (testActive && hasMovement && move)
        {
            if (Vector3.Distance(endPos, transform.position) < 0.01f && !reverse && loopMovement)
            {
                reverse = true;
                t = 0;
            }

            if (Vector3.Distance(transform.position, startPos) < 0.01f && reverse && loopMovement)
            {
                reverse = false;
                t = 0;
            }

            if (!reverse)
            {
                t += Time.deltaTime / moveTime;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                // var step = speed * Time.deltaTime; // calculate distance to move
                // transform.position = Vector3.MoveTowards(transform.position, endPos, step);
            }
            else
            {
                t += Time.deltaTime / moveTime;
                transform.position = Vector3.Lerp(endPos, startPos, t);
                //var step = speed * Time.deltaTime; // calculate distance to move
                //transform.position = Vector3.MoveTowards(transform.position, startPos, step);
            }
        }

        // Counts number of seconds passed since start of test until the object has been seen
        if (!hasBeenSeen && testActive)
        {
            timePassedBeforeSeen += Time.deltaTime;
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

    /// <summary>
    /// Handles selection of the selectable object
    /// </summary>
    public void Selected()
    {
        // Checks if this gameobject is already selected
        if (!GameObject.ReferenceEquals(objectSelectionController.GetComponent<ObjectMenuController>().selectedObject, this.gameObject))
        {

            objectSelectionController.GetComponent<ObjectMenuController>().ChangeSelectedObject(this.gameObject);

            objectSelectionController.GetComponent<ObjectMenuController>().UpdateVisionDetectionTimeDisplay();

            this.gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

    }

    /// <summary>
    /// If run when the selectable object is deselected.
    /// Stores the start and end position.
    /// </summary>
    public void DeSelected()
    {
       endPos = ghostSphere.transform.position;
       startPos = transform.position;

    }

    /// <summary>
    /// Adds movement to the selectable object
    /// </summary>
    public void AddMovement()
    {
        if (!hasMovement) ghostSphere.transform.position = transform.position;

        ShowGhostSphere(true);

        hasMovement = true;
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

    public void StartTest()
    {
        if (testActive)
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

            timePassedBeforeSeen = 0;
            hasBeenSeen = false;

            lineRenderer.enabled = false;
            ghostSphere.SetActive(false);

        }


        if (testCoroutine != null)
        {
            StopCoroutine(testCoroutine);
            testCoroutine = null;

            ShowChildren(true);
        }
        else
        {
            testCoroutine = StartCoroutine(DelayedStart());
        }


        move = false;
        t = 0;
        testActive = !testActive;
    }

    private IEnumerator DelayedStart()
    {
        ShowChildren(false);

        yield return new WaitForSeconds(startDelay);

        ShowChildren(true);

        move = true;

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

    private void ShowChildren(bool enable)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null && child.GetComponent<MeshCollider>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().enabled = enable;
                child.gameObject.GetComponent<MeshCollider>().enabled = enable;
            }
        }
    }

    public void SetScale(float scale_)
    {
        transform.localScale = new Vector3(scale_, scale_, scale_);
        scale = new Vector3(scale_, scale_, scale_);
    }
}
