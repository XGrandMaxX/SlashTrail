using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    private void Awake()
    {
        _startSize = _fill.rect.width;
    }

    [SerializeField] private RectTransform _fill;
    private Image _fillImage;

    private float _startSize;

    [SerializeField] private float maxvalue;
    [SerializeField] private float minvalue;
    [SerializeField] private float curvalue;

    private void Start()
    {
        _fillImage = _fill.GetComponent<Image>();
        MaxValue = maxvalue;
        MinValue = minvalue;
        Value = curvalue;
    }


    public float Value
    {
        get => value;
        set
        {
            if (value > MaxValue)
                this.value = MaxValue;
            if (value < MinValue)
                this.value = MinValue;
            this.value = value;
            CalculateSize(value);
        }
    }

    private float value;
    
    public float MaxValue { get => maxValue;
        set
        {
            maxValue = value;
            CalculateSize(value);
        } 
    }
    private float maxValue;
    
    public float MinValue { get => minValue;
        set
        {
            minValue = value;
            CalculateSize(value);
        }  }
    private float minValue;

    public void CalculateSize(float value)
    {

        var coefficient = (value - MinValue) / (MaxValue - MinValue);
        var size = _startSize * coefficient;

        _fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    public void Block()
    {
        _fillImage.color = Color.grey;
        
    }

    public void Unblock()
    {
        _fillImage.color = Color.white;
    }
    
}
