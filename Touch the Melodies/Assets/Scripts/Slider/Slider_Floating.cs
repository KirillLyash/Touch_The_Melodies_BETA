using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slider_Floating : MonoBehaviour
{
    public float[] XtilePositions;
    public List<float> _movementPoints = new List<float>();
    public bool Left, Right;
    private float slideCentre;
    private float _startPosition, _movementDistance, newPosition, _movementTime;
    private float _unitModifier = 0.0f;
    private float _cameraOffset = 25.0f;

    float minPosition, maxPosition;
    private void Awake()
    {
        XtilePositions = new float[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
        XtilePositions[i] = transform.GetChild(i).position.x;
        }
        minPosition = Mathf.Min(XtilePositions);
        maxPosition = Mathf.Max(XtilePositions);
        if (minPosition == -2 || maxPosition == 2)
        {
            SetSlidePath(true);
        }
        if (minPosition != -2 && maxPosition != 2)
        {
            SetSlidePath(false);
        }
    }

    private void SetSlidePath(bool onEdge)
    {
        if (onEdge)
        {
            if (minPosition == -2 && maxPosition != 2)
            {
                if (Left)
                {
                    _movementPoints.Add((minPosition + maxPosition) / 2 - 4.0f);
                    _movementPoints.Add((minPosition + maxPosition) / 2);
                    slideCentre = (_movementPoints[1] - _movementPoints[0]) / 2;
                    _movementDistance = 8.0f;
                }
                if (Right)
                {
                    _movementPoints.Add((minPosition + maxPosition) / 2 - 4.0f);
                    _movementPoints.Add(-1 * (minPosition + maxPosition));
                    slideCentre = (_movementPoints[1] + _movementPoints[0]) / 2;
                    _movementDistance = 8.0f + Mathf.Abs((minPosition + maxPosition) / 2 - _movementPoints[1]) * 2;
                }
            }
            else if (maxPosition == 2 && minPosition != -2)
            {
                if (Left)
                {
                    _movementPoints.Add((minPosition + maxPosition) / 2 + 4.0f);
                    _movementPoints.Add(-1 * (minPosition + maxPosition));
                    slideCentre = (_movementPoints[1] + _movementPoints[0]) / 2;
                    _movementDistance = 8.0f + Mathf.Abs((minPosition + maxPosition) / 2 - _movementPoints[1]) * 2;
                }
                if (Right)
                {
                    _movementPoints.Add((minPosition + maxPosition) / 2 + 4.0f);
                    _movementPoints.Add((minPosition + maxPosition) / 2);
                    slideCentre = (_movementPoints[1] - _movementPoints[0]) / 2;
                    _movementDistance = 8.0f;
                }
            }
            else if (minPosition == -2 && maxPosition == 2)
            {
                if (Left)
                {
                    _movementPoints.Add(transform.position.x + 4);
                    _movementPoints.Add(transform.position.x);
                    slideCentre = (_movementPoints[1] + _movementPoints[0]) / 2;
                    _movementDistance = 8.0f;
                }
                if (Right)
                {
                    _movementPoints.Add(transform.position.x - 4);
                    _movementPoints.Add(transform.position.x);
                    slideCentre = (_movementPoints[0] - _movementPoints[1]) / 2;
                    _movementDistance = 8.0f;
                }
            }
        }
        else
        {
            if (Left)
            {
                _movementPoints.Add(transform.position.x + 4.0f);
                _movementPoints.Add(transform.position.x + -2 - minPosition);
                slideCentre = _movementPoints[1] + Mathf.Abs(_movementPoints[0] - _movementPoints[1]) / 2;
                _movementDistance = 8.0f + 2 * Mathf.Abs(transform.position.x - _movementPoints[1]);
            }
            if (Right)
            {
                _movementPoints.Add(transform.position.x - 4.0f);
                _movementPoints.Add(transform.position.x + 2.0f - maxPosition);
                slideCentre = _movementPoints[1] - Mathf.Abs(_movementPoints[0] - _movementPoints[1]) / 2;
                _movementDistance = 8.0f + 2 * Mathf.Abs(transform.position.x + _movementPoints[1]);
            }
        }
        _startPosition = transform.position.x;
        _movementTime = _cameraOffset / CrossingToLevel.playSpeed;
        float offset = _startPosition - slideCentre;
        int n = 1;
        if (offset < 0)
        {
            n = -1;
        }
        float directOffset = n * _movementDistance / 4 - offset;
        _unitModifier = Mathf.Asin(offset / (_movementDistance / 4.0f)) / (2 * Mathf.PI);
        //Debug.Log($"minPos: {minPosition}, maxPos: {maxPosition}; point1: {_movementPoints[0]}, point2: {_movementPoints[1]}");
        Debug.Log($"Centre: {slideCentre}, Unit modifier: {_unitModifier}, Distance: {_movementDistance}");
        Debug.Log($"{Mathf.Asin(offset / (_movementDistance / 4.0f))}, {offset / (_movementDistance / 4.0f)}");
        newPosition = slideCentre + Mathf.Sin(2 * Mathf.PI * _unitModifier) * _movementDistance / 4.0f;
        Debug.Log(newPosition);
        //Debug.Log($"{slideCentre}, {Mathf.Sin(2 * Mathf.PI * _unitModifier) * Mathf.Abs(_movementPoints[0] - _movementPoints[1]) / 2}, {Mathf.Sin(2 * Mathf.PI * _unitModifier)}");
        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
    }
    private void Start()
    {
        float distanceToBall = transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z;
        if (distanceToBall <= _cameraOffset)
        {
            float offset = _movementDistance - distanceToBall * _movementDistance / _cameraOffset;
            if (Left)
            {
                _unitModifier += offset / _movementDistance;
            }
            if (Right)
            {
                _unitModifier -= offset / _movementDistance;
            }
            newPosition = slideCentre + Mathf.Sin(2 * Mathf.PI * _unitModifier) * Mathf.Abs(_movementPoints[0] - _movementPoints[1]) / 2;
            transform.localPosition = new Vector3(newPosition, transform.position.y, transform.position.z);
        }

    }
    private void FixedUpdate()
    {
        if (GameManager.instance != null && GameManager.instance.hasStarted && transform.position.z - GameManager.instance.ballPlayerInstance.transform.position.z <= _cameraOffset) //Moving tile if distance between ball and tile less than speed*4
        {
            if (Left)
            {
                _unitModifier += Time.fixedDeltaTime / _movementTime;
            }
            if (Right)
            {
                _unitModifier -= Time.fixedDeltaTime / _movementTime;
            }
            newPosition = slideCentre + Mathf.Sin(2 * Mathf.PI * _unitModifier) * Mathf.Abs(_movementPoints[0] - _movementPoints[1]) / 2;
            transform.localPosition = new Vector3(newPosition, transform.position.y, transform.position.z);
        }
    }

}
