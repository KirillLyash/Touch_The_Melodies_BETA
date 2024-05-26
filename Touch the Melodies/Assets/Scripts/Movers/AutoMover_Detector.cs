using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover_Detector : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.CompareTag("MoverAuto") && gameObject.transform.parent.transform.GetChild(0).GetComponent<Mover_Activation>().Moved)
        {
            collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<Mover_Activation>()._activateAutoMover = true;
        }
    }
}
