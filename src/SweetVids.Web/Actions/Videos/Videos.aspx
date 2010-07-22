<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Videos.aspx.cs" Inherits="SweetVids.Web.Actions.Videos.Videos"
    MasterPageFile="~/Shared/Site.Master" %>

<%@ Import Namespace="SweetVids.Web.Actions.Videos" %>
<asp:Content ContentPlaceHolderID="_headerContent" runat="server">
<script type="text/javascript">
    jQuery(function ($) {
        $('#video-form-wrapper').slideToggle();

        $('#submit-button').click(function () {
            $('#video-form-wrapper').slideToggle('open');    
        });
    });
</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
<div class="grid_16">
    <div class="grid_3 prefix_1 suffix_1 alpha ">
        <a href="#" id="submit-button"><h6 class="color1">submit a sweet vid</h6></a>
    </div>
    <div class="grid_3 suffix_8 omega">
        <a href="#" id="hater-button"><h6 class="color1">send us hate mail</h6></a>
    </div>
</div>
    <% this.Partial(new AddVideoFormRequest()); %>
    <% foreach (var video in Model.Videos)
       { %>
        
        <div class="mod prefix_3 grid_10 suffix_3">
            <div class="inner">
                <div class="hd">
                    <a href="/videos/<%=video.Id %>"><h2 class="lobster color1"><%= video.Title %></h2></a>        
                </div>
                <div class="bd">
                    <object width="660" height="525">
        <param name="movie" value="<%=video.GetYouTubeUrl() %>"></param>
        <param name="allowFullScreen" value="true"></param>
        <param name="allowscriptaccess" value="always"></param>
        <embed src="<%=video.GetYouTubeUrl() %>" type="application/x-shockwave-flash" allowscriptaccess="always"
            allowfullscreen="true" width="660" height="525"></embed></object>
                </div>
                <div class="ft">
                <p class="description"><%=video.Description %></p>
                <div class="comment-line">
                Added on: <%= video.Created.ToLongDateString() %> |  (<%=video.GetVideoComments().Count()%>) Comments so far | <a href="/videos/<%=video.Id %>"> add a comment</a>
                </div>
                </div>
            </div>
        
        </div>
        
        
    
    
    
        
    <%} %>
</asp:Content>
