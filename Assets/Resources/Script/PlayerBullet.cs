
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IActorTemplate
{
    int travelSpeed;
    int health;
    int hitPower;
    GameObject actor;

    [SerializeField]
    SOActorModel bulletModel;
    
    void Awake()
    {
        ActorStats(bulletModel);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if(other.GetComponent<IActorTemplate>()!= null)
            {
                if (health >= 1)
                {
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
            }
           
            if (health <= 0)
            {
                Die();
            }
        }
    }
    void Movement()
    {
        transform.localPosition += new Vector3(Time.deltaTime * travelSpeed, 0, 0);
    }
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        actor = actorModel.actor;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

}
