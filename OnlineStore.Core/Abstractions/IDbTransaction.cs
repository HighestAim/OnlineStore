using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Core.Abstractions
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
