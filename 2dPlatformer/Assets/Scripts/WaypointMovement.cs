using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;
    
    private int _currentPoint;
    private Transform[] _points;

    private void Start()
    {
        _points = new Transform[_path.childCount];

        for (int index = 0; index < _path.childCount; index++)
        {
            _points[index] = _path.GetChild(index);
        }
    }

    private void Update()
    {
        Transform target = _points[_currentPoint];

        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if(transform.position == target.position)
        {
            _currentPoint++;

            if( _currentPoint >= _points.Length)
            {
                _currentPoint = 0;
            }
        }
    }
}