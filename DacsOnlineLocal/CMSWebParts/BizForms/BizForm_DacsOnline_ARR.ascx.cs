using System;
using System.Web;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.PortalControls;
using CMS.WebAnalytics;
using DacsOnline.Model.Manager.Interfaces;
using DacsOnline.Model.Adadpters;
using CMS.FormControls;

public partial class CMSWebParts_BizForms_BizForm_DacsOnline_ARR: CMSAbstractWebPart
{
    #region "Properties"

    /// <summary>
    /// Gets or sets the form name of BizForm.
    /// </summary>
    public string BizFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("BizFormName"), "");
        }
        set
        {
            this.SetValue("BizFormName", value);
        }
    }


    /// <summary>
    /// Gets or sets the alternative form full name (ClassName.AlternativeFormName).
    /// </summary>
    public string AlternativeFormName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("AlternativeFormName"), "");
        }
        set
        {
            this.SetValue("AlternativeFormName", value);
        }
    }


    /// <summary>
    /// Gets or sets the site name.
    /// </summary>
    public string SiteName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("SiteName"), "");
        }
        set
        {
            this.SetValue("SiteName", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the WebPart use colon behind label.
    /// </summary>
    public bool UseColonBehindLabel
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("UseColonBehindLabel"), true);
        }
        set
        {
            this.SetValue("UseColonBehindLabel", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after validation failed.
    /// </summary>
    public string ValidationErrorMessage
    {
        get
        {
            return "Please scroll through the form to complete all highlighted mandatory fields.";// ValidationHelper.GetString(this.GetValue("ValidationErrorMessage"), "");
        }
        set
        {
            this.SetValue("ValidationErrorMessage", "Please scroll through the form to complete all highlighted mandatory fields.");//this.SetValue("ValidationErrorMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string TrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(this.GetValue("TrackConversionName"), "");
        }
        set
        {
            if (value.Length > 400)
            {
                value = value.Substring(0, 400);
            }
            this.SetValue("TrackConversionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion value used after successful registration.
    /// </summary>
    public double ConversionValue
    {
        get
        {
            return ValidationHelper.GetDouble(this.GetValue("ConversionValue"), 0);
        }
        set
        {
            this.SetValue("ConversionValue", value);
        }
    }

    #endregion


    #region "Methods"

    protected override void OnLoad(EventArgs e)
    {
        viewBiz.OnAfterSave += viewBiz_OnAfterSave;
        viewBiz.OnValidationFailed += viewBiz_OnValidationFailed;
        viewBiz.OnBeforeValidate += viewBiz_OnBeforeValidate;
        base.OnLoad(e);
    }


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
        
       
    }


    /// <summary>
    /// Reloads data for partial caching.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing)
        {
            // Do nothing
            viewBiz.StopProcessing = true;
        }
        else
        {
            // Set BizForm properties
            viewBiz.FormName = this.BizFormName;
            viewBiz.SiteName = this.SiteName;
            viewBiz.UseColonBehindLabel = this.UseColonBehindLabel;
            viewBiz.AlternativeFormFullName = this.AlternativeFormName;
            viewBiz.ValidationErrorMessage = this.ValidationErrorMessage;


            // Set the live site context
            if (viewBiz.BasicForm != null)
            {
                viewBiz.BasicForm.ControlContext.ContextName = CMS.SiteProvider.ControlContext.LIVE_SITE;
            }
            
            
        }
    }



    void viewBiz_OnValidationFailed(object sender, EventArgs e)
    {
        FormEngineUserControl firstControl = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("Title");
        this.Page.MaintainScrollPositionOnPostBack = false;
        this.Page.SetFocus(firstControl.ClientID);
    }

    void viewBiz_OnBeforeValidate(object sender, EventArgs e)
    {

        FormEngineUserControl BankAddressControl = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("BankAddress");
        FormEngineUserControl BankAccountTypeControl = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("BankAccountType");
        if ((BankAccountTypeControl.Value == "UBA" || BankAccountTypeControl.Value == "UBS") && BankAddressControl.Value =="")
        {
            BankAddressControl.Value = "N/A";
        }

        //string value = BankAccountTypeControl.SelectedValue.ToString();
        //string values = BankAddressControl.Text;
        
    }

 
    void viewBiz_OnAfterSave(object sender, EventArgs e)
    {
        if (this.TrackConversionName != String.Empty)
        {
            string siteName = CMSContext.CurrentSiteName;

            if (AnalyticsHelper.AnalyticsEnabled(siteName) && AnalyticsHelper.TrackConversionsEnabled(siteName) && !AnalyticsHelper.IsIPExcluded(siteName, HTTPHelper.UserHostAddress))
            {
                HitLogProvider.LogConversions(CMSContext.CurrentSiteName, CMSContext.PreferredCultureCode, this.TrackConversionName, 0, ConversionValue);
            }
        }
        Session["emailUser"] = this.viewBiz.BasicForm.GetDataValue("Email").ToString();

        SubsCribeToMailChimp();
    }

    private void SubsCribeToMailChimp()
    {
        string signup = this.viewBiz.BasicForm.GetDataValue("SignUp").ToString();

        if (signup == "Yes")
        {
            string fName = this.viewBiz.BasicForm.GetDataValue("FirstNames").ToString();
            string lName = this.viewBiz.BasicForm.GetDataValue("LastName").ToString();
            string email = this.viewBiz.BasicForm.GetDataValue("Email").ToString();
            ISubscribeEmail SubscribeAdapter = new MailChimpAdapter();
            SubscribeAdapter.SubscribeUser(email, "HTML", fName, lName);
        }
    }

    //private void HideBankData()
    //{
       
       
    //    FormEngineUserControl bankAccountType = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("BankAccountType");
        
    //    FormEngineUserControl ukSortCode = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("UKSortCode");
    //    FormEngineUserControl rollNumber = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("RollNumber");
    //    FormEngineUserControl iBAN = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("IBAN");
    //    FormEngineUserControl swiftBic = (FormEngineUserControl)this.viewBiz.BasicForm.FindControl("SWIFT_BIC");
    //    if (bankAccountType.Value == "IBA")
    //    {
    //        ukSortCode.Enabled = false;
    //        rollNumber.Enabled = true;
    //        iBAN.Enabled = true;
    //        swiftBic.Enabled = true;
    //    }
    //    else if (bankAccountType.Value == "-1")
    //    {
    //        ukSortCode.Enabled = true;
    //        rollNumber.Enabled = true;
    //        iBAN.Enabled = true;
    //        swiftBic.Enabled = true;
    //    }
    //    else
    //    {
    //        ukSortCode.Enabled = true;
    //        rollNumber.Enabled = false;
    //        iBAN.Enabled = false;
    //        swiftBic.Enabled = false;
    //    }
    //}

    #endregion
}