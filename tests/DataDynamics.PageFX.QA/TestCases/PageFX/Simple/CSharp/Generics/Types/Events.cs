using System;

class X
{
    public class CustomEventArgs : EventArgs
    {
        public string info = "data";
    }

    public delegate void MEventHandler<TEventArgs>(Object sender, TEventArgs e) where TEventArgs : EventArgs;
    
    public class ClassThatRaisesEvent<T> where T : EventArgs, new()
    {
        public event MEventHandler<T> SomeEvent;

        protected virtual void OnSomeEvent(T e)
        {
            if (SomeEvent != null)
                SomeEvent(this, e);
        }

        public void SimulateEvent() 
        {
            OnSomeEvent(new T());
        }
    }

    public class ClassThatHandlesEvent
    {
        public ClassThatHandlesEvent(ClassThatRaisesEvent<CustomEventArgs> eventRaiser)
        {
            eventRaiser.SomeEvent +=
               new MEventHandler<CustomEventArgs>(HandleEvent);
        }

        private void HandleEvent(object sender, CustomEventArgs e)
        {
            Console.WriteLine("Event handled: {0}", e.info);
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        ClassThatRaisesEvent<CustomEventArgs> eventRaiser = new ClassThatRaisesEvent<CustomEventArgs>();
        ClassThatHandlesEvent eventHandler = new ClassThatHandlesEvent(eventRaiser);
        eventRaiser.SimulateEvent();
    }

    
    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}

    

