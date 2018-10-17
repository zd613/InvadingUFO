using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoManager : MonoBehaviour
{
    public List<BaseUfoCore> ufos = new List<BaseUfoCore>();

    public int Count { get { return ufos.Count; } }




    public void Add(BaseUfoCore ufo)
    {
        if (!ufos.Contains(ufo))
        {
            ufos.Add(ufo);
        }
    }

    public bool Remove(BaseUfoCore ufo)
    {
        return ufos.Remove(ufo);
    }

    private void Update()
    {
        ufos.RemoveAll(x => !x.IsAlive);


        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var item in ufos)
            {
                item.Health.KillInstantly(gameObject);
            }
        }


    }

}
