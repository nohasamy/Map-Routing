using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_Routing_Project_Console
{
	public class Vertices
	{

		public int Vertices_Num;
		public double X_Axis;
		public double Y_Axis;

		public void display()
		{
			Console.WriteLine(Vertices_Num);
			Console.WriteLine(X_Axis);
			Console.WriteLine(Y_Axis);
		}
			
	}
}
