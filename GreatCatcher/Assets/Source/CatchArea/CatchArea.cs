using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 7;
    
    [SerializeField] private MeshRenderer _catchAreaMesh;
    [SerializeField] private ParticleSystem _particleSystem;
     
    private Player _player;
    private float _radius = 5f;
    private float _timeToCatch = 3f;
    private float _elapsedTime;
    private Bag _bag;
    //private MeshCollider _collider;
    private SphereCollider _collider;
    private float _viewAngle = 90;
    private GameObject _lastTryCatchAnimal;

    public float Radius => _radius;
    public float ElapsedTime => _elapsedTime;
    public float TimeToCatch => _timeToCatch;
    
    public event Action<GameObject> AnimalCatched;

    private void Awake()
    {
        //_collider = GetComponent<MeshCollider>();
        _collider = GetComponent<SphereCollider>();
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
            _collider.enabled = true;
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
        const float maxDegreesDelta = 30f;

        if (target != null && target.TryGetComponent(out Animal animal))
        {
            if (animal.Level > _player.Level) return;
            _lastTryCatchAnimal = target;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            target.TryGetComponent(out UIContainer uiContainer); 
            var targetUI = uiContainer;

            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / angleDivider)
            {
                targetUI.CatchBar.TurnOnCanvas();
                _catchAreaMesh.enabled = true;
                _elapsedTime += Time.deltaTime;
                var direction = (target.transform.position - transform.position).normalized;
                direction.y = 0f;
                _player.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), maxDegreesDelta);
             
                if (_elapsedTime >= _timeToCatch)
                {
                    _particleSystem.gameObject.transform.position = target.transform.position;
                    _particleSystem.Play();
                    _elapsedTime = 0;
                    target.SetActive(false);
                    //target.TryGetComponent(out UIContainer uiContainer);
                    targetUI.CatchBar.TurnOffCanvas();
                    AnimalCatched?.Invoke(target);
                }
            }
        }
        else
        {
            if (_lastTryCatchAnimal != null)
            {
                _lastTryCatchAnimal.TryGetComponent(out UIContainer uiContainer);
                uiContainer.CatchBar.TurnOffCanvas();
            }
            
            _catchAreaMesh.enabled = false;
            _elapsedTime = 0;
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
