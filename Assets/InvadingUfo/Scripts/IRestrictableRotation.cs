using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Ame
{
    public interface IRestrictableRotation
    {

        void RestrictRotation();
        void FreeRotation();
    }
}
