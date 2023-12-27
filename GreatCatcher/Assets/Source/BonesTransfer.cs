using UnityEngine;

[ExecuteInEditMode]
public class BonesTransfer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer sourceMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer targetMeshRenderer;
    [SerializeField] private Transform[] bones;
    
    public bool getBones;
    public bool setBones;

    private void Update()
    {
        if(getBones)
        {
            getBones = false;

            bones = sourceMeshRenderer.bones;
        }
        if (setBones)
        {
            targetMeshRenderer.bones = bones;
            setBones = false;
        }
    }
}

