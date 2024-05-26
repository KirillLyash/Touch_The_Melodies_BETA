using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlayer_Movement : MonoBehaviour
{
    public bool _startedToPlay = false;
    private float lastPoint;
    private float newPosition;
    private bool _dead = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Start movement if clicked
        {
            _startedToPlay = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
        if (_startedToPlay && !BallPlayer_CollisionSystem.instance.Failed && !_dead) //Moving ball to end position 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, newPosition);
            newPosition += CrossingToLevel.playSpeed * Time.deltaTime;
            lastPoint = transform.position.z;
        }
        if (BallPlayer_CollisionSystem.instance.Failed) //Move 10 tiles forward if failed
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, lastPoint + 10, CrossingToLevel.playSpeed * Time.deltaTime));
            newPosition += CrossingToLevel.playSpeed * Time.deltaTime;
        }
    }

    public void OnDeath()
    {
        Debug.Log("AHHHH, Failed!");
        _dead = true;
        GetComponent<BallPlayer_Controller>().enabled = false;
    }
}
