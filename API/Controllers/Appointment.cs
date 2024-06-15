using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using API.Data.Repositories.Interfaces;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentRepository appointmentRepository)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
        }

        ///ONLY TEST AWS DUMMY POINT DELETE AFTER
        [AllowAnonymous]
        [HttpGet("awstest")]
        public void TestAws()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            ;
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

            try
            {
                DynamoDBContext context = new DynamoDBContext(client);
                // Get an item.
                GetBook(context, 101);


                Console.WriteLine("To continue, press Enter");
                Console.ReadLine();
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmens()
        {
            return new OkObjectResult(await _appointmentRepository.GetAppointments());
        }

        [AllowAnonymous]
        [HttpGet("coach-appointments/{coachID}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByCoachID(int coachID)
        {
            return new OkObjectResult(await _appointmentRepository.GetAppointmentsByCoachID(coachID));
        }

        [Authorize("coach")]
        [HttpPost("appointment-create")]
        public async Task<ActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            var result = await _appointmentRepository.Add(appointment);

            if (result.GetType() == typeof(OkObjectResult))
                await _appointmentRepository.SaveChangesAsync();

            return result;
        }

        [Authorize("coach")]
        [HttpDelete("appointment-delete")]
        public async Task<ActionResult> DeleteAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
                return new BadRequestObjectResult("Appointment cannot be null.");

            var result = await _appointmentRepository.Remove(appointment);

            if (result.GetType() == typeof(OkObjectResult))
                await _appointmentRepository.SaveChangesAsync();

            return result;
        }

        private static async void GetBook(DynamoDBContext context, int productId)
        {
            testtable bookItem = await context.LoadAsync<testtable>("asdasd", "asdasdasds");

            Console.WriteLine("\nGetBook: Printing result.....");
            Console.WriteLine("Title: {0} \n No.Of threads:{1}",
                      bookItem.testid, bookItem.testdate);
        }



        ///ONLY TEST AWS DUMMY POINT DELETE AFTER
        [DynamoDBTable("test-table")]
        public class testtable
        {
            [DynamoDBHashKey] //Partition key
            public string testid
            {
                get; set;
            }

            [DynamoDBRangeKey] //Sort key
            public string testdate
            {
                get; set;
            }

        }
    }



}