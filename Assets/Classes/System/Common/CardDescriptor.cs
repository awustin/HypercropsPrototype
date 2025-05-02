using System;

using Assets.Classes.Common.Enums;

namespace Assets.Classes.System.Common
{
    [Serializable]
    public class CardDescriptor
    {
        public int Id;
        public CardType Type;
        public string Name;
        public string Label;

        #nullable enable
        public string? Attribute;
        #nullable disable

        public override string ToString()
        {
            return $"{Label} ({Name})";
        }
    }
}
