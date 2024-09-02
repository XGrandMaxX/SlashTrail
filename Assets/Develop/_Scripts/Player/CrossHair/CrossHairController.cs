using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] private Image _crossHairBorder;
    [SerializeField] private Image _crossHairCenter;
    [SerializeField] private Image _crossHairDot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var color = new Color(_crossHairCenter.color.r, _crossHairCenter.color.g, _crossHairCenter.color.b, .58f);
            _crossHairCenter.transform.DOScale(_crossHairBorder.transform.localScale/3, .3f).SetEase(Ease.Flash);
            DOTween.To(() => _crossHairCenter.color, x => _crossHairCenter.color = x, color, .3f).SetEase(Ease.Flash);
            //DOTween.To(() => _crossHairDot.color, x => _crossHairDot.color = x, color, .3f).SetEase(Ease.Flash);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            var color = new Color(_crossHairCenter.color.r, _crossHairCenter.color.g, _crossHairCenter.color.b, 0);
            _crossHairCenter.transform.DOScale(_crossHairBorder.transform.localScale, .3f).SetEase(Ease.Flash);
            DOTween.To(() => _crossHairCenter.color, x => _crossHairCenter.color = x, color, .3f).SetEase(Ease.Flash);
            //DOTween.To(() => _crossHairDot.color, x => _crossHairDot.color = x, color, .3f).SetEase(Ease.Flash);
        }
    }
}
