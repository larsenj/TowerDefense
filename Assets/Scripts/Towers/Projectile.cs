using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //projectile's target
    private Mobs target;

    //reference to parent tower
    private Towers parent;

    private Element elementType;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        SeekTarget();
	}

    //set parent and target
    public void Init(Towers parent)
    {
        this.parent = parent;
        this.target = parent.TargetMob;
        this.elementType = parent.elementType;
    }//end Init

    private void SeekTarget()
    {
        if(target != null && target.IsActive)
        {
            //Vector3.MoveTowards(Vector3 current, Vector3 target, float maxdistancedelta
            transform.position = Vector3.MoveTowards(transform.position,
                target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
        }
        else if(!target.IsActive)   //despawn the projectile
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }

    }//end SeekTarget

    //have the projectiles damage the mob
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Mob")
        {
            //don't count mobs the projectile passes through to the tgt
            if (target.gameObject == collision.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);
                ApplyEffect();
            }
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }//end ontriggerenter2d

    //apply the effect to the mob
    public void ApplyEffect()
    {

        target.AddStatus(parent.GetEffect());
    }//end ApplyEffect

}//end class Projectile
