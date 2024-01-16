using Amazon.SQS;
using Cloud_Assignment.Data;
using Cloud_Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Cloud_Assignment.Controllers
{
    public class DistributorRecordController : Controller
    {
        private const string s3BucketName = "cloudassignment-g15"; //need to change to your bucket name
        private readonly Cloud_AssignmentContext _context;

        public DistributorRecordController(Cloud_AssignmentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<DistributionSchedule> schedule = await _context.DistributionSchedule.ToListAsync();
            return View(schedule);
        }

        public async Task<IActionResult> ViewDetail(int? RecordID) //View
        {
            if (RecordID == null)
            {
                return NotFound();
            }
            var recordID = await _context.RequestRecord.FindAsync(RecordID);
            if (recordID == null)
            {
                return BadRequest(recordID + " is not found in the table!");
            }
            return View(recordID);
        }

        public async Task<IActionResult> DeliverOrder(int? DistributionID) //Edit
        {
            if (DistributionID == null)
            {
                return NotFound();
            }
            var distribute_ID = await _context.DistributionSchedule.FindAsync(DistributionID);

            if (distribute_ID == null)
            {
                return BadRequest(distribute_ID + " is not found in the table!");
            }
            return View(distribute_ID);
        }

        public async Task<IActionResult> ViewRecord() //View
        {
            List<DistributionSchedule> record = await _context.DistributionSchedule.ToListAsync();
            return View(record);
        }

        public async Task<IActionResult> DeleteOrder(int? DistributionID) //Delete
        {
            if (DistributionID == null)
            {
                return NotFound();
            }
            var selected_Id = await _context.DistributionSchedule.FindAsync(DistributionID);
            if (selected_Id == null)
            {
                return BadRequest(DistributionID + " is not found in the list!");
            }
            _context.DistributionSchedule.Remove(selected_Id);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewRecord");
        }

        private List<string> getKeys()
        {
            List<string> keys = new List<string>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();

            keys.Add(configure["Keys:Key1"]);
            keys.Add(configure["Keys:Key2"]);
            keys.Add(configure["Keys:Key3"]);

            return keys;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder(DistributionSchedule distributionSchedule, IFormFile imagefile)
        {

            //Upload image to S3
            try
            {
                List<string> keys = getKeys();
                AmazonS3Client agentManage = new AmazonS3Client(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
                PutObjectRequest uploadRequest = new PutObjectRequest
                {
                    InputStream = imagefile.OpenReadStream(),
                    BucketName = s3BucketName,
                    Key = "image/" + imagefile.FileName,
                    CannedACL = S3CannedACL.PublicRead
                };

                await agentManage.PutObjectAsync(uploadRequest);
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest("Unable to upload image. Please contact the developer. Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to upload image. Please contact the developer. Error: " + ex.Message);
            }
            distributionSchedule.ImageURL = "https://" + s3BucketName + ".s3.amazonaws.com/image/" + imagefile.FileName;
            distributionSchedule.ImageS3Key = imagefile.FileName;

            if (ModelState.IsValid)
            {
                _context.DistributionSchedule.Update(distributionSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("DeliverOrder", distributionSchedule);

        }
    }
}

