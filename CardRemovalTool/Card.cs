using System;
using System.Collections.Generic;

namespace CardRemovalTool
{
    public enum CardValue
    {
        [TextValue("2")]
        DEUCE,

        [TextValue("3")]
        TREY,

        [TextValue("4")]
        FOUR,

        [TextValue("5")]
        FIVE,

        [TextValue("6")]
        SIX,

        [TextValue("7")]
        SEVEN,

        [TextValue("8")]
        EIGHT,

        [TextValue("9")]
        NINE,

        [TextValue("T")]
        TEN,

        [TextValue("J")]
        JACK,

        [TextValue("Q")]
        QUEEN,

        [TextValue("K")]
        KING,

        [TextValue("A")]
        ACE
    }

    public enum CardSuit
    {
        [TextValue("s")]
        SUITED,

        [TextValue("o")]
        OFFSUIT,

        [TextValue("")]
        PAIR
    }

    public enum CardColor
    {
        [TextValue("h")]
        HEART,

        [TextValue("d")]
        DIAMONDS,

        [TextValue("c")]
        CLUBS,

        [TextValue("s")]
        SPADES
    }

    [Serializable]
    public class Card
    {
        public CardColor Color { get; set; }
        public CardValue Value { get; set; }

        public Card() { }

        public Card(CardValue value, CardColor color)
        {
            Value = value;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            var right = obj as Card;
            return right != null && (Color == right.Color && Value == right.Value);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", _cardValsDict[Value], _cardColorsDict[Color]);
        }

        public override int GetHashCode()
        {
            return ((int) Value*4 + (int) Color);
        }

        private static Dictionary<CardValue, string> _cardValsDict = new Dictionary<CardValue, string>()
        {
            { CardValue.ACE, "A" },
            { CardValue.KING, "K" },
            { CardValue.QUEEN, "Q" },
            { CardValue.JACK, "J" },
            { CardValue.TEN, "T" },
            { CardValue.NINE, "9" },
            { CardValue.EIGHT, "8" },
            { CardValue.SEVEN, "7" },
            { CardValue.SIX, "6" },
            { CardValue.FIVE, "5" },
            { CardValue.FOUR, "4" },
            { CardValue.TREY, "3" },
            { CardValue.DEUCE, "2" }
        };

        private static Dictionary<CardColor, string> _cardColorsDict = new Dictionary<CardColor, string>()
        {
            { CardColor.CLUBS, "c" },
            { CardColor.DIAMONDS, "d" },
            { CardColor.HEART, "h" },
            { CardColor.SPADES, "s" }
        };


        public static Card GetFromString(string text)
        {
            text = text.Trim();

            if (text.Length != 2)
                return null;

            var cardtext = text[0].ToString();
            var colortext = text[1].ToString();
            object resultValue = null;
            object resultColor = null;

            foreach (CardValue cardvalue in Enum.GetValues(typeof(CardValue)))
            {
                if (_cardValsDict[cardvalue] == cardtext)
                //if (cardvalue.GetText() == cardtext)
                {
                    resultValue = cardvalue;
                    break;
                }
            }

            foreach (CardColor cardcolor in Enum.GetValues(typeof(CardColor)))
            {
                if (_cardColorsDict[cardcolor] == colortext)
                //if (cardcolor.GetText() == colortext)
                {
                    resultColor = cardcolor;
                    break;
                }
            }

            if (resultValue == null || resultColor == null)
                return null;

            return new Card((CardValue)resultValue,
                            (CardColor)resultColor);
        }
    }
}
