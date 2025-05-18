using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Tools")]
    [SerializeField] private ItemData[] tools = new ItemData[8];
    public ItemData equippedTool = null;

    [Header("Items")]
    [SerializeField] private ItemData[] items = new ItemData[8];
    public ItemData equippedItem = null;
}