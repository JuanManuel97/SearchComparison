using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace SearchComparison.Util
{
    public static class CommonMethods
    {
        public static T MaxValue<T>(this IEnumerable<T> source, Func<T, long> func)
        {
            if (source == null)
                throw new Exception();

            using (var en = source.GetEnumerator())
            {
                if (!en.MoveNext())
                    throw new Exception();

                long max = func(en.Current);
                T maxValue = en.Current;

                while (en.MoveNext())
                {
                    var possible = func(en.Current);

                    if (max < possible)
                    {
                        max = possible;
                        maxValue = en.Current;
                    }
                }
                return maxValue;
            }
        }

        public static T DeserializeJson<T>(this string json)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Deserialize<T>(json);
        }

        public static List<string> GenerateInputList(string input)
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            List<string> inputList = new List<string>();

            if (input.IndexOf('"') > -1)
            {
                List<string> firstSplit = input.Split('"').ToList();
                foreach (var item in firstSplit)
                {
                    if (!String.IsNullOrEmpty(item.Trim()))
                    {
                        if (item.StartsWith(" ") || item.EndsWith(" "))
                        {
                            string newItem = item.Trim();

                            if (newItem.Split(' ').Length > 1)
                                Array.ForEach(newItem.Split(' '), x => inputList.Add(x));
                            else
                                inputList.Add(newItem);
                        }
                        else
                        {
                            inputList.Add(item);
                        }
                    }

                }
            }
            else
            {
                Array.ForEach(input.Split(' '), x => inputList.Add(x));
            }

            return inputList.Distinct().ToList();
        }
    }
}
