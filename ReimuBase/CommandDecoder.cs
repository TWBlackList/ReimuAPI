using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReimuAPI.ReimuBase
{
    public class DecodeException : Exception
    {
    }

    public class CommandDecoder
    {
        public static Dictionary<string, string> cutKeyIsValue(string message)
        {
            var values = new Dictionary<string, string>();
            var CommandSpace = message.IndexOf(" ");
            var key = "";
            var value = "";
            var finalKey = 0;
            var process = 0;
            var started = false;
            var haveSpace = false;
            var finishKey = false;
            var allFinish = false;
            var strLength = message.Length;
            for (var i = 0; i < strLength; i++)
            {
                if (i < finalKey) continue;
                if (finishKey)
                {
                    process = 1;
                    started = false;
                    haveSpace = false;
                    finishKey = false;
                }

                if (allFinish)
                {
                    process = 0;
                    values.Add(
                        key.Replace("\\\\", "\\").Replace("\\n", "\n").Replace("\\\"", "\""),
                        value.Replace("\\\\", "\\").Replace("\\n", "\n").Replace("\\\"", "\"")
                    );
                    key = "";
                    value = "";
                    started = false;
                    haveSpace = false;
                    allFinish = false;
                }

                var ch = message[i] + "";
                if (process == 0)
                {
                    if (haveSpace == false && ch == "=")
                    {
                        finalKey = i + 1;
                        finishKey = true;
                        continue;
                    }

                    if (ch == "\"")
                    {
                        if (started == false)
                        {
                            finalKey = i + 1;
                            haveSpace = true;
                            started = true;
                            continue;
                        }

                        if (haveSpace)
                            if (i + 2 <= strLength && message[i + 1] + "" == "=")
                            {
                                if (message[i - 1] + "" == "\\")
                                {
                                    finalKey = i + 2;
                                    key += "\\\"=";
                                    continue;
                                }

                                finalKey = i + 2;
                                finishKey = true;
                                continue;
                            }
                            else if (message[i - 1] + "" != "\\")
                            {
                                throw new DecodeException();
                            }
                    }

                    if (haveSpace == false)
                        if (!Regex.IsMatch(ch, @"^[A-Za-z0-9]+$"))
                            throw new DecodeException();
                    started = true;
                    key += ch;
                }
                else if (process == 1)
                {
                    if (haveSpace == false && ch == " ")
                    {
                        finalKey = i + 1;
                        allFinish = true;
                        continue;
                    }

                    if (ch == "\"")
                    {
                        if (started == false)
                        {
                            finalKey = i + 1;
                            haveSpace = true;
                            started = true;
                            continue;
                        }

                        if (i + 2 <= strLength && haveSpace)
                        {
                            var space = message[i + 1] + "";
                            if (space == " ")
                            {
                                if (message[i - 1] + "" == "\\")
                                {
                                    finalKey = i + 2;
                                    value += "\\\" ";
                                    continue;
                                }

                                finalKey = i + 2;
                                allFinish = true;
                                continue;
                            }

                            if (message[i - 1] + "" != "\\")
                            {
                                throw new DecodeException();
                            }
                        }
                        else if (i == strLength - 1)
                        {
                            continue;
                        }

                        if (haveSpace == false)
                            if (Regex.IsMatch(ch, @"^[A-Za-z0-9]+$"))
                                throw new DecodeException();
                    }

                    started = true;
                    value += ch;
                }
            }

            values.Add(
                key.Replace("\\\\", "\\").Replace("\\n", "\n").Replace("\\\"", "\""),
                value.Replace("\\\\", "\\").Replace("\\n", "\n").Replace("\\\"", "\"")
            );
            return values;
        }
    }
}