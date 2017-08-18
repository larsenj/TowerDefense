using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobs : MonoBehaviour
{
    //determine how much a mob is worth
    [SerializeField]
    private int value;
    public int Value { get { return value; } }
    
    //detemines how fast the mobs move - actual value will be set in the prefab
    //so different mobs can have different speeds
    private float speed;
    [SerializeField]
    public float MaxSpeed;
    public float Speed
    {
        get { return speed; }
        set { this.speed = value; }
    }
    
    //determine if mob is moving or not
    public bool IsActive { get; set; }

    //track the path the mob will follow
    private Stack<Node> path;

    //track where in the world is... Carmen Sandiego! But not really...
    public Point GridPosition { get; set; }
    private SpriteRenderer mobLayer;

    //tracks the next tile to move to
    private Vector3 destination;

    //track the health of the mob
    [SerializeField]
    private float maxHealth;
    private float health;
    public bool Alive { get { return health > 0; } }//return true if health greater than zero

    //track any sort of status effect currently applied
    private List<StatusEffects> debuffs = new List<StatusEffects>();

    //extra lists to preven race conditions
    private List<StatusEffects> debuffsToRemove = new List<StatusEffects>();
    private List<StatusEffects> debuffsToAdd = new List<StatusEffects>();

    public void Spawn()
    {
        debuffs.Clear();    //remove all debuffs, since reusing object
        transform.position = TileManager.Instance.StartPoint.transform.position; //set position
        SetPath(TileManager.Instance.Path); //find the path
        IsActive = true;
        if (GameManager.Instance.Wave <= 5)
            health = maxHealth;
        else
            health = maxHealth + GameManager.Instance.Wave;
        mobLayer = GetComponent<SpriteRenderer>();
        //MaxSpeed = speed;
        speed = MaxSpeed;
    }//end spawn

    private void Update()
    {
        HandleEffects();
        Move(); 
    }//end Update
    

    //moves the mob from start to end points
    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination,
                speed * Time.deltaTime);

            //check if reached next tile
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
    }//end Move

    //ensure mob has destination immediately when spawned - executed by Spawn()
    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }//end SetPath

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EndPoint")
        {
            Despawn();
            //Destroy(gameObject);

            //decrement lives
            GameManager.Instance.Lives--;
        }

        if (collision.tag == "Tile")
        {
            mobLayer.sortingOrder = collision.GetComponent<TileScript>().GridPosition.Y;
        }
    }

    //decrement health due to damage, check if dead
    public void TakeDamage(float damage, Element damageType)
    {
        if (IsActive)
        {
            health -= damage;
            if (health <= 0)
            {
                GameManager.Instance.Currency += Value;
                Despawn();
                GetComponent<SpriteRenderer>().sortingOrder--;

            }
        }
    }//end TakeDamage

    //remove any debuffs and remove the mob from the screen
    private void Despawn()
    {
        IsActive = false;   //set to inactive
        GridPosition = TileManager.Instance.StartSpawn;//reset position
        debuffsToAdd.Clear();
        debuffsToRemove.Clear();
        debuffs.Clear();

        GameManager.Instance.Pool.ReleaseObject(gameObject);//release back to pool
        GameManager.Instance.RemoveMob(this);//re-enable wave button, remove from active list
    }//end Despawn

    //add status effects to the mob
    public void AddStatus(StatusEffects effect)
    {
        //lambda expression: iterates through list looking for any item that
        //has the same status effect as the parameter
        if (!debuffs.Exists(x => x.GetType() == effect.GetType()))
            debuffsToAdd.Add(effect);

    }//end AddStatus

    //iterate through the list of effects and update/apply them
    private void HandleEffects()
    {
        //check if there are new debuffs
        if(debuffsToAdd.Count > 0)
        {
            //if so, add them, then clear the "to add" list
            //lock (debuffs)
            //{
                debuffs.AddRange(debuffsToAdd);
            //}
            //lock (debuffsToAdd)
            //{
                debuffsToAdd.Clear();
            //}
        }

        //iterate through the remove list, removeing as necessary
        //lock (debuffsToRemove)
        //{
            foreach (StatusEffects effect in debuffsToRemove)
            {
          //      lock (debuffs)
          //      {
                    debuffs.Remove(effect);
          //      }
            }
        debuffsToRemove.Clear();
        // }
        lock (debuffs)
        {
            foreach (StatusEffects effect in debuffs)
                effect.Update();    //call individual status effect update function
        }
    }//end HandleEffects

    //remove a status effect
    public void RemoveEffect(StatusEffects effect)
    {
        //add it to the "to remove" list
        debuffsToRemove.Add(effect);
    }//end RemoveEffect

}//end class
