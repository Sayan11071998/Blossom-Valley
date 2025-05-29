using UnityEngine;
using UnityEngine.UI;

public class NPCRelationshipListing : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite fullHeart;

    [Header("UI Elements")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Image[] hearts;

    public void Display(CharacterScriptableObject characterData, NPCRelationshipState relationship)
    {
        portraitImage.sprite = characterData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }

    public void Display(AnimalData animalData, AnimalRelationshipState relationship)
    {
        portraitImage.sprite = animalData.portrait;
        nameText.text = relationship.name;

        DisplayHearts(relationship.Hearts());
    }

    private void DisplayHearts(float number)
    {
        foreach (Image heart in hearts)
            heart.sprite = emptyHeart;

        for (int i = 0; i < number; i++)
            hearts[i].sprite = fullHeart;
    }
}