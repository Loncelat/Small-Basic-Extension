using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.SmallBasic.Library;
using static System.Math;

namespace Small_Basic_Extension_1
{
    /// <summary>
    /// De Logo class voegt extra schildpad-dingen toe.
    /// </summary>
  	[SmallBasicType]
    public static class Logo
    {
        const string operators = "+-*/";
        const double epsilon = 1e-14;

        static bool turtleIsHidden = false;
        static bool speedChangedManually = false;
        static int lengthOfNumericalInput;
        static Random numberGenerator = new Random();
        static Stack<List<string>> repeatStack = new Stack<List<string>>();
        static List<double> multiNumberList = new List<double>(new double[3]);
        static Dictionary<string, List<string>> toShortcuts = new Dictionary<string, List<string>>();
        static Dictionary<string, double> variables = new Dictionary<string, double>()
        {
            { "speed", 10 },
            { "angle", Turtle.Angle },
            { "x", Turtle.X },
            { "y", Turtle.Y },
            { "repcount", 0 }
        };

        /// <summary>
        /// Laat de Turtle de input uitvoeren.
        /// </summary>
        /// <param name="input">Uit te voeren commando's.</param>
        /// <returns>Niets.</returns>
        public static void Draw(Primitive input)
        {
            List<string> commandStorage = new List<string>();
            Turtle.Speed = speedChangedManually ? Turtle.Speed : (Primitive)10;

            input = System.Text.RegularExpressions.Regex.Replace(input, "[ ]{2,}", " ");
            SplitTextIntoWords(Text.ConvertToLowerCase(input.ToString().Trim()), commandStorage);
            AnalyseAndPerformActions(commandStorage);
        }


        //Splitst een string op in losse woorden/haakjesgroepen.
        private static void SplitTextIntoWords(string command, List<string> storageList)
        {
            int startOfSubstring = 0;
            int[] bracketCount = new int[2];

            command = ReplaceCommaWithDot(command);

            storageList.Clear();

            for (var i = 0; i < command.Length; i++)
            {
                if (command[i] == ' ' && bracketCount[0] == bracketCount[1])
                {
                    storageList.Add(command.Substring(startOfSubstring, i - startOfSubstring));
                    startOfSubstring = i + 1;
                }

                else if (command[i] == '[') { bracketCount[0]++; }
                else if (command[i] == ']') { bracketCount[1]++; }
            }

            // Kijkt of de hele string opgesplitst is. Zo niet, dan maak van het resterende stuk tekst een los woord.
            if (startOfSubstring != command.Length)
            {
                storageList.Add(command.Substring(startOfSubstring, command.Length - startOfSubstring));
            }

            if (bracketCount[0] != bracketCount[1]) { ThrowException(2); bracketCount[0] = bracketCount[1]; }
        }


