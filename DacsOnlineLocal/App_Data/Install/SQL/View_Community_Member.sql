CREATE VIEW [View_Community_Member] AS SELECT CMS_User.[UserID], CMS_User.[UserName], CMS_User.[FirstName], CMS_User.[MiddleName], CMS_User.[LastName], CMS_User.[FullName], CMS_User.[Email], CMS_User.[UserPassword], CMS_User.[PreferredCultureCode], CMS_User.[PreferredUICultureCode], CMS_User.[UserEnabled], CMS_User.[UserIsEditor], CMS_User.[UserIsGlobalAdministrator], CMS_User.[UserIsExternal], CMS_User.[UserPasswordFormat], CMS_User.[UserCreated], CMS_User.[LastLogon], CMS_User.[UserStartingAliasPath], CMS_User.[UserGUID], CMS_User.[UserLastModified], CMS_User.[UserLastLogonInfo], CMS_User.[UserIsHidden], CMS_User.[UserVisibility], CMS_User.[UserIsDomain], CMS_User.[UserHasAllowedCultures], CMS_User.[UserSiteManagerDisabled], CMS_UserSettings.[UserSettingsID], CMS_UserSettings.[UserNickName], CMS_UserSettings.[UserPicture], CMS_UserSettings.[UserSignature], CMS_UserSettings.[UserURLReferrer], CMS_UserSettings.[UserCampaign], CMS_UserSettings.[UserMessagingNotificationEmail], CMS_UserSettings.[UserCustomData], CMS_UserSettings.[UserRegistrationInfo], CMS_UserSettings.[UserPreferences], CMS_UserSettings.[UserActivationDate], CMS_UserSettings.[UserActivatedByUserID], CMS_UserSettings.[UserTimeZoneID], CMS_UserSettings.[UserAvatarID], CMS_UserSettings.[UserBadgeID], CMS_UserSettings.[UserShowSplashScreen], CMS_UserSettings.[UserActivityPoints], CMS_UserSettings.[UserForumPosts], CMS_UserSettings.[UserBlogComments], CMS_UserSettings.[UserGender], CMS_UserSettings.[UserDateOfBirth], CMS_UserSettings.[UserMessageBoardPosts], CMS_UserSettings.[UserSettingsUserGUID], CMS_UserSettings.[UserSettingsUserID], CMS_UserSettings.[WindowsLiveID], CMS_UserSettings.[UserBlogPosts], CMS_UserSettings.[UserWaitingForApproval], CMS_UserSettings.[UserDialogsConfiguration], CMS_UserSettings.[UserDescription], CMS_UserSettings.[UserUsedWebParts], CMS_UserSettings.[UserUsedWidgets], CMS_UserSettings.[UserFacebookID], CMS_UserSettings.[UserAuthenticationGUID], CMS_UserSettings.[UserSkype], CMS_UserSettings.[UserIM], CMS_UserSettings.[UserPhone], CMS_UserSettings.[UserPosition], CMS_UserSettings.[UserBounces], CMS_UserSettings.[UserLinkedInID], CMS_UserSettings.[UserLogActivities], CMS_UserSettings.[UserPasswordRequestHash], Community_GroupMember.[MemberID], Community_GroupMember.[MemberGUID], Community_GroupMember.[MemberUserID], Community_GroupMember.[MemberGroupID], Community_GroupMember.[MemberJoined], Community_GroupMember.[MemberApprovedWhen], Community_GroupMember.[MemberRejectedWhen], Community_GroupMember.[MemberApprovedByUserID], Community_GroupMember.[MemberComment], Community_GroupMember.[MemberInvitedByUserID], Community_GroupMember.[MemberStatus], CMS_UserSite.SiteID, CMS_Avatar.AvatarID,CMS_Avatar.AvatarGUID, CMS_Avatar.AvatarName FROM CMS_User LEFT JOIN CMS_UserSettings ON CMS_UserSettings.UserSettingsUserID = CMS_User.UserID LEFT JOIN CMS_UserSite ON CMS_UserSite.UserID = CMS_User.UserID LEFT JOIN Community_GroupMember ON Community_GroupMember.MemberUserID = CMS_User.UserID LEFT JOIN CMS_Avatar ON CMS_Avatar.AvatarID = CMS_UserSettings.UserAvatarID 
GO
