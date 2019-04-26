namespace SimpleRegexLib
{
    public interface IRegex
    {
        bool Matches(string rule, string input);
    }
}