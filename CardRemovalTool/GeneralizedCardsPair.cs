using System;
using System.Collections.Generic;

namespace CardRemovalTool
{
    public class GeneralizedCardsPair : IComparable
    {
        public CardValue Left { get; set; }
        public CardValue Right { get; set; }
        public CardSuit Suit { get; set; }

        public int Combinations
        {
            get
            {
                switch (Suit)
                {
                    case CardSuit.PAIR:
                        return 6; 
                    case CardSuit.SUITED:
                        return 4;
                    case CardSuit.OFFSUIT:
                        return 12;
                    default: throw new Exception("Suit?");
                }
            }
        }

        public int Amount
        {
            get
            {
                if (Suit == CardSuit.PAIR)
                    return 6;
                return Suit == CardSuit.SUITED ? 4 : 12;
            }
        }

        public GeneralizedCardsPair() { }

        private static CardValue Card2CardValue(string card)
        {
            switch (card)
            {
                case "A": return CardValue.ACE;
                case "K": return CardValue.KING;
                case "Q": return CardValue.QUEEN;
                case "J": return CardValue.JACK;
                case "T": return CardValue.TEN;
                case "9": return CardValue.NINE;
                case "8": return CardValue.EIGHT;
                case "7": return CardValue.SEVEN;
                case "6": return CardValue.SIX;
                case "5": return CardValue.FIVE;
                case "4": return CardValue.FOUR;
                case "3": return CardValue.TREY;
                case "2": return CardValue.DEUCE;
            }

            throw new Exception("unknown card");
        }

        public static GeneralizedCardsPair FromGenString(string hand)
        {
            var suited = false;
            if (hand.StartsWith("["))
            {
                suited = true;
                hand = hand.Replace("[", "").Replace("]", "");
            }
            var A = hand.Substring(0, 1);
            var B = hand.Substring(1, 1);
            var paired = A == B;

            var res = new GeneralizedCardsPair()
            {
                Left = Card2CardValue(A),
                Right = Card2CardValue(B),
                Suit = paired ? CardSuit.PAIR : (suited ? CardSuit.SUITED : CardSuit.OFFSUIT)
            };
            return res;
        }

        public override int GetHashCode()
        {
            var x = Suit == CardSuit.SUITED ? (int)Left : (int)Right;
            var y = Suit == CardSuit.SUITED ? (int)Right : (int)Left;
            return 13 * x + y;
        }
        
        public static List<GeneralizedCardsPair> GetAllGeneralizedPairs()
        {
            var result = new List<GeneralizedCardsPair>();
            foreach (CardValue left in Enum.GetValues(typeof(CardValue)))
                foreach (CardValue right in Enum.GetValues(typeof(CardValue)))
                    result.Add(new GeneralizedCardsPair(left > right ? left : right,
                                                        left > right ? right : left,
                                                        left == right
                                                            ? CardSuit.PAIR
                                                            : (left > right
                                                                   ? CardSuit.SUITED
                                                                   : CardSuit.OFFSUIT)));
            result.Sort();
            return result;
        }

        public int CompareTo(object c)
        {
            var rp = (GeneralizedCardsPair)c;

            if (rp.Left > Left)
                return 1;
            if (rp.Left < Left)
                return -1;
            if (rp.Right > Right)
                return 1;
            if (rp.Right < Right)
                return -1;
            if (rp.Suit > Suit) 
                return 1; 
            if (rp.Suit < Suit) 
                return -1; 

            return 0;
        }

        public override bool Equals(object obj)
        {
            var rp = (GeneralizedCardsPair)obj;
            return (Left == rp.Left) && (Right == rp.Right) && (Suit == rp.Suit);
        } 

        public GeneralizedCardsPair(CardValue left, CardValue right, CardSuit suit = CardSuit.PAIR)
        {
            if ((left != right && suit == CardSuit.PAIR) ||
                (left == right && suit != CardSuit.PAIR))
                throw new Exception("wrong cards");

            Left = left;
            Right = right;
            Suit = suit;
        }

        public GeneralizedCardsPair(int hashCode)
        {
            var x = (hashCode) / 13;
            var y = hashCode - x * 13;

            Left = (CardValue)(x > y ? x : y);
            Right = (CardValue) (x > y ? y : x);

            Suit = CardSuit.PAIR;
            switch (x.CompareTo(y))
            {
                case 1: Suit = CardSuit.SUITED; break;
                case -1: Suit = CardSuit.OFFSUIT; break;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Left.GetText(), Right.GetText(), Suit.GetText());
        }

