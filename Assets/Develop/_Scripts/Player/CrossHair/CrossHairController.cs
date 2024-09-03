using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] private Image _crossHairBorder;
    [SerializeField] private Image _crossHairCenter;
    [SerializeField] private Image _crossHairDot;
    [Space] 
    [SerializeField] private Image _spellCircleOutline;
    [SerializeField] private Image _spellCircleContent;

    private TweenerCore<float, float, FloatOptions> _timeTween;
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var color = new Color(_spellCircleContent.color.r, _spellCircleContent.color.g, _spellCircleContent.color.b, 1);
            DOTween.To(() => _spellCircleContent.color, x => _spellCircleContent.color = x, color, .3f).SetEase(Ease.Flash);
            DOTween.To(() => _spellCircleOutline.fillAmount, x => _spellCircleOutline.fillAmount = x, 1, .15f).SetEase(Ease.Linear);
            _timeTween.Kill();
            _timeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, .1f, .15f).SetEase(Ease.OutSine);
            //DOTween.To(() => _crossHairDot.color, x => _crossHairDot.color = x, color, .3f).SetEase(Ease.Flash);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            var color = new Color(_spellCircleContent.color.r, _spellCircleContent.color.g, _spellCircleContent.color.b, 0);
            DOTween.To(() => _spellCircleContent.color, x => _spellCircleContent.color = x, color, .3f).SetEase(Ease.Flash);
            DOTween.To(() => _spellCircleOutline.fillAmount, x => _spellCircleOutline.fillAmount = x, 0, .15f).SetEase(Ease.Linear);
            _timeTween.Kill();
            _timeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, .05f).SetEase(Ease.InSine);
            //DOTween.To(() => _crossHairDot.color, x => _crossHairDot.color = x, color, .3f).SetEase(Ease.Flash);
        }
    }
}
