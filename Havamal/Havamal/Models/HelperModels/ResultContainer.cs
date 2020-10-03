using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Havamal.Models.HelperModels
{
    public class ResultContainer<TModel>
    {
        public DataContainer<TModel> Data { get; }
        public DataContainer<Exception> Exception { get; }
        public Status Status { get; }

        public static ResultContainer<TModel> CreateContainer()
        {
            return new ResultContainer<TModel>(DataContainer<TModel>.Empty(), DataContainer<Exception>.Empty(), Status.Fail);
        }

        public static ResultContainer<TModel> CreateSuccess(DataContainer<TModel> data)
        {
            return new ResultContainer<TModel>(data, DataContainer<Exception>.Empty(), Status.Success);
        }

        public static ResultContainer<TModel> CreateEmpty(Exception e)
        {
            return new ResultContainer<TModel>(DataContainer<TModel>.Empty(), DataContainer<Exception>.WithValue(e), Status.Fail);
        }

        public static ResultContainer<TModel> CreateSuccess(TModel data)
        {
            return new ResultContainer<TModel>(DataContainer<TModel>.WithValue(data), DataContainer<Exception>.Empty(), Status.Success);
        }


        public U SuccessOrFail<U>(Func<TModel, U> success, Func<Exception, U> empty)
        {
            if (Status == Status.Fail) return (empty(Exception.GetValueOrDefault()));
            return success(Data.GetValueOrDefault());
        }

        public void SuccessOrFail(Action<TModel> success, Action<Exception> empty)
        {
            if (Status == Status.Fail) {
                empty(Exception.GetValueOrDefault());
                return;
            }
            ;
            success(Data.GetValueOrDefault());
        }

        public ResultContainer(DataContainer<TModel> data, DataContainer<Exception> exception, Status status)
        {
            Data = data;
            Exception = exception;
            Status = status;
        }
    }

    public enum Status
    {
        Success = 1,
        Fail = 0
    }
}
