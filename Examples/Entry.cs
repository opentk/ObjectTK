using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Examples {
	class Entry {
		static void Main(string[] args) {

			var Examples = Assembly.GetExecutingAssembly().GetTypes()
				.Where(_ => _ != typeof(ExampleWindow) && typeof(ExampleWindow).IsAssignableFrom(_))
				.Select(Type => 
				new {
					Caption = Type.GetCustomAttributes<ExampleProjectAttribute>(false).FirstOrDefault()?.Caption ?? nameof(Type),
					Type = Type
				}).ToArray();

			while (PromptForExample(out Type Example)) {
				using (var ExampleWindow = (GameWindow)Activator.CreateInstance(Example)) {
					ExampleWindow.Run();
				}
			}

			bool PromptForExample(out Type Example) {
				Example = null;
				int Count = 1;
				Console.WriteLine("Select example: ");

				foreach (var Pair in Examples) {
					Console.WriteLine($"{Count++} - {Pair.Caption}");
				}

				string input = Console.ReadLine();
				if (int.TryParse(input, out int Selection) && Selection - 1 >= 0 && Selection - 1 < Examples.Length) {
					Example = Examples[Selection - 1].Type;
					return true;
				}

				return false;
			}


		}

	}
}
