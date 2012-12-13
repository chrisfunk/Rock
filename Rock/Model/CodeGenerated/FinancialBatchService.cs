//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//

using System;
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// FinancialBatch Service class
    /// </summary>
    public partial class FinancialBatchService : Service<FinancialBatch, FinancialBatchDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialBatchService"/> class
        /// </summary>
        public FinancialBatchService()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialBatchService"/> class
        /// </summary>
        public FinancialBatchService(IRepository<FinancialBatch> repository) : base(repository)
        {
        }

        /// <summary>
        /// Creates a new model
        /// </summary>
        public override FinancialBatch CreateNew()
        {
            return new FinancialBatch();
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public override IQueryable<FinancialBatchDto> QueryableDto( )
        {
            return QueryableDto( this.Queryable() );
        }

        /// <summary>
        /// Query DTO objects
        /// </summary>
        /// <returns>A queryable list of DTO objects</returns>
        public IQueryable<FinancialBatchDto> QueryableDto( IQueryable<FinancialBatch> items )
        {
            return items.Select( m => new FinancialBatchDto()
                {
                    Name = m.Name,
                    BatchDate = m.BatchDate,
                    IsClosed = m.IsClosed,
                    CampusId = m.CampusId,
                    Entity = m.Entity,
                    EntityId = m.EntityId,
                    ForeignReference = m.ForeignReference,
                    Id = m.Id,
                    Guid = m.Guid,
                });
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( FinancialBatch item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<FinancialTransaction>().Queryable().Any( a => a.BatchId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", FinancialBatch.FriendlyTypeName, FinancialTransaction.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }
}
