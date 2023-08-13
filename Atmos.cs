using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gases2
{
    
    public interface IAtmosVar
    {
        public class InvalidSizeOfLayersException : Exception { };
        public class InvalidAtmosphericVariableException : Exception { };
        Layer ChangeO(Ozone l);
        Layer ChangeX(Oxygen l);
        Layer ChangeC(Carbon l);
        public void simulation(List<Layer> layers)
        {
            if (layers.Count < 3)
                throw new InvalidSizeOfLayersException();
            try
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].WeatherOnLayer(this, layers, i);
                }
            }
            catch (InvalidAtmosphericVariableException)
            {
                Console.WriteLine("Atmospheric variable invalid!");
            }
        }
    }

    //singletons - atmospheric variables
  public  class Thunderstorm : IAtmosVar
    {
       
        public Layer ChangeO(Ozone l)
        {
            return new Ozone(0);
        }
        public Layer ChangeX(Oxygen l)
        {
            //50% turns to ozone
            Ozone o = new Ozone(l.getT() / 2);
            l.setT(l.getT() / 2);
            return o;
        }
        public Layer ChangeC(Carbon l)
        {
            return new Carbon(0);
        }
        private Thunderstorm() { }
        private static Thunderstorm instance = null;
        public static Thunderstorm Instance() //creates only one instance
        {
            if (instance == null)
            {
                instance = new Thunderstorm();
            }
            return instance;
        }
    }

    public class Sunshine : IAtmosVar
    {
      
        public Layer ChangeO(Ozone l)
        {
            return new Ozone(0);
        }
        public Layer ChangeX(Oxygen l)
        {
            //5% turns to ozone
            Ozone o = new Ozone(l.getT() * 0.05);
            l.setT(l.getT() * 0.95);
            return o;
        }
        public Layer ChangeC(Carbon l)
        {
            //5% to oxygen 
            Oxygen o = new Oxygen(l.getT() * 0.05);
            l.setT(l.getT() * 0.95);
            return o;

        }
        private Sunshine() { }
        private static Sunshine instance = null;
        public static Sunshine Instance() //creates only one instance
        {
            if (instance == null)
            {
                instance = new Sunshine();
            }
            return instance;
        }
    }

    public class Other : IAtmosVar
    {
       
        public Layer ChangeO(Ozone l)
        {
            //5% to oxygen 
            Oxygen o = new Oxygen(l.getT() * 0.05);
            l.setT(l.getT() * 0.95);
            return o;
        }
        public Layer ChangeX(Oxygen l)
        {
            //10% turns to carbon
            Carbon o = new Carbon(l.getT() * 0.1);
            l.setT(l.getT() * 0.9);
            return o;
        }
        public Layer ChangeC(Carbon l)
        {
            return new Carbon(0);
        }
        private Other() { }
        private static Other instance = null;
        public static Other Instance() //creates only one instance
        {
            if (instance == null)
            {
                instance = new Other();
            }
            return instance;
        }
    }
}
