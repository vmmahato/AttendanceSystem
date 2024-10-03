using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;

namespace AttendanceSystem.Services
{
    public interface IUnitOfWorkManager : IDisposable
    {
        /// <summary>
        /// For Entity changes
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction NewUnitOfWork();

        /// <summary>
        /// For SQL changes
        /// </summary>
        /// <returns></returns>
        IUnitOfWork NewUnitOfWorkSql();
    }
}
