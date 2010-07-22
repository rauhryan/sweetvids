<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddVideoForm.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.AddVideoForm" %>
<%@ Import Namespace="SweetVids.Core.Domain" %>

<div id="video-form-wrapper">

<!-- InputFor stuff here -->
<form action="<%=Urls.UrlForNew<Video>() %>" method="post">
    <%= this.InputFor(x => x.Video.Title) %>
    <%= this.InputFor(x => x.Video.Link) %>
    <%= this.InputFor(x => x.Video.Description) %>
    <%= this.InputFor(x => x.Video.VideoType) %>
    <input type="submit" value="Add Video"/>
</form>

</div>
