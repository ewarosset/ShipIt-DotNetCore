using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShipIt.Exceptions;
using ShipIt.Models.ApiModels;
using ShipIt.Repositories;

namespace ShipIt_DotNetCore.Services
{
    public class OutboundServivces
    {
        public static List<String> AddUniqueGtin(OutboundOrderRequestModel request)
        {
            var gtins = new List<String>();
            foreach (var orderLine in request.OrderLines)
            {
                if (gtins.Contains(orderLine.gtin))
                {
                    throw new ValidationException(String.Format("Outbound order request contains duplicate product gtin: {0}", orderLine.gtin));
                }
                gtins.Add(orderLine.gtin);
            }
            
            return gtins;
        }
        
        public static int CalculateTrucksNeeded(List<OrderLine> orderLines, Dictionary<string, Product> products)
        {
            var orderWeight = orderLines.Select(line => line.quantity * products[line.gtin].Weight).Sum();
            var truckCapacity = 2000000;
            
            var trucksNeeded = (int) Math.Ceiling(orderWeight / truckCapacity);
            return trucksNeeded;
        }
    }
}