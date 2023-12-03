using System;
using System.Collections.Generic;
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

        static void Main(string[] args)
        {
            // create xml-file for data of the current car
            createMyCar();

            // Create xml-file for data of cars around
            createNeighbourCars();
        }
    }
}
