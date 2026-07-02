using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PicPay.Filters
{
    public class ValidateImageFileAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var _maxFileSize = config.GetValue<long>("ImageSettings:MaxFileSize");
            var _allowedImageTypes = config.GetSection("ImageSettings:AllowedImageTypes")
                .Get<List<string>>();
            var _allowedImageExtensions = config.GetSection("ImageSettings:AllowedImageExtensions")
                .Get<List<string>>()!.Select(x => x.ToLower());

            var file = context.ActionArguments.Values.OfType<IFormFile>().FirstOrDefault();

            var fileContentType = file?.ContentType;

            if (file == null || file.Length <= 0)
            {
                context.Result = new BadRequestObjectResult(new ProblemDetails
                {
                    Title = "Tamanho do arquivo 0 ou não enviado corretamente",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"Arquivo {file?.FileName} tem tamanho 0 ou nulo",
                    Instance = context.HttpContext.Request.Path.ToString(),
                });
                return;
            }

            if (file!.Length > _maxFileSize)
            {
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Title = "Tamanho do arquivo muito grande",
                    Status = StatusCodes.Status413PayloadTooLarge,
                    Detail = $"Arquivo {file.FileName}({file.Length / 1024 / 1024}Mb) passa do tamanho limite ({_maxFileSize / 1024 / 1024}Mb)",
                    Instance = context.HttpContext.Request.Path.ToString(),
                })
                { 
                    StatusCode = StatusCodes.Status413PayloadTooLarge 
                };
                return;
            }

            if (!string.IsNullOrEmpty(file.ContentType) || !_allowedImageTypes.Contains(file.ContentType) || !file.ContentType.StartsWith("image/"))
            {
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Title = "Media Type de arquivo não suportado para esse endpoint",
                    Status = StatusCodes.Status415UnsupportedMediaType,
                    Detail = $"Media Type: {file.ContentType} do arquivo {file.FileName}, não é suportado",
                    Instance = context.HttpContext.Request.Path.ToString(),
                    Extensions = { ["SupportedMediaType"] = _allowedImageTypes }
                })
                {
                    StatusCode = StatusCodes.Status415UnsupportedMediaType
                };
                return;
            }

            var extension = Path.GetExtension(file?.FileName ?? string.Empty).ToLowerInvariant();

            if (!_allowedImageExtensions.Contains(extension))
            {
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Title = "Extensão de arquivo não suportado para esse endpoint",
                    Status = StatusCodes.Status415UnsupportedMediaType,
                    Detail = $"Extensão: {extension} do arquivo {file?.FileName}, não é suportado",
                    Instance = context.HttpContext.Request.Path.ToString(),
                    Extensions = { ["SupportedExtensions"] = _allowedImageExtensions }
                })
                {
                    StatusCode = StatusCodes.Status415UnsupportedMediaType
                };
            }


            
            /*ProblemDetails problemDetails = new()
            {
                Title = "Extensão de arquivo não suportada para esse endpoint",
                UnsupportedMediaType = extensions,
                Path = context.HttpContext.Request.Path.ToString(),
                Status = StatusCodes.Status415UnsupportedMediaType
            };*/



            base.OnActionExecuting(context);
        }
    }
}
