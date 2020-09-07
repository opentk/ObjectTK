using Examples.BasicExamples;
using log4net;
using log4net.Config;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Examples {
	public class Entry {

		public static void Main() {

			// initialize log4net via app.config if available
			if (ObjectTK.Logging.LogFactory.IsAvailable)
				ConfigureLogging();

			var examples = GetExamples();

			Tuple<string, Type> example = null;
			while (PromptForExample(examples, out example)) {

				using (var exampleWindow = (GameWindow)Activator.CreateInstance(example.Item2)) {
					exampleWindow.Title = example.Item1;
					exampleWindow.Run();
				}
			}


		}
		public static void ConfigureLogging() {
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
		}


		private static List<Tuple<string, Type>> GetExamples() {
			var baseType = typeof(ExampleWindow);
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(_ => _ != baseType && baseType.IsAssignableFrom(_));
			// add tree nodes for example projects
			List<Tuple<string, Type>> examples = new List<Tuple<string, Type>>();
			foreach (var type in types) {
				// add this example and get the caption from the attribute
				var captionAttribute = type.GetCustomAttributes<ExampleProjectAttribute>(false).FirstOrDefault();
				// remember example type
				examples.Add(new Tuple<string, Type>(type.Name, type));
			}
			return examples;
		}

		private static bool PromptForExample(List<Tuple<string, Type>> examples, out Tuple<string, Type> example) {
			example = null;
			int count = 1;
			Console.WriteLine("Select example: ");
			
			foreach(var pair in examples) {
				Console.WriteLine($"{count++} - {pair.Item1}");
			}

			string input = Console.ReadLine();
			if(int.TryParse(input, out int selection) && selection - 1 >= 0 && selection - 1 < examples.Count) {
				example = examples[selection - 1];
				return true;
			}

			return false;
		}

	}

}
