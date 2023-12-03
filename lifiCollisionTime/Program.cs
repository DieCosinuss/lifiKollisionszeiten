using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lifiCollisionTime
{
    internal class Program
    {
        static void createMyCar()
        {
            // Create data of myself in XML.
            XDocument aktuellerVerkehrsteilnehmer = new XDocument(
                new XElement("verkehrsteilnehmende",
                    new XElement("auto",
                        new XElement("geschwindigkeit", "50"),
                        new XElement("beschleunigung", "20")
                    )
                )
            );
            aktuellerVerkehrsteilnehmer.Save("lifiMeinDatenpaket.xml");
        }

        static void createNeighbourCars()
        {
            // Create data of cars in XML.
            XDocument andereVerkehrsteilnehmende = new XDocument(
                new XElement("verkehrsteilnehmende",
                    new XElement("auto",
                        new XElement("distanz", "10"),
                        new XElement("geschwindigkeit", "20"),
                        new XElement("beschleunigung", "0")
                    ),
                    new XElement("auto",
                        new XElement("distanz", "2"),
                        new XElement("geschwindigkeit", "40"),
                        new XElement("beschleunigung", "10")
                    )
                )
            );
            andereVerkehrsteilnehmende.Save("lifiDatenpaketEmpfangen.xml");
        }

        static XElement loadXMLDataToXElement(string filename)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var datenpaket = Path.Combine(currentDirectory, filename);
            return XElement.Load(datenpaket);
        }

        static void checkForCollision(double currentAcceleration, double myAcceleration, double currentVelocity, double myVelocity, double currentDistance)
        {
            // Compute collision time according to physical newtons law
            if (currentAcceleration != myAcceleration)
            {
                double Da = currentAcceleration - myAcceleration;
                double Dv = currentVelocity - myVelocity;
                double p = 2.0 * Dv / Da;
                double q = 2.0 * currentDistance / Da;
                double insideSqrt = p * p / 4.0 - q;
                if (insideSqrt < 0.0)
                {
                    Console.WriteLine("Keine Kollision bevorstehend...");
                }
                else if (insideSqrt == 0.0)
                {
                    Console.WriteLine("Achtung: Kollision in ungefaehr {0} Sekunden...", -p / 2.0);
                }
                else
                {
                    double timeToCollision1 = -p / 2.0 + Math.Sqrt(insideSqrt);
                    double timeToCollision2 = -p / 2.0 - Math.Sqrt(insideSqrt);
                    if (timeToCollision1 >= 0.0 && timeToCollision2 >= 0.0)
                    {
                        Console.WriteLine("Achtung: Kollision in ungefaehr {0} Sekunden...", Math.Min(timeToCollision1, timeToCollision2));
                    }
                    else if ((timeToCollision1 >= 0.0 && timeToCollision2 <= 0.0) || (timeToCollision1 <= 0.0 && timeToCollision2 >= 0.0))
                    {
                        Console.WriteLine("Achtung: Kollision in ungefaehr {0} Sekunden...", Math.Max(timeToCollision1, timeToCollision2));
                    }
                    else
                    {
                        Console.WriteLine("Keine Kollision bevorstehend...");
                    }
                }
            }
            else
            {
                Console.WriteLine("Keine Kollision bevorstehend...");
            }
        }

        static void Main(string[] args)
        {
            // create xml-file for data of the current car
            createMyCar();

            // Create xml-file for data of cars around
            createNeighbourCars();

            // Loading my data
            XElement meinDatenpaketFilepath = loadXMLDataToXElement("lifiMeinDatenpaket.xml");
            double myVelocity = (double)meinDatenpaketFilepath.Element("auto").Element("geschwindigkeit");
            double myAcceleration = (double)meinDatenpaketFilepath.Element("auto").Element("beschleunigung");

            // The Three Parts of a LINQ Query: Loading and processing of the data of neighbouring cars
            // 1. Data source.
            XElement datenpaketEmpfangen = loadXMLDataToXElement("lifiDatenpaketEmpfangen.xml");

            // 2. Query creation.
            IEnumerable<XElement> autos = from item in datenpaketEmpfangen.Descendants("auto") select item;

            // 3. Query execution.
            foreach (XElement currentCar in autos)
            {
                double currentDistance = (double)currentCar.Element("distanz");
                double currentVelocity = (double)currentCar.Element("geschwindigkeit");
                double currentAcceleration = (double)currentCar.Element("beschleunigung");

                // Compute collision times according to newtons law
                checkForCollision(currentAcceleration, myAcceleration, currentVelocity, myVelocity, currentDistance);
            }

            Console.ReadKey();
        }
    }
}
