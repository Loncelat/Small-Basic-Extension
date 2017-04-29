using System;
using System.Collections.Generic;
using System.Globalization;

namespace Small_Basic_Extension_1
{
    class MathInterpreter
    {
        private double[] numbers = new double[2];

        public bool ErrorOccurred { get; private set; } = false;

        public double Solve(List<string> mathStringList)
        {
            WorkOutBrackets(mathStringList);
            if (TryMultiplicationAndDivision(mathStringList))
            {
                AdditionAndSubtraction(mathStringList);
            }
            else
            {
                ErrorOccurred = true;
                return 0;
            }
            return double.Parse(mathStringList[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
        }

        private void WorkOutBrackets(List<string> equationList)
        {
            int[] bracketCount = new int[2];
            List<string> bracketMath = new List<string>();
            for (var i = 0; i < equationList.Count; i++)
            {
                if (StartsWith(equationList[i], "("))
                {
                    bracketCount[0] += GetCount(equationList[i], '(');
                    bracketMath.Add(equationList[i].Remove(0, 1));

                    // Zoek naar het einde van de zojuist geopende haakjes.
                    for (var k = i + 1; k < equationList.Count; k++)
                    {
                        if (EndsWith(equationList[k], ")"))
                        {
                            bracketCount[1] += GetCount(equationList[k], ')');

                            if (bracketCount[0] == bracketCount[1])
                            {
                                bracketMath.Add(equationList[k].Substring(0, equationList[k].Length - 1));
                                break;
                            }
                        }
                        else if (StartsWith(equationList[k], "("))
                        {
                            bracketCount[0] += GetCount(equationList[k], '(');
                        }

                        bracketMath.Add(equationList[k]);
                    }
                    equationList.RemoveRange(i, bracketMath.Count);
                    bracketMath.RemoveAll(str => str == " ");
                    equationList.Insert(i, ReplaceCommaWithDot(Solve(bracketMath).ToString()));
                    bracketMath.Clear();
                }
            }
        }

        private bool TryMultiplicationAndDivision(List<string> equationList)
        {
            string operationResult = "";

            for (var i = 1; i < equationList.Count; i += 2)
            {
                if (i + 1 < equationList.Count)
                {
                    if (double.TryParse(equationList[i - 1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out numbers[0])
                                       && double.TryParse(equationList[i + 1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out numbers[1]))
                    {
                        switch (equationList[i])
                        {
                            case "*":
                                operationResult = (numbers[0] * numbers[1]).ToString();
                                equationList.RemoveRange(i - 1, 3);
                                equationList.Insert(i - 1, ReplaceCommaWithDot(operationResult));
                                i -= 2;
                                break;

                            case "/":
                                operationResult = (numbers[0] / numbers[1]).ToString();
                                equationList.RemoveRange(i - 1, 3);
                                equationList.Insert(i - 1, ReplaceCommaWithDot(operationResult));
                                i -= 2;
                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void AdditionAndSubtraction(List<string> equationList)
        {
            string operationResult = "";

            for (var i = 1; i < equationList.Count; i += 2)
            {
                if (i + 1 < equationList.Count)
                {
                    numbers[0] = double.Parse(equationList[i - 1], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                    numbers[1] = double.Parse(equationList[i + 1], NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                    switch (equationList[i])
                    {
                        case "+":
                            operationResult = (numbers[0] + numbers[1]).ToString();
                            equationList.RemoveRange(i - 1, 3);
                            equationList.Insert(i - 1, ReplaceCommaWithDot(operationResult));
                            i -= 2;
                            break;

                        case "-":
                            operationResult = (numbers[0] - numbers[1]).ToString();
                            equationList.RemoveRange(i - 1, 3);
                            equationList.Insert(i - 1, ReplaceCommaWithDot(operationResult));
                            i -= 2;
                            break;
                    }
                }
            }
        }

        private string ReplaceCommaWithDot(string input) => input.Replace(',', '.');
        private bool StartsWith(string input, string character) => input.StartsWith(character, StringComparison.Ordinal);
        private bool EndsWith(string input, string character) => input.EndsWith(character, StringComparison.Ordinal);

        private int GetCount(string input, char CharacterToCount)
        {
            int count = 0;
            foreach (var i in input)
            {
                if (i == CharacterToCount)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
