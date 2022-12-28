using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 5;
    
    [SerializeField] private GameObject _catchAreaMesh;
    [SerializeField] private float _radius;

    private float _elapsedTime;
    
    private void Awake()
    {
        _catchAreaMesh.SetActive(false);
    }

    private void Update()
    {
        Collider[] hits = new Collider[MaxColliders];
        Physics.OverlapSphereNonAlloc(transform.position, _radius, hits);

        Collider catchTarget = null;
        
        foreach (var hit in hits)
        {
            if (hit != null && hit.gameObject.TryGetComponent(out Sheep sheep))
            {
                catchTarget = hit;
                break;
            }
        }
        
        Catch(catchTarget);
    }

    private void Catch(Collider target)
    {
        var timeToCatch = 3f;
        
        if (target != null)
        {
            _elapsedTime += Time.deltaTime;
            _catchAreaMesh.SetActive(true);
            transform.LookAt(target.transform);
            
            if (_elapsedTime >= timeToCatch)
            {
                Destroy(target.gameObject);
                _elapsedTime = 0;
            }
        }
        else
        {
            _catchAreaMesh.SetActive(false);
        }
    }
}
