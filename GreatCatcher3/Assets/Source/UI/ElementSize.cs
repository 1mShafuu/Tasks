using UnityEngine;
using UnityEngine.UI;

public class ElementSize : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup; 
    [SerializeField] private RectTransform[] _elements; 
    
    private Vector2 _portraitSize = new Vector2(150, 55); 
    private Vector2 _landscapeSize = new Vector2(330, 85);

    private bool _isPortraitMode;

    private void Start()
    {
        _isPortraitMode = Screen.width < Screen.height;
        UpdateElementSizes();
    }

    private void Update()
    {
        bool isCurrentPortraitMode = Screen.width < Screen.height;
        
        if (_isPortraitMode == isCurrentPortraitMode) return;
        
        _isPortraitMode = isCurrentPortraitMode;
        UpdateElementSizes();
    }

    private void UpdateElementSizes()
    {
        if (gridLayoutGroup != null)
        {
            // Определение целевого размера
            Vector2 targetSize = _isPortraitMode ? _portraitSize : _landscapeSize;

            // Изменение размера компонента Grid Layout Group и элементов
            gridLayoutGroup.cellSize = targetSize;
            
            foreach (RectTransform element in _elements)
            {
                element.sizeDelta = targetSize;
            }
        }
    }
}