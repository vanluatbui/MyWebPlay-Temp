﻿using MailKit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Extension;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics.Eventing.Reader;
using System.Formats.Tar;
using System.Globalization;
using System.IO;
using System.Net.Mail;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult HD_API()
        {
            return View();
        }

        public ActionResult HD_Web_AspNetCore()
        {
            return View();
        }

        public ActionResult UploadFile(int? sl = 1, int? name = 0, int? upload =0)
        {
            if (sl == null)
                ViewBag.SL = 0;

            if (name == 0)
                ViewBag.X = 0;
            else
            ViewBag.X = 1;

            if (upload == 0)
                ViewBag.Y = 0;
            else
                ViewBag.Y = 1;

            ViewBag.SL = sl;

            return View();
        }

   
        [HttpPost]
        public async Task<ActionResult> UploadFile(List<IFormFile> fileUpload, List<string> TenFile)
        {
            int flag = 0;
            if (TenFile.Count() > 0)
            {
                ViewBag.X = 1;
                ViewBag.Y = 1;
            }

            try
            {
                if (fileUpload.Count() > 0)
                {
                    Calendar x = CultureInfo.InvariantCulture.Calendar;

                    string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    //DateTime dt = DateTime.ParseExact(x.AddHours(DateTime.UtcNow, 7).ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    string name = Request.HttpContext.Connection.RemoteIpAddress + ":" + Request.HttpContext.Connection.RemotePort + " - " + xuxu;

                    MailRequest mail = new MailRequest();
                    mail.Subject = "Send file from " + name;
                    mail.Body = "Send file from " + name;
                    mail.ToEmail = "mywebplay.savefile@gmail.com";

                    mail.Attachments = new List<IFormFile>();

                    for (int i = 0; i < fileUpload.Count(); i++)
                        mail.Attachments.Add(fileUpload[i]);

                    await _mailService.SendEmailAsync(mail);
                }
            }
            finally
            {
                if (ViewBag.X == 1 && ViewBag.Y == 1)
                {
                    for (int i = 0; i < fileUpload.Count(); i++)
                    {
                        if (fileUpload[i] != null && (TenFile[i] == null || TenFile[i].Length <= 0))
                        {
                            flag = 1;
                            break;
                        }
                    }

                    if (flag == 0)
                    {
                        for (int i = 0; i < fileUpload.Count(); i++)
                        {
                            var fileName = Path.GetFileName(fileUpload[i].FileName);

                            var path = Path.Combine(_webHostEnvironment.WebRootPath, "file", fileName);

                            string tenfile = TenFile[i].ToString();

                            var pth = Path.Combine(_webHostEnvironment.WebRootPath, "file", tenfile + System.IO.Path.GetExtension(path));

                            if (System.IO.File.Exists(pth))
                            {
                                flag = 2;
                                break;
                            }
                        }
                    }

                    if (flag == 0)
                    {
                        for (int i = 0; i < TenFile.Count(); i++)
                        {
                            int dem = 0;
                            for (int j = 0; j < TenFile.Count(); j++)
                            {
                                if ((TenFile[j] != null && TenFile[j].Length > 0) && TenFile[j] == TenFile[i])
                                    dem++;
                            }

                            if (dem > 1)
                            {
                                flag = 3;
                                break;
                            }
                        }
                    }


                    if (flag == 0)
                    {
                        for (int i = 0; i < fileUpload.Count(); i++)
                        {

                            var fileName = Path.GetFileName(fileUpload[i].FileName);

                            var path = Path.Combine(_webHostEnvironment.WebRootPath, "file", fileName);

                            string tenfile = TenFile[i].ToString();

                            var pth = Path.Combine(_webHostEnvironment.WebRootPath, "file", tenfile + System.IO.Path.GetExtension(path));

                            using (Stream fileStream = new FileStream(path, FileMode.Create))
                            {
                                fileUpload[i].CopyTo(fileStream);

                            }

                            System.IO.File.Move(path, pth);
                        }
                    }
                }
            }

            //----------------------------------

            if (ViewBag.X == 1 && ViewBag.Y == 1)
            {
                if (flag == 1)
                {
                    ViewData["Loi"] = "Lỗi hệ thống. Nếu bạn đã đăng tải một file, hãy tự đặt lại tên mới gợi nhớ của bạn cho từng file đó...";
                    return this.UploadFile(1, 0, 0);
                }
                else if (flag == 2)
                {
                    ViewData["Loi"] = "Một trong những file bạn sắp tải - tên file bạn sắp upload (tên mới bạn tự đặt) đã tồn tại!\r\nTất cả các file đã bị lỗi khi đăng tải, mời bạn thực hiện lại...";
                    return this.UploadFile(1, 0, 0);
                }
                else if (flag == 3)
                {
                    ViewData["Loi"] = "Một trong những file bạn sắp tải - tên file bạn sắp upload (tên mới bạn tự đặt) đã tồn tại hoặc bị trùng!\r\nTất cả các file đã bị lỗi khi đăng tải, mời bạn kiểm tra và thực hiện lại...";
                    return this.UploadFile(1, 0, 0);
                }
            }

                //SendEmail.SendMail2Step("mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", name, name, "teinnkatajeqerfl");

                ViewBag.KetQua = "Thành công! Tất cả các file đã được đăng tải lên Server hệ thống...";
           
                return View();
            
        }

        public ActionResult DownloadFile(int? sl, int? all = 0)
        {
            if (sl == null)
                ViewBag.SL = 0;

            ViewBag.SL = sl;

            if (all == 0)
                ViewBag.All = 0;
            else
                ViewBag.All = 1;

            string ketqua = "";

            if (ViewBag.All == 1)
            {
                ketqua = "";
                int k = 0;
                var listFile = new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "file")).GetFiles();
                foreach (var item in listFile)
                {
 
                    ketqua = "Thành công! Xem hoặc download file của bạn <a style=\"color:purple\" href=\"/file/" + item.Name + "\" download> tại đây</a>! <br> Link xem đầy đủ : <a target=\"_blank\" style=\"color:green\"" +
                   "href=\"/file/" + item.Name + "\">/file/" + item.Name + "</a><br> Tải lại hoặc chờ một khoảng thời gian để link file được xử lý - tất cả file trên hệ thống admin sẽ tự động xoá sau 24h bạn đăng tải...  " +
                  "<a target=\"_blank\" style=\"color:grey\" href=\"/Home/XoaFile?file=" + item.Name + "\" onclick=\"xacnhan()\">Click để xoá thủ công file này?</a><br><br>";
                    ketqua += "<br><br>";
                    ViewBag.XL = listFile.Count();
                    ViewData["KetQua" + k] = ketqua;
                    ketqua = "";
                    k++;
                }
               
            }

            return View();
        }

       [HttpPost]
        public ActionResult DownloadFile(List<string> TenFile)
        {

            for (int i = 0; i < TenFile.Count(); i++)
            {
                string ketqua = "";

                string tenfile = TenFile[i];


                if (tenfile != null && tenfile.Length > 0)
                {
                    var pth = Path.Combine(_webHostEnvironment.WebRootPath, "file", tenfile);
                    if (!System.IO.File.Exists(pth))
                        ketqua = "<p style=\"color:red\"> Tên file \"<span style=\"color:blue\">" + tenfile + "</span>\" bạn cần tìm để download không tồn tại trên Server (đảm bảo bạn phải nhập đủ tên file kèm theo đuôi file extension (VD : abc.txt hoặc abc.jpg hoặc abc.mp3 ...)!</p>";
                    else
                    {
                        ViewBag.XL = TenFile.Count();
                        ketqua = "Thành công! Xem hoặc download file của bạn <a style=\"color:purple\" href=\"/file/" + tenfile + "\" download> tại đây</a>! <br> Link xem đầy đủ : <a target=\"_blank\" style=\"color:green\"" +
                           "href=\"/file/" + tenfile + "\">/file/" + tenfile + "</a><br> Tải lại hoặc chờ một khoảng thời gian để link file được xử lý - tất cả file trên hệ thống admin sẽ tự động xoá sau 24h bạn đăng tải...  " +
                          "<a target=\"_blank\" style=\"color:grey\" href=\"/Home/XoaFile?file=" + tenfile + "\" onclick=\"xacnhan()\">Click để xoá thủ công file này?</a><br><br>";
                    }
                }

                ViewData["KetQua" + i] = ketqua;
            }      
            return View();
        }

        public ActionResult XoaAllFile(string password)
        {
            if (string.Compare(password,"admin-VANLUAT", false) == 0)
            {
                var listFile = System.IO.Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "file"));

                foreach (var file in listFile)
                {
                    FileInfo f = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, "file", file));
                    f.Delete();
                }
            }

            return RedirectToAction("UploadFile");
        }

        public ActionResult XoaFile(string file)
        {
                    FileInfo f = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, "file", file));
                    f.Delete();

            return RedirectToAction("DownloadFile");
        }
    }
}
