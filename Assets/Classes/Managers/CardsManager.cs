using System.Collections.Generic;
using System.Runtime.Serialization;
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
        State.SetNumberOfCardsInHand(CardsInHand.Count);
        CreateCardDebug(100, "Wheat", "Recovered Generic Wheat", CardType.Crop, 3);
        CreateCardDebug(600, "IrrigationPipe", "Irrigation Pipe", CardType.Infrastructure, 1);
        CreateCardDebug(1000, "RandomMutagenesis", "Random Mutagenesis", CardType.Tech, 1);
        // RestartHand();
    }

    public void CreateCardDebug(int id, string name, string label, CardType type, int number)
    {
        GameObject CardPrefab = Resources.Load<GameObject>("Prefabs/Cards/CardPrefab " + type.ToString());
        GameObject Deck = GameObject.Find("CardsPanel");
        GameObject CardInstance = Instantiate(CardPrefab, Deck.transform);
        Card CardScript = CardInstance.GetComponent<Card>();

        CardScript.id = id;
        CardScript.name = name;
        CardScript.label = label;
        CardScript.cardType = type;
        CardScript.order = CardsInHand.Count;
        CardScript.number = number;
        CardScript.enabled = true;

        CardsInHand.Add(CardInstance.GetInstanceID(), CardInstance);
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
        // instantiated.TurnUpInHand(DeckUtils.MapHandOrderToPosition(integer))

        // Update global state
        State.IncreaseNumberOfCardsInHand();
    }
}

public enum CardType
{
    [EnumMember(Value = "0")]
    Crop,
    [EnumMember(Value = "1")]
    Infrastructure,
    [EnumMember(Value = "2")]
    Tech,
    [EnumMember(Value = "3")]
    None,
}

public enum CardStatus
{
    FaceDown,
    TurnUp,
    Move,
    Idle,
    Discard,
}
