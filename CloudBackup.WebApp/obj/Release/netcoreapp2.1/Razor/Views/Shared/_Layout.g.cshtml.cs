#pragma checksum "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "283cbe474cb18dfdb3cff3b436fa4692f0710e0e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Layout), @"mvc.1.0.view", @"/Views/Shared/_Layout.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_Layout.cshtml", typeof(AspNetCore.Views_Shared__Layout))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"283cbe474cb18dfdb3cff3b436fa4692f0710e0e", @"/Views/Shared/_Layout.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5558d39de9a3d8e55d1c68a558d8ac6f9591bca", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Layout : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/sweetalert/sweetalert2.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/bootbox/bootbox.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Scripts/common.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("hold-transition sidebar-mini skin-green"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 25, true);
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");
            EndContext();
            BeginContext(25, 2039, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e6ffe93557b54b2f9693733716856f97", async() => {
                BeginContext(31, 1913, true);
                WriteLiteral(@"
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <title>EBackup Yedekleme Platformu</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content=""width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"" name=""viewport"">
    <!-- Bootstrap 3.3.7 -->
    <link rel=""stylesheet"" href=""/bower_components/bootstrap/dist/css/bootstrap.min.css"">
    <!-- Font Awesome -->
    <link rel=""stylesheet"" href=""/bower_components/font-awesome/css/font-awesome.min.css"">
    <!-- Ionicons -->
    <link rel=""stylesheet"" href=""/bower_components/Ionicons/css/ionicons.min.css"">
    <!-- Theme style -->
    <link rel=""stylesheet"" href=""/dist/css/AdminLTE.min.css"">
    <!-- AdminLTE Skins. Choose a skin from the css/skins
         folder instead of downloading all of them to reduce the load. -->
    <link rel=""stylesheet"" href=""/dist/css/skins/_all-skins.min.css"">


    <!-- Date Picker -->
    <link rel=""stylesheet"" href=""/bower_co");
                WriteLiteral(@"mponents/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"">
    <!-- Daterange picker -->
    <link rel=""stylesheet"" href=""/bower_components/bootstrap-daterangepicker/daterangepicker.css"">
    <!-- bootstrap wysihtml5 - text editor -->
    <link rel=""stylesheet"" href=""/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src=""https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js""></script>
    <script src=""https://oss.maxcdn.com/respond/1.4.2/respond.min.js""></script>
    <![endif]-->
    <!-- Google Font -->
    <link rel=""stylesheet"" href=""https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic"">

    ");
                EndContext();
                BeginContext(1944, 65, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "87a7147d1cb84c638f4f3c7bd7b6e0e5", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2009, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(2016, 39, false);
#line 38 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml"
Write(RenderSection("Heads", required: false));

#line default
#line hidden
                EndContext();
                BeginContext(2055, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2064, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(2066, 16505, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5bba2fef80dc4d77ba1bf55e8eff4015", async() => {
                BeginContext(2120, 894, true);
                WriteLiteral(@"
    <div class=""wrapper"">
        <header class=""main-header"">
            <!-- Logo -->
            <a href=""/Home/Dashboard"" class=""logo"">
                <!-- mini logo for sidebar mini 50x50 pixels -->
                <span class=""logo-mini""><b>E</b>BK</span>
                <!-- logo for regular state and mobile devices -->
                <span class=""logo-lg""><b>E</b>Backup</span>
            </a>
            <!-- Header Navbar: style can be found in header.less -->
            <nav class=""navbar navbar-static-top"">
                <!-- Sidebar toggle button-->
                <a href=""#"" class=""sidebar-toggle"" data-toggle=""push-menu"" role=""button"">
                    <span class=""sr-only"">Toggle navigation</span>
                </a>
                <div class=""navbar-custom-menu"">
                    <ul class=""nav navbar-nav"">
                       
");
                EndContext();
                BeginContext(10742, 1019, true);
                WriteLiteral(@"
                        <li class=""dropdown user user-menu"">
                            <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown"">
                                <img src=""/dist/img/user2-160x160.jpg"" class=""user-image"" alt=""User Image"">
                                <span class=""hidden-xs"">İsmail ERDEN</span>
                            </a>
                            <ul class=""dropdown-menu"">
                                <!-- User image -->
                                <li class=""user-header"">
                                    <img src=""/dist/img/user2-160x160.jpg"" class=""img-circle"" alt=""User Image"">
                                    <p>
                                      
                                       
                                    </p>
                                </li>
                                <!-- Menu Body -->
                                <!-- Menu Footer-->
                                <li class=""user-footer"">
");
                EndContext();
                BeginContext(11989, 3440, true);
                WriteLiteral(@"                                    <div class=""pull-right"">
                                        <a href=""/Home/LogOut"" class=""btn btn-default btn-flat"">Çıkış</a>
                                    </div>
                                </li>
                            </ul>
                        </li>
                        <!-- Control Sidebar Toggle Button -->

                    </ul>
                </div>
            </nav>
        </header>
        <!-- Left side column. contains the logo and sidebar -->
        <aside class=""main-sidebar"">
            <!-- sidebar: style can be found in sidebar.less -->
            <section class=""sidebar"">
                <!-- Sidebar user panel -->
                <!-- sidebar menu: : style can be found in sidebar.less -->
                <ul class=""sidebar-menu"" data-widget=""tree"">
                    <li class=""header"">Ana Menü</li>

                    <li>
                        <a href=""/Home/Dashboard"">
                       ");
                WriteLiteral(@"     <i class=""fa fa-dashboard""></i> <span>Anasayfa</span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href=""/Device/Index"">
                            <i class=""fa fa-th""></i> <span>Cihazlar</span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href="""">
                            <i class=""fa fa-download""></i> <span>İndirmeler</span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </spa");
                WriteLiteral(@"n>
                        </a>
                    </li>
                    <li>
                        <a href=""/Platform/Index"">
                            <i class=""fa fa-download""></i> <span>Platformlar</span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href=""/Device/AllJobLog"">
                            <i class=""fa fa-gears""></i> <span>İşlem Geçmişi </span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </span>
                        </a>
                    </li>
                    <li>
                        <a href=""/Core/SSS"">
                            <i class=""fa fa-question""></i> <span>Sıkça Sorulan ");
                WriteLiteral(@"Sorular </span>
                            <span class=""pull-right-container"">
                                <small class=""label pull-right bg-green""></small>
                            </span>
                        </a>
                    </li>
                </ul>
            </section>
            <!-- /.sidebar -->
        </aside>


        ");
                EndContext();
                BeginContext(15430, 12, false);
#line 274 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml"
   Write(RenderBody());

#line default
#line hidden
                EndContext();
                BeginContext(15442, 183, true);
                WriteLiteral("\r\n        <footer class=\"main-footer\">\r\n            <div class=\"pull-right hidden-xs\">\r\n                <b>Version</b> 2.4.0\r\n            </div>\r\n            <strong>Copyright &copy; ");
                EndContext();
                BeginContext(15626, 17, false);
#line 279 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml"
                                Write(DateTime.Now.Year);

#line default
#line hidden
                EndContext();
                BeginContext(15643, 2567, true);
                WriteLiteral(@" EBackup Cloud</strong> Tüm Hakları Saklıdır.
        </footer>
      
      
    </div>



    <div class=""modal fade"" id=""modal-"">
        
        <div class=""modal-dialog"">
          
            <div class=""modal-content"">
                
                <iframe id=""iframeModalBox"" frameborder=""0""  width=""100%""></iframe>
                
            </div>
            
        </div>
        
    </div>


    <!-- ./wrapper -->
    <!-- jQuery 3 -->
    <script src=""/bower_components/jquery/dist/jquery.min.js""></script>
    <!-- jQuery UI 1.11.4 -->
    <script src=""/bower_components/jquery-ui/jquery-ui.min.js""></script>
    <!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
    <script>
        $.widget.bridge('uibutton', $.ui.button);
    </script>
    <!-- Bootstrap 3.3.7 -->
    <script src=""/bower_components/bootstrap/dist/js/bootstrap.min.js""></script>
    <!-- Morris.js charts -->
    <script src=""/bower_components/raphael/raphael.min.js""></sc");
                WriteLiteral(@"ript>
    <script src=""/bower_components/morris.js/morris.min.js""></script>
    <!-- Sparkline -->
    <script src=""/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js""></script>
    <!-- jvectormap -->
    <script src=""/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js""></script>
    <script src=""/plugins/jvectormap/jquery-jvectormap-world-mill-en.js""></script>
    <!-- jQuery Knob Chart -->
    <script src=""/bower_components/jquery-knob/dist/jquery.knob.min.js""></script>
    <!-- daterangepicker -->
    <script src=""/bower_components/moment/min/moment.min.js""></script>
    <script src=""/bower_components/bootstrap-daterangepicker/daterangepicker.js""></script>
    <!-- datepicker -->
    <script src=""/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js""></script>
    <!-- Bootstrap WYSIHTML5 -->
    <script src=""/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js""></script>
    <!-- Bootstrap time Picker -->
    <link rel=""stylesheet"" href=""../../plu");
                WriteLiteral(@"gins/timepicker/bootstrap-timepicker.min.css"">
    <!-- Slimscroll -->
    <script src=""/bower_components/jquery-slimscroll/jquery.slimscroll.min.js""></script>
    <!-- FastClick -->
    <script src=""/bower_components/fastclick/lib/fastclick.js""></script>
    <!-- AdminLTE App -->
    <script src=""/dist/js/adminlte.min.js""></script>
    <!-- AdminLTE dashboard demo (This is only for demo purposes) -->
    <script src=""/dist/js/pages/dashboard.js""></script>
    <!-- AdminLTE for demo purposes -->


    ");
                EndContext();
                BeginContext(18210, 48, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "363fb05f4afb42269b232fa6e2a0d7ec", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(18258, 52, true);
                WriteLiteral("\r\n    <script src=\"/dist/js/demo.js\"></script>\r\n    ");
                EndContext();
                BeginContext(18310, 43, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a72ef305ac3f4afb887caa3107aae27f", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(18353, 95, true);
                WriteLiteral("\r\n\r\n    <script>\r\n        $(document).ready(function () {\r\n            common.init();\r\n        ");
                EndContext();
                BeginContext(18449, 34, false);
#line 350 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml"
   Write(Html.Raw(TempData["Notification"]));

#line default
#line hidden
                EndContext();
                BeginContext(18483, 37, true);
                WriteLiteral(";\r\n        });\r\n    </script>\r\n\r\n    ");
                EndContext();
                BeginContext(18521, 41, false);
#line 354 "C:\Kişisel\Projeler\CloudBackup\CloudBackup.WebApp\Views\Shared\_Layout.cshtml"
Write(RenderSection("Scripts", required: false));

#line default
#line hidden
                EndContext();
                BeginContext(18562, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(18571, 17, true);
            WriteLiteral("\r\n</html>\r\n\r\n\r\n\r\n");
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
