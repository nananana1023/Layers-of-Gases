using System.Reflection.Emit;
using TextFile;

namespace Gases2
{
    internal class Program
    {
        public class InvalidSizeOfLayersException : Exception { };
        static void Main(string[] args)
        {
            string textfile = "4.txt";
            TextFileReader r = new(textfile);

            r.ReadLine(out string line);
            int n = int.Parse(line); //number of layers
            List<Layer> layers = new List<Layer>();   //list of layers
            for (int i = 0; i < n; i++)
            {
                Layer lay = null;

                if (r.ReadLine(out line))
                {
                    string[] tokens = line.Split(' ');
                    char ch = char.Parse(tokens[0]); //layer
                    double t = double.Parse(tokens[1]); //thickness

                    switch (ch)
                    {
                        case 'Z': lay = new Ozone(t); break;
                        case 'X': lay = new Oxygen(t); break;
                        case 'C': lay = new Carbon(t); break;
                    }
                }
                layers.Add(lay);
            }

            //first state
            for (int i = 0; i < layers.Count(); ++i)
            {
                Console.WriteLine(layers[i].getName() + ": " + layers[i].getT());
            }

            /* In case the simulation is over, it continues from the beginning.The program should continue the simulation until 
            the number of layers is the triple of the initial number of layers or is less than three. The program should
            print all attributes of the layers by simulation rounds!*/

            r.ReadLine(out string line2);
            int sim = 0;
            if(layers.Count <3)
                throw new InvalidSizeOfLayersException();
            while (layers.Count() >= 3 && layers.Count() < n * 3)
            {
                for (int j = 0; layers.Count() >= 3 && layers.Count() < n * 3 && j < line2.Length; ++j)
                {
                    char c = line2[j];
                   
                    IAtmosVar weather=null; 
                    switch (c)
                    {
                        case 'S': weather=Sunshine.Instance(); break;
                        case 'O': weather = Other.Instance(); break;
                        case 'T': weather = Thunderstorm.Instance(); break;
                    }
                    weather.simulation(layers);
                    Console.WriteLine(++sim + "th simulation:");
                    for (int i = 0; i < layers.Count(); ++i)
                    {
                        Console.WriteLine(layers[i].getName() + ": " + layers[i].getT());
                    }
                }
            }
            
        }
    }
}