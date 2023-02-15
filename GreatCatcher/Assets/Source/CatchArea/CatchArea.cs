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
    private float _timeToCatch = 3f;
    private float _elapsedTime;
    private Bag _bag;
    private MeshCollider _collider;
    private float _viewAngle = 90;

    public float Radius => _radius;
    public float ElapsedTime => _elapsedTime;
    public float TimeToCatch => _timeToCatch;
    
    public event Action<GameObject> AnimalCatched;

    private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        _player = GetComponentInParent<Player>();
        _bag = GetComponentInParent<Bag>();
        _catchAreaMesh = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Collider[] hits = new Collider[MaxColliders];
        Physics.OverlapSphereNonAlloc(transform.position, _radius, hits);
        hits = hits.Where(hit => hit != null && hit.TryGetComponent(out Animal animal)).ToArray();
        var catchTarget = TryGetClosest(hits);

        if (_bag.AnimalsInBag < _bag.MaxAmountOfAnimalsInBag)
        {
            Catch(catchTarget);
        }
        else
        {
            _catchAreaMesh.enabled = false;
            _collider.enabled = false;
        }
    }
    

    private void Catch(GameObject target)
    {
        const int angleDivider = 2;
        const float maxDegreesDelta = 360f;
        
        if (target != null && target.TryGetComponent(out Animal animal))
        {
            if (animal.Level > _player.Level) return;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            _catchAreaMesh.enabled = true;
            
            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / angleDivider)
            {
                _elapsedTime += Time.deltaTime;
                var direction = (target.transform.position - transform.position).normalized;
                direction.y = 0f;
                
                _player.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), maxDegreesDelta);
             
                if (_elapsedTime >= _timeToCatch)
                {
                    _elapsedTime = 0;
                    target.SetActive(false);
                    target.TryGetComponent(out UIContainer uiContainer);
                    uiContainer.CatchBar.TurnOffCanvas();
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
