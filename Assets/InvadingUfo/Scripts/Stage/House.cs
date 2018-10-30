using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : AttractableObject, IFinancialDamage
{

    public event Action<long> OnFinancialDamageOccured;

    public void TakeFinancialDamage()
    {
        var price = GetComponent<Price>();
        OnFinancialDamageOccured?.Invoke(price.price);
    }
}
