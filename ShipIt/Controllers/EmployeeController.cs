﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShipIt.Exceptions;
using ShipIt.Models.ApiModels;
 using ShipIt.Models.DataModels;
 using ShipIt.Repositories;

namespace ShipIt.Controllers
{

    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("")]
        public EmployeeResponse GetByName([FromQuery] string name)
        {
            Log.Info($"Looking up employee by name: {name}");
        
            var employees = _employeeRepository.GetEmployeesByName(name).Select(employeeDataModel => new Employee(employeeDataModel));

            Log.Info("Found employee(s): " + employees);
            return new EmployeeResponse(employees);
        }
        
        [HttpGet("id/{id}")]
        public EmployeeResponse GetById([FromRoute] int id)
        {
            Log.Info($"Looking up employee by name: {id}");
        
            var employee = new Employee(_employeeRepository.GetEmployeeById(id));
        
            Log.Info("Found employee: " + employee);
            return new EmployeeResponse(employee);
        }
        
        [HttpGet("{warehouseId}")]
        public EmployeeResponse Get([FromRoute] int warehouseId)
        {
            Log.Info(String.Format("Looking up employee by id: {0}", warehouseId));

            var employees = _employeeRepository
                .GetEmployeesByWarehouseId(warehouseId)
                .Select(e => new Employee(e));

            Log.Info(String.Format("Found employees: {0}", employees));
            
            return new EmployeeResponse(employees);
        }

        [HttpPost("")]
        public Response Post([FromBody] AddEmployeesRequest requestModel)
        {
            List<Employee> employeesToAdd = requestModel.Employees;

            if (employeesToAdd.Count == 0)
            {
                throw new MalformedRequestException("Expected at least one <employee> tag");
            }

            Log.Info("Adding employees: " + employeesToAdd);

            _employeeRepository.AddEmployees(employeesToAdd);

            Log.Debug("Employees added successfully");

            return new Response() { Success = true };
        }

        [HttpDelete("")]
        public void Delete([FromBody] RemoveEmployeeRequest requestModel)
        {
            // string name = requestModel.Name;
            int id = requestModel.Id;
            
            if (id == null)
            {
                throw new MalformedRequestException("Unable to parse name from request parameters");
            }

            try
            {
                _employeeRepository.RemoveEmployee(id);
            }
            catch (NoSuchEntityException)
            {
                throw new NoSuchEntityException("No employee exists with id: " + id);
            }
        }
    }
}
