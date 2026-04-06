using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Constants;

public class CacheProfiles
{
    public const string Defaul10 = "Defaul10";
    public const string Defaul20 = "Defaul20";

    public static readonly CacheProfile Profile10 = new()
    {
        Duration = 10
    }; 
    public static readonly CacheProfile Profile20 = new()
    {
        Duration = 20
    }; 
}
