using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SearchComparison.Services;

namespace SearchComparison.BL
{
    public class SearchLogicConstr
    {
        public static ISearchLogic CreateSearchLogic() => CreateSearchClients();

        private static SearchLogic CreateSearchClients()
        {
            Assembly services = Assembly.Load("SearchComparison.Services");

            List<Type> assemblyTypes = new List<Type>();

            foreach (var item in services.GetTypes())
            {
                if (item.GetInterface(typeof(ISearchClient).ToString()) != null)
                    assemblyTypes.Add(item);
            }

            var searchClients = assemblyTypes.Select(type => Activator.CreateInstance(type) as ISearchClient);

            return new SearchLogic(searchClients);
        }
    }
}
