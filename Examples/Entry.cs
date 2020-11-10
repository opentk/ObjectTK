using System;
using System.Linq;
using System.Reflection;
using Examples.Examples;
using OpenTK.Windowing.Desktop;

namespace Examples {
    internal class Entry {
        private static void Main(string[] args) {
            using var gw = new HelloTriangle();
            gw.Run();
            //
            // var Examples = Assembly.GetExecutingAssembly().GetTypes()
            //     .Where(_ => _ != typeof(ExampleWindow) && typeof(ExampleWindow).IsAssignableFrom(_))
            //     .Select(Type =>
            //         new {
            //             Caption = Type.GetCustomAttributes<ExampleProjectAttribute>(false).FirstOrDefault()?.Caption ?? nameof(Type), Type
            //         }).ToArray();
            //
            // while (PromptForExample(out var Example)) {
            //     using (var ExampleWindow = (GameWindow) Activator.CreateInstance(Example)) {
            //         ExampleWindow.Run();
            //     }
            // }
            //
            // bool PromptForExample(out Type Example) {
            //     Example = null;
            //     var Count = 1;
            //     Console.WriteLine("Select example: ");
            //
            //     foreach (var Pair in Examples) {
            //         Console.WriteLine($"{Count++} - {Pair.Caption}");
            //     }
            //
            //     var input = Console.ReadLine();
            //     if (int.TryParse(input, out var Selection) && Selection - 1 >= 0 && Selection - 1 < Examples.Length) {
            //         Example = Examples[Selection - 1].Type;
            //         return true;
            //     }
            //
            //     return false;
            // }
        }
    }
}
