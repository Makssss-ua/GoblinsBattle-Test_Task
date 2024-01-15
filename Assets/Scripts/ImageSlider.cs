using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlider : MonoBehaviour
{
    [SerializeField] private Image slideImage;
    [SerializeField] private Vector2 minMaxValue = new Vector2(0f, 1f);
    [SerializeField] private float Value = 0f;
    [Space(5f)]
    [Header("Additional settings")]
    [SerializeField] private Slider refSlider;
    [SerializeField] private bool updateInUpdate;
    [SerializeField] private bool animatedUpdate;
    public Action OnChangeMinMax;
    public Action OnUpdateValue;

    private void OnEnable()
    {
        slideImage.fillAmount = 0f;
        UpdateValue();
    }

    public void SetMinMaxValue(Vector2 minmaxvalue)
    {
        minMaxValue = minmaxvalue;
        OnChangeMinMax?.Invoke();
        SetValue(Value);
    }
    public void SetMinMaxValue(float min, float max)
    {
        minMaxValue = new Vector2(min, max);
        OnChangeMinMax?.Invoke();
        SetValue(Value);
    }

    public void SetValue(float value)
    {
        Value = Mathf.Clamp(value, minMaxValue.x, minMaxValue.y);
        OnUpdateValue?.Invoke();
        UpdateValue();
    }

    public void UpdateValue()
    {
        if (animatedUpdate && gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(AnimatedUpdate((Value - minMaxValue.x) / (minMaxValue.y - minMaxValue.x)));
        }
        else
            slideImage.fillAmount = (Value - minMaxValue.x) / (minMaxValue.y - minMaxValue.x);
    }

    IEnumerator AnimatedUpdate(float targetValue)
    {
        float time = 0f;
        float startValue = slideImage.fillAmount;
        while (slideImage.fillAmount != targetValue)
        {
            slideImage.fillAmount = Mathf.Lerp(startValue, targetValue, time);
            time += 4f * Time.unscaledDeltaTime;
            if(Mathf.Abs(slideImage.fillAmount - targetValue) <= 0.01)
            {
                slideImage.fillAmount = targetValue;
                break;
            }
            yield return null;
        }
    }

    public void SetSliderValue()
    {
        if (refSlider)
        {
            SetMinMaxValue(refSlider.minValue, refSlider.maxValue);
            SetValue(refSlider.value);
        }
    }

    public Vector2 GetMinMaxValue()
    {
        return minMaxValue;
    }

    public float GetNormalizedValue()
    {
        return (Value - minMaxValue.x) / (minMaxValue.y - minMaxValue.x);
    }

    private void LateUpdate()
    {
        if (updateInUpdate)
            UpdateValue();
    }
}
