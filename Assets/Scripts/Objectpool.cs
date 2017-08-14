using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The object pool maintins various objects that are normally created and destroyed
throughout the game - they are reused instead of garbage collected to as to
improve perfomance
*/
public class Objectpool : MonoBehaviour
{
    //stores the mobs - number and type set in Unity inspector
    [SerializeField]
    private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();

    //
    public GameObject GetObject(string type)
    {
        foreach(GameObject obj in pooledObjects)
        {
            //if the object is found but not currently active
            if(obj.name == type && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        //iterate through the prefabs looking for an object of the correct type
        for(int i = 0; i < objectPrefabs.Length; i++)
        {
            if(objectPrefabs[i].name == type)
            {
                GameObject newObj = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObj);
                newObj.name = type;
                return newObj;
            }
        }

        return null;
    }//end GetObject

    //makes given object inactive
    public void ReleaseObject(GameObject gameObj)
    {
        gameObj.SetActive(false);
    }
}
