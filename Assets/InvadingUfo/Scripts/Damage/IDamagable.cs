using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public interface IDamageable
    {
        void ApplyDamage(float damageValue,GameObject attacker);
    }
}
