using SearchComparison.BL;
using SearchComparison.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SearchComparison
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("================ Starting Search Engine Comparator ================" +
                        "\n\nPlease enter TWO OR MORE terms to start the comparison. " +
                        "\nIf the term you want to write includes two(or more) words, surround them with quotation marks. " +
                        "\nTwo equal terms will be understood as one");
            List<string> lsElements = new List<string>();

            try
            {
                if (args.Length == 0)
                {                    
                    lsElements = CommonMethods.GenerateInputList(Console.ReadLine());
                }
                else
                {
                    if (args.Length > 1)
                    {
                        StringBuilder input = new StringBuilder("");
                        for (int i = 0; i < args.Length; i++)
                        {
                            input = args[i].IndexOf(' ') > -1 ? input.Append('"') : input;
                            input = i + 1 < args.Length ? input.Append(args[i] + " ") : input.Append(args[i]);
                            input = args[i].IndexOf(' ') > -1 ? input.Append('"') : input;
                        }

                        lsElements = CommonMethods.GenerateInputList(input.ToString());
                    }
                }

                if (lsElements.Count < 2)
                {
                    Console.WriteLine("Very few entries. Ending the program.");
                }
                else
                {
                    try
                    {
                        var searchManager = SearchLogicConstr.CreateSearchLogic();
                        var result = await searchManager.GetResultsReport(lsElements);
                        Console.WriteLine(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }
    }
}
