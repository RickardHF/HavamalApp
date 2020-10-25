using System;
using System.Collections.Generic;
using System.Text;

namespace Havamal.Models.HelperModels
{
    public static class ComputerExtensions
    {
        public static Computer<TModel> ComputerSaysYes<TModel>(Darling<TModel> data)
        {
            return new Computer<TModel>(data, DarlingExtensions.No<Exception>(), Answer.Yes);
        }

        public static Computer<TModel> StartComputer<TModel>()
        {
            return new Computer<TModel>(DarlingExtensions.No<TModel>(), DarlingExtensions.No<Exception>(), Answer.No);
        }

        public static Computer<TModel> ComputerSaysNo<TModel>(Exception e)
        {
            return new Computer<TModel>(DarlingExtensions.No<TModel>(), DarlingExtensions.Allow(e), Answer.No);
        }

        public static Computer<TModel> ComputerSaysYes<TModel>(TModel data)
        {
            return new Computer<TModel>(DarlingExtensions.Allow(data), DarlingExtensions.No<Exception>(), Answer.Yes);
        }
    }
}
