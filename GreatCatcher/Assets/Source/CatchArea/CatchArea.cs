using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 7;
    
    [SerializeField] private GameObject _catchAreaMesh;
    [SerializeField] private float _radius;

    private float _elapsedTime;

    public event Action AnimalCatched;
    
    private void Awake()
    {
        _catchAreaMesh.SetActive(false);
    }

    private void Update()
    {
        Collider[] hits = new Collider[MaxColliders];
        Physics.OverlapSphereNonAlloc(transform.position, _radius, hits);

        Collider catchTarget = null;
        hits = hits.Where(hit => hit != null && hit.TryGetComponent(out Animal animal)).ToArray();
        catchTarget = TryGetClosest(hits);
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
                target.gameObject.SetActive(false);
                AnimalCatched?.Invoke();
                _elapsedTime = 0;
            }
        }
        else
        {
            _catchAreaMesh.SetActive(false);
        }
    }

    private Collider TryGetClosest(Collider[] hits)
    {
        var minDistanceToSheep = float.MaxValue;
        Collider closestCollider = null;
        
        foreach (var hit in hits)
        {
            float currentDistance = Vector3.Distance(transform.position, hit.gameObject.transform.position);
            if (currentDistance < minDistanceToSheep)
            {
                minDistanceToSheep = currentDistance;
                closestCollider = hit;
            }
        }

        return closestCollider;
    }
}
