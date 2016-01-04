using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IronicSA
{
    class dataset
    {
        //minimum term/word length
        int _minTermLength;

        //language of dataset for lower function
        CultureInfo lanculture = CultureInfo.GetCultureInfoByIetfLanguageTag("en");

        //stemmer definition
        Stemmer porter;

        //path of dataset
        string _path;

        public string[] articles;

        //trash strings to clear
        string[] trash = { " i.e.", " e.g.", "<article>", "/", "|", "...", ":", "=", "-", "_", "&quot" };

        //stopwords 
        List<string> stopWords = new List<string>() { "a", "able", "about", "across", "after", "all", "almost", "also", "am", "among", "an", "and", "any", "are", "as", "at", "be",
            "because", "been", "but", "by", "can", "cannot", "could", "dear", "did", "do", "does", "either", "else", "ever", "every", "for", "from", "get", "got", "had", "has", 
            "have", "he", "her", "hers", "him", "his", "how", "however", "i", "if", "in", "into", "is", "it", "its", "just", "least", "let", "like", "likely", "may", "me", "might",
            "most", "must", "my", "neither", "no", "nor", "not", "of", "off", "often", "on", "only", "or", "other", "our", "own", "rather", "said", "say", "says", "she", "should", 
            "since", "so", "some", "than", "that", "the", "their", "them", "then", "there", "these", "they", "this", "tis", "to", "too", "twas", "us", "wants", "was", "we", "were", 
            "what", "when", "where", "which", "while", "who", "whom", "why", "will", "with", "would", "yet", "you", "your", "s" };

        /// <summary>
        /// Dataset constructor
        /// </summary>
        /// <param name="path">path of dataset file</param>
        /// <param name="minTermLength">Minimum term/word length</param>
        public dataset(string path, int minTermLength = 3)
        {
            _path = path;
            _minTermLength = minTermLength;
            porter = new Stemmer();
        }

        public dataset(int minTermLength = 3)
        { 
            _minTermLength = minTermLength;
            porter = new Stemmer();
        }

        /// <summary>
        /// Cleans and stems the articles
        /// </summary>
        public void cleanAndStem()
        {
            read();
            Console.WriteLine(DateTime.Now);
            clean();
            Console.WriteLine(DateTime.Now);
            stem();
            Console.WriteLine(DateTime.Now);
        }

        public string CleanAndStemString(string Adirty)
        {
            return strStem(strClean(Adirty.ToLower()));
        }

        /// <summary>
        /// stems the dataset
        /// </summary>
        /// <param name="separator"></param>
        private void read(string separator = "</article>")
        {
            if (File.Exists(_path))
                articles = File.ReadAllText(_path).Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// stems the dataset
        /// </summary>
        private void stem()
        {
            for (int i = 0; i < articles.Length; i++)
            {
                articles[i] = strStem(articles[i]);
            }
        }

        /// <summary>
        /// cleans the dataset
        /// </summary>
        public void clean()
        {
            for (int i = 0; i < articles.Length; i++)
            {
                articles[i] = strClean(articles[i]);
            }
        }

        /// <summary>
        /// Stems the line 
        /// </summary>
        /// <param name="line">line: separates with blanks</param>
        /// <returns>stemmed line</returns>
        private string strStem(string line)
        {
            string result = "";
            foreach (var item in line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!isStopWord(item))
                    if (item.Length >= _minTermLength)
                        result += porter.steming(item) + " ";
            }
            return result.Trim().ToLower(lanculture);
        }

        /// <summary>
        /// Checks the stopwords list for input
        /// </summary>
        /// <param name="input">item to the control</param>
        /// <returns>returns true if the input in the stopwords list</returns>
        private bool isStopWord(string input)
        {
            return stopWords.Contains(input.Trim().ToLower(lanculture));
        }

        /// <summary>
        /// Cleans the given string 
        /// </summary>
        /// <param name="str">string to clean</param>
        /// <returns>cleaned string</returns>
        private string strClean(string str)
        {
            //clean new lines
            str = str.Replace('\n', ' ');

            //clean urls
            int urlIndex = 0;
            do
            {
                urlIndex = str.IndexOf("url=");
                if (urlIndex > -1)
                {
                    int firstspace = str.IndexOf(' ', urlIndex);
                    if (firstspace > -1)
                        str = str.Substring(0, urlIndex) + str.Substring(firstspace, str.Length - firstspace);
                    else
                        str = str.Substring(0, urlIndex);
                }
            }
            while (urlIndex > -1);

            //clean trashs
            foreach (string item in trash)
                str = str.Replace(item, " ");

            //clean non alphabetic chars
            Regex rgx = new Regex("[^a-zA-Z ]");
            str = rgx.Replace(str, "");

            //return result
            return str;
        }


    }
}
