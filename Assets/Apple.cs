using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private GameObject objectSelectionController;

    public GameObject ghostObject;
    public Rigidbody rb;

    private Rigidbody ghostRb;
    private LineRenderer lineRenderer;
    Vector3 startPos;
    Vector3 endPos;

    private bool move = false;
    private bool reverse = false;

    void Start()
    {
        objectSelectionController = GameObject.Find("ObjectSelectionController");
        lineRenderer = GetComponent<LineRenderer>();
        ghostRb = ghostObject.GetComponent<Rigidbody>();
    }
     
    private void Update()
    {
        
        var points = new Vector3[2];
        points[0] = rb.position;
        points[1] = ghostRb.position;
        lineRenderer.SetPositions(points);
        var speed = 2;
        
        if (move)
        {
            if (Vector3.Distance(endPos, transform.position) < 0.01f)
            {
                Debug.Log("reverse");
                reverse = true;
            }
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                reverse = false;
                Debug.Log("unreverse");
            }
            if (!reverse)
            {
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, endPos, step);
            }
            else
            {
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, startPos, step);
            }
        }
        
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
        Debug.Log("Deselect");
    }

    public void EditMovement()
    {
        ghostObject.SetActive(true);
        GetComponent<LineRenderer>().enabled = true;

    }

    public void DeleteObject()
    {
        Destroy(transform.parent.gameObject);
    }
    
    public void Preview()
    {
        if (move)
        {
            transform.position = startPos;
            GetComponent<Collider>().isTrigger = false;
            ghostObject.GetComponent<Collider>().isTrigger = false;
        } else
        {
            startPos = transform.position;
            endPos = ghostObject.transform.position;
            GetComponent<Collider>().isTrigger = true;
            ghostObject.GetComponent<Collider>().isTrigger = true;
        }
        move = !move;
    }
    
}
