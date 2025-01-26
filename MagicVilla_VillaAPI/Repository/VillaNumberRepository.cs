using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db) 
        {
            this.db = db;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.CreatedDate = DateTime.Now;
            db.villaNumbers.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
