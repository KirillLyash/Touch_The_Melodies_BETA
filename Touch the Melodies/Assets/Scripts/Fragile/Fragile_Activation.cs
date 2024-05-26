using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile_Activation : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _fallingCurve = new AnimationCurve();
    private float _fallTime;
    public bool Fall;
    private float timeToGetUntouch;

    private void Awake() //Setting Curve keys
    {
        _fallingCurve.AddKey(0, transform.position.y);
        _fallingCurve.AddKey(CrossingToLevel.playSpeed / 6, -1.3f);
        _fallingCurve.AddKey(CrossingToLevel.playSpeed / 2, -5.3f);
    }

    private void Update()
    {
        if(Fall && _fallingCurve.length != 0)
        {
            if(transform.position.y >= _fallingCurve.Evaluate(CrossingToLevel.playSpeed / 2)) //Falling and setting Active texture
            {
                transform.position = new Vector3(transform.position.x, _fallingCurve.Evaluate(_fallTime), transform.position.z);
                _fallTime += Time.deltaTime * CrossingToLevel.playSpeed;
                if (GetComponent<Fragile_ConnectionIdentity>() == null || !GetComponent<Fragile_ConnectionIdentity>().f_single)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/FragileActive");
                    }
                    timeToGetUntouch += Time.deltaTime;
                    if(timeToGetUntouch >= 1 / CrossingToLevel.playSpeed)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            Destroy(transform.GetChild(i).GetComponent<BoxCollider>());
                        }
                    }
                }
                else
                {
                    transform.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/FragileActive");
                }
            }
        }
    }
}
