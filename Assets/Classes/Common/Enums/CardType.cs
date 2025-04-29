using System.Runtime.Serialization;

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
