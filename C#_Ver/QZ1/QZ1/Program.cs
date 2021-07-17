using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;

namespace QZ1
{
    public class DataOutput
    {
        public int Counter { get; set; }
        public double Good_value { get; set; }
    }
    class Program
    {
        static public string[] generatePopulation(int n_gene, int n_chro)
        {
            string[] chro = new string[n_chro];
            for (int i = 0; i < n_chro; i++)
            {
                Random num = new Random();
                int ran = num.Next();
                System.Threading.Thread.Sleep(2);
                string[] gene = new string[n_gene];
                for (int j = 0; j < n_gene; j++)
                {
                    gene[j] = Convert.ToString(ran % 2);
                    ran = ran / 2;
                }
                chro[i] = String.Join(null, gene);
            }
            return chro;
        }
        static public string decoder(string chro)
        {
            //integer-[1-10]
            string[] int_part = new string[10];
            for (int i = 1; i < 11; i++)
            {
                int_part[i - 1] = Convert.ToString(chro[i]);
            }
            string int_part_joined = String.Join("", int_part);
            int int_p = Convert.ToInt32(int_part_joined, 2);
            //decimal-[11-20]
            string[] dec_part = new string[10];
            for (int i = 0; i < 10; i++)
            {
                dec_part[i] = Convert.ToString(chro[11 + i]);
            }
            double dec = 0;
            for (int i = 0; i < 10; i++)
            {
                dec = dec + Convert.ToDouble(dec_part[i]) * Math.Pow(2, -i - 1);
            }
            //merge
            double encoded = int_p + dec;
            //sign
            if (Convert.ToString(chro[0]) == "0")
            {
                return "-" + encoded;
            }
            else
            {
                return "" + encoded;
            }
        }
        static public double scaling(string value, int upper, int lower)
        {
            double m = upper - lower;
            double n = Convert.ToDouble(value) - 1023.9990234375;
            double new_value = m * (n / (2 * (1023.9990234375))) + upper;
            return new_value;
        }
        static public double[] fitness(double[] y)
        {
            double a = 0;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] >= 0) y[i] = 0;
            }
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] == 0)
                {
                }
                else if (y[i] < 0)
                {
                    a = a + y[i];
                }
            }
            double[] b = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] == 0)
                {
                    b[i] = 0;
                }
                else
                {
                    b[i] = y[i] / a;
                }
            }
            return b;
        }
        static public double Function(double x1, double x2)
        {
            double a = 4 - 2.1 * x1,
                b = Math.Pow(x1, 4) / 3,
                c = (a + b) * x1 * x1,
                d = x1 * x2,
                e = 4 * x2 * x2 - 4,
                f = e * x2 * x2;
            double ans = c + d + f;
            return ans;
        }
        static public string mutation(string[] a, double[] fitness)
        {
            bool[] b = new bool[21];
            string d;
            int[] c = new int[21];
            Random num = new Random();
            int locationChro = location(fitness)[0];
            for (int j = 0; j < 21; j++)
            {
                if (a[locationChro][j] == Convert.ToChar("0"))
                {
                    b[j] = false;
                }
                else if (a[locationChro][j] == Convert.ToChar("1"))
                {
                    b[j] = true;
                }
            }
            int k = num.Next(0, 21);
            if (b[k] == true) b[k] = false;
            else if (b[k] == false) b[k] = true;
            for (int i = 0; i < b.Length; i++)
            {
                c[i] = Convert.ToInt32(b[i]);
            }
            d = string.Join("", c);
            return d;
        }
        static public string crossover(string[] a, double[] fitness)
        {
            int[] locations = location(fitness);
            int locationChro = locations[0],
                locationChro2 = locations[1];
            string a1 = a[locationChro],
                a2 = a[locationChro2];
            bool[] a1_b = new bool[21];
            Random num = new Random();
            int ran = num.Next(4, 10);
            for (int i = 0; i < ran; i++)
            {
                if (a1[i] != 48)
                {
                    a1_b[i] = true;
                }
                else if (a1[i] != 49)
                {
                    a1_b[i] = false;
                }
            }
            for (int i = ran; i < a1_b.Length; i++)
            {
                if (a2[i] != 48)
                {
                    a1_b[i] = true;
                }
                else if (a2[i] != 49)
                {
                    a1_b[i] = false;
                }
            }
            string afterCross;
            int[] c1 = new int[21];
            for (int i = 0; i < c1.Length; i++)
            {
                c1[i] = Convert.ToInt32(a1_b[i]);
            }
            afterCross = string.Join("", c1);
            return afterCross;
        }
        static public string reproduction(string[] a, double[] fitness)
        {
            int locationChro = location(fitness)[0];
            return a[locationChro];
        }
        static public int[] location(double[] fitness)
        {
            Random num = new Random();
            double[] range = new double[fitness.Length + 1];
            range[0] = 0;
            for (int i = 0; i < fitness.Length; i++)
            {
                range[i + 1] = range[i] + fitness[i];
            }
            for (int i = 0; i < fitness.Length; i++)
            {
                range[i] = range[i] * 100;
            }
            int[] locations = new int[2];
            double ran = num.Next(1, 101);
            for (int i = 0; i < fitness.Length; i++)
            {
                if (ran > range[i] & ran <= range[i + 1])
                {
                    locations[0] = i;
                    break;
                }
            }
            double ran2 = num.Next(1, 101);
            for (int i = 0; i < fitness.Length; i++)
            {
                if (ran2 > range[i] & ran2 <= range[i + 1])
                {
                    locations[1] = i;
                    break;
                }
            }
            if (locations[0] == locations[1])
            {
                locations[1]++;
            }
            return locations;
        }

        static double max_value, X1, X2, Y1;
        static int max_index;
        static string X1s, X2s;
        static int counter;
        static double[] good_value = new double[3001];
        static void Main(string[] args)
        {
            int n_chro = 200;
            string[] newPopulation = generatePopulation(21, n_chro);
            string[] newPopulation2 = generatePopulation(21, n_chro);
            double[] y1 = new double[n_chro],
                x1 = new double[n_chro],
                x2 = new double[n_chro];
            double[] y_fNew;
            string[,] Next;
            for (int i = 0; i < n_chro; i++)
            {
                x1[i] = scaling(decoder(newPopulation[i]), -3, 3);
                x2[i] = scaling(decoder(newPopulation2[i]), -2, 2);
                y1[i] = Function(x1[i], x2[i]);
            }
            y_fNew = fitness(y1);
            max_value = y_fNew.Max();
            max_index = y_fNew.ToList().IndexOf(max_value);
            X1s = newPopulation[max_index];
            X2s = newPopulation2[max_index];
            X1 = scaling(decoder(newPopulation[max_index]), -3, 3);
            X2 = scaling(decoder(newPopulation2[max_index]), -2, 2);
            Y1 = Function(X1, X2);
            counter = 0;
            good_value[counter] = Y1;
            Console.WriteLine(X1 + " " + X2 + " " + Y1);
            for (int j = 0; j < 3000; j++)
            {
                Next = NextPopulation(newPopulation, newPopulation2, y_fNew);
                for (int i = 0; i < n_chro; i++)
                {
                    newPopulation[i] = Next[0, i];
                }
                for (int i = 0; i < n_chro; i++)
                {
                    newPopulation2[i] = Next[1, i];
                }
                for (int i = 0; i < n_chro; i++)
                {
                    x1[i] = scaling(decoder(newPopulation[i]), -3, 3);
                    x2[i] = scaling(decoder(newPopulation2[i]), -2, 2);
                    y1[i] = Function(x1[i], x2[i]);
                }
                double[] test_fitness = fitness(y1);
                double min_value = test_fitness.Min();
                int min_index = test_fitness.ToList().IndexOf(min_value);
                newPopulation[min_index] = X1s;
                newPopulation2[min_index] = X2s;
                for (int i = 0; i < n_chro; i++)
                {
                    x1[i] = scaling(decoder(newPopulation[i]), -3, 3);
                    x2[i] = scaling(decoder(newPopulation2[i]), -2, 2);
                    y1[i] = Function(x1[i], x2[i]);
                }
                y_fNew = fitness(y1);
                max_value = y_fNew.Max();
                max_index = y_fNew.ToList().IndexOf(max_value);
                X1s = newPopulation[max_index];
                X2s = newPopulation2[max_index];
                X1 = scaling(decoder(newPopulation[max_index]), -3, 3);
                X2 = scaling(decoder(newPopulation2[max_index]), -2, 2);
                Y1 = Function(X1, X2);
                counter++;
                good_value[counter] = Y1;
                Console.WriteLine(X1 + " " + X2 + " " + Y1);
            }
            for (int i = 0; i < 3001; i++)
            {
                var records = new List<DataOutput> { new DataOutput { Counter = i, Good_value = good_value[i] }, };
                bool append = true;
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                config.HasHeaderRecord = !append;
                using (var writer = new StreamWriter("C:/Users/quack/Desktop/QZ1Function.csv", append))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(records);
                }
            }
            Console.Read();
        }
        static public string[,] NextPopulation(string[] x1, string[] x2, double[] fitness)
        {
            Random num = new Random();
            string[] NewX1 = new string[x1.Length],
                NewX2 = new string[x2.Length];
            for (int i = 0; i < x1.Length; i++)
            {
                int prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 400)
                {
                    NewX1[i] = mutation(x1, fitness);
                }
                else if (prop > 400 & prop <= 800)
                {
                    NewX1[i] = crossover(x1, fitness);
                }
                else
                {
                    NewX1[i] = reproduction(x1, fitness);
                }
                prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 300)
                {
                    NewX2[i] = mutation(x2, fitness);
                }
                else if (prop > 300 & prop <= 850)
                {
                    NewX2[i] = crossover(x2, fitness);
                }
                else
                {
                    NewX2[i] = reproduction(x2, fitness);
                }
            }
            string[,] NextPop = new string[2, x1.Length];
            for (int i = 0; i < x1.Length; i++)
            {
                NextPop[0, i] = NewX1[i];
                NextPop[1, i] = NewX2[i];
            }
            return NextPop;
        }
    }
}