        // Analyseert de lijst en voert bijbehorende commando's uit.
        private static void AnalyseAndPerformActions(List<string> commandList)
        {
            for (var i = 0; i < commandList.Count; i++)
            {
                switch (commandList[i])
                {
                    case "fd":
                    case "forward":
                        Turtle.Move(GetNextNumber(i, commandList));
                        UpdatePositionVariables();
                        i += lengthOfNumericalInput;
                        break;

                    case "bk":
                    case "backward":
                        Turtle.Move(-GetNextNumber(i, commandList));
                        UpdatePositionVariables();
                        i += lengthOfNumericalInput;
                        break;

                    case "rt":
                    case "turn":
                        Turtle.Turn(GetNextNumber(i, commandList));
                        variables["angle"] = Turtle.Angle;
                        i += lengthOfNumericalInput;
                        break;

                    case "lt":
                        Turtle.Turn(-GetNextNumber(i, commandList));
                        variables["angle"] = Turtle.Angle;
                        i += lengthOfNumericalInput;
                        break;

                    case "wait":
                        System.Threading.Thread.Sleep((int)System.Math.Round(GetNextNumber(i, commandList) * 1000 / 60));
                        i += lengthOfNumericalInput;
                        break;

                    case "speed":
                        Turtle.Speed = GetNextNumber(i, commandList);
                        variables["speed"] = Turtle.Speed;
                        i += lengthOfNumericalInput;
                        break;

                    case "msg":
                    case "message":
                    case "print":
                        if (i + 1 < commandList.Count)
                        {
                            TextWindow.WriteLine(RemoveBrackets(commandList[i + 1]));
                            i++;
                        }
                        else { ThrowException(5); }
                        break;

                    case "line":
                        SplitMultiNumberList(i, commandList, 4);

                        GraphicsWindow.DrawLine(multiNumberList[0], multiNumberList[1], multiNumberList[2], multiNumberList[3]);
                        i++;
                        break;

                    case "setpos":
                        SplitMultiNumberList(i, commandList, 2);
                        Turtle.X = multiNumberList[0];
                        Turtle.Y = multiNumberList[1];
                        UpdatePositionVariables();
                        i++;
                        break;

                    case "setpc":
                        SplitMultiNumberList(i, commandList, fromPenColor: true);

                        GraphicsWindow.PenColor = GraphicsWindow.GetColorFromRGB(
                            multiNumberList[0], multiNumberList[1], multiNumberList[2]);
                        i++;
                        break;

                    case "setbc":
                        SplitMultiNumberList(i, commandList);

                        GraphicsWindow.BackgroundColor = GraphicsWindow.GetColorFromRGB(
                            multiNumberList[0], multiNumberList[1], multiNumberList[2]);
                        i++;
                        break;

                    case "hide":
                    case "hideturtle":
                    case "ht":
                        Turtle.Hide();
                        turtleIsHidden = true;
                        break;

                    case "show":
                    case "showturtle":
                    case "st":
                        if (turtleIsHidden)
                            Turtle.Show();

                        turtleIsHidden = false;
                        break;

                    case "pu":
                    case "penup":
                        Turtle.PenUp();
                        break;

                    case "pd":
                    case "pendown":
                        Turtle.PenDown();
                        break;

                    case "file":
                        if (i + 1 < commandList.Count)
                        {
                            DrawFromFile(RemoveBrackets(commandList[i + 1]));
                            i++;
                        }
                        else { ThrowException(5); }
                        break;

                    case "setorientation":
                        Turtle.Angle = GetNextNumber(i, commandList);
                        variables["angle"] = Turtle.Angle;
                        i += lengthOfNumericalInput;
                        break;

                    case "setpx":
                    case "setpixel":
                        SplitMultiNumberList(i, commandList, 2);

                        GraphicsWindow.SetPixel(multiNumberList[0], multiNumberList[1], GraphicsWindow.PenColor);
                        i++;
                        break;


                    case "repeat":
                        i += RepeatCommands(i, commandList);
                        break;

                    case "for":
                        ForLoop(i, commandList);
                        i += 2;
                        break;

                    case "while":
                        WhileLoop(i, commandList);
                        i += 2;
                        break;

                    case "circle":
                        int circleRadius = (int)GetNextNumber(i, commandList);
                        GraphicsWindow.DrawEllipse(Turtle.X - circleRadius, Turtle.Y - circleRadius, circleRadius * 2, circleRadius * 2);
                        i += lengthOfNumericalInput;
                        break;

                    case "cs":
                        ResetTurtle();
                        break;

                    case "ct":
                        TextWindow.Clear();
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    case "if":
                        i += CompareInputs(i, commandList);
                        break;

                    case "make":
                        AddVariable(i, commandList);
                        i += lengthOfNumericalInput + 1;
                        break;

                    case "to":
                        i += ToCommandBase(i, commandList) + 2;
                        break;

                    default:
                        if (toShortcuts.ContainsKey(commandList[i]))
                        {
                            AnalyseAndPerformActions(toShortcuts[commandList[i]]);
                        }
                        else if (commandList[i].StartsWith(":"))
                        {
                            TextWindow.WriteLine(GetNextNumber(i, commandList, 0));
                            i += lengthOfNumericalInput - 1;
                        }
                        else { ThrowException(0, commandList[i]); }
                        break;
                }

            }
        }


