using System;

namespace CardRemovalTool
{
    [Serializable]
    public class OmahaHand
    {
        public Card Card1 { get; set; }
        public Card Card2 { get; set; }
        public Card Card3 { get; set; }
        public Card Card4 { get; set; }

        public OmahaHand(Card card1, Card card2, Card card3, Card card4)
        {
            Card1 = card1;
            Card2 = card2;
            Card3 = card3;
            Card4 = card4;
        }

        public static OmahaHand GetFromString(string text)
        {
            text = text.Trim().Replace(" ", string.Empty);

            if (text.Length != 8)
                return null;

            var card1 = Card.GetFromString(text.Substring(0, 2));
            var card2 = Card.GetFromString(text.Substring(2, 2));
            var card3 = Card.GetFromString(text.Substring(4, 2));
            var card4 = Card.GetFromString(text.Substring(6, 2));

            if (card1 == null || card2 == null || card3 == null || card4 == null)
                return null;

            return new OmahaHand(card1, card2, card3, card4);
        }

        public override string ToString()
        {
            return $"{Card1}{Card2}{Card3}{Card4}";
        }

    }
}
