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
    private const string topicARN = "arn:aws:sns:us-east-1:743981991027:FoodBankManagementSystemTopic";
    AmazonSimpleNotificationServiceClient snsagent;
    public Function()
    {
        snsagent = new AmazonSimpleNotificationServiceClient
        (
			"ASIA22OGIIRZ4CZ4LKSW",
			"e12+EoJl22g6QG0xc/20wtx9pwRtl31FMXWpzmGh",
			"FwoGZXIvYXdzEDcaDF/NgAECQdhoafLcbSLJAQHYpbUZAQ6nh9KTxdXHXLtqeRD3Xoheui67wCFzxnqylSjLJy14ZPHl8LFQbATK1Oawkzhp8AIEj3EO3+6CGQWUo5EwgxVsWSGq8gFCxCl+ijWQqFdT5MupfoEw6YuZIbm9krAX7MC1tX2DwvmY1zwz0trHJWiGLk1ul+Q9IRwQ0MlPrxYjHJycJJJmDe2qP46ZJfcSh88yyI7J0Evw5JuMtEW/0e2jIhrtH9zTm7yaXTTz1Z94Is43RimkHgICxf2nusJb3kst1yi/+qKtBjItRJcG3TG0VBvsbZ/8lwhEbc1qYjmAl8j8he8zrkLQWNt/kao5ciFZlUcnj/2o",
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
