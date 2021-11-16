using System.Collections.Generic;
using System.Data;
using ShipIt.Models.ApiModels;

namespace ShipIt.Models.DataModels
{
    public class EmployeesDataModel : EmployeeDataModel
    {
        public IEnumerable<EmployeeDataModel> Employees;

        public EmployeesDataModel(IDataReader dataReader) : base(dataReader)
        { }
    }
}