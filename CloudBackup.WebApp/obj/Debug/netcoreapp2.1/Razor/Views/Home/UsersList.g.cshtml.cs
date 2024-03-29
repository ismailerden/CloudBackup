#pragma checksum "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Home\UsersList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa7cb40822f54a5f88632e36a161a3ea73197989"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_UsersList), @"mvc.1.0.view", @"/Views/Home/UsersList.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/UsersList.cshtml", typeof(AspNetCore.Views_Home_UsersList))]
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
#line 1 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\_ViewImports.cshtml"
using CloudBackup.WebApp;

#line default
#line hidden
#line 2 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\_ViewImports.cshtml"
using CloudBackup.WebApp.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fa7cb40822f54a5f88632e36a161a3ea73197989", @"/Views/Home/UsersList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5558d39de9a3d8e55d1c68a558d8ac6f9591bca", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_UsersList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(28, 342, true);
                WriteLiteral(@"
    <script src=""../../bower_components/datatables.net/js/jquery.dataTables.min.js""></script>
    <script src=""../../bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js""></script>
    <script src=""/Scripts/home_userslist.js""></script>
    <script>
        $(document).ready(function () {
            home_userslist.init('");
                EndContext();
                BeginContext(371, 28, false);
#line 11 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Home\UsersList.cshtml"
                            Write(ViewBag.HashedOrganizationId);

#line default
#line hidden
                EndContext();
                BeginContext(399, 33, true);
                WriteLiteral("\');\r\n        });\r\n    </script>\r\n");
                EndContext();
            }
            );
            DefineSection("Heads", async() => {
                BeginContext(451, 112, true);
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css\">\r\n");
                EndContext();
            }
            );
            BeginContext(566, 138, true);
            WriteLiteral("<div class=\"content-wrapper\">\r\n    <!-- Content Header (Page header) -->\r\n    <section class=\"content-header\">\r\n        <h1>\r\n            ");
            EndContext();
            BeginContext(705, 24, false);
#line 22 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Home\UsersList.cshtml"
       Write(ViewBag.OrganizationName);

#line default
#line hidden
            EndContext();
            BeginContext(729, 1289, true);
            WriteLiteral(@" Kullanıcıları
            <small>EBackup</small>
        </h1>
        <ol class=""breadcrumb"">
            <li><a href=""#""><i class=""fa fa-dashboard""></i> Kullanıcılar</a></li>
        </ol>
    </section>
    <section class=""content"">

        <div class=""box"">
            <div class=""box-header"">
                <h3 class=""box-title"">Kullanıcı Listesi</h3>
            </div>

            <!-- /.box-header -->
            <div class=""box-body"">
                <div style=""margin-bottom:10px; text-align:right;"">
                    <a class=""btn btn-success ModalPopup"" data-post-url=""/Home/UsersNew"" data-height=""320px"">Yeni Kullanıcı Ekle</a>
                </div>
                <table id=""dtDevices"" class=""table table-bordered table-striped"">
                    <thead>
                        <tr>
                            <th>Kullanıcı Adı</th>
                            <th>Ad Soyad</th>
                            <th>Email</th>
                            <th>Aktiflik</th>");
            WriteLiteral(@"
                            <th>İşlem</th>
                        </tr>
                    </thead>
                    <tbody></tbody>

                </table>
            </div>
            <!-- /.box-body -->
        </div>

    </section>
</div>");
            EndContext();
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
