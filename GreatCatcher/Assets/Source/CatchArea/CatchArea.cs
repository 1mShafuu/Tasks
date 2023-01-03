using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 7;
    
    [SerializeField] private GameObject _catchAreaMesh;

    private float _radius;
    private float _elapsedTime;
    private Bag _bag;
    private float _viewAngle = 90;
    
    public event Action<GameObject> AnimalCatched;
    
    private void Awake()
    {
        _radius = 5f;
        _bag = GetComponentInParent<Bag>();
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
            Vector3 directioToTarget = (target.transform.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, directioToTarget) < _viewAngle / 2)
            {
                _elapsedTime += Time.deltaTime;
                _catchAreaMesh.SetActive(true);
                var direction = (target.transform.position - transform.position).normalized;
                direction.y = 0f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),5f );
             
                if (_elapsedTime >= timeToCatch)
                {
                    _elapsedTime = 0;
                    target.gameObject.SetActive(false);
                    AnimalCatched?.Invoke(target.gameObject);
                }
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
