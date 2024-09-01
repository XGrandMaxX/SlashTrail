using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells/SpellBase")]
public class SpellBase : ScriptableObject
{
    [SerializeField] private GameObject handEffect;
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private RFX4_EffectSettings spellProjectile;

    private RFX4_PhysicsMotion collisionObject;

    protected Vector3 direction;
    
    
    public bool isHandling;
    public void PrepareSpell(Transform pos)
    {
        Destroy(Instantiate(handEffect, pos),10f);
        
        onSpellPreparing?.Invoke();

        onSpellHandlingEnded += (s) =>
        {
            Debug.Log(s.HitPoint);
        };
    }

    public void ReleaseSpell(Vector3 pos,Vector3 direction)
    {
        isHandling = true;
        SetDirection(direction);
        onSpellReleased?.Invoke(pos,direction);
        CreateProjectile(pos);
    }

    public async UniTask HandleSpell()
    {
        while (isHandling)
        {
            onSpellHandling?.Invoke();
            await UniTask.Yield();
        }
    }
    
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void CreateProjectile(Vector3 pos)
    {
        var spell = Instantiate(spellProjectile, pos, spellProjectile.transform.rotation);
        collisionObject = spell.GetComponentInChildren<RFX4_PhysicsMotion>();
        collisionObject.CollisionEnter += (i, a) =>
        {
            Debug.Log("ENTERED");
            onSpellHandlingEnded?.Invoke(a);
            isHandling = false;
            Destroy(spell.gameObject,spell.FadeoutTime);
        };
        spell.transform.rotation = Quaternion.LookRotation(this.direction);
        Destroy(spell.gameObject,10); //УБРАТЬ ЕСЛИ БУДЕТ НЕ НУЖЕН
    }

    public event Action<Vector3,Vector3> onSpellReleased;
    public event Action onSpellHandling;
    public event Action<RFX4_PhysicsMotion.RFX4_CollisionInfo> onSpellHandlingEnded;
    public event Action onSpellPreparing;
}
