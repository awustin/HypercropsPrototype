using UnityEngine;
using System.Collections.Generic;
using System;

public class DataLoader : MonoBehaviour
{
    // TODO: Rename CardData and DeckData with Descriptor. Rename class
    private static DataLoader _instance;
    public static DataLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<DataLoader>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DataLoader");
                    _instance = singletonObject.AddComponent<DataLoader>();
                }
            }

            return _instance;
        }
    }

    public string CropsPath = "/Data/Crops";
    public string CardsCropsPath = "/Data/Cards/Crops";
    public string CardsInfrastructurePath = "/Data/Cards/Infrastructure";
    public string CardsTechPath = "/Data/Cards/Tech";
    public Dictionary<Species, CropDescriptor> CropDescriptors = new();
    public Dictionary<int, CardData> CardsData = new();
    public DeckData InitialDeckData = new();

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

    public void LoadGameDescriptors()
    {
        // Each type (crops, buildings, cards, etc) should be stored in a folder under Data/
        LoadCropDescriptors();

        // Load cards
        List<string> cards = FileUtils.ListJSONFiles(CardsCropsPath);
        List<string> cardInfrastructureFiles = FileUtils.ListJSONFiles(CardsInfrastructurePath);
        List<string> cardTechFiles = FileUtils.ListJSONFiles(CardsTechPath);

        cards.AddRange(cardInfrastructureFiles);
        cards.AddRange(cardTechFiles);

        foreach (string cardFile in cards)
        {
            CardData data = FileUtils.ReadJSON<CardData>(cardFile);

            CardsData.Add(data.id, data);
        }
    }

    public CropDescriptor GetCropDescriptor(string name)
    {
        Enum.TryParse(name, out Species nameToEnum);

        return CropDescriptors[nameToEnum];
    }

    public CardData GetCardData(int id)
    {
        return CardsData[id];
    }

    public List<CardWeight> GetInitialCardWeights()
    {
        if (!InitialDeckData.IsLoaded)
        {
            InitialDeckData = FileUtils.ReadJsonFromFile<DeckData>("Data/InitialDeck.json");
            InitialDeckData.IsLoaded = true;
        }

        return InitialDeckData.cardWeights;
    }

    private void LoadCropDescriptors()
    {
        List<string> cropFiles = FileUtils.ListJSONFiles(CropsPath);

        foreach (string cropFile in cropFiles)
        {
            CropDescriptor data = FileUtils.ReadJSON<CropDescriptor>(cropFile);
            string name = FileUtils.RemoveJSONExtension(cropFile);
            Enum.TryParse(name, out Species nameToEnum);

            CropDescriptors.Add(nameToEnum, data);
        }
    }
}
