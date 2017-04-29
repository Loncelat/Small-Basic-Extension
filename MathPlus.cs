using System;
using static System.Math;

using Microsoft.SmallBasic.Library;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Small_Basic_Extension_1
{
    /// <summary>
    /// De MathPlus-bibliotheek voegt meer wiskundige functies toe.
    /// </summary>
    [SmallBasicType]
    public static class MathPlus
    {
        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de cosinus hyperbolicus waarde.
        /// </summary>
        /// <param name="coshValue">De cosinus hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven cosinus hyperbolicus waarde.</returns>
        public static Primitive ArcCosh(Primitive coshValue) => Log(coshValue + Sqrt(coshValue - 1) * Sqrt(coshValue + 1));

        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de cotangens hyperbolicus waarde.
        /// </summary>
        /// <param name="cothValue">De cotangens hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven cotangens hyperbolicus waarde.</returns>
        public static Primitive ArcCoth(Primitive cothValue) => 0.5 * (Log(1 + 1 / cothValue) - Log(1 - 1 / cothValue));

        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de cosecans hyperbolicus waarde.
        /// </summary>
        /// <param name="cschValue">De cosecans hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven cosecans hyperbolicus waarde.</returns>
        public static Primitive ArcCsch(Primitive cschValue) => Log(Sqrt(1 + (1 / Pow(cschValue, 2))) + (1 / cschValue));

        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de secans hyperbolicus waarde.
        /// </summary>
        /// <param name="sechValue">De secans hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven secans hyperbolicus waarde.</returns>
        public static Primitive ArcSech(Primitive sechValue) => Log(Sqrt((1 / sechValue) - 1) * Sqrt(1 + (1 / sechValue)) + 1 / sechValue);

        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de sinus hyperbolicus waarde.
        /// </summary>
        /// <param name="sinhValue">De sinus hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven sinus hyperbolicus waarde.</returns>
        public static Primitive ArcSinh(Primitive sinhValue) => Log(sinhValue + Sqrt(1 + Pow(sinhValue, 2)));

        /// <summary>
        /// Krijgt de hoek in radialen, gegeven de tangens hyperbolicus waarde.
        /// </summary>
        /// <param name="tanhValue">De tangens hyperbolicus waarde waarvan de hoek moet worden verkregen.</param>
        /// <returns>De hoek (in radialen) voor de gegeven tangens hyperbolicus waarde.</returns>
        public static Primitive ArcTanh(Primitive tanhValue) => 0.5 * (Log(1 + tanhValue) - Log(1 - tanhValue));

        /// <summary>
        /// Berekent de hoek tussen de x-as en het opgegeven punt.
        /// </summary>
        /// <param name="y">Het y-coördinaat.</param>
        /// <param name="x">Het x-coördinaat.</param>
        /// <returns>De hoek tussen te x-as en het opgegeven punt.</returns>
        public static Primitive ArcTan2(Primitive y, Primitive x) => Atan2(y, x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trials">Hoe vaak het kansexperiment herhaald wordt.</param>
        /// <param name="p">De kans. (0-1)</param>
        /// <param name="n">Hoe vaak de kans uit moet komen.</param>
        /// <returns>De kans op hoogstens n. (0-1)</returns>
        public static Primitive BinomCdf(Primitive trials, Primitive p, Primitive n)
        {
            Primitive p2 = 1 - p;
            if (trials > n && trials > 0 && n >= 0 && p >= 0 && p <= 1)
            {
                Primitive chanceSum = Pow(p2, trials);
                for (int i = 1; i <= n; i++)
                {
                    chanceSum += Combination(trials, i) * Pow(p, i) * Pow(p2, trials - i);
                }
                return chanceSum;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trials"></param>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Primitive BinomPdf(Primitive trials, Primitive p, Primitive n)
        {
            if (trials > n && trials > 0 && n >= 0 && p >= 0 && p <= 1)
            {
                if (n == 0) { return Pow(1 - p, trials); }
                else if (n == 1) { return trials * p * Pow(1 - p, trials - 1); }
                return Combination(trials, n) * Pow(p, n) * Pow(1 - p, trials - n);
            }
            return 0;
        }
        /// <summary>
        /// Kiest k uit n, waarbij de volgorde niet uitmaakt.
        /// </summary>
        /// <param name="n">Het aantal dingen waaruit gekozen kan worden.</param>
        /// <param name="r">Het te kiezen aantal.</param>
        /// <returns>Het aantal combinaties van n over k.</returns>
        public static Primitive Combination(Primitive n, Primitive r)
        {
            if (IsInteger(n) && IsInteger(r))
            {
                return (r <= n) ? Factorial(n) / (Factorial(r) * Factorial(n - r)) : (Primitive)0;
            }
            else { return 0; }
        }

        /// <summary>
        /// Berekent de cosinus van een hoek in graden.
        /// </summary>
        /// <param name="Angle">De hoek in graden.</param>
        /// <returns>De cosinus van de hoek.</returns>
        public static Primitive Cos(Primitive Angle) => System.Math.Cos(PI * Angle / 180.0);

        /// <summary>
        /// Berekent de cosinus hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de cosinus hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De cosinus hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Cosh(Primitive Angle) => (Exp(Angle) + Exp(-Angle)) / 2;

        /// <summary>
        /// Berekent de cotangens hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de cotangens hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De cotangens hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Coth(Primitive Angle) => Cosh(Angle) / Sinh(Angle);

        /// <summary>
        /// Berekent de cosecans hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de cosecans hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De cosecans hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Csch(Primitive Angle) => 1 / Sinh(Angle);

        /// <summary>
        /// Berekent de lengte van de vector met de kentallen x en y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Primitive Dist(Primitive x, Primitive y) => Sqrt(x * x + y * y);

        /// <summary>
        /// Geeft de waarde van het getal e.
        /// </summary>
        public static Primitive E => E;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Primitive Erf(Primitive z)
        {
            if (IsNumeric(z))
            {
                if (z < 0)
                {
                    return -Erf(-z);
                }

                //Maak variabelen.
                double x = z;
                double[] a = { 0.278393d, 0.230389d, 0.000972d, 0.078108d };

                //Berekening van de errorfunctie.
                double denom = Pow(1 + x *
                    (a[0] + x *
                    (a[1] + x *
                    (a[2] + x * a[3]))), 4);
                return 1 - (1 / denom);
            }
            return 0;
        }

        /// <summary>
        /// Verheft e tot de opgegeven macht.
        /// </summary>
        /// <param name="exponent">De exponent waarmee e moet worden verheven.</param>
        /// <returns>De uitkomst van de machtsverheffing van e tot de opgegeven macht.</returns>
        public static Primitive Exp(Primitive exponent) => Exp(exponent);

        /// <summary>
        /// Berekent de faculteit van het opgegeven getal.
        /// </summary>
        /// <param name="number">Een positief, heel getal.</param>
        /// <returns>De faculteit van het getal.</returns>
        public static Primitive Factorial(Primitive number)
        {
            try
            {
                //Nul.
                if (number == 0)
                {
                    return 1;
                }

                //Heel getal groter dan nul.
                else if (IsInteger(number) && number > 0)
                {
                    double result = 1;

                    //Loop de getallen af tot 1.
                    for (int i = number; i > 1; i--)
                    {
                        //Vermenigvuldig het totaal met het volgende getal.
                        result *= i;
                    }

                    return result;
                }

                //Kommagetal.
                else
                {
                    return !IsInteger(number) ? Gamma(number + 1) : (Primitive)0;
                }

            }

            catch (OverflowException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Neemt het gedeelte na de komma van het opgegeven getal.
        /// </summary>
        /// <param name="number">Het te bewerken getal.</param>
        /// <returns>Het gedeelte na de komma van het opgegeven getal.</returns>
        public static Primitive FloatPart(Primitive number) => number - System.Math.Truncate((decimal)number);

        /// <summary>
        /// De gammafunctie.
        /// </summary>
        /// <param name="z">Het getal waarvan de gammawaarde verkregen moet worden.</param>
        /// <returns>Ongeveer de gammawaarde van het getal.</returns>
        public static Primitive Gamma(Primitive z)
        {
            //Lanczos Approximatie.
            try
            {
                double[] pn = { 1.000000000190015d, 76.18009172947146d, -86.50532032941677d, 24.01409824083091d, -1.231739572450155d,
            0.001208650973866179d, -0.000005395239384953d };


                double rootTwoPi = 2.506628274631000502415765284811d / z;
                double ePow = System.Math.Exp(-(z + 5.5));
                double zPow = Pow(z + 5.5, z + 0.5);

                double pSum = pn[0];

                for (var i = 1; i <= 6; i++)
                {
                    pSum += pn[i] / (z + i);
                }

                return rootTwoPi * pSum * zPow * ePow;

            }

            catch (OverflowException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Berekent de grootste gemene deler van de opgegeven getallen.
        /// </summary>
        /// <param name="a">Het eerste getal.</param>
        /// <param name="b">Het tweede getal.</param>
        /// <returns>De grootste gemene deler van de opgegeven getallen.</returns>
        public static Primitive Gcd(Primitive a, Primitive b)
        {
            return GcdCalc(Abs(a), Abs(b));
        }

        /// <summary>
        /// Genereert een willekeurig getal tussen 0 en 1.
        /// </summary>
        /// <returns>Een willekeurig getal tussen 0 en 1.</returns>
        public static Primitive GetRandomFloat()
        {
            Random rnd = new Random();
            return rnd.NextDouble();
        }

        /// <summary>
        /// De gulden snede.
        /// </summary>
        public static Primitive GoldenRatio => (1 + Sqrt(5)) / 2;

        /// <summary>
        /// Neemt het gedeelte voor de komma van het opgegeven getal.
        /// </summary>
        /// <param name="number">Het te bewerken getal.</param>
        /// <returns>Het gedeelte voor de komma van het opgegeven getal.</returns>
        public static Primitive IntPart(Primitive number) => System.Math.Truncate((double)number);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Primitive IsInteger(Primitive number) => number == (int)number;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Primitive IsNumeric(Primitive str) => float.TryParse(str, out float x);

        /// <summary>
        /// Berekent de kleinste gemene veelvoud van de opgegeven getallen.
        /// </summary>
        /// <param name="a">Het eerste getal.</param>
        /// <param name="b">het tweede getal.</param>
        /// <returns>De kleinste gemene veelvoud van de opgegeven getallen.</returns>
        public static Primitive Lcm(Primitive a, Primitive b) => Abs(a * b) / Gcd(a, b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">Het getal waarvan de logaritme moet worden berekend.</param>
        /// <param name="base"></param>
        /// <returns>De logaritmische waarde van het gegeven getal.</returns>
        public static Primitive LogBASE(Primitive number, Primitive @base) => Log10(number) / Log10(@base);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="n"></param>
        /// <returns>De nde wortel van het getal.</returns>
        public static Primitive NthRoot(Primitive number, Primitive n) => Pow(number, (1 / n));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">Het aantal dingen waaruit gekozen kan worden.</param>
        /// <param name="r">Het te kiezen aantal.</param>
        /// <returns></returns>
        public static Primitive Permutation(Primitive n, Primitive r)
        {
            if (IsInteger(n) && IsInteger(r))
            {
                return (r <= n) ? Factorial(n) / Factorial(n - r) : (Primitive)0;
            }
            else { return "ERROR"; }
        }

        /// <summary>
        /// Rondt een getal af tot n getallen achter de komma.
        /// </summary>
        /// <param name="number">Het af te ronden getal.</param>
        /// <param name="n">Het aantal getallen achter de komma.</param>
        /// <returns>Het getal afgerond tot n getallen achter de komma.</returns>
        public static Primitive RoundTo(Primitive number, Primitive n) => Round((double)number, n);

        /// <summary>
        /// Berekent de secans hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de secans hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De secans hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Sech(Primitive Angle) => 1 / Cosh(Angle);

        /// <summary>
        /// De sinus van een hoek in graden.
        /// </summary>
        /// <param name="Angle">De hoek in graden.</param>
        /// <returns>De sinus van de hoek.</returns>
        public static Primitive Sin(Primitive Angle) => System.Math.Sin(PI * Angle / 180.0);

        /// <summary>
        /// Berekent de sinus hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de sinus hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De sinus hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Sinh(Primitive Angle) => (Exp(Angle) - Exp(-Angle)) / 2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Primitive SolveMath(Primitive str)
        {
            MathInterpreter m = new MathInterpreter();
            double output = m.Solve(new List<string>(str.ToString().Split(' ')));

            if (m.ErrorOccurred)
            {
                throw new FormatException("De opgegeven string is niet op te lossen.");
            }

            return output;
        }

        /// <summary>
        /// De tangens van een hoek in graden.
        /// </summary>
        /// <param name="Angle">De hoek in graden.</param>
        /// <returns>De tangens van de hoek.</returns>
        public static Primitive Tan(Primitive Angle) => Tan(PI * Angle / 180.0);

        /// <summary>
        /// Berekent de tangens hyperbolicus van een gegeven hoek in radialen.
        /// </summary>
        /// <param name="Angle">De hoek waarvan de tangens hyperbolicus moet worden berekend (in radialen).</param>
        /// <returns>De tangens hyperbolicus van de gegeven hoek.</returns>
        public static Primitive Tanh(Primitive Angle) => Sinh(Angle) / Cosh(Angle);

        /// <summary>
        /// Neemt het gedeelte voor de komma van het opgegeven getal.
        /// </summary>
        /// <param name="a">Het te bewerken getal.</param>
        /// <returns>Het gedeelte voor de komma van het opgegeven getal.</returns>
        public static Primitive Truncate(Primitive a) => Truncate(a);

        private static int GcdCalc(int a, int b)
        {
            if (b == 0) { return a; }
            return GcdCalc(b, a % b);
        }
    }
}
