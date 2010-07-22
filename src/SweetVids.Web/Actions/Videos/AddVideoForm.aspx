<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddVideoForm.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.AddVideoForm" %>
<%@ Import Namespace="SweetVids.Core.Domain" %>
<%@ Import Namespace="SweetVids.Web" %>

<div id="video-form-wrapper" class="prefix_2 grid_12 suffix_2">

<!-- InputFor stuff here -->
<form action="<%=Urls.UrlForNew<Video>() %>" method="post">
    <%= this.Edit(x => x.Video.Title) %>
    <%= this.Edit(x => x.Video.Link)%>
    <%= this.Edit(x => x.Video.Description)%>
    <%= this.Edit(x => x.Video.VideoType)%>
    <input type="submit" value="Add Video"/>
</form>

</div>
