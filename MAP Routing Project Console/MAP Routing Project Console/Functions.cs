using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MAP_Routing_Project_Console
{

    public class Class1
    {
        public void ReadMap(List<Vertices> verlist, List<Edges> edglist)
        {
            Vertices ver;
            Edges ed;
            FileStream fs = new FileStream("map8.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            int NumOfVer = int.Parse(sr.ReadLine());
            for (int i = 0; i < NumOfVer; i++)
            {
                ver = new Vertices();
                string line = sr.ReadLine();
                string[] data = line.Split(' ');
                ver.Vertices_Num = int.Parse(data[0]);
                ver.X_Axis = double.Parse(data[1]);
                ver.Y_Axis = double.Parse(data[2]);
                verlist.Add(ver);
            }
            int NumOfEdg = int.Parse(sr.ReadLine());
            for (int i = 0; i < NumOfEdg; i++)
            {
                ed = new Edges();
                string line = sr.ReadLine();
                string[] data = line.Split(' ');
                ed.Vertices_1 = int.Parse(data[0]);
                ed.Vertices_2 = int.Parse(data[1]);
                ed.Distance = double.Parse(data[2]);
                ed.Speed = double.Parse(data[3]);
                ed.time = (ed.Distance / ed.Speed) * 60;
                edglist.Add(ed);
            }
            sr.Close();
            fs.Close();
        }
        public void ReadQuery(List<Query> querylist)
        {

            FileStream fs = new FileStream("queries8.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            Query qer;

            int NumOfQer = int.Parse(sr.ReadLine());
            for (int i = 0; i < NumOfQer; i++)
            {
                qer = new Query();
                string line = sr.ReadLine();
                string[] data = line.Split(' ');
                qer.X_Source = double.Parse(data[0]);
                qer.Y_Source = double.Parse(data[1]);
                qer.X_Destnation = double.Parse(data[2]);
                qer.Y_Destnation = double.Parse(data[3]);
                qer.R = double.Parse(data[4]);
                querylist.Add(qer);
            }

        }
        public Dictionary<Vertices, List<Edges>> ConstructGraph(List<Vertices> VerList, List<Edges> EdgList, Dictionary<Vertices, List<Edges>> Graph)
        {
            ReadMap(VerList, EdgList);
            List<Edges> list;
            Graph = new Dictionary<Vertices, List<Edges>>();

            for (int i = 0; i < VerList.Count; i++)
            {
                list = new List<Edges>();
                Graph[VerList[i]] = list;
            }
            list = new List<Edges>();

            for (int i = 0; i < EdgList.Count; i++)
            {
               list = Graph.ElementAt(EdgList[i].Vertices_1).Value;
                list.Add(EdgList[i]);
                Graph[Graph.ElementAt(EdgList[i].Vertices_1).Key] = list;

               
                list = Graph.ElementAt(EdgList[i].Vertices_2).Value;
                list.Add(EdgList[i]);
                Graph[Graph.ElementAt(EdgList[i].Vertices_2).Key] = list;
            }
            return Graph;
        }

        public void CalculateShortestPath(List<Vertices> vertix, List<Query> query, Dictionary<Vertices, List<Edges>> graph)
        {

            ReadQuery(query);
            bool ExistS = false;
            bool ExistD = false;
            float z = 0;
            double minPathTime = 1 / z, minPathDes = 0 , Walking_Distance=0 , Vehicle_Distance = 0;
            double NewR, DistanceEquation_S=0, DistanceEquation_D=0, Time_S, Time_D;
            KeyValuePair<double, double> Min_TD = new KeyValuePair<double, double>();
           
            Vertices Source = new Vertices(), Destnation = new Vertices();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < query.Count; i++)
            {
                NewR = (query[i].R / 1000);
                for (int j = 0; j < vertix.Count; j++)
                {
                    if (vertix[j].X_Axis == query[i].X_Source && vertix[j].Y_Axis == query[i].Y_Source)
                    {
                        ExistS = true;
                        Source = vertix[j];
                        
                    }
                    if (vertix[j].X_Axis == query[i].X_Destnation && vertix[j].Y_Axis == query[i].Y_Destnation)
                    {
                        ExistD = true;
                        Destnation = vertix[j];
                       
                    }

                }
               
                if (ExistD == true)
                {
                    if (ExistS == false)
                    {
                        for (int j = 0; j < vertix.Count; j++)
                        {
                            DistanceEquation_S = (((query[i].X_Source - vertix[j].X_Axis) * (query[i].X_Source - vertix[j].X_Axis)) + ((query[i].Y_Source - vertix[j].Y_Axis) * (query[i].Y_Source - vertix[j].Y_Axis)));
                            DistanceEquation_S = Math.Sqrt(DistanceEquation_S);
                            if (DistanceEquation_S <= NewR)
                            {
                                Time_S = (DistanceEquation_S / 5) * 60;
                                Min_TD = Dijkstra(vertix[j], Destnation, graph);
                                if (minPathTime > Time_S + Min_TD.Key)
                                {
                                    Walking_Distance = DistanceEquation_S;
                                    Vehicle_Distance = Min_TD.Value;
                                    minPathTime = Time_S + Min_TD.Key;
                                    minPathDes = DistanceEquation_S + Min_TD.Value;
                                    
                                }

                            }
                        }
                        Console.WriteLine("Min time: ");
                        Console.WriteLine(Math.Round( minPathTime,2));
                        Console.WriteLine("Min des: ");
                        Console.WriteLine(Math.Round(minPathDes, 2));
                        Console.WriteLine("Vehicle: ");
                        Console.WriteLine(Math.Round(Vehicle_Distance, 2));
                        Console.WriteLine("No Walking distance");
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine("Excu: ");
                        Console.WriteLine(elapsedMs);
                        Console.WriteLine();

                    }
                    else
                    {
                        Min_TD = Dijkstra(Source, Destnation, graph);
                        Console.WriteLine("Min time: ");
                        Console.WriteLine(Math.Round(minPathTime, 2));
                        Console.WriteLine("Min des: ");
                        Console.WriteLine(Math.Round(minPathDes, 2));
                        Console.WriteLine("Vehicle: ");
                        Console.WriteLine(Math.Round(Vehicle_Distance, 2));
                        Console.WriteLine("No Walking distance");
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine("Excu: ");
                        Console.WriteLine(elapsedMs);
                        Console.WriteLine();

                    }
                }
                else
                {
                    if (ExistS == false)
                    {
                        for (int j = 0; j < vertix.Count; j++)
                        {
                            DistanceEquation_S = (((query[i].X_Source - vertix[j].X_Axis) * (query[i].X_Source - vertix[j].X_Axis)) + ((query[i].Y_Source - vertix[j].Y_Axis) * (query[i].Y_Source - vertix[j].Y_Axis)));
                            DistanceEquation_S = Math.Sqrt(DistanceEquation_S);
                            if (DistanceEquation_S <= NewR)
                            {
                                Time_S = (DistanceEquation_S / 5) * 60;
                                for (int K = 0; K < vertix.Count; K++)
                                {
                                    DistanceEquation_D = (((query[i].X_Destnation - vertix[K].X_Axis) * (query[i].X_Destnation - vertix[K].X_Axis)) + ((query[i].Y_Destnation - vertix[K].Y_Axis) * (query[i].Y_Destnation - vertix[K].Y_Axis)));
                                    DistanceEquation_D = Math.Sqrt(DistanceEquation_D);

                                    if (DistanceEquation_D <= NewR)
                                    {
                                        Time_D = (DistanceEquation_D / 5) * 60;
                                        Min_TD = Dijkstra(vertix[j], vertix[K], graph);
                                        if (minPathTime > Time_S + Min_TD.Key+Time_D)
                                        {
                                            Walking_Distance = DistanceEquation_S+DistanceEquation_D;
                                            Vehicle_Distance = Min_TD.Value;
                                            minPathTime = Time_S + Min_TD.Key + Time_D;
                                            minPathDes = DistanceEquation_S + Min_TD.Value + DistanceEquation_D;
                                        }

                                    }
                                }
                            }
                        }
                        Console.WriteLine("Min time: ");
                        Console.WriteLine(Math.Round(minPathTime, 2));
                        Console.WriteLine("Min des: ");
                        Console.WriteLine(Math.Round(minPathDes, 2));
                        Console.WriteLine("Vehicle: ");
                        Console.WriteLine(Math.Round(Vehicle_Distance, 2));
                        Console.WriteLine("Walking: ");
                        Console.WriteLine(Math.Round( Walking_Distance,2));
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine("Excu: ");
                        Console.WriteLine(elapsedMs);
                        Console.WriteLine();

                    }
                    else
                    {
                        if (ExistS == false)
                        {
                            for (int j = 0; j < vertix.Count; j++)
                            {
                                DistanceEquation_D = (((query[i].X_Destnation - vertix[j].X_Axis) * (query[i].X_Destnation - vertix[j].X_Axis)) + ((query[i].Y_Destnation - vertix[j].Y_Axis) * (query[i].Y_Destnation - vertix[j].Y_Axis)));
                                DistanceEquation_D = Math.Sqrt(DistanceEquation_D);

                                if (DistanceEquation_D <= NewR)
                                {
                                    Time_D = (DistanceEquation_D / 5) * 60;
                                    Min_TD = Dijkstra(Source, vertix[j], graph);
                                    if (minPathTime > Time_D + Min_TD.Key)
                                    {
                                        Walking_Distance = DistanceEquation_D;
                                        Vehicle_Distance = Min_TD.Value;
                                        minPathTime = Time_D + Min_TD.Key;
                                        minPathDes = DistanceEquation_D + Min_TD.Value;
                                    }

                                }
                            }
                            Console.WriteLine("Min time: ");
                            Console.WriteLine(Math.Round(minPathTime, 2));
                            Console.WriteLine("Min des: ");
                            Console.WriteLine(Math.Round(minPathDes, 2));
                            Console.WriteLine("Vehicle: ");
                            Console.WriteLine(Math.Round(Vehicle_Distance, 2));
                            Console.WriteLine("Walking: ");
                            Console.WriteLine(Math.Round(Walking_Distance, 2));
                            watch.Stop();
                            var elapsedMs = watch.ElapsedMilliseconds;
                            Console.WriteLine("Excu: ");
                            Console.WriteLine(elapsedMs);
                            Console.WriteLine();

                        }
                    }
                }
                minPathTime = 1 / z;
            }
        }
        public KeyValuePair<double,double> Dijkstra(Vertices source, Vertices destnation, Dictionary<Vertices, List<Edges>> Graph)
        {
            PriorityQueue pq = new PriorityQueue();
            KeyValuePair<double, double> Min_TD = new KeyValuePair<double, double>(0,0);
            List<Int32> vistedvertices = new List<Int32>(); //visited vertices
            List<KeyValuePair<Int32, double>> pqlist1 = new List<KeyValuePair<Int32, double>>();
            // vertices and time 
            List<KeyValuePair<Int32, double>> pqlist2 = new List<KeyValuePair<int, double>>();
            // parent of each vertix and distance
            float zero = 0; // infinity
                            //distance to source vertex is zero
            KeyValuePair<Int32, double> item;
            //set all other times to infinity 
            for (int i = 0; i < Graph.Count; i++)
            {
                if (i == source.Vertices_Num)
                {
                    item = new KeyValuePair<Int32, double>(source.Vertices_Num, 0);
                    pqlist1.Add(item);
                   
                }
                else
                {
                    item = new KeyValuePair<Int32, double>(Graph.ElementAt(i).Key.Vertices_Num, 1 / zero);
                    pqlist1.Add(item);
                    
                }
                item = new KeyValuePair<Int32, double>(0, 0);
                pqlist2.Add(item);

            }
            pq.Enqueue(pqlist1[source.Vertices_Num].Key, pqlist1[source.Vertices_Num].Value);
            // while Priority Queue isnot empty
            while (pq.data.Count != 0)
            {
                item = pq.Dequeue(); // get the min item with the min time 
                vistedvertices.Add(item.Key); // add this item to visited vertices

                if (item.Key != destnation.Vertices_Num)
                {
                    for (int j = 1; j <= Graph.Values.ElementAt(item.Key).Count; j++)
                    {
                        if (Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1 == item.Key && !vistedvertices.Contains(Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2))
                        {
                            if (pqlist1[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2].Value > (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time))
                            {
                                pqlist1[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2] = new KeyValuePair<Int32, double>(Graph.ElementAt(item.Key).Value.ElementAt(j - 1).Vertices_2, (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time));
                                pq.Enqueue(Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2, (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time));
                                pqlist2[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2] = new KeyValuePair<Int32, double>(item.Key, (pqlist2[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].Distance));

                            }
                        }
                        else if (Graph.Values.ElementAt(item.Key)[j - 1].Vertices_2 == item.Key && !vistedvertices.Contains(Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1))
                        {
                            if (pqlist1[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1].Value > (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time))
                            {
                                pqlist1[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1] = new KeyValuePair<Int32, double>(Graph.ElementAt(item.Key).Value.ElementAt(j - 1).Vertices_1, (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time));
                                pq.Enqueue(Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1, (pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time));
                                pqlist2[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1] = new KeyValuePair<Int32, double>(item.Key, (pqlist2[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].Distance));

                            }
                            //Console.WriteLine(pqlist1[Graph.Values.ElementAt(item.Key)[j - 1].Vertices_1].Value);
                            //Console.WriteLine(pqlist1[item.Key].Value);
                            //Console.WriteLine(Graph.Values.ElementAt(item.Key)[j - 1].time);
                            //Console.WriteLine(pqlist1[item.Key].Value + Graph.Values.ElementAt(item.Key)[j - 1].time);

                        }
                    }
                }
                else
                {
                    Min_TD = new KeyValuePair<double, double>(pqlist1[item.Key].Value, pqlist2[item.Key].Value);
                    return Min_TD;
                }

            }
            return Min_TD;
            
        }
    }
}
   /* public class PriorityQueue 
    {
        #region internal properties
        public int Capacity { get; set; }
        public int Size { get; set; }
        Tuple<Tuple<double, double>, Vertices> Node { get; set; }
        List<Tuple<double, Vertices>> Nodes { get; set; }
        #endregion
        #region constructors
        public PriorityQueue() 
        {
            Capacity = 200000;
            Size = 0;
            Nodes = new List<Tuple<double, Vertices>>();
        }
        #endregion

      
        #region helperMethods
        public int getLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 1;
        }

        public bool hasLeftChild(int parentIndex)
        {
            return getLeftChildIndex(parentIndex) < Size;
        }

        public Tuple<double, Vertices> leftChild(int index)
        {
            return Nodes[getLeftChildIndex(index)];
        }

        public int getRightChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        public bool hasRightChild(int parentIndex)
        {
            return getRightChildIndex(parentIndex) < Size;
        }

        public Tuple<double, Vertices> rightChild(int index)
        {
            return Nodes[getRightChildIndex(index)];
        }

        public int getParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        public bool hasParent(int childIndex)
        {
            return getParentIndex(childIndex) >= 0;
        }

        public Tuple<double, Vertices> parent(int index)
        {
            return Nodes[getParentIndex(index)];
        }

        public void swap(int index1, int index2)
        {
            Tuple<double, Vertices> temp = Nodes[index1];
            Nodes[index1] = Nodes[index2];
            Nodes[index2] = temp;
        }

        #endregion

        #region available public methods

        /// <summary>
        /// Gets the minimum element at the root of the tree
        /// </summary>
        /// <returns>Int value of minimum element</returns>
        /// <exception cref="">InvalidOperationException when heap is empty</exception>
        public bool peek()
        {
            if (Size == 0)
            {
                return false;
            }
              return true;
        }

        /// <summary>
        /// Removes the minimum element at the root of the tree
        /// </summary>
        /// <returns>Int value of minimum element</returns>
        /// <exception cref="">InvalidOperationException when heap is empty</exception>
        public Tuple<double, Vertices> pop()
        {
            if (Size == 0)
                throw new InvalidOperationException("Heap is empty");

            Tuple<double, Vertices> item = Nodes[0];
            Nodes[0] = Nodes[Size - 1];
            Size--;
            heapifyDown();
            return item;
        }

        /// <summary>
        /// Add a new item to heap, enlarging the array if needed
        /// </summary>
        /// <returns>void</returns>
        public void add(double t, Vertices ver)
        {
            Tuple<double, Vertices> item = new Tuple<double, Vertices>(t, ver);
            Nodes[Size] = item;
            Size++;
            heapifyUp();
        }
        #endregion

        #region internal methods
        public void heapifyDown()
        {
            int index = 0;
            while (hasLeftChild(index))
            {
                int smallerChildIndex = getLeftChildIndex(index);
                if (hasRightChild(index) &&  rightChild(index).Item1 < leftChild(index).Item1)
                {
                    smallerChildIndex = getRightChildIndex(index);
                }
                 
                if (Nodes[smallerChildIndex].Item1 < Nodes[index].Item1)
                    swap(index, smallerChildIndex);
                else
                    break;
                index = smallerChildIndex;
            }
        }
        public void heapifyUp()
        {
            int index = Size - 1;

            while (hasParent(index) && parent(index).Item1 > Nodes[index].Item1)
            {
                swap(index, getParentIndex(index));
                index = getParentIndex(index);
            }
        }
        #endregion
    }*/
     
