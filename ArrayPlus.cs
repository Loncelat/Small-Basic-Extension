using System;

using Microsoft.SmallBasic.Library;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Small_Basic_Extension_1
{
    [SmallBasicType]
    public static class ArrayPlus
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Primitive ItemCount(Primitive array) => IsArray(array) ? ArrayToDoubleList(array).Count : 0;

        /// <summary>
        /// Berekent het gemiddelde van alle getallen in de array.
        /// </summary>
        /// <param name="array">De array waarvan het gemiddelde berekend moet worden.</param>
        /// <returns>Het gemiddelde van alle getallen in de array.</returns>
        public static Primitive Mean(Primitive array)
        {
            double sum = 0;
            if (IsArray(array))
            {
                List<double> itemList = ArrayToDoubleList(array);

                foreach (var x in itemList)
                {
                    sum += x;
                }
                return sum / itemList.Count;
            }
            return 0;
        }

        /// <summary>
        /// De mediaan van alle getallen in de array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Primitive Median(Primitive array)
        {
            if (IsArray(array))
            {
                List<double> itemList = ArrayToDoubleList(array);
                double count = itemList.Count;
                if (count > 0)
                {
                    //Sorteer de getallen.
                    itemList.Sort();

                    int midIndex = (int)count / 2;

                    if (count % 2 == 0)
                    {
                        return (itemList[midIndex] + itemList[midIndex - 1]) / 2;
                    }
                    else
                    {
                        return itemList[midIndex];
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Berekent het product van allegetallen in de array.
        /// </summary>
        /// <param name="array">De array waarvan het product berekend moet worden.</param>
        /// <returns>Het product van alle getallen in de array.</returns>
        public static Primitive Product(Primitive array)
        {
            double product = 1.0;
            if (IsArray(array))
            {
                List<double> itemList = ArrayToDoubleList(array);

                foreach (var x in itemList)
                {
                    product *= x;

                    // Eindig vroegtijdig als de som nul geworden is.
                    if (product == 0) { return 0; }

                }
            }
            return product;
        }

        /// <summary>
        /// Berekent de som van alle getallen in de array.
        /// </summary>
        /// <param name="array">De array waarvan de som berekent moet worden.</param>
        /// <returns>De som van alle getallen in de array.</returns>
        public static Primitive Sum(Primitive array)
        {
            double sum = 0;
            if (IsArray(array))
            {
                List<double> itemList = ArrayToDoubleList(array);

                foreach (var x in itemList)
                {
                    sum += x;
                }
            }
            return sum;
        }

        private static bool IsArray(Primitive array) => Microsoft.SmallBasic.Library.Array.IsArray(array);

        private static List<double> ArrayToDoubleList(string array)
        {
            //Vervang "\=" met "s".
            array = Regex.Replace(array, @"(\\=)", "s");

            List<double> lst = new List<double>();
			//Tussen '=' en ';'
            foreach (Match regexMatch in Regex.Matches(array, "(?<==).*?(?=;)"))
            {
                if (double.TryParse(regexMatch.Value, out double val))
                {
                    lst.Add(val);
                }
            }

            return lst;
        }
    }
}
