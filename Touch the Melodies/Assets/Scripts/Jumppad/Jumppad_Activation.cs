using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad_Activation : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve activationCurve;

    private void Awake() //Setting Jump Curve
    {
        activationCurve.AddKey(0, transform.position.y);
        activationCurve.AddKey(4 / CrossingToLevel.playSpeed / 2, transform.position.y + 1);
        activationCurve.AddKey(4 / CrossingToLevel.playSpeed, transform.position.y);
    }

    public void Activate(float activateTime) //Activation jump tile
    {
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(1).transform.gameObject.SetActive(true);
        transform.GetChild(1).transform.position = new Vector3(transform.GetChild(1).transform.position.x, activationCurve.Evaluate(activateTime), transform.GetChild(1).transform.position.z);
    }
}
