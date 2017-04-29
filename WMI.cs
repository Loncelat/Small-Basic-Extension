using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;
using Microsoft.SmallBasic.Library;

namespace Small_Basic_Extension_1
{
    /// <summary>
    /// 
    /// </summary>
    [SmallBasicType]
    public static class WMI
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WMI_Class"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static Primitive FetchProperty(Primitive WMI_Class, Primitive PropertyName)
        {
            try
            {
                ////Kijkt of Win32_Class met "Win32_" begint.
                //if (!(WMI_Class.ToString().StartsWith("Win32_"))) { WMI_Class = "Win32_" + WMI_Class; }

                //Maak een ManagementObjectSearcher aan.
                ManagementObjectSearcher _searcher = new ManagementObjectSearcher("SELECT * FROM " + WMI_Class);

                foreach (ManagementObject _mo in _searcher.Get())
                {
                    PropertyData prop = _mo.Properties[PropertyName];

                    return prop?.Value.ToString();

                }

            }
            catch { }

            return null;
        }

        /// <summary>
        /// Deze subroutine print alle eigenschappen van een WMI-Class naar de TextWindow.
        /// </summary>
        /// <param name="Win32_Class">De naam van de WMI-Class waarvan de eigenschappen geprint moeten worden.</param>
        /// <returns>Alle eigenschappen van de opgegeven WMI-class</returns>
        public static Primitive PrintAllProperties(Primitive Win32_Class)
        {
            try
            {
                ////Kijkt of Win32_Class met "Win32_" begint.
                //if (!(Win32_Class.ToString().StartsWith("Win32_"))) { Win32_Class = "Win32_" + Win32_Class; }

                //Maak een ManagementObjectSearcher aan.
                ManagementObjectSearcher _searcher = new ManagementObjectSearcher("SELECT * FROM " + Win32_Class);

                //Loop door elk object in de opgegeven class.
                foreach (ManagementObject _mo in _searcher.Get())
                {
                    //Schrijf de naam van het object waar naar gekeken wordt.
                    TextWindow.WriteLine(_mo.Properties["Name"].Value.ToString());

                    //Loop door elke eigenschap van het object.
                    foreach (PropertyData prop in _mo.Properties)
                    {
                        //Print het object als het een waarde heeft.
                        if (prop.Value != null)
                        {

                            TextWindow.WriteLine("--" + prop.Name.ToString() + ": " + prop.Value.ToString());

                        }
                    }

                    TextWindow.WriteLine("");

                }
            }
            catch { }

            return null;

        }

    }
}
