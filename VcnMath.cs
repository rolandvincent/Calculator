using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculator
{
    class VcnMath
    {
        //All code created by Vincent
        //Fast Calc
        public struct TextInfo
        {
            public int Start;
            public int StartLength;
            public string StartText;
            public Match StartMatch;
            public int End;
            public int EndLength;
            public string EndText;
            public Match EndMatch;
            public int NeedCount;
            public string Value;
            public int ValueIndex;
            public int ValueEndIndex;
            public string All
            {
                get
                {
                    return StartText + Value + EndText;
                }
            }
        }

        public static Match Currentmatch;
        public static Exception GetLastError
        {
            get
            {
                try
                {
                    if (Currentmatch.Success)
                    {
                        return new Exception("Error in col:" + Currentmatch.Index + ", length:" + Currentmatch.Length);
                    }
                    return new Exception();
                }
                catch
                {
                    return new Exception();
                }
            }
        }

        public static List<string> stepBystep=new List<string>();
        public static string GetResult(string Expression)
        {
            stepBystep.Clear();
            string Res = Result(Expression.Replace(" ", ""));
            return Res;
        }

        public static string Result(string Expression)
        {
            //Sin,Cos,Tan
            //Created By Vincent
            string result = Expression;

        result:
            result = result.Replace("---", "+-").Replace("+--", "+").Replace("×--", "×").Replace("÷--", "÷").Replace("^--", "^");
            if(!stepBystep.Contains("->" + result))
            stepBystep.Add("->" + result);
            MatchCollection sincostanCollection = Regex.Matches(result, @"(?i)(sin|cos|tan|asin|acos|atan|sinh|cosh|tanh|cot|sec|csc|sech|csch|coth|ceiling|trunc|floor|exp|ln|sign|log(?<dot>\d+(\.\d+)?)?|round(?<dotb>\d+)?)\((?i)");
            if (sincostanCollection.Count > 0)
            {
                TextInfo textInfo = GetTextInfo(result, sincostanCollection[0].Index, @"(?i)(sin|cos|tan|asin|acos|atan|sinh|cot|sec|csc|cosh|tanh|sech|csch|coth|ceiling|trunc|floor|exp|ln|sign|log(?<dot>\d+(\.\d+)?)?|round(?<dotb>\d+)?)?\((?i)");
                Currentmatch = textInfo.StartMatch;
                if (textInfo.NeedCount == 0)
                {
                    if (!(FilterCalc(Result(textInfo.Value)) == "..." || FilterCalc(Result(textInfo.Value)).ToLower().Contains("error")))
                    {
                        if (textInfo.StartText.ToLower().Contains("asin"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Asin(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("acos"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Acos(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("atan"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Atan(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("sinh"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Sinh(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("cosh"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Cosh(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("tanh"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Tanh(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("sech"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round(1 / (decimal)Math.Cosh(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("csch"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round(1 / (decimal)Math.Sinh(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("coth"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Cosh(Convert.ToDouble(Result(textInfo.Value))) /(decimal)Math.Sinh(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("sec"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round(1 / (decimal)Math.Cos(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("csc"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round(1 / (decimal)Math.Sin(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("cot"))
                        {
                            if (Math.Tan(Convert.ToDouble(Result(textInfo.Value))) > 280000000000000 || Math.Tan(Convert.ToDouble(Result(textInfo.Value))) < -280000000000000)
                                result = result.Replace(textInfo.All, "0");
                            else
                                result = result.Replace(textInfo.All, "(" + decimal.Round(1/(decimal)Math.Tan(Convert.ToDouble(Result(textInfo.Value))), 7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("sin"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Sin(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("cos"))
                        {
                            result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Cos(Convert.ToDouble(Result(textInfo.Value))),7).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("tan"))
                        {
                            if (Math.Tan(Convert.ToDouble(Result(textInfo.Value)))> 280000000000000 || Math.Tan(Convert.ToDouble(Result(textInfo.Value))) < -280000000000000)
                                result = result.Replace(textInfo.All, "∞");
                            else
                                result = result.Replace(textInfo.All, "(" + decimal.Round((decimal)Math.Tan(Convert.ToDouble(Result(textInfo.Value))),7) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("log"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Log(Convert.ToDouble(Result(textInfo.Value)), textInfo.StartMatch.Groups["dot"].Success ? Convert.ToDouble(textInfo.StartMatch.Groups["dot"].Value) : 10).ToString() + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("ln"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Log(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("exp"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Exp(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("sign"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Sign(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("ceiling"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Ceiling(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("trunc"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Truncate(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("floor"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Floor(Convert.ToDouble(Result(textInfo.Value))) + ")");
                        }
                        else if (textInfo.StartText.ToLower().Contains("round"))
                        {
                            result = result.Replace(textInfo.All, "(" + Math.Round(Convert.ToDouble(Result(textInfo.Value)), textInfo.StartMatch.Groups["dotb"].Success ? Convert.ToInt32(textInfo.StartMatch.Groups["dotb"].Value) : 0).ToString() + ")");
                        }
                        if (!stepBystep.Contains("->" + textInfo.StartText + Expression + textInfo.EndText + "=>" + result))
                            stepBystep.Add("->" + Expression +"=>" + result);
                        goto result;
                    }
                }
            }
            result = Fix(result);
            MatchCollection matchCollection = Regex.Matches(result, @"\((?<ex>((?![\(\)]).)+)\)");
            if (matchCollection.Count > 0)
            {
                Match match = matchCollection[0];
                Currentmatch = match;
                result = result.Substring(0,match.Index) + Calc( match.Groups["ex"].Value) + result.Substring(match.Index + match.Length);
                goto result;
            }
            else
            {
                result = Calc(result);
            }
            return result;
        }

        public static string Fix(string prop)
        {
            TRY:
            MatchCollection exceptStyaxforStart = Regex.Matches(prop, @"(?i)(mod|[×÷\+\-\^\(√]|^)(?i)");
            MatchCollection exceptStyaxforEnd = Regex.Matches(prop, @"(?i)(mod|[\W×÷°√])(?i)");
            MatchCollection openbracketfix = Regex.Matches(prop, @"(?!^)(?<char>\(|∞)");
            if (openbracketfix.Count > 0)
            {
                Currentmatch = openbracketfix[0];
                foreach (Match match in openbracketfix)
                { 
                    if (match.Index >= 0)
                    {
                        if (!PositionHover(match.Index - 1, exceptStyaxforStart, out int StyaxIndex))
                        {
                            prop = prop.Substring(0, match.Index) + "×" + match.Groups["char"].Value + prop.Substring(match.Index + 1);
                            goto TRY;
                        }
                    }
                }
            }
            MatchCollection closebracketfix = Regex.Matches(prop, @"(?<char>\)|∞)");
            if (closebracketfix.Count > 0)
            {
                Currentmatch = closebracketfix[0];
                foreach (Match match in closebracketfix)
                {
                    if (match.Index + match.Length < prop.Length)
                    {
                        if (!PositionHover(match.Index + match.Length, exceptStyaxforEnd, out int StyaxIndex))
                        {
                            prop = prop.Substring(0, match.Index) + match.Groups["char"].Value + "×" + prop.Substring(match.Index + 1);
                            goto TRY;
                        }
                    }
                }
            }
            return prop;
        }

        public static string Calc(string Expression)
        {
            //Faster Result
            //Created By Vincent
            //Include *,-,+,/,^,!,%,e,π,√,°
            string Result = Expression;
        start:
            if (!stepBystep.Contains("-->" + Result))
                stepBystep.Add("-->" + Result);
            MatchCollection fixCollection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))\-(?<b>(\d+\.\d+|\d+|∞))");
            if (fixCollection.Count > 0)
            {
                Match match = fixCollection[0];
                Currentmatch = match;
                Result = Result.Replace(match.Value, match.Groups["a"].Value + "+-" + match.Groups["b"].Value);
                goto start;
            }
            if (Result.Contains("!"))
            {
                MatchCollection fix2Collection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))\!(?<b>(\d+\.\d+|\d+|∞))");
                if (fix2Collection.Count > 0)
                {
                    Match match = fix2Collection[0];
                    Currentmatch = match;
                    Result = Result.Replace(match.Value, match.Groups["a"].Value + "!×" + match.Groups["b"].Value);
                    goto start;
                }
            }
            if (Result.Contains("π"))
            {
                MatchCollection phiCollection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))?π(?<b>(\d+\.\d+|\d+|∞))?");
                if (phiCollection.Count > 0)
                {
                    Match match = phiCollection[0];
                    Currentmatch = match;
                    Result = Result.Substring(0, match.Index) + (match.Groups["a"].Success ? (match.Groups["a"].Value + "×") : string.Empty) + Math.PI + (match.Groups["b"].Success ? "×" + match.Groups["b"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    goto start;
                }
            }
            if (Result.Contains("e"))
            {
                MatchCollection eulerCollection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))?e(?<b>(\d+\.\d+|\d+|∞))?");
                if (eulerCollection.Count > 0)
                {
                    Match match = eulerCollection[0];
                    Currentmatch = match;
                    Result = Result.Substring(0, match.Index) + (match.Groups["a"].Success ? (match.Groups["a"].Value + "×") : string.Empty) + Math.E + (match.Groups["b"].Success ? "×" + match.Groups["b"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    goto start;
                }
            }
            if (Result.Contains("γ"))
            {
                MatchCollection eulerCollection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))?γ(?<b>(\d+\.\d+|\d+|∞))?");
                if (eulerCollection.Count > 0)
                {
                    Match match = eulerCollection[0];
                    Currentmatch = match;
                    Result = Result.Substring(0, match.Index) + (match.Groups["a"].Success ? (match.Groups["a"].Value + "×") : string.Empty) + 0.5772156649 + (match.Groups["b"].Success ? "×" + match.Groups["b"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    goto start;
                }
            }
            if (Result.Contains("φ"))
            {
                MatchCollection eulerCollection = Regex.Matches(Result, @"(?<a>(\d+\.\d+|\d+|∞))?φ(?<b>(\d+\.\d+|\d+|∞))?");
                if (eulerCollection.Count > 0)
                {
                    Match match = eulerCollection[0];
                    Currentmatch = match;
                    Result = Result.Substring(0, match.Index) + (match.Groups["a"].Success ? (match.Groups["a"].Value + "×") : string.Empty) + Convert.ToDecimal( (1+Math.Pow(5,0.5))/2) + (match.Groups["b"].Success ? "×" + match.Groups["b"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    goto start;
                }
            }
            MatchCollection styax1Collection = Regex.Matches(Result, @"(?<dega>\-?(\d+\.\d+|\d+|∞))°(?<degb>\-?(\d+\.\d+|\d+|∞))?|(?<perca>\-?(\d+\.\d+|\d+|∞))\%(?<percb>\-?(\d+\.\d+|\d+|∞))?|(?<fraca>\-?(\d+\.\d+|\d+|∞))\!");
            if (styax1Collection.Count > 0)
            {
                Match match = styax1Collection[0];
                Currentmatch = match;
                if (match.Groups["dega"].Success)
                {
                    if (match.Groups["dega"].Value== "-∞"| match.Groups["dega"].Value == "∞")
                        Result = Result.Replace(match.Value, match.Groups["dega"].Value + (match.Groups["degb"].Success ? "×" + match.Groups["degb"].Value : string.Empty));
                    else
                        Result = Result.Replace(match.Value, getRadian(Convert.ToDouble(match.Groups["dega"].Value)) + (match.Groups["degb"].Success ? "×" + match.Groups["degb"].Value : string.Empty));


                    goto start;
                }
                else if (match.Groups["perca"].Success)
                {
                    if (match.Groups["perca"].Value == "-∞" | match.Groups["perca"].Value == "∞")
                        Result = Result.Substring(0, match.Index) + match.Groups["dega"].Value + (match.Groups["percb"].Success ? "×" + match.Groups["percb"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    else
                        Result = Result.Substring(0, match.Index) + Convert.ToDecimal(match.Groups["perca"].Value) / 100 + (match.Groups["percb"].Success ? "×" + match.Groups["percb"].Value : string.Empty) + Result.Substring(match.Index + match.Length);
                    goto start;
                }
                else if (match.Groups["fraca"].Success)
                {
                    if (match.Groups["fraca"].Value == "-∞" | match.Groups["fraca"].Value == "∞")
                    {
                        if (match.Groups["fraca"].Value.Contains("-")) return "NaN";
                        Result = Result.Replace(match.Value, "∞");
                    }
                    else
                    {
                        if (Convert.ToDecimal(match.Groups["fraca"].Value) < 0) return "NaN";
                        if (match.Groups["fraca"].Value.Contains(".")) return "NaN";
                        decimal H = 1;
                        for (int i = 1; i <= Convert.ToInt32(match.Groups["fraca"].Value); i++)
                        {
                            H *= i;
                        }
                        Result = Result.Replace(match.Value, H.ToString());
                    }
                    goto start;
                }
            }
            if (Result.Contains("√"))
            {
                MatchCollection rootCollection = Regex.Matches(Result, @"(?<a>\-?(\d+\.\d+|\d+|∞))?√(?<b>\-?(\d+\.\d+|\d+|∞))");
                if (rootCollection.Count > 0)
                {
                    Match match = rootCollection[0];
                    Currentmatch = match;
                    Result = Result.Replace(match.Value, Math.Pow(Convert.ToDouble(match.Groups["b"].Value), 1 / (match.Groups["a"].Success ? Convert.ToDouble(match.Groups["a"].Value) : 2)).ToString());
                    if (Result.ToLower().Contains("nan")) return "NaN";
                    if (Math.Pow(Convert.ToDouble(match.Groups["b"].Value), 1 / (match.Groups["a"].Success ? Convert.ToDouble(match.Groups["a"].Value) : 2)).ToString().ToLower().Contains("e+")) return Result;
                    goto start;
                }
            }
            if (Result.Contains("^"))
            {
                MatchCollection powerCollection = Regex.Matches(Result, @"(?<a>\-?(\d+\.\d+|\d+|∞))\^(?<b>\-?(\d+\.\d+|\d+|∞))");
                if (powerCollection.Count > 0)
                {
                    Match match = powerCollection[0];
                    Currentmatch = match;
                    Result = Result.Replace(match.Value, Math.Pow(Convert.ToDouble(match.Groups["a"].Value), Convert.ToDouble(match.Groups["b"].Value)).ToString());
                    if (Math.Pow(Convert.ToDouble(match.Groups["a"].Value), Convert.ToDouble(match.Groups["b"].Value)).ToString().ToLower().Contains("e+"))
                    {
                        return Result;
                    }
                    goto start;
                }
            }
            MatchCollection timesdevideCollection = Regex.Matches(Result, @"(?<ax>\-?(\d+\.\d+|\d+|∞))×(?<bx>\-?(\d+\.\d+|\d+|∞))|(?<ad>\-?(\d+\.\d+|\d+|∞))÷(?<bd>\-?(\d+\.\d+|\d+|∞))|(?<ao>\-?(\d+\.\d+|\d+|∞))mod(?<bo>\-?(\d+\.\d+|\d+|∞))");
            if (timesdevideCollection.Count > 0)
            {
                Match match = timesdevideCollection[0];
                Currentmatch = match;
                if (match.Groups["ax"].Success)
                {
                    bool zeroRes = match.Groups["ax"].Value == "0" || match.Groups["bx"].Value == "0" || match.Groups["ax"].Value == "-0" || match.Groups["bx"].Value == "-0";
                    bool firstInf = match.Groups["ax"].Value.Contains("∞");
                    bool secInf = match.Groups["bx"].Value.Contains("∞");
                    if (zeroRes)
                    {
                        Result = Result.Replace(match.Value, "0");
                    }
                    else if (firstInf || secInf)
                    {
                        bool firstNeg = match.Groups["ax"].Value.Contains("-");
                        bool secNeg = match.Groups["bx"].Value.Contains("-");
                        bool negRes = (firstNeg && !secNeg) || (!firstNeg && secNeg);
                        Result = Result.Replace(match.Value, (negRes ? "-" : string.Empty) + "∞");
                    }
                    else
                    {
                        Result = Result.Replace(match.Value, (Convert.ToDecimal(match.Groups["ax"].Value) * Convert.ToDecimal(match.Groups["bx"].Value)).ToString());
                    }
                }
                else if(match.Groups["ao"].Success)
                {
                    bool firstNeg = match.Groups["ao"].Value.Contains("-");
                    bool secNeg = match.Groups["bo"].Value.Contains("-");
                    bool negRes = (firstNeg && !secNeg) || (!firstNeg && secNeg);
                    bool firstZero = match.Groups["ao"].Value == "0" || match.Groups["ao"].Value == "-0";
                    bool secZero = match.Groups["bo"].Value == "0" || match.Groups["bo"].Value == "-0";
                    bool zeroRes = match.Groups["ao"].Value == "0" || match.Groups["bo"].Value == "0" || match.Groups["ao"].Value == "-0" || match.Groups["bo"].Value == "-0";
                    bool firstInf = match.Groups["ao"].Value.Contains("∞");
                    bool secInf = match.Groups["bo"].Value.Contains("∞");
                    if (!firstInf && secInf && !firstZero)
                    {
                        Result = Result.Replace(match.Value,"0");
                    }
                    else if ((firstInf && secInf)||(firstZero && secZero))
                    {
                        return "NaN";
                    }
                    else if (firstInf  && !secInf && !secZero)
                    {
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    }
                    else if (firstInf && secZero)
                    {
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    }
                    else if (firstZero && secInf)
                    {
                        Result = Result.Replace(match.Value, "0");
                    }
                    else
                    {
                        Result = Result.Replace(match.Value, (Convert.ToDecimal(match.Groups["ao"].Value) % Convert.ToDecimal(match.Groups["bo"].Value)).ToString());
                    }
                }
                else
                {
                    bool firstNeg = match.Groups["ad"].Value.Contains("-");
                    bool secNeg = match.Groups["bd"].Value.Contains("-");
                    bool negRes = (firstNeg && !secNeg) || (!firstNeg && secNeg);
                    bool firstZero = match.Groups["ad"].Value == "0" || match.Groups["ad"].Value == "-0";
                    bool secZero = match.Groups["bd"].Value == "0" || match.Groups["bd"].Value == "-0";
                    bool firstInf = match.Groups["ad"].Value.Contains("∞");
                    bool secInf = match.Groups["bd"].Value.Contains("∞");
                    if (firstZero && secZero)
                        return "NaN";
                    else if (!firstZero && !firstInf && secInf)
                        Result = Result.Replace(match.Value, "0");
                    else if (firstZero && secInf)
                        Result = Result.Replace(match.Value, "0");
                    else if (firstInf && secInf)
                        return "NaN";
                    else if (firstInf && secZero)
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else if (firstInf && !secInf && !secZero)
                        Result = Result.Replace(match.Value, (negRes ? "-" : string.Empty) + "∞");
                    else if (!firstInf && !firstZero && secZero)
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else
                    {
                        Result = Result.Replace(match.Value, (Convert.ToDecimal(match.Groups["ad"].Value) / Convert.ToDecimal(match.Groups["bd"].Value)).ToString());
                    }
                }
                goto start;
            }
            MatchCollection minusCollection = Regex.Matches(Result, @"(?<ap>\-?(\d+\.\d+|\d+|∞))\+(?<bp>\-?(\d+\.\d+|\d+|∞))|(?<am>\-?(\d+\.\d+|\d+|∞))\-(?<bm>\-?(\d+\.\d+|\d+|∞))");
            if (minusCollection.Count > 0)
            {
                Match match = minusCollection[0];
                Currentmatch = match;
                if (match.Groups["am"].Success)
                {
                    //Gw buat capek - capek teryata gak kepake cuma buat ∞--∞ doang... (-_-)!
                    bool firstNeg = match.Groups["am"].Value.Contains("-");
                    bool secNeg = match.Groups["bm"].Value.Contains("-");
                    bool negRes = (firstNeg && !secNeg) || (!firstNeg && secNeg);
                    bool firstInf = match.Groups["am"].Value.Contains("∞");
                    bool secInf = match.Groups["bm"].Value.Contains("∞");
                    if (((firstNeg && secNeg) || (!firstNeg && !secNeg)) && firstInf && secInf) //-∞ - -∞ / ∞ - ∞
                        return "NaN";
                    else if (firstInf && !secInf && firstNeg) //-∞ - 1
                        Result = Result.Replace(match.Value, (secNeg ? string.Empty : "-") + "∞");
                    else if (firstInf && !secInf && secNeg) //-∞ - -1
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else if (!firstInf && secInf && firstNeg) //-1 - ∞
                        Result = Result.Replace(match.Value, (secNeg ? string.Empty : "-") + "∞");
                    else if (!firstInf && secInf && secNeg) //1 - -∞
                        Result = Result.Replace(match.Value, "∞");
                    else if (firstInf && secInf && negRes) //1 - -∞
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else
                        Result = Result.Replace(match.Value, (Convert.ToDecimal(match.Groups["am"].Value) - Convert.ToDecimal(match.Groups["bm"].Value)).ToString());
                }
                else
                {
                    bool firstNeg = match.Groups["ap"].Value.Contains("-");
                    bool secNeg = match.Groups["bp"].Value.Contains("-");
                    bool negRes = (firstNeg && !secNeg) || (!firstNeg && secNeg);
                    bool firstInf = match.Groups["ap"].Value.Contains("∞");
                    bool secInf = match.Groups["bp"].Value.Contains("∞");
                    if (((firstNeg && secNeg) || (!firstNeg && !secNeg)) && firstInf && secInf) //-∞ + -∞ / ∞ + ∞
                        Result = Result.Replace(match.Value, (secNeg ? "-" : string.Empty) + "∞");
                    else if (firstInf && !secInf && firstNeg) //-∞ +-1
                        Result = Result.Replace(match.Value, "-∞");
                    else if (firstInf && !secInf && secNeg) //+-∞ + -1
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else if (!firstInf && secInf && firstNeg) //-1 + +-∞
                        Result = Result.Replace(match.Value, (secNeg ? "-" : string.Empty) + "∞");
                    else if (!firstInf && secInf && secNeg) //+-1 + -∞
                        Result = Result.Replace(match.Value, "-∞");
                    else if (firstInf && secInf && negRes) //+-1 + -∞
                        return "NaN";
                    else if (!firstInf && secInf && !secNeg) //+-1 + ∞
                        Result = Result.Replace(match.Value, (secNeg ? "-" : string.Empty) + "∞");
                    else if (firstInf && !secInf && !firstNeg) //+-∞ + +-1
                        Result = Result.Replace(match.Value, (firstNeg ? "-" : string.Empty) + "∞");
                    else
                        Result = Result.Replace(match.Value, (Convert.ToDecimal(match.Groups["ap"].Value) + Convert.ToDecimal(match.Groups["bp"].Value)).ToString());
                }
                goto start;
            }

            return Result;
        }

        public static double getRadian(double angle)
        {
            return Math.PI * angle / 180;
        }

        public static double getAngle(double rad)
        {
            return 180 * rad / Math.PI ;
        }

        public static TextInfo GetTextInfo(string Text, int CurrentIndex, string RegexStartString = @"\(", string RegexEndString = @"\)")
        {
            if (!(CurrentIndex >= 0 && CurrentIndex < Text.Length)) throw new Exception("Index must between 0 and " + (Text.Length - 1)+".");
            if (RegexStartString == RegexEndString ) throw new Exception("RegexStartString and RegexEndString cannot be same!");
            TextInfo textInfo = new TextInfo();
            MatchCollection matchCollection = Regex.Matches(Text, RegexStartString + "|" + RegexEndString);
            if (PositionHover(CurrentIndex, matchCollection, out int Index))
            {
                if (Regex.IsMatch(matchCollection[Index].Value, RegexStartString))
                {
                    textInfo.Start = matchCollection[Index].Index;
                    textInfo.StartLength = matchCollection[Index].Length;
                    textInfo.StartText = matchCollection[Index].Value;
                    textInfo.StartMatch = matchCollection[Index];
                    int Level = 0;
                    for (int i = Index; i < matchCollection.Count; i++)
                    {
                        if (Regex.IsMatch(matchCollection[i].Value, RegexStartString))
                        {
                            Level++;
                            textInfo.NeedCount = Level;
                        }
                        else if (Regex.IsMatch(matchCollection[i].Value, RegexEndString))
                        {
                            Level--;
                            textInfo.NeedCount = Level;
                            if (Level == 0)
                            {
                                textInfo.End = matchCollection[i].Index;
                                textInfo.EndLength = matchCollection[i].Length;
                                textInfo.EndText = matchCollection[i].Value;
                                textInfo.Value = Text.Substring(textInfo.Start + textInfo.StartLength, textInfo.End - textInfo.Start - textInfo.StartLength);
                                textInfo.ValueIndex = textInfo.Start + textInfo.StartLength;
                                textInfo.ValueEndIndex = textInfo.End - textInfo.Start - textInfo.StartLength;
                                textInfo.EndMatch = matchCollection[i];
                                return textInfo;
                            }
                        }
                    }
                }
                else
                {
                    textInfo.End = matchCollection[Index].Index;
                    textInfo.EndLength = matchCollection[Index].Length;
                    textInfo.EndText = matchCollection[Index].Value;
                    textInfo.EndMatch = matchCollection[Index];
                    int Level = 0;
                    for (int i = Index ; i >= 0; i--)
                    {
                        if (Regex.IsMatch(matchCollection[i].Value, RegexStartString))
                        {
                            Level++;
                            textInfo.NeedCount = Level;
                            if (Level == 0)
                            {
                                textInfo.Start = matchCollection[i].Index;
                                textInfo.StartLength = matchCollection[i].Length;
                                textInfo.StartText = matchCollection[i].Value;
                                textInfo.Value = Text.Substring(textInfo.Start + textInfo.StartLength, textInfo.End - textInfo.Start - textInfo.StartLength);
                                textInfo.ValueIndex = textInfo.Start + textInfo.StartLength;
                                textInfo.ValueEndIndex = textInfo.End - textInfo.Start - textInfo.StartLength;
                                textInfo.StartMatch = matchCollection[i];
                                return textInfo;
                            }
                        }
                        else if (Regex.IsMatch(matchCollection[i].Value, RegexEndString))
                        {
                            Level--;
                            textInfo.NeedCount = Level;
                        }
                    }
                }
                
            }
            return textInfo;
        }

        public static bool PositionHover(int Index, MatchCollection MatchCollectionData, out int MatchIndex)
        {
            MatchIndex = -1;
            for (int i = 0; i < MatchCollectionData.Count;i++)
            {
                if (MatchCollectionData[i].Index <= Index && (MatchCollectionData[i].Index + MatchCollectionData[i].Length) > Index)
                {
                    MatchIndex = i;
                    return true;
                }
            }
            return false;
        }

        public static List<TextInfo> WhereIsMe(string Text, int CurrentIndex, string RegexStartString = @"\(", string RegexEndString = @"\)")
        {
            List<TextInfo> list = new List<TextInfo>();
            MatchCollection matchCollection = Regex.Matches(Text, RegexStartString + "|" + RegexEndString);
            for (int i = 0; i< matchCollection.Count; i++)
            {
                TextInfo matches = GetTextInfo(Text, matchCollection[i].Index, RegexStartString, RegexEndString);
                if (matches.NeedCount == 0)
                {
                    if (CurrentIndex >= matches.Start + matches.StartLength && CurrentIndex < matches.ValueIndex + matches.Value.Length + 1)
                    {
                        list.Add(matches);
                    }
                }
            }
            return list;
        }

        public static TextInfo GetRootListTextInfo(List<TextInfo> ListOfTxtInfo)
        {
            TextInfo textInfo = new TextInfo();
            int Index = 0;
            foreach (TextInfo info in ListOfTxtInfo)
            {
                if(Index > info.Start)
                {
                    return textInfo;
                }
                else
                {
                    Index = info.Start;
                    textInfo = info;
                }
            }
            return textInfo;
        }

        public static string FilterCalc(string txt)
        {
            if (txt.ToLower().Contains("nan")) return "NaN";
            if (Regex.IsMatch(txt, @"^\-?(\d+\.\d+(E|e)\+\d+|\d+\.\d+|\d+)$") || Regex.IsMatch(txt, @"^\-?∞$"))
                return txt.Replace(" ",""); 
            else return "...";
        }
    }
}
