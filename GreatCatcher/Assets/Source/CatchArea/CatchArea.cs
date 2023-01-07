using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 7;
    
    [SerializeField] private MeshRenderer _catchAreaMesh;
    
    private Player _player;
    private float _radius = 5f;
    private float _elapsedTime;
    private Bag _bag;
    private float _viewAngle = 90;

    public float Radius => _radius;
    
    public event Action<GameObject> AnimalCatched;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _bag = GetComponentInParent<Bag>();
        _catchAreaMesh = GetComponent<MeshRenderer>();
        _catchAreaMesh.enabled = false;
    }

    private void Update()
    {
        Collider[] hits = new Collider[MaxColliders];
        Physics.OverlapSphereNonAlloc(transform.position, _radius, hits);
        GameObject catchTarget = null;
        hits = hits.Where(hit => hit != null && hit.TryGetComponent(out Animal animal)).ToArray();

        if (_bag.AnimalsInBag < _bag.MaxAmountOfAnimalsInBag)
        {
            catchTarget = TryGetClosest(hits);
            Catch(catchTarget);
        }
        else
        {
            _catchAreaMesh.enabled = false;
        }
    }

    private void Catch(GameObject target)
    {
        const float timeToCatch = 3f;
        const int angleDivider = 2;
        const float maxDegreesDelta = 5f;
        
        if (target != null && target.GetComponent<Animal>().Level <= _player.Level)
        {
            Vector3 directioToTarget = (target.transform.position - transform.position).normalized;
            _catchAreaMesh.enabled = true;
            
            if (Vector3.Angle(transform.forward, directioToTarget) < _viewAngle / angleDivider)
            {
                _elapsedTime += Time.deltaTime;
                var direction = (target.transform.position - transform.position).normalized;
                direction.y = 0f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), maxDegreesDelta);
             
                if (_elapsedTime >= timeToCatch)
                {
                    _elapsedTime = 0;
                    target.SetActive(false);
                    AnimalCatched?.Invoke(target);
                }
            }
        }
        else
        {
            _catchAreaMesh.enabled = false;
        }
    }

    private GameObject TryGetClosest(IEnumerable<Collider> hits)
    {
        var minDistanceToSheep = float.MaxValue;
        GameObject closestCollider = null;
        
        foreach (var hit in hits)
        {
            float currentDistance = Vector3.Distance(transform.position, hit.gameObject.transform.position);
            
            if (currentDistance < minDistanceToSheep)
            {
                minDistanceToSheep = currentDistance;
                closestCollider = hit.gameObject;
            }
        }

        return closestCollider;
    }
}
