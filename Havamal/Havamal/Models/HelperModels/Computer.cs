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

        public static Computer<TModel> StartComputer()
        {
            return new Computer<TModel>(Darling<TModel>.No(), Darling<Exception>.No(), Answer.No);
        }

        public static Computer<TModel> ComputerSaysYes(Darling<TModel> data)
        {
            return new Computer<TModel>(data, Darling<Exception>.No(), Answer.Yes);
        }

        public static Computer<TModel> ComputerSaysNo(Exception e)
        {
            return new Computer<TModel>(Darling<TModel>.No(), Darling<Exception>.Allow(e), Answer.No);
        }

        public static Computer<TModel> ComputerSaysYes(TModel data)
        {
            return new Computer<TModel>(Darling<TModel>.Allow(data), Darling<Exception>.No(), Answer.Yes);
        }


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
