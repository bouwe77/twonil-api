using System;
using System.Data.Common;

namespace TwoNil.Data
{
    public interface IDbExceptionMapper
    {
        /// <summary>
        /// Attempts to map a persistance specific DbException to an application known exception type.
        /// </summary>
        /// <param name="exception">The persistance specific DbException to map.</param>
        /// <returns>
        /// A known application exception if mappable to a type which can represent the original exception, 
        /// otherwise null. The original exception will be set in <see cref="Exception.InnerException"/>.
        /// </returns>
        Exception MapFromDbException(DbException exception);
    }
}
