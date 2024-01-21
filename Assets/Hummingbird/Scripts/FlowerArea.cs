using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Manage a colection of flower plants
/// </summary>
public class FlowerArea : MonoBehaviour
{
    // Use to observing relative distance from agents to flower
   public const float AreaDiameter = 20f;

    // List of all flower plants this this flower area
    private List<GameObject> flowerPlants;

    // A lookup dictionary for looking up flower from nectar collider
    private Dictionary<Collider, Flower> nectarFlowerDictionary;

    /// <summary>
    /// List of all flowers in the flower area
    /// </summary>
    public List<Flower> Flowers {get; private set;}

    public void ResetFlowers()
    {
        //Rotate flowers around Y, X and Z axis
        foreach (GameObject flowerPlant in flowerPlants)
        {
            float xRotation = UnityEngine.Random.Range(-5f,5f);
            float yRotation = UnityEngine.Random.Range(-180f, 180f);
            float zRotation = UnityEngine.Random.Range(-5f, 5f);
            flowerPlant.transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        foreach (Flower flower in Flowers)
        {
            flower.ResetFlower();
        }       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public Flower GetFlowerFromNectar(Collider collider)
    {
        return nectarFlowerDictionary[collider];
    }

    private void Awake()
    {
        //Initialize variables
        flowerPlants = new List<GameObject>();
        nectarFlowerDictionary = new Dictionary<Collider, Flower>();
        Flowers = new List<Flower> ();
    }
    ///
    /// Called this when the game starts
    ///

    private void Start()
    {
        // find all the flowers that are children of this GameObject/Transfrom  
        FindChildFlowers(transform);
    }

    /// <summary>
    /// Recursively finds all flowers and flower plants that are childern of a parent transfrom
    /// </summary>
    /// <param name="parent">The parent of the children to check</param>
    private void FindChildFlowers(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.CompareTag("flower_plant"))
            {
                //Found a flower plant, add it to the flowerPlant list
                flowerPlants.Add(child.gameObject);

                //Look for flower within the flower plant
                FindChildFlowers (child);
            }

            else
            {
                //Not a flower plant, look for a flower component
                Flower flower = child.GetComponent<Flower>();
                if(flower != null)
                {
                    //Found a flower, add it to the flower list
                    Flowers.Add(flower);

                    //Add the nectar collider to the looking dictionary
                    nectarFlowerDictionary.Add(flower.nectarCollider, flower);

                    //Note: there are no flowers that are children of other flowers
                }
                else
                {
                    //Flower component not found, so check children
                    FindChildFlowers(child);
                }
            }
        }
    }
}