        // Schrijft een uitzondering op in de TextWindow.
        private static void ThrowException(int exceptionId, string com = "null")
        {
            // Slaat de oude kleur op en maakt de huidig kleur rood.
            string originalForegroundColor = TextWindow.ForegroundColor;
            TextWindow.ForegroundColor = "Red";

            switch (exceptionId)
            {
                case 0:
                    TextWindow.WriteLine($"De opdracht \"{com}\" is niet herkend.");
                    break;

                case 1:
                    TextWindow.WriteLine("Er ontbreekt een getal.");
                    break;

                case 2:
                    TextWindow.WriteLine("Haakjes zijn niet afgesloten.");
                    break;

                case 3:
                    TextWindow.WriteLine("Het opgegeven bestand bestaat niet of is geen .lgo-bestand.");
                    break;
                case 4:
                    TextWindow.WriteLine("Het aantal keren om te herhalen moet 1 of meer zijn.");
                    break;

                case 5:
                    TextWindow.WriteLine("Er ontbreken opdrachten.");
                    break;

                case 6:
                    TextWindow.WriteLine("Een 'to' mag geen andere 'to' of zichzelf bevatten.");
                    break;

                case 7:
                    TextWindow.WriteLine("Het 'end'-commando is niet gevonden.");
                    break;

                case 8:
                    TextWindow.WriteLine("De haakjesconstructie is verkeerd.");
                    break;

                case 9:
                    TextWindow.WriteLine("Een variabele moet met ? of \" beginnen.");
                    break;

                case 10:
                    TextWindow.WriteLine("De opgegeven variabele bestaat niet.");
                    break;

                case 11:
                    TextWindow.WriteLine("Fout bij if-statement.");
                    break;

                case 12:
                    TextWindow.WriteLine("De opgegeven som kan niet opgelost worden.");
                    break;

                case 13:
                    TextWindow.WriteLine("De opgegeven step zorgt voor een eindeloze herhaling.");
                    break;
            }
            TextWindow.ForegroundColor = originalForegroundColor;
        }


        // Verandert een string in een double.
        private static double GetNextNumber(int index, List<string> commands, int offset = 1)
        {
            if (index + offset < commands.Count)
            {
                if (index + offset + 1 < commands.Count && operators.Contains(commands[index + offset + 1]))
                {
                    List<string> mathToSolve = new List<string>();

                    PrepareEquationForSolving(index + offset + 1, mathToSolve, commands);
                    lengthOfNumericalInput = mathToSolve.Count;
                    return SolveEquation(mathToSolve);

                }
                else if (commands[index + offset].StartsWith(":"))
                {
                    string key = commands[index + offset].Remove(0, 1);
                    lengthOfNumericalInput = 1;
                    return GetVariable(key);
                }
                else if (commands[index + offset] == "random")
                {
                    return GetRandomNumber(index + offset + 1, commands);
                }
                else if (commands[index + offset].StartsWith("(") && commands[index + offset].EndsWith(")"))
                {
                    commands[index + offset] = RemoveBrackets(commands[index + offset]);
                    return GetNextNumber(index, commands);
                }
                else
                {
                    if (double.TryParse(commands[index + offset], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double parseResult))
                    {
                        lengthOfNumericalInput = 1; return parseResult;
                    }
                    else { ThrowException(1); }
                }

            }
            else
            {
                ThrowException(1);
            }
            lengthOfNumericalInput = 1;
            return 0;
        }


        private static void SplitMultiNumberList(int index, List<string> commands, int numberAmountExpected = 3, bool fromPenColor = false)
        {
            List<string> tempList = new List<string>();
            multiNumberList.Clear();
            if (index + 1 < commands.Count && HasBrackets(commands[index + 1]))
            {
                SplitTextIntoWords(RemoveBrackets(commands[index + 1]), tempList);

                if (tempList.Count >= numberAmountExpected)
                {
                    for (var i = 0; i < tempList.Count;)
                    {
                        multiNumberList.Add(GetNextNumber(i, tempList, 0));
                        i += lengthOfNumericalInput;
                    }
                }
                else { MNumError(numberAmountExpected, fromPenColor); }
            }
            else { MNumError(numberAmountExpected, fromPenColor); }
        }


