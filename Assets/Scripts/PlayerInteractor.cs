using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Land selectedLand = null;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
    }

    private void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        if (other.tag == "Land")
        {
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }

    private void SelectLand(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }

        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        if (selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land!");
    }
}