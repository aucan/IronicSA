using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronicSA
{
    class indexing
    {
        private string _dataPath;
        int dtIndex = 6;

        public Dictionary<string, int> vocabulary = new Dictionary<string, int>();
        public List<Dictionary<int, int>> tftable = new List<Dictionary<int, int>>();
        public List<double> values = new List<double>();
        public List<string> contradictions = new List<string>();
        dataset ds = new dataset();


        public indexing(string dataPath)
        {
            _dataPath = dataPath;
            getContradictions();
            for (int i = 0; i < 6; i++)
                vocabulary.Add("f" + i.ToString(), i);
        }

        private void getContradictions()
        {
            foreach (var item in File.ReadAllLines(@"C:\Users\Alaattin\Source\Repos\IronicSA\IronicSA\IronicSA\data\contradictions.txt"))
            {
                contradictions.Add(item.Split(':')[0].ToLower());
            }
        }

        public void IndexingData()
        {
            foreach (string line in File.ReadAllLines(_dataPath))
            {
                if (line.Length > 0)
                {
                    var splitted = line.Split('\t');
                    if (splitted.Length > 2)
                    {
                        string[] terms = ds.CleanAndStemString(cleanHashTags(splitted[3])).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (terms.Length > 0)
                        {
                            values.Add(double.Parse(splitted[2].Replace('.', ',')));
                            docToTftable(terms, getOtherFeatures(splitted[3]));
                        }
                        else Console.WriteLine(splitted[3]);
                    }
                }
            }
        }

        private List<int> getOtherFeatures(string tweet)
        {
            List<int> result = new List<int>();
            result.Add(getNumberOfHashTags(tweet));
            result.Add(NegPosTogether(tweet));
            result.Add(InterjectionCount(tweet));
            result.Add(PunctCount(tweet));
            result.Add(EmoticonCount(tweet));
            result.Add(Contradiction(tweet));
            result.Add(SentiWordNetPos(tweet));
            result.Add(SentiWordNetNeg(tweet));
            return result;
        }

        private int SentiWordNetNeg(string tweet)
        {
            throw new NotImplementedException();
        }

        private int SentiWordNetPos(string tweet)
        {
            throw new NotImplementedException();
        }

        private int getNumberOfHashTags(string str)
        {
            int result = 0;
            List<string> HasTags = new List<string>();
            HasTags.Add("#sarcasm");
            HasTags.Add("#irony");
            HasTags.Add("#not");
            foreach (string item in str.ToLower().Split())
            {
                if (HasTags.Contains(item))
                    result++;
            }
            return result;
        }

        private int Contradiction(string tweet)
        {
            foreach (var item in contradictions)
            {
                if (tweet.Contains(' '+item+' '))
                    return 1;
            }
            return 0;
        }

        private int EmoticonCount(string tweet)
        {
            return 0;
        }

        private int PunctCount(string tweet)
        {
            return 0;
        }

        private int InterjectionCount(string tweet)
        {
            return 0;
        }

        private int NegPosTogether(string tweet)
        {
            return 0;
        }

        private void docToTftable(string[] terms, List<int> otherFeatures)
        {
            try
            {
                tftable.Add(new Dictionary<int, int>());
                int docIndex = tftable.Count - 1;
                for (int i = 0; i < otherFeatures.Count; i++)
                {
                    tftable[docIndex].Add(i, otherFeatures[i]);
                }
                int index = 0;
                foreach (var t in terms)
                {
                    index = checkVocabulary(t);
                    if (tftable[docIndex].ContainsKey(index))
                        tftable[docIndex][index]++;
                    else
                        tftable[docIndex].Add(index, 1);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private int checkVocabulary(string term)
        {
            int currentIndex = 0;
            if (!vocabulary.ContainsKey(term))
            {
                vocabulary.Add(term, dtIndex);
                currentIndex = dtIndex;
                dtIndex++;
            }
            else
                currentIndex = vocabulary[term];

            return currentIndex;
        }



        private string cleanHashTags(string str)
        {
            string newstr = "";
            if (str.Contains('#'))
            {
                foreach (string item in str.Split())
                {
                    if (item.Length > 0)
                        newstr += (item[0] != '#' ? item : "") + " ";
                }
            }
            return newstr;
        }

        public string getMatrixesFull(bool classification)
        {
            string folder = Path.GetDirectoryName(_dataPath) + "\\Full\\";
            string dataset = folder + "dataset.txt";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            if (File.Exists(dataset))
                File.Delete(dataset);

            List<string> T = new List<string>();

            for (int i = 0; i < tftable.Count; i++)
            {
                T.Add(getLine(i, classification));
            }

            File.AppendAllLines(dataset, T);

            return dataset;
        }

        public string getMatrixes(int foldCount, bool classification)
        {
            string folder = Path.GetDirectoryName(_dataPath) + '\\' + foldCount.ToString() + "Folds\\";
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
            Directory.CreateDirectory(folder);
            int foldhas = (int)(tftable.Count / foldCount);

            List<List<string>> T = new List<List<string>>();

            for (int i = 0; i < foldCount; i++)
            {
                List<string> t = new List<string>();
                for (int j = 0; j < foldhas; j++)
                {
                    t.Add(getLine((i * foldhas) + j, classification));
                }
                T.Add(t);
            }

            for (int i = 0; i < foldCount; i++)
            {
                for (int j = 0; j < foldCount; j++)
                {
                    if (i == j)
                        File.AppendAllLines(folder + "Test" + i.ToString() + ".txt", T[j]);
                    else
                        File.AppendAllLines(folder + "Train" + i.ToString() + ".txt", T[j]);
                }
            }
            return folder;
        }

        private string getLine(int order, bool classification)
        {
            double value;
            if (classification)
                value = (values[order] > 0 ? 1 : 0);
            else
                value = values[order];
            string row = value.ToString(CultureInfo.CreateSpecificCulture("en-GB")) + '\t';
            foreach (int key in tftable[order].Keys)
            {
                row += key.ToString() + ':' + tftable[order][key].ToString() + '\t';
            }
            return row;
        }
    }
}
