using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private const int MaxColliders = 7;
    
    [SerializeField] private MeshRenderer _catchAreaMesh;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _unloadAreaTarget;
    
    private ArrowRenderer _arrowRenderer;
    private Player _player;
    private float _radius = 5f;
    private float _timeToCatch = 3f;
    private float _elapsedTime;
    private Bag _bag;
    private SphereCollider _collider;
    private float _viewAngle = 90;
    private GameObject _lastTryCatchAnimal;

    public float Radius => _radius;
    public float ElapsedTime => _elapsedTime;
    public float TimeToCatch => _timeToCatch;
    
    public event Action<GameObject> AnimalCatched;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _player = GetComponentInParent<Player>();
        _bag = GetComponentInParent<Bag>();
        _catchAreaMesh = GetComponent<MeshRenderer>();
        _arrowRenderer = _player.GetComponentInChildren<ArrowRenderer>();
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
        
        if (_bag.AnimalsInBag >= _bag.MaxAmountOfAnimalsInBag)
        {
            _catchAreaMesh.enabled = false;
            _collider.enabled = false;
            _arrowRenderer.gameObject.SetActive(true);
            _arrowRenderer.GetNewEndPoint(_unloadAreaTarget.position);
            return;
        }
        
        
        _collider.enabled = true;
        
        if (hits.Length != 0)
        {
            if (catchTarget.TryGetComponent(out UIContainer container))
            {
                container.CatchBar.TurnOnCanvas();
                container.LockImage.Open();
                Catch(catchTarget, container);
            }
        }
        else
        {
            if (_lastTryCatchAnimal != null)
            {
                _lastTryCatchAnimal.TryGetComponent(out UIContainer lastTryCatchAnimalContainer);
                lastTryCatchAnimalContainer.LockImage.Close();
                lastTryCatchAnimalContainer.CatchBar.TurnOffCanvas();
                _lastTryCatchAnimal = null;
            }
            
            _catchAreaMesh.enabled = false;
            _elapsedTime = 0;
        }
    }
    

    private void Catch(GameObject target, UIContainer uiContainer)
    {
        const int angleDivider = 2;
        const float maxDegreesDelta = 30f;

        if (target.TryGetComponent(out Animal animal))
        {
            if (animal.Level > _player.Level) return;
            uiContainer.LockImage.Close();
            _lastTryCatchAnimal = target;
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / angleDivider)
            {
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
                    uiContainer.CatchBar.TurnOffCanvas();
                    AnimalCatched?.Invoke(target);
                }
            }
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
