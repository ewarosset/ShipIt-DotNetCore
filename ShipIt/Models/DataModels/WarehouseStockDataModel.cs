using System.Data;

namespace ShipIt.Models.DataModels
{
    public class WarehouseStockDataModel : DataModel
    {
        [DatabaseColumnName("p_id")] public int ProductId { get; set; }
        [DatabaseColumnName("w_id")] public int WarehouseId { get; set; }
        [DatabaseColumnName("name")] public int Name { get; set; }
        [DatabaseColumnName("role")] public int Role { get; set; }
        [DatabaseColumnName("ext")] public int Extension { get; set; }
        [DatabaseColumnName("gtin_cd")] public int GlobalTradeIndexCode { get; set; }
        [DatabaseColumnName("gtin_nm")] public int GlobalTradeIndexName { get; set; }
        [DatabaseColumnName("gln_nm")] public int CompanyName { get; set; }
        [DatabaseColumnName("gln_addr_02")] public string Addr2 { get; set; }
        [DatabaseColumnName("gln_addr_03")] public string Addr3 { get; set; }
        [DatabaseColumnName("gln_addr_04")] public string Addr4 { get; set; }
        [DatabaseColumnName("gln_addr_postalcode")]  public string PostalCode { get; set; }
        [DatabaseColumnName("gln_addr_city")] public string City { get; set; }
        [DatabaseColumnName("contact_tel")] public string Tel { get; set; }

        public WarehouseStockDataModel(IDataReader dataReader): base(dataReader) { }
        public WarehouseStockDataModel() {}
    }
}