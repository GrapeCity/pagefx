function clickHandler()
{
	var q = $(this);
	var url = "";
  	if (q.attr("child-loaded") == "false")
  	{
        	url = q.attr("child-url");
     		if (url != "")
     			loadChildren(q, url);
  	}
  	if (q.attr("content-loaded") == "false")
  	{
  		url = q.attr("content-url");
   		if (url != "")
   			loadContent(q, url);
   	}
   	else
   	{
   		$("#content").replaceWith("<div class=\"fixed\" id=\"content\"></div>");	
   	}
}

function loadChildren(q, url)
{
	$.ajax({
   		url: url,
   		dataType: "html",
   		type: "GET",
   		complete:function(res, status)
   		{
   			q.attr("child-loaded", "true");
       		q.find("ul").replaceWith(res.responseText);
   			q.find("ul").treeview();
   			q.find("li").click(clickHandler);
   		}
   	});
}

function loadContent(q, url)
{
	$.ajax({
   		url: url,
   		dataType: "html",
   		type: "GET",
   		complete:function(res, status)
   		{
   			q.attr("content-loaded", "true");
   			$("#content").replaceWith(res.responseText);
   		}
   	});
}

$(document).ready(function(){

  $("#apitree").treeview();
  $("#testtree").treeview();

  $(".treeview li").click(clickHandler);

});