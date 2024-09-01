using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] private SpellBase currentSpell;
    [SerializeField] private Transform castPosition;

    [SerializeField] private Transform cameraTransform;

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentSpell.PrepareSpell(castPosition);
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            var pos = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit);
            var direction = hit.point - castPosition.position;
            currentSpell.ReleaseSpell(castPosition.position,castPosition.transform.forward);
            currentSpell.onSpellHandlingEnded += info =>
            {
                var colliders = Physics.OverlapSphere(info.HitPoint, 5);

                foreach (var c in colliders)
                {
                    if (c.TryGetComponent(out PlayerCharacter player))
                    {
                        Debug.Log("ASSY");
                        var direction = player.transform.position - info.HitPoint;
                        player.Throw(new Vector3(direction.x,direction.y ,direction.z), 1);
                    }
                }
            };
        }
    }

    public void PrepareSpell()
    {
        
    }

    public void ReleaseSpell()
    {
        
    }
}
