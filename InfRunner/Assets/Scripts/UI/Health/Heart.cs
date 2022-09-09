using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Heart : MonoBehaviour
{
   private const float _startValue = 0;
   private const float _endValue = 1;
   
   [SerializeField] private float _lerpDuration;
   
   private Image _image;
   private Coroutine _coroutine;

   private void Awake()
   {
      _image = GetComponent<Image>();
      _image.fillAmount = 1;
   }

   public void ToFill()
   {
      if (_coroutine != null)
      {
         StopCoroutine(_coroutine);
      }

      _coroutine = StartCoroutine(Filling(_startValue, _endValue, _lerpDuration, Fill));
   }

   public void ToEmpty()
   {
      if (_coroutine != null)
      {
         StopCoroutine(_coroutine);
      }

      _coroutine = StartCoroutine(Filling(_endValue, _startValue, _lerpDuration, Destroy));
   }

   private void Destroy(float value)
   {
      _image.fillAmount = value;
      Destroy(gameObject);
   }

   private void Fill(float value)
   {
      _image.fillAmount = value;
   }
   
   private IEnumerator Filling(float startValue, float endValue, float duration, UnityAction<float> lerpingEnd)
   {
      float elapsed = 0;
      float nextValue;

      while (elapsed < duration)
      {
         nextValue = Mathf.Lerp(startValue, endValue, elapsed / duration);
         _image.fillAmount = nextValue;
         elapsed += Time.deltaTime;
         yield return null;
      }
      
      lerpingEnd?.Invoke(endValue);
   }
}
