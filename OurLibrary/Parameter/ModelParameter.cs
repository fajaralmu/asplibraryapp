using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Parameter
{
    public class ModelParameter
    {
        public const string NameSpace = "OurLibrary.Models.";
        public const string ServiceNameSpace = "OurLibrary.Service.";

        public const string BookRecordId = "BookRecordId";
        public const string MasterBookId = "MasterBookId";
        public const string PublisherId = "PublisherId";
        public const string DetailBookId = "DetailBookId";
        public const string ObjectName = "ObjectName";
        public const string ObjectId = "ObjectId";
        public const int EDIT = 2, ADD = 1, DELETE = 3;
    }
}