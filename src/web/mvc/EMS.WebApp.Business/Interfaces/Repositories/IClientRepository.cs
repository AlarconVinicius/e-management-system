﻿using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<PagedResult<Client>> GetAllPagedAsync(int pageSize, int pageIndex, string query = null);
}
