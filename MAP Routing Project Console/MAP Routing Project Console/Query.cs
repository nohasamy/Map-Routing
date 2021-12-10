using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP_Routing_Project_Console
{
	public class Query
	{
		public double X_Source;
		public double Y_Source;
		public double X_Destnation;
		public double Y_Destnation;
		public double R;

		public void display()
		{
			Console.WriteLine(X_Source);
			Console.WriteLine(Y_Source);
			Console.WriteLine(X_Destnation);
			Console.WriteLine(Y_Destnation);
			Console.WriteLine(R);
		}
	}
}
