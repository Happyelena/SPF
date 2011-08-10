<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComments.aspx.cs" Inherits="Website.addComments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Comments</title>
    <script type="text/javascript" src="static/js/jquery-1.6.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#btnSubmit').click(function () {
                var userId = '1';
                var $comments = $('#txtComments').val();
                var articleId = '1';

                $.ajax({ url: "../AjaxService/CommentsService.aspx?op=AddComments", type: "post", data: ("userId=" +
                userId + "&commentsContent=" +
                $comments + "&articleId=" +
                articleId), success: function (msg) {
                    alert(msg);
                }
                });
            });
        });
    </script>
</head>
<body>
    <h1>
        Have a nice bed for night</h1>
    <p>
        Paris Hilton has snubbed offers to appear on reality TV shows "Dancing With the
        Stars" and "Celebrity Apprentice" because she has no desire to share the spotlight
        with other famous faces. The blonde beauty has starred in several of her own shows,
        with her new series, "The World According to Paris," currently airing in America.
        Hilton reveals she has been asked to be a contestant on "Dancing With The Stars"
        and Donald Trump's "Celebrity Apprentice" -- but she turned down the offers to focus
        on her own projects. She tells the Windy City Times, "I've been approached every
        season by both of those shows but I would never do them. "I like doing my own show.
        I wouldn't want to be with a bunch of other people on a show who I don't know. Some
        of the people on those shows I really wouldn't want to be associated with."
    </p>
    <div>
        <div>
            Add your comments
        </div>
        <div>
            <span>Nick Name:</span><span><input type="text" id="txtuname" name="txtuname" /></span>
        </div>
        <div>
            <span>Comments:</span><span><textarea id="txtComments" name="txtComments" rows="5"
                cols="20"></textarea></span>
        </div>
        <div>
            <input type="button" id="btnSubmit" value="submit" />
        </div>
    </div>
</body>
</html>
