using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimalInstaller : MonoInstaller
{
    [SerializeField] private Animal _animal;
    [SerializeField] private Transform _animalContainer;
    
    public override void InstallBindings()
    {
        var animalInstance = Container.InstantiatePrefabForComponent<Animal>(_animal, _animalContainer);
        Container.Bind<Animal>().FromInstance(_animal).AsSingle().NonLazy();
    }
}
