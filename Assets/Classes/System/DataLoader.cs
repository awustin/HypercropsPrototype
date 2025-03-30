using UnityEngine;
using System.Collections.Generic;

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

    public string CropsPath = "/Data/Crops";
    public string CardsCropsPath = "/Data/Cards/Crops";
    public string CardsInfrastructurePath = "/Data/Cards/Infrastructure";
    public string CardsTechPath = "/Data/Cards/Tech";
    public Dictionary<string, CropData> CropsData = new();
    public Dictionary<int, CardData> CardsData = new();

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

    public void LoadGameData()
    {
        // Each type (crops, buildings, cards, etc) should be stored in a folder under Data/

        // Load crops
        List<string> cropFiles = FileUtils.ListJSONFiles(CropsPath);

        foreach (string cropFile in cropFiles)
        {
            CropData data = FileUtils.ReadJSON<CropData>(cropFile);
            string name = FileUtils.RemoveJSONExtension(cropFile);

            CropsData.Add(name, data);
        }

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

    public CropData GetCropData(string name)
    {
        return CropsData[name];
    }
}

[System.Serializable]
public class CropData
{
    public CropStageData Ghost;
    public CropStageData Seed;
    public CropStageData Growing;
    public CropStageData Ready;
    public CropStageData Dead;

    public CropStageData GetStage(string stageName)
    {
        switch (stageName)
        {
            case ("Ghost"):
                return this.Ghost;
            case ("Seed"):
                return this.Seed;
            case ("Growing"):
                return this.Growing;
            case ("Ready"):
                return this.Ready;
            case ("Dead"):
                return this.Dead;
            default:
                return new CropStageData();
        }
    }

    public override string ToString()
    {
        return Ghost.name + "-" + Seed.name + "-" + Growing.name + "-" + Ready.name + "-" + Dead.name + "";
    }
}

[System.Serializable]
public class CropStageData
{
    public string name;
    public string meshCollider;
    public List<string> materials;
}

[System.Serializable]
public class CardData
{
    public int id;
    public CardType type;
    public string name;

    public override string ToString()
    {
        return name;
    }
}