        public CardsPair[] GetCardsPairsList()
        {
            var count = 0;
            switch (Suit)
            {
                case CardSuit.SUITED:
                    count = 4; break;
                case CardSuit.OFFSUIT:
                    count = 12; break;
                case CardSuit.PAIR:
                    count = 6; break;
            }
            var list = new CardsPair[count];

            switch (Suit)
            {
                case CardSuit.SUITED:
                    for (var i = 0; i < count; i++)
                        list[i] = new CardsPair(new Card(Left, (CardColor) i), new Card(Right, (CardColor) i));
                    break;

                case CardSuit.OFFSUIT:
                    var counter = 0;
                    for (var i = 0; i < 4; i++)
                        for (var j = 0; j < 4; j++ )
                        {
                            if (i==j)
                                continue;
                            list[counter++] = new CardsPair(new Card(Left, (CardColor) i), new Card(Right, (CardColor) j));
                        }
                    break;

                case CardSuit.PAIR:
                    counter = 0;
                    for (var i = 0; i < 4; i++)
                        for (var j = i + 1; j < 4; j++)
                            list[counter++] = new CardsPair(new Card(Left, (CardColor) i), new Card(Right, (CardColor) j));
                    break;
            }

            return list;
        }


        public static List<CardsPair[]> GetAllCardPairs(GeneralizedCardsPair[] gcpList)
        {
            //ћожно с помощью рекурсии сн€ть это ограничение, но практического смысле здесь нет
            if (gcpList.Length > 3 || gcpList.Length < 2)
                throw new Exception("неправильное количество элементов во входном массиве. должно быть 2 или 3");

            var cpList = new List<CardsPair[]>();

            foreach (var cp1 in gcpList[0].GetCardsPairsList())
                foreach (var cp2 in gcpList[1].GetCardsPairsList())
                    if (gcpList.Length == 2)
                    {
                        //var cpl = cp1.GetHashCode() >= cp2.GetHashCode() ? new[] { cp1, cp2 } : new[] { cp2, cp1 };
                        var cpl = new[] { cp1, cp2 };
                        if (CardsPair.IsAvailableCardList(cpl))
                            cpList.Add(cpl);
                    }
                    else foreach (var cp3 in gcpList[2].GetCardsPairsList())
                    {
                        var tempList = new List<CardsPair>(3) { cp1, cp2, cp3 };
                        tempList.Sort();
                        var cpl = tempList.ToArray();
                        if (CardsPair.IsAvailableCardList(cpl))
                            cpList.Add(cpl);
                    }

            return cpList;
        }

        public static Int32 GetArrayHashCode(GeneralizedCardsPair[] gcpList)
        {
            var mult = 1;
            var result = 0;
            for (int i = gcpList.Length; i > 0; i--)
            {
                result += gcpList[i-1].GetHashCode() * mult;
                mult *= 169;
            }
            return result;
        }

        public static GeneralizedCardsPair[] GetGCPListFromHashCode(Int32 code, bool three)
        {
            var list = new List<GeneralizedCardsPair>();

            var c = code;

            if (three)
            {
                var s = c/(169*169);
                list.Add(new GeneralizedCardsPair(s));
                c = c - s*169*169;
            }

            var sub = c/169;
            list.Add(new GeneralizedCardsPair(sub));
            list.Add(new GeneralizedCardsPair(c - sub*169));

            return list.ToArray();
        }

        public static bool IsAvailableCardList(IEnumerable<GeneralizedCardsPair> cpList)
        {
            var tempDic = new Dictionary<CardValue, int>();

            foreach (var gcp in cpList)
            {
                if (!tempDic.ContainsKey(gcp.Left))
                    tempDic.Add(gcp.Left,0);
                tempDic[gcp.Left]++;

                if (tempDic[gcp.Left] > 4)
                    return false;

                if (!tempDic.ContainsKey(gcp.Right))
                    tempDic.Add(gcp.Right, 0);
                tempDic[gcp.Right]++;

                if (tempDic[gcp.Right] > 4)
                    return false;
            }

            return true;
        }

    }
}