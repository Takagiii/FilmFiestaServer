using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IRepository<DbEntity, ModelEntity>
    {
        Task<IEnumerable<ModelEntity>> Get(string includeTables = "");
        Task<ModelEntity> Insert(ModelEntity entity);
        Task<ModelEntity> Update(ModelEntity entity);
        Task<bool> Delete(long idEntity);
    }
}
