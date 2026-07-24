public enum StateEventType
    {
        Enter,
        Update,
        Exit
    }

    public delegate void StateEvent();

    public class State
    {
        public string Name { get; set; }

        // Use single linked states
        public string NextStateName { get; set; }
        public bool IsReady { get; set; }
        public bool JustEntered { get; set; }

        public State(string name, string NextStage)
        {
            Name = name;
            NextStateName = NextStage;
            IsReady = false;
            JustEntered = false;
        }

        public event StateEvent OnStateEnterEvent;
        public event StateEvent DoUpdateEvent;
        public event StateEvent OnStateExitEvent;

        public void OnStateEnter()
        {
            OnStateEnterEvent?.Invoke();
        }

        public void DoUpdate()
        {
            DoUpdateEvent?.Invoke();
        }

        public void OnStateExit()
        {
            OnStateExitEvent?.Invoke();
            IsReady = false;
        }

        public void SetReady() { IsReady = true; }
    }
