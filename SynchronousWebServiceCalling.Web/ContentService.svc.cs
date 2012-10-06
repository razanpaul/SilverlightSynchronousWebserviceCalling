using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using System.Collections;

namespace SynchronousWebServiceCalling.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContentService
    {      
        [OperationContract]
        public string ReturnServerTime()
        {
            return DateTime.Now.ToString();
        }
    }   
}
