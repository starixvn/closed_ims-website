namespace Fanex.Ares.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base Repository.
    /// </summary>
    public class BaseRepository : DynamicRepository
    {
        /// <summary>
        /// Fetch Data from Criteria.
        /// </summary>
        /// <typeparam name="S">Class data reader.</typeparam>
        /// <param name="criteria">The Criteria.</param>
        /// <param name="mapObjectFunction">Read data reader and pass object.</param>
        /// <returns>Collection of object.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Justification = "Reviewed")]
        public virtual IList<S> Fetch<S>(ICriteria criteria, Func<IDataReader, S> mapObjectFunction) where S : class
        {
            IList<S> listObject = new List<S>();
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            listObject.Add(mapObjectFunction(reader));
                        }
                    }
                }
            }

            return listObject;
        }

        /// <summary>
        /// Execute command to Database.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <returns>Return value.</returns>
        public virtual Hashtable ExecuteCommand(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                return db.GetOuputParams();
            }
        }
    }
}