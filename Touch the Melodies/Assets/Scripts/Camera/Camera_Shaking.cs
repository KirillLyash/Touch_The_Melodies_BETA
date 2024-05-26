using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shaking : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = Camera.main.transform.parent.GetComponent<Animator>();
    }
    public void ShakeTheCamera(string animationName)
    {
        anim.speed = 9 / 6;
        anim.Play(animationName);
    }
}
