<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.Details"
    MasterPageFile="~/Shared/Site.Master" %>

<%@ Import Namespace="SweetVids.Core" %>
<%@ Import Namespace="SweetVids.Web.Actions.Videos" %>
<asp:Content ContentPlaceHolderID="_headerContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
    <div class="mod prefix_2 grid_12 suffix_2">
        <div class="inner">
            <div class="hd">
                <h2>
                    <%= Model.Video.Title %></h2>
            </div>
            <div class="bd">
                <object width="960" height="745">
                    <param name="movie" value="<%=Model.Video.GetYouTubeUrl() %>"></param>
                    <param name="allowFullScreen" value="true"></param>
                    <param name="allowscriptaccess" value="always"></param>
                    <embed src="<%=Model.Video.GetYouTubeUrl() %>" type="application/x-shockwave-flash"
                        allowscriptaccess="always" allowfullscreen="true" width="660" height="525"></embed></object>
            </div>
            <div class="ft">
                <p class="description">
                    <%=Model.Video.Description %></p>
                <ul class="comments">
                    <% for (int i = 0; i < Model.Video.GetVideoComments().Count(); i++)
                       { %>
                    <li class="<%= i%2 == 0 ? "even" : "odd" %> mod">
                        <div class="gravatar">
                            <img src="http://www.gravatar.com/avatar/<%= Model.Video.GetVideoComments().ToList()[i].Email.ToGravatarHash() %>?d=monsterid&s=60"
                                alt="gravatar" />
                        </div>
                        <div class="comment">
                            <h3>
                                <%= Model.Video.GetVideoComments().ToList()[i].Name %></h3>
                            <p>
                                <%=Model.Video.GetVideoComments().ToList()[i].Comment %>
                            </p>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <%} %>
                </ul>
            </div>
        </div>
    </div>
    <div class="mod prefix_2 grid_12 suffix_2">
        <div class="inner">
            <div class="bd">
                <form action="/comments" method="post">
                <p>
                    <label class="form-label" for="name">
                        Name:</label>
                    <input class="form-text color10 required" type="text" id="name" name="CommentName" value="" />
                </p>
                <p>
                    <label class="form-label" for="email">
                        Email:</label>
                    <input class="form-text color10 required email" type="text" id="email" name="CommentEmail" />
                </p>
                <p>
                    <label class="form-label" for="comment">
                        Comment:</label>
                    <textarea class="form-textarea color10 required" id="comment" name="CommentComment" cols="15"
                        rows="5"></textarea>
                </p>
                <p>
                    <input class="form-label" type="submit" value="Post Comment" />
                    <input type="hidden" name="VideoId" value="<%= Model.Video.Id %>" />
                </p>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
