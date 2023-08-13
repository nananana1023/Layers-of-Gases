using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gases2
{
   public abstract class Layer
    {
        public class InvalidAtmosphericVariableException : Exception { };
        private double thickness;
            public double getT()
            {
                return thickness;
            }
            public void setT(double t)
            {
                thickness = t;
            }
            private string name;
            public string getName()
            {
                return name;
            }
            protected Layer(double tick, string n)
            {
                thickness = tick;
                name = n;
            }
            //the newly transformed layer ascends and engrosses the first identical type of layer 
            // of gases over it .In case there is no identical layer above, it creates a new layer on the top of the atmosphere
            public void WeatherOnLayer(IAtmosVar weather, List<Layer> layers, int index)
            {
                if(weather==null)
                throw new InvalidAtmosphericVariableException();
                if (Alive(layers))
                {
                    Layer newLayer = Changes(weather); //newly transformed layer
                    bool exists = false;
                    for (int k = index; k < layers.Count(); k++) //upcoming layers
                    {
                        if (layers[k].getName() == newLayer.getName()) // first identical type
                        {
                            exists = true;
                            layers[k].setT(layers[k].getT() + newLayer.getT());
                            break;
                        }
                    }
                    if (!exists && newLayer.getT() >= 0.5) // no identical layer above
                    {
                        layers.Add(newLayer); //new layer on the top of the atmosphere
                    }
                }
                else
                    layers.Remove(this);
            }
            /*No layer can have a thickness less than 0.5 km, unless it ascends to the identical-type upper layer. In case there is 
            no identical one, the layer perishes. */
            public bool Alive(List<Layer> layers)
            {
      
                if (thickness < 0.5)
                { 
                    for (int k = 0; this != layers[k] && k < layers.Count(); k++)
                    {
                        if (layers[k].getName() == this.name) //identical-type upper layer
                        {
                            layers[k].setT(this.thickness + layers[k].getT());
                            return true;
                        }
                    }
                    return false; //no identical one, the layer perishes
                }
                else
                {
                    return true;
                }
           
            
            }
            protected abstract Layer Changes(IAtmosVar var);
        }

        //ozone, oxygen, carbon classes 
       public class Ozone : Layer
        {
            public Ozone(double t, string n = "Ozone") : base(t, n) { }
            protected override Layer Changes(IAtmosVar var)
            {
                return var.ChangeO(this);
            }
        }
       public class Oxygen : Layer
        {
            public Oxygen(double t, string n = "Oxygen") : base(t, n) { }
            protected override Layer Changes(IAtmosVar var)
            {
                return var.ChangeX(this);
            }

        }
      public  class Carbon : Layer
        {
            public Carbon(double t, string n = "Carbon") : base(t, n) { }
            protected override Layer Changes(IAtmosVar var)
            {
                return var.ChangeC(this);
            }

        
    }
}
