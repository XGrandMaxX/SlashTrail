using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] private SpellBase currentSpell;
    [SerializeField] private Transform castPosition;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private ManaController _manaController;

    [SerializeField] private MySlider _manaSlider;

    private void Start()
    {
        _manaController.OnManaBlock += _manaSlider.Block;
        _manaController.OnManaRelease += _manaSlider.Unblock;

        _manaSlider.Value = 50;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PrepareSpell();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
           ReleaseSpell();  
        }

        _manaSlider.Value = _manaController.Mana;
    }

    public void PrepareSpell()
    {
        currentSpell.PrepareSpell(castPosition);
    }

    public void ReleaseSpell()
    {
        if (!_manaController.mayCast)
            return;
        
        var pos = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit);
        var direction = hit.point - castPosition.position;
        currentSpell.ReleaseSpell(cameraTransform.position, cameraTransform.forward);
        currentSpell.onSpellHandlingEnded += info =>
        {
            var colliders = Physics.OverlapSphere(info.HitPoint, 8);

            foreach (var c in colliders)
            {
                if (c.TryGetComponent(out PlayerCharacter player))
                {
                    Debug.Log("ASSY");
                    var direction = player.transform.position - info.HitPoint;
                    player.Throw(new Vector3(direction.x,direction.y ,direction.z), 50);
                }
            }
        };

        _manaController.SpendMana();
    }
    
}
