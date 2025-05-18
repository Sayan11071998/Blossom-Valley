using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;

    private Land selectedLand = null;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, interactionDistance))
        {
            OnInteractableHit(hit);
        }
        else
        {
            if (selectedLand != null)
            {
                selectedLand.Select(false);
                selectedLand = null;
            }
        }
    }

    private void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        if (other.CompareTag("Land"))
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