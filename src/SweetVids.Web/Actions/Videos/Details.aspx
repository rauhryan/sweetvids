<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.Details"
 MasterPageFile="~/Shared/Site.Master" %>
<%@ Import Namespace="SweetVids.Web.Actions.Videos" %>

 <asp:Content  ContentPlaceHolderID=_headerContent runat=server >
 
 </asp:Content>

 <asp:Content ContentPlaceHolderID="_mainContent" runat=server>

 
<h3><%= Model.Video.Title %></h3>

  <object width="960" height="745"><param name="movie" value="<%=Model.Video.GetYouTubeUrl() %>"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="<%=Model.Video.GetYouTubeUrl() %>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="660" height="525"></embed></object>

  <p><%=Model.Video.Description %></p>
  <a href="/videos/<%=Model.Video.Id %>" >Comments(<%=Model.Video.GetComments().Count()%>)</a>
 
 </asp:Content>
