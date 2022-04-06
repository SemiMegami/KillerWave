using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActorStats(SOActorModel actorModel)
    {
       // throw new System.NotImplementedException();
    }

    public void Die()
    {
       // throw new System.NotImplementedException();
    }

    public int SendDamage()
    {
        return 0;
      //  throw new System.NotImplementedException();
    }

    public void TakeDamage(int incomingDamage)
    {
      //  throw new System.NotImplementedException();
    }
}
