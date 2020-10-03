using Havamal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havamal.Interfaces.BaseInterfaces
{
    public interface ICreate<TResult, TModel, TParam>
    {
        Task<Computer<TResult>> Create(TModel data, TParam param);
    }
}
