using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardRemovalTool
{
    public partial class MainForm : Form
    {
        private Dictionary<CardsPair, decimal> BaseHoldemRange { get; set; } = new Dictionary<CardsPair, decimal>();
        private Dictionary<OmahaHand, decimal> BaseOmahaRange { get; set; } = new Dictionary<OmahaHand, decimal>();
        private Dictionary<Card, decimal> CardWeights { get; set; } = new Dictionary<Card, decimal>();

        private bool CSV_Mode { get; set; } = false;

        public MainForm()
        {
            InitializeComponent();

            InitBaseHoldem100Range();            
            InitBaseCardWeights();
        }

        private void InitBaseCardWeights()
        {
            CardWeights.Clear();
            foreach (var p in BaseHoldemRange)
            {
                if (!CardWeights.ContainsKey(p.Key.Left))
                {
                    CardWeights[p.Key.Left] = 1m;
                }
                if (!CardWeights.ContainsKey(p.Key.Right))
                {
                    CardWeights[p.Key.Right] = 1m;
                }
            }

            outputTextBox.Text += $"Base deck 52 cards {Environment.NewLine}{Environment.NewLine}";
        }

        private Dictionary<CardsPair, decimal> GetHoldemRangeFromText(string text)
        {
            var range = new Dictionary<CardsPair, decimal>();
            foreach (var item in text.Replace(Environment.NewLine,"").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var tokens = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                var weight = 1m;
                if (tokens.Count() == 2)
                    weight = decimal.Parse(tokens[1], CultureInfo.InvariantCulture);

                if (weight == 0)
                    continue;

                var handString = tokens[0];
                if (handString.Length == 4)
                {
                    var hand = CardsPair.GetFromString(handString);
                    range.Add(hand, weight);
                }
                else
                {
                    if (handString.Length == 3 || handString.Length == 2 && handString[0] == handString[1])
                    {
                        var suited = handString.Contains("s");
                        handString = handString.Replace("s", "").Replace("o", "");
                        if (suited)
                            handString = $"[{handString}]";

                        var genHand = GeneralizedCardsPair.FromGenString(handString);
                        var pairs = genHand.GetCardsPairsList();
                        foreach (var p in pairs)
                            range.Add(p, weight);
                    }
                    else
                    {
                        var genHand = GeneralizedCardsPair.FromGenString(handString);
                        var pairs = genHand.GetCardsPairsList();
                        foreach (var p in pairs)
                            range.Add(p, weight);

                        genHand = GeneralizedCardsPair.FromGenString($"[{handString}]");
                        pairs = genHand.GetCardsPairsList();
                        foreach (var p in pairs)
                            range.Add(p, weight);
                    }
                }
            }

            return range;
        }

        private Dictionary<OmahaHand, decimal> GetOmahaRangeFromText(string text)
        {
            var range = new Dictionary<OmahaHand, decimal>();
            foreach (var item in text.Replace(Environment.NewLine, "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var tokens = item.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                var weight = 1m;
                if (tokens.Count() == 2)
                    weight = decimal.Parse(tokens[1], CultureInfo.InvariantCulture)/100;

                if (weight == 0)
                    continue;

                var handString = tokens[0];
                if (handString.Length == 8)
                {
                    var hand = OmahaHand.GetFromString(handString);
                    range.Add(hand, weight);
                }
            }

            return range;
        }

        private Dictionary<Card, decimal> CalcCardWeights(Dictionary<CardsPair, decimal> range)
        {
            var dict = new Dictionary<Card, decimal>();
            foreach (var pair in range)
            {
                var left = pair.Key.Left;
                var right = pair.Key.Right;

                if (!dict.ContainsKey(left))
                    dict[left] = pair.Value;
                else
                    dict[left] += pair.Value;

                if (!dict.ContainsKey(right))
                    dict[right] = pair.Value;
                else
                    dict[right] += pair.Value;
            }

            var summWeight = dict.Sum(t => t.Value);

            var keylist = dict.Keys.ToList();
            foreach (var key in keylist)
            {
                dict[key] = dict[key] / summWeight * 2;
            }

            return dict;
        }

        private Dictionary<Card, decimal> CalcCardWeights(Dictionary<OmahaHand, decimal> range)
        {
            var dict = new Dictionary<Card, decimal>();
            foreach (var pair in range)
            {
                var c1 = pair.Key.Card1;
                var c2 = pair.Key.Card2;
                var c3 = pair.Key.Card3;
                var c4 = pair.Key.Card4;

                if (!dict.ContainsKey(c1))
                    dict[c1] = pair.Value;
                else
                    dict[c1] += pair.Value;

                if (!dict.ContainsKey(c2))
                    dict[c2] = pair.Value;
                else
                    dict[c2] += pair.Value;

                if (!dict.ContainsKey(c3))
                    dict[c3] = pair.Value;
                else
                    dict[c3] += pair.Value;

                if (!dict.ContainsKey(c4))
                    dict[c4] = pair.Value;
                else
                    dict[c4] += pair.Value;
            }

            var summWeight = dict.Sum(t => t.Value);

            var keylist = dict.Keys.ToList();
            foreach (var key in keylist)
            {
                dict[key] = dict[key] / summWeight * 4;
            }

            return dict;
        }

        private Dictionary<Card, decimal> SubstructCardWeights(Dictionary<Card, decimal> fromRange, Dictionary<Card, decimal> range)
        {
            var resRange = new Dictionary<Card, decimal>();
            foreach (var item in fromRange)
            {
                if (range.ContainsKey(item.Key))
                {
                    resRange.Add(item.Key, item.Value - range[item.Key]);
                }
                else
                {
                    resRange.Add(item.Key, item.Value);
                }
            }
            return resRange;
        }

        private void cardDistributionClick(object sender, EventArgs e)
        {
            var hcStart = "";
            foreach (var cv in CardWeights.Select(t => $"{t.Key}:{Math.Round(t.Value, 6)}"))
            {
                var tempLetter = cv.Substring(0, 1);
                if (tempLetter == hcStart)
                    outputTextBox.Text += "  " + cv;
                else
                {
                    hcStart = tempLetter;
                    outputTextBox.Text += Environment.NewLine;
                    outputTextBox.Text += cv;
                }
            }
            outputTextBox.Text += Environment.NewLine + Environment.NewLine;

            var total = 0m;
            if (rbHoldem.Checked)
            {
                total = CalcTotalCombinationsHoldem();
            }
            else
            {
                total = CalcTotalCombinationsOmaha();
            }

            outputTextBox.Text += $"{Math.Round(total,2)} totalHands {Environment.NewLine}{Environment.NewLine}";

        }

        private void InitBaseHoldem100Range()
        {
            var genPairs = GeneralizedCardsPair.GetAllGeneralizedPairs();
            var allPairs = new List<CardsPair>();
            foreach (var pair in genPairs)
            {
                var list = pair.GetCardsPairsList();
                foreach (var item in list)
                    BaseHoldemRange.Add(item, 1);
            }

            CardWeights = CalcCardWeights(BaseHoldemRange);

            outputTextBox.Text += $"Base range 100% {Environment.NewLine}{Environment.NewLine}";
        }

        private void InitBaseOmaha100Range()
        {
            outputTextBox.Invoke((MethodInvoker)delegate
            {
                outputTextBox.Text += $"Loading omaha start range {Environment.NewLine}{Environment.NewLine}";
            });

            var counter = 0;
            progressBar.Invoke((MethodInvoker)delegate
            {
                progressBar.Maximum = 270;
                progressBar.Value = 0;
            });
            foreach (var line in File.ReadAllLines("fullrange.csv"))
            {
                var tokens = line.Split(new char[] { ',' });
                BaseOmahaRange.Add(OmahaHand.GetFromString(line), 1m);
                counter++;

                if (counter % 1000 == 0)
                {
                    progressBar.Invoke((MethodInvoker)delegate
                    {
                        progressBar.Value++;
                    });
                }
            }

            progressBar.Invoke((MethodInvoker)delegate
            {
                progressBar.Value = 0;
            });

            outputTextBox.Invoke((MethodInvoker)delegate
            {
                outputTextBox.Text += $"270725 starters loaded {Environment.NewLine}{Environment.NewLine}";
            });
        }

        private void Reset()
        {
            CardWeights.Clear();

            outputTextBox.Clear();

            InitBaseCardWeights();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnAddFromClipboard_Click(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();
            if (string.IsNullOrEmpty(text))
                return;

            if (rbHoldem.Checked)
            {
                var newRange = GetHoldemRangeFromText(text);

                if (!newRange.Any())
                    return;

                AdjustRangeToWeights(newRange);

                var newRangeWeights = CalcCardWeights(newRange);

                CardWeights = SubstructCardWeights(CardWeights, newRangeWeights);

                var totalWeight = newRange.Sum(t => t.Value);
                var p = Math.Round(totalWeight * 100 / 1326, 2);

                outputTextBox.Text += $"Added range {p}% {Environment.NewLine}";

                var sumCardWeights = Math.Round(CardWeights.Sum(t => t.Value));

                outputTextBox.Text += $"Deck {sumCardWeights} {Environment.NewLine}{Environment.NewLine}";
            }
            else
            {
                var newRange = GetOmahaRangeFromText(text);

                if (!newRange.Any())
                    return;

                AdjustRangeToWeights(newRange);

                var newRangeWeights = CalcCardWeights(newRange);

                CardWeights = SubstructCardWeights(CardWeights, newRangeWeights);

                var totalWeight = newRange.Sum(t => t.Value);
                var p = Math.Round(totalWeight * 100 / 270725, 2);

                outputTextBox.Text += $"Added range {p}% {Environment.NewLine}";

                var sumCardWeights = Math.Round(CardWeights.Sum(t => t.Value));

                outputTextBox.Text += $"Deck {sumCardWeights} {Environment.NewLine}{Environment.NewLine}";
            }
        }

        private decimal AdjustRangeToWeights(Dictionary<CardsPair, decimal> newRange)
        {
            var result = 0m;
            foreach (var key in newRange.Keys.ToList())
            {
                var wl = CardWeights[key.Left];
                var wr = CardWeights[key.Right];
                var w = newRange[key];
                var res = w * wl * wr;
                result += res;
                newRange[key] = res;
            }

            var maxWeight = newRange.Max(t => t.Value);
            foreach (var key in newRange.Keys.ToList())
                newRange[key] /= maxWeight;

            return result;
        }

        private decimal AdjustRangeToWeights(Dictionary<OmahaHand, decimal> newRange)
        {
            var result = 0m;
            foreach (var key in newRange.Keys.ToList())
            {
                var w1 = CardWeights[key.Card1];
                var w2 = CardWeights[key.Card2];
                var w3 = CardWeights[key.Card3];
                var w4 = CardWeights[key.Card4];

                var w = newRange[key];
                var res = w * w1 * w2 * w3 * w4;
                result += res;
                newRange[key] = res;
            }

            var maxWeight = newRange.Max(t => t.Value);
            foreach (var key in newRange.Keys.ToList())
                newRange[key] /= maxWeight;

            return result;
        }

        private decimal CalcTotalCombinationsHoldem()
        {
            var result = 0m;
            foreach (var key in BaseHoldemRange.Keys.ToList())
            {
                var wl = CardWeights[key.Left];
                var wr = CardWeights[key.Right];
                var res = wl * wr;
                result += res;
            }

            return result;
        }

        private decimal CalcTotalCombinationsOmaha()
        {
            var result = 0m;
            foreach (var key in BaseOmahaRange.Keys.ToList())
            {
                var w1 = CardWeights[key.Card1];
                var w2 = CardWeights[key.Card2];
                var w3 = CardWeights[key.Card3];
                var w4 = CardWeights[key.Card4];
                var res = w1 * w2 * w3 * w4;
                result += res;
            }

            return result;
        }

        private void btnGetCurrentWithRemoval_Click(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();
            if (string.IsNullOrEmpty(text))
                return;

            if (rbHoldem.Checked)
            {
                var newRange = GetHoldemRangeFromText(text);

                if (!newRange.Any())
                    return;

                var rangeCombos = Math.Round(AdjustRangeToWeights(newRange));
                var totalCombos = Math.Round(CalcTotalCombinationsHoldem());
                var percents = Math.Round(rangeCombos / totalCombos * 100.0m, 2);

                var range = string.Join(",", newRange.Select(t => $"{t.Key}:{t.Value.ToString().Replace(",", ".")}"));

                Clipboard.SetText(range);

                outputTextBox.Text += $"New range is in clipboard {rangeCombos}/{totalCombos} {percents}% {Environment.NewLine}{Environment.NewLine}";
            }
            else
            {
                var newRange = GetOmahaRangeFromText(text);

                if (!newRange.Any())
                    return;

                var rangeCombos = Math.Round(AdjustRangeToWeights(newRange));
                var totalCombos = Math.Round(CalcTotalCombinationsOmaha());
                var percents = Math.Round(rangeCombos / totalCombos * 100.0m, 2);

                var range = string.Join(",", newRange.Select(t => $"{t.Key}@{Math.Round(t.Value*100).ToString().Replace(",", ".")}"));

                if (CSV_Mode)
                    range = ConvertRange2CSV(range);

                Clipboard.SetText(range);

                outputTextBox.Text += $"New range is in clipboard {rangeCombos}/{totalCombos} {percents}% {Environment.NewLine}{Environment.NewLine}";
            }

        }

        private string ConvertRange2CSV(string range)
        {
            var lines = new List<string>();
            lines.Add("Combo,Weight,EV");

            foreach (var hand in range.Split(new char[] { ','}))
            {
                var tokens = hand.Split(new char[] { '@' });
                var h = tokens[0];
                var w = int.Parse(tokens[1])/100.0;
                var line = $"{h},{w.ToString().Replace(",",".")},0";
                lines.Add(line);
            }


            return string.Join(Environment.NewLine, lines);
        }

        private void ConvertFile(string sourceFile, string destFile, int colIndex)
        {
            var lines = File.ReadAllLines(sourceFile);

            var resItems = new List<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("Hand"))
                    continue;

                var tokens = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var hand = tokens[0].Replace(" ", "");
                var weight = decimal.Parse(tokens[colIndex].Replace(",", "."), CultureInfo.InvariantCulture) / 100;

                if (weight == 0)
                    continue;

                var newItem = $"{hand}:{weight.ToString().Replace(",", ".").TrimEnd(new char[] {'0'}).TrimEnd(new char[] {'.'}) }";
                resItems.Add(newItem);
            }
            File.WriteAllText(destFile, string.Join(",", resItems));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ConvertFile(@"C:\funnyregency\trunk\GTOHUD\HoldemHUD\preflop\100.csv", @"c:\omaha\EP_fold.txt", 2);
            ConvertFile(@"C:\funnyregency\trunk\GTOHUD\HoldemHUD\preflop\100-f.csv", @"c:\omaha\MP_fold.txt", 2);
            ConvertFile(@"C:\funnyregency\trunk\GTOHUD\HoldemHUD\preflop\100-f-f.csv", @"c:\omaha\CO_fold.txt", 2);
            ConvertFile(@"C:\funnyregency\trunk\GTOHUD\HoldemHUD\preflop\100-f-f-f.csv", @"c:\omaha\BU_fold.txt", 2);
            ConvertFile(@"C:\funnyregency\trunk\GTOHUD\HoldemHUD\preflop\100-f-f-f.csv", @"c:\omaha\BU_open.txt", 4);
        }

        private async void rbOmaha_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbOmaha.Checked)
                return;

            Reset();

            outputGroupBox.Visible = true;

            await Task.Factory.StartNew(() =>
            {
                if (!BaseOmahaRange.Any())
                    InitBaseOmaha100Range();
            });
        }

        private void rbHoldem_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbHoldem.Checked)
                return;

            outputGroupBox.Visible = false;

            Reset();
        }

        private void rbPPT_CheckedChanged(object sender, EventArgs e)
        {
            CSV_Mode = false;
        }

        private void rbCSV_CheckedChanged(object sender, EventArgs e)
        {
            CSV_Mode = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var rangeA = GetHoldemRangeFromText(File.ReadAllText(@"C:\Users\Regent\Desktop\article 1\holdem_ranges\holdem_BU_open.txt"));
            var rangeB = GetHoldemRangeFromText(File.ReadAllText(@"C:\Users\Regent\Desktop\article 1\holdem_ranges\bu adjusted.txt"));

            var rangeC = new Dictionary<CardsPair, decimal>();
            foreach (var a in rangeA)
            {
                if (rangeB.ContainsKey(a.Key))
                {
                    var newval = a.Value - rangeB[a.Key];
                    if (newval > 0)
                        rangeC.Add(a.Key, Math.Round(100*newval));
                }
                else
                {
                    rangeC.Add(a.Key, Math.Round(100*a.Value));
                }
            }

            var range = string.Join(",", rangeC.Select(t => $"{t.Key}@{t.Value.ToString().Replace(",", ".")}"));
            File.WriteAllText(@"C:\Users\Regent\Desktop\article 1\holdem_ranges\subsctruct.txt", range);

        }
    }
}
