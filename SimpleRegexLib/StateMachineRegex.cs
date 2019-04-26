using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleRegexLib
{
    public class StateMachineRegex : IStateMachine, IRegex
    {
        private IDictionary<(State, char?), State> transitions;

        private IDictionary<State, State> epsilonTransitions;

        private readonly State initialState = new State();
        private IEnumerable<State> currentStates;

        public void Configure(string pattern)
        {
            transitions = new Dictionary<(State, char?), State>();
            epsilonTransitions = new Dictionary<State, State>();

            var state = initialState;

            foreach (var c in pattern)
            {
                switch (c)
                {
                    case '*':
                    {
                        var midState = new State();
                        epsilonTransitions.Add(state, midState);

                        transitions.Add((midState, null), midState);

                        var nextState = new State();
                        epsilonTransitions.Add(midState, nextState);

                        state = nextState;
                        break;
                    }
                    case '.':
                    {
                        var nextState = new State();
                        transitions.Add((state, null), nextState);
                        state = nextState;
                        break;
                    }
                    default:
                    {
                        var nextState = new State();
                        transitions.Add((state, c), nextState);
                        state = nextState;
                        break;
                    }
                }
            }

            state.Accepting = true;

            currentStates = new[] {initialState};
        }

        public StateMachineRegex() : this(String.Empty)
        {
        }

        public StateMachineRegex(string pattern)
        {
            Configure(pattern);
        }

        public void Interpret(string input)
        {
            foreach (var c in input)
            {
                Interpret(c);
            }
        }

        public void Interpret(char c)
        {
            IEnumerable<State> StateSelector(State s)
            {
                if (transitions.TryGetValue((s, c), out var nextState)
                    || transitions.TryGetValue((s, null), out nextState))
                {
                    yield return nextState;

                    while (epsilonTransitions.TryGetValue(nextState, out nextState))
                    {
                        yield return nextState;
                    }
                }
            }

            currentStates = currentStates.SelectMany(StateSelector);
        }

        public bool Evaluate()
        {
            currentStates = currentStates.ToArray();
            return currentStates.Any(s => s.Accepting);
        }

        public bool Matches(string rule, string input)
        {
            Configure(rule);
            Interpret(input);
            return Evaluate();
        }
    }
}