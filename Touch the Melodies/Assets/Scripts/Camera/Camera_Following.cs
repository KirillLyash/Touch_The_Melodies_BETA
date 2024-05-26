using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Camera_Following : MonoBehaviour
{
    private float zOffset = 4f;
    private float fadingDistance = 0;
    [SerializeField]
    private AnimationCurve fadingCurve;

    private void Awake()
    {
        fadingCurve.AddKey(0, CrossingToLevel.playSpeed * Time.deltaTime); //Setting to Fading Curve keys
        fadingCurve.AddKey(6 / CrossingToLevel.playSpeed, 0);
    }
    private void Update()
    {
        if (GameManager.instance != null)
        {
            if (!GameManager.instance.ballPlayerInstance.GetComponent<BallPlayer_CollisionSystem>().Failed) //Moving camera behind the ball
            {
                transform.position = new Vector3(GameManager.instance.ballPlayerInstance.transform.position.x / 2, transform.position.y, Mathf.MoveTowards(transform.position.z, GameManager.instance.ballPlayerInstance.transform.position.z - zOffset, CrossingToLevel.playSpeed));
            }
            else //Fading camera position if ball failed
            {
                transform.position = new Vector3(GameManager.instance.ballPlayerInstance.transform.position.x / 2, transform.position.y, Mathf.MoveTowards(transform.position.z, GameManager.instance.ballPlayerInstance.transform.position.z - zOffset, fadingCurve.Evaluate(fadingDistance)));
                fadingDistance += Time.deltaTime;
            }
        }
    }
}
