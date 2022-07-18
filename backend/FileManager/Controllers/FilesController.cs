using FileManager.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("/api/files")]
    [Produces("application/json")]
    public class FilesController : ControllerBase
    {
        public FilesController() { }

        [HttpPost("create-file")]
        public ActionResult<EmptyResponse> CreateFile(string path)
        {
            bool isFile = Regex.IsMatch(path, @".+\..+$");
            try
            {
                if (isFile && !System.IO.File.Exists(path))
                {
                    FileStream fs = System.IO.File.Create(path);
                    fs.Close();
                }
                else if (!isFile && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    return StatusCode(400);
                }

                return new EmptyResponse();
            }
            catch (Exception e)
            {
                return StatusCode(400, e);
            }
        }

        [HttpPost("delete-file")]
        public ActionResult<EmptyResponse> DeleteFile(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    return StatusCode(400);
                }

                return new EmptyResponse();
            }
            catch (Exception e)
            {
                return StatusCode(400, e);
            }
        }

        [HttpPost("execute-file")]
        public ActionResult<EmptyResponse> ExecuteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                try
                {
                    var process = new Process();

                    process.StartInfo = new ProcessStartInfo(path) { UseShellExecute = true, };
                    process.Start();

                    return new EmptyResponse();
                }
                catch (Exception e)
                {
                    return StatusCode(400, e);
                }
            }
            else
            {
                return StatusCode(400);
            }
        }

        [HttpGet("get-files")]
        public ActionResult<GetFilesResponse> GetFiles(string path)
        {
            List<FileData> files = new();

            string formattedPath = Regex.Replace(path, @"(\\)+", @"\");

            try
            {
                if (Directory.Exists(formattedPath))
                {
                    Func<string, string> extractName = (fullName) =>
                        Regex.Match(fullName, @"([^\\]+)$").Value;

                    files = Directory
                        .GetDirectories(formattedPath)
                        .Select<string, FileData>(
                            fullName =>
                                new FileData
                                {
                                    Name = extractName(fullName),
                                    FullName = fullName,
                                    LastModifiedAt = Directory.GetLastWriteTime(fullName),
                                    IsDirectory = true
                                }
                        )
                        .ToList();

                    files = files
                        .Concat<FileData>(
                            Directory
                                .GetFiles(formattedPath)
                                .Select<string, FileData>(
                                    fullName =>
                                        new FileData
                                        {
                                            Name = extractName(fullName),
                                            FullName = fullName,
                                            LastModifiedAt = Directory.GetLastWriteTime(fullName),
                                            IsDirectory = false
                                        }
                                )
                        )
                        .ToList();
                }
                else
                {
                    return StatusCode(400);
                }
            } catch (Exception e)
            {
                return StatusCode(400, e);
            }

            return new GetFilesResponse
            {
                Files = files,
                ParentPath = Directory.GetParent(formattedPath)?.FullName
            };
        }
    }
}
