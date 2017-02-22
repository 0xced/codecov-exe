﻿using System;
using System.Net;
using codecov.Program;
using codecov.Services.Utils;

namespace codecov.Coverage
{
    public class Upload
    {
        public Upload(Options options)
        {
            Options = options;
        }

        private Options Options { get; }

        public void Uploader(string report, IService service)
        {
            var url = $"https://codecov.io/upload/v4?{service.CreateQuery(Options)}";

            //Post: Ping
            var post = new WebClient();
            post.Headers.Add("Content-Type: text/plain");
            var response = post.UploadString(url, "POST", string.Empty);
            Console.WriteLine(response);

            // Put: Upload report.
            url = response.Trim().Split('\n')[1];
            var put = new WebClient();
            put.Headers.Add("Content-Type: text/plain");
            put.Headers.Add("x-amz-acl: public-read");
            put.Headers.Add("x-amz-storage-class: REDUCED_REDUNDANCY");
            put.UploadString(url, "PUT", report);
        }
    }
}