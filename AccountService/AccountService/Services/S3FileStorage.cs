using System.Net;
using AccountService.Options;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace AccountService.Services;

public class S3FileStorage(
    IOptions<S3Options> options,
    IAmazonS3 s3Client,
    ILogger<S3FileStorage> logger
)
    : IS3FileStorage
{
    private readonly S3Options s3Options = options.Value;

    public async Task<string?> UploadAsync(Stream fileStream, string bucketName, string fileName)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                InputStream = fileStream,
            };

            var response = await s3Client.PutObjectAsync(putRequest);
            return response.HttpStatusCode == HttpStatusCode.OK ? GetUrl(bucketName, fileName) : null;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error when uploading file to S3");
            return null;
        }
    }

    public async Task DeleteAsync(string fileUrl)
    {
        var (bucket, key) = ReadBucketAndKey(fileUrl);

        var request = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = key,
        };

        await s3Client.DeleteObjectAsync(request);
    }

    private string GetUrl(string bucket, string key) => $"{s3Options.ServiceUrl}/{bucket}/{key}";

    private static (string, string) ReadBucketAndKey(string url)
    {
        var bucketIndex = url.IndexOf('/');
        var keyIndex = url.LastIndexOf('/');

        var bucket = url.Substring(bucketIndex, keyIndex - bucketIndex);
        var key = url[keyIndex..];

        return (bucket, key);
    }
}