﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18444
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SHCollege.Properties {
    using System;
    
    
    /// <summary>
    ///   用於查詢當地語系化字串等的強類型資源類別。
    /// </summary>
    // 這個類別是自動產生的，是利用 StronglyTypedResourceBuilder
    // 類別透過 ResGen 或 Visual Studio 這類工具。
    // 若要加入或移除成員，請編輯您的 .ResX 檔，然後重新執行 ResGen
    // (利用 /str 選項)，或重建您的 VS 專案。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   傳回這個類別使用的快取的 ResourceManager 執行個體。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SHCollege.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   覆寫目前執行緒的 CurrentUICulture 屬性，對象是所有
        ///   使用這個強類型資源類別的資源查閱。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查詢類型 System.Drawing.Bitmap 的當地語系化資源。
        /// </summary>
        internal static System.Drawing.Bitmap Import_Image {
            get {
                object obj = ResourceManager.GetObject("Import_Image", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查詢類似 &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;?xml-stylesheet type=&quot;text/xsl&quot; href=&quot;format.xsl&quot; ?&gt;
        ///&lt;ValidateRule Name=&quot;匯入學測報名序號&quot;&gt;
        ///  &lt;DuplicateDetection&gt;
        ///    &lt;Detector Name=&quot;組合鍵值&quot;&gt;
        ///      &lt;Field Name=&quot;學號&quot; /&gt;
        ///      &lt;Field Name=&quot;報名序號&quot; /&gt;
        ///    &lt;Field Name=&quot;學測班級&quot; /&gt;
        ///    &lt;Field Name=&quot;學測座號&quot; /&gt;
        ///    &lt;/Detector&gt;
        ///  &lt;/DuplicateDetection&gt;
        ///  &lt;FieldList&gt;
        ///
        ///    &lt;Field Required=&quot;True&quot; Name=&quot;學號&quot; Description=&quot;學號&quot;&gt;
        ///      &lt;Validate AutoCorrect=&quot;False&quot; Description=&quot;「學號」不允許空白。&quot; ErrorType=&quot;Error&quot; Validator=&quot;不可空白&quot; When=&quot;&quot; /&gt;
        /// </summary>
        internal static string ImportSATStudXML {
            get {
                return ResourceManager.GetString("ImportSATStudXML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類型 System.Drawing.Bitmap 的當地語系化資源。
        /// </summary>
        internal static System.Drawing.Bitmap Report {
            get {
                object obj = ResourceManager.GetObject("Report", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查詢類型 System.Byte[] 的當地語系化資源。
        /// </summary>
        internal static byte[] template {
            get {
                object obj = ResourceManager.GetObject("template", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   查詢類型 System.Drawing.Bitmap 的當地語系化資源。
        /// </summary>
        internal static System.Drawing.Bitmap 匯出 {
            get {
                object obj = ResourceManager.GetObject("匯出", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}