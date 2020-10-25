using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Havamal.Models.HelperModels
{
    public class Computer<TModel>
    {
        public Darling<TModel> Happy { get; }
        public Darling<Exception> NotHappy { get; }
        public Answer Answer { get; }

        public U CanI<U>(Func<TModel, U> yes, Func<Exception, U> no)
        {
            if (Answer == Answer.No) return (no(NotHappy.HopeForYes()));
            return yes(Happy.HopeForYes());
        }

        public void CanI(Action<TModel> yes, Action<Exception> no)
        {
            if (Answer == Answer.No) {
                no(NotHappy.HopeForYes());
                return;
            }
            ;
            yes(Happy.HopeForYes());
        }

        public Computer(Darling<TModel> happy, Darling<Exception> notHappy, Answer answer)
        {
            Happy = happy;
            NotHappy = notHappy;
            Answer = answer;
        }
    }

    public enum Answer
    {
        Yes = 1,
        No = 0
    }
}
