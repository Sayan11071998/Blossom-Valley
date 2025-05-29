using UnityEngine;
using UnityEngine.AI;

public class AnimalRenderer : MonoBehaviour
{

    [SerializeField] private Animator childModel, adultModel;

    private NavMeshAgent agent;
    private Animator animatorToWorkWith;
    private int age;
    private AnimalData animalType;

    private void Start() => agent = GetComponent<NavMeshAgent>();

    public void RenderAnimal(int age, string animalName)
    {
        animalType = AnimalStats.GetAnimalTypeFromString(animalName);
        this.age = age;

        animatorToWorkWith = (age >= animalType.daysToMature) ? adultModel : childModel;

        childModel.gameObject.SetActive(false);
        adultModel.gameObject.SetActive(false);
        animatorToWorkWith.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (animatorToWorkWith != null)
            animatorToWorkWith.SetBool("Walk", agent.velocity.sqrMagnitude > 0);
    }
}