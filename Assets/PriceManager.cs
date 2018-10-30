using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceManager : MonoBehaviour
{
    public Transform houseHolder;
    public Transform apartmentHolder;
    public Transform buildingHolder;


    //event
    //損害発生
    public event System.Action<long> OnPriceDamage;
    public long damagePrice = 0;

    public event System.Action OnDamagePriceChanged;

    private void Start()
    {
        SetEventToChildren(houseHolder);
        SetEventToChildren(apartmentHolder);
        SetEventToChildren(buildingHolder);
    }

    void SetEventToChildren(Transform parent)
    {
        var fds = parent.GetComponentsInChildren<IFinancialDamage>();
        foreach (var item in fds)
        {
            item.OnFinancialDamageOccured += (p) =>
            {
                damagePrice += p;
                OnDamagePriceChanged?.Invoke();
            };
        }
    }

}
