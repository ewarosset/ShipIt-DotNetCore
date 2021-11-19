﻿using System;
using System.Collections.Generic;
using System.Linq;
 using NUnit.Framework;
 using ShipIt.Controllers;
using ShipIt.Exceptions;
using ShipIt.Models.ApiModels;
 using ShipIt.Models.DataModels;
 using ShipIt.Repositories;
using ShipItTest.Builders;

namespace ShipItTest
{
    public class EmployeeControllerTests : AbstractBaseTest
    {
        EmployeeController employeeController = new EmployeeController(new EmployeeRepository());
        EmployeeRepository employeeRepository = new EmployeeRepository();

        private const string Name = "Ewa Rosset";
        private const string MissingName = "Missing Name";
        private const int WarehouseId = 1;
        private const int EmployeeId = 1000000;
        


        // [Test]
        // public void TestRoundtripEmployeeRepository()
        // {
        //     onSetUp();
        //     var employee = new EmployeeBuilder().CreateEmployee();
        //     employeeRepository.AddEmployees(new List<Employee>() {employee});
        //     Assert.AreEqual(employeeRepository.GetEmployeeById(employee.Id).Name, employee.Name);
        //     Assert.AreEqual(employeeRepository.GetEmployeeById(employee.Id).Ext, employee.ext);
        //     Assert.AreEqual(employeeRepository.GetEmployeeById(employee.Id).WarehouseId, employee.WarehouseId);
        // }
        //
        // [Test]
        // public void TestGetEmployeeByName()
        // {
        //     onSetUp();
        //     var employeeBuilder = new EmployeeBuilder().setName(NAME);
        //     employeeRepository.AddEmployees(new List<Employee>() {employeeBuilder.CreateEmployee()});
        //     var result = employeeController.GetByName(NAME);
        //
        //     var correctEmployee = employeeBuilder.CreateEmployee();
        //     Assert.IsTrue(EmployeesAreEqual(correctEmployee, result.Employees.First()));
        //     Assert.IsTrue(result.Success);
        // }
        //
        [Test]
        public void TestGetEmployeesByWarehouseId()
        {
            onSetUp();
            var employeeBuilderA = new EmployeeBuilder().setWarehouseId(WarehouseId).setName("A");
            var employeeBuilderB = new EmployeeBuilder().setWarehouseId(WarehouseId).setName("B");
            employeeRepository.AddEmployees(new List<Employee>() { employeeBuilderA.CreateEmployee(), employeeBuilderB.CreateEmployee() });
            var result = employeeController.Get(WarehouseId).Employees.ToList();
        
            var correctEmployeeA = employeeBuilderA.CreateEmployee();
            var correctEmployeeB = employeeBuilderB.CreateEmployee();
        
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(EmployeesAreEqual(correctEmployeeA, result.First()));
            Assert.IsTrue(EmployeesAreEqual(correctEmployeeB, result.Last()));
        }
        
        [Test]
        public void TestGetNonExistentEmployee()
        {
            onSetUp();

            try
            {
                employeeController.GetByName(MissingName).Employees.ToList();
                Assert.Fail("Expected exception to be thrown.");
            }
            catch (NoSuchEntityException e)
            {
                Assert.IsTrue(e.Message.Contains(MissingName));
            }
        }
        
        // [Test]
        // public void TestGetEmployeeInNonexistentWarehouse()
        // {
        //     onSetUp();
        //     try
        //     {
        //         var employees = employeeController.Get(WAREHOUSE_ID).Employees.ToList();
        //         Assert.Fail("Expected exception to be thrown.");
        //     }
        //     catch (NoSuchEntityException e)
        //     {
        //         Assert.IsTrue(e.Message.Contains(WAREHOUSE_ID.ToString()));
        //     }
        // }
        //
        [Test]
        public void TestAddEmployees()
        {
            onSetUp();
            var employeeBuilder = new EmployeeBuilder().setName(Name);
            var addEmployeesRequest = employeeBuilder.CreateAddEmployeesRequest();
        
            var response = employeeController.Post(addEmployeesRequest);
            var databaseEmployee = employeeRepository.GetEmployeesByName(Name).First();
            var correctDatabaseEmployee = employeeBuilder.CreateEmployee();
        
            Assert.IsTrue(response.Success);
            Assert.IsTrue(EmployeesAreEqual(new Employee(databaseEmployee), correctDatabaseEmployee));
            
        }
        
        [Test]
        public void TestDeleteEmployees()
        {
            onSetUp();
            var employeeBuilder = new EmployeeBuilder().setName(Name);
            employeeRepository.AddEmployees(new List<Employee>() {employeeBuilder.CreateEmployee()});
            var employee = employeeRepository.GetEmployeesByName(Name).First();
            
            var removeEmployeeRequest = new RemoveEmployeeRequest() {Id = employee.Id};
            employeeController.Delete(removeEmployeeRequest);
            
            try
            {
                employeeController.GetById(employee.Id);
                Assert.Fail("Expected exception to be thrown.");
            }
            catch (NoSuchEntityException e)
            {
                Assert.IsTrue(e.Message.Contains(employee.Id.ToString()));
            }
        }

        [Test]
        public void TestDeleteNonexistentEmployee()
        {
            onSetUp();
            
            // TODO Issue with removing by a none existing ID (currently set to a high number)
            var removeEmployeeRequest = new RemoveEmployeeRequest() { Id = EmployeeId };
        
            try
            {
                employeeController.Delete(removeEmployeeRequest);
                Assert.Fail("Expected exception to be thrown.");
            }
            catch (NoSuchEntityException e)
            {
                Assert.IsTrue(e.Message.Contains(EmployeeId.ToString()));
            }
        }

        [Test]
        public void TestDuplicateEmployeeHaveDifferentIDs()
        {
            onSetUp();
            var employeeBuilder = new EmployeeBuilder().setName(Name);
            var addEmployeesRequest = employeeBuilder.CreateAddEmployeesRequest();
        
            var response = employeeController.Post(addEmployeesRequest);
            var databaseEmployee = employeeRepository.GetEmployeesByName(Name).First();
            var correctDatabaseEmployee = employeeBuilder.CreateEmployee();
        
            Assert.IsTrue(response.Success);
            Assert.AreNotEqual(new Employee(databaseEmployee), correctDatabaseEmployee);
        }
        
        
        private bool EmployeesAreEqual(Employee A, Employee B)
        {
            return A.WarehouseId == B.WarehouseId
                   && A.Name == B.Name
                   && A.role == B.role
                   && A.ext == B.ext;
        }
    }
}