        private static void MNumError(int numAmEx, bool frPC)
        {
            ThrowException(8);

            if (numAmEx == 2)
            {
                multiNumberList[0] = Turtle.X; multiNumberList[1] = Turtle.Y;
            }
            else if (numAmEx == 4) { multiNumberList.ForEach(num => num = 0); }
            else
            {
                if (frPC)
                {
                    System.Drawing.Color tempClr = System.Drawing.ColorTranslator.FromHtml(GraphicsWindow.PenColor);
                    multiNumberList[0] = tempClr.R;
                    multiNumberList[1] = tempClr.G;
                    multiNumberList[2] = tempClr.B;
                }
                else
                {
                    System.Drawing.Color tempClr = System.Drawing.ColorTranslator.FromHtml(GraphicsWindow.BackgroundColor);
                    multiNumberList[0] = tempClr.R;
                    multiNumberList[1] = tempClr.G;
                    multiNumberList[2] = tempClr.B;
                }
            }
        }


        private static int RepeatCommands(int index, List<string> commands)
        {
            if (index + 2 < commands.Count)
            {
                List<string> commandsToBeRepeated = new List<string>();
                int repeatLimit = (int)GetNextNumber(index, commands);
                int indicesToSkip = lengthOfNumericalInput + 1;
                double originalRepcount = variables["repcount"];

                SplitTextIntoWords(RemoveBrackets(commands[index + indicesToSkip]), commandsToBeRepeated);

                if (repeatLimit <= 0) { ThrowException(4); return indicesToSkip; }

                repeatStack.Push(commandsToBeRepeated);
                for (var rp = 0; rp < repeatLimit; rp++)
                {
                    variables["repcount"] = rp + 1;
                    AnalyseAndPerformActions(repeatStack.Peek());
                }
                repeatStack.Pop();

                variables["repcount"] = originalRepcount;

                return indicesToSkip;
            }
            else { ThrowException(5); }
            return 0;
        }


        private static void AddVariable(int index, List<string> commands)
        {
            if (index + 2 < commands.Count)
            {
                string key = commands[index + 1].Remove(0, 1);
                double value = GetNextNumber(index + 1, commands);

                if (commands[index + 1].StartsWith("?") || commands[index + 1].StartsWith("\""))
                {
                    if (!variables.ContainsKey(key)) { variables.Add(key, value); }
                    else
                    {
                        variables[key] = value;
                        SetTurtlePropertiesToVariables(key);
                    }
                }
                else { ThrowException(9); }

            }
            else { ThrowException(1); }
        }


        private static int CompareInputs(int index, List<string> commands)
        {
            if (index + 2 < commands.Count)
            {
                List<string> statementToCheck = new List<string>();
                string conditionString = commands[index + 1];
                string commandString = commands[index + 2];
                bool elseKeywordPresent = (index + 4 < commands.Count && commands[index + 3] == "else");
                bool conditionStateTracker = true;

                if (HasBrackets(conditionString)
                    && HasBrackets(commandString))
                {

                    if (EvaluateCondition(conditionString))
                    {
                        SplitTextIntoWords(RemoveBrackets(commandString), statementToCheck);
                        AnalyseAndPerformActions(statementToCheck);
                        conditionStateTracker = false;
                    }

                    else if (elseKeywordPresent && conditionStateTracker)
                    {
                        commandString = commands[index + 4];
                        SplitTextIntoWords(RemoveBrackets(commandString), statementToCheck);
                        AnalyseAndPerformActions(statementToCheck);
                    }
                    return elseKeywordPresent ? 4 : 2;
                }
                else { ThrowException(11); }

            }
            else { ThrowException(1); }
            return 0;
        }


        private static int ToCommandBase(int index, List<string> commands)
        {
            if (index + 3 < commands.Count)
            {
                List<string> temporaryStringList = new List<string>();
                string toName = commands[index + 1];
                bool endedViaEndKeyword = false;

                for (var i = index + 2; i < commands.Count; i++)
                {
                    string currentStringCommand = commands[i];

                    if (currentStringCommand == "to" || currentStringCommand == toName) { ThrowException(6); temporaryStringList.Clear(); break; }
                    else if (currentStringCommand == "end") { endedViaEndKeyword = true; break; }
                    else { temporaryStringList.Add(currentStringCommand); }
                }

                if (!endedViaEndKeyword) { ThrowException(7); return 0; }
                else
                {
                    if (!toShortcuts.ContainsKey(toName)) { toShortcuts.Add(toName, temporaryStringList); }
                    else { toShortcuts[toName] = temporaryStringList; }
                }

                return temporaryStringList.Count;
            }
            else { ThrowException(1); }

            return 0;
        }


