using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFinancialDamage
{
    event System.Action<long> OnFinancialDamageOccured;
}
