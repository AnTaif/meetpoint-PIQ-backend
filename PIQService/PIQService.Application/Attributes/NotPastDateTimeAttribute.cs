using System.ComponentModel.DataAnnotations;

namespace PIQService.Application.Attributes;

public class NotPastDateTimeAttribute : ValidationAttribute
{
    public NotPastDateTimeAttribute()
    {
        ErrorMessage = "Дата не может быть в прошлом. Пожалуйста, укажите текущую или будущую дату.";
    }
    
    public override bool IsValid(object? value) => value != null && IsValid((DateTime)value);

    private static bool IsValid(DateTime value) => value >= DateTime.UtcNow;
}