        private static void ForLoop(int index, List<string> commands)
        {
            if (index + 2 < commands.Count)
            {
                if (HasBrackets(commands[index + 1])
                    && HasBrackets(commands[index + 2]))
                {
                    string key;
                    int totalMathLength = 1;
                    int decimalPoints = 0;
                    double start, end, step;
                    List<string> varsAndCommands = new List<string>();


                    SplitTextIntoWords(RemoveBrackets(commands[index + 1]), varsAndCommands);
                    key = varsAndCommands[0];

                    if (!variables.ContainsKey(key)) { variables.Add(key, 0); }

                    start = GetNextNumber(totalMathLength, varsAndCommands, 0);
                    totalMathLength += lengthOfNumericalInput;

                    end = GetNextNumber(totalMathLength, varsAndCommands, 0);
                    totalMathLength += lengthOfNumericalInput;

                    step = varsAndCommands.Count >= totalMathLength + 1 ?
                        GetNextNumber(totalMathLength, varsAndCommands, 0) : Sign(end - start);

                    //Kijk hoeveel getallen achter de komma staan.
                    if (step != (int) step)
                    {
                        string stepStr = step.ToString();
                        decimalPoints = stepStr.Substring(stepStr.LastIndexOf('.') + 1).Length;
                    }

                    if ((end - start) / step > 0 && step != 0)
                    {
                        SplitTextIntoWords(RemoveBrackets(commands[index + 2]), varsAndCommands);
                        for (var i = start; step > 0 ? i <= end : i >= end; i += step)
                        {
                            variables[key] = i;
                            AnalyseAndPerformActions(varsAndCommands);
                            i = Round(i, decimalPoints);
                        }
                    }
                    else
                    {
                        ThrowException(13);
                    }
                }
                else { ThrowException(2); }
            }
            else
            {
                ThrowException(5);
            }
        }


        private static void WhileLoop(int index, List<string> commands)
        {
            if (index + 2 < commands.Count)
            {
                string conditionString = commands[index + 1];
                List<string> commandsToExecute = new List<string>();
                SplitTextIntoWords(RemoveBrackets(commands[index + 2]), commandsToExecute);

                while (EvaluateCondition(conditionString))
                {
                    AnalyseAndPerformActions(commandsToExecute);
                }

            }
            else { ThrowException(5); }
        }


        private static void UpdatePositionVariables()
        {
            variables["x"] = Turtle.X;
            variables["y"] = Turtle.Y;
            variables["angle"] = Turtle.Angle;
        }
        private static void SetTurtlePropertiesToVariables(string key)
        {
            Turtle.X = variables["x"];
            Turtle.Y = variables["y"];
            Turtle.Angle = variables["angle"];
            Turtle.Speed = variables["speed"];
            speedChangedManually = key == "speed" ? true : speedChangedManually;
        }

        private static bool EvaluateCondition(string condition)
        {
            condition = RemoveBrackets(condition);

            if (condition == "true")
            {
                return true;
            }
            else if (condition == "false")
            {
                return false;
            }

            List<string> statementToCheck = new List<string>();
            bool[] operatorsFound = new bool[3];
            double[] valuesToCompare = new double[2];
            string comparisonOperator;

            SplitTextIntoWords(condition, statementToCheck);

            valuesToCompare[0] = GetNextNumber(0, statementToCheck, 0);
            comparisonOperator = statementToCheck[lengthOfNumericalInput];
            valuesToCompare[1] = GetNextNumber(lengthOfNumericalInput + 1, statementToCheck, 0);

            for (var i = 0; i < 3; i++)
            {
                operatorsFound[i] = comparisonOperator.Contains(Convert.ToChar(i + 60).ToString());
            }

            return operatorsFound[valuesToCompare[0].CompareTo(valuesToCompare[1]) + 1];
        }

        private static string ReplaceCommaWithDot(string input) => input.Replace(',', '.');
        private static string RemoveBrackets(string input) => input.Substring(1, input.Length - 2);
        private static bool HasBrackets(string input) => input.StartsWith("[") && input.EndsWith("]");

        private static void ResetTurtle()
        {
            GraphicsWindow.Clear();
            GraphicsWindow.BackgroundColor = "White";
            Turtle.Angle = 0;
            Turtle.X = 320;
            Turtle.Y = 240;
            Turtle.Show();
            Turtle.PenDown();
            UpdatePositionVariables();
            turtleIsHidden = false;
        }

