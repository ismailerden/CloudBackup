#pragma checksum "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Device\DeviceJobLog.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "04e5f84918917c13abab922215f154d379b0a08f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Device_DeviceJobLog), @"mvc.1.0.view", @"/Views/Device/DeviceJobLog.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Device/DeviceJobLog.cshtml", typeof(AspNetCore.Views_Device_DeviceJobLog))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"04e5f84918917c13abab922215f154d379b0a08f", @"/Views/Device/DeviceJobLog.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5558d39de9a3d8e55d1c68a558d8ac6f9591bca", @"/Views/_ViewImports.cshtml")]
    public class Views_Device_DeviceJobLog : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CloudBackup.Database.ViewModel.CreateModel.DevicePlanInsertModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            DefineSection("Scripts", async() => {
                BeginContext(99, 352, true);
                WriteLiteral(@"
    <script src=""../../bower_components/datatables.net/js/jquery.dataTables.min.js""></script>
    <script src=""../../bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js""></script>
    <script src=""/Scripts/device_devicejoblog.js""></script>
    <script>
        $(document).ready(function () {
            device_devicejoblog.init('");
                EndContext();
                BeginContext(452, 20, false);
#line 11 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Device\DeviceJobLog.cshtml"
                                 Write(ViewBag.DevicePlanId);

#line default
#line hidden
                EndContext();
                BeginContext(472, 33, true);
                WriteLiteral("\');\r\n        });\r\n    </script>\r\n");
                EndContext();
            }
            );
            DefineSection("Heads", async() => {
                BeginContext(524, 112, true);
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css\">\r\n");
                EndContext();
            }
            );
            BeginContext(639, 138, true);
            WriteLiteral("<div class=\"content-wrapper\">\r\n    <!-- Content Header (Page header) -->\r\n    <section class=\"content-header\">\r\n        <h1>\r\n            ");
            EndContext();
            BeginContext(778, 10, false);
#line 22 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Device\DeviceJobLog.cshtml"
       Write(Model.Name);

#line default
#line hidden
            EndContext();
            BeginContext(788, 2155, true);
            WriteLiteral(@" Kayıtları 
            <small>EBackup</small>
        </h1>
        <ol class=""breadcrumb"">
            <li><a href=""#""><i class=""fa fa-dashboard""></i> Kayıtlar</a></li>
        </ol>
    </section>
    <section class=""content"">

        <div class=""box"">
            <div class=""box-header"">
                <h3 class=""box-title"">Kayıtlar</h3>
            </div>

            <!-- /.box-header -->
            <div class=""box-body"">
               
                <table id=""dtDevices"" class=""table table-bordered table-striped"">
                    <thead>
                        <tr>
                            <th>İşlem Tarihi</th>
                            <th>Yapılan İşlem</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                30.08.2018 03:00:00
                            </td>
                            <td>Evdeki Bilgisayar</td>
             ");
            WriteLiteral(@"           </tr>
                        <tr>
                            <td>
                                30.08.2018 03:00:00
                            </td>
                            <td>Evdeki Bilgisayar</td>
                        </tr>
                        <tr>
                            <td>
                                30.08.2018 03:00:00
                            </td>
                            <td>Evdeki Bilgisayar</td>
                        </tr>
                        <tr>
                            <td>
                                30.08.2018 03:00:00
                            </td>
                            <td>Evdeki Bilgisayar</td>
                        </tr>
                        <tr>
                            <td>
                                30.08.2018 03:00:00
                            </td>
                            <td>Evdeki Bilgisayar</td>
                        </tr>



                    </tbody>

            ");
            WriteLiteral("    </table>\r\n            </div>\r\n            <!-- /.box-body -->\r\n        </div>\r\n\r\n    </section>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CloudBackup.Database.ViewModel.CreateModel.DevicePlanInsertModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
