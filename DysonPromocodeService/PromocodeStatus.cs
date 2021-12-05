﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Этот исходный код был создан с помощью wsdl, версия=4.8.3928.0.
// 
namespace DysonPromocodeService
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "PromocodeStatusSoapBinding", Namespace = "https://crm1ctest.dyson.ru/CRM_TEST")]
    public partial class PromocodeStatus : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback GetStatusOperationCompleted;

        /// <remarks/>
        public PromocodeStatus()
        {
            this.Url = "https://crm1ctest.dyson.ru/CRM_TEST/ws/PromocodeStatus.1cws";
        }

        /// <remarks/>
        public event GetStatusCompletedEventHandler GetStatusCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://crm1ctest.dyson.ru/CRM_TEST#PromocodeStatus:GetStatus", RequestNamespace = "https://crm1ctest.dyson.ru/CRM_TEST", ResponseNamespace = "https://crm1ctest.dyson.ru/CRM_TEST", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public string GetStatus(string Promocode, [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)] string ArticleList)
        {
            object[] results = this.Invoke("GetStatus", new object[] {
                        Promocode,
                        ArticleList});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetStatus(string Promocode, string ArticleList, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetStatus", new object[] {
                        Promocode,
                        ArticleList}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetStatus(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void GetStatusAsync(string Promocode, string ArticleList)
        {
            this.GetStatusAsync(Promocode, ArticleList, null);
        }

        /// <remarks/>
        public void GetStatusAsync(string Promocode, string ArticleList, object userState)
        {
            if ((this.GetStatusOperationCompleted == null))
            {
                this.GetStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatusOperationCompleted);
            }
            this.InvokeAsync("GetStatus", new object[] {
                        Promocode,
                        ArticleList}, this.GetStatusOperationCompleted, userState);
        }

        private void OnGetStatusOperationCompleted(object arg)
        {
            if ((this.GetStatusCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatusCompleted(this, new GetStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    public delegate void GetStatusCompletedEventHandler(object sender, GetStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}
