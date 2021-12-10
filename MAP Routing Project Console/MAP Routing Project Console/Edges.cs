using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_Routing_Project_Console
{
	public class Edges
	{

		public int Vertices_1;
		public int Vertices_2;
		public double Distance;
		public double Speed;
        public double time;
		public void display()
		{
			Console.WriteLine(Vertices_1);
			Console.WriteLine(Vertices_2);
			Console.WriteLine(Distance);
			Console.WriteLine(Speed);
		}
	}
}
