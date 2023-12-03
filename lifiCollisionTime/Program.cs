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
        }
    }
}
