﻿using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Templates;
using ServiceStack.DataAnnotations;
using MyApp.ServiceModel;

namespace MyApp.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }

        [Authenticate]
        public object Any(RequiresAuth request)
        {
            return new RequiresAuthResponse { Result = $"Hello, {request.Name}!" };
        }

        [RequiredRole("Manager")]
        public object Any(RequiresRole request)
        {
            return new RequiresRoleResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}