using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public interface IDamagable
    {
        void ApplyDamage(float damageValue,GameObject attacker);
    }
}
