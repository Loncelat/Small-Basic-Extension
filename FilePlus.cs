using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Reflection;

using Microsoft.SmallBasic.Library;

namespace Small_Basic_Extension_1
{
    /// <summary>
    /// De FilePlus-bibliotheek voegt meer functies toe die te maken hebben met bestanden.
    /// </summary>
    [SmallBasicType]
    public static class FilePlus
    { 
        /// <summary>
        /// De locatie van de bestandmap "Roaming".
        /// </summary>
        public static Primitive AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\";

        /// <summary>
        /// Maakt een directory en alle subdirectories aan, tenzij ze al bestaan.
        /// </summary>
        /// <param name="Path">De locatie van de aan te maken directory.</param>
        /// <returns>Niets.</returns>
        public static Primitive CreateDirectory(Primitive Path)
        {
            Directory.CreateDirectory(Path);
            return null;
        }

        /// <summary>
        /// Controleert op de opgegeven directory bestaat.
        /// </summary>
        /// <param name="Path">De locatie van de directory.</param>
        /// <returns>True of False</returns>
        public static Primitive DirectoryExists(Primitive Path) => Directory.Exists(Path);

        /// <summary>
        /// Decodeerd het opgegeven bestand.
        /// </summary>
        /// <param name="Path">De locatie van het bestand.</param>
        /// <returns>Niets.</returns>
        public static Primitive Decrypt(Primitive Path)
        {
            if (System.IO.File.Exists(Path))
                try { System.IO.File.Decrypt(Path); }
                catch { }

            return null;
        }

        /// <summary>
        /// Versleuteld het opgegeven bestand.
        /// </summary>
        /// <param name="Path">De locatie van het bestand.</param>
        /// <returns>Niets.</returns>
        public static Primitive Encrypt(Primitive Path)
        {
            if (System.IO.File.Exists(Path))
                try { System.IO.File.Encrypt(Path); }
                catch { }
            
            return null;
        }

        /// <summary>
        /// Controleert of het opgegeven bestand bestaat.
        /// </summary>
        /// <param name="Path">De locatie van het bestand.</param>
        /// <returns>True of False</returns>
        public static Primitive FileExists(Primitive Path) => System.IO.File.Exists(Path);

        /// <summary>
        /// Telt het aantal regels in een tekstdocument.
        /// </summary>
        /// <param name="Path">De locatie van het tekstdocument.</param>
        /// <returns>Het aantal regels in het tekstdocument.</returns>
        public static Primitive GetLineCount(Primitive Path)
        {
            int lineCount = 0;
            using (var doc = System.IO.File.OpenText(Path))
            {
                //ALS ER EEN CHARACTER (INCL. ENTER ETC.) STAAT
                while (doc.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }

        /// <summary>
        /// Geeft "True" of "False" afhankelijk van of de gebruiker toegang heeft tot het internet.
        /// </summary>
        public static Primitive HasInternetConnection
        {
            get
            {
                try
                {
                    using (var client = new System.Net.WebClient())
                    {
                        using (var stream = client.OpenRead("http://www.google.com"))
                        {
                            return "True";
                        }
                    }
                }

                catch
                {
                    return "False";
                }

            }

        }        

        /// <summary>
        /// De locatie van het programma.
        /// </summary>
        public static Primitive Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";

        /// <summary>
        /// Maakt een nieuw tekstbestand aan op de gegeven locatie.
        /// </summary>
        /// <param name="Destination">De locatie waar het tekstbestand komt te staan.</param>
        /// <param name="Name">De naam die het tekstbestand krijgt.</param>
        /// <returns>Niets.</returns>
        public static Primitive MakeTextFile(Primitive Destination, Primitive Name)
        {
            if (!Text.EndsWith(Destination, @"\"))
                Destination += @"\";

            if (!Text.EndsWith(Name, ".txt"))
                Name += ".txt";

            System.IO.File.CreateText(Destination + Name);
            return null;
        }
        
        /// <summary>
        /// De locatie van de bestandsmap "Deze pc".
        /// </summary>
        public static Primitive MyComputer => Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) + @"\";

        /// <summary>
        /// De locatie van de bestandsmap "Mijn Documenten".
        /// </summary>
        public static Primitive MyDocuments => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";

        /// <summary>
        /// De locatie van de bestandsmap "Mijn Muziek".
        /// </summary>
        public static Primitive MyMusic => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\";

        /// <summary>
        /// De locatie van de bestandsmap "Mijn Afbeeldingen".
        /// </summary>
        public static Primitive MyPictures => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + @"\";

        /// <summary>
        /// De locatie van de bestandsmap "Mijn Videos".
        /// </summary>
        public static Primitive MyVideos => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\";

        /// <summary>
        /// Geeft de mogelijkheid om afbeeldingen vanaf het internet opslaan.
        /// </summary>
        /// <param name="URL">De URL van de afbeelding.</param>
        /// <param name="Destination">De locatie waar de afbeelding wordt opgeslagen.</param>
        /// <param name="Name">De naam van de afbeelding.</param>
        /// <returns>Niets.</returns>
        public static Primitive SaveImageFromURL(Primitive URL, Primitive Destination, Primitive Name)
        {
            if (!Text.EndsWith(Destination, @"\"))
            {
                Destination += @"\";
            }
            
            try
            {
                WebRequest requestImage = WebRequest.Create(URL);
                WebResponse responseImage = requestImage.GetResponse();
                Image webImage = Image.FromStream(responseImage.GetResponseStream());
                webImage.Save(Destination + Name + ".png", System.Drawing.Imaging.ImageFormat.Png);
                return null;
            }
            catch { return "ERROR"; }
        }
    }
}
