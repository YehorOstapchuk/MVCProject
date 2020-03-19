#pragma checksum "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "835e734ff9940b2ba2d553a724bb27690288336c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cathedras_Index), @"mvc.1.0.view", @"/Views/Cathedras/Index.cshtml")]
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
#line 1 "C:\Users\Егор\Desktop\ProjectMVC\Views\_ViewImports.cshtml"
using ProjectMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Егор\Desktop\ProjectMVC\Views\_ViewImports.cshtml"
using ProjectMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"835e734ff9940b2ba2d553a724bb27690288336c", @"/Views/Cathedras/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d79579bcf7c60e4fb109a6469c9c4d1e3ea9c8c5", @"/Views/_ViewImports.cshtml")]
    public class Views_Cathedras_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ProjectMVC.Cathedras>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
  
    ViewData["Title"] = "Кафедри за факультетами";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <h1>Кафедри за факультетом ");
#nullable restore
#line 7 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
                          Write(ViewBag.FacultyName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n    <p>\r\n        ");
#nullable restore
#line 10 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
   Write(Html.ActionLink("Додати нову кафедру", "Create", new { facultyID = @ViewBag.FacultyID }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </p>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 16 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CathedraName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 19 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.Faculty));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 25 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 28 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CathedraName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 31 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Faculty.FacultyName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 34 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.ActionLink("Edit", "Edit", new { id = item.Id, facultyID = @ViewBag.FacultyID }));

#line default
#line hidden
#nullable disable
            WriteLiteral("|\r\n                <!--  <a asp-action=\"Edit\" asp-route-id=\"");
#nullable restore
#line 35 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
                                                    Write(item.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">Edit</a>   | -->\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "835e734ff9940b2ba2d553a724bb27690288336c6398", async() => {
                WriteLiteral("Details");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 36 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
                                          WriteLiteral(item.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\r\n                <!--<a asp-action=\"Delete\" asp-route-id=\"");
#nullable restore
#line 37 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
                                                    Write(item.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">Delete</a>-->\r\n                ");
#nullable restore
#line 38 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
           Write(Html.ActionLink("Delete", "Delete", new { id = item.Id, facultyID = @ViewBag.FacultyID }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 41 "C:\Users\Егор\Desktop\ProjectMVC\Views\Cathedras\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ProjectMVC.Cathedras>> Html { get; private set; }
    }
}
#pragma warning restore 1591