#pragma checksum "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5d629f58f947d61167f48de55e6c1664f469d341"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\_ViewImports.cshtml"
using ServeNObserve;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\_ViewImports.cshtml"
using ServeNObserve.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d629f58f947d61167f48de55e6c1664f469d341", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc5f5e7ebb9581b20e33ad90a775a0a70a153d07", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <h1 class=\"display-4\">Welcome</h1>\r\n    <p>Learn about <a href=\"https://docs.microsoft.com/aspnet/core\">building Web apps with ASP.NET Core</a>.</p>\r\n");
#nullable restore
#line 8 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
      
        var jobs = ViewBag.Jobs;
        foreach (var item in jobs)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <p>Name: ");
#nullable restore
#line 12 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
                Write(item.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n            <p>Income: ");
#nullable restore
#line 13 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
                  Write(item.Income);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n            <p>Balance: ");
#nullable restore
#line 14 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
                   Write(item.Balance);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n            <p>-----------------</p>\r\n");
#nullable restore
#line 16 "C:\Users\Burki\Documents\GitHub\ServeNObserve\ServeNObserve\ServeNObserve\Views\Home\Index.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
