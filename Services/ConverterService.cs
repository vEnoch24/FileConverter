using ConvertApiDotNet;

using CloudConvert.API;
using CloudConvert.API.Models.ExportOperations;
using CloudConvert.API.Models.ImportOperations;
using CloudConvert.API.Models.JobModels;
using CloudConvert.API.Models.TaskOperations;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace FileConverter.Services
{
    public class ConverterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ConverterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task convert()
        {
            var convertApi = new ConvertApi("hyfzEoT6NJbmgHgM");

            var convertResult = await convertApi.ConvertAsync("pdf", "docx", new ConvertApiFileParam("file", "path/to/your/file.pdf"));

            var convertedFile = convertResult.Files[0];
            await convertedFile.SaveFileAsync("path/to/save/converted/file.docx");
        }


        public async Task<string> FileConvertAsync(IBrowserFile file, string format)
        {
            var apiKey = _configuration["CloudConvert:ApiKey"];
            var url = "https://api.cloudconvert.com/v2/convert";

            using var content = new MultipartFormDataContent
            {
                { new StreamContent(file.OpenReadStream()), "file", file.Name },
                { new StringContent(format), "format" }
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("File conversion failed.");
            }

            var result = await response.Content.ReadFromJsonAsync<CloudConvertResponse>();
            return result.Data.Url;
        }


       public async Task<string> ConvertAsync(IBrowserFile selectedFile, string filename, string out_format)
        {
            try
            {
                var apiKey = _configuration["CloudConvert:ApiKey"];
                var _cloudConvert = new CloudConvertAPI(apiKey);

                var job = await _cloudConvert.CreateJobAsync(new JobCreateRequest
                {
                    Tasks = new
                    {
                        import = new ImportUploadCreateRequest(),
                        convert = new ConvertCreateRequest
                        {
                            Input = "import",
                            //Input_Format = in_format,
                            Output_Format = out_format
                        },
                        export_1 = new ExportUrlCreateRequest
                        {
                            Input = "convert",
                            Inline = false,
                            Archive_Multiple_Files = false
                        }
                    },
                    Tag = "jobbuilder"
                });

                //Uploads File

                using (var memoryStream = new MemoryStream())
                {
                    var uploadTask = job.Data.Tasks.FirstOrDefault(t => t.Name == "import");
                    using (var fileStream = selectedFile.OpenReadStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                    }
                    byte[] file = memoryStream.ToArray();
                    string fileName = filename;

                    await _cloudConvert.UploadAsync(uploadTask.Result.Form.Url.ToString(), file, fileName, uploadTask.Result.Form.Parameters);
                };


                job = await _cloudConvert.WaitJobAsync(job.Data.Id);

                // download export file

                var exportTask = job.Data.Tasks.FirstOrDefault(t => t.Name == "export_1");
                //if (exportTask.Result == null)
                //{
                //    throw new InvalidOperationException("Error Converting file. Inernal Server Error");
                //}


                var fileExport = exportTask.Result.Files.FirstOrDefault();
                

                return fileExport.Url.ToString();

            }
            catch(Exception ex)
            {
                return $"Error: {ex.Message}";
            }  
        }

    }

    public class CloudConvertResponse
    {
        public CloudConvertData Data { get; set; }
    }

    public class CloudConvertData
    {
        public string Url { get; set; }
    }
}
