using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftable
{
    string GetName();
    Resource[] GetRequiredResources();
    bool CanCraft(Storage storage);
    void Craft(Storage storage);
}