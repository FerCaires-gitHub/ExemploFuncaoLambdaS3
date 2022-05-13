
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ExemploFuncaoLambda
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(S3Event s3Event, ILambdaContext context)
        {
            var dadosEvento = s3Event.Records[0].S3;
            var bucket = dadosEvento.Bucket.Name;
            var key = dadosEvento.Object.Key;
            var amazonS3Client = new AmazonS3Client();
            try
            {
                var file = await amazonS3Client.GetObjectAsync(new GetObjectRequest { BucketName = bucket, Key = key });
                var line = string.Empty;
                using (var stream = file.ResponseStream)
                {
                    using (var sr = new StreamReader(stream))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }
                    }

                }
                return "Sucesso";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Erro";
            }

        }
    }

}