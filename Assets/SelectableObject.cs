using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private GameObject objectSelectionController;

    public GameObject ghostObject;
    public Rigidbody rb;

    private Rigidbody ghostRb;
    private LineRenderer lineRenderer;
    Vector3 startPos;
    Vector3 endPos;
    public float speed = 1;
    float t;

    public bool move = false;
    private bool reverse = false;
    public bool loopMovement = false;
    public bool hasMovement = false;

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
        
        if (move)
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
       endPos = ghostObject.transform.position;
       startPos = transform.position;

       Debug.Log("Deselect");
    }

    public void EditMovement()
    {
        if (!hasMovement) ghostObject.transform.position = transform.position;

        ghostObject.SetActive(true);
        GetComponent<LineRenderer>().enabled = true;

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

        ghostObject.SetActive(false);
        GetComponent<LineRenderer>().enabled = false;
    }

    public void SaveMovement()
    {
        //GetComponent<Collider>().isTrigger = true;
        //ghostObject.GetComponent<Collider>().isTrigger = true;
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
    
    public void Preview()
    {
        if (move)
        {
            transform.position = startPos;
            GetComponent<Collider>().isTrigger = false;
            ghostObject.GetComponent<Collider>().isTrigger = false;
        } 
        else
        {
            startPos = transform.position;
            endPos = ghostObject.transform.position;
            GetComponent<Collider>().isTrigger = true;
            ghostObject.GetComponent<Collider>().isTrigger = true;
        }

        t = 0;
        move = !move;
    }
}