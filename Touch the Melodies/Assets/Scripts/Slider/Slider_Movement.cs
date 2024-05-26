using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Slider_Movement : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _slidingCurve;
    private float _slidingTime;
    [SerializeField]
    private  bool Left, Right;
    
    private void Awake() //Setting Curve
    {
        _slidingCurve.AddKey(0, transform.position.x);
        if(Left)
        {
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 0.8f, transform.position.x - 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed, transform.position.x - 5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 1.2f, transform.position.x - 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 2, transform.position.x);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 2.8f, transform.position.x + 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 3, transform.position.x + 5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 3.2f, transform.position.x + 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 4, transform.position.x);
        }
        if (Right)
        {
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 0.8f, transform.position.x + 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed, transform.position.x + 5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 1.2f, transform.position.x + 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 2, transform.position.x);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 2.8f, transform.position.x - 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 3, transform.position.x - 5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 3.2f, transform.position.x - 4.5f);
            _slidingCurve.AddKey(CrossingToLevel.playSpeed * 4, transform.position.x);
        }
    }

    private void Start() //Setting delault position
    {
        if (transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z <= CrossingToLevel.playSpeed * 4)
        {
            _slidingTime = 4 * CrossingToLevel.playSpeed - (transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z);
        }
        transform.GetChild(0).position = new Vector3(_slidingCurve.Evaluate(_slidingTime), transform.position.y, transform.position.z);
    }
    private void FixedUpdate()
    {
        if (_slidingTime >= CrossingToLevel.playSpeed * 4) //Resetting time if it beyond the max curve time
        {
            _slidingTime = 0;
        }
        if (GameManager.instance != null && GameManager.instance.hasStarted && transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z <= CrossingToLevel.playSpeed * 4) //Moving tile if distance between ball and tile less than speed*4
        {
            transform.GetChild(0).position = new Vector3(_slidingCurve.Evaluate(_slidingTime), transform.position.y, transform.position.z);
            _slidingTime += Time.fixedDeltaTime * CrossingToLevel.playSpeed;
        }
    }
}
