namespace SimpleRegexLib
{
    public interface IStateMachine
    {
        void Interpret(string input);
        void Interpret(char c);
        bool Evaluate();
    }
}