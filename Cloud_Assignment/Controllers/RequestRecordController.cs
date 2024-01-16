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
using Azure.Core;
using Amazon.Runtime.Internal;

namespace Cloud_Assignment.Controllers
{
	public class RequestRecordController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
		public RequestRecordController(Cloud_AssignmentContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            //havent create view yet (refer tutorial)
            return View();
        }

		public async Task<IActionResult> ViewRequest() {
			List<RequestRecord> foodRequestList = await _context.RequestRecord.ToListAsync();
			return View(foodRequestList);
		}

		public async Task<IActionResult> ViewRequestByUserIdAndStatus(string userId, string requestStatus)
		{
			var query = _context.RequestRecord.AsQueryable();
			if (!string.IsNullOrEmpty(userId))
			{
				query = query.Where(r => r.UserId == userId);
			}
			if (!string.IsNullOrEmpty(requestStatus))
			{
				query = query.Where(r => r.RequestStatus == requestStatus);
			}
			List<RequestRecord> filteredRequestList = await query.ToListAsync();
			return View(filteredRequestList);
		}

		public async Task<IActionResult> EditRequest(int? RecordId) 
		{ 
			if (RecordId == null) 
			{ 
				return NotFound(); 
			} 
			var request = await _context.RequestRecord.FindAsync(RecordId); 
			
			if (request == null)
			{ 
				return BadRequest(RecordId + " is not found in the table!"); 
			} 
			return View(request); 
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateRequest(RequestRecord foodRequest)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (foodRequest.RequestStatus == "deny" && foodRequest.FoodId != null)
					{
						InventoryRecord specificInventory = await _context.InventoryRecord.FindAsync(foodRequest.FoodId);
						if (specificInventory != null)
						{
							int newFoodTotal = specificInventory.FoodTotal.GetValueOrDefault() + foodRequest.FoodQuantity.GetValueOrDefault();
							specificInventory.FoodTotal = newFoodTotal;
							_context.InventoryRecord.Update(specificInventory);
						}
					}
					else
					{
                        var Donation = new FoodRecord
                        {
                            RecordType = "distribution",
                            UserId = Request.Form["StaffId"],
                            FoodId = foodRequest.FoodId,
                            FoodQuantity = foodRequest.FoodQuantity,
                            DOR = DateTime.Now,
                            Description = "Food Distribution"
                        };

                        _context.Add(Donation);
                        await _context.SaveChangesAsync();
                    }
					_context.RequestRecord.Update(foodRequest);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(ViewRequest));
				}
				return View("EditRequest", foodRequest);
			}
			catch (Exception ex)
			{
				return BadRequest("Error: " + ex.Message);
			}
		}

		public async Task<IActionResult> DeleteRequest(int? RecordId) { 
			if (RecordId == null) { 
				return NotFound(); 
			} 
			var request = await _context.RequestRecord.FindAsync(RecordId); 
			if (request == null) { 
				return BadRequest(RecordId + " is not found in the list!"); 
			} 
			_context.RequestRecord.Remove(request); 
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(ViewRequest));
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

		public async Task<IActionResult> StaffPendingMessage()
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
						messages.Add(new KeyValuePair<RequestRecord, string>(user, message.ReceiptHandle));
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
				return RedirectToAction("ViewRequest", "RequestRecord", new { msg = ex.Message });
			}
			return View(messages);
		}

		public async Task<IActionResult> DeleteMessage(string deleteToken, string action, string StaffId, int FoodId, int FoodQuantity, string RequestDesc, int RecordId, String UserId, DateTime DOR)
		{
			List<string> keys = getKeys();
			var sqsClient = new AmazonSQSClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
			var response = await sqsClient.GetQueueUrlAsync(new GetQueueUrlRequest { QueueName = "pending-list" });

			string message = "";
			try
			{
				if (action == "accept")
				{
					var request = new RequestRecord
					{
						RecordId= RecordId,
						UserId = UserId,
						StaffId = StaffId,
						requestType = "food",
						FoodId = FoodId,
						FoodQuantity = FoodQuantity,
						RequestStatus = "accept",
						RequestDesc = RequestDesc,
						DOR = DOR,
					};
					_context.Add(request);
					await _context.SaveChangesAsync();
				}
				else if (action == "deny")
				{
					InventoryRecord specificInventory = await _context.InventoryRecord.FindAsync(FoodId);
					if (specificInventory != null)
					{
						int newFoodTotal = specificInventory.FoodTotal.GetValueOrDefault() + FoodQuantity;
						specificInventory.FoodTotal = newFoodTotal;
						_context.InventoryRecord.Update(specificInventory);
					}

					var request = new RequestRecord
					{
						RecordId = RecordId,
						UserId = UserId,
						StaffId = StaffId,
						requestType = "food",
						FoodId = FoodId,
						FoodQuantity = FoodQuantity,
						RequestStatus = "deny",
						RequestDesc = RequestDesc,
						DOR = DOR,
					};
					_context.Add(request);
					await _context.SaveChangesAsync();
				}

				var deleteMessageRequest = new DeleteMessageRequest
				{
					QueueUrl = response.QueueUrl,
					ReceiptHandle = deleteToken
				};

				await sqsClient.DeleteMessageAsync(deleteMessageRequest);

			}
			catch (AmazonSQSException ex)
			{
				Console.WriteLine($"Error deleting message: {ex.Message}");
				message = "Unable to accept / reject the message from the queue!";
			}
			string successScript = "<script>alert('Successfully modified request status!');</script>";
			TempData["SuccessScript"] = successScript;
			return RedirectToAction("ViewRequest", "RequestRecord", new { msg = message });
		}
	}
}
