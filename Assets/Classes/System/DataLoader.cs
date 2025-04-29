using UnityEngine;
using System.Collections.Generic;
using System;

public class DataLoader : MonoBehaviour
{
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

    public string CropDescriptorsPath = "/Data/Crops/Descriptors";
    public string CardDescriptorsPath = "/Data/Cards/Descriptors";
    public string InitialDeckPath = "Data/InitialDeck.json";

    private readonly Dictionary<Species, CropDescriptor> _cropDescriptorsLoaded = new();
    private readonly Dictionary<int, CardDescriptor> _cardDescriptorsLoaded = new();
    private DeckData _initialDeckData = new();

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
        LoadCardDescriptors();
    }

    public CropDescriptor GetCropDescriptor(string name)
    {
        Enum.TryParse(name, out Species nameToEnum);

        return _cropDescriptorsLoaded[nameToEnum];
    }

    public CardDescriptor GetCardDescriptor(int id)
    {
        return _cardDescriptorsLoaded[id];
    }

    public List<CardWeight> GetInitialCardWeights()
    {
        if (!_initialDeckData.IsLoaded)
        {
            _initialDeckData = FileUtils.ReadJsonFromFile<DeckData>(InitialDeckPath);
            _initialDeckData.IsLoaded = true;
        }

        return _initialDeckData.cardWeights;
    }

    private void LoadCropDescriptors()
    {
        List<string> cropFiles = FileUtils.ListJSONFiles(CropDescriptorsPath);

        foreach (string cropFile in cropFiles)
        {
            CropDescriptor data = FileUtils.ReadJSON<CropDescriptor>(cropFile);
            string name = FileUtils.RemoveJSONExtension(cropFile);
            Enum.TryParse(name, out Species nameToEnum);

            _cropDescriptorsLoaded.Add(nameToEnum, data);
        }
    }

    private void LoadCardDescriptors()
    {
        List<string> cards = FileUtils.ListJSONFiles($"{CardDescriptorsPath}/Crops");
        List<string> cardInfrastructureFiles = FileUtils.ListJSONFiles($"{CardDescriptorsPath}/Infrastructure");
        List<string> cardTechFiles = FileUtils.ListJSONFiles($"{CardDescriptorsPath}/Tech");

        cards.AddRange(cardInfrastructureFiles);
        cards.AddRange(cardTechFiles);

        foreach (string cardFile in cards)
        {
            CardDescriptor data = FileUtils.ReadJSON<CardDescriptor>(cardFile);

            _cardDescriptorsLoaded.Add(data.id, data);
        }
    }
}
