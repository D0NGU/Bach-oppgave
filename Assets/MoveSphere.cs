using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSphere : MonoBehaviour
{
    public Camera myCam;
    
    private float startXPos;
    private float startYPos;
    public Button button;
    private Transform target;
    public GameObject endPos;
    public  GameObject startPos;
    [SerializeField] private Slider slider;
    private float speed = 1;

    public bool editMode = false;
    private bool move = false;
    private bool reverse = false;

    private bool isDragging = false;

    void Start(){
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        if(move){
            target.transform.position = new Vector3(0, 0, 0);
        } else {
            float xPos = transform.position.x;
            float yPos = transform.position.y;
        
            // Grab cylinder values and place on the target.
            target = endPos.transform;
            target.transform.position = new Vector3(0, 0, 0);

        }
    move = !move;
    }
    private void Update()
    {  
        slider.onValueChanged.AddListener((v) =>{
            speed = v;
        });
        if(move){
            if(!reverse){
                var step =  speed* Time.deltaTime; // calculate distance to move
                target.position = Vector3.MoveTowards(target.position, transform.position, step);
            } else {
                var step =  speed* Time.deltaTime; // calculate distance to move
                target.position = Vector3.MoveTowards(target.position, startPos.transform.position, step);
            }
            if(Vector3.Distance(target.position, transform.position) < 0.01f){
                Debug.Log("reverse");
                reverse = true;
            }
            if(Vector3.Distance(target.position, startPos.transform.position) < 0.01f){
                reverse = false;
                Debug.Log("unreverse");
            }
        } else if (isDragging){
            DragObject();
        }
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;

        if (!myCam.orthographic)
        {
            mousePos.z = 10;
        }

        mousePos = myCam.ScreenToWorldPoint(mousePos);

        startXPos = mousePos.x - transform.localPosition.x;
        startYPos = mousePos.y - transform.localPosition.y;

        isDragging = true;
    
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    public void DragObject()
    {
        Vector3 mousePos = Input.mousePosition;

        if(!myCam.orthographic)
        {
            mousePos.z = 10;
        }

        mousePos = myCam.ScreenToWorldPoint(mousePos);
        transform.localPosition = new Vector3(mousePos.x - startXPos, mousePos.y - startYPos, transform.localPosition.z);
    }
    
}
