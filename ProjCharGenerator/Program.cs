using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace generator
{    
    class CharGenerator
    {
        private string syms = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя";
        private char[] data;
        private int size;
        private Random random = new Random();
        public CharGenerator()
        {
            size = syms.Length;
            data = syms.ToCharArray();
        }
        public char getsym()
        {
            return data[random.Next(0, size)];
        }
    }    
    class Program
    {
        static void Main(string[] args)
        {
            BigramGenerator bigeneric = new BigramGenerator(@"..\..\..\BasisBigram.txt");
            FrequencyGeneryc fgen1 = new FrequencyGeneryc(@"..\..\..\Frequency1word.txt");
            FrequencyGeneryc fgen2 = new FrequencyGeneryc(@"..\..\..\Frequency2word.txt");

            File.WriteAllText(@"..\..\..\BiOutput.txt", bigeneric.Next(1000));
            File.WriteAllText(@"..\..\..\OneWordOutput.txt", fgen1.Next(1000));
            File.WriteAllText(@"..\..\..\TwoWordOutput.txt", fgen2.Next(1000));
        }
    }

    class FrequencyGeneryc
    {
        public FrequencyGeneryc(String filePath)
        {
            WordBase = new Dictionary<string, int>();
            StreamReader sr = File.OpenText(filePath);
            String str;
            List<List<String>> lines = new List<List<string>>();
            while ((str = sr.ReadLine()) != null)
            {
                lines.Add(new List<string>());
                lines[lines.Count - 1] = GeneratorClass.utilits.readLine(str);
            }
            str = "";
            foreach (var line in lines)
            {
                str += line[0];
                for (int i = 1; i < line.Count() - 1; i++)
                {
                    str += " " + line[i];
                }
                WordBase[str] = Int32.Parse(line[line.Count() - 1]);
                str = "";
            }
        }
        public FrequencyGeneryc(Dictionary<String, int> WordTable)
        {
            WordBase = WordTable;
        }
        public string Next(int size)
        {
            String returnString = "";
            int chanceSum = 0;
            int[] mas;
            long chance;
            for (int i = 0; i < size; i++)
            {
                mas = new int[WordBase.Values.Count];
                WordBase.Values.CopyTo(mas, 0);
                chance = rand.Next(0, mas.Sum());
                foreach (var pair in WordBase)
                {
                    if (pair.Value + chanceSum >= chance)
                    {
                        returnString += pair.Key + " ";
                        chanceSum = 0;
                        break;
                    }
                    else
                    {
                        chanceSum += pair.Value;
                    }
                }
            }
            return returnString;
        }
        public void printBase()
        {
            foreach (var pair in WordBase)
            {
                Console.WriteLine(pair.Key + ": " + pair.Value.ToString());
            }
        }
        private Random rand = new Random();
        private Dictionary<String, int> WordBase;
    }
    class BigramGenerator
    {
        public BigramGenerator(String filePath)
        {
            WordBase = new Dictionary<char, Dictionary<char, int>>();
            StreamReader sr = File.OpenText(filePath);
            String str;
            List<List<String>> lines = new List<List<string>>();
            while ((str = sr.ReadLine()) != null)
            {
                lines.Add(new List<string>());
                lines[lines.Count - 1] = GeneratorClass.utilits.readLine(str);
            }
            syms = new char[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                syms[i] = lines[i][0][0];
            }
            for (int i = 0; i < syms.Length; i++)
            {
                WordBase[syms[i]] = new Dictionary<char, int>();
                for (int j = 1; j < lines[i].Count; j++)
                {
                    WordBase[syms[i]][syms[j - 1]] = Int32.Parse(lines[i][j]);
                }
            }
        }
        public BigramGenerator(Dictionary<char, Dictionary<char, int>> WordTable)
        {
            WordBase = WordTable;
            syms = new char[WordBase.Keys.Count];
            WordBase.Keys.CopyTo(syms, 0);
        }
        public String Next(int size)
        {
            String returnString = "";

            int nextPos = rand.Next(0, syms.Length);
            char nextsym = syms[nextPos];
            int chanceSum = 0;
            int[] mass;
            int chance;

            returnString += nextsym;
            for (int i = 0; i < size - 1; i++)
            {
                mass = new int[WordBase[nextsym].Values.Count];
                WordBase[nextsym].Values.CopyTo(mass, 0);
                chance = rand.Next(0, mass.Sum());
                foreach (var pair in WordBase[nextsym])
                {
                    if (pair.Value + chanceSum >= chance)
                    {
                        nextsym = pair.Key;
                        returnString += nextsym;
                        chanceSum = 0;
                        break;
                    }
                    else
                    {
                        chanceSum += pair.Value;
                    }
                }
            }
            return returnString;
        }
        public void PrintBase()
        {
            foreach (var key in WordBase)
            {
                Console.Write(key.Key.ToString() + ": { ");
                foreach (var value in key.Value)
                {
                    Console.Write(value.Value.ToString() + " ");
                }
                Console.WriteLine("} ");
            }
        }
        private char[] syms;
        private Random rand = new Random();
        private Dictionary<char, Dictionary<char, int>> WordBase;
    }
}
namespace GeneratorClass
{
    abstract class utilits
    {
        public static List<String> readLine(String line)
        {
            List<String> result = new List<String>();
            String word = "";
            foreach (char c in line)
            {
                if (c == ' ' || c == '\t')
                {
                    result.Add(word);
                    word = "";
                }
                else
                {
                    word += c;
                }
            }
            if (word.Length != 0)
            {
                result.Add(word);
            }
            return result;
        }
    }
}
