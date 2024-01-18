using Amazon.Lambda.Core;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace subscription_lambda;

public class Function
{
    private const string topicARN = "arn:aws:sns:us-east-1:332687543123:newsletterEmailExample";
    AmazonSimpleNotificationServiceClient snsagent;
    public Function()
    {
        snsagent = new AmazonSimpleNotificationServiceClient
        (
            "ASIAU25NZBNJWKLRJEPY",
            "4BBtyTLRR3kiCvZpwoZ5LJuyENBNAc+nwvqSkEj/",
            "FwoGZXIvYXdzECQaDPiOUcfvp5oH6Vw2gyLJAQcFOGB40TDL1AuoeS+Jl7eBm3+rfH33ZGUF42Acpd3hlf0jp4AdSa+agXKQVM0jRls3LpAvGaCnWi3B2iu9dZLY8qhBJSf2Opp4NPVcRxUHCaHDtBqU9VhLDa/SuMu0PlgdS3M2peQy/biI818zpGtRUQ/nr0JOfcYthJyw4FtgpGygypwsRX4VNOTkrKM1q++DJhXXgSbbVlp0A6epA8tqRL/hEXyCIY1iRNZFqLgjubQTprVZV/bt5ovf3dsF/PX6CXs506LhiSjB1Z6tBjItpxic8Q4P0/qCIHJofTOnLslDczB1nO6DhW+hh9Oa6SUAK9BEh3fkGj+bIx+v",
            RegionEndpoint.USEast1
        );
    }

    public async Task<string> FunctionHandler(Notification email, ILambdaContext context)
    {
        try
        {
            SubscribeRequest emailRequest = new SubscribeRequest
            {
                TopicArn = topicARN,
                Protocol = "email",
                Endpoint = email.EmailAccount
            };
            SubscribeResponse response = await snsagent.SubscribeAsync(emailRequest);
            context.Logger.LogInformation(response.ResponseMetadata.RequestId);
            return JsonConvert.SerializeObject(
                new
                {
                    success = "200",
                    message = "Successfully subscribe! Subscription ID: "+ response.ResponseMetadata.RequestId.ToString()
                }

            ) ;
        }
        catch(AmazonSimpleNotificationServiceException ex)
        {
            context.Logger.LogInformation(ex.Message);
            return JsonConvert.SerializeObject(
                new
                {
                    success = "400",
                    message = "Error in subscribe: " + ex.Message
                }

            );
        }
    }
}
