using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mover_Activation : MonoBehaviour
{
    public bool LetMove;
    public bool moverParent;
    private Vector3 firstPosition;
    [Header("Direction")]
    public bool Left, Right, Forward, Backward;
    [Header("Type")]
    public bool Mover, MoverAuto, Moved;
    public Transform moverActualObs;

    public bool _activateAutoMover;

    public int unitID;
    [SerializeField]
    private AnimationCurve _activator;
    float _time = 0;

    private void Awake()
    {
        _activator.AddKey(0, 0);
        _activator.AddKey(1f / CrossingToLevel.playSpeed * 0.75f, 1f);
        _activator.AddKey(1f / CrossingToLevel.playSpeed * 1f, 0.75f);
        _activator.AddKey(1f / CrossingToLevel.playSpeed * 1.25f, 1);
        for(int i = 0; i < _activator.length; i++)
        {
            _activator.SmoothTangents(i, 0);
        }
    }

    private void Update() //Moving tile to new position depending to the it's position and arrow orientation
    {
        if((LetMove && Mover) || (MoverAuto && _activateAutoMover))
        {
            Transform moverObs = gameObject.transform;
            if(!gameObject.GetComponent<Mover_Identificator>().m_single)
            {
                moverObs = gameObject.transform.parent;
            }
            transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            if (Left)
            {
                moverObs.position = new Vector3(-_activator.Evaluate(_time), moverObs.transform.position.y, moverObs.transform.position.z);
            }
            if (Right)
            {
                moverObs.position = new Vector3(_activator.Evaluate(_time), moverObs.transform.position.y, moverObs.transform.position.z);
            }
            if (Forward)
            {
                moverObs.position = new Vector3(moverObs.transform.position.x, moverObs.transform.position.y, _activator.Evaluate(_time));
            }
            if (Backward)
            {
                moverObs.position = new Vector3(moverObs.transform.position.x, moverObs.transform.position.y, -_activator.Evaluate(_time));
            }
            //moverObs.position = new Vector3(moverObs.transform.position.x, moverObs.transform.position.y, Mathf.MoveTowards(moverObs.transform.position.z, 1.0f, Time.deltaTime / (1 / LevelInfo_Data.LevelSpeeds[0])));
            _time += Time.deltaTime;
            Moved = true;
        }
    }
}
