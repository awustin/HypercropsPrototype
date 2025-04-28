using System.Runtime.Serialization;

public enum GameMode
{
    Default,
    Farming,
    Building,
}

public enum WorldTimeScale
{
    Normal = 1,
    Fast = 100,
    Fastest = 500,
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
