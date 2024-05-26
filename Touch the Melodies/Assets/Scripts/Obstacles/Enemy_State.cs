using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Enemy_State : MonoBehaviour
{
    private Animator obsAnimator;
    [SerializeField]
    private float fixedOffset;
    //[SerializeField]
    //private AnimationClip anim;
    private float _playSpeed;
    private bool _isStarted = false;
    void Awake()
    {

    }
    void Start()
    {
        obsAnimator = GetComponent<Animator>();
        _playSpeed = CrossingToLevel.playSpeed / 6;
        float offset = transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z;
        if (GameManager.instance != null && offset < fixedOffset)
        {
            obsAnimator.speed = 0;
            obsAnimator.Play("ActualState", 0, (fixedOffset - offset) / fixedOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float offset = transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z;
        if (GameManager.instance != null && GameManager.instance.hasStarted && offset < fixedOffset && !_isStarted)
        {
            obsAnimator.speed = _playSpeed;
            obsAnimator.Play("ActualState", 0, (fixedOffset - offset) / fixedOffset);
            _isStarted = true;
        }
    }
}
