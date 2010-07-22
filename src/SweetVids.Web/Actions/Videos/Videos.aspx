<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Videos.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.Videos"
 MasterPageFile="~/Shared/Site.Master" %>
<%@ Import Namespace="SweetVids.Web.Actions.Videos" %>

 <asp:Content  ContentPlaceHolderID=_headerContent runat=server >
 
 </asp:Content>

 <asp:Content ContentPlaceHolderID="_mainContent" runat=server>

 <% this.Partial(new AddVideoFormRequest()); %>

 <% foreach (var video in Model.Videos)
{ %>
<a href="/videos/<%=video.Id %>"><h3><%= video.Title %></h3></a>

  <object width="660" height="525"><param name="movie" value="<%=video.GetYouTubeUrl() %>"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="<%=video.GetYouTubeUrl() %>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="660" height="525"></embed></object>

  <p><%=video.Description %></p>
  <a href="/videos/<%=video.Id %>" >Comments(<%=video.GetVideoComments().Count()%>)</a>
<%} %>
 
 </asp:Content>