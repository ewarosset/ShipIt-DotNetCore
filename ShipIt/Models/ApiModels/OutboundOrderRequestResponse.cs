using System.Collections.Generic;

namespace ShipIt.Models.ApiModels
{
    public class OrdersByTruck
        {
            public int TruckNumber { get; set; }

            public List<OrderLine> Orders { get; set; }
            public float TruckLoadInKg { get; set; }
        }

        public class OutboundOrdersRequest
        {
            public int TrucksNeeded { get; set; }
            public List<OrdersByTruck> OrdersByTruck { get; set; }
        }
    }
