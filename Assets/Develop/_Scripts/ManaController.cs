using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public float MaxMana;

    public bool mayCast = true;

    public event Action OnManaBlock;
    public event Action OnManaRelease;

    private void Start()
    {
        Mana = MaxMana;
    }

    public float Mana
    {
        get => mana;
        set
        {
            if (value > MaxMana)
            {
                mana = MaxMana;
                return;
            }

            mana = value;

        }
    }

    private float mana;
    public float ManaRegenerationSpeed;

    public float SpellCost;

    public void SpendMana()
    {
        Mana -= SpellCost;
        print("Spend");
        print(Mana);
        if (Mana < 0)
        {
            Mana = 0;
            BlockSpellAsync();
        }
    }
    public void Update()
    {
        if (Mana < MaxMana)
            Mana += ManaRegenerationSpeed * Time.deltaTime;
    }

    private async UniTask BlockSpellAsync()
    {
        mayCast = false;
        OnManaBlock?.Invoke();;
        while (Mana < MaxMana)
        {
            await Task.Yield();
        }
        mayCast = true;
        OnManaRelease?.Invoke();;
    }
}
