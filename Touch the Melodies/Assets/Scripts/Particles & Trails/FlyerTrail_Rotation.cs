using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerTrail_Rotation : MonoBehaviour
{
    private float _trailAngle;
    [SerializeField]
    private float rotationSpeed;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, _trailAngle);
        _trailAngle += 1 * Time.fixedDeltaTime * rotationSpeed;
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _trailAngle = 0;
    }
}
