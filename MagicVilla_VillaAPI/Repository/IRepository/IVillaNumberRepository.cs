﻿using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber> 
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
