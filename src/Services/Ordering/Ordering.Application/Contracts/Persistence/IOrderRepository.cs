﻿using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
	internal interface IOrderRepository : IAsyncRepository<Order>
	{
		Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
	}
}
