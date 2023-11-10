using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.Messaging;
using CMS.CMSHelper;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_Messaging_Controls_SendMessage : CMSUserControl
{
    #region "Protected & private variables"

    protected string mMessageSubject = null;
    protected string mMessageBody = null;
    protected bool isAnonymousUser = false;

    private bool mAllowAnonymousUsers = true;
    private bool mAllowAnonymousRecipientSelection = true;
    private string mDefaultRecipient = "";
    private string mErrorMessage = "";
    private MessageInfo mMessage = null;

    #endregion


    #region "Public events"

    /// <summary>
    /// Event for send button click.
    /// </summary>
    public event EventHandler SendButtonClick;


    /// <summary>
    /// Event for close button click.
    /// </summary>
    public event EventHandler CloseButtonClick;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Display close button.
    /// </summary>
    public bool DisplayCloseButton
    {
        get
        {
            return btnClose.Visible;
        }
        set
        {
            btnClose.Visible = value;
        }
    }


    /// <summary>
    /// Related message ID.
    /// </summary>
    public int MessageId
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["MessageId"], 0);
        }
        set
        {
            ViewState["MessageId"] = value;
            if (value <= 0)
            {
                mMessage = null;
            }
        }
    }


    /// <summary>
    /// Message.
    /// </summary>
    public MessageInfo Message
    {
        get
        {
            if ((mMessage == null) && (MessageId > 0))
            {
                mMessage = MessageInfoProvider.GetMessageInfo(MessageId);
            }
            return mMessage;
        }
        set
        {
            mMessage = value;
            if (mMessage != null)
            {
                ViewState["MessageId"] = mMessage.MessageID;
            }
        }
    }


    /// <summary>
    /// Error message.
    /// </summary>
    public string ErrorMessage
    {
        get
        {
            return mErrorMessage;
        }
        set
        {
            mErrorMessage = value;
        }
    }


    /// <summary>
    /// Send message mode.
    /// </summary>
    public MessageActionEnum SendMessageMode
    {
        get
        {
            object sendMessageMode = ViewState["SendMessageMode"];
            return (sendMessageMode != null)
                       ? (MessageActionEnum)Enum.Parse(typeof(MessageActionEnum), sendMessageMode.ToString())
                       : MessageActionEnum.None;
        }
        set
        {
            ViewState["SendMessageMode"] = value;
        }
    }


    /// <summary>
    /// Paste original message.
    /// </summary>
    public bool PasteOriginalMessage
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["PasteOriginalMessage"], true);
        }
        set
        {
            ViewState["PasteOriginalMessage"] = value;
        }
    }


    /// <summary>
    /// True if anonymous users should be able to send messages.
    /// </summary>
    public bool AllowAnonymousUsers
    {
        get
        {
            return mAllowAnonymousUsers;
        }
        set
        {
            mAllowAnonymousUsers = value;
        }
    }


    /// <summary>
    /// True if anonymous users should be able to select recipient fo the messages.
    /// </summary>
    public bool AllowAnonymousRecipientSelection
    {
        get
        {
            return mAllowAnonymousRecipientSelection;
        }
        set
        {
            mAllowAnonymousRecipientSelection = value;
        }
    }


    /// <summary>
    /// Default recipient of the message.
    /// </summary>
    public string DefaultRecipient
    {
        get
        {
            return mDefaultRecipient;
        }
        set
        {
            mDefaultRecipient = value;
        }
    }


    /// <summary>
    /// Message subject.
    /// </summary>
    public string MessageSubject
    {
        get
        {
            return mMessageSubject;
        }
        set
        {
            mMessageSubject = value;
        }
    }


    /// <summary>
    /// Message body.
    /// </summary>
    public string MessageBody
    {
        get
        {
            return mMessageBody;
        }
        set
        {
            mMessageBody = value;
        }
    }


    /// <summary>
    /// Use prompt dialog.
    /// </summary>
    public bool UsePromptDialog
    {
        get
        {
            return ucBBEditor.UsePromptDialog;
        }
        set
        {
            ucBBEditor.UsePromptDialog = value;
        }
    }


    /// <summary>
    /// Information text.
    /// </summary>
    public string InformationText
    {
        get
        {
            return lblSendInfo.Text;
        }
    }


    /// <summary>
    /// Errror text.
    /// </summary>
    public string ErrorText
    {
        get
        {
            return lblSendError.Text;
        }
    }


    /// <summary>
    /// Send button.
    /// </summary>
    public LocalizedButton SendButton
    {
        get
        {
            return btnSendMessage;
        }
    }


    /// <summary>
    /// Cancel button.
    /// </summary>
    public LocalizedButton CancelButton
    {
        get
        {
            return btnClose;
        }
    }


    /// <summary>
    /// BB editor.
    /// </summary>
    public BBEditor BBEditor
    {
        get
        {
            return ucBBEditor;
        }
    }


    /// <summary>
    /// Box with subject.
    /// </summary>
    public TextBox SubjectBox
    {
        get
        {
            return txtSubject;
        }
    }


    /// <summary>
    /// Box with "From" item.
    /// </summary>
    public TextBox FromBox
    {
        get
        {
            return txtFrom;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Load page.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessageUserSelector.IsLiveSite = IsLiveSite;
        ReloadData();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ucBBEditor.TextArea.CssClass = "BodyField";

        // Dialogs mode
        if (DisplayCloseButton)
        {
            pnlButtons.CssClass += " PageFooterLine";
            pnlSubPanel.CssClass = "FloatRight";
        }

        // WAI validation
        lblToCaption.AssociatedControlClientID = ucMessageUserSelector.InputClientID;
    }

    #endregion


    #region "Other methods"

    /// <summary>
    /// Reload data.
    /// </summary>
    public void ReloadData()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            CurrentUserInfo currentUser = CMSContext.CurrentUser;

            // WAI validation
            lblText.AssociatedControlClientID = ucBBEditor.TextArea.ClientID;

            // Default settings
            lblSendInfo.Visible = false;
            lblSendError.Visible = false;
            pnlSendMessage.Visible = true;
            pnlNoUser.Visible = false;

            if (currentUser.IsAuthenticated() || AllowAnonymousUsers)
            {
                // Decide if the user is anonymous
                isAnonymousUser = currentUser.IsPublic();

                // Change controls
                if (isAnonymousUser)
                {
                    lblFrom.Visible = false;
                    txtFrom.Visible = true;
                }
                else
                {
                    lblFrom.Visible = true;
                    lblFrom.Text = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(currentUser.UserName, currentUser.FullName, currentUser.UserNickName, IsLiveSite));
                    txtFrom.Visible = false;
                }

                // Reacting on the message
                // Get message
                if (Message != null)
                {
                    UserInfo sender = UserInfoProvider.GetUserInfo(Message.MessageSenderUserID);
                    // Reply
                    switch (SendMessageMode)
                    {
                        case MessageActionEnum.Reply:
                            if (sender != null)
                            {
                                hdnUserId.Value = sender.UserID.ToString();

                                // Check if postback wasn't caused by send button to avoid overriding values
                                if (ControlsHelper.GetPostBackControl(Page) != btnSendMessage)
                                {
                                    // Set message subject
                                    MessageSubject += Message.MessageSubject;
                                    // Set message body
                                    if (currentUser.UserSignature.Trim() != string.Empty)
                                    {
                                        MessageBody += "\n\n\n" + currentUser.UserSignature;
                                    }
                                    if (PasteOriginalMessage)
                                    {
                                        MessageBody += "\n\n// " + Message.MessageBody.Replace("\n", "\n// ");
                                    }
                                }

                                // Set recipient
                                lblTo.Text = HTMLHelper.HTMLEncode(Functions.GetFormattedUserName(sender.UserName, sender.FullName, sender.UserNickName, IsLiveSite));
                                lblTo.Visible = true;
                                ucMessageUserSelector.Visible = false;
                            }
                            else
                            {
                                pnlNoUser.Visible = true;
                                pnlSendMessage.Visible = false;
                                lblNoUser.Text = GetString("SendMessage.NoUser");
                            }
                            break;

                        case MessageActionEnum.Forward:
                            // Check if postback wasn't caused by send button to avoid overriding values
                            if (ControlsHelper.GetPostBackControl(Page) != btnSendMessage)
                            {
                                MessageSubject += Message.MessageSubject;
                                string signature = (currentUser.UserSignature.Trim() != "") ? "\n\n\n" + currentUser.UserSignature : "";
                                MessageBody += signature + "\n\n// " + Message.MessageBody.Replace("\n", "\n//");
                            }

                            lblTo.Visible = false;
                            ucMessageUserSelector.Visible = true;
                            break;
                    }
                }
                // New message
                else if (SendMessageMode == MessageActionEnum.New)
                {
                    if (DefaultRecipient != "")
                    {
                        lblTo.Visible = true;
                        ucMessageUserSelector.Visible = false;

                        // Get default recipient name
                        string userName = UserInfoProvider.GetUserNameById(ValidationHelper.GetInteger(DefaultRecipient, 0));
                        userName = Functions.GetFormattedUserName(userName, this.IsLiveSite);

                        lblTo.Text = HTMLHelper.HTMLEncode(userName);
                        hdnUserId.Value = DefaultRecipient;
                    }
                    else
                    {
                        lblTo.Visible = false;
                        if (isAnonymousUser)
                        {
                            ucMessageUserSelector.Visible = AllowAnonymousRecipientSelection;
                        }
                        else
                        {
                            ucMessageUserSelector.Visible = true;
                        }
                    }

                    // Add signature
                    if (!isAnonymousUser)
                    {
                        if (currentUser.UserSignature.Trim() != "")
                        {
                            MessageBody += "\n\n\n" + currentUser.UserSignature;
                        }
                    }
                }

                // Initialize subject and body
                if (txtSubject.Text == string.Empty)
                {
                    txtSubject.Text = MessageSubject;
                }
                if (ucBBEditor.Text == string.Empty)
                {
                    ucBBEditor.Text = MessageBody;
                }
            }
            else
            {
                Visible = false;
            }
        }
    }


    private bool ValidateBody(string body)
    {
        return ((body.Trim() != string.Empty) && (body.Trim().ToLower() != CMSContext.CurrentUser.UserSignature.Trim().ToLower()));
    }

    #endregion


    #region "Button handling"

    protected void btnClose_Click(object sender, EventArgs e)
    {
        // External event
        if (CloseButtonClick != null)
        {
            CloseButtonClick(sender, e);
        }
    }


    protected void btnSendMessage_Click(object sender, EventArgs e)
    {
        // This is because of ASP.NET default behaviour
        // The first empty line was trimmed after each postback
        if (BBEditor.Text.StartsWith("\n"))
        {
            BBEditor.Text = "\n" + BBEditor.Text;
        }
        // Flood protection
        if (!FloodProtectionHelper.CheckFlooding(CMSContext.CurrentSiteName, CMSContext.CurrentUser))
        {
            CurrentUserInfo currentUser = CMSContext.CurrentUser;

            // Check banned IP
            if (BannedIPInfoProvider.IsAllowed(CMSContext.CurrentSiteName, BanControlEnum.AllNonComplete))
            {
                int recipientId = ucMessageUserSelector.Visible
                                      ? ucMessageUserSelector.SelectedUserID
                                      : ValidationHelper.GetInteger(hdnUserId.Value, 0);
                string message = string.Empty;
                string nickName = HTMLHelper.HTMLEncode(txtFrom.Text.Trim());
                if (!ValidateBody(DiscussionMacroHelper.RemoveTags(ucBBEditor.Text)))
                {
                    message = GetString("SendMessage.EmptyBody");
                }


                // Check sender nick name if anonymous
                if (isAnonymousUser && (nickName == string.Empty))
                {
                    message = GetString("SendMesage.NoNickName");
                }

                UserInfo recipient = null;

                // Check recipient
                if (recipientId == 0)
                {
                    if (string.IsNullOrEmpty(ucMessageUserSelector.UserNameTextBox.Text.Trim()))
                    {
                        message = GetString("SendMesage.NoRecipient");
                    }
                    else
                    {
                        message = GetString("SendMesage.UserDoesntExists");
                    }
                }
                else
                {
                    recipient = UserInfoProvider.GetUserInfo(recipientId);

                    // Normal users can't send message to user from other site except for global admin
                    if (!recipient.IsInSite(CMSContext.CurrentSiteName) && !currentUser.IsGlobalAdministrator)
                    {
                        message = GetString("SendMesage.UserDoesntExists");
                    }

                    int defRecipientId = ValidationHelper.GetInteger(DefaultRecipient, 0);

                    // If default recipient selected and is same as message recipient, skip check on hidden users
                    if (recipient.UserID != defRecipientId)
                    {
                        // Hide hidden, disabled and not approved user for all users except for global admins and public user for all users
                        if ((IsLiveSite && (!recipient.Enabled || recipient.UserIsHidden || recipient.UserSettings.UserWaitingForApproval) && !currentUser.IsGlobalAdministrator) || (recipient.UserName.ToLower() == "public"))
                        {
                            message = GetString("SendMesage.UserDoesntExists");
                        }
                    }
                }

                if (message == string.Empty)
                {
                    // Send message
                    try
                    {
                        // Check if current user is in recipient's ignore list
                        bool isIgnored = IgnoreListInfoProvider.IsInIgnoreList(recipientId, currentUser.UserID);


                        Message = new MessageInfo();
                        Message.MessageBody = ucBBEditor.Text;
                        string subject = (txtSubject.Text.Trim() == string.Empty) ? GetString("Messaging.NoSubject") : txtSubject.Text.Trim();
                        Message.MessageSubject = TextHelper.LimitLength(subject, 200);
                        Message.MessageRecipientUserID = recipientId;
                        Message.MessageRecipientNickName = TextHelper.LimitLength(Functions.GetFormattedUserName(recipient.UserName, recipient.FullName, recipient.UserNickName, IsLiveSite), 200);
                        Message.MessageSent = DateTime.Now;

                        // Anonymous user
                        if (isAnonymousUser)
                        {
                            Message.MessageSenderNickName = TextHelper.LimitLength(nickName, 200);
                            Message.MessageSenderDeleted = true;
                        }
                        else
                        {
                            Message.MessageSenderUserID = currentUser.UserID;
                            Message.MessageSenderNickName = TextHelper.LimitLength(Functions.GetFormattedUserName(currentUser.UserName, currentUser.FullName, currentUser.UserNickName, IsLiveSite), 200);

                            // If the user is ignored, delete message automatically
                            if (isIgnored)
                            {
                                Message.MessageRecipientDeleted = true;
                            }
                        }

                        string error = string.Empty;

                        // Check bad words
                        if (!BadWordInfoProvider.CanUseBadWords(currentUser, CMSContext.CurrentSiteName))
                        {
                            // Prepare columns to check
                            Dictionary<string, int> columns = new Dictionary<string, int>();
                            columns.Add("MessageSubject", 200);
                            columns.Add("MessageBody", 0);
                            columns.Add("MessageSenderNickName", 200);
                            columns.Add("MessageRecipientNickName", 200);

                            // Perform bad word check
                            error = BadWordsHelper.CheckBadWords(Message, columns, currentUser.UserID);
                        }

                        if (error != string.Empty)
                        {
                            lblSendError.Visible = true;
                            lblSendError.Text = error;
                        }
                        else
                        {
                            // Check message subject, if empty set no subject text
                            if (Message.MessageSubject.Trim() == string.Empty)
                            {
                                Message.MessageSubject = GetString("Messaging.NoSubject");
                            }

                            // Whole text has been removed
                            if (!ValidateBody(Message.MessageBody))
                            {
                                lblSendError.Visible = true;
                                lblSendError.Text = GetString("SendMessage.EmptyBodyBadWords");
                            }
                            else
                            {
                                // Save the message
                                MessageInfoProvider.SetMessageInfo(Message);

                                // Send notification email, if not ignored
                                if (!isIgnored)
                                {
                                    MessageInfoProvider.SendNotificationEmail(Message, recipient, currentUser, CMSContext.CurrentSiteName);
                                }

                                lblSendInfo.Visible = true;
                                lblSendInfo.Text = GetString("SendMesage.MessageSent");
                                MessageId = 0;
                                ucMessageUserSelector.SelectedUserID = 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblSendError.Visible = true;
                        lblSendError.Text = ex.Message;
                        ErrorMessage = ex.Message;
                    }
                }
                // Error in the form
                else
                {
                    lblSendError.Visible = true;
                    lblSendError.Text = message;
                    ErrorMessage = message;
                }
            }
            else
            {
                lblSendError.Visible = true;
                lblSendError.Text = GetString("General.BannedIP");
            }
        }
        else
        {
            lblSendError.Visible = true;
            lblSendError.Text = GetString("General.FloodProtection");
        }

        // External event
        if (SendButtonClick != null)
        {
            SendButtonClick(sender, e);
        }
    }

    #endregion
}
