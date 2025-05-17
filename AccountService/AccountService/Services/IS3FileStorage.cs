namespace AccountService.Services;

public interface IS3FileStorage
{ 
    Task<string?> UploadAsync(Stream fileStream, string bucketName, string fileName);

    Task DeleteAsync(string fileUrl);
}