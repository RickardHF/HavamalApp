using Havamal.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havamal.Interfaces.BaseInterfaces
{
    public interface IRead<TModel, TParam>
    {
        Task<Computer<TModel>> Get(TParam param, CancellationToken cancellationToken);
    }
}
