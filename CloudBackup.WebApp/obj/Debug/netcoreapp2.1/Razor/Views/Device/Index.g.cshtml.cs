#pragma checksum "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Device\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8dc233ebf524bf133de6543a0eb991b93c9edef5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Device_Index), @"mvc.1.0.view", @"/Views/Device/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Device/Index.cshtml", typeof(AspNetCore.Views_Device_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8dc233ebf524bf133de6543a0eb991b93c9edef5", @"/Views/Device/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5558d39de9a3d8e55d1c68a558d8ac6f9591bca", @"/Views/_ViewImports.cshtml")]
    public class Views_Device_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(28, 369, true);
                WriteLiteral(@"
    <script src=""../../bower_components/datatables.net/js/jquery.dataTables.min.js""></script>
    <script src=""../../bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js""></script>
    <script src=""/Scripts/device_index.js""></script>
    <script>
        $(document).ready(function () {
            device_index.init();
        });
    </script>
");
                EndContext();
            }
            );
            DefineSection("Heads", async() => {
                BeginContext(416, 112, true);
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css\">\r\n");
                EndContext();
            }
            );
            BeginContext(531, 1488, true);
            WriteLiteral(@"<div class=""content-wrapper"">
    <!-- Content Header (Page header) -->
    <section class=""content-header"">
        <h1>
            Cihazlar
            <small>EBackup</small>
        </h1>
        <ol class=""breadcrumb"">
            <li><a href=""#""><i class=""fa fa-dashboard""></i> Cihazlar</a></li>
        </ol>
    </section>
    <section class=""content"">

        <div class=""box"">
            <div class=""box-header"">
                <h3 class=""box-title"">Cihaz Listesi</h3>
            </div>
            
            <!-- /.box-header -->
            <div class=""box-body"">
                <div style=""margin-bottom:10px; text-align:right;"">
                    <a  class=""btn btn-success ModalPopup"" data-post-url=""/Device/New"" data-height=""320px"">Yeni Cihaz Ekle</a>
                </div>
                <table id=""dtDevices"" class=""table table-bordered table-striped"">
                    <thead>
                        <tr>
                            <th>Cihaz İsmi</th>
         ");
            WriteLiteral(@"                   <th>Son Yapılan İşlem Tarihi</th>
                            <th>Cihaz Kayıt Tarihi</th>
                            <th>Durum</th>
                            <th>İşlem</th>
                        </tr>
                    </thead>
                    <tbody>
                      
                    </tbody>

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
