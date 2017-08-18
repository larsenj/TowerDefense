using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { EARTH, WIND, FIRE, GROOVE }

//super class that specific tower types inherit from
public abstract class Towers : MonoBehaviour
{
    private SpriteRenderer towerSpriteRenderer;

    //for tracking a targeted mob
    private Mobs targetMob;
    public Mobs TargetMob
    {
        get
        {
            return targetMob;
        }
    }   
    //used to ensure targeted mob is dead before moving on to the next one
    private Queue<Mobs> mobQueue = new Queue<Mobs>();

    //determines is the tower can attack a mob - defaults to true
    private bool canAttack = true;
    //track time since firing
    private float attackTimer;
    //track how often can fire - adjusted in the inspector
    [SerializeField]
    private float attackCoolDown;

    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    //how long a status effect lasts
    [SerializeField]
    private float effectDuration;
    public float EffectDuration
    {
        get { return effectDuration; }
        set { this.effectDuration = value; }//in case upgrades are implemented
    }
    //[SerializeField]
    //private float proc; //if there is a chance of effect not applying that tic


    //specific type of projectile that the tower does
    [SerializeField]
    private string projectileType;

    public Element elementType { get; protected set; }

    //future-proof by allowing for projectiles ot have different speeds
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }

	// Use this for initialization
	void Start ()
    {
        //create a reference to the sprite
        towerSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Attack();

        //Debug.Log(targetMob);
	}

    public void Select()
    {
        //toggle the spriterenderer
        towerSpriteRenderer.enabled = !towerSpriteRenderer.enabled;

    }//end Select

    //runs when mobs enter a tower's range
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Mob")
        {
            //add the mob to the queue
            mobQueue.Enqueue(collision.GetComponent<Mobs>());
            //targetMob = collision.GetComponent<Mobs>();
        }
    }//end ontriggerenter2d

    //attack the target mob
    public void Attack()
    {
        //increment the time until the tower can fire again
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCoolDown)
            {
                canAttack = true;
                attackTimer = 0;//reset timer
            }
        }  
        //if current target gone and targets still available, target next in queue
        if(targetMob == null && mobQueue.Count > 0)
        {
            targetMob = mobQueue.Dequeue();
        }
        //keep attacking target if active, and can attack them
        if(targetMob != null && targetMob.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                //set to false in case moves/dies, and so there's time between shots
                canAttack = false;
            }
        }
        else if(mobQueue.Count > 0)
        {
            targetMob = mobQueue.Dequeue();
        }

        //if target dead, set target to null
        if ( (targetMob != null && !targetMob.Alive) ||
            (targetMob != null && !targetMob.IsActive) )
            targetMob = null;


    }//end Attack

    //instantiates a projectile
    private void Shoot()
    {
        //gets a projectile from the object pool and initialize it
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.Init(this);
        projectile.transform.position = transform.position;
    }//end shoot

    //untrack the mob after it leaves the range
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mob")
        {
            //remove the mob from the queue
            //mobQueue.Dequeue(collision.GetComponent<Mobs>());
            targetMob = null;
        }
    }//end ontriggerexit

    //get the type of damage a tower does - varies, so implemented by specific
    //tower
    public abstract StatusEffects GetEffect();
}//end class Towers

