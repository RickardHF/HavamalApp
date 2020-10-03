using Havamal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havamal.Interfaces.BaseInterfaces
{
    public interface IDelete<TResult, TModel, TParam>
    {
        Task<Computer<TResult>> Delete(TModel model, TParam param);
    }
}
