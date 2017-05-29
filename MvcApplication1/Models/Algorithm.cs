using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.Models
{
    class Algorithm
    {
        List<DataPoint> rawDataToCluster = new List<DataPoint>();
        List<DataPoint> normalizedDataToCluster = new List<DataPoint>();
        List<DataPoint> clusters = new List<DataPoint>();
        private int numberOfClusters = 0;
        private int N = 0;

        public string[] Init(string currentPerson, List<DataPoint> points)
        {
            InitilizeRawData(points);
            
            for (int i = 0; i < numberOfClusters; i++)
            {
                clusters.Add(new DataPoint(N) { Cluster = i });
            }
            //System.Console.WriteLine("Data BEFORE normalizing-------------------");
            ShowData(rawDataToCluster);
            NormalizeData();

            //System.Console.WriteLine("Data AFTER normalizing-------------------");
            ShowData(normalizedDataToCluster);
            Cluster(normalizedDataToCluster, numberOfClusters);
            StringBuilder sb = new StringBuilder();
            var group = rawDataToCluster.GroupBy(s => s.Cluster).OrderBy(s => s.Key);
            foreach (var g in group)
            {
                sb.AppendLine("Cluster # " + g.Key + ":");
                foreach (var value in g)
                {

                    sb.Append(value.pointId + " | " + value.ToString());
                    sb.AppendLine();
                }
                sb.AppendLine("------------------------------");
            }
            //System.Console.WriteLine(sb);

            string pointIdForFinding;
            
            pointIdForFinding = currentPerson;
            //do
            //{
                //sb.Clear();
                //do
                //{
                //    //System.Console.Write("\nEnter point id for which you want to find simular points: ");

                //    ConsoleKeyInfo key;
                //    while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter) // пока не нажали Enter
                //    {
                //        char c = key.KeyChar;
                //        if ((Char.IsNumber(key.KeyChar)))
                //        {
                //            Console.Write(c);
                //            sb.Append(c);

                //        }
                //    }
                //    pointIdForFinding = Int32.Parse(sb.ToString());
                //} while (pointIdForFinding <= 0);
                sb.Clear();
                int clusterIdForFinding = -1;
                foreach (var g in group)
                {
                    foreach (var value in g)
                    {
                        if (value.pointId == pointIdForFinding)
                        {
                            clusterIdForFinding = g.Key;
                        }
                    }
                }
                string[] str = new string[100];
                int index = 0 ;
                if (clusterIdForFinding != -1)
                {
                    var simular = rawDataToCluster.FindAll(s => s.Cluster == clusterIdForFinding).OrderBy(s => s.pointId);
                    sb.AppendLine("\nCluster # " + clusterIdForFinding + ":");
                    foreach (var value in simular)
                    {
                        str[index] = value.pointId;
                        sb.Append(value.pointId + " | " + value.ToString());
                        sb.AppendLine();
                        sb.AppendLine("------------------------------");
                        index++;
                    }
                    //System.Console.WriteLine(sb);
                }
                //else System.Console.WriteLine("\nNot found.");
            //} while (true);
                return str;
        }

        private void InitilizeRawData(List<DataPoint> points)
        {
            if (rawDataToCluster.Count == 0)
            {
                StringBuilder str = new StringBuilder();
                int numberOfPoints;

                //do
                //{
                //    System.Console.Write("\nEnter N - space dimension [more than zero]: ");

                //    ConsoleKeyInfo key;
                //    while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter) // пока не нажали Enter
                //    {
                //        char c = key.KeyChar;
                //        if ((Char.IsNumber(key.KeyChar)))
                //        {
                //            Console.Write(c);
                //            str.Append(c);

                //        }
                //    }
                //    N = Int32.Parse(str.ToString());
                //} while (N <= 0);

                //str.Clear();
                //do
                //{
                //    System.Console.Write("\nEnter number of N-dimensional points [more than zero]: ");

                //    ConsoleKeyInfo key;
                //    while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter) // пока не нажали Enter
                //    {
                //        char c = key.KeyChar;
                //        if ((Char.IsNumber(key.KeyChar)))
                //        {
                //            Console.Write(c);
                //            str.Append(c);
                //        }
                //    }
                //    numberOfPoints = Int32.Parse(str.ToString());
                //} while (numberOfPoints <= 0);

                //str.Clear();
                //do
                //{
                //    System.Console.Write("\nEnter a desired number of clusters [must be 0< and <{0}]: ", numberOfPoints);

                //    ConsoleKeyInfo key;
                //    while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter) // пока не нажали Enter
                //    {
                //        char c = key.KeyChar;
                //        if ((Char.IsNumber(key.KeyChar)))
                //        {
                //            Console.Write(c);
                //            str.Append(c);

                //        }
                //    }
                //    numberOfClusters = Int32.Parse(str.ToString());
                //} while (numberOfClusters > numberOfPoints || numberOfClusters <= 0);
                N = 2;
                numberOfPoints = 4;
                numberOfClusters = 2;
                rawDataToCluster = points;
                //for (int i = 0; i < numberOfPoints; i++)
                //{
                    
                //    DataPoint dp = new DataPoint(N);
                //    dp.pointId = ;
                //    rawDataToCluster.Add(dp);
                //}
            }
        }

        private void ShowData(List<DataPoint> data)
        {
            string str = "";
            foreach (DataPoint dp in data)
            {
                str += dp.ToString() + Environment.NewLine;
            }
            System.Console.WriteLine(str);
        }


        private void NormalizeData()
        {
            double[] max = new double[N];
            for (int i = 0; i < max.Length; i++)
            {
                max[i] = 0.0;
            }
            double[] min = new double[N];
            for (int i = 0; i < min.Length; i++)
            {
                min[i] = Double.PositiveInfinity;
            }

            double[] aNew = new double[N];
            for (int i = 0; i < aNew.Length; i++)
            {
                aNew[i] = 0.0;
            }

            foreach (DataPoint dataPoint in rawDataToCluster)
            {
                for (int i = 0; i < dataPoint.a.Length; i++)
                {
                    if (dataPoint.a[i] > max[i])
                        max[i] = dataPoint.a[i];
                    if (dataPoint.a[i] < min[i])
                        min[i] = dataPoint.a[i];
                }
            }

            foreach (DataPoint dataPoint in rawDataToCluster)
            {
                for (int i = 0; i < aNew.Length; i++)
                {
                    aNew[i] = (dataPoint.a[i] - min[i]) / (max[i] - min[i]);
                }
                DataPoint dp = new DataPoint(N);
                for (int i = 0; i < N; i++)
                {
                    dp.a[i] = aNew[i];
                }

                normalizedDataToCluster.Add(dp);
            }
        }

        public void Cluster(List<DataPoint> data, int numClusters)
        {
            bool changed = true;
            bool success = true;
            //визначаємо рандомно ценрт кожного класу
            InitializeCentroids();

            int maxIteration = data.Count * 10;
            //номер ітерацій (поріг н повинен перевищувати максимума)
            int threshold = 0;
            //поки жоден кластер не пустий і відбуваються переміщення даних з кластеру до кластеру
            while (success == true && changed == true && threshold < maxIteration)
            {
                ++threshold;
                success = UpdateDataPointMeans();
                changed = UpdateClusterMembership();
            }
        }

        private void InitializeCentroids()
        {
            Random random = new Random(numberOfClusters);
            for (int i = 0; i < numberOfClusters; ++i)//кожен кластер повинен мати хоч один член (точку, вектор)
            {
                normalizedDataToCluster[i].Cluster = rawDataToCluster[i].Cluster = i;
            }
            for (int i = numberOfClusters; i < normalizedDataToCluster.Count; i++)//дал робимо рандомно
            {
                normalizedDataToCluster[i].Cluster = rawDataToCluster[i].Cluster = random.Next(0, numberOfClusters);
            }
        }

        private bool UpdateDataPointMeans()
        {
            if (EmptyCluster(normalizedDataToCluster)) return false;

            //групуємо за номером кластеру
            var groupToComputeMeans = normalizedDataToCluster.GroupBy(s => s.Cluster).OrderBy(s => s.Key);
            int clusterIndex = 0;
            double[] a = new double[N];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 0.0;
            }


            foreach (var item in groupToComputeMeans)
            {
                foreach (var value in item)
                {
                    for (int i = 0; i < N; i++)
                    {
                        a[i] += value.a[i];
                    }
                }
                for (int i = 0; i < N; i++)
                {
                    clusters[clusterIndex].a[i] = a[i] / item.Count();
                }

                clusterIndex++;
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = 0.0;
                }
            }
            return true;
        }

        private bool EmptyCluster(List<DataPoint> data)
        {
            var emptyCluster =
            data.GroupBy(s => s.Cluster).OrderBy(s => s.Key).Select(g => new { Cluster = g.Key, Count = g.Count() });

            foreach (var item in emptyCluster)
            {
                if (item.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private double EuclideanDistance(DataPoint dataPoint, DataPoint mean)
        {
            double diffs = 0.0;
            diffs = Math.Pow(dataPoint.a[0] - mean.a[0], 2);
            for (int i = 1; i < N; i++)
            {
                diffs += Math.Pow(dataPoint.a[i] - mean.a[i], 2);
            }
            return Math.Sqrt(diffs);
        }

        private bool UpdateClusterMembership()
        {
            bool changed = false;

            double[] distances = new double[numberOfClusters];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < normalizedDataToCluster.Count; ++i)
            {

                for (int k = 0; k < numberOfClusters; ++k)
                    distances[k] = EuclideanDistance(normalizedDataToCluster[i], clusters[k]);

                int newClusterId = MinIndex(distances);
                if (newClusterId != normalizedDataToCluster[i].Cluster)
                {
                    changed = true;
                    normalizedDataToCluster[i].Cluster = rawDataToCluster[i].Cluster = newClusterId;
                    string str = "";
                    for (int j = 0; j < N; j++)
                    {
                        str += "a" + j + 1 + ": " + rawDataToCluster[i].a[j] + ", ";
                    }
                    sb.AppendLine("Data Point >> " + str + " moved to Cluster # " + newClusterId);
                }
                else
                {
                    sb.AppendLine("No change.");
                }
                sb.AppendLine("------------------------------");
                //System.Console.WriteLine(sb);
                //txtIterations.Text += sb.ToString();
            }
            if (changed == false)
                return false;
            if (EmptyCluster(normalizedDataToCluster)) return false;
            return true;
        }

        private int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
    }
}
