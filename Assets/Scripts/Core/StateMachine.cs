using System.Collections.Generic;
public struct StateUpdateEventArgs
    {
        public string Name;
        public StateEventType Type;
        public StateUpdateEventArgs(string Name, StateEventType Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }

    public delegate void StateUpdateEvent(StateUpdateEventArgs e);

    public class StateMachine
    {
        // This implementation is a single outbound directional state machine
        readonly Dictionary<string, State> States;
        public State CurrentState { get; set; }
        public event StateUpdateEvent OnStateUpdate;

        public void AddState(string Name, string Next)
        {
            States.Add(Name, new State(Name, Next));
        }

        public void AddState(State state)
        {
            States.Add(state.Name, state);
        }

        public StateMachine()
        {
            States = new Dictionary<string, State>();
        }

        public void Update(bool externalCondition)
        {
            // Ideally only trigger the update once per frame?
            if (CurrentState != null)
            {
                if (CurrentState.JustEntered)
                {
                    CurrentState.JustEntered = false;
                    ExecuteEntry();
                }
                else if (CurrentState.IsReady && externalCondition)
                    Transition(CurrentState.NextStateName);
                else ExecuteUpdate();
            }
        }

        public void Transition(string StateName)
        {
            if (!States.ContainsKey(StateName)) return;
            if (CurrentState != null) ExecuteExit();
            CurrentState = States[StateName];
            CurrentState.JustEntered = true;
        }

        public void AddStateEvent(string Name, StateEventType type, StateEvent e)
        {
            if (States.ContainsKey(Name))
            {
                State s = States[Name];
                switch (type)
                {
                    case StateEventType.Enter:
                        s.OnStateEnterEvent += e;
                        break;
                    case StateEventType.Update:
                        s.DoUpdateEvent += e;
                        break;
                    case StateEventType.Exit:
                        s.OnStateExitEvent += e;
                        break;
                    default:
                        break;
                }
            }
        }

        void ExecuteEntry()
        {
            CurrentState.OnStateEnter();
            OnStateUpdate?.Invoke(new StateUpdateEventArgs(CurrentState.Name, StateEventType.Enter));
        }

        void ExecuteExit()
        {
            CurrentState.OnStateExit();
            OnStateUpdate?.Invoke(new StateUpdateEventArgs(CurrentState.Name, StateEventType.Exit));
        }

        void ExecuteUpdate()
        {
            CurrentState.DoUpdate();
            OnStateUpdate?.Invoke(new StateUpdateEventArgs(CurrentState.Name, StateEventType.Update));
        }

        public void SetCurrentReady()
        {
            CurrentState.SetReady();
        }
    }