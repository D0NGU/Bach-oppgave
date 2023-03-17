using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject sphere;
    

    public void Spawn()
    {
        Instantiate(sphere, transform);
    }
}
