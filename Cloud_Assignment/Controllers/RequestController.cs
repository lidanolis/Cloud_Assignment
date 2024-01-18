using Microsoft.AspNetCore.Mvc;
using Cloud_Assignment.Models;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cloud_Assignment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon;
using Amazon.SQS.Model;
using Amazon.SQS;
using Newtonsoft.Json;
using System;

namespace Cloud_Assignment.Controllers
{
	public class RequestController : Controller
	{
		private readonly Cloud_AssignmentContext _context;
		public RequestController(Cloud_AssignmentContext context)
		{
			_context = context;
		}
		public IActionResult RequestForHelp()
		{
			//view havent created yet
			return View();
		}

        public async Task<IActionResult> RequestFood()
        {
            List<InventoryRecord> foodList = await _context.InventoryRecord.ToListAsync();
            return View(foodList);
        }

        private List<string> getKeys()
        {
            List<string> keys = new List<string>();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot conf = builder.Build();
            keys.Add(conf["Keys:Key1"]);
            keys.Add(conf["Keys:Key2"]);
            keys.Add(conf["Keys:Key3"]);
            return keys;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFoodRequest()
        {
            try
            {
                List<InventoryRecord> foodList = await _context.InventoryRecord.ToListAsync();
                List<string> keys = getKeys();
                AmazonSQSClient sqsClient = new AmazonSQSClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
				DateTime now = DateTime.UtcNow;
				int timestamp = (int)(now - new DateTime(1970, 1, 1)).TotalSeconds;

				foreach (var item in foodList)
                {
                    if (item.FoodId == int.Parse(Request.Form["FoodId"]))
                    {
                        item.FoodTotal = item.FoodTotal - int.Parse(Request.Form["FoodQuantity"]);
                    }
                }
                _context.InventoryRecord.UpdateRange(foodList);
                await _context.SaveChangesAsync();
                var request = new SendMessageRequest
                {
                    QueueUrl = "https://sqs.us-east-1.amazonaws.com/743981991027/pending-list",
                    MessageBody = JsonConvert.SerializeObject(new
                    {
				        RecordId = (timestamp << 16),
						UserId = Request.Form["UserId"].FirstOrDefault(),
						RequestType = "food",
                        FoodId = int.Parse(Request.Form["FoodId"]),
                        FoodQuantity = int.Parse(Request.Form["FoodQuantity"]),
                        RequestStatus = "pending",
                        RequestDesc = Request.Form["RequestDesc"].FirstOrDefault(),
                        DOR = DateTime.Now,
                    })
                };
                await sqsClient.SendMessageAsync(request);
                string successScript = "<script>alert('Food request sent successfully!');</script>";
                TempData["SuccessScript"] = successScript;
                return RedirectToAction("RequestFood");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // Function: View the message info from the "pending-list" queue for a specific UserId
        public async Task<IActionResult> PendingMessage(string userId)
        {
            List<string> keys = getKeys();
            var sqsClient = new AmazonSQSClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            var response = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest { QueueName = "pending-list" });
            List<KeyValuePair<RequestRecord, string>> messages = new List<KeyValuePair<RequestRecord, string>>();
            try
            {
                ReceiveMessageRequest receivedRequest = new ReceiveMessageRequest
                {
                    QueueUrl = response.QueueUrl,
                    WaitTimeSeconds = 10,
                    MaxNumberOfMessages = 10,
                    VisibilityTimeout = 10,
                };
                ReceiveMessageResponse returnResponse = await sqsClient.ReceiveMessageAsync(receivedRequest);
                Console.WriteLine($"Number of messages received: {returnResponse.Messages.Count}");

                foreach (var message in returnResponse.Messages)
                {
                    try
                    {
                        RequestRecord user = JsonConvert.DeserializeObject<RequestRecord>(message.Body);
                        if (user.UserId == userId)
                        {
                            messages.Add(new KeyValuePair<RequestRecord, string>(user, message.ReceiptHandle));
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine($"Error deserializing message.Body: {message.Body}");
                        Console.WriteLine($"Exception details: {ex}");
                    }
                }
            }
			catch (AmazonSQSException ex)
            {
                return RedirectToAction("RequestFood", "Request", new { msg = ex.Message });
            }
            return View(messages);
        }
    }
}
