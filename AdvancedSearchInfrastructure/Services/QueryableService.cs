using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

namespace AdvancedSearchInfrastructure.Services
{
    public interface IQueryableService
    {
        string DynamicPattern { get; }

        void SetQueryable(string input);
    }

    public class QueryableService : IQueryableService
    {
        public string DynamicPattern { get; private set; }

        public void SetQueryable(string input)
        {
            // Define a regular expression pattern to match text inside double quotes.
            string pattern = "\"(.*?)\"";

            // Use Regex.Matches to find all matches of text inside double quotes.
            MatchCollection quoteMatches = Regex.Matches(input, pattern);

            // Replace the quoted strings with a placeholder to temporarily remove them.
            string tempInput = Regex.Replace(input, pattern, "QUOTED_STRING");

            // Split the remaining text using " and " and " not " as delimiters.
            string[] splitStrings = Regex.Split(tempInput, @"\b(and|not)\b");

            // Replace the placeholders with the original quoted strings.
            string dynamicPattern = string.Empty;
            int quoteIndex = 0;

            for (int i = 0; i < splitStrings.Length; i++)
            {
                if (splitStrings[i].Trim() == "QUOTED_STRING")
                {
                    splitStrings[i] = quoteMatches[quoteIndex].Groups[1].Value;
                    quoteIndex++;
                }
            }

            string andOrNot = "=";
            // Print the results.
            foreach (string result in splitStrings)
            {
                if (result == "and" || result == "not")
                    andOrNot = (result == "and" ? "=" : "!");
                else
                    dynamicPattern += $@"(?{andOrNot}.*\b{result.Trim()}\b)";

                Console.WriteLine(result.Trim()); // Trim to remove leading/trailing whitespace.
            }

            dynamicPattern += $@".*$";

            DynamicPattern = dynamicPattern;
        }
    }
}