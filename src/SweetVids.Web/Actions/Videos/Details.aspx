<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.Details"
 MasterPageFile="~/Shared/Site.Master" %>
<%@ Import Namespace="SweetVids.Core" %>
<%@ Import Namespace="SweetVids.Web.Actions.Videos" %>

 <asp:Content  ContentPlaceHolderID=_headerContent runat=server >
 
 </asp:Content>

 <asp:Content ContentPlaceHolderID="_mainContent" runat=server>

 
<h2><%= Model.Video.Title %></h2>

  <object width="960" height="745"><param name="movie" value="<%=Model.Video.GetYouTubeUrl() %>"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="<%=Model.Video.GetYouTubeUrl() %>" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="660" height="525"></embed></object>

  <p><%=Model.Video.Description %></p>
<% foreach (var comment in Model.Video.GetVideoComments())
{ %>
  
  <div class="comment">
  <img src="http://www.gravatar.com/avatar/<%= comment.Email.ToGravatarHash() %>?d=monsterid&s=60"
        alt="gravatar" />
   
    <h3><%=comment.Name %></h3>
    <p>
        <%=comment.Comment %>
    </p>
  
  </div>

<%} %> 

<form action="/comments" method="post">

<label for="name">Name:</label>
<input type="text" id="name" name="CommentName" value="" />

<label for="email">Email:</label>
<input type="text" id="email" name="CommentEmail" />

<label for="comment">Comment:</label>
<textarea id="comment" name="CommentComment" cols="15" rows="5"></textarea>
    
    <input type="submit" value="Post Comment" />

    <input type="hidden" name="VideoId" value="<%= Model.Video.Id %>"/>
</form>
 
 </asp:Content>
