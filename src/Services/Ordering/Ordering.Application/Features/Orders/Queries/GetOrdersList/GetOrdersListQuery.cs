﻿using MediatR;


namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	internal class GetOrdersListQuery : IRequest<List<OrdersVm>>
	{
		public string UserName { get; set; }
		public GetOrdersListQuery(string userName)
		{
			UserName = userName;
		}
	}
}