using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a single flower with nectar
/// </summary>

public class Flower : MonoBehaviour
{
    [Tooltip("The color when the flower is full")]
    public Color fullFlowerColor = new Color(1f, 0f, 3f);

    [Tooltip("The color when the flower is empty")]
    public Color emptyFlowerColor = new Color(.5f, 0f, 1f);

    /// <summary>
    /// The trigger collider repesenting the nectar
    /// </summary>
    [HideInInspector]
    public Collider nectarCollider;

    //The solid collider repesenting the flower petals
    private Collider flowerCollider;

    //The flower's material
    private Material flowerMaterial;

    /// <summary>
    /// Help to tell the orientation of the flowers
    /// </summary>
    public Vector3 FlowerUpVector
    {
        get
        {
            return nectarCollider.transform.up;
        }

    }
     public Vector3 FlowerCenterPosition
    {
        get
        {
            return nectarCollider.transform.position;
        }
    }

    /// <summary>
    /// The amount of nectar remaining in the flower
    /// </summary>
    public float NectarAmount { get;private set; }

    /// <summary>
    /// Whether the flower have nectar remaining
    /// </summary>
    public bool HasNectar
    {
        get
        {
            return NectarAmount > 0f;
        }
    }

    /// <summary>
    /// Attemp to remove nectar from the flower
    /// </summary>
    /// <param name="amount"> The amount of nectar to remove</param>
    /// <returns> The actual amount of nectar succesfully removed </returns>
    
    public float Feed(float amount)
    {
        float nectarTaken =Mathf.Clamp(amount, 0f, NectarAmount);

        //subtract the nectar
        NectarAmount -= amount;

        if(NectarAmount <= 0) 
            {   
                //No nectar remainning  
                NectarAmount = 0;

                //Disable the flower and nectar colliders
                flowerCollider.gameObject.SetActive(false);
                nectarCollider.gameObject.SetActive(false);

                //Change the flower color to indicate that it is empty  
                flowerMaterial.SetColor("_BaseColor", emptyFlowerColor);
            }
        //return the amount of nectar that was taken
        return nectarTaken;
    } 

    /// <summary>
    /// Reset the flower stage
    /// </summary>
    public void ResetFlower()
    {
        //Refill the nectar
        NectarAmount = 1f;

        //Enable the flower collider
        flowerCollider.gameObject.SetActive(true);
        nectarCollider.gameObject.SetActive(true);

        //Change the flower color to indicate that it is full
        flowerMaterial.SetColor("_BaseColor", fullFlowerColor);
    }
    
    /// <summary>
    /// Called when the flower wake up
    /// </summary>
    private void Awake()
    {
        //Find the flower's mesh renderer and get the main material
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        flowerMaterial = meshRenderer.material;

        //Find flower and nectar colliders
        flowerCollider = transform.Find("FlowerCollider").GetComponent<Collider>();
        nectarCollider = transform.Find("FlowerNectarCollider").GetComponent<Collider>();
    }
}

