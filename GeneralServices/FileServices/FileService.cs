using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using GeneralServices.Models;
using GeneralServices.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GeneralServices.FileServices
{
    public class FileService
    {
        public readonly bool IsLocal;
        private readonly string _localSubFolderAddress;

        public readonly string[] UploadFolders;

        public FileService(IOptions<FileSetting> fileSetting)
        {
            var fs = fileSetting.Value;
            IsLocal = fs.IsLocal;
            _localSubFolderAddress = fs.SubFolderAddress;

            UploadFolders = new string[] { "uploads" };
        }

        public async Task<GeneralServiceResponse> UploadFile(IFormFile file, string folderName = null,
            string validExtensions = null, string fileExtenstion = "Same As File", string filename = "Same As File",
            bool toLocalFolder = false)
        {
            var validExtensionsArray = new string[] { };

            if (validExtensions != null)
                validExtensionsArray = validExtensions.Split(",").ToArray();

            return await UploadFile(file, folderName, validExtensionsArray, fileExtenstion, filename, toLocalFolder);
        }

      

        private async Task<GeneralServiceResponse> UploadFile(IFormFile file, string folderName = null, string[] validExtensions = null, string fileExtenstion = "Same As File", string filename = "Same As File", bool toLocalFolder = false)
        {
            if (toLocalFolder && !IsLocal) return new GeneralServiceResponse(GeneralResponseStatus.Fail, "You request to upload file in local file but not in local mode.");

            if (folderName == null) folderName = PathFromArrayFolders(UploadFolders);

            string extension = Path.GetExtension(file.FileName).ToLower().Substring(1);

            if (validExtensions != null && validExtensions.Length > 0)
            {
                if (!validExtensions.Contains(extension))
                    return new GeneralServiceResponse(GeneralResponseStatus.Fail, "Extension is not valid.");
            }

            string uploadFileName = $"{(filename == "Same As File" ? file.FileName : $"{filename}.{(fileExtenstion == "Same As File" ? extension : fileExtenstion)}")}";

            folderName = Path.Combine(SubFolderPath(toLocalFolder), folderName);
            string path1 = Path.Combine(folderName, uploadFileName);

            try
            {
                if (!System.IO.Directory.Exists(folderName)) Directory.CreateDirectory(folderName);

                if (System.IO.File.Exists(path1))
                {
                    System.IO.File.Delete(path1);
                }

                await using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return new GeneralServiceResponse(GeneralResponseStatus.Success, "Upload Succeeded.", path1);
            }
            catch (Exception e)
            {
                return new GeneralServiceResponse(GeneralResponseStatus.Fail, e.Message);
            }

        }
        public async Task<GeneralServiceResponse> UploadFile(IFormFile file, string[] folders = null, string[] validExtensions = null, string fileExtenstion = "Same As File", string filename = "Same As File", bool toLocalFolder = false)
        {
            string folderName = folders == null ? null : PathFromArrayFolders(folders);

            return await UploadFile(file, folderName, validExtensions, fileExtenstion, filename, toLocalFolder);
        }

        public async Task<GeneralServiceResponse> UploadTemplateExcelFile(IFormFile file, string validExtensions = "xls,xlsx", string fileExtenstion = "Same As File", string filename = "Same As File",
            bool toLocalFolder = false)
        {
            var validExtensionsArray = new string[] { };

            if (validExtensions != null)
                validExtensionsArray = validExtensions.Split(",").ToArray();

            return await UploadFile(file, "", validExtensionsArray, fileExtenstion, filename, toLocalFolder);
        }

        public GeneralServiceResponse DeleteFile(string folderName, string fileName, bool isLocal = false)
        {
            if (!IsExistFile(folderName, fileName, inLocalFolder: true))
                return new GeneralServiceResponse(GeneralResponseStatus.NotFound);

            var path = Path.Combine(GetFullPath(folderName, isLocal), fileName);

            try
            {
                File.Delete(path);

                return new GeneralServiceResponse();
            }
            catch (Exception e)
            {
                return new GeneralServiceResponse(GeneralResponseStatus.Fail, e.Message);
            }
        }

        public List<FileViewModel> GetDirectoryItems(string[] folders, bool inLocalFolder = false)
        {
            return GetDirectoryItems(PathFromArrayFolders(folders), inLocalFolder);
        }

        public List<FileViewModel> GetDirectoryItems(string folderName, bool inLocalFolder = false)
        {
            if (inLocalFolder && !IsLocal) return new List<FileViewModel>();

            var path = GetFullPath(folderName, inLocalFolder);

            var model = new List<FileViewModel>();

            try
            {
                var items = Directory.GetFiles(path);

                foreach (var listItem in items)
                {
                    var l = listItem.Replace(@"\", "/");
                    var f = l.Split("/").ToArray();

                    var nf = new FileViewModel();

                    if (f.Length > 0)
                    {
                        nf.FileName = f[f.Length - 1];
                        nf.FilePath = l;
                    }

                    model.Add(nf);
                }

                return model;

            }
            catch (Exception e)
            {
                var s = e;
                return model;
            }
        }

        public bool IsExistFile(string folderName, string fileName, string extension = "Default", bool inLocalFolder = false)
        {
            if (inLocalFolder && !IsLocal) return false;

            if (extension != "Default")
            {
                fileName = fileName + "." + extension;
            }

            var path = Path.Combine(GetFullPath(folderName, inLocalFolder), fileName);

            return File.Exists(path);
        }

        public bool IsExistFile(string[] folders, string fileName, string extension = "Default", bool inLocalFolder = false)
        {
            return IsExistFile(PathFromArrayFolders(folders), fileName, extension, inLocalFolder);
        }

        public bool IsExistFolder(string folderName, bool inLocalFolder = false)
        {
            if (inLocalFolder && !IsLocal) return false;

            return Directory.Exists(GetFullPath(folderName, inLocalFolder));

        }
        public bool IsExistFolder(string[] folders, bool inLocalFolder = false)
        {
            return IsExistFolder(PathFromArrayFolders(folders));
        }

        public string GetFullPath(string folderName, bool isLocalFolder = false)
        {
            return Path.Combine(SubFolderPath(isLocalFolder), folderName);
        }

        private string PathFromArrayFolders(string[] folders)
        {
            var path = "";

            foreach (var folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            return path;
        }

        public string SubFolderPath(bool localFolder)
        {
            return localFolder ? _localSubFolderAddress : Path.Combine("wwwroot", "uploads");
        }

    }
}
