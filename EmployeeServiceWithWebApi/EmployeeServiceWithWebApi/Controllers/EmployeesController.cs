using EmployeeDataAccess;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Net.Http;
using System.Net;
using System;
using System.Web.Http.Cors;


namespace EmployeeServiceWithWebApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EmployeesController : ApiController 
    {

        public IEnumerable<Employee> Get() 
        {

            using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities()) 
            {
                return entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int id) 
        {
            using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities()) 
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else 
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Employee found with Id = " + id);
                }
            }
        }

        public HttpResponseMessage GeteEmployeesByGender(string gender)
        {
            try
            {
                using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities())
                {

                    switch (gender.ToLower())
                    {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Employee Found with " + gender + "found. Please Enter 'ALL', 'Male' OR 'Female' ");
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
                            

                   
        [HttpPost]
        //[Route ("api/employees")]
        public HttpResponseMessage Post([FromBody]Employee employee) 
        {
            try
            {
                using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities())
                {
                    
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());

                    return message;
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            // {"FirstName":"Waqar", "LastName":"Khan","Gender":"Male","Salary":50000}
        }

        public HttpResponseMessage Delete(int id) 
        {
            try
            {
                using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id.ToString() + " could not be found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
                
        }


        public HttpResponseMessage Put(int id, [FromBody]Employee employee) 
        {
            try
            {
                using (EmployeeDatabaseForWebApiEntities entities = new EmployeeDatabaseForWebApiEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee With id = " + id.ToString() + " not found");
                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }

   
}
