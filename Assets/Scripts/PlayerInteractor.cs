using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    //The land the player is currently selecting
    Land selectedLand = null;

    //The interactable object the player is currently selecting
    InteractableObject selectedInteractable = null; 

    // Start is called before the first frame update
    void Start()
    {
        //Get access to our PlayerController component
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Check if raycast hits anything
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
        else
        {
            // If raycast doesn't hit anything, deselect both land and interactable
            DeselectLand();
            DeselectInteractable();
        }
    }

    // Helper method to deselect land
    void DeselectLand()
    {
        if(selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    // Helper method to deselect interactable
    void DeselectInteractable()
    {
        if(selectedInteractable != null)
        {
            selectedInteractable = null; 
        }
    }

    //Handles what happens when the interaction raycast hits something interactable
    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;
        
        //Check if the player is going to interact with land
        if(other.tag == "Land")
        {
            //Get the land component
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            // Deselect any interactable when on land
            DeselectInteractable();
            return; 
        }

        //Check if the player is going to interact with an Item
        if(other.tag == "Item")
        {
            //Set the interactable to the currently selected interactable
            selectedInteractable = other.GetComponent<InteractableObject>();
            // Deselect any land when on an item
            DeselectLand();
            return; 
        }

        // If we hit something that's not land or an item, deselect both
        DeselectLand();
        DeselectInteractable();
    }

    //Handles the selection process of the land
    void SelectLand(Land land)
    {
        //Set the previously selected land to false (If any)
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }
        
        //Set the new selected land to the land we're selecting now. 
        selectedLand = land; 
        land.Select(true);
    }

    //Triggered when the player presses the tool button
    public void Interact()
    {
        //The player shouldn't be able to use his tool when he has his hands full with an item
        if(InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }

        //Check if the player is selecting any land
        if(selectedLand != null)
        {
            selectedLand.Interact();
            return; 
        }

        Debug.Log("Not on any land!");
    }

    //Triggered when the player presses the item interact button
    public void ItemInteract()
    {
        //If the player is holding something, keep it in his inventory
        if(InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }

        //If the player isn't holding anything, pick up an item

        //Check if there is an interactable selected
        if (selectedInteractable != null)
        {
            //Pick it up
            selectedInteractable.Pickup();
        }
    }
}