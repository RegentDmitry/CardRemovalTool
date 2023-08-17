using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CardRemovalTool
{
    public class CardsPair : IComparable
    {
        public Card Left { get; set; }
        public Card Right { get; set; }

        public static CardsPair GetFromString(string text)
        {
            text = text.Trim().Replace(" ", string.Empty);

            if (text.Length != 4)
                return null;

            var left = Card.GetFromString(text.Substring(0, 2));
            var right = Card.GetFromString(text.Substring(2, 2));

            if (left == null || right == null)
                return null;

            return new CardsPair(left,right);
        }

        public int CompareTo(object c)
        {
            var rp = (CardsPair) c;

            var i = rp.Left.Value.CompareTo(Left.Value);
            if (i != 0)
                return i;

            i = rp.Right.Value.CompareTo(Right.Value);
            if (i != 0)
                return i;

            var lhc = GetHashCode();
            var rhc = rp.GetHashCode();
            return rhc.CompareTo(lhc);
        }

        public CardsPair() { }

        public CardsPair(Card left, Card right)
        {
            if (left.Equals(right))
                throw new Exception("wrong pair");

            var ordered = left.GetHashCode() >= right.GetHashCode();
            Left = ordered ? left : right;
            Right = ordered ? right : left;
        }

        public CardSuit Suit
        {
            get
            {
                if (Left.Value == Right.Value)
                    return CardSuit.PAIR;

                return Left.Color == Right.Color ? CardSuit.SUITED : CardSuit.OFFSUIT;
            }
        }

        public GeneralizedCardsPair Generalized
        {
            get
            {
                return new GeneralizedCardsPair(Left.Value, Right.Value, Suit);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Left, Right);
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode()*52 + Right.GetHashCode();
        }

        public static Int32 GetListHashCode(CardsPair[] cpl)
        {
            var vector = new BitVector32();
            var first = true;

            var counter = 0;
            var colors = new Dictionary<CardColor, int>();

            var section = new BitVector32.Section();

            foreach (var cp in cpl)
            {
                for (var i = 0; i < 2; i++)
                {
                    var card = i == 0 ? (first ? cp.Right : cp.Left) : (first ? cp.Left : cp.Right);
                    section = BitVector32.CreateSection(12, section);
                    vector[section] = (int)card.Value;
                    if (!colors.ContainsKey(card.Color))
                        colors[card.Color] = counter++;

                    if (first) continue;
                    section = BitVector32.CreateSection(3, section);
                    vector[section] = colors[card.Color];
                }
                first = false;
            }
            return vector.Data;
        }

        public static CardsPair[] CardsFromHashCode(Int32 code)
        {
            var resultList = new List<CardsPair>();
            var vector = new BitVector32(code);


            var section = BitVector32.CreateSection(12);
            var left = (CardValue)vector[section];
            section = BitVector32.CreateSection(12, section);
            var right = (CardValue)vector[section];

            if (right <= left)
            {
                resultList.Add(new CardsPair(new Card(right, 0), new Card(left, (CardColor)1)));
            }
            else if (right > left)
            {
                resultList.Add(new CardsPair(new Card(right, 0), new Card(left, 0)));
            }

            for (var i = 0; i < 2; i++)
            {
                var tempSection = BitVector32.CreateSection(2048, section);
                if (vector[tempSection] == 0)
                    continue;

                section = BitVector32.CreateSection(12, section);
                left = (CardValue)vector[section];
                section = BitVector32.CreateSection(3, section);
                var leftcolor = (CardColor)vector[section];
                section = BitVector32.CreateSection(12, section);
                right = (CardValue)vector[section];
                section = BitVector32.CreateSection(3, section);
                var rightcolor = (CardColor)vector[section];
                resultList.Add(new CardsPair(new Card(left, leftcolor), new Card(right, rightcolor)));
            }
        
            return resultList.ToArray();
        }

        public static CardsPair[] StandardizeCardPairs(CardsPair[] cpl)
        {
            var resultList = new List<CardsPair>();
            var colors = new Dictionary<CardColor, CardColor>();
            var i = 0;
            foreach (var cp in cpl)
            {
                if (!colors.ContainsKey(cp.Left.Color))
                    colors[cp.Left.Color] = (CardColor)i++;
                if (!colors.ContainsKey(cp.Right.Color))
                    colors[cp.Right.Color] = (CardColor)i++;
                resultList.Add(new CardsPair(new Card(cp.Left.Value, colors[cp.Left.Color]),
                                             new Card(cp.Right.Value, colors[cp.Right.Color])));
            }
            return resultList.ToArray();
        }

        public static bool IsAvailableCardList(IEnumerable<CardsPair> cpList)
        {
            var valueList = new List<int>();
            foreach (var cp in cpList)
            {
                var hashCode = cp.Left.GetHashCode();
                if (valueList.Contains(hashCode))
                    return false;

                valueList.Add(hashCode);

                hashCode = cp.Right.GetHashCode();
                if (valueList.Contains(hashCode))
                    return false;

                valueList.Add(hashCode);
            }
            return true;
        }
    }
}