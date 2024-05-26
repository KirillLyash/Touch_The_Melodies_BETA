using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallPlayer_Controller : MonoBehaviour
{
    //Ball movement to mouse
    [SerializeField]
    private float sensX;
    private float ballPositionX;
    private Vector2 mousePosition;
    private void Update()
    {
        if (!BallPlayer_CollisionSystem.instance.Failed && GameManager.instance != null && !GameManager.instance.paused) //Getting mouse and ball position on click
        {
            if (Input.GetMouseButtonDown(0))
            {
                ballPositionX = transform.position.x;
                mousePosition = Input.mousePosition; 
            }
            if (Input.GetMouseButton(0)) //Moving ball to mouse by delta between ball position and mouse position
            {
                transform.position = new Vector3(ballPositionX + ((Input.mousePosition.x - mousePosition.x) / 100) * sensX, transform.position.y, transform.position.z);
            }
        }
    }
}
