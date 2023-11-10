using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using CMS.LicenseProvider;
using CMS.GlobalHelper;
using CMS.CMSHelper;
using CMS.UIControls;

public partial class CMSMessages_BannedIP : MessagePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.titleElem.TitleText = GetString("banip.cmsmessagesbantitle");
        this.titleElem.TitleImage = GetImageUrl("Others/Messages/denied.png");

        lblMessage.Text = GetString("banip.banmessage");
    }
}
