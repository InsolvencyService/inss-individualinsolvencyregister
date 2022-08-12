using INSS.EIIR.Web.INSS.EIIR.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS.EIIR.Web.INSS.EIIR.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public readonly RegisterContext registerContext;
        public Repository(RegisterContext registerContext)
        {
            this.registerContext = registerContext;
        }
    }
}
