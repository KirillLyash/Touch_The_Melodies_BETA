using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Jobs;

public class BallPlayer_CollisionSystem : MonoBehaviour
{
    public static BallPlayer_CollisionSystem instance;
    public LayerMask GroundMask;
    private bool _isJumping, _isFlying = true;
    public bool Failed;
    private float _fallTime, _jumpTime;
    private GameObject jumppad, fragile, moverarrow, flyer;
    private bool setMissing, _jumpedOnStart = false;

    [Header("Curves")]
    [SerializeField]
    private AnimationCurve fallingCurve;
    [SerializeField]
    private AnimationCurve jumpingCurve;
    [SerializeField]
    private AnimationCurve flyingCurve;
    public bool _onGround = true;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _onGround = true;
        _isJumping = false;
        _isFlying = false;
        CalculateCurves();
    }
    private void Update()
    {
        CheckTiles();
        if (_isJumping) //Jumping
        {
            Jump();
        }
        if(!CheckGround() && !_isJumping && !_isFlying) //Falling
        {
            Fall();
            GameManager.instance.SaveAfterDie();
        }
        if(_isFlying)
        {
            Fly();
        }
        if(GameManager.instance != null && GameManager.instance.ballPlayerInstance.GetComponent<BallPlayer_Movement>()._startedToPlay && !_jumpedOnStart)
        {
            _isJumping = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z), 0.2f);
    }
    private void Jump()
    {
        if (!_jumpedOnStart || (jumppad != null && transform.position.z >= jumppad.transform.position.z))
        {
            if (jumppad != null && transform.position.z > jumppad.transform.position.z && !setMissing)
            {
                float missingOffset = transform.position.z - jumppad.transform.position.z;
                _jumpTime = missingOffset / CrossingToLevel.playSpeed;
                transform.position = new Vector3(transform.position.x, jumpingCurve.Evaluate(_jumpTime), transform.position.z);
                setMissing = true;
            }
            _jumpTime += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, jumpingCurve.Evaluate(_jumpTime), transform.position.z);
            if (transform.position.y == jumpingCurve.Evaluate(4f / CrossingToLevel.playSpeed))
            {
                _jumpTime = 0;
                setMissing = false;
                _isJumping = false;
                _jumpedOnStart = true;
            }
            if(jumppad != null)
            {
                jumppad.transform.GetComponent<Jumppad_Activation>().Activate(_jumpTime);
            }
        }
    }

    private void Fly()
    {
        if (transform.position.z >= flyer.transform.position.z)
        {
            if (transform.position.z > flyer.transform.position.z && !setMissing)
            {
                float missingOffset = transform.position.z - flyer.transform.position.z;
                _jumpTime = missingOffset / CrossingToLevel.playSpeed;
                transform.position = new Vector3(transform.position.x, flyingCurve.Evaluate(_jumpTime), transform.position.z);
                setMissing = true;
            }
            _jumpTime += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, flyingCurve.Evaluate(_jumpTime), transform.position.z);
            if (transform.position.y == flyingCurve.Evaluate(8f / CrossingToLevel.playSpeed))
            {
                _jumpTime = 0;
                setMissing = false;
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(0).GetChild(1).GetComponent<FlyerTrail_Rotation>().ResetRotation();
                _isFlying = false;
            }
        }
    }
    private void Fall()
    {
        if (transform.position.y != fallingCurve.Evaluate(10 / CrossingToLevel.playSpeed))
        {
            Failed = true;
            _isJumping = false;
            _fallTime += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, fallingCurve.Evaluate(_fallTime), transform.position.z);
            if (transform.position.y == fallingCurve.Evaluate(10 / CrossingToLevel.playSpeed))
            {
                _fallTime = 0;
            }
        }
    }
    private void CheckTiles() //Checking tiles at the bottom of the ball
    {
        RaycastHit Hit;
        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z), 0.2f, Vector3.down, out Hit))
        {
            if (Hit.transform.gameObject.CompareTag("Jumppad") && !_isJumping && _onGround)
            {
                _isJumping = true;
                jumppad = Hit.transform.gameObject;
            }
            if(Hit.transform.gameObject.CompareTag("Fragile") && _onGround && !_isJumping)
            {
                fragile = Hit.transform.gameObject;
                fragile.transform.GetComponent<Fragile_Activation>().Fall = true;
                if(transform.position.y - fragile.transform.position.y > 2.5f)
                {
                    _onGround = false;
                }
                Debug.Log("im on fragile bitch XP");
            }
            if (Hit.transform.gameObject.CompareTag("MoverArrow") && _onGround && !_isJumping)
            {
                moverarrow = Hit.transform.gameObject;
                moverarrow.transform.GetComponent<Mover_Activation>().LetMove = true;
            }

        }
    }
    private bool CheckGround() //Finding ground at the bottom of the ball
    {
        RaycastHit Hit;
        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z), 0.2f, Vector3.down, out Hit))
        {
            if (Hit.transform.gameObject.layer == GroundMask)
            {
                _onGround = true;
            }
        }
        else if (!_isJumping && !_isFlying)
        {
            _onGround = false;
        }
            return _onGround;
    }
    private void CalculateCurves() //Setting keys to the Curves
    {
        jumpingCurve.AddKey(0, 0);
        jumpingCurve.AddKey((4f/ CrossingToLevel.playSpeed) /4, 1.7f);
        jumpingCurve.AddKey((4f / CrossingToLevel.playSpeed) / 2, 2);
        jumpingCurve.AddKey(3*((4f / CrossingToLevel.playSpeed) / 4), 1.7f);
        jumpingCurve.AddKey(4f / CrossingToLevel.playSpeed, 0);

        flyingCurve.AddKey(0, 0);
        flyingCurve.AddKey((8f / CrossingToLevel.playSpeed) / 4, 1.7f);
        flyingCurve.AddKey((8f / CrossingToLevel.playSpeed) / 2, 2);
        flyingCurve.AddKey(3 * ((8f / CrossingToLevel.playSpeed) / 4), 1.7f);
        flyingCurve.AddKey(8f / CrossingToLevel.playSpeed, 0);

        fallingCurve.AddKey(0, 0);
        //fallingCurve.AddKey((5 / speed) / 4, -1.5f);
        //fallingCurve.AddKey((5 / speed) / 2, -3);
        //fallingCurve.AddKey(3 * ((5 / speed) / 4), -6.5f);
        fallingCurve.AddKey(10 / CrossingToLevel.playSpeed, -24f);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Flyer"))
        {
            flyer = collision.gameObject;
            flyer.transform.GetChild(0).gameObject.SetActive(false);
            flyer.transform.GetChild(1).gameObject.SetActive(false);
            flyer.transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            _isFlying = true;
        }
        if (collision.gameObject.CompareTag("Pick-up"))
        {
            GameObject gem = collision.gameObject;
            gem.transform.GetChild(0).gameObject.SetActive(true);
            gem.transform.GetChild(1).gameObject.SetActive(false);
            gem.transform.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.currentGemsCount++;
        }
        if (collision.gameObject.CompareTag("Crown"))
        {
            GameObject crown = collision.gameObject;
            crown.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            crown.transform.GetChild(2).gameObject.SetActive(true);
            crown.transform.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.currentCrownsCount++;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Failed = true;
            GetComponent<BallPlayer_Movement>().OnDeath();
            GameManager.instance.SaveAfterDie();
        }
    }

}
