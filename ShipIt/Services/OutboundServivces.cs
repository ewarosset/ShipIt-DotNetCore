using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShipIt.Exceptions;
using ShipIt.Models.ApiModels;
using ShipIt.Models.DataModels;
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
                    throw new ValidationException(
                        String.Format("Outbound order request contains duplicate product gtin: {0}", orderLine.gtin));
                }

                gtins.Add(orderLine.gtin);
            }

            return gtins;
        }

        public static int CalculateTrucksRequired(List<OrderLine> orderLines, Dictionary<string, Product> products)
        {
            var orderWeight = orderLines.Select(line => line.quantity * products[line.gtin].Weight).Sum();
            var truckCapacity = 2000000;

            var trucksNeeded = (int) Math.Ceiling(orderWeight / truckCapacity);
            return trucksNeeded;
        }

        public static void OrdersByTruck()
        {
            // TODO What items need to be contained in each truck
            // TODO total weight of the items in each truck
            // As we go through the list of orders we'll need to keep checking what the total weight is
            // We also want a list of list of items that will be in that trucks
            
            // If when the current item is added the weight is over 2000kg (keep in mind weight is currently in g) then: 
            // 1. We want to add that total weight to a list to store it 
            // 2. Then the weight resets to 0 until it reaches over 200kg
            // 3. We want to add the current item's weight to the new truck's weight
            // 4. We want to add the current list of items to the main List of truck orders
            // 5. We need to create a new list of orders and add the item to it
            
            // Otherwise add to the truck's weight
            // Add to the items list
        }
        
        
        public static void CheckForInsufficientStock(OutboundOrderRequestModel request,
            Dictionary<int, StockDataModel> stock, List<StockAlteration> lineItems)
        {
            var orderLines = request.OrderLines.ToList();
            var errors = new List<string>();

            for (int i = 0; i < lineItems.Count; i++)
            {
                var lineItem = lineItems[i];
                var orderLine = orderLines[i];

                if (!stock.ContainsKey(lineItem.ProductId))
                {
                    NoStockErrorMessage(errors, orderLine);
                    continue;
                }

                var item = stock[lineItem.ProductId];
                if (lineItem.Quantity > item.held)
                {
                    StockToRemoveErrorMessage(errors, orderLine, item, lineItem);
                }
            }

            if (errors.Count > 0)
            {
                throw new InsufficientStockException(string.Join("; ", errors));
            }
        }
        

        private static void NoStockErrorMessage(List<string> errors, OrderLine orderLine)
        {
            errors.Add(string.Format("Product: {0}, no stock held", orderLine.gtin));
        }
        
        private static void StockToRemoveErrorMessage(List<string> errors, OrderLine orderLine, StockDataModel item, StockAlteration lineItem)
        {
            errors.Add(
                string.Format("Product: {0}, stock held: {1}, stock to remove: {2}", orderLine.gtin, item.held,
                    lineItem.Quantity));
        }
    }
}