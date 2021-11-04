using System.Text.RegularExpressions;

namespace BlazorMovie.Server;
public class Censor
{
    public IList<string> CensoredWords { get; private set; } = new List<string>();

    public Censor()
    {
        StreamReader reader = new StreamReader(Path.GetFullPath(Path.Combine("wwwroot/Bad Words/base-list-of-bad-words_csv-file_2021_01_18.csv")));
        while (!reader.EndOfStream)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string line = reader.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string[] values = line.Split(',');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CensoredWords.Add(values[0]);
        }
    }

    public string CensorText(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException("text");
        }

        string censoredText = text;

        foreach (string censoredWord in CensoredWords)
        {
            string regularExpression = ToRegexPattern(censoredWord);

            censoredText = Regex.Replace(censoredText, regularExpression, StarCensoredMatch,
              RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return censoredText;
    }
    private static string StarCensoredMatch(Match m)
    {
        string word = m.Captures[0].Value;

        return new string('*', word.Length);
    }

    private string ToRegexPattern(string wildcardSearch)
    {
        string regexPattern = Regex.Escape(wildcardSearch);

        regexPattern = regexPattern.Replace(@"\*", ".*?");
        regexPattern = regexPattern.Replace(@"\?", ".");

        if (regexPattern.StartsWith(".*?"))
        {
            regexPattern = regexPattern.Substring(3);
            regexPattern = @"(^\b)*?" + regexPattern;
        }

        regexPattern = @"\b" + regexPattern + @"\b";

        return regexPattern;
    }
}
