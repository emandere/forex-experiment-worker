using System;
using System.Text;
using System.Text.Json;

using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using forex_experiment_worker.Models;

namespace forex_experiment_worker
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static List<string> pairs = new List<string>()
        {
            "AUDUSD",
            "EURUSD",
            "GBPUSD",
            "NZDUSD",
            "USDCAD",
            "USDCHF",
            "USDJPY"
        };
        static async Task Main(string[] args)
        {
            Console.WriteLine("Test Secrets!");
            Console.WriteLine(Environment.GetEnvironmentVariable("mysecret"));
            
            //await UploadFileToS3("forexexperiments","Hello","world");
            /*
            var startDate = "20190324";
            var endDate = "20200522";
            
            foreach(var pair in pairs)
            {
                var url = $"http://localhost:5002/api/forexdailyprices/{pair}/{startDate}/{endDate}";
                var days = await GetAsync<List<ForexDailyPriceDTO>>(url);
                foreach (var day in days)
                {
                    //var dayString= JsonSerializer.Serialize<ForexDailyPriceDTO>(day);
                    //await UploadFileToS3("forexdailyprices",day.Pair+day.Date,dayString);
                    //Console.WriteLine(day.Pair+day.Date + " uploaded");

                    //var realPrices = await GetRealPrices(day.Pair,day.Datetime.ToString("yyyyMMdd"));

                    //await UploadFileToS3("forexdailyrealprices",realPrices.Item1,JsonSerializer.Serialize<ForexPricesDTO>(realPrices.Item2));
                    var prices = await ReadFileFromS3($"{pair}{day.Datetime.ToString("yyyyMMdd")}","forexdailyrealprices");
                    Console.WriteLine($"{pair} {prices.prices.Length}");
                    //Console.WriteLine(realPrices.Item1 + " uploaded");
                }
                
                
            }*/
           

        }

        public static async Task<ForexPricesDTO> ReadFileFromS3(string key,string bucket)
        {
            var responseBody = string.Empty;
            var request = new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            };
            using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                //string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                //string contentType = response.Headers["Content-Type"];
                //Console.WriteLine("Object metadata, Title: {0}", title);
                //Console.WriteLine("Content type: {0}", contentType);

                responseBody = reader.ReadToEnd(); // Now you process the response body.
                return JsonSerializer.Deserialize<ForexPricesDTO>(responseBody);
            }
            
        }

        public static async Task UploadFileToS3(string bucketname,string key,string info)
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = new MemoryStream(Encoding.UTF8.GetBytes(info)),
                        Key = key,
                        BucketName = bucketname,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }

        static async Task<(string,ForexPricesDTO)> GetRealPrices(string pair,string day)
        {
            var urlgetdailyrealprices = $"http://localhost:5002/api/forexdailyrealprices/{pair}/{day}";
                    
            var dailyrealprices = await GetAsync<ForexPricesDTO>(urlgetdailyrealprices);
            Console.WriteLine($"reading {pair} {day}");

            return (pair+day,dailyrealprices);
        }

        static async Task<T> GetAsync<T>(string url)
        {
            var responseBody = await client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<T>(responseBody);
            return data;
        }

        static async Task<HttpResponseMessage> PatchAsync<T>(T dto,string url)
        {
            var stringPrice= JsonSerializer.Serialize<T>(dto);
            var stringPriceContent = new StringContent(stringPrice,UnicodeEncoding.UTF8,"application/json");
            var responsePriceBody = await client.PatchAsync(url,stringPriceContent);
            return responsePriceBody;
        }

        static async Task<HttpResponseMessage> PostAsync<T>(T dto,string url)
        {
            var stringPrice= JsonSerializer.Serialize<T>(dto);
            var stringPriceContent = new StringContent(stringPrice,UnicodeEncoding.UTF8,"application/json");
            var responsePriceBody = await client.PostAsync(url,stringPriceContent);
            return responsePriceBody;
        }
    }

    
}