        //private static bool ShouldStopForLoop(double i, double end, double step) => step > 0 ? i <= end + epsilon : i >= end - epsilon;

        private static int GetRandomNumber(int startIndex, List<string> inputList)
        {
            int randomNumberToReturn = 0;
            if (inputList[startIndex].StartsWith("("))
            {

            }
            else
            {
                randomNumberToReturn = numberGenerator.Next(0, (int)GetNextNumber(startIndex, inputList, 0));
                lengthOfNumericalInput++;
            }
            return randomNumberToReturn;
        }

        private static double GetVariable(string key) => variables.ContainsKey(key) ? variables[key] : 0;
        private static int GetCharacterCount(string input, char CharacterToCount)
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

        private static void PrepareEquationForSolving(int indexOfFirstOperator, List<string> resultList, List<string> inputList)
        {
            for (int i = indexOfFirstOperator; i < inputList.Count + 1; i += 2)
            {
                if (i < inputList.Count && operators.Contains(inputList[i]))
                {
                    resultList.Add(inputList[i - 1]);
                    resultList.Add(inputList[i]);
                }
                else
                {
                    resultList.Add(inputList[i - 1]);
                    break;
                }
            }

            // Verander variabelen in de juiste getallen.
            ReplaceVariablesWithValue(resultList);
        }
        private static void ReplaceVariablesWithValue(List<string> inputList)
        {
            for (var i = 0; i < inputList.Count; i += 2)
            {
                if (inputList[i].Contains(":"))
                {
                    string varString = "";
                    for (var charIndex = inputList[i].IndexOf(':'); charIndex < inputList[i].Length && inputList[i][charIndex] != ')'
                    && inputList[i][charIndex] != ')'; charIndex++)
                    {
                        varString += inputList[i][charIndex];
                    }
                    inputList[i] = inputList[i].Replace(varString, ReplaceCommaWithDot(GetVariable(varString.Remove(0, 1)).ToString()));
                }
            }
        }
        private static double SolveEquation(List<string> equationList)
        {
            MathInterpreter mathInterpreter = new MathInterpreter();
            double solution = mathInterpreter.Solve(equationList);
            if (mathInterpreter.ErrorOccurred) { ThrowException(12); }

            return solution;
        }

        private static void PrintHelp()
        {
            string originalForegroundColour = TextWindow.ForegroundColor;
            string helpLine;

            TextWindow.Clear();
            TextWindow.ForegroundColor = "LightBlue";
            TextWindow.WriteLine("Help:");
            TextWindow.ForegroundColor = "Cyan";

            StreamReader file = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Small_Basic_Extension_1.helpText.txt"));
            while ((helpLine = file.ReadLine()) != null) { TextWindow.WriteLine(helpLine); }

            TextWindow.ForegroundColor = originalForegroundColour;
        }


        /// <summary>
        /// Neem de commando's uit een bestand.
        /// </summary>
        /// <param name="path">Exacte locatie van het bestand.</param>
        /// <returns>Niets.</returns>
        public static Primitive DrawFromFile(Primitive path)
        {
            if (System.IO.File.Exists(path) && Path.GetExtension(path) == ".lgo")
            {
                // TODO: Voorkom crash.
                StreamReader file = new StreamReader(path);
                string codeRegel;
                while ((codeRegel = file.ReadLine()) != null) { Draw(codeRegel); }
                file.Close();
            }
            else { ThrowException(3); }

            return null;
        }

        /// <summary>
        /// Open een TextWindow waarin de gebruiker commando's voor de Turtle op kan geven.
        /// </summary>
        /// <returns>Niets.</returns>
        public static Primitive DrawFromTextWindow()
        {
            GraphicsWindow.Title = "Output";
            GraphicsWindow.Top = (Desktop.Height - GraphicsWindow.Height) / 2;
            GraphicsWindow.Left = (Desktop.Width - GraphicsWindow.Width) / 12;

            TextWindow.Title = "Input";
            TextWindow.Top = GraphicsWindow.Top;
            TextWindow.Left = GraphicsWindow.Left + GraphicsWindow.Width + 20;

            while (true) { Draw(TextWindow.Read()); }
        }

    }
}
