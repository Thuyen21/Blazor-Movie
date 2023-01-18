using System.Text.RegularExpressions;

namespace BlazorMovie.Server;
public class Censor
{
    public IList<string> CensoredWords { get; private set; } = new List<string>();

/* Reading the csv file and adding the words to the list. */
    public Censor()
    {
        StreamReader reader = new(Path.GetFullPath(Path.Combine("wwwroot/Bad Words/base-list-of-bad-words_csv-file_2021_01_18.csv")));
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            CensoredWords.Add(values[0]);
        }
    }

  /// <summary>
  /// It takes a string and replaces all the words in the CensoredWords list with asterisks.
  /// 
  /// The function is pretty simple, but there are a few things to note:
  /// 
  /// 1. The function takes a string as a parameter.
  /// 2. The function throws an exception if the string is null.
  /// 3. The function loops through each word in the CensoredWords list.
  /// 4. The function uses a regular expression to find the word in the string.
  /// 5. The function replaces the word with asterisks.
  /// 6. The function returns the censored string.
  /// 
  /// The function uses a regular expression to find the word in the string. 
  /// 
  /// The regular expression is created by the ToRegexPattern function. 
  /// 
  /// The ToRegexPattern function is shown below:
  /// 
  /// /*
  /// C#
  /// */
  /// </summary>
  /// <param name="text">The text to censor.</param>
  /// <returns>
  /// The censored text.
  /// </returns>
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
  /// <summary>
  /// It takes a Match object, gets the word that was matched, and returns a string of asterisks the
  /// same length as the word
  /// </summary>
  /// <param name="Match">The match that was found.</param>
  /// <returns>
  /// A string of asterisks the same length as the word that was matched.
  /// </returns>
    private static string StarCensoredMatch(Match m)
    {
        string word = m.Captures[0].Value;

        return new string('*', word.Length);
    }

  /// <summary>
  /// It takes a string that may contain wildcards and returns a regex pattern that will match the
  /// string
  /// </summary>
  /// <param name="wildcardSearch">The wildcard search string.</param>
  /// <returns>
  /// A string that is a regex pattern.
  /// </returns>
    private string ToRegexPattern(string wildcardSearch)
    {
        string regexPattern = Regex.Escape(wildcardSearch);

        regexPattern = regexPattern.Replace(@"\*", ".*?");
        regexPattern = regexPattern.Replace(@"\?", ".");

        if (regexPattern.StartsWith(".*?"))
        {
            regexPattern = regexPattern[3..];
            regexPattern = @"(^\b)*?" + regexPattern;
        }

        regexPattern = @"\b" + regexPattern + @"\b";

        return regexPattern;
    }
}
