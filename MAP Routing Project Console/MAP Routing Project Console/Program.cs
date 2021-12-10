using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MAP_Routing_Project_Console
{
	class Program
	{
		static void Main(string[] args)
		{

           
            Class1 functions = new Class1();
            List<Vertices> VerList = new List<Vertices>();
            List<Edges> EdgList = new List<Edges>();
            List<Query> QueryList = new List<Query>();
            Dictionary<Vertices, List<Edges>> Graph = new Dictionary<Vertices, List<Edges>>();
            Graph = functions.ConstructGraph(VerList, EdgList, Graph);
            functions.CalculateShortestPath(VerList, QueryList, Graph);
          
		}
	}
}
