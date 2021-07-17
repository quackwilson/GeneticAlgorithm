using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace P3_GammaDensityFunction
{
    class Program
    {
        //Generating Populations
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
            string[] int_part = new string[10];
            for (int i = 1; i < 11; i++)
            {
                int_part[i - 1] = Convert.ToString(chro[i]);
            }
            string int_part_joined = String.Join("", int_part);
            int int_p = Convert.ToInt32(int_part_joined, 2);
            string[] dec_part = new string[20];
            for (int i = 0; i < 20; i++)
            {
                dec_part[i] = Convert.ToString(chro[11 + i]);
            }
            double dec = 0;
            for (int i = 0; i < 20; i++)
            {
                dec = dec + Convert.ToDouble(dec_part[i]) * Math.Pow(2, -i - 1);
            }
            double encoded = int_p + dec;
            if (Convert.ToString(chro[0]) == "0")
            {
                return "-" + encoded;
            }
            else
            {
                return "" + encoded;
            }
        }
        static public double scaling(string value, double upper, double lower)
        {
            double m = upper - lower;
            double n = Convert.ToDouble(value) - 1023.99999904632568359375;
            double new_value = m * (n / (2 * (1023.99999904632568359375))) + upper;
            return new_value;
        }
        //Generating Next Populations
        static public string mutation(string[] a, double[] fitness)
        {
            bool[] b = new bool[31];
            string d;
            int[] c = new int[31];
            Random num = new Random();
            int locationChro = location(fitness)[0];
            for (int j = 0; j < 31; j++)
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
            int k = num.Next(1, 31);
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
            bool[] a1_b = new bool[31];
            Random num = new Random();
            int ran = num.Next(4, 17);
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
            int[] c1 = new int[31];
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
        static public string[,] NextPopulation(string[] x1, string[] x2, string[] x3, string[] x4, string[] x5, double[] fitness)
        {
            Random num = new Random();
            string[] NewX1 = new string[x1.Length],
                NewX2 = new string[x2.Length],
                NewX3 = new string[x3.Length],
                NewX4 = new string[x4.Length],
                NewX5 = new string[x5.Length];
            for (int i = 0; i < x1.Length; i++)
            {
                int prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 450)
                {
                    NewX1[i] = mutation(x1, fitness);
                }
                else if (prop > 450 & prop <= 850)
                {
                    NewX1[i] = crossover(x1, fitness);
                }
                else
                {
                    NewX1[i] = reproduction(x1, fitness);
                }
                prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 450)
                {
                    NewX2[i] = mutation(x2, fitness);
                }
                else if (prop > 450 & prop <= 850)
                {
                    NewX2[i] = crossover(x2, fitness);
                }
                else
                {
                    NewX2[i] = reproduction(x2, fitness);
                }
                prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 450)
                {
                    NewX3[i] = mutation(x3, fitness);
                }
                else if (prop > 450 & prop <= 850)
                {
                    NewX3[i] = crossover(x3, fitness);
                }
                else
                {
                    NewX3[i] = reproduction(x3, fitness);
                }
                prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 450)
                {
                    NewX4[i] = mutation(x4, fitness);
                }
                else if (prop > 450 & prop <= 850)
                {
                    NewX4[i] = crossover(x4, fitness);
                }
                else
                {
                    NewX4[i] = reproduction(x4, fitness);
                }
                prop = num.Next(1, 1001);
                if (prop > 0 & prop <= 450)
                {
                    NewX5[i] = mutation(x5, fitness);
                }
                else if (prop > 450 & prop <= 850)
                {
                    NewX5[i] = crossover(x5, fitness);
                }
                else
                {
                    NewX5[i] = reproduction(x5, fitness);
                }
            }
            string[,] NextPop = new string[5, x1.Length];
            for (int i = 0; i < x1.Length; i++)
            {
                NextPop[0, i] = NewX1[i];
                NextPop[1, i] = NewX2[i];
                NextPop[2, i] = NewX3[i];
                NextPop[3, i] = NewX4[i];
                NextPop[4, i] = NewX5[i];
            }
            return NextPop;
        }
        //Code depends on function or problem
        static public double[] Gamma(double alpha,double beta,double delta,double A,double B)
        {
            double[] y_value = new double[40];
            for (int i = 0; i < 40; i++)
            {
                if ((i + 1) < delta)
                {
                    y_value[i] = A * Math.Pow((i + 1), alpha) * Math.Exp(((-beta) * (i + 1)) / 10);
                }
                else
                {
                    y_value[i] = (A * Math.Pow((i + 1), alpha) * Math.Exp(((-beta) * (i + 1)) / 10)) + (B * Math.Pow(((i + 1) - delta), alpha) * Math.Exp(((-beta) * (i + 1 - delta)) / 10));
                }
            }
            return y_value;
        }
        static public double[] fitness(double[,] y_value,double[] y_actual)
        {
            //Calculate RMSE
            double[] RMSE = new double[30];
            double[] Operator = new double[40];
            for (int i = 0; i < 30; i++)
            {
                double c = 0;
                for (int j = 0; j < 40; j++)
                {
                    Operator[j] = (y_value[i, j] - y_actual[j]) * (y_value[i, j] - y_actual[j]);
                    c = c + Operator[j];
                }
                RMSE[i] = Math.Sqrt(c / 40);
            }
            //Calculate fitness by using RMSE
            double[] fitness = new double[30];
            double a = 0;
            for (int i = 0; i < 30; i++)
            {
                a = a + RMSE[i];
            }
            for (int i = 0; i < 30; i++)
            {
                fitness[i] = 1 - (RMSE[i] / a);
            }
            return fitness;
        }
        //Declare long-life variables
        static double max_value, X1, X2, X3, X4, X5;
        static double[] Y1;
        static int max_index;
        static string X1s, X2s, X3s, X4s, X5s;
        static void Main(string[] args)
        {
            //Read the data from CSV
            var reader = new StreamReader(File.OpenRead(@"..\..\Pulse_data.csv"));
            List<string> t = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                t.Add(values[0]);
            }
            string[] actual_data = new string[40];
            for (int i = 0; i < 40; i++)
            {
                actual_data[i] = t[i];
            }
            double[] actual_data_double = new double[40];
            for (int i = 0; i < 40; i++)
            {
                actual_data_double[i] = Convert.ToDouble(actual_data[i]);
            }
            //Start of the Algorithm
            int n_chro = 30;
            string[] newPopulation = generatePopulation(31, n_chro),
                newPopulation2 = generatePopulation(31, n_chro),
                newPopulation3 = generatePopulation(31, n_chro),
                newPopulation4 = generatePopulation(31, n_chro),
                newPopulation5 = generatePopulation(31, n_chro);
            double[] x1 = new double[n_chro],
                x2 = new double[n_chro],
                x3 = new double[n_chro],
                x4 = new double[n_chro],
                x5 = new double[n_chro];
            double[,] y1 = new double[n_chro, 40];
            double[] y_fNew;
            string[,] Next;
            for (int i = 0; i < n_chro; i++)
            {
                x1[i] = scaling(decoder(newPopulation[i]), 0, 5);
                x2[i] = scaling(decoder(newPopulation2[i]), 0, 5);
                x3[i] = scaling(decoder(newPopulation3[i]), 18, 20);
                x4[i] = scaling(decoder(newPopulation4[i]), 50, 150);
                x5[i] = scaling(decoder(newPopulation5[i]), 0, 50);
                for (int j = 0; j < 40; j++)
                {
                    y1[i,j] = Gamma(x1[i], x2[i], x3[i], x4[i], x5[i])[j];
                }
            }
            y_fNew = fitness(y1,actual_data_double);
            max_value = y_fNew.Max();
            max_index = y_fNew.ToList().IndexOf(max_value);
            X1s = newPopulation[max_index];
            X2s = newPopulation2[max_index];
            X3s = newPopulation3[max_index];
            X4s = newPopulation4[max_index];
            X5s = newPopulation5[max_index];
            X1 = scaling(decoder(newPopulation[max_index]), 0, 5);
            X2 = scaling(decoder(newPopulation2[max_index]), 0, 5);
            X3 = scaling(decoder(newPopulation3[max_index]), 18, 20);
            X4 = scaling(decoder(newPopulation4[max_index]), 50, 150);
            X5 = scaling(decoder(newPopulation5[max_index]), 0, 50);
            Y1 = Gamma(X1, X2, X3, X4, X5);
            for (int k = 0; k < 1000; k++)
            {
                Next = NextPopulation(newPopulation, newPopulation2, newPopulation3, newPopulation4, newPopulation5, y_fNew);
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
                    newPopulation3[i] = Next[2, i];
                }
                for (int i = 0; i < n_chro; i++)
                {
                    newPopulation4[i] = Next[3, i];
                }
                for (int i = 0; i < n_chro; i++)
                {
                    newPopulation5[i] = Next[4, i];
                }
                for (int i = 0; i < n_chro; i++)
                {
                    x1[i] = scaling(decoder(newPopulation[i]), 0, 5);
                    x2[i] = scaling(decoder(newPopulation2[i]), 0, 5);
                    x3[i] = scaling(decoder(newPopulation3[i]), 18, 20);
                    x4[i] = scaling(decoder(newPopulation4[i]), 50, 150);
                    x5[i] = scaling(decoder(newPopulation5[i]), 0, 50);
                    for (int j = 0; j < 40; j++)
                    {
                        y1[i, j] = Gamma(x1[i], x2[i], x3[i], x4[i], x5[i])[j];
                    }
                }
                double[] test_fitness = fitness(y1,actual_data_double);
                double min_value = test_fitness.Min();
                int min_index = test_fitness.ToList().IndexOf(min_value);
                newPopulation[min_index] = X1s;
                newPopulation2[min_index] = X2s;
                newPopulation3[min_index] = X3s;
                newPopulation4[min_index] = X4s;
                newPopulation5[min_index] = X5s;
                for (int i = 0; i < n_chro; i++)
                {
                    x1[i] = scaling(decoder(newPopulation[i]), 0, 5);
                    x2[i] = scaling(decoder(newPopulation2[i]), 0, 5);
                    x3[i] = scaling(decoder(newPopulation3[i]), 18, 20);
                    x4[i] = scaling(decoder(newPopulation4[i]), 50, 150);
                    x5[i] = scaling(decoder(newPopulation5[i]), 0, 50);
                    for (int j = 0; j < 40; j++)
                    {
                        y1[i, j] = Gamma(x1[i], x2[i], x3[i], x4[i], x5[i])[j];
                    }
                }
                y_fNew = fitness(y1,actual_data_double);
                max_value = y_fNew.Max();
                max_index = y_fNew.ToList().IndexOf(max_value);
                X1s = newPopulation[max_index];
                X2s = newPopulation2[max_index];
                X3s = newPopulation3[max_index];
                X4s = newPopulation4[max_index];
                X5s = newPopulation5[max_index];
                X1 = scaling(decoder(newPopulation[max_index]), 0, 5);
                X2 = scaling(decoder(newPopulation2[max_index]), 0, 5);
                X3 = scaling(decoder(newPopulation3[max_index]), 18, 20);
                X4 = scaling(decoder(newPopulation4[max_index]), 50, 150);
                X5 = scaling(decoder(newPopulation5[max_index]), 0, 50);
                Y1 = Gamma(X1, X2, X3, X4, X5);
            }
            double RMSE;
            double[] Operator = new double[40];
            double c = 0;
            for (int j = 0; j < 40; j++)
            {
                Operator[j] = (Y1[j] - actual_data_double[j]) * (Y1[j] - actual_data_double[j]);
                c = c + Operator[j];
            }
            RMSE = Math.Sqrt(c / 40);
            Console.WriteLine(X1 + " " + X2 + " " + X3 + " " + X4 + " " + X5 + " " + RMSE);
            Console.Read();
        }
    }
}
