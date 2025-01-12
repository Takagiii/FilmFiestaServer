using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.DataAccess.Repositories
{
    public class Repository<DbEntity, ModelEntity> : IRepository<DbEntity, ModelEntity>
        where DbEntity : class, new()
        where ModelEntity : class, IObjectWithId, new()
    {
        private readonly DbSet<DbEntity> _dbSet;
        protected FilmFiestaContext _context;
        protected ILogger _logger;
        protected readonly IMapper _mapper;

        public Repository(FilmFiestaContext context, ILogger logger, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<DbEntity>();
        }

        public virtual async Task<IEnumerable<ModelEntity>> Get(string includeTables = "")
        {
            try
            {
                List<DbEntity> query = string.IsNullOrEmpty(includeTables)
                    ? await _dbSet.AsNoTracking().ToListAsync()
                    : await _dbSet.Include(includeTables).AsNoTracking().ToListAsync();
                return _mapper.Map<ModelEntity[]>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error when getting in db: {ex}", ex);
                return null;
            }
        }

        public virtual async Task<ModelEntity> Insert(ModelEntity entity)
        {
            DbEntity dbEntity = _mapper.Map<DbEntity>(entity);
            _ = _dbSet.Add(dbEntity);
            try
            {
                _ = await _context.SaveChangesAsync();
                ModelEntity newEntity = _mapper.Map<ModelEntity>(dbEntity);
                return newEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error when inserting in db: {ex}", ex);
                return null;
            }

        }

        public virtual async Task<ModelEntity> Update(ModelEntity entity)
        {
            DbEntity dbEntity = _dbSet.Find(entity.Id);


            if (dbEntity == null)
            {
                return null;
            }
            _ = _mapper.Map(entity, dbEntity);
            if (!_context.ChangeTracker.HasChanges())
            {
                return entity;
            }
            try
            {
                _ = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("error when updating db", ex);

                return null;
            }
            return _mapper.Map<ModelEntity>(dbEntity);

        }

        public virtual async Task<bool> Delete(long idEntity)
        {
            DbEntity dbEntity = _dbSet.Find(idEntity);


            if (dbEntity == null)
            {
                return false;
            }
            _ = _dbSet.Remove(dbEntity);
            try
            {
                _ = await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("error when deleting in db", ex);
                return false;
            }
        }
    }
}
