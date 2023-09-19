using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore;
using System.Linq;
// using swensen_api.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Azure.Identity;
using System.Dynamic;

namespace swensen_api.Class
{
    public class Utilities
    {

        public static string GetUIID()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }
    }

}