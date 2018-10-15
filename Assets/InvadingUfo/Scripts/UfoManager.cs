using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoManager : MonoBehaviour
{
    public List<CommonCore> ufos = new List<CommonCore>();

    public int Count { get { return ufos.Count; } }




    public void Add(CommonCore ufo)
    {
        if (!ufos.Contains(ufo))
        {
            ufos.Add(ufo);
        }
    }

    public bool Remove(CommonCore ufo)
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
                item.gameObject.GetComponent<Health>().KillInstantly(gameObject);
            }
        }


    }

}
