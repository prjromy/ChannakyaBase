using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.CustomHelper
{
    public static class CustomHelper
    {
        private static ChannakyaBaseEntities db = new ChannakyaBaseEntities();
        public static MvcHtmlString Paging(this HtmlHelper helper, string action, string controller, int pageCount, int pageNo, int pageSize)
        {
            StringBuilder sb = new System.Text.StringBuilder();

            //sb.Append("<div class='box-body no-padding'>");
            sb.Append("<div class='btn-groups modal-footer' style='padding: 5px'>");
            sb.Append("<div class='form-group col-md-12 paging' style='margin:0px;padding-left:0px;' id ='paging' controller = '" + controller + "' action = '" + action + "' container = '' pageCount='" + pageCount + "'  >");

            string isFirstPage = "";
            string isLastPage = "";
            if (pageNo == 1)
            {
                isFirstPage = "disabled='disabled'";
            }
            if (pageNo == pageCount || pageCount == 0)
            {
                isLastPage = "disabled='disabled'";
            }
            sb.Append("<div class='col-md-12 startDiv' style='display:inline-flex;padding-left:0px;'>");
            sb.Append("<input type = 'button' value = '<<' id = 'btnFirst' class='btn btn-info btn-xs btnPaging' " + isFirstPage + "/>");
            sb.Append("<input type = 'button' value = '<' id = 'btnPrev' class='btn btn-info btn-xs btnPaging'  style='margin-left:2px;' " + isFirstPage + " />&nbsp");
            sb.Append("<div class='col-md-0 hidden-xs'>Page</div>");
            sb.Append("<input type='text' id='pageNo' class='form-control txtPaging'  style='max-width:75px;max-height:24px;text-align:center;padding:0px;' value='" + pageNo + "'/> ");
            sb.Append("<span style='display:inline-block'>&nbspof&nbsp;" + pageCount + "&nbsp</span>");
            sb.Append("<input type = 'button' value = ' > ' id = 'btnNxt' class='btn btn-info btn-xs btnPaging'  " + isLastPage + " />");
            sb.Append("<input type = 'button' value = '>>' id = 'btnLast' class='btn btn-info btn-xs btnPaging' style='margin-left:2px;'  " + isLastPage + " />");
            sb.Append("<div class='col-md-5'>");
            sb.Append("Page Size:<input type='text' id='pageSize' class='form-control txtPaging' style='max-width:30px;height:24px;text-align:center;display:inline-block;padding:0px;' value='" + pageSize + "'> ");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div id='erormsg'  class='alert-danger' style='display:none'>");
            sb.Append("<p>No page!! Moved to first page</p>");
            sb.Append("</div>");
            sb.Append("<div id='erormsgpagesize'  class='alert-danger' style='display:none'>");
            sb.Append("<p>Can not be less than 1!!! setting to default</p>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString CustomerSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return CustomerSearch(html, expression, ECustomerSearchType.AccountOpen, TypeOfCustomer.Customer.GetDescription());
        }
        public static MvcHtmlString CustomerSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, ECustomerSearchType ECustomerSearchTypes)
        {
            return CustomerSearch(html, expression, ECustomerSearchTypes, TypeOfCustomer.Customer.GetDescription());
        }
        public static MvcHtmlString CustomerSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, ECustomerSearchType ECustomerSearchTypes, string customerType)
        {
            MvcHtmlString mvcHtml = default(MvcHtmlString);
            StringBuilder htmlBuilder = new StringBuilder();
            string mode = "";
            if (ECustomerSearchTypes == ECustomerSearchType.Limited)
            {
                mode = ECustomerSearchType.Limited.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.Unlimited)
            {
                mode = ECustomerSearchType.Unlimited.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.SingleType)
            {
                mode = ECustomerSearchType.SingleType.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.AccountOpen)
            {
                mode = ECustomerSearchType.AccountOpen.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.CompanyOnly)
            {
                mode = ECustomerSearchType.CompanyOnly.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.CustomerOnly)
            {
                mode = ECustomerSearchType.CustomerOnly.GetDescription();
            }
            else if (ECustomerSearchTypes == ECustomerSearchType.VerifiedRegisteredLoanList) {
                mode = ECustomerSearchType.VerifiedRegisteredLoanList.GetDescription();


            }
            else
            {
                mode = ECustomerSearchType.All.GetDescription();
            }

            //var items = expression.Compile()(html.ViewData.Model);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var cntrlName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var cntrlId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName);
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            htmlBuilder.AppendFormat(@"<div id='pop-up-div' class='modal fade' role='dialog'></div>");
            htmlBuilder.AppendFormat(@"<div class='box-tools'>");
            htmlBuilder.AppendFormat(@"<div class='input-group input-group-sm pull-right CommonSearchDiv'>");
            htmlBuilder.AppendFormat(@"<select class='chzn-select chosen-select form-control multiselectCustomer' id='" + cntrlName + "' multiple='' name='" + cntrlName + "' placeholder='Customer:'></select>");
            htmlBuilder.AppendFormat(@" <div class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btncustomersearch' id='btncustomersearch' class='btn btn-default btncustomersearch' style='margin-left: 0px;height: 29px !important;' mode='" + mode + "' customerType='" + customerType + "'><i class='fa fa-search'></i></button>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
            mvcHtml = MvcHtmlString.Create(htmlBuilder.ToString());

            return mvcHtml;
        }
        public static MvcHtmlString LocationSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {

            MvcHtmlString mvcHtml = default(MvcHtmlString);
            StringBuilder htmlBuilder = new StringBuilder();

            string displayValue = "";
            //var items = expression.Compile()(html.ViewData.Model);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var cntrlName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var cntrlId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName);
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;

            //var htmlFieldName1 = ExpressionHelper.GetExpressionText(expression1);
            //var cntrlName1 = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName1);
            //var cntrlId1= html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName1);
            //var value1= ModelMetadata.FromLambdaExpression(expression1, html.ViewData).Model;

            if (Convert.ToInt32(value) != 0)
            {
                var staffRow = db.FGetLocationTB().AsQueryable().Where(x => x.LId.ToString() == value.ToString()).ToList();
                if (staffRow.Count() == 1)
                {
                    displayValue = staffRow[0].LocationName;
                }

            }


            //var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            //var description = metadata.Description;
            htmlBuilder.AppendFormat(@"<div id='location-search-container' class='location-search-container' isactive=''>");
            htmlBuilder.AppendFormat(@"<input type='hidden' id='locationid' class='locationid' name='{0}' value='{1}' />", cntrlName, value);
            //htmlBuilder.AppendFormat(@"<input style='width:170px;border-radius:5px;height:25px;' type='text' id='locationname' class='locationname' name='locationname' placeholder='Search Location' value='{0}' />", displayValue);

            htmlBuilder.AppendFormat(@"<input style='display:inline;' type='text' id='locationname' class='form-control locationname' name='locationname' placeholder='Search Location' value='{0}' data-toggle='tooltip' title='{0}'  />", displayValue);
            htmlBuilder.AppendFormat(@"<script>");
            //htmlBuilder.AppendFormat(@"$(document).ready(function(){");
           // htmlBuilder.AppendFormat(@"$(data-toggle = \"tooltip\"");
            //htmlBuilder.AppendFormat(@"$('[data-toggle=\"tooltip\"]').tooltip();");
            //htmlBuilder.AppendFormat(@"");    
            //htmlBuilder.AppendFormat(@"});");
            htmlBuilder.AppendFormat(@"</script>");



            htmlBuilder.AppendFormat(@"<button type='button' name='btnlocationsearch' id='btnlocationsearch' class='btnlocationsearch' style='border:none;background-color:white;margin-left:-35px;' data-toggle='modal' ><i class='fa fa-search fa-lg'></i></button>");

            //htmlBuilder.AppendFormat(@"<i class='fa fa-search' id='btnemployeesearch'></i>");
            //htmlBuilder.AppendFormat(@" <input type='button' name='btnlocationsearch' id='btnlocationsearch' class='btn btn-info btn-xs' value='search' data-toggle='modal' data-target='#dialoglocation' />");
            htmlBuilder.AppendFormat(@"<div id='locationsearchresult' class='locationsearchresult'>
                            <div id='dialoglocation' class='modal fade dialoglocation'>
                            <div class='modal-dialog modal-lg'>
                            <div class='modal-content'>
                            <div class='modal-header'><button type='button' class='close' data-dismiss='modal'>&times;</button></div>
                            <div class='modal-body details'></div>
                            <div class='modal-footer'></div>
                            </div></div>
                            </div></div>");
            htmlBuilder.AppendFormat(@"</div>");

            mvcHtml = MvcHtmlString.Create(htmlBuilder.ToString());

            return mvcHtml;

        }
        public static MvcHtmlString SearchSorting(this HtmlHelper helper, string action, string controller, List<SelectListItem> sortFilter = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='form-group searchsorting' style='margin:0px;padding-left:0px;' id = 'searchsorting' controller = '" + controller + "' action = '" + action + "' container = '' pageCount=''  >");
            sb.Append("<div style='display:inline-flex;padding-left:0px;'>");
            if (sortFilter != null)
            {
                var selected = "";
                sb.Append("<select id='sortFilter' class='form-control sortFilter' style='max-width:150px;max-height:24px;padding:0px;'>");
                foreach (var item in sortFilter)
                {
                    selected = item.Selected ? "selected" : "";
                    sb.Append("<option value='" + item.Value + "' " + selected + " >" + item.Text + "</option>");
                }
                sb.Append("</select>");
            }
            sb.Append("<input type='text' id='search' class='form-control search' style='max-width:168px;height:24px;text-align:center;' placeholder='Search'/>");
            //sb.Append("<button id='_exportTo' type='button' class='_exportTo' style='border:none;background-color:white;' ><i class='fa fa-file-excel-o fa-lg'></i></button>");

            sb.Append("</div>");

            //
            sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static class helperFunction
        {
            public static List<SelectListItem> getSelectedListItem(string column, string selected)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                string[] array = column.Split(',');

                foreach (var item in array)
                {
                    //if (selected == item)
                    //{
                    list.Add(new SelectListItem { Text = item, Value = item, Selected = true });
                    //}
                    //else
                    //{

                    //    list.Add(new SelectListItem { Text = item, Value = item });
                    //}
                }
                return list;
            }
        }
        public static MvcHtmlString EmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            int branchId = 0;
            string employeeName = "";
            return EmployeeSearch(html, expression, branchId, employeeName, "", EEmployeeOrShare.Employee.GetDescription());
        }
        public static MvcHtmlString EmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string employeeName = "", string employeeChange = "")
        {
            int branchId = 0;
            return EmployeeSearch(html, expression, branchId, employeeName, employeeChange, EEmployeeOrShare.Employee.GetDescription());
        }
        public static MvcHtmlString EmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string employeeName = "", string employeeChange = "", string employeeOrShare="")
        {
            int branchId = 0;
            return EmployeeSearch(html, expression, branchId, employeeName, employeeChange, employeeOrShare);
        }
        public static MvcHtmlString EmployeeSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int branchId, string employeeName, string employeeChange,string employeeOrShare)
        {
            MvcHtmlString mvcHtml = default(MvcHtmlString);
            StringBuilder htmlBuilder = new StringBuilder();
            //var items = expression.Compile()(html.ViewData.Model);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var cntrlName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var cntrlId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName);
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;
            htmlBuilder.AppendFormat(@"<div id='employee-pop-up-div' class='modal fade' role='dialog'></div>");
            htmlBuilder.AppendFormat(@"<div class='box-tools'>");
            htmlBuilder.AppendFormat(@"<div class='input-group input-group-sm pull-right CommonSearchDiv'>");
            htmlBuilder.AppendFormat(@"<input style='display:inline;' type='text' id='" + cntrlName + "' class='form-control employeeName' name='employeeName' placeholder='Search "+ employeeOrShare + "' value='" + employeeName + "' />");

            htmlBuilder.AppendFormat(@"<input type='hidden' id='" + cntrlName + "' class='employeeId' name='" + cntrlName + "' value='" + value + "'  employeeChange='" + employeeChange + "' />");
            htmlBuilder.AppendFormat(@" <div class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btnemployeesearch' id='btnemployeesearch' branchId='" + branchId + "' class='btn btn-default btnemployeesearch' showType='"+ employeeOrShare + "' style='margin-left: 0px;' ><i class='fa fa-search'></i></button>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
            mvcHtml = MvcHtmlString.Create(htmlBuilder.ToString());

            return mvcHtml;
        }
        public static MvcHtmlString AccountNumberSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool withDetails = true)
        {
            string accountNumber = "";
            return AccountNumberSearch(html, expression, EAccountDetailsShow.Deposit.GetDescription(), EAccountFilter.DepositAccept.GetDescription(), EAccountType.Normal.GetDescription(), accountNumber, withDetails);
        }


        public static MvcHtmlString AccountNumberSearch<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string eaccDetails = "", string accountFilter = "", string accountType = "", string accountNumber = "",bool withDetails = true)
        {
            MvcHtmlString mvcHtml = default(MvcHtmlString);
            StringBuilder htmlBuilder = new StringBuilder();

            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var cntrlName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            var cntrlId = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(cntrlName);
            var value = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;


            htmlBuilder.AppendFormat(@"<div id='account-pop-up-div' class='modal fade' role='dialog'></div>");
            //htmlBuilder.AppendFormat(@"<div class='account-holder-info-name'> <div class='form-control'> <span class='account-holder-name'></span></div></div>");
            htmlBuilder.AppendFormat(@"<div class='box-tools'>");
            //htmlBuilder.AppendFormat(@"");
            htmlBuilder.AppendFormat(@"<div class='input-group input-group-sm pull-right account-number-div'>");
            htmlBuilder.AppendFormat(@"<input style='display:inline;' type='text' id='" + cntrlName + "' class='form-control account-aumber' name='accountNumber' placeholder='Account Number' value='" + accountNumber + "' showwith='" + eaccDetails + "' accountFilter='" + accountFilter + "' accountType='" + accountType + "'withDetails='" + withDetails + "' />");
            htmlBuilder.AppendFormat(@"<input type='hidden' id='" + cntrlName + "' class='account-id' name='" + cntrlName + "' value='" + value + "' />");
            

            htmlBuilder.AppendFormat(@" <div class='input-group-btn'>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btn-account-open-search' id='btn-account-openSearch' class='btn btn-default btn-account-open-search' style='margin-left: 0px;' ><i class='fa fa-search'></i></button>");
            htmlBuilder.AppendFormat(@"<button type='button' name='btn-account-view-details' id='btn-account-view-details-user-Account' class='btn btn-default hidden btn-account-view-details-button' style='margin-left: 0px;' Action='Show'> <i class='fa fa-toggle-on' title='show details'></i><i class='fa fa-toggle-off hidden' title='hide details'></i></button>");
            htmlBuilder.AppendFormat(@"</div>");
            htmlBuilder.AppendFormat(@"</div>");
           
            htmlBuilder.AppendFormat(@"</div>");
          
            mvcHtml = MvcHtmlString.Create(htmlBuilder.ToString());

            return mvcHtml;
        }
    }


}