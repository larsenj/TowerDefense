using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobs : MonoBehaviour
{
    //detemines how fast the mobs move - actual value will be set in the prefab
    //so different mobs can have different speeds
    [SerializeField]
    private float speed;
    
    //determine if mob is moving or not
    public bool IsActive { get; set; }

    //track the path the mob will follow
    private Stack<Node> path;

    public Point GridPosition { get; set; }

    //tracks the next tile to move to
    private Vector3 destination;

    public void Spawn()
    {
        transform.position = TileManager.Instance.StartPoint.transform.position;
        SetPath(TileManager.Instance.Path);
        IsActive = true;

    }//end spawn

    private void Update()
    {
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
    }

    //remove any debuffs and remove the mob from the screen
    private void Despawn()
    {
        IsActive = false;   //set to inactive
        GridPosition = TileManager.Instance.StartSpawn;//reset position
        GameManager.Instance.Pool.ReleaseObject(gameObject);//release back to pool
        GameManager.Instance.RemoveMob(this);//re-enable wave button, remove from active list
    }

}
