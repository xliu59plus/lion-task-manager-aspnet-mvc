using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace LionTaskManagementApp.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service()
        {
            try
            {
                // Use your preferred method for credential configuration
                _bucketName = Environment.GetEnvironmentVariable("HEROKU_S3_BUCKET_NAME").Trim();
                var accessKeyId = Environment.GetEnvironmentVariable("HEROKU_S3_ACCESS_KEY_ID").Trim();
                var secretAccessKey = Environment.GetEnvironmentVariable("HEROKU_S3_SECRET_ACCESS_KEY").Trim();
                if (_bucketName == null || accessKeyId == null || secretAccessKey == null)
                {
                    throw new Exception("_bucketName or accessKeyId or secretAccessKey is null, unable to create S3 Client");
                }

                _s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.USEast1);
            }
            catch (Exception ex)
            {
                throw new Exception("S3 Service Creation Exception:" + ex.Message);
            }
        }

        public async Task UploadFileAsync(string filePath, string keyName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);

                // Create a request to upload a file
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _bucketName,
                    Key = keyName, // The name of the object in S3
                    FilePath = filePath
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
                Console.WriteLine("Upload completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }

        public async Task<string> DownloadFileAsync(string keyName, string downloadFilePath)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);

                // Create a request to download a file
                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = _bucketName,
                    Key = keyName,
                    FilePath = downloadFilePath
                };

                await fileTransferUtility.DownloadAsync(downloadRequest);
                Console.WriteLine("Download completed");
                return downloadFilePath;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }

        public async Task<string> GetPreSignedUrlAsync(string keyName, TimeSpan expiry)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = keyName,
                    Expires = DateTime.UtcNow + expiry,
                    Verb = HttpVerb.GET
                };

                string url = await _s3Client.GetPreSignedURLAsync(request);
                return url;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when generating pre-signed URL", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when generating pre-signed URL", e.Message);
                throw;
            }
        }
    }
}
