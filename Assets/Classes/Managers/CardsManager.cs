using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private static CardsManager _instance;
    public static CardsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CardsManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new("CardsManager");
                    _instance = singletonObject.AddComponent<CardsManager>();
                }
            }

            return _instance;
        }
    }

    public GameState State;
    public int MaxNumberOfCards = 4;
    public List<int> CropCardIdsList = new() { 100 };
    public List<int> InfrastructureCardIdsList = new() { 600 };
    public List<int> TechCardIdsList = new() { 1000 };
    public Dictionary<int, GameObject> CardsInHand = new();
    [HideInInspector] public ObjectFactory Factory;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        State = GameState.Instance;
        Factory = ObjectFactory.Instance;
        State.SetNumberOfCardsInHand(CardsInHand.Count);
        // RestartHand();

        CreateCardDebug(100);
        CreateCardDebug(101);
        CreateCardDebug(600);
        CreateCardDebug(1000);

        // TODO: Create data files to hold the initial card list
        // TODO: Add card to advance time
        // TODO: Create system to shuffle cards
    }

    public void CreateCardDebug(int id)
    {
        GameObject cardInstance = Factory.MakeCard(id, CardsInHand.Count, GameObject.Find("CardsPanel").transform);
        CardsInHand.Add(cardInstance.GetInstanceID(), cardInstance);
    }

    public void RestartHand()
    {
        // Read cards data and any other param to calculate draw
        DestroyCurrentCards();
        DrawCardRandom(CropCardIdsList);
        DrawCardRandom(CropCardIdsList);
        DrawCardRandom(InfrastructureCardIdsList);
        DrawCardRandom(TechCardIdsList);
    }

    public void UseCardAndDiscard(GameObject selected)
    {
        // Apply effect/trigger event
        // Find card in dictionary and remove
        // Re order and animate
        // Destroy instance
    }

    private void DestroyCurrentCards()
    {
        if (CardsInHand.Count == 0)
        {
            return;
        }

        // TODO: destroy instances
    }

    private void DrawCardRandom(List<int> cardIds)
    {
        if (CardsInHand.Count == MaxNumberOfCards)
        {
            return;
        }

        // Select random value from cardIds

        // Instantiate card based on JSON data
        GameObject instantiated = new("Card Placeholder");
        instantiated.SetActive(true);

        // Add card to CardsInHand dictionary with InstanceID
        CardsInHand.Add(instantiated.GetInstanceID(), instantiated);

        // Calculate order in hand (integer number to indicate position)
        // instantiated.TurnUpInHand(order)

        // Update global state
        State.IncreaseNumberOfCardsInHand();
    }
}
