using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.IO;
using Microsoft.Extensions.Configuration;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.EntityFrameworkCore;
using Cloud_Assignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace Cloud_Assignment.Controllers
{
    public class SNSController : Controller
    {
        private readonly Cloud_AssignmentContext _context;
        private readonly UserManager<Cloud_AssignmentUser> _userManager;

        public SNSController(Cloud_AssignmentContext context, UserManager<Cloud_AssignmentUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        private const string SNSTopicARN = "arn:aws:sns:us-east-1:116183498964:FoodBankManagementSystemTopic";

        private List<string> getKeys()
        {
            List<string> keys = new List<string>();
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfigurationRoot conf = builder.Build();
            keys.Add(conf["Keys:Key1"]);
            keys.Add(conf["Keys:Key2"]);
            keys.Add(conf["Keys:Key3"]);
            return keys;
        }

        //Add API Below MK
        public async Task<string> doSubscribe(string emailaccount)
        {
            //var httpcall = new HttpClient();
            //await httpcall.GetAsync("" + emailaccount);
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            try
            {
                /*SubscribeRequest request = new SubscribeRequest
                {
                    TopicArn = SNSTopicARN,
                    Protocol = "email",
                    Endpoint = emailaccount
                };*/

                //---------------------------
                var httpcall = new HttpClient();
                await httpcall.GetAsync("https://tog2o71o6c.execute-api.us-east-1.amazonaws.com/dev/SubscribeNewsletter?ec=" + emailaccount);
                //---------------------------
                //await client.SubscribeAsync(request);
                return "success";
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                return (" Error: " + ex.Message);
            }

        }
        //Add API Above MK

        public async Task<IActionResult> Index()
        {
            List<NewsletterEmail> subscriptionEmails = await _context.NewsletterEmail.ToListAsync();
            return View(subscriptionEmails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerNewlettersAfterLogin(string newsLetter, string emailId)
        {
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            var user = await _userManager.GetUserAsync(User);
            try
            {
                string emailInUse = emailId;
                if (newsLetter == "true")
                {
                    emailInUse = user.Email;
                }
                var SubscriptionEmail = new NewsletterEmail
                {
                    Email = emailInUse,
                    UserId = user.Id
                };

                _context.Add(SubscriptionEmail);
                await _context.SaveChangesAsync();

                SubscribeRequest request = new SubscribeRequest
                {
                    TopicArn = SNSTopicARN,
                    Protocol = "email",
                    Endpoint = emailInUse
                };
                await client.SubscribeAsync(request);
                return RedirectToAction("Index");
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                return BadRequest("Eamil:" + emailId + " Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updatelettersAfterLogin(int emailSubId, string emailId, string currentEmail)
        {
            string subscriptionArn = null;
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            var user = await _userManager.GetUserAsync(User);
            try
            {
                var subscriptionEmailToUpdate = await _context.NewsletterEmail.FindAsync(emailSubId);
                if (subscriptionEmailToUpdate != null)
                {
                    subscriptionEmailToUpdate.Email = emailId;
                    await _context.SaveChangesAsync();
                }

                try
                {
                    ListSubscriptionsByTopicRequest listSubscriptionsRequest = new ListSubscriptionsByTopicRequest
                    {
                        TopicArn = SNSTopicARN
                    };

                    ListSubscriptionsByTopicResponse listSubscriptionsResponse = await client.ListSubscriptionsByTopicAsync(listSubscriptionsRequest);

                    foreach (var subscription in listSubscriptionsResponse.Subscriptions)
                    {
                        if (subscription.Protocol == "email" && subscription.Endpoint == currentEmail)
                        {
                            subscriptionArn = subscription.SubscriptionArn;
                            break;
                        }
                    }

                    if (subscriptionArn != null && subscriptionArn != "PendingConfirmation")
                    {
                        UnsubscribeRequest unsubscribeRequest = new UnsubscribeRequest
                        {
                            SubscriptionArn = subscriptionArn
                        };

                        await client.UnsubscribeAsync(unsubscribeRequest);
                    }
                    SubscribeRequest request = new SubscribeRequest
                    {
                        TopicArn = SNSTopicARN,
                        Protocol = "email",
                        Endpoint = emailId
                    };
                    await client.SubscribeAsync(request);
                    return RedirectToAction("Index");
                }
                catch (AmazonSimpleNotificationServiceException ex)
                {
                    return BadRequest("Error-> " + ex.Message + "\narn-" + subscriptionArn);
                }
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                return BadRequest("Eamil:" + emailId + " Error: " + ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerNewletters(string emailId)
        {
            string result = await doSubscribe(emailId);
            if (result == "success")
            {
                return RedirectToAction("Index", "SNS");
            }
            else
            {
                return BadRequest("Email:" + emailId + " " + result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> sendBroadcastMessage(string subjecttitle, string msgbody)
        {
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            try
            {
                PublishRequest request = new PublishRequest
                {
                    TopicArn = SNSTopicARN,
                    Subject = subjecttitle,
                    Message = msgbody
                };
                await client.PublishAsync(request);
                return RedirectToAction("SendBroadcastMessage", "SNS");
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                return BadRequest(" Error: " + ex.Message);
            }
        }

        public IActionResult SendBroadcastMessage()
        {
            return View();
        }
    }
}